using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.Designer
{
	/// <summary>
	/// Zusammenfassung für EventPropertyDescriptor.
	/// </summary>
	[TypeConverter(typeof(StringConverter))]
    public class EventPropertyDescriptor : PropertyDescriptor
	{

        private EventDescriptor baseDesc;
        private IEventBindingService service;

		public EventPropertyDescriptor(EventDescriptor eventDesc, IEventBindingService service) : base(eventDesc)
		{
            baseDesc = eventDesc;
            this.service = service;
		}

        public IEventBindingService Service { get { return service; } }

        public EventDescriptor Event 
        {
            get
            {
                return baseDesc;
            }
        }

        public override Type ComponentType
        {
            get
            {
                return baseDesc.ComponentType;
            }
        }

        public override TypeConverter Converter
        {
            get
            {
                return ((EventBindingService) service).EventTypeConverter;
            }
        }

        public override bool IsReadOnly
        {
            get
            {                
                return false;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return typeof(string);
            }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            if (GetValueRequest != null)
            {
                GetValueRequest(component, EventArgs.Empty);
            } 
            return null;
        }

        public override void ResetValue(object component)
        {
            if (ResetValueRequest != null)
            {
                ResetValueRequest(component, EventArgs.Empty);
            }
            SetValue(component, null);
        }

        public override void SetValue(object component, object value)
        {
            if (SetValueRequest != null)
            {
                SetValueRequest(component, EventArgs.Empty);
            }            
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }


        /// <summary>
        /// Requests a value set.
        /// </summary>
        public event EventHandler SetValueRequest;
        /// <summary>
        /// Requests a value get.
        /// </summary>
        public event EventHandler GetValueRequest;
        /// <summary>
        /// Requests a value reset.
        /// </summary>
        public event EventHandler ResetValueRequest;



	}
}
