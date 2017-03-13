using System;
using System.Collections;
using System.Collections.Specialized;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.TableDesigner
{
    /// <summary>
    /// Gives information about the table layout and provides table manipulation methods.
    /// </summary>
    public interface ITableLayoutInfo
    {
         
        /// <summary>
        /// Gives access to the underlaying table structure
        /// </summary>
        Interop.IHTMLTable Table { get; }

        /// <summary>
        /// Adds a cell to the specified row
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="cell"></param>
        void AddCell(int rowIndex, Interop.IHTMLTableCell cell);

        /// <summary>
        /// Retrieves the cell at given coordinates
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="col">column index</param>
        /// <returns>cell element</returns>
        Interop.IHTMLTableCell Item (int row, int col);

        /// <summary>
        /// Total number of rows.
        /// </summary>
        /// <returns></returns>
        int GetRowNumber();

        /// <summary>
        /// Total number of cols in the given row.
        /// </summary>
        /// <param name="row">Row number to check</param>
        /// <returns></returns>
        int GetColNumber(int row);

        /// <summary>
        /// Returns the real cells of a specific row. This method behaves on the
        /// real number of cells, which is sometimes smaller than the absolute number 
        /// due to rowspan attributes in previous rows.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        HybridDictionary CellCollectionFromRow(int row);

        /// <summary>
        /// Returns the real cells of a specific column. This method behaves on the
        /// real number of cells, which is sometimes smaller than the absolute number 
        /// due to colspan attributes in previous rows.
        /// </summary>
        /// <param name="col">Column from which the collection is retrieved.</param>
        /// <returns></returns>
        HybridDictionary CellCollectionFromCol(int col);

        /// <summary>
        /// Cell collection to be used elsewhere.
        /// </summary>
        IDictionary Cells { get; }

        /// <summary>
        /// Returns the coordinates of a given cell element
        /// </summary>
        /// <param name="cell">Cell element to check</param>
        /// <param name="row">row index</param>
        /// <param name="col">column index</param>
        void GetCellPoint (Interop.IHTMLTableCell cell, ref int row, ref int col);

        /// <summary>
        /// Returns a string representation of the table
        /// </summary>
        /// <returns></returns>
        string ToString();

    }
}
