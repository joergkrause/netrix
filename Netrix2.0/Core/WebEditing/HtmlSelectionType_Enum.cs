using System;
namespace GuruComponents.Netrix 
{
    /// <summary>
    /// Declares the type of selection currently made by user.
    /// </summary>
    /// <remarks>
    /// This enumeration is used by the <see cref="ISelection"/> interface.
    /// The Flags attribute was added in v2.0 (2010) to support elements, which are selectable and contain selectable text, such as SPAN. In those case
    /// the selection class returns the element as part of the elements' collection and the selected text that the element contains.
    /// </remarks>
    [Flags]
    public enum HtmlSelectionType 
    {
        /// <summary>
        /// No selection.
        /// </summary>
        Empty = 0,
        /// <summary>
        /// Text selection or elements with texts are selected.
        /// </summary>
        TextSelection = 1,
        /// <summary>
        /// Block elements only selected.
        /// </summary>
        ElementSelection = 2
    }
}