using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
	/// <summary>
	/// A matrix used for skewing shapes.
	/// </summary>
	/// <remarks>
	/// A matrix used for skewing shapes, a perspective transform matrix in the form, "sxx,sxy,syx,syy,px,py" [s=scale, p=perspective]. If offset is in absolute units, then px,py are in emu ^-1 units; otherwise they are an inverse fraction of shape size.
	/// </remarks>
	public class VgSkewMatrix
	{

        private Comzept.Genesis.NetRix.VgxDraw.IVgSkewMatrix nativeMatrix;

        internal VgSkewMatrix(Comzept.Genesis.NetRix.VgxDraw.IVgSkewMatrix native)
        {
            nativeMatrix = native;
        }


        #region IVgSkewMatrix Member

        [Browsable(false)]
		public int Creator
        {
            get
            {
                return nativeMatrix.Creator;
            }
        }

        public double PerspectiveY
        {
            get
            {
                return nativeMatrix.perspectivey;
            }
            set
            {
                nativeMatrix.perspectivey = value;
            }
        }

		[Browsable(false)]
        public object Application
        {
            get
            {
                return nativeMatrix.Application;
            }
        }

		public Comzept.Genesis.NetRix.VgxDraw.VgSkewTransformType TransformType
        {
            get
            {
                return nativeMatrix.transformtype;
            }
        }

		[RefreshProperties(RefreshProperties.Repaint)]
		public double XToY
        {
            get
            {
                return nativeMatrix.xtoy;
            }
            set
            {
                nativeMatrix.xtoy = value;
            }
        }

		[RefreshProperties(RefreshProperties.Repaint)]
		public double YToY
        {
            get
            {
                return nativeMatrix.ytoy;
            }
            set
            {
                nativeMatrix.ytoy = value;
            }
        }

		[RefreshProperties(RefreshProperties.Repaint)]
		public double YToX
        {
            get
            {
                return nativeMatrix.ytox;
            }
            set
            {
                nativeMatrix.ytox = value;
            }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
		public double XToX
        {
            get
            {
                return nativeMatrix.xtox;
            }
            set
            {
                nativeMatrix.xtox = value;
            }
        }

		[RefreshProperties(RefreshProperties.Repaint)]
		public string Value
        {
            get
            {
                return nativeMatrix.value;
            }
            set
            {
                nativeMatrix.value = value;
            }
        }

        public double PerspectiveX
        {
            get
            {
                return nativeMatrix.perspectivex;
            }
            set
            {
                nativeMatrix.perspectivex = value;
            }
        }

        #endregion


		public override string ToString()
		{
			return "SkewMatrix Properties";
		}


    }
}
