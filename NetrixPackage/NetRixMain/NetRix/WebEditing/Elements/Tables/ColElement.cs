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
    /// Sets properties for a column of table cells.
    /// </summary>
    /// <remarks>
    /// This is a HTML 4.0 tag. 
    /// COL goes after the TABLE tag and before any TR, THEAD, or TBODY elements. (It may go inside a COLGROUP element but it 
    /// doesn't have to.) Each COL defines the properties of one column, unless you use SPAN to indicate that it is for more 
    /// than one column. So the first COL sets the properties for the first column, the second COL sets the properties for the 
    /// second column, and so on. 
    /// <para>
    /// For example, the following code uses three COL tags to set properties of the cells in the first, second, and third 
    /// columns of the table. The first COL doesn't do anything except serve a placeholder for the first column. The second COL 
    /// uses the ALIGN attribute to right align all the cells in the second column. The third COL uses STYLE to set the styles 
    /// of the cells in the third column so that the font color is red: 
    /// <example>
    /// <code>
    ///&lt;TABLE BORDER CELLPADDING=5&gt;
    ///
    ///&lt;COL&gt;
    ///&lt;COL ALIGN=RIGHT&gt;
    ///&lt;COL STYLE="color:red"&gt;
    ///
    ///&lt;TR&gt; &lt;TH&gt;Expense&lt;/TH&gt; &lt;TH&gt;Price&lt;/TH&gt; &lt;TH&gt;Status&lt;/TH&gt; &lt;/TR&gt;
    ///&lt;TR&gt; &lt;TD&gt;office suite&lt;/TD&gt; &lt;TD&gt;1,343.56&lt;/TD&gt; &lt;TD&gt;rental&lt;/TD&gt; &lt;/TR&gt;
    ///&lt;TR&gt; &lt;TD&gt;cabling&lt;/TD&gt; &lt;TD&gt;1.25&lt;/TD&gt; &lt;TD&gt;installed&lt;/TD&gt; &lt;/TR&gt;
    ///&lt;/TABLE&gt;
    /// </code>
    /// This gives us this table:
    /// <code>
    /// &lt;TABLE BORDER CELLPADDING=5&gt;
    /// 
    /// &lt;COL&gt;
    /// &lt;COL ALIGN=RIGHT&gt;
    /// &lt;COL STYLE="color:red"&gt;
    /// 
    /// &lt;TR&gt; &lt;TH&gt;Expense&lt;/TH&gt; &lt;TH&gt;Price&lt;/TH&gt; &lt;TH&gt;Status&lt;/TH&gt; &lt;/TR&gt;
    /// &lt;TR&gt; &lt;TD&gt;office suite&lt;/TD&gt; &lt;TD&gt;1,343.56&lt;/TD&gt; &lt;TD&gt;rental&lt;/TD&gt; &lt;/TR&gt;
    /// &lt;TR&gt; &lt;TD&gt;cabling&lt;/TD&gt; &lt;TD&gt;1.25&lt;/TD&gt; &lt;TD&gt;installed&lt;/TD&gt; &lt;/TR&gt;
    /// &lt;/TABLE&gt;
    /// </code>
    ///</example>
    /// It's important to be absolutely clear on this point: COL does not create columns. It merely sets the properties of columns 
    /// that will be defined later in the code. Cells are not "contained" in COL elements, they just set attributes which are 
    /// applied to the cells in that column position.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableElement">TableElement</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.CaptionAlignment">CaptionAlignment</seealso>
    /// </remarks>
    public sealed class ColElement : StyledElement
    {

        # region Public Properties

        /// <summary>
        /// Gets or sets how many columns this affects.
        /// </summary>
        /// <remarks>
        /// SPAN indicates how many columns the COL tag affects. The default value (i.e. if you don't use SPAN) is 1.
        /// </remarks>
        [Description("")]
        [DefaultValueAttribute(1)]
        [CategoryAttribute("Element Layout")]
        [DisplayNameAttribute()]
        public int span
        {
            get
            {
                return base.GetIntegerAttribute("span", 1);
            } 
            set
            {
                base.SetIntegerAttribute("span", value, 1);
            } 
      
        }

        /// <summary>
        /// The alignment of the column's content.
        /// </summary>
        /// <remarks>
        /// ALIGN sets the horizontal alignment of the cells in the column. The four possible values are LEFT, RIGHT, CENTER, and JUSTIFY. JUSTIFY is poorly supported. 
        /// <para>
        /// This property is used for compatibility only. It is recommended to format columns with styles (CSS) instead.
        /// </para>
        /// </remarks>
        [DescriptionAttribute("")]
        [DefaultValueAttribute(CaptionAlignment.Top)]
        [CategoryAttribute("Element Layout")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]
        public CaptionAlignment align
        {
            get
            {
                return (CaptionAlignment) this.GetEnumAttribute ("align", (CaptionAlignment) 0);
            } 
            set
            {
                this.SetEnumAttribute ("align", value, (CaptionAlignment) 0);
            }       
        }

        /// <summary>
        /// The width of the columns.
        /// </summary>
        /// <remarks>WIDTH fixes the width of the column.</remarks>
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
                return this.GetUnitAttribute("width");
            }

            set
            {
                this.SetUnitAttribute("width", value);
                return;
            }

        }

        /// <summary>
        /// Background color for the whole column.
        /// </summary>
        /// <remarks>
        /// Cell styles may overwrite this value.
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
                return this.GetColorAttribute("bgColor");
            }

            set
            {
                this.SetColorAttribute("bgColor", value);
                return;
            }

        }

        # endregion

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
        public ColElement(IHtmlEditor editor)
            : base("col", editor)
        {
        }

        internal ColElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}