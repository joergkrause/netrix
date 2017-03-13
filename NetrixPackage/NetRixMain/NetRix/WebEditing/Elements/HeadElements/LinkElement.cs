using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.WebEditing.Styles;
using DisplayNameAttribute = GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE = GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The LINK element defines document relationships. Any number of LINK elements may be contained in the HEAD of a document.
    /// </summary>
    /// <remarks>
    /// The REL and REV attributes define the nature of the relationship between the documents and the linked resource. REL defines a link relationship from the current document to the linked resource while REV defines a relationship in the opposite direction. For example,
    /// &lt;LINK REL=Glossary HREF="foo.html"&gt; indicates that foo.html is a glossary for the current document while &lt;LINK REV=Subsection HREF="bar.html"&gt; indicates that the current document is a subsection of bar.html. The value of the REL and REV attributes is a space-separated list of link types.
    /// <para>
    /// Commonly used relationships include the next or previous document in a sequence, the starting page in a collection of documents, a document with copyright information, and information about the author. A document could define these relationships as follows:
    /// 
    /// &lt;LINK REL=Prev HREF="base.html" TITLE="BASE - Document Base URI"&gt;
    /// &lt;LINK REL=Next HREF="meta.html" TITLE="META - Metadata"&gt;
    /// &lt;LINK REL=Start HREF="../" TITLE="HTML 4.0 Reference"&gt;
    /// &lt;LINK REL=Copyright HREF="/copyright.html" TITLE="Copyright Notice"&gt;
    /// &lt;LINK REV=Made HREF="mailto:joerg@krause.net" TITLE="NetRix Component"&gt;
    /// </para>
    /// <para>
    /// While the value of REL and REV is case-insensitive, the Lynx browser renders the relationship exactly as given by the author. Authors should therefore be consistent in their case, and may wish to capitalize the first letter while using lowercase for the rest. 
    /// Authors can also use the LINK element to apply an external style sheet. REL=StyleSheet specifies a persistent or preferred style while REL="Alternate StyleSheet" defines an alternate style. A persistent style is one that is always applied when style sheets are enabled. The absence of the TITLE attribute indicates a persistent style.
    /// A preferred style is one that is automatically applied. The combination of REL=StyleSheet and a TITLE attribute specifies a preferred style. Authors cannot specify more than one preferred style.
    /// An alternate style is indicated by REL="Alternate StyleSheet". The user could choose to replace the preferred style sheet with an alternate one, though current browsers generally lack the ability to choose alternate styles.
    /// A single style may also be given through multiple style sheets:
    /// 
    /// &lt;LINK REL=StyleSheet HREF="basics.css" TITLE="Contemporary" TYPE="text/css"&gt;
    /// &lt;LINK REL=StyleSheet HREF="tables.css" TITLE="Contemporary" TYPE="text/css"&gt;
    /// &lt;LINK REL=StyleSheet HREF="forms.css" TITLE="Contemporary" TYPE="text/css"&gt;
    /// </para>
    /// <para>
    /// In this example, three style sheets are combined into one "Contemporary" style that is applied as a preferred style sheet. To combine multiple style sheets into a single style, each style sheet's LINK must use the same TITLE.
    /// LINK's MEDIA attribute specifies the media for which the linked resource is designed. With REL=StyleSheet, this allows authors to restrict a style sheet to certain output devices, such as printers or aural browsers. The attribute's value is a comma-separated list of media descriptors. The following media descriptors are defined in HTML 4.0 and are case-sensitive:
    /// <list type="bullet">
    /// <item>screen (the default), for non-paged computer screens; </item>
    /// <item>tty, for fixed-pitch character grid displays (such as the display used by Lynx); </item>
    /// <item>tv, for television-type devices with low resolution and limited scrollability; </item>
    /// <item>projection, for projectors; </item>
    /// <item>handheld, for handheld devices (characterized by a small, monochrome display and limited bandwidth); </item>
    /// <item>print, for output to a printer; </item>
    /// <item>braille, for braille tactile feedback devices; </item>
    /// <item>aural, for speech synthesizers; </item>
    /// <item>all, for all devices. </item>
    /// </list>
    /// </para>
    /// Netscape Navigator 4.x incorrectly ignores any style sheet linked with a MEDIA value other than screen. For example, MEDIA="screen, projection" will cause the style sheet to be ignored by Navigator 4.x, even if the presentation device is a computer screen. Navigator 4.x also ignores style sheets declared with MEDIA=all.
    /// The optional HREFLANG and CHARSET attributes of LINK give the language and character encoding, respectively, of the link. The language should be specified according to RFC 1766; examples include en for English, en-US for American English, and ja for Japanese. Examples of character encodings include ISO-8859-1, SHIFT_JIS, and UTF-8.
    /// The Alternate link relationship defines an alternate version of the document. Translations of a page can be identified by using REL=Alternate along with the HREFLANG attribute. Versions of the page tailored for specific media can be provided by combining REL=Alternate with the MEDIA attribute. Some examples follow:
    /// Note that the LANG and DIR attributes apply to the text of the TITLE attribute, not to the content of the link.
    /// The TARGET attribute is used with frames to specify in which frame the link should be rendered. If no frame with such a name exists, the link is rendered in a new window unless overridden by the user. Special frame names begin with an underscore:
    /// In HTML 4.0, the TARGET attribute value is case-insensitive, so that _top and _TOP both have the same meaning. However, most browsers treat the TARGET attribute value as case-sensitive and do not recognize _TOP as having the special meaning of _top.
    /// </remarks>
    public sealed class LinkElement : Element
    {

        [Browsable(false)]
        private new string InnerHtml
        {
            get { return String.Empty; }
        }

        [Browsable(false)]
        private new string InnerText
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

        /// <summary>
        /// This attribute specifies the character encoding of the resource designated by the link.
        /// </summary>
        /// <remarks>
        /// Find more information about character sets in 
        /// <a href="http://www.w3.org/TR/html401/charset.html">Charset Definition</a> (External).
        /// <para>Not supported in HTML5.</para>
        /// </remarks>
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]
        [Obsolete("Not supported in HTML 5.")]
        public string charset
        {
            get
            {
                return base.GetStringAttribute("charset");
            }

            set
            {
                base.SetStringAttribute("charset", value);
            }
        }

        /// <summary>
        /// This attribute specifies the location of a Web resource, thus defining a link between the current element (the source anchor) and the destination anchor defined by this attribute.
        /// </summary>
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string href
        {
            set
            {
                styleSheetContent = null;
                this.SetStringAttribute("href", this.GetRelativeUrl(value));
            }
            get
            {
                return this.GetRelativeUrl(this.GetStringAttribute("href"));
            }
        }

        /// <summary>
        /// Specifies on what type of device the linked document or resource is optimized for.
        /// </summary>
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("Specifies on what type of device the linked document or resource is optimized for.")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]
        public LinkElementMedia media
        {
            set
            {
                this.SetEnumAttribute("media", value, (LinkElementMedia)0);
                return;
            }
            get
            {
                return (LinkElementMedia)base.GetEnumAttribute("media", (LinkElementMedia)0);
            }
        }

        /// <summary>
        /// This attribute describes the relationship from the current document to the anchor specified by the href attribute. 
        /// </summary>
        /// <remarks>
        /// The value of this attribute is a space-separated list of link types. These are some typical values:
        /// <list type="bullet">
        ///     <item>alternate</item>
        ///     <item>author</item>
        ///     <item>help</item>
        ///     <item>icon</item>
        ///     <item>licence</item>
        ///     <item>next</item>
        ///     <item>pingback</item>
        ///     <item>prefetch</item>
        ///     <item>prev</item>
        ///     <item>search</item>
        ///     <item>sidebar</item>
        ///     <item>stylesheet</item>
        ///     <item>tag</item>
        /// </list>
        /// </remarks>
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string rel
        {
            get
            {
                return base.GetStringAttribute("rel", "stylesheet");
            }

            set
            {
                base.SetStringAttribute("rel", value);
            }
        }

        /// <summary>
        /// This attribute is used to describe a reverse link from the anchor specified by the href attribute to the current document.
        /// </summary>
        /// <remarks>
        /// The value of this attribute is a space-separated list of link types.
        /// <para>
        /// Not supported in HTML 5.
        /// </para>
        /// </remarks>
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        [Obsolete("Not supported in HTML 5.")]
        public string rev
        {
            get
            {
                return base.GetStringAttribute("rev");
            }

            set
            {
                base.SetStringAttribute("rev", value);
            }
        }

        /// <include file='DocumentorIncludes.xml' path='WebEditing/Elements[@name="TargetAttribute"]'/>
        [CategoryAttribute("Element Behavior")]
        [TypeConverterAttribute(typeof(TargetConverter))]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [DisplayName()]
        [Obsolete("Not supported in HTML 5.")]
        public string target
        {
            get
            {
                return base.GetStringAttribute("target");
            }

            set
            {
                base.SetStringAttribute("target", value);
            }
        }


        /// <summary>
        /// Gets or sets the title attribute for this element.
        /// </summary>
        /// <remarks>
        /// Visible elements may render the title as a tooltip.
        /// Invisible elements may not render, but use internally to support design time environments.
        /// Some elements may neither render nor use but still accept the attribute.
        /// </remarks>
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string title
        {
            get
            {
                return base.GetStringAttribute("title");
            }

            set
            {
                base.SetStringAttribute("title", value);
            }
        }

        /// <summary>
        /// This attribute gives an advisory hint as to the content type of the content available at the link target address.
        /// </summary>
        /// <remarks>
        /// It allows user agents to opt to use a fallback mechanism rather than fetch the content if they are advised that they will get content in a content type they do not support. 
        /// Authors who use this attribute take responsibility to manage the risk that it may become inconsistent with the content available at the link target address. 
        /// For the current list of registered content types, please consult MIMETYPES section. 
        /// </remarks>
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string type
        {
            get
            {
                return base.GetStringAttribute("type", "text/css");
            }

            set
            {
                base.SetStringAttribute("type", value);
            }
        }

        public IStyleSheet styleSheetContent;

        /// <summary>
        /// Get the content of a linked stylesheet file, or <c>null</c> if there is no linked stylesheet.
        /// </summary>
        /// <remarks>
        /// Version hint: This property is new in NetRix Professional 1.7. This property is not avaialable
        /// in Advanced or Standard version.
        /// </remarks>
        /// <seealso cref="StyleSheet"/>
        [Browsable(false)]
        public IStyleSheet StyleSheetContent
        {
            get
            {
                if (styleSheetContent == null)
                {
                    if (((Interop.IHTMLLinkElement)base.GetBaseElement()).styleSheet != null)
                    {
                        styleSheetContent = new StyleSheet(((Interop.IHTMLLinkElement)base.GetBaseElement()).styleSheet, base.HtmlEditor);
                    }
                }
                return styleSheetContent;
            }
        }

        /// <summary>
        /// Textual content of the style sheet file, if link is related to a css file.
        /// </summary>
        [CategoryAttribute("Content")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorScript),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string StylesheetContent
        {
            get
            {
                if (((Interop.IHTMLLinkElement)base.GetBaseElement()).styleSheet == null)
                {
                    return String.Empty;
                }
                else
                {
                    return ((Interop.IHTMLLinkElement)base.GetBaseElement()).styleSheet.GetCssText();
                }
            }

            set
            {
                if (((Interop.IHTMLLinkElement)base.GetBaseElement()).styleSheet == null)
                {
                    return;
                }
                else
                {
                    ((Interop.IHTMLLinkElement)base.GetBaseElement()).styleSheet.SetCssText(value);
                }
            }
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
        public LinkElement(IHtmlEditor editor)
            : base("link", editor)
        {
        }

        /// <summary>
        /// Ctor for PropertyGrid support. Do not call directly.
        /// </summary>
        /// <remarks>
        /// This ctor supports the NetRix infrastructure is not intendent to be called directly.
        /// <para>By default, creates a linked stylesheet element.</para>
        /// </remarks>
        public LinkElement()
            : base("link", null)
        {
            type = "text/css";
        }

        internal LinkElement(Interop.IHTMLElement peer, IHtmlEditor editor)
            : base(peer, editor)
        {
        }
    }

}
