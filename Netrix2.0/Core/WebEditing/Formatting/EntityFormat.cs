using System;

namespace GuruComponents.Netrix.HtmlFormatting
{
    /// <summary>
    /// The MSHTML DLL internally codes all enhanced characters in the coding of the document
    /// and replaces all entities with there values in the current codepage. To avoid conflict
    /// with other systems expecting entities, the formatter can replace all enhanced characters
    /// with entities. This enumeration determines if and how this replacement will go.
    /// </summary>
    public enum EntityFormat
    {
        /// <summary>
        /// Using named entities, like &amp;name;.
        /// </summary>
        Named       = 1,
        /// <summary>
        /// using numeric entities, like &amp;#123;.
        /// </summary>
        Numeric     = 2,
        /// <summary>
        /// Do not replace any enhanced characters.
        /// </summary>
        NoEntity    = 0
    }
}
