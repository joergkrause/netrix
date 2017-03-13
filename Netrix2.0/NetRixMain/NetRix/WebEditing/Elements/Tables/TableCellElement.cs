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
    /// This class represents a TD tag.
    /// </summary>
    /// <remarks>
    /// TD means table data cell. If the cell contains a header rather than data, th should be used instead. 
    /// TD must be used inside a TR element.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableRowElement">TableRowElement</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableHeaderElement">TableHeaderElement</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableElement">TableElement</seealso>
    /// </remarks>
	public class TableCellElement : StyledElement
	{

        protected internal int cellsRow, cellsColumn;


        internal TableElement baseTable;

        /// <summary>
        /// A reference to the Table that contains this Cell. 
        /// </summary>
        [Browsable(true), TypeConverter(typeof(ExpandableObjectConverter))]
        public TableElement Table
        {
            get
            {
                if (baseTable == null)
                {                    
                    baseTable = HtmlEditor.GenericElementFactory.CreateElement(((HtmlEditor) base.HtmlEditor).GetParentElement(this.GetBaseElement(), "table")) as TableElement;
                }
                return baseTable;
            }
        }

        internal TableRowElement baseRow;

        /// <summary>
        /// A reference to the Row that contains this Cell.
        /// </summary>
        [Browsable(true), TypeConverter(typeof(ExpandableObjectConverter))]
        public TableRowElement Row
        {
            get
            {
                if (baseRow == null)
                {
                    baseRow = HtmlEditor.GenericElementFactory.CreateElement(((HtmlEditor) base.HtmlEditor).GetParentElement(this.GetBaseElement(), "tr")) as TableRowElement;
                }
                return baseRow;
            }
        }

        # region Public Properties 

       /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="HorizontalAlign"]/*'/>
		[DescriptionAttribute("")]
		[DefaultValueAttribute(HorizontalAlign.Left)]
		[CategoryAttribute("Element Layout")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayNameAttribute()]

		public HorizontalAlign align
		{
			get
			{
				return (HorizontalAlign) this.GetEnumAttribute ("align", (HorizontalAlign) 0);
			} 
			set
			{
				if (value.Equals(HorizontalAlign.Justify))
				{
					this.SetEnumAttribute ("align", (HorizontalAlign) 0, (HorizontalAlign) 0);
				}
				else 
				{
					this.SetEnumAttribute ("align", value, (HorizontalAlign) 0);
				}
				return;
			} 
      
		}

        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="VerticalAlign"]/*'/>
		[DescriptionAttribute("")]
		[DefaultValueAttribute(VerticalAlign.NotSet)]
		[CategoryAttribute("Element Layout")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

		public VerticalAlign valign
		{
			get
			{
				return (VerticalAlign) this.GetEnumAttribute ("valign", (VerticalAlign) 0);
			} 
			set
			{
				this.SetEnumAttribute ("valign", value, (VerticalAlign) 0);
			} 
      
		}

        /// <summary>
        /// A background image for the cell.
        /// </summary>
        /// <remarks>
        /// The value should be relative path to an image. Remember that absolute paths don't work on a webserver.
        /// </remarks>

		[DescriptionAttribute("")]
		[CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string background
		{
            set
            {
                this.SetStringAttribute ("background", this.GetRelativeUrl(value));
            } 
            get
            {
                return this.GetRelativeUrl (this.GetStringAttribute ("background"));
            }  
		}

        /// <summary>
        /// The background color of the cell.
        /// </summary>

		[DescriptionAttribute("")]
		[DefaultValueAttribute("")]
		[CategoryAttribute("Element Layout")]
		[TypeConverterAttribute(typeof(UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UITypeEditorColor),
			 typeof(UITypeEditor))]
		[DisplayName()]

        public Color bgColor
		{
			set
			{
				this.SetColorAttribute ("bgColor", value);
			} 
			get
			{
				return this.GetColorAttribute ("bgColor");
			} 
      
		}

        /// <summary>
        /// The color of the cells border.
        /// </summary>

		[DescriptionAttribute("")]
		[DefaultValueAttribute("")]
		[CategoryAttribute("Element Layout")]
		[TypeConverterAttribute(typeof(UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UITypeEditorColor),
			 typeof(UITypeEditor))]
		[DisplayName()]

        public Color borderColor
		{
			set
			{
				this.SetColorAttribute ("borderColor", value);
			} 
			get
			{
				return this.GetColorAttribute ("borderColor");
			} 
      
		}

        /// <summary>
        /// The number of columns this cell spans.
        /// </summary>
        /// <remarks>
        /// Changing this value will destroy the table structure if the spanned
        /// column does contain cell on other rows with the wrong number of cells.
        /// </remarks>

		[DescriptionAttribute("")]
		[DefaultValueAttribute(1)]
		[CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int colSpan
		{
			get
			{
				return this.GetIntegerAttribute ("colSpan", 1);
			} 
			set
			{
				this.SetIntegerAttribute ("colSpan", value, 1);
			}       
		}

        /// <summary>
        /// The HEIGHT of the element.
        /// </summary>

		[DescriptionAttribute("")]
		[DefaultValueAttribute("")]
		[CategoryAttribute("Element Layout")]
		[EditorAttribute(
			 typeof(UITypeEditorUnit),
			 typeof(UITypeEditor))]
		[DisplayName()]

        public Unit height
		{
			set
			{
				this.SetUnitAttribute ("height", value);
			} 
			get
			{
				return this.GetUnitAttribute ("height");
			} 
      
		}		

        /// <summary>
        /// The WIDTH of the element.
        /// </summary>

		[DescriptionAttribute("")]
		[DefaultValueAttribute("")]
		[CategoryAttribute("Element Layout")]
		[EditorAttribute(
			 typeof(UITypeEditorUnit),
			 typeof(UITypeEditor))]
		[DisplayName()]

        public Unit width
		{
			set
			{
				this.SetUnitAttribute ("width", value);
			} 
			get
			{
				return this.GetUnitAttribute ("width");
			} 
      
		}

        /// <summary>
        /// Suppresses the text wrapping of the cell.
        /// </summary>                               
        /// <remarks>Setting this attribute can enhance the cell width.</remarks>

		[DescriptionAttribute("")]
		[DefaultValueAttribute(false)]
		[CategoryAttribute("Element Layout")]
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
			} 
      
		}
		

        [DescriptionAttribute("")]
		[DefaultValueAttribute("")]
		[CategoryAttribute("JavaScript Events")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnResize
		{
			set
			{
				this.SetStringAttribute ("onResize", value);
			} 
			get
			{
				return this.GetStringAttribute ("onResize");
			} 
      
		}


		[DescriptionAttribute("")]
		[DefaultValueAttribute(1)]
		[CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int rowSpan
		{
			get
			{
				return this.GetIntegerAttribute ("rowSpan", 1);
			} 
			set
			{
				this.SetIntegerAttribute ("rowSpan", value, 1);
			} 
      
		}

        # endregion

		public TableCellElement() : base("td", null)
		{
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
        public TableCellElement(IHtmlEditor editor)
            : base("td", editor)
        {
        }

		/// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="PublicElementConstructor"]'/>
        public TableCellElement(string tag) : base(tag, null)
		{
		}

		protected TableCellElement(string tag, IHtmlEditor editor) : base (tag, editor)
		{
		}

		internal TableCellElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
		}
	}
}
