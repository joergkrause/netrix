using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The IFRAME element defines an inline frame for the inclusion of external objects.
    /// </summary>
    /// <remarks>
    /// <para>
    /// IFRAME provides a subset of the functionality of OBJECT; the only advantage to IFRAME is that an inline frame can act as a target for other links. OBJECT is more widely supported than IFRAME, and, unlike IFRAME, OBJECT is included in HTML 4.0 Strict.
    /// </para><para>
    /// The content of the IFRAME element is used as an alternative for browsers that are not configured to show or do not support inline frames. The content may consist of inline or block-level elements, though any block-level elements must be allowed inside the containing element of IFRAME. For example, an IFRAME within an H1 cannot contain an H2, but an IFRAME within a DIV can contain any block-level elements.
    /// </para>
    /// </remarks>
	public sealed class IFrameElement : SelectableElement
	{

        /// <summary>
        /// The source of the content. Required.
        /// </summary>
        /// <remarks>
        /// IFRAME's SRC attribute provides the location of the frame content - typically a HTML document. The path can target local resources which
        /// is may be irritating to the user, because he sees the content of his local dics in the browser window. This is not a security hole, indeed, because the
        /// content cannot be used from within a server driven application.
        /// </remarks>

		[Category("Standard Values")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
		[EditorAttribute(
			 typeof(UITypeEditorUrl),
			 typeof(UITypeEditor))]
		[DisplayNameAttribute()]

        public string src
		{
            set
            {
                this.SetStringAttribute ("src", this.GetRelativeUrl(value));
            } 
            get
            {
                return this.GetRelativeUrl (this.GetStringAttribute ("src"));
            }  
		}

        /// <summary>
        /// The name of the frame. Optional.
        /// </summary>
        /// <remarks>
        /// The optional NAME attribute specifies the name of the inline frame, allowing links to target the frame.
        /// <para>
        /// Set this property to <see cref="string.Empty"/> to remove it.
        /// </para>
        /// </remarks>

        [CategoryAttribute("Standard Values")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string name
        {
            set
            {
                base.SetStringAttribute ("name", value, String.Empty);
            } 
            get
            {
                return base.GetStringAttribute ("name");
            }  
        }

        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="HorizontalAlign"]/*'/>
		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
		[DescriptionAttribute("")]
		[DisplayName()]

        public HorizontalAlign align
		{
			set
			{
				this.SetEnumAttribute ("align", value, (HorizontalAlign) 0);
				return;
			} 
      
			get
			{
				return (HorizontalAlign) this.GetEnumAttribute ("align", (HorizontalAlign) 0);
			} 
      
		}

        /// <summary>
        /// The FRAMEBORDER attribute specifies whether or not a border should be drawn. 
        /// </summary>
        /// <remarks>
        /// The default value of 1 results in a border while a value of 0 suppresses the border. 
        /// Style sheets allow greater flexibility in suggesting the border presentation. The boolean value used by the property will
        /// be converted into 0 (zero) or 1 (one) if HTML is written.
        /// </remarks>

		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute(true)]
		[DescriptionAttribute("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

		public bool frameBorder
		{
			set
			{
				this.SetAttribute ("frameBorder", (value) ? 1 : 0);
				return;
			} 
      
			get
			{
				return this.GetIntegerAttribute ("frameBorder", 1)  ==  1;
			} 
      
		}

        /// <summary>
        /// The MARGINHEIGHT attribute define the number of pixels to use as the margins.
        /// </summary>
        /// <remarks>
        /// The MARGINWIDTH and MARGINHEIGHT attributes define the number of pixels to use as the left/right margins and top/bottom margins, 
        /// respectively, within the inline frame. The value must be greater than one pixel.
        /// <para>
        /// To remove the attribute set the value to 0 (zero). It is not allowed to asign values less than 0 (zero).
        /// </para>
        /// </remarks>

		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute(0)]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

		public uint marginHeight
		{
			get
			{
				return (uint) this.GetIntegerAttribute ("marginHeight", 0);
			} 
      
			set
			{
				this.SetIntegerAttribute ("marginHeight", (int) value, 0);
			} 
      
		}

        /// <summary>
        /// The MARGINWIDTH attribute define the number of pixels to use as the margins.
        /// </summary>
        /// <remarks>
        /// The MARGINWIDTH and MARGINHEIGHT attributes define the number of pixels to use as the left/right margins and top/bottom margins, 
        /// respectively, within the inline frame. The value must be greater than one pixel.
        /// <para>
        /// To remove the attribute set the value to 0 (zero). It is not allowed to asign values less than 0 (zero).
        /// </para>
        /// </remarks>

		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute(-1)]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

		public int marginWidth
		{
			set
			{
				this.SetIntegerAttribute ("marginWidth", value, -1);
				return;
			} 
      
			get
			{
				return this.GetIntegerAttribute ("marginWidth", -1);
			} 
      
		}


		[CategoryAttribute("JavaScript Events")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnActivate
		{
			set
			{
				this.SetStringAttribute ("onActivate ", value);
				return;
			} 
      
			get
			{
				return this.GetStringAttribute ("onActivate ");
			} 
      
		}


		[CategoryAttribute("JavaScript Events")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnDeactivate
		{
			set
			{
				this.SetStringAttribute ("onDeactivate", value);
				return;
			} 
      
			get
			{
				return this.GetStringAttribute ("onDeactivate");
			} 
      
		}


		[CategoryAttribute("JavaScript Events")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnLoad
		{
			set
			{
				this.SetStringAttribute ("onLoad", value);
				return;
			} 
      
			get
			{
				return this.GetStringAttribute ("onLoad");
			} 
      
		}


		[CategoryAttribute("JavaScript Events")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnScroll
		{
			set
			{
				this.SetStringAttribute ("onScroll", value);
				return;
			} 
  
			get
			{
				return this.GetStringAttribute ("onScroll");
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
        public IFrameElement(IHtmlEditor editor)
            : base("iframe", editor)
        {
        }

        internal IFrameElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
		}

	}
}