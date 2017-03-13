using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using GuruComponents.Netrix.Designer;
//using GuruComponents.Netrix.Licensing;
using GuruComponents.Netrix.PlugIns;
using GuruComponents.Netrix.SpellChecker.NetSpell;
using GuruComponents.Netrix.WebEditing.Elements;
using System.IO;

namespace GuruComponents.Netrix.SpellChecker
{
	/// <summary>
	/// The Spellchecker ExtenderProvider component.
	/// </summary>
    /// <remarks>
    /// To use this plug-in simply add the component to the toolbox and drag drop to the current window.
    /// It will extend all available editor instances there. This component is based on the common NetSpell classes
    /// and the OpenOffice based dictionaries. Dictionaries are available as files so they can simply replaced by
    /// newer or specialized versions.
    /// <para>
    /// Several methods need the current HtmlEditor instance. This is necessary because the Speller PlugIn exists only once and
    /// 
    /// </para>
    /// <para>
    /// The spellchecker supports both "as-you-type" (background checking) and interactive spell checking. Both methods
    /// are event based and very fast.
    /// </para>
    /// <example>
    /// First the recommended way to implement background spellchecking. Assume <c>backgroundOn</c> is set by some sort of
    /// command or menu item click:
    /// <code> 
    ///if (backgroundOn)
    ///{
    ///    htmlEditor1.InvokeCommand(speller1.Commands.StartBackground);
    ///}
    ///else
    ///{
    ///    htmlEditor1.InvokeCommand(speller1.Commands.StopBackground);
    ///    htmlEditor1.InvokeCommand(speller1.Commands.RemoveHighLight);
    ///}
    /// </code>
    /// You can see that we invoke commands against an extender by calling the editor instance. This way we can deal with
    /// many different instances, even if only one extender exists.
    /// <para>
    /// To change the current dictionary and start interactive spelling the following code gives you a starter:
    /// </para>
    /// <code>
    /// switch (dicName)
    ///{
    ///    case "Deutsch":
    ///        speller1.Speller.GetSpellChecker(htmlEditor1).LanguageType = new CultureInfo("de-DE");
    ///        break;
    ///    case "English (US)":
    ///        speller1.Speller.GetSpellChecker(htmlEditor1).LanguageType = new CultureInfo("en-US");
    ///        break;
    ///    case "English (UK)":
    ///        speller1.Speller.GetSpellChecker(htmlEditor1).LanguageType = new CultureInfo("en-UK")
    ///        break;
    ///    case "Spanish":
    ///        speller1.Speller.GetSpellChecker(htmlEditor1).LanguageType = new CultureInfo("es-ES");
    ///        break;
    ///    case "French":
    ///        speller1.Speller.GetSpellChecker(htmlEditor1).LanguageType = new CultureInfo("fr-FR");
    ///        break;
    ///    case "Italian":
    ///        speller1.Speller.GetSpellChecker(htmlEditor1).LanguageType = new CultureInfo("it-IT");
    ///        break;
    ///}
    ///htmlEditor1.InvokeCommand(speller1.Commands.StartWordByWord);
    /// </code>
    /// <para>
    /// However, all these examples will not work properly without attaching the appropriate events. First, operating with a context menu:
    /// </para>
    /// <code>
    /// this.speller1.WordOnContext += new GuruComponents.Netrix.PlugIns.SpellChecker.WordOnContextHandler(this.speller1_WordOnContext);
    /// </code>
    /// It's necessary to use this event because it appears at a different time than the usual OnContextMenu event. This is necessary to first place
    /// the caret on the word and open the menu, then. This makes it possible to get suggestions for the current word.
    /// <code>
    /// private void speller1_WordOnContext(object sender, GuruComponents.Netrix.PlugIns.SpellChecker.WordOnContextEventArgs e)
    /// {
    ///     contextMenu1.MenuItems.Clear();
    ///     contextMenu1.MenuItems.Add(e.Word, new EventHandler(this.contextMenu_Word));
    /// }
    /// </code>
    /// Here we simply add the word to the context menu, but the <c>e.Suggestions</c> property will provide suggestions, if any.
    /// <para>
    /// </para>
    /// Various other events helps building sophisticated spell checker enabled applications.
    /// </example>
    /// <para>
    /// The spellchecker will operate with defaults here, that means wrong spelled words have red waved lines.
    /// </para>
    /// <para>
    /// Recommended readings regarding NetSpell are:
    /// <list type="bullet">
    ///     <item>How to create a custom dictionary: http://www.loresoft.com/Applications/NetSpell/Articles/246.aspx </item>
    ///     <item>Overview NetSpell: http://www.loresoft.com/Applications/NetSpell/Articles/152.aspx </item>
    /// </list>
    /// </para>
    /// </remarks>
    [ToolboxBitmap(typeof(Speller), "Resources.Toolbox.ico")]
    [ProvideProperty("Speller", typeof(GuruComponents.Netrix.IHtmlEditor))]
    [DesignerAttribute(typeof(SpellControlDesigner))]
    public class Speller : Component, IExtenderProvider, IPlugIn
    {
        private Hashtable properties;
        private Hashtable checker;
        /// <summary>
        /// Indicates that a spell check operation is currently in progress.
        /// </summary>
        public bool IsInCheck { get; private set; }
        /// <summary>
        /// Indicates that the spell checker runs in background ('as you type') mode.
        /// </summary>
        public bool IsInBackground { get; private set; }
        private System.Windows.Forms.ToolStrip toolStripSpeller;
        private System.Windows.Forms.ToolStripButton toolStripButtonSpellerSpell;
        private System.Windows.Forms.MenuStrip menuStripSpeller;
        private System.Windows.Forms.ToolStripMenuItem toolStripParentSpeller;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSpellerChecker;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSpellerAsYouType;

