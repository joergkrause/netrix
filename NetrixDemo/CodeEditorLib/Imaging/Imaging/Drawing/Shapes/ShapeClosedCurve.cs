using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Comzept.Library.Drawing.Shapes
{

	public class ShapeClosedCurve :ShapePolygon
    {
        #region Constructors

        internal ShapeClosedCurve()
		{

		}

        public ShapeClosedCurve(PointF[] points)
            : this(points, 0, null, null) { }
        public ShapeClosedCurve(PointF[] points, float rotation)
            : this(points, rotation, null, null) { }
        public ShapeClosedCurve(PointF[] points, Pen pen)
            : base(points, 0, pen) { }
        public ShapeClosedCurve(PointF[] points, float rotation, Pen pen)
            : base(points, rotation, pen) { }
        public ShapeClosedCurve(PointF[] points, float rotation, Brush brush)
            : this(points, rotation, null,brush) { }
        public ShapeClosedCurve(PointF[] points, float rotation, Pen pen, Brush brush)
            : base(points, rotation, pen)
        {
            this.Brush = brush;
        }

        #endregion

        #region Overrides

        public override Image GetThumb(ShapeThumbSize thumbSize)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override string ToString()
        {
            return "{Comzept.Library.Drawing.Shapes.ShapeClosedCurve}";
        }

        protected override void UpdatePath()
        {
            if (this.Points == null || this.Points.Length == 0) return;

            InternalPath = new GraphicsPath();
            InternalPath.AddClosedCurve(this.Points);

            Matrix mtx = new Matrix();
            mtx.RotateAt(this.Rotation, InternalRotationBasePoint);
            InternalPath.Transform(mtx);
        }

        #endregion      
	}
}