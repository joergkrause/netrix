using System;
using System.ComponentModel;
using System.Drawing;
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
    /// The HR element defines a horizontal rule for visual browsers.
    /// </summary>
    /// <remarks>
    /// While this element is inherently presentational, it can be used structurally as a section divider. However, for greater flexibility the HR element can be replaced with the border-bottom or border-top properties of Cascading Style Sheets. For example, the following style rule would suggest a horizontal line above all DIV elements with CLASS=navbar.
    /// </remarks>
    public sealed class HorizontalRuleElement : SelectableElement
    {

        /// <summary>
        /// Gets or sets the style string for this element.
        /// </summary>
        /// <remarks>
        /// To replace the deprecated properties use this option to set the styles.
        /// </remarks>

        [Category("Style")]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [TypeConverterAttribute(typeof(string))]        
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorAttribute(
             typeof(UITypeEditorStyleStyle),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public new string style
        {
            get
            {
                return base.GetBaseElement().GetStyle().GetCssText();
            }
            set
            {
                base.GetBaseElement().GetStyle().SetCssText(value);
                if (base.GetStyleAttribute("width") != null && !base.GetStyleAttribute("width").Equals(String.Empty))
                {
                    Unit widthUnit = Unit.Parse(base.GetStyleAttribute("width"));
                    this.width = widthUnit;
                }
                if (base.GetStyleAttribute("height") != null && !base.GetStyleAttribute("height").Equals(String.Empty))
                {
                    Unit sizeUnit = Unit.Parse(base.GetStyleAttribute("height"));
                    this.size = Convert.ToInt32(sizeUnit.Value);
                }
            }
        }

        /// <summary>
        /// The WIDTH attribute specifies the width of the line as a percentage or a number of pixels.
        /// </summary>
        /// <remarks>
        /// These attribute is deprecated in favor of style sheets. If a width is specified, percentages are 
        /// generally preferred since they adjust to varying window sizes. The width property of Cascading 
        /// Style Sheets provides greater flexibility in suggesting the width of horizontal rules.
        /// </remarks>
        [DescriptionAttribute("w")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorUnit),
             typeof(UITypeEditor))]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DisplayName()]
		public Unit width
		{
			set
			{
				this.SetUnitAttribute ("width", value);
                this.RemoveStyleAttribute("width");
				return;
			} 
            get
            {
                return this.GetUnitAttribute ("width");
            }   
		}

        /// <summary>
        /// Horizontal alignment (left, center, right) within the surrounding block element.
        /// </summary>
        /// <remarks>
        /// This attributes allows the horizontal alignment within the surrounding block element. The options
        /// and there behavior differs slightly between the elements. The property allows the list provided 
        /// by the <see cref="System.Web.UI.WebControls.HorizontalAlign">HorizontalAlign</see> enumeration.
        /// </remarks>

        [CategoryAttribute("Element Layout")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
		[DisplayName()]
        public HorizontalAlign align
		{
			set
			{
				this.SetEnumAttribute ("align", (HorizontalAlign) value, (HorizontalAlign) 0);
				return;
			} 
			get
			{
				return (HorizontalAlign) this.GetEnumAttribute ("align", (HorizontalAlign) 0);
			} 
      
		}
	
        /// <summary>
        /// The COLOR of the rule.
        /// </summary>
        /// <remarks>
        /// These attribute is deprecated in favor of style sheets.
        /// </remarks>

		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
		[TypeConverterAttribute(typeof(UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UITypeEditorColor),
			 typeof(UITypeEditor))]
		[DisplayName()]
        public Color color
		{
			set
			{
				this.SetColorAttribute ("color", value);
				return;
			} 
      
			get
			{
				return this.GetColorAttribute ("color");
			} 
      
		}

        /// <summary>
        /// The boolean NOSHADE attribute suggests that the rule be rendered as a solid line rather than the groove style commonly used.
        /// </summary>
        /// <remarks>
        /// These attribute is deprecated in favor of style sheets.
        /// </remarks>

		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute(false)]
		[DescriptionAttribute("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
		[DisplayName()]

        public bool noShade
		{
			get
			{
				return this.GetBooleanAttribute ("noShade");
			} 
      
			set
			{
				this.SetBooleanAttribute ("noShade", value);
				return;
			} 
      
		}

        /// <summary>
        /// The SIZE attribute suggests the height of the line in pixels.
        /// </summary>
        /// <remarks>
        /// These attribute is deprecated in favor of style sheets.
        /// </remarks>

		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute(1)]
		[DescriptionAttribute("size_Hr")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
		[DisplayName("size_Hr")]

        public int size
		{
			set
			{
				this.SetIntegerAttribute ("size", value, 1);
                this.RemoveStyleAttribute("height");
				return;
			} 
      
			get
			{
				return this.GetIntegerAttribute ("size", 1);
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
        public HorizontalRuleElement(IHtmlEditor editor)
            : base("hr", editor)
        {
        }

		internal HorizontalRuleElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
		} 
	}
}