using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Comzept.Library.Drawing.Shapes
{

	public class ShapeCurve  :ShapePolyline
	{

		#region Constructors

		internal ShapeCurve()
		{
		}

		public ShapeCurve(PointF[] points)
			: base(points, 0, null) { }
		public ShapeCurve(PointF[] points, float rotation)
			: base(points, rotation, null) { }
		public ShapeCurve(PointF[] points, Pen pen)
			: base(points, 0, pen) { }
		public ShapeCurve(PointF[] points, float rotation, Pen pen)
			: base(points, rotation, pen) { }

		#endregion

        public override Image GetThumb(ShapeThumbSize thumbSize)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override string ToString()
        {
            return "{Comzept.Library.Drawing.Shapes.ShapeCurve}";
        }

		protected override void UpdatePath()
		{
            if (this.Points == null || this.Points.Length == 0) return;

			InternalPath = new GraphicsPath();
			InternalPath.AddCurve(this.Points);

			Matrix mtx = new Matrix();
			mtx.RotateAt(this.Rotation, InternalRotationBasePoint);
			InternalPath.Transform(mtx);
		}

	}
}