using System;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
	/// <summary>
	/// Determines the z-order in which the binary behavior will drawn against the underlying element.	
	/// </summary>
    /// <remarks>
    /// If a behavior is drawn behind an element, then it is not visible if the element has a opaque surface.
    /// </remarks>
	public enum HtmlZOrder
	{
        /// <summary>
        /// The behavior draws after the element and all its child elements are drawn.
        /// </summary>
        AboveContent = 7,
        /// <summary>
        /// The behavior draws after the element itself is rendered, but before the element's child elements with a positive z-order value are rendered.
        /// </summary>
        AboveFlow = 6,
        /// <summary>
        /// The behavior draws after the background is drawn and before the element (along with any of its child elements) is drawn.
        /// </summary>
        BelowContent = 4,
        /// <summary>
        /// The behavior draws after the element's child elements with a negative z-order value are rendered, but before the element itself is rendered.
        /// </summary>
        BelowFlow = 5,
        /// <summary>
        /// Not defined.
        /// </summary>
        None = 0,
        /// <summary>
        /// The behavior replaces the element content, including the background.
        /// </summary>
        ReplaceAll = 1,
        /// <summary>
        /// The behavior replaces the element content but not the background.
        /// </summary>
        ReplaceContent = 2,
        /// <summary>
        /// The behavior replaces the element's background, but not its content.
        /// </summary>
        ReplaceBackground = 3,
        /// <summary>
        /// The behavior draws after the whole page is drawn and draws on top of all the content.
        /// </summary>
        WindowTop = 8
	}
}
