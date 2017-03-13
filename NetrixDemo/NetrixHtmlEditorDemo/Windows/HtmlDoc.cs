using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using GuruComponents.Netrix;
using GuruComponents.Netrix.HelpLine;
using GuruComponents.Netrix.TableDesigner;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.SpellChecker;
using GuruComponents.EditorDemo.Dialogs;
using GuruComponents.Netrix.WebEditing.UndoRedo;
using System.Threading;
using GuruComponents.Netrix.HtmlFormatting;

namespace NetrixHtmlEditorDemo.Windows
{
    public partial class HtmlDoc : DockContent
    {
        public HtmlDoc()
        {
            InitializeComponent();
            this.IsMdiContainer = false;

            // Create and attach Plug-Ins
            this.tableDesigner1 = new GuruComponents.Netrix.TableDesigner.TableDesigner(this.components);
            this.speller1 = new GuruComponents.Netrix.SpellChecker.Speller(this.components);

            // create the netspell instance, connect to property bag and register with one editor instance
            IDocumentSpellChecker doc = this.speller1.GetSpellChecker(htmlEditor1);
            SpellCheckerProperties spellProps = new SpellCheckerProperties(doc);
            this.speller1.SetSpeller(htmlEditor1, spellProps);

            this.helpLine1 = new GuruComponents.Netrix.HelpLine.HelpLine(this.components);
            this.helpLine1.SetHelpLine(htmlEditor1, new HelpLineProperties());
            this.helpLine2 = new GuruComponents.Netrix.HelpLine.HelpLine(this.components);
            this.helpLine2.SetHelpLine(htmlEditor1, new HelpLineProperties());

            // Here we register the plug-in as an extender with the control
            this.tableDesigner1.SetTableDesigner(htmlEditor1, new TableDesignerProperties());
            // Setting properties requires retrieving the propertybag using the editor as a key. That way one plug-in can support multiple editor instances.
            // commands defined by the plug-in can be send through the Invoke method
            this.tableDesigner1.GetTableDesigner(htmlEditor1).Active = true;

            // The spellchecker has two bags, 'GetSpellChecker' provides direct access to commands. Invoke is just an alternative way.
            doc.BackgroundService = false;
            // 'GetSpeller' retrieves the regular property bag
            this.speller1.GetSpeller(htmlEditor1).IgnoreHtml = true;

            // 'GetHelpLine'  retrieves the regular property bag
            this.helpLine2.GetHelpLine(htmlEditor1).LineVisible = false;
            this.helpLine2.GetHelpLine(htmlEditor1).LineColor = Color.Blue;
            this.helpLine2.GetHelpLine(htmlEditor1).X = 100;
            this.helpLine2.GetHelpLine(htmlEditor1).Y = 100;

            this.helpLine1.GetHelpLine(htmlEditor1).LineVisible = false;
            this.helpLine1.GetHelpLine(htmlEditor1).LineColor = Color.Red;
            this.helpLine1.GetHelpLine(htmlEditor1).X = 30;
            this.helpLine1.GetHelpLine(htmlEditor1).Y = 30;
            // some plug-ins support events
            helpLine1.HelpLineMoving += new GuruComponents.Netrix.HelpLine.Events.HelpLineMoving(helpLine1_HelpLineMoving);
            helpLine1.HelpLineMoved += new GuruComponents.Netrix.HelpLine.Events.HelpLineMoved(helpLine1_HelpLineMoved);

            tableDesigner1.TableBecomesActive += new TableEventBecomesActiveHandler(tableDesigner1_TableBecomesActive);
            tableDesigner1.TableBecomesInactive += new TableEventBecomesInactiveHandler(tableDesigner1_TableBecomesInactive);

            speller1.WordChecker += new GuruComponents.Netrix.SpellChecker.WordCheckerHandler(speller1_WordChecker);
            speller1.WordOnContext += new GuruComponents.Netrix.SpellChecker.WordOnContextHandler(speller1_WordOnContext);
            speller1.MisspelledWord += new GuruComponents.Netrix.SpellChecker.WordEventHandler(speller1_MisspelledWord);
            speller1.EndOfText += new GuruComponents.Netrix.SpellChecker.WordEventHandler(speller1_EndOfText);
            speller1.DoubledWord += new GuruComponents.Netrix.SpellChecker.WordEventHandler(speller1_DoubledWord);

            // some plug-ins have methods that act as shortcuts 
            tableDesigner1.SetActive(htmlEditor1, true);
            // other controls are just regular user controls
            codeEditorControl1.Document.SyntaxFile = @"Resources\Syntaxfile\HTML.syn";

        }


