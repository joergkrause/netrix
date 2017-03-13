using System;

namespace GuruComponents.Netrix.XmlDesigner
{
    /// <summary>
    /// Decorates an XML element to control the formatting behavior.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class XmlElementAttribute : Attribute
    {

        private string name;
        private string alias;
        private string ns;

        /// <summary>
        /// Ctor
        /// </summary>
        public XmlElementAttribute(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public XmlElementAttribute(string alias, string name)
        {
            this.name = name;
            this.alias = alias;
        }

        /// <summary>
        /// Namespace alias
        /// </summary>
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        /// <summary>
        /// Element name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Namespace
        /// </summary>
        public string Namespace
        {
            get { return ns; }
            set { ns = value; }
        }

    }
}
