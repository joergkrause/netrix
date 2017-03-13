using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// Supports cancellable tool item click events.
    /// </summary>
    /// <remarks>
    /// This class allows to cancel the internal processing of tool operations.
    /// </remarks>
    public class ToolClickedCancelEventArgs : CancelEventArgs
    {

        private ToolStripItem item;
        private string toolName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clickedItem"></param>
        public ToolClickedCancelEventArgs(ToolStripItem clickedItem)
        {
            item = clickedItem;
            toolName = (item.Tag == null) ? String.Empty : item.Tag.ToString();
        }

        /// <summary>
        /// Gets the item that was clicked on the System.Windows.Forms.ToolStrip.
        /// </summary>
        public ToolStripItem ClickedItem
        {
            get { return item; }
        }

        /// <summary>
        /// Returns the internal name of the tool, e.g. "Open" for the Open tool.
        /// </summary>
        /// <remarks>
        /// Tools which do not have any clickable action, like parents or separators, may return an empty string.
        /// </remarks>
        public string ToolName
        {
            get { return toolName; }   
        }
    }
}