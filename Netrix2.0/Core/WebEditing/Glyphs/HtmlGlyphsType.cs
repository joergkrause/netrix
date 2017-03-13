namespace GuruComponents.Netrix.WebEditing.Glyphs
{
	/// <summary>
	/// The type of glyph we use to for a specific tag.
	/// </summary>
	/// <remarks>
	/// Most tags support opening and closing glyph. Some which don't appear as containers, like BR,
	/// do support opening tag only. Some others, which do cover mostly a rectangle region, like TABLE or IMG,
	/// may accept both options, but still display the opening tag only.
	/// However, for some inline tags like STRONG or EM, both glyphs are supported and makes perfectly sense.
	/// </remarks>
    public enum HtmlGlyphsType
    {
        /// <summary>
        /// Glyph appears for opening tag only.
        /// </summary>
        OpenTag = 0,
        /// <summary>
        /// Glyph appears for closing tag only.
        /// </summary>
        CloseTag = 1,
        /// <summary>
        /// Glyph appear for both tags.
        /// </summary>
        BothTags = 2,
    }
}
