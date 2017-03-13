using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.TableDesigner
{
    /// <summary>
    /// Represention of a table. Provides table operations like merge, split, delete, and add.
    /// </summary>
    /// <remarks>
    /// The table formatter is a tool which manipulates the table's rows and columns in various 
    /// ways. All commands supplied here are synchronous and change the table's DOM immediately.
    /// All commands watch the table to hold it in well formed state. Supposedly one calls a method
    /// which would result in wrong rendering the command is going to be ignored.
    /// </remarks>
    public interface ITableFormatter
    {

        /// <summary>
        /// Returns an object which contains internal information about the table layout.
        /// </summary>
        ITableLayoutInfo TableInfo { get; set; }

        /// <summary>
        /// Takes the current cell and set the caret to previous one. 
        /// </summary>
        /// <remarks>
        /// The caret stops at first cell.
        /// If the caret is in the first column and the current row is not the first one, the caret
        /// jumps to the last cell of the previous row.
        /// </remarks>
        /// <returns>True, if successfull, false in case on error</returns>
        bool PrevCellPosition(TableCellElement cell);

        /// <summary>
        /// Takes the current cell and set the caret to next possible cell. 
        /// </summary>
        /// <remarks>
        /// If there is
        /// no cell anymore, a new row is created and the caret is placed in the first column
        /// of the new row.
        /// </remarks>
        /// <returns>True, if successfull, false in case on error</returns>
        bool NextCellPosition(TableCellElement cell);

        /// <summary>
        /// Get or set the table the formatter works with.
        /// </summary>
        TableElement Table {get; set;}

        
        /// <summary>
        /// Deletes the current table and creates a new one with the give parameters.
        /// All content will be lost.
        /// </summary>
        void RebuildTable(int rows, int cols);

        /// <summary>
        /// Deletes the current table and creates a new one with the give parameters.
        /// All content will be lost.
        /// </summary>
        void RebuildTable(int rows, int cols, int border);

        /// <summary>
        /// Deletes the current table and creates a new one with the give parameters.
        /// All content will be lost.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="border"></param>
        /// <param name="width"></param>
        void RebuildTable(int rows, int cols, int border, System.Web.UI.WebControls.Unit width);

        /// <summary>
        /// Inserting of a table is possible.
        /// </summary>
        bool CanInsertTableRow { get; }

        /// <summary>
        /// Inserting of new column is possible.
        /// </summary>
        bool CanInsertTableColumn { get; }

        /// <summary>
        /// Deleting of Row is possible.
        /// </summary>
        bool CanDeleteTableRow { get; }

        /// <summary>
        /// Deleting of column is possible.
        /// </summary>
        bool CanDeleteTableColumn { get; }

        /// <summary>
        /// Left merging is possible.
        /// </summary>
        bool CanMergeLeft { get; }

        /// <summary>
        /// Right merging is possible
        /// </summary>
        bool CanMergeRight { get; }

        /// <summary>
        /// Upwards merging is possible.
        /// </summary>
        bool CanMergeUp { get; }

        /// <summary>
        /// Determines whether the current cellstack is a rectangular are which can merge 
        /// without destroying table structure.
        /// </summary>
        /// <remarks>
        /// See class implementation 
        /// </remarks>
        bool CanMergeCells { get; }

        /// <summary>
        /// Down merging is possible.
        /// </summary>
        bool CanMergeDown { get; }

        /// <summary>
        /// Horizontal splitting is possible.
        /// </summary>
        bool CanSplitHorizontal { get; }

        /// <summary>
        /// Vertical splitting is possible.
        /// </summary>
        bool CanSplitVertical { get; }

        /// <summary>
        /// Add the given caption element to the current table. If no caption exists it will be created.
        /// </summary>
        /// <returns>The new created <see cref="GuruComponents.Netrix.WebEditing.Elements.CaptionElement"/> object.</returns>
        IElement AddCaption();

        /// <summary>
        /// Removes the caption element from the current table.
        /// </summary>
        /// <remarks>
        /// Does nothing if no <see cref="GuruComponents.Netrix.WebEditing.Elements.CaptionElement"/> exists.
        /// </remarks>
        void RemoveCaption();

        /// <summary>
        /// Delete the current row from the table.
        /// </summary>
        /// <remarks>
        /// The current row is determined by the cell in which the user has placed the caret before the operation starts.
        /// After the operation the caret is placed in the first cell of the previous row. If the first row is deleted
        /// the caret is placed in the first cell of the first row.
        /// </remarks>
        void DeleteTableRow();

        /// <summary>
        /// Deletes the current column from table.
        /// </summary>
        /// <remarks>
        /// The current column is determined by the cell in which the user has placed the caret before the operation starts.
        /// After the operation the caret is placed in the first cell of the previous column. If the first column is deleted
        /// the caret is placed in the first cell of the first column.
        /// </remarks>
        void DeleteTableColumn();

        /// <overloads>Inserts a new row after the current row.</overloads>
        /// <summary>
        /// Inserts a new row after the current row.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The new row beeing inserted follows the structure of the current row. That means that any merged or splitted cells
        /// will be merged or splitted in the new row too.
        /// </para>
        /// <para>
        /// Inserting a new row can also be done by the TAB key. If the property 
        /// <see cref="TableDesignerProperties.ProcessTABKey">ProcessTABKey</see> is on and
        /// the user hits TAB in the last cell of the last row a new row will be inserted.
        /// </para>
        /// </remarks>
        void InsertTableRow();

        /// <summary>
        /// Inserts a new row before or after the current row.
        /// </summary>
        /// <param name="After">Set this parameter to <c>false</c> to insert the row before the current one.</param>
        void InsertTableRow(bool After);

        /// <summary>
        /// Insert a new column in the current table after the current column.
        /// </summary>
        void InsertTableColumn();

        /// <summary>
        /// Insert a new column in the current table
        /// </summary>
        void InsertTableColumn(bool After);
      
        /// <summary>
        /// Merge the current cell with the cell left of these.
        /// </summary>
        void MergeLeft();

        /// <summary>
        /// Merge the current cell with the cell right of these.
        /// </summary>
        void MergeRight();

        /// <overloads>Merge the currently selected cells.</overloads>
        /// <summary>
        /// Merge the currently selected cells and preserve the content.
        /// </summary>
        /// <remarks>
        /// The merging works as expected only if the given cell collection is a rectangle. Otherwise the method
        /// will do nothing. Needs at least two cells. The content of the merged cells be rewritten into the resulting cell.
        /// </remarks>
        void MergeCells();

        /// <summary>
        /// Merge the currently selected cells. Controls content preservation by parameter.
        /// </summary>
        /// <param name="SaveCellContent">if True the content of the merged cells will be saved, otherwise deleted.</param>
        void MergeCells(bool SaveCellContent);

        /// <summary>
        /// Merge the cell the caret is in with the cell above these.
        /// </summary>
        /// <remarks>
        /// Does nothing if there is no cell above
        /// the current one.
        /// </remarks>
        void MergeUp();

        /// <summary>
        /// Merge the cell the caret is in with the cell below these.
        /// </summary>
        /// <remarks>
        /// Does nothing if there is no cell above
        /// the current one.
        /// </remarks>
        void MergeDown();

        /// <summary>
        /// Inserts a cell in the current row right from the current cell.
        /// </summary>
        void InsertCell();

        /// <summary>
        /// Removes the selected Cell from the cell collection of the current row.
        /// </summary>
        void DeleteCell();

        /// <summary>
        /// Split the cell where the caret is currently in.
        /// </summary>
        /// <remarks>
        /// Does nothing if the caret is not inside a table.
        /// </remarks>
        void SplitHorizontal();

        /// <summary>
        /// Split the current cell vertical.
        /// </summary>
        void SplitVertical();

//        /// <summary>
//        /// Will fired if the user has made a cell selection.
//        /// </summary>
//        /// <remarks>
//        /// This informs the handler that operations based 
//        /// on selections are now possible.
//        /// </remarks>
//        event TableCellSelectionEventHandler TableCellSelection;
//
//        /// <summary>
//        /// Will fired if the selection was removed from the table and the cellstack is cleared.
//        /// </summary>
//        event TableCellSelectionEventHandler TableCellSelectionRemoved;

        /// <summary>
        /// Gets or sets the cells for actions which are based on cells, like <see cref="TableFormatter.MergeCells()"/>.
        /// </summary>
        /// <remarks>
        /// This property gets or sets cell collection, which must belong to the current table. If set
        /// by user action the cells are highlighted, though, this option does not highlight the cells
        /// but subsequent operations will operate on this collection like highlighted cells would be.
        /// </remarks>
        TableCellElement[] CellCollection
        {
            get;
            set;
        }
    }
}