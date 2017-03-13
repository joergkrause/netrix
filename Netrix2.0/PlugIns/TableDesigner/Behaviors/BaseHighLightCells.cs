using System;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.TableDesigner
{
    /// <summary>
    /// The base class for highlighted cells. The highlighted collection contain objects derived from this type.
    /// </summary>
    internal abstract class BaseHighLightCell
    {
        protected Interop.IHTMLElement GenericCell = null;	//highlight cell
        private int m_Col;
        private int m_Row;

        /// <summary>
        /// The column in which the cell resides. This property does not recognize span attributes. 
        /// </summary>
        internal int Col
        {
            get
            {
                return m_Col;
            }
        }

        /// <summary>
        /// The row in which the cell resides. This property does not recognize span attributes. 
        /// </summary>
        internal int Row
        {
            get
            {
                return m_Row;
            }
        }

        internal BaseHighLightCell(Interop.IHTMLTableCell Cell, IHtmlEditor htmlEditor)
        {
            if (Cell != null)
            {
                GenericCell = (Interop.IHTMLElement) Cell;
                m_Col = Cell.cellIndex;
                m_Row = ((Interop.IHTMLTableRow)((Interop.IHTMLElement) Cell).GetParentElement()).rowIndex;
            }
        }	

        internal Interop.IHTMLTableCell TableCell
        {
            get
            {
                return GenericCell as Interop.IHTMLTableCell;
            }
        }

        internal abstract void MakeHighLight();
        internal abstract void ReleaseHighLight();

        /// <summary>
        /// We need to override this because otherwise the collection will not operate correctly due
        /// to the com interface which generates new hashes on each request.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_Row + (m_Col << 16);
        }

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }


    }
}
