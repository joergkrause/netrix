using System;
using System.Collections;
using System.IO;
using System.Text;

namespace GuruComponents.Netrix.UserInterface.StyleParser
{
	/// <summary>
	/// This class parses a stylesheet file
	/// </summary>
	/// <remarks>
	/// It checks for style selectors and fires an event
	/// if a selector was found.
	/// </remarks>
	public class CssParser
	{

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <remarks>
        /// Initialises the instance of the parser class.
        /// </remarks>
		public CssParser()
		{
			p = new Parser();
		}

		private string _source;
		private string _fileName;
		private Parser p;

        /// <summary>
        /// The style sheet file.
        /// </summary>
		public string StyleSheet
		{
			get
			{
				return _source;
			}
			set
			{
				_source = value;
			}
		}

        /// <summary>
        /// Load styles from a file.
        /// </summary>
        /// <remarks>
        /// Throws a FileNotFoundException if the file cannot be found.
        /// 
        /// </remarks>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the style file was not found.</exception>
        /// <param name="fileName">The name of the file what is loaded.</param>
		public void LoadStylesheetFromFile(string fileName)
		{
			try
			{
				_fileName = fileName;
				StreamReader sr = new StreamReader(fileName, Encoding.ASCII);
				_source = sr.ReadToEnd();
				sr.Close();
			}
			catch
			{
				throw new FileNotFoundException("File not found");
			}
		}

		/// <summary>
		/// Parses the previously loaded document.
		/// </summary>
		public void Parse()
		{
			if (_source == null || _source.Length == 0)
			{
				throw new ArgumentNullException("Source is empty");
			}
			p.ParseStylesheet(this, _source);
		}

		/// <summary>
		/// Gets the filename from which the document was loaded.
		/// </summary>
		public string FileName
		{
			get
			{
				return _fileName;
			}
		}

        /// <summary>
        /// Returns the collection of parsed styles as style objects
        /// </summary>
        /// <remarks>
        /// Returns either <c>null</c> or old data if <see cref="Parse"/> has not been called before reading.
        /// <seealso cref="StyleObject"/>
        /// </remarks>
        public IList ParsedStyles
        {
            get { return p.ParsedStyles; }
        }

        /// <summary>
        /// Event which is fired when a new selector was found.
        /// </summary>
		public event SelectorEventHandler SelectorReady;

        /// <summary>
        /// This method fires the <see cref="SelectorReady"/> event and informs the caller
        /// in this way that a selector was found.
        /// </summary>
        /// <param name="so"></param>
		internal void OnSelectorReady(StyleObject so)
		{
			if (SelectorReady != null)
			{
                SelectorEventArgs args = new SelectorEventArgs();
                args.Name = so.SelectorName;
                args.Type = so.SelectorType;
                args.Selector = so;
                SelectorReady(this, args);
			}
		}

	}
}
