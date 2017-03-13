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
    /// The P element defines a paragraph.
    /// </summary>
    /// <remarks>
    /// The closing tag for P is optional, but its use prevents common browser bugs with style sheets. Note that P cannot contain block-level elements such as TABLE and ADDRESS.
    /// </remarks>
    public sealed class ParagraphElement : StyledElement
	{
		/// <summary>
		/// The ALIGN attribute.
		/// </summary>
		/// <remarks>
		/// This attribute is deprecated in HTML 4.0. Use styles instead.
		/// </remarks>
		[Description("")]
		[DefaultValueAttribute(HorizontalAlign.NotSet)]
		[CategoryAttribute("Element Layout")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayNameAttribute()]
        public HorizontalAlign align
		{
			set
			{
				this.SetEnumAttribute ("align", value, (HorizontalAlign) 0);
			} 
			get
			{
				return (HorizontalAlign) this.GetEnumAttribute ("align", (HorizontalAlign) 0);
			} 
      
		}

        /// <summary>
        /// Disables the element.
        /// </summary>
        /// <remarks>
        /// The most elements are drawn grayed or with a gray shadow to inform the user that the element is disabled.
        /// Field elements do not except the focus and do not display the caret.
        /// <para>
        /// In HTML the element is disables if the attribute exists and it is enabled if it doesn't exist. The parameter assigned
        /// to the attribute is useless.
        /// </para>
        /// </remarks>
		[DescriptionAttribute("")]
		[DefaultValueAttribute(false)]
		[CategoryAttribute("Element Behavior")]
		[TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]
        public bool disabled
		{
			set
			{
				this.SetBooleanAttribute ("disabled", value);
			} 
			get
			{
				return this.GetBooleanAttribute ("disabled");
			} 
      
		}

        /// <summary>
        /// Gets or set the TAB index.
        /// </summary>
        /// <remarks>
        /// If the user hits the TAB key the focus moves from block element to block element, beginning with the first element on a page.
        /// To change the default TAB chain the tabIndex can be set to any numeric (integer) value which force the order of the TAB key.
        /// The value must be greater or equal than 0 (zero).
        /// <para>
        /// To remove the attribute set the value to 0 (zero).
        /// </para>
        /// </remarks>

		[DescriptionAttribute("")]
		[DefaultValueAttribute(0)]
		[CategoryAttribute("Element Behavior")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]
        public short tabIndex
		{
			get
			{
				return (short) this.GetIntegerAttribute ("tabIndex", 0);
			} 
			set
			{
				if (value  <  -1)
				{
					throw new ArgumentOutOfRangeException ("value");
				}
				this.SetIntegerAttribute ("tabIndex", value, 0);
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
        public ParagraphElement(IHtmlEditor editor)
            : base("p", editor)
        {
        }

		internal ParagraphElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
		}
	}
}