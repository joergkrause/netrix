using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GuruComponents.Netrix.TableDesigner
{

    /// <summary>
    /// Used to determine the position of caret after cell, row, or column deletion.
    /// </summary>
    public enum CaretPosition
    {
        /// <summary>
        /// After removing of a cell, row, or column the caret is placed in the cell, row, or column before (in order) of the deleted cell.
        /// </summary>
        /// <remarks>
        /// If the first cell of a row, the first row, or the first column is removed the caret stays in the new first cell, row, or column.
        /// </remarks>
        Before,
        /// <summary>
        /// After removing of a cell, row, or column the caret is placed in the cell, row, or column after (in order) of the deleted cell.
        /// </summary>
        /// <remarks>
        /// If the last cell of a row, the last row, or the last column is removed the caret stays in the new last cell, row, or column.
        /// </remarks>
        After,
        /// <summary>
        /// The caret is positioned in the first row or column regardless which row has been removed.
        /// </summary>
        First,
        /// <summary>
        /// The caret is positioned in the last row or column regardless which row has been removed.
        /// </summary>
        Last
    }

    /// <summary>
    /// Used to determine the position of the new cell, row, or column elements.
    /// </summary>
    public enum InsertPosition
    {
        /// <summary>
        /// When adding a cell, row, or column the new elements are placed in the cell, row, or column before (in order) the current one.
        /// </summary>
        Before,
        /// <summary>
        /// When adding a cell, row, or column the new elements are placed in the cell, row, or column after (in order) the current one.
        /// </summary>
        After,
        /// <summary>
        /// When adding a cell, row, or column the new elements are placed in the first cell, row, or column.
        /// </summary>
        First,
        /// <summary>
        /// When adding a cell, row, or column the new elements are placed in the last cell, row, or column.
        /// </summary>
        Last
    }

    /// <summary>
    /// Properties that influence the table formatter behavior.
    /// </summary>
    /// <remarks>
    /// This class was added in 2.0 (2010).
    /// </remarks>
    [Serializable()]
    public class TableFormatterProperties
    {
        /// <summary>
        /// Creates an instance of a property bag, that influence the table formatter behavior.
        /// </summary>
        public TableFormatterProperties()
        {
            _caretAfterDeletion = CaretPosition.Before;
            _keepOppositePosition = true;
            _beforeText = false;
            _deleteFollowsSelection = false;
            _newElementPosition = InsertPosition.After;
        }

        private CaretPosition _caretAfterDeletion;
        private bool _keepOppositePosition;
        private bool _beforeText;
        private bool _deleteFollowsSelection;
        private InsertPosition _newElementPosition;

        /// <summary>
        /// Gets or sets how the caret is positioned if the caret was in the cell, row, or column that has been removed.
        /// </summary>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("How the caret is positioned if the caret was in the cell, row, or column that has been removed.")]
        [DefaultValue(typeof(CaretPosition), "Before")]
        public CaretPosition CaretAfterDeletion
        {
            set
            {
                _caretAfterDeletion = value;
            }
            get
            {
                return _caretAfterDeletion;
            }
        }

        /// <summary>
        /// Gets or sets how where new element appears following the position of the caret.
        /// </summary>
        /// <remarks>
        /// The options <see cref="InsertPosition">After</see> and <see cref="InsertPosition">Before</see> refer to the caret's position.
        /// The options <see cref="InsertPosition">First</see> and <see cref="InsertPosition">Last</see> ignore the caret's position and
        /// add the element at the respective positions.
        /// </remarks>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Gets or sets how where new element appears following the position of the caret.")]
        [DefaultValue(typeof(CaretPosition), "Before")]
        public InsertPosition NewElementPosition
        {
            set
            {
                _newElementPosition = value;
            }
            get
            {
                return _newElementPosition;
            }
        }

        /// <summary>
        /// Gets or sets how the caret is set according to current row if a column has been deleted, or to current column, if a row has been deleted.
        /// </summary>
        /// <remarks>
        /// In a grid-like table you can remove cells, rows, or columns, like this:
        /// <para>
        /// [ 1 ]|[ 4 ]|[ 7 ]        
        /// [ 2 ]|[ 5 ]|[ 8 ]        
        /// [ 3 ]|[ 6 ]|[ 9 ]
        /// </para>
        /// Say the caret is in cell 6 and the user calls the DeleteRow method with property <see cref="CaretAfterDeletion"/> set to <c>Before</c>.
        /// This command removes cells 3, 6, and 9. The cursor is placed in the row before, which consists of cells 2, 5, and 8 respectively.
        /// If <c>KeepOppositePosition</c> is <c>true</c>, the caret appears in cell 5. If it is <c>false</c>, it would appear in 2.
        /// </remarks>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Set the caret after deleting according to ist previous position. Default is true.")]
        [DefaultValue(true)]
        public bool KeepOppositePosition
        {
            set
            {
                _keepOppositePosition = value;
            }
            get
            {
                return _keepOppositePosition;
            }
        }

        /// <summary>
        /// If the caret is placed in the new cell after a delete operation, this properties determines where it appears. If true, it appears before the text, otherwise at the end.
        /// </summary>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("The caret is placed in the new cell after a delete operation, this properties determines where it appears. If true, it appears before the text, otherwise at the end.")]
        [DefaultValue(false)]
        public bool BeforeText
        {
            get { return _beforeText; }
            set { _beforeText = value; }
        }

        /// <summary>
        /// If the caret is placed in the new cell after a delete operation, this properties determines where it appears. If true, it appears before the text, otherwise at the end.
        /// </summary>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("The caret is placed in the new cell after a delete operation, this properties determines where it appears. If true, it appears before the text, otherwise at the end.")]
        [DefaultValue(false)]
        public bool DeleteFollowsSelection
        {
            get { return _deleteFollowsSelection; }
            set { _deleteFollowsSelection = value; }
        }

        

        /// <summary>
        /// Supports the propertygrid element.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int changed = 0;
            changed += (_caretAfterDeletion == CaretPosition.Before) ? 0 : 1;
            changed += (_keepOppositePosition == true) ? 0 : 1;
            changed += (_beforeText == false) ? 0 : 1;
            changed += (_deleteFollowsSelection == false) ? 0 : 1;
            return String.Format("{0} propert{1} changed", changed, (changed == 1) ? "y" : "ies");
        }
    }
}
