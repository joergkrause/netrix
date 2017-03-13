using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.HighLighting;
using GuruComponents.Netrix.SpellChecker.NetSpell;
using GuruComponents.Netrix.SpellChecker.NetSpell.Dictionary;
using System.IO;
using System.Collections.Generic;

namespace GuruComponents.Netrix.SpellChecker
{
    /// <summary>
    /// This realizes the spell checker API using callback methods.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Usage instructions: To use this class add the SpellChecker component to the form where the NetRix HtmlEditor
    /// component is used. It will extend the editor by adding commands and properties for spell checking. The various
    /// properties can be set within the designer. For more information have a look at <see cref="GuruComponents.Netrix.SpellChecker.Speller">Speller</see>
    /// class.
    /// </para>
    /// This module does implement any spell checking feature but it supports word selection and
    /// highlighting. This allows third party developers to attach there own spell checker and commercial
    /// dictionaries.
    /// <para>
    /// Currently the module supports word checking only. That means that the text fragments returned are clipped
    /// on word boundaries.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.SpellChecker.Speller">Speller</seealso>
    /// </remarks>
    public class DocumentSpellChecker : IDocumentSpellChecker
    {
        private ArrayList wordSelections;
        private ArrayList bgwordSelections;
        private IHighLightStyle highLightStyle;
        private bool backgroundService;
        private Interop.IHighlightRenderingServices render = null;
        private string currentWordToCheck;
        private string currentHtmlToCheck;
        private IHtmlEditor editor;
        private bool refireCheck;
        private int wordCount;
        private Spelling netSpell;
        private WordDictionary wordDictionary;
        private Interop.IHighlightRenderingServices bgRenderService = null;
        private Interop.IHTMLRenderStyle bgRenderStyle = null;
        private Interop.IDisplayServices displayService;
        private Interop.IMarkupServices markupService;
        private Interop.IHTMLTxtRange trg;
        private bool wordExists = false;
        private bool checkInternal;

