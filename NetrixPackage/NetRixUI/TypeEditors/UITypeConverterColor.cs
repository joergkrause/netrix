using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;

namespace GuruComponents.Netrix.UserInterface.TypeConverters
{
	/// <summary>
	/// This class provides a type converter to convert instances of color from an to html color codes.
	/// </summary>
	/// <remarks>
	/// Html
	/// color codes are written as three hex codes with a preceding # sign. If the user types a well know name
	/// the control converts it into the #HHHHHH representing the color.
	/// </remarks>
	public class UITypeConverterColor : TypeConverter
	{

        static string colorError = "This is not a HTML color code nor a known color name. Please enter a valid code or leave the field empty and hit ENTER to remove the color attribute.";

		/// <summary>
		/// Check what this type can be created from
		/// </summary>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="sourceType">The type of the source data.</param>
		/// <returns>True if the source type can be converted.</returns>
		public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
		{
			// Just strings for now
			bool canConvert = ( sourceType == typeof(string));
            if (!canConvert)
            {
                canConvert = base.CanConvertFrom (context, sourceType) ;
            }
			return canConvert;
		}

		/// <summary>
		/// Convert from a specified type to a native, if possible.
		/// </summary>
		/// <remarks>
		/// This function tries to set
		/// allways colors as hex strings, even if the user types names.
		/// </remarks>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="culture"></param>
		/// <param name="value">Color of HTML format</param>
		/// <returns>The converted data as <see langref="object"/>.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			object retVal;            
			if (value is string)
			{
				string val = value.ToString();
                Color col;
				if (val.Equals(String.Empty)) return Color.Empty;
                try
                {
                    col = ColorTranslator.FromHtml(val);
                }
                catch
                {
                    throw new ArgumentException(colorError);
                }
				if (col.A == 0)
				{
					// Unknown color name or #RRGGBB?
                    if (val[0] == '#')
                    {
                        if (val.Length == 7)
                        {
                            retVal = val;
                        }
                        else
                        {
                            retVal = (val.Length < 7) ? val.PadRight(7, '0') : val.Substring(0, 7);
                        }
                        try
                        {
                            col = ColorTranslator.FromHtml(retVal.ToString());
                        }
                        catch
                        {
                            throw new ArgumentException(colorError);
                        }
                    }
                    else if (val.Length == 6)
                    {
                        val = "#" + val;
                        col = ColorTranslator.FromHtml(val);
                        if (col.A == 0)
                        {
                            col = Color.Empty;
                        }
                    }
				} 
				retVal = col;
			}
			else                 
			{
				retVal = base.ConvertFrom (context, culture, value);
			}
            return retVal;
		}

		/// <summary>
		/// Check what the type can be converted to.
		/// </summary>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="destinationType">The destination type in which the converter converts.</param>
		/// <returns>True if conversion is possible.</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			// InstanceDescriptor is used in the code behind
			bool canConvert = (destinationType == typeof (InstanceDescriptor));
			if (!canConvert) 
				canConvert = base.CanConvertFrom (context, destinationType) ;
			return canConvert ;
		}

		/// <summary>
		/// Convert user input to a specified type that is delivered to color control.
		/// </summary>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="culture">The current culture (not applicable in NetRix).</param>
		/// <param name="value">Color of type <see cref="System.Drawing.Color"/>.</param>
		/// <param name="destinationType">The destination type in which the converter converts.</param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			object retVal = null;
            if (destinationType == typeof(string))
			{
				if (value is Color)
				{
					Color c = (Color) value;
					if (c == Color.Empty)
						retVal = String.Empty;
					else
						retVal = ColorTranslator.ToHtml(c);
				} 
				else 
				{
					retVal = String.Empty;
				}
			}
			else
			{
				retVal = base.ConvertTo (context, culture, value, destinationType);
			}
            return retVal ;
		}
	}
}

