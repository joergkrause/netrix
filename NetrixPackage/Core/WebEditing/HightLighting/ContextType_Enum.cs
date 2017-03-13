using System;

namespace GuruComponents.Netrix.WebEditing.HighLighting
{
    /// <summary>
    /// An enumerated type that contains the different options for the Pointer behavior.
    /// </summary>
    public enum ContextType
    {
        /// <summary>
        /// There is no markup content next to the markup pointer in the specified direction.
        /// </summary>
        None = 0,
        /// <summary>
        /// The markup content next to the markup pointer in the specified direction is text.
        /// </summary>
        Text = 1,
        /// <summary>
        /// An element's opening tag is next to the markup pointer in the specified direction.
        /// </summary>
        EnterScope = 2,
        /// <summary>
        /// An element's closing tag is next to the markup pointer in the specified direction.
        /// </summary>
        ExitScope = 3,
        /// <summary>
        /// A element that doesn't have a closing tag, like a br, is next to the markup pointer in the specified direction.
        /// </summary>
        NoScope = 4
    }
}
