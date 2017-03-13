using System;
using System.ComponentModel;
using System.Drawing;

namespace Comzept.Genesis.NetRix.Tidy
{
	/// <summary>
	/// Properties for TidySharp Extender Provider.
	/// </summary>
    /// <remarks>
    /// </remarks>
    [Serializable()]
	public class TidySharpProperties : ICustomTypeDescriptor
	{

        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetClassName()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetComponentName()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public TypeConverter GetConverter()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EventDescriptor GetDefaultEvent()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object GetEditor(Type editorBaseType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EventDescriptorCollection GetEvents()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public PropertyDescriptorCollection GetProperties()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
