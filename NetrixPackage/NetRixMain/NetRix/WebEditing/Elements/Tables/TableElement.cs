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
    /// This class represents a table element.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Tables are widely used to build data tables or to create a sophisticated layout. To help users
    /// using tables it is resommended to use the TableDesigner PlugIn and the 
    /// TableFormatter classes there. To access parts of the table you can use the 
    /// <see cref="GuruComponents.Netrix.WebEditing.Elements.TableCellElement">TableCellElement</see> and
    /// <see cref="GuruComponents.Netrix.WebEditing.Elements.TableRowElement">TableRowElement</see> classes.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.HtmlDocument"/>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableCellElement"/>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableRowElement"/>
    /// </remarks>
    /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="ElementInsertion"]/*'/>
    /// <example>
    /// Assuming that the user can reach the click method by clicking a button with the text "Insert Table" on it,
    /// the following code creates a new table at the current caret position and assign various formatting:
///<code>
///private void buttonTable_Click(object sender, System.EventArgs e)
///{
///  // create a table and set various options
///  TableElement t = (TableElement) this.htmlEditor1.Document.InsertTable(3, 4);
///  t.border = 2;
///  t.cellPadding = 2;
///  t.cellSpacing = 1;
///  // create a new caption and add it to the table    
///  CaptionElement c = t.TableFormatter.AddCaption();     
///  c.align = CaptionAlignment.Left;
///  c.InnerText = "This is the caption";
///}</code>
    /// <para>
    /// You may store the table element object for further reference. To manipulate the content of the table 
    /// your own way you can use the property <see cref="GuruComponents.Netrix.WebEditing.Elements.Element.InnerHtml"/>.
    /// </para>
    /// </example>
    public sealed class TableElement : SelectableElement
	{

        private CaptionElement cE = null;
        private TableRowElements rowElements = null;

        /// <summary>
        /// The underlying Element as a Interop.IHTMLTable type.
        /// </summary>
        /// <remarks>
        /// This property allows direct property access without additional casting for the caller.
        /// </remarks>
        [Browsable(false)]
        internal Interop.IHTMLTable BaseTable
        {
            get
            {
                return base.GetBaseElement() as Interop.IHTMLTable;
            }
        }

        /// <summary>
        /// Insert a new row at the given index.
        /// </summary>
        /// <exception cref="Exception">Throws an exception if insertion fails.</exception>
        /// <param name="index">Index, zero based.</param>
        public TableRowElement InsertRow(int index)
        {
            try
            {
                object newRow = ((Interop.IHTMLTable)GetBaseElement()).insertRow(index);
                if (newRow is Interop.IHTMLTableRow)
                {
                    return base.HtmlEditor.GenericElementFactory.CreateElement(newRow as Interop.IHTMLElement) as TableRowElement;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete a new row at the given index.
        /// </summary>
        /// <exception cref="Exception">Throws an exception if deletion fails.</exception>
        /// <param name="index">Index, zero based.</param>
        public void DeleteRow(int index)
        {
            ((Interop.IHTMLTable)GetBaseElement()).deleteRow(index);
        }

        /// <summary>
        /// Create the TFOOT element.
        /// </summary>
        public void CreateTableFoot()
        {
            ((Interop.IHTMLTable)GetBaseElement()).createTFoot();
        }

        /// <summary>
        /// Create the THEAD element.
        /// </summary>
        public void CreateTableHead()
        {
            ((Interop.IHTMLTable)GetBaseElement()).createTHead();
        }

        /// <summary>
        /// Remove the TFOOT element.
        /// </summary>
        public void DeleteTableFoot()
        {
            ((Interop.IHTMLTable)GetBaseElement()).deleteTFoot();
        }

        /// <summary>
        /// Remove the THEAD element.
        /// </summary>
        public void DeleteTableHead()
        {
            ((Interop.IHTMLTable)GetBaseElement()).deleteTHead();
        }

        /// <summary>
        /// Remove the CAPTION element.
        /// </summary>
        /// <seealso cref="Caption"/>
        public void DeleteCaption()
        {
            ((Interop.IHTMLTable)GetBaseElement()).deleteCaption();
        }

        /// <summary>
        /// Current TFOOT element, if present.
        /// </summary>
        [DescriptionAttribute("Current TFOOT element, if present.")]
        [CategoryAttribute("Element Layout")]
        [Browsable(true)]
        [TypeConverter(typeof(ExpandCaptionConverter))]
        public TableFootElement TableFoot
        {
            get
            {
                if (((Interop.IHTMLTable)GetBaseElement()).tFoot != null)
                {
                    return HtmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement)((Interop.IHTMLTable)GetBaseElement()).tFoot) as TableFootElement;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Current THEAD element, if present.
        /// </summary>
        [DescriptionAttribute("Current THEAD element, if present.")]
        [CategoryAttribute("Element Layout")]
        [Browsable(true)]
        [TypeConverter(typeof(ExpandCaptionConverter))]
        public TableHeadElement TableHead
        {
            get
            {
                if (((Interop.IHTMLTable)GetBaseElement()).tHead != null)
                {
                    return HtmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement)((Interop.IHTMLTable)GetBaseElement()).tFoot) as TableHeadElement;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Current TBODY elements, if present.
        /// </summary>
        /// <remarks>
        /// If there is no body the property returns an empty collection.
        /// </remarks>
        [DescriptionAttribute("Current TFOOT element, if present.")]
        [CategoryAttribute("Element Layout")]
        [Browsable(true)]
        [TypeConverter(typeof(ExpandCaptionConverter))]
        public ElementCollection TableBodies
        {
            get
            {
                if (((Interop.IHTMLTable)GetBaseElement()).tBodies != null)
                {
                    ElementCollection ec = new ElementCollection();
                    Interop.IHTMLElementCollection iec = ((Interop.IHTMLTable)GetBaseElement()).tBodies;
                    for (int i = 0; i < iec.GetLength(); i++)
                    {
                        ec.Add(HtmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement)iec.Item(i, i)));
                    }                    
                    return ec;
                }
                else
                {
                    return null;
                }
            }
        }

        # region Public Properties

        /// <summary>
        /// Gets the collection of rows.
        /// </summary>
        /// <remarks>
        /// The purpose of this class is to provide access to the rows to assign formatting.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableRowElements"/>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableRowElement"/>
        /// </remarks>
        [Browsable(false)]
        public TableRowElements RowElements
        {
            get
            {
                Interop.IHTMLTable baseTable = this.BaseTable;
                if (baseTable != null && baseTable.rows != null && rowElements == null || rowElements.Count != baseTable.rows.GetLength())
                {
                    rowElements = new TableRowElements();
                    for (int r = 0; r < baseTable.rows.GetLength(); r++)
                    {
                        TableRowElement tre = (TableRowElement)HtmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement)baseTable.rows.Item(r, r));
                        tre.baseTable = this;
                        rowElements.Add(tre);
                    }
                    // make the collection later internactively by looking for internal events
                    rowElements.OnClearHandler += new CollectionClearHandler(rowElements_OnClearHandler);
                    rowElements.OnInsertHandler += new CollectionInsertHandler(rowElements_OnInsertHandler);
                }                 
                return rowElements;
            }
        }

        private void rowElements_OnClearHandler()
        {
            while (BaseTable.rows.GetLength() > 0)
            {
                BaseTable.deleteRow(0);
            }
        }

        private void rowElements_OnInsertHandler(int index, object value)
        {
            Interop.IHTMLTableRow row = BaseTable.insertRow(index) as Interop.IHTMLTableRow;
            if (row != null)
            {
                row.bgColor         = ColorTranslator.ToHtml(((TableRowElement) value).bgColor);
                row.borderColor     = ColorTranslator.ToHtml(((TableRowElement) value).borderColor);
                row.vAlign          = ((TableRowElement) value).GetAttribute("valign") == null ? String.Empty : ((TableRowElement) value).GetAttribute("valign").ToString();
                row.align           = ((TableRowElement) value).GetAttribute("align") == null ? String.Empty : ((TableRowElement) value).GetAttribute("align").ToString();
            }
        }

        /// <summary>
        /// Gets the caption for the table.
        /// </summary>
        /// <remarks>
        /// The purpose of this property is to get the currently assigned caption. The property will
        /// return <c>null</c> (<c>Nothing</c> in Visual Basic) if there is no caption assigned. To 
        /// create a new caption you can use the TableFormatter class
        /// and the AddCaption command of TableDesigner PlugIn.
        /// <para>
        /// If you want to create a caption and there is no need to assign properties immediately you can use
        /// the <see cref="WithCaption"/> property, set to <c>true</c>, to create a empty caption element. Later you 
        /// can reference to that caption by using the Caption property and manipulate the content or behavior.
        /// Setting the <see cref="WithCaption"/> property to <c>false</c> will remove the caption from table.
        /// </para>
        /// </remarks>
        /// <example>
        /// Assuming that the user can reach the click method by clicking a button with the text "Insert Caption" on it,
        /// the following code creates a new caption:
        /// <code>
        /// //
        /// // t is a TableElement object assigned earlier in the program
        /// //
        /// private void buttonCaption_Click(object sender, System.EventArgs e)
        /// {
        ///     // create a new caption and add it to the table    
        ///     CaptionElement c = t.TableFormatter.AddCaption();     
        ///     c.align = CaptionAlignment.Left;
        ///     c.InnerText = "This is the caption";
        /// }
        /// </code>
        /// You may store the table element object for further reference. To manipulate the content of the table 
        /// your own way you can use the property <see cref="GuruComponents.Netrix.WebEditing.Elements.Element.InnerHtml">InnerHtml</see>.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.CaptionElement">CaptionElement</seealso>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.CaptionAlignment">CaptionAlignment</seealso>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableElement">TableElement</seealso>
        /// </example>
        [DescriptionAttribute("")]
        [CategoryAttribute("Element Layout")]
        [Browsable(true)]
        [TypeConverter(typeof(ExpandCaptionConverter))]
        [DisplayNameAttribute()]
        public CaptionElement Caption
        {
            get
            {
                return cE;
            }
        }

        /// <summary>
        /// Creates or removes a caption.
        /// </summary>
        /// <remarks>
        /// You can use
        /// the <see cref="WithCaption"/> property, set to <c>true</c>, to create a empty caption element. Later you 
        /// can reference to that caption by using the Caption property and manipulate the content or behavior.
        /// Setting the <see cref="WithCaption"/> property to <c>false</c> will remove the caption from table.
        /// <para>
        /// If the caption is set once the value becomes persistant. Setting the value to <c>true</c> again will
        /// not create a new caption or replace the existing one. To remove the old caption and create a new one it
        /// is necessary to call the property twice (see example).
        /// </para>
        /// </remarks>
        /// <example>
        /// This example shows how to use this property to replace an old caption with a new one:
        /// <code>
        /// // t is a table object
        /// t.WithCaption = false;                 // remove old caption (destroyes the object internally)
        /// t.WithCaption = true;                  // create new caption (only works if there is no caption assigned)
        /// CaptionElement ce = t.Caption;         // get the new caption from table object
        /// ce.InnerText = "I'm the new caption";  // assign the content of the caption
        /// </code>
        /// Instead of using this property it is possible to use the 
        /// TableFormatter class and the TableDesigner's AddCaption and RemoveCaption command, 
        /// which is a more logical way. The intention to provide a property is the support of the PropertyGrid, which is part of the
        /// integrated NetRix user interface.
        /// </example>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(false)]
        [DescriptionAttribute("")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]
        public bool WithCaption
        {
            get
            {                
                if (cE == null && !ForceCaptionRemoving)
                {
                    // if not exists, look for a previously set or loaded caption
                    Interop.IHTMLTableCaption nativeCE = this.BaseTable.caption;
                    if (nativeCE != null)
                    {                        
                        // first call to property, create object to synchronize it
                        cE = (CaptionElement) HtmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) nativeCE);
                    } 
                    else 
                    {
                        cE = null;
                    }
                } 
                if (cE != null && ForceCaptionRemoving)
                {
                    this.BaseTable.deleteCaption();
                    cE = null;
                }
                return (cE == null) ? false : true;
            }

            set
            {
                if (value)
                {
                    if (cE == null)
                    {
                        Interop.IHTMLTableCaption nativeCE = this.BaseTable.caption;
                        if (nativeCE == null)
                        {                        
                            nativeCE = this.BaseTable.createCaption();                            
                        }
                        cE = (CaptionElement) HtmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) nativeCE);
                        ForceCaptionRemoving = false;
                    }
                } 
                else 
                {
                    try
                    {
                        // delete forces removing of complete caption                    
                        // ((Interop.IHTMLDOMNode) this.BaseTable).removeChild((Interop.IHTMLDOMNode) this.BaseTable.caption);
                        // cE = null;
                        ForceCaptionRemoving = true;
                    }
                    catch
                    {
                    }
                }
            }
        }

        private bool ForceCaptionRemoving = false;

        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="HorizontalAlign"]/*'/>
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Layout")]
        [Browsable(true)]
        [TypeConverter(typeof(UITypeConverterDropList))]
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


		[DescriptionAttribute("")]
		[DefaultValueAttribute("")]
		[CategoryAttribute("Element Layout")]
		[Browsable(true)]
		[DisplayName()]
        public int rows
		{
			get
			{
				return this.BaseTable.rows.GetLength();
			}
		}


		[DescriptionAttribute("")]
		[DefaultValueAttribute("")]
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
                return;
            } 
            get
            {
                return this.GetRelativeUrl (this.GetStringAttribute ("background"));
            }  
		}
	

        /// <summary>
        /// The background color of the whole table.>
        /// </summary>
        /// <remarks>
        /// Row, Col, and Cell definitions may overwrite this value.
        /// </remarks>
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
			get
			{
				return this.GetColorAttribute ("bgColor");
			} 
      
			set
			{
				this.SetColorAttribute ("bgColor", value);
				return;
			} 
      
		}


		[DescriptionAttribute("")]
		[DefaultValueAttribute(1)]
		[CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]
        public int border
		{
			set
			{
				this.SetIntegerAttribute ("border", value, 0);
				return;
			} 
      
			get
			{
				return this.GetIntegerAttribute ("border", 0);
			} 
      
		}


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
			get
			{
				return this.GetColorAttribute ("borderColor");
			} 
      
			set
			{
				this.SetColorAttribute ("borderColor", value);
			} 
      
		}


		[DescriptionAttribute("")]
		[DefaultValueAttribute(1)]
		[CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int cellPadding
		{
			get
			{
				return this.GetIntegerAttribute ("cellpadding", 1);
			} 
      
			set
			{
				this.SetIntegerAttribute ("cellPadding", value, 1);
			} 
      
		}


		[DescriptionAttribute("")]
		[DefaultValueAttribute(1)]
		[CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int cellSpacing
		{
			set
			{
				this.SetIntegerAttribute ("cellSpacing", value, 1);
			} 
			get
			{
				return this.GetIntegerAttribute ("cellSpacing", 1);
			} 
      
		}


		[DescriptionAttribute("")]
		[DefaultValueAttribute("")]
		[CategoryAttribute("Element Layout")]
		[EditorAttribute(
			 typeof(UITypeEditorUnit),
			 typeof(UITypeEditor))]
		[DisplayName()]
        public Unit height
		{
			get
			{
				return this.GetUnitAttribute ("height");
			} 
      
			set
			{
				this.SetUnitAttribute ("height", value);
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
			get
			{
				return this.GetStringAttribute ("onResize");
			} 
  
			set
			{
				this.SetStringAttribute ("onResize", value);
				return;
			} 
  
		}


        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Layout")]
        [Browsable(true)]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

        public RulesType rules
		{
			set
			{
				this.SetEnumAttribute ("rules", value, RulesType.NotSet);
				return;
			} 
  
			get
			{
				return (RulesType) this.GetEnumAttribute ("rules", RulesType.NotSet);
			} 
  
		}

        /// <summary>
        /// Width of whole table.
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
			get
			{
				return this.GetUnitAttribute ("width");
			} 
  
			set
			{
				this.SetUnitAttribute ("width", value);
				return;
			} 
  
		}

        #endregion

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
        public TableElement(IHtmlEditor editor)
            : base("table", editor)
        {
        }

		internal TableElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
        }
	}
}