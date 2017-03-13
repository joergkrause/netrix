using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using System.Collections.Generic;
using System.Drawing;

namespace GuruComponents.Netrix.WebEditing.Glyphs
{
    /// <summary>
    /// Build the glyph string to load user defined resources.
    /// </summary>
    /// <remarks>
    /// This class covers the building of glyph resources internally and holds the corresponding
    /// properties. By default, NetRix comes with two flavers of glyphs: Standard, similar to 
    /// former WebMatrix or FrontPage, and colored onces similar to InfoPath/WordXML.
    /// For proper usage the resource assembly must be present, <i>GuruComponents.Netrix.Resources.dll</i>.
    /// This assembly contains only image resources, most of them are glyphs, but also contains several
    /// others.
    /// <para>
    /// Note: Changing properties at runtime requires the control to stay in ready state (e.g. <see cref="IHtmlEditor.IsReady">IsReady</see> is <c>true</c>).
    /// We recommend using the ReadyStateComplete event to get informed when the controls turns into ready state.
    /// </para> 
    /// </remarks>
    /// <seealso cref="IHtmlEditor.Glyphs"/>
    /// <seealso cref="IHtmlEditor.ReadyStateComplete"/>
    /// <seealso cref="IHtmlEditor.IsReady"/>
    [Serializable()]
    public sealed class BuildGlyphs
    {
        private const string RES_ASSEMBLY = "GuruComponents.Netrix.Resources.dll";
        private const string RES_GLYPHCLASS = @"/GIFS/";

        private GlyphVariant _glyphVariant = GlyphVariant.Standard;
        private bool _glyphsVisible = false;
        private HtmlGlyphsKind _glyphskind = HtmlGlyphsKind.None;

        // private static Dictionary<string, HtmlGlyphs> _glyphs;
        private static Dictionary<GlyphVariant, Dictionary<string, HtmlGlyphs>> _glyphs;

        [NonSerialized()]
        private static Hashtable _table = new Hashtable();
        [NonSerialized()]
        private IHtmlEditor _htmlEditor;

        static BuildGlyphs()
        {
            SetGlyphs();
        }

        # region Public Properties

        /// <summary>
        /// Gets or sets the kind of glyph shown in the document.
        /// </summary>
        /// <remarks>
        /// This property one should set before activating glyphs by calling GlyphVisible.
        /// </remarks>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.Glyphs"/>
        /// <seealso cref="GlyphVariant"/>
        [Browsable(true), Category("NetRix Component")]
        [DefaultValue(typeof(HtmlGlyphsKind), "None")]
        public HtmlGlyphsKind GlyphKind
        {
            get
            {
                return _glyphskind;
            }
            set
            {
                _glyphskind = value;
                _htmlEditor.Exec(Interop.IDM.EMPTYGLYPHTABLE);
                if (_glyphskind != HtmlGlyphsKind.None && _glyphsVisible)
                {
                    _htmlEditor.Exec(Interop.IDM.ADDTOGLYPHTABLE, GetDefinitionString(_glyphskind, _glyphVariant));
                }
            }
        }

        /// <summary>
        /// Type of glyph we use.
        /// </summary>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.Glyphs"/>
        /// <seealso cref="GlyphKind"/>
        [Category("NetRix Component")]
        [Browsable(true), DefaultValue(typeof(GlyphVariant), "Standard")]
        public GlyphVariant GlyphVariant
        {
            get
            {
                return _glyphVariant;
            }
            set
            {
                _glyphVariant = value;
                _htmlEditor.Exec(Interop.IDM.EMPTYGLYPHTABLE);
                if (_glyphskind != HtmlGlyphsKind.None)
                {
                    _htmlEditor.Exec(Interop.IDM.ADDTOGLYPHTABLE, GetDefinitionString(_glyphskind, _glyphVariant));
                }
            }
        }

        /// <summary>
        /// Shows or Hides custom glyphs.
        /// </summary>
        /// <remarks>
        /// Glyphs are images representing tags in design view.
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.Glyphs"/>
        /// <seealso cref="GlyphKind"/>
        /// </remarks>
        [Category("NetRix Component")]
        [DefaultValue(false), Browsable(true), Description("If set to true the editor area shows basic glyphs around the tag areas to identify tag boundaries")]
        public bool GlyphsVisible
        {
            get
            {
                return _glyphsVisible;
            }
            set
            {
                _glyphsVisible = value;
                if (_htmlEditor.IsReady)
                {
                    _htmlEditor.Exec(Interop.IDM.EMPTYGLYPHTABLE);
                    if (_glyphsVisible)
                    {
                        _htmlEditor.Exec(Interop.IDM.ADDTOGLYPHTABLE, GetDefinitionString(_glyphskind, _glyphVariant));
                    }
                }
            }
        }