        void tableDesigner1_TableBecomesInactive(TableEventArgs e)
        {
            if (TableInActive != null)
            {
                TableInActive(tableDesigner1, e);
            }
        }

        void tableDesigner1_TableBecomesActive(TableEventArgs e)
        {
            if (TableActive != null)
            {
                TableActive(tableDesigner1, e);
            }
        }

        void helpLine1_HelpLineMoved(object sender, GuruComponents.Netrix.HelpLine.Events.HelplineMovedEventArgs e)
        {
            if (HelpLineAMoved != null)
            {
                HelpLineAMoved(sender, e);
            }
        }

        void helpLine1_HelpLineMoving(object sender, GuruComponents.Netrix.HelpLine.Events.HelplineMovedEventArgs e)
        {
            if (HelpLineAMoving != null)
            {
                HelpLineAMoving(sender, e);
            }
        }

        public event EventHandler<GuruComponents.Netrix.HelpLine.Events.HelplineMovedEventArgs> HelpLineAMoving;
        public event EventHandler<GuruComponents.Netrix.HelpLine.Events.HelplineMovedEventArgs> HelpLineAMoved;

        public event EventHandler<TableEventArgs> TableActive;
        public event EventHandler<TableEventArgs> TableInActive;

        private string m_fileName = string.Empty;

        public string FileName
        {
            get { return m_fileName; }
            set
            {
                if (value != string.Empty)
                {
                    m_fileName = value;
                    htmlEditor1.LoadFile(m_fileName);
                }
                this.ToolTipText = value;
                this.toolStripStatusLabelFilename.Text = Path.GetFileName(value);
            }
        }

        protected override string GetPersistString()
        {
            // Add extra information into the persist string for this document
            // so that it is available when deserialized.
            return GetType().ToString() + "," + FileName + "," + Text;
        }

        private void htmlEditor1_ReadyStateComplete(object sender, EventArgs e)
        {
            statusStrip1.Enabled = true;
            OnDocumentReady();
        }

        # region Access to Editor and PlugIns

        /// <summary>
        /// Expose an instance of the editor to let other windows directly work with the current document
        /// </summary>
        public IHtmlEditor HtmlEditor
        {
            get
            {
                return htmlEditor1;
            }
        }

        /// <summary>
        /// Expose Helpline 1 (with grid and snap features)
        /// </summary>
        public HelpLineProperties HelpLineA
        {
            get
            {
                return helpLine1.GetHelpLine(htmlEditor1);
            }
        }



        /// <summary>
        /// Expose Helpline 2 (plain)
        /// </summary>
        public HelpLineProperties HelpLineB
        {
            get
            {
                return helpLine2.GetHelpLine(htmlEditor1);
            }
        }

        public TableDesigner TableDesigner
        {
            get
            {
                return tableDesigner1;
            }
        }

        public GuruComponents.Netrix.SpellChecker.Speller SpellChecker
        {
            get
            {
                return speller1;
            }
        }

        # endregion

        # region Expose Events to communicate with other instances

        public event EventHandler DocumentReady;

