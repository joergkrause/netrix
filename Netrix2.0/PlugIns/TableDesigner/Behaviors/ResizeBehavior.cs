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
    internal class ResizeBehavior : BaseBehavior
    {

        private Icon ActivatorIcon;
        private Pen CurrentTableBorderPen;
        private SolidBrush _bgBrush;
        private Pen _bgPen;
        private TableEditDesigner designer;
        private Font _font;
        private SolidBrush _txBrush;

        /// <summary>
        /// Instantiates a new table designer behavior and sets some defaults.
        /// </summary>
        public ResizeBehavior(TableEditDesigner designer, IHtmlEditor host) : base(host)
        {
            this.designer = designer;
            CurrentTableBorderPen = new Pen(Color.Black, 1.5F);
            CurrentTableBorderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            CurrentTableBorderPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
            // Slider
            // Painter defaults
            base.HtmlPaintFlag = HtmlPainter.Opaque;
            base.HtmlZOrderFlag = HtmlZOrder.AboveContent;
            base.BorderMargin = new Rectangle(0, 0, 0, 0);
            System.IO.Stream stream = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.Netrix.TableDesigner.Resources.TableActivator.ico");
            ActivatorIcon = new Icon(stream);
            Color color = Color.FromArgb(128, Color.Beige);
            _bgBrush = new SolidBrush(color);
            _bgPen = new Pen(Color.Blue, 1F);
            _font = new Font(FontFamily.GenericSansSerif, 8F);
            _txBrush = new SolidBrush(Color.Black);
        }

        /// <summary>
        /// The name of this behavior.
        /// </summary>
        public override string Name
        {
            get
            {
                return "TableDesigner#Resize";
            }
        }

        /// <summary>
        /// Sets the table background color in design mode.
        /// </summary>
        public Color EffectColor
        {
            set
            {
                _bgBrush.Color = value;
            }
        }

        # region Draw methods


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
            gr.PageUnit = GraphicsUnit.Pixel;
            Rectangle r = new Rectangle(leftBounds, topBounds, rightBounds - leftBounds, bottomBounds - topBounds);
            // Draw table border to fix handles optically, make a white background to stop grid
            if (_bgBrush.Color != Color.Empty && _bgBrush.Color.ToArgb() != 0)
            {
                gr.FillRectangle(_bgBrush, r.Left, r.Top, r.Width, r.Height);
                //gr.DrawRectangle(_bgPen, r);
            }
            Interop.IHTMLElement table = designer.CurrentTable as Interop.IHTMLElement; // base.behaviorSite.GetElement();
            if (table != null)
            {
                Interop.IHTMLStyle style = table.GetStyle();
                string text = String.Format("{0}px x {1}px", style.GetPixelWidth(), style.GetPixelHeight());
                SizeF stringSize = gr.MeasureString(text, _font);
                float x, y;
                x = (r.Width / 2) - (stringSize.Width / 2) + leftBounds;
                y = (r.Height / 2) - (stringSize.Height / 2) + topBounds;
                gr.DrawString(text, _font, _txBrush, x, y);
                // draw table specific behaviors
                if (style.GetPosition() != null && style.GetPosition().Equals("absolute"))
                {
                    text = String.Format("{0} x {1}", style.GetTop(), style.GetLeft());
                    stringSize = gr.MeasureString(text, _font);
                    //x = (r.Width / 2) - (stringSize.Width / 2) + leftBounds;
                    //y = (r.Height / 2) - (stringSize.Height / 2) + topBounds + stringSize.Height+2;
                    x = leftBounds + 16;
                    y = topBounds + 16;
                    gr.DrawString(text, _font, _txBrush, x, y);
                }
            }
        }

        # endregion
 
    }
}
