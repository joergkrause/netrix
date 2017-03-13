using System;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms.Design;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{

	/// <summary>
	/// Modifies the descriptor content populated to propertygrid.
	/// </summary>
	/// <remarks>
	/// This overwrites the
	/// default property names and enhances the standard attributes with the DisplayNameAttribute.
	/// </remarks>
	public class WebControlPropertyDescriptor : PropertyDescriptor
	{
        private PropertyDescriptor baseProp;
        Attribute[] _filter;

        /// <summary>
        /// The constructor called on first access to the property from any designer.
        /// </summary>
        /// <remarks>
        /// This class should not being instantiated directly. It supports the NetRix infrastructure
        /// and external designers, like VS.NET or the propertygrid.
        /// </remarks>
        /// <param name="pd">The descriptor for which the properties beeing requested.</param>
        /// <param name="filter">The filter which controls the properties not beeing shown.</param>
        public WebControlPropertyDescriptor(PropertyDescriptor pd, Attribute[] filter) : base(pd)
        {
            baseProp = pd;             
            _filter = filter;
        }

        /// <summary>
        /// Makes the decsriptor localizable.
        /// </summary>
        /// <remarks>
        /// This property is overwritten to return always <c>true</c>.
        /// </remarks>
        public override bool IsLocalizable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns whether this property is browsable (viewable) in the grid.
        /// </summary>
        public override bool IsBrowsable 
        { 
            get 
            { 
                return base.IsBrowsable; 
            } 
        }

        /// <summary>
        /// Returns whether this property is readonly.
        /// </summary>
        public override bool IsReadOnly 
        { 
            get 
            {
                return baseProp.IsReadOnly;
            } 
        } 
 
        /// <summary>
        /// Returns whether this property resetting when the value is overwritten. 
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public override bool CanResetValue(object component) 
        { 
            return this.baseProp.CanResetValue(component); 
        } 
 
        /// <summary>
        /// Returns the type of the component containing this property.
        /// </summary>
        public override Type ComponentType 
        { 
            get
            {
                return baseProp.ComponentType;
            } 
        } 
 
        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        /// <param name="component">The component which contains the property.</param>
        /// <returns>An object that represents the value. It is up to the caller to cast to the right type.</returns>
        public override object GetValue(object component) 
        {
            if (component is IElement)
            {
                return ((IElement)component).GetAttribute(baseProp.Name);
            }
            else
            {
                return this.baseProp.GetValue(component);
            }
        } 
 
        /// <summary>
        /// The type of the property themselfes.
        /// </summary>
        public override Type PropertyType 
        { 
            get
            {
                return this.baseProp.PropertyType;
            } 
        } 
 
        /// <summary>
        /// Reset the value of the property to the default one.
        /// </summary>
        /// <param name="component">The component which contains the property.</param>
        public override void ResetValue(object component) 
        {
            if (component is IElement)
            {
                ((IElement)component).RemoveAttribute(baseProp.Name);
            }
            else
            {
                baseProp.ResetValue(component);
            }
        } 
 
        /// <summary>
        /// Set the value of the property. 
        /// </summary>
        /// <param name="component">The component which contains the property.</param>
        /// <param name="Value">The value beeing set.</param>
        public override void SetValue(object component, object Value) 
        {
            if (component is IElement)
            {
                ((IElement)component).SetAttribute(baseProp.Name, Value);
            }
            else
            {
                this.baseProp.SetValue(component, Value);
            }
        } 
 
        /// <summary>
        /// Determines a value indicating whether the value of this property needs to be persistend.
        /// </summary>
        /// <param name="component">The component which contains the property.</param>
        /// <returns><c>True</c> if the property should be persistent; otherwise, <c>false</c>.</returns>
        public override bool ShouldSerializeValue(object component)  
        {
            if (component is IElement)
            {
                return true;
            }
            else
            {
                return this.baseProp.ShouldSerializeValue(component);
            }
        }  

	}
}
