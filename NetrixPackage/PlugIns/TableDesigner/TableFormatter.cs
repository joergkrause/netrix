using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using GuruComponents.Netrix;
using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.UndoRedo;
using GuruComponents.Netrix.WebEditing.Behaviors;
using System.Collections.Generic;

namespace GuruComponents.Netrix.TableDesigner
{
    /// <summary>
    /// Represention of a table. Provides table operations like merge, split, delete, and add.
    /// </summary>
    /// <remarks>
    /// This class is not for direct instantiation. It is necessary to use the 
    /// <see cref="TableFormatter">TableFormatter</see> property
    /// of the <see cref="TableElement">TableElement</see> class instead. To
    /// support public access to this class the type 
    /// <see cref="ITableFormatter">ITableFormatter</see> has to
    /// be used.
    /// </remarks>
    public class TableFormatter : ITableFormatter
    {
        private Interop.IHTMLTable _table;
        private Interop.IHTMLElement _currentCell;
        private HighLightCellList _cellStack = null;
        private TableLayoutInfo _tableInfo;
        private IHtmlEditor htmlEditor;

        # region Internal Properties to set or check the Formatter

        private Interop.IHTMLElement GetParentElement(Interop.IHTMLElement element, string tagName)
        {
            if (element.GetTagName() == tagName)
            {
                return element;
            }
            Interop.IHTMLElement parent = element.GetParentElement();
            while (parent != null)
            {
                if (parent.GetTagName() == tagName.ToUpper()) break;
                parent = parent.GetParentElement();
            }
            return parent;
        }

        /// <summary>
        /// The current element, from which the action starts
        /// </summary>
        internal Interop.IHTMLElement CurrentElement
        {
            get
            {
                // try looking for proper position after previous deletions                
                IElement element = htmlEditor.GetCurrentElement();
                if (element != null)
                {
                    switch (element.TagName)
                    {
                        case "TD":
                            _currentCell = element.GetBaseElement();
                            break;
                        default:
                            element = htmlEditor.GetParentFromHierarchy(element, "TD") as IElement;
                            if (element != null)
                            {
                                _currentCell = element.GetBaseElement();
                            }
                            break;
                    }
                }
                return _currentCell as Interop.IHTMLElement;
            }
            set
            {
                _currentCell = value;
                // let table follow the cell
                _table = null;
                // prepare to new cycle
                _tableInfo = (TableLayoutInfo)TableInfo;
            }
        }

        /// <summary>
        /// The current cell, from which the action starts.
        /// </summary>
        /// <remarks>
        /// Be carefully by setting this property and assure that the cell belongs to the table which is actually
        /// available from <see cref="Table"/> property. However, once set the cell, the class tries to figure out
        /// what table the cell belongs to and forces this table to be set as current table.
        /// </remarks>
        public TableCellElement CurrentCell
        {
            get
            {
                if (CurrentElement != null)
                {
                    return htmlEditor.GenericElementFactory.CreateElement(_currentCell) as TableCellElement;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                CurrentElement = (value == null) ? null : value.GetBaseElement();
            }
        }

        //private Interop.IHTMLElement CurrentElement;

        /// <summary>
        /// Gives access to the table currently being formatted.
        /// </summary>
        /// <remarks>
        /// The property will return <c>null</c> (<c>Nothing</c> in VB.NET) if there is no related table.
        /// <para>
        /// The setting will be overwritten, if the caret is placed inside a table by user action. If there is no
        /// user action, you can set the table the formatter belongs to by using this property.
        /// </para>
        /// </remarks>
        public TableElement Table
        {
            get
            {
                if (CurrentTable != null)
                {
                    return htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement)CurrentTable) as TableElement;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                CurrentTable = value.GetBaseElement() as Interop.IHTMLTable;
            }
        }

        /// <summary>
        /// Gives access to the table structure of the underlying table currently being formatted.
        /// </summary>
        internal Interop.IHTMLTable CurrentTable
        {
            get
            {
                if (this._table == null && this._currentCell != null)
                {
                    this._table = this.GetParentElement(_currentCell, "TABLE") as Interop.IHTMLTable;
                }
                return this._table;
            }
            set
            {
                this._table = value;
                this._tableInfo = null;
            }
        }

        /// <summary>
        /// Is <c>true</c> if a table is associated with the formatter. 
        /// </summary>
        /// <remarks>
        /// If the formatter is being used interactional and the caret is outside a table the table field is null and
        /// the property returns <c>false</c>. However, if set programmatically, the property will always return <c>true</c>.
        /// </remarks>
        internal bool IsInTable
        {
            get
            {
                return (TableInfo == null) ? false : true;
            }
        }

