using System;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
    /// <summary>
    /// Determines against which snapzone an element is currently resized.
    /// </summary>
	public enum SnapZone
	{
        /// <summary>
        /// No snap
        /// </summary>
		None				= 0,
        /// <summary>
        /// Snap the top zone.
        /// </summary>
        ZoneTop			= 1,
        /// <summary>
        /// Snap the left zone.
        /// </summary>
        ZoneLeft = 2,
        /// <summary>
        /// Snap the bottom zone.
        /// </summary>
        ZoneBottom = 3,
        /// <summary>
        /// Snap the right zone.
        /// </summary>
        ZoneRight = 4,
        /// <summary>
        /// Snap the top left corner.
        /// </summary>
        CornerTopLeft = 5,
        /// <summary>
        /// Snap the top right corner.
        /// </summary>
        CornerTopRight = 6,
        /// <summary>
        /// Snap the bottom left corner.
        /// </summary>
        CornerBottomLeft = 7,
        /// <summary>
        /// Snap the bottom right corner.
        /// </summary>
        CornerBottomRight = 8
	}
}
