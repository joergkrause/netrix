using System;
using System.Net;

using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Networking
{
    /// <summary>
    /// Creates a protocol handler to handle file based loading of documents.
    /// </summary>
	public sealed class FileProtocolHandlerFactory : IProtocolFactory
	{

		private HtmlEditor editor;
		internal static FileProtocolHandler _client;

        /// <summary>
        /// Creates a loader that loads file based documents and belongs to the referenced editor.
        /// </summary>
        /// <param name="editor">The editor this handler blongs to.</param>
		public FileProtocolHandlerFactory(IHtmlEditor editor)
		{
			this.editor = (HtmlEditor)editor;
			_client = new FileProtocolHandler((HtmlEditor)editor);
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
			GenericProtocolHandler handler=new GenericProtocolHandler(false);
			handler.DownloadData += new GenericProtocolHandler.DownloadDataInvoker(DownloadHttpData);
			return (Interop.IInternetProtocol) handler;
		}
		#endregion

		private byte[] DownloadHttpData(string url, Interop.IInternetProtocolSink pSink)
		{
            if (AllowDownloadDelegate == null || AllowDownloadDelegate(url) == true)
            {

                return _client.Start(url, pSink);
            }
            else
            {
                return null;
            }
		}

        /// <summary>
        /// A delegate which is used to invoke the private handler once a resource download is required.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
		public delegate bool DownloadControllerDelegate(string url);

        /// <summary>
        /// Allows an external method to handle downloads instead of the internal procedure. 
        /// </summary>
		public static DownloadControllerDelegate AllowDownloadDelegate;
	}
}
