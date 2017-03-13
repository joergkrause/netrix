using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
    /// <summary>
    /// This class draws a border around a specific element to inform the user where the current selection
    /// is.</summary><remarks>
    /// Works only with elements having width and height attributes or block elements, regardless wether
    /// the attributes are set or not. It's used internally to use with atomic behaviors.
    /// </remarks>
    internal sealed class ElementSelectedBehavior : BaseBehavior
    {
        public override string Name
        {
            get
            {
                return "ElementSelected#" + BaseBehavior.url;
            }
        }

        public ElementSelectedBehavior(IHtmlEditor host) : base(host)
        {
            BorderMargin = new Rectangle(2, 2, 2, 2);
            BorderPenStyle = new Pen(new SolidBrush(Color.Black));
            BorderPenStyle.DashStyle = DashStyle.DashDot;
            BorderPenStyle.Width = 2F;            
        }

        protected override void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, Graphics gr)
        {
            gr.PageUnit = GraphicsUnit.Pixel;
            gr.DrawRectangle(BorderPenStyle, leftBounds, topBounds, rightBounds - leftBounds, bottomBounds - topBounds);
        }
    }

}
