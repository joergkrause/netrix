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
    /// Sets properties for a column of table cells.
    /// </summary>
    /// <remarks>
    /// This is a HTML 4.0 tag. 
    /// <para>
    /// COLGROUP defines a group of columns in the table and allows you to set properties of those columns. COLGROUP goes 
    /// immediately after the TABLE tag and before any TR, THEAD, TBODY, or TFOOT tags. COLGROUP works very much like 
    /// COL, but be sure to note that COLGROUP requires both an opening and closing tag. </para>
    /// <para>
    /// COLGROUP is most useful for defining column groups to use in conjunction with TABLE RULES=GROUPS to put borders between 
    /// groups of columns instead of between every column. For example the following code creates a table that puts the first 
    /// column in a group by itself, all the remaining columns in a group together, and puts borders between the groups of columns.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableElement">TableElement</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.CaptionAlignment">CaptionAlignment</seealso>
    /// </remarks>
    public sealed class ColgroupElement : StyledElement
    {

        # region Public Properties

        /// <summary>
        /// Gets or sets how many columns this affects.
        /// </summary>
        /// <remarks>
        /// SPAN indicates how many columns the COLGROUP tag affects. The default value (i.e. if you don't use SPAN) is 1.
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
        public ColgroupElement(IHtmlEditor editor)
            : base("colgroup", editor)
        {
        }

        internal ColgroupElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}