using System.ComponentModel;
using System.Web;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This class defines data for the GetResource event.
    /// </summary>
    /// <remarks>
    /// THIS CLASS IS EXPERIMENTAL AND IS SUBJECT TO CHANGE IN LATER VERSIONS WITHOUT FURTHER NOTICE.
    /// <para>
    /// DO NOT USE IN CUSTOM CODE!
    /// </para>
    /// </remarks>
    public class GetResourceEventArgs : CancelEventArgs
    {
        string url;
        byte[] nativeContent;

        internal GetResourceEventArgs(string Url, byte[] Content)
        {
            this.url = HttpUtility.UrlDecode(Url);
            this.nativeContent = Content;
        }

        /// <summary>
        /// The Url from which the resource should be retrieved
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
        /// The content which is loaded as the embedded resource.
        /// </summary>
        public byte[] NativeContent
        {
            get
            {
                return nativeContent;
            }
            set
            {
                nativeContent = value;
            }
        }

    }
}