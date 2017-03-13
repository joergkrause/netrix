using System;
using System.ComponentModel;
using System.Drawing;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
	/// <summary>
	/// Specifies a color. 
	/// </summary>
	public class VgColor
	{

        private Color color;
        private Comzept.Genesis.NetRix.VgxDraw.VgColorType type;
        private Comzept.Genesis.NetRix.VgxDraw.IVgColor nativeColor;

        /// <overloads/>
        /// <summary>
        /// Constructor. Creates a new instance based on .NET <see cref="System.Drawing.Color">Color</see> type.
        /// </summary>
        /// <param name="color"></param>
		public VgColor(Color color)
		{
            this.color = color;
            type = Comzept.Genesis.NetRix.VgxDraw.VgColorType.vgColorTypeMixed;
        }

        internal VgColor(Comzept.Genesis.NetRix.VgxDraw.IVgColor color)
        {
            nativeColor = color;
            this.color = Color.FromArgb(nativeColor.RGB);
            type = nativeColor.Type;
        }

        /// <summary>
        /// Returns the native (COM) color object.
        /// </summary>
        internal Comzept.Genesis.NetRix.VgxDraw.IVgColor NativeColor
        {
            get
            {
                return nativeColor;
            }
        }

        #region IVgColor Member

		[Browsable(false)]
        public int Creator
        {
            get
            {
                return nativeColor.Creator;
            }
        }

		[Browsable(false)]
		public int RGB
        {
            get
            {
                return color.ToArgb();
            }
            set
            {
                color = Color.FromArgb(value);
                type = Comzept.Genesis.NetRix.VgxDraw.VgColorType.vgColorTypeRGB;
            }
        }

        /// <summary>
        /// Blue component of the color. Can range between 0 and 255.
        /// </summary>
        public int B
        {
            get
            {
                return color.B;
            }
            set
            {
                color = Color.FromArgb(this.R, this.G, value);
                type = Comzept.Genesis.NetRix.VgxDraw.VgColorType.vgColorTypeRGB;
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
                return nativeColor.Application;
            }
        }

        /// <summary>
        /// Red component of the color. Can range between 0 and 255. 
        /// </summary>
        public int R
        {
            get
            {
                return color.R;
            }
            set
            {
                color = Color.FromArgb(value, this.G, this.B);
                type = Comzept.Genesis.NetRix.VgxDraw.VgColorType.vgColorTypeRGB;
            }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
		public Color Value
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                if (color.IsNamedColor)
                {
                    type = Comzept.Genesis.NetRix.VgxDraw.VgColorType.vgColorTypeNamed;
                } 
                else 
                {
                    type = Comzept.Genesis.NetRix.VgxDraw.VgColorType.vgColorTypeMixed;
                }
            }
        }

        /// <summary>
        /// Type of color.
        /// </summary>
        public Comzept.Genesis.NetRix.VgxDraw.VgColorType Type
        {
            get
            {
                return (Comzept.Genesis.NetRix.VgxDraw.VgColorType)nativeColor.Type;
            }
        }

        /// <summary>
        /// Green component of the color. Can range between 0 and 255.
        /// </summary>
        public int G
        {
            get
            {
                return color.G;
            }
            set
            {
                color = Color.FromArgb(this.R, value, this.B);
                type = Comzept.Genesis.NetRix.VgxDraw.VgColorType.vgColorTypeRGB;
            }
        }

        #endregion

		public override string ToString()
		{
			return "Color Properties";
		}


    }
}
