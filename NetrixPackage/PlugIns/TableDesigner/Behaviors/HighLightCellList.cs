using System;
using System.Collections;
using System.Collections.Generic;

namespace GuruComponents.Netrix.TableDesigner
{
	/// <summary>
	/// An overwritten version of ArrayList to provide additional cell access and
	/// have a more type safe access.
	/// </summary>
	internal class HighLightCellList : System.Collections.ArrayList
	{

		internal HighLightCellList() : base()
		{			
		}

        internal HighLightCellList(int elements) : base(elements)
        {			
        }

        internal new BaseHighLightCell this[int index]
        {
            get
            {
                return base[index] as BaseHighLightCell;
            }
        }

        /// <summary>
        /// Returns the cell at the specified position or null if no cell found.
        /// This indexer is read only.
        /// </summary>
        internal BaseHighLightCell this[int row, int col]
        {
            get
            {
                IEnumerator eCells = base.GetEnumerator();
                while (eCells.MoveNext())
                {
                    BaseHighLightCell cell = (BaseHighLightCell) eCells.Current;
                    if (cell.Col == col && cell.Row == row)
                    {
                        return (BaseHighLightCell) eCells.Current;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Return all cells form a specified column.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        internal BaseHighLightCell[] GetRowCollection(int col)
        {            
            ArrayList eRowsAl = new ArrayList();
            IEnumerator eCells = base.GetEnumerator();
            while (eCells.MoveNext())
            {
                if (((BaseHighLightCell)eCells.Current).Col == col)
                {
                    eRowsAl.Add(eCells.Current);
                }
            }
            BaseHighLightCell[] eRows = new BaseHighLightCell[eRowsAl.Count];
            eRowsAl.CopyTo(eRows);
            return eRows;
        }

        /// <summary>
        /// Return all cells from a specified row.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        internal BaseHighLightCell[] GetColCollection(int row)
        {            
            ArrayList eColsAl = new ArrayList();
            IEnumerator eCells = base.GetEnumerator();
            while (eCells.MoveNext())
            {
                if (((BaseHighLightCell)eCells.Current).Row == row)
                {
                    eColsAl.Add(eCells.Current);    
                }
            }
            BaseHighLightCell[] eCols = new BaseHighLightCell[eColsAl.Count];
            eColsAl.CopyTo(eCols);
            return eCols;
        }

        /// <summary>
        /// Returns all column's indices which do have a cell in it.
        /// </summary>
        internal int[] ColumnIndices
        {
            get
            {
                List<int> eColsAl = new List<int>();
                foreach (BaseHighLightCell cell in this)
                {
                    if (!eColsAl.Contains(cell.Col))
                    {
                        eColsAl.Add(cell.Col);
                    }
                }
                return eColsAl.ToArray();
            }
        }

        /// <summary>
        /// Returns all row's indices which do have a cell in it.
        /// </summary>
        internal int[] RowIndices
        {
            get
            {
                List<int> eRowsAl = new List<int>();
                foreach (BaseHighLightCell cell in this)
                {
                    if (!eRowsAl.Contains(cell.Row))
                    {
                        eRowsAl.Add(cell.Row);
                    }
                }
                return eRowsAl.ToArray();
            }
        }

        internal bool IsRectangular
        {
            get
            {
                List<int> colIndices = new List<int>(ColumnIndices);
                // all row collection must have same col signature
                string compareSignature = "";
                string signature = "";
                for (int i = 0; i < colIndices.Count; i++)
                {
                    BaseHighLightCell[] rowCells = GetRowCollection(colIndices[i]);
                    foreach (BaseHighLightCell cell in rowCells)
                    {
                        signature += String.Concat(cell.Row, "-");
                    }
                    if (compareSignature != signature) return false;
                    compareSignature = signature;
                    signature = "";
                }
                return true;
            }
        }

	}
}
