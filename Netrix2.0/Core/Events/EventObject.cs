using System;
using GuruComponents.Netrix.ComInterop;
#pragma warning disable 1591
namespace GuruComponents.Netrix.Events
{
	/// <summary>
	/// This class provides access to the legacy IHTMLEventObj class.
	/// </summary>
	public class EventObject : Interop.IHTMLEventObj
    {

        private Interop.IHTMLEventObj e;

        /// <summary>
        /// Internally used public ctor.
        /// </summary>
        /// <param name="e"></param>
        public EventObject(Interop.IHTMLEventObj e)
        {
            this.e = e;
        }

        #region IHTMLEventObj Member

        public String qualifier
        {
            get
            {
                return e.qualifier;
            }
        }

        public Boolean ctrlKey
        {
            get
            {
                return e.ctrlKey;
            }
        }

        public Object srcFilter
        {
            get
            {
                return e.srcFilter;
            }
        }

        public Int32 screenX
        {
            get
            {
                return e.screenX;
            }
        }

        /// <summary>
        /// State of ALT key in case of key operations.
        /// </summary>
        public Boolean altKey
        {
            get
            {
                return e.altKey;
            }
        }

        /// <summary>
        /// X position if there is an event that has position information.
        /// </summary>
        public Int32 x
        {
            get
            {
                return e.x;
            }
        }

        public Int32 offsetY
        {
            get
            {
                return e.offsetY;
            }
        }

        public Int32 reason
        {
            get
            {
                return e.reason;
            }
        }

        public Int32 clientX
        {
            get
            {
                return e.clientX;
            }
        }

        public Boolean cancelBubble
        {
            get
            {
                return e.cancelBubble;
            }
            set
            {
                e.cancelBubble = value;
            }
        }

        public Int32 clientY
        {
            get
            {
                return e.clientY;
            }
        }

        public Int32 offsetX
        {
            get
            {
                return e.offsetX;
            }
        }

        /// <summary>
        /// Y position in screen coordinates if there is an event that has position information.
        /// </summary>
        public Int32 screenY
        {
            get
            {
                return e.screenY;
            }
        }

        public Object returnValue
        {
            get
            {
                return e.returnValue;
            }
            set
            {
                e.returnValue = value;
            }
        }

        public Interop.IHTMLElement toElement
        {
            get
            {
                return e.toElement;
            }
        }

        public String type
        {
            get
            {
                return e.type;
            }
        }

        public Interop.IHTMLElement fromElement
        {
            get
            {
                return e.fromElement;
            }
        }

        /// <summary>
        /// Y position if there is an event that has position information.
        /// </summary>
        public Int32 y
        {
            get
            {
                return e.y;
            }
        }

        /// <summary>
        /// State of shft key.
        /// </summary>
        public Boolean shiftKey
        {
            get
            {
                return e.shiftKey;
            }
        }

        /// <summary>
        /// Mouse button code in case the event was caused by mouse operation.
        /// </summary>
        public Int32 button
        {
            get
            {
                return e.button;
            }
        }

        /// <summary>
        /// Keycode in case the event is based on key operations.
        /// </summary>
        public Int32 keyCode
        {
            get
            {
                return e.keyCode;
            }
            set
            {
                e.keyCode = value;
            }
        }

        /// <summary>
        /// Source element which is responsible for the event fired.
        /// </summary>
        public Interop.IHTMLElement srcElement
        {
            get
            {
                return e.srcElement;
            }
        }

        #endregion
    }
}
