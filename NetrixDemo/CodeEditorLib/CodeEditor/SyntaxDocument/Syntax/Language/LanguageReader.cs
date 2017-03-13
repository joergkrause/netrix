using System;
using System.Xml;
using System.Collections;
using System.Drawing;
using GuruComponents.CodeEditor.CodeEditor.Syntax;
using System.IO;
using System.Collections.Specialized;
using System.Text;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
    /// <summary>
    /// 
    /// </summary>
    public class SyntaxLoader
    {
        private Hashtable mStyles = new Hashtable();
        private Hashtable mBlocks = new Hashtable();
        private Language mLanguage = new Language();
        private static bool _UseUserCustomStyles = false;

        //protected BlockType		mLanguage.MainBlock=null;

        private static string _UserCustomStyles = null;


        /// <summary>
        /// Directory where is saved language font and color configurations
        /// </summary>
        public static string UserCustomStyles
        {
            get { return SyntaxLoader._UserCustomStyles; }
            set { SyntaxLoader._UserCustomStyles = value; }
        }

        /// <summary>
        /// Set if user can user custom user styles for the Language
        /// </summary>
        public static bool UseUserCustomStyles
        {
            get { return _UseUserCustomStyles; }
            set { _UseUserCustomStyles = value; }
        }

        /// <summary>
        /// Load a specific language file
        /// </summary>
        /// <param name="File">File name</param>
        /// <returns>Language object</returns>
        public Language Load(string filename)
        {
            return Load(File.OpenRead(filename));
        }

        public Language Load(Stream stream)
        {
            mStyles = new Hashtable();
            mBlocks = new Hashtable();
            mLanguage = new Language();

            XmlDocument myXmlDocument = new XmlDocument();
            myXmlDocument.Load(stream);


            if (_UseUserCustomStyles && Directory.Exists(_UserCustomStyles))
            {
                string langName = myXmlDocument.SelectSingleNode("Language/@Name").InnerText;

                string path = Path.Combine(SyntaxLoader.UserCustomStyles, langName + ".conf");

                if (File.Exists(path))
                {
                    XmlDocument userConf = new XmlDocument();

                    userConf.Load(path);

                    XmlNodeList xlist = myXmlDocument.SelectNodes("Language/Style");

                    foreach (XmlNode current in xlist)
                    {
                        XmlNode userStyleNode = userConf.SelectSingleNode("styles/Style[@Name='" +
                            current.Attributes["Name"].InnerText + "']");

                        if (userStyleNode == null)
                            continue;

                        foreach (XmlAttribute userAtt in userStyleNode.Attributes)
                        {
                            current.Attributes[userAtt.LocalName].InnerText = userAtt.InnerText;
                        }
                    }
                }
            }

            ReadLanguageDef(myXmlDocument);

            return mLanguage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        public Language Load(string File, string Separators)
        {
            mStyles = new Hashtable();
            mBlocks = new Hashtable();
            mLanguage = new Language();
            mLanguage.Separators = Separators;

            XmlDocument myXmlDocument = new XmlDocument();
            myXmlDocument.Load(File);
            ReadLanguageDef(myXmlDocument);

            return mLanguage;
        }

        /// <summary>
        /// Load a specific language from an xml string
        /// </summary>
        /// <param name="XML"></param>
        /// <returns></returns>
        public Language LoadXML(string XML)
        {
            mStyles = new Hashtable();
            mBlocks = new Hashtable();
            mLanguage = new Language();

            XmlDocument myXmlDocument = new XmlDocument();
            myXmlDocument.LoadXml(XML);
            ReadLanguageDef(myXmlDocument);

            return mLanguage;
        }

        private void ReadLanguageDef(XmlDocument xml)
        {
            ParseLanguage(xml["Language"]);
        }

        private void ParseLanguage(XmlNode node)
        {

            //get language name and startblock
            string Name = "";
            string StartBlock = "";

            foreach (XmlAttribute att in node.Attributes)
            {
                if (att.Name.ToLower() == "name")
                    Name = att.Value;

                if (att.Name.ToLower() == "startblock")
                    StartBlock = att.Value;
            }

            mLanguage.Name = Name;
            mLanguage.MainBlock = GetBlock(StartBlock);

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.NodeType == XmlNodeType.Element)
                {
                    if (n.Name.ToLower() == "filetypes")
                        ParseFileTypes(n);
                    if (n.Name.ToLower() == "block")
                        ParseBlock(n);
                    if (n.Name.ToLower() == "style")
                        ParseStyle(n);
                }
            }
        }
        private void ParseFileTypes(XmlNode node)
        {

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.NodeType == XmlNodeType.Element)
                {
                    if (n.Name.ToLower() == "filetype")
                    {
                        //add filetype
                        string Extension = "";
                        string Name = "";
                        foreach (XmlAttribute a in n.Attributes)
                        {
                            if (a.Name.ToLower() == "name")
                                Name = a.Value;
                            if (a.Name.ToLower() == "extension")
                                Extension = a.Value;
                        }
                        FileType ft = new FileType();
                        ft.Extension = Extension;
                        ft.Name = Name;
                        mLanguage.FileTypes.Add(ft);
                    }
                }
            }
        }

        private void ParseBlock(XmlNode node)
        {
            string Name = "", Style = "", PatternStyle = "";
            bool IsMultiline = false;
            bool TerminateChildren = false;
            Color BackColor = Color.Transparent;
            foreach (XmlAttribute att in node.Attributes)
            {
                if (att.Name.ToLower() == "name")
                    Name = att.Value;
                if (att.Name.ToLower() == "style")
                    Style = att.Value;
                if (att.Name.ToLower() == "patternstyle")
                    PatternStyle = att.Value;
                if (att.Name.ToLower() == "ismultiline")
                    IsMultiline = bool.Parse(att.Value);
                if (att.Name.ToLower() == "terminatechildren")
                    TerminateChildren = bool.Parse(att.Value);
                if (att.Name.ToLower() == "backcolor")
                {
                    BackColor = Color.FromName(att.Value);
                    //Transparent =false;
                }
            }

            //create block object here
            BlockType bl = GetBlock(Name);
            bl.BackColor = BackColor;
            bl.Name = Name;
            bl.MultiLine = IsMultiline;
            bl.Style = GetStyle(Style);
            bl.TerminateChildren = TerminateChildren;
            //		if (PatternStyle!="")
            //			bl.PatternStyle = GetStyle(PatternStyle);
            //		else
            //			bl.PatternStyle = bl.Style;			

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.NodeType == XmlNodeType.Element)
                {
                    if (n.Name.ToLower() == "scope")
                    {
                        //bool IsComplex=false;
                        //bool IsSeparator=false;
                        string Start = "";
                        string End = "";
                        string style = "";
                        string text = "";
                        string EndIsSeparator = "";
                        string StartIsSeparator = "";
                        string StartIsComplex = "false";
                        string EndIsComplex = "false";
                        string StartIsKeyword = "false";
                        string EndIsKeyword = "false";
                        string spawnstart = "";
                        string spawnend = "";
                        string EscapeChar = "";
                        string CauseIndent = "false";

                        bool expanded = true;

                        foreach (XmlAttribute att in n.Attributes)
                        {
                            if (att.Name.ToLower() == "start")
                                Start = att.Value;
                            if (att.Name.ToLower() == "escapechar")
                                EscapeChar = att.Value;
                            if (att.Name.ToLower() == "end")
                                End = att.Value;
                            if (att.Name.ToLower() == "style")
                                style = att.Value;
                            if (att.Name.ToLower() == "text")
                                text = att.Value;
                            if (att.Name.ToLower() == "defaultexpanded")
                                expanded = bool.Parse(att.Value);
                            if (att.Name.ToLower() == "endisseparator")
                                EndIsSeparator = att.Value;
                            if (att.Name.ToLower() == "startisseparator")
                                StartIsSeparator = att.Value;
                            if (att.Name.ToLower() == "startiskeyword")
                                StartIsKeyword = att.Value;
                            if (att.Name.ToLower() == "startiscomplex")
                                StartIsComplex = att.Value;
                            if (att.Name.ToLower() == "endiscomplex")
                                EndIsComplex = att.Value;
                            if (att.Name.ToLower() == "endiskeyword")
                                EndIsKeyword = att.Value;
                            if (att.Name.ToLower() == "spawnblockonstart")
                                spawnstart = att.Value;
                            if (att.Name.ToLower() == "spawnblockonend")
                                spawnend = att.Value;
                            if (att.Name.ToLower() == "causeindent")
                                CauseIndent = att.Value;
                        }
                        if (Start != "")
                        {
                            //bl.StartPattern =new Pattern (Pattern,IsComplex,false,IsSeparator);
                            //bl.StartPatterns.Add (new Pattern (Pattern,IsComplex,IsSeparator,true));
                            Scope scop = new Scope();
                            scop.Style = GetStyle(style);
                            scop.ExpansionText = text;
                            scop.DefaultExpanded = expanded;
                            bool blnStartIsComplex = bool.Parse(StartIsComplex);
                            bool blnEndIsComplex = bool.Parse(EndIsComplex);
                            bool blnCauseIndent = bool.Parse(CauseIndent);
                            scop.CauseIndent = blnCauseIndent;

                            Pattern StartP = new Pattern(Start, blnStartIsComplex, false, bool.Parse(StartIsKeyword));
                            Pattern EndP = null;
                            if (EscapeChar != "")
                            {
                                EndP = new Pattern(End, blnEndIsComplex, false, bool.Parse(EndIsKeyword), EscapeChar);
                            }
                            else
                            {
                                EndP = new Pattern(End, blnEndIsComplex, false, bool.Parse(EndIsKeyword));
                            }

                            if (EndIsSeparator != "")
                                EndP.IsSeparator = bool.Parse(EndIsSeparator);
                            scop.Start = StartP;
                            scop.EndPatterns.Add(EndP);
                            bl.ScopePatterns.Add(scop);
                            if (spawnstart != "")
                            {
                                scop.SpawnBlockOnStart = GetBlock(spawnstart);
                            }
                            if (spawnend != "")
                            {
                                scop.SpawnBlockOnEnd = GetBlock(spawnend);
                            }
                        }
                    }
                    if (n.Name.ToLower() == "bracket")
                    {
                        //bool IsComplex=false;
                        //bool IsSeparator=false;
                        string Start = "";
                        string End = "";
                        string style = "";

                        string EndIsSeparator = "";
                        string StartIsSeparator = "";

                        string StartIsComplex = "false";
                        string EndIsComplex = "false";

                        string StartIsKeyword = "false";
                        string EndIsKeyword = "false";
                        string IsMultiLineB = "true";

                        foreach (XmlAttribute att in n.Attributes)
                        {
                            if (att.Name.ToLower() == "start")
                                Start = att.Value;
                            if (att.Name.ToLower() == "end")
                                End = att.Value;
                            if (att.Name.ToLower() == "style")
                                style = att.Value;
                            if (att.Name.ToLower() == "endisseparator")
                                EndIsSeparator = att.Value;
                            if (att.Name.ToLower() == "startisseparator")
                                StartIsSeparator = att.Value;
                            if (att.Name.ToLower() == "startiskeyword")
                                StartIsKeyword = att.Value;
                            if (att.Name.ToLower() == "startiscomplex")
                                StartIsComplex = att.Value;
                            if (att.Name.ToLower() == "endiscomplex")
                                EndIsComplex = att.Value;
                            if (att.Name.ToLower() == "endiskeyword")
                                EndIsKeyword = att.Value;
                            if (att.Name.ToLower() == "ismultiline")
                                IsMultiLineB = att.Value;
                        }
                        if (Start != "")
                        {
                            PatternList pl = new PatternList();
                            pl.Style = GetStyle(style);

                            bool blnStartIsComplex = bool.Parse(StartIsComplex);
                            bool blnEndIsComplex = bool.Parse(EndIsComplex);
                            bool blnIsMultiLineB = bool.Parse(IsMultiLineB);

                            Pattern StartP = new Pattern(Start, blnStartIsComplex, false, bool.Parse(StartIsKeyword));
                            Pattern EndP = new Pattern(End, blnEndIsComplex, false, bool.Parse(EndIsKeyword));

                            StartP.MatchingBracket = EndP;
                            EndP.MatchingBracket = StartP;
                            StartP.BracketType = BracketType.StartBracket;
                            EndP.BracketType = BracketType.EndBracket;
                            StartP.IsMultiLineBracket = EndP.IsMultiLineBracket = blnIsMultiLineB;

                            pl.Add(StartP);
                            pl.Add(EndP);
                            bl.OperatorsList.Add(pl);
                        }
                    }

                }

                if (n.Name.ToLower() == "keywords")
                    foreach (XmlNode cn in n.ChildNodes)
                    {
                        if (cn.Name.ToLower() == "patterngroup")
                        {
                            PatternList pl = new PatternList();
                            bl.KeywordsList.Add(pl);
                            foreach (XmlAttribute att in cn.Attributes)
                            {
                                if (att.Name.ToLower() == "style")
                                    pl.Style = GetStyle(att.Value);

                                if (att.Name.ToLower() == "name")
                                    pl.Name = att.Value;

                                if (att.Name.ToLower() == "normalizecase")
                                    pl.NormalizeCase = bool.Parse(att.Value);

                                if (att.Name.ToLower() == "casesensitive")
                                    pl.CaseSensitive = bool.Parse(att.Value);

                            }
                            foreach (XmlNode pt in cn.ChildNodes)
                            {
                                if (pt.Name.ToLower() == "pattern")
                                {
                                    bool IsComplex = false;
                                    bool IsSeparator = false;
                                    string Category = null;
                                    string Pattern = "";
                                    if (pt.Attributes != null)
                                    {
                                        foreach (XmlAttribute att in pt.Attributes)
                                        {
                                            if (att.Name.ToLower() == "text")
                                                Pattern = att.Value;
                                            if (att.Name.ToLower() == "iscomplex")
                                                IsComplex = bool.Parse(att.Value);
                                            if (att.Name.ToLower() == "isseparator")
                                                IsSeparator = bool.Parse(att.Value);
                                            if (att.Name.ToLower() == "category")
                                                Category = (att.Value);

                                        }
                                    }
                                    if (Pattern != "")
                                    {
                                        Pattern pat = new Pattern(Pattern, IsComplex, IsSeparator, true);
                                        pat.Category = Category;
                                        pl.Add(pat);
                                    }

                                }
                                else if (pt.Name.ToLower() == "patterns")
                                {
                                    string Patterns = pt.ChildNodes[0].Value;
                                    Patterns = Patterns.Replace("\t", " ");
                                    while (Patterns.IndexOf("  ") >= 0)
                                        Patterns = Patterns.Replace("  ", " ");


                                    foreach (string Pattern in Patterns.Split())
                                    {
                                        if (Pattern != "")
                                            pl.Add(new Pattern(Pattern, false, false, true));
                                    }
                                }
                            }
                        }
                    }
                //if (n.Name == "Operators")
                //	ParseStyle(n);
                if (n.Name.ToLower() == "operators")
                    foreach (XmlNode cn in n.ChildNodes)
                    {
                        if (cn.Name.ToLower() == "patterngroup")
                        {
                            PatternList pl = new PatternList();
                            bl.OperatorsList.Add(pl);
                            foreach (XmlAttribute att in cn.Attributes)
                            {
                                if (att.Name.ToLower() == "style")
                                    pl.Style = GetStyle(att.Value);

                                if (att.Name.ToLower() == "name")
                                    pl.Name = att.Value;

                                if (att.Name.ToLower() == "normalizecase")
                                    pl.NormalizeCase = bool.Parse(att.Value);

                                if (att.Name.ToLower() == "casesensitive")
                                    pl.CaseSensitive = bool.Parse(att.Value);
                            }

                            foreach (XmlNode pt in cn.ChildNodes)
                            {
                                if (pt.Name.ToLower() == "pattern")
                                {
                                    bool IsComplex = false;
                                    bool IsSeparator = false;
                                    string Pattern = "";
                                    string Category = null;
                                    if (pt.Attributes != null)
                                    {
                                        foreach (XmlAttribute att in pt.Attributes)
                                        {
                                            if (att.Name.ToLower() == "text")
                                                Pattern = att.Value;
                                            if (att.Name.ToLower() == "iscomplex")
                                                IsComplex = bool.Parse(att.Value);
                                            if (att.Name.ToLower() == "isseparator")
                                                IsSeparator = bool.Parse(att.Value);
                                            if (att.Name.ToLower() == "category")
                                                Category = (att.Value);

                                        }
                                    }
                                    if (Pattern != "")
                                    {
                                        Pattern pat = new Pattern(Pattern, IsComplex, IsSeparator, false);
                                        pat.Category = Category;
                                        pl.Add(pat);
                                    }
                                }
                                else if (pt.Name.ToLower() == "patterns")
                                {
                                    string Patterns = pt.ChildNodes[0].Value;
                                    Patterns = Patterns.Replace("\t", " ");
                                    while (Patterns.IndexOf("  ") >= 0)
                                        Patterns = Patterns.Replace("  ", " ");

                                    string[] pattSplit = Patterns.Split();

                                    foreach (string Pattern in pattSplit)
                                    {
                                        if (Pattern != "")
                                            pl.Add(new Pattern(Pattern, false, false, false));
                                    }
                                }
                            }
                        }
                    }

                if (n.Name.ToLower() == "childblocks")
                {
                    foreach (XmlNode cn in n.ChildNodes)
                    {
                        if (cn.Name.ToLower() == "child")
                        {
                            foreach (XmlAttribute att in cn.Attributes)
                                if (att.Name.ToLower() == "name")
                                    bl.ChildBlocks.Add(GetBlock(att.Value));
                        }
                    }
                }
            }
        }


        //done
        private TextStyle GetStyle(string Name)
        {
            if (mStyles[Name] == null)
            {
                TextStyle s = new TextStyle();
                mStyles.Add(Name, s);
            }

            return (TextStyle)mStyles[Name];
        }

        //done
        private BlockType GetBlock(string Name)
        {
            if (mBlocks[Name] == null)
            {
                GuruComponents.CodeEditor.CodeEditor.Syntax.BlockType b = new GuruComponents.CodeEditor.CodeEditor.Syntax.BlockType(mLanguage);
                mBlocks.Add(Name, b);
            }

            return (BlockType)mBlocks[Name];
        }

        //done
        private void ParseStyle(XmlNode node)
        {
            string Name = "";
            string ForeColor = "", BackColor = "";
            bool Bold = false, Italic = false, Underline = false;



            foreach (XmlAttribute att in node.Attributes)
            {
                if (att.Name.ToLower() == "name")
                    Name = att.Value;

                if (att.Name.ToLower() == "forecolor")
                    ForeColor = att.Value;

                if (att.Name.ToLower() == "backcolor")
                    BackColor = att.Value;

                if (att.Name.ToLower() == "bold")
                    Bold = bool.Parse(att.Value);

                if (att.Name.ToLower() == "italic")
                    Italic = bool.Parse(att.Value);

                if (att.Name.ToLower() == "underline")
                    Underline = bool.Parse(att.Value);
            }

            TextStyle st = GetStyle(Name);

            if (BackColor != "")
            {
                st.BackColor = Color.FromName(BackColor);
            }
            else
            {

            }

            st.ForeColor = Color.FromName(ForeColor);
            st.Bold = Bold;
            st.Italic = Italic;
            st.Underline = Underline;
            st.Name = Name;
        }

    }
}
