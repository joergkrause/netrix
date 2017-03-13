using System;
using System.Collections;
using System.Collections.Generic;

using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GuruComponents.Netrix;
using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.UndoRedo;
using System.Drawing.Drawing2D;

namespace GuruComponents.Netrix.TableDesigner
{    
   
    /// <summary>
    /// The TableDesigner class realises all mouse and key related operations against tables.
    /// </summary>
    /// <remarks>
    /// It recognizes
    /// keystrokes to set current cell, move caret with TAB key, detect mouse over cell borders to do 
    /// cell width and height changes, table resize, and cell selection.
    /// <para>
    /// See also <see cref="TableFormatter"/> for additonal table operations based on the cell collection the
    /// table designer can store based on cell selection operations.
    /// </para>
    /// </remarks>
    [Serializable()]
    public class TableEditDesigner : Interop.IHTMLEditDesigner
    {

        # region Constants and Variables

        /// <summary>
        /// Number of pixels where the mouse detects Row marker
        /// </summary>
        private const int ZONEROW  = 2;
        /// <summary>
        /// Number of pixels where the mouse detects Col marker
        /// </summary>
        private const int ZONECOL  = 2;
        /// <summary>
        /// Right shift of pixels where the mouse detects Cell marker. Should set to double of ZONECOL or more.
        /// If this value is to high and cells are very small, it is impossible to place the caret inside the
        /// cell because the cell marker detection area occupies the whole cell surface.
        /// </summary>
        private const int DZONECOL  = 6;
        /// <summary>
        /// Number of pixels where the mouse detects Cell marker
        /// </summary>
        private const int ZONECELL = 2;
        /// <summary>
        /// Slider area width; negative values detects before cell area
        /// </summary>
        private const int ZONESLW  = 2;
        /// <summary>
        /// Slider area height; negative values detects before cell area
        /// </summary>
        private const int ZONESLT  = 2; 
        /// <summary>
        /// CONFIG: Width and height of handles for table resizing
        /// </summary>
        private const int HANDLESIZE = 6;
        /// <summary>
        /// The predesign mode, which is active (true) if the mouse hovers the activation icon
        /// </summary>
        internal bool PreDesignMode;
        /// <summary>
        /// The preresize mode, which is active if the mouse is over a resize handle
        /// </summary>
        internal bool PreResizeMode;
        /// <summary>
        /// The resize mode is entered, if true. During resizing the mouse moves the table handles. 
        /// See <see cref="ResizeDirection">GuruComponents.Netrix.WebEditing.Behaviors.TableDesigner.Table.</see> for current resize type.
        /// </summary>
        private bool isResizeMode;
        /// <summary>
        /// Last cell in simple cell selection mode
        /// </summary>
        private Interop.IHTMLTableCell lastCell;

        //private bool resizeBehaviorActive = false;

        //public bool ResizeBehaviorActive
        //{
        //    get { return resizeBehaviorActive; }
        //    set { resizeBehaviorActive = value; }
        //}

        internal bool IsResizeMode
        {
            get { return isResizeMode; }
            set 
            { 
                isResizeMode = value;
                if (value && resizeBehaviorCookie == 0 && advancedParameters)
                {
                    object r = ResizeBehavior;
                    resizeBehaviorCookie = ((Interop.IHTMLElement2)CurrentTable).AddBehavior(ResizeBehavior.Name, ref r);
                }
                if (!value && resizeBehaviorCookie != 0)
                {
                    ((Interop.IHTMLElement2)CurrentTable).RemoveBehavior(resizeBehaviorCookie);
                    resizeBehaviorCookie = 0;
                }
            }
        }
        /// <summary>
        /// The list of cell pairs which are part of the current width slider operation
        /// </summary>
        private ArrayList WidthSliderCellList = null;
        /// <summary>
        /// The list of cell pairs which are part of the current height slider operation
        /// </summary>
        private ArrayList HeightSliderCellList = null;
        /// <summary>
        /// This field contains a information about the current resizing process
        /// </summary>
        internal ResizeModeEnum ResizeDirection = ResizeModeEnum.None;
        /// <summary>
        /// True if the previous operation was a succesfully finished slider operation
        /// </summary>
        internal bool SliderWasMoved;
        /// <summary>
        /// The current borders, where the silder stops
        /// </summary>
        internal int LeftTopBorder = 0, RightBottomBorder = 0;
        /// <summary>
        /// The cellspacing, used to correct the calculation of active areas and borders 
        /// </summary>
        internal int CellSpacing = 0;
        /// <summary>
        /// The current x coordinate of the mouse pointer
        /// </summary>
        private int x;
        /// <summary>
        /// The current y coordinate of the mouse pointer
        /// </summary>
        private int y;
        /// <summary>
        /// A reference to the host control. Gets access to exec methods.
        /// </summary>
        private IHtmlEditor htmlEditor;
        /// <summary>
        /// The return state of the current operation, prevents other designers from being called if set to S_OK.
        /// </summary>
        private int preHandleReturnState = Interop.S_FALSE;
        /// <summary>
        /// Controls the activation state. If activated the behaviors will applied, if not they will removed.
        /// </summary>
        private bool _isActivated = false;   
        /// <summary>
        /// The table, associated with this designer. Only one table is associated at the time.
        /// </summary>
        private Interop.IHTMLTable currentTable;
        /// <summary>
        /// The table that was selected before the current table becomes active, used to deal with nested tables.
        /// </summary>
        private Interop.IHTMLTable OldCurrentTable;
        /// <summary>
        /// Left mouse button
        /// </summary>
        private const int LEFT     = 1;
        /// <summary>
        /// Right mouse button
        /// </summary>
        private const int RIGHT    = 2;
        /// <summary>
        /// No mouse button
        /// </summary>
        private const int NONE     = 0;
        /// <summary>
        /// TAB key code
        /// </summary>
        private const int TAB      = 9;

        /// <summary>
        /// The behavior which draw the rectangle around the whole table
        /// </summary>
        private TableDesignerBehavior _singletonTableBehavior = null;
        /// <summary>
        /// A single instance of the table cell highlight behavior, which draws the highlighted background
        /// </summary>
        private TableCellHighLightBehavior _singletonHighLightBehavior = null;
        /// <summary>
        /// The images used a mouse pointer for cell/row/col selections.
        /// </summary>
        private string[] astrCursorsName = new string[3] {"DiagonalArrow.cur", "DownArrow.cur", "RightArrow.cur"};
        /// <summary>
        /// The behavior which draws the dashed line to align the slider with the ruler.
        /// </summary>
        private TableSliderLineBehavior BodySliderBehavior;        

        /// <summary>
        /// These elements are the base areas for getting scroll positions, body for standard elements, document for frames.
        /// </summary> 
        private Interop.IHTMLElement body;
        private Interop.IHTMLElement2 body2;

        internal Interop.IHTMLElement2 Body
        {
            get { return body2; }
        }

        internal Interop.IHTMLStyle BodyStyle
        {
            get { return body.GetStyle(); }
        }


        /// <summary>
        /// Contains the currently highlighted cells and offers various methods to deal with
        /// </summary>
        internal HighLightCells cellStack;
        /// <summary>
        /// The type of selection for cell/row/col selections with highlightmode. Used via <see cref="ztTableZone"/> property.
        /// </summary>
        private ZoneTypeEnum zt;

        /// <summary>
        /// If true the width/height slider will only add space, which make the table growing during slider operations.
        /// The <see cref="TableDesignerProperties"/> class sets this value to false per default.
        /// </summary>
        private bool sliderAddMode;

        /// <summary>
        /// If true the slider line will drawn. This drawing is critical on slow systems because the complete screen is
        /// refreshing after each step.
        /// </summary>
        private bool sliderActivated;


        /// <summary>
        /// Shows advanced cell width and height values during resize.
        /// </summary>
        private bool advancedParameters;

        /// <summary>
        /// If true the slider is and the resize does not use WYSIWYG to increase speed.
        /// </summary>
        private bool fastResizeMode;

        /// <summary>
        /// This field holds the configured state of the cell selection feature. If false, no selection is possible.
        /// </summary>
        private bool withCellSelection;

        /// <summary>
        /// This struct controls the cell selection feature.
        /// </summary>
        private CellSelectionStruct IsInCellSelection;

        /// <summary>
        /// This struct controls the cell slider feature.
        /// </summary>
        private CellMoveStruct IsInSlider;

        /// <summary>
        /// The color used to draw a background for selected cells.
        /// </summary>
        private Color cellSelectionColor;

        /// <summary>
        /// The color used to draw the border. If equals to <see cref="cellSelectionColor"/> the border is unvisible.
        /// </summary>
        private Color cellSelectionBorderColor;

        /// <summary>
        /// Stores the undo steps during slider operations to reset slider state with one undo command.
        /// </summary>
        private IUndoStack batchedUndoUnit;

        /// <summary>
        /// Stores a reference to the current slider behavior, if any.
        /// </summary>
        private int sliderCookie;

        /// <summary>
        /// Stores references to behaviors for cells.
        /// </summary>
        private Hashtable elementCookie;

        # endregion

        # region Embedded Classes for internal used behaviors

        /// <summary>
        /// Holds data for the current cell selection process.
        /// </summary>
        private struct CellSelectionStruct
        {
            /// <summary>
            /// True if currently a selection was made.
            /// </summary>
            public bool InCellSelection;
            /// <summary>
            /// Current Row
            /// </summary>
            public int CurrentRow;
            /// <summary>
            /// Current Column
            /// </summary>
            public int CurrentCol;
            /// <summary>
            /// This field controls the cell selection feature. It is true if the designer allows a cell selection.
            /// </summary>
            public bool StartCellSelection;

            public CellSelectionStruct(bool start)
            {
                InCellSelection = start;
                StartCellSelection = start;
                CurrentRow = -1;
                CurrentCol = -1;
            }
        }

        /// <summary>
        /// Holds data for the current cell moving process.
        /// </summary>
        private struct CellMoveStruct
        {
            private bool isInWidthSlider;
            private bool isInHeightSlider;
            /// <summary>
            /// True during the slider operation
            /// </summary>
            public bool isDuringWidthSliderOperation;
            /// <summary>
            /// True during the slider operation
            /// </summary>
            public bool isDuringHeightSliderOperation;
            public int CurrentRow;
            public int CurrentCol;

