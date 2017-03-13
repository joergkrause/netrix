using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuruComponents.CodeEditor.Library.Drawing.Shapes
{

	public class ShapeArc  :ShapeLine,IRectangle
    {

        private float _left, _top, _width, _height, _startAngle, _sweepAngle;

        #region Constructors

        internal ShapeArc()
        {
        }

        public ShapeArc(float left, float top, float width, float height, float startAngle, float sweepAngle, float rotation, Pen pen)
        {
            _left = left;
            _top = top;
            _width = width;
            _height = height;
            _startAngle = startAngle;
            _sweepAngle = sweepAngle;

            this.Rotation = rotation;
            this.Pen = pen;

            UpdatePath();
        }

        #endregion

        #region Overrides

        protected override void UpdatePath()
        {
            InternalPath = new GraphicsPath();
            InternalPath.AddArc(_left, _top, _width, _height, _startAngle, _sweepAngle);

            Matrix mtx = new Matrix();
            mtx.RotateAt(this.Rotation, InternalRotationBasePoint);
            InternalPath.Transform(mtx);
        }

        public override PointF StartPoint
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override PointF EndPoint
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override PointF Location
        {
            get
            {
                return new PointF(_left,_top);
            }
            set
            {
                _left = value.X;
                _top = value.Y;
            }
        }
        public override RectangleF Bounds
        {
            get
            {
                return new RectangleF(_left, _top, _width, _height);
            }
        }

        #endregion

        #region IRectangle Members

        public float Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
                UpdatePath();
            }
        }
        public float Top
        {
            get
            {
                return _top;
            }
            set
            {
                _top = value;
                UpdatePath();
            }
        }
        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                UpdatePath();
            }
        }
        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                UpdatePath();
            }
        }

        #endregion

        #region Virtual Members

        public virtual float StartAngle
        {
            get
            {
                return _startAngle;
            }
            set
            {
                _startAngle = value;
                UpdatePath();
            }
        }
        public virtual float SweepAngle
        {
            get
            {
                return _sweepAngle;
            }
            set
            {
                _sweepAngle = value;
                UpdatePath();
            }
        }

        #endregion
    }
}