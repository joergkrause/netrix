using System;
using System.Collections;

namespace GuruComponents.Netrix.SpellChecker
{
	/// <summary>
	/// The event arguments for the <see cref="WordCheckerFinishedHandler"/>.
	/// </summary>
	/// <remarks>
	/// The event arguments contain information about the number of words in the document and the
	/// list of segments as <see cref="System.Collections.ArrayList">ArrayList</see>.
	/// </remarks>
    /// <seealso cref="WordCheckerFinishedHandler"/>
	public class WordCheckerEventArgs : EventArgs
	{
		private int wordCount;
		private ArrayList segmentStore;

		internal WordCheckerEventArgs(int wc, ArrayList ss)
		{
			this.wordCount = wc;
			this.segmentStore = ss;
		}

		/// <summary>
		/// Gets the number of word the document contains.
		/// </summary> 
		public int WordCount
		{
			get
			{
				return this.wordCount;
			}
		}

		/// <summary>
		/// Gets the list of highlighted segments.
		/// </summary>
		/// <remarks>
		/// This property supports the NetRix infrastructure
		/// and cannot be used to manipulate the highlighting later on.
		/// </remarks>
		public ArrayList Segments
		{
			get
			{
				return this.segmentStore;
			}
		}
	}
}