            public CellMoveStruct(bool start)
            {
                isInWidthSlider = start;
                isInHeightSlider = start;
                isDuringWidthSliderOperation = start;
                isDuringHeightSliderOperation = start;
                CurrentRow = -1;
                CurrentCol = -1;
            }

            /// <summary>
            /// True if the mouse cursors is a split cursor an the table can switch to slider mode. 
            /// </summary>
            public bool IsInHeightSlider
            {
                get
                {
                    return isInHeightSlider;
                }
                set
                {
                    isInHeightSlider = value;
                    if (!value)
                    {
                        this.CurrentCol = -1;
                        this.CurrentRow = -1;
                    }
                }

            }

            /// <summary>
            /// True if the mouse cursors is a split cursor an the table can switch to slider mode. 
            /// </summary>
            public bool IsInWidthSlider
            {
                get
                {
                    return isInWidthSlider;
                }
                set
                {
                    isInWidthSlider = value;
                    if (!value)
                    {
                        this.CurrentCol = -1;
                        this.CurrentRow = -1;
                    }
                }

            }

        }

        /// <summary>
        /// Behavior to draw a customizable border around any cell during table design time.
        /// </summary>
        /// <remarks>
        /// This behavior is used instead the ZEROBORDERATDESIGNTIME command to may other
        /// table designer functions better visible.
        /// </remarks>
        internal class TableCellBorderBehavior : BaseBehavior
        {

            private Pen _cellborder;
            private TableEditDesigner _td;
            private Interop.IHTMLElement2 _cell;
            private SolidBrush _cellbrush;
            private SolidBrush _textbrush;
            private Font _cellFont;
            private GraphicsPath _pathTopLeft;
            private GraphicsPath _pathBotRight;
            private IHtmlEditor _htmlEditor;
            private Pen _pen;

            public TableCellBorderBehavior(TableEditDesigner td, IHtmlEditor editor, Interop.IHTMLElement2 cell) : base(editor)
            {
                _cell = cell;
                _htmlEditor = editor;
                // For advanced parameter mode
                _cellbrush = new SolidBrush(Color.FromArgb(228, Color.LightYellow));
                _textbrush = new SolidBrush(Color.Blue);
                _pen = new Pen(_textbrush, 1F);
                _cellFont = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Pixel);
                _pathTopLeft = new GraphicsPath(new Point[] { Point.Empty, Point.Empty, Point.Empty }, new byte[] { (byte)PathPointType.Start, (byte)PathPointType.Line, (byte)PathPointType.CloseSubpath });                
                _pathBotRight = new GraphicsPath(new Point[] { Point.Empty, Point.Empty, Point.Empty }, new byte[] { (byte)PathPointType.Start, (byte)PathPointType.Line, (byte)PathPointType.CloseSubpath });
                _td = td;
                _cellborder = new Pen(Color.Gray, 1.0F);
                _cellborder.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                base.HtmlPaintFlag = HtmlPainter.Transparent;
                base.HtmlZOrderFlag = HtmlZOrder.BelowContent;
            }

            public override string Name
            {
                get
                {
                    return "TableCellBehavior#" + BaseBehavior.url;
                }
            }

            internal Pen CellBorder
            {
                set
                {
                    _cellborder = value;
                }
            }

