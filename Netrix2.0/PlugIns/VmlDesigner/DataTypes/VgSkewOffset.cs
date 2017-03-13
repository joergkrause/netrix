using System;
using System.ComponentModel;
using System.Drawing;

using Comzept.Genesis.NetRix.VgxDraw;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
    /// <summary>
    /// Specifies the offset of the skew.
    /// </summary>
    public class VgSkewOffset
    {

		IVgSkewOffset native;

		internal VgSkewOffset(IVgSkewOffset native)
		{
			this.native = native;
		}

        #region IVgSkewOffset Member

		[Browsable(false)]
        public int Creator
        {
            get
            {
                return native.Creator;
            }
        }

        public double X
        {
            get
            {
                return native.x;
            }
            set
            {
                native.x = value;
            }
        }

        [Browsable(false)]
		public object Application
        {
            get
            {
                return native.Application;
            }
        }

        public string Value
        {
            get
            {
                return native.value;
            }
            set
            {
                native.value = value;
            }
        }

        /// <summary>
        /// Specifies the type of transformation. 
        /// </summary>
        /// <remarks>
        /// Valid values are integer points ranging between -infinity and infinity. 
        /// Type ShapeRelative: The values of the offset are percentages (ratios) of the original shape's size; e.g., a value of 0.5 means an offset half the size of the shape. 
        /// Type Absolute: The values are absolute units. 
        /// </remarks>
        public Comzept.Genesis.NetRix.VgxDraw.VgSkewTransformType Type
        {
            get
            {
                return native.Type;
            }
            set
            {
                native.Type = value;
            }
        }

        [Browsable(false)]
		public object parentSkew
        {
            get
            {
                return native.parentSkew;
            }
        }

        public double Y
        {
            get
            {
                return native.y;
            }
            set
            {
                native.y = value;
            }
        }

        #endregion

		public override string ToString()
		{
			return "SkewOffset Properties";
		}


    }
}

