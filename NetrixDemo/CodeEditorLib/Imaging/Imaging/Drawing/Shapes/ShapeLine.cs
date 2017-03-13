using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Comzept.Library.Drawing.Shapes
{

	public class ShapeLine :Shape,IDrawable
	{
		private float _x1, _y1, _x2, _y2;

		#region Constructors

		internal ShapeLine()
		{
		}

		public ShapeLine(float xStartPoint, float yStartPoint, float xEndPoint, float yEndPoint)
			: this(xStartPoint, yStartPoint, xEndPoint, yEndPoint,0 , null) { }
		public ShapeLine(float xStartPoint, float yStartPoint, float xEndPoint, float yEndPoint, Pen pen)
			: this(xStartPoint, yStartPoint, xEndPoint, yEndPoint, 0, pen) { }

		public ShapeLine(float xStartPoint, float yStartPoint, float xEndPoint, float yEndPoint,float rotation)
			: this(xStartPoint, yStartPoint, xEndPoint, yEndPoint,0, null) { }
		public ShapeLine(float xStartPoint, float yStartPoint, float xEndPoint, float yEndPoint,float rotation, Pen pen)
		{
			_x1 = xStartPoint;
			_y1 = yStartPoint;
			_x2 = xEndPoint;
			_y2 = yEndPoint;

			this.Rotation = rotation;
			this.Pen = pen;

			this.UpdatePath();
		}

		public ShapeLine(PointF startPoint, PointF endPoint)
			:this(startPoint.X,startPoint.Y,endPoint.X,endPoint.Y,0,Pens.Black) { }
		public ShapeLine(PointF startPoint, PointF endPoint, Pen pen)
			: this(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y,0, pen) { }

		public ShapeLine(PointF startPoint, PointF endPoint,float rotation)
			: this(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y,rotation, null) { }
		public ShapeLine(PointF startPoint, PointF endPoint,float rotation, Pen pen)
			: this(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y,rotation, pen) { }

		#endregion

		#region Overrides

		public override RectangleF Bounds
		{
			get
			{
				return GetBounds();
			}
		}
		public override PointF Center
		{
			get
			{
				RectangleF bounds = this.Bounds;
				float x = (bounds.Left + bounds.Right) / 2;
				float y = (bounds.Top + bounds.Bottom) / 2;
				return new PointF(x, y);
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
				return Bounds.Location;
			}
			set
			{
				PointF oldLocation = this.Location;
				float dx = value.X - oldLocation.X;
				float dy = value.Y - oldLocation.Y;
				Translate(dx, dy);
			}
		}

		public override void Paint(Graphics graphics, SmoothingMode smoothingMode)
		{
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
			_x1 += dx;
			_x2 += dx;
			_y1 += dy;
			_y2 += dy;
			UpdatePath();
		}
        public override Image GetThumb(ShapeThumbSize thumbSize)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override string ToString()
        {
            return "{Comzept.Library.Drawing.Shapes.ShapeLine}";
        }

        protected override void UpdatePath()
		{
			InternalPath = new GraphicsPath();
			InternalPath.AddLine(_x1, _y1, _x2, _y2);

			Matrix mtx = new Matrix();
			mtx.RotateAt(this.Rotation, new PointF(_x1,_y1));
			InternalPath.Transform(mtx);
		}

		#endregion 

		#region Private Members

		private RectangleF GetBounds()
		{
			float left = Math.Min(_x1, _x2);
			float top = Math.Min(_y1, _y2);
			float width = Math.Abs(_x1 - _x2);
			float height = Math.Abs(_y1 - _y2);

			return new RectangleF(left, top, width, height);
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

			graphics.DrawPath(this.Pen, InternalPath);

			graphics.SmoothingMode = sMode;
		}

		#endregion

		#region Virtuals

		public virtual PointF StartPoint
		{
			get
			{
				return new PointF(_x1, _y1);
			}
			set
			{
				_x1 = value.X;
				_y1 = value.Y;
				UpdatePath();
			}
		}
		public virtual PointF EndPoint
		{
			get
			{
				return new PointF(_x2, _y2);
			}
			set
			{
				_x2 = value.X;
				_y2 = value.Y;
				UpdatePath();
			}
		}

		#endregion

    }
}