using System;
using GuruComponents.Netrix.UserInterface.TypeEditors;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{

	/// <summary>
	/// DisplayNameAttribute overwrites the property name for the property grid.
	/// </summary>
	/// <remarks>
	/// If the string is empty the PropertyDescriptor tries to use the resource file
	/// to gather the string for displaying.
	/// <para>
	/// If the string is not empty the resource manager tries to gather the information
	/// from the given name instead of the attribute (property) name. This allows the
	/// usage of different properties with the same name.
	/// </para>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class DisplayNameAttribute : System.Attribute
	{

        private string _sText;

        /// <overloads>Instantiate the DisplayNameAttribute class.</overloads>
        /// <summary>
        /// The contructor sets the state to an empty string.
        /// </summary>
        /// <remarks>
        /// This constructor supports the NetRix infrastructure and should never be called from user code directly.
        /// </remarks>
        public DisplayNameAttribute() : base()
        {
            _sText = String.Empty;
        }

        /// <summary>
        /// The constructor sets the property to a defined text.
        /// </summary>
        /// <remarks>
        /// This constructor supports the NetRix infrastructure and should never be called from user code directly.
        /// </remarks>
        /// <param name="Text"></param>
        public DisplayNameAttribute(string Text) : base()
        {
            _sText = Text;
        }
        
        /// <summary>
        /// Returns the display name of the property.
        /// </summary>
        /// <remarks>
        /// This method is necessary to support automatic display option some components
        /// use simple by calling the ToString method.
        /// </remarks>
        /// <returns>The Text being displayed instead of the real property name.</returns>
        public override string ToString()
        {
            return _sText;
        }

        /// <summary>
        /// The Text beeing displayed instead of the real property name.
        /// </summary>
        /// <remarks>
        /// This method is necessary to support automatic display option some components
        /// use simple by calling the Name property. The name of this property is predefined.
        /// </remarks>
        public string DisplayName
        {
            get
            {
                return _sText;
            }
        }
        
	}

}