            protected override void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, Graphics gr)
            {
                if (_td.IsActivated)
                {
                    gr.PageUnit = GraphicsUnit.Pixel;
                    gr.DrawRectangle(_cellborder, leftBounds, topBounds, rightBounds - leftBounds, bottomBounds - topBounds);
                    if (_td.advancedParameters)
                    {
                        if (_td.IsDuringWidthSliderOperation || _td.IsDuringHeightSliderOperation)
                        {
                            //Interop.IHTMLElement cell = _cell.GetBaseElement(); // base.GetElement(_htmlEditor)
                            Interop.IHTMLRect r = _cell.GetBoundingClientRect();
                            Interop.IHTMLElement table = ((Interop.IHTMLElement)_cell).GetParentElement().GetParentElement().GetParentElement();
                            if (table.Equals(_td.currentTable))
                            {
                                int middleY = topBounds + ((bottomBounds - topBounds) / 2);
                                int middleX = leftBounds + ((rightBounds - leftBounds) / 2);
                                if (_td.IsDuringWidthSliderOperation)
                                {
                                    string width = String.Format("{0}  ", (r.right - r.left));
                                    SizeF textSize = gr.MeasureString(width, _cellFont);
                                    // <--- [  ] -->
                                    gr.FillRectangle(_cellbrush, leftBounds + 1, topBounds + 1, rightBounds - leftBounds - 2, bottomBounds - topBounds - 2);
                                    gr.DrawLine(_pen, leftBounds + 3, middleY - 3, leftBounds, middleY);
                                    gr.DrawLine(_pen, leftBounds + 3, middleY + 3, leftBounds, middleY);
                                    gr.DrawLine(_pen, rightBounds - 3, middleY - 3, rightBounds, middleY);
                                    gr.DrawLine(_pen, rightBounds - 3, middleY + 3, rightBounds, middleY);
                                    gr.DrawLine(_pen, leftBounds, middleY, rightBounds, middleY);
                                    gr.FillRectangle(_cellbrush, (middleX) - textSize.Width / 2, (middleY) - textSize.Height / 2, textSize.Width, textSize.Height);
                                    gr.DrawString(width, _cellFont, _textbrush, (middleX) - textSize.Width / 2, (middleY) - textSize.Height / 2, StringFormat.GenericDefault);
                                }
                                else
                                {
                                    Invalidate();
                                }
                                if (_td.IsDuringHeightSliderOperation)
                                {
                                    string height = String.Format("{0}  ", (r.bottom - r.top));
                                    SizeF textSize = gr.MeasureString(height, _cellFont);
                                    // <--- [  ] -->
                                    gr.FillRectangle(_cellbrush, leftBounds + 1, topBounds + 1, rightBounds - leftBounds - 2, bottomBounds - topBounds - 2);
                                    gr.DrawLine(_pen, middleX - 3, topBounds + 3, middleX, topBounds);
                                    gr.DrawLine(_pen, middleX + 3, topBounds + 3, middleX, topBounds);
                                    gr.DrawLine(_pen, middleX - 3, bottomBounds - 3, middleX, bottomBounds);
                                    gr.DrawLine(_pen, middleX + 3, bottomBounds - 3, middleX, bottomBounds);
                                    gr.DrawLine(_pen, middleX, topBounds, middleX, bottomBounds);
                                    gr.FillRectangle(_cellbrush, (middleX) - textSize.Width / 2, (middleY) - textSize.Height / 2, textSize.Width, textSize.Height);
                                    gr.DrawString(height, _cellFont, _textbrush, (middleX) - textSize.Width / 2, (middleY) - textSize.Height / 2, StringFormat.GenericDefault);
                                }
                                else
                                {
                                    Invalidate();
                                }
                            }
                        } // end slider mode
                    }
                }
                else
                {
                    Invalidate();
                }
            }
        }

        
        /// <summary>
        /// Set cell background to mark highlighting
        /// </summary>
        internal class TableCellHighLightBehavior : BaseBehavior
        {
            private SolidBrush _cellBrush;
            private TableEditDesigner _td;

            # region Public Properties

            public SolidBrush CellBrush
            {
                get
                {
                    return _cellBrush;
                }

                set
                {
                    _cellBrush = value;
                }
            }

            # endregion

            public TableCellHighLightBehavior(TableEditDesigner td, IHtmlEditor host) : base(host)
            {
                _td = td;
                _cellBrush = new SolidBrush(Color.Pink);
                base.HtmlPaintFlag = HtmlPainter.Transparent;
                base.HtmlZOrderFlag = HtmlZOrder.BelowContent;
                base.BorderMargin = new Rectangle(-1, -1, -1, -1);
            }

            public override string Name
            {
                get
                {
                    return "TableCellHighLight#" + BaseBehavior.url;
                }
            }

            protected override void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, Graphics gr)
            {
                if (_td.IsActivated)
                {
                    gr.PageUnit = GraphicsUnit.Pixel;
                    gr.FillRectangle(CellBrush, leftBounds, topBounds, rightBounds - leftBounds, bottomBounds - topBounds);
                }
                else
                {
                    Invalidate();
                }
            }
        }

        # endregion

        private ITableFormatter tableFormatter = null;

        internal Interop.IHTMLTable CurrentTable
        {
            get
            {
                return currentTable;
            }
            set
            {
                currentTable = value;
                ((TableFormatter)this.TableFormatter).CurrentTable = currentTable;
            }
        }

        /// <summary>
        /// Returns the current table formatter instance.
        /// </summary>
        public ITableFormatter TableFormatter
        {
            get
            {
                if (tableFormatter == null)
                {
                    tableFormatter = new TableFormatter(this.htmlEditor, this);
                }
                return tableFormatter;
            }
        }

        internal TableDesigner designer;
        private ResizeBehavior resizeBehavior;
        private int resizeBehaviorCookie;

        internal ResizeBehavior ResizeBehavior
        {
            get
            {
                if (resizeBehavior == null)
                {
                    resizeBehavior = new ResizeBehavior(this, htmlEditor);
                }
                return resizeBehavior;
            }            
        }

        /// <summary>
        /// TableDesigner constructor.
        /// </summary>
        /// <remarks>
        /// Called during ActivateTableDesigner call. Under normal circumstances
        /// this constructor should never called directly from user code. It cannot work without adding as
        /// edit host designer, which is impossible from users code due to security restrictions.
        /// </remarks>
        /// <param name="host">The host component which the designer is related too.</param>
        /// <param name="properties">The property object.</param>
        /// <param name="designer">The designer.</param>
        internal TableEditDesigner(IHtmlEditor host, TableDesignerProperties properties, TableDesigner designer)
        {
            htmlEditor = host;
            BodySliderBehavior = new TableSliderLineBehavior(htmlEditor);
            designer.NotifySubReadyStateCompleted += new EventHandler(host_ReadyStateComplete);
            this.designer = designer;
            x = y = 0;
            IsInSlider = new CellMoveStruct(false);
            IsInCellSelection = new CellSelectionStruct(false);
            elementCookie = new Hashtable();
            sliderCookie = -1;
            SetProperties(properties);
            //properties.PropertyChanged += (o, e) => SetProperties(o as TableDesignerProperties, e.PropertyName);
            //Because not working in 2005 above line
            properties.PropertyChanged += new PropertyChangedEventHandler(properties_PropertyChanged);
        }

        void properties_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetProperties((TableDesignerProperties)sender, e.PropertyName);
        }


        void host_ReadyStateComplete(object sender, EventArgs e)
        {
            CurrentTable = null;
            OldCurrentTable = null;
            ztTableZone = ZoneTypeEnum.None;
            isResizeMode = false;
            sliderCookie = 0;
            //resizeBehaviorCookie = 0;
            LastElement = null;
            // always get the current active document, which is the main doc or any of the frame documents
            body = htmlEditor.GetBodyElement().GetBaseElement();
            body2 = (Interop.IHTMLElement2)body;
        }

        # region Methods to operate with table and cell elements

        /// <summary>
        /// Gets or sets the selection for cell/row/col selections with highlightmode.
        /// </summary>
        private ZoneTypeEnum ztTableZone
        {
            get
            {
                return zt;
            }
            set
            {
                zt = value;
            }
        }

        /// <summary>
        /// Loop recursivly through parent elements and collect offsets. Needed to operate with
        /// nested tables correctly. This property returns the left offset.
        /// </summary>
        private int ParentOffsetLeft
        {
            get
            {
                int po = 0;
                Interop.IHTMLElement ce = (Interop.IHTMLElement) CurrentTable;
                try
                {
                    while (ce.GetOffsetParent() != null)
                    {
                        po += ((Interop.IHTMLElement)ce).GetOffsetParent().GetOffsetLeft();
                        ce = ce.GetOffsetParent();
                    }
                }
                catch
                {
                }
                return po;
            }
        }
            
        /// <summary>
        /// Loop recursivly through parent elements and collect offsets. Needed to operate with
        /// nested tables correctly. This property returns the top offset. 
        /// </summary>
        private int ParentOffsetTop
        {
            get
            {
                int po = 0;
                Interop.IHTMLElement ce = (Interop.IHTMLElement) CurrentTable;
                try
                {
                    while (ce.GetOffsetParent() != null)
                    {
                        po += ((Interop.IHTMLElement)ce).GetOffsetParent().GetOffsetTop();
                        ce = ce.GetOffsetParent();
                    }
                }
                catch
                {
                }
                return po;
            }
        }

        /// <summary>
        /// Left offset of table including any subsequent offset for nested tables
        /// </summary>
        private int L
        {
            get
            {
                return ((Interop.IHTMLElement) CurrentTable).GetOffsetLeft() + ParentOffsetLeft;
            }
        }

        /// <summary>
        /// Top offset of table including any subsequent offset for nested tables
        /// </summary>
        private int T
        {
            get
            {
                return ((Interop.IHTMLElement) CurrentTable).GetOffsetTop() + ParentOffsetTop;
            }
        }

        /// <summary>
        /// Height of table
        /// </summary>
        private int H
        {
            get
            {
                return  ((Interop.IHTMLElement) CurrentTable).GetOffsetHeight();
            }
        }

        /// <summary>
        /// Width of table. Returns the real width, not the width or style attribute
        /// </summary>
        private int W
        {
            get
            {
                return  ((Interop.IHTMLElement) CurrentTable).GetOffsetWidth();
            }
        }

        /// <summary>
        /// Absolute right border (x) including any offset calculated on base of left border.
        /// </summary>
        private int R
        {
            get
            {
                return L + W;
            }
        }
        /// <summary>
        /// Absolute bottom border (y) including any offset calculated on base of left border.
        /// </summary>
        private int B
        {
            get
            {
                return T + H;
            }
        }


        /// <summary>
        /// This field is true if a cell should loose their style attribute before a new slider operation
        /// sets a new start value. This is delayed from the cell collection gather method to the SetRow/SetCol
        /// methods to avoid flicker.
        /// </summary>
        private bool DelayedCellReset;
        private Interop.IHTMLTableCell DelayedCell;

        /// <summary>
        /// Registers the current state of the slider. Is true during slider move.
        /// In case of begin the moved columns are retrieved.
        /// </summary>
        private bool IsDuringWidthSliderOperation
        {
            get
            {
                return IsInSlider.isDuringWidthSliderOperation;
            }
            set
            {
                CreateWidthSliderCellList(IsInSlider.CurrentCol, value);
                IsInSlider.isDuringWidthSliderOperation = value;
                if (value)
                {
                    batchedUndoUnit = htmlEditor.GetUndoManager("WidthSlider");
                } 
                else 
                {
                    if (batchedUndoUnit.Name == "WidthSlider")
                    {
                        batchedUndoUnit.Close();
                    }
                }

            }
        }

        /// <summary>
        /// Registers the current state of the slider. Is true during slider move.
        /// In case of begin the moved columns are retrieved.
        /// </summary>
        private bool IsDuringHeightSliderOperation
        {
            get
            {
                return IsInSlider.isDuringHeightSliderOperation;
            }
            set
            {
                CreateHeightSliderCellList(IsInSlider.CurrentRow, value);
                IsInSlider.isDuringHeightSliderOperation = value;
                if (value)
                {
                    batchedUndoUnit = htmlEditor.GetUndoManager("HeightSlider");
                } 
                else 
                {
                    if (batchedUndoUnit.Name == "HeightSlider")
                    {
                        batchedUndoUnit.Close();
                    }
                }
            }
        }


        private Interop.IHTMLTableCell GetCell(int row, int col)
        {
            return this.TableFormatter.TableInfo.Item(row, col);
        }

        private Interop.IHTMLElement GetCellElement(int row, int col)
        {
            return this.TableFormatter.TableInfo.Item(row, col) as Interop.IHTMLElement;
        }

        private Interop.IHTMLTableRow GetRow(int row)
        {
            return ((Interop.IHTMLTableRow)CurrentTable.rows.Item(0, 0)) as Interop.IHTMLTableRow;
        }

        /// <summary>
        /// Detects the slider area and return the column or row number or -1 if no inside the area.
        /// As column is always the left one returned (as row the top one), even if the left area of the right cell was detected, 
        /// or the top area of the bottom cell, respectively.
        /// </summary>
        /// <param name="x">X coordinate of mouse</param>
        /// <param name="y">Y coordinate of mouse</param>
        /// <returns>True if inside the area, false else.</returns>
        private bool IsInSliderArea(int x, int y)
        {
            Interop.IHTMLElement te;
            if (IsDuringWidthSliderOperation || IsDuringHeightSliderOperation || this.TableFormatter.TableInfo == null) return false;
            foreach (Interop.IHTMLTableCell tc in this.TableFormatter.TableInfo.Cells.Keys)
            {
                te = (Interop.IHTMLElement) tc;
                // do not detect width slider for the first column, we take care to use the real cel pos here, to
                // detect spanned rows in the first column correctly
                int r = 0;
                int c = 0;
                this.TableFormatter.TableInfo.GetCellPoint(tc, ref r, ref c);
                // detect the height slider
                if (r > 0)
                {
                    if (IsBetween(y, te.GetOffsetTop() - ZONESLW, te.GetOffsetTop() + ZONESLW + HalfCellSpacing)
                        &&
                        IsBetween(x, te.GetOffsetLeft() - ZONESLW, te.GetOffsetLeft() + ZONESLW + te.GetOffsetWidth()))
                    {
                        SetMousePointer(Cursors.HSplit);
                        IsInSlider.IsInHeightSlider = true;
                        IsInSlider.IsInWidthSlider = false;
                        IsInSlider.CurrentRow = r;
                        Interop.IHTMLElement cellElement = GetCellElement(r - 1, c);
                        if (cellElement != null)
                        {
                            this.LeftTopBorder = GetCellElement(r - 1, c).GetOffsetTop();
                        }
                        this.RightBottomBorder = te.GetOffsetTop() + te.GetOffsetHeight();
                        this.ztTableZone = ZoneTypeEnum.None;
                        return true;
                    }
                }
                if (c == 0) continue;                
                // detect the width slider
                if (IsBetween(x, te.GetOffsetLeft() - ZONESLW - HalfCellSpacing, te.GetOffsetLeft() + ZONESLW)
                    &&
                    IsBetween(y, te.GetOffsetTop() - ZONESLW, te.GetOffsetTop() + ZONESLW + te.GetOffsetHeight() + HalfCellSpacing))
                {
                    // slider was detected, so put CurrentTable in "possible" slidermode to move the cellborders
                    // The state is left if the user does not click the mouse now
                    SetMousePointer(Cursors.VSplit);
                    IsInSlider.IsInWidthSlider = true;
                    IsInSlider.IsInHeightSlider = false;
                    IsInSlider.CurrentCol = c;
                    this.LeftTopBorder = GetCellElement(r, c - 1).GetOffsetLeft();
                    this.RightBottomBorder = te.GetOffsetLeft() + te.GetOffsetWidth();
                    this.ztTableZone = ZoneTypeEnum.None;
                    return true;
                } 
            }
            SetMousePointer();
            IsInSlider.IsInWidthSlider = false;
            IsInSlider.IsInHeightSlider = false;
            //            IsDuringWidthSliderOperation = IsDuringHeightSliderOperation = false;
            this.ztTableZone = ZoneTypeEnum.None;
            return false;
        }

        private int HalfCellSpacing
        {
            get
            {
                if (CurrentTable.cellSpacing != null)
                {
                    return (int) Math.Floor(Double.Parse(CurrentTable.cellSpacing.ToString()) / 2.0);
                } 
                else 
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// This method detects the cell selection area (left zone in cell to cell selection, top of columns
        /// for column selection and right zone of left column for row selection).
        /// The method will overwrite the CurrentRow or CurrentCol properties to set the right values for
        /// subsequent parts of the designer.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>true if found a hit zone.</returns>
        private bool GetSelection(int x, int y)
        {
            Interop.IHTMLElement te;
            if (this.TableFormatter.TableInfo == null) return false;
            foreach (Interop.IHTMLTableCell tc in this.TableFormatter.TableInfo.Cells.Keys)
            {
                te = (Interop.IHTMLElement) tc;
                // do not detect width slider for the first column, we take care to use the real cel pos here, to
                // detect spanned rows in the first column correctly
                int r = 0;
                int c = 0;
                this.TableFormatter.TableInfo.GetCellPoint(tc, ref r, ref c);
                // check for row
                if (c == 0)
                {
                    if (IsBetween(x, te.GetOffsetLeft() - ZONEROW, te.GetOffsetLeft() + ZONEROW)
                        &&
                        IsBetween(y, te.GetOffsetTop() + ZONEROW, te.GetOffsetTop() + te.GetOffsetHeight() - ZONEROW)
                        )
                    {
                        IsInCellSelection.CurrentRow = r;
                        IsInCellSelection.CurrentCol = -1;
                        IsInSlider.IsInWidthSlider = false;
                        SetMousePointer(GetMouseCursor(MouseCursorTypeEnum.RightArrow));
                        this.ztTableZone = ZoneTypeEnum.RowSelect;
                        return true;
                    }
                }
                // check for column
                if (r == 0)
                {
                    if (IsBetween(y, te.GetOffsetTop() - ZONECOL, te.GetOffsetTop() + ZONECOL)
                        &&
                        IsBetween(x, te.GetOffsetLeft() + ZONECOL, te.GetOffsetLeft() + te.GetOffsetWidth() - ZONECOL))
                    {
                        IsInCellSelection.CurrentRow = -1;
                        IsInCellSelection.CurrentCol = c;
                        IsInSlider.IsInHeightSlider = false;
                        SetMousePointer(GetMouseCursor(MouseCursorTypeEnum.DownArrow));
                        this.ztTableZone = ZoneTypeEnum.ColumnSelect;
                        return true;
                    }
                }
                // check for cell
                if (IsBetween(x, te.GetOffsetLeft() + ZONECOL, te.GetOffsetLeft() + DZONECOL)
                    &&
                    IsBetween(y, te.GetOffsetTop(), te.GetOffsetTop() + te.GetOffsetHeight() + ZONECOL))
                {
                    IsInSlider.IsInHeightSlider = false;
                    IsInCellSelection.CurrentCol = c;
                    IsInCellSelection.CurrentRow = r;
                    SetMousePointer(GetMouseCursor(MouseCursorTypeEnum.DiagonalArrow));
                    this.ztTableZone = ZoneTypeEnum.CellSelect;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This method returns the table designers binary behavior.
        /// </summary>
        /// <returns>BaseBehavior part of the designer</returns>
        private BaseBehavior GetTableBehavior()
        {
            if (_singletonTableBehavior == null)
            {
                _singletonTableBehavior = new TableDesignerBehavior(this, htmlEditor);
            }
            return (BaseBehavior) _singletonTableBehavior;

        }

        /// <summary>
        /// This Method returns the instance of <c>CellHighLightbehavior</c>. It is a singleton instance, only
        /// one instance per TableDesigner may exist.
        /// </summary>
        /// <returns>TableCellHighLightBehavior instance as <see cref="BaseBehavior"/></returns>
        private BaseBehavior GetCellHighLightBehavior()
        {
            if (_singletonHighLightBehavior == null)
            {
                _singletonHighLightBehavior = new TableCellHighLightBehavior(this, htmlEditor);
            }
            return _singletonHighLightBehavior;
        }

        /// <summary>
        /// Reset the slider state but does not leave design mode.
        /// </summary>
        private void SliderReset()
        {
            IsInSlider.IsInHeightSlider = false;
            IsInSlider.IsInWidthSlider = false;
            this.IsDuringHeightSliderOperation = false;
            this.IsDuringWidthSliderOperation = false;
            this.IsResizeMode = false;
            this.ResizeDirection = ResizeModeEnum.None;
        }

        # endregion

        # region Public Properties

        /// <summary>
        /// Gets the cellstack and creates the stack object on first call.
        /// </summary>
        private HighLightCells CellStack
        {
            get
            {
                if (cellStack == null)
                {
                    cellStack = new HighLightCells(CurrentTable, cellSelectionColor, cellSelectionBorderColor, this.htmlEditor);
                }
                return cellStack;
            }
        }

        /// <summary>
        /// Set the cellstack in the formatter object. Merge operations are based on the stack there. To give
        /// the table formatter access to the right table the CurrentTable is send. Even fired if the collection
        /// is empty to inform external handler that there is nothing left to do.
        /// </summary>
        internal void FireTableCellSelection()
        {
            // FIRE Event in Case of Cell Stack ops, the host app must do something with the cells. 
            if (this.CellStack != null)
            {
                ((TableFormatter)this.TableFormatter).OnTableCellSelection(CurrentTable, this.CellStack.HighLightCellCollection);
            }
        }

        /// <summary>
        /// Get the current state of the table designer.
        /// </summary>
        /// <remarks>
        /// Always returns <c>false</c> if the NetRix component is not ready yet.
        /// </remarks>
        public bool IsActivated
        {
            get
            {
                return this._isActivated && htmlEditor.IsReady;
            }
        }

        private bool processTABKey;
        private bool staticBehavior;

        internal void SetProperties(TableDesignerProperties properties)
        {
            SetProperties(properties, "");
        }

        /// <summary>
        /// Set the current state of the table designer.
        /// </summary>
        /// <remarks>
        /// The additonal setter method prevents users of this class to set the state from outside, which results in an unpredictable 
        /// program state. The preferred call is from constructor and from ExtenderProvider class.
        /// </remarks>
        /// <param name="properties">Sets the designer properties</param>
        /// <param name="property">The name of the property.</param>
        internal void SetProperties(TableDesignerProperties properties, string property)
        {
            switch (property)
            {
                case "Active":
                    this._isActivated = properties.Active;
                    break;
                case "CellSelectionColor":
                    cellSelectionColor = properties.CellSelectionColor;
                    break;
                case "SliderAddMode":
                    sliderAddMode = properties.SliderAddMode;
                    break;
                case "SliderLineWidth":
                    BodySliderBehavior.LinePen = new System.Drawing.Pen(properties.SliderLine.SliderLineColor, (float)properties.SliderLine.SliderLineWidth);
                    break;
                case "CellSelectionBorderColor":
                    cellSelectionBorderColor = properties.CellSelectionBorderColor;
                    break;
                case "WithCellSelection":
                    withCellSelection = properties.WithCellSelection;
                    break;
                case "SliderActivated":
                    sliderActivated = properties.SliderActivated;
                    break;
                case "FastResizeMode":
                    fastResizeMode = properties.FastResizeMode;
                    break;
                case "AdvancedParameters":
                    advancedParameters = properties.AdvancedParameters;
                    break;
                case "ProcessTABKey":
                    processTABKey = properties.ProcessTABKey;
                    break;
                case "StaticBehavior":
                    staticBehavior = properties.StaticBehavior;
                    break;
                case "TableBackground":
                    ((TableDesignerBehavior)GetTableBehavior()).TableBackground = properties.TableBackground;
                    break;
                default:
                    this._isActivated = properties.Active;
                    sliderAddMode = properties.SliderAddMode;
                    BodySliderBehavior.LinePen = new System.Drawing.Pen(properties.SliderLine.SliderLineColor, (float)properties.SliderLine.SliderLineWidth);
                    cellSelectionColor = properties.CellSelectionColor;
                    cellSelectionBorderColor = properties.CellSelectionBorderColor;
                    withCellSelection = properties.WithCellSelection;
                    sliderActivated = properties.SliderActivated;
                    fastResizeMode = properties.FastResizeMode;
                    advancedParameters = properties.AdvancedParameters;
                    processTABKey = properties.ProcessTABKey;
                    staticBehavior = properties.StaticBehavior;
                    ((TableDesignerBehavior)GetTableBehavior()).TableBackground = properties.TableBackground;
                    break;
            }
        }

        /// <summary>
        /// Remove the beaviors from all cells.
        /// </summary>
        /// <param name="el"></param>
        internal void RemoveBehaviors(Interop.IHTMLTable el)
        {
            try
            {
                // no cookies stored, nothing to remove
                if (elementCookie.Count == 0 ) return;
                // add cell behaviors to each cell individually, this creates the dotted borders
                for (int r = 0; r < el.rows.GetLength(); r++)
                {
                    Interop.IHTMLTableRow tr = (Interop.IHTMLTableRow) el.rows.Item(r, r);
                    for (int c = 0; c < tr.cells.GetLength(); c++)
                    {
                        Interop.IHTMLElement2 cell = (Interop.IHTMLElement2) tr.cells.Item(c, c);
                        //pFactory = new TableCellBorderBehavior(this, htmlEditor, cell); // this.GetCellBorderBehavior();
                        object cookie = elementCookie[cell];
                        //System.Diagnostics.Debug.WriteLine("Remove Cookie", cookie.ToString());
                        if (cookie != null)
                        {
                            cell.RemoveBehavior(Convert.ToInt32(cookie));
                            elementCookie.Remove(cell);
                        }
                        foreach (int cs in elementCookie.Values)
                        {
                            cell.RemoveBehavior(cs);
                        }
                    }
                }
                // add table behavior at last to overwrite cell behaviors during drawing 
                object cookie2 = elementCookie[el];
                if (cookie2 != null)
                {
                    ((Interop.IHTMLElement2) el).RemoveBehavior(Convert.ToInt32(cookie2));
                    elementCookie.Remove(el);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Add the designer behaviors to the table. Sets white background and dotted cell borders for all tables.
        /// </summary>
        /// <param name="el">Table element which the behaviors are attached to</param>
        internal void AddBehaviors(Interop.IHTMLTable el)
        {           
            System.Diagnostics.Debug.WriteLine("AddBehavior");
            object behavior;
            RemoveBehaviors(el);
            try
            {
                // add cell behaviors to each cell individually, this creates the dotted borders
                // the cell behavior is defined in a subclass of TableDesigner
                for (int r = 0; r < el.rows.GetLength(); r++)
                {
                    Interop.IHTMLTableRow tr = (Interop.IHTMLTableRow) el.rows.Item(r, r);
                    for (int c = 0; c < tr.cells.GetLength(); c++)
                    {
                        Interop.IHTMLElement2 cell = (Interop.IHTMLElement2) tr.cells.Item(c, c);
                        behavior = new TableCellBorderBehavior(this, htmlEditor, cell); // this.GetCellBorderBehavior();                    
                        try
                        {
                            int cookie = cell.AddBehavior("CellBehavior", ref behavior);
                            if (!elementCookie.Contains(cell))
                            {
                                elementCookie.Add(cell, cookie);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                // add table behavior at last to overwrite cell behaviors during drawing 
                behavior = this.GetTableBehavior();
                object cookie2 = ((Interop.IHTMLElement2) el).AddBehavior("TableBehavior", ref behavior);
                if (!elementCookie.Contains(el) && cookie2 != null)
                {
                    elementCookie.Add(el, Convert.ToInt32(cookie2));
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Add the current behavior to all tables on the page. 
        /// </summary>
        public void AddBehaviors()
        {
            if (staticBehavior)
            {
                ElementCollection tables = this.htmlEditor.GetElementsByTagName("TABLE");
                if (tables != null && tables.Count > 0)
                {
                    //RemoveBehaviors();
                    foreach (IElement table in tables)
                    {
                        AddBehaviors((Interop.IHTMLTable) table.GetBaseElement());
                    }
                }
            }
        }

        /// <summary>
        /// Add the current behavior to all tables on the page. 
        /// </summary>
        public void RemoveBehaviors()
        {
            if (staticBehavior)
            {
                ElementCollection tables = this.htmlEditor.GetElementsByTagName("TABLE");
                if (tables != null && tables.Count > 0)
                {
                    foreach (IElement table in tables)
                    {
                        RemoveBehaviors((Interop.IHTMLTable) table.GetBaseElement());
                    }
                }
            }
        }

        /// <summary>
        /// Activate/Deactivate the designer using extender command.
        /// </summary>
        /// <param name="active"></param>
        public void Activate(bool active)
        {
            this._isActivated = active;
        }

        # endregion 

        # region Register Mouse Position and Mouse Cursor methods

        /// <summary>
        /// Detects the mouse in the activator icon to support entering the advanced designer mode
        /// </summary>
        /// <param name="x">X coordinate of mouse</param>
        /// <param name="y">Y coordinate of mouse</param>
        /// <returns>True, if mouse is over activator icon</returns>
        private bool IsOnActivator(int x, int y)
        {
            if (((Math.Abs(x - this.L - 10) <= 6) && Math.Abs(y - this.T - 10) <= 6))
            {
                SetMousePointer(Cursors.Hand);
                return true;
            } 
            else 
            {
                return false;
            }
        }

        /// <summary>
        /// Detects the mouse over a resizing handle. This method detects the build in handles in the lower right corner,
        /// the middle handle on the right side and the middle handle on the bottom side. It does not change the mouse 
        /// cursor. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsOnResize(int x, int y)
        {
            if (this.htmlEditor.Selection.GetUnsynchronizedElement() != null
				&&
				this.htmlEditor.Selection.GetUnsynchronizedElement().TagName == ((Interop.IHTMLElement) this.CurrentTable).GetTagName())
            {
                int Offset = 0; // do reach handles placed outside the table area
                int halfX = (this.R + this.L) / 2 + Offset;
                int halfY = (this.T + this.B) / 2 + Offset;       
                if (IsBetween(x, halfX, halfX + HANDLESIZE) && IsBetween(y, this.B - HANDLESIZE, this.B + HANDLESIZE))
                {
                    // bottom middle side
                    this.ResizeDirection = ResizeModeEnum.NS;
                    return true;
                }
                if (IsBetween(x, this.R - HANDLESIZE, this.R + HANDLESIZE) && IsBetween(y, this.B - HANDLESIZE, this.B + HANDLESIZE))
                {
                    // lower right corner
                    this.ResizeDirection = ResizeModeEnum.NWSE;
                    return true;
                }
                if (IsBetween(x, this.R - HANDLESIZE, this.R + HANDLESIZE) && IsBetween(y, halfY - HANDLESIZE, halfY + HANDLESIZE))         
                {
                    // right middle side
                    this.ResizeDirection = ResizeModeEnum.WE;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Creates a list of cells which are set during the slider operation.
        /// </summary>
        /// <param name="LastColumn"></param>
        /// <param name="value"></param>
        private void CreateWidthSliderCellList(int LastColumn, bool value)
        {
            if (WidthSliderCellList == null)
                WidthSliderCellList = new ArrayList();
            WidthSliderCellList.Clear();
            if (value)
            {
                DelayedCellReset = false;
                for (int r = 0; r < this.TableFormatter.TableInfo.GetRowNumber(); r++)
                {
                    Interop.IHTMLTableCell c1 = this.TableFormatter.TableInfo.Item(r, LastColumn - 1);
                    Interop.IHTMLTableCell c2 = this.TableFormatter.TableInfo.Item(r, LastColumn);
                    // check out if left and right cell are different, if not they are spanned and should not changed
                    if (c1 != null && c2 != null)
                    {
                        if (c1.GetHashCode() != c2.GetHashCode())
                        {
                            WidthSliderCellList.Add(new System.Web.UI.Pair(c1, c2));
                        } 
                        else 
                        {
                            // this is a spanned cell, if it is the first cell in the collection we need to reset
                            // the style attribute because the upper most cell is dominant above all other cells!
                            if (r == 0)
                            {
                                DelayedCellReset = true;                                
                                DelayedCell = c1;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates a list of cells which are set during the slider operation.
        /// </summary>
        /// <param name="LastRow"></param>
        /// <param name="value"></param>
        private void CreateHeightSliderCellList(int LastRow, bool value)
        {
            if (HeightSliderCellList == null)
                HeightSliderCellList = new ArrayList();
            HeightSliderCellList.Clear();
            if (value)
            {
                DelayedCellReset = false;
                for (int c = 0; c < this.TableFormatter.TableInfo.GetColNumber(LastRow); c++)
                {
                    Interop.IHTMLTableCell c1 = this.TableFormatter.TableInfo.Item(LastRow - 1, c);
                    Interop.IHTMLTableCell c2 = this.TableFormatter.TableInfo.Item(LastRow, c);
                    // check out if left and right cell are different, if not they are spanned and should not changed
                    if (c1 != null && c2 != null && c1.GetHashCode() != c2.GetHashCode())
                    {
                        HeightSliderCellList.Add(new System.Web.UI.Pair(c1, c2));
                    } 
                    else 
                    {
                        if (c == 0)
                        {   
                            DelayedCellReset = true;
                            DelayedCell = c1;
                        }
                    }
                }
            } 
        }

        /// <summary>
        /// This method swaps two variables so the second one has always the bigger value.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns>true, if the values are different, false if the are equal.</returns>
        private bool SwapHighVariables(ref int first, ref int second)
        {
            if (second == first) return false;
            int temp = first;            
            if (second > first)
            {
                first = second;
                second = temp;                
            }
            return true;
        }

        /// <summary>
        /// Checks id a value is between to boundaries (including the boundary value).
        /// </summary>
        /// <param name="check">Value to check</param>
        /// <param name="min">Lower boundary</param>
        /// <param name="max">Upper boundary</param>
        /// <returns></returns>
        private bool IsBetween(int check, int min, int max)
        {
            if (check <= max && check >= min)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Froze old mouse pointer and reload with a different one
        /// </summary>
        /// <param name="cursor"></param>
        private void SetMousePointer(Cursor cursor)
        {
            this.htmlEditor.SetMousePointer(true);
            Cursor.Current = cursor;
        }

        /// <summary>
        /// Reset mousepointer from frozen state and activate original one
        /// </summary>
        private void SetMousePointer()
        {
            this.htmlEditor.SetMousePointer(false);
        }

        /// <summary>
        /// Loads mouse pointer from resource
        /// </summary>
        /// <param name="MouseType">Cursor needed</param>
        /// <returns>Cursor returns</returns>
        private System.Windows.Forms.Cursor GetMouseCursor(MouseCursorTypeEnum MouseType)
        {
            string s = "GuruComponents.Netrix.TableDesigner.Resources." + astrCursorsName[(int)MouseType];
            return new System.Windows.Forms.Cursor(this.GetType().Assembly.GetManifestResourceStream(s));
        }

        private int GetCellWidth(Interop.IHTMLElement cell)
        {
            return (cell != null) ? cell.GetOffsetWidth() : 0;
        }

        private int GetCellHeight(Interop.IHTMLElement cell)
        {
            return (cell != null) ? cell.GetOffsetHeight() : 0;
        }

        internal int GetCellTop(Interop.IHTMLElement cell)
        {
            return (cell != null) ? cell.GetOffsetTop() : 0;
        }

        internal int GetCellLeft(Interop.IHTMLElement cell)
        {
            return (cell != null) ? cell.GetOffsetLeft() : 0;
        }

        private int PercentToPixel(decimal p_Size, int p_ParentSize)
        {
            return Convert.ToInt32(p_Size * p_ParentSize / 100);
        }

        /// <summary>
        /// Get the table the current element is in. If the element is not in a table the method returns null.
        /// </summary>
        /// <returns>Table element or null if no table found.</returns>
        private Interop.IHTMLElement ParentTable()
        {
            Interop.IHTMLElement p = ((Interop.IHTMLElement) CurrentTable).GetParentElement();
            while(p != null)
            {
                if(p.GetTagName() == "TABLE") break;
                p = p.GetParentElement();
            }
            return p;
        }

        /// <summary>
        /// Checks a value for the size type, pixel or percent.
        /// </summary>
        /// <param name="p_Size"></param>
        /// <returns></returns>
        private ESizeType SizeType(string p_Size)
        {
            ESizeType res = ESizeType.None;

            if(p_Size.EndsWith("px"))
                res = ESizeType.Pixel;
            else
                res = ESizeType.Percent;

            return res;
        }

        /// <summary>
        /// Converts a string into a pixel value by adding "px".
        /// </summary>
        /// <param name="p_Data"></param>
        /// <returns></returns>
        private int StringToPixel(string p_Data)
        {
            int res = 0;

            if(SizeType(p_Data) == ESizeType.Pixel)
                res = Int32.Parse(p_Data.Replace("px", ""));

            return res;
        }

        /// <summary>
        /// Sets a new value and preserve the previously used type.
        /// </summary>
        /// <param name="p_Size"></param>
        /// <param name="p_ParentSize"></param>
        /// <returns></returns>
        private int SizeInPixel(string p_Size, int p_ParentSize)
        {
            int ret = 0;
            ESizeType t = SizeType(p_Size);

            if(t == ESizeType.Pixel)
                ret = StringToPixel(p_Size);
            else
                ret = PercentToPixel(StringToPercent(p_Size), p_ParentSize);

            return ret;
        }

        /// <summary>
        /// Set a percentage value.
        /// </summary>
        /// <param name="p_Data"></param>
        /// <returns></returns>
        private decimal StringToPercent(string p_Data)
        {
            System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo();
            decimal res = 0;

            nf.CurrencyDecimalSeparator = ".";
            if(SizeType(p_Data) == ESizeType.Percent)
                res = Decimal.Parse(p_Data.Replace("%", ""), nf);

            return res;
        }

        /// <summary>
        /// Convert a numeric value in the pixel representation by adding "px" and returning as string.
        /// </summary>
        /// <param name="p_Data"></param>
        /// <returns></returns>
        private string PixelToString(int p_Data)
        {
            return string.Concat(p_Data.ToString(), "px");
        }

        /// <summary>
        /// The width from style:width attribute.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private int GetCssWidth(Interop.IHTMLElement cell)
        {
            if (cell.GetStyle().GetWidth() == null)
            {
                SetCssWidth(GetCellWidth(cell), cell);
            }
            return SizeInPixel(cell.GetStyle().GetWidth().ToString(), 0 /*ParentOffsetWidth*/);
        }

        /// <summary>
        /// The real cell width.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cell"></param>
        private void SetCellWidth(int value, Interop.IHTMLElement cell)
        {
            cell.SetAttribute("width", value, 0);
        }

        /// <summary>
        /// The real cell height.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cell"></param>
        private void SetCellHeight(int value, Interop.IHTMLElement cell)
        {
            cell.SetAttribute("height", value, 0);
        }

        /// <summary>
        /// Set the width by using the style:width attribute.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cell"></param>
        private void SetCssWidth(int value, Interop.IHTMLElement cell)
        {
            cell.GetStyle().SetWidth(PixelToString(Math.Max(1, value)));
        }

        /// <summary>
        /// Remove the style:width attribute.
        /// </summary>
        /// <param name="cell"></param>
        private void ResetCssWidth(Interop.IHTMLElement cell)
        {
            cell.GetStyle().RemoveAttribute("width", 0);
        }

        /// <summary>
        /// Remove the style:height attribute.
        /// </summary>
        /// <param name="cell"></param>
        private void ResetCssHeight(Interop.IHTMLElement cell)
        {
            cell.GetStyle().RemoveAttribute("height", 0);
        }

        /// <summary>
        /// The style:height attribute value.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private int GetCssHeight(Interop.IHTMLElement cell)
        {
            // assuming that the setted value is always in pixel
            if (cell.GetStyle().GetHeight() == null)
            {
                SetCssHeight(GetCellHeight(cell), cell);
            }
            return SizeInPixel(cell.GetStyle().GetHeight().ToString(), 0/*ParentOffsetHeight*/);
        }
           
        /// <summary>
        /// Set the style:height value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cell"></param>
        private void SetCssHeight(int value, Interop.IHTMLElement cell)
        {
            cell.GetStyle().SetHeight(PixelToString(Math.Max(1, value)));
        }

        /// <summary>
        /// Set the Column to the last position of the slider after each mouse move.
        /// </summary>
        /// <param name="Column">Column, one based. If -1 suppress changing left cell.</param>
        /// <param name="XPosition">Position in Client coordinates</param>
        /// <returns>The last real position of the slider. Used to set the dashed line.</returns>
        private int SetColumn(int Column, int XPosition)
        {
            if (WidthSliderCellList == null) return -1;
            Interop.IHTMLElement el1 = null;
            Interop.IHTMLElement el2 = null;
            if (DelayedCellReset)
            {
                DelayedCellReset = false;
                ResetCssWidth((Interop.IHTMLElement) DelayedCell);
            }
            foreach (System.Web.UI.Pair cellPair in WidthSliderCellList)
            {
                el1 = (Interop.IHTMLElement) cellPair.First;
                el2 = (Interop.IHTMLElement) cellPair.Second;
                int h1 = GetCssWidth(el1);
                int r1 = GetCellWidth(el1);
                int d = GetCellLeft(el2) - XPosition;
                int h2 = GetCssWidth(el2);        
                int r2 = GetCellWidth(el2);
                SetCssWidth(h1 - d, el1);
                if (!sliderAddMode)
                {
                    SetCssWidth(h2 + d, el2);
                }
                if (GetCellWidth(el1) == r1 || (!sliderAddMode && GetCellWidth(el2) == r2))
                {
                    SetCssWidth(h1, el1);
                    if (!sliderAddMode)
                    {
                        SetCssWidth(h2, el2);
                    }
                }
                break;
            }
            return GetCellLeft(el2) + this.L - 2;
        }

        /// <summary>
        /// Set the Row to the last position of the slider after each mouse move.
        /// </summary>
        /// <param name="Row">Row, one based</param>
        /// <param name="YPosition">Position in Client coordinates</param>
        /// <returns>The last real position of the slider. Used to set the dashed line.</returns>
        private int SetRow(int Row, int YPosition)
        {
            if (HeightSliderCellList == null) return -1;
            Interop.IHTMLElement el1 = null;
            Interop.IHTMLElement el2 = null;
            if (DelayedCellReset)
            {
                DelayedCellReset = false;
                ResetCssWidth((Interop.IHTMLElement) DelayedCell);
            }
            foreach (System.Web.UI.Pair cellPair in HeightSliderCellList)
            {
                el1 = (Interop.IHTMLElement) cellPair.First;
                el2 = (Interop.IHTMLElement) cellPair.Second;                
                int h1 = GetCssHeight(el1);
                int r1 = GetCellHeight(el1);
                int h2 = GetCssHeight(el2);        
                int r2 = GetCellHeight(el2);
                int d  =  GetCellTop(el2) - YPosition;
                SetCssHeight(h1 - d, el1);
                if (!sliderAddMode)
                {
                    SetCssHeight(h2 + d, el2);
                }
                //else
                {
                    if (GetCellHeight(el1) == r1 || (!sliderAddMode && GetCellHeight(el2) == r2))
                    {
                        SetCssHeight(h1, el1);
                        if (!sliderAddMode)
                        {
                            SetCssHeight(h2, el2);
                        }
                    }
                }
                break;
            }
            return GetCellTop(el2) + this.T - 2;
        }

        /// <summary>
        /// This method operates the table resizing by changing the width and height style.
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        private void ResizeTable(int newX, int newY)
        {
            //if (this.ResizeDirection == ResizeModeEnum.WE || this.ResizeDirection == ResizeModeEnum.NWSE)
            //{
            //    SetColumn(-1, newX);
            //}
            //if (this.ResizeDirection == ResizeModeEnum.NS || this.ResizeDirection == ResizeModeEnum.NWSE)
            //{
            //    SetRow(-1, newY);
            //}
        }

        # endregion

        # region Table TAB key manager

        /// <summary>
        /// Sets the caret to a specific cell using MarkupServices.
        /// </summary>
        /// <param name="nextCell"></param>
        /// <returns></returns>
        private bool SetCaretToCell(Interop.IHTMLTableCell nextCell)
        {
            try
            {
                Interop.IMarkupServices ms = (Interop.IMarkupServices) this.htmlEditor.GetActiveDocument(false);
                Interop.IMarkupPointer mp;
                ms.CreateMarkupPointer(out mp);
                // CONFIG:
                // ELEM_ADJ_BeforeEnd places the caret to the end of the text in cell
                // You may alter this behavior by setting ELEM_ADJ_AfterBegin, this sets the caret before any text
                mp.MoveAdjacentToElement((Interop.IHTMLElement) nextCell, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeEnd);
                Interop.IDisplayServices ds = (Interop.IDisplayServices) this.htmlEditor.GetActiveDocument(false);
                Interop.IDisplayPointer dp;
                ds.CreateDisplayPointer(out dp);
                dp.MoveToMarkupPointer(mp, null);
                Interop.IHTMLCaret cr;
                ds.GetCaret(out cr);
                cr.MoveCaretToPointer(dp, true, Interop.CARET_DIRECTION.CARET_DIRECTION_SAME);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private int ParentOffsetWidth
        {
            get
            {
                if (ParentTable() != null)
                {
                    return ParentTable().GetOffsetWidth();
                } 
                else 
                {
                    return 0;
                }
            }
        }

        private int ParentOffsetHeight
        {
            get
            {
                if (ParentTable() != null)
                {
                    return ParentTable().GetOffsetHeight();
                } 
                else 
                {
                    return 0;
                }
            }
        }

        # endregion

        private Interop.IHTMLElement GetParentElement(Interop.IHTMLElement element, string tagName)
        {
            if (element == null) return null;
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

        #region IHTMLEditDesigner Member

        /// <summary>
        /// The implementation of <c>PreHandleEvent</c>. This method is called as a callback before an event is processed
        /// by other hosts or MSHTML.
        /// <para>
        /// This method does only cover mouse related operations. To set the current table for formatting options the
        /// <see cref="GuruComponents.Netrix.MSHTMLSite"/> class covers key and click events and sets the current table only when the caret is
        /// inside the table. That means that table formatting depends on the caret, whereas table resizing depends on
        ///  the mouse position. There is one exception from this rule: If the user selects cells with the highlight service
        ///  this means that cells are well defined, even when the caret is outside the table. In that special case
        ///  the current table is set too and the formatter options can applied to the selection.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.ComInterop.Interop.IHTMLEditDesigner"/>
        /// </summary>
        /// <param name="dispId">The basic event type</param>
        /// <param name="e">The event object, which contains details about event source and type</param>
        /// <returns>S_OK if no further processing needed, S_FALSE else.</returns>
        int Interop.IHTMLEditDesigner.PreHandleEvent(int dispId, GuruComponents.Netrix.ComInterop.Interop.IHTMLEventObj e)
        {
            preHandleReturnState = Interop.S_FALSE;
            object pFactory;
            if (body == null) return preHandleReturnState;
            // the mouse related events are used for the table "redesign"
            if (this.IsActivated && htmlEditor.DesignModeEnabled)   // mouse actions only
            {
                // this designer is only active if anything happens inside a table                    
                if (e.button == NONE && dispId == DispId.MOUSEMOVE)
                {
                    CurrentTable = GetParentElement(e.srcElement, "TABLE") as Interop.IHTMLTable;                    
                    if (CurrentTable == null) return preHandleReturnState;
                } 
                // set Formatter only on key or click because move opt does not change caret position
                if (dispId == DispId.KEYDOWN || dispId == DispId.MOUSEDOWN)
                {
                    if (OldCurrentTable == null || CurrentTable == null || !OldCurrentTable.Equals(CurrentTable))
                    {
                        this.TableFormatter.TableInfo = null;
                    }
                    ((TableFormatter)this.TableFormatter).CurrentTable = CurrentTable;
                }                
                if (CurrentTable == null || !CurrentTable.readyState.Equals("complete"))
                {
                    SetMousePointer();  // if we go outside any table reset the mouse pointer
                }
                else
                {                    
                    // Get current coordinates and respect scroll position
                    this.x = e.clientX + body2.GetScrollLeft();
                    this.y = e.clientY + body2.GetScrollTop();
                    int EndPositionX = x - this.L;
                    int EndPositionY = y - this.T;
                    switch (dispId)
                    {
                        case DispId.MOUSEUP:
                            # region MOUSEUP
                            if (fastResizeMode)
                            {
                                if (IsDuringWidthSliderOperation)
                                {
                                    SetColumn(IsInSlider.CurrentCol, EndPositionX);
                                }
                                if (IsDuringHeightSliderOperation)
                                {
                                    SetRow(IsInSlider.CurrentRow, EndPositionY);
                                }
                            }
                            body2.RemoveBehavior(sliderCookie);
                            // despite which state the table currently is in, we release the slider state when mouse goes up
                            
                            if (this.IsDuringWidthSliderOperation || this.IsDuringHeightSliderOperation)
                            {
                                this.SliderReset();
                                
                            }
                            this.IsResizeMode = false;
                            // reset always the mouse pointer
                            SetMousePointer();
                            // Fire table cell selection, if any selection was stored
                            FireTableCellSelection();
                            this.IsInCellSelection.StartCellSelection = false;
                            IsInCellSelection.InCellSelection = false;
                            // 
                            preHandleReturnState = Interop.S_FALSE;
                            break;

                            # endregion
                        case DispId.MOUSEMOVE:
                            # region MOUSEMOVE
                            # region MOUSEMOVE NO BUTTON
                            if (e.button == NONE)
                            {
                                // The sensibility of mouse depends on the selection priority
                                // PRIO 1: Slider
                                if (IsInSliderArea(EndPositionX, EndPositionY)) // if TABLE, designmode, detect columns or rows 
                                {
                                    preHandleReturnState = Interop.S_OK;
                                    break;
                                }
                                // PRIO 2: Activator/Context
                                if (IsOnActivator(x, y))
                                {
                                    this.PreDesignMode = true;
                                    preHandleReturnState = Interop.S_OK;
                                    break;
                                } 
                                else 
                                {
                                    this.PreDesignMode = false;
                                }
                                // PRIO 3: Handles, used to reset outer cells to reformat table widht/height
                                if (IsOnResize(x, y))
                                {
                                    this.PreResizeMode = true;
                                    break;
                                } 
                                else 
                                {
                                    this.PreResizeMode = false;
                                }
                                if (withCellSelection && GetSelection(EndPositionX, EndPositionY))
                                {
                                    preHandleReturnState = Interop.S_OK;
                                    IsInCellSelection.StartCellSelection = true;
                                    break;
                                }
                                SetMousePointer();
                                preHandleReturnState = Interop.S_FALSE;
                                break;
                            }
                            # endregion
                            # region MOUSEMOVE LEFT BUTTON
                            if (e.button == LEFT)
                            {
                                // Selection by simple move
                                if (e.srcElement is Interop.IHTMLTableCell && !e.srcElement.Equals(lastCell)
                                    && !IsDuringWidthSliderOperation
                                    && !IsDuringHeightSliderOperation
                                    && withCellSelection)
                                {
                                    if (lastCell != null)
                                    {
                                        this.CellStack.AddCell(lastCell);
                                        this.CellStack.AddCell((Interop.IHTMLTableCell)e.srcElement);
                                        htmlEditor.Selection.ClearSelection();
                                    }
                                    lastCell = (Interop.IHTMLTableCell)e.srcElement;
                                }
                                // Sliders
                                if (this.IsDuringWidthSliderOperation)
                                {
                                    this.SliderWasMoved = true;
                                    SetMousePointer(Cursors.VSplit);
                                    preHandleReturnState = Interop.S_OK;
                                    if (EndPositionX <= this.LeftTopBorder) break;	// not beyond the left border
                                    if (EndPositionX >= this.RightBottomBorder) break;
                                    if (fastResizeMode)
                                    {
                                        BodySliderBehavior.X = x;
                                    } 
                                    else 
                                    {
                                        BodySliderBehavior.X = SetColumn(IsInSlider.CurrentCol, EndPositionX);
                                    }
                                    break;
                                }
                                if (this.IsDuringHeightSliderOperation)
                                {
                                    this.SliderWasMoved = true;
                                    SetMousePointer(Cursors.HSplit);
                                    preHandleReturnState = Interop.S_OK;
                                    if (EndPositionY <= this.LeftTopBorder) break;	// not beyond the top border
                                    if (EndPositionY >= this.RightBottomBorder) break;
                                    if (fastResizeMode)
                                    {
                                        BodySliderBehavior.Y = y;
                                    } 
                                    else 
                                    {
                                        BodySliderBehavior.Y = SetRow(IsInSlider.CurrentRow, EndPositionY);
                                    }
                                    break;
                                }
                                if (IsInCellSelection.InCellSelection)
                                {
                                    // look for the next cell the pointer is positioned in, zone doesn't matter here
                                    Interop.IHTMLElement elTD = this.GetParentElement(e.srcElement, "TD");
                                    if (elTD == null) break;
                                    Interop.IHTMLTableCell tcCurrent = (Interop.IHTMLTableCell) elTD;
                                    int tcCurrentRow = 0, tcCurrentCol = 0;
                                    this.TableFormatter.TableInfo.GetCellPoint(tcCurrent, ref tcCurrentRow, ref tcCurrentCol);
                                    if (this.ztTableZone == ZoneTypeEnum.CellSelect)
                                    {                                        
                                        if (tcCurrent != null)
                                        {
                                            SetMousePointer(GetMouseCursor(MouseCursorTypeEnum.DiagonalArrow));
                                            Interop.IHTMLTableCell tcFirst = this.CellStack.GetFirstCell();
                                            if (tcFirst == null || e.ctrlKey)
                                            {
                                                // first cell, simply add current and make them the first
                                                this.CellStack.AddCell(tcCurrent);
                                            } 
                                            else 
                                            {
                                                // has cell previously added, calculate rectangle and set all cells
                                                int rowFirst = 0, colFirst = 0;
                                                this.TableFormatter.TableInfo.GetCellPoint(tcFirst, ref rowFirst, ref colFirst);
                                                // normalize the values to run from the lowest to highest, assure that the right expression is always evaluated
                                                if (SwapHighVariables(ref rowFirst, ref tcCurrentRow) | SwapHighVariables(ref colFirst, ref tcCurrentCol))
                                                {
                                                    // looping through the rectangle to add cells
                                                    for (int c = tcCurrentCol; c <= colFirst; c++)
                                                    {
                                                        for (int r = tcCurrentRow; r <= rowFirst; r++)
                                                        {
                                                            this.CellStack.AddCell(this.GetCell(r, c));
                                                        }
                                                    }
                                                }
                                            }
                                            preHandleReturnState = Interop.S_OK;
                                        }
                                        break;
                                    }
                                    if (this.ztTableZone == ZoneTypeEnum.RowSelect)
                                    {
                                        // select from start cell to end cell of selected row
                                        foreach (Interop.IHTMLTableCell tc in this.TableFormatter.TableInfo.Cells.Keys)
                                        {
                                            int r = 0;
                                            int c = 0;
                                            this.TableFormatter.TableInfo.GetCellPoint(tc, ref r, ref c);
                                            if (r == tcCurrentRow)
                                            {
                                                this.CellStack.AddCell(tc);
                                            }
                                        }
                                        preHandleReturnState = Interop.S_OK;
                                        break;
                                    } 
                                    if (this.ztTableZone == ZoneTypeEnum.ColumnSelect)
                                    {
                                        // select from start cell to end cell of selected row
                                        foreach (Interop.IHTMLTableCell tc in this.TableFormatter.TableInfo.Cells.Keys)
                                        {
                                            int r = 0;
                                            int c = 0;
                                            this.TableFormatter.TableInfo.GetCellPoint(tc, ref r, ref c);
                                            if (c == tcCurrentCol)
                                            {
                                                this.CellStack.AddCell(tc);
                                            }
                                        }
                                        preHandleReturnState = Interop.S_OK;
                                        break;
                                    } 
                                }
                                if (this.IsResizeMode)
                                {
                                    // one of the handles was clicked. 
                                    ResizeTable(x, y);
                                    this.htmlEditor.LiveResize = true;
                                    preHandleReturnState = Interop.S_FALSE;
                                    break;
                                }
                            }
                            # endregion
                            SetMousePointer();
                            preHandleReturnState = Interop.S_FALSE;
                            break;
                            # endregion
                        case DispId.MOUSEDOWN:
                            # region MOUSEDOWN
                            // check for table is in slider mode and button goes down, then start moving the border
                            // subsequent moves will then change the cell widht/height
                            if (e.button == LEFT)
                            {
                                lastCell = null;
                                if (IsInSlider.IsInWidthSlider)
                                {
                                    IsDuringWidthSliderOperation = true;
                                    SetMousePointer(Cursors.VSplit);
                                    if (this.sliderActivated)
                                    {
                                        BodySliderBehavior.IsVertical = true;
                                        pFactory = BodySliderBehavior;
                                        sliderCookie = body2.AddBehavior("BodySliderBehavior", ref pFactory);
                                        BodySliderBehavior.X = this.x - 2;  // must set after adding, because this forces invalidate, and it fails if not attached
                                    }
                                    preHandleReturnState = Interop.S_OK;
                                    break;
                                }
                                if (IsInSlider.IsInHeightSlider)
                                {
                                    IsDuringHeightSliderOperation = true;
                                    SetMousePointer(Cursors.HSplit);
                                    if (this.sliderActivated)
                                    {
                                        BodySliderBehavior.IsVertical = false;
                                        pFactory = BodySliderBehavior;
                                        sliderCookie = body2.AddBehavior("BodySliderBehavior", ref pFactory);
                                        BodySliderBehavior.Y = this.y - 2;  // must set after adding, because this forces invalidate, and it fails if not attached
                                    }
                                    preHandleReturnState = Interop.S_OK;
                                    break;
                                }
                                // if was predesignmode and mouse clicked, fire table context menu event
                                if (this.PreDesignMode)
                                {
                                    Interop.IHTMLElement2 element = (Interop.IHTMLElement2) CurrentTable;
                                    Interop.IHTMLControlRange tableRange = body2.CreateControlRange() as Interop.IHTMLControlRange;
                                    if (tableRange != null && element is Interop.IHTMLControlElement)
                                    {
                                        tableRange.add((Interop.IHTMLControlElement) element);
                                        tableRange.select();
                                    }
                                    //                                    this.htmlEditor.Exec(Interop.IDM.DISABLE_EDITFOCUS_UI, false);
                                    //                                    this.htmlEditor.InvokeTableContextMenu(CurrentTable, new ShowContextMenuEventArgs(new Point(this.x, this.y), false, e.GetSrcElement(), htmlEditor));  // entering designer mode fires mouse down externally
                                    preHandleReturnState = Interop.S_OK;
                                    this.PreDesignMode = false; // one time event only
                                    break;
                                }
                                // if preresizemode and mouse clicked, enter regular resize mode
                                // this event relates to BODY because the handles are drawn outside the regular table area
                                if (this.PreResizeMode)
                                {
                                    // get cells to beeing changed after entering resize mode. To resize the table we remove
                                    // the style of the outer cells (right col, bottom row) and the global table width
                                    // and height can effect the table measures.
                                    Interop.IHTMLTableCell cell;
                                    cell = this.TableFormatter.TableInfo.Item(0, this.TableFormatter.TableInfo.GetColNumber(0) - 1);
                                    ResetCssWidth((Interop.IHTMLElement) cell);
                                    cell = this.TableFormatter.TableInfo.Item(this.TableFormatter.TableInfo.GetRowNumber() - 1, 0);
                                    ResetCssHeight((Interop.IHTMLElement) cell);
                                    this.IsResizeMode = true;
                                    this.PreResizeMode = false;
                                    // after resetting the outer cells we let the embedded designer do the resizing stuff
                                    preHandleReturnState = Interop.S_FALSE;
                                    break;
                                }
                                // no more selections and click inside the table, remove all highlighting
                                // hold the stack if Shft key is pressed to add more cells
                                if (!e.shiftKey)
                                {
                                    this.CellStack.RemoveHighLight();
                                    // inform the host that we lost the collection
                                    FireTableCellSelection();
                                }
                                if (IsInCellSelection.StartCellSelection)
                                {
                                    if (this.ztTableZone == ZoneTypeEnum.CellSelect)
                                    {
                                        // Adding a cell with Shft key pressed, remove collection and set new start cell else
                                        this.CellStack.AddCell(GetCell(IsInCellSelection.CurrentRow, IsInCellSelection.CurrentCol));
                                        IsInCellSelection.InCellSelection = true;
                                        preHandleReturnState = Interop.S_OK;
                                        break;
                                    } 
                                    if (this.ztTableZone == ZoneTypeEnum.RowSelect)
                                    {
                                        // select from start cell to end cell of selected row
                                        foreach (Interop.IHTMLTableCell tc in this.TableFormatter.TableInfo.Cells.Keys)
                                        {
                                            int r = 0;
                                            int c = 0;
                                            this.TableFormatter.TableInfo.GetCellPoint(tc, ref r, ref c);
                                            if (r == IsInCellSelection.CurrentRow)
                                            {
                                                this.CellStack.AddCell(tc);
                                            }
                                        }
                                        IsInCellSelection.InCellSelection = true;
                                        preHandleReturnState = Interop.S_OK;
                                        break;
                                    } 
                                    if (this.ztTableZone == ZoneTypeEnum.ColumnSelect)
                                    {
                                        // select from start cell to end cell of selected row
                                        foreach (Interop.IHTMLTableCell tc in this.TableFormatter.TableInfo.Cells.Keys)
                                        {
                                            int r = 0;
                                            int c = 0;
                                            this.TableFormatter.TableInfo.GetCellPoint(tc, ref r, ref c);
                                            if (c == IsInCellSelection.CurrentCol)
                                            {
                                                this.CellStack.AddCell(tc);
                                            }
                                        }
                                        IsInCellSelection.InCellSelection = true;
                                        preHandleReturnState = Interop.S_OK;
                                        Application.DoEvents();
                                        break;
                                    }
                                }
                                SetMousePointer();
                                preHandleReturnState = Interop.S_FALSE;
                            }
                            // right click on activator fires context menu for table
                            if (e.button == RIGHT)
                            {
                                if (this.PreDesignMode)
                                {
                                    // entering designer mode fires mouse down externally
                                    // TODO: Do we need this?? this.htmlEditor.Exec(Interop.IDM.DISABLE_EDITFOCUS_UI, true);
                                    // TODO: this.htmlEditor.InvokeTableContextMenu(CurrentTable, new ShowContextMenuEventArgs(new Point(e.GetClientX(), e.GetClientY()), false, e.GetSrcElement(), this.htmlEditor));  
                                    preHandleReturnState = Interop.S_OK;
                                    this.PreDesignMode = false; // one time event only
                                    break;
                                }
                            }
                            break;

                            # endregion
                    } // end switch
                } 
            }
            return preHandleReturnState;
        }

        /// <summary>
        /// Fired after the event was handled.
        /// </summary>
        /// <param name="dispId"></param>
        /// <param name="eventObj"></param>
        /// <returns></returns>
        int Interop.IHTMLEditDesigner.PostHandleEvent(int dispId, GuruComponents.Netrix.ComInterop.Interop.IHTMLEventObj eventObj)
        {

            return preHandleReturnState;
        }

        /// <summary>
        /// Fired after a key event occurs to implement a way to change the key behavior. 
        /// </summary>
        /// <param name="dispId"></param>
        /// <param name="eventObj"></param>
        /// <returns></returns>
        int Interop.IHTMLEditDesigner.TranslateAccelerator(int dispId, GuruComponents.Netrix.ComInterop.Interop.IHTMLEventObj eventObj)
        {
            return preHandleReturnState;
        }

        /// <summary>
        /// The interface implementation. Looks for the current element and set the table state. If the caret is
        /// inside a table the TAB key is activated to mode the caret between cells.
        /// </summary>
        /// <param name="dispId"></param>
        /// <param name="eventObj"></param>
        /// <returns></returns>
        int Interop.IHTMLEditDesigner.PostEditorEventNotify(int dispId, GuruComponents.Netrix.ComInterop.Interop.IHTMLEventObj eventObj)
        {
            int returnCode = preHandleReturnState;
            // if TAB is pressed check for current cell and set caret to next cell/row
            // if there are no more rows, add a new row to mimic MS WORD behavior
            if (htmlEditor.DesignModeEnabled && (dispId == DispId.KEYDOWN || dispId == DispId.MOUSEDOWN))
            {
                Interop.IHTMLElement element = null;
                // We check the current scope only if the caret is visible for the user
                if (dispId == DispId.KEYDOWN)
                {
                    element = this.htmlEditor.GetCurrentScopeElement().GetBaseElement();
                    if (element != null)
                    {
                        EventPreparation(element, dispId, eventObj.keyCode, eventObj.ctrlKey, ref returnCode);
                    }
                }
                // if a mouse click was handled the event source has the element
                if (dispId == DispId.MOUSEDOWN)
                {
                    element = eventObj.srcElement;
                    if (element != null && ztTableZone == ZoneTypeEnum.None)
                    {
                        EventPreparation(element, dispId, eventObj.keyCode, eventObj.ctrlKey, ref returnCode);
                    }
                    // Context Menu
                    if (IsActivated && eventObj.button == RIGHT)
                    {
                        this.designer.OnShowContextMenu(eventObj.clientX, eventObj.clientY, (Interop.IHTMLElement)OldCurrentTable, htmlEditor);
                    }
                }                
            }
            return returnCode;
        }

        #endregion

        Interop.IHTMLElement LastElement;

        private void EventPreparation(Interop.IHTMLElement element, int dispId, int keyCode, bool ctrlKey, ref int returnCode)
        {
            if (element != LastElement || (keyCode ==9  && processTABKey))
            {
                
                // register caret position to check whether the caret is inside a table
                Interop.IHTMLElement t = this.GetParentElement(element, "TABLE");
                Interop.IHTMLElement cell = this.GetParentElement(element, "TD");
                if (t == null || !t.Equals(OldCurrentTable))
                {
                    if (this.OldCurrentTable != null)
                    {
                        if (!staticBehavior)
                        {
                            RemoveBehaviors(this.OldCurrentTable);
                        }
                        ((TableFormatter)this.TableFormatter).CurrentCell = null;
                        this.designer.OnTableBecomesInactive(new TableEventArgs((Interop.IHTMLElement)this.OldCurrentTable, this.htmlEditor));
                        this.OldCurrentTable = null;
                    }
                }
                if (t != null)
                {
                    // do not use behaviors if not active
                    if (this.IsActivated && !staticBehavior)
                    {
                        AddBehaviors(t as Interop.IHTMLTable);
                    }
                    this.TableFormatter.TableInfo = null;
                    ((TableFormatter)this.TableFormatter).CurrentElement = cell;
                    if (OldCurrentTable == null || !OldCurrentTable.Equals(t))
                    {
                        CurrentTable = t as Interop.IHTMLTable;
                        this.designer.OnTableBecomesActive(new TableEventArgs(t, this.htmlEditor));
                    }
                    this.OldCurrentTable = t as Interop.IHTMLTable;
                }
                // check for TAB key to move caret, if ProcessTABKey is turned on
                // do not process TAB if not activated
                if (IsActivated && dispId == DispId.KEYDOWN && keyCode == 9 /*TAB*/)
                {
                    // caret is in table
                    if (t != null && this.processTABKey)
                    {
                        // loop for the cell which the caret is currently in
                        if (cell != null)
                        {
                            if (ctrlKey)
                            {
                                ((TableFormatter)this.TableFormatter).PrevCellPosition((Interop.IHTMLTableCell)cell);
                            }
                            else
                            {
                                ((TableFormatter)this.TableFormatter).NextCellPosition((Interop.IHTMLTableCell)cell);
                            }
                            // ok, this is a final action and stops further key processing
                            
                            returnCode = Interop.S_OK;
                        }
                    }
                }
                LastElement = element;
            }
        }

    }
}
