using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;

# pragma warning disable 0672

namespace GuruComponents.Netrix.AspDotNetDesigner
{
    /// <summary>
    /// Base designer for elements.
    /// </summary>
    public class ElementDesigner : HtmlControlDesigner
    {
        private IHtmlControlDesignerBehavior behavior;
        private bool shouldCodeSerialize;

        /// <summary>
        /// behavior
        /// </summary>
        public new IHtmlControlDesignerBehavior Behavior
        {
            get
            {
                return this.behavior;
            }
            set
            {
                if (this.behavior != value)
                {
                    if (this.behavior != null)
                    {
                        this.OnBehaviorDetaching();
                        this.behavior.Designer = null;
                        this.behavior = null;
                    }
                    if (value != null)
                    {
                        this.behavior = value;
                        this.OnBehaviorAttached();
                    }
                }
            }
        }

        /// <summary>
        /// Expose private werbs to netrix webcontrol.
        /// </summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                return this.behavior.Designer.Verbs;
            }
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="properties"></param>
        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            PropertyDescriptor descriptor1 = (PropertyDescriptor)properties["Name"];
            if (descriptor1 != null)
            {
                properties["Name"] = TypeDescriptor.CreateProperty(descriptor1.ComponentType, descriptor1, new Attribute[] { BrowsableAttribute.No });
            }
            descriptor1 = (PropertyDescriptor)properties["Modifiers"];
            if (descriptor1 != null)
            {
                properties["Modifiers"] = TypeDescriptor.CreateProperty(descriptor1.ComponentType, descriptor1, new Attribute[] { BrowsableAttribute.No });
            }
            properties["DataBindings"] = TypeDescriptor.CreateProperty(base.GetType(), "DataBindings", typeof(DataBindingCollection), new Attribute[] { DesignerSerializationVisibilityAttribute.Hidden, CategoryAttribute.Data, new EditorAttribute(typeof(DataBindingCollectionEditor), typeof(UITypeEditor)), new TypeConverterAttribute(typeof(DataBindingCollectionConverter)), new ParenthesizePropertyNameAttribute(true), MergablePropertyAttribute.No, new DescriptionAttribute("Control_DataBindings") });
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="events"></param>
        protected override void PreFilterEvents(IDictionary events)
        {
            base.PreFilterEvents(events);
            if (!this.ShouldCodeSerialize)
            {
                ICollection collection1 = events.Values;
                if ((collection1 != null) && (collection1.Count != 0))
                {
                    object[] objArray1 = new object[collection1.Count];
                    collection1.CopyTo(objArray1, 0);
                    for (int num1 = 0; num1 < objArray1.Length; num1++)
                    {
                        EventDescriptor descriptor1 = (EventDescriptor)objArray1[num1];
                        descriptor1 = TypeDescriptor.CreateEvent(descriptor1.ComponentType, descriptor1, new Attribute[] { BrowsableAttribute.No });
                        events[descriptor1.Name] = descriptor1;
                    }
                }
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.Behavior != null))
            {
                this.Behavior.Designer = null;
                this.Behavior = null;
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether to create a field declaration for
        ///     the control in the code-behind file for the current design document during
        ///     serialization.
        /// </summary>
        public override bool ShouldCodeSerialize
        {
            get
            {
                return this.shouldCodeSerialize;
            }
            set
            {
                this.shouldCodeSerialize = value;
            }
        }

        /// <summary>
        /// Gets the data bindings collection for the current control.
        /// </summary>
        /// <remarks>A System.Web.UI.DataBindingCollection that contains the data bindings for
        ///    the current control.</remarks>
        public new DataBindingCollection DataBindings
        {
            get
            {
                return ((IDataBindingsAccessor)base.Component).DataBindings;
            }
        }




    }
}