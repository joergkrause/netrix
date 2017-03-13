using System;
using System.Drawing;
using System.Web.UI;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// Specifies the identifier of the shortcut menu to be displayed. 
    /// </summary>
    /// <remarks>
    /// Some of the options do not work in design mode but in browse mode only.
    /// </remarks>
    public enum MenuIdentifier
    {
        /// <summary>
        /// Anything else. Available both in design and browse mode.
        /// </summary>
        Default = 0,
        /// <summary>
        /// On an image. Available both in design and browse mode.
        /// </summary>
        Image = 1,
        /// <summary>
        /// On a field control. Available both in design and browse mode.
        /// </summary>
        Control = 2,
        /// <summary>
        /// On a table. Available in browse mode only.
        /// </summary>
        Table = 3,
        /// <summary>
        /// On selected text. Available in browse mode only.
        /// </summary>
        TextSelect = 4,
        /// <summary>
        /// On an anchor. Available in browse mode only.
        /// </summary>
        Anchor = 5,
        /// <summary>
        /// Unknow. Available in browse mode only.
        /// </summary>        
        Unknown = 6,
        /// <summary>
        /// On image with DYNSRC property set. Available in browse mode only.
        /// </summary>        
        ImageDynSrc = 7,
        /// <summary>
        /// On image. Available in browse mode only.
        /// </summary>
        ImageArt = 8,
        /// <summary>
        /// On script in debug mode. Available in browse mode only.
        /// </summary>
        ScriptDebug = 9,
        /// <summary>
        /// On vertical scroll bar. Available in browse mode only.
        /// </summary>
        VerticalScroll = 10,
        /// <summary>
        /// On horizontal scroll bar. Available in browse mode only.
        /// </summary>        
        HorizontalScroll = 11 
    }

    /// <summary>
    /// This class defines the event arguments used in the ContextMenu event.
    /// </summary>
    public class ShowContextMenuEventArgs : EventArgs
    {
        private Point _location;
        private bool _keyboard;
        private Control _element;
        private MenuIdentifier _identifier;

        /// <summary>
        /// The element which is located under the pointer position.
        /// </summary>
        /// <remarks>
        /// This property can used to change the content of the context menu accordingly to the element
        /// the user has the right mouse button clicked on.
        /// </remarks>
        public Control ElementAtPoint
        {
            get
            {
                return _element;
            }
        }

        /// <summary>
        /// Set the element during the event firing process, used by table context menu.
        /// </summary>
        /// <param name="e"></param>
        public void SetElementAtPoint(Control e)
        {
            this._element = e;
        }

        /// <summary>
        /// The <see cref="Point"/> where the mouse click happens that causes this event.
        /// </summary>
        /// <remarks>
        /// This point should be used to place the context menu at the right screen position.
        /// </remarks>
        public Point Location
        {
            get
            {
                return _location;
            }
        }

        /// <summary>
        /// Gets true, if a key pressed event causes this event.
        /// </summary>
        /// <remarks>
        /// This value depends on hardware
        /// support and seems to be not reliable under some circumstances.
        /// </remarks>
        public bool UsingKeyboard
        {
            get
            {
                return _keyboard;
            }
        }

        /// <summary>
        /// Returns the type of context menu which sould appear.
        /// </summary>
        /// <remarks>
        /// This information represents the element or text type recognized under the
        /// mouse pointer
        /// </remarks>
        public MenuIdentifier Identifier
        {
            get
            {
                return _identifier;
            }
        }

        /// <summary>
        /// Ctor for event args. User internally.
        /// </summary>
        /// <param name="location">Mouse poiner location.</param>
        /// <param name="keyboard">Keyboard state.</param>
        /// <param name="type">Type of context menu id.</param>
        /// <param name="element">Control under mouse pointer.</param>
        public ShowContextMenuEventArgs(Point location, bool keyboard, int type, Control element)
        {
            _location = location;
            _keyboard = keyboard;
            if (element != null)
            {
                _element = element;
            }
            _identifier = (MenuIdentifier)type;
        }
    }

}
