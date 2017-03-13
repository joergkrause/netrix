using System;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// Provides information about why an element change event happens.
    /// </summary>
    public enum HtmlEventTypeEnum
    {
        /// <summary>
        /// Mouse click
        /// </summary>
        Click		= 0,        // 
        /// <summary>
        /// Any key event.
        /// </summary>
        KeyPress	= 1,        // 
        /// <summary>
        /// Mouse move or drop operation happens in conjunction with a selection process.
        /// </summary>
        Selection	= 2,        // 
        /// <summary>
        /// Caret moved into element boundaries.
        /// </summary>
        FocusIn		= 3,        // 
        /// <summary>
        /// Caret moved from element.
        /// </summary>
        FocusOut	= 4,        // 
        /// <summary>
        /// Element was inserted.
        /// </summary>
        Inserted    = 8,        // 
        /// <summary>
        /// Any other situation (there is no definition when this happens).
        /// </summary>
        Unknown		= 9         // 
    }
}
