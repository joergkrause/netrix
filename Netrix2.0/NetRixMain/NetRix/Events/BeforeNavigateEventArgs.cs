using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This class defines data for the <see cref="GuruComponents.Netrix.HtmlEditor.BeforeNavigate">BeforeNavigate</see> event.
    /// </summary>
    /// <remarks>
    /// This class allows to cancel the navigation by setting the cancel property to <c>true</c>.
    /// </remarks>
    public class BeforeNavigateEventArgs : CancelEventArgs
    {
        
        private string url;
        private static readonly Regex fileRegex = new Regex(@"file:/+", RegexOptions.IgnoreCase);

        internal BeforeNavigateEventArgs(string Url)
        {
            this.url = fileRegex.Replace(Url, String.Empty, 1);
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
        /// The Url passed to the event, encoded.
        /// </summary>
        public string UrlEncoded
        {
            get
            {
                return HttpUtility.UrlEncode(this.url);
            }
        }


        /// <summary>
        /// Set the translated version of the Url.
        /// </summary>
        /// <remarks>
        /// NetRix will load the source from the translated Url, 
        /// if the host application changes the value. This will be ignored if cancel is set to <c>true</c>.
        /// </remarks>
        public string UrlTranslated
        {
            set
            {
                this.url = value;
            }
        }
    }
}