using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace GuruComponents.Netrix.Designer
{
	/// <summary>
	/// Handle display of events in PropertyGrid.
	/// </summary>
    /// <remarks>
    /// We handle events as strings, which means we handle the names of the handlers in code here.
    /// </remarks>
	public class EventTypeConverter : TypeConverter
	{
        internal EventTypeConverter()
		{
		}

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType == typeof(string));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (context != null)
            {
                EventPropertyDescriptor ed = context.PropertyDescriptor as EventPropertyDescriptor;
                if (ed != null) ed.SetValue(context.Instance, value);
                return value;
            }
            else return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                if (value.GetType() == typeof(string))
                {
                    return value;
                }
            }
            if (context != null)
            {
                EventPropertyDescriptor ed = context.PropertyDescriptor as EventPropertyDescriptor;
                if (ed != null) return ed.GetValue(context.Instance);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Allows a dropdown list for event handlers.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Allows not only values from dropdown list but also entering a string directly.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <summary>
        /// Get the values to form the list by calling the <c>IEventBindingService</c> service
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            EventPropertyDescriptor ed = context.PropertyDescriptor as EventPropertyDescriptor;
            if (ed != null)
            {
                ICollection col = ed.Service.GetCompatibleMethods(((EventPropertyDescriptor) context.PropertyDescriptor).Event);
                return new StandardValuesCollection(col);
            }else
            {
                return null;
            }
        }

        /// <summary>
        /// Optionally validates the names (Currently not implemented, always returns <c>true</c>).
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return true;
        }

	}
}
