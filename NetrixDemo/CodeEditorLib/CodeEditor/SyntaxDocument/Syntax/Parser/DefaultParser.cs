using System;
using System.Drawing;
//no parsing , just splitting and making whitespace possible
//1 sec to finnish ca 10000 rows

namespace GuruComponents.CodeEditor.CodeEditor.Syntax.SyntaxDocumentParsers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DefaultParser : IParser
    {


        private Language mLanguage;
        private SyntaxDocument mDocument;
        private long Version = 0;

        /// <summary>
        /// 
        /// </summary>
        public Language Language
        {
            get { return mLanguage; }
            set
            {
                mLanguage = value;
                this.Document.ReParse();
            }
        }

        #region PUBLIC PROPERTY SEPARATORS
        public string Separators
        {
            get
            {
                return this.Language.Separators;
            }
            set
            {
                this.Language.Separators = value;
            }
        }
        #endregion

        #region Optimerat och klart



        // ska anropas om "is same but different" är true
        private void MakeSame(int RowIndex)
        {

            Row row = Document[RowIndex];
            Segment seg = null;
            Segment OldStartSegment = Document[RowIndex + 1].StartSegment;

            //copy back the old segments to this line...
            seg = row.EndSegment;
            Segment seg2 = Document[RowIndex + 1].StartSegment;
            while (seg != null)
            {
                foreach (Word w in row)
                {

                    if (w.Segment == seg)
                    {
                        if (w.Segment.StartWord == w)
                            seg2.StartWord = w;

                        if (w.Segment.EndWord == w)
                            seg2.EndWord = w;

                        w.Segment = seg2;
                    }

                }

                if (seg == row.StartSegment)
                    row.StartSegment = seg2;

                if (seg == row.EndSegment)
                    row.EndSegment = seg2;


                if (row.StartSegments.IndexOf(seg) >= 0)
                    row.StartSegments[row.StartSegments.IndexOf(seg)] = seg2;

                if (row.EndSegments.IndexOf(seg) >= 0)
                    row.EndSegments[row.EndSegments.IndexOf(seg)] = seg2;

                seg = seg.Parent;
                seg2 = seg2.Parent;


            }
            row.SetExpansionSegment();
        }

        //om denna är true
        // så ska INTE nästa rad parse'as , utan denna ska fixas så den blir som den förra... (kopiera segment)
        private bool IsSameButDifferent(int RowIndex, Segment OldStartSegment)
        {
            //is this the last row ? , if so , bailout
            if (RowIndex >= Document.Count - 1)
                return false;

            Row row = Document[RowIndex];
            Segment seg = row.EndSegment;
            Segment OldEndSegment = Document[RowIndex + 1].StartSegment;
            Segment oseg = OldEndSegment;

            bool diff = false;

            while (seg != null)
            {
                if (oseg == null)
                {
                    diff = true;
                    break;
                }

                //Id1+=seg.BlockType.GetHashCode ().ToString ();
                if (seg.BlockType != oseg.BlockType)
                {
                    diff = true;
                    break;
                }

                if (seg.Parent != oseg.Parent)
                {
                    diff = true;
                    break;
                }

                seg = seg.Parent;
                oseg = oseg.Parent;
            }



            if (diff || row.StartSegment != OldStartSegment)
                return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxDocument Document
        {
            get { return mDocument; }
            set { mDocument = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SyntaxFile"></param>
        public void Init(string SyntaxFile)
        {
            if (!SyntaxFile.ToLower().EndsWith(".syn"))
                SyntaxFile += ".syn";


            this.Language = new SyntaxLoader().Load(SyntaxFile);
        }

        public void Init(string SyntaxFile, string Separators)
        {
            if (!SyntaxFile.ToLower().EndsWith(".syn"))
                SyntaxFile += ".syn";

            this.Language = new SyntaxLoader().Load(SyntaxFile, Separators);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Language"></param>
        public void Init(Language Language)
        {
            this.Language = Language;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RowIndex"></param>
        public void ParsePreviewLine(int RowIndex)
        {
            Row Row = Document[RowIndex];
            Row.Clear();
            Row.Add(Row.Text);
        }

        #endregion

        private ScanResult_Word GetNextWord(string Text, Segment CurrentSegment, int StartPos, ref bool HasComplex)
        {
            BlockType block = CurrentSegment.BlockType;

            #region ComplexFind
            int BestComplexPos = -1;
            Pattern BestComplexPattern = null;
            string BestComplexToken = "";
            ScanResult_Word complexword = new ScanResult_Word();
            if (HasComplex)
            {
                foreach (Pattern pattern in block.ComplexPatterns)
                {

                    PatternScanResult scanres = pattern.IndexIn(Text, StartPos, pattern.Parent.CaseSensitive, this.Separators);
                    if (scanres.Token != "")
                    {
                        if (scanres.Index < BestComplexPos || BestComplexPos == -1)
                        {
                            BestComplexPos = scanres.Index;
                            BestComplexPattern = pattern;
                            BestComplexToken = scanres.Token;
                        }
                    }
                }


                if (BestComplexPattern != null)
                {
                    complexword.HasContent = true;
                    complexword.ParentList = BestComplexPattern.Parent;
                    complexword.Pattern = BestComplexPattern;
                    complexword.Position = BestComplexPos;
                    complexword.Token = BestComplexToken;
                    HasComplex = true;
                }
                else
                {
                    HasComplex = false;
                }
            }
            #endregion

            #region SimpleFind
            ScanResult_Word simpleword = new ScanResult_Word();
            for (int i = StartPos; i < Text.Length; i++)
            {
                //bailout if we found a complex pattern before this char pos
                if (i > complexword.Position && complexword.HasContent)
                    break;




                #region 3+ char pattern

                if (i <= Text.Length - 3)
                {
                    string key = Text.Substring(i, 3).ToLower();
                    PatternCollection patterns2 = (PatternCollection)block.LookupTable[key];
                    //ok , there are patterns that start with this char
                    if (patterns2 != null)
                    {
                        foreach (Pattern pattern in patterns2)
                        {
                            int len = pattern.StringPattern.Length;
                            if (i + len > Text.Length)
                                continue;

                            char lastpatternchar = char.ToLower(pattern.StringPattern[len - 1]);
                            char lasttextchar = char.ToLower(Text[i + len - 1]);


                            #region Case Insensitive
                            if (lastpatternchar == lasttextchar)
                            {
                                if (!pattern.IsKeyword || (pattern.IsKeyword && pattern.HasSeparators(Text, i)))
                                {
                                    if (!pattern.Parent.CaseSensitive)
                                    {
                                        string s = Text.Substring(i, len).ToLower();

                                        if (s == pattern.StringPattern.ToLower())
                                        {
                                            simpleword.HasContent = true;
                                            simpleword.ParentList = pattern.Parent;
                                            simpleword.Pattern = pattern;
                                            simpleword.Position = i;
                                            if (pattern.Parent.NormalizeCase)
                                                simpleword.Token = pattern.StringPattern;
                                            else
                                                simpleword.Token = Text.Substring(i, len);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        string s = Text.Substring(i, len);

                                        if (s == pattern.StringPattern)
                                        {
                                            simpleword.HasContent = true;
                                            simpleword.ParentList = pattern.Parent;
                                            simpleword.Pattern = pattern;
                                            simpleword.Position = i;
                                            simpleword.Token = pattern.StringPattern;
                                            break;
                                        }
                                    }


                                }
                            }
                        }
                            #endregion
                    }
                }

                #endregion

                if (simpleword.HasContent)
                    break;

                #region single char pattern
                char c = Text[i];
                PatternCollection patterns = (PatternCollection)block.LookupTable[c];
                if (patterns != null)
                {
                    //ok , there are patterns that start with this char
                    foreach (Pattern pattern in patterns)
                    {
                        int len = pattern.StringPattern.Length;
                        if (i + len > Text.Length)
                            continue;

                        char lastpatternchar = pattern.StringPattern[len - 1];
                        char lasttextchar = Text[i + len - 1];

                        if (!pattern.Parent.CaseSensitive)
                        {
                            #region Case Insensitive
                            if (char.ToLower(lastpatternchar) == char.ToLower(lasttextchar))
                            {
                                if (!pattern.IsKeyword || (pattern.IsKeyword && pattern.HasSeparators(Text, i)))
                                {
                                    string s = Text.Substring(i, len).ToLower();

                                    if (s == pattern.StringPattern.ToLower())
                                    {
                                        simpleword.HasContent = true;
                                        simpleword.ParentList = pattern.Parent;
                                        simpleword.Pattern = pattern;
                                        simpleword.Position = i;
                                        simpleword.Token = Text.Substring(i, len);
                                        break;
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Case Sensitive
                            if (lastpatternchar == lasttextchar)
                            {
                                if (!pattern.IsKeyword || (pattern.IsKeyword && pattern.HasSeparators(Text, i)))
                                {
                                    string s = Text.Substring(i, len);

                                    if (s == pattern.StringPattern)
                                    {
                                        simpleword.HasContent = true;
                                        simpleword.ParentList = pattern.Parent;
                                        simpleword.Pattern = pattern;
                                        simpleword.Position = i;
                                        simpleword.Token = pattern.StringPattern;
                                        break;
                                    }
                                }
                            }
                            #endregion
                        }
                    }

                    if (simpleword.HasContent)
                        break;
                }
                #endregion
            }



            #endregion

            if (complexword.HasContent && simpleword.HasContent)
            {
                if (simpleword.Position == complexword.Position)
                {
                    if (simpleword.Token.Length > complexword.Token.Length)
                        return simpleword;
                    else
                        return complexword;
                }

                if (simpleword.Position < complexword.Position)
                    return simpleword;

                if (simpleword.Position > complexword.Position)
                    return complexword;

            }

            if (simpleword.HasContent)
                return simpleword;

            if (complexword.HasContent)
                return complexword;



            return new ScanResult_Word();
        }



        private void ParseText(Row Row, Segment CurrentSegment, string Text)
        {
            int CurrentPosition = 0;
            bool HasComplex = true;
            while (true)
            {
                ScanResult_Word Word = GetNextWord(Text, CurrentSegment, CurrentPosition, ref HasComplex);

                if (!Word.HasContent)
                {
                    ParseTools.AddString(Text.Substring(CurrentPosition), Row, CurrentSegment.BlockType.Style, CurrentSegment);
                    break;
                }
                else
                {
                    ParseTools.AddString(Text.Substring(CurrentPosition, Word.Position - CurrentPosition), Row, CurrentSegment.BlockType.Style, CurrentSegment);
                    ParseTools.AddPatternString(Word.Token, Row, Word.Pattern, Word.ParentList.Style, CurrentSegment, false);
                    CurrentPosition = Word.Position + Word.Token.Length;
                }
            }
        }

        private ScanResult_Word GetNextComplexWord(String Text, Segment CurrentSegment, int StartPositon)
        {
            if (StartPositon >= Text.Length)
                return new ScanResult_Word();

            ScanResult_Word Result = new ScanResult_Word();

            int CurrentPosition = 0;

            //look for keywords


            PatternListList keywordsList = CurrentSegment.BlockType.KeywordsList;

            PatternList List = null;

            for (int i = 0; i < keywordsList.Count; i++)
            {
                List = keywordsList[i];

                PatternCollection complexPatterns = List.ComplexPatterns;

                Pattern Word = null;

                for (int j = 0; j < complexPatterns.Count; j++)
                {
                    Word = complexPatterns[j];

                    PatternScanResult psr = Word.IndexIn(Text, StartPositon, false, Separators);
                    CurrentPosition = psr.Index;
                    if ((CurrentPosition < Result.Position || Result.HasContent == false) && psr.Token != "")
                    {
                        Result.HasContent = true;
                        Result.Position = CurrentPosition;
                        Result.Token = psr.Token;
                        Result.Pattern = Word;
                        Result.ParentList = List;

                        if (List.NormalizeCase)
                            if (!Word.IsComplex)
                                Result.Token = Word.StringPattern;
                    }
                }
            }

            //look for operators

            PatternListList pattList = CurrentSegment.BlockType.OperatorsList;


            PatternList patternList = null;

            for (int i = 0; i < pattList.Count; i++)
            {
                patternList = pattList[i];

                PatternCollection complexPatterns = patternList.ComplexPatterns;

                for (int j = 0; j < complexPatterns.Count; j++)
                {
                    Pattern Word = complexPatterns[j];

                    PatternScanResult psr = Word.IndexIn(Text, StartPositon, false, Separators);

                    CurrentPosition = psr.Index;

                    if ((CurrentPosition < Result.Position || Result.HasContent == false) && psr.Token != "")
                    {
                        Result.HasContent = true;
                        Result.Position = CurrentPosition;
                        Result.Token = psr.Token;
                        Result.Pattern = Word;
                        Result.ParentList = patternList;

                        if (patternList.NormalizeCase)
                            if (!Word.IsComplex)
                                Result.Token = Word.StringPattern;
                    }
                }
            }

            if (Result.HasContent)
                return Result;
            else
                return new ScanResult_Word();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ParseKeywords"></param>
        public void ParseLine(int index, bool ParseKeywords)
        {
            InternalParseLine(index, ParseKeywords);
            if (this.Language != null)
            {
                if (this.Version != this.Language.Version)
                {
                    this.Language.UpdateLists();
                    this.Version = this.Language.Version;
                }
            }

            this.Document.InvokeRowParsed(this.Document[index]);
        }

        private void InternalParseLine(int index, bool ParseKeywords)
        {

            if (this.mLanguage == null)
                return;

            //
            //			if (ParseKeywords)
            //				return;
            //			ParseKeywords=true;
            SyntaxDocument doc = Document;
            Row Row = doc[index];
            Segment OldEndSegment = Row.EndSegment;
            Segment OldStartSegment = Row.StartSegment;
            bool Fold = !Row.IsCollapsed;


            if (Row.IsCollapsedEndPart)
            {
                //Row.Expansion_EndSegment.Expanded = true;
                //Row.Expansion_EndSegment.EndRow = null;
                Row.Expansion_EndSegment.EndWord = null;
            }


            //set startsegment for this row
            if (index > 0)
            {
                Row.StartSegment = Document[index - 1].EndSegment;
            }
            else
            {
                if (Row.StartSegment == null)
                {
                    Row.StartSegment = new Segment(Row);
                    Row.StartSegment.BlockType = this.mLanguage.MainBlock;
                }
            }

            int CurrentPosition = 0;
            Segment CurrentSegment = Row.StartSegment;


            //kör tills vi kommit till slutet av raden..
            Row.EndSegments.Clear();
            Row.StartSegments.Clear();
            Row.Clear();
            //		bool HasEndSegment=false;

            while (true)
            {
                ScanResult_Segment ChildSegment = GetNextChildSegment(Row, CurrentSegment, CurrentPosition);
                ScanResult_Segment EndSegment = GetEndSegment(Row, CurrentSegment, CurrentPosition);

                if ((EndSegment.HasContent && ChildSegment.HasContent && EndSegment.Position <= ChildSegment.Position) || (EndSegment.HasContent && ChildSegment.HasContent == false))
                {
                    //this is an end segment

                    if (ParseKeywords)
                    {
                        string Text = Row.Text.Substring(CurrentPosition, EndSegment.Position - CurrentPosition);
                        ParseText(Row, CurrentSegment, Text);
                    }

                    Segment oldseg = CurrentSegment;
                    while (CurrentSegment != EndSegment.Segment)
                    {
                        Row.EndSegments.Add(CurrentSegment);
                        CurrentSegment = CurrentSegment.Parent;
                    }
                    Row.EndSegments.Add(CurrentSegment);

                    TextStyle st2 = CurrentSegment.Scope.Style;

                    ParseTools.AddPatternString(EndSegment.Token, Row, EndSegment.Pattern, st2, CurrentSegment, false);
                    while (oldseg != EndSegment.Segment)
                    {

                        oldseg.EndRow = Row;
                        oldseg.EndWord = Row[Row.Count - 1];
                        oldseg = oldseg.Parent;
                    }

                    CurrentSegment.EndRow = Row;
                    CurrentSegment.EndWord = Row[Row.Count - 1];



                    if (CurrentSegment.Parent != null)
                        CurrentSegment = CurrentSegment.Parent;

                    CurrentPosition = EndSegment.Position + EndSegment.Token.Length;
                }
                else if (ChildSegment.HasContent)
                {
                    //this is a child block

                    if (ParseKeywords)
                    {
                        string Text = Row.Text.Substring(CurrentPosition, ChildSegment.Position - CurrentPosition);
                        //TextStyle st=CurrentSegment.BlockType.Style;
                        ParseText(Row, CurrentSegment, Text);
                        //ParseTools.AddString (Text,Row,st,CurrentSegment);
                    }



                    Segment NewSeg = new Segment();
                    NewSeg.Parent = CurrentSegment;
                    NewSeg.BlockType = ChildSegment.BlockType;
                    NewSeg.Scope = ChildSegment.Scope;

                    Row.StartSegments.Add(NewSeg);

                    TextStyle st2 = NewSeg.Scope.Style;
                    ParseTools.AddPatternString(ChildSegment.Token, Row, ChildSegment.Pattern, st2, NewSeg, false);
                    NewSeg.StartRow = Row;
                    NewSeg.StartWord = Row[Row.Count - 1];


                    CurrentSegment = NewSeg;
                    CurrentPosition = ChildSegment.Position + ChildSegment.Token.Length;

                    if (ChildSegment.Scope.SpawnBlockOnStart != null)
                    {
                        Segment SpawnSeg = new Segment();
                        SpawnSeg.Parent = NewSeg;
                        SpawnSeg.BlockType = ChildSegment.Scope.SpawnBlockOnStart;
                        SpawnSeg.Scope = new Scope();
                        SpawnSeg.StartWord = NewSeg.StartWord;
                        Row.StartSegments.Add(SpawnSeg);
                        CurrentSegment = SpawnSeg;
                    }



                }
                else
                {
                    if (CurrentPosition < Row.Text.Length)
                    {
                        if (ParseKeywords)
                        {
                            //we did not find a childblock nor an endblock , just output the last pice of text
                            string Text = Row.Text.Substring(CurrentPosition);
                            //TextStyle st=CurrentSegment.BlockType.Style;	
                            ParseText(Row, CurrentSegment, Text);
                            //ParseTools.AddString (Text,Row,st,CurrentSegment);
                        }
                    }
                    break;
                }
            }

            while (!CurrentSegment.BlockType.MultiLine)
            {
                Row.EndSegments.Add(CurrentSegment);
                CurrentSegment = CurrentSegment.Parent;
            }

            Row.EndSegment = CurrentSegment;
            Row.SetExpansionSegment();

            if (ParseKeywords)
                Row.RowState = RowState.AllParsed;
            else
                Row.RowState = RowState.SegmentParsed;

            if (IsSameButDifferent(index, OldStartSegment))
            {
                MakeSame(index);
                //if (!IsSameButDifferent(index))
                //	System.Diagnostics.Debugger.Break();
            }

            if (Row.CanFold)
                Row.Expansion_StartSegment.Expanded = Fold;

            //dont flag next line as needs parsing if only parsing keywords
            if (!ParseKeywords)
            {
                if (OldEndSegment != null)
                {
                    if (Row.EndSegment != OldEndSegment && index <= Document.Count - 2)
                    {

                        //if (Row.CanFold)
                        //	Row.Expansion_StartSegment.Expanded = true;
                        Document[index + 1].AddToParseQueue();
                        Document.NeedResetRows = true;
                    }
                }
                else if (index <= Document.Count - 2)
                {
                    //if (Row.CanFold)
                    //	Row.Expansion_StartSegment.Expanded = true;
                    Document[index + 1].AddToParseQueue();
                    Document.NeedResetRows = true;
                }
            }

            if (OldEndSegment != null)
            {
                //expand segment if this line dont have an end word
                if (OldEndSegment.EndWord == null)
                    OldEndSegment.Expanded = true;
            }
        }



        private ScanResult_Segment GetEndSegment(Row Row, Segment CurrentSegment, int StartPos)
        {
            //this row has no text , just bail out...
            if (StartPos >= Row.Text.Length || CurrentSegment.Scope == null)
                return new ScanResult_Segment();

            ScanResult_Segment Result = new ScanResult_Segment();
            Result.HasContent = false; //make sure we have no content in this object
            Result.IsEndSegment = false; //nope , were looking for start blocks here


            //--------------------------------------------------------------------------------
            //scan for childblocks
            //scan each scope in each childblock

            Segment seg = CurrentSegment;

            while (seg != null)
            {
                if (seg == CurrentSegment || seg.BlockType.TerminateChildren)
                {
                    foreach (Pattern end in seg.Scope.EndPatterns)
                    {
                        PatternScanResult psr = end.IndexIn(Row.Text, StartPos, seg.Scope.CaseSensitive, Separators);
                        int CurrentPosition = psr.Index;
                        if (psr.Token != "")
                        {
                            if ((psr.Index < Result.Position && Result.HasContent) || !Result.HasContent)
                            {
                                //we found a better match
                                //store this new match
                                Result.Pattern = end;
                                Result.Position = CurrentPosition;
                                Result.Token = psr.Token;
                                Result.HasContent = true;
                                Result.Segment = seg;
                                Result.Scope = null;


                                if (!end.IsComplex)
                                {
                                    if (seg.Scope.NormalizeCase)
                                        if (!seg.Scope.Start.IsComplex)
                                            Result.Token = end.StringPattern;
                                }
                            }
                        }
                    }
                }
                seg = seg.Parent;
            }

            //no result , return new ScanResult_Segment();
            if (!Result.HasContent)
                return new ScanResult_Segment();

            return Result;
        }

        private ScanResult_Segment GetNextChildSegment(Row Row, Segment CurrentSegment, int StartPos)
        {
            //this row has no text , just bail out...
            if (StartPos >= Row.Text.Length)
                return new ScanResult_Segment();



            ScanResult_Segment Result = new ScanResult_Segment();
            Result.HasContent = false; //make sure we have no content in this object
            Result.IsEndSegment = false; //nope , were looking for start blocks here

            BlockType ChildBlock;
            //foreach (BlockType ChildBlock in CurrentSegment.BlockType.ChildBlocks)

            BlockTypeCollection blocks = CurrentSegment.BlockType.ChildBlocks;

            for (int i = 0; i < blocks.Count; i++)
            {
                ChildBlock = blocks[i];
                //scan each scope in each childblock
                //foreach (Scope Scope in ChildBlock.ScopePatterns)
                Scope Scope;
                ScopeCollection scopeColl = ChildBlock.ScopePatterns;
                for (int j = 0; j < scopeColl.Count; j++)
                {
                    Scope = scopeColl[j];

                    PatternScanResult psr = Scope.Start.IndexIn(Row.Text, StartPos, Scope.CaseSensitive, Separators);
                    int CurrentPosition = psr.Index;
                    if ((!Result.HasContent || CurrentPosition < Result.Position) && psr.Token != "")
                    {
                        //we found a better match
                        //store this new match
                        Result.Pattern = Scope.Start;
                        Result.Position = CurrentPosition;
                        Result.Token = psr.Token;
                        Result.HasContent = true;
                        Result.BlockType = ChildBlock;
                        Result.Scope = Scope;

                        if (Scope.NormalizeCase)
                            if (!Scope.Start.IsComplex)
                                Result.Token = Scope.Start.StringPattern;
                    }
                }
            }






            //no result ,  new ScanResult_Segment();
            if (!Result.HasContent)
                return new ScanResult_Segment();

            return Result;
        }

        private void ParseSegemnts(Row Row)
        {
            //skapa segment för raden och sätt start och endsegment för raden
            Row.Clear();
            string k = Row.Text;
            TextStyle st = new TextStyle();
            st.ForeColor = Color.Red;
            ParseTools.AddString(k, Row, st, null);
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        public DefaultParser()
        {
            mLanguage = null;

        }
    }
}