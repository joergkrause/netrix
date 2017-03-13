using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// Type of numbers used for the ordered list element.
    /// </summary>
    public enum OrderedListType
    {
        /// <summary>
        /// The default value, arabic digits.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Numeric values, also arabic digits.
        /// </summary>
        Numeric = 1,
        /// <summary>
        /// Alpha numeric, a., b., c. and so on, in lower cases.
        /// </summary>
        LowerCaseAlphabetic = 2,
        /// <summary>
        /// Alpha numeric, A., B., C. and so on, in upper cases.
        /// </summary>
        UpperCaseAlphabetic = 3,
        /// <summary>
        /// Roman digits, like iv. or xii, written in lower cases.
        /// </summary>
        LowerCaseRoman = 4,
        /// <summary>
        /// Roman digits, like IV. or XII, written in upper cases.
        /// </summary>
        UpperCaseRoman = 5,
    }
}