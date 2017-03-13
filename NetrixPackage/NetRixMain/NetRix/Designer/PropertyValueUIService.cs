using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Provides an interface to manage the images, ToolTips, and event handlers for the properties of a component displayed in a property browser.
    /// </summary>
    public class PropertyValueUIService : IPropertyValueUIService
    {

        private IHtmlEditor editor;

        /// <summary>
        /// Default ctor creating an instance of the service.
        /// </summary>
        /// <param name="editor"></param>
        public PropertyValueUIService(IHtmlEditor editor)
        {
            this.editor = editor;
        }

        #region IPropertyValueUIService Members

        /// <summary>
        /// The method or operation is not implemented.
        /// </summary>
        /// <param name="newHandler"></param>
        public void AddPropertyValueUIHandler(PropertyValueUIHandler newHandler)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Provides information about a property displayed in the Properties window, including the associated event handler, pop-up information string, and the icon to display for the property. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="propDesc"></param>
        /// <returns></returns>
        public PropertyValueUIItem[] GetPropertyUIValueItems(ITypeDescriptorContext context, PropertyDescriptor propDesc)
        {
            //return new PropertyValueUIItem[] {
            //    new PropertyValueUIItem(image, handler, tooltip)
            //};
            return null;
        }

        /// <summary>
        /// The method or operation is not implemented.
        /// </summary>
        public void NotifyPropertyValueUIItemsChanged()
        {
            if (PropertyUIValueItemsChanged != null)
            {
                PropertyUIValueItemsChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fired when a value item is being changed. Currently not used.
        /// </summary>
        public event EventHandler PropertyUIValueItemsChanged;

        /// <summary>
        /// The method or operation is not implemented.
        /// </summary>
        /// <param name="newHandler"></param>
        public void RemovePropertyValueUIHandler(PropertyValueUIHandler newHandler)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
