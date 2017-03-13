using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace GuruComponents.Netrix.UserInterface.TypeConverters
{
	/// <summary>
	/// This class provides a type converter to convert dropdown lists to divide the UI (language) from
	/// the internal value. It handles dropdown lists generated from enum or boolean values. In the resource
	/// file the descriptor has the format "PropertyGrid.XXX" whereas "XXX" is the name of the enum type or
	/// the string "Boolean". The type is given with its name only, without any part of the namespace.
	/// </summary>
	public class UITypeConverterDropList : TypeConverter
	{

		/// <summary>
		/// Check what this type can be created from
		/// </summary>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
		{
			bool canConvert;
			canConvert = (sourceType == typeof(string) || sourceType == null);
			if (!canConvert)
				canConvert = base.CanConvertFrom (context, sourceType) ;
			return canConvert;
		}

        /// <summary>
        /// Check what the type can be converted to
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
        /// Indicates that the UI should request standard values for the list.
        /// </summary>
        /// <param name="context">Context from which the editor is called.</param>
        /// <returns>Always true.</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        
        /// <summary>
        /// Get the collection of default values for the list.
        /// </summary>
        /// <remarks>
        /// For boolean types a short list with the values "true" and "false" will be created.
        /// <para>
        /// Use this attribute to get this typeconverter working:
        /// [TypeConverter(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterDropList))]
        /// </para>
        /// </remarks>
        /// <param name="context">Context from which the editor is called.</param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            // we need to populate the original list here, because the grid loops on double click through the
            // original type. Afterwards the ConvertXX methods will convert the value to set the display correctly.
            // Using the localized version here will cause a type conversion exception because string is not convertible to enum.
            // Convert enum into array...
            if (context.PropertyDescriptor.PropertyType.BaseType.Name.Equals("Enum"))
            {
                return new StandardValuesCollection(Enum.GetValues(context.PropertyDescriptor.PropertyType));
            }
            if (context.PropertyDescriptor.PropertyType.IsValueType)
            {
                if (context.PropertyDescriptor.PropertyType.UnderlyingSystemType.Name.Equals("Boolean"))
                {
                    return new StandardValuesCollection(new object[] { true, false });
                }
            }
            return base.GetStandardValues(context);
        }

        /// <summary>
        /// This method returns always true and does not allow the user to enter own values 
        /// for droplists.
        /// </summary>
        /// <remarks>
        /// This behavior can be modified by detecting the property name and 
        /// decide to return false instead. 
        /// </remarks>
        /// <param name="context">Context from which the editor is called.</param>
        /// <returns></returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

		/// <summary>
		/// Returns the localized version of the current value into underlying system type.
		/// </summary>
		/// <remarks>
		/// For boolean values,
		/// treated here as a collection of two list elements (which represent true and false), the resource must
		/// return the text for "true" as first value, the text for "false" as second one.
		/// </remarks>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="culture">The current UI culture.</param>
		/// <param name="value">The value from which the converter converts.</param>
		/// <returns>The value into which the converter has the input converted.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
            string retVal = (value == null) ? String.Empty : value.ToString();
		    if (context != null)
            {
                Type t = context.PropertyDescriptor.PropertyType;
                object o;
                if (ResourceManager.GetGridLanguage() == ResourceManager.GridLanguageType.Localized)
                {
                    StringCollection ic = new StringCollection();
                    string propValue =
                        ResourceManager.GetString(
                            String.Concat("PropertyGrid", ".", context.PropertyDescriptor.PropertyType.Name));
                    ic.AddRange(propValue.Split(','));
                    int i = ic.IndexOf(retVal);
                    if (t.Name.Equals("Boolean"))
                    {
                        o = (i == 0) ? true : false;
                    }
                    else
                    {
                        o = Enum.Parse(t, Enum.GetName(t, (i > -1) ? i : 0), true);
                    }
                    return o;
                }
                else
                {
                    if (t.Name.Equals("Boolean"))
                    {
                        o = (retVal.ToLower().Equals("true")) ? true : false;
                    }
                    else
                    {
                        o = Enum.Parse(t, retVal, true);
                    }
                    return o;
                }
            }
		    return null;
		}

    	/// <summary>
		/// Convert the given value from a enum or bool field into the localized version.
		/// </summary>
		/// <remarks>
		/// The boolean values are treated as some special kind of enum, because we display the values as dropdownlist containing "Yes" or "No" list elements.
		/// </remarks>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="culture">The current UI culture.</param>
		/// <param name="value">The value beeing converted.</param>
		/// <param name="destinationType">The type to which the converter converts the value.</param>
		/// <returns>The converted value.</returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
            if (context != null && ResourceManager.GetGridLanguage() == ResourceManager.GridLanguageType.Localized)
            {
                StringCollection ic = new StringCollection();
                Type t = context.PropertyDescriptor.PropertyType;
                string propValue = ResourceManager.GetString(String.Concat("PropertyGrid", ".", t.Name));
				if (propValue != null)
				{
					ic.AddRange(propValue.Split(',')); // enhance the char list to support comma in asian languages; file must be saved as UTF-8 to protect the char then
				}
                string returnValue = String.Empty;
                if (t.Name.Equals("Boolean") && ic.Count == 2)
                {
                    returnValue = ((bool) value) ? ic[0] : ic[1];
                }
                else 
                {
                    if (value == null)
                    {
                        // can be null if multiple object selected and list is not filled due to concurrent values
                        returnValue = String.Empty;
                    } 
                    else 
                    {
                        string[] names = Enum.GetNames(t);
                        for (int i = 0; i < names.Length && ic.Count > i; i++)
                        {
                            if (value.ToString() == Enum.GetName(t, i))
                            {
                                returnValue = ic[i];
                                break;
                            }
                        }
                    }
                }
                return returnValue;
            } 
            else 
            {
                return  (value == null) ? String.Empty : value.ToString(); // base.ConvertTo(context, culture, value, destinationType);
            }
		}
	}
}
