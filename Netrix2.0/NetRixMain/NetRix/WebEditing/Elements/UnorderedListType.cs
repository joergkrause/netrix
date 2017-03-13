using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The type of listitems in an unordered (bulleted) list.
    /// </summary>
    public enum UnorderedListType
    {
        /// <summary>
        /// The default value.
        /// </summary>
        Default = 0,
        /// <summary>
        /// A circle is used.
        /// </summary>
        Circle = 1,
        /// <summary>
        /// A disc is used (full circle).
        /// </summary>
        Disc = 2,
        /// <summary>
        /// A square icon is used.
        /// </summary>
        Square = 3,
    }
}