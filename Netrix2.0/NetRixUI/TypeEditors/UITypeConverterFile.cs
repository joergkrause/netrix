using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.IO;

namespace GuruComponents.Netrix.UserInterface.TypeConverters
{
	/// <summary>
	/// Sets and resets the file monikers internally used to ease the input for the user
	/// </summary>
	public class UITypeConverterFile : TypeConverter
	{
		/// <summary>
		/// Check what this type can be created from
		/// </summary>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
		{
			// Just strings for now
			bool canConvert = ( sourceType == typeof(string));
			if (!canConvert)
				canConvert = base.CanConvertFrom (context, sourceType) ;
			return canConvert;
		}

		/// <summary>
		/// Convert from a specified type to a native, if possible. 
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
				retVal = value.ToString();
			}
			else
			{
				retVal = (string) base.ConvertFrom ( context , culture , value ) ;
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
		/// Convert user input to a specified type that is delivered to open file dialog.
		/// </summary>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="culture"></param>
		/// <param name="value">File name</param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			string retVal;
			if (destinationType == typeof(string))
			{
				retVal = value.ToString();
				if (retVal.StartsWith(Path.PathSeparator.ToString()))
				{
					retVal = retVal.Substring(1);
				}
			}
			else
			{
				retVal = (base.ConvertTo (context, culture, value, destinationType)).ToString();
			}
			return retVal ;
		}
	}
}

