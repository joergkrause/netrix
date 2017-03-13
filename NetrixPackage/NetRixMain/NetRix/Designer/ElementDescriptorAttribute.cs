using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Descriptor value which controls the literalcontent attribute for element behaviors.
    /// </summary>
    /// <remarks>
    /// Element behaviors attached via namespaces are a common way to render XML within the HTML document.
    /// To control the render behavior, the literalcontent attribute is used. In NetRix the
    /// <see cref="ElementDescriptorAttribute"/> attribute is used to control the behavior, and the descriptors
    /// replace the attribute accordingly. 
    /// </remarks>
    public enum Descriptor
    {
        /// <summary>
        /// Default behavior (literalcontent=false).
        /// </summary>
        Default = 0,
        /// <summary>
        /// Literal behavior (content is treated as literal, and not rendered, literalcontent=true).
        /// </summary>
        Literal = Interop.ELEMENTDESCRIPTOR_FLAGS_LITERAL,
        /// <summary>
        /// Nested behavior (content is nested literal element, literalcontent=nested).
        /// </summary>
        NestedLiteral = Interop.ELEMENTDESCRIPTOR_FLAGS_NESTED_LITERAL,        
    }


    /// <summary>
    /// Decorates an XML element type to control the render behavior.
    /// </summary>
    /// <remarks>
    /// Element behaviors attached via namespaces are a common way to render XML within the HTML document.
    /// To control the render behavior, the literalcontent attribute is used. In NetRix the
    /// <see cref="Descriptor"/> attributenum is used to control the behavior, and the attribute decorates
    /// the XML element class.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ElementDescriptorAttribute : Attribute
    {
        Descriptor _d = Descriptor.Default;

        /// <summary>
        /// Creates a new attribute instance with the given descriptor.
        /// </summary>
        /// <param name="d">Descriptor (Default is Descriptor.Default).</param>
        public ElementDescriptorAttribute(Descriptor d)
        {
            _d = d;
        }

        /// <summary>
        /// Returns the descriptor, used internally by namespace manager.
        /// </summary>
        public Descriptor Descriptor
        {
            get { return _d; }
        }

    }
}
