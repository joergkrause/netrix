using System;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// Used to place the caret during replace and insertion of text.
	/// </summary>
	public enum ElementAdjacency
	{
        /// <summary>
        /// Place the caret after the word begun.
        /// </summary>
		AfterBegin,
        /// <summary>
        /// Place the caret after the word end.
        /// </summary>
		AfterEnd,
        /// <summary>
        /// Place the caret before the word begun.
        /// </summary>
		BeforeBegin,
        /// <summary>
        /// Place the caret before the word end.
        /// </summary>
		BeforeEnd
	}
}
