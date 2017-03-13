using System;
using System.ComponentModel;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// This class defines event arguments.
    /// </summary>
    /// <remarks>
    /// This class allows to cancel the navigation by setting the cancel property to <c>true</c>.
    /// </remarks>
	public class EventDisplayEventArgs : EventArgs
	{
		private EventDescriptorCollection pdc;
        private IElement element;
        private bool resetEventFilterList;

        /// <summary>
        /// If set to true the EventFilterList will refresh on next request.
        /// </summary>
        public bool ResetEventFilterList
        {
            get { return resetEventFilterList; }
            set { resetEventFilterList = value; }
        } 

        /// <summary>
        /// Internally used constructor for event arguments.
        /// </summary>
        /// <remarks>
        /// This constructor supports the NetRix infrastructure it has not being called from user code.
        /// </remarks>
        /// <param name="element">The element that delivers the properties.</param>
        /// <param name="pdc">Property descriptor collection.</param>
        public EventDisplayEventArgs(IElement element, EventDescriptorCollection pdc)
		{
			this.pdc = pdc;
			this.element = element;
            this.resetEventFilterList = false;
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
		/// Gets or sets he current value.
		/// </summary>
        public EventDescriptorCollection DescriptorCollection
		{
			get
			{
				return pdc;
			}
            set
            {
                pdc = value;
            }
		}

    }
}