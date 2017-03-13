using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GuruComponents.CodeEditor.Library.Xml
{
    public class XmlDocumentExtended : XmlDocument
    {
        private int m_ElementCount;

        private XmlTextReader m_Reader = null;

        private string m_RealFileName;

        public string RealFileName
        {
            get { return m_RealFileName; }
        }

        public XmlDocumentExtended()
            : base()
        {
            m_ElementCount = 0;
        }

        public override XmlElement CreateElement(string prefix, string localname, string nsURI)
        {
            XmlNodeExtended elem = new XmlNodeExtended(prefix, localname, nsURI, this);

            elem.SetLineInfo(m_Reader.LineNumber, m_Reader.LinePosition);


            return elem;
        }

        public XmlNodeExtended SelectSingleNodeEx(string xpath)
        {
            return (XmlNodeExtended)this.SelectSingleNode(xpath);
        }

        public override void Load(string filename)
        {
            m_RealFileName = filename;

            m_Reader = new XmlTextReader(filename);

            base.Load(m_Reader);

            m_Reader.Close();
        }

        public override void Load(System.IO.Stream inStream)
        {
            m_Reader = new XmlTextReader(inStream);

            base.Load(m_Reader);

            m_Reader.Close();
        }

        public override void Load(System.IO.TextReader txtReader)
        {
            m_Reader = new XmlTextReader(txtReader);

            base.Load(m_Reader);

            m_Reader.Close();
        }

        public override void Load(XmlReader reader)
        {
            string xml = reader.ReadOuterXml();

            m_Reader = new XmlTextReader(xml, XmlNodeType.Document, null);

            base.Load(m_Reader);

            m_Reader.Close();
        }

        public override void LoadXml(string xml)
        {
            m_Reader = new XmlTextReader(xml, XmlNodeType.Document, null);

            base.Load(m_Reader);

            m_Reader.Close();
        }

        public void IncrementElementCount()
        {
            m_ElementCount++;
        }

        public int GetCount()
        {
            return m_ElementCount;
        }

    }
}
