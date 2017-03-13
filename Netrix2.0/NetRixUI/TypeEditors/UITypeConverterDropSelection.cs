using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using GuruComponents.Netrix.WebEditing.Styles;

namespace GuruComponents.Netrix.UserInterface.TypeConverters
{
    /// <summary>
	/// This class provides a type converter which generates predefined values for string properties
	/// as a drop down list.
	/// </summary><remarks>
	/// This is used by properties like usemap, class, and id to have a conjunction
	/// between the current element and connection points in the current document.
	/// </remarks>
	public class UITypeConverterDropSelection : TypeConverter
	{

		/// <summary>
		/// Check what this type can be created from.
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
        /// Get the preselection for either id or classes from the current linked stylesheet, if any.
        /// </summary>
        /// <param name="context">Context from which the editor is called.</param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {            
            MemberInfo[] methods = context.Instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
            MethodInfo mi = null;
            foreach (MethodInfo info in methods)
            {
                if (info.IsDefined(typeof(DropAttribute), true))
                {
                    Attribute[] attributes = (Attribute[]) info.GetCustomAttributes(typeof(DropAttribute), true);
                    if (attributes != null && attributes.Length > 0)
                    {
                        mi = info;
                        break;
                    }
                }
            }
            if (mi != null)
            {
                StyleType st = (StyleType)Enum.Parse(typeof(StyleType), context.PropertyDescriptor.Name, true);
                object rawColl = mi.Invoke(context.Instance, new object[] { st });
                //(Convert.ChangeType(context.Instance, t)).GetDocumentStyleSelectors(context.PropertyDescriptor.Name);
                if (rawColl != null)
                {
                    object[] collection = (object[]) rawColl;
                    if (collection.Length > 0)
                    {                    
                        return new StandardValuesCollection(collection);
                    }
                }
            }
            return base.GetStandardValues(context);
        }

        /// <summary>
        /// This method returns always false and does allow the user to enter own values for this drop list.
        /// </summary>
        /// <param name="context">Context from which the editor is called.</param>
        /// <returns></returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

		/// <summary>
		/// Returns the localized version of the current value into underlying system type.
		/// </summary>
		/// <remarks>
		/// This methos accepts all values except empty strings. Empty strings a treated a "no value" and used to remove
		/// the setting from current selection.
		/// </remarks>
        /// <param name="context">Context from which the editor is called.</param>
        /// <param name="culture">The current UI culture.</param>
        /// <param name="value">The value from which the converter converts.</param>
        /// <returns>The value into which the converter has the input converted.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
            if (context != null)
            {                                
                return (value == null) ? String.Empty : value.ToString();
            } 
            else 
            {
                return base.ConvertFrom(context, culture, value);
            }
		}

    	/// <summary>
		/// Convert the given value from a enum or bool field into the localized version.
		/// </summary>
		/// <remarks>
		/// 
		/// </remarks>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
            if (context != null)
            {
                return (value == null) ? String.Empty : value.ToString();
            } 
            else 
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
		}
	}
}
