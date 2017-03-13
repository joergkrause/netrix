using System;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms.Design;
using GuruComponents.Netrix.UserInterface.TypeEditors;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{

	/// <summary>
	/// Modifies the descriptor content populated to propertygrid.
	/// </summary>
	/// <remarks>
	/// This overwrites the
	/// default property names and enhances the standard attributes with the DisplayNameAttribute.
	/// </remarks>
	public class CustomPropertyDescriptor : PropertyDescriptor
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
        public CustomPropertyDescriptor(PropertyDescriptor pd, Attribute[] filter) : base(pd)
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
        /// Retrieve the resource string from localization assembly.
        /// </summary>
        /// <remarks>
        /// The resource strings must follow
        /// the pattern "Type.Attribute", e.g. "Category.Class".
        /// </remarks>
        /// <param name="Type">Type, such as "Attribute", "Category"</param>
        /// <param name="Element">Attribute name for the given type</param>
        /// <returns>String with the localized name</returns>
        private string GetResource(string Type, string Element)
        {
            string getter = String.Concat(Type, ".", Element);
            string r = GuruComponents.Netrix.UserInterface.ResourceManager.GetString(getter);
            if (r == null)
            {
                return Element;
            }                   
            else 
            {
                return r;
            }
        }


        /// <summary>
        /// Indicates the category that the property will appear in.
        /// </summary>
        /// <remarks>
        /// The value will be the value given for the CategoryAttribute
        /// attribute for the property, if one exists.  If there is no CategoryAttribute, then this will be DefaultCategoryName.
        /// </remarks>
        public override string Category
        {
            get
            {
                AttributeCollection atts = baseProp.Attributes;
                if (atts.Count > 0)
                {
                    if (((CategoryAttribute)atts[typeof(CategoryAttribute)]).Category == String.Empty)
                    {                                           
                        return GetResource("Category", baseProp.Name);
                    } 
                    else 
                    {
                        return GetResource("Category", ((CategoryAttribute)atts[typeof(CategoryAttribute)]).Category);
                    }
                }
                else 
                {
                    return "General";
                }
            }
        }


        /// <summary>
        /// Indicates the name for the property that will be displayed in the PropertyGrid control.
        /// </summary>
        /// <remarks>
        /// The value will be the value given for the
        /// isplayNameAttribute attribute for the property. If there is no DisplayNameAttribute, then this will be the name of
        /// the property.
        /// </remarks>
        public override string DisplayName
        {
            get
            {
                AttributeCollection atts = baseProp.Attributes;
                if (atts.Count > 0 && ResourceManager.GetGridLanguage() == ResourceManager.GridLanguageType.Localized)
                {
                    if (((DisplayNameAttribute)atts[typeof(DisplayNameAttribute)]) == null || ((DisplayNameAttribute)atts[typeof(DisplayNameAttribute)]).DisplayName == String.Empty)
                    {
                        return GetResource("Attribute", baseProp.Name);
                    } 
                    else 
                    {
                        return GetResource("Attribute", ((DisplayNameAttribute)atts[typeof(DisplayNameAttribute)]).DisplayName);
                    }
                } 
                else 
                {
                    return baseProp.Name;
                }
            }
        }

        /// <summary>
        /// Indicates the description for the property.</summary><remarks>The value will be the value given for the <see cref="DescriptionAttribute"/>
        /// attribute for the property, if one exists. If there is no DescriptionAttribute, then this will be null.
        /// </remarks>
        public override string Description
        {
            get
            {
                AttributeCollection atts = baseProp.Attributes;
                if (atts.Count > 0)
                {
                    if (((DescriptionAttribute)atts[typeof(DescriptionAttribute)]) == null || ((DescriptionAttribute)atts[typeof(DescriptionAttribute)]).Description == String.Empty)
                    {
                        return GetResource("Description", baseProp.Name);
                    } 
                    else 
                    {
                        return GetResource("Description",((DescriptionAttribute)atts[typeof(DescriptionAttribute)]).Description);
                    }
                }
                else 
                {
                    return baseProp.Name;
                }
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
            return this.baseProp.GetValue(component); 
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
            baseProp.ResetValue(component); 
        } 
 
        /// <summary>
        /// Set the value of the property. 
        /// </summary>
        /// <param name="component">The component which contains the property.</param>
        /// <param name="Value">The value beeing set.</param>
        public override void SetValue(object component, object Value) 
        { 
            this.baseProp.SetValue(component, Value); 
        } 
 
        /// <summary>
        /// Determines a value indicating whether the value of this property needs to be persistend.
        /// </summary>
        /// <param name="component">The component which contains the property.</param>
        /// <returns><c>True</c> if the property should be persistent; otherwise, <c>false</c>.</returns>
        public override bool ShouldSerializeValue(object component)  
        { 
            return this.baseProp.ShouldSerializeValue(component); 
        }  

	}
}