        /// <summary>
        /// The <see cref="TableLayoutInfo"/> class provides structural information about the table in multiple ways. 
        /// This getter returns the singleton instance of <see cref="TableLayoutInfo"/> class.
        /// </summary>
        public ITableLayoutInfo TableInfo
        {
            get
            {
                // prevent callers from calling methods when table is not set, but if caret is still valid,
                // we try to recognize the table from current cell.
                if (_table == null)
                {
                    if (CurrentCell != null)
                    {
                        Interop.IHTMLTable table = CurrentCell.Table.GetBaseElement() as Interop.IHTMLTable;
                        if (table != null)
                        {
                            _table = table;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                // instanciate the info class as singleton, should be rebuild after each operation
                if (_tableInfo == null)
                {
                    _tableInfo = new TableLayoutInfo(_table);
                    Interop.IHTMLElementCollection rowCollection = _table.rows;
                    int rowsLength = rowCollection.GetLength();
                    for (int j = 0; j < rowsLength; j++)
                    {
                        Interop.IHTMLElementCollection cellCollection = (rowCollection.Item(j, j) as Interop.IHTMLTableRow).cells;
                        int colsLength = cellCollection.GetLength();
                        for (int i = 0; i < colsLength; i++)
                        {
                            Interop.IHTMLTableCell interop_IHTMLTableCell = cellCollection.Item(i, i) as Interop.IHTMLTableCell;
                            _tableInfo.AddCell(j, interop_IHTMLTableCell);
                        }
                    }
                }
                return _tableInfo;
            }
            set
            {
                // only used to reset state
                _tableInfo = value as TableLayoutInfo;
            }
        }

        private TableDesigner designer;
        private TableEditDesigner editDesigner;

        internal TableFormatter(IHtmlEditor editor, TableEditDesigner editDesigner)
        {
            this.htmlEditor = editor;
            this.designer = editDesigner.designer;
            this.editDesigner = editDesigner;
        }

        private TableFormatterProperties FormatterProperties
        {
            get
            {
                return designer.GetTableDesigner(htmlEditor).FormatterProperties;
            }
        }

        # endregion

        # region TAB key manager

        /// <summary>
        /// Detects if the given cell is the last one in that row.
        /// </summary>
        /// <param name="cell">Cell to be checked</param>
        /// <param name="row">Row to check for</param>
        /// <param name="col">Column to check for</param>
        /// <returns></returns>
        private bool IsLastCell(Interop.IHTMLTableCell cell, int row, int col)
        {
            int column = ((Interop.IHTMLTableRow)this.CurrentTable.rows.Item(row, row)).cells.GetLength() - 1;
            this.TableInfo.GetCellPoint(cell, ref row, ref col);
            return (col >= column);
        }

        /// <summary>
        /// Detects if the given cell is in the last row and in the last column (last cell other all).
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="row">Row to check for</param>
        /// <param name="col">Column to check for</param>
        /// <returns></returns>
        private bool IsLastRowCell(Interop.IHTMLTableCell cell, int row, int col)
        {
            int column = col;
            int rows = this.CurrentTable.rows.GetLength() - 1;
            this.TableInfo.GetCellPoint(cell, ref row, ref col);
            return (row >= rows && IsLastCell(cell, rows, column));
        }

        /// <summary>
        /// Takes the current cell and set the caret to next possible cell. 
        /// </summary>
        /// <remarks>
        /// If there is
        /// no cell anymore, a new row is created and the caret is placed in the first column
        /// of the new row.
        /// </remarks>
        /// <returns>True, if successfull, false in case on error</returns>
        public bool NextCellPosition(TableCellElement cell)
        {
            return NextCellPosition(cell.GetBaseElement() as Interop.IHTMLTableCell);
        }

        internal bool NextCellPosition(Interop.IHTMLTableCell cell)
        {
            int col = 0;
            int row = 0;
            this.TableInfo.GetCellPoint(cell, ref row, ref col);
            if (IsLastRowCell(cell, row, col))
            {
                InsertTableRow();
                return SetCaretToCell(TableInfo.Item(row + 1, 0));
            }
            if (IsLastCell(cell, row, col))
            {
                return SetCaretToCell(TableInfo.Item(row + 1, 0));
            }
            return SetCaretToCell(TableInfo.Item(row, col + 1));
        }

        /// <summary>
        /// Takes the current cell and set the caret to previous one. 
        /// </summary>
        /// <remarks>
        /// The caret stops at first cell.
        /// If the caret is in the first column and the current row is not the first one, the caret
        /// jumps to the last cell of the previous row.
        /// </remarks>
        /// <returns>True, if successfull, false in case on error</returns>
        public bool PrevCellPosition(TableCellElement cell)
        {
            return PrevCellPosition(cell.GetBaseElement() as Interop.IHTMLTableCell);
        }

        internal bool PrevCellPosition(Interop.IHTMLTableCell cell)
        {
            int col = 0;
            int row = 0;
            this.TableInfo.GetCellPoint(cell, ref row, ref col);
            if (col == 0)
            {
                if (row == 0)
                    return true;
                else
                    return SetCaretToCell(TableInfo.Item(row - 1, ((Interop.IHTMLTableRow)CurrentTable.rows.Item(row, row)).cells.GetLength() - 1));
            }
            return SetCaretToCell(TableInfo.Item(row, col - 1));
        }

        # endregion

        # region Private Methods

        /// <summary>
        /// Sets the caret to a specific cell using MarkupServices. This allows the user to use multiple
        /// table operations in a row without setting the caret manually by keystrokes or mouse hits.
        /// </summary>
        /// <param name="nextCell">The cell to which the caret should be set.</param>
        /// <returns>Returns false, if setting fails.</returns>
        private bool SetCaretToCell(Interop.IHTMLTableCell nextCell)
        {
            try
            {
                if (nextCell == null) return false;
                //CurrentCell = null;
                Interop.IMarkupServices ms = (Interop.IMarkupServices)htmlEditor.GetActiveDocument(false);
                Interop.IMarkupPointer mp;
                ms.CreateMarkupPointer(out mp);
                // CONFIG:
                // ELEM_ADJ_BeforeEnd places the caret to the end of the text in cell
                // You may alter this behavior by setting ELEM_ADJ_BeforeEnd, this sets the caret after any text
                if (FormatterProperties.BeforeText)
                {
                    mp.MoveAdjacentToElement((Interop.IHTMLElement)nextCell, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterBegin);
                }
                else
                {
                    mp.MoveAdjacentToElement((Interop.IHTMLElement)nextCell, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeEnd);
                }
                Interop.IDisplayServices ds = (Interop.IDisplayServices)this.htmlEditor.GetActiveDocument(false);
                Interop.IDisplayPointer dp;
                ds.CreateDisplayPointer(out dp);
                dp.MoveToMarkupPointer(mp, null);
                Interop.IHTMLCaret cr;
                ds.GetCaret(out cr);
                cr.MoveCaretToPointer(dp, true, Interop.CARET_DIRECTION.CARET_DIRECTION_SAME);
                _currentCell = nextCell as Interop.IHTMLElement;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:SetCaretToCell:Exception");
                return false;
            }
            return true;
        }

        # endregion

        # region Public CanOperateXXX Properties

        /// <summary>
        /// Inserting of a table is possible.
        /// </summary>
        public bool CanInsertTableRow
        {
            get
            {
                return this.IsInTable;
            }
        }

        /// <summary>
        /// Inserting of new column is possible.
        /// </summary>
        public bool CanInsertTableColumn
        {
            get
            {
                return this.IsInTable;
            }
        }

        /// <summary>
        /// Deleting of Row is possible.
        /// </summary>
        public bool CanDeleteTableRow
        {
            get
            {
                if (CurrentCell != null && TableInfo != null)
                {
                    if (String.Compare(_currentCell.GetTagName(), "td", true) != 0)
                    {
                        return String.Compare(_currentCell.GetTagName(), "tr", true) == 0;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Deleting of column is possible.
        /// </summary>
        public bool CanDeleteTableColumn
        {
            get
            {
                if (CurrentCell != null && TableInfo != null)
                {
                    return String.Compare(_currentCell.GetTagName(), "td", true) == 0;
                }
                return false;
            }
        }

        /// <summary>
        /// Left merging is possible.
        /// </summary>
        public bool CanMergeLeft
        {
            get
            {
                int col2 = 0;
                int row2 = 0;
                int row = 0;
                int col = 0;
                if (CurrentCell == null || TableInfo == null) return false;
                Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    Interop.IHTMLTableCell leftCell = TableInfo.Item(row, col - 1);
                    if (leftCell != null)
                    {
                        TableInfo.GetCellPoint(leftCell, ref row2, ref col2);
                        if (row2 == row)
                        {
                            return leftCell.rowSpan == cell.rowSpan;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Right merging is possible
        /// </summary>
        public bool CanMergeRight
        {
            get
            {
                int col2 = 0;
                int row = 0;
                int row2 = 0;
                int col = 0;
                if (CurrentCell == null || TableInfo == null) return false;
                Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    Interop.IHTMLTableCell rightCell = TableInfo.Item(row, col + cell.colSpan);
                    if (rightCell != null)
                    {
                        TableInfo.GetCellPoint(rightCell, ref row2, ref col2);
                        if (row2 == row)
                        {
                            return rightCell.rowSpan == cell.rowSpan;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Upwards merging is possible.
        /// </summary>
        public bool CanMergeUp
        {
            get
            {
                int row2 = 0;
                int row = 0;
                int col2 = 0;
                int col = 0;
                if (CurrentCell == null || TableInfo == null) return false;
                Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    Interop.IHTMLTableCell upCell = TableInfo.Item(row - 1, col);
                    if (upCell != null)
                    {
                        TableInfo.GetCellPoint(upCell, ref row2, ref col2);
                        if (col2 == col)
                        {
                            return upCell.colSpan == cell.colSpan;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Down merging is possible.
        /// </summary>
        public bool CanMergeDown
        {
            get
            {
                int row2 = 0;
                int col = 0;
                int row = 0;
                int col2 = 0;
                if (CurrentCell == null || TableInfo == null) return false;
                Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    Interop.IHTMLTableCell upCell = TableInfo.Item(row + cell.rowSpan, col);
                    if (upCell != null)
                    {
                        TableInfo.GetCellPoint(upCell, ref row2, ref col2);
                        if (col2 == col)
                        {
                            return upCell.colSpan == cell.colSpan;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Determines whether the current cellstack is a rectangular are which can merge 
        /// without destroying table structure.
        /// </summary>
        /// <remarks>
        /// The MergeCells method will always try to merge the cells available
        /// in the stack. However, this might destroy table structure and result in weird
        /// table layout. Even if it's possible to correct this, the way depends on desired
        /// behavior of the host application. To avoid this, check whether the collection
        /// is rectangular using this property.
        /// </remarks>
        public bool CanMergeCells
        {
            get
            {
                // trivia checks
                if (TableInfo == null) return false;
                if (_cellStack == null || _cellStack.Count <= 1) return false;
                // check whether it's an rectangle area each row must have same number of cols
                return _cellStack.IsRectangular;
            }
        }

        /// <summary>
        /// Horizontal splitting is possible.
        /// </summary>
        public bool CanSplitHorizontal
        {
            get
            {
                if (CurrentCell != null && TableInfo != null)
                {
                    return String.Compare(_currentCell.GetTagName(), "td", true) == 0;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Vertical splitting is possible.
        /// </summary>
        public bool CanSplitVertical
        {
            get
            {
                if (CurrentCell != null && TableInfo != null)
                {
                    return String.Compare(_currentCell.GetTagName(), "td", true) == 0;
                }
                else
                {
                    return false;
                }
            }
        }

        # endregion

        # region Public Table Operations

        /// <overloads/>
        /// <summary>
        /// Re-Creates the current table setting a complete new structure.
        /// </summary>
        /// <remarks>
        /// This ,ethod will destroy the current content re-creates a new empty table,
        /// using the given number of rows and columns.
        /// </remarks>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public void RebuildTable(int rows, int cols)
        {
            RebuildTable(rows, cols, 1);
        }

        /// <summary>
        /// Re-Creates the current table setting a complete new structure.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="border"></param>
        public void RebuildTable(int rows, int cols, int border)
        {
            RebuildTable(rows, cols, border, System.Web.UI.WebControls.Unit.Percentage(100F));
        }

        /// <summary>
        /// Re-Creates the current table setting a complete new structure.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="border"></param>
        /// <param name="width"></param>
        public void RebuildTable(int rows, int cols, int border, System.Web.UI.WebControls.Unit width)
        {
            if (CurrentTable != null)
            {
                ((Interop.IHTMLElement)CurrentTable).SetInnerText("");
                for (int r = 0; r < rows; r++)
                {
                    Interop.IHTMLTableRow row = (Interop.IHTMLTableRow)CurrentTable.insertRow(0);
                    for (int c = 0; c < cols; c++)
                    {
                        row.insertCell(0);
                    }
                }
            }
        }

        /// <summary>
        /// Add the given caption element to the current table. 
        /// </summary>
        /// <remarks>If no caption exists it will be created. To remove an existing caption, use the
        /// <see cref="TableFormatter.RemoveCaption">RemoveCaption</see>
        /// method.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.CaptionElement">CaptionElement</seealso>
        /// </remarks>
        /// <returns>The new created <see cref="GuruComponents.Netrix.WebEditing.Elements.CaptionElement"/> object.</returns>
        public IElement AddCaption()
        {
            if (CurrentTable != null)
            {
                Interop.IHTMLTableCaption NewTableCaption = CurrentTable.createCaption();
                return htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement)NewTableCaption) as IElement;
            }
            return null;
        }

        /// <summary>
        /// Removes the caption element from the current table. 
        /// </summary>
        /// <remarks>
        /// Does nothing if no <see cref="GuruComponents.Netrix.WebEditing.Elements.CaptionElement">CaptionElement</see> exists.
        /// <seealso cref="TableFormatter.AddCaption">AddCaption</seealso>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.CaptionElement">CaptionElement</seealso>
        /// </remarks>
        public void RemoveCaption()
        {
            if (CurrentTable != null)
            {
                CurrentTable.deleteCaption();
            }
        }

        /// <summary>
        /// Delete the current row from table.
        /// </summary>
        public void DeleteTableRow()
        {
            if (CurrentCell == null) return;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("DeleteTableRow");
            try
            {

                if (FormatterProperties.DeleteFollowsSelection)
                {
                    List<int> rows = new List<int>();
                    foreach (BaseHighLightCell bhl in _cellStack)
                    {
                        if (!rows.Contains(bhl.Row))
                            rows.Add(bhl.Row);
                    }
                    if (rows.Count > 0)
                    {
                        rows.Sort();
                        for (int r = 0; r < rows.Count; r++)
                        {
                            Interop.IHTMLTableCell cell = TableInfo.Item(rows[r], 0);
                            _tableInfo = null;
                            DeleteRowInternal(cell);
                        }
                    }
                    else
                    {
                        DeleteRowInternal(CurrentCell.GetBaseElement() as Interop.IHTMLTableCell);
                    }
                }
                else
                {
                    DeleteRowInternal(CurrentCell.GetBaseElement() as Interop.IHTMLTableCell);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:DeleteTableRow:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        private void DeleteRowInternal(Interop.IHTMLTableCell cell)
        {
            if (_currentCell == null) return;
            int row = 0;
            int col = 0;
            TableInfo.GetCellPoint(cell, ref row, ref col);
            row += cell.rowSpan - 1;
            if (row < 0) return; // cannot remove last row in table
            Interop.IHTMLTable currentTable = TableInfo.Table;
            Interop.IHTMLTableRow currentRow = currentTable.rows.Item(row, row) as Interop.IHTMLTableRow;
            int k = 0;
            for (Interop.IHTMLTableCell currentCell = TableInfo.Item(row, k); currentCell != null; currentCell = TableInfo.Item(row, k))
            {
                if (((Interop.IHTMLElement)currentCell).GetParentElement() as Interop.IHTMLTableRow != currentRow)
                {
                    currentCell.rowSpan = currentCell.rowSpan - 1;
                }
                k += currentCell.colSpan;
            }
            currentTable.deleteRow(row);
            Interop.IHTMLTableCell setCell = null;
            _tableInfo = null;
            switch (FormatterProperties.CaretAfterDeletion)
            {
                case CaretPosition.After:
                    setCell = TableInfo.Item(Math.Min(TableInfo.GetRowNumber() - 1, row + 1), (FormatterProperties.KeepOppositePosition) ? col : 0);
                    break;
                case CaretPosition.Before:
                    setCell = TableInfo.Item(Math.Min(TableInfo.GetRowNumber() - 1, row - 1), (FormatterProperties.KeepOppositePosition) ? col : 0);
                    break;
                case CaretPosition.First:
                    setCell = TableInfo.Item(Math.Min(TableInfo.GetRowNumber() - 1, 0), (FormatterProperties.KeepOppositePosition) ? col : 0);
                    break;
                case CaretPosition.Last:
                    setCell = TableInfo.Item(Math.Max(TableInfo.GetRowNumber() - 1, 0), (FormatterProperties.KeepOppositePosition) ? col : 0);
                    break;
            }
            SetCaretToCell(setCell);
            CurrentElement = (Interop.IHTMLElement)setCell;
        }

        /// <summary>
        /// Deletes a column from table.
        /// </summary>
        public void DeleteTableColumn()
        {
            if (_currentCell == null) return;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("DeleteTableColumn");
            Interop.IHTMLTableCell cell = null;
            try
            {
                if (FormatterProperties.DeleteFollowsSelection)
                {
                    List<int> cols = new List<int>();
                    foreach (BaseHighLightCell bhl in _cellStack)
                    {
                        if (!cols.Contains(bhl.Col))
                            cols.Add(bhl.Col);
                    }
                    if (cols.Count > 0)
                    {
                        cols.Sort();
                        for (int r = 0; r < cols.Count; r++)
                        {
                            cell = TableInfo.Item(0, cols[r]);
                            _tableInfo = null;
                            DeleteColumnInternal(cell);
                        }
                    }
                    else
                    {
                        DeleteColumnInternal(CurrentCell.GetBaseElement() as Interop.IHTMLTableCell);
                    }
                }
                else
                {
                    DeleteColumnInternal(CurrentCell.GetBaseElement() as Interop.IHTMLTableCell);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:DeleteTableColumn:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        private void DeleteColumnInternal(Interop.IHTMLTableCell cell)
        {
            if (_currentCell == null) return;
            int col = 0;
            int row = 0;
            int col2 = 0;
            int row2 = 0;
            TableInfo.GetCellPoint(cell, ref row, ref col);
            col += cell.colSpan - 1;
            int k1 = 0;
            for (Interop.IHTMLTableCell currentCell = TableInfo.Item(k1, col); currentCell != null; currentCell = TableInfo.Item(k1, col))
            {
                Interop.IHTMLTableRow currentRow = ((Interop.IHTMLElement)currentCell).GetParentElement() as Interop.IHTMLTableRow;
                TableInfo.GetCellPoint(currentCell, ref row2, ref col2);
                if (col == col2)
                {
                    int k2 = col;
                    currentRow.deleteCell(k2);
                }
                else
                {
                    currentCell.colSpan = currentCell.colSpan - 1;
                }
                k1 += currentCell.rowSpan;
            }
            Interop.IHTMLTableCell setCell = null;
            _tableInfo = null;
            switch (FormatterProperties.CaretAfterDeletion)
            {
                case CaretPosition.After:
                    setCell = TableInfo.Item((FormatterProperties.KeepOppositePosition) ? row : 0, Math.Min(TableInfo.GetColNumber(row), col));
                    break;
                case CaretPosition.Before:
                    setCell = TableInfo.Item((FormatterProperties.KeepOppositePosition) ? row : 0, Math.Min(TableInfo.GetColNumber(row) - 1, col));
                    break;
                case CaretPosition.First:
                    setCell = TableInfo.Item((FormatterProperties.KeepOppositePosition) ? row : 0, 0);
                    break;
                case CaretPosition.Last:
                    setCell = TableInfo.Item((FormatterProperties.KeepOppositePosition) ? row : 0, TableInfo.GetColNumber(row));
                    break;
            }
            SetCaretToCell(setCell);
            CurrentElement = (Interop.IHTMLElement)setCell;
        }

        InsertPosition lastRowOperation = InsertPosition.Before;

        /// <summary>
        /// Inserts a new row before or after the current row, depending on last operation (repeats last).
        /// </summary>
        public void InsertTableRow()
        {
            InsertTableRow(lastRowOperation == InsertPosition.After);
        }

        /// <summary>
        /// Inserts a new row before or after the current row depending on parameter.
        /// </summary>
        /// <param name="After"><c>True</c> to insert after current row, otherwise its being inserted before.</param>
        public void InsertTableRow(bool After)
        {
            if (CurrentCell == null) return;
            lastRowOperation = After ? InsertPosition.After : InsertPosition.Before;
            int col = 0;
            int row = 0;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("InsertTableRow");
            try
            {
                Interop.IHTMLTableCell cell = (Interop.IHTMLTableCell)CurrentCell.GetBaseElement();
                TableInfo.GetCellPoint(cell, ref row, ref col);
                Interop.IHTMLTableRow parentRow = CurrentCell.Row.GetBaseElement() as Interop.IHTMLTableRow;
                int rowIndex = -1;
                // Setting First/Last override After behavior
                switch (FormatterProperties.NewElementPosition)
                {
                    case InsertPosition.First:
                        rowIndex = 0;
                        break;
                    case InsertPosition.Last:
                        rowIndex = TableInfo.GetRowNumber() - 1;
                        break;
                    default:
                        rowIndex = parentRow.rowIndex + (After ? 1 : 0);
                        break;
                } 
                Interop.IHTMLTable interop_IHTMLTable3 = TableInfo.Table;
                Interop.IHTMLTableRow currentRow = interop_IHTMLTable3.insertRow(rowIndex) as Interop.IHTMLTableRow;
                int colWithSpan = 0;
                for (Interop.IHTMLTableCell currentCell = TableInfo.Item(row, colWithSpan); currentCell != null; currentCell = TableInfo.Item(row, colWithSpan))
                {
                    Interop.IHTMLTableRow rowCellIsIn = ((Interop.IHTMLElement)currentCell).GetParentElement() as Interop.IHTMLTableRow;
                    if (rowCellIsIn == parentRow)
                    {
                        Interop.IHTMLTableCell newCell = (Interop.IHTMLTableCell)currentRow.insertCell(-1);
                        newCell.colSpan = currentCell.colSpan;
                        BaseBehavior cellBehavior = new TableEditDesigner.TableCellBorderBehavior(editDesigner, htmlEditor, newCell as Interop.IHTMLElement2);
                        object oCellBehavior = cellBehavior;
                        ((Interop.IHTMLElement2)newCell).AddBehavior("CellBehavior", ref oCellBehavior);
                        //htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) newCell).ElementBehaviors.AddBehavior(cellBehavior);
                    }
                    else
                    {
                        currentCell.rowSpan = currentCell.rowSpan + 1;
                    }
                    colWithSpan += currentCell.colSpan;
                }
                Interop.IHTMLTableCell setCell = (Interop.IHTMLTableCell)currentRow.cells.Item(0, 0);
                SetCaretToCell(setCell);
                CurrentElement = (Interop.IHTMLElement)setCell;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:InsertTableRow:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        InsertPosition lastColOperation = InsertPosition.Before;

        /// <summary>
        /// Insert a new column in the current table after the current column.
        /// </summary>
        public void InsertTableColumn()
        {
            InsertTableColumn(lastColOperation == InsertPosition.After);
        }

        /// <summary>
        /// Insert a new column in the current table
        /// </summary>
        public void InsertTableColumn(bool After)
        {
            if (CurrentCell == null) return;
            lastColOperation = After ? InsertPosition.After : InsertPosition.Before;
            int row = 0;
            int col = 0;
            int rowIn = 0;
            int colIn = 0;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("InsertTableColumn");
            try
            {
                Interop.IHTMLTableCell cell = (Interop.IHTMLTableCell)CurrentCell.GetBaseElement();
                if (cell != null)
                {
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    int currentRow = 0;
                    Interop.IHTMLTableCell currentCell;
                    for (currentCell = TableInfo.Item(currentRow, col); currentCell != null; currentCell = TableInfo.Item(currentRow, col))
                    {
                        Interop.IHTMLTableRow rowCellIsIn = CurrentCell.Row.GetBaseElement() as Interop.IHTMLTableRow;
                        TableInfo.GetCellPoint(currentCell, ref rowIn, ref colIn);
                        if (col == colIn)
                        {
                            int targetCol = -1;
                            // Setting First/Last override After behavior
                            switch (FormatterProperties.NewElementPosition)
                            {
                                case InsertPosition.First:
                                    targetCol = 0;
                                    break;
                                case InsertPosition.Last:
                                    targetCol = TableInfo.GetColNumber(row) - 1;
                                    break;
                                default:
                                    targetCol = col + (After ? 1 : 0);
                                    break;
                            }
                            Interop.IHTMLTableCell cellToSpan = rowCellIsIn.insertCell(targetCol) as Interop.IHTMLTableCell;
                            BaseBehavior cellBehavior = new TableEditDesigner.TableCellBorderBehavior(editDesigner, htmlEditor, cellToSpan as Interop.IHTMLElement2);
                            object oCellBehavior = cellBehavior;
                            ((Interop.IHTMLElement2)cellToSpan).AddBehavior("CellBehavior", ref oCellBehavior);
                            //htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) cellToSpan).ElementBehaviors.AddBehavior(cellBehavior);
                            cellToSpan.rowSpan = currentCell.rowSpan;
                        }
                        else
                        {
                            currentCell.colSpan = currentCell.colSpan + 1;
                        }
                        currentRow += currentCell.rowSpan;
                    }
                    // let the caret on the old cell
                    SetCaretToCell(cell);
                    CurrentElement = (Interop.IHTMLElement)cell;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:InsertTableColumn:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        /// <summary>
        /// Merge the current cell with the cell left of these.
        /// </summary>
        public void MergeLeft()
        {
            if (CurrentCell == null) return;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("MergeLeft");
            try
            {
                int row = 0;
                int col = 0;
                Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    Interop.IHTMLTableCell leftCell = TableInfo.Item(row, col - 1);
                    if (leftCell == null) return; // cell is in first column therefore we don't have a left cell!
                    Interop.IHTMLElement leftElement = (Interop.IHTMLElement)leftCell;
                    string str = (_currentCell.GetInnerHTML() == null) ? String.Empty : _currentCell.GetInnerHTML();
                    int cellColSpan = cell.colSpan;
                    int cellRowSpan = cell.rowSpan;
                    Interop.IHTMLTableRow currentRow = _currentCell.GetParentElement() as Interop.IHTMLTableRow;
                    currentRow.deleteCell(cell.cellIndex);
                    leftCell.colSpan = leftCell.colSpan + cellColSpan;
                    leftCell.rowSpan = cellRowSpan;
                    leftElement.InsertAdjacentHTML("beforeEnd", str);
                    SetCaretToCell(leftCell);
                    CurrentElement = (Interop.IHTMLElement)leftCell;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:MergeLeft:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        /// <summary>
        /// Merge the current cell with the cell right of these.
        /// </summary>
        public void MergeRight()
        {
            if (CurrentCell == null) return;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("MergeRight");
            try
            {
                Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int row = 0;
                    int col = 0;
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    col += cell.colSpan - 1;
                    Interop.IHTMLTableCell rightCell = TableInfo.Item(row, col + 1);
                    if (rightCell == null) return; // cell is in last col therefore we don't have a right cell!
                    Interop.IHTMLElement rightElement = (Interop.IHTMLElement)rightCell;
                    Interop.IHTMLElement leftElement = (Interop.IHTMLElement)cell;
                    string strR = (rightElement.GetInnerHTML() == null) ? String.Empty : rightElement.GetInnerHTML();
                    int rightCellColSpan = rightCell.colSpan;
                    int rightCellRowSpan = rightCell.rowSpan;
                    Interop.IHTMLTableRow parentRow = rightElement.GetParentElement() as Interop.IHTMLTableRow;
                    int rightCellIndex = rightCell.cellIndex;
                    parentRow.deleteCell(rightCellIndex);
                    cell.colSpan = cell.colSpan + rightCellColSpan;
                    cell.rowSpan = rightCellRowSpan;
                    leftElement.InsertAdjacentHTML("beforeEnd", strR);
                    SetCaretToCell(cell);
                    CurrentElement = (Interop.IHTMLElement)cell;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:MergeRight:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        /// <summary>
        /// Merge the currently selected cells if the given cell collection is a rectangle. Otherwise the method
        /// will do nothing. Needs at least two cells. The content of the merged cells be rewritten into the resulting cell.
        /// </summary>
        public void MergeCells()
        {
            MergeCells(true);
        }

        /// <summary>
        /// Merge the currently selected cells if the given cell collection is a rectangle. Otherwise the method
        /// will do nothing. Needs at least two cells.
        /// </summary>
        /// <param name="SaveCellContent">if True the content of the merged cells will be saved, otherwise deleted.</param>
        public void MergeCells(bool SaveCellContent)
        {
            if (_cellStack == null || _cellStack.Count <= 1) return;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("MergeCells");
            try
            {
                string str = String.Empty;
                Interop.IHTMLTableRow parentRow;
                BaseHighLightCell leftBaseCell = (BaseHighLightCell)this._cellStack[0];
                Interop.IHTMLTableCell leftCell = (leftBaseCell).TableCell as Interop.IHTMLTableCell;
                int currentRow = leftBaseCell.Row;
                ArrayList leftCellStack = new ArrayList();
                // merging all cells rowswise first.                
                int newColSpan = leftCell.colSpan;
                int leftCellIndex = leftCell.cellIndex + 1;
                for (int i = 1; i < this._cellStack.Count; i++)
                {

                    BaseHighLightCell rightBaseCell = (BaseHighLightCell)this._cellStack[i];
                    Interop.IHTMLTableCell rightCell = rightBaseCell.TableCell as Interop.IHTMLTableCell;
                    Interop.IHTMLElement rightElement = (Interop.IHTMLElement)rightCell;
                    int nextRow = rightBaseCell.Row;
                    if (currentRow == nextRow)
                    {
                        // there is a cell on the right, we do merge no

                        str += (rightElement.GetInnerHTML() == null) ? String.Empty : rightElement.GetInnerHTML();
                        int rightCellColSpan = rightCell.colSpan;
                        parentRow = rightElement.GetParentElement() as Interop.IHTMLTableRow;
                        parentRow.deleteCell(leftCellIndex);
                        newColSpan += rightCellColSpan;
                    }
                    else
                    {
                        // end merging, go to next row and merge there
                        currentRow = nextRow;
                        // the element we have merged at last we add to the stack to merge it columnwise later
                        leftCell.colSpan = newColSpan;
                        leftCellStack.Add(leftCell);
                        leftCell = rightCell;
                        newColSpan = leftCell.colSpan;
                    }
                }
                leftCell.colSpan = newColSpan;
                leftCellStack.Add(leftCell);
                // all rows merged or there are no rowwise merging necessary (e.g. we have only one column)
                // the cells we merge no columnwise are already stored in the leftCellStack var
                Interop.IHTMLTableCell upperCell = (Interop.IHTMLTableCell)leftCellStack[0];
                int newRowSpan = upperCell.rowSpan;
                for (int c = 1; c < leftCellStack.Count; c++)
                {
                    Interop.IHTMLTableCell nextCell = (Interop.IHTMLTableCell)leftCellStack[c];
                    if (nextCell == null) return;
                    Interop.IHTMLElement nextElement = (Interop.IHTMLElement)nextCell;
                    str += (nextElement.GetInnerHTML() == null) ? String.Empty : nextElement.GetInnerHTML();
                    parentRow = nextElement.GetParentElement() as Interop.IHTMLTableRow;
                    parentRow.deleteCell(nextCell.cellIndex);
                    newRowSpan += nextCell.rowSpan;
                }
                upperCell.rowSpan = newRowSpan;
                if (SaveCellContent)
                {
                    ((Interop.IHTMLElement)upperCell).InsertAdjacentHTML("beforeEnd", str);
                }
                SetCaretToCell(upperCell);
                CurrentElement = (Interop.IHTMLElement)upperCell;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:MergeCells:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        /// <summary>
        /// Merge the cell the caret is in with the cell above these. Does nothing if there is no cell above
        /// the current one.
        /// </summary>
        public void MergeUp()
        {
            if (CurrentCell == null) return;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("MergeUp");
            try
            {
                Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int row = 0;
                    int col = 0;
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    Interop.IHTMLTableCell upCell = TableInfo.Item(row - 1, col);
                    if (upCell == null) return; // cell is in first row therefore we don't have a upper cell!
                    Interop.IHTMLElement upElement = (Interop.IHTMLElement)upCell;
                    string str = (_currentCell.GetInnerHTML() == null) ? String.Empty : _currentCell.GetInnerHTML();
                    int rowSpan = cell.rowSpan;
                    Interop.IHTMLTableRow parentRow = _currentCell.GetParentElement() as Interop.IHTMLTableRow;
                    parentRow.deleteCell(cell.cellIndex);
                    upCell.rowSpan = upCell.rowSpan + rowSpan;
                    upElement.InsertAdjacentHTML("beforeEnd", str);
                    SetCaretToCell(upCell);
                    CurrentElement = (Interop.IHTMLElement)upCell;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:MergeUp:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        /// <summary>
        /// Merge the cell the caret is in with the cell below these.  Does nothing if there is no cell above
        /// the current one.
        /// </summary>
        public void MergeDown()
        {
            if (!IsInTable || CurrentCell == null) return;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("MergeDown");
            try
            {
                Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    int row = 0;
                    int col = 0;
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    row += cell.rowSpan - 1;
                    Interop.IHTMLTableCell nextCell = TableInfo.Item(row + 1, col);
                    if (nextCell == null) return; // cell is in last row therefore we don't have a down cell! 
                    Interop.IHTMLElement nextElement = (Interop.IHTMLElement)nextCell;
                    string str = (nextElement.GetInnerHTML() == null) ? String.Empty : nextElement.GetInnerHTML();
                    int rowSpan = nextCell.rowSpan;
                    Interop.IHTMLTableRow parentRow = nextElement.GetParentElement() as Interop.IHTMLTableRow;
                    parentRow.deleteCell(nextCell.cellIndex);
                    cell.rowSpan = (cell.rowSpan + rowSpan);
                    ((Interop.IHTMLElement)cell).InsertAdjacentHTML("beforeEnd", str);
                    SetCaretToCell(cell);
                    CurrentElement = (Interop.IHTMLElement)cell;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:MergeDown:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        /// <summary>
        /// Inserts a cell in the current row right from the current cell
        /// </summary>
        public void InsertCell()
        {
            if (CurrentCell == null) return;
            Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("InsertCell");
            try
            {
                if (cell != null)
                {
                    int row = 0;
                    int col = 0;
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    Interop.IHTMLTableRow parentRow = ((Interop.IHTMLElement)cell).GetParentElement() as Interop.IHTMLTableRow;
                    if (parentRow == null) return;
                    Interop.IHTMLTableCell nextCell = (Interop.IHTMLTableCell)parentRow.insertCell(col);
                    if (nextCell != null)
                    {
                        BaseBehavior cellBehavior = new TableEditDesigner.TableCellBorderBehavior(editDesigner, htmlEditor, nextCell as Interop.IHTMLElement2);

                        object oCellBehavior = cellBehavior;
                        ((Interop.IHTMLElement2)nextCell).AddBehavior("CellBehavior", ref oCellBehavior);
                        //htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement)nextCell).ElementBehaviors.AddBehavior(cellBehavior);
                        SetCaretToCell(nextCell);
                        CurrentElement = (Interop.IHTMLElement)nextCell;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:InsertCell:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        /// <summary>
        /// Removes the selected Cell from the cell collection of the current row.
        /// </summary>
        public void DeleteCell()
        {
            if (CurrentCell == null) return;
            Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("DeleteCell");
            try
            {
                if (cell != null)
                {
                    int row = 0;
                    int col = 0;
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    Interop.IHTMLTableRow parentRow = ((Interop.IHTMLElement)cell).GetParentElement() as Interop.IHTMLTableRow;
                    if (parentRow == null) return;
                    parentRow.deleteCell(col);

                    Interop.IHTMLTableCell setCell = null;
                    switch (FormatterProperties.CaretAfterDeletion)
                    {
                        case CaretPosition.After:
                            setCell = TableInfo.Item((FormatterProperties.KeepOppositePosition) ? row : 0, Math.Min(TableInfo.GetColNumber(row) - 1, col));
                            break;
                        case CaretPosition.Before:
                            setCell = TableInfo.Item((FormatterProperties.KeepOppositePosition) ? row : 0, Math.Max(0, Math.Min(TableInfo.GetColNumber(row) - 2, col)));
                            break;
                        case CaretPosition.First:
                            setCell = TableInfo.Item((FormatterProperties.KeepOppositePosition) ? row : 0, 0);
                            break;
                        case CaretPosition.Last:
                            setCell = TableInfo.Item((FormatterProperties.KeepOppositePosition) ? row : 0, TableInfo.GetColNumber(row) - 1);
                            break;
                    }
                    if (setCell != null)
                    {
                        SetCaretToCell(setCell);
                        CurrentElement = (Interop.IHTMLElement)setCell;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:DeleteCell:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        /// <summary>
        /// Split the cell where the caret is currently in. Does nothing if the caret is not inside a table.
        /// </summary>
        public void SplitHorizontal()
        {
            if (CurrentCell == null) return;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("SplitHorizontal");
            try
            {
                int row = 0;
                int col = 0;
                Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    int cellColSpan = cell.colSpan;
                    if (cellColSpan > 1)
                    {
                        cell.colSpan = cellColSpan - 1;
                    }
                    else
                    {
                        int i2 = 0;
                        for (Interop.IHTMLTableCell cellUnSpanned = TableInfo.Item(i2, col); cellUnSpanned != null; cellUnSpanned = TableInfo.Item(i2, col))
                        {
                            if (cellUnSpanned != cell)
                            {
                                cellUnSpanned.colSpan = cellUnSpanned.colSpan + 1;
                            }
                            i2 += cellUnSpanned.rowSpan;
                        }
                    }
                    Interop.IHTMLTableRow parentRow = ((Interop.IHTMLElement)cell).GetParentElement() as Interop.IHTMLTableRow;
                    Interop.IHTMLTableCell newCell = parentRow.insertCell(cell.cellIndex + 1) as Interop.IHTMLTableCell;
                    BaseBehavior cellBehavior = new TableEditDesigner.TableCellBorderBehavior(editDesigner, htmlEditor, newCell as Interop.IHTMLElement2);
                    object oCellBehavior = cellBehavior;
                    ((Interop.IHTMLElement2)newCell).AddBehavior("CellBehavior", ref oCellBehavior);
                    //htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) newCell).ElementBehaviors.AddBehavior(cellBehavior);
                    newCell.rowSpan = cell.rowSpan;
                    Interop.IHTMLSelectionObject selectionObj = ((Interop.IHTMLDocument2)htmlEditor.GetActiveDocument(false)).GetSelection();
                    object currentSelection = selectionObj.CreateRange();
                    Interop.IHTMLTxtRange txtRange = currentSelection as Interop.IHTMLTxtRange;
                    if (txtRange != null)
                    {
                        for (int j2 = 1; j2 > 0; j2 = txtRange.MoveEnd("character", 1))
                        {
                        }
                        string str1 = (_currentCell.GetInnerHTML() == null) ? String.Empty : _currentCell.GetInnerHTML();
                        string str2 = txtRange.GetText();
                        if (str2 != null && str2.Length > 0)
                        {
                            int k2 = str1.LastIndexOf(str2);
                            if (k2 != -1)
                            {
                                _currentCell.SetInnerHTML(str1.Substring(0, k2));
                                ((Interop.IHTMLElement)newCell).SetInnerHTML(str2);
                            }
                        }
                    }
                    SetCaretToCell(newCell);
                    CurrentElement = (Interop.IHTMLElement)newCell;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:SplitHorizontal:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        /// <summary>
        /// Split the current cell vertical
        /// </summary>
        public void SplitVertical()
        {
            if (CurrentCell == null) return;
            IUndoStack batchedUndoUnit = htmlEditor.GetUndoManager("SplitVertical");
            try
            {
                int row = 0;
                int col = 0;
                Interop.IHTMLTableCell cell = CurrentCell.GetBaseElement() as Interop.IHTMLTableCell;
                if (cell != null)
                {
                    TableInfo.GetCellPoint(cell, ref row, ref col);
                    int cellRowSpan = cell.rowSpan;
                    if (cellRowSpan > 1)
                    {
                        cell.rowSpan = cellRowSpan - 1;
                    }
                    else
                    {
                        int col2 = 0;
                        for (Interop.IHTMLTableCell cellUnSpanned = TableInfo.Item(row, col2); cellUnSpanned != null; cellUnSpanned = TableInfo.Item(row, col2))
                        {
                            if (cellUnSpanned != cell)
                            {
                                cellUnSpanned.rowSpan = (cellUnSpanned.rowSpan + 1);
                            }
                            col2 += cellUnSpanned.colSpan;
                        }
                    }
                    Interop.IHTMLTableRow parentRow = ((Interop.IHTMLElement)cell).GetParentElement() as Interop.IHTMLTableRow;
                    Interop.IHTMLTable parentTable = TableInfo.Table;
                    int row2 = parentRow.rowIndex + cell.rowSpan;
                    Interop.IHTMLTableRow secondRow = parentTable.rows.Item(row2, row2) as Interop.IHTMLTableRow;
                    Interop.IHTMLTableCell newCell = null;
                    bool flag = false;
                    if (secondRow != null && cellRowSpan > 1)
                    {
                        int k2;

                        for (k2 = col; k2 >= 0 && ((Interop.IHTMLElement)TableInfo.Item(row2, k2)).GetParentElement() as Interop.IHTMLTableRow != secondRow; k2--)
                        {
                        }
                        if (parentRow.cells.GetLength() > secondRow.cells.GetLength())
                        {
                            int nextCellIndex = 0;
                            if (k2 >= 0)
                            {
                                nextCellIndex = TableInfo.Item(row2, k2).cellIndex + 1;
                            }
                            newCell = secondRow.insertCell(nextCellIndex) as Interop.IHTMLTableCell;
                            BaseBehavior cellBehavior = new TableEditDesigner.TableCellBorderBehavior(editDesigner, htmlEditor, newCell as Interop.IHTMLElement2);

                            object oCellBehavior = cellBehavior;
                            ((Interop.IHTMLElement2)newCell).AddBehavior("CellBehavior", ref oCellBehavior);
                            //htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement)newCell).ElementBehaviors.AddBehavior(cellBehavior);
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        secondRow = parentTable.insertRow(row2) as Interop.IHTMLTableRow;
                        newCell = secondRow.insertCell(-1) as Interop.IHTMLTableCell;
                    }
                    newCell.colSpan = cell.colSpan;
                    Interop.IHTMLSelectionObject selectionObj = ((Interop.IHTMLDocument2)htmlEditor.GetActiveDocument(false)).GetSelection();
                    object currentSelection = selectionObj.CreateRange();
                    Interop.IHTMLTxtRange txtRange = currentSelection as Interop.IHTMLTxtRange;
                    if (txtRange != null)
                    {
                        for (int j3 = 1; j3 > 0; j3 = txtRange.MoveEnd("character", 1))
                        {
                        }
                        string str1 = _currentCell.GetInnerHTML();
                        string str2 = txtRange.GetText();
                        if (str2 != null && str2.Length > 0)
                        {
                            int k3 = String.IsNullOrEmpty(str1) ? -1 : str1.LastIndexOf(str2);
                            if (k3 != -1)
                            {
                                _currentCell.SetInnerHTML(str1.Substring(0, k3));
                                ((Interop.IHTMLElement)newCell).SetInnerHTML(str2);
                            }
                        }
                    }
                    SetCaretToCell(newCell);
                    CurrentElement = (Interop.IHTMLElement)newCell;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message, "TableFormatter:SplitVertical:Exception");
            }
            finally
            {
                batchedUndoUnit.Close();
                _tableInfo = null;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the cells for actions which are based on cells, like <see cref="MergeCells()"/>.
        /// </summary>
        /// <remarks>
        /// This property gets or sets cell collection, which must belong to the current table. If set
        /// by user action the cells are highlighted, though, this option does not highlight the cells
        /// but subsequent operations will operate on this collection like highlighted cells would be.
        /// <para>Returns <c>null</c> (<c>Nothing</c> in VB.NET) if there is no current selection.</para>
        /// </remarks>
        public TableCellElement[] CellCollection
        {
            get
            {
                if (this._cellStack == null) return null;
                TableCellElement[] temp = new TableCellElement[this._cellStack.Count];
                for (int i = 0; i < this._cellStack.Count; i++)
                {
                    temp.SetValue(htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement)((BaseHighLightCell)this._cellStack[i]).TableCell), i);
                }
                return temp;
            }
            set
            {
                _cellStack = new HighLightCellList();
                _cellStack.AddRange(value);
                CurrentCell = htmlEditor.GenericElementFactory.CreateElement(_cellStack[0].TableCell as Interop.IHTMLElement) as TableCellElement;
                Table = htmlEditor.GetParentFromHierarchy(CurrentCell, "table") as TableElement;
            }
        }

        # region Events

        /// <summary>
        /// Fires the <see cref="TableDesigner.TableCellSelection"/> event handler and set the internal cell stack.
        /// </summary>
        /// <param name="t">The table which the selection contains</param>
        /// <param name="cellStack">The collection of cells within the selection</param>
        internal void OnTableCellSelection(Interop.IHTMLTable t, HighLightCellList cellStack)
        {
            _cellStack = cellStack;
            if (cellStack.Count > 0)
            {
                designer.OnTableCellSelection(new TableCellSelectionEventArgs(t, cellStack, this.htmlEditor));
                // allow the Remove event to be fired on times, because this method will called on any table event
                //                RemoveEventHasFired = false;
            }
            else
            {
                designer.OnTableCellSelectionRemoved(new TableCellSelectionEventArgs(t, cellStack, this.htmlEditor));
                // has fired, will fire again only if a new selection was made.
                //                RemoveEventHasFired = true;
            }
        }

        # endregion

    }
}