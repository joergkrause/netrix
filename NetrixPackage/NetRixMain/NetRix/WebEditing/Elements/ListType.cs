using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// Used to assign a list type.
    /// </summary>
    public enum ListType
    {
        /// <summary>
        /// Default beahvior.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Show numeric values.
        /// </summary>
        Numeric = 1,
        /// <summary>
        /// Use lower case alphanumeric values.
        /// </summary>
        LowerCaseAlphabetic = 2,
        /// <summary>
        /// Use upper case alphanumeric values.
        /// </summary>
        UpperCaseAlphabetic = 3,
        /// <summary>
        /// Use lower case Roman values.
        /// </summary>
        LowerCaseRoman = 4,
        /// <summary>
        /// Use upper case Roman values.
        /// </summary>
        UpperCaseRoman = 5,
        /// <summary>
        /// Use discs (full circle) as bullet icon.
        /// </summary>
        Disk = 6,
        /// <summary>
        /// Use circle (empty circel) as bullet icon.
        /// </summary>
        Circle = 7,
        /// <summary>
        /// Use a square as bullet icon.
        /// </summary>
        Square = 8,
    }
}
