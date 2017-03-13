using System;
using System.Reflection;

namespace GuruComponents.Netrix.UserInterface.TypeConverters
{
	/// <summary>
	/// This attribute is used to sign a method, which can return a list, which in turn is used to
	/// fill a dropdown element of the corresponding PropertyGrid's property item. 
	/// </summary>
	/// <remarks>
	/// Implementers of Plug-Ins who wish to implement XML elements, can extend their design
	/// time support by adding this attribute to exact one method. Each property, which has the
    /// <c>UITypeConverterDropSelection</c> (in UI assembly) type converter attribute attached, will use
	/// that method as a callback to fill the appropriate list.
	/// <para>
    /// The behavior can be extended by inheriting from this and the <c>UITypeConverterDropSelection</c>
	/// class and implementing additional constructor overloads, which the derived classes can use to
	/// start different action apart from default one.
	/// </para>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method)]
	public class DropAttribute : Attribute
	{
        /// <summary>
        /// Ctor for Drop attribute.
        /// </summary>
		public DropAttribute() : base()
		{
		}
	}
}
