using System;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

namespace GuruComponents.Netrix.WebEditing.Glyphs
{
    /// <summary>
    /// Holds information about one glyph and it's current state.
    /// </summary>
    public class HtmlGlyphs
    {
        private string _tag;
        private string _imageUrl;
        private HtmlGlyphsType _type;
        private int _width;
        private int _height;
        private bool _visible;
        private static Dictionary<string, Size> imageSize;

        static HtmlGlyphs()
        {
            imageSize = new Dictionary<string, Size>();
            SetGifTable();
        }

        /// <summary>
        /// Returns the current definition string, used by resources which show glyphs.
        /// </summary>
		[Browsable(false)]
        internal string DefinitionString
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("%%");
                stringBuilder.Append(_tag);
                stringBuilder.Append("^^%%");
                stringBuilder.Append(_imageUrl);
                stringBuilder.Append("^^%%");
                stringBuilder.Append((int)_type);
                stringBuilder.Append("^^%%");
                stringBuilder.Append(3);
                stringBuilder.Append("^^%%");
                stringBuilder.Append(3);
                stringBuilder.Append("^^%%");
                stringBuilder.Append(4);
                stringBuilder.Append("^^%%");
                stringBuilder.Append(_width);
                stringBuilder.Append("^^%%");
                stringBuilder.Append(_height);
                stringBuilder.Append("^^%%");
                stringBuilder.Append(_width);
                stringBuilder.Append("^^%%");
                stringBuilder.Append(_height);
                stringBuilder.Append("^^**");
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Height of glyph.
        /// </summary>
        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                _height = value;
            }
        }

        /// <summary>
        /// URL of glyph within the resource file.
        /// </summary>
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }

            set
            {
                _imageUrl = value;
            }
        }

        /// <summary>
        /// Key 
        /// </summary>
        public string Key
        {
            get
            {
                return GetGlyphKey(Tag, Type);
            }
        }

        /// <summary>
        /// Tag name.
        /// </summary>
        public string Tag
        {
            get
            {
                return _tag;
            }

            set
            {
                _tag = value;
            }
        }

        /// <summary>
        /// Glyph type
        /// </summary>
        public HtmlGlyphsType Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        /// <summary>
        /// Visible or not (only recognized during copy operation, not interactive).
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visible;
            }

            set
            {
                _visible = value;
            }
        }

        /// <summary>
        /// Width of glyph.
        /// </summary>
        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
            }
        }

        /// <summary>
        /// Default ctor for glyph information class.
        /// </summary>
        /// <remarks>
        /// The default settings are:
        /// <list type="bullet">
        ///     <item>Tag: Empty</item>
        ///     <item>Url: Empty</item>
        ///     <item>Type: BothTags</item>
        ///     <item>Width: 32</item>
        ///     <item>Height: 16</item>
        ///     <item>Visible: False</item>
        /// </list>
        /// </remarks>
		public HtmlGlyphs()
		{
			_tag = "";
			_imageUrl = "";
			_type = HtmlGlyphsType.BothTags;
			_width = 32;
			_height = 16;
			_visible = false;
		}

        /// <summary>
        /// Internally used ctor for glyph information class.
        /// </summary>
        /// <remarks>
        /// The default settings are:
        /// <list type="bullet">
        ///     <item>Tag: Parameter</item>
        ///     <item>Url: Parameter</item>
        ///     <item>Type: Parameter</item>
        ///     <item>Width: Parameter</item>
        ///     <item>Height: Parameter</item>
        ///     <item>Visible: True</item>
        /// </list>
        /// </remarks>
        /// <param name="tag">Tag this definition refers to.</param>
        /// <param name="type">Type of glyph.</param>
        /// <param name="imageUrl">URL of resource</param>
        /// <param name="width">Width in pixel (will bend image to fit into given value).</param>
        /// <param name="height">Height in pixel (will bend image to fit into given value).</param>
        public HtmlGlyphs(string tag, HtmlGlyphsType type, string imageUrl, int width, int height)
        {
            _tag = tag;
            _imageUrl = imageUrl;
            _type = type;
            _width = width;
            _height = height;
            _visible = true;
        }

        /// <summary>
        /// Internally used ctor for glyph information class.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        /// <param name="imageUrl"></param>
        /// <param name="prefix"></param>
        public HtmlGlyphs(string tag, HtmlGlyphsType type, string imageUrl, string prefix)
        {
            _tag = tag;
            _imageUrl = imageUrl;
            _type = type;
            string key = String.Format("{0}{1}{2}", prefix, tag.ToUpper(), GetTypeCode(type));
            Size s = imageSize[key];
            _width = s.Width;
            _height = s.Height;
            _visible = true;
        }

        private static string GetTypeCode(HtmlGlyphsType type)
        {
            switch (type)
            {
                case HtmlGlyphsType.OpenTag:
                    return "-OPEN";
                case HtmlGlyphsType.CloseTag:
                    return "-CLOSE";
                default:
                    return "";
            }
        }

        internal static string GetGlyphKey(string tag, HtmlGlyphsType type)
        {
            return String.Concat(tag, type);
        }

        /// <summary>
        /// Overriden to support PropertyGrid if publicitly editable.
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return String.Format("{0} [{3}] ({1}x{2}) {4}", _tag, _width, _height, _type,(_visible)?"":"*");
		}

        private static void SetGifTable()
        {
            // Standard
            imageSize.Add("A-CLOSE", new Size(22, 17));
            imageSize.Add("A-OPEN", new Size(38, 17));
            imageSize.Add("ADDRESS-CLOSE", new Size(58, 17));
            imageSize.Add("ADDRESS-OPEN", new Size(58, 17));
            imageSize.Add("APPLET-CLOSE", new Size(58, 17));
            imageSize.Add("APPLET-OPEN", new Size(58, 17));
            imageSize.Add("B-CLOSE", new Size(22, 17));
            imageSize.Add("B-OPEN", new Size(22, 17));
            imageSize.Add("BIG-CLOSE", new Size(28, 17));
            imageSize.Add("BIG-OPEN", new Size(28, 17));
            imageSize.Add("BLOCKQUOTE-CLOSE", new Size(62, 17));
            imageSize.Add("BLOCKQUOTE-OPEN", new Size(62, 17));
            imageSize.Add("BR", new Size(18, 17));
            imageSize.Add("CAPTION-CLOSE", new Size(62, 17));
            imageSize.Add("CAPTION-OPEN", new Size(62, 17));
            imageSize.Add("CENTER-CLOSE", new Size(44, 17));
            imageSize.Add("CENTER-OPEN", new Size(44, 17));
            imageSize.Add("CITE-CLOSE", new Size(44, 17));
            imageSize.Add("CITE-OPEN", new Size(44, 17));
            imageSize.Add("CODE-CLOSE", new Size(44, 17));
            imageSize.Add("CODE-OPEN", new Size(44, 17));
            imageSize.Add("DD-CLOSE", new Size(28, 17));
            imageSize.Add("DD-OPEN", new Size(28, 17));
            imageSize.Add("DFN-CLOSE", new Size(28, 17));
            imageSize.Add("DFN-OPEN", new Size(28, 17));
            imageSize.Add("DIR-CLOSE", new Size(28, 17));
            imageSize.Add("DIR-OPEN", new Size(28, 17));
            imageSize.Add("DIV-CLOSE", new Size(32, 17));
            imageSize.Add("DIV-OPEN", new Size(50, 17));
            imageSize.Add("DL-CLOSE", new Size(28, 17));
            imageSize.Add("DL-OPEN", new Size(28, 17));
            imageSize.Add("DT-CLOSE", new Size(28, 17));
            imageSize.Add("DT-OPEN", new Size(28, 17));
            imageSize.Add("EM-CLOSE", new Size(28, 17));
            imageSize.Add("EM-OPEN", new Size(28, 17));
            imageSize.Add("FONT-CLOSE", new Size(44, 17));
            imageSize.Add("FONT-OPEN", new Size(44, 17));
            imageSize.Add("FORM-CLOSE", new Size(40, 17));
            imageSize.Add("FORM-OPEN", new Size(62, 17));
            imageSize.Add("H1-CLOSE", new Size(28, 17));
            imageSize.Add("H1-OPEN", new Size(28, 17));
            imageSize.Add("H2-CLOSE", new Size(28, 17));
            imageSize.Add("H2-OPEN", new Size(28, 17));
            imageSize.Add("H3-CLOSE", new Size(28, 17));
            imageSize.Add("H3-OPEN", new Size(28, 17));
            imageSize.Add("H4-CLOSE", new Size(28, 17));
            imageSize.Add("H4-OPEN", new Size(28, 17));
            imageSize.Add("H5-CLOSE", new Size(28, 17));
            imageSize.Add("H5-OPEN", new Size(28, 17));
            imageSize.Add("H6-CLOSE", new Size(28, 17));
            imageSize.Add("H6-OPEN", new Size(28, 17));
            imageSize.Add("HR-OPEN", new Size(38, 17));
            imageSize.Add("I-CLOSE", new Size(22, 17));
            imageSize.Add("I-OPEN", new Size(22, 17));
            imageSize.Add("IMG", new Size(44, 17));
            imageSize.Add("KBD-CLOSE", new Size(28, 17));
            imageSize.Add("KBD-OPEN", new Size(28, 17));
            imageSize.Add("LI-CLOSE", new Size(20, 17));
            imageSize.Add("LI-OPEN", new Size(20, 17));
            imageSize.Add("MAP-CLOSE", new Size(40, 17));
            imageSize.Add("MAP-OPEN", new Size(62, 17));
            imageSize.Add("MENU-CLOSE", new Size(44, 17));
            imageSize.Add("MENU-OPEN", new Size(44, 17));
            imageSize.Add("OL-CLOSE", new Size(24, 17));
            imageSize.Add("OL-OPEN", new Size(38, 17));
            imageSize.Add("P-CLOSE", new Size(23, 17));
            imageSize.Add("P-OPEN", new Size(23, 17));
            imageSize.Add("PARAM-CLOSE", new Size(58, 17));
            imageSize.Add("PARAM-OPEN", new Size(58, 17));
            imageSize.Add("PRE-CLOSE", new Size(28, 17));
            imageSize.Add("PRE-OPEN", new Size(28, 17));
            imageSize.Add("SAMP-CLOSE", new Size(44, 17));
            imageSize.Add("SAMP-OPEN", new Size(44, 17));
            imageSize.Add("SCRIPT", new Size(19, 16));
            imageSize.Add("SELECT-CLOSE", new Size(62, 17));
            imageSize.Add("SELECT-OPEN", new Size(62, 17));
            imageSize.Add("SMALL-CLOSE", new Size(44, 17));
            imageSize.Add("SMALL-OPEN", new Size(44, 17));
            imageSize.Add("STRIKE-CLOSE", new Size(58, 17));
            imageSize.Add("STRIKE-OPEN", new Size(58, 17));
            imageSize.Add("STRONG-CLOSE", new Size(58, 17));
            imageSize.Add("STRONG-OPEN", new Size(58, 17));
            imageSize.Add("STYLE", new Size(22, 17));
            imageSize.Add("SPAN-CLOSE", new Size(40, 17));
            imageSize.Add("SPAN-OPEN", new Size(62, 17));
            imageSize.Add("SUB-CLOSE", new Size(28, 17));
            imageSize.Add("SUB-OPEN", new Size(28, 17));
            imageSize.Add("SUP-CLOSE", new Size(28, 17));
            imageSize.Add("SUP-OPEN", new Size(28, 17));
            imageSize.Add("TABLE", new Size(48, 17));
            imageSize.Add("TEXTAREA-CLOSE", new Size(62, 17));
            imageSize.Add("TEXTAREA-OPEN", new Size(62, 17));
            imageSize.Add("TT-CLOSE", new Size(28, 17));
            imageSize.Add("TT-OPEN", new Size(28, 17));
            imageSize.Add("U-CLOSE", new Size(22, 17));
            imageSize.Add("U-OPEN", new Size(22, 17));
            imageSize.Add("UL-CLOSE", new Size(28, 17));
            imageSize.Add("UL-OPEN", new Size(28, 17));
            imageSize.Add("VAR-CLOSE", new Size(28, 17));
            imageSize.Add("VAR-OPEN", new Size(28, 17));
            imageSize.Add("COMMENT-OPEN", new Size(24, 16));
            // Colored
            imageSize.Add("C-A-CLOSE", new Size(17, 18));
            imageSize.Add("C-A-OPEN", new Size(17, 18));
            imageSize.Add("C-ADDRESS-CLOSE", new Size(60, 18));
            imageSize.Add("C-ADDRESS-OPEN", new Size(60, 18));
            imageSize.Add("C-APPLET-CLOSE", new Size(60, 18));
            imageSize.Add("C-APPLET-OPEN", new Size(60, 18));
            imageSize.Add("C-B-CLOSE", new Size(17, 18));
            imageSize.Add("C-B-OPEN", new Size(17, 18));
            imageSize.Add("C-BIG-CLOSE", new Size(40, 18));
            imageSize.Add("C-BIG-OPEN", new Size(40, 18));
            imageSize.Add("C-BLOCKQUOTE-CLOSE", new Size(82, 18));
            imageSize.Add("C-BLOCKQUOTE-OPEN", new Size(82, 18));
            imageSize.Add("C-BR", new Size(35, 18));
            imageSize.Add("C-CAPTION-CLOSE", new Size(60, 18));
            imageSize.Add("C-CAPTION-OPEN", new Size(60, 18));
            imageSize.Add("C-CENTER-CLOSE", new Size(60, 18));
            imageSize.Add("C-CENTER-OPEN", new Size(60, 18));
            imageSize.Add("C-CITE-CLOSE", new Size(40, 18));
            imageSize.Add("C-CITE-OPEN", new Size(40, 18));
            imageSize.Add("C-CODE-CLOSE", new Size(40, 18));
            imageSize.Add("C-CODE-OPEN", new Size(40, 18));
            imageSize.Add("C-DD-CLOSE", new Size(28, 18));
            imageSize.Add("C-DD-OPEN", new Size(28, 18));
            imageSize.Add("C-DFN-CLOSE", new Size(40, 18));
            imageSize.Add("C-DFN-OPEN", new Size(40, 18));
            imageSize.Add("C-DIR-CLOSE", new Size(40, 18));
            imageSize.Add("C-DIR-OPEN", new Size(40, 18));
            imageSize.Add("C-DIV-CLOSE", new Size(40, 18));
            imageSize.Add("C-DIV-OPEN", new Size(40, 18));
            imageSize.Add("C-DL-CLOSE", new Size(28, 18));
            imageSize.Add("C-DL-OPEN", new Size(28, 18));
            imageSize.Add("C-DT-CLOSE", new Size(28, 18));
            imageSize.Add("C-DT-OPEN", new Size(28, 18));
            imageSize.Add("C-EM-CLOSE", new Size(28, 18));
            imageSize.Add("C-EM-OPEN", new Size(28, 18));
            imageSize.Add("C-FONT-CLOSE", new Size(40, 18));
            imageSize.Add("C-FONT-OPEN", new Size(40, 18));
            imageSize.Add("C-FORM-CLOSE", new Size(40, 18));
            imageSize.Add("C-FORM-OPEN", new Size(40, 18));
            imageSize.Add("C-H1-CLOSE", new Size(28, 18));
            imageSize.Add("C-H1-OPEN", new Size(28, 18));
            imageSize.Add("C-H2-CLOSE", new Size(28, 18));
            imageSize.Add("C-H2-OPEN", new Size(28, 18));
            imageSize.Add("C-H3-CLOSE", new Size(28, 18));
            imageSize.Add("C-H3-OPEN", new Size(28, 18));
            imageSize.Add("C-H4-CLOSE", new Size(28, 18));
            imageSize.Add("C-H4-OPEN", new Size(28, 18));
            imageSize.Add("C-H5-CLOSE", new Size(28, 18));
            imageSize.Add("C-H5-OPEN", new Size(28, 18));
            imageSize.Add("C-H6-CLOSE", new Size(28, 18));
            imageSize.Add("C-H6-OPEN", new Size(28, 18));
            imageSize.Add("C-HR-OPEN", new Size(28, 18));
            imageSize.Add("C-I-CLOSE", new Size(17, 18));
            imageSize.Add("C-I-OPEN", new Size(17, 18));
            imageSize.Add("C-IMG", new Size(40, 18));
            imageSize.Add("C-KBD-CLOSE", new Size(40, 18));
            imageSize.Add("C-KBD-OPEN", new Size(40, 18));
            imageSize.Add("C-LI-CLOSE", new Size(28, 18));
            imageSize.Add("C-LI-OPEN", new Size(28, 18));
            imageSize.Add("C-MAP-CLOSE", new Size(40, 18));
            imageSize.Add("C-MAP-OPEN", new Size(40, 18));
            imageSize.Add("C-MENU-CLOSE", new Size(60, 18));
            imageSize.Add("C-MENU-OPEN", new Size(60, 18));
            imageSize.Add("C-OL-CLOSE", new Size(28, 18));
            imageSize.Add("C-OL-OPEN", new Size(28, 18));
            imageSize.Add("C-P-CLOSE", new Size(17, 18));
            imageSize.Add("C-P-OPEN", new Size(17, 18));
            imageSize.Add("C-PARAM-CLOSE", new Size(60, 18));
            imageSize.Add("C-PARAM-OPEN", new Size(60, 18));
            imageSize.Add("C-PRE-CLOSE", new Size(40, 18));
            imageSize.Add("C-PRE-OPEN", new Size(40, 18));
            imageSize.Add("C-SAMP-CLOSE", new Size(40, 18));
            imageSize.Add("C-SAMP-OPEN", new Size(40, 18));
            imageSize.Add("C-SCRIPT", new Size(60, 16));
            imageSize.Add("C-SELECT-CLOSE", new Size(60, 18));
            imageSize.Add("C-SELECT-OPEN", new Size(60, 18));
            imageSize.Add("C-SMALL-CLOSE", new Size(60, 18));
            imageSize.Add("C-SMALL-OPEN", new Size(60, 18));
            imageSize.Add("C-STRIKE-CLOSE", new Size(60, 18));
            imageSize.Add("C-STRIKE-OPEN", new Size(60, 18));
            imageSize.Add("C-STRONG-CLOSE", new Size(60, 18));
            imageSize.Add("C-STRONG-OPEN", new Size(60, 18));
            imageSize.Add("C-STYLE", new Size(60, 18));
            imageSize.Add("C-SPAN-CLOSE", new Size(40, 18));
            imageSize.Add("C-SPAN-OPEN", new Size(60, 18));
            imageSize.Add("C-SUB-CLOSE", new Size(40, 18));
            imageSize.Add("C-SUB-OPEN", new Size(40, 18));
            imageSize.Add("C-SUP-CLOSE", new Size(40, 18));
            imageSize.Add("C-SUP-OPEN", new Size(40, 18));
            imageSize.Add("C-TABLE", new Size(60, 18));
            imageSize.Add("C-TEXTAREA-CLOSE", new Size(82, 18));
            imageSize.Add("C-TEXTAREA-OPEN", new Size(82, 18));
            imageSize.Add("C-TT-CLOSE", new Size(28, 18));
            imageSize.Add("C-TT-OPEN", new Size(28, 18));
            imageSize.Add("C-U-CLOSE", new Size(17, 18));
            imageSize.Add("C-U-OPEN", new Size(17, 18));
            imageSize.Add("C-UL-CLOSE", new Size(28, 18));
            imageSize.Add("C-UL-OPEN", new Size(28, 18));
            imageSize.Add("C-VAR-CLOSE", new Size(40, 18));
            imageSize.Add("C-VAR-OPEN", new Size(40, 18));
            imageSize.Add("C-COMMENT-OPEN", new Size(40, 16));
        }


    }

}
