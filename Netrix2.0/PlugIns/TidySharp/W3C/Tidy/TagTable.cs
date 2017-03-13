/*
* @(#)TagTable.java   1.11 2000/08/16
*
*/

using System;
namespace Comzept.Genesis.Tidy
{
    /// <summary> 
    /// Tag dictionary node hash table</summary>
    /// <remarks>
    /// (c) 1998-2000 (W3C) MIT, INRIA, Keio University
    /// See Tidy.java for the copyright notice.
    /// Derived from <a href="http://www.w3.org/People/Raggett/tidy">
    /// HTML Tidy Release 4 Aug 2000</a>
    /// </remarks>
    /// 
    /// <author>   Dave Raggett dsr@w3.org
    /// </author>
    /// <author>   Andy Quick ac.quick@sympatico.ca (translation to Java)
    /// </author>
    /// <version>  1.0, 1999/05/22
    /// </version>
    /// <version>  1.0.1, 1999/05/29
    /// </version>
    /// <version>  1.1, 1999/06/18 Java Bean
    /// </version>
    /// <version>  1.2, 1999/07/10 Tidy Release 7 Jul 1999
    /// </version>
    /// <version>  1.3, 1999/07/30 Tidy Release 26 Jul 1999
    /// </version>
    /// <version>  1.4, 1999/09/04 DOM support
    /// </version>
    /// <version>  1.5, 1999/10/23 Tidy Release 27 Sep 1999
    /// </version>
    /// <version>  1.6, 1999/11/01 Tidy Release 22 Oct 1999
    /// </version>
    /// <version>  1.7, 1999/12/06 Tidy Release 30 Nov 1999
    /// </version>
    /// <version>  1.8, 2000/01/22 Tidy Release 13 Jan 2000
    /// </version>
    /// <version>  1.9, 2000/06/03 Tidy Release 30 Apr 2000
    /// </version>
    /// <version>  1.10, 2000/07/22 Tidy Release 8 Jul 2000
    /// </version>
    /// <version>  1.11, 2000/08/16 Tidy Release 4 Aug 2000
    /// Modified from a Singleton to a non-Singleton.
    /// </version>	
    public class TagTable
    {
        private void InitBlock()
        {
            xmlTags = new Dict(null, Dict.VERS_ALL, Dict.CM_BLOCK, null, null);
        }
        virtual public Configuration Configuration
        {
            set
            {
                this.configuration = value;
            }

        }

        private Configuration configuration = null;

        /// <summary>
        /// Table of acceptable tags.
        /// </summary>
        public TagTable()
        {
            InitBlock();
            for (int i = 0; i < tags.Length; i++)
            {
                install(tags[i]);
            }
            tagHtml = lookup("html");
            tagHead = lookup("head");
            tagBody = lookup("body");
            tagFrameset = lookup("frameset");
            tagFrame = lookup("frame");
            tagNoframes = lookup("noframes");
            tagMeta = lookup("meta");
            tagTitle = lookup("title");
            tagBase = lookup("base");
            tagHr = lookup("hr");
            tagPre = lookup("pre");
            tagListing = lookup("listing");
            tagH1 = lookup("h1");
            tagH2 = lookup("h2");
            tagP = lookup("p");
            tagUl = lookup("ul");
            tagOl = lookup("ol");
            tagDir = lookup("dir");
            tagLi = lookup("li");
            tagDt = lookup("dt");
            tagDd = lookup("dd");
            tagDl = lookup("dl");
            tagTd = lookup("td");
            tagTh = lookup("th");
            tagTr = lookup("tr");
            tagCol = lookup("col");
            tagBr = lookup("br");
            tagA = lookup("a");
            tagLink = lookup("link");
            tagB = lookup("b");
            tagI = lookup("i");
            tagStrong = lookup("strong");
            tagEm = lookup("em");
            tagBig = lookup("big");
            tagSmall = lookup("small");
            tagParam = lookup("param");
            tagOption = lookup("option");
            tagOptgroup = lookup("optgroup");
            tagImg = lookup("img");
            tagMap = lookup("map");
            tagArea = lookup("area");
            tagNobr = lookup("nobr");
            tagWbr = lookup("wbr");
            tagFont = lookup("font");
            tagSpacer = lookup("spacer");
            tagLayer = lookup("layer");
            tagCenter = lookup("center");
            tagStyle = lookup("style");
            tagScript = lookup("script");
            tagNoscript = lookup("noscript");
            tagTable = lookup("table");
            tagCaption = lookup("caption");
            tagForm = lookup("form");
            tagTextarea = lookup("textarea");
            tagBlockquote = lookup("blockquote");
            tagApplet = lookup("applet");
            tagObject = lookup("object");
            tagDiv = lookup("div");
            tagSpan = lookup("span");
        }

