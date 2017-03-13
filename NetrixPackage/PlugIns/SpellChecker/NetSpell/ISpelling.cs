using System;
using System.Collections.Generic;
namespace GuruComponents.Netrix.SpellChecker.NetSpell
{
    /// <summary>
    /// Common access to all public NetSpell features and methods.
    /// </summary>
    /// <remarks>
    /// This is a netspell specific access interface and does not handle any calls if an external
    /// spellchecker is being used.
    /// </remarks>
    interface ISpelling
    {
        /// <summary>
        ///     Display the 'Spell Check Complete' alert.
        /// </summary>
        bool AlertComplete { get; set; }
        /// <summary>
        ///     The current word being spell checked from the text property
        /// </summary>
        string CurrentWord { get; }
        /// <summary>
        ///     Deletes the CurrentWord from the Text Property
        /// </summary>
        /// <remarks>
        ///		Note, calling ReplaceWord with the ReplacementWord property set to 
        ///		an empty string has the same behavior as DeleteWord.
        /// </remarks>
        void DeleteWord();
        /// <summary>
        ///     The WordDictionary object to use when spell checking
        /// </summary>
        GuruComponents.Netrix.SpellChecker.NetSpell.Dictionary.WordDictionary Dictionary { get; set; }
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
        ///     Ignore words with all capital letters when spell checking
        /// </summary>
        bool IgnoreAllCapsWords { get; set; }
        /// <summary>
        /// Ignores all instances of the CurrentWord in the Text Property
        /// </summary>
        void IgnoreAllWord();
        /// <summary>
        ///     Ignore html tags when spell checking
        /// </summary>
        bool IgnoreHtml { get; set; }
        /// <summary>
        ///     List of words to automatically ignore
        /// </summary>
        /// <remarks>
        ///		When <see cref="IgnoreAllWord"/> is clicked, the <see cref="CurrentWord"/> is added to this list
        /// </remarks>
        List<string> IgnoreList { get; }
        void IgnoreWord();
        /// <summary>
        ///     Ignore words with digits when spell checking
        /// </summary>
        bool IgnoreWordsWithDigits { get; set; }
        /// <summary>
        ///     The maximum number of suggestions to generate
        /// </summary>
        int MaxSuggestions { get; set; }
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
        ///     Replaces all instances of the CurrentWord in the Text Property
        /// </summary>
        void ReplaceAllWord();
        /// <summary>
        ///     List of words and replacement values to automatically replace
        /// </summary>
        /// <remarks>
        ///		When <see cref="ReplaceAllWord()"/> is clicked, the <see cref="CurrentWord"/> is added to this list
        /// </remarks>
        Dictionary<string, string> ReplaceList { get; }
        /// <summary>
        /// The word that is being replaced.
        /// </summary>
        string ReplacementWord { get; set; }
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
        ///     Replaces the instances of the CurrentWord in the Text Property
        /// </summary>
        void ReplaceWord();
        bool ShowDialog { get; set; }
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
        /// <seealso cref="CurrentWord"/>
        /// <seealso cref="WordIndex"/>
        bool SpellCheck(string text);
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
        /// <seealso cref="CurrentWord"/>
        /// <seealso cref="WordIndex"/>
        bool SpellCheck(int startWordIndex, int endWordIndex);
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
        /// <seealso cref="CurrentWord"/>
        /// <seealso cref="WordIndex"/>
        bool SpellCheck(string text, int startWordIndex);
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
        /// <seealso cref="CurrentWord"/>
        /// <seealso cref="WordIndex"/>
        bool SpellCheck(int startWordIndex);
        /// <summary>
        ///     Spell checks the words in the <see cref="Text"/> property starting
        ///     at the <see cref="WordIndex"/> position.
        /// </summary>
        /// <returns>
        ///     Returns true if there is a word found in the text 
        ///     that is not in the dictionaries
        /// </returns>
        /// <seealso cref="CurrentWord"/>
        /// <seealso cref="WordIndex"/>
        bool SpellCheck();
        /// <summary>
        ///     Populates the <see cref="Suggestions"/> property with word suggestions
        ///     for the word
        /// </summary>
        /// <param name="word" type="string">
        ///     <para>
        ///         The word to generate suggestions on
        ///     </para>
        /// </param>
        /// <remarks>
        ///		This method sets the <see cref="Text"/> property to the word. 
        ///		Then calls <see cref="TestWord"/> on the word to generate the need
        ///		information for suggestions. Note that the Text, CurrentWord and WordIndex 
        ///		properties are set when calling this method.
        /// </remarks>
        /// <seealso cref="CurrentWord"/>
        /// <seealso cref="Suggestions"/>
        /// <seealso cref="TestWord"/>
        void Suggest(string word);
        /// <summary>
        ///     Populates the <see cref="Suggestions"/> property with word suggestions
        ///     for the <see cref="CurrentWord"/>
        /// </summary>
        /// <remarks>
        ///		<see cref="TestWord"/> must have been called before calling this method
        /// </remarks>
        /// <seealso cref="CurrentWord"/>
        /// <seealso cref="Suggestions"/>
        /// <seealso cref="TestWord"/>
        void Suggest();
        /// <summary>
        /// Gets or sets the internal spelling suggestions dialog form.
        /// </summary>
        System.Windows.Forms.Form SpellcheckerForm { get; set; }
        /// <summary>
        ///     The suggestion strategy to use when generating suggestions
        /// </summary>
        SuggestionEnum SuggestionMode { get; set; }
        /// <summary>
        ///     An array of word suggestions for the correct spelling of the misspelled word
        /// </summary>
        /// <seealso cref="Suggest()"/>
        /// <seealso cref="SpellCheck()"/>
        /// <seealso cref="MaxSuggestions"/>
        List<string> Suggestions { get; }
        /// <summary>
        ///     Checks to see if the word is in the dictionary
        /// </summary>
        /// <param name="word" type="string">
        ///     <para>
        ///         The word to check
        ///     </para>
        /// </param>
        /// <returns>
        ///     Returns true if word is found in dictionary
        /// </returns>
        bool TestWord(string word);
        /// <summary>
        ///     The text to spell check
        /// </summary>
        string Text { get; set; }
        /// <summary>
        ///     TextIndex is the index of the current text being spell checked
        /// </summary>
        int TextIndex { get; }
        /// <summary>
        ///     The number of words being spell checked
        /// </summary>
        int WordCount { get; }
        /// <summary>
        ///     WordIndex is the index of the current word being spell checked
        /// </summary>
        int WordIndex { get; set; }
    }
}