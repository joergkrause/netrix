using System;
using System.Windows.Forms;
using System.Drawing;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This class defines data for the ShowError event in HtmlWindow class.
    /// </summary>
    /// <remarks>
    /// This class allows to cancel the navigation by setting the cancel property to <c>true</c>.
    /// </remarks>
    public class ShowErrorEventArgs : System.ComponentModel.CancelEventArgs
    {
        private string desc;
        private string url;
        private int line;

        /// <summary>
        /// Ctor for event arguments.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="url"></param>
        /// <param name="line"></param>
        public ShowErrorEventArgs(string description, string url, int line)
        {
            this.desc = description;
            this.url = url;
            this.line = line;
        }

        /// <summary>
        /// The Url passed to the event.
        /// </summary>
        public string Url
        {
            get
            {
                return this.url;
            }
        }

        /// <summary>
        /// The Description of error
        /// </summary>
        public string Description
        {
            get
            {
                return desc;
            }
        }


        /// <summary>
        /// The line in which the error occured.
        /// </summary>
        /// <remarks>
        /// Line numbe ris relative to the Url the error comes from.
        /// </remarks>
        public int Line
        {
            get
            {
                return line;
            }
        }
    }
}