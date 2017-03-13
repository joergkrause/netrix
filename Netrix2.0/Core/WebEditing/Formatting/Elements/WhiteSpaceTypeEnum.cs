using System;

namespace GuruComponents.Netrix.HtmlFormatting.Elements
{
	/// <summary>
	/// Determines how significant are whitespaces within this element.
	/// </summary>
    public enum WhiteSpaceType
    {
        /// <summary>
        /// Preserve Whitespaces in any case.
        /// </summary>
        Significant = 0,
        /// <summary>
        /// Treat whitespaces as non significant.
        /// </summary>
        NotSignificant = 1,
        /// <summary>
        /// Let whitespaces as they are, but add when formatting requires one.
        /// </summary>
        CarryThrough = 2,
    }
}
