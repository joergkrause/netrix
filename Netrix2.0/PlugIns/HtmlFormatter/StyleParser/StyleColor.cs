using System;
using System.Drawing;

namespace GuruComponents.Netrix.UserInterface.StyleParser
{
	/// <summary>
	/// Zusammenfassung für StyleColor.
	/// </summary>
	[Serializable()]
    public class StyleColor
	{
		private short _r, _g, _b;
		private string _name;
		private string _html;

        /// <summary>
        /// Set the colors to no color.
        /// </summary>
		public StyleColor()
		{
			_r = _g = _b = 0;
			_name = String.Empty;
		}

		private void Rgb2Name()
		{
			_html = Util.ColorToHexString(_r, _g, _b);
			_name = Util.HexStringToColorName(_html);
		}
		private void Name2Rgb()
		{
			_html = Util.ColorToHexString(_name);
			Util.HexStringToRgb(_html, ref _r, ref _g, ref _b);
		}
		/// <summary>
		/// Gets or sets the value for color Red.
		/// </summary>
		public short R
		{
			get { return _r; }
			set { _r = value; }
		}
		/// <summary>
		/// Gets or sets the value for color Green.
		/// </summary>
		public short G
		{
			get { return _g; }
			set { _g = value; }
		}
		/// <summary>
		/// Gets or sets the value for color Blue.
		/// </summary>
		public short B
		{
			get { return _b; }
			set { _b = value; }
		}
		/// <summary>
		/// Gets or sets the color name and internally the appropriciate RGB values.
		/// </summary>
		public string Name
		{
			get { return _name; }
			set 
			{
				_name = value; 
				Name2Rgb();
			}
		}

        /// <summary>
        /// HTML form of color.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ColorTranslator.ToHtml(Color.FromArgb(R, G, B));
        }

	}
}
