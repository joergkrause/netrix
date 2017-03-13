using System;
using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.WebEditing.Styles;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The META element provides meta-information about your page, such as descriptions and keywords for search 
    /// engines and refresh rates.
    /// </summary>
    /// <remarks>
    /// The tag always goes inside the head element.
    /// <para>
    /// Metadata is always passed as name/value pairs.
    /// </para>
    /// </remarks>
    public sealed class MetaElement : Element
    {

        [Browsable(false)]
        private new string InnerHtml
        {
            get { return String.Empty; }
        }


        /// <summary>
        /// This member overwrites the existings member to correct the PropertyGrid display.
        /// </summary>
        /// <remarks>
        /// This property cannot be used in user code. It supports the NetRix infrastructure only.
        /// </remarks>
        /// <exception cref="System.NotImplementedException">Always fired on call.</exception>
        [Browsable(false)]
        public new IEffectiveStyle EffectiveStyle
        {
            get
            {
                throw new NotImplementedException("Effective Style not available for that kind of element");
            }
        }





        [CategoryAttribute("Content")]
        [DescriptionAttribute("name_Meta")]
        [TypeConverterAttribute(typeof(UITypeConverterMetaEnum))]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DisplayNameAttribute("name_Meta")]

        public string name
        {
            get
            {
                object val = ((Interop.IHTMLMetaElement) base.GetBaseElement()).name;
                return (val == null) ? String.Empty : val.ToString();
            }

            set
            {
                ((Interop.IHTMLMetaElement) base.GetBaseElement()).name = value;
            }
        }


        [CategoryAttribute("Content")]
        [DescriptionAttribute("")]
        [TypeConverterAttribute(typeof(UITypeConverterMetaEnum))]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DisplayName()]

        public string HttpEquiv
        {
            get
            {
                object val = ((Interop.IHTMLMetaElement) base.GetBaseElement()).httpEquiv;
                return (val == null) ? String.Empty : val.ToString();
            }

            set
            {
                ((Interop.IHTMLMetaElement) base.GetBaseElement()).httpEquiv = value;
            }
        }
       

        /// <summary>
        /// Defines meta information to be associated with http-equiv or name.
        /// </summary>
        [CategoryAttribute("Content")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string Content
        {
            get
            {
                object val = ((Interop.IHTMLMetaElement) base.GetBaseElement()).content;
                return (val == null) ? String.Empty : val.ToString();
            }

            set
            {
                ((Interop.IHTMLMetaElement) base.GetBaseElement()).content = value;
            }
        }

        /// <summary>
        /// Represents the element as string.
        /// </summary>
        /// <returns>Returns the element as HTML string.</returns>
        public override string ToString()
        {
            if (name != String.Empty && HttpEquiv != null)
            {
                return String.Concat(@"<META name=""", this.name, @""">");
            } 
            if (HttpEquiv != String.Empty && name != null) 
            {
                return String.Concat(@"<META http-equiv=""", this.HttpEquiv, @""">");
            }
            return String.Concat(@"<META>");            
        }

        /// <summary>
        /// Creates the specified element.
        /// </summary>
        /// <remarks>
        /// The element is being created and attached to the current document, but nevertheless not visible,
        /// until it's being placed anywhere within the DOM. To attach an element it's possible to either
        /// use the <see cref="ElementDom"/> property of any other already placed element and refer to this
        /// DOM or use the body element (<see cref="HtmlEditor.GetBodyElement"/>) and add the element there. Also, in 
        /// case of user interactive solutions, it's possible to add an element near the current caret 
        /// position, using <see cref="HtmlEditor.CreateElementAtCaret(string)"/> method.
        /// <para>
        /// Note: Invisible elements do neither appear in the DOM nor do they get saved.
        /// </para>
        /// </remarks>
        /// <param name="editor">The editor this element belongs to.</param>
        public MetaElement(IHtmlEditor editor)
            : base("meta", editor)
        {
        }

        /// <summary>
        /// Ctor for PropertyGrid support. Do not call directly.
        /// </summary>
        /// <remarks>
        /// This ctor supports the NetRix infrastructure is not intendent to be called directly.
        /// </remarks>
        public MetaElement()
            : base("meta", null)
        {
        }

        internal MetaElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