        public virtual Dict lookup(System.String name)
        {
            return (Dict)tagHashtable[name];
        }

        public virtual Dict install(Dict dict)
        {
            Dict d = (Dict)tagHashtable[dict.name];
            if (d != null)
            {
                d.versions = dict.versions;
                d.model |= dict.model;
                d.parser = dict.parser;
                d.chkattrs = dict.chkattrs;
                return d;
            }
            else
            {
                tagHashtable[dict.name] = dict;
                return dict;
            }
        }

        /// <summary>
        /// public interface for finding tag by name
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual bool findTag(Node node)
        {
            Dict np;

            if (configuration != null && configuration.XmlTags)
            {
                node.tag = xmlTags;
                return true;
            }

            if (node.element != null)
            {
                np = lookup(node.element);
                if (np != null)
                {
                    node.tag = np;
                    return true;
                }
            }

            return false;
        }

        public virtual Parser findParser(Node node)
        {
            Dict np;

            if (node.element != null)
            {
                np = lookup(node.element);
                if (np != null)
                {
                    return np.parser;
                }
            }

            return null;
        }

        private System.Collections.Hashtable tagHashtable = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());

        //UPGRADE_NOTE: The initialization of  'tags' was moved to static method 'Comzept.Genesis.Tidy.TagTable'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        private static Dict[] tags;

