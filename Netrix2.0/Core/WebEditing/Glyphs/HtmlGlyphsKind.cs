using System;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Glyphs
{
    /// <summary>
    /// Type of glyphs displayed in the document.
    /// </summary>
	[Serializable()]
    public enum HtmlGlyphsKind
	{
        /// <summary>
        /// No glyphs shown.
        /// </summary>
        None            = 0,
        /// <summary>
        /// All tags get a glyph sign.
        /// </summary>
        AllTags         = Interop.IDM.SHOWALLTAGS,
        /// <summary>
        /// Tags with align attributes only. 
        /// </summary>
        AlignedSite     = Interop.IDM.SHOWALIGNEDSITETAGS,
        /// <summary>
        /// Script tags only. 
        /// </summary>
        ScriptTags      = Interop.IDM.SHOWSCRIPTTAGS,
        /// <summary>
        /// Style tags only. 
        /// </summary>
        StyleTags       = Interop.IDM.SHOWSTYLETAGS,
        /// <summary>
        /// Only comments are presented as glyphs. 
        /// </summary>
        CommentTags     = Interop.IDM.SHOWCOMMENTTAGS,
        /// <summary>
        /// Only area tags. Currently not implemented.
        /// </summary>
        AreaTags        = Interop.IDM.SHOWAREATAGS,
        /// <summary>
        /// Only insisible tags are shown. Use to replace the not implemented options.
        /// </summary>
        InvisibleTags   = Interop.IDM.SHOWUNKNOWNTAGS,
        /// <summary>
        /// Table tags only. 
        /// </summary>
        TableTags       = Interop.IDM.SHOWTABLE,
        /// <summary>
        /// Only other tags. Currently not implemented. 
        /// </summary>
        MiscTags        = Interop.IDM.SHOWMISCTAGS,
        /// <summary>
        /// Shows the word like 'P' sign as the one and only glyph.
        /// </summary>
        WordPara        = 98,
        /// <summary>
        /// Shows the word like 'P' sign as glyph for &lt;p&gt; and &lt;div&gt; on closing tag, as well as the small arrow for &lt;br&gt; tags.
        /// </summary>
        WordParaDivBr	= 99
	}
}
