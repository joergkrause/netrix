using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.Designer
{
    public class DesignSite : IDesignSite
    {
        private IServiceProvider serviceProvider;
        private IComponent component;
        private string name;
        private System.Web.UI.Control element;

        public DesignSite(IServiceProvider serviceProvider, string name)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            this.serviceProvider = serviceProvider;
            this.name = name;
        }

        public virtual IComponent Component
        {
            get
            {
                return component;
            }
        }

        public virtual System.Web.UI.Control Element
        {
            get
            {
                return element;
            }
        }

        public virtual IContainer Container
        {
            get
            {
                if (serviceProvider != null)
                {
                    return ((IDesignerHost)serviceProvider.GetService(typeof(IDesignerHost))).Container;
                } 
                else 
                {
                    return null;
                }
            }
        }

        public virtual bool DesignMode
        {
            get
            {
                return true;
            }
        }

        public virtual string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ArgumentNullException("Null or Empty String is not allowed as Name.");
                }
                if (value.Equals(name))
                {
                    return;
                }
                ((DesignerHost)serviceProvider.GetService(typeof(IDesignerHost))).RenameComponent(this, value);
            }
        }

        public void SetComponent(IComponent component)
        {
            this.component = component;
            if (component is System.Web.UI.Control)
            {
                //name = ((System.Web.UI.Control)component).ID;
                element = ((System.Web.UI.Control)component);
            }
        }

        public void SetElement(System.Web.UI.Control element)
        {
            //this.element = element;
            //if (element != null)
            //{ 
            //    this.component = element;
            //}
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceProvider != null)
            {
                return serviceProvider.GetService(serviceType);
            } 
            else 
            {
                return null;
            }
        }
    }

}