        /// <summary>
        /// Default Constructor supports design time behavior.
        /// </summary>
        public Speller()
        {
            InitializeComponent();
            properties = new Hashtable();            
            checker = new Hashtable();            
        }
        
        /// <summary>
        /// Constructor supports design time behavior.
        /// </summary>
        /// <param name="parent">Forms container the extender belongs to.</param>
        public Speller(IContainer parent) : this()
        {
            parent.Add(this);
        }

        private SpellCheckerProperties EnsurePropertiesExists(IHtmlEditor key)
        {
            SpellCheckerProperties p = (SpellCheckerProperties) properties[key];
            if (p == null)
            {
                p = new SpellCheckerProperties(EnsureSpellCheckerExists(key));
                properties[key] = p;
            }
            return p;
        }

        private IDocumentSpellChecker EnsureSpellCheckerExists(IHtmlEditor key)
        {
            DocumentSpellChecker p = (DocumentSpellChecker)checker[key];
            if (p == null)
            {
                p = new DocumentSpellChecker(key);
                p.WordChecker += new WordCheckerHandler(p_WordChecker);
                p.WordCheckerFinished += new WordCheckerFinishedHandler(p_WordCheckerFinished);
                p.WordCheckerStop += new WordCheckerStopHandler(p_WordCheckerStop);
                p.WordOnContext += new WordOnContextHandler(p_WordOnContext);
                checker[key] = p;
            }
            return p;
        }

        void p_WordOnContext(object sender, WordOnContextEventArgs e)
        {
            InvokeWordOnContext(sender, e);
        }

        bool p_WordCheckerStop(object sender, EventArgs e)
        {
            return InvokeWordCheckerStop(sender, e);
        }

        void p_WordCheckerFinished(object sender, WordCheckerEventArgs e)
        {
            InvokeWordCheckerFinished(sender, e);
        }

        bool p_WordChecker(object sender, WordEventArgs e)
        {
            return InvokeWordChecker(sender, e);
        }

        /// <summary>
        /// Returns the instance of base spell checker module for the given editor instance.
        /// </summary>
        /// <remarks>
        /// This instance provides access to the commands as regular method calls. It's recommended to use the commands and HtmlEditor's Invoke
        /// method. You may consider using this instance for advanced implementions that use more than just the common features.
        /// </remarks>
        /// <param name="key">The editor we reference to.</param>
        /// <returns>The base spellchecker instance this extender uses.</returns>
        public IDocumentSpellChecker GetSpellChecker(IHtmlEditor key)
        {
            return EnsureSpellCheckerExists(key);
        }

