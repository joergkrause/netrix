using System;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.PlugIns
{
    /// <summary>
    /// Allows plugins to extend the available verbs within a design time environment.
    /// </summary>
    /// <remarks>
    /// Within the menu command service the extender commands are transformed into verbs and
    /// used by invoking the commands the plugin exposes. This method allows plugins to extend
    /// the available commands for specific elements in particular situations. The commands
    /// can be used to show command links within the PropertyGrid, creating dynamic context menus
    /// or extending existing main menus.
    /// </remarks>
    public class CommandExtender
    {

        private bool check;
        private bool enabled;
        private bool supported;
        private bool visible;
        private string text;
        private string description;
        private CommandID command;

        ///<summary>
        ///     Initializes a new instance of the System.ComponentModel.Design.MenuCommand
        ///     class.
        ///</summary>
        ///<param name="command">The unique command ID that links this menu command to the environment's menu.</param>
        public CommandExtender(CommandID command)
        {
            this.check = false;
            this.enabled = true;
            this.supported = true;
            this.visible = true;
            this.command = command;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this menu item is checked.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>true if the item is checked; otherwise, false.</returns>
        public bool Checked { get { return check; } set { check = value; } }

        /// <summary>
        /// Gets the System.ComponentModel.Design.CommandID associated with this menu command.
        /// </summary>
        /// <returns>The System.ComponentModel.Design.CommandID associated with the menu command.</returns>
        public CommandID CommandID { get { return command; } }
        
        /// <summary>
        /// Gets a value indicating whether this menu item is available.
        /// </summary>
        /// <value>true if the item is enabled; otherwise, false.</value>
        public bool Enabled { get { return enabled; } set { enabled = value; } }

        /// <summary>
        /// Gets or sets a value indicating whether this menu item is supported.
        /// </summary>
        /// <value>true if the item is supported, which is the default; otherwise, false.</value>
        public bool Supported { get { return supported; } set { supported = value; } }
        /// <summary>
        /// Gets or sets a value indicating whether this menu item is visible.
        /// </summary>
        /// <value>true if the item is visible; otherwise, false.</value>
        public bool Visible { get { return visible; } set { visible = value; } }
        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        /// <summary>
        /// Returns a string representation of this menu command.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}|Enabled={1}|Checked={2}", Text, Enabled, Checked);
        }
    }
}