using System;
using GuruComponents.Netrix.HtmlFormatting.Elements;

namespace GuruComponents.Netrix.XmlDesigner
{
    /// <summary>
    /// Decorates an XML element to control the formatting behavior.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class FormattingAttribute : Attribute
    {

        private FormattingFlags formatting;

        /// <summary>
        /// Ctor
        /// </summary>
        public FormattingAttribute()
        {
            this.formatting = FormattingFlags.Xml;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="formFlag"></param>
        public FormattingAttribute(FormattingFlags formFlag)
        {
            this.formatting = formFlag;
        }

        /// <summary>
        /// Formatting type.
        /// </summary>
        public FormattingFlags Formatting
        {
            get { return formatting; }
            set { formatting = value; }
        }


    }
}