        private SpellCommands commands;

        /// <summary>
        /// Returns the definitions for commands accepted by the Speller extender.
        /// </summary>
        /// <remarks>
        /// The <see cref="SpellCommands"/> class defines the commands, which the extender registers 
        /// within the base control. The host application is supposed to issue the commands via the base component
        /// instead of calling the Speller directly to assure that multiple instances of the NetRox component are
        /// invoked properly.
        /// <seealso cref="SpellCommands"/>
        /// </remarks>
        [Browsable(false)]
        public SpellCommands Commands
        {
            get
            {
                if (commands == null)
                {
                    commands = new SpellCommands();
                }
                return commands;
            }
        }

        /// <summary>
        /// Support the designer infrastructure.
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <returns></returns>
        [ExtenderProvidedProperty(), Category("NetRix Component"), Description("SpellChecker Properties")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DefaultValue("0 properties changed")]
        public SpellCheckerProperties GetSpeller(IHtmlEditor htmlEditor)
        {
            return this.EnsurePropertiesExists(htmlEditor);
        }

        /// <summary>
        /// Support the designer infrastructure.
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <param name="props"></param>
        public void SetSpeller(IHtmlEditor htmlEditor, SpellCheckerProperties props)
        {
            if (props == null) return;
            if (props.HighLightStyle != null)
            {
                EnsurePropertiesExists(htmlEditor).HighLightStyle.LineColor = props.HighLightStyle.LineColor;
                EnsurePropertiesExists(htmlEditor).HighLightStyle.LineThroughStyle = props.HighLightStyle.LineThroughStyle;
                EnsurePropertiesExists(htmlEditor).HighLightStyle.LineType = props.HighLightStyle.LineType;
                EnsurePropertiesExists(htmlEditor).HighLightStyle.Priority = props.HighLightStyle.Priority;
                EnsurePropertiesExists(htmlEditor).HighLightStyle.TextBackgroundColor = props.HighLightStyle.TextBackgroundColor;
                EnsurePropertiesExists(htmlEditor).HighLightStyle.TextColor = props.HighLightStyle.TextColor;
                EnsurePropertiesExists(htmlEditor).HighLightStyle.UnderlineStyle = props.HighLightStyle.UnderlineStyle;
            }
            EnsurePropertiesExists(htmlEditor).IgnoreUpperCaseWords = props.IgnoreUpperCaseWords;
            EnsurePropertiesExists(htmlEditor).IgnoreWordsWithDigits = props.IgnoreWordsWithDigits;
            EnsurePropertiesExists(htmlEditor).MaxSuggestionsCount = props.MaxSuggestionsCount;
            EnsurePropertiesExists(htmlEditor).SuggestionType = props.SuggestionType;
            EnsurePropertiesExists(htmlEditor).IgnoreHtml = props.IgnoreHtml;
            EnsurePropertiesExists(htmlEditor).Dictionary = props.Dictionary;
            EnsurePropertiesExists(htmlEditor).DictionaryPath = props.DictionaryPath;
            EnsurePropertiesExists(htmlEditor).LanguageType = props.LanguageType;
            EnsurePropertiesExists(htmlEditor).CheckInternal = props.CheckInternal;
            EnsurePropertiesExists(htmlEditor).IgnoreList = props.IgnoreList;
            EnsurePropertiesExists(htmlEditor).ReplaceList = props.ReplaceList;

            EnsurePropertiesExists(htmlEditor).MainMenuVisible = props.MainMenuVisible;
            EnsurePropertiesExists(htmlEditor).ToolStripVisible = props.ToolStripVisible;

            if (menuStripSpeller != null && menuStripSpeller.Items.Count > 0 && props.MainMenuVisible)
            {
                menuStripSpeller.Items[0].Visible = true;
                ((HtmlEditor)htmlEditor).MenuStrip.Items.Add(menuStripSpeller.Items[0]);
            }
            if (toolStripSpeller != null && props.ToolStripVisible)
            {
                toolStripSpeller.Visible = true;
                ((HtmlEditor)htmlEditor).ToolStripContainer.TopToolStripPanel.Controls.Add(toolStripSpeller);
            }

            htmlEditor.RegisterPlugIn(this);
            htmlEditor.Loaded += new GuruComponents.Netrix.Events.LoadEventHandler(htmlEditor_Loaded);
        }
        
