using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
    /// Sets or retrieves whether to break words when the content exceeds the boundaries of its container.
	/// </summary>
	public enum WordWrap
    {
        /// <summary>
        /// Default. Content exceeds the boundaries of its container.
        /// </summary>
        Normal,
        /// <summary>
        /// Content wraps to next line, and a word-break occurs when necessary.
        /// </summary>
        BreakWord
	}
}
