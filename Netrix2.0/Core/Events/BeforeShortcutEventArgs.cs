using System;
using System.Windows.Forms;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This class defines data for the <see cref="GuruComponents.Netrix.IHtmlEditor.BeforeShortcut">BeforeShortcut</see>.
    /// </summary>
    /// <remarks>
    /// This class can be used to catch and cancel a shortcut. A shortcut is in NetRix any key pressed in 
    /// design or browse mode together with the control (Ctrl) key.
    /// <para>
    /// NetRix provides a few hardwired shortcuts. If you don't wish the internal processing you can disable the
    /// hardwired keys globally by using the <see cref="GuruComponents.Netrix.IHtmlEditor.InternalShortcutKeys">InternalShortcutKeys</see> property.
    /// To cancel onyl specific shortcut keys add a handler for the 
    /// <see cref="GuruComponents.Netrix.IHtmlEditor.BeforeShortcut">BeforeShortcut</see> event and set
    /// the property <see cref="GuruComponents.Netrix.Events.BeforeShortcutEventArgs.Cancel">Cancel</see> property to <c>true</c>.
    /// Remember, that the handler is called for any control-key combination, even if they has no hardwired function assigned.
    /// </para>
    /// </remarks>
# if WPF
    public class BeforeShortcutEventArgs : EventArgs
    {
        bool cancel = false;
        Keys key;

        /// <summary>
        /// Constructor for event arguments.
        /// </summary>
        /// <remarks>
        /// Used internally to support NetRix infrastructure. There is no need to call this constructor directly.
        /// </remarks>
        /// <param name="key"></param>
        public BeforeShortcutEventArgs(Keys key)
        {
            this.key = key;
        }

        /// <summary>
        /// The key pressed as a shortcut key.
        /// </summary>
        /// <remarks>
        /// This is the base key, which the user has pressed together with the control key. Readonly.
        /// </remarks>
        public Keys Key
        {
            get
            {
                return this.key;
            }
        }
		
        /// <summary>
        /// Gets or sets the current cancel state.
        /// </summary>
        /// <remarks>
        /// To cancel the internal processing of the currently pressed key just set the value to <c>true</c> in the
        /// event handler.
        /// </remarks>
        public bool Cancel
        {
            get
            {
                return cancel;
            }
            set
            {
                cancel = value;
            }
        }
    }
# else
    public class BeforeShortcutEventArgs : EventArgs
    {
        bool cancel = false;
        Keys key;

        /// <summary>
        /// Constructor for event arguments.
        /// </summary>
        /// <remarks>
        /// Used internally to support NetRix infrastructure. There is no need to call this constructor directly.
        /// </remarks>
        /// <param name="key"></param>
        public BeforeShortcutEventArgs(Keys key)
        {
            this.key = key;
        }

        /// <summary>
        /// The key pressed as a shortcut key.
        /// </summary>
        /// <remarks>
        /// This is the base key, which the user has pressed together with the control key. Readonly.
        /// </remarks>
        public Keys Key
        {
            get
            {
                return this.key;
            }
        }
		
        /// <summary>
        /// Gets or sets the current cancel state.
        /// </summary>
        /// <remarks>
        /// To cancel the internal processing of the currently pressed key just set the value to <c>true</c> in the
        /// event handler.
        /// </remarks>
        public bool Cancel
        {
            get
            {
                return cancel;
            }
            set
            {
                cancel = value;
            }
        }
    }
# endif
}