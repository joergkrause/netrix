using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This class is internally used as a base class for THEAD, TBODY and TFOOT.
    /// </summary>
    /// <remarks>
    /// This class defines various attributes used in the structural elements TBODY, THEAD and TFOOT. These
    /// tags are HTML 4.0 elements and not completely supported in all browsers.
    /// </remarks>
    public class TableSectionElement : StyledElement
    {

        /// <summary>
        /// Insert a new row at the given index.
        /// </summary>
        /// <exception cref="Exception">Throws an exception if insertion fails.</exception>
        /// <param name="index">Index, zero based.</param>
        public TableRowElement InsertRow(int index)
        {
            try
            {
                object newRow = ((Interop.IHTMLTableSection)GetBaseElement()).insertRow(index);
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
            ((Interop.IHTMLTableSection)GetBaseElement()).deleteRow(index);
        }


        /// <summary>
        /// Character to align content.
        /// </summary>
        /// <remarks>
        /// This attribute is used to align the content of a specific column at this character. This is normally used to align decimal
        /// values at the decimal sign (dot or comma).
        /// <para>
        /// To reset the property assign the value <see cref="System.String.Empty">String.Empty</see>.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableColumnHorizontalAlign">TableColumnHorizontalAlign</seealso>
        /// </remarks>
        [DescriptionAttribute("")]
        [DefaultValueAttribute(typeof(string), "")]
        [CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]
        public string @char
        {
            get
            {
                return (base.GetStringAttribute("char").Length == 0 ? String.Empty : base.GetStringAttribute("char")[0].ToString());
            }

            set
            {
                base.SetStringAttribute("char", value.ToString().Length == 0 ? String.Empty : value.ToString()[0].ToString());
            }
        }

        /// <summary>
        /// charoff can be used to specify the number of pixels the alignment should be offset from the char character.
        /// </summary>
        /// <remarks>
        /// This attibute is not supported by any major browser. It is defined here to assure full HTML 4.0 compatibility.
        /// </remarks>
        [DescriptionAttribute("")]
        [DefaultValueAttribute(typeof(int), "0")]
        [CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorInt),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]
        public int charoff
        {
            get
            {
                return base.GetIntegerAttribute("charoff", 0);
            }

            set
            {
                base.SetIntegerAttribute("charoff", value, 0);
            }
        }

        /// <summary>
        /// Can be used to horizontally align the cells within the element. 
        /// </summary>
        /// <remarks>
        /// Additionally to the standard alignment values it is possible to use a character to align the content. To do so set this
        /// property to <see cref="GuruComponents.Netrix.WebEditing.Elements.TableColumnHorizontalAlign">TableColumnHorizontalAlign.Char</see>.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableSectionElement.@char">char</seealso>        
        /// </remarks>
        [DescriptionAttribute("")]
        [DefaultValueAttribute(TableColumnHorizontalAlign.NotSet)]
        [CategoryAttribute("Element Layout")]
        [TypeConverter(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterDropList))]
        [TE.DisplayName()]

        public TableColumnHorizontalAlign align
        {
            set
            {
                this.SetEnumAttribute ("align", (TableColumnHorizontalAlign) value, (TableColumnHorizontalAlign) 0);
                return;
            } 
      
            get
            {
                return (TableColumnHorizontalAlign) this.GetEnumAttribute ("align", (TableColumnHorizontalAlign) 0);
            } 
      
        }

        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="VerticalAlign"]/*'/>
        [DescriptionAttribute("")]
        [DefaultValueAttribute(System.Web.UI.WebControls.VerticalAlign.NotSet)]
        [CategoryAttribute("Element Layout")]
        [TypeConverter(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterDropList))]
        [TE.DisplayName()]
        public System.Web.UI.WebControls.VerticalAlign valign
        {
            get
            {
                return (VerticalAlign) this.GetEnumAttribute ("valign", (VerticalAlign) 0);
            } 
            set
            {
                this.SetEnumAttribute ("valign", (VerticalAlign) value, (VerticalAlign) 0);
            } 
      
        }
        protected TableSectionElement (string tag, IHtmlEditor editor) : base (tag, editor)
        {
        }
        internal TableSectionElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }

    }
}

