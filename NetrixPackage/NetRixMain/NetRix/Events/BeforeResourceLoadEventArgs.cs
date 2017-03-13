using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This class defines data for the <see cref="GuruComponents.Netrix.HtmlEditor.BeforeResourceLoad">BeforeResourceLoad</see> event.
    /// </summary>
    /// <remarks>
    /// This class allows to cancel the loading by setting the cancel property to <c>true</c>. It also allows to change
    /// the resource on-the-fly by either providing another URL or a stream. Cancelling the event will suppress both, 
    /// stream and URL.
    /// <para>
    /// It also allows the loader to get the content from a stream instead of a file or URL, if the stream property
    /// is set to anything but <c>null</c> (<c>Nothing</c> in Visual Basic). <br/>
    /// </para>
    /// <example>
    /// Typically, an event handler reading a stream instead a file could look like this example. It assumes that the
    /// image src attribute provides a database id (a number) and the file is read into a memory stream to simulate the
    /// database access easily.
    /// <para>
    /// In a real life application one would use something like this: <c>MemoryStream ms = GetBlobFromDB(result);</c>.
    /// </para>
    /// <code>
    /// private void htmlEditor1_BeforeResourceLoad(object sender, GuruComponents.Netrix.Events.BeforeResourceLoadEventArgs e)
///{
///    int result = -1;
///    string file = Path.GetFileNameWithoutExtension(e.Url);
///    string path = Path.GetDirectoryName(Application.ExecutablePath);
///    // got a number?
///    if (Int32.TryParse(file, out result))
///    {
///        // just to simulate DB Blob and assure it's a not a file
///        MemoryStream ms = new MemoryStream();
///        FileStream fs = new FileStream(String.Format("{1}{2}{0}.jpg", result, path, Path.DirectorySeparatorChar), FileMode.Open);
///        byte[] buffer = new byte[fs.Length];
///        fs.Seek(0, SeekOrigin.Begin);
///        fs.Read(buffer, 0, buffer.Length);
///        fs.Close();
///        ms.Write(buffer, 0, buffer.Length);
///        // return a stream instead a URL
///        e.ResourceStream = ms;
///    }
///}
    /// </code>
    /// <seealso cref="ResourceStream"/>
    /// </example>
    /// </remarks>
    public class BeforeResourceLoadEventArgs : System.ComponentModel.CancelEventArgs
    {
        private string url;
        private Stream resourceStream;
        private TimeSpan timeOut;

        internal BeforeResourceLoadEventArgs(string Url)
        {
            Url = (Url.StartsWith("file://")) ? Url.Substring(8) : Url;
            Url = (Url.StartsWith("/")) ? Url.Substring(1) : Url;
            this.url = System.Web.HttpUtility.UrlDecode(Url);
            timeOut = TimeSpan.FromSeconds(5);
        }

        /// <summary>
        /// Set timeout used for this request.
        /// </summary>
        /// <remarks>
        /// Use this to reduce timeout for adserver and similar calls which are not
        /// required to be completely resolved.
        /// </remarks>
        public TimeSpan TimeOut
        {
            get
            {
                return this.timeOut;
            }
            set
            {
                this.timeOut = value;
            }
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
                return System.Web.HttpUtility.UrlEncode(this.url);
            }
        }


        /// <summary>
        /// Sets the translated version of the Url.
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

        /// <summary>
        /// Resource stream used instead of the content of the URL.
        /// </summary>
        /// <remarks>
        /// Within the event handler one might set the property to a stream and the loader will
        /// read the stream instead the URL. Set to <c>null</c> (<c>Nothing</c> in VB.NET) to assure the URL
        /// is read. This property is usually used to replace the content of objects like images from
        /// datasources other than files, for instance, accessing a database blob instead. 
        /// <para>
        /// The stream is read from position 0 regardless the current pointer position.
        /// The stream is not closed but the position remains on the last byte after reading (this behavior
        /// is like the regular Read method call is behaving).
        /// </para>
        /// </remarks>
        public Stream ResourceStream
        {
            get
            {
                return resourceStream;
            }
            set
            {
                resourceStream = value;
            }
        }
    }
}