        /* create dummy entry for all xml tags */
        //UPGRADE_NOTE: The initialization of  'xmlTags' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        public Dict xmlTags;
        /// <summary>
        /// Html
        /// </summary>
        public Dict tagHtml = null;
        /// <summary>
        /// Head
        /// </summary>
        public Dict tagHead = null;
        /// <summary>
        /// Body
        /// </summary>
        public Dict tagBody = null;
        /// <summary>
        /// Frameset
        /// </summary>
        public Dict tagFrameset = null;
        /// <summary>
        /// Frame
        /// </summary>
        public Dict tagFrame = null;
        /// <summary>
        /// NoFrames
        /// </summary>
        public Dict tagNoframes = null;
        /// <summary>
        /// Meta
        /// </summary>
        public Dict tagMeta = null;
        /// <summary>
        /// Title
        /// </summary>
        public Dict tagTitle = null;
        /// <summary>
        /// Base
        /// </summary>
        public Dict tagBase = null;
        /// <summary>
        /// Hr
        /// </summary>
        public Dict tagHr = null;
        /// <summary>
        /// Pre
        /// </summary>
        public Dict tagPre = null;
        /// <summary>
        /// Listing
        /// </summary>
        public Dict tagListing = null;
        /// <summary>
        /// H1
        /// </summary>
        public Dict tagH1 = null;
        /// <summary>
        /// H2
        /// </summary>
        public Dict tagH2 = null;
        /// <summary>
        /// P
        /// </summary>
        public Dict tagP = null;
        /// <summary>
        /// UL
        /// </summary>
        public Dict tagUl = null;
        /// <summary>
        /// OL
        /// </summary>
        public Dict tagOl = null;
        /// <summary>
        /// Dir
        /// </summary>
        public Dict tagDir = null;
        /// <summary>
        /// LI
        /// </summary>
        public Dict tagLi = null;
        /// <summary>
        /// DT
        /// </summary>
        public Dict tagDt = null;
        /// <summary>
        /// DD
        /// </summary>
        public Dict tagDd = null;
        /// <summary>
        /// DL
        /// </summary>
        public Dict tagDl = null;
        /// <summary>
        /// TD
        /// </summary>
        public Dict tagTd = null;
        /// <summary>
        /// TH
        /// </summary>
        public Dict tagTh = null;
        /// <summary>
        /// TR
        /// </summary>
        public Dict tagTr = null;
        /// <summary>
        /// COL
        /// </summary>
        public Dict tagCol = null;
        /// <summary>
        /// BR
        /// </summary>
        public Dict tagBr = null;
        /// <summary>
        /// A
        /// </summary>
        public Dict tagA = null;
        /// <summary>
        /// LINK
        /// </summary>
        public Dict tagLink = null;
        /// <summary>
        /// B
        /// </summary>
        public Dict tagB = null;
        /// <summary>
        /// I
        /// </summary>
        public Dict tagI = null;
        /// <summary>
        /// STRONG
        /// </summary>
        public Dict tagStrong = null;
        /// <summary>
        /// EM
        /// </summary>
        public Dict tagEm = null;
        /// <summary>
        /// BIG
        /// </summary>
        public Dict tagBig = null;
        /// <summary>
        /// SMALL
        /// </summary>
        public Dict tagSmall = null;
        /// <summary>
        /// PARAM
        /// </summary>
        public Dict tagParam = null;
        /// <summary>
        /// OPTION
        /// </summary>
        public Dict tagOption = null;
        /// <summary>
        /// OPTGROUP
        /// </summary>
        public Dict tagOptgroup = null;
        /// <summary>
        /// IMG
        /// </summary>
        public Dict tagImg = null;
        /// <summary>
        /// MAP
        /// </summary>
        public Dict tagMap = null;
        /// <summary>
        /// AREA
        /// </summary>
        public Dict tagArea = null;
        /// <summary>
        /// NOBR
        /// </summary>
        public Dict tagNobr = null;
        /// <summary>
        /// WBR
        /// </summary>
        public Dict tagWbr = null;
        /// <summary>
        /// FONT
        /// </summary>
        public Dict tagFont = null;
        /// <summary>
        /// SPACER
        /// </summary>
        public Dict tagSpacer = null;
        /// <summary>
        /// LAYER
        /// </summary>
        public Dict tagLayer = null;
        /// <summary>
        /// CENTER
        /// </summary>
        public Dict tagCenter = null;
        /// <summary>
        /// STYLE
        /// </summary>
        public Dict tagStyle = null;
        /// <summary>
        /// SCRIPT
        /// </summary>
        public Dict tagScript = null;
        /// <summary>
        /// NoScript
        /// </summary>
        public Dict tagNoscript = null;
        /// <summary>
        /// TABLE
        /// </summary>
        public Dict tagTable = null;
        /// <summary>
        /// CAPTION
        /// </summary>
        public Dict tagCaption = null;
        /// <summary>
        /// FORM
        /// </summary>
        public Dict tagForm = null;
        /// <summary>
        /// TextArea
        /// </summary>
        public Dict tagTextarea = null;
        /// <summary>
        /// Blockquote
        /// </summary>
        public Dict tagBlockquote = null;
        /// <summary>
        /// Applet
        /// </summary>
        public Dict tagApplet = null;
        /// <summary>
        /// Object
        /// </summary>
        public Dict tagObject = null;
        /// <summary>
        /// Div
        /// </summary>
        public Dict tagDiv = null;
        /// <summary>
        /// Span
        /// </summary>
        public Dict tagSpan = null;

        /// <summary>
        /// Define a inline tag.
        /// </summary>
        /// <param name="name"></param>
        public virtual void DefineInlineTag(System.String name)
        {
            install(new Dict(name, Dict.VERS_PROPRIETARY, (Dict.CM_INLINE | Dict.CM_NO_INDENT | Dict.CM_NEW), ParserImpl.getParseBlock(), null));
        }
        /// <summary>
        /// Define a block tag
        /// </summary>
        /// <param name="name"></param>
        public virtual void DefineBlockTag(System.String name)
        {
            install(new Dict(name, Dict.VERS_PROPRIETARY, (Dict.CM_BLOCK | Dict.CM_NO_INDENT | Dict.CM_NEW), ParserImpl.getParseBlock(), null));
        }

