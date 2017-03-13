// it is necessary to save this file as unicode to preserve the embedded entities
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using GuruComponents.Netrix.HtmlFormatting.Elements;

namespace GuruComponents.Netrix.HtmlFormatting
{
    /// <summary>
    /// The HtmlFormatter formats a HTML stream into XHTML format and indent the element in a functional
    /// related way, e.g. it recognizes tables and can distinguish between inline and block tags.
    /// </summary>
    public sealed class HtmlFormatter : IHtmlFormatter
    {
        private static IDictionary tagTable = new HybridDictionary(true);
        // custom element table

        private static TagInfo commentTag = new TagInfo("", FormattingFlags.Comment | FormattingFlags.NoEndTag, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, ElementType.Any);
        private static TagInfo directiveTag = new TagInfo("", FormattingFlags.NoEndTag, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
        private static TagInfo otherServerSideScriptTag = new TagInfo("", FormattingFlags.Inline | FormattingFlags.NoEndTag, ElementType.Any);
        private static TagInfo nestedXmlTag = new TagInfo("", FormattingFlags.AllowPartialTags, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
        private static TagInfo unknownXmlTag = new TagInfo("", FormattingFlags.Xml, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
        private static TagInfo unknownHtmlTag = new TagInfo("", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
        private static string[] tagList;
        private static Hashtable EntityInfo = new Hashtable();

        private static readonly Regex entRegx = new Regex("&([^;])+;", RegexOptions.Compiled);
        private static readonly Regex rxPFake = new Regex(@"<(?<tago>P|DIV)(?<attr>.*)>( )?</(?<tagc>P|DIV)>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static MatchEvaluator entEval;
        private static MatchEvaluator entAntiEval;


        private static GuruComponents.Netrix.UserInterface.StyleParser.Parser p = new UserInterface.StyleParser.Parser();
        private static GuruComponents.Netrix.UserInterface.StyleParser.CssParser cp = new UserInterface.StyleParser.CssParser();
        
        /// <summary>
        /// Contains information about how to format a specific tag.
        /// </summary>
        internal class FormatInfo
        {
            /// <summary>
            /// Indentation
            /// </summary>
            public int indent;
            public bool isEndTag;
            public ITagInfo tagInfo;

            /// <summary>
            /// Currently after a beginning tag
            /// </summary>
            public bool isBeginTag
            {
                get
                {
                    return !this.isEndTag;
                }

            }

            /// <summary>
            /// Register the type of formatting.
            /// </summary>
            /// <param name="info">Tag information</param>
            /// <param name="isEnd">True if it is an end tag.</param>
            public FormatInfo(ITagInfo info, bool isEnd)
            {
                this.tagInfo = info;
                this.isEndTag = isEnd;
                return;
            }
        }


        /// <summary>
        /// This method adds a new TagInfo object to the collection of TagInfos. These collection is used to format
        /// any registered tag correctly. All tag known in HTML 4.0 are predefined. The Plug-In module uses this method to 
        /// enhance the know tags.
        /// </summary>
        /// <param name="ti"></param>
        public void AddCustomElement(ITagInfo ti)
        {
            tagTable[ti.TagName] = ti;
        }

        /// <summary>
        /// This static constructor builds the replacement table for entities and defines the type of HTML tags that the
        /// formatter recognizes to build a functional related tree for output.
        /// </summary>
        static HtmlFormatter()
        {
            // HTML 4.0
            tagTable["a"]           = new TagInfo("a", FormattingFlags.Inline, ElementType.Inline);
            tagTable["acronym"]     = new TagInfo("acronym", FormattingFlags.Inline, ElementType.Inline);
            tagTable["address"]     = new TagInfo("address", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["applet"]      = new TagInfo("applet", FormattingFlags.Inline, WhiteSpaceType.CarryThrough, WhiteSpaceType.Significant, ElementType.Block);
            tagTable["area"]        = new TagInfo("area", FormattingFlags.NoEndTag);
            tagTable["b"]           = new TagInfo("b", FormattingFlags.Inline, ElementType.Inline);
            tagTable["base"]        = new TagInfo("base", FormattingFlags.NoEndTag);
            tagTable["basefont"]    = new TagInfo("basefont", FormattingFlags.NoEndTag, ElementType.Block);
            tagTable["bdo"]         = new TagInfo("bdo", FormattingFlags.Inline, ElementType.Inline);
            tagTable["bgsound"]     = new TagInfo("bgsound", FormattingFlags.NoEndTag);
            tagTable["big"]         = new TagInfo("big", FormattingFlags.Inline, ElementType.Inline);
            tagTable["blink"]       = new TagInfo("blink", FormattingFlags.Inline);
            tagTable["blockquote"]  = new TagInfo("blockquote", FormattingFlags.Inline, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["body"]        = new TagInfo("body", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["br"]          = new TagInfo("br", FormattingFlags.NoEndTag, WhiteSpaceType.NotSignificant, WhiteSpaceType.Significant, ElementType.Inline);
            tagTable["button"]      = new TagInfo("button", FormattingFlags.Inline, ElementType.Inline);
            tagTable["caption"]     = new TagInfo("caption", FormattingFlags.None);
            tagTable["cite"]        = new TagInfo("cite", FormattingFlags.Inline, ElementType.Inline);
            tagTable["center"] = new TagInfo("center", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["code"] = new TagInfo("code", FormattingFlags.Inline, ElementType.Inline);
            tagTable["col"] = new TagInfo("col", FormattingFlags.NoEndTag);
            tagTable["colgroup"] = new TagInfo("colgroup", FormattingFlags.None);
            tagTable["dd"] = new TagInfo("dd", FormattingFlags.None);
            tagTable["del"] = new TagInfo("del", FormattingFlags.None);
            tagTable["dfn"] = new TagInfo("dfn", FormattingFlags.Inline, ElementType.Inline);
            tagTable["dir"] = new TagInfo("dir", FormattingFlags.None, ElementType.Block);
            tagTable["div"] = new TagInfo("div", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["dl"] = new TagInfo("dl", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["dt"] = new TagInfo("dt", FormattingFlags.Inline);
            tagTable["em"] = new TagInfo("em", FormattingFlags.Inline, ElementType.Inline);
            tagTable["embed"] = new TagInfo("embed", FormattingFlags.Inline, WhiteSpaceType.Significant, WhiteSpaceType.CarryThrough, ElementType.Inline);
            tagTable["fieldset"] = new TagInfo("fieldset", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["font"] = new TagInfo("font", FormattingFlags.Inline, ElementType.Inline);
            tagTable["form"] = new TagInfo("form", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["frame"] = new TagInfo("frame", FormattingFlags.NoEndTag);
            tagTable["frameset"] = new TagInfo("frameset", FormattingFlags.None);
            tagTable["head"] = new TagInfo("head", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant);
            tagTable["h1"] = new TagInfo("h1", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["h2"] = new TagInfo("h2", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["h3"] = new TagInfo("h3", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["h4"] = new TagInfo("h4", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["h5"] = new TagInfo("h5", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["h6"] = new TagInfo("h6", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["hr"] = new TagInfo("hr", FormattingFlags.NoEndTag, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["html"] = new TagInfo("html", FormattingFlags.NoIndent, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant);
            tagTable["i"] = new TagInfo("i", FormattingFlags.Inline, ElementType.Inline);
            tagTable["iframe"] = new TagInfo("iframe", FormattingFlags.None, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Inline);
            tagTable["img"] = new TagInfo("img", FormattingFlags.Inline | FormattingFlags.NoEndTag, WhiteSpaceType.Significant, WhiteSpaceType.Significant, ElementType.Inline);
            tagTable["input"] = new TagInfo("input", FormattingFlags.NoEndTag, WhiteSpaceType.Significant, WhiteSpaceType.Significant, ElementType.Inline);
            tagTable["ins"] = new TagInfo("ins", FormattingFlags.None);
            tagTable["isindex"] = new TagInfo("isindex", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.CarryThrough, ElementType.Block);
            tagTable["kbd"] = new TagInfo("kbd", FormattingFlags.Inline, ElementType.Inline);
            tagTable["label"] = new TagInfo("label", FormattingFlags.Inline, ElementType.Inline);
            tagTable["legend"] = new TagInfo("legend", FormattingFlags.None);
            tagTable["li"] = new LITagInfo();
            tagTable["link"] = new TagInfo("link", FormattingFlags.NoEndTag);
            tagTable["listing"] = new TagInfo("listing", FormattingFlags.None, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["map"] = new TagInfo("map", FormattingFlags.Inline, ElementType.Inline);
            tagTable["marquee"] = new TagInfo("marquee", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["menu"] = new TagInfo("menu", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["meta"] = new TagInfo("meta", FormattingFlags.NoEndTag);
            tagTable["nobr"] = new TagInfo("nobr", FormattingFlags.Inline | FormattingFlags.NoEndTag, ElementType.Inline);
            tagTable["noembed"] = new TagInfo("noembed", FormattingFlags.None, ElementType.Block);
            tagTable["noframes"] = new TagInfo("noframes", FormattingFlags.None, ElementType.Block);
            tagTable["noscript"] = new TagInfo("noscript", FormattingFlags.None, ElementType.Block);
            tagTable["object"] = new TagInfo("object", FormattingFlags.Inline, WhiteSpaceType.CarryThrough, WhiteSpaceType.Significant, ElementType.Block);
            tagTable["ol"] = new TagInfo("ol", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["option"] = new TagInfo("option", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.CarryThrough);
            tagTable["optgroup"] = new TagInfo("optgroup", FormattingFlags.None, WhiteSpaceType.Significant, WhiteSpaceType.CarryThrough, ElementType.Block);
            tagTable["p"] = new PTagInfo();
            tagTable["param"] = new TagInfo("param", FormattingFlags.NoEndTag);
            tagTable["pre"] = new TagInfo("pre", FormattingFlags.PreserveContent, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["q"] = new TagInfo("q", FormattingFlags.Inline, ElementType.Inline);
            tagTable["rt"] = new TagInfo("rt", FormattingFlags.None);
            tagTable["ruby"] = new TagInfo("ruby", FormattingFlags.None, ElementType.Inline);
            tagTable["s"] = new TagInfo("s", FormattingFlags.Inline, ElementType.Inline);
            tagTable["samp"] = new TagInfo("samp", FormattingFlags.None, ElementType.Inline);
            tagTable["script"] = new TagInfo("script", FormattingFlags.PreserveContent, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, ElementType.Inline);
            tagTable["select"] = new TagInfo("select", FormattingFlags.None, WhiteSpaceType.CarryThrough, WhiteSpaceType.Significant, ElementType.Block);
            tagTable["small"] = new TagInfo("small", FormattingFlags.Inline, ElementType.Inline);
            tagTable["span"] = new TagInfo("span", FormattingFlags.Inline, ElementType.Inline);
            tagTable["strike"] = new TagInfo("strike", FormattingFlags.Inline, ElementType.Inline);
            tagTable["strong"]              = new TagInfo("strong",     FormattingFlags.Inline, ElementType.Inline);
            tagTable["style"]               = new TagInfo("style",      FormattingFlags.PreserveContent, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
            tagTable["sub"]                 = new TagInfo("sub",        FormattingFlags.Inline, ElementType.Inline);
            tagTable["sup"]                 = new TagInfo("sup",        FormattingFlags.Inline, ElementType.Inline);
            tagTable["table"]               = new TagInfo("table",      FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["tbody"]               = new TagInfo("tbody",      FormattingFlags.None);
            tagTable["td"]                  = new TDTagInfo();
            tagTable["textarea"]            = new TagInfo("textarea",   FormattingFlags.PreserveContent, WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["tfoot"]               = new TagInfo("tfoot",      FormattingFlags.None);
            tagTable["th"]                  = new TagInfo("th",         FormattingFlags.None);
            tagTable["thead"]               = new TagInfo("thead",      FormattingFlags.None);
            tagTable["title"]               = new TagInfo("title",      FormattingFlags.Inline, ElementType.Block);
            tagTable["tr"]                  = new TRTagInfo();
            tagTable["tt"]                  = new TagInfo("tt",         FormattingFlags.Inline, ElementType.Inline);
            tagTable["u"]                   = new TagInfo("u",          FormattingFlags.Inline, ElementType.Inline);
            tagTable["ul"]                  = new TagInfo("ul",         FormattingFlags.None,               WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["xml"]                 = new TagInfo("xml",        FormattingFlags.Xml,                WhiteSpaceType.Significant, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["xmp"]                 = new TagInfo("xmp",        FormattingFlags.PreserveContent,    WhiteSpaceType.CarryThrough, WhiteSpaceType.NotSignificant, ElementType.Block);
            tagTable["var"]                 = new TagInfo("var",        FormattingFlags.Inline,                                                     ElementType.Inline);
            tagTable["wbr"]                 = new TagInfo("wbr",        FormattingFlags.Inline | FormattingFlags.NoEndTag, ElementType.Inline);
            
            // TagList for Word Cleanup
            tagList = new string[tagTable.Count];
            tagTable.Keys.CopyTo(tagList, 0);
                       
            // Entities
            entEval = new MatchEvaluator(Match);
            entAntiEval = new MatchEvaluator(AntiMatch);

            EntityInfo.Add(128, "&euro;"); // €;
            EntityInfo.Add(129, ""); // 
            EntityInfo.Add(130, "&lsquor;"); // &lsquor;
            EntityInfo.Add(131, "&fnof;"); //    --> ƒ florin
            EntityInfo.Add(132, "&ldquor;"); //   --> &ldquor;
            EntityInfo.Add(133, "&hellip;"); //   --> … &ldots;    --> &ldots;
            EntityInfo.Add(134, "&dagger;"); //   --> †
            EntityInfo.Add(135, "&Dagger;"); //   --> ‡
            EntityInfo.Add(136, "");  //      ¹
            EntityInfo.Add(137, "&permil;"); //;   --> ‰
            EntityInfo.Add(138, "&Scaron;"); //;   --> Š
            EntityInfo.Add(139, "&lsaquo;"); //;   --> ‹ (guillemet)
            EntityInfo.Add(140, "&OElig;"); //;    --> Œ
            EntityInfo.Add(141, ""); //
            EntityInfo.Add(142, ""); //
            EntityInfo.Add(143, ""); //
            EntityInfo.Add(144, ""); //
            EntityInfo.Add(145, "&lsquo;"); //    --> ‘ high right rising single quote                        &rsquor;   --> &rsquor;
            EntityInfo.Add(146, "&rsquo;"); //    --> ’
            EntityInfo.Add(147, "&ldquo;"); //    --> “ high right rising double quote                        &rdquor;   --> &rdquor;
            EntityInfo.Add(148, "&rdquo;"); //    --> ”
            EntityInfo.Add(149, "&bull;"); //     --> •
            EntityInfo.Add(150, "&ndash;"); //    --> – endash;   --> &endash;
            EntityInfo.Add(151, "&mdash;"); //    --> — &emdash;   --> &emdash;
            EntityInfo.Add(152, "&tilde;"); //¹
            EntityInfo.Add(153, "&trade;"); //    --> ™
            EntityInfo.Add(154, "&scaron;"); //   --> š
            EntityInfo.Add(155, "&rsaquo;"); //   --> › (guillemet)
            EntityInfo.Add(156, "&oelig;"); //    --> œ
            EntityInfo.Add(157, "");
            EntityInfo.Add(158, "");
            EntityInfo.Add(159, "&Yuml;"); //     --> Ÿ
            EntityInfo.Add(160, "&nbsp;");
            EntityInfo.Add(161, "&iexcl;");
            EntityInfo.Add(162, "&cent;");
            EntityInfo.Add(163, "&pound;");
            EntityInfo.Add(164, "&curren;");
            EntityInfo.Add(165, "&yen;");
            EntityInfo.Add(166, "&brvbar;");
            EntityInfo.Add(167, "&sect;");
            EntityInfo.Add(168, "&uml;");
            EntityInfo.Add(169, "&copy;");
            EntityInfo.Add(170, "&ordf;");
            EntityInfo.Add(171, "&laquo;");
            EntityInfo.Add(172, "&not;");
            EntityInfo.Add(173, "&shy;");
            EntityInfo.Add(174, "&reg;");
            EntityInfo.Add(175, "&macr;");
            EntityInfo.Add(176, "&deg;");
            EntityInfo.Add(177, "&plusmn;");
            EntityInfo.Add(178, "&sup2;");
            EntityInfo.Add(179, "&sup3;");
            EntityInfo.Add(180, "&acute;");
            EntityInfo.Add(181, "&micro;");
            EntityInfo.Add(182, "&para;");
            EntityInfo.Add(183, "&middot;");
            EntityInfo.Add(184, "&cedil;");
            EntityInfo.Add(185, "&sup1;");
            EntityInfo.Add(186, "&ordm;");
            EntityInfo.Add(187, "&raquo;");
            EntityInfo.Add(188, "&frac14;");
            EntityInfo.Add(189, "&frac12;");
            EntityInfo.Add(190, "&frac34;"); // 190 1 ¾ Y Y Y Y Y Y Y Y U Y Y U Y U N N");
            EntityInfo.Add(191, "&iquest;"); // 191 0 ¿ Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(192, "&Agrave;"); // 192 0 À Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(193, "&Aacute;"); // 193 0 Á Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(194, "&Acirc;"); // 194 0 Â Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(195, "&Atilde;"); // 195 0 Ã Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(196, "&Auml;"); // 196 0 Ä Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(197, "&Aring;"); // 197 0 Å Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(198, "&AElig;"); // 198 0 Æ Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(199, "&Ccedil;"); // 199 0 Ç Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(200, "&Egrave;"); // 200 0 È Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(201, "&Eacute;"); // 201 0 É Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(202, "&Ecirc;"); // 202 0 Ê Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(203, "&Euml;"); // 203 0 Ë Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(204, "&Igrave;"); // 204 0 Ì Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(205, "&Iacute;"); // 205 0 Í Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(206, "&Icirc;"); // 206 0 Î Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(207, "&Iuml;"); // 207 0 Ï Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(208, "&ETH;"); // 208 0 Ð Y Y Y Y Y Y Y Y U Y Y U Y U N N");
            EntityInfo.Add(209, "&Ntilde;"); // 209 0 Ñ Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(210, "&Ograve;"); // 210 0 Ò Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(211, "&Oacute;"); // 211 0 Ó Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(212, "&Ocirc;"); // 212 0 Ô Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(213, "&Otilde;"); // 213 0 Õ Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(214, "&Ouml;"); // 214 0 Ö Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(215, "&times;"); // 215 1 × Y Y Y Y Y Y Y Y U Y Y U Y U N N");
            EntityInfo.Add(216, "&Oslash;"); // 216 0 Ø Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(217, "&Ugrave;"); // 217 0 Ù Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(218, "&Uacute;"); // 218 0 Ú Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(219, "&Ucirc;"); // 219 0 Û Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(220, "&Uuml;"); // 220 0 Ü Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(221, "&Yacute;"); // 221 0 Ý Y Y Y Y Y Y Y Y U Y Y U Y U N N");
            EntityInfo.Add(222, "&THORN;"); // 222 0 Þ Y Y Y Y Y Y Y Y U Y Y U Y U N N");
            EntityInfo.Add(223, "&szlig;"); // 223 0 ß Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(224, "&agrave;"); // 224 0 à Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(225, "&aacute;"); // 225 0 á Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(226, "&acirc;"); // 226 0 â Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(227, "&atilde;"); // 227 0 ã Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(228, "&auml;"); // 228 0 ä Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(229, "&aring;"); // 229 0 å Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(230, "&aelig;"); // 230 0 æ Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(231, "&ccedil;"); // 231 0 ç Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(232, "&egrave;"); // 232 0 è Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(233, "&eacute;"); // 233 0 é Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(234, "&ecirc;"); // 234 0 ê Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(235, "&euml;"); // 235 0 ë Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(236, "&igrave;"); // 236 0 ì Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(237, "&iacute;"); // 237 0 í Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(238, "&icirc;"); // 238 0 î Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(239, "&iuml;"); // 239 0 ï Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(240, "&eth;"); // 240 0 ð Y Y Y Y Y Y Y Y U Y Y U Y U N N");
            EntityInfo.Add(241, "&ntilde;"); // 241 0 ñ Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(242, "&ograve;"); // 242 0 ò Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(243, "&oacute;"); // 243 0 ó Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(244, "&ocirc;"); // 244 0 ô Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(245, "&otilde;"); // 245 0 õ Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(246, "&ouml;"); // 246 0 ö Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(247, "&divide;"); // 247 2 ÷ Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(248, "&oslash;"); // 248 2 ø Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(249, "&ugrave;"); // 249 0 ù Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(250, "&uacute;"); // 250 0 ú Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(251, "&ucirc;"); // 251 0 û Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(252, "&uuml;"); // 252 0 ü Y Y Y Y Y Y Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(253, "&yacute;"); // 253 0 ý Y Y Y Y Y Y Y Y U Y Y U Y U N N");
            EntityInfo.Add(254, "&thorn;"); // 254 0 þ Y Y Y Y Y Y Y Y U Y Y U Y U N N");
            EntityInfo.Add(255, "&yuml;"); // 255 0 ÿ Y Y Y Y Y Y Y Y U Y Y U Y U N N");
            EntityInfo.Add(338, "&OElig;"); // 338 0 Œ N Y Y Y Y N Y Y U N Y U Y U Y Y");
            EntityInfo.Add(339, "&oelig;"); // 339 0 œ N Y Y Y Y N Y Y U N Y U Y U Y Y");
            EntityInfo.Add(352, "&Scaron;"); // 352 0 Š N Y Y Y Y N Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(353, "&scaron;"); // 353 0 š N Y Y Y Y N Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(376, "&Yuml;"); // 376 0 Ÿ N Y Y Y Y N Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(402, "&fnof;"); // 402 2 ƒ N Y Y Y Y N Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(710, "&circ;"); // 710 1 ˆ N Y Y Y Y N Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(732, "&tilde;"); //"); // 732 1 ˜ N Y Y Y Y N Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(913, "&Alpha;"); // 913 3 Α N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(914, "&Beta;"); // 914 3 Β N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(915, "&Gamma;"); // 915 3 Γ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(916, "&Delta;"); // 916 3 Δ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(917, "&Epsilon;"); // 917 3 Ε N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(918, "&Zeta;"); // 918 3 Ζ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(919, "&Eta;"); // 919 3 Η N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(920, "&Theta;"); // 920 3 Θ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(921, "&Iota;"); // 921 3 Ι N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(922, "&Kappa;"); // 922 3 Κ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(923, "&Lambda;"); // 923 3 Λ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(924, "&Mu;"); // 924 3 Μ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(925, "&Nu;"); // 925 3 Ν N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(926, "&Xi;"); // 926 3 Ξ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(927, "&Omicron;"); // 927 3 Ο N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(928, "&Pi;"); // 928 3 Π N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(929, "&Rho;"); // 929 3 Ρ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(931, "&Sigma;"); // 931 3 Σ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(932, "&Tau;"); // 932 3 Τ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(933, "&Upsilon;"); // 933 3 Υ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(934, "&Phi;"); // 934 3 Φ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(935, "&Chi;"); // 935 3 Χ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(936, "&Psi;"); // 936 3 Ψ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(937, "&Omega;"); // 937 3 Ω N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(945, "&alpha;"); // 945 3 α N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(946, "&beta;"); // 946 3 β N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(947, "&gamma;"); // 947 3 γ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(948, "&delta;"); // 948 3 δ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(949, "&epsilon;"); // 949 3 ε N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(950, "&zeta;"); // 950 3 ζ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(951, "&eta;"); // 951 3 η N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(952, "&theta;"); // 952 3 θ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(953, "&iota;"); // 953 3 ι N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(954, "&kappa;"); // 954 3 κ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(955, "&lambda;"); // 955 3 λ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(956, "&mu;"); // 956 3 μ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(957, "&nu;"); // 957 3 ν N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(958, "&xi;"); // 958 3 ξ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(959, "&omicron;"); // 959 3 ο N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(960, "&pi;"); // 960 3 π N Y Y Y Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(961, "&rho;"); // 961 3 ρ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(962, "&sigmaf;"); // 962 3 ς N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(963, "&sigma;"); // 963 3 σ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(964, "&tau;"); // 964 3 τ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(965, "&upsilon;"); // 965 3 υ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(966, "&phi;"); // 966 3 φ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(967, "&chi;"); // 967 3 χ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(968, "&psi;"); // 968 3 ψ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(969, "&omega;"); // 969 3 ω N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(977, "&thetasym;"); // 977 1 ϑ N Y N N N N N Y U N N U N U N N");
            EntityInfo.Add(978, "&upsih;"); // 978 1 ϒ N Y N N N N N Y U N N U N U N N");
            EntityInfo.Add(982, "&piv;"); // 982 1 ϖ N Y N N N N N Y U N N U N U N N");
            EntityInfo.Add(8226, "&bull;"); // 8226 1 • N Y Y Y Y N Y Y U P Y U Y U Y Y");   
            EntityInfo.Add(8230, "&hellip;"); // 8230 1 … N Y Y Y Y N Y Y U N Y U Y U Y Y");
            EntityInfo.Add(8242, "&prime;"); // 8242 1 ′ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8243, "&Prime;"); // 8243 1 ″ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8254, "&oline;"); // 8254 1 ‾ N Y Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8260, "&frasl;"); // 8260 1 ⁄ N Y Y Y Y N N Y U Y Y U Y U Y Y");
            EntityInfo.Add(8472, "&weierp;"); // 8472 2 ℘ N N N N N N N Y U Y Y U Y U N N");
            EntityInfo.Add(8465, "&image;"); // 8465 2 ℑ N N N N N N N Y U Y Y U Y U N N");
            EntityInfo.Add(8476, "&real;"); // 8476 2 ℜ N N N N N N N Y U Y Y U Y U N N");
            EntityInfo.Add(8482, "&trade;"); // 8482 1 ™ P Y Y Y Y N Y Y U Y Y U Y U Y Y");  
            EntityInfo.Add(8501, "&alefsym;"); // 8501 2 ℵ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8592, "&larr;"); // 8592 4 ← N N Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8593, "&uarr;"); // 8593 4 ↑ N N Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8594, "&rarr;"); // 8594 4 → N N Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8595, "&darr;"); // 8595 4 ↓ N N Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8596, "&harr;"); // 8596 4 ↔ N N Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8629, "&crarr;"); // 8629 4 ↵ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8656, "&lArr;"); // 8656 4 ⇐ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8657, "&uArr;"); // 8657 4 ⇑ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8658, "&rArr;"); // 8658 4 ⇒ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8659, "&dArr;"); // 8659 4 ⇓ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8660, "&hArr;"); // 8660 4 ⇔ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8704, "&forall;"); // 8704 2 ∀ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8706, "&part;"); // 8706 2 ∂ N Y Y Y Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8707, "&exist;"); // 8707 2 ∃ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8709, "&empty;"); // 8709 2 ∅ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8711, "&nabla;"); // 8711 2 ∇ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8712, "&isin;"); // 8712 2 ∈ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8713, "&notin;"); // 8713 2 ∉ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8715, "&ni;"); // 8715 2 ∋ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8719, "&prod;"); // 8719 2 ∏ N Y Y Y Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8721, "&sum;"); // 8721 2 ∑ N Y Y Y Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8722, "&minus;"); // 8722 2 − N Y Y Y Y N N Y U Y Y U Y U Y Y");
            EntityInfo.Add(8727, "&lowast;"); // 8727 2 ∗ N N N N N N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8730, "&radic;"); // 8730 2 √ N Y Y Y Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8733, "&prop;"); // 8733 2 ∝ N N N N Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8734, "&infin;"); // 8734 2 ∞ N Y Y Y Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8736, "&ang;"); // 8736 2 ∠ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8743, "&and;"); // 8743 2 ∧ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8744, "&or;"); // 8744 2 ∨ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8745, "&cap;"); // 8745 2 ∩ N N Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8746, "&cup;"); // 8746 2 ∪ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8747, "&int;"); // 8747 2 ∫ N Y Y Y Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8756, "&there4;"); // 8756 2 ∴ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8764, "&sim;"); // 8764 2 ∼ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8773, "&cong;"); // 8773 2 ≅ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8776, "&asymp;"); // 8776 2 ≈ N Y Y Y Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8800, "&ne;"); // 8800 2 ≠ N Y Y N N N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8801, "&equiv;"); // 8801 2 ≡ N N Y N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8804, "&le;"); // 8804 2 ≤ N Y Y Y Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8805, "&ge;"); // 8805 2 ≥ N Y Y Y Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(8834, "&sub;"); // 8834 2 ⊂ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8835, "&sup;"); // 8835 2 ⊃ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8836, "&nsub;"); // 8836 2 ⊄ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8838, "&sube;"); // 8838 2 ⊆ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8839, "&supe;"); // 8839 2 ⊇ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8853, "&oplus;"); // 8853 2 ⊕ N N N N Y N N Y U N Y U Y U N N");
            EntityInfo.Add(8855, "&otimes;"); // 8855 2 ⊗ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8869, "&perp;"); // 8869 2 ⊥ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8901, "&sdot;"); // 8901 2 ⋅ N N N N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8968, "&lceil;"); // 8968 1 ⌈ N N N N N N N Y U N Y U N U N N");
            EntityInfo.Add(8969, "&rceil;"); // 8969 1 ⌉ N N N N N N N Y U N Y U N U N N");
            EntityInfo.Add(8970, "&lfloor;"); // 8970 1 ⌊ N N N N N N N Y U N Y U N U N N");
            EntityInfo.Add(8971, "&rfloor;"); // 8971 1 ⌋ N N N N N N N Y U N Y U N U N N");
            EntityInfo.Add(9001, "&lang;"); // 9001 4 〈 N N N N N N N Y U N N U Y U N N");   
            EntityInfo.Add(9002, "&rang;"); // 9002 4 〉 N N N N N N N Y U N N U Y U N N");   
            EntityInfo.Add(9674, "&loz;"); // 9674 1 ◊ N Y Y Y Y N N Y U N Y U Y U Y Y");
            EntityInfo.Add(9824, "&spades;"); // 9824 1 ♠ N N Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(9827, "&clubs;"); // 9827 1 ♣ N N Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(9829, "&hearts;"); // 9829 1 ♥ N N Y Y Y N N Y U N Y U Y U N N");
            EntityInfo.Add(9830, "&diams;"); // 9830 1 ♦ N N Y N N N N Y U N Y U Y U N N");
            EntityInfo.Add(8194, "&ensp;"); // 8194 1   N N N N N N N Y U N N U Y U N N");
            EntityInfo.Add(8195, "&emsp;"); // 8195 1   N N N N N N N Y U N N U Y U N N");
            EntityInfo.Add(8201, "&thinsp;"); // 8201 1   N N N N N N N Y U N N U Y U N N");
            EntityInfo.Add(8204, "&zwnj;"); // 8204 1 ‌ N N N N N N N Y U N N U N U N N");
            EntityInfo.Add(8205, "&zwj;"); // 8205 1 ‍ N N N N N N N Y U N N U N U N N");
            EntityInfo.Add(8206, "&lrm;"); // 8206 1 ‎ N Y N N N N N Y U N N U N U N N");
            EntityInfo.Add(8207, "&rlm;"); // 8207 1 ‏ N Y N N N N N Y U N N U N U N N");
            EntityInfo.Add(8211, "&ndash;"); // 8211 1 – N Y Y Y Y N Y Y U P Y U Y U Y Y");
            EntityInfo.Add(8212, "&mdash;"); // 8212 1 — N Y Y Y Y N Y Y U P Y U Y U Y Y");   
            EntityInfo.Add(8216, "&lsquo;"); // 8216 1 ‘ N Y Y Y Y N Y Y U P Y U Y U Y Y");   
            EntityInfo.Add(8217, "&rsquo;"); // 8217 1 ’ N Y Y Y Y N Y Y U P Y U Y U Y Y");   
            EntityInfo.Add(8222, "&bdquo;"); // 8222 1 „ N Y Y Y Y N Y Y U P Y U Y U Y Y");   
            EntityInfo.Add(8224, "&dagger;"); // 8224 1 † N Y Y Y Y N Y Y U N Y U Y U Y Y");
            EntityInfo.Add(8225, "&Dagger;"); // 8225 1 ‡ N Y Y Y Y N Y Y U N Y U Y U Y Y");
            EntityInfo.Add(8240, "&permil;"); // 8240 1 ‰ N Y Y Y Y N Y Y U N Y U Y U Y Y");
            EntityInfo.Add(8249, "&lsaquo;"); // 8249 1 ‹ N Y Y Y Y N Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(8250, "&rsaquo;"); // 8250 1 › N Y Y Y Y N Y Y U Y Y U Y U Y Y");
            EntityInfo.Add(8364, "&euro;"); // 8364 5 € N N Y Y Y N Y Y U Y Y U Y U Y Y");

            // now we store the reverse version in same table
            object[] keys = new object[EntityInfo.Count];
            EntityInfo.Keys.CopyTo(keys, 0);
            foreach (int key in keys)
            {                
                if (key < 160) continue; // do not reverse replace non-conformant codes
                EntityInfo.Add(EntityInfo[key], key);
            }

        }

        /// <summary>
        /// Will fire if the formatter is ready.
        /// </summary>
        public event FinishEventHandler OnFinish;

        /// <summary>
        /// Will fire if the formatter reached an unresolvable conflict in input stream.
        /// </summary>
        public event ErrorEventHandler OnError;

        /// <summary>
        /// Starts the beautifier and returns the resulting code in the TextWriter.
        /// </summary>
        /// <param name="input">The string to be beautified.</param>
        /// <param name="output">A TextWriter derived pointer to the result.</param>
        /// <param name="options">The formatting options used to handle the content.</param>
        public void Format(string input, TextWriter output, IHtmlFormatterOptions options)
        {
            bool xhtml = options.MakeXhtml;
            if (options.FakeEmptyParaTag)
            {
                input = rxPFake.Replace(input, @"<${tago}${attr}>&nbsp;</${tagc}>");
            }
            int maxLineLength = options.MaxLineLength;
            string indentString = new String(options.IndentChar, options.IndentSize);
            char[] chs = input.ToCharArray();
            Stack formatStack = new Stack();
            Stack writerStack = new Stack();
            FormatInfo formatInfo1 = null;
            string txtToken = String.Empty;
            bool flag2 = false;
            bool flag3 = false;
            bool flagP = false;
            bool hasErrors = false;
            // TODO: Implement better PRE recognizing here
            bool isInPre = false;
            bool wroteClosingBracket = false;
            // Clean before formatting starts
            if (options.WordClean)
            {
                input = StripHTML(input, true);
            }
            // Write formatted tokens into the writer
            HtmlWriter htmlWriter1 = new HtmlWriter(output, indentString, maxLineLength);
            writerStack.Push(htmlWriter1);
            Token currToken = HtmlTokenizer.GetFirstToken(chs);
            Token nextToken = currToken;
            for (; currToken != null; currToken = HtmlTokenizer.GetNextToken(currToken))
            {
                htmlWriter1 = (HtmlWriter)writerStack.Peek();
                switch (currToken.Type)
                {
                    case TokenType.AttrName:
                        if (xhtml)
                        {
                            string str3;
                            if (!formatInfo1.tagInfo.IsXml)
                            {
                                str3 = currToken.Text.ToLower();
                            }
                            else
                            {
                                str3 = currToken.Text;
                            }
                            htmlWriter1.Write(str3);
                            if (HtmlTokenizer.GetNextToken(currToken).Type != TokenType.EqualsChar)
                            {
                                htmlWriter1.Write(String.Concat("=\"", str3, "\""));
                            }
                        }
                        else if (formatInfo1.tagInfo.IsXml)
                        {
                            htmlWriter1.Write(currToken.Text);
                        }
                        else if (options.AttributeCasing == HtmlFormatterCase.UpperCase)
                        {
                            htmlWriter1.Write(currToken.Text.ToUpper());
                        }
                        else if (options.AttributeCasing == HtmlFormatterCase.LowerCase)
                        {
                            htmlWriter1.Write(currToken.Text.ToLower());
                        }
                        else
                        {
                            htmlWriter1.Write(currToken.Text);
                        }
                        break;

                    case TokenType.AttrVal:
                        if (xhtml && nextToken.Type != TokenType.DoubleQuote && nextToken.Type != TokenType.AttrVal)
                        {
                            htmlWriter1.Write('\"');
                            htmlWriter1.Write(currToken.Text.Replace("\"", "&quot;"));
                            htmlWriter1.Write('\"');
                        }
                        else
                        {
                            if (options.ReformatStyles)
                            {
                                p.ParseStylesheet(cp, String.Format("b {{{0}}}", currToken.Text));
                                ArrayList parsed = (ArrayList)p.ParsedStyles;
                                StringBuilder parsedStyle = new StringBuilder();
                                foreach (GuruComponents.Netrix.UserInterface.StyleParser.StyleObject so in parsed)
                                {
                                    foreach (DictionaryEntry hash in so.Styles)
                                    {
                                        parsedStyle.AppendFormat("{0}:{1};", hash.Key, hash.Value);
                                    }
                                }
                                htmlWriter1.Write(parsedStyle.ToString());
                            }
                            else
                            {
                                htmlWriter1.Write(currToken.Text);
                            }
                        }
                        break;

                    case TokenType.CloseBracket:
                        if (wroteClosingBracket)
                        {
                            wroteClosingBracket = false;
                        }
                        else if (!xhtml)
                        {
                            htmlWriter1.Write('>');
                        }
                        else if (flag3 && !formatInfo1.tagInfo.IsComment)
                        {
                            htmlWriter1.Write(" />");
                        }
                        else
                        {
                            htmlWriter1.Write('>');
                        }
                        break;

                    case TokenType.DoubleQuote:
                        htmlWriter1.Write('\"');
                        break;

                    case TokenType.EqualsChar:
                        htmlWriter1.Write('=');
                        break;

                    case TokenType.Error:
                        hasErrors = true;
                        if (nextToken.Type == TokenType.OpenBracket)
                        {
                            htmlWriter1.Write('<');
                        }
                        // fire the error event handler
                        if (OnError != null)
                        {
                            OnError(this, new GuruComponents.Netrix.HtmlFormatting.ErrorEventArgs(currToken, nextToken));
                        }
                        htmlWriter1.Write(currToken.Text);
                        break;

                    case TokenType.SelfTerminating:
                        formatInfo1.isEndTag = true;
                        if (!formatInfo1.tagInfo.NoEndTag)
                        {
                            formatStack.Pop();
                            if (formatInfo1.tagInfo.IsXml)
                            {
                                HtmlWriter htmlWriter2 = (HtmlWriter)writerStack.Pop();
                                htmlWriter1 = (HtmlWriter)writerStack.Peek();
                                htmlWriter1.Write(htmlWriter2.Content);
                            }
                        }
                        if (nextToken.Type == TokenType.Whitespace && nextToken.Text.Length > 0)
                        {
                            htmlWriter1.Write("/>");
                        }
                        else
                        {
                            htmlWriter1.Write(" />");
                        }
                        break;

                    case TokenType.SingleQuote:
                        htmlWriter1.Write('\'');
                        break;

                    case TokenType.XmlDirective:
                        htmlWriter1.WriteLineIfNotOnNewLine();
                        htmlWriter1.Write('<');
                        htmlWriter1.Write(currToken.Text);
                        htmlWriter1.Write('>');
                        htmlWriter1.WriteLineIfNotOnNewLine();
                        formatInfo1 = new FormatInfo(directiveTag, false);
                        wroteClosingBracket = true;
                        break;

                    case TokenType.TagName:
                    case TokenType.Comment:
                    case TokenType.InlineServerScript:
                        string txtTagName;
                        ITagInfo tagInfo;
                        flag3 = false;
                        if (currToken.Type == TokenType.Comment)
                        {
                            txtTagName = currToken.Text;
                            tagInfo = new TagInfo(currToken.Text, commentTag);
                        }
                        else if (currToken.Type == TokenType.InlineServerScript)
                        {
                            string txtInlineScript = currToken.Text.Trim();
                            txtInlineScript = txtInlineScript.Substring(1);
                            txtTagName = txtInlineScript;
                            if (txtInlineScript.StartsWith("%@"))
                            {
                                tagInfo = new TagInfo(txtInlineScript, directiveTag);
                            }
                            else
                            {
                                tagInfo = new TagInfo(txtInlineScript, otherServerSideScriptTag);
                            }
                        }
                        else
                        {
                            txtTagName = currToken.Text;
                            tagInfo = tagTable[txtTagName] as ITagInfo;
                            if (tagInfo == null)
                            {
                                if (txtTagName.IndexOf(':') > -1)
                                {
                                    tagInfo = new TagInfo(txtTagName, unknownXmlTag);
                                }
                                else if (htmlWriter1 is XmlWriter)
                                {
                                    tagInfo = new TagInfo(txtTagName, nestedXmlTag);
                                }
                                else
                                {
                                    tagInfo = new TagInfo(txtTagName, unknownHtmlTag);
                                }
                            }
                            else if (options.ElementCasing == HtmlFormatterCase.LowerCase || xhtml)
                            {
                                txtTagName = tagInfo.TagName;
                            }
                            else if (options.ElementCasing == HtmlFormatterCase.UpperCase)
                            {
                                txtTagName = tagInfo.TagName.ToUpper();
                            }
                        }
                        if (!flagP && txtTagName.ToLower().Equals("p"))
                        {
                            flagP = true;
                        }
                        if (formatInfo1 == null)
                        {
                            formatInfo1 = new FormatInfo(tagInfo, false);
                            formatInfo1.indent = 0;
                            formatStack.Push(formatInfo1);
                            htmlWriter1.Write(txtToken);
                            if (tagInfo.IsXml)
                            {
                                HtmlWriter htmlWriter3 = new XmlWriter(htmlWriter1.Indent, tagInfo.TagName, indentString, maxLineLength);
                                writerStack.Push(htmlWriter3);
                                htmlWriter1 = htmlWriter3;
                            }
                            if (nextToken.Type == TokenType.ForwardSlash)
                            {
                                htmlWriter1.Write("</");
                            }
                            else
                            {
                                htmlWriter1.Write('<');
                            }
                            htmlWriter1.Write(txtTagName);
                            txtToken = String.Empty;
                        }
                        else
                        {
                            WhiteSpaceType whiteSpaceType;

                            FormatInfo formatInfo2;
                            formatInfo2 = new FormatInfo(tagInfo, nextToken.Type == TokenType.ForwardSlash);
                            if (formatInfo1.isEndTag)
                            {
                                whiteSpaceType = formatInfo1.tagInfo.FollowingWhiteSpaceType;
                            }
                            else
                            {
                                whiteSpaceType = formatInfo1.tagInfo.InnerWhiteSpaceType;
                            }
                            bool isInline = formatInfo1.tagInfo.IsInline;
                            bool flag6 = false;
                            bool flag7 = false;
                            if (htmlWriter1 is XmlWriter)
                            {
                                XmlWriter xmlWriter = (XmlWriter)htmlWriter1;
                                if (xmlWriter.IsUnknownXml)
                                {
                                    flag7 = (formatInfo1.isBeginTag && formatInfo1.tagInfo.TagName.ToLower() == xmlWriter.TagName.ToLower() || formatInfo2.isEndTag && formatInfo2.tagInfo.TagName.ToLower() == xmlWriter.TagName.ToLower()) ? (FormattedTextWriter.IsWhiteSpace(txtToken) == false) : false;
                                }
                                if (formatInfo1.isBeginTag)
                                {
                                    if (FormattedTextWriter.IsWhiteSpace(txtToken))
                                    {
                                        if (xmlWriter.IsUnknownXml && formatInfo2.isEndTag && formatInfo1.tagInfo.TagName.ToLower() == formatInfo2.tagInfo.TagName.ToLower())
                                        {
                                            isInline = true;
                                            flag6 = true;
                                            txtToken = "";
                                        }
                                    }
                                    else if (!xmlWriter.IsUnknownXml)
                                    {
                                        xmlWriter.ContainsText = true;
                                    }
                                }
                            }
                            bool hasFrontWhitespace = true;
                            if (formatInfo1.isBeginTag && formatInfo1.tagInfo.PreserveContent)
                            {
                                htmlWriter1.Write(txtToken);
                            }
                            else
                            {
                                if (whiteSpaceType == WhiteSpaceType.NotSignificant)
                                {
                                    if (!isInline && !flag7)
                                    {
                                        if (txtToken.StartsWith(" ") || txtToken.StartsWith("<"))
                                        {
                                            //if (str2.StartsWith("&nbsp;")) {
                                            //    str2 = str2.Replace("&nbsp;", " ");
                                            //}
                                            htmlWriter1.WriteLineIfNotOnNewLine();
                                        }
                                        hasFrontWhitespace = false;
                                    }
                                }
                                else if (whiteSpaceType == WhiteSpaceType.Significant)
                                {
                                    if (FormattedTextWriter.HasFrontWhiteSpace(txtToken) && !isInline && !flag7)
                                    {
                                        htmlWriter1.WriteLineIfNotOnNewLine();
                                        hasFrontWhitespace = false;
                                    }
                                }
                                else if (whiteSpaceType == WhiteSpaceType.CarryThrough && (flag2 || FormattedTextWriter.HasFrontWhiteSpace(txtToken)) && !isInline && !flag7)
                                {
                                    htmlWriter1.WriteLineIfNotOnNewLine();
                                    hasFrontWhitespace = false;
                                }
                                if (formatInfo1.isBeginTag && !formatInfo1.tagInfo.NoIndent && !isInline)
                                {
                                    htmlWriter1.Indent = htmlWriter1.Indent + 1;
                                }
                                if (flag7 || isInPre)
                                {
                                    htmlWriter1.Write(txtToken);
                                }
                                else
                                {
                                    htmlWriter1.WriteLiteral(txtToken, hasFrontWhitespace);
                                }
                            }
                            if (!formatInfo2.isEndTag)
                            {
                                bool xhtml1 = false;
                                while (!xhtml1 && formatStack.Count > 0)
                                {
                                    FormatInfo formatInfo5 = (FormatInfo)formatStack.Peek();
                                    xhtml1 = formatInfo5.tagInfo.CanContainTag(formatInfo2.tagInfo);
                                    if (!xhtml1)
                                    {
                                        formatStack.Pop();
                                        htmlWriter1.Indent = formatInfo5.indent;
                                        if (xhtml)
                                        {
                                            if (!formatInfo5.tagInfo.IsInline)
                                            {
                                                htmlWriter1.WriteLineIfNotOnNewLine();
                                            }
                                            htmlWriter1.Write(String.Concat("</", formatInfo5.tagInfo.TagName, ">"));
                                        }
                                    }
                                }
                                formatInfo2.indent = htmlWriter1.Indent;
                                if (!flag7 && !formatInfo2.tagInfo.IsInline && !formatInfo2.tagInfo.PreserveContent && (FormattedTextWriter.IsWhiteSpace(txtToken) || FormattedTextWriter.HasBackWhiteSpace(txtToken) || txtToken.Length == 0 && (formatInfo1.isBeginTag && formatInfo1.tagInfo.InnerWhiteSpaceType == WhiteSpaceType.NotSignificant || formatInfo1.isEndTag && formatInfo1.tagInfo.FollowingWhiteSpaceType == WhiteSpaceType.NotSignificant)))
                                {
                                    htmlWriter1.WriteLineIfNotOnNewLine();
                                }
                                if (!formatInfo2.tagInfo.NoEndTag)
                                {
                                    formatStack.Push(formatInfo2);
                                }
                                else
                                {
                                    flag3 = true;
                                }
                                if (formatInfo2.tagInfo.IsXml)
                                {
                                    HtmlWriter htmlWriter6 = new XmlWriter(htmlWriter1.Indent, formatInfo2.tagInfo.TagName, indentString, maxLineLength);
                                    writerStack.Push(htmlWriter6);
                                    htmlWriter1 = htmlWriter6;
                                }
                                htmlWriter1.Write('<');
                                htmlWriter1.Write(txtTagName);
                            }
                            else if (!formatInfo2.tagInfo.NoEndTag)
                            {
                                ArrayList arrayList = new ArrayList();
                                FormatInfo formatInfo3;
                                bool flag9 = false;
                                bool xhtml0 = false;
                                if ((formatInfo2.tagInfo.Flags & FormattingFlags.AllowPartialTags) != 0)
                                {
                                    xhtml0 = true;
                                }
                                if (formatStack.Count > 0)
                                {
                                    formatInfo3 = (FormatInfo)formatStack.Pop();
                                    arrayList.Add(formatInfo3);
                                    while (formatStack.Count > 0 && formatInfo3.tagInfo.TagName.ToLower() != formatInfo2.tagInfo.TagName.ToLower())
                                    {
                                        if ((formatInfo3.tagInfo.Flags & FormattingFlags.AllowPartialTags) != 0)
                                        {
                                            xhtml0 = true;
                                            break;
                                        }
                                        formatInfo3 = (FormatInfo)formatStack.Pop();
                                        arrayList.Add(formatInfo3);
                                    }
                                    if (formatInfo3.tagInfo.TagName.ToLower() != formatInfo2.tagInfo.TagName.ToLower())
                                    {
                                        for (int j = arrayList.Count - 1; j >= 0; j--)
                                        {
                                            formatStack.Push(arrayList[j]);
                                        }
                                    }
                                    else
                                    {
                                        flag9 = true;
                                        for (int k = 0; k < arrayList.Count - 1; k++)
                                        {
                                            FormatInfo formatInfo4 = (FormatInfo)arrayList[k];
                                            if (formatInfo4.tagInfo.IsXml && writerStack.Count > 1)
                                            {
                                                HtmlWriter htmlWriter4 = (HtmlWriter)writerStack.Pop();
                                                htmlWriter1 = (HtmlWriter)writerStack.Peek();
                                                htmlWriter1.Write(htmlWriter4.Content);
                                            }
                                            if (!formatInfo4.tagInfo.NoEndTag)
                                            {
                                                htmlWriter1.WriteLineIfNotOnNewLine();
                                                htmlWriter1.Indent = formatInfo4.indent;
                                                if (xhtml && !xhtml0)
                                                {
                                                    htmlWriter1.Write(String.Concat("</", formatInfo4.tagInfo.TagName, ">"));
                                                }
                                            }
                                        }
                                        htmlWriter1.Indent = formatInfo3.indent;
                                    }
                                }
                                if (flag9 || xhtml0)
                                {
                                    if (!flag6 && !flag7 && !formatInfo2.tagInfo.IsInline && !formatInfo2.tagInfo.PreserveContent && (FormattedTextWriter.IsWhiteSpace(txtToken) || FormattedTextWriter.HasBackWhiteSpace(txtToken) || formatInfo2.tagInfo.FollowingWhiteSpaceType == WhiteSpaceType.NotSignificant) && (!(formatInfo2.tagInfo is TDTagInfo) || FormattedTextWriter.HasBackWhiteSpace(txtToken)))
                                    {
                                        htmlWriter1.WriteLineIfNotOnNewLine();
                                    }
                                    htmlWriter1.Write("</");
                                    htmlWriter1.Write(txtTagName);
                                }
                                else
                                {
                                    wroteClosingBracket = true;
                                }
                                if (formatInfo2.tagInfo.IsXml && writerStack.Count > 1)
                                {
                                    HtmlWriter htmlWriter5 = (HtmlWriter)writerStack.Pop();
                                    htmlWriter1 = (HtmlWriter)writerStack.Peek();
                                    htmlWriter1.Write(htmlWriter5.Content);
                                }
                            }
                            else
                            {
                                wroteClosingBracket = true;
                            }
                            flag2 = FormattedTextWriter.HasBackWhiteSpace(txtToken);
                            txtToken = String.Empty;
                            formatInfo1 = formatInfo2;
                        }
                        break;

                    case TokenType.TextToken:
                        string newText = currToken.Text;
                        if (options.Entities != EntityFormat.NoEntity)
                        {
                            BuildEntities(ref newText, options.Entities);
                        }
                        txtToken = String.Concat(txtToken, newText);
                        break;
                    case TokenType.Style:
                        txtToken = String.Concat(txtToken, currToken.Text);
                        if (xhtml && txtToken.Length > 0 && txtToken.TrimStart().IndexOf("<![CDATA[") == -1)
                        {
                            if (options.ReformatStyles)
                            {
                                p.ParseStylesheet(cp, txtToken);
                                ArrayList parsed = (ArrayList)p.ParsedStyles;
                                StringBuilder parsedStyle = new StringBuilder();
                                foreach (GuruComponents.Netrix.UserInterface.StyleParser.StyleObject so in parsed)
                                {
                                    parsedStyle.AppendFormat("{0} {{ {1}", so.SelectorName, Environment.NewLine);
                                    foreach (DictionaryEntry hash in so.Styles)
                                    {
                                        parsedStyle.AppendFormat("{0}:{1};{2}", hash.Key, hash.Value, Environment.NewLine);
                                    }
                                    parsedStyle.AppendFormat("}}{0}", Environment.NewLine);
                                }
                                txtToken = parsedStyle.ToString();
                            }
                            txtToken = String.Format("{2}<![CDATA[{3}{0}{1}{0}{2}]]>{3}",
                                options.CommentCDATAAddsNewline ? Environment.NewLine : "",
                                txtToken,
                                "/*", "*/");
                            
                        }
                        break;
                    case TokenType.ClientScriptBlock:
                    case TokenType.ServerScriptBlock:
                    case TokenType.PhpScriptTag:
                        txtToken = String.Concat(txtToken, currToken.Text);
                        if (xhtml && txtToken.Length > 0 && txtToken.TrimStart().IndexOf("<![CDATA[") == -1)
                        {
                            txtToken = String.Format("{2}<![CDATA[{0}{1}{0}{2}]]>",
                                options.CommentCDATAAddsNewline ? Environment.NewLine : "",
                                txtToken,
                                (options.CommentCDATA) ? options.CommentCDATAString : "");
                        }
                        break;
                    case TokenType.Whitespace:
                        if (currToken.Text.Length <= 0)
                        {
                            continue;
                        }
                        htmlWriter1.Write(' ');
                        break;
                }
                nextToken = currToken;
            }
            if (txtToken.Length > 0)
            {
                htmlWriter1.Write(txtToken);
            }
            while (writerStack.Count > 1)
            {
                HtmlWriter htmlWriter7 = (HtmlWriter)writerStack.Pop();
                htmlWriter1 = (HtmlWriter)writerStack.Peek();
                htmlWriter1.Write(htmlWriter7.Content);
            }
            htmlWriter1.Flush();
            // fire the Finish Event now
            if (OnFinish != null)
            {
                OnFinish(this, new FinishEventArgs(output, input, hasErrors));
            }
        }    

        /// <summary>
        /// Formats a string by replacing extended characters as entities.
        /// </summary>
        /// <param name="text">The text to be formatted.</param>
        /// <param name="Entities">The way to format.</param>
        /// <returns>The formatted text.</returns>
        public static string GetEntities(string text, EntityFormat Entities)
        {
            BuildEntities(ref text, Entities);
            return text;
        }

        /// <summary>
        /// Change any Unicode or higher ASCII into entities, if possible.
        /// </summary>
        /// <param name="input">The text in which the entities are formatted.</param>
        /// <param name="Entities">Determines the formatting of entities.</param>
        private static void BuildEntities(ref string input, EntityFormat Entities)
        {
            if (input.Trim().Length == 0) return;
            // convert enhanced codes into entities
            StringBuilder sb = new StringBuilder(input.Length);
            switch (Entities)
            {
                case EntityFormat.NoEntity:
                    input = entRegx.Replace(input, entAntiEval);
                    break;
                case EntityFormat.Named:
                    int Ord;
                    foreach (char c in input)
                    {
                        Ord = Convert.ToInt32(c);
                        if (Ord >= 128)
                        {
                            object o = EntityInfo[Ord];
                            if (o != null)
                                sb.Append(o);
                            else
                                sb.Append(c);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                    input = sb.ToString();
                    break;
                case EntityFormat.Numeric:
                    // Convert values already exists as entities into numeric one                    
                    input = entRegx.Replace(input, entEval);
                    foreach (char c in input)
                    {
                        Ord = Convert.ToInt32(c);
                        if (Ord == 128) Ord = 8364; // HACK: Euro Replacement
                        if (Ord == 150) Ord = 8211; // HACK: ndash Replacement
                        if (Ord == 151) Ord = 8212; // HACK: mdash Replacement
                        if (Ord >= 128)
                        {
                            sb.Append(String.Concat("&#", Ord, ";"));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                    input = sb.ToString();
                    break;
            }
        }

        /// <summary>
        /// Converts string entities into numeric ones.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string Match(Match m)
        {
            return String.Concat("&#", EntityInfo[m.Value], ";");
        }

        /// <summary>
        /// Converts string entities into numeric ones.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string AntiMatch(Match m)
        {
            return EntityInfo[m.Value].ToString();
        }


        /// <summary> 
        /// This function filters out unwanted html tags from a html string. 
        /// It also empties tags of any attributes, e.g. &lt;p style="msonormal"&gt; becomes &lt;p&gt;
        /// </summary> 
        /// <param name="html">The HTML fragment to work on</param> 
        /// <param name="keepTags">An array of strings, each representing a html tag, e.g. "a","img", etc.</param> 
        /// <returns>Returns the clean HTML as string.</returns> 
        private static string StripHTML(string html, bool keepTags)
        {
            Regex reg = new Regex("<(.|\n)+?>", RegexOptions.IgnoreCase);
            if (!keepTags)
            {
                return reg.Replace(html, ""); // gives the plain text of the html with no tags 
            }
            string cleanedHTML = html;

            html = Regex.Replace(html, @"<\/?SPAN[^>]*>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, @"<(\w[^>]*) class=([^ |>]*)([^>]*)", "<$1$3", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, @"<(\w[^>]*) style=""([^""]*)""([^>]*)", "<$1$3", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, @"<(\w[^>]*) lang=([^ |>]*)([^>]*)", "<$1$3", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, @"<\\?\?xml[^>]*>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, @"<\/?\w+:[^>]*>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            html = Regex.Replace(html, @" ", " ", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return cleanedHTML.ToString();
        }


    }
}