        /// <summary>
        /// Constructor, creates an instance. Used internally through plug-in.
        /// </summary>
        /// <param name="editor">The associated editor.</param>
        public DocumentSpellChecker(IHtmlEditor editor)
        {
            this.editor = editor;
            this.editor.ReadyStateChanged += new ReadyStateChangedHandler(editor_ReadyStateChanged);
            wordDictionary = new WordDictionary();
            wordDictionary.DictionaryFolder = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), "Dictionary");
            wordDictionary.DictionaryFile = "en-US.dic";
            this.netSpell = new Spelling();
            this.netSpell.Dictionary = wordDictionary;
            this.netSpell.EndOfText += new Spelling.EndOfTextEventHandler(this.spellChecker_EndOfText);
            this.netSpell.DeletedWord += new Spelling.DeletedWordEventHandler(this.spellChecker_DeletedWord);
            this.netSpell.ReplacedWord += new Spelling.ReplacedWordEventHandler(this.spellChecker_ReplacedWord);
            this.netSpell.MisspelledWord += new Spelling.MisspelledWordEventHandler(this.spellChecker_MisspelledWord);
            this.netSpell.IgnoredWord += new Spelling.IgnoredWordEventHandler(this.spellChecker_IgnoredWord);
            this.netSpell.DoubledWord += new Spelling.DoubledWordEventHandler(this.spellChecker_DoubledWord);
            checkInternal = true;
        }

        void editor_ReadyStateChanged(object sender, ReadyStateChangedEventArgs e)
        {
            if (e.State == GuruComponents.Netrix.WebEditing.ReadyState.Interactive)
            {
                // Reset all
                RemoveWordCheckingHighlights();
                // force using new document
                activeDocument = null;
                // reset pointers
                bgRenderStyle = null;
                trg = null;
            }
        }

        /// <summary>
        /// Access to the underlying NetSpell instance.
        /// </summary>
        internal Spelling NetSpell
        {
            get { return netSpell; }
        }

        # region NetSpell Events


        /// <summary>
        ///     This event is fired when a word is deleted.
        /// </summary>
        /// <remarks>
        ///		Use this event to update the parent text.
        /// </remarks>
        [Category("NetSpell Events")]
        public event WordEventHandler DeletedWord;

        /// <summary>
        ///     This event is fired when word is detected two times in a row.
        /// </summary>
        [Category("NetSpell Events")]
        public event WordEventHandler DoubledWord;

        /// <summary>
        ///     This event is fired when the spell checker reaches the end of
        ///     the text in the Text property.
        /// </summary>
        [Category("NetSpell Events")]
        public event WordEventHandler EndOfText;

        /// <summary>
        ///     This event is fired when a word is skipped.
        /// </summary>
        [Category("NetSpell Events")]
        public event WordEventHandler IgnoredWord;

        /// <summary>
        ///     This event is fired when the spell checker finds a word that. 
        ///     is not in the dictionaries
        /// </summary>
        [Category("NetSpell Events")]
        public event WordEventHandler MisspelledWord;

        /// <summary>
        /// This event is fired when a word is replaced.
        /// </summary>
        /// <remarks>
        ///	Use this event to update the parent text.
        /// </remarks>
        [Category("NetSpell Events")]
        public event ReplacedWordEventHandler ReplacedWord;

        private void spellChecker_MisspelledWord(object sender, SpellingEventArgs e)
        {
            if (MisspelledWord != null)
            {
                MisspelledWord(sender, e);
            }
        }

        private void spellChecker_DeletedWord(object sender, SpellingEventArgs e)
        {
            if (DeletedWord != null)
            {
                DeletedWord(sender, e);
            }
        }

        private void spellChecker_DoubledWord(object sender, SpellingEventArgs e)
        {
            if (DoubledWord != null)
            {
                DoubledWord(sender, e);
            }
        }

        private void spellChecker_EndOfText(object sender, EventArgs e)
        {
            if (EndOfText != null)
            {
                EndOfText(sender, new SpellingEventArgs(null, 0, 0, null));
            }
        }

        private void spellChecker_IgnoredWord(object sender, SpellingEventArgs e)
        {
            if (IgnoredWord != null)
            {
                IgnoredWord(sender, e);
            }
            if (BackgroundService && BgWordSelections != null && BgWordSelections.Count > 0)
            {
                foreach (SegmentStore segment in BgWordSelections)
                {
                    if (segment.Word == e.Word)
                    {
                        // hit the ignored word
                        bgRenderService.RemoveSegment(segment.segment);
                        break;
                    }
                }
            }
        }

        // htmleditor has requested a word replacement 
        // method further delegates this event to directly attached event
        private void spellChecker_ReplacedWord(object sender, ReplaceWordEventArgs e)
        {
            if (ReplacedWord != null)
            {
                ReplacedWord(sender, e);
            }
            if (trg != null)
            {
                string oldWord = trg.GetText();
                // double check that we're still on the very same word in document
                if (!String.IsNullOrEmpty(oldWord) && e.Word.Equals(oldWord))
                {
                    trg.SetText(e.ReplacementWord);
                }
            }
        }

        # endregion

        # region Put Through Events

        /// <summary>
        /// Event fired for each word found.
        /// </summary>
        public event WordCheckerHandler WordChecker;

        private bool InvokeWordChecker(WordEventArgs e)
        {
            if (CheckInternal)
            {
                netSpell.Text = e.Word;
                bool wrong = netSpell.SpellCheck(e.Word);
                if (netSpell.ShowDialog && netSpell.SpellcheckerForm != null && netSpell.SpellcheckerForm is ISpellcheckerDialog)
                {
                    switch (netSpell.SpellcheckerForm.ShowDialog())
                    {
                        case DialogResult.OK:
                            e.ReplacementWord = ((ISpellcheckerDialog)netSpell.SpellcheckerForm).NewWord;
                            return true;
                        default:
                        case DialogResult.Cancel:
                        case DialogResult.Abort:
                            //TODO: Cancel spelling
                            return wrong;
                        case DialogResult.Ignore:
                            return wrong;
                    }
                }
                else
                {
                    return wrong;
                }
            }
            else
            {
                return WordChecker(this, e);
            }
        }

        /// <summary>
        /// Event fired for each word found. The return value of the callback function is used to stop the checking
        /// process immediately.
        /// </summary>
        public event WordCheckerStopHandler WordCheckerStop;
        /// <summary>
        /// Event fired once the word checking is done.
        /// </summary>
        public event WordCheckerFinishedHandler WordCheckerFinished;

        /// <summary>
        /// Event fired if the suer has right clicked the word and the caret has moved.
        /// </summary>
        /// <remarks>
        /// This event is later than
        /// ContextMenu because it is necessary to being delayed to set the caret to the final position before
        /// an attached context menu opens.
        /// </remarks>
        public event WordOnContextHandler WordOnContext;

        private void InvokeWordOnContext(WordOnContextEventArgs e)
        {
            netSpell.Text = e.Word;
            e.Suggestions = netSpell.Suggestions;
            WordOnContext(this, e);
        }

        private void OnWordCheckerFinished()
        {
            if (WordCheckerFinished != null)
            {
                WordCheckerFinished(editor, new WordCheckerEventArgs(this.wordCount, WordSelections));
            }
        }


        # endregion Put Through Events

        # region Properties

        private Interop.IHTMLDocument2 activeDocument;

        private Interop.IHTMLDocument2 ActiveDocument
        {
            get
            {
                if (activeDocument == null)
                {
                    activeDocument = editor.GetActiveDocument(false);
                    displayService = null;
                    markupService = null;
                }
                return activeDocument;
            }
        }

        private Interop.IDisplayServices DisplayService
        {
            get
            {
                if (displayService == null)
                {
                    displayService = (Interop.IDisplayServices)ActiveDocument;
                }
                return displayService;
            }
        }

        private Interop.IMarkupServices MarkupService
        {
            get
            {
                if (markupService == null)
                {
                    markupService = (Interop.IMarkupServices)ActiveDocument;
                }
                return markupService;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="GuruComponents.Netrix.WebEditing.HighLighting.HighLightStyle"/> for the
        /// spell checker.
        /// </summary>
        /// <remarks>
        /// The style is used to highlight words marked as wrong.
        /// </remarks>
        [Browsable(true), Category("NetRix Component"), DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IHighLightStyle HighLightStyle
        {
            get
            {
                if (highLightStyle == null)
                {	// defaults for designtime support
                    highLightStyle = new HighLightStyle();
                    highLightStyle.LineColor = new HighlightColor(Color.Red, ColorType.Color);
                    highLightStyle.UnderlineStyle = UnderlineStyle.Wave;
                }
                return highLightStyle;
            }
            set
            {
                highLightStyle = value;
            }
        }

        /// <summary>
        /// Activates or deactivates the background service.
        /// </summary>
        /// <remarks>
        /// This service runs through the document and restarts
        /// at the end automatically. Setting this property to <c>false</c> will stop the service.
        /// </remarks>
        [Browsable(true), Category("NetRix Component"), DefaultValue(false)]
        public bool BackgroundService
        {
            get
            {
                return backgroundService;
            }
            set
            {
                backgroundService = value;
            }
        }

        # endregion

        /// <summary>
        /// Remove the highlighted sections from the whole document.
        /// </summary>
        public void RemoveWordCheckingHighlights()
        {
            if (WordSelections.Count > 0 || BgWordSelections.Count > 0)
            {
                foreach (SegmentStore ss in WordSelections)
                {
                    ss.RemoveHighlight();
                }
                foreach (SegmentStore ss in BgWordSelections)
                {
                    ss.RemoveHighlight();
                }
                wordSelections.Clear();
                wordSelections = null;
                bgwordSelections.Clear();
                bgwordSelections = null;
            }
        }

        private ArrayList WordSelections
        {
            get
            {
                if (wordSelections == null)
                {
                    wordSelections = new ArrayList();
                }
                return wordSelections;
            }
        }

        private ArrayList BgWordSelections
        {
            get
            {
                if (bgwordSelections == null)
                {
                    bgwordSelections = new ArrayList();
                }
                return bgwordSelections;
            }
        }

        private static bool IsWordChar(string Chars)
        {
            if (Chars.Length == 0)
            {
                return false;
            }
            else
            {
                return Char.IsLetterOrDigit(Chars, Chars.Length - 1);
            }
        }


        /// <summary>
        /// Checks the complete document and highlight any wrong word using default highlight style.
        /// </summary>
        public void DoWordCheckingHighlights()
        {
            DoWordCheckingHighlights(HighLightStyle);
        }

        /// <summary>
        /// Checks the complete document and highlight any wrong word.
        /// </summary>
        /// <remarks>
        /// The method calls a callback method the host application
        /// must provide and which returns a boolean value to control the behavior of the highlighter.
        /// </remarks>
        /// <param name="highLightStyle">The style used to mark wrong words</param>
        public void DoWordCheckingHighlights(IHighLightStyle highLightStyle)
        {
            this.HighLightStyle = highLightStyle;
            DoWordChecking();
        }

        /// <summary>
        /// Checks the complete document and highlight any wrong word.
        /// </summary>
        /// <remarks>
        /// The method calls a callback method the host application
        /// must provide and which returns a boolean value to control the behavior of the highlighter.
        /// </remarks>
        /// <param name="highLightStyle">The style used to mark wrong word</param>
        /// <param name="backgroundService">If true the service runs as a endless background process until the WordStop callback handler returns true.</param>
        public void DoWordCheckingHighlights(IHighLightStyle highLightStyle, bool backgroundService)
        {
            this.HighLightStyle = highLightStyle;
            this.BackgroundService = backgroundService;
            if (BackgroundService)
            {
                DoCheckBeforeBackgroundWordChecking();
                this.editor.PostEditorEvent -= new PostEditorEventHandler(editor_PostEditorEvent);
                ((Control)this.editor).MouseDown -= new MouseEventHandler(editor_MouseDown);
                ((Control)this.editor).MouseUp -= new MouseEventHandler(editor_MouseUp);
                this.editor.ShowContextMenu -= new ShowContextMenuEventHandler(editor_ShowContextMenu);

                this.editor.PostEditorEvent += new PostEditorEventHandler(editor_PostEditorEvent);
                ((Control)this.editor).MouseDown += new MouseEventHandler(editor_MouseDown);
                ((Control)this.editor).MouseUp += new MouseEventHandler(editor_MouseUp);
                this.editor.ShowContextMenu += new ShowContextMenuEventHandler(editor_ShowContextMenu);
            }
            else
            {
                DoWordChecking();
            }
        }

        /// <summary>
        /// Checks the complete document and select any wrong word with the standard selection.
        /// </summary>
        public void DoWordCheckingSelected()
        {
            this.HighLightStyle = null;
            this.BackgroundService = false;
            DoWordChecking();
        }

        /// <summary>
        /// Checks a block (normally, a sentence) and forwards the block to the spellchecker callback.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="highLightStyle">The style being used to highlight (select) the current block.</param>
        public void DoBlockCheckingHighlights(IHighLightStyle highLightStyle)
        {
            this.HighLightStyle = highLightStyle;
            this.BackgroundService = false;
            DoBlockChecking();
        }

        /// <summary>
        /// Checks the complete document interactively and tags the current word in default selection style.
        /// </summary>
        public void DoBlockCheckingSelected()
        {
            this.HighLightStyle = null;
            this.BackgroundService = false;
            DoBlockChecking();
        }

        private void DoBlockChecking()
        {
            RemoveWordCheckingHighlights();
            // Highlighter
            render = (Interop.IHighlightRenderingServices)ActiveDocument;
            Interop.IHTMLRenderStyle renderStyle = null;
            if (HighLightStyle != null)
            {
                renderStyle = ((HighLightStyle)HighLightStyle).GetRenderStyle(ActiveDocument);
            }
            // Marker
            Interop.IDisplayPointer dpStart, dpEnd;
            trg = ((Interop.IHtmlBodyElement)ActiveDocument.GetBody()).createTextRange();
            Interop.IMarkupPointer pCursor1, pCursor2;
            DisplayService.CreateDisplayPointer(out dpStart);
            DisplayService.CreateDisplayPointer(out dpEnd);
            MarkupService.CreateMarkupPointer(out pCursor1);
            MarkupService.CreateMarkupPointer(out pCursor2);
            // now reset pointer to caret 
            trg.Collapse(true);
            int me = trg.MoveEnd("sentence", 1);
            //MarkupService.MovePointersToRange(trg, pCursor1, pCursor2);
            while (me == 1)
            {
                if (trg.GetText() != null)
                {
                    string block = trg.GetText();
                    WordEventArgs seags = new WordEventArgs(block, trg.GetHtmlText(), wordCount);
                    // true if still a wrong word found
                    if (InvokeWordChecker(seags))
                    {
                        try
                        {
                            // search occurence of wrong word and ...
                            //trg.FindText(seags.Word, 1, 1);							
                            if (seags.Word != seags.ReplacementWord && seags.ReplacementWord != null)
                            {
                                // ... try to replace
                                trg.SetText(seags.ReplacementWord);
                            }
                            else
                            {
                                if (trg.GetText() != null && HighLightStyle != null)
                                {
                                    trg.Select();
                                }
                                else
                                {
                                    // ... tag as wrong
                                    MarkupService.MovePointersToRange(trg, pCursor1, pCursor2);
                                    // Set display pointers to mark words with highlightservices
                                    dpStart.MoveToMarkupPointer(pCursor1, null);
                                    dpEnd.MoveToMarkupPointer(pCursor2, null);
                                    Interop.IHighlightSegment ppi;
                                    render.AddSegment(dpStart, dpEnd, renderStyle, out ppi);
                                    WordSelections.Add(new SegmentStore(MarkupService, ppi, pCursor1, pCursor2, seags.Word, render));
                                }
                            }
                            continue;
                        }
                        catch
                        {
                            // avoid wrong positioning at beginning and end
                        }
                    }
                }
                trg.MoveStart("sentence", 1);
                me = trg.MoveEnd("sentence", 1);
            }
            if (!BackgroundService)
            {
                this.editor.SendIDMCommand((int)Interop.IDM.CLEARSELECTION);
                OnWordCheckerFinished();
            }
        }

        private void DoWordChecking()
        {
            RemoveWordCheckingHighlights();
            // Highlighter
            Interop.IHTMLRenderStyle renderStyle = null;
            render = (Interop.IHighlightRenderingServices)ActiveDocument;
            if (HighLightStyle != null)
            {
                renderStyle = ((HighLightStyle)HighLightStyle).GetRenderStyle(ActiveDocument);
            }
            // Marker
            trg = ((Interop.IHtmlBodyElement)ActiveDocument.GetBody()).createTextRange();
            Interop.IMarkupPointer pStart, pEnd, pCursor1, pCursor2;
            Interop.IDisplayPointer dpStart, dpEnd;
            DisplayService.CreateDisplayPointer(out dpStart);
            DisplayService.CreateDisplayPointer(out dpEnd);
            MarkupService.CreateMarkupPointer(out pStart);
            MarkupService.CreateMarkupPointer(out pEnd);
            MarkupService.CreateMarkupPointer(out pCursor1);
            MarkupService.CreateMarkupPointer(out pCursor2);
            MarkupService.MovePointersToRange(trg, pStart, pEnd);
            // set start location for pointers
            pCursor1.MoveToPointer(pStart);
            pCursor2.MoveToPointer(pStart);
            pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_PREVCLUSTERBEGIN);
            pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_PREVCLUSTERBEGIN);
            pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDEND);
            pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_PREVWORDEND);
            int bDone = 0;
            while (bDone == 0)
            {
                # region While
                pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDBEGIN);
                pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDEND);
                pCursor2.IsEqualTo(pEnd, out bDone);
                if (bDone == 0)
                {
                    pCursor2.IsEqualTo(pCursor1, out bDone);
                    if (bDone != 0)
                    {
                        pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTCLUSTERBEGIN);
                        pCursor2.MoveToPointer(pCursor1);
                        pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDEND);
                        pCursor2.IsEqualTo(pCursor1, out bDone);
                        pCursor1.MoveToPointer(pCursor2);
                    }
                }
                MarkupService.MoveRangeToPointers(pCursor1, pCursor2, trg);
                if (trg.GetText() != null)
                {
                    string txt = trg.GetText().Trim();
                    if (IsWordChar(txt) && txt.Length >= MinCharCount)
                    {
                        wordCount++;
                        string word = trg.GetText().Trim();
                        if (WordChecker != null)
                        {
                            WordEventArgs seags = new WordEventArgs(word, trg.GetHtmlText(), wordCount);
                            string oldWord = word;
                            if (HighLightStyle != null)
                            {
                                if (WordChecker(editor, seags))
                                {
                                    try
                                    {
                                        // Set display pointers to mark words with highlightservices
                                        dpStart.MoveToMarkupPointer(pCursor1, null);
                                        dpEnd.MoveToMarkupPointer(pCursor2, null);
                                        Interop.IHighlightSegment ppi;
                                        render.AddSegment(dpStart, dpEnd, renderStyle, out ppi);
                                        word = seags.ReplacementWord;
                                        if (!word.Equals(oldWord))
                                        {
                                            trg.SetText(seags.ReplacementWord);
                                            if (netSpell.SpellCheck(word))
                                            {
                                                WordSelections.Add(new SegmentStore(MarkupService, ppi, pCursor1, pCursor2, word, render));
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        // avoid wrong positioning at beginning and end
                                    }
                                }
                            }
                            else
                            {
                                trg.Select();
                                trg.ScrollIntoView(true);
                                if (WordChecker(editor, seags))
                                {
                                    if (!seags.Word.Equals(oldWord))
                                    {
                                        trg.SetText(seags.Word);
                                        trg.Expand("word");
                                        pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_PREVWORDEND);
                                        pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDBEGIN);
                                        pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDBEGIN);
                                        pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_PREVWORDEND);
                                    }
                                }
                            }
                        }
                        MarkupService.MoveRangeToPointers(pCursor1, pCursor2, trg);
                    }
                }
                if (WordCheckerStop != null)
                {
                    
                    if (WordCheckerStop(editor, EventArgs.Empty))
                    {
                        BackgroundService = false;
                        RemoveWordCheckingHighlights();
                        break;
                    }
                }
                # endregion
            }
            if (!BackgroundService)
            {
                this.editor.SendIDMCommand((int)Interop.IDM.CLEARSELECTION);
                OnWordCheckerFinished();
            }
        }

        private void DoCheckBeforeBackgroundWordChecking()
        {
            if (HighLightStyle != null)
            {
                bgRenderStyle = ((HighLightStyle)HighLightStyle).GetRenderStyle(ActiveDocument);
            }
            wordCount = 0;
            bgRenderService = ActiveDocument as Interop.IHighlightRenderingServices;
            // Reset
            RemoveWordCheckingHighlights();
            // Global Marker
            Interop.IMarkupPointer pStart, pEnd;
            Interop.IDisplayPointer dpStart, dpEnd;
            DisplayService.CreateDisplayPointer(out dpStart);
            DisplayService.CreateDisplayPointer(out dpEnd);
            MarkupService.CreateMarkupPointer(out pStart);
            pStart.SetCling(1);
            MarkupService.CreateMarkupPointer(out pEnd);
            pEnd.SetCling(1);
            // Range
            trg = ((Interop.IHtmlBodyElement)ActiveDocument.GetBody()).createTextRange();
            // Local Marker
            Interop.IMarkupPointer pCursor1, pCursor2;
            MarkupService.CreateMarkupPointer(out pCursor1);
            MarkupService.CreateMarkupPointer(out pCursor2);
            MarkupService.MovePointersToRange(trg, pStart, pEnd);
            // set start location for pointers
            pCursor1.MoveToPointer(pStart);
            pCursor2.MoveToPointer(pStart);
            pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_PREVCLUSTERBEGIN);
            pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_PREVCLUSTERBEGIN);
            pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDEND);
            pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_PREVWORDEND);
            int bDone = 0;
            // Start one complete check before activating background checking	
            while (bDone == 0)
            {
                # region While
                pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDBEGIN);
                pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDEND);
                pCursor2.IsEqualTo(pEnd, out bDone);
                if (bDone == 0)
                {
                    pCursor2.IsEqualTo(pCursor1, out bDone);
                    if (bDone != 0)
                    {
                        pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTCLUSTERBEGIN);
                        pCursor2.MoveToPointer(pCursor1);
                        pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDEND);
                        pCursor2.IsEqualTo(pCursor1, out bDone);
                        pCursor1.MoveToPointer(pCursor2);
                    }
                }
                MarkupService.MoveRangeToPointers(pCursor1, pCursor2, trg);
                if (trg.GetText() != null)
                {
                    string txt = trg.GetText().Trim();
                    if (IsWordChar(txt) && txt.Length >= MinCharCount)
                    {
                        wordCount++;
                        string word = trg.GetText().Trim();
                        if (WordChecker != null)
                        {
                            string oldWord = word;
                            Debug.Assert(HighLightStyle != null);
                            WordEventArgs seags = new WordEventArgs(word, trg.GetHtmlText(), wordCount);
                            if (InvokeWordChecker(seags))
                            {
                                try
                                {
                                    // Set display pointers to mark words with highlightservices
                                    dpStart.MoveToMarkupPointer(pCursor1, null);
                                    dpEnd.MoveToMarkupPointer(pCursor2, null);
                                    Interop.IHighlightSegment ppi;
                                    bgRenderService.AddSegment(dpStart, dpEnd, bgRenderStyle, out ppi);
                                    BgWordSelections.Add(new SegmentStore(MarkupService, ppi, pCursor1, pCursor2, word, bgRenderService));
                                    if (!seags.Word.Equals(oldWord))
                                    {
                                        trg.SetText(seags.Word);
                                    }
                                }
                                catch
                                {
                                    // avoid wrong positioning at beginning and end
                                }
                            }
                        }
                        MarkupService.MoveRangeToPointers(pCursor1, pCursor2, trg);
                    }
                }
                if (WordCheckerStop != null)
                {
                    if (WordCheckerStop(editor, EventArgs.Empty))
                    {
                        BackgroundService = false;
                        RemoveWordCheckingHighlights();
                        break;
                    }
                }
                # endregion
            }
            // The keybord/mouse event will now drive the current word checking	
            wordExists = false;
        }


        /// <summary>
        /// Returns the collection of misspelled words.
        /// </summary>
        /// <remarks>
        /// This collection contains objects of type <see cref="SegmentStore"/>, a struct, which contains
        /// the misspelled word and the reference to the highlight engine. You can call
        /// <see cref="SegmentStore.RemoveHighlight"/> to remove the highlighting of a specific word.
        /// <para>
        /// <b>Important:</b> This property is only available if background spelling is enabled.  
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public IList MisSpelledWords
        {
            get
            {
                return BgWordSelections;
            }
        }

        private bool reformatHtml = true;
        /// <summary>
        /// Try to persist the inline HTML at the exact position it was before the word was replaced.
        /// </summary>
        [Browsable(true), Category("NetRix Component"), DefaultValue(true), Description("Try to persist the inline HTML at the exact position it was before the word was replaced.")]
        public bool ReformatHtml
        {
            get
            {
                return reformatHtml;
            }
            set
            {
                reformatHtml = value;
            }
        }

        private uint minCharCount = 0;
        /// <summary>
        /// The minimum number of characters a word must have to being checked.
        /// </summary>
        /// <remarks>
        /// This property applies to both, spellchecker and background spellchecker.
        /// </remarks>
        [Browsable(true), Category("NetRix Component"), DefaultValue(typeof(uint), "0")]
        [Description("The minimum number of characters a word must have to being checked.")]
        public uint MinCharCount
        {
            get
            {
                return minCharCount;
            }
            set
            {
                minCharCount = value;
            }
        }

        private void GetCurrentWord()
        {
            GetCurrentWord(true, false);
        }

        Interop.IMarkupPointer pStart, pEnd;
        Interop.IDisplayPointer dpStart, dpEnd;

        /// <summary>
        /// Get the current word left, right or under caret, depending on movement and punctuation.
        /// </summary>
        /// <param name="expand"></param>
        /// <param name="onReturn"></param>
        private void GetCurrentWord(bool expand, bool onReturn)
        {
            Interop.IHTMLSelectionObject selTest = ActiveDocument.GetSelection();
            object sel = selTest.CreateRange();
            if (sel is Interop.IHTMLControlRange) return;
            try
            {
                // reset pointer to caret 
                Interop.IHTMLCaret cr;
                DisplayService.GetCaret(out cr);
                MarkupService.CreateMarkupPointer(out pStart);
                MarkupService.CreateMarkupPointer(out pEnd);
                cr.MoveMarkupPointerToCaret(pStart);
                cr.MoveMarkupPointerToCaret(pEnd);
                Interop.CARET_DIRECTION dir;
                cr.GetCaretDirection(out dir);
                // Move range to pointer
                MarkupService.MoveRangeToPointers(pStart, pEnd, trg);
                int result;
                int breakCount = 0;
                while (trg.GetText() == null && ++breakCount < 4)
                {
                    if (dir == Interop.CARET_DIRECTION.CARET_DIRECTION_BACKWARD)
                    {
                        // look forward for word if caret runs backward
                        result = trg.MoveEnd("word", onReturn ? 2 : 1);
                        if ((result == 1 || result == 2) && expand)
                        {
                            // in case of success expand, to get full word
                            if (trg.Expand("word")) continue;
                        }
                        // look forward only if unsuccessfully
                        // avoid trailing spaces being part of the current range
                        TrimRange(trg);
                        // if still null, try looking backward
                        if (trg.GetText() == null)
                        {
                            trg.MoveEnd("word", -1);
                            trg.MoveStart("word", -1);
                        }
                        break;
                    }
                    else
                    {
                        // look backward for word if caret runs forward
                        result = trg.MoveStart("word", onReturn ? -2 : -1);
                        if ((result == -1 || result == -2) && expand)
                        {
                            // in case of success expand, to get full word
                            if (trg.Expand("word")) continue;
                        }
                        // if we got a word boundary (., etc.) we need to check both directions:
                        if (trg.GetText() != null && trg.GetText().Length == 1 && Char.IsPunctuation(trg.GetText()[0]))
                        {
                            refireCheck = true;
                        }
                        else
                        {
                            refireCheck = false;
                        }
                        // look forward only if unsuccessfully
                        // avoid trailing spaces being part of the current range
                        TrimRange(trg);
                        result = trg.MoveEnd("word", onReturn ? 0 : 1);
                        if (result == 1) continue;
                        // break if no result
                        break;
                    }
                }
                TrimRange(trg);
                if (trg.GetText() != null)
                {
                    wordExists = true;
                    currentWordToCheck = trg.GetText();
                    currentHtmlToCheck = trg.GetHtmlText();
                }
                else
                {
                    wordExists = false;
                    currentWordToCheck = null;
                    currentHtmlToCheck = null;
                }
                MarkupService.MovePointersToRange(trg, pStart, pEnd);
            }
            catch
            {
                // ignore errors 
            }
        }

        private static void TrimRange(Interop.IHTMLTxtRange tr)
        {
            string txt;
            if (tr.GetText() != null)
            {
                txt = tr.GetText();
                int i = txt.Length - 1;
                while (txt != null
                    &&
                    i > 0 && i < txt.Length
                    &&
                    (
                    Char.IsPunctuation(txt[i])
                    ||
                    Char.IsWhiteSpace(txt[i])
                    ))
                {
                    tr.MoveEnd("character", -1);
                    txt = tr.GetText();
                    i--;
                }
                i = 0;
                while (txt != null
                    &&
                    i < txt.Length
                    &&
                    (
                    Char.IsPunctuation(txt[i])
                    ||
                    Char.IsWhiteSpace(txt[i])
                    ))
                {
                    tr.MoveStart("character", 1);
                    txt = tr.GetText();
                    i++;
                }
            }
        }

        /// <summary>
        /// Allows access to the collection of misspelled words during background checking.
        /// </summary>
        public struct SegmentStore
        {

            internal Interop.IHighlightSegment segment;
            internal Interop.IMarkupPointer pStartInternal;
            internal Interop.IMarkupPointer pEndInternal;
            internal string word;
            private Interop.IHighlightRenderingServices bgRenderService;

            internal SegmentStore(Interop.IMarkupServices ims, Interop.IHighlightSegment segment, Interop.IMarkupPointer pStart, Interop.IMarkupPointer pEnd, string word, Interop.IHighlightRenderingServices bgRenderService)
            {
                ims.CreateMarkupPointer(out pStartInternal);
                ims.CreateMarkupPointer(out pEndInternal);
                pStartInternal.MoveToPointer(pStart);
                pEndInternal.MoveToPointer(pEnd);
                this.segment = segment;
                this.word = word;
                this.bgRenderService = bgRenderService;
            }

            internal Interop.IMarkupPointer pStart
            {
                get
                {
                    return pStartInternal;
                }
            }

            internal Interop.IMarkupPointer pEnd
            {
                get
                {
                    return pEndInternal;
                }
            }


            /// <summary>
            /// Misspelled word.
            /// </summary>
            public string Word
            {
                get
                {
                    return word;
                }
            }

            /// <summary>
            /// Remove the highlighting from associated word.
            /// </summary>
            public void RemoveHighlight()
            {
                bgRenderService.RemoveSegment(segment);
            }

        }

        /// <summary>
        /// Check current word, if exists.
        /// </summary>
        private void CheckWord()
        {
            if (wordExists && WordChecker != null && currentWordToCheck.Length >= MinCharCount)
            {
                int wordCount = 0;
                //Interop.IMarkupPointer pStart;
                //Interop.IMarkupPointer pEnd;
                //MarkupService.CreateMarkupPointer(out pStart);
                //MarkupService.CreateMarkupPointer(out pEnd);
                if (dpStart == null)
                    DisplayService.CreateDisplayPointer(out dpStart);
                if (dpEnd == null)
                    DisplayService.CreateDisplayPointer(out dpEnd);
                Interop.IHighlightSegment ppi;
                //currentWordToCheck;
                // look if the same word was previously checked and remove exactly THAT highlight
                //MarkupService.MovePointersToRange(trg, pStart, pEnd);
                dpStart.MoveToMarkupPointer(pStart, null);
                dpEnd.MoveToMarkupPointer(pEnd, null);
                RemoveHighLight(pStart, pEnd);
                if (currentWordToCheck == null) return;
                string oldWort = currentWordToCheck;
                string oldHtml = currentHtmlToCheck;
                WordEventArgs seags = new WordEventArgs(currentWordToCheck, currentHtmlToCheck, wordCount);
                while (InvokeWordChecker(seags))
                {
                    if (oldWort != seags.Word && seags.Word != null)
                    {
                        if (ReformatHtml)
                        {
                            string newText = MergeFormatting(oldHtml, seags.Word);
                            trg.PasteHTML(newText);
                        }
                        else
                        {
                            trg.SetText(seags.Word);
                        }
                        oldWort = currentWordToCheck;
                        continue;
                    }
                    MarkupService.MovePointersToRange(trg, pStart, pEnd);
                    dpStart.MoveToMarkupPointer(pStart, null);
                    dpEnd.MoveToMarkupPointer(pEnd, null);
                    // Add wrong word to list of highlighted words						
                    bgRenderService.AddSegment(dpStart, dpEnd, bgRenderStyle, out ppi);
                    BgWordSelections.Add(new SegmentStore(MarkupService, ppi, pStart, pEnd, currentWordToCheck, bgRenderService));
                    break;
                }
            }
            refireCheck = true;
        }

        private static string MergeFormatting(string src, string trg)
        {
            StringBuilder fin = new StringBuilder();
            for (int i = 0, j = 0; i < trg.Length; )
            {
                if (j + 1 == src.Length)
                {
                    // copy rest of src, and terminate
                    fin.Append(trg, i, trg.Length - i);
                    break;
                }
                if (src[j] >= (char)0x9 && src[j] <= (char)0xD)
                {
                    // copy newline and tab chars directly, do not calculate from target
                    fin.Append(src[j]);
                    j++;
                    continue;
                }
                if (src[j] != '<')
                {
                    // append if no tag appears
                    fin.Append(trg[i]);
                    j++;
                    i++;
                }
                else
                {
                    // add tag, then procede
                    do
                    {
                        fin.Append(src[j]);
                        if (j + 1 == src.Length) break;
                    } while (src[j++] != '>');
                }
            }
            return fin.ToString();
        }

        private bool RemoveHighLight(Interop.IMarkupPointer BS, Interop.IMarkupPointer BE)
        {
            int AS_L_BS, AS_RE_BS, AS_R_BS, AS_LE_BS;
            int AS_L_BE, AS_R_BE, AS_RE_BE, AS_LE_BE;

            int AE_LE_BS, AE_R_BS, AE_L_BS, AE_RE_BS;
            int AE_LE_BE, AE_RE_BE, AE_L_BE, AE_R_BE;

            int AS_E_BS, AS_E_BE, AE_E_BS, AE_E_BE;
            //MarkupService.MoveRangeToPointers(BS, BE, trg);
            //System.Diagnostics.Debug.WriteLine(trg.GetText() != null ? trg.GetText() : "NULL", " REMOVE B");
            foreach (SegmentStore A in BgWordSelections)
            {
                //				System.Diagnostics.Debug.Write(A.Word, "REMOVE A");

                // Prepare

                // START <-> START
                A.pStart.IsLeftOf(BS, out AS_L_BS);			// AS  < BS
                A.pStart.IsLeftOfOrEqualTo(BS, out AS_LE_BS);			// AS <= BS
                A.pStart.IsRightOf(BS, out AS_R_BS);			// AS  > BS
                A.pStart.IsRightOfOrEqualTo(BS, out AS_RE_BS);			// AS >= BS
                A.pStart.IsEqualTo(BS, out AS_E_BS);			// AS == BS
                // START <-> END
                A.pStart.IsLeftOf(BE, out AS_L_BE);			// AS  < BE
                A.pStart.IsLeftOfOrEqualTo(BE, out AS_LE_BE);			// AS <= BE
                A.pStart.IsRightOf(BE, out AS_R_BE);			// AS  > BE
                A.pStart.IsRightOfOrEqualTo(BE, out AS_RE_BE);			// AS >= BE
                A.pStart.IsEqualTo(BE, out AS_E_BE);			// AS == BE
                // END <-> START
                A.pEnd.IsLeftOf(BS, out AE_L_BS);
                A.pEnd.IsLeftOfOrEqualTo(BS, out AE_LE_BS);
                A.pEnd.IsRightOf(BS, out AE_R_BS);
                A.pEnd.IsRightOfOrEqualTo(BS, out AE_RE_BS);
                A.pEnd.IsEqualTo(BS, out AE_E_BS);			// AS == BS
                // END <-> END
                A.pEnd.IsLeftOf(BE, out AE_L_BE);
                A.pEnd.IsLeftOfOrEqualTo(BE, out AE_LE_BE);
                A.pEnd.IsRightOf(BE, out AE_R_BE);
                A.pEnd.IsRightOfOrEqualTo(BE, out AE_RE_BE);
                A.pEnd.IsEqualTo(BE, out AE_E_BE);			// AS == BS


                if (AS_E_BS == 1 || AE_E_BE == 1)
                {
                    bgRenderService.RemoveSegment(A.segment);
                    BgWordSelections.Remove(A);
                    return true;
                }

                // Group 1, A end pointer is within B word			
                if (AS_L_BS == 1 && AE_L_BE == 1 && AE_R_BS == 1)
                {
                    //					System.Diagnostics.Debug.WriteLine(A.word, "REMOVE 1!");
                    bgRenderService.RemoveSegment(A.segment);
                    BgWordSelections.Remove(A);
                    return true;
                }
                // Group 2, A start pointer is within B word
                if (AS_R_BS == 1 && AE_R_BE == 1 && AS_L_BE == 1)
                {
                    //					System.Diagnostics.Debug.WriteLine(A.word, "REMOVE 2!");
                    bgRenderService.RemoveSegment(A.segment);
                    BgWordSelections.Remove(A);
                    return true;
                }
                // Group 3, A word is completely inside or equal B  or  B word is completely inside or equal A
                if ((AS_L_BS == 1 && AE_R_BE == 1) || (AS_R_BS == 1 && AE_L_BE == 1))
                {
                    //					System.Diagnostics.Debug.WriteLine(A.word, "REMOVE 3!");
                    bgRenderService.RemoveSegment(A.segment);
                    BgWordSelections.Remove(A);
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Returns the word the caret is on on nearby.
        /// </summary>
        /// <returns>The recognized word, or <c>null</c>, if nothing was recognized.</returns>
        public string WordUnderPointer()
        {
            return WordUnderPointer(false);
        }

        /// <summary>
        /// Replaces the word the caret is currently in with other text.
        /// </summary>
        /// <param name="replaceWith">Text for replacement.</param>
        public void ReplaceWordUnderPointer(string replaceWith)
        {
            ReplaceWordUnderPointer(false, replaceWith);
        }

        /// <summary>
        ///     Checks to see if the word is in the dictionary.
        /// </summary>
        /// <param name="word" type="string">
        ///     <para>
        ///         The word to check.
        ///     </para>
        /// </param>
        /// <returns>
        ///     Returns true if word is found in dictionary.
        /// </returns>
        public bool TestWord(string word)
        {
            return netSpell.TestWord(word);
        }

        /// <summary>
        ///     Populates the <see cref="Suggestions"/> property with word suggestions.
        ///     for the word
        /// </summary>
        /// <param name="word" type="string">
        ///     <para>
        ///         The word to generate suggestions on.
        ///     </para>
        /// </param>
        /// <remarks>
        ///		This method sets the <see cref="Text"/> property to the word. 
        ///		Then calls <see cref="TestWord"/> on the word to generate the need
        ///		information for suggestions. Note that the Text, CurrentWord and WordIndex 
        ///		properties are set when calling this method.
        /// </remarks>
        /// <seealso cref="Suggestions"/>
        /// <seealso cref="TestWord"/>
        public void Suggest(string word)
        {
            netSpell.Suggest(word);
        }
        /// <summary>
        ///     Populates the <see cref="Suggestions"/> property with word suggestions.
        ///     for the current word.
        /// </summary>
        /// <remarks>
        ///		<see cref="TestWord"/> must have been called before calling this method.
        /// </remarks>
        /// <seealso cref="Suggestions"/>
        /// <seealso cref="TestWord"/>
        public void Suggest()
        {
            netSpell.Suggest();
        }

        /// <summary>
        ///     Deletes the CurrentWord from the Text Property
        /// </summary>
        /// <remarks>
        ///		Note, calling ReplaceWord with the ReplacementWord property set to 
        ///		an empty string has the same behavior as DeleteWord.
        /// </remarks>
        public void DeleteWord()
        {
            netSpell.DeleteWord();
        }

        /// <summary>
        ///     Calculates the minimum number of change, inserts or deletes
        ///     required to change firstWord into secondWord
        /// </summary>
        /// <param name="source" type="string">
        ///     <para>
        ///         The first word to calculate
        ///     </para>
        /// </param>
        /// <param name="target" type="string">
        ///     <para>
        ///         The second word to calculate
        ///     </para>
        /// </param>
        /// <param name="positionPriority" type="bool">
        ///     <para>
        ///         set to true if the first and last char should have priority
        ///     </para>
        /// </param>
        /// <returns>
        ///     The number of edits to make firstWord equal secondWord
        /// </returns>
        public int EditDistance(string source, string target, bool positionPriority)
        {
            return netSpell.EditDistance(source, target, positionPriority);
        }

        /// <summary>
        ///     Calculates the minimum number of change, inserts or deletes
        ///     required to change firstWord into secondWord
        /// </summary>
        /// <param name="source" type="string">
        ///     <para>
        ///         The first word to calculate
        ///     </para>
        /// </param>
        /// <param name="target" type="string">
        ///     <para>
        ///         The second word to calculate
        ///     </para>
        /// </param>
        /// <returns>
        ///     The number of edits to make firstWord equal secondWord
        /// </returns>
        /// <remarks>
        ///		This method automatically gives priority to matching the first and last char
        /// </remarks>
        public int EditDistance(string source, string target)
        {
            return netSpell.EditDistance(source, target, true);
        }

        /// <summary>
        ///		Gets the word index from the text index.  Use this method to 
        ///		find a word based on the text position.
        /// </summary>
        /// <param name="textIndex">
        ///		<para>
        ///         The index to search for
        ///     </para>
        /// </param>
        /// <returns>
        ///		The word index that the text index falls on
        /// </returns>
        public int GetWordIndexFromTextIndex(int textIndex)
        {
            return netSpell.GetWordIndexFromTextIndex(textIndex);
        }

        /// <summary>
        /// Ignores all instances of the CurrentWord in the Text Property
        /// </summary>
        public void IgnoreAllWord()
        {
            netSpell.IgnoreAllWord();
        }

        /// <summary>
        /// Ignores the instances of the CurrentWord in the Text Property.
        /// </summary>
        /// <remarks>
        /// Must call SpellCheck after call this method to resume spell checking.
        /// Forces the IgnoredWord event on Speller plug-in. Host application must handle.
        /// </remarks>
        public void IgnoreWord()
        {
            netSpell.IgnoreWord();
        }

        /// <summary>
        ///     Replaces all instances of the CurrentWord in the Text Property
        /// </summary>
        public void ReplaceAllWord()
        {
            netSpell.ReplaceAllWord();
        }

        /// <summary>
        ///     Replaces all instances of the CurrentWord in the Text Property
        /// </summary>
        /// <param name="replacementWord" type="string">
        ///     <para>
        ///         The word to replace the CurrentWord with
        ///     </para>
        /// </param>
        public void ReplaceAllWord(string replacementWord)
        {
            netSpell.ReplaceAllWord(replacementWord);
        }


        /// <summary>
        ///     Replaces the instances of the CurrentWord in the Text Property
        /// </summary>
        public void ReplaceWord()
        {
            netSpell.ReplaceWord();
        }

        /// <summary>
        ///     Replaces the instances of the CurrentWord in the Text Property
        /// </summary>
        /// <param name="replacementWord" type="string">
        ///     <para>
        ///         The word to replace the CurrentWord with
        ///     </para>
        /// </param>
        public void ReplaceWord(string replacementWord)
        {
            netSpell.ReplaceWord(replacementWord);
        }

        /// <summary>
        ///     Spell checks the words in the <see cref="Text"/> property starting
        ///     at the <see cref="WordIndex"/> position.
        /// </summary>
        /// <returns>
        ///     Returns true if there is a word found in the text 
        ///     that is not in the dictionaries
        /// </returns>
        /// <seealso cref="WordIndex"/>
        public bool SpellCheck()
        {
            return netSpell.SpellCheck(netSpell._wordIndex, netSpell.WordCount - 1);
        }

        /// <summary>
        ///     Spell checks the words in the <see cref="Text"/> property starting
        ///     at the <see cref="WordIndex"/> position. This overload takes in the
        ///     WordIndex to start checking from.
        /// </summary>
        /// <param name="startWordIndex" type="int">
        ///     <para>
        ///         The index of the word to start checking from. 
        ///     </para>
        /// </param>
        /// <returns>
        ///     Returns true if there is a word found in the text 
        ///     that is not in the dictionaries
        /// </returns>
        /// <seealso cref="WordIndex"/>
        public bool SpellCheck(int startWordIndex)
        {
            return netSpell.SpellCheck(startWordIndex);
        }

        /// <summary>
        ///     Spell checks a range of words in the <see cref="Text"/> property starting
        ///     at the <see cref="WordIndex"/> position and ending at endWordIndex. 
        /// </summary>
        /// <param name="startWordIndex" type="int">
        ///     <para>
        ///         The index of the word to start checking from. 
        ///     </para>
        /// </param>
        /// <param name="endWordIndex" type="int">
        ///     <para>
        ///         The index of the word to end checking with. 
        ///     </para>
        /// </param>
        /// <returns>
        ///     Returns true if there is a word found in the text 
        ///     that is not in the dictionaries
        /// </returns>
        /// <seealso cref="WordIndex"/>
        public bool SpellCheck(int startWordIndex, int endWordIndex)
        {
            return netSpell.SpellCheck(startWordIndex, endWordIndex);

        } // SpellCheck

        /// <summary>
        ///     Spell checks the words in the <see cref="Text"/> property starting
        ///     at the <see cref="WordIndex"/> position. This overload takes in the 
        ///     text to spell check
        /// </summary>
        /// <param name="text" type="string">
        ///     <para>
        ///         The text to spell check
        ///     </para>
        /// </param>
        /// <returns>
        ///     Returns true if there is a word found in the text 
        ///     that is not in the dictionaries
        /// </returns>
        /// <seealso cref="WordIndex"/>
        public bool SpellCheck(string text)
        {
            return netSpell.SpellCheck(text);
        }

        /// <summary>
        ///     Spell checks the words in the <see cref="Text"/> property starting
        ///     at the <see cref="WordIndex"/> position. This overload takes in 
        ///     the text to check and the WordIndex to start checking from.
        /// </summary>
        /// <param name="text" type="string">
        ///     <para>
        ///         The text to spell check
        ///     </para>
        /// </param>
        /// <param name="startWordIndex" type="int">
        ///     <para>
        ///         The index of the word to start checking from
        ///     </para>
        /// </param>
        /// <returns>
        ///     Returns true if there is a word found in the text 
        ///     that is not in the dictionaries
        /// </returns>
        /// <seealso cref="WordIndex"/>
        public bool SpellCheck(string text, int startWordIndex)
        {
            return netSpell.SpellCheck(text, startWordIndex);
        }

        /// <summary>
        /// Replaces the word the caret is currently in with other text.
        /// </summary>
        /// <param name="withHtml">If set to <c>true</c> it's allowed to add HTML.</param>
        /// <param name="replaceWith">Text or HTML for replacement.</param>
        public void ReplaceWordUnderPointer(bool withHtml, string replaceWith)
        {
            WordUnderPointer(withHtml);
            if (trg != null && trg.GetText() != null)
            {
                trg.SetText(replaceWith);
            }
        }

        /// <summary>
        /// Returns the word with or without inline HTML the caret is on or nearby.
        /// </summary>
        /// <param name="withHtml">If true the method returns any embedded HTML too, otherwise HTML is stripped out.</param>
        /// <returns>The recognized HTML, or <c>null</c>, if nothing was recognized.</returns>
        public string WordUnderPointer(bool withHtml)
        {
            Interop.IMarkupServices MarkupService = ActiveDocument as Interop.IMarkupServices;
            Interop.IHTMLTxtRange tr = ((Interop.IHtmlBodyElement)ActiveDocument.GetBody()).createTextRange();
            Interop.IMarkupPointer pStart, pEnd;
            MarkupService.CreateMarkupPointer(out pStart);
            MarkupService.CreateMarkupPointer(out pEnd);
            // now reset pointer to caret 
            Interop.IHTMLCaret cr;
            DisplayService.GetCaret(out cr);
            cr.MoveMarkupPointerToCaret(pStart);
            cr.MoveMarkupPointerToCaret(pEnd);
            Interop.CARET_DIRECTION dir;
            cr.GetCaretDirection(out dir);
            MarkupService.MoveRangeToPointers(pStart, pEnd, tr);
            int result;
            Debug.WriteLine(dir, "DIR");
            while (tr.GetText() == null)
            {
                if (dir == Interop.CARET_DIRECTION.CARET_DIRECTION_BACKWARD)
                {
                    // look forward for word if caret runs backward
                    result = tr.MoveEnd("word", 1);
                    if (result == 1)
                    {
                        // in case of success expand, to get full word
                        if (tr.Expand("word")) continue;
                    }
                    // look forward only if unsuccessfully
                    // avoid trailing spaces being part of the current range
                    TrimRange(tr);
                    // if still null, try looking backward
                    if (tr.GetText() == null)
                    {
                        tr.MoveEnd("word", -1);
                        tr.MoveStart("word", -1);
                    }
                    break;
                }
                else
                {
                    // look backward for word if caret runs forward
                    result = tr.MoveStart("word", -1);
                    if (result == -1)
                    {
                        // in case of success expand, to get full word
                        if (tr.Expand("word")) continue;
                    }
                    // look forward only if unsuccessfully
                    // avoid trailing spaces being part of the current range
                    TrimRange(tr);
                    result = tr.MoveEnd("word", 1);
                    if (result == 1) continue;
                    // break if no result
                    break;
                }
            }
            TrimRange(tr);
            return tr.GetText();
        }


        # region Internal Event Handlers

        private void editor_PostEditorEvent(object sender, PostEditorEventArgs e)
        {
            Interop.IHTMLEventObj eventObj = e.EventObject;
            if (eventObj.type.StartsWith("key") && BackgroundService)
            {
                Keys key;
                // Simulate the missing KeyUp event here
                switch (eventObj.type)
                {
                    case "keydown":
                        key = (Keys)eventObj.keyCode;
                        if (key != Keys.ShiftKey && key != Keys.ControlKey)
                        {
                            key |= eventObj.ctrlKey ? Keys.Control : Keys.None; // 
                            key |= eventObj.shiftKey ? Keys.Shift : Keys.None; // 
                            key |= eventObj.altKey ? Keys.Alt : Keys.None; // 
                        }
                        KeyEventArgs kdargs = new KeyEventArgs(key);
                        editor_KeyDown(sender, kdargs);
                        break;
                    case "keypress":
                        int keycode = (eventObj.keyCode >= 97 && eventObj.keyCode <= 122) ? eventObj.keyCode - 32 : eventObj.keyCode;
                        key = (Keys)keycode;
                        if (key != Keys.ShiftKey && key != Keys.ControlKey)
                        {
                            key |= eventObj.ctrlKey ? Keys.Control : Keys.None; // 
                            key |= eventObj.shiftKey ? Keys.Shift : Keys.None; // 
                            key |= eventObj.altKey ? Keys.Alt : Keys.None; // 
                        }
                        KeyEventArgs keargs = new KeyEventArgs(key);
                        editor_KeyUp(sender, keargs);
                        break;
                }
            }
        }

        private void editor_MouseDown(object sender, MouseEventArgs e)
        {
            if (BackgroundService && e.Button != MouseButtons.Right)
            {
                GetCurrentWord(true, false);
            }
        }

        private void editor_ShowContextMenu(object sender, ShowContextMenuEventArgs e)
        {
            GetCurrentWord();
            Point p = e.Location;
            if (e.Location.X == 0 && e.Location.Y == 0)
            {
                // assume that the context call was forced by special windows key
                p = editor.Window.GetCaretLocation();
            }
            InvokeWordOnContext(new WordOnContextEventArgs(currentWordToCheck, currentHtmlToCheck, p));
        }

        private void editor_MouseUp(object sender, MouseEventArgs e)
        {
            if (BackgroundService && e.Button != MouseButtons.Right) // no context menu to invoke
            {
                CheckWord();
            }
        }

        private void editor_KeyDown(object sender, KeyEventArgs e)
        {
            //GetCurrentWord();
            string check;
            bool hasNewLine;
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    GetCurrentWord(true, false);
                    CheckWord();
                    break;
                case Keys.Left:
                    // check on left step if character or right from caret was whitespace or punctuation
                    check = CharacterOnCaret(false, out hasNewLine);
                    if (check != null && (!Char.IsLetterOrDigit(check[0]) || hasNewLine))
                    {
                        GetCurrentWord(false, hasNewLine);
                        CheckWord();
                    }
                    break;

                case Keys.Right:
                    // check on left step if character or right from caret was whitespace or punctuation
                    check = CharacterOnCaret(true, out hasNewLine);

                    if (check != null && check.Length > 1 && (!Char.IsLetterOrDigit(check[check.Length - 1])))
                    {
                        GetCurrentWord(true, hasNewLine);
                        CheckWord();
                    }
                    break;
                case Keys.Up:
                case Keys.Down:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Home:
                case Keys.End:

                    if (currentWordToCheck == null)
                    {
                        GetCurrentWord();
                    }
                    CheckWord();
                    break;

                default:
                    break;
            }
        }

        private void editor_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                case Keys.Enter:
                    GetCurrentWord(false, false);
                    CheckWord();
                    break;

                case Keys.Control:
                case Keys.ControlKey:
                case Keys.Alt:
                case Keys.Shift:
                case Keys.ShiftKey:
                    break;

                default:
                    // any punctuation
                    if (!Char.IsLetterOrDigit((char)e.KeyValue))
                    {
                        GetCurrentWord(true, false);
                        if (refireCheck)
                        {
                            refireCheck = false;
                            trg.MoveStart("word", -2);
                            string text = trg.GetText();
                            if (text != null && text[text.Length - 1] == (char)e.KeyValue)
                            {
                                trg.MoveEnd("word", -1);
                            }
                            else
                            {
                                trg.MoveEnd("word", -2);
                            }
                            if (trg.GetText() != null)
                            {
                                wordExists = true;
                                currentWordToCheck = trg.GetText();
                                currentHtmlToCheck = trg.GetHtmlText();
                            }
                        }
                        CheckWord();
                    }
                    break;

            }
            if (e.KeyCode == Keys.RButton)
            {
                GetCurrentWord();
                if (WordOnContext != null && DisplayService != null)
                {
                    Interop.IHTMLCaret cr;
                    DisplayService.GetCaret(out cr);
                    Interop.POINT pPoint = new Interop.POINT();
                    cr.GetLocation(ref pPoint, true);
                    WordOnContext(this, new WordOnContextEventArgs(currentWordToCheck, currentHtmlToCheck, new Point(pPoint.x, pPoint.y)));
                }
            }
        }

        # endregion

        private string CharacterOnCaret(bool rightFromCaret, out bool hasNewLine)
        {
            Interop.IMarkupServices MarkupService = ActiveDocument as Interop.IMarkupServices;
            Interop.IHTMLTxtRange tr = ((Interop.IHtmlBodyElement)ActiveDocument.GetBody()).createTextRange();
            Interop.IMarkupPointer pStart, pEnd;
            MarkupService.CreateMarkupPointer(out pStart);
            MarkupService.CreateMarkupPointer(out pEnd);
            // now reset pointer to caret 
            Interop.IHTMLCaret cr;
            DisplayService.GetCaret(out cr);
            cr.MoveMarkupPointerToCaret(pStart);
            cr.MoveMarkupPointerToCaret(pEnd);
            string result;
            MarkupService.MoveRangeToPointers(pStart, pEnd, tr);
            if (rightFromCaret)
            {
                // called on Cursor Right
                tr.MoveEnd("character", 1);
                // check if a Newline precedes
                tr.MoveStart("character", -1);
                result = tr.GetText();
            }
            else
            {
                // Called on Cursor Left
                tr.MoveStart("character", -1);
                // check if a Newline follows
                tr.MoveEnd("character", 2);
                result = tr.GetText();
            }
            // force check on parts we cannot recognize immediately
            hasNewLine = (result == null) ? true : (result.IndexOf('\r') != -1);
            return result;
        }

        /// <summary>
        /// Gets or sets a value that activates the internal NetSpell spell checker.
        /// </summary>
        public bool CheckInternal
        {
            get { return checkInternal; }
            set { checkInternal = value; }
        }

        # region netSpell Properties

        /// <summary>
        ///     The suggestion strategy to use when generating suggestions
        /// </summary>
        public SuggestionEnum SuggestionMode
        {
            set { netSpell.SuggestionMode = value;  }
            get { return netSpell.SuggestionMode; }
        }

        /// <summary>
        ///     An array of word suggestions for the correct spelling of the misspelled word
        /// </summary>
        /// <seealso cref="Suggest()"/>
        /// <seealso cref="SpellCheck()"/>
        public List<string> Suggestions
        {
            get { return netSpell.Suggestions; }
        }

        /// <summary>
        ///     The text to spell check
        /// </summary>
        public string Text
        {
            get { return netSpell.Text; }
        }

        /// <summary>
        ///     TextIndex is the index of the current text being spell checked
        /// </summary>
        public int TextIndex
        {
            get
            {
                return netSpell.TextIndex;
            }
        }

        /// <summary>
        ///     The number of words being spell checked
        /// </summary>
        public int WordCount
        {
            get
            {
                return netSpell.WordCount;
            }
        }

        /// <summary>
        ///     WordIndex is the index of the current word being spell checked
        /// </summary>
        public int WordIndex
        {
            get
            {
                return netSpell.WordIndex;
            }
        }

        /// <summary>
        ///     The WordDictionary object to use when spell checking.
        /// </summary>
        public WordDictionary Dictionary
        {
            get
            {
                return netSpell.Dictionary;
            }
        }

        # endregion

        /// <summary>
        /// Access to the underyling NetSpell instance. For internal use only.
        /// </summary>
        internal Spelling Spelling { get { return netSpell; } }

        /// <summary>
        /// Overwritten for PropertyGrid Designtime support.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Click + to see options";
        }


    }
}
