using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Comzept.Library.Drawing.Shapes
{

	public class ShapeRectangle : Shape,IDrawable,IFillable,IRectangle
	{
		private RectangleF _bounds;

		#region Constructors

		internal ShapeRectangle()
		{
			this.Rotation = 0;
			this.Pen = Pens.Transparent;
		}

		public ShapeRectangle(float left, float top, float width, float height)
			: this(new RectangleF(left, top, width, height), 0) { }
		public ShapeRectangle(float left, float top, float width, float height, float rotation)
			: this(new RectangleF(left, top, width, height), rotation) { }
		public ShapeRectangle(RectangleF bounds)
			: this(bounds, 0) { }
		public ShapeRectangle(RectangleF bounds, float rotation)
		{
			_bounds = bounds;
			this.Rotation = rotation;
			UpdatePath();
            this.Pen = Pens.Transparent;
		}
		public ShapeRectangle(PointF firstCorner, PointF secondCorner)
			: this(firstCorner, secondCorner, 0) { }
		public ShapeRectangle(PointF firstCorner, PointF secondCorner, float rotation)
		{
			float left = Math.Min(firstCorner.X, secondCorner.X);
			float top = Math.Min(firstCorner.X, secondCorner.X);
			float width = Math.Abs(firstCorner.X - secondCorner.X);
			float height = Math.Abs(firstCorner.Y - secondCorner.Y);

			_bounds = new RectangleF(left, top, width, height);
			this.Rotation = rotation;
			UpdatePath();
            this.Pen = Pens.Transparent;
		}

		public ShapeRectangle(float left, float top, float width, float height, Pen pen)
			: this(new RectangleF(left, top, width, height), 0, pen) { }
		public ShapeRectangle(float left, float top, float width, float height, float rotation, Pen pen)
			: this(new RectangleF(left, top, width, height), rotation, pen) { }
		public ShapeRectangle(RectangleF bounds, Pen pen)
			: this(bounds, 0, pen) { }
		public ShapeRectangle(RectangleF bounds, float rotation, Pen pen)
			: this(bounds, rotation)
		{
			this.Pen = pen;
            this.Brush = Brushes.Transparent;
		}
		public ShapeRectangle(PointF firstCorner, PointF secondCorner, Pen pen)
			: this(firstCorner, secondCorner, 0, pen) { }
		public ShapeRectangle(PointF firstCorner, PointF secondCorner, float rotation, Pen pen)
			: this(firstCorner, secondCorner, rotation)
		{
			this.Pen = pen;
            this.Brush = Brushes.Transparent;
		}

		public ShapeRectangle(float left, float top, float width, float height, Brush brush)
			: this(new RectangleF(left, top, width, height), 0, brush) { }
		public ShapeRectangle(float left, float top, float width, float height, float rotation, Brush brush)
			: this(new RectangleF(left, top, width, height), rotation, brush) { }
		public ShapeRectangle(RectangleF bounds, Brush brush)
			: this(bounds, 0, brush) { }
		public ShapeRectangle(RectangleF bounds, float rotation, Brush brush)
			: this(bounds, rotation)
		{
            this.Pen = Pens.Transparent;
			this.Brush = brush;
		}
		public ShapeRectangle(PointF firstCorner, PointF secondCorner, Brush brush)
			: this(firstCorner, secondCorner, 0, brush) { }
		public ShapeRectangle(PointF firstCorner, PointF secondCorner, float rotation, Brush brush)
			: this(firstCorner, secondCorner, rotation)
		{
            this.Pen = Pens.Transparent;
			this.Brush = brush;
		}

		public ShapeRectangle(float left, float top, float width, float height, Pen pen, Brush brush)
			: this(new RectangleF(left, top, width, height), 0, pen, brush) { }
		public ShapeRectangle(float left, float top, float width, float height, float rotation, Pen pen, Brush brush)
			: this(new RectangleF(left, top, width, height), 0, pen, brush) { }
		public ShapeRectangle(RectangleF bounds, Pen pen, Brush brush)
			: this(bounds, 0, pen, brush) { }
		public ShapeRectangle(RectangleF bounds, float rotation, Pen pen, Brush brush)
			: this(bounds, rotation)
		{
			this.Pen = pen;
			this.Brush = brush;
		}
		public ShapeRectangle(PointF firstCorner, PointF secondCorner, Pen pen, Brush brush)
			: this(firstCorner, secondCorner, 0, pen, brush) { }
		public ShapeRectangle(PointF firstCorner, PointF secondCorner, float rotation, Pen pen, Brush brush)
			: this(firstCorner, secondCorner, rotation)
		{
			this.Pen = pen;
			this.Brush = brush;
		}

		#endregion

		#region Overrides

		public override RectangleF Bounds
		{
			get
			{
				return _bounds;
			}
		}
		public override PointF Center
		{
			get
			{
				return new PointF
					(
						(_bounds.Left + _bounds.Right) / 2,
						(_bounds.Top + _bounds.Bottom) / 2
					);
			}
			set
			{
				PointF oldCenter = this.Center;
				float dx = value.X - oldCenter.X;
				float dy = value.Y - oldCenter.Y;
				Translate(dx, dy);
			}
		}
		public override PointF Location
		{
			get
			{
				return _bounds.Location;
			}
			set
			{
				_bounds.Location = value;
				UpdatePath();
			}
		}

		public override void Paint(Graphics graphics, SmoothingMode smoothingMode)
		{
			this.Fill(graphics);
			this.Draw(graphics, smoothingMode);
		}
		public override void Rotate(float value, PointF basePoint)
		{
			this.Rotation = value;
			InternalRotationBasePoint = basePoint;
			UpdatePath();
		}
		public override void Translate(float dx, float dy)
		{
			_bounds.X += dx;
			_bounds.Y += dy;
			UpdatePath();
		}
        public override Image GetThumb(ShapeThumbSize thumbSize)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override string ToString()
        {
            return "{Comzept.Library.Drawing.Shapes.ShapeRectangle}";
        }

        protected override void UpdatePath()
		{
			InternalPath = new GraphicsPath(FillMode.Winding);
			InternalPath.AddRectangle(_bounds);

			Matrix mtx = new Matrix();
			mtx.RotateAt(this.Rotation, InternalRotationBasePoint);
			InternalPath.Transform(mtx);
		}

		#endregion

        #region IDrawable Members

        public void Draw(Graphics graphics)
        {
            this.Draw(graphics, graphics.SmoothingMode);
        }
        public void Draw(Graphics graphics, SmoothingMode smoothingMode)
        {
            SmoothingMode sMode = graphics.SmoothingMode;
            graphics.SmoothingMode = smoothingMode;

            if (this.Brush != null)
                graphics.FillPath(this.Brush, InternalPath);

            graphics.DrawPath(this.Pen, InternalPath);

            graphics.SmoothingMode = sMode;
        }

        #endregion

        #region IFillable Members

        public void Fill(Graphics graphics)
        {
            if(this.Brush != null)
                graphics.FillPath(this.Brush, InternalPath);
        }

        #endregion

        #region IRectangle Members - Virtuals

        public virtual float Left
        {
            get
            {
                return _bounds.Left;
            }
            set
            {
                _bounds.X = value;
                UpdatePath();
            }
        }
        public virtual float Top
        {
            get
            {
                return _bounds.Top;
            }
            set
            {
                _bounds.Y = value;
                UpdatePath();
            }
        }
        public virtual float Width
        {
            get
            {
                return _bounds.Width;
            }
            set
            {
                _bounds.Width = value;
                UpdatePath();
            }
        }
        public virtual float Height
        {
            get
            {
                return _bounds.Height;
            }
            set
            {
                _bounds.Height = value;
                UpdatePath();
            }
        }

        #endregion
    }
}