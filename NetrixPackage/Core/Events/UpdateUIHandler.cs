using System;
using GuruComponents.Netrix.WebEditing.Elements;
namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// Declare the update event handler.
    /// </summary>
    public delegate void UpdateUIHandler(object sender, UpdateUIEventArgs e);

    /// <summary>
    /// Event that causes the update
    /// </summary>
    public enum UpdateReason
    {
        /// <summary>
        /// Key stroke
        /// </summary>
        Key,
        /// <summary>
        /// Mouse click
        /// </summary>
        Mouse,
        /// <summary>
        /// Internal tools
        /// </summary>
        Tools,
        /// <summary>
        /// Other
        /// </summary>
        Other
    }

    /// <summary>
    /// Event arguments for UpdateUI event.
    /// </summary>
    public class UpdateUIEventArgs : EventArgs
    {
        /// <summary>
        /// Creates the event argument object
        /// </summary>
        /// <param name="element"></param>
        /// <param name="reason"></param>
        public UpdateUIEventArgs(IElement element, UpdateReason reason)
            : base()
        {
            CurrentElement = element;
            Reason = reason;
        }

        private IElement _currentElement;

        /// <summary>
        /// Element currently active.
        /// </summary>
        public IElement CurrentElement
        {
            get  {return _currentElement;}
            set { _currentElement = value; }
        }

        private UpdateReason _reason;

        /// <summary>
        /// Reason that causes the update
        /// </summary>
        public UpdateReason Reason
        {
            get{return _reason;}
            set{_reason =value;}
        }


    }

}