        /// <summary>
        /// Define a empty tag
        /// </summary>
        /// <param name="name"></param>
        public virtual void DefineEmptyTag(System.String name)
        {
            install(new Dict(name, Dict.VERS_PROPRIETARY, (Dict.CM_EMPTY | Dict.CM_NO_INDENT | Dict.CM_NEW), ParserImpl.getParseBlock(), null));
        }

        /// <summary>
        /// Define a pre tag
        /// </summary>
        /// <param name="name"></param>
        public virtual void DefinePreTag(System.String name)
        {
            install(new Dict(name, Dict.VERS_PROPRIETARY, (Dict.CM_BLOCK | Dict.CM_NO_INDENT | Dict.CM_NEW), ParserImpl.ParsePre, null));
        }

        static TagTable()
        {
            tags = new Dict[]{new Dict("html", (short) (Dict.VERS_ALL | Dict.VERS_FRAMES), (Dict.CM_HTML | Dict.CM_OPT | Dict.CM_OMITST), ParserImpl.getParseHTML(), CheckAttribsImpl.getCheckHTML()), new Dict("head", (short) (Dict.VERS_ALL | Dict.VERS_FRAMES), (Dict.CM_HTML | Dict.CM_OPT | Dict.CM_OMITST), ParserImpl.getParseHead(), null), new Dict("title", (short) (Dict.VERS_ALL | Dict.VERS_FRAMES), Dict.CM_HEAD, ParserImpl.getParseTitle(), null), new Dict("base", (short) (Dict.VERS_ALL | Dict.VERS_FRAMES), (Dict.CM_HEAD | Dict.CM_EMPTY), null, null), new Dict("link", (short) (Dict.VERS_ALL | Dict.VERS_FRAMES), (Dict.CM_HEAD | Dict.CM_EMPTY), null, CheckAttribsImpl.getCheckLINK()), new Dict("meta", (short) (Dict.VERS_ALL | Dict.VERS_FRAMES), (Dict.CM_HEAD | Dict.CM_EMPTY), null, null), new Dict("style", (short) (Dict.VERS_FROM32 | Dict.VERS_FRAMES), Dict.CM_HEAD, ParserImpl.getParseScript(), CheckAttribsImpl.getCheckSTYLE()), new Dict("script", (short) (Dict.VERS_FROM32 | Dict.VERS_FRAMES), (Dict.CM_HEAD | Dict.CM_MIXED | Dict.CM_BLOCK | Dict.CM_INLINE), ParserImpl.getParseScript(), CheckAttribsImpl.getCheckSCRIPT()), new Dict("server", Dict.VERS_NETSCAPE, (Dict.CM_HEAD | Dict.CM_MIXED | Dict.CM_BLOCK | Dict.CM_INLINE), ParserImpl.getParseScript(), null), new Dict("body", Dict.VERS_ALL, (Dict.CM_HTML | Dict.CM_OPT | Dict.CM_OMITST), ParserImpl.getParseBody(), null), new Dict("frameset", Dict.VERS_FRAMES, (Dict.CM_HTML | Dict.CM_FRAMES), ParserImpl.getParseFrameSet(), null), new Dict("p", Dict.VERS_ALL, (Dict.CM_BLOCK | Dict.CM_OPT), ParserImpl.getParseInline(), null), new Dict("h1", Dict.VERS_ALL, (Dict.CM_BLOCK | Dict.CM_HEADING), ParserImpl.getParseInline(), null), new Dict("h2", Dict.VERS_ALL, (Dict.CM_BLOCK | Dict.CM_HEADING), ParserImpl.getParseInline(), null), new Dict("h3", Dict.VERS_ALL, (Dict.CM_BLOCK | Dict.CM_HEADING), ParserImpl.getParseInline(), null), new Dict("h4", Dict.VERS_ALL, (Dict.CM_BLOCK | Dict.CM_HEADING), ParserImpl.getParseInline(), null), new Dict("h5", Dict.VERS_ALL, (Dict.CM_BLOCK
				 | Dict.CM_HEADING), ParserImpl.getParseInline(), null), new Dict("h6", Dict.VERS_ALL, (Dict.CM_BLOCK | Dict.CM_HEADING), ParserImpl.getParseInline(), null), new Dict("ul", Dict.VERS_ALL, Dict.CM_BLOCK, ParserImpl.ParseList, null), new Dict("ol", Dict.VERS_ALL, Dict.CM_BLOCK, ParserImpl.ParseList, null), new Dict("dl", Dict.VERS_ALL, Dict.CM_BLOCK, ParserImpl.ParseDefList, null), new Dict("dir", Dict.VERS_LOOSE, (Dict.CM_BLOCK | Dict.CM_OBSOLETE), ParserImpl.ParseList, null), new Dict("menu", Dict.VERS_LOOSE, (Dict.CM_BLOCK | Dict.CM_OBSOLETE), ParserImpl.ParseList, null), new Dict("pre", Dict.VERS_ALL, Dict.CM_BLOCK, ParserImpl.ParsePre, null), new Dict("listing", Dict.VERS_ALL, (Dict.CM_BLOCK | Dict.CM_OBSOLETE), ParserImpl.ParsePre, null), new Dict("xmp", Dict.VERS_ALL, (Dict.CM_BLOCK | Dict.CM_OBSOLETE), ParserImpl.ParsePre, null), new Dict("plaintext", Dict.VERS_ALL, (Dict.CM_BLOCK | Dict.CM_OBSOLETE), ParserImpl.ParsePre, null), new Dict("address", Dict.VERS_ALL, Dict.CM_BLOCK, ParserImpl.getParseBlock(), null), new Dict("blockquote", Dict.VERS_ALL, Dict.CM_BLOCK, ParserImpl.getParseBlock(), null), new Dict("form", Dict.VERS_ALL, Dict.CM_BLOCK, ParserImpl.getParseBlock(), null), new Dict("isindex", Dict.VERS_LOOSE, (Dict.CM_BLOCK | Dict.CM_EMPTY), null, null), new Dict("fieldset", Dict.VERS_HTML40, Dict.CM_BLOCK, ParserImpl.getParseBlock(), null), new Dict("table", Dict.VERS_FROM32, Dict.CM_BLOCK, ParserImpl.getParseTableTag(), CheckAttribsImpl.getCheckTABLE()), new Dict("hr", Dict.VERS_ALL, (Dict.CM_BLOCK | Dict.CM_EMPTY), null, CheckAttribsImpl.getCheckHR()), new Dict("div", Dict.VERS_FROM32, Dict.CM_BLOCK, ParserImpl.getParseBlock(), null), new Dict("multicol", Dict.VERS_NETSCAPE, Dict.CM_BLOCK, ParserImpl.getParseBlock(), null), new Dict("nosave", Dict.VERS_NETSCAPE, Dict.CM_BLOCK, ParserImpl.getParseBlock(), null), new Dict("layer", Dict.VERS_NETSCAPE, Dict.CM_BLOCK, ParserImpl.getParseBlock(), null), new Dict("ilayer", Dict.
				VERS_NETSCAPE, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("nolayer", Dict.VERS_NETSCAPE, (Dict.CM_BLOCK | Dict.CM_INLINE | Dict.CM_MIXED), ParserImpl.getParseBlock(), null), new Dict("align", Dict.VERS_NETSCAPE, Dict.CM_BLOCK, ParserImpl.getParseBlock(), null), new Dict("center", Dict.VERS_LOOSE, Dict.CM_BLOCK, ParserImpl.getParseBlock(), null), new Dict("ins", Dict.VERS_HTML40, (Dict.CM_INLINE | Dict.CM_BLOCK | Dict.CM_MIXED), ParserImpl.getParseInline(), null), new Dict("del", Dict.VERS_HTML40, (Dict.CM_INLINE | Dict.CM_BLOCK | Dict.CM_MIXED), ParserImpl.getParseInline(), null), new Dict("li", Dict.VERS_ALL, (Dict.CM_LIST | Dict.CM_OPT | Dict.CM_NO_INDENT), ParserImpl.getParseBlock(), null), new Dict("dt", Dict.VERS_ALL, (Dict.CM_DEFLIST | Dict.CM_OPT | Dict.CM_NO_INDENT), ParserImpl.getParseInline(), null), new Dict("dd", Dict.VERS_ALL, (Dict.CM_DEFLIST | Dict.CM_OPT | Dict.CM_NO_INDENT), ParserImpl.getParseBlock(), null), new Dict("caption", Dict.VERS_FROM32, Dict.CM_TABLE, ParserImpl.getParseInline(), CheckAttribsImpl.getCheckCaption()), new Dict("colgroup", Dict.VERS_HTML40, (Dict.CM_TABLE | Dict.CM_OPT), ParserImpl.getParseColGroup(), null), new Dict("col", Dict.VERS_HTML40, (Dict.CM_TABLE | Dict.CM_EMPTY), null, null), new Dict("thead", Dict.VERS_HTML40, (Dict.CM_TABLE | Dict.CM_ROWGRP | Dict.CM_OPT), ParserImpl.getParseRowGroup(), null), new Dict("tfoot", Dict.VERS_HTML40, (Dict.CM_TABLE | Dict.CM_ROWGRP | Dict.CM_OPT), ParserImpl.getParseRowGroup(), null), new Dict("tbody", Dict.VERS_HTML40, (Dict.CM_TABLE | Dict.CM_ROWGRP | Dict.CM_OPT), ParserImpl.getParseRowGroup(), null), new Dict("tr", Dict.VERS_FROM32, (Dict.CM_TABLE | Dict.CM_OPT), ParserImpl.getParseRow(), null), new Dict("td", Dict.VERS_FROM32, (Dict.CM_ROW | Dict.CM_OPT | Dict.CM_NO_INDENT), ParserImpl.getParseBlock(), CheckAttribsImpl.getCheckTableCell()), new Dict("th", Dict.VERS_FROM32, (Dict.CM_ROW | Dict.CM_OPT | Dict.CM_NO_INDENT), ParserImpl.getParseBlock(), CheckAttribsImpl.getCheckTableCell()), 
				new Dict("q", Dict.VERS_HTML40, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("a", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), CheckAttribsImpl.getCheckAnchor()), new Dict("br", Dict.VERS_ALL, (Dict.CM_INLINE | Dict.CM_EMPTY), null, null), new Dict("img", Dict.VERS_ALL, (Dict.CM_INLINE | Dict.CM_IMG | Dict.CM_EMPTY), null, CheckAttribsImpl.getCheckIMG()), new Dict("object", Dict.VERS_HTML40, (Dict.CM_OBJECT | Dict.CM_HEAD | Dict.CM_IMG | Dict.CM_INLINE | Dict.CM_PARAM), ParserImpl.getParseBlock(), null), new Dict("applet", Dict.VERS_LOOSE, (Dict.CM_OBJECT | Dict.CM_IMG | Dict.CM_INLINE | Dict.CM_PARAM), ParserImpl.getParseBlock(), null), new Dict("servlet", Dict.VERS_SUN, (Dict.CM_OBJECT | Dict.CM_IMG | Dict.CM_INLINE | Dict.CM_PARAM), ParserImpl.getParseBlock(), null), new Dict("param", Dict.VERS_FROM32, (Dict.CM_INLINE | Dict.CM_EMPTY), null, null), new Dict("embed", Dict.VERS_NETSCAPE, (Dict.CM_INLINE | Dict.CM_IMG | Dict.CM_EMPTY), null, null), new Dict("noembed", Dict.VERS_NETSCAPE, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("iframe", Dict.VERS_HTML40_LOOSE, Dict.CM_INLINE, ParserImpl.getParseBlock(), null), new Dict("frame", Dict.VERS_FRAMES, (Dict.CM_FRAMES | Dict.CM_EMPTY), null, null), new Dict("noframes", Dict.VERS_IFRAMES, (Dict.CM_BLOCK | Dict.CM_FRAMES), ParserImpl.getParseNoFrames(), null), new Dict("noscript", (short) (Dict.VERS_FRAMES | Dict.VERS_HTML40), (Dict.CM_BLOCK | Dict.CM_INLINE | Dict.CM_MIXED), ParserImpl.getParseBlock(), null), new Dict("b", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("i", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("u", Dict.VERS_LOOSE, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("tt", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("s", Dict.VERS_LOOSE, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("strike", Dict.VERS_LOOSE, Dict.CM_INLINE, ParserImpl.getParseInline(), null), 
				new Dict("big", Dict.VERS_FROM32, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("small", Dict.VERS_FROM32, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("sub", Dict.VERS_FROM32, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("sup", Dict.VERS_FROM32, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("em", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("strong", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("dfn", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("code", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("samp", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("kbd", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("var", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("cite", Dict.VERS_ALL, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("abbr", Dict.VERS_HTML40, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("acronym", Dict.VERS_HTML40, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("span", Dict.VERS_FROM32, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("blink", Dict.VERS_PROPRIETARY, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("nobr", Dict.VERS_PROPRIETARY, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("wbr", Dict.VERS_PROPRIETARY, (Dict.CM_INLINE | Dict.CM_EMPTY), null, null), new Dict("marquee", Dict.VERS_MICROSOFT, (Dict.CM_INLINE | Dict.CM_OPT), ParserImpl.getParseInline(), null), new Dict("bgsound", Dict.VERS_MICROSOFT, (Dict.CM_HEAD | Dict.CM_EMPTY), null, null), new Dict("comment", Dict.VERS_MICROSOFT, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("spacer", Dict.VERS_NETSCAPE, (Dict.CM_INLINE | Dict.CM_EMPTY), null, null), new Dict("keygen", Dict.VERS_NETSCAPE, (Dict.CM_INLINE | Dict.CM_EMPTY), null, null), new Dict(
				"nolayer", Dict.VERS_NETSCAPE, (Dict.CM_BLOCK | Dict.CM_INLINE | Dict.CM_MIXED), ParserImpl.getParseBlock(), null), new Dict("ilayer", Dict.VERS_NETSCAPE, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("map", Dict.VERS_FROM32, Dict.CM_INLINE, ParserImpl.getParseBlock(), CheckAttribsImpl.getCheckMap()), new Dict("area", Dict.VERS_ALL, (Dict.CM_BLOCK | Dict.CM_EMPTY), null, CheckAttribsImpl.getCheckAREA()), new Dict("input", Dict.VERS_ALL, (Dict.CM_INLINE | Dict.CM_IMG | Dict.CM_EMPTY), null, null), new Dict("select", Dict.VERS_ALL, (Dict.CM_INLINE | Dict.CM_FIELD), ParserImpl.getParseSelect(), null), new Dict("option", Dict.VERS_ALL, (Dict.CM_FIELD | Dict.CM_OPT), ParserImpl.getParseText(), null), new Dict("optgroup", Dict.VERS_HTML40, (Dict.CM_FIELD | Dict.CM_OPT), ParserImpl.ParseOptGroup, null), new Dict("textarea", Dict.VERS_ALL, (Dict.CM_INLINE | Dict.CM_FIELD), ParserImpl.getParseText(), null), new Dict("label", Dict.VERS_HTML40, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("legend", Dict.VERS_HTML40, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("button", Dict.VERS_HTML40, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("basefont", Dict.VERS_LOOSE, (Dict.CM_INLINE | Dict.CM_EMPTY), null, null), new Dict("font", Dict.VERS_LOOSE, Dict.CM_INLINE, ParserImpl.getParseInline(), null), new Dict("bdo", Dict.VERS_HTML40, Dict.CM_INLINE, ParserImpl.getParseInline(), null)};
        }
    }
}