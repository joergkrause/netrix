using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.Netrix.UserInterface.ToolBox
{

    /// <summary>
    /// Event arguments for Toolbox.
    /// </summary>
    /// <remarks>
    /// Provides access to selected item.
    /// </remarks>
    public class ToolBoxEventArgs
    {
        /// <summary>
        /// Caption of the Tab containing the selected Item.
        /// </summary>
        public string TabCaption = "";
        /// <summary>
        /// Index of the Tab.
        /// </summary>
        public int TabIndex = -1;       
        /// <summary>
        /// Caption of the item.
        /// </summary>
        public string ItemCaption = "";  
        /// <summary>
        /// ID of the item.
        /// </summary>
        public string ItemId = "";        
        /// <summary>
        /// Index of the item.
        /// </summary>
        public int ItemIndex = -1;        
        /// <summary>
        /// Tag of the item.
        /// </summary>
        public object Tag = null;         


        /// <summary>
        /// Event arguments ctor for Toolbox event.
        /// </summary>
        /// <param name="tc">Caption of the Tab containing the selected Item.</param>
        /// <param name="ic">Caption of the item.</param>
        /// <param name="iid">ID of the item.</param>
        /// <param name="ti">Index of the Tab.</param>
        /// <param name="ii">Index of the item.</param>
        /// <param name="tag">Tag of the item.</param>
        public ToolBoxEventArgs(string tc, string ic, string iid, int ti, int ii, object tag)
        {
            TabCaption = tc;
            ItemCaption = ic;
            ItemId = iid;
            TabIndex = ti;
            ItemIndex = ii;
            Tag = tag;
        }
    }
}
