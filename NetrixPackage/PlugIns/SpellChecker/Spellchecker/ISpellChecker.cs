using System.Collections;
using GuruComponents.Netrix.WebEditing.HighLighting;
using GuruComponents.Netrix.SpellChecker.NetSpell.Dictionary;
using GuruComponents.Netrix.SpellChecker.NetSpell;
using System.Collections.Generic;

namespace GuruComponents.Netrix.SpellChecker
{
    /// <summary>
    /// This realizes the basic spell checker API using callback methods.
    /// </summary>
    /// <remarks>
    /// See <see cref="SpellChecker"/> class for more information.
    /// </remarks>
    /// <seealso cref="SpellChecker"/>
    public interface IDocumentSpellChecker
    {
        /// <summary>
        /// Gets or sets the <see cref="GuruComponents.Netrix.WebEditing.HighLighting.HighLightStyle"/> for the
        /// spell checker.
        /// </summary>
        /// <remarks>
        /// The style is used to highlight words marked as wrong.
        /// </remarks>
        IHighLightStyle HighLightStyle { get; set; }
        /// <summary>
        /// Activates or deactivates the background service.
        /// </summary>
        /// <remarks>
        /// This service runs through the document and restarts
        /// at the end automatically. Setting this property to <c>false</c> will stop the service.
        /// </remarks>
        bool BackgroundService { get; set; }
        
        /// <summary>
        /// Returns the word the caret is on on nearby.
        /// </summary>
        /// <returns>The recognized word, or <c>null</c>, if nothing was recognized.</returns>
		string WordUnderPointer();

        /// <summary>
        /// Replaces the word the caret is currently in with other text.
        /// </summary>
        /// <param name="replaceWith">Text for replacement.</param>
        void ReplaceWordUnderPointer(string replaceWith);

        /// <summary>
        /// Replaces the word the caret is currently in with other text.
        /// </summary>
        /// <param name="withHtml">If set to <c>true</c> it's allowed to add HTML.</param>
        /// <param name="replaceWith">Text or HTML for replacement.</param>
        void ReplaceWordUnderPointer(bool withHtml, string replaceWith);
		
        /// <summary>
        /// Returns the word with or without inline HTML the caret is on on nearby.
        /// </summary>
        /// <param name="withHtml">If true the method returns any embedded HTML too, otherwise HTML is stripped out.</param>
        /// <returns>The recognized HTML, or <c>null</c>, if nothing was recognized.</returns>
        string WordUnderPointer(bool withHtml);

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
        bool TestWord(string word);

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
        void Suggest(string word);
        /// <summary>
        ///     Populates the <see cref="Suggestions"/> property with word suggestions.
        /// </summary>
        /// <remarks>
        ///		<see cref="TestWord"/> must have been called before calling this method.
        /// </remarks>
        /// <seealso cref="Suggestions()"/>
        /// <seealso cref="TestWord"/>
        void Suggest();

        /// <summary>
        ///     Deletes the CurrentWord from the Text Property
        /// </summary>
        /// <remarks>
        ///		Note, calling ReplaceWord with the ReplacementWord property set to 
        ///		an empty string has the same behavior as DeleteWord.
        /// </remarks>
        void DeleteWord();

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
        int EditDistance(string source, string target, bool positionPriority);

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
        int EditDistance(string source, string target);

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
        int GetWordIndexFromTextIndex(int textIndex);

        /// <summary>
        /// Ignores all instances of the CurrentWord in the Text Property
        /// </summary>
        void IgnoreAllWord();

        /// <summary>
        /// Ignores the instances of the CurrentWord in the Text Property.
        /// </summary>
        /// <remarks>
        /// Must call SpellCheck after call this method to resume spell checking.
        /// Forces the IgnoredWord event on Speller plug-in. Host application must handle.
        /// </remarks>
        void IgnoreWord();

        /// <summary>
        ///     Replaces all instances of the CurrentWord in the Text Property
        /// </summary>
        void ReplaceAllWord();

        /// <summary>
        ///     Replaces all instances of the CurrentWord in the Text Property
        /// </summary>
        /// <param name="replacementWord" type="string">
        ///     <para>
        ///         The word to replace the CurrentWord with
        ///     </para>
        /// </param>
        void ReplaceAllWord(string replacementWord);


        /// <summary>
        ///     Replaces the instances of the CurrentWord in the Text Property
        /// </summary>
        void ReplaceWord();

        /// <summary>
        ///     Replaces the instances of the CurrentWord in the Text Property
        /// </summary>
        /// <param name="replacementWord" type="string">
        ///     <para>
        ///         The word to replace the CurrentWord with
        ///     </para>
        /// </param>
        void ReplaceWord(string replacementWord);

