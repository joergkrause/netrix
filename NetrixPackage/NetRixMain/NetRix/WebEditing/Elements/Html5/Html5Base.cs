using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.WebEditing.Elements;
using System.ComponentModel;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements.Html5
{
    public class Html5Base : StyledElement
    {



        /// <summary>
        /// Specifies if the element must have its spelling and grammar checked
        /// </summary>
        /// <remarks>
        /// This attribute does not has any internal meaning. It's just to support the creation of valid HTML 5 content.
        /// </remarks>
        [Category("Element Behavior")]
        [DefaultValueAttribute(false)]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DescriptionAttribute("")]
        [DisplayNameAttribute()]
        public bool spellcheck
        {
            get
            {
                return base.GetBooleanAttribute("spellcheck");
            }

            set
            {
                base.SetBooleanAttribute("spellcheck", value);
            }
        }

        /// <summary>
        /// Specifies that the element is not relevant. 
        /// </summary>
        /// <remarks>
        /// Hidden elements are not displayed.
        /// </remarks>
        [Category("Element Behavior")]
        [DefaultValueAttribute(false)]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DescriptionAttribute("")]
        [DisplayNameAttribute()]
        public bool hidden
        {
            get
            {
                return base.GetBooleanAttribute("hidden");
            }

            set
            {
                base.SetBooleanAttribute("hidden", value);
            }
        }

        public Html5Base(string element, IHtmlEditor editor)
            : base(element, editor)
        {
        }

        internal Html5Base(Interop.IHTMLElement peer, HtmlEditor editor)
            : base(peer, editor)
        {
        }
    }
}
