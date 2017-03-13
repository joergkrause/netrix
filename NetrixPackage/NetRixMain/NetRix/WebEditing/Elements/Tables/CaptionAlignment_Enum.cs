using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// Alignment for table caption.
	/// </summary>
	/// <remarks>
	/// This enumeration is used for compatibility only. It is recommended to format 
	/// captions with styles (CSS) instead.
	/// </remarks>
    public enum CaptionAlignment
    {
        /// <summary>
        /// Place the caption in middle top of the table.
        /// </summary>
        Top     = 0,
        /// <summary>
        /// Place caption in top left corner.
        /// </summary>
        Left    = 1,
        /// <summary>
        /// Place caption in the top right corner.
        /// </summary>
        Right   = 2,
        /// <summary>
        /// Place caption in the bottom middle.
        /// </summary>
        Bottom  = 3
    }
}
