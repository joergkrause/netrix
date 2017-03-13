using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix;
using GuruComponents.Netrix.WebEditing;

namespace GuruComponents.Netrix.TableDesigner
{

    /// <summary>
    /// This class builds a complete description of the current table with information
    /// about the cells, merge / split conditions. It contains methods to check the
    /// number of cells per row and per columns and some helper properties.
    /// </summary>
    public class TableLayoutInfo : ITableLayoutInfo
    {

        internal struct CellCoordinate
        {
            internal int x;
            internal int y;
            internal int w;
            internal int h;
            internal int r;
            internal int b;
            internal int row;
            internal int col;

            internal bool GetHit(int XHit, int YHit, int XLeftTolerance, int XRightTolerance, int YTopTolerance, int YBottomTolerance, ref int HitRow, ref int HitCol)
            {
                if ((x >= XHit - XLeftTolerance && x <= XHit + XRightTolerance)
                    &&
                    (y >= YHit - YTopTolerance && y <= YHit + YBottomTolerance))
                {
                    HitRow = row;
                    HitCol = col;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// The cell collection, Keys are cells, Values are <see cref="Pair"/> objects
        /// containing row and col information.
        /// </summary>
        private IDictionary _cells;
        /// <summary>
        /// The list of rows, each contains a list of cells.
        /// </summary>
        private ArrayList _rows;
        /// <summary>
        /// The cell coordinates, used to check for mouse pointer selection much faster
        /// </summary>
        internal ArrayList CellCoordinates;
        /// <summary>
        /// The underlying table this object belongs to.
        /// </summary>
        private Interop.IHTMLTable _table;
          
        /// <summary>
        /// Gives access to the underlaying table structure
        /// </summary>
        public Interop.IHTMLTable Table
        {
            get
            {
                return this._table;
            } 
      
        }
        /// <summary>
        /// Adds a cell to the specified row
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="cell"></param>
        public void AddCell(int rowIndex, Interop.IHTMLTableCell cell)
        {
            ArrayList currentRow;
            int row;
            int colSpan;
            int colIndex;
            int rowSpanCount;
            ArrayList cellList;

            currentRow = (ArrayList) this._rows[rowIndex];
            row = 0;
            while (row < currentRow.Count && currentRow[row] != null)
                row++;
            colSpan = 0;
            while (colSpan < cell.colSpan) 
            {
                colIndex = row + colSpan;
                this.EnsureIndex(currentRow, colIndex);
                currentRow[colIndex] = cell;
                if (!(this._cells.Contains(cell)))
                    this._cells[cell] = new Pair(rowIndex, colIndex);
                rowSpanCount = 1;
                while (rowSpanCount < cell.rowSpan) 
                {
                    if (rowIndex + 1 >= this._rows.Count)
                        this._rows.Add(new ArrayList());
                    cellList = (ArrayList) this._rows[rowIndex + rowSpanCount];
                    this.EnsureIndex(cellList, colIndex);
                    cellList[colIndex] = cell;
                    if (!(this._cells.Contains(cell)))
                        this._cells[cell] = new Pair(rowIndex + rowSpanCount, colIndex);
                    rowSpanCount++;
                }
                colSpan++;
            }
            CellCoordinate cc = new CellCoordinate();
            cc.row = (int) ((Pair) this._cells[cell]).First;
            cc.col = (int) ((Pair) this._cells[cell]).Second;
            cc.x = ((Interop.IHTMLElement) cell).GetOffsetLeft();
            cc.y = ((Interop.IHTMLElement) cell).GetOffsetTop();
            cc.w = ((Interop.IHTMLElement) cell).GetOffsetWidth();
            cc.h = ((Interop.IHTMLElement) cell).GetOffsetHeight();
            cc.r = cc.x + cc.w;
            cc.b = cc.y + cc.h;
            this.CellCoordinates.Add(cc);            
        }

        /// <summary>
        /// Creates the internal table layout from current table
        /// </summary>
        /// <param name="table"></param>
        public TableLayoutInfo (Interop.IHTMLTable table)
        {
            this._table = table;
            int i = table.rows.GetLength();
            this._rows = new ArrayList (i);
            for (int j = 0; j < i; j++)
            {
                this._rows.Add (new ArrayList ());
            }
            this._cells = new HybridDictionary();
            this.CellCoordinates = new ArrayList();
        } 

        /// <summary>
        /// Enhances the current list with null elements to increase capacity
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        private void EnsureIndex (ArrayList list, int index)
        {
            int i = index - (list.Count - 1);
            for (int j = 0; j < i; j++)
            {
                list.Add (null);
            }
        } 

        /// <summary>
        /// Retrieves the cell at given coordinates
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="col">column index</param>
        /// <returns>cell element</returns>
        public Interop.IHTMLTableCell Item (int row, int col)
        {
            if ((row  >=  this._rows.Count) || (row  <  0))
            {
                return null;
            }
            ArrayList arrayList = (ArrayList) this._rows[row];
            if ((col  >=  arrayList.Count) || (col  <  0))
            {
                return null;
            }  
            return (Interop.IHTMLTableCell) arrayList[col];
        } 

        /// <summary>
        /// Total number of rows.
        /// </summary>
        /// <returns></returns>
        public int GetRowNumber()
        {
            return this._rows.Count;
        }

        /// <summary>
        /// Total number of cols in the given row.
        /// </summary>
        /// <param name="row">Row number to check</param>
        /// <returns></returns>
        public int GetColNumber(int row)
        {
            return ((ArrayList) this._rows[row]).Count;
        }

        /// <summary>
        /// Returns the real cells of a specific row. This method behaves on the
        /// real number of cells, which is sometimes smaller than the absolute number 
        /// due to rowspan attributes in previous rows.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public HybridDictionary CellCollectionFromRow(int row)
        {
            HybridDictionary cells = new HybridDictionary();
            foreach(Interop.IHTMLTableCell tc in this._cells.Keys)
            {
                if ((int)((Pair)this._cells[tc]).First == row)
                {
                    cells.Add(tc, this._cells[tc]);
                }
            }
            return cells;
        }

        /// <summary>
        /// Returns the real cells of a specific column. This method behaves on the
        /// real number of cells, which is sometimes smaller than the absolute number 
        /// due to colspan attributes in previous rows.
        /// </summary>
        /// <param name="col">Column from which the collection is retrieved.</param>
        /// <returns></returns>
        public HybridDictionary CellCollectionFromCol(int col)
        {
            HybridDictionary cells = new HybridDictionary();
            foreach(Interop.IHTMLTableCell tc in this._cells.Keys)
            {
                if ((int)((Pair)this._cells[tc]).Second == col)
                {
                    cells.Add(tc, this._cells[tc]);
                }
            }
            return cells;
        }

        /// <summary>
        /// Cell collection to be used elsewhere.
        /// </summary>
        public IDictionary Cells
        {
            get
            {
                return this._cells;
            }
        }

        /// <summary>
        /// Returns the coordinates of a given cell element
        /// </summary>
        /// <param name="cell">Cell element to check</param>
        /// <param name="row">row index</param>
        /// <param name="col">column index</param>
        public void  GetCellPoint (Interop.IHTMLTableCell cell, ref int row, ref int col)
        {
            Pair pair = (Pair) this._cells [cell];
            if (pair  !=  null)
            {
                row = (int) pair.First;
                col = (int) pair.Second;
                return;
            }
            row = -1;
            col = -1;
            return;
        }

        /// <summary>
        /// Returns a string representation of the table
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
        {
            StringBuilder sb;
            int currentRow;
            ArrayList rowElements;
            int r1;
            int r2;
            Interop.IHTMLElement el;
            string content;

            sb = new StringBuilder();
            currentRow = 0;
            while (currentRow < this._rows.Count) 
            {
                rowElements = (ArrayList) this._rows[currentRow];
                r1 = 0;
                while (r1 < rowElements.Count) 
                {
                    sb.Append("----------");
                    r1++;
                }
                sb.Append(Environment.NewLine);
                r2 = 0;
                while (r2 < rowElements.Count) 
                {
                    el = (Interop.IHTMLElement) rowElements[r2];
                    if (el != null) 
                    {
                        sb.Append("|");
                        content = el.GetInnerHTML();
                        if (content != null) 
                        {
                            sb.Append(content.Substring(0, Math.Min(content.Length, 8)));
                            r2++;
                            continue;
                        }
                        sb.Append("empty");
                    }
                    else
                        sb.Append("|   null  ");
                    r2++;
                }
                sb.Append("|");
                sb.Append(Environment.NewLine);
                currentRow++;
            }
            return sb.ToString();
        }

    }
}