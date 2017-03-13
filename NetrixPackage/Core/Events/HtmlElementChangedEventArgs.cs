using System;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Web.UI;
using System.ComponentModel;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// The type of event which fired the <see cref="HtmlElementChangedEventArgs"/>.
    /// </summary>
    public enum HtmlElementChangedType
    {
        /// <summary>
        /// A mouse event has changed the element.
        /// </summary>
        Mouse   = 1,
        /// <summary>
        /// A keystroke has changed the element.
        /// </summary>
        Key     = 2,
        /// <summary>
        /// The internal event was not recognized (reserved for future use).
        /// </summary>
        Unknown = 0
    }

    /// <summary>
    /// The EventArgs class which covers the element and event type for the changed element event.
    /// </summary>
    public class HtmlElementChangedEventArgs : EventArgs
    {
        private IComponent _element;
        private HtmlElementChangedType _type;

        /// <summary>
        /// Retrieves the element which fired the event.
        /// </summary>
        /// <remarks>
        /// This property returns the element only if inherited from <see cref="IElement"/>.
        /// </remarks>
        public IElement CurrentElement
        {
            get
            {
                return _element as IElement;
            }
        }


        /// <summary>
        /// Retrieves the element which fired the event.
        /// </summary>
        /// <remarks>
        /// This property returns the element only if inherited from <see cref="IElement"/>.
        /// </remarks>
        public Control CurrentControl
        {
            get
            {
                return _element as Control;
            }
        }

        /// <summary>
        /// Checks how the event was fired internally, e.g. ether by mouse or by keystroke.
        /// </summary>
        public HtmlElementChangedType EventType
        {
            get
            {
                return _type;
            }
        }

        /// <summary>
        /// Internally used ctor.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        public HtmlElementChangedEventArgs(IComponent element, HtmlElementChangedType type)
        {   
            _element = element;
            _type = type;
        }
    }

}