        void htmlEditor_Loaded(object sender, GuruComponents.Netrix.Events.LoadEventArgs e)
        {
            IHtmlEditor editor = (IHtmlEditor)sender;
            // add commands
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.RemoveHighLight));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StartBackground));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StopBackground));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StartWordByWord));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StopWordByWord));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StartBlock));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StopBlock));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.ClearBuffer));
        }

        /// <summary>
        /// Informs the extender that the containing control is ready.
        /// </summary>
        /// <param name="editor"></param>
        public void NotifyReadyStateCompleted(IHtmlEditor editor)
        {
            htmlEditor_ReadyStateCompleteInternal(editor);
            // reset former state
            if (IsInCheck)
            {
                EnsureSpellCheckerExists(editor).DoWordCheckingHighlights(EnsurePropertiesExists(editor).HighLightStyle, true);
            }
            else
            {
                EnsureSpellCheckerExists(editor).BackgroundService = false;
            }
        }

        void htmlEditor_ReadyStateCompleteInternal(IHtmlEditor editor)
        {
            // add commands
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.RemoveHighLight));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StartBackground));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StopBackground));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StartWordByWord));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StopWordByWord));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StartBlock));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.StopBlock));
            editor.AddCommand(new CommandWrapper(new EventHandler(SpellerOperation), Commands.ClearBuffer));            
        }

        void SpellerOperation(object sender, EventArgs e)
        {
            CommandWrapper cw = (CommandWrapper) sender;
            string checkpath;
            if (cw.CommandID.Guid.Equals(Commands.CommandGroup))
            {
                DocumentSpellChecker ds = (DocumentSpellChecker)EnsureSpellCheckerExists(cw.TargetEditor);
                switch ((SpellCommand)cw.ID)
                {
                    case SpellCommand.RemoveHighLight:
                        EnsureSpellCheckerExists(cw.TargetEditor).RemoveWordCheckingHighlights();
                        break;
                    case SpellCommand.ClearBuffer:
                        EnsureSpellCheckerExists(cw.TargetEditor).MisSpelledWords.Clear();
                        break;
                    case SpellCommand.StopBackground:
                        IsInBackground = false;
                        EnsureSpellCheckerExists(cw.TargetEditor).BackgroundService = false;
                        IsInCheck = false;
                        break;
                    case SpellCommand.StartBackground:
                        IsInBackground = true;
                        if (ds.Spelling.Dictionary.DictionaryFolder == null)
                        {
                            ds.Spelling.Dictionary.DictionaryFolder = "Dictionary";
                        }
                        checkpath = Path.Combine(ds.Spelling.Dictionary.DictionaryFolder, ds.Spelling.Dictionary.DictionaryFile);
                        if (!File.Exists(checkpath))
                        {
                            ds.Spelling.Dictionary.DictionaryFile = String.Format("{0}.dic", EnsurePropertiesExists(cw.TargetEditor).LanguageType);
                        }
                        ds.Spelling.SuggestionMode = EnsurePropertiesExists(cw.TargetEditor).SuggestionType;
                        ds.Spelling.IgnoreAllCapsWords = EnsurePropertiesExists(cw.TargetEditor).IgnoreUpperCaseWords;
                        ds.Spelling.IgnoreWordsWithDigits = EnsurePropertiesExists(cw.TargetEditor).IgnoreWordsWithDigits;
                        ds.Spelling.MaxSuggestions = EnsurePropertiesExists(cw.TargetEditor).MaxSuggestionsCount;
                        ds.Spelling.IgnoreHtml = EnsurePropertiesExists(cw.TargetEditor).IgnoreHtml;
                        ds.Spelling.IgnoreList.AddRange(EnsurePropertiesExists(cw.TargetEditor).IgnoreList);
                        foreach (KeyValuePair<string, string> de in EnsurePropertiesExists(cw.TargetEditor).ReplaceList)
                        {
                            ds.Spelling.ReplaceList[de.Key] = de.Value;
                        }
                        IsInCheck = true;
                        EnsureSpellCheckerExists(cw.TargetEditor).DoWordCheckingHighlights(EnsurePropertiesExists(cw.TargetEditor).HighLightStyle, true);
                        break;
                    case SpellCommand.StartWordByWord:
                        if (ds.Spelling.Dictionary.DictionaryFolder == null)
                        {
                            ds.Spelling.Dictionary.DictionaryFolder = "Dictionary";
                        }
                        checkpath = Path.Combine(ds.Spelling.Dictionary.DictionaryFolder, ds.Spelling.Dictionary.DictionaryFile);
                        if (!File.Exists(checkpath))
                        {
                            ds.Spelling.Dictionary.DictionaryFile = String.Format("{0}.dic", EnsurePropertiesExists(cw.TargetEditor).LanguageType);
                        }
                        ds.Spelling.Dictionary.DictionaryFile = String.Format("{0}.dic", EnsurePropertiesExists(cw.TargetEditor).LanguageType);
                        ds.Spelling.SuggestionMode = EnsurePropertiesExists(cw.TargetEditor).SuggestionType;
                        ds.Spelling.IgnoreAllCapsWords = EnsurePropertiesExists(cw.TargetEditor).IgnoreUpperCaseWords;
                        ds.Spelling.IgnoreWordsWithDigits = EnsurePropertiesExists(cw.TargetEditor).IgnoreWordsWithDigits;
                        ds.Spelling.MaxSuggestions = EnsurePropertiesExists(cw.TargetEditor).MaxSuggestionsCount;
                        ds.Spelling.IgnoreHtml = EnsurePropertiesExists(cw.TargetEditor).IgnoreHtml;
                        ds.Spelling.IgnoreList.AddRange(EnsurePropertiesExists(cw.TargetEditor).IgnoreList);
                        foreach (KeyValuePair<string, string> de in EnsurePropertiesExists(cw.TargetEditor).ReplaceList)
                        {
                            ds.Spelling.ReplaceList[de.Key] = de.Value;
                        }                        
                        IsInCheck = true;
                        ds.DoWordCheckingHighlights(ds.HighLightStyle, false);
                        break;
                    case SpellCommand.StopWordByWord:
                        IsInCheck = false;
                        break;
                    case SpellCommand.StopBlock:
                        IsInCheck = false;
                        break;
                    case SpellCommand.StartBlock:
                        ds.Spelling.SuggestionMode = EnsurePropertiesExists(cw.TargetEditor).SuggestionType;
                        ds.Spelling.IgnoreAllCapsWords = EnsurePropertiesExists(cw.TargetEditor).IgnoreUpperCaseWords;
                        ds.Spelling.IgnoreWordsWithDigits = EnsurePropertiesExists(cw.TargetEditor).IgnoreWordsWithDigits;
                        ds.Spelling.MaxSuggestions = EnsurePropertiesExists(cw.TargetEditor).MaxSuggestionsCount;
                        IsInCheck = true;
                        ds.DoBlockCheckingHighlights(EnsurePropertiesExists(cw.TargetEditor).HighLightStyle);
                        break;
                }
            }
        }

        /// <summary>
        /// Event fired for each word found. The event is disabled if the internal spell checker is being used (default).
        /// </summary>
        [Category("NetRix Events (external only)")] 
        public event WordCheckerHandler WordChecker;
        /// <summary>
        /// Event fired for each word found. The event is disabled if the internal spell checker is being used (default).
        /// </summary>
        /// <remarks>
        /// The return value of the callback function is used to stop the checking
        /// process immediately. In case of internal spell checker the event doesn't fire. One can use the
        /// StopWordByWord command as well as the StopBackground command. Issuing commands is based on the 
        /// <see cref="GuruComponents.Netrix.IHtmlEditor.InvokeCommand"/>
        /// </remarks>
        [Category("NetRix Events (external only)")] 
        public event WordCheckerStopHandler WordCheckerStop;
        
        /// <summary>
        /// Event fired once the word checking is done. 
        /// </summary>
        [Category("NetRix Events")] 
        public event WordCheckerFinishedHandler WordCheckerFinished;

        /// <summary>
        /// Event fired if the user has right clicked the word and the caret has moved.
        /// </summary>
        /// <remarks>
        /// This event is later than
        /// ContextMenu because it is necessary to being delayed to set the caret to the final position before
        /// an attached context menu opens.
        /// </remarks>
        [Category("NetRix Events")] 
        public event WordOnContextHandler WordOnContext;

        /// <summary>
		///     This event is fired when a word is deleted.
		/// </summary>
		/// <remarks>
		///		Use this event to update the parent text.
		/// </remarks>
		[Category("NetSpell Events ")] 
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
        
        
        private bool InvokeWordChecker(object sender, WordEventArgs e)
        {
            if (WordChecker != null)
            {
                return WordChecker(sender, e);
            }
            return false;
        }
        private void InvokeWordOnContext(object sender, WordOnContextEventArgs e)
        {
            if (WordOnContext != null)
            {
                WordOnContext(sender, e);
            }
        }
        private void InvokeWordCheckerFinished(object sender, WordCheckerEventArgs e)
        {
            if (WordCheckerFinished != null)
            {
                WordCheckerFinished(sender, e);
            }
        }
        private bool InvokeWordCheckerStop(object sender, EventArgs e)
        {
            if (WordCheckerStop != null)
            {
                return WordCheckerStop(sender, e);
            }
            // if NetSpell is being used we return false if no longer in check mode
            return !IsInCheck;
        }

	    private void spellChecker_MisspelledWord(object sender, SpellingEventArgs e)
        {
            Debug.WriteLine(e.Word, "MISSPELLED");
            if (MisspelledWord != null)
            {
                MisspelledWord(sender, e);                
            }
        }

        private void spellChecker_DeletedWord(object sender, SpellingEventArgs e)
        {
	        Debug.WriteLine(e.Word, "DELETED");
            if (DeletedWord != null)
            {
                DeletedWord(sender, e);
            }
        }

		private void spellChecker_DoubledWord(object sender, SpellingEventArgs e)
		{
			Debug.WriteLine(e.Word, "DOUBLED");
            if (DoubledWord != null)
            {
                DoubledWord(sender, e);
            }
		}

		private void spellChecker_EndOfText(object sender, EventArgs e)
		{
			IsInCheck = false;
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
		}

		private void spellChecker_ReplacedWord(object sender, ReplaceWordEventArgs e)
		{
            if (ReplacedWord != null)
            {
                ReplacedWord(sender, e);
            }
		}

        /// <summary>
        /// Returns the current assembly version number.
        /// </summary>
        [Category("NetRix Speller")]
        public string Version
        {
            get
            {
                return this.GetType().Assembly.GetName().Version.ToString(4);
            }
        }

        #region IExtenderProvider Member

        /// <summary>
        /// Informs the designer that the currently selected control can be extended by the Speller.
        /// </summary>
        /// <remarks>
        /// To make the Speller available the component must derive from <see cref="IHtmlEditor"/>.
        /// </remarks>
        /// <param name="extendee">Control which tries to adopt the Speller. Expected control is NetRix HtmlEditor only.</param>
        /// <returns>Returns <c>true</c> in case of NetRix component, <c>false</c> otherwise.</returns>
        public bool CanExtend(object extendee)
        {
            if (extendee is IHtmlEditor)
            {
                return true;
            } 
            else 
            {
                return false;
            }
        }

        /// <summary>
        /// Return the editor which the given instance actually extends.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        internal IHtmlEditor GetExtendee(Speller instance)
        {
            return (IHtmlEditor) properties[instance];   
        }

        #endregion

        #region IPlugIn Members

        /// <summary>
        /// Type
        /// </summary>
        [Browsable(false)]
        public Type Type
        {
            get { return this.GetType(); }
        }

        /// <summary>
        /// Internal name
        /// </summary>
        [Category("NetRix Speller")]
        public string Name
        {
            get { return "Speller"; }
        }

        /// <summary>
        /// Extends Netrix
        /// </summary>
        [Browsable(false)]
        public bool IsExtenderProvider
        {
            get { return true; }
        }

        /// <summary>
        /// The Spellchecker does not support any extender features.
        /// </summary>
        [Browsable(false)]
        public Feature Features
        {
            get { return Feature.None; }
        }

        /// <summary>
        /// The Spellchecker control does not support any namespaces.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IDictionary GetSupportedNamespaces(IHtmlEditor key)
        {
            return null;
        }
        
        private List<CommandExtender> extenderVerbs;

        /// <summary>
        /// List of element types, which the extender plugin extends.
        /// </summary>
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.PlugIns.IPlugIn.GetElementExtenders"/> for background information.
        /// </remarks>
        public List<CommandExtender> GetElementExtenders(IElement component)
        {
            if (component is BodyElement)
            {
                extenderVerbs = new List<CommandExtender>();
                CommandExtender ceOn = new CommandExtender(Commands.StartBackground);
                ceOn.Text = "SpellChecker On";
                extenderVerbs.Add(ceOn);
                CommandExtender ceOff = new CommandExtender(Commands.StopBackground);
                ceOff.Text = "SpellChecker Off";
                extenderVerbs.Add(ceOff);
                return extenderVerbs;
            }
            else
            {
                return null;
            }
        }

        System.Web.UI.Control IPlugIn.CreateElement(string tagName, IHtmlEditor editor)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        # region Toolbar
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Speller));
            this.toolStripSpeller = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSpellerSpell = new System.Windows.Forms.ToolStripButton();
            this.menuStripSpeller = new System.Windows.Forms.MenuStrip();
            this.toolStripParentSpeller = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSpellerChecker = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSpellerAsYouType = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSpeller.SuspendLayout();
            this.menuStripSpeller.SuspendLayout();
            // 
            // toolStripSpeller
            // 
            this.toolStripSpeller.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSpellerSpell});
            this.toolStripSpeller.Location = new System.Drawing.Point(0, 0);
            this.toolStripSpeller.Name = "toolStripSpeller";
            this.toolStripSpeller.Size = new System.Drawing.Size(100, 25);
            this.toolStripSpeller.TabIndex = 0;
            this.toolStripSpeller.Text = "Spell &Checker";
            // 
            // toolStripButtonSpellerSpell
            // 
            this.toolStripButtonSpellerSpell.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            //this.toolStripButtonSpellerSpell.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSpellerSpell.Image")));
            this.toolStripButtonSpellerSpell.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSpellerSpell.Name = "toolStripButtonSpellerSpell";
            this.toolStripButtonSpellerSpell.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSpellerSpell.Tag = "Spellcheck";
            this.toolStripButtonSpellerSpell.Text = "Spell &Checker";
            // 
            // menuStripSpeller
            // 
            this.menuStripSpeller.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripParentSpeller});
            this.menuStripSpeller.Location = new System.Drawing.Point(0, 0);
            this.menuStripSpeller.Name = "menuStripSpeller";
            this.menuStripSpeller.Size = new System.Drawing.Size(200, 24);
            this.menuStripSpeller.TabIndex = 0;
            this.menuStripSpeller.Text = "Spell &Checker";
            // 
            // toolStripParentSpeller
            // 
            this.toolStripParentSpeller.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSpellerChecker,
            this.toolStripMenuItemSpellerAsYouType});
            this.toolStripParentSpeller.Name = "toolStripParentSpeller";
            this.toolStripParentSpeller.Size = new System.Drawing.Size(113, 20);
            this.toolStripParentSpeller.Tag = "SpellerSpell";
            this.toolStripParentSpeller.Text = "Specl &Checker";
            // 
            // toolStripMenuItemSpellerChecker
            // 
            this.toolStripMenuItemSpellerChecker.Name = "toolStripMenuItemSpellerChecker";
            this.toolStripMenuItemSpellerChecker.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItemSpellerChecker.Tag = "SpellerChecker";
            this.toolStripMenuItemSpellerChecker.Text = "Spell Checker";
            // 
            // toolStripMenuItemSpellerAsYouType
            // 
            this.toolStripMenuItemSpellerAsYouType.Name = "toolStripMenuItemSpellerAsYouType";
            this.toolStripMenuItemSpellerAsYouType.Size = new System.Drawing.Size(149, 22);
            this.toolStripMenuItemSpellerAsYouType.Tag = "SpellerAsYouType";
            this.toolStripMenuItemSpellerAsYouType.Text = "As-You-Type";
            this.toolStripSpeller.ResumeLayout(false);
            this.toolStripSpeller.PerformLayout();
            this.menuStripSpeller.ResumeLayout(false);
            this.menuStripSpeller.PerformLayout();

        }

        # endregion

        /*
        # region External Speller Support

        /// <summary>
        /// Returns the word the caret is on or nearby.
        /// </summary>
        /// <returns>The recognized word, or <c>null</c>, if nothing was recognized.</returns>
        public string WordUnderPointer(IHtmlEditor editor)
        {
            return WordUnderPointer(false, editor);
        }

        ///<overloads/>
        /// <summary>
        /// Replaces the word the caret is on.
        /// </summary>
        /// <param name="replaceWith">String the word to be replaced with</param>
        public void ReplaceWordUnderPointer(string replaceWith, IHtmlEditor editor)
        {
            ReplaceWordUnderPointer(false, replaceWith, editor);
        }

        /// <summary>
        /// Replaces the word the caret is on.
        /// </summary>
        /// <param name="withHtml"></param>
        /// <param name="replaceWith">tring the word to be replaced with</param>
        public void ReplaceWordUnderPointer(bool withHtml, string replaceWith, IHtmlEditor editor)
        {
            WordUnderPointer(withHtml, editor);
            Interop.IHTMLDocument2 activeDocument = editor.GetActiveDocument(false);
            Interop.IHTMLTxtRange trg = ((Interop.IHtmlBodyElement)activeDocument.GetBody()).createTextRange();
            if (trg != null && trg.GetText() != null)
            {
                trg.SetText(replaceWith);
            }
        }

        /// <summary>
        /// Returns the word with or without inline HTML the caret is on on nearby.
        /// </summary>
        /// <param name="withHtml">If true the method returns any embedded HTML too, otherwise HTML is stripped out.</param>
        /// <returns>The recognized HTML, or <c>null</c>, if nothing was recognized.</returns>
        public string WordUnderPointer(bool withHtml, IHtmlEditor editor)
        {
            Interop.IHTMLDocument2 activeDocument = editor.GetActiveDocument(false);
            Interop.IDisplayServices ds = (Interop.IDisplayServices) activeDocument;
            Interop.IMarkupServices ims = activeDocument as Interop.IMarkupServices;
            Interop.IHTMLTxtRange tr = ((Interop.IHtmlBodyElement)activeDocument.GetBody()).createTextRange();
            Interop.IMarkupPointer pStart, pEnd;
            ims.CreateMarkupPointer(out pStart);
            ims.CreateMarkupPointer(out pEnd);
            // now reset pointer to caret 
            Interop.IHTMLCaret cr;
            ds.GetCaret(out cr);
            cr.MoveMarkupPointerToCaret(pStart);
            cr.MoveMarkupPointerToCaret(pEnd);
            Interop.CARET_DIRECTION dir;
            cr.GetCaretDirection(out dir);
            ims.MoveRangeToPointers(pStart, pEnd, tr);
            int result = 0;
            System.Diagnostics.Debug.WriteLine((Interop.CARET_DIRECTION)dir, "DIR");
            while (tr.GetText() == null)
            {
                if ((Interop.CARET_DIRECTION)dir == Interop.CARET_DIRECTION.CARET_DIRECTION_BACKWARD)
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

        private void TrimRange(Interop.IHTMLTxtRange tr)
        {
            string txt = null;
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


        # endregion
         * */
    }
}
