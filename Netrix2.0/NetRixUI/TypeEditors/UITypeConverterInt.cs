using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace GuruComponents.Netrix.UserInterface.TypeConverters
{
	/// <summary>
	/// This class provides a type converter to convert instances of color from an to html color codes. Html
	/// color codes are written as three hex codes with a preceding # sign. If the user types a well know name
	/// the control converts it into the #HHHHHH representing the color.
	/// </summary>
	public class UITypeConverterInt : TypeConverter
	{
		/// <summary>
		/// Check what this type can be created from
		/// </summary>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
		{
			// Just int and empty for now
			bool canConvert;
			canConvert = (sourceType == typeof(string) || sourceType == null);
			if (!canConvert)
				canConvert = base.CanConvertFrom (context, sourceType) ;
			return canConvert;
		}

		/// <summary>
		/// Returns the localized version of the current value into underlying system type.
		/// </summary>
        /// <param name="context">Context from which the editor is called.</param>
        /// <param name="culture">The current UI culture.</param>
        /// <param name="value">The value from which the converter converts.</param>
        /// <returns>The value into which the converter has the input converted.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string retVal;
			if (value is string)
			{
				try
				{
					retVal = Int32.Parse(value.ToString()).ToString();
				}
				catch
				{
					retVal = String.Empty;
				}
			}
			else
			{
				retVal = (string) base.ConvertFrom (context, culture, value);
			}
			return retVal;
		}

		/// <summary>
		/// Check what the type can be converted to.
		/// </summary>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			// InstanceDescriptor is used in the code behind
			bool canConvert = (destinationType == typeof (InstanceDescriptor));
			if (!canConvert) 
				canConvert = base.CanConvertFrom (context, destinationType) ;
			return canConvert ;
		}

		/// <summary>
		/// Convert to a specified type
		/// </summary>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			object retVal;
			if (destinationType == typeof(string))
			{
				retVal = value.ToString().ToUpper();
			}
			else
			{
				retVal = base.ConvertTo (context, culture, value, destinationType);
			}
			return retVal ;
		}
	}
}
