using System;
using System.ComponentModel;
using System.Drawing;

using Comzept.Genesis.NetRix.VgxDraw;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes {
    /// <summary>
    /// Definition of a vector of Point.
    /// </summary>
    public class VgVector2D
	{
        private IVgVector2D nativeVector;
		private PointF directVector;

        public VgVector2D() {
            directVector = new PointF(0F, 0F);
        }

		public VgVector2D(string coords) 
		{
			string[] c = coords.Split(' ', ',');
			directVector = new PointF(Single.Parse(c[0]), Single.Parse(c[1]));
		}

        public VgVector2D(IVgVector2D nativeVector) 
		{
            this.nativeVector = nativeVector;
        }

        /// <summary>
        /// Gets or sets the vector in Pixel co-ordinates.
        /// </summary>
        public Point Pixel {
            get {
                //return new Point(Convert.ToInt32(this.x / 0.75), Convert.ToInt32(this.y / 0.75));
				return new Point(Convert.ToInt32(this.x), Convert.ToInt32(this.y));
            }
            set {
                //this.x = value.X * 0.75;
                //this.y = value.Y * 0.75;
				this.x = value.X;
				this.y = value.Y;
			}
        }

        /// <summary>
        /// Used internally to set the attribute back. Later, when the native wrapper classes are ready, we 
        /// will remove this and let the wrapper classes handle the internal stuff.
        /// </summary>
        internal IVgVector2D NativeVector {
            get {
                return this.nativeVector;
            }
        }

        #region IVgVector2D Member

        [Browsable(false)]
        public object Application {
            get {
                return nativeVector.Application;
            }
        }
        [Browsable(false)]
        public int Creator {
            get {
                return nativeVector.Creator;
            }
        }
        [Browsable(false)]
        public string value {
            get {
                return nativeVector.@value;
            }
            set {
                nativeVector.@value = value;
            }
        }
        [Browsable(false)]
        public object Parent {
            get {
                return nativeVector.Parent;
            }
        }
        [Browsable(false)]
        public double x {
            get {
				if (nativeVector == null)
				{
					return directVector.X;
				} 
				else 
				{
					return ((int)nativeVector.x == System.Int32.MinValue) ? 0D : nativeVector.x;
				}
            }
            set {
				if (nativeVector == null)
				{
					directVector.X = (float)value;
				} 
				else 
				{
					nativeVector.x = value;
				}
            }
        }
        [Browsable(false)]
		public double y 
		{
			get 
			{
				if (nativeVector == null)
				{
					return directVector.Y;
				} 
				else 
				{
					return ((int)nativeVector.y == System.Int32.MinValue) ? 0D : nativeVector.y;
				}
			}
			set 
			{
				if (nativeVector == null)
				{
					directVector.Y = (float)value;
				} 
				else 
				{
					nativeVector.y = value;
				}
			}
		}

        [Browsable(false)]
        public VgVectorType Type {
            get {
                return nativeVector.Type;
            }
        }

        #endregion

        public static VgVector2D Empty {
            get {
                VgVector2D empty = new VgVector2D();
                empty.x = 0;
                empty.y = 0;
                return empty;
            }
        }

        public static VgVector2D Default {
            get {
                VgVector2D def = new VgVector2D();
                def.x = 1000;
                def.y = 1000;
                return def;
            }
        }

        public override string ToString() {
            return String.Format("VgVector2D [{0}:{1}]", x, y);
        }


    }
}
