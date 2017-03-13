using System;
using System.Drawing;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Behaviors;

namespace GuruComponents.Netrix.TableDesigner
{
    /// <summary>
    /// This class draws the line which helps the positioning of table sliders related to a ruler.
    /// </summary>
    /// <remarks>
    /// The line also indicates that the slider is in slide state and will change cell width.
    /// </remarks>
    public class TableSliderLineBehavior : BaseBehavior
    {

        private const float LINEWIDTH = 1.0F;
        private Pen _linePen;
        private int Offset;
        private bool isVertical;
        private int x, y;
//        private Interop.RECT _updaterect;
        
        /// <summary>
        /// Creates an instances of TableSliderLineBehavior.
        /// </summary>
        /// <remarks>
        /// Sets default properties: brown line with 1px width, line style is dashed, and line appears above content.
        /// </remarks>
        public TableSliderLineBehavior(IHtmlEditor host) : base(host)
        {
            _linePen = new Pen(Color.Brown, LINEWIDTH);
            _linePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            base.HtmlZOrderFlag = HtmlZOrder.AboveFlow;
        }

        /// <summary>
        /// Unique behavior name (constant).
        /// </summary>
        public override string Name
        {
            get
            {
                return "TableSliderLine#" + BaseBehavior.url;
            }
        }

        /// <summary>
        /// Sets a different Pen to draw the slider line.
        /// </summary>
        public Pen LinePen
        {
            set
            {
                _linePen = value;
                Offset = (int) Math.Ceiling(_linePen.Width / 2.0F);
            }
            get
            {
                return _linePen;
            }
        }

        /// <summary>
        /// The X coordinate where the vertical line is drawn. Invalidate designer area if value changes.
        /// </summary>
        public int X
        {
            set
            {
                if (x != value && isVertical)
                {
                    base.Invalidate();
                }
                x = value;
            }
            get
            {
                return x;
            }
        }
        
        /// <summary>
        /// The Y coordinate where the horizontal line is drawn. Invalidate designer area if value changes.
        /// </summary>
        public int Y
        {
            set
            {
                if (y != value && !isVertical)
                {
                    base.Invalidate();
                }
                y = value;
            }
            get
            {
                return y;
            }
        }

        /// <summary>
        /// Set true if vertical line is drawn, otherwise a horizontal line appears
        /// </summary>
        public bool IsVertical
        {
            set
            {
                this.isVertical = value;
            }
        }

        /// <summary>
        /// Draw method which draws the line.
        /// </summary>
        /// <param name="leftBounds"></param>
        /// <param name="topBounds"></param>
        /// <param name="rightBounds"></param>
        /// <param name="bottomBounds"></param>
        /// <param name="gr"></param>
        protected override void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, Graphics gr)
        {
            //if (topBounds < 0) return;
            gr.PageUnit = GraphicsUnit.Pixel;            
            if (isVertical)
            {
                gr.DrawLine(_linePen, leftBounds + x + Offset, topBounds, leftBounds + x + Offset, bottomBounds);
            }
            else 
            {
                gr.DrawLine(_linePen, leftBounds, topBounds + y + Offset, rightBounds, topBounds + y + Offset);
            }
        }
    }
}
