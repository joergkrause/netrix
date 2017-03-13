using System;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.TableDesigner
{

    internal enum TableCommand
    {
        AddCaption = 100,
        RemoveCaption = 101,
        DeleteTableRow = 102,
        DeleteTableColumn = 103,
        InsertTableRow = 104,
        InsertTableRowAfter = 105,
        InsertTableColumn = 106,
        InsertTableColumnAfter = 107,
        MergeLeft = 108,
        MergeRight = 109,
        MergeCells = 110,
        MergeCellsPreserveContent = 111,
        MergeUp = 112,
        MergeDown = 113,
        InsertCell = 114,
        DeleteCell = 115,
        SplitHorizontal = 116,
        SplitVertical = 117,
        AddBehaviors = 150,
        RemoveBehaviors = 151,
        Activate = 160,
        DeActivate = 161,
    }

	/// <summary>
	/// Zusammenfassung für TableCommands.
	/// </summary>
	public class TableCommands
	{

        private Guid tableDesignerGuid;

		internal TableCommands()
		{
            tableDesignerGuid = this.GetType().GUID;
            AddCaption              = new CommandID(tableDesignerGuid, (int)TableCommand.AddCaption);
            RemoveCaption           = new CommandID(tableDesignerGuid, (int)TableCommand.RemoveCaption);
            DeleteTableRow          = new CommandID(tableDesignerGuid, (int)TableCommand.DeleteTableRow);
            DeleteTableColumn       = new CommandID(tableDesignerGuid, (int)TableCommand.DeleteTableColumn);
            InsertTableRow          = new CommandID(tableDesignerGuid, (int)TableCommand.InsertTableRow);
            InsertTableRowAfter     = new CommandID(tableDesignerGuid, (int)TableCommand.InsertTableRowAfter);
            InsertTableColumn       = new CommandID(tableDesignerGuid, (int)TableCommand.InsertTableColumn);
            InsertTableColumnAfter  = new CommandID(tableDesignerGuid, (int)TableCommand.InsertTableColumnAfter);
            MergeLeft               = new CommandID(tableDesignerGuid, (int)TableCommand.MergeLeft);
            MergeRight              = new CommandID(tableDesignerGuid, (int)TableCommand.MergeRight);
            MergeCells              = new CommandID(tableDesignerGuid, (int)TableCommand.MergeCells);
            MergeCellsPreserveContent= new CommandID(tableDesignerGuid, (int)TableCommand.MergeCellsPreserveContent);
            MergeUp                 = new CommandID(tableDesignerGuid, (int)TableCommand.MergeUp);
            MergeDown               = new CommandID(tableDesignerGuid, (int)TableCommand.MergeDown);
            InsertCell              = new CommandID(tableDesignerGuid, (int)TableCommand.InsertCell);
            DeleteCell              = new CommandID(tableDesignerGuid, (int)TableCommand.DeleteCell);
            SplitHorizontal         = new CommandID(tableDesignerGuid, (int)TableCommand.SplitHorizontal);
            SplitVertical           = new CommandID(tableDesignerGuid, (int)TableCommand.SplitVertical);
            AddBehaviors            = new CommandID(tableDesignerGuid, (int)TableCommand.AddBehaviors);
            RemoveBehaviors         = new CommandID(tableDesignerGuid, (int)TableCommand.RemoveBehaviors);
            Activate                = new CommandID(tableDesignerGuid, (int)TableCommand.Activate);
            DeActivate              = new CommandID(tableDesignerGuid, (int)TableCommand.DeActivate);
        }

        /// <summary>
        /// Designer support: Returns command group for this plugin.
        /// </summary>
        public Guid CommandGroup
        {
            get
            {
                return this.tableDesignerGuid;
            }
        }
        /// <summary>
        /// Add caption to table.
        /// </summary>
        public readonly CommandID AddCaption;
        /// <summary>
        /// Remove caption.
        /// </summary>
        public readonly CommandID RemoveCaption;
        /// <summary>
        /// Delete current row.
        /// </summary>
        public readonly CommandID DeleteTableRow;
        /// <summary>
        /// Delete current column.
        /// </summary>
        public readonly CommandID DeleteTableColumn;
        /// <summary>
        /// Insert row before current one.
        /// </summary>
        public readonly CommandID InsertTableRow;
        /// <summary>
        /// Insert row after current one.
        /// </summary>
        public readonly CommandID InsertTableRowAfter;
        /// <summary>
        /// Insert column before current one.
        /// </summary>
        public readonly CommandID InsertTableColumn;
        /// <summary>
        /// Insert columns after current one.
        /// </summary>
        public readonly CommandID InsertTableColumnAfter;
        /// <summary>
        /// Merge current cell with left one.
        /// </summary>
        public readonly CommandID MergeLeft;
        /// <summary>
        /// Merge current cell with right one.
        /// </summary>
        public readonly CommandID MergeRight;
        /// <summary>
        /// Merge selected cells.
        /// </summary>
        public readonly CommandID MergeCells;
        /// <summary>
        /// Merge selected cells and preserve content.
        /// </summary>
        public readonly CommandID MergeCellsPreserveContent;
        /// <summary>
        /// Merge current cell with up one.
        /// </summary>
        public readonly CommandID MergeUp;
        /// <summary>
        /// Merge current cell with down one.
        /// </summary>
        public readonly CommandID MergeDown;
        /// <summary>
        /// Insert cell right of current one.
        /// </summary>
        public readonly CommandID InsertCell;
        /// <summary>
        /// Delete current cell.
        /// </summary>
        public readonly CommandID DeleteCell;
        /// <summary>
        /// Split cells horizontally.
        /// </summary>
        public readonly CommandID SplitHorizontal;
        /// <summary>
        /// Split cells vertically.
        /// </summary>
        public readonly CommandID SplitVertical;
        /// <summary>
        /// Add default behaviors to current table.
        /// </summary>
        public readonly CommandID AddBehaviors;
        /// <summary>
        /// Remove default behaviors from current table.
        /// </summary>
        public readonly CommandID RemoveBehaviors;
        /// <summary>
        /// Activate the designer globally.
        /// </summary>
        public readonly CommandID Activate;
        /// <summary>
        /// Deactivate the designer globally.
        /// </summary>
        public readonly CommandID DeActivate;


	}
}

