using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;


namespace GuruComponents.EditorDemo.Commands
{

    public sealed class DotNetResourceHandler
    {

        private byte[] _data;
        private uint _length;
        private int _index;
        private HtmlEditor htmlEditor;
        private bool baseDocumentLoaded;
        private Encoding _contentEncoding;

        public DotNetResourceHandler(HtmlEditor htmlEditor)
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

        public byte[] Start(string szUrl, Interop.IInternetProtocolSink protocolSink)
        {
            _length = 0;
            string mimeType;
            _index = 0;

            return _data;
        }

    }
}
