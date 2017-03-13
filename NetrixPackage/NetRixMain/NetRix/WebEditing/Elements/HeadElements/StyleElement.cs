using System;
using System.ComponentModel;
using System.Diagnostics;
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
    /// The STYLE element embeds a style sheet in the document.
    /// </summary>
    /// <remarks>
    /// Any number of STYLE elements may be contained in the HEAD of a document.
    /// </remarks>
    public sealed class StyleElement : Element
    {

        private StyleRuleCollection _styleselectorcollection;

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


        /// <summary>
        /// The MEDIA attribute specifies the media on which the style sheet should be applied.
        /// </summary>
        /// <remarks>
        /// This allows authors to restrict a style sheet to certain output devices, such as printers or aural browsers. The attribute's value is a comma-separated list of media descriptors. The following media descriptors are defined in HTML 4.0 and are case-sensitive:
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
        /// To assign one of these values use the <see cref="GuruComponents.Netrix.WebEditing.Elements.LinkElementMedia">LinkElementMedia</see> enumaration constants.
        /// </remarks>

        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayNameAttribute()]

        public LinkElementMedia media
        {
            set
            {
                this.SetEnumAttribute ("media", (LinkElementMedia) value, (LinkElementMedia) 0);
                return;
            } 
      
            get
            {
                return (LinkElementMedia) base.GetEnumAttribute ("media", (LinkElementMedia) 0);
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
        /// The TYPE attribute of STYLE is used to specify the Internet media. Required.
        /// </summary>
        /// <remarks>
        /// The TYPE attribute specify the type of the style language. For Cascading Style Sheets, 
        /// the TYPE attribute value should be text/css.
        /// <para>
        /// If the property is not assigned the value "text/css" will be returned.
        /// </para>
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

        /// <summary>
        /// The embedded content of the container.
        /// </summary>
        /// <remarks>
        /// This property gives access to the content of the style container. It is recommended
        /// to provide a style editor within the host application that can access this content and
        /// the content of an external style file linked with the LINK tag.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.LinkElement">LinkElement (LINK)</seealso>
        /// </remarks>

        [Browsable(false)]
        [CategoryAttribute("Content")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorScript),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string Content
        {
            get
            {
                Interop.IHTMLStyleSheet ss = ((Interop.IHTMLStyleElement) base.GetBaseElement()).styleSheet;
                return ss.GetCssText();
            }

            set
            {
                Interop.IHTMLStyleSheet ss = ((Interop.IHTMLStyleElement) base.GetBaseElement()).styleSheet;
                ss.SetCssText( (value == null) ? String.Empty : value);
            }
        }


        [CategoryAttribute("Content")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(StyleSelectorEditor),
             typeof(UITypeEditor))]
        [DisplayName()]

        public StyleRuleCollection Selector
        {
            get
            {
                RefreshPeer(base.GetBaseElement());
				return _styleselectorcollection;
            }
        }

		private void RefreshPeer(Interop.IHTMLElement peer)
		{
			_styleselectorcollection = new StyleRuleCollection(peer);
			_styleselectorcollection.OnInsertHandler += new CollectionInsertHandler(_styleselectorcollection_OnInsertHandler);
			_styleselectorcollection.OnClearHandler += new CollectionClearHandler(_styleselectorcollection_OnClearHandler);            
		}




        private Interop.IHTMLStyleSheet BaseStylesheet
        {
            get
            {
                return ((Interop.IHTMLStyleElement) base.GetBaseElement()).styleSheet;
            }
        }

        private void _styleselectorcollection_OnInsertHandler(int index, object value)
        {
            StyleRule rule = (StyleRule) value;
            // do not add rules without a name
            if (!rule.RuleName.Equals(String.Empty) && !rule.style.Equals(String.Empty) && BaseStylesheet != null)
            {
                try
                {
                    BaseStylesheet.AddRule(rule.RuleName, rule.style, index);
                }
                catch(Exception)
                {
                }
            }
        }

        private void _styleselectorcollection_OnClearHandler()
        {
            Interop.IHTMLStyleSheetRulesCollection rules = BaseStylesheet.GetRules();
            int length = rules.GetLength();
            for(int i = 0; i < length; i++)
            {
                BaseStylesheet.RemoveRule(0);
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
        public StyleElement(IHtmlEditor editor)
            : base("style", editor)
        {
        }

        internal StyleElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
			RefreshPeer(peer);
        }

    }
}