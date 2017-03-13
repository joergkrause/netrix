using System;
using System.ComponentModel;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// This class defines event arguments or
    /// </summary>
    /// <remarks>
    /// This class allows to cancel the navigation by setting the cancel property to <c>true</c>.
    /// </remarks>
	public class PropertyDisplayEventArgs : EventArgs
	{
		private PropertyDescriptorCollection pdc;
        private IElement element;
        private bool resetPropertyFilterList;

        /// <summary>
        /// If set to true the PropertyFilterList will refresh on next request.
        /// </summary>
        public bool ResetPropertyFilterList
        {
            get { return resetPropertyFilterList; }
            set { resetPropertyFilterList = value; }
        } 

        /// <summary>
        /// Internally used constructor for event arguments.
        /// </summary>
        /// <remarks>
        /// This constructor supports the NetRix infrastructure it has not being called from user code.
        /// </remarks>
        /// <param name="element">The element that delivers the properties.</param>
        /// <param name="pdc">Property descriptor collection.</param>
		public PropertyDisplayEventArgs(IElement element, PropertyDescriptorCollection pdc)
		{
			this.pdc = pdc;
			this.element = element;
            this.resetPropertyFilterList = false;
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
		public PropertyDescriptorCollection DescriptorCollection
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