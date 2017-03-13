using System;

namespace GuruComponents.Netrix.HtmlFormatting.Elements
{
	/// <summary>
	/// What kind of element this is.
	/// </summary>
    public enum ElementType
    {
        /// <summary>
        /// Not specified
        /// </summary>
        Other = 0,
        /// <summary>
        /// BLock
        /// </summary>
        Block = 1,
        /// <summary>
        /// Inline 
        /// </summary>
        Inline = 2,
        /// <summary>
        /// Any element without rendering information
        /// </summary>
        Any = 3
    }
}
