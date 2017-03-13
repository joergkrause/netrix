using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;
using System;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// This class represents a table row. 
    /// </summary>
    /// <remarks>
    /// Each TR element contains one or more TD or TH elements. These elements are build using the formatter options of
    /// the TableFormatter class, provided by the TableDesigner PlugIn. See <see cref="GuruComponents.Netrix.WebEditing.Elements.TableCellElement">TableCellElement</see> for
    /// information about dealing with the cells of a row.
    /// <para>
    /// To access the cells of the row object you can use the <see cref="CellElements"/> property. 
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableRowElements"/>
    /// </remarks>
    public sealed class TableRowElement : StyledElement
    {

        private TableCellElements cellElements;

        internal TableElement baseTable;

        /// <summary>
        /// A reference to the Table that contains this Row.
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

        /// <summary>
        /// Insert a cell at the given index.
        /// </summary>
        /// <param name="index"></param>
        public TableCellElement InsertCell(int index)
        {
            try
            {
                object o = ((Interop.IHTMLTableRow)GetBaseElement()).insertCell(index);
                if (o is Interop.IHTMLTableCell)
                {
                    return base.HtmlEditor.GenericElementFactory.CreateElement(o as Interop.IHTMLElement) as TableCellElement;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Insert a cell at the given index.
        /// </summary>
        /// <param name="index"></param>
        public void DeleteCell(int index)
        {
            try
            {
                ((Interop.IHTMLTableRow)GetBaseElement()).deleteCell(index);
            }
            catch (Exception)
            {
                throw;
            }
        }

        # region Public Properties

        /// <summary>
        /// Gets the collection of cells in this row.
        /// </summary>
        /// <remarks>
        /// The purpose of this class is to provide access to the cells to assign formatting.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableCellElements"/>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableCellElement"/>
        /// </remarks>
        [Browsable(false)]
        public TableCellElements CellElements
        {
            get
            {
                Interop.IHTMLTableRow row = (Interop.IHTMLTableRow) base.GetBaseElement();
                if (row != null && row.cells != null && cellElements == null || cellElements.Count != row.cells.GetLength())
                {
                    cellElements = new TableCellElements();
                    for (int c = 0; c < row.cells.GetLength(); c++)
                    {
                        TableCellElement tce = (TableCellElement)HtmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement)row.cells.Item(c, c));
                        tce.baseTable = this.Table;
                        tce.baseRow = this;
                        cellElements.Add(tce);
                    }
                } 
                return cellElements;
            }
        }

        /// <summary>
        /// Gets the number of cells.
        /// </summary>
        /// <remarks>
        /// Gets the number of cells of the current row. The number may differ from row to row depending on the table
        /// structure. This property returns always the real number of cells, which may reduced by previous merging operations.
        /// The property reflect changes immediately. ReadOnly, use insertion methods to change the number of rows.
        /// </remarks>
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Layout")]
        [Browsable(true)]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]
        public int cells
        {
            get
            {
                return ((Interop.IHTMLTableRow) base.GetBaseElement()).cells.GetLength();
            }
        }

        /// <summary>
        /// BGCOLOR sets the background color for a table row. 
        /// </summary>
        /// <remarks>
        /// BGCOLOR should only be used if you already know that the background color is compatible with the font color of the page. 
        /// Otherwise it's easier and more reliable to use styles.
        /// </remarks>
        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="ColorAttributes"]'/>
        [DescriptionAttribute("")]
        [DefaultValueAttribute(typeof(Color), "")]
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
                return base.GetColorAttribute("bgColor");
            }

            set
            {
                base.SetColorAttribute("bgColor", value);
            }
        }

        /// <summary>
        /// BORDERCOLOR work just like their corresponding attributes in the TABLE tag. 
        /// </summary>
        /// <remarks>
        /// One important difference is that this only set the colors of the inside borders.
        /// One of the nifty effects you can do with BORDERCOLOR is to set the colors to white so that the cells don't appear sunk in, but they still have words in them.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableElement">TableElement</seealso>
        /// </remarks>
        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="ColorAttributes"]'/>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(typeof(Color), "")]
        [DescriptionAttribute("")]
        [TypeConverterAttribute(typeof(UITypeConverterColor))]
        [EditorAttribute(
             typeof(UITypeEditorColor),
             typeof(UITypeEditor))]
        [DisplayName()]
        public Color borderColor
        {
            get
            {
                return base.GetColorAttribute("borderColor");
            }

            set
            {
                base.SetColorAttribute("borderColor", value);
            }
        }

        /// <summary>
        /// The height of the row.
        /// </summary>
        /// <remarks>
        /// The height cannot be lower than the content requires. If the table designer is used to change the height this attribute
        /// is always removed and replaced by the corresponding style attribute. The UI recognizes the value of 25 as the default.
        /// <para>
        /// To remove this attribute set the value of the property to -1.
        /// </para>
        /// </remarks>
        [DefaultValueAttribute(25)]
        [CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DescriptionAttribute("")]
        [DisplayName()]
        public int height
        {
            get
            {
                return base.GetIntegerAttribute("height", -1);
            }

            set
            {
                base.SetIntegerAttribute("height", value, -1);
            }
        }

        # endregion

        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="PublicElementConstructor"]'/>
        public TableRowElement() : base("tr", null)
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
        public TableRowElement(IHtmlEditor editor)
            : base("tr", editor)
        {
        }

        internal TableRowElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}