using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
    /// <summary>
    /// This class draws a border around a specific element to inform the user where the current selection
    /// is. Works only with elements having width and height attributes or block elements, regardless wether
    /// the attributes are set or not.
    /// </summary>
    internal sealed class MapAreaBehavior : BaseBehavior
    {

        public MapAreaBehavior(IHtmlEditor host) : base(host)
        {
            BorderMargin = new Rectangle(0, 0, 0, 0);
            BorderPenStyle = new Pen(Color.Red, 1.0F);
            BorderPenStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        }

        public override string Name
        {
            get
            {
                return "MapArea#" + BaseBehavior.url;
            }
        }

        protected override void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, Graphics gr)
        {
            gr.PageUnit = GraphicsUnit.Pixel;
            Interop.RECT rcBounds = new Interop.RECT(leftBounds, topBounds, rightBounds, bottomBounds);
            gr.DrawRectangle(BorderPenStyle, rcBounds.left, rcBounds.top, rcBounds.right - rcBounds.left, rcBounds.bottom - rcBounds.top);
        }
    }
}
