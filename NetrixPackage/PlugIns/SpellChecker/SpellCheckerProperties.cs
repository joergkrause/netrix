using System;
using System.Windows.Forms;
using System.ComponentModel;
using GuruComponents.Netrix;

using GuruComponents.Netrix.WebEditing.HighLighting;
using GuruComponents.Netrix.SpellChecker.NetSpell;
using System.Collections;
using System.Globalization;
using System.Drawing;
using System.Collections.Generic;

namespace GuruComponents.Netrix.SpellChecker
{
    /// <summary>
    /// This class collects the properties used to configure the spell checker as well as the spell checker dialog.
    /// </summary>
    /// <remarks>
    /// The direct usage of this class is not recommended. Instead, the Extenderprovider class <see cref="SpellChecker"/>
    /// should be used to set the properties using the VS.NET designer. The properties will appear in the NetRix Component
    /// group as "SpellChecker on spellchecker1" behind the plus sign (other languages may have other descriptions and
    /// the name depends on the name the developer gave the extender). 
    /// </remarks>
    [Serializable()]
    [DefaultValue("0 properties changed")]
    public class SpellCheckerProperties
    {

        private string dictionary;

        [NonSerialized()]
        private DocumentSpellChecker documentSpellChecker;

        private bool mainMenuVisible, toolStripVisible;

        /// <summary>
        /// Set main menu.
        /// </summary>
        [Category("NetRix Speller UI")]
        public bool MainMenuVisible
        {
            // Add parent dropdown
            set
            { mainMenuVisible = value; }
            get { return mainMenuVisible; }
        }

        /// <summary>
        /// Set toolstrip.
        /// </summary>
        [Category("NetRix Speller UI")]
        public bool ToolStripVisible
        {
            set { toolStripVisible = value; }
            get { return toolStripVisible; }
        }


        /// <summary>
        /// Path to the dictionary being used for current spell process.
        /// </summary>
        [Browsable(true)]
        [CategoryAttribute("Options")]
        [Description("List of words being automatically replaced.")]
        [DefaultValue("en-US")]
        public string Dictionary
        {
            get { return dictionary; }
            set 
            { 
                dictionary = value;
                if (!String.IsNullOrEmpty(dictionary))
                {
                    if (dictionary.EndsWith(".dic"))
                    {
                        dictionary = dictionary.Replace(".dic", "");
                    }
                }
                if (documentSpellChecker != null)
                {
                    documentSpellChecker.Spelling.Dictionary.DictionaryFile = dictionary + ".dic";
                }
            }
        }
        private Dictionary<string, string> replaceList;

        /// <summary>
		/// List of words being automatically replaced.
		/// </summary>
        [Browsable(false)]
		[CategoryAttribute("Options")]
		[Description("List of words being automatically replaced.")]
		public Dictionary<string,string> ReplaceList
		{
			get {return replaceList;}
			set 
            {
                replaceList = value;
                if (documentSpellChecker != null)
                {
                    documentSpellChecker.Spelling.ReplaceList.Clear();
                    if (value != null)
                    {
                        foreach (KeyValuePair<string,string> de in value)
                        {
                            documentSpellChecker.Spelling.ReplaceList.Add(de.Key, de.Value);
                        }
                    }
                }
            }
		}

        private List<string> ignoreList;

        /// <summary>
		/// List of words being ignored during check procedure.
		/// </summary>
        [Browsable(false)]
		[CategoryAttribute("Options")]
		[Description("List of words being ignored.")]
		public List<string> IgnoreList
		{
			get {return ignoreList;}
			set 
            {
                ignoreList = value;
                if (documentSpellChecker != null)
                {
                    documentSpellChecker.Spelling.IgnoreList.Clear();
                    if (value != null)
                    {
                        documentSpellChecker.Spelling.IgnoreList.AddRange(value.ToArray());
                    }
                }
            }
		}

        private bool ignoreHtml;

        /// <summary>
		///     Ignore html tags when spell checking.
		/// </summary>
		[DefaultValue(true)]
		[CategoryAttribute("Options")]
		[Description("Ignore html tags when spell checking")]
		public bool IgnoreHtml
		{
			get {return ignoreHtml;}
			set 
            {
                ignoreHtml = value;
                if (documentSpellChecker != null)
                {
                    documentSpellChecker.Spelling.IgnoreHtml = value;
                }
            }
		}

        CultureInfo languageType;

        /// <summary>
        /// Language (Culture) from list of supported languages.
        /// </summary>
        /// <remarks>
        /// The component will seek for a dictionary according to the settings. If the culture is set to
        /// "de-DE" the component will look for "de-DE.dic" in the default or defined folder. There is no
        /// fallback to another language or base language.
        /// </remarks>
        [Browsable(true)]
        [Description("Language (Culture) from list of supported languages.")]
        public CultureInfo LanguageType
        {
            get { return languageType; }
            set 
            { 
                languageType = value;
            }
        }

        string dictionaryPath;

        /// <summary>
        /// Path to the dictionaries.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Browsable(false)]
        [Description("Path to the dictionaries.")]
        [DefaultValue("")]
        public string DictionaryPath
        {
            get { return dictionaryPath; }
            set 
            { 
                dictionaryPath = value;
                if (documentSpellChecker != null)
                {
                    documentSpellChecker.Spelling.Dictionary.DictionaryFolder = value;
                }
            }
        }

        bool ignoreWordsWithDigits;

