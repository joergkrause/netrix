namespace GuruComponents.Netrix.SpellChecker
{
	/// <summary>
	/// Event arguments used for the various speller handler.
	/// </summary>
	public class WordEventArgs
	{

		private string replacementWord;
		private string currentWord;
		private string currentHtml;
		private int wordCount;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="word"></param>
        /// <param name="html"></param>
        /// <param name="count"></param>
		public WordEventArgs(string word, string html, int count)
		{
			currentHtml = html;
			currentWord = word;
			wordCount = count;

		}

        /// <summary>
        /// Word which replaces the current word.
        /// </summary>
		public string ReplacementWord
		{
			get
			{
				return replacementWord;
			}
			set
			{
				replacementWord = value;
			}
		}

        /// <summary>
        /// Word recognizes as wrongly spelled.
        /// </summary>
		public string Word
		{
			get
			{
				return currentWord;
			}
			set
			{
				currentWord = value;
			}
		}

        /// <summary>
        /// Word with HTML formatting recognizes as wrongly spelled.
        /// </summary>
		public string Html
		{
			get
			{
				return currentHtml;
			}
		}

        /// <summary>
        /// Number of words currently counted in the document.
        /// </summary>
		public int WordCount
		{
			get
			{
				return wordCount;
			}
		}




	}
}
