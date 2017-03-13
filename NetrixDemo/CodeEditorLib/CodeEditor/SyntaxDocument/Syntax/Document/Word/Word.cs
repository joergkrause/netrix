using System.Drawing;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// Word types
	/// </summary>
	public enum WordType
	{
		/// <summary>
		/// The word is a normal word/text
		/// </summary>
		xtWord = 0,
		/// <summary>
		/// The word is a space char
		/// </summary>
		xtSpace = 1,
		/// <summary>
		/// The word is a tab char
		/// </summary>
		xtTab = 2
	}

	/// <summary>
	/// The word object class represents a word in a Row object
	/// </summary>
	public sealed class Word
	{
		#region General Declarations

		/// <summary>
		/// The parent row
		/// </summary>
		public Row Row; //the row that holds this word
		/// <summary>
		/// The parent segment
		/// </summary>
		public Segment Segment; //the segment that this word is located in
		/// <summary>
		/// The type of the word
		/// </summary>
		public WordType Type; //word type , space , tab , word
		/// <summary>
		/// The pattern that created this word
		/// </summary>
		public Pattern Pattern; //the pattern that found this word

		/// <summary>
		/// The text of the word
		/// </summary>
		public string Text; //the text in the word
		/// <summary>
		/// The style of the word
		/// </summary>
		public TextStyle Style; //the style of the word
		/// <summary>
		/// Color of the error wave lines
		/// </summary>
		public Color ErrorColor = Color.Red;

		/// <summary>
		/// True if the word has error wave lines
		/// </summary>
		public bool HasError = false;

		/// <summary>
		/// The ToolTip text for the word
		/// </summary>
		public string InfoTip = null;

		#endregion

		/// <summary>
		/// Gets the index of the word in the parent row
		/// </summary>
		public int Index
		{
			get { return this.Row.IndexOf(this); }
		}

		/// <summary>
		/// Returns the column where the word starts on the containing row.
		/// </summary>
		public int Column
		{
			get
			{
				int x = 0;
				foreach (Word w in this.Row)
				{
					if (w == this)
						return x;
					x += w.Text.Length;
				}
				return -1;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public Word()
		{
		}
	}
}