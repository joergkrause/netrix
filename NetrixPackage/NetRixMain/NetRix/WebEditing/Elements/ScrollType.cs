using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The scroll type used in scrollable elements.
    /// </summary>
    public enum ScrollType
    {
        /// <summary>
        /// Scrolling is allowed and bars are always visible.
        /// </summary>
        Yes = 0,
        /// <summary>
        /// Scrolling is not allowed. Content outside the are is clipped.
        /// </summary>
        No = 1,
        /// <summary>
        /// Scrolling is allowed. The scrollbars are hide if content is smaller than area.
        /// </summary>
        Auto = 2,
    }
}