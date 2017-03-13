using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuruComponents.Netrix.Networking;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.EditorDemo.Commands
{
    public class DotNetResourceHandlerFactory : IProtocolFactory
    {
        private HtmlEditor editor;
        internal static DotNetResourceHandler _client;

        /// <summary>
        /// Creates a loader that loads file based documents and belongs to the referenced editor.
        /// </summary>
        /// <param name="editor">The editor this handler blongs to.</param>
        public DotNetResourceHandlerFactory(IHtmlEditor editor)
        {
            this.editor = (HtmlEditor)editor;
            _client = new DotNetResourceHandler((HtmlEditor)editor);
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
            GenericProtocolHandler handler = new GenericProtocolHandler(false);
            handler.DownloadData += new GenericProtocolHandler.DownloadDataInvoker(DownloadResourceData);
            return (Interop.IInternetProtocol)handler;
        }
        #endregion

        private byte[] DownloadResourceData(string url, Interop.IInternetProtocolSink pSink)
        {
            return _client.Start(url, pSink);
        }
    }
}
