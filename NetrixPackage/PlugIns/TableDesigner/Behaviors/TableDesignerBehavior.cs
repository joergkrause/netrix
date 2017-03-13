using System;
using System.Drawing;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Behaviors;

namespace GuruComponents.Netrix.TableDesigner
{
    /// <summary>
    /// This class controls the table appearance during design time.
    /// </summary>
    /// <remarks>
    /// It contains the draw method to draw the white background and
    /// the solid border around the whole table. It also draw the activator icon in the upper left corner.
    /// It has a predefined implementation of an advanced resizing technique. The default drawing conditions
    /// are TRANSPARENT (does not change existing pixels) and BELOW_CONTENT (Content is dominant, all
    /// drawings are behind the text/borders/images).
    /// <para>As of version 1.0 the drawing style is hard coded. Future version may have a configuration option here.</para>
    /// </remarks>
    internal class TableDesignerBehavior : BaseBehavior
    {

        private Icon ActivatorIcon;
        private Pen CurrentTableBorderPen;
        private SolidBrush _handleBrush = new SolidBrush(Color.Black);
        private SolidBrush _bgBrush = new SolidBrush(Color.White);
        private TableEditDesigner designer;

        /// <summary>
        /// Instantiates a new table designer behavior and sets some defaults.
        /// </summary>
        public TableDesignerBehavior(TableEditDesigner designer, IHtmlEditor host) : base(host)
        {
            this.designer = designer;
            CurrentTableBorderPen = new Pen(Color.Black, 1.5F);
            CurrentTableBorderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            CurrentTableBorderPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
            // Slider
            // Painter defaults
            base.HtmlPaintFlag = HtmlPainter.Transparent;
            base.HtmlZOrderFlag = HtmlZOrder.BelowContent;
            base.BorderMargin = new Rectangle(16, 16, 16, 16);
            System.IO.Stream stream = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.Netrix.TableDesigner.Resources.TableActivator.ico");
            System.Diagnostics.Debug.Assert(stream != null);
            ActivatorIcon = new Icon(stream);
        }

        /// <summary>
        /// The name of this behavior.
        /// </summary>
        public override string Name
        {
            get
            {
                return "TableDesigner#" + BaseBehavior.url;
            }
        }

        /// <summary>
        /// Sets the table background color in design mode.
        /// </summary>
        public Color TableBackground
        {
            set
            {
                _bgBrush.Color = value;
            }
        }

        # region Draw methods

        int lastTop = 0;

        /// <summary>
        /// The drawing method which produces the border and the background using GDI+. The parameters are the 
        /// rectangle area which is currently drawn.
        /// </summary>
        /// <param name="leftBounds">Left border</param>
        /// <param name="topBounds">Top border</param>
        /// <param name="rightBounds">Right border</param>
        /// <param name="bottomBounds">Bottom border</param>
        /// <param name="gr">The graphics context object.</param>
        protected override void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, Graphics gr)
        {
            lastTop = topBounds;
            Rectangle r = new Rectangle(leftBounds + BorderMargin.Left, topBounds + BorderMargin.Top, rightBounds - leftBounds - 3*BorderMargin.Width, bottomBounds - topBounds - 3*BorderMargin.Height);
            gr.PageUnit = GraphicsUnit.Pixel;
            if (designer.IsActivated)
            {
                if (!designer.IsResizeMode)
                {
                    // Draw table border to fix handles optically, make a white background to stop grid
                    if (_bgBrush.Color != Color.Empty && _bgBrush.Color.ToArgb() != 0)
                    {
                        gr.FillRectangle(_bgBrush, r.Left, r.Top, r.Width - 0, r.Height - 0);
                    }
                    gr.DrawRectangle(CurrentTableBorderPen, r.Left, r.Top, r.Width - 0, r.Height - 0);
                }
                // Draw master handle to activate/deactivate design mode any time
                gr.DrawIcon(ActivatorIcon, r.Left, r.Top);
            }
            else
            {
                Invalidate();
            }
            // Draw the resize handler
            /* HANDLES
            int halfX = (rightBounds + leftBounds) / 2;
            int halfY = (topBounds + bottomBounds) / 2;
            gr.FillRectangle(_handleBrush, halfX, bottomBounds - HANDLESIZE, HANDLESIZE, HANDLESIZE);    // bottom middle side
            gr.FillRectangle(_handleBrush, rightBounds - HANDLESIZE, bottomBounds - HANDLESIZE, HANDLESIZE, HANDLESIZE);  // lower right corner
            gr.FillRectangle(_handleBrush, rightBounds - HANDLESIZE, halfY, HANDLESIZE, HANDLESIZE);         // right middle side
            */
        }

        # endregion
 
    }
}
