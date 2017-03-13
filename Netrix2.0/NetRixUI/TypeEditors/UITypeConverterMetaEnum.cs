using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace GuruComponents.Netrix.UserInterface.TypeConverters
{
	/// <summary>
	/// This class is a type converter used to convert the META tag enumerations into strings.
	/// </summary>
	/// <remarks>
	/// It preserves different (customized) strings. The droplist created contains the following 
	/// META tag names (hardcoded). First, the list of named META tags:
	/// <list type="bullet">
    /// <item>Robots</item>
    /// <item>Description</item>
    /// <item>Keywords</item>
    /// <item>Author</item>
    /// <item>Generator</item>
    /// <item>Formatter</item>
    /// <item>Classification</item>
    /// <item>Copyright</item>
    /// <item>Rating</item>
    /// </list>
    /// Note: The NetRix component will nether set the "Generator" META tag internally. If you
    /// want your application set this tag this must be done programmatically.
    /// <para>
    /// Second, the list of HTTP-Equiv META tags. These tags creating HTTP equivalent headers sent
    /// to the browser.
    /// <list type="bullet">
    /// <item>Content-Disposition</item>
    /// <item>Expires</item>
    /// <item>Pragma</item>
    /// <item>Content-Type</item>
    /// <item>Content-ScriptType</item>
    /// <item>Content-StyleType</item>
    /// <item>Default-Style</item>
    /// <item>Content-Language</item>
    /// <item>Refresh</item>
    /// <item>Window-Target</item>
    /// <item>ExtCache</item>
    /// <item>SetCookie</item>
    /// <item>PICSLabel</item>
    /// <item>Cache-Control</item>
    /// <item>Vary</item>
	/// </list>
	/// </para>
    /// </remarks>
	public class UITypeConverterMetaEnum : TypeConverter
	{

        string[] NameTags = new string[] {   "Robots", 
                                             "Description",
                                             "Keywords",
                                             "Author",
                                             "Generator",
                                             "Formatter",
                                             "Classification",
                                             "Copyright",
                                             "Rating"};
        string[] HttpEquivTags = new string[] {   "Content-Disposition",
                                                  "Expires",
                                                  "Pragma",
                                                  "Content-Type",
                                                  "Content-ScriptType",
                                                  "Content-StyleType",
                                                  "Default-Style",
                                                  "Content-Language",
                                                  "Refresh",
                                                  "Window-Target",
                                                  "ExtCache",
                                                  "SetCookie",
                                                  "PICSLabel",
                                                  "Cache-Control",
                                                  "Vary"};


		/// <summary>
		/// Check what this type can be created from.
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
            if (value == null)
            {
				// TODO: Check for content without having the right class...
//                if (((GuruComponents.Netrix.WebEditing.Elements.MetaElement) context.Instance).HttpEquiv.Equals(String.Empty)
//                    &&
//                    ((GuruComponents.Netrix.WebEditing.Elements.MetaElement) context.Instance).name.Equals(String.Empty)
//                    )
//                {
//                    throw new ArgumentException(@"For a new instance you must define ether the ""name"" or the ""http-equiv"" attribute. It is illegal to leave both empty.");
//                } 
                retVal = String.Empty;
            }
            else
                retVal = (string) value;
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
			return canConvert;
		}

		/// <summary>
		/// Convert to a specified type.
		/// </summary>
		/// <param name="context">Context from which the editor is called.</param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			object retVal = value;
			return retVal;
		}

        /// <summary>
        /// Indicates that the UI should accept user input.
        /// </summary>
        /// <param name="context">Context from which the editor is called.</param>
        /// <returns>Always false.</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <summary>
        /// Indicates that the UI should not request standard values for the list.
        /// </summary>
        /// <param name="context">Context from which the editor is called.</param>
        /// <returns>Always false.</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Get the default list of options.
        /// </summary>
        /// <remarks>
        /// This method is called to fill the list of default values.
        /// </remarks>
        /// <param name="context">Context from which the editor is called.</param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context == null) return null;
            if (context.PropertyDescriptor.Name.ToLower().Equals("httpequiv"))
            {
                return new StandardValuesCollection(HttpEquivTags);
            } 
            if (context.PropertyDescriptor.Name.ToLower().Equals("name"))
            {
                StandardValuesCollection s = new StandardValuesCollection(NameTags);
                return s;
            }
            return null;
        }

	}
}
