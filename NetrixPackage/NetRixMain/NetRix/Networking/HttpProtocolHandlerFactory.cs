using System;
using System.IO;
using System.Net;
using System.Threading;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
#if !DOTNET20

#endif

namespace GuruComponents.Netrix.Networking
{

    /// <summary>
    /// HttpProtocolHandlerFactory class. This class implements IProtocolFactory interface.
    /// </summary>
    /// <remarks>
    /// The purpose is a custom loader mechanism. It allows the control to intercept the loading procedure and
    /// change the content while reading bytes.
    /// </remarks>
    public sealed class HttpProtocolHandlerFactory : IProtocolFactory
    {

        private HtmlEditor editor;

        public HttpProtocolHandlerFactory(IHtmlEditor editor)
        {
            this.editor = (HtmlEditor) editor;
        }

        #region IProtocolFactory Members
        
        /// <summary>
        /// Gets IInternetProtocol interface.
        /// </summary>
        /// <remarks>
        /// IInternetProtocol is the main interface exposed by an asynchronous pluggable protocol. 
        /// This interface and the IInternetProtocolSink interface communicate with 
        /// each other very closely during download operations.
        /// </remarks>
        /// <returns>Returns IIneternetProtocol handler.</returns>
        public Interop.IInternetProtocol GetIInternetProtocol()
        {
            GenericProtocolHandler handler = new GenericProtocolHandler(true); // IInternetProtocol
            handler.DownloadData += new GenericProtocolHandler.DownloadDataInvoker(DownloadHttpData);
            return (Interop.IInternetProtocol) handler;
        }
        #endregion

        ManualResetEvent mre;
        byte[] s;
        string url;

        internal byte[] DownloadHttpData(string rawurl, Interop.IInternetProtocolSink pSink)
        {
            BeforeResourceLoadEventArgs args = new BeforeResourceLoadEventArgs(rawurl);
            editor.OnBeforeResourceLoad(args);
            if (!args.Cancel)
            {
                try
                {
                    this.url = args.Url;
                    if (args.ResourceStream == null)
                    {
                        mre = new ManualResetEvent(false);
                        int timeOut = Convert.ToInt32(args.TimeOut.TotalMilliseconds);
                        SendRequest(timeOut);
                        mre.WaitOne(timeOut, true);
                    }
                    else
                    {
                        args.ResourceStream.Seek(0, SeekOrigin.Begin);
                        s = new byte[args.ResourceStream.Length];
                        args.ResourceStream.Read(s, 0, s.Length);
                    }
                    return s;
                }
                catch (WebException wex)
                {
                    editor.OnWebException(wex, ref this.url);
                    System.Diagnostics.Debug.WriteLine(wex.Message);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return null;
        }

        private void SendRequest(int timeOut)
        {
            HttpRequestResponse Response = new HttpRequestResponse("", url);
            Response.WebError += new EventHandler<GuruComponents.Netrix.Events.WebExceptionEventArgs>(Response_WebError);
            Response.HttpUserName = this.editor.UserName;
            Response.HttpUserPassword = this.editor.Password;
            Response.UserAgent = this.editor.UserAgent; 
            if (editor.Proxy != null && editor.Proxy.Address != null)
            {
                Response.ProxyServer = editor.Proxy.Address.Host;
                Response.ProxyPort = editor.Proxy.Address.Port;
                Response.ProxyCredentials = editor.Proxy.Credentials;
            }
            s = Response.SendRequest(editor, timeOut);
            mre.Set();
        }

        void Response_WebError(object sender, GuruComponents.Netrix.Events.WebExceptionEventArgs e)
        {
            string url = e.Url;
            editor.OnWebException(e.InnerException, ref url);
            e.UrlTranslated = url;
        }

    }
}