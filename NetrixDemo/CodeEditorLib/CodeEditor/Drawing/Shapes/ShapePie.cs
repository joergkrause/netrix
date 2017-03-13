using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuruComponents.CodeEditor.Library.Drawing.Shapes
{

	public class ShapePie  :ShapeArc,IFillable
	{

		internal ShapePie()
		{

		}
        public ShapePie(float left, float top, float width, float height, float startAngle, float sweepAngle, float rotation, Pen pen)
            : base(left, top, width, height, startAngle, sweepAngle, rotation, pen) { }

        protected override void UpdatePath()
        {
            InternalPath = new GraphicsPath();
            InternalPath.AddPie(this.Left, this.Top, this.Width, this.Height, this.StartAngle, this.SweepAngle);

            Matrix mtx = new Matrix();
            mtx.RotateAt(this.Rotation, InternalRotationBasePoint);
            InternalPath.Transform(mtx);
        }

        #region IFillable Members


        public void Fill(Graphics graphics)
        {
            graphics.FillPath(this.Brush, InternalPath);
        }

        #endregion
    }
}