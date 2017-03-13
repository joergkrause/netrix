using System;
using System.IO;

namespace GuruComponents.Netrix.HtmlFormatting
{

    /// <summary>
    /// Fired when formatting is done.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FinishEventHandler(object sender, FinishEventArgs e);

	/// <summary>
	/// The Event Arguments for the Finish Event, fired after finishing a html formatter task.
	/// </summary>
	/// <remarks>
	/// This event is internally fired after after each save operation (means save the html content
	/// from component to a variable.)
	/// </remarks>
	public class FinishEventArgs
	{
        private bool withErrors;
        private TextWriter tw;
        private string source;

		internal FinishEventArgs(TextWriter tw, string s, bool hasErrors)
		{
            this.tw = tw;
            this.source = s;
            this.withErrors = hasErrors;
		}

        /// <summary>
        /// The TextWriter which contains the beautified content
        /// </summary>
        public TextWriter Output
        {
            get
            {
                return tw;
            }
        }

        /// <summary>
        /// The originally used source string
        /// </summary>
        public string Source
        {
            get
            {
                return source;
            }
        }

        /// <summary>
        /// Set to true if an error occured during formatting.
        /// </summary>
        /// <remarks>
        /// Use the OnError event to get information about what kind of error
        /// occured.
        /// </remarks>
        public bool WithErrors
        {
            get
            {
                return withErrors;
            }
        }
	}
}