        # endregion

        /// <summary>
        /// Return the glyphs definition string used internally.
        /// </summary>
        /// <param name="glyphsKind"></param>
        /// <param name="glyphVariant"></param>
        /// <returns></returns>
        public static string GetDefinitionString(HtmlGlyphsKind glyphsKind, GlyphVariant glyphVariant)
        {
            if (glyphsKind == HtmlGlyphsKind.None) return String.Empty;
            string key = String.Format("{0}:{1}", glyphsKind, glyphVariant);
            if (!_table.ContainsKey(key))
            {
                StringBuilder stringBuilder = new StringBuilder();
                GetGlyphs(glyphsKind, glyphVariant);
                foreach (KeyValuePair<string, HtmlGlyphs> editorGlyph in _glyphs[glyphVariant])
                {
                    if (editorGlyph.Value.Visible)
                    {
                        stringBuilder.Append(editorGlyph.Value.DefinitionString);
                    }
                }
                _table[key] = stringBuilder.ToString();

            }
            return _table[key].ToString();
        }

        private static void SetGlyphs()
        {
            _glyphs = new Dictionary<GlyphVariant, Dictionary<string, HtmlGlyphs>>();
            _glyphs.Add(GlyphVariant.Standard, GetGlyphCollection(""));
            _glyphs.Add(GlyphVariant.Colored, GetGlyphCollection("C-"));
        }

