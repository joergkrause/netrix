using System;
using System.Drawing;
using System.Collections;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.TableDesigner
{

    /// <summary>
    /// Purpose of this class is to provide highlight service for table cells. Each highlight action follows a
    /// mouse action and sets a start and an end cell. The highlight area is between these two cells, if row or
    /// column selection is used. To select a cell area each cell is separatly added to the collection and there
    /// is no specific start and end cell.
    /// </summary>
    internal class HighLightCells
    {
        private HighLightCellFactory m_HighLightCellFactory = null;
        private HighLightCellComparer CellComparer;

        /// <summary>
        /// Store of highlight cells.
        /// </summary>
        private	HighLightCellList m_Cells = new HighLightCellList();

        /// <summary>
        /// The table the cell collection is from.
        /// </summary>
        private Interop.IHTMLTable m_Tab = null;

        internal HighLightCells(Interop.IHTMLTable Table, Color HighLightColor, Color HighLightBorderColor, IHtmlEditor htmlEditor)
        {
            m_HighLightCellFactory = new HighLightCellFactory(HighLightColor, HighLightBorderColor, htmlEditor);
            m_Tab = Table; 
            CellComparer = new HighLightCellComparer();
        }

        /// <summary>
        /// Add a cell to the collection of highlighted cells.
        /// </summary>
        /// <param name="Cell">The base cell</param>
        /// <returns>The highlighted cell</returns>
        private BaseHighLightCell AddHighLightCell(Interop.IHTMLTableCell Cell)
        {
            BaseHighLightCell hCell = m_HighLightCellFactory.CreateHighLightCell(Cell);
            if (!m_Cells.Contains(hCell))
            {
                System.Diagnostics.Debug.WriteLine("AddHighLightCell", Cell.GetHashCode().ToString());
                hCell.MakeHighLight();
                m_Cells.Add(hCell);
            }
            return hCell;
        }

        /// <summary>
        /// Method to add a new cell to the collection.
        /// </summary>
        /// <param name="Cell">The cell to add.</param>
        internal void AddCell(Interop.IHTMLTableCell Cell)
        {
            this.AddHighLightCell(Cell);
        }

        /// <summary>
        /// Provide collection of highlight cells to methods doing something with the selection, like merging.
        /// </summary>
        internal HighLightCellList HighLightCellCollection
        {
            get
            {
                m_Cells.Sort(CellComparer);
                return m_Cells;
            }
        }

        /// <summary>
        /// Returns the first cell in the collection which is used to calculate "cell rectangles".
        /// </summary>
        /// <returns></returns>
        internal Interop.IHTMLTableCell GetFirstCell()
        {
            if (m_Cells.Count > 0)
                return ((BaseHighLightCell) m_Cells[0]).TableCell;
            else
                return null;
        }

        /// <summary>
        /// Remove highlight cells. This method removes the behavior and clears the stack completely.
        /// </summary>
        internal void RemoveHighLight()
        {
            RemoveHighLight(false);
        }
    
        /// <summary>
        /// Remove highlight cells and protect the first cell. This method removes the behavior and clears the stack completely.
        /// </summary>
        /// <param name="PreserveFirst"></param>
        internal void RemoveHighLight(bool PreserveFirst)
        {
            Interop.IHTMLTableCell preservedCell = null;
            if (m_Cells.Count > 0)
            {
                preservedCell = m_Cells[0] as Interop.IHTMLTableCell;
            }
            foreach(BaseHighLightCell cell in m_Cells)
            {
                cell.ReleaseHighLight();
            }
            m_Cells.Clear();
            if (PreserveFirst && preservedCell != null)
            {
                this.AddHighLightCell(preservedCell);
            }
        }

        /// <summary>
        /// If highlight cells present then return true or false else.
        /// </summary>
        /// <returns>Status of Highlighting</returns>
        internal bool HaveHighLightCells()
        {
            return m_Cells.Count > 0 ? true : false;
        }
    }

}