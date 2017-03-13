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
    /// The DIV element, a generic block element container.
    /// </summary>
    /// <remarks>
    /// The DIV element is often used to define a paragraph and assign various styles to layout the paragraph.
    /// </remarks>
    public sealed class DivElement : StyledElement
	{
        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="HorizontalAlign"]/*'/>
        [Category("Element Layout")]
        [DefaultValue("")]
        [Description("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayNameAttribute()]

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

		[CategoryAttribute("Element Behavior")]
		[DefaultValue(false)]
		[Description("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
		[DisplayName()]

        public bool disabled
		{
			get
			{
				return this.GetBooleanAttribute ("disabled");
			} 
      
			set
			{
				this.SetBooleanAttribute ("disabled", value);
				return;
			} 
      
		}


		[CategoryAttribute("Element Layout")]
		[DefaultValue("")]
		[Description("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
		[DisplayName()]

        public bool noWrap
		{
			get
			{
				return this.GetBooleanAttribute ("noWrap");
			} 
      
			set
			{
				this.SetBooleanAttribute ("noWrap", value);
				return;
			} 
      
		}


		[CategoryAttribute("JavaScript Events")]
		[DefaultValue("")]
		[Description("")]
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
        public DivElement(IHtmlEditor editor)
            : base("div", editor)
        {
        }

		internal DivElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
            
		} 
	}
}