        protected void OnDocumentReady()
        {
            RegisterEvent(EventWindow.EventType.Control, "ReadyStateComplete", htmlEditor1, EventArgs.Empty);
            if (DocumentReady != null)
            {
                DocumentReady(this.htmlEditor1, EventArgs.Empty);
            }
            // Register Events that require a loaded document
            htmlEditor1.Selection.SelectionChanged += new GuruComponents.Netrix.Events.SelectionChangedEventHandler(Selection_SelectionChanged);
            htmlEditor1.DocumentStructure.ControlSelect += new GuruComponents.Netrix.Events.DocumentEventHandler(DocumentStructure_ControlSelect);
            htmlEditor1.DocumentStructure.DblClick += new GuruComponents.Netrix.Events.DocumentEventHandler(DocumentStructure_DblClick);
            htmlEditor1.DocumentStructure.Help += new GuruComponents.Netrix.Events.DocumentEventHandler(DocumentStructure_Help);
            htmlEditor1.DocumentStructure.SelectStart += new GuruComponents.Netrix.Events.DocumentEventHandler(DocumentStructure_SelectStart);
            htmlEditor1.DocumentStructure.SelectionChange += new GuruComponents.Netrix.Events.DocumentEventHandler(DocumentStructure_SelectionChange);
            htmlEditor1.Window.Resize += new GuruComponents.Netrix.Events.DocumentEventHandler(Window_Resize);
            htmlEditor1.Window.ScriptError += new GuruComponents.Netrix.Events.ShowErrorHandler(Window_ScriptError);
            htmlEditor1.Window.ScriptMessage += new GuruComponents.Netrix.Events.ShowMessageHandler(Window_ScriptMessage);
            htmlEditor1.Window.Scroll += new GuruComponents.Netrix.Events.DocumentEventHandler(Window_Scroll);
        }

        void Window_Scroll(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Window, "Scroll", sender, e);
        }

