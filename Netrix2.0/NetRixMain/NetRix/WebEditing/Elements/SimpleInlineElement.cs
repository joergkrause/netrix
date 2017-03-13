using System;
using System.ComponentModel;
using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;

using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{    
    /// <summary>
    /// A base class for simple inline elements.
    /// </summary>
    /// <remarks>
    /// This class could not be used to instantiate HTML elements directly.
    /// </remarks>
    public abstract class SimpleInlineElement : Element
    {

        /// <summary>
        /// The CLASS attribute.
        /// </summary>
        /// <remarks>
        /// This attribute is used to assign style classes from a style sheet (CSS).
        /// </remarks>
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [CategoryAttribute("Element Style")]
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterDropSelection))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]
        public string @class
        {
            get
            {
                return base.GetBaseElement().GetClassName();
            }

            set
            {
                base.GetBaseElement().SetClassName(value);
            }
        }

        /// <summary>
        /// The ID attribute.
        /// </summary>
        /// <remarks>
        /// This attribute defines an unique ID to the element. It can be used to assign styles from a style sheet (CSS) or
        /// to access the element using Scripting.
        /// </remarks>
        [MergablePropertyAttribute(false)]
        [DescriptionAttribute("")]
        [ParenthesizePropertyNameAttribute(true)]
        [CategoryAttribute("Element Style")]
        [DefaultValueAttribute("")]
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterDropSelection))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]
        public string id
        {
            get
            {
                return base.GetBaseElement().GetId();
            }
            set
            {
                base.GetBaseElement().SetId(value);
            }
        }

        /// <summary>
        /// Defines the STYLE attribute.
        /// </summary>
        /// <remarks>
        /// The STYLE attribute can contain inline style definitions. If the NetRix UI is used (not supported by Light Version) the integrated
        /// style editor of the UI assembly can be used to create the styles.
        /// </remarks>

        [CategoryAttribute("Element Style")]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [TypeConverterAttribute(typeof(string))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorStyleStyle),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]
        public string style
        {
            get
            {
                Interop.IHTMLStyle s = base.GetBaseElement().GetStyle() as Interop.IHTMLStyle;
                if (s == null)
                    return System.String.Empty;
                else
                    return s.GetCssText();
            }
            set
            {
                base.GetBaseElement().GetStyle().SetCssText(value);
            }
        }

        internal SimpleInlineElement(string newTag, IHtmlEditor editor) : base(newTag, editor)
        {
        }

        internal SimpleInlineElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}