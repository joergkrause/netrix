using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// Alle Media values acceptable within the media attribute of link tag.
	/// </summary>
	public enum LinkElementMedia
	{
        /// <summary>
        /// All media allowed.
        /// </summary>
        All         = 0,
        /// <summary>
        /// Relevant for screen.
        /// </summary>
        Screen      = 1,
        /// <summary>
        /// Teletype terminals. Deprecated.
        /// </summary>
        TTY         = 2,
        /// <summary>
        /// For usage on TV, Set-top boxes.
        /// </summary>
        TV          = 3,
        /// <summary>
        /// For projection.
        /// </summary>
        Projection  = 4,
        /// <summary>
        /// For handhelds (pocket PC).
        /// </summary>
        HandHeld    = 5,
        /// <summary>
        /// For printing.
        /// </summary>
        Print       = 6,
        /// <summary>
        /// For output to Braille devices.
        /// </summary>
        Braille     = 7,
        /// <summary>
        /// For aural output.
        /// </summary>
        Aural       = 8,
	}
}
