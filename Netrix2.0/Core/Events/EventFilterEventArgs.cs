using System.ComponentModel;
using GuruComponents.Netrix.WebEditing.Elements;
using System;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// This class handles event arguments for PropertyFilter event.
    /// </summary>
    /// <remarks>
    /// This class allows to cancel the properties shown in the PropertyGrid.
    /// </remarks>
	public class EventFilterEventArgs : CancelEventArgs
	{
		EventDescriptor pd;
		IElement element;

        /// <summary>
        /// Internally used constructor for event arguments.
        /// </summary>
        /// <remarks>
        /// This constructor supports the NetRix infrastructure it has not being called from user code.
        /// </remarks>
        /// <param name="element"></param>
        /// <param name="pd"></param>
        public EventFilterEventArgs(IElement element, EventDescriptor pd)
		{
			this.pd = pd;
			this.element = element;
		}

        /// <summary>
        /// Returns the element the PropertyGrid currently shows.
        /// </summary>
		public IElement Element
		{
			get
			{
				return this.element;
			}
		}

		/// <summary>
		/// Gets or sets the Descriptor of the property.
		/// </summary>
		/// <remarks>
		/// The host application can create its own PropertyDescriptor to change the behavior
		/// of the property in PropertyGrid.
		/// </remarks>
        public EventDescriptor Event
		{
			get
			{
				return this.pd;
			}
			set
			{
				this.pd = value;
			}
		}

		/// <summary>
		/// Gets the Name of Event.
		/// </summary>
		public string PropertyName
		{
			get
			{
				return this.pd.Name;
			}
		}

		/// <summary>
		/// Gets the Category the property appears in.
		/// </summary>
		public string Category
		{
			get
			{
				return this.pd.Category;
			}
		}

		/// <summary>
		/// Gets the Localized (full) name.
		/// </summary>
		public string DisplayName
		{
			get
			{
				return this.pd.DisplayName;
			}
		}

		/// <summary>
		/// Gets the Description that appears in lower help section of propertygrid.
		/// </summary>
		public string Description
		{
			get
			{
				return this.pd.Description;
			}
		}


    }
}