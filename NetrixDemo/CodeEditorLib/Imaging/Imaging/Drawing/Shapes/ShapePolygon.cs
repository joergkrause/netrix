using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Comzept.Library.Drawing.Shapes
{

    public class ShapePolygon : ShapePolyline, IFillable, IDrawable
    {

        #region Constructors

        internal ShapePolygon()
        {
        }

        public ShapePolygon(PointF[] points)
            : base(points, 0, null) { }
        public ShapePolygon(PointF[] points, float rotation)
            : base(points, rotation, null) { }
        public ShapePolygon(PointF[] points, Pen pen)
            : base(points, 0, pen) { }
        public ShapePolygon(PointF[] points, float rotation, Pen pen)
            : base(points, rotation, pen) { }
        public ShapePolygon(PointF[] points, float rotation, Brush brush)
            : this(points, rotation, null,brush) { }
        public ShapePolygon(PointF[] points, float rotation, Pen pen, Brush brush)
            : base(points, rotation, pen)
        {
            this.Brush = brush;
        }

        #endregion

        #region Overrides

        public override void Paint(Graphics graphics, SmoothingMode smoothingMode)
        {
            this.Fill(graphics);
            this.Draw(graphics, smoothingMode);
        }
        public override void Paint(Graphics graphics)
        {
            this.Paint(graphics, graphics.SmoothingMode);
        }

        public override Image GetThumb(ShapeThumbSize thumbSize)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override string ToString()
        {
            return "{Comzept.Library.Drawing.Shapes.ShapePolygon}";
        }

        protected override void UpdatePath()
        {
            if (this.Points == null || this.Points.Length == 0) return;

            InternalPath = new GraphicsPath();
            InternalPath.AddPolygon(this.Points);

            Matrix mtx = new Matrix();
            mtx.RotateAt(this.Rotation, InternalRotationBasePoint);
            InternalPath.Transform(mtx);
        }

        #endregion

        #region IFillable Members

        public void Fill(Graphics graphics)
        {
            if(this.Brush != null)
                graphics.FillPath(this.Brush, InternalPath);
        }

        #endregion
    }
}