using System;

namespace GuruComponents.Netrix.HtmlFormatting
{
    /// <summary>
    /// An enumaration to control the case folding option the formatter uses during the
    /// rewriting of tagnames and attributes.
    /// </summary>
    public enum HtmlFormatterCase
    {
        /// <summary>
        /// The case found in the MSHTML source will not changed.
        /// </summary>
        PreserveCase = 0,
        /// <summary>
        /// The content will changed to upper case.
        /// </summary>
        UpperCase = 1,
        /// <summary>
        /// The content will changed to lower case.
        /// </summary>
        LowerCase = 2,
    }

}
