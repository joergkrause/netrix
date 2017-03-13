using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace GuruComponents.Netrix.UserInterface.StyleParser
{

    /// <summary>
    ///  Utility class with various useful static functions.
    /// </summary>
    internal class Util
    {
        private static System.Resources.ResourceManager _ResourceManager = null;

        internal static System.Resources.ResourceManager GetResourceManager()
        {
            if (_ResourceManager == null)
            {
                Type ourType = typeof(Util);
                _ResourceManager = new System.Resources.ResourceManager(ourType.Namespace, ourType.Module.Assembly);
            }

            return _ResourceManager;
        }

        internal static string GetStringResource(string name)
        {
            return (string)GetResourceManager().GetObject(name);
        }

        /// <summary>
        ///  Converts a FontUnit to a size for the HTML FONT tag
        /// </summary>
        internal static string ConvertToHtmlFontSize(FontUnit fu)
        {
            if ((int)(fu.Type) > 3)
            {
                return ((int)(fu.Type)-3).ToString();
            }

            if (fu.Type == FontSize.AsUnit) {
                if (fu.Unit.Type == UnitType.Point) {
                    if (fu.Unit.Value <= 8)
                        return "1";
                    else if (fu.Unit.Value <= 10)
                        return "2";
                    else if (fu.Unit.Value <= 12)
                        return "3";
                    else if (fu.Unit.Value <= 14)
                        return "4";
                    else if (fu.Unit.Value <= 18)
                        return "5";
                    else if (fu.Unit.Value <= 24)
                        return "6";
                    else
                        return "7";
                }
            }

            return null;
        }

        /// <summary>
        ///  Searches for an object's parents for a Form object
        /// </summary>
        internal static Control FindForm(Control child)
        {
            Control parent = child;

            while (parent != null)
            {
                if (parent is HtmlForm)
                    break;

                parent = parent.Parent;
            }

            return parent;
        }

        /// <summary>
        /// Given a string that contains a number, extracts the substring containing the number.
        /// Returns only the first number.
        /// Example: "5px" returns "5"
        /// </summary>
        /// <param name="str">The string containing the number.</param>
        /// <returns>The extracted number or String.Empty.</returns>
        internal static string ExtractNumberString(string str)
        {
            string[] strings = ExtractNumberStrings(str);
            if (strings.Length > 0)
            {
                return strings[0];
            }

            return String.Empty;
        }

        /// <summary>
        /// Extracts all numbers from the string.
        /// </summary>
        /// <param name="str">The string containing numbers.</param>
        /// <returns>An array of the numbers as strings.</returns>
        internal static string[] ExtractNumberStrings(string str)
        {
            if (str == null)
            {
                return new string[0];
            }

            // Match the digits
            MatchCollection matches = Regex.Matches(str, @"(-?\d+\.?\d{0,2})", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Move to a string array
            string[] strings = new string[matches.Count];
            int index = 0;

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    strings[index] = match.Value;
                }
                else
                {
                    strings[index] = String.Empty;
                }

                index++;
            }

            return strings;
        }

        /// <summary>
        ///  Converts a color string to a hex value string ("Green" -> "#000800")
        /// </summary>
        internal static string ColorToHexString(string color)
        {
			try
			{
				if (color.Length == 0) return String.Empty;
				Color c = RgbColorConverter(color);
				return ColorToHexString(c);
			} 
			catch
			{
				return String.Empty;
			}            
        }

        private static Color RgbColorConverter(string cssColor)
        {
            if (cssColor.StartsWith("rgb("))
            {
                string[] rgb = cssColor.Substring(4, cssColor.Length - 5).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                cssColor = String.Format("#{0:X2}{1:X2}{2:X2}",
                    Int32.Parse(rgb[0], System.Globalization.NumberStyles.Number),
                    Int32.Parse(rgb[1], System.Globalization.NumberStyles.Number),
                    Int32.Parse(rgb[2], System.Globalization.NumberStyles.Number)
                    );
            }
            return ColorTranslator.FromHtml(cssColor);
        }

        /// <summary>
        ///  Converts a Color to a hex value string (Color.Green -> "#000800")
        /// </summary>
        internal static string ColorToHexString(Color c)
        {
            string r = Convert.ToString(c.R, 16);
            if (r.Length < 2)
                r = "0" + r;
            string g = Convert.ToString(c.G, 16);
            if (g.Length < 2)
                g = "0" + g;
            string b = Convert.ToString(c.B, 16);
            if (b.Length < 2)
                b = "0" + b;

            string str = "#" + r + g + b;
            return str;
        }

		internal static string ColorToHexString(short R, short G, short B)
		{
			string r = Convert.ToString(R, 16);
			if (r.Length < 2)
				r = "0" + r;
			string g = Convert.ToString(G, 16);
			if (g.Length < 2)
				g = "0" + g;
			string b = Convert.ToString(B, 16);
			if (b.Length < 2)
				b = "0" + b;

			string str = "#" + r + g + b;
			return str;
		}

		internal static string HexStringToColorName(string hex)
		{
			string hexValues;
			string name = hex;
			if (hex[0] != '#' && hex.Length == 6)
			{
				hexValues = "#" + hex;
			} 
			else 
			{
				hexValues = hex;
			}
			if (hexValues.Length != 7)
			{
				throw new ArgumentException("Given string does not match color values");
			} 
			else 
			{
				try
				{
					name = ColorName(RgbColorConverter(hexValues));
				}
				catch 
				{
					// no action if translation fails
				}
			}
			return name;
		}

        private static string ColorName(Color c)
        {
            if (c.Name == "0") return "";
            if (c.IsNamedColor) return c.Name;
            foreach (KnownColor kcc in Enum.GetValues(typeof(KnownColor)))
            {
                if (kcc.ToString().StartsWith("Active") || kcc.ToString().StartsWith("Control") || kcc.ToString().StartsWith("Highlight") || kcc.ToString().StartsWith("Info") || kcc.ToString().StartsWith("Menu") || kcc.ToString().StartsWith("Window") || kcc.ToString().StartsWith("Transparent")) continue;
                Color cc = Color.FromKnownColor(kcc);
                if (cc.R == c.R && cc.G == c.G && cc.B == c.B) return kcc.ToString();
            }
            return ColorTranslator.ToHtml(c).ToUpper();
        }

        /// <summary>
        /// Converts a hex string into name and resulting int color.
        /// </summary>
        /// <param name="hex">The hex value to convert.</param>
        /// <param name="_r">The R (red) part.</param>
        /// <param name="_g">The G (green) part.</param>
        /// <param name="_b">The B part (blue).</param>
        /// <returns>An integer value of the whole color.</returns>
		internal static int HexStringToRgb(string hex, ref short _r, ref short _g, ref short _b)
		{
			string hexValues;
			int sum = 0;
			if (hex.Length == 0)
			{
				_r = _b = _g = 0;
				return 0;
			}
			if (hex[0] == '#')
			{
				hexValues = hex.Substring(1);
			} 
			else 
			{
				hexValues = hex;
			}
			if (hexValues.Length != 6)
			{
				_r = _b = _g = 0;
				return 0;
			} 
			else 
			{
				for (int i = 0, j = 65536, r = 1; i < 6; i+=2, j/=256, r++)
				{
					short val = Convert.ToInt16(hexValues.Substring(i, 2), 16);
					sum += val * j;
					switch (r)
					{
						case 1: _r = val; break;
						case 2: _g = val; break;
						case 3: _b = val; break;
					}
				}
			}
			return sum;
		}

    }
}