        /// <summary>
        /// Ignore words with digits when spell checking.
        /// </summary>
        [DefaultValue(true)]
        [Browsable(true)]
        [Description("Ignore words with digits when spell checking.")]
        public bool IgnoreWordsWithDigits
        {
            get { return ignoreWordsWithDigits; }
            set 
            {
                ignoreWordsWithDigits = value;
                if (documentSpellChecker != null)
                {
                    documentSpellChecker.Spelling.IgnoreWordsWithDigits = value;
                }
            }
        }
        bool ignoreUpperCaseWords;

        /// <summary>
        /// Whether or not ognore uppercase words.
        /// </summary>
        /// <remarks>
        /// If set to <c>true</c> the speller will ignore words which consists of upper case letters only.
        /// </remarks>
        [DefaultValue(true)]
        [Browsable(true)]
        [Description("Whether or not ognore uppercase words.")]
        public bool IgnoreUpperCaseWords
        {
            get { return ignoreUpperCaseWords; }
            set 
            { 
                ignoreUpperCaseWords = value;
                if (documentSpellChecker != null)
                {
                    documentSpellChecker.Spelling.IgnoreAllCapsWords = value;
                }
            }
        }
        int maxSuggestionsCount;

        /// <summary>
        /// The maximum number of suggestions to generate. Default is 25.
        /// </summary>
        /// <remarks>
        /// Suggestions can be used to create a context menu or dialog where the user could choose
        /// suggestions from. Suggestions are create based on current word and pulled from the current
        /// dictionary.
        /// </remarks>
        [DefaultValue(25)]
        [Browsable(true)]
        [Description("The maximum number of suggestions to generate. Default is 25.")]
        public int MaxSuggestionsCount
        {
            get { return maxSuggestionsCount; }
            set 
            { 
                maxSuggestionsCount = value;
                if (documentSpellChecker != null)
                {
                    documentSpellChecker.Spelling.MaxSuggestions = value;
                }
            }
        }
        
        /// <summary>
        /// The style used to format wrongly spelled words.
        /// </summary>
        /// <remarks>
        /// By default a red waved line is being used.
        /// </remarks>
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IHighLightStyle HighLightStyle
        {
            get 
            {
                if (documentSpellChecker != null)
                {
                    return documentSpellChecker.HighLightStyle;
                } 
                return null; 
            }
            set
            {
                if (documentSpellChecker != null)
                {
                    documentSpellChecker.HighLightStyle = value;
                } 
            }
        }

        SuggestionEnum suggestionType;

        /// <summary>
        /// The suggestion strategy to use when generating suggestions.
        /// </summary>
        [Browsable(true)]
        [Description("The suggestion strategy to use when generating suggestions.")]
        [DefaultValue(typeof(SuggestionEnum), "PhoneticNearMiss")]
        public SuggestionEnum SuggestionType
        {
            get { return suggestionType; }
            set 
            { 
                suggestionType = value;
                if (documentSpellChecker != null)
                {
                    documentSpellChecker.Spelling.SuggestionMode = value;
                }
            }
        }

        bool checkInternal;

        /// <summary>
        /// Activates the internal spell checker, based on NetSpell.
        /// </summary>
        ///<remarks>
        /// If the internal spell checker is used the NetRix Spell events are disabled. 
        ///</remarks>
        [Browsable(true)]
        [DefaultValue(true)]
        [Description("Activates the internal spell checker, based on NetSpell.")]
        public bool CheckInternal
        {
            get { return checkInternal; }
            set 
            { 
                checkInternal = value;
                if (documentSpellChecker != null)
                {
                    documentSpellChecker.CheckInternal = value;
                }
            }
        }

        /// <summary>
        /// Constructor for properties with backreference parameter to spellchecker instance.
        /// </summary>
        /// <param name="documentSpellChecker">Spellchecker instance this property instance refers to.</param>
        public SpellCheckerProperties(IDocumentSpellChecker documentSpellChecker)
        {
            this.documentSpellChecker = (DocumentSpellChecker) documentSpellChecker;
            // default for spellchecking
            HighlightColor hc = HighlightColor.Color(Color.Red);
            UnderlineStyle us = UnderlineStyle.Wave;
            IHighLightStyle hs = new HighLightStyle();
            hs.LineColor = hc;
            hs.UnderlineStyle = us;
            this.documentSpellChecker.HighLightStyle = hs;
            ignoreWordsWithDigits = true;
            ignoreUpperCaseWords = true;
            maxSuggestionsCount = 25;
            ignoreHtml = true;
            ignoreList = new List<string>();
            replaceList = new Dictionary<string,string>();
            checkInternal = true;
            if (String.IsNullOrEmpty(this.Dictionary))
            {
                this.Dictionary = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            }
        }

        /// <summary>
        /// Overridden to support design time features.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int changes = 0;
            changes += (checkInternal == true) ? 0 : 1;
            changes += (ignoreHtml == true) ? 0 : 1;
            changes += (ignoreWordsWithDigits == true) ? 0 : 1;
            changes += (maxSuggestionsCount == 25) ? 0 : 1;
            changes += (ignoreUpperCaseWords == true) ? 0 : 1;
            changes += (suggestionType == SuggestionEnum.PhoneticNearMiss) ? 0 : 1;
            changes += (Dictionary == System.Threading.Thread.CurrentThread.CurrentUICulture.Name) ? 0 : 1;
            changes += (MainMenuVisible) ? 1 : 0;
            changes += (ToolStripVisible) ? 1 : 0;
            return String.Format("{0} propert{1} changed", changes, (changes == 1) ? "y" : "ies");
        }


    }
            
}
