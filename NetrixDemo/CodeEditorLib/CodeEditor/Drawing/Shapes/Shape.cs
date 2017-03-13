using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuruComponents.CodeEditor.Library.Drawing.Shapes
{

	public abstract class Shape  
	{

        private Pen _pen;
        private Brush _brush;
		private float _rotation;

        #region Protected

        protected GraphicsPath InternalPath;
        protected PointF InternalRotationBasePoint;

        #endregion

        #region Abstract

        public abstract RectangleF Bounds { get;}
		public abstract PointF Center { get;set;}
		public abstract PointF Location { get;set;}

        public abstract Image GetThumb(ShapeThumbSize thumbSize);
		public abstract void Paint(Graphics graphics, SmoothingMode smoothingMode);
		public abstract void Rotate(float value, PointF basePoint);
		public abstract void Translate(float dx, float dy);

        protected abstract void UpdatePath();

        #endregion

        #region Virtual

        public virtual GraphicsPath Path 
		{
			get
			{
				return InternalPath;
			}
		}
		public virtual SizeF Size
		{
			get
			{
				return Bounds.Size;
			}
		}
		public virtual Pen Pen
		{
			get
			{
				return _pen;
			}
			set
			{
				_pen = value;
			}
		}
		public virtual Brush Brush
		{
			get
			{
				return _brush;
			}
			set
			{
				_brush = value;
			}
		}
		public virtual float Rotation 
		{
			get
			{
				return _rotation;
			}
			set
			{
				_rotation = value;
				UpdatePath();
			}
		}

		public virtual void Paint(Graphics graphics)
		{
			this.Paint(graphics, graphics.SmoothingMode);
		}
		public virtual void Update()
		{
			this.UpdatePath();
		}
		public virtual void Rotate(float value)
		{
			this.Rotate(value, this.Center);
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return "{GuruComponents.CodeEditor.Library.Drawing.Shapes.Shape}";
        }

        #endregion

    }
}