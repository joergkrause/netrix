using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;

# pragma warning disable 0618

namespace GuruComponents.Netrix.Networking
{

    /// <summary>
    /// This is the implementation of IInternetProtocol, the main class for using asynchronous pluggable
    /// protocol.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class and the implementation of IInternetProtocolSink communicate closely during
    /// load operations to resolve the local paths whereever relative paths used in src or href attributes.
    /// </para>
    /// <para>
    /// The implementation uses only Load operations, not real downloads from network, therefore some methods
    /// are not fully implemented.
    /// </para>
    /// </remarks> 
    public sealed class FileProtocolHandler // : Interop.IInternetProtocol
    {

        private byte[] _data;
        private uint _length;
        private int _index;
        private HtmlEditor htmlEditor;
        private MhtBuilder mht;
        private bool baseDocumentLoaded;
		private Encoding _contentEncoding;

        public FileProtocolHandler(HtmlEditor htmlEditor)
        {
            this.htmlEditor = htmlEditor;
            baseDocumentLoaded = false;
			_contentEncoding = htmlEditor.Encoding;
        }

		public Encoding ContentEncoding
		{
			get
			{
				return _contentEncoding;
			}
			set
			{
				_contentEncoding = value;
			}
		}

        /// <summary>
        /// Called during the start of any load sequence. If the caller does not provide a valid source, such
        /// as an empty string, a standard HTML code block will created.
        /// </summary>
        /// <param name="szUrl">Complete URL to download from, including pluggable protocol.</param>
        /// <param name="protocolSink">The IInternetProtocolSink interface, to which the loader communicates.</param>
        /// <returns>0 in case of success</returns>
        public byte[] Start(string szUrl, Interop.IInternetProtocolSink protocolSink) //, Interop.IInternetBindInfo bindInfo, uint grfPI, uint dwReserved)
        {
            _length = 0;
            szUrl = GetLocalPath(szUrl);
            string mimeType;
            _index = 0;
            if (!htmlEditor.IsFileBasedDocument)
            {
                throw new ApplicationException("Improper Call To File Loader");
            } 
            else 
            {
                BeforeResourceLoadEventArgs args = new BeforeResourceLoadEventArgs(szUrl);
                htmlEditor.OnBeforeResourceLoad(args);
                if (!args.Cancel)
                {
                    bool hasStream = false;
                    if (args.ResourceStream != null) hasStream = true;
                    if (!Directory.Exists(Path.GetDirectoryName(szUrl)) || !File.Exists(szUrl) && !hasStream)
                    {
                        _data = ContentEncoding.GetBytes(String.Concat("File not found: ", szUrl));
                        _length = (uint)_data.Length;
                    }
                    else
                    {
                        // call handler and look if the resource should be changed
                        Stream fs;
                        if (hasStream)
                        {
                            fs = args.ResourceStream;
                            fs.Seek(0, SeekOrigin.Begin);
                        }
                        else
                        {
                            fs = new FileStream(szUrl, FileMode.Open);
                        }
                        _data = (byte[])Array.CreateInstance(typeof(byte), (int)fs.Length);
                        _length = (uint)fs.Read(_data, _index, (int)fs.Length);
                        fs.Close();
                        if (htmlEditor.CanBuildMht)
                        {
                            # region MHT Builder Only
                            if (!this.baseDocumentLoaded || mht == null)
                            {
                                mht = new MhtBuilder(szUrl);
                                mht.AddQuotedString(System.Text.Encoding.GetEncoding(ContentEncoding.WebName).GetString(_data), true);
                                baseDocumentLoaded = true;
                            }
                            else
                            {
                                mht.AppendBoundary();
                                mht.AppendText(String.Format("Content-Type: {0}", GetMIMEType(szUrl, _data, _length)), true);
                                mht.AppendText("Content-Transfer-Encoding: base64", true);
                                if (htmlEditor.BaseUrl != null)
                                {
                                    string str = Path.GetDirectoryName(htmlEditor.BaseUrl);
                                    str += str.EndsWith("\\") ? String.Empty : Path.DirectorySeparatorChar.ToString();
                                    if (str != null && str.Length > 0)
                                    {
                                        Uri uri1 = new Uri(str);
                                        Uri uri2 = new Uri(szUrl);
                                        str = uri1.MakeRelative(uri2);
                                    }
                                    else
                                    {
                                        str = szUrl;
                                    }
                                    mht.AppendText(String.Format("Content-Location: {0}", str), true);
                                    mht.AppendNewLine();
                                    mht.AppendChunkBase64(_data, 76);
                                }
                            }
                            mht.AppendNewLine();
                            mht.AppendNewLine();
                            // Set the current state to the main control
                            this.htmlEditor.MhtBuilder = mht.GetStringBuilder();
                            # endregion
                        }
                    }
                }
                else
                {
                    return _data;
                }
            }
            mimeType = GetMIMEType(szUrl, _data, _length);
            // in case we want always see the content we could overwrite the mime type
            if (htmlEditor.ForceMimeType)
            {
                mimeType = "text/html";
            }
            if (!mimeType.Equals(String.Empty))
            {
                protocolSink.ReportProgress(Interop.BINDSTATUS.BINDSTATUS_VERFIEDMIMETYPEAVAILABLE | Interop.BINDSTATUS.BINDSTATUS_LOADINGMIMEHANDLER, mimeType);
                //protocolSink.ReportProgress(Interop.BINDSTATUS.BINDSTATUS_MIMETYPEAVAILABLE, mimeType);
            }
            if (mimeType.StartsWith("application") && mimeType.IndexOf("shockwave") == -1 && !(Path.GetExtension(szUrl).Equals(".js")) && !Path.GetFileName(szUrl).Equals(String.Empty) && File.Exists(szUrl))
            {
                try
                {
                    System.Diagnostics.Process.Start(szUrl);
                }
                catch
                {
                    // currently we ignore it if the call fails
                }
            } 
            return _data;
        }

        private static string GetMIMEType(string szUrl, byte[] _data, uint _length)
        {
            string mimeType = String.Empty;
            Win32.FindMimeFromData(IntPtr.Zero, szUrl, _data, (int)_length, null, 0, ref mimeType, 0);
            return mimeType;
        }

        private static string GetLocalPath(string szUrl)
        {
            szUrl = szUrl.Replace("file:///", "");
            szUrl = szUrl.Replace("file://", "");
            szUrl = System.Web.HttpUtility.UrlDecode(szUrl);
            return szUrl;
        }

    }
}