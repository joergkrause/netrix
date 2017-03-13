using System;
using System.Drawing;

namespace Comzept.Library.Drawing.Shapes
{
    public class ShapeStar : ShapePolygon,IRectangle
    {
        private int _corners;

        private float m_Width = 0;
        private float m_Height = 0;


        internal ShapeStar()
        {
            
        }

        private static float ToRad(float deg)
        {

            return (float)(deg / 180 * Math.PI);
        
        }
        public ShapeStar(RectangleF ret, int corners,Pen pen,Brush brush)
        {
            _corners = corners;

            this.Points = Star(ret, corners);

           //this.Size = new SizeF(ret.Size.Width, ret.Size.Height);
            
            this.Pen = pen;
            this.Brush = brush;

            UpdatePath();
        }

        public ShapeStar(RectangleF ret, int corners, float rotation, float diff, Pen pen, Brush brush)
        {
            _corners = corners;

            this.Points = Star(ret, corners, ToRad(rotation), diff);
            this.Rotation = rotation;
            this.Pen = pen;
            this.Brush = brush;

            UpdatePath();
        }
        public ShapeStar(RectangleF ret, int corners, float rotation, Pen pen, Brush brush)
        {
            _corners = corners;

            this.Points = Star(ret, corners, ToRad(rotation));
            this.Rotation = rotation;
            this.Pen = pen;
            this.Brush = brush;

            UpdatePath();
        }
        //***
        public ShapeStar(PointF center, float Ir, float Er, int corners, Pen pen, Brush brush)
        {
            _corners = corners;

            this.Points = Star(center, Ir, Er, corners);
            
            this.Pen = pen;
            this.Brush = brush;

            UpdatePath();
        }
        //***
     
          public ShapeStar(PointF Center, float radius, float internalRadius, int corners,float rotation, Pen pen, Brush brush)
        {
            _corners = corners;

            this.Points = Star(Center,internalRadius,radius,corners);
            this.Rotation = rotation;
            this.Pen = pen;
            this.Brush = brush;

            UpdatePath();
        }
//************************************************************
        public PointF[] Star(RectangleF ret,int num)
         {
             PointF Center = new PointF(ret.Width / 2, ret.Height / 2);
             int ir = (int)( ret.Width / 3);
             int er=(int)ret.Width/2-2;
             return Star(Center, ir, er, num);

         }
         public PointF[] Star(RectangleF ret, int num,float PH,float diff)
         {
             PointF Center = new PointF(ret.Width / 2, ret.Height / 2);
            
             int er = (int)ret.Width / 2 - 2;
             int ir = (int)(er *diff);
             return Star(Center, ir, er, num, PH);

         }
         public PointF[] Star(RectangleF ret, int num, float PH)
         {
             PointF Center = new PointF(ret.Width / 2, ret.Height / 2);
             int ir = (int)(ret.Width / 3);
             int er = (int)ret.Width / 2 - 2;
             return Star(Center, ir, er, num, PH);

         }
         public PointF[] Star(PointF center, int Ir, int Er, int num, float PH)
         {
             PointF[] Points = new PointF[num * 2];

             float AngleStep = (float)(2 * Math.PI) / num;
             float phase = AngleStep / 2;
             float Angle = 0;
             for (int i = 0; i < num * 2; i += 2)
             {
                 Points[i].X = (int)(center.X + Er * Math.Cos(Angle + PH));
                 Points[i].Y = (int)(center.Y + Er * Math.Sin(Angle + PH));
                 Points[i + 1].X = (int)(center.X + Ir * Math.Cos(Angle + phase + PH));
                 Points[i + 1].Y = (int)(center.Y + Ir * Math.Sin(Angle + phase + PH));
                 Angle += AngleStep;
             }
             return Points;

         }
 //***  
        public PointF[] Star(PointF center ,float Ir,float Er, int num)
         {
             PointF[] Points = new PointF[num * 2];

             float AngleStep = (float)(2 * Math.PI) / num;
             float phase= AngleStep/2;
             float Angle = 0;
             for (int i = 0; i < num*2; i+=2)
             {
                 Points[i].X = (float)(center.X + Er*Math.Cos(Angle));
                 Points[i].Y = (float)(center.Y + Er * Math.Sin(Angle));
                 Points[i + 1].X = (float)(center.X + Ir*Math.Cos(Angle  + phase));
                 Points[i + 1].Y = (float)(center.Y +  Ir*Math.Sin(Angle  + phase));
                 Angle += AngleStep;
             }
             return Points;
         }

        public override void Update()
        {
            base.Update();
            this.Points = Star(new RectangleF(this.Location.X, 
                this.Location.Y, m_Width, m_Height)
                , _corners);

            UpdatePath();
        }


         public override string ToString()
        {
            return "{Comzept.Library.Drawing.Shapes.ShapeStar}";
        }


        #region IRectangle Members

        public float Left
        {
            get
            {
                return this.Location.X;
            }
            set
            {
                this.Location = new PointF(value, this.Location.Y);
            }
        }

        public float Top
        {
            get
            {
                return this.Location.Y;
            }
            set
            {
                this.Location = new PointF(this.Location.X, value);
            }
        }

        public float Width
        {
            get
            {
                return m_Width;
            }
            set
            {
                m_Width = value;
            }
        }

        public float Height
        {
            get
            {
                return m_Height;
            }
            set
            {
                m_Height = value;
            }
        }

        #endregion
}
}
