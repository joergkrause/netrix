using System;
using System.Drawing;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Behaviors;

namespace GuruComponents.Netrix.TableDesigner
{
    /// <summary>
    /// Set cell background and border to mark highlighted cells during selection processes.
    /// </summary>
    /// <remarks>
    /// This class cannot be inherited. The behavior can be controlled by setting the properties for background 
    /// brush and border pen.
    /// </remarks>
    internal sealed class HighLightCellBehavior : BaseBehavior
    {
        /// <summary>
        /// The current brush used to draw the cell background.
        /// </summary>
        private SolidBrush _cellBrush;
        /// <summary>
        /// The current pen used to draw the cell border.
        /// </summary>
        private Pen _borderPen;

        # region Public Properties

        public override string Name
        {
            get
            {
                return "TableCellHighLight#" + BaseBehavior.url;
            }
        }

        /// <summary>
        /// Set the border pen style for selected cells. Defaults to black, 2 pixel.
        /// </summary>
        public System.Drawing.Pen BorderPen
        {
            get
            {
                return _borderPen;
            }

            set
            {
               _borderPen = value;
            }
        }

        /// <summary>
        /// Set the background style for selected cells. Defaults to a solid pink rectangle.
        /// </summary>
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

        /// <summary>
        /// The constructor used to build the highlight object.
        /// </summary>
        /// <remarks>
        /// This constructor sets various defaults:
        /// <list>
        ///     <item>CellBrush is set to <see cref="Color.Pink"/></item>
        ///     <item>BorderPen is set to <see cref="Pen"/> with Color.Black and a width of 2 pixels.</item>
        ///     <item>HtmlPaintFlag is set to TRANSPARENT</item>
        ///     <item>HtmlZOrderFlag is set to BELOW_CONTENT</item>
        ///     <item>BorderMargin is set to 0 on all borders.</item>
        /// </list>
        /// </remarks>
        public HighLightCellBehavior(IHtmlEditor host) : base(host)
        {
            _cellBrush = new SolidBrush(Color.Pink);
            _borderPen = new Pen(Color.Black, 2.0F);
            base.HtmlPaintFlag = HtmlPainter.Transparent;
            base.HtmlZOrderFlag = HtmlZOrder.BelowContent;
            base.BorderMargin = new Rectangle(0, 0, 0, 0);
        }

        /// <summary>
        /// This method draws the cell background and border by using GDI+ function calls. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// Not for implementors: This is a time critical method. To avoid perfomance flaws please do not insert
        /// complex graphical operations here.
        /// </para>
        /// </remarks>
        /// <param name="leftBounds"></param>
        /// <param name="topBounds"></param>
        /// <param name="rightBounds"></param>
        /// <param name="bottomBounds"></param>
        /// <param name="gr"></param>
        protected override void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, Graphics gr)
        {
			if (CellBrush.Color.ToArgb() != 0)
			{
				gr.PageUnit = GraphicsUnit.Pixel;			
				gr.FillRectangle(CellBrush, leftBounds, topBounds, rightBounds - leftBounds, bottomBounds - topBounds);
				// setting -2 for right/lower border prevent the cell refresh process leaving zombie pixel borders there
				gr.DrawRectangle(BorderPen, leftBounds + 1, topBounds + 1, rightBounds - leftBounds - 2, bottomBounds - topBounds - 2);
			}
        }
    }
}