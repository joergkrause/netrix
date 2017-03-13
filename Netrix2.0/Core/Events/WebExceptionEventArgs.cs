using System;
using System.Net;
using System.Web;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This class defines data for the <see cref="IHtmlEditor.WebException">WebException</see> event.
    /// </summary>
    /// <remarks>
    /// This class allows changing of URL to replace pages in case of error with some customized content. Additionally,
    /// the exception thrown internally is accessible without rethrow so the loading procedure will continue (so called 
    /// silent exception).
    /// </remarks>
    public class WebExceptionEventArgs : EventArgs
    {
        WebException wex;
        string url;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="url"></param>
        public WebExceptionEventArgs(WebException ex, string url)
        {
            this.wex = ex;
            this.url = url;
        }

        /// <summary>
        /// The inner exception thrown by URL loader.
        /// </summary>
        public WebException InnerException
        {
            get { return wex; }
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