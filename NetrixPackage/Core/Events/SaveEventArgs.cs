using System.Text;

namespace GuruComponents.Netrix.Events
{
	/// <summary>
	/// SaveEventArgs provides information about the current save process.
	/// </summary>
	public class SaveEventArgs : LoadEventArgs
	{

		/// <summary>
		/// Constructor. It's build by Save event handler and not intendet to being called from user's code.
		/// </summary>
		/// <param name="encoding"></param>
		/// <param name="url"></param>
		public SaveEventArgs(Encoding encoding, string url) : base(encoding, url)
		{
		}

        /// <summary>
        /// Set another encoding temporarily, if used within the <see cref="IHtmlEditor.Saving">Saving</see> event.
        /// </summary>
        /// <param name="encoding"></param>
        public void SetEncoding(Encoding encoding)
        {
            _encoding = encoding;
        }

	}
}
