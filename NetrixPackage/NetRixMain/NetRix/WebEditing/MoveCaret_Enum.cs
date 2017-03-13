using System;

namespace GuruComponents.Netrix.WebEditing
{
    /// <summary>
    /// Defines hwo the caret is being moved.
    /// </summary>
	public enum MoveCaret : long
	{
        /// <summary>
        /// Move to previous line.
        /// </summary>
		PreviousLine = 1,
        /// <summary>
        /// Move to next line.
        /// </summary>
		NextLine = 2,
        /// <summary>
        /// Move to beginning of current line.
        /// </summary>
		CurrentLineStart = 3,
        /// <summary>
        /// Move to end of current line.
        /// </summary>
		CurrentLineEnd = 4,
        /// <summary>
        /// Move to top of window, e.g. the first visible line.
        /// </summary>
		TopOfWindow = 5,
        /// <summary>
        /// Move to bottom of window, e.g. the last visible line.
        /// </summary>
		BottomOfWindow = 6
	}
}