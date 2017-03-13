using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GuruComponents.CodeEditor.Library.Xml
{
    public  class XmlNodeExtended : XmlElement, IXmlLineInfo
    {
        int lineNumber = 0;
        int linePosition = 0;

        internal XmlNodeExtended(string prefix, string localname, string nsURI, XmlDocument doc)
            : base(prefix, localname, nsURI, doc)
        {
            ((XmlDocumentExtended)doc).IncrementElementCount();
        }

        public void SetLineInfo(int linenum, int linepos)
        {
            lineNumber = linenum;
            linePosition = linepos;
        }

   
        public int LineNumber
        {
            get
            {
                return lineNumber;
            }
        }
        public int LinePosition
        {
            get
            {
                return linePosition;
            }
        }
        public bool HasLineInfo()
        {
            return true;
        }

        public XmlNodeExtended SelectSingleNodeEx(string xpath)
        {
            return (XmlNodeExtended)this.SelectSingleNode(xpath);
        }

        public XmlDocumentExtended OwnerDocumentEx
        {
            get
            {
                if (this.OwnerDocument is XmlDocumentExtended)
                    return (XmlDocumentExtended)this.OwnerDocument;
                return null;
            }
        }
    }// End LineInfoElement class.
}