        private static Dictionary<string, HtmlGlyphs> GetGlyphCollection(string v)
        {
            String str = String.Format("res://{0}\\{1}{2}",
                                       Path.GetDirectoryName(typeof(BuildGlyphs).Assembly.Location).ToLower(),
                                       RES_ASSEMBLY,
                                       RES_GLYPHCLASS);
            Dictionary<string, HtmlGlyphs> d1 = new Dictionary<string, HtmlGlyphs>();
            d1.Add(HtmlGlyphs.GetGlyphKey("word.p", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("p", HtmlGlyphsType.CloseTag, String.Concat(str, "WORD-PARA"), 16, 16));
            d1.Add(HtmlGlyphs.GetGlyphKey("word.div", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("div", HtmlGlyphsType.CloseTag, String.Concat(str, "WORD-PARA"), 16, 14));
            d1.Add(HtmlGlyphs.GetGlyphKey("word.br", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("br", HtmlGlyphsType.OpenTag, String.Concat(str, "WORD-BREAK"), 16, 14));
            d1.Add(HtmlGlyphs.GetGlyphKey("a", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("a", HtmlGlyphsType.CloseTag, String.Concat(str, v, "A-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("a", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("a", HtmlGlyphsType.OpenTag, String.Concat(str, v, "A-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("address", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("address", HtmlGlyphsType.CloseTag, String.Concat(str, v, "ADDRESS-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("address", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("address", HtmlGlyphsType.OpenTag, String.Concat(str, v, "ADDRESS-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("applet", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("applet", HtmlGlyphsType.CloseTag, String.Concat(str, v, "APPLET-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("applet", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("applet", HtmlGlyphsType.OpenTag, String.Concat(str, v, "APPLET-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("b", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("b", HtmlGlyphsType.CloseTag, String.Concat(str, v, "B-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("b", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("b", HtmlGlyphsType.OpenTag, String.Concat(str, v, "B-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("big", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("big", HtmlGlyphsType.CloseTag, String.Concat(str, v, "BIG-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("big", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("big", HtmlGlyphsType.OpenTag, String.Concat(str, v, "BIG-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("blockquote", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("blockquote", HtmlGlyphsType.CloseTag, String.Concat(str, v, "BLOCKQ-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("blockquote", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("blockquote", HtmlGlyphsType.OpenTag, String.Concat(str, v, "BLOCKQ-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("br", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("br", HtmlGlyphsType.BothTags, String.Concat(str, v, "BR"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("caption", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("caption", HtmlGlyphsType.CloseTag, String.Concat(str, v, "CAPTION-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("caption", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("caption", HtmlGlyphsType.OpenTag, String.Concat(str, v, "CAPTION-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("center", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("center", HtmlGlyphsType.CloseTag, String.Concat(str, v, "CENTER-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("center", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("center", HtmlGlyphsType.OpenTag, String.Concat(str, v, "CENTER-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("cite", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("cite", HtmlGlyphsType.CloseTag, String.Concat(str, v, "CITE-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("cite", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("cite", HtmlGlyphsType.OpenTag, String.Concat(str, v, "CITE-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("code", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("code", HtmlGlyphsType.CloseTag, String.Concat(str, v, "CODE-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("code", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("code", HtmlGlyphsType.OpenTag, String.Concat(str, v, "CODE-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("dd", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("dd", HtmlGlyphsType.CloseTag, String.Concat(str, v, "DD-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("dd", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("dd", HtmlGlyphsType.OpenTag, String.Concat(str, v, "DD-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("dfn", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("dfn", HtmlGlyphsType.CloseTag, String.Concat(str, v, "DFN-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("dfn", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("dfn", HtmlGlyphsType.OpenTag, String.Concat(str, v, "DFN-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("dir", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("dir", HtmlGlyphsType.CloseTag, String.Concat(str, v, "DIR-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("dir", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("dir", HtmlGlyphsType.OpenTag, String.Concat(str, v, "DIR-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("siv", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("div", HtmlGlyphsType.CloseTag, String.Concat(str, v, "DIV-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("div", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("div", HtmlGlyphsType.OpenTag, String.Concat(str, v, "DIV-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("dl", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("dl", HtmlGlyphsType.CloseTag, String.Concat(str, v, "DL-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("dl", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("dl", HtmlGlyphsType.OpenTag, String.Concat(str, v, "DL-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("dt", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("dt", HtmlGlyphsType.CloseTag, String.Concat(str, v, "DT-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("dt", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("dt", HtmlGlyphsType.OpenTag, String.Concat(str, v, "DT-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("em", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("em", HtmlGlyphsType.CloseTag, String.Concat(str, v, "EM-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("em", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("em", HtmlGlyphsType.OpenTag, String.Concat(str, v, "EM-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("font", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("font", HtmlGlyphsType.CloseTag, String.Concat(str, v, "FONT-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("font", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("font", HtmlGlyphsType.OpenTag, String.Concat(str, v, "FONT-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("form", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("form", HtmlGlyphsType.CloseTag, String.Concat(str, v, "FORM-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("form", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("form", HtmlGlyphsType.OpenTag, String.Concat(str, v, "FORM-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h1", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("h1", HtmlGlyphsType.CloseTag, String.Concat(str, v, "H1-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h1", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("h1", HtmlGlyphsType.OpenTag, String.Concat(str, v, "H1-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h2", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("h2", HtmlGlyphsType.CloseTag, String.Concat(str, v, "H2-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h2", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("h2", HtmlGlyphsType.OpenTag, String.Concat(str, v, "H2-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h3", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("h3", HtmlGlyphsType.CloseTag, String.Concat(str, v, "H3-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h3", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("h3", HtmlGlyphsType.OpenTag, String.Concat(str, v, "H3-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h4", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("h4", HtmlGlyphsType.CloseTag, String.Concat(str, v, "H4-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h4", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("h4", HtmlGlyphsType.OpenTag, String.Concat(str, v, "H4-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h5", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("h5", HtmlGlyphsType.CloseTag, String.Concat(str, v, "H5-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h5", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("h5", HtmlGlyphsType.OpenTag, String.Concat(str, v, "H5-OPEN"),v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h6", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("h6", HtmlGlyphsType.CloseTag, String.Concat(str, v, "H6-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("h6", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("h6", HtmlGlyphsType.OpenTag, String.Concat(str, v, "H6-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("hr", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("hr", HtmlGlyphsType.OpenTag, String.Concat(str, v, "HR"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("i", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("i", HtmlGlyphsType.CloseTag, String.Concat(str, v, "I-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("i", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("i", HtmlGlyphsType.OpenTag, String.Concat(str, v, "I-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("img", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("img", HtmlGlyphsType.BothTags, String.Concat(str, v, "IMG"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("kbd", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("kbd", HtmlGlyphsType.CloseTag, String.Concat(str, v, "KBD-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("kbd", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("kbd", HtmlGlyphsType.OpenTag, String.Concat(str, v, "KBD-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("li", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("li", HtmlGlyphsType.CloseTag, String.Concat(str, v, "LI-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("li", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("li", HtmlGlyphsType.OpenTag, String.Concat(str, v, "LI-OPEN"),v));
            d1.Add(HtmlGlyphs.GetGlyphKey("map", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("map", HtmlGlyphsType.CloseTag, String.Concat(str, v, "MAP-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("map", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("map", HtmlGlyphsType.OpenTag, String.Concat(str, v, "MAP-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("menu", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("menu", HtmlGlyphsType.CloseTag, String.Concat(str, v, "MENU-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("menu", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("menu", HtmlGlyphsType.OpenTag, String.Concat(str, v, "MENU-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("ol", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("ol", HtmlGlyphsType.CloseTag, String.Concat(str, v, "OL-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("ol", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("ol", HtmlGlyphsType.OpenTag, String.Concat(str, v, "OL-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("p", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("p", HtmlGlyphsType.CloseTag, String.Concat(str, v, "P-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("p", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("p", HtmlGlyphsType.OpenTag, String.Concat(str, v, "P-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("param", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("param", HtmlGlyphsType.CloseTag, String.Concat(str, v, "PARAM-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("param", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("param", HtmlGlyphsType.OpenTag, String.Concat(str, v, "PARAM-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("pre", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("pre", HtmlGlyphsType.CloseTag, String.Concat(str, v, "PRE-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("pre", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("pre", HtmlGlyphsType.OpenTag, String.Concat(str, v, "PRE-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("samp", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("samp", HtmlGlyphsType.CloseTag, String.Concat(str, v, "SAMP-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("samp", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("samp", HtmlGlyphsType.OpenTag, String.Concat(str, v, "SAMP-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("script", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("script", HtmlGlyphsType.BothTags, String.Concat(str, v, "SCRIPT"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("select", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("select", HtmlGlyphsType.CloseTag, String.Concat(str, v, "SELECT-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("select", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("select", HtmlGlyphsType.OpenTag, String.Concat(str, v, "SELECT-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("small", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("small", HtmlGlyphsType.CloseTag, String.Concat(str, v, "SMALL-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("small", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("small", HtmlGlyphsType.OpenTag, String.Concat(str, v, "SMALL-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("strike", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("strike", HtmlGlyphsType.CloseTag, String.Concat(str, v, "STRIKE-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("strike", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("strike", HtmlGlyphsType.OpenTag, String.Concat(str, v, "STRIKE-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("strong", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("strong", HtmlGlyphsType.CloseTag, String.Concat(str, v, "STRONG-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("strong", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("strong", HtmlGlyphsType.OpenTag, String.Concat(str, v, "STRONG-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("style", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("style", HtmlGlyphsType.BothTags, String.Concat(str, v, "STYLE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("span", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("span", HtmlGlyphsType.CloseTag, String.Concat(str, v, "SPAN-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("span", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("span", HtmlGlyphsType.OpenTag, String.Concat(str, v, "SPAN-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("sub", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("sub", HtmlGlyphsType.CloseTag, String.Concat(str, v, "SUB-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("sub", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("sub", HtmlGlyphsType.OpenTag, String.Concat(str, v, "SUB-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("sup", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("sup", HtmlGlyphsType.CloseTag, String.Concat(str, v, "SUP-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("sup", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("sup", HtmlGlyphsType.OpenTag, String.Concat(str, v, "SUP-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("table", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("table", HtmlGlyphsType.BothTags, String.Concat(str, v, "TABLE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("textarea", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("textarea", HtmlGlyphsType.CloseTag, String.Concat(str, v, "TEXTAREA-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("textarea", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("textarea", HtmlGlyphsType.OpenTag, String.Concat(str, v, "TEXTAREA-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("tt", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("tt", HtmlGlyphsType.CloseTag, String.Concat(str, v, "TT-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("tt", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("tt", HtmlGlyphsType.OpenTag, String.Concat(str, v, "TT-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("u", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("u", HtmlGlyphsType.CloseTag, String.Concat(str, v, "U-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("u", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("u", HtmlGlyphsType.OpenTag, String.Concat(str, v, "U-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("ul", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("ul", HtmlGlyphsType.CloseTag, String.Concat(str, v, "UL-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("ul", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("ul", HtmlGlyphsType.OpenTag, String.Concat(str, v, "UL-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("var", HtmlGlyphsType.CloseTag),
                        new HtmlGlyphs("var", HtmlGlyphsType.CloseTag, String.Concat(str, v, "VAR-CLOSE"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("var", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("var", HtmlGlyphsType.OpenTag, String.Concat(str, v, "VAR-OPEN"), v));
            d1.Add(HtmlGlyphs.GetGlyphKey("comment", HtmlGlyphsType.OpenTag),
                        new HtmlGlyphs("comment", HtmlGlyphsType.OpenTag, String.Concat(str, v, "COMMENT"), v));

            return d1;
        }

        private static void SetGlyphsState(bool state, GlyphVariant v)
        {
            foreach (KeyValuePair<string, HtmlGlyphs> glyph in _glyphs[v])
            {
                glyph.Value.Visible = state;
            }
        }

        private static void GetGlyphs(HtmlGlyphsKind glyphKind, GlyphVariant v)
        {
            switch (glyphKind)
            {
                case HtmlGlyphsKind.InvisibleTags:
                    SetGlyphsState(false, v);
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("br", HtmlGlyphsType.OpenTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("comment", HtmlGlyphsType.OpenTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("div", HtmlGlyphsType.CloseTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("div", HtmlGlyphsType.OpenTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("script", HtmlGlyphsType.OpenTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("form", HtmlGlyphsType.CloseTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("form", HtmlGlyphsType.OpenTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("map", HtmlGlyphsType.CloseTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("map", HtmlGlyphsType.OpenTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("span", HtmlGlyphsType.CloseTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("span", HtmlGlyphsType.OpenTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("p", HtmlGlyphsType.CloseTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("p", HtmlGlyphsType.OpenTag)].Visible = true;
                    break;
                case HtmlGlyphsKind.AreaTags:
                    SetGlyphsState(false, v);
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("map", HtmlGlyphsType.CloseTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("map", HtmlGlyphsType.OpenTag)].Visible = true;
                    break;
                case HtmlGlyphsKind.CommentTags:
                    SetGlyphsState(false, v);
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("comment", HtmlGlyphsType.OpenTag)].Visible = true;
                    break;
                case HtmlGlyphsKind.ScriptTags:
                    SetGlyphsState(false, v);
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("script", HtmlGlyphsType.OpenTag)].Visible = true;
                    break;
                case HtmlGlyphsKind.TableTags:
                    SetGlyphsState(false, v);
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("table", HtmlGlyphsType.OpenTag)].Visible = true;
                    break;
                case HtmlGlyphsKind.WordPara:
                    SetGlyphsState(false, v);
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("word.p", HtmlGlyphsType.CloseTag)].Visible = true; 
                    break;
                case HtmlGlyphsKind.WordParaDivBr:
                    SetGlyphsState(false, v);
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("word.p", HtmlGlyphsType.CloseTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("word.br", HtmlGlyphsType.OpenTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("word.div", HtmlGlyphsType.CloseTag)].Visible = true;
                    break;
                case HtmlGlyphsKind.StyleTags:
                    SetGlyphsState(false, v);
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("style", HtmlGlyphsType.OpenTag)].Visible = true;
                    break;
                case HtmlGlyphsKind.MiscTags:
                    SetGlyphsState(false, v);
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("p", HtmlGlyphsType.OpenTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("p", HtmlGlyphsType.CloseTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("br", HtmlGlyphsType.OpenTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("div", HtmlGlyphsType.CloseTag)].Visible = true;
                    _glyphs[v][HtmlGlyphs.GetGlyphKey("div", HtmlGlyphsType.OpenTag)].Visible = true;
                    break;
                default:
                    SetGlyphsState(true, v);
                    break;
            }
        }


        /// <summary>
        /// Ctor which creates an instance of the glyph build and management class.
        /// </summary>
        /// <param name="htmlEditor">Reference to attached editor.</param>
        public BuildGlyphs(IHtmlEditor htmlEditor)
        {
            _htmlEditor = htmlEditor;

            //string reslib = Path.Combine(Path.GetDirectoryName(typeof (BuildGlyphs).Assembly.Location).ToLower(), RES_ASSEMBLY);
            //EnumResources.Load(reslib);
        }

        /// <summary>
        /// Support for design time.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int props = 0;
            props += (_glyphVariant == GlyphVariant.Standard) ? 0 : 1;
            props += (_glyphsVisible == false) ? 0 : 1;
            props += (_glyphskind == HtmlGlyphsKind.None) ? 0 : 1;
            return String.Format("{0} propert{1} changed", props, props == 1 ? "y" : "ies");
        }

    }

}