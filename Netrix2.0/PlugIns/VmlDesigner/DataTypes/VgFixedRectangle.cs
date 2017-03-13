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
	public class VgFixedRectangleArray
	{

        private Comzept.Genesis.NetRix.VgxDraw.IVgFixedRectangleArray native;

        internal VgFixedRectangleArray(Comzept.Genesis.NetRix.VgxDraw.IVgFixedRectangleArray native)
        {
            this.native = native;
		}

		internal Comzept.Genesis.NetRix.VgxDraw.IVgFixedRectangleArray NativeElement
		{
			get
			{
				return native;
			}
		}

		#region IVgFixedRectangleArray Members

		[Browsable(false)]
		public int Creator
		{
			get
			{
				return native.Creator;
			}
		}

		public System.Collections.IEnumerator GetEnumerator()
		{
			return native.GetEnumerator();
		}

		public Comzept.Genesis.NetRix.VgxDraw.IVgFixedRectangle this[int Index]
		{
			get
			{
				return native[Index];
			}
		}

        /// <summary>
        /// Application
        /// </summary>				
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
				return (native == null) ? "" : native.@value;
			}
			set
			{
                if (native != null)
                {
                    native.@value = value;
                }
			}
		}

		public int Length
		{
			get
			{
				return (native == null) ? 0 : native.length;
			}
		}

		#endregion

		public override string ToString()
		{
			return "TextboxRect Properties";
		}



	}
}
