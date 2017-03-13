using System;

namespace GuruComponents.Netrix.HtmlFormatting.Elements
{
	/// <summary>
	/// Determines how to format this kind of element.
	/// </summary>
	/// <remarks>
	/// These elements can be combined in any way.
	/// </remarks>
    [FlagsAttribute()]
    public enum FormattingFlags
    {
        /// <summary>
        /// No information present.
        /// </summary>
        None = 0,
        /// <summary>
        /// It's an inline element, like &lt;b&gt;, which is rendered in line.
        /// </summary>
        Inline = 1,
        /// <summary>
        /// This renders the element on its own line, but does not indent it.
        /// </summary>
        NoIndent = 2,
        /// <summary>
        /// This renders the element without expecting an end element.
        /// </summary>
        NoEndTag = 4,
        /// <summary>
        /// The content of the element has to be preserved.
        /// </summary>
        PreserveContent = 8,
        /// <summary>
        /// The element is supposed to render always as XML.
        /// </summary>
        Xml = 16,
        /// <summary>
        /// Treat as a Comment.
        /// </summary>
        Comment = 32,
        /// <summary>
        /// The element can be build as partial tag.
        /// </summary>
        AllowPartialTags = 64,
    }

}
