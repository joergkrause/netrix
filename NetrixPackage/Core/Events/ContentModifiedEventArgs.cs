using System;
using System.Windows.Forms;
using System.Drawing;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// This class is used to set the event arguments for the <see cref="ContentModifiedHandler"/>
    /// event.
    /// </summary>
    /// <remarks>
    /// It returns the element that was modified last.
    /// </remarks>
	public class ContentModifiedEventArgs : EventArgs
	{

        /// <summary>
        /// Stores the last modified element internally.
        /// </summary>
		private GuruComponents.Netrix.WebEditing.Elements.IElement lastModifiedElement = null;		

        /// <summary>
        /// The constructor used to build the event argument internally. This constructor supports
        /// the NetRix component architecture internally is not intendet to beeing called from 
        /// host applications.
        /// </summary>
        /// <param name="e"></param>
		public ContentModifiedEventArgs(IElement e)
		{
			lastModifiedElement = e;
		}

        /// <summary>
        /// Gets the element that was last modified. May be null if the element was not recognized.
        /// It is recommended to check for null to avoid a NullReferenceExpection. This property is
        /// readonly.
        /// </summary>
        public IElement LastModifiedElement
        {
            get
            {
                return this.lastModifiedElement;
            }
        }

	}
}