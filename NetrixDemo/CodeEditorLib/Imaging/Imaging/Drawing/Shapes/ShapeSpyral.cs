using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Comzept.Library.Drawing.Shapes
{

	public class ShapeSpyral  :ShapeCurve
	{
        private PointF[] _visiblePoints;
		private PointF _startPoint;
		private float _frequency;
		private int _numSteps;

        #region Constructors

        public ShapeSpyral(float xStartPoint,float yStartPoint, float frequency, int numSteps)
            : this(new PointF(xStartPoint,yStartPoint), frequency, numSteps, 0, null) { }
        public ShapeSpyral(float xStartPoint, float yStartPoint, float frequency, int numSteps, float rotation)
            : this(new PointF(xStartPoint, yStartPoint), frequency, numSteps, rotation, null) { }
        public ShapeSpyral(float xStartPoint, float yStartPoint, float frequency, int numSteps, float rotation, Pen pen)
            : this(new PointF(xStartPoint, yStartPoint), frequency, numSteps, rotation, pen) { }

        public ShapeSpyral(PointF startPoint, float frequency, int numSteps)
            : this(startPoint, frequency, numSteps, 0, null) { }
        public ShapeSpyral(PointF startPoint, float frequency, int numSteps, float rotation)
            : this(startPoint, frequency, numSteps, rotation, null) { }
		public ShapeSpyral(PointF startPoint, float frequency, int numSteps, float rotation, Pen pen)
			:base()
		{
			_startPoint = startPoint;
			_frequency = frequency;
            if (_frequency < 1) _frequency = 1;
			_numSteps = numSteps;
            if (_numSteps < 1) _numSteps = 1;

			this.Rotation = rotation;
			this.Pen = pen;

            InternalRotationBasePoint = _startPoint;

			UpdatePath();
        }

        #endregion

        #region Overrides

        public override PointF StartPoint
        {
            get
            {
                return _startPoint;
            }
            set
            {
                _startPoint = value;
                UpdatePath();
            }
        }
        public override PointF[] Points
        {
            get
            {
                return _visiblePoints;
            }
            set
            {
                // base.Points = value;
            }
        }

        public override Image GetThumb(ShapeThumbSize thumbSize)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override string ToString()
        {
            return "{Comzept.Library.Drawing.Shapes.ShapeSpyral}";
        }

        protected override void UpdatePath()
		{
			if (_numSteps == 0) return;

            PointF[] pts = CalculatePoints();
			InternalPath = new GraphicsPath();
            InternalPath.AddCurve(pts, 0, pts.Length - 2, 0.85f);

            _visiblePoints = new PointF[pts.Length - 1];
            Array.Copy(pts, _visiblePoints, _visiblePoints.Length);

			Matrix mtx = new Matrix();
			mtx.RotateAt(this.Rotation, InternalRotationBasePoint);
			InternalPath.Transform(mtx);
        }

        #endregion

        #region Private Members

        private PointF[] CalculatePoints()
		{
			int pointCount = (4 * _numSteps);
			PointF[] points = new PointF[pointCount];
			points[0]=_startPoint;
			PointF prevPoint = points[0];
			int lastQuadrant = 1;
			float step = _frequency / 4;

			for (int i = 1; i < pointCount; i++)
			{
				switch (lastQuadrant)
				{
					default:
					case 1:
						points[i] = new PointF(prevPoint.X - (i * step), prevPoint.Y + (i * step));
						break;
					case 2:
						points[i] = new PointF(prevPoint.X - (i * step), prevPoint.Y - (i * step));
						break;
					case 3:
						points[i] = new PointF(prevPoint.X + (i * step), prevPoint.Y - (i * step));
						break;
					case 4:
						points[i] = new PointF(prevPoint.X + (i * step), prevPoint.Y + (i * step));
						break;
				}

				prevPoint = points[i];

				lastQuadrant++;
				if (lastQuadrant > 4) lastQuadrant = 1;
			}

            //points[points.Length - 1] = new PointF(prevPoint.X-5, prevPoint.Y-10);// new PointF(prevPoint.X, prevPoint.Y - 20);

			return points;
        }

        #endregion

    }
}