        void Window_ScriptMessage(object sender, GuruComponents.Netrix.Events.ShowMessageEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Window, "ScriptMessage", sender, e);
        }

        void Window_ScriptError(object sender, GuruComponents.Netrix.Events.ShowErrorEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Window, "ScriptError", sender, e);
        }

        void Window_Resize(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Window, "Resize", sender, e);
        }

        void DocumentStructure_SelectionChange(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "SelectionChange", sender, e);
        }

        void DocumentStructure_SelectStart(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "SelectStart", sender, e);
        }

        void DocumentStructure_Help(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "Help", sender, e);
        }

        void DocumentStructure_DblClick(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "DblClick", sender, e);
        }

        void DocumentStructure_ControlSelect(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "ControlSelect", sender, e);
        }

        void Selection_SelectionChanged(object sender, GuruComponents.Netrix.Events.SelectionChangedEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "SelectionChanged", sender, e);
        }

        public event EventHandler UIUpdated;

        protected void OnUIUpdated()
        {
            if (UIUpdated != null)
            {
                UIUpdated(this.htmlEditor1, EventArgs.Empty);
            }
        }
        # endregion


        EventWindow eventWindow;

        // Use this to expose all events to a windows so the user can see what happens internally
        public void RegisterEventWindow(EventWindow eventWindow)
        {
            this.eventWindow = eventWindow;
        }

        private void RegisterEvent(EventWindow.EventType eventType, string name, object sender, EventArgs e)
        {
            if (this.eventWindow != null)
            {
                this.eventWindow.RegisterEvent(eventType, name, sender, e);
            }
        }

        private void htmlEditor1_UpdateUI(object sender, GuruComponents.Netrix.Events.UpdateUIEventArgs e)
        {
            OnUIUpdated();
            RegisterEvent(EventWindow.EventType.Control, "UpdateUI", sender, e);
        }

        private void htmlEditor1_DragDrop(object sender, DragEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Mouse, "DragDrop", sender, e);
        }

        private void htmlEditor1_DragEnter(object sender, DragEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Mouse, "DragEnter", sender, e);
        }

        private void htmlEditor1_DragOver(object sender, DragEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Mouse, "DragOver", sender, e);
        }

        private void htmlEditor1_HtmlElementChanged(object sender, GuruComponents.Netrix.Events.HtmlElementChangedEventArgs e)
        {
            BuildElementChain(e.CurrentElement);
            RegisterEvent(EventWindow.EventType.Element, "HtmlElementChanged", sender, e);
        }

        private void BuildElementChain(IElement el)
        {
            List<IElement> hierarchy = new List<IElement>();
            IElement current = el;
            while (current != null && current.TagName.ToUpper() != "BODY")
            {
                hierarchy.Add(current);
                current = current.GetParent() as IElement;
            }
            hierarchy.Add(htmlEditor1.GetBodyElement());
            hierarchy.Reverse();
            for (int i = statusStrip1.Items.Count - 1; i >= 0; i--)
            {
                ToolStripItem item = statusStrip1.Items[i];
                if (item is ToolStripLabel)
                {
                    if (((ToolStripLabel)item).Tag != null && ((ToolStripLabel)item).Tag is IElement)
                    {
                        statusStrip1.Items.RemoveAt(i);
                    }
                }
            }
            ToolStripLabel last = null;
            foreach (IElement element in hierarchy)
            {
                ToolStripLabel l = new ToolStripLabel();
                last = l;
                l.Click += new EventHandler(l_Click);
                l.Text += String.Format("<{0}>", element.TagName);
                l.LinkBehavior = LinkBehavior.HoverUnderline;
                l.IsLink = true;
                l.Tag = element;
                l.ToolTipText = element.OuterHtml;
                statusStrip1.Items.Add(l);
            }
            if (last != null)
            {
                last.ForeColor = Color.Red;
            }
        }

        void l_Click(object sender, EventArgs e)
        {
            IElement elementClicked = ((ToolStripLabel)sender).Tag as IElement;
            if (elementClicked != null)
            {
                if (elementClicked.IsSelectable())
                    htmlEditor1.Selection.SelectElement(elementClicked);
                else
                    htmlEditor1.TextSelector.SelectElementText(elementClicked);
            }
        }

        private void htmlEditor1_HtmlKeyOperation(object sender, GuruComponents.Netrix.Events.HtmlKeyEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Key, "HtmlKeyOperation", sender, e);
        }

        private void htmlEditor1_HtmlMouseOperation(object sender, GuruComponents.Netrix.Events.HtmlMouseEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Mouse, "HtmlMouseOperation", sender, e);
        }

        private void htmlEditor1_NextOperationAdded(object sender, UndoEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "NextOperationAdded", sender, e);
        }

        private void htmlEditor1_ReadyStateChanged(object sender, GuruComponents.Netrix.Events.ReadyStateChangedEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "ReadyStateChanged", sender, e);
        }

        private void htmlEditor1_ElementCreated(object sender, EventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "ElementCreated", sender, e);
        }

        private void htmlEditor1_FindHasReachedEnd(object sender, EventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "FindHasReachedEnd", sender, e);
        }

        private void htmlEditor1_ContentModified(object sender, GuruComponents.Netrix.Events.ContentModifiedEventArgs e)
        {
            // put an asterisk at the end to indicate content changes
            this.Text += (!this.Text.EndsWith(" *") ? " *" : "");
            RegisterEvent(EventWindow.EventType.Control, "ContentModified", sender, e);
        }

        private void htmlEditor1_ContentChanged(object sender, EventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "ContentChanged", sender, e);
        }

        private void htmlEditor1_BeforeSnapRect(object sender, GuruComponents.Netrix.Events.BeforeSnapRectEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Element, "BeforeSnapRect", sender, e);
        }

        private void htmlEditor1_BeforeShortcut(object sender, GuruComponents.Netrix.Events.BeforeShortcutEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Key, "BeforeShortcut", sender, e);
        }

        private void htmlEditor1_BeforeReplace(object sender, GuruComponents.Netrix.Events.BeforeReplaceEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "HtmlBeforeReplaceElementChanged", sender, e);
        }

        private void htmlEditor1_BeforeResourceLoad(object sender, GuruComponents.Netrix.Events.BeforeResourceLoadEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "BeforeResourceLoad", sender, e);
        }

        private void htmlEditor1_BeforeNavigate(object sender, GuruComponents.Netrix.Events.BeforeNavigateEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "BeforeNavigate", sender, e);
        }

        private void htmlEditor1_AfterSnapRect(object sender, GuruComponents.Netrix.Events.AfterSnapRectEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "AfterSnapRect", sender, e);
        }

        private void htmlEditor1_Loading(object sender, GuruComponents.Netrix.Events.LoadEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "Loading", sender, e);
        }

        private void htmlEditor1_Loaded(object sender, GuruComponents.Netrix.Events.LoadEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "Loaded", sender, e);
        }

        private void htmlEditor1_Saving(object sender, GuruComponents.Netrix.Events.SaveEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "Saving", sender, e);
        }

        private void htmlEditor1_Saved(object sender, GuruComponents.Netrix.Events.SaveEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "Saved", sender, e);
        }

        private void htmlEditor1_WebException(object sender, GuruComponents.Netrix.Events.WebExceptionEventArgs e)
        {
            RegisterEvent(EventWindow.EventType.Control, "WebException", sender, e);
        }

        # region Common edit function that apply to both editors

        public void Redo()
        {
            switch (tabControlDoc.SelectedIndex)
            {
                case 0:
                    htmlEditor1.Redo();
                    break;
                case 1:
                    codeEditorControl1.Redo();
                    break;
            }
        }

        public void Undo()
        {
            switch (tabControlDoc.SelectedIndex)
            {
                case 0:
                    htmlEditor1.Undo();
                    break;
                case 1:
                    codeEditorControl1.Undo();
                    break;
            }
        }

        public void Copy()
        {
            switch (tabControlDoc.SelectedIndex)
            {
                case 0:
                    htmlEditor1.Copy();
                    break;
                case 1:
                    codeEditorControl1.Copy();
                    break;
            }
        }

        public void Cut()
        {
            switch (tabControlDoc.SelectedIndex)
            {
                case 0:
                    htmlEditor1.Cut();
                    break;
                case 1:
                    codeEditorControl1.Cut();
                    break;
            }
        }

        public void Paste()
        {
            switch (tabControlDoc.SelectedIndex)
            {
                case 0:
                    htmlEditor1.Paste();
                    break;
                case 1:
                    codeEditorControl1.Paste();
                    break;
            }
        }

        # endregion

        # region TAB control

        public void ShowPane(int paneIndex)
        {
            if (paneIndex < 0 || paneIndex > 1)
                throw new ArgumentOutOfRangeException();
            tabControlDoc.SelectedIndex = paneIndex;
        }

        private void tabControlDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControlDoc.SelectedIndex)
            {
                case 0:
                    htmlEditor1.LoadHtml(codeEditorControl1.Document.Text);
                    if (HtmlEditorPaneActive != null)
                    {
                        HtmlEditorPaneActive(this, EventArgs.Empty);
                    }
                    break;
                case 1:
                    LoadCodeEditorWithOptions();
                    if (CodeEditorPaneActive != null)
                    {
                        CodeEditorPaneActive(this.codeEditorControl1, EventArgs.Empty);
                    } 
            break;
            }
        }

        public event EventHandler HtmlEditorPaneActive;
        public event EventHandler CodeEditorPaneActive;

        # endregion

        # region Formatting

        IHtmlFormatterOptions fo = new HtmlFormatterOptions() { PreserveWhitespace = true };

        private void LoadCodeEditorWithOptions()
        {
            fo.PreserveWhitespace = checkBoxPreserve.Checked;
            fo.MakeXhtml = checkBoxXhtml.Checked;
            fo.IndentSize = (int)numericUpDownIndent.Value;
            fo.MaxLineLength = (int)numericUpDownBreak.Value;
            codeEditorControl1.Document.Text = htmlEditor1.GetFormattedHtml(fo);
        }

        private void buttonApplyFO_Click(object sender, EventArgs e)
        {
            LoadCodeEditorWithOptions();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "These options set the properties for the HtmlFormatterOptions object. An instance is used to determine the behavior of the formatter plug-in. Use the 'Advanced' button to find all available properties.", "Help", MessageBoxButtons.OK);
        }

        private void buttonFOAdvanced_Click(object sender, EventArgs e)
        {
            FormatterOptions formatterDialog = new FormatterOptions();
            formatterDialog.OptionsObject = fo;
            formatterDialog.ShowDialog();
            fo = formatterDialog.OptionsObject;
        }

        # endregion

        # region Spellchecker and Context Menu Support

        // Dialog for word by word checking
        private Spellchecker SpellCheckerDialog;
        private bool block = true;
        
        // Get an instance once required
        private Spellchecker GetSpellCheckerDialog()
        {
            if (SpellCheckerDialog == null)
            {
                SpellCheckerDialog = new Spellchecker();
                SpellCheckerDialog.TopMost = true;
                SpellCheckerDialog.Action += new EventHandler((o, ea) => block = false);
                SpellCheckerDialog.Show();
            }
            else
            {
                if (!SpellCheckerDialog.Visible)
                {
                    SpellCheckerDialog.Show();
                }
            }
            return SpellCheckerDialog;
        }

        void speller1_DoubledWord(object sender, GuruComponents.Netrix.SpellChecker.NetSpell.SpellingEventArgs e)
        {
            // Add Handling heres
        }

        void speller1_EndOfText(object sender, GuruComponents.Netrix.SpellChecker.NetSpell.SpellingEventArgs e)
        {
            MessageBox.Show("Reached end of text.", "Spellchecker", MessageBoxButtons.OK);
        }

        void speller1_MisspelledWord(object sender, GuruComponents.Netrix.SpellChecker.NetSpell.SpellingEventArgs e)
        {
            GetSpellCheckerDialog().WrongWord = e.Word;
        }

        void speller1_WordOnContext(object sender, GuruComponents.Netrix.SpellChecker.WordOnContextEventArgs e)
        {
            contextMenuTabPage.Items.Clear();
            // Add common operations
            // Add Spellchecker suggestions
            if (speller1.GetSpellChecker(htmlEditor1).BackgroundService)
            {
                speller1.GetSpellChecker(htmlEditor1).Suggest(e.Word);
                List<string> suggestions = speller1.GetSpellChecker(htmlEditor1).Suggestions;
                if (suggestions != null && suggestions.Count > 0)
                {
                    foreach (string suggestion in suggestions)
                    {
                        // must not change "Text" property as it is used in the click event handler
                        contextMenuTabPage.Items.Add(suggestion, null, new EventHandler(ReplacementOnClick));
                    }
                }
                else
                {
                    ToolStripItem noSuggestion = contextMenuTabPage.Items.Add("No Suggestions");
                    noSuggestion.Enabled = false;
                }
            }
            contextMenuTabPage.Show(htmlEditor1, e.Position);
        }

        void ReplacementOnClick(object sender, EventArgs ea)
        {
            if (sender is ToolStripItem)
            {
                string suggestion = ((ToolStripItem)sender).Text;
                speller1.GetSpellChecker(htmlEditor1).ReplaceWord(suggestion);
            }
        }

        bool speller1_WordChecker(object sender, GuruComponents.Netrix.SpellChecker.WordEventArgs e)
        {
            // we have to handle this because this demo shows all modes
            if (speller1.GetSpellChecker(htmlEditor1).BackgroundService) return false;
            // handle dialog
            GetSpellCheckerDialog().WrongWord = e.Word;
            speller1.GetSpellChecker(htmlEditor1).Suggest(e.Word);
            List<string> suggestions = speller1.GetSpellChecker(htmlEditor1).Suggestions;
            GetSpellCheckerDialog().Suggestions = suggestions;
            while (block)
            {
                Application.DoEvents();
                Thread.Sleep(50);
            }
            switch (GetSpellCheckerDialog().DialogResult)
            {
                case System.Windows.Forms.DialogResult.OK:
                    e.ReplacementWord = GetSpellCheckerDialog().NewWord;
                    block = true;
                    return true;
                case System.Windows.Forms.DialogResult.Cancel:
                    htmlEditor1.InvokeCommand(speller1.Commands.StopWordByWord);
                    block = true;
                    break;
                default:
                    block = true;
                    break;
            }
            return false;
        }

        private void contextMenuTabPage_Opening(object sender, CancelEventArgs e)
        {
            if (speller1.GetSpellChecker(htmlEditor1).BackgroundService)
            {
                // in case of spellchecker use the spellcheckers context menu event, because it is fired at a different 
                // point to retrieve the current word correctly
                //e.Cancel = true;
            }
            else
            {
                contextMenuTabPage.Items.Clear();
                // Add common operations
                contextMenuTabPage.Items.Add("Undo", imageListContext.Images[0], (o, ea) => Undo());
                contextMenuTabPage.Items.Add("Copy", imageListContext.Images[1], (o, ea) => Copy());
                contextMenuTabPage.Items.Add("Paste", imageListContext.Images[2], (o, ea) => Paste());
                contextMenuTabPage.Items.Add("Cut", imageListContext.Images[3], (o, ea) => Cut());
            }
        }

        # endregion


    }
}