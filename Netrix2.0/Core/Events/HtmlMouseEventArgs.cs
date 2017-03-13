using System;
using System.Windows.Forms;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This enumeration defined various mouse event types.
    /// </summary>
    public enum MouseEventType
    {
        /// <summary>
        /// The type was not recognized.
        /// </summary>
        Unknown     = 0,
        /// <summary>
        /// The mouse was moved.
        /// </summary>
        MouseMove   = 1,
        /// <summary>
        /// The mouse button was released.
        /// </summary>
        MouseUp     = 2,
        /// <summary>
        /// The mouse button was pressed.
        /// </summary>
        MouseDown   = 3
    }

    /// <summary>
    /// Used to get specific informatin about the mouse event.
    /// </summary>
    public class HtmlMouseEventArgs : EventArgs
    {
        private MouseEventType eventType = MouseEventType.Unknown;
        private System.Web.UI.Control element;
        private int x, y;
        private int docx, docy;
        private MouseButtons buttons;
        private bool outside = false;
        private bool handled = false;

        /// <summary>
        /// Retrieves the element which is currently under the mouse pointer.
        /// </summary>
        public System.Web.UI.Control ElementUnderPointer
        {
            get
            {
                return element;
            }
        }

        /// <summary>
        /// Get the MouseEventType, which can be "move", "up" or "down".
        /// </summary>
        public MouseEventType EventType
        {
            get
            {
                return eventType;
            }
        }

        /// <summary>
        /// The X coordinate. Can be negative if move operation outside the editor area.
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
        }

        /// <summary>
        /// The Y coordinate. Can be negative if move operation outside the editor area
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
        }

        /// <summary>
        /// The X coordinate within the container. 
        /// </summary>
        /// <remarks>
        /// Can be negative if move operation outside the editor area.
        /// </remarks>
        public int ContainerX
        {
            get
            {
                return docx;
            }
        }

        /// <summary>
        /// The Y coordinate within the container.
        /// </summary>
        /// <remarks>
        /// Can be negative if move operation outside the editor area.
        /// </remarks>
        public int ContainerY
        {
            get
            {
                return docy;
            }
        }

        /// <summary>
        /// True if the mousepointer is outside the editor area. 
        /// </summary>
        /// <remarks>
        /// This is when ether x or y or both are negative.
        /// During normal operations the property should always return false.
        /// </remarks>
        public bool Outside
        {
            get
            {
                return outside;
            }
        }

        /// <summary>
        /// Cancels event bubble.
        /// </summary>
        /// <remarks>
        /// If set to <c>true</c> subsequent objects in the event chain will not receive this event.
        /// <para>
        /// Warning: Unproperly usage can result in internal failure of global operations.
        /// </para>
        /// </remarks>
        public bool Handled
        {
            get
            {
                return handled;
            }
            set
            {
                handled = value;
            }
        }

        /// <summary>
        /// Handles Mouse events and fill the event arguments with the values form the last event. 
        /// </summary>
        /// <remarks>
        /// This constructor set the current element in the selection to, which forces the SelectionChanged event if the mouse click
        /// operation has changed the element. This constructor needs access to the Interop namespace is for internal
        /// use only.
        /// </remarks>
        /// <param name="element"></param>
        /// <param name="eventObject"></param>
        /// <param name="htmlEditor"></param>
        public HtmlMouseEventArgs(System.Web.UI.Control element, Interop.IHTMLEventObj eventObject, IHtmlEditor htmlEditor)
        {
            try
            {
                this.element = element;
                x = eventObject.x;
                y = eventObject.y;
                docx = eventObject.offsetX;
                docy = eventObject.offsetY;
                switch (eventObject.type)
                {
                    case "mousemove":
                        eventType = MouseEventType.MouseMove;
                        break;
                    case "mouseup":
                        // using "up" in absence of a "click" event here
                        eventType = MouseEventType.MouseUp;
                        if (element != null)
                        {
                            htmlEditor.Document.SetActiveElement(element as IElement);
                        }
                        break;
                    case "mousedown":
                        eventType = MouseEventType.MouseDown;
                        break;
                    default:
                        return;
                }
                buttons = (MouseButtons)eventObject.button;
                outside = (x < 0 || y < 0) ? true : false;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace, ex.Message);
            }
        }

    }


}