        /// <summary>
        ///     Spell checks the words in the <see cref="Text"/> property starting
        ///     at the <see cref="WordIndex"/> position.
        /// </summary>
        /// <returns>
        ///     Returns true if there is a word found in the text 
        ///     that is not in the dictionaries
        /// </returns>
        /// <seealso cref="WordIndex"/>
        bool SpellCheck();

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
        bool SpellCheck(int startWordIndex);

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
        bool SpellCheck(int startWordIndex, int endWordIndex);

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
        bool SpellCheck(string text);

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
        bool SpellCheck(string text, int startWordIndex);

        /// <summary>
        /// Event fired for each word found.
        /// </summary>
        event WordCheckerHandler WordChecker;
        /// <summary>
        /// Event fired for each word found. The return value of the callback function is used to stop the checking
        /// process immediately.
        /// </summary>
        event WordCheckerStopHandler WordCheckerStop;
        /// <summary>
        /// Event fired once the word checking is done.
        /// </summary>
        event WordCheckerFinishedHandler WordCheckerFinished;

        /// <summary>
        /// Event fired if the suer has right clicked the word and the caret has moved.
        /// </summary>
        /// <remarks>
        /// This event is later than
        /// ContextMenu because it is necessary to being delayed to set the caret to the final position before
        /// an attached context menu opens.
        /// </remarks>
        event WordOnContextHandler WordOnContext;

        /// <summary>
        /// Remove the highlighted sections from the whole document.
        /// </summary>
        void RemoveWordCheckingHighlights();

		/// <summary>
		/// Checks the complete document and highlight any wrong word using default highlight style.
		/// </summary>
        void DoWordCheckingHighlights();

        /// <summary>
        /// Checks the complete document and highlight any wrong word. Search and spell call back on word base.</summary>
        /// <remarks>
        /// The method calls a callback method the host application
        /// must provide and which returns a boolean value to control the behavior of the highlighter.
        /// </remarks>
        /// <param name="highLightStyle">The style used to mark wrong words</param>
        void DoWordCheckingHighlights(IHighLightStyle highLightStyle);

        /// <summary>
        /// Checks the complete document and highlight any wrong word. Search and spell call back on word base. 
        /// </summary>
        /// <remarks>
		/// The method calls a callback method the host application
		/// must provide and which returns a boolean value to control the behavior of the highlighter.
        /// </remarks>
        /// <param name="highLightStyle">The style used to mark wrong word</param>
        /// <param name="backgroundService">If true the service runs as a endless background process until the WordStop callback handler returns true.</param>
        void DoWordCheckingHighlights(IHighLightStyle highLightStyle, bool backgroundService);
        

		/// <summary>
		/// Checks the complete document and highlight any wrong word. Search and spell call back on sentence base.
		/// </summary>
		/// <param name="highLightStyle"></param>
		void DoBlockCheckingHighlights(IHighLightStyle highLightStyle);

		/// <summary>
		/// Checks the complete document interactively and tags the current word in default selection style.
		/// </summary>
		void DoBlockCheckingSelected();

        /// <summary>
        /// Checks the complete document and select any wrong word with the standard selection.
        /// </summary>
        void DoWordCheckingSelected();

		/// <summary>
		/// Returns the collection of misspelled words.
		/// </summary>
		IList MisSpelledWords { get; }

        # region NetSpell Properties

        /// <summary>
        /// Gets or sets a value that activates the internal NetSpell spell checker.
        /// </summary>
        bool CheckInternal
        {
            get;
            set;
        }

        /// <summary>
        ///     The suggestion strategy to use when generating suggestions
        /// </summary>
        SuggestionEnum SuggestionMode
        {
            set;
            get;
        }

        /// <summary>
        ///     An array of word suggestions for the correct spelling of the misspelled word
        /// </summary>
        /// <seealso cref="Suggest()"/>
        /// <seealso cref="SpellCheck()"/>
        List<string> Suggestions
        {
            get;
        }

        /// <summary>
        ///     The text to spell check
        /// </summary>
        string Text
        {
            get;
        }

        /// <summary>
        ///     TextIndex is the index of the current text being spell checked
        /// </summary>
        int TextIndex
        {
            get;
        }

        /// <summary>
        ///     The number of words being spell checked
        /// </summary>
        int WordCount
        {
            get;
        }

        /// <summary>
        ///     WordIndex is the index of the current word being spell checked
        /// </summary>
        int WordIndex
        {
            get;
        }

        /// <summary>
        ///     The WordDictionary object to use when spell checking.
        /// </summary>
        WordDictionary Dictionary
        {
            get;
        }

        # endregion

    }
}