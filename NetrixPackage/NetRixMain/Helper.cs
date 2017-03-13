using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Styles;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Collections;

namespace GuruComponents.Netrix
{
    internal static class Helper
    {
        internal static string BuildUniqueName(IElement tag, IHtmlEditor htmlEditor)
        {
            //// Build a unique name
            IDesignerHost host = (IDesignerHost)htmlEditor.ServiceProvider.GetService(typeof(IDesignerHost));
            GuruComponents.Netrix.Designer.NameCreationService nc = (GuruComponents.Netrix.Designer.NameCreationService)htmlEditor.ServiceProvider.GetService(typeof(System.ComponentModel.Design.Serialization.INameCreationService));
            return nc.CreateName(host.Container, tag.GetType());
        }

        internal static Interop.ELEMENT_TAG_ID GetTagId(string name)
        {
            return (Interop.ELEMENT_TAG_ID) Enum.Parse(typeof (Interop.ELEMENT_TAG_ID), name, true);
        }

        /// <summary>
        /// Returns the tagname for an element from a given element id.
        /// </summary>
        /// <param name="tagID">Element id</param>
        /// <returns>tagname without angle brackets.</returns>
        static internal string GetElementName(Interop.ELEMENT_TAG_ID tagID)
        {
            switch (tagID)
            {
                case Interop.ELEMENT_TAG_ID.A:
                    return "a";
                case Interop.ELEMENT_TAG_ID.ACRONYM:
                    return "acronym";
                case Interop.ELEMENT_TAG_ID.ADDRESS:
                    return "address";
                case Interop.ELEMENT_TAG_ID.APPLET:
                    return "applet";
                case Interop.ELEMENT_TAG_ID.AREA:
                    return "area";
                case Interop.ELEMENT_TAG_ID.B :
                    return "b";
                case Interop.ELEMENT_TAG_ID.BASE:
                    return "base";
                case Interop.ELEMENT_TAG_ID.BASEFONT:
                    return "basefont";
                case Interop.ELEMENT_TAG_ID.BDO:
                    return "bdo";
                case Interop.ELEMENT_TAG_ID.BGSOUND:
                    return "bgsound";
                case Interop.ELEMENT_TAG_ID.BIG:
                    return "big";
                case Interop.ELEMENT_TAG_ID.BLINK:
                    return "blink";
                case Interop.ELEMENT_TAG_ID.BLOCKQUOTE:
                    return "blockquote";
                case Interop.ELEMENT_TAG_ID.BODY:
                    return "body";
                case Interop.ELEMENT_TAG_ID.BR:
                    return "br";
                case Interop.ELEMENT_TAG_ID.BUTTON:
                    return "button";
                case Interop.ELEMENT_TAG_ID.CAPTION:
                    return "caption";
                case Interop.ELEMENT_TAG_ID.CENTER:
                    return "center";
                case Interop.ELEMENT_TAG_ID.CITE:
                    return "cite";
                case Interop.ELEMENT_TAG_ID.CODE:
                    return "code";
                case Interop.ELEMENT_TAG_ID.COL:
                    return "col";
                case Interop.ELEMENT_TAG_ID.COLGROUP:
                    return "colgroup";
                case Interop.ELEMENT_TAG_ID.DD:
                    return "dd";
                case Interop.ELEMENT_TAG_ID.DEL:
                    return "del";
                case Interop.ELEMENT_TAG_ID.DFN:
                    return "dfn";
                case Interop.ELEMENT_TAG_ID.DIR:
                    return "dir";
                case Interop.ELEMENT_TAG_ID.DIV:
                    return "div";
                case Interop.ELEMENT_TAG_ID.DL:
                    return "dl";
                case Interop.ELEMENT_TAG_ID.DT:
                    return "dt";
                case Interop.ELEMENT_TAG_ID.EM:
                    return "em";
                case Interop.ELEMENT_TAG_ID.EMBED:
                    return "embed";
                case Interop.ELEMENT_TAG_ID.FIELDSET:
                    return "fieldset";
                case Interop.ELEMENT_TAG_ID.FONT:
                    return "font";
                case Interop.ELEMENT_TAG_ID.FORM:
                    return "form";
                case Interop.ELEMENT_TAG_ID.FRAME:
                    return "frame";
                case Interop.ELEMENT_TAG_ID.FRAMESET:
                    return "frameset";
                case Interop.ELEMENT_TAG_ID.H1:
                    return "h1";
                case Interop.ELEMENT_TAG_ID.H2:
                    return "h2";
                case Interop.ELEMENT_TAG_ID.H3:
                    return "h3";
                case Interop.ELEMENT_TAG_ID.H4:
                    return "h4";
                case Interop.ELEMENT_TAG_ID.H5:
                    return "h5";
                case Interop.ELEMENT_TAG_ID.H6:
                    return "h6";
                case Interop.ELEMENT_TAG_ID.HEAD:
                    return "head";
                case Interop.ELEMENT_TAG_ID.HR:
                    return "hr";
                case Interop.ELEMENT_TAG_ID.HTML:
                    return "html";
                case Interop.ELEMENT_TAG_ID.I:
                    return "i";
                case Interop.ELEMENT_TAG_ID.IFRAME:
                    return "iframe";
                case Interop.ELEMENT_TAG_ID.IMG:
                    return "img";
                case Interop.ELEMENT_TAG_ID.INPUT:
                    return "input";
                case Interop.ELEMENT_TAG_ID.INS:
                    return "ins";
                case Interop.ELEMENT_TAG_ID.KBD:
                    return "kbd";
                case Interop.ELEMENT_TAG_ID.LABEL:
                    return "label";
                case Interop.ELEMENT_TAG_ID.LEGEND:
                    return "legend";
                case Interop.ELEMENT_TAG_ID.LI:
                    return "li";
                case Interop.ELEMENT_TAG_ID.LINK:
                    return "link";
                case Interop.ELEMENT_TAG_ID.LISTING:
                    return "listing";
                case Interop.ELEMENT_TAG_ID.MAP:
                    return "map";
                case Interop.ELEMENT_TAG_ID.MARQUEE:
                    return "marquee";
                case Interop.ELEMENT_TAG_ID.MENU:
                    return "menu";
                case Interop.ELEMENT_TAG_ID.META:
                    return "meta";
                case Interop.ELEMENT_TAG_ID.NOBR:
                    return "nobr";
                case Interop.ELEMENT_TAG_ID.NOEMBED:
                    return "noembed";
                case Interop.ELEMENT_TAG_ID.NOFRAMES:
                    return "noframes";
                case Interop.ELEMENT_TAG_ID.NOSCRIPT:
                    return "noscript";
                case Interop.ELEMENT_TAG_ID.OBJECT:
                    return "object";
                case Interop.ELEMENT_TAG_ID.OL:
                    return "ol";
                case Interop.ELEMENT_TAG_ID.OPTION:
                    return "option";
                case Interop.ELEMENT_TAG_ID.P:
                    return "p";
                case Interop.ELEMENT_TAG_ID.PARAM:
                    return "param";
                case Interop.ELEMENT_TAG_ID.PRE:
                    return "pre";
                case Interop.ELEMENT_TAG_ID.Q:
                    return "q";
                case Interop.ELEMENT_TAG_ID.RP:
                    return "rp";
                case Interop.ELEMENT_TAG_ID.RT:
                    return "rt";
                case Interop.ELEMENT_TAG_ID.RUBY:
                    return "ruby";
                case Interop.ELEMENT_TAG_ID.S:
                    return "s";
                case Interop.ELEMENT_TAG_ID.SAMP:
                    return "samp";
                case Interop.ELEMENT_TAG_ID.SCRIPT:
                    return "script";
                case Interop.ELEMENT_TAG_ID.SELECT:
                    return "select";
                case Interop.ELEMENT_TAG_ID.SMALL:
                    return "small";
                case Interop.ELEMENT_TAG_ID.SPAN:
                    return "span";
                case Interop.ELEMENT_TAG_ID.STRIKE:
                    return "strike";
                case Interop.ELEMENT_TAG_ID.STRONG:
                    return "strong";
                case Interop.ELEMENT_TAG_ID.STYLE:
                    return "style";
                case Interop.ELEMENT_TAG_ID.SUB:
                    return "sub";
                case Interop.ELEMENT_TAG_ID.SUP:
                    return "sup";
                case Interop.ELEMENT_TAG_ID.TABLE:
                    return "table";
                case Interop.ELEMENT_TAG_ID.TBODY:
                    return "tbody";
                case Interop.ELEMENT_TAG_ID.TD:
                    return "td";
                case Interop.ELEMENT_TAG_ID.TEXTAREA:
                    return "textarea";
                case Interop.ELEMENT_TAG_ID.TFOOT:
                    return "tfoot";
                case Interop.ELEMENT_TAG_ID.TH:
                    return "th";
                case Interop.ELEMENT_TAG_ID.THEAD:
                    return "thead";
                case Interop.ELEMENT_TAG_ID.TITLE:
                    return "title";
                case Interop.ELEMENT_TAG_ID.TR:
                    return "tr";
                case Interop.ELEMENT_TAG_ID.TT:
                    return "tt";
                case Interop.ELEMENT_TAG_ID.U:
                    return "u";
                case Interop.ELEMENT_TAG_ID.UL:
                    return "ul";
                case Interop.ELEMENT_TAG_ID.VAR:
                    return "var";
                case Interop.ELEMENT_TAG_ID.WBR:
                    return "wbr";
                case Interop.ELEMENT_TAG_ID.XMP:
                    return "xmp";
                case Interop.ELEMENT_TAG_ID.OPTGROUP:
                    return "optgroup";
            }
            return String.Empty;
        }


        static internal IStyleRule[] AddRuleFromCollection(string selectorType, Interop.IHTMLStyleSheetRulesCollection rules)
        {            
            ArrayList selector = new ArrayList();
            for (int i = 0; i < rules.GetLength(); i++)
            {
                Interop.IHTMLStyleSheetRule rule = (Interop.IHTMLStyleSheetRule) rules.Item(i);
                if (rule.GetSelectorText().Length == 0) continue;
                IStyleRule sr = new StyleRule(rule);

                if (selectorType == null || selectorType.Equals(String.Empty))
                {
                    selector.Add(sr);
                    continue;
                }
                // first char determines type (. = class, # = id, @ = pseudo)
                if (selectorType == "." && rule.GetSelectorText()[0].Equals('.'))
                {
                    selector.Add(sr); //rule.GetSelectorText().Substring(1)
                    continue;
                }
                if (selectorType == "#" && rule.GetSelectorText()[0].Equals('#'))
                {
                    selector.Add(sr); // rule.GetSelectorText().Substring(1)
                    continue;
                }
                if (selectorType == "@" && rule.GetSelectorText()[0].Equals('@'))
                {
                    selector.Add(sr); //rule.GetSelectorText().Substring(1) 
                    continue;
                }
                if (selectorType.ToLower().Equals(rule.GetSelectorText().ToLower()))
                {
                    selector.Add(sr); //rule.GetSelectorText().Substring(1) 
                    continue;
                }
            }
            IStyleRule[] stylerules = new IStyleRule[selector.Count];
            Array.Copy(selector.ToArray(), stylerules, selector.Count);
            return stylerules;
        }


    }
}
