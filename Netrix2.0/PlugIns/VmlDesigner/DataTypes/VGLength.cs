using System;
using System.Drawing;

using Comzept.Genesis.NetRix.VgxDraw;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
	/// <summary>
	/// A positive signed integer that indicates a length of measurement. 
	/// </summary>
	/// <remarks>
	/// The unit value of this is an Emu (English Metrical Unit). There are 914,400 emus in an inch. The class
	/// provides some ways to re-calculate for different units.
	/// </remarks>
	public class VgLength
	{

        double emu;

		public VgLength(double emu)
		{
			this.emu = emu;
		}

        public double Inch
        {
            get
            {
                return emu / 72;
            }
        }

        public int Pixel
        {
            get
            {
                return Convert.ToInt32(emu);
            }
        }


	}
}
