using System;

namespace GuruComponents.Netrix.WebEditing.Glyphs
{
	/// <summary>
	/// Defines the variant in which the selected glyph appears.
	/// </summary>
	[Serializable()]   
    public enum GlyphVariant
	{
		/// <summary>
		/// NetRix standard glyphs, mimic vs.net or web matrix.
		/// </summary>
		Standard,
		/// <summary>
		/// Colored glyphs, mimic Office Word/InfoPath 2003.
		/// </summary>
		Colored,
		/// <summary>
		/// Calls an external resource file with custom glyphs. Not yet implemented.
		/// </summary>
		Custom
	}
}
