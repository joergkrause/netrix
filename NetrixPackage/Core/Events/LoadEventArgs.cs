using System;

namespace GuruComponents.Netrix.Events
{
	/// <summary>
	/// LoadEventArgs provides information about the current load process.
	/// </summary>
	public class LoadEventArgs : EventArgs
	{
        /// <summary>
        /// Current encoding used on save.
        /// </summary>
        protected System.Text.Encoding _encoding;
        private string _url;

		/// <summary>
		/// Constructor. It's build by Load event handler and not intendet to being called from user's code.
		/// </summary>
		/// <param name="encoding"></param>
		/// <param name="url"></param>
		public LoadEventArgs(System.Text.Encoding encoding, string url)
		{
            _url = url;
            _encoding = encoding;
		}

        /// <summary>
        /// Gets the URL or filename which causes the load process.
        /// </summary>
        public string Url
        {
            get
            {
                return _url;
            }
        }

        /// <summary>
        /// Gets the encoding used to load the content.
        /// </summary>
        public System.Text.Encoding Encoding
        {
            get
            {
                return _encoding;
            }
        }
	}
}
