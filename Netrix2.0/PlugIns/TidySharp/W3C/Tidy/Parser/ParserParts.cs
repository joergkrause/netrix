using System;
using System.Collections.Generic;
using System.Text;

namespace Comzept.Genesis.Tidy
{

    public class ParseDefList : Parser
    {

        public virtual void Parse(Lexer lexer, Node list, short mode)
        {
            Node node, parent;
            TagTable tt = lexer.configuration.tt;

            if ((list.tag.model & Dict.CM_EMPTY) != 0)
                return;

            lexer.insert = -1; /* defer implicit inline start tags */

            while (true)
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);
                if (node == null)
                    break;
                if (node.tag == list.tag && node.type == Node.EndTag)
                {
                    list.closed = true;
                    Node.trimEmptyElement(lexer, list);
                    return;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(list, node))
                    continue;

                if (node.type == Node.TextNode)
                {
                    lexer.ungetToken();
                    node = lexer.inferredTag("dt");
                    Report.Warning(lexer, list, node, Report.MISSING_STARTTAG);
                }

                if (node.tag == null)
                {
                    Report.Warning(lexer, list, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* 
                if this is the end tag for an ancestor element
                then infer end tag for this element
                */
                if (node.type == Node.EndTag)
                {
                    if (node.tag == tt.tagForm)
                    {
                        lexer.badForm = 1;
                        Report.Warning(lexer, list, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    for (parent = list.parent; parent != null; parent = parent.parent)
                    {
                        if (node.tag == parent.tag)
                        {
                            Report.Warning(lexer, list, node, Report.MISSING_ENDTAG_BEFORE);

                            lexer.ungetToken();
                            Node.trimEmptyElement(lexer, list);
                            return;
                        }
                    }
                }

                /* center in a dt or a dl breaks the dl list in two */
                if (node.tag == tt.tagCenter)
                {
                    if (list.content != null)
                        Node.insertNodeAfterElement(list, node);
                    /* trim empty dl list */
                    else
                    {
                        Node.insertNodeBeforeElement(list, node);
                        Node.discardElement(list);
                    }

                    /* and parse contents of center */
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, mode);

                    /* now create a new dl element */
                    list = lexer.inferredTag("dl");
                    Node.insertNodeAfterElement(node, list);
                    continue;
                }

                if (!(node.tag == tt.tagDt || node.tag == tt.tagDd))
                {
                    lexer.ungetToken();

                    if (!((node.tag.model & (Dict.CM_BLOCK | Dict.CM_INLINE)) != 0))
                    {
                        Report.Warning(lexer, list, node, Report.TAG_NOT_ALLOWED_IN);
                        Node.trimEmptyElement(lexer, list);
                        return;
                    }

                    /* if DD appeared directly in BODY then exclude blocks */
                    if (!((node.tag.model & Dict.CM_INLINE) != 0) && lexer.excludeBlocks)
                    {
                        Node.trimEmptyElement(lexer, list);
                        return;
                    }

                    node = lexer.inferredTag("dd");
                    Report.Warning(lexer, list, node, Report.MISSING_STARTTAG);
                }

                if (node.type == Node.EndTag)
                {
                    Report.Warning(lexer, list, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* node should be <DT> or <DD>*/
                Node.insertNodeAtEnd(list, node);
                Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
            }

            Report.Warning(lexer, list, node, Report.MISSING_ENDTAG_FOR);
            Node.trimEmptyElement(lexer, list);
        }
    }


    public class ParsePre : Parser
    {

        public virtual void Parse(Lexer lexer, Node pre, short mode)
        {
            Node node, parent;
            TagTable tt = lexer.configuration.tt;

            if ((pre.tag.model & Dict.CM_EMPTY) != 0)
                return;

            if ((pre.tag.model & Dict.CM_OBSOLETE) != 0)
                Node.coerceNode(lexer, pre, tt.tagPre);

            lexer.inlineDup(null); /* tell lexer to insert inlines if needed */

            while (true)
            {
                node = lexer.GetToken(Lexer.Preformatted);
                if (node == null)
                    break;
                if (node.tag == pre.tag && node.type == Node.EndTag)
                {
                    Node.trimSpaces(lexer, pre);
                    pre.closed = true;
                    Node.trimEmptyElement(lexer, pre);
                    return;
                }

                if (node.tag == tt.tagHtml)
                {
                    if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                        Report.Warning(lexer, pre, node, Report.DISCARDING_UNEXPECTED);

                    continue;
                }

                if (node.type == Node.TextNode)
                {
                    /* if first check for inital newline */
                    if (pre.content == null)
                    {
                        if (node.textarray[node.start] == (sbyte)'\n')
                            ++node.start;

                        if (node.start >= node.end)
                        {
                            continue;
                        }
                    }

                    Node.insertNodeAtEnd(pre, node);
                    continue;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(pre, node))
                    continue;

                /* discard unknown  and PARAM tags */
                if (node.tag == null || node.tag == tt.tagParam)
                {
                    Report.Warning(lexer, pre, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                if (node.tag == tt.tagP)
                {
                    if (node.type == Node.StartTag)
                    {
                        Report.Warning(lexer, pre, node, Report.USING_BR_INPLACE_OF);

                        /* trim white space before <p> in <pre>*/
                        Node.trimSpaces(lexer, pre);

                        /* coerce both <p> and </p> to <br/> */
                        Node.coerceNode(lexer, node, tt.tagBr);
                        Node.insertNodeAtEnd(pre, node);
                    }
                    else
                    {
                        Report.Warning(lexer, pre, node, Report.DISCARDING_UNEXPECTED);
                    }
                    continue;
                }

                if ((node.tag.model & Dict.CM_HEAD) != 0 && !((node.tag.model & Dict.CM_BLOCK) != 0))
                {
                    Comzept.Genesis.Tidy.ParserImpl.moveToHead(lexer, pre, node);
                    continue;
                }

                /* 
                if this is the end tag for an ancestor element
                then infer end tag for this element
                */
                if (node.type == Node.EndTag)
                {
                    if (node.tag == tt.tagForm)
                    {
                        lexer.badForm = 1;
                        Report.Warning(lexer, pre, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    for (parent = pre.parent; parent != null; parent = parent.parent)
                    {
                        if (node.tag == parent.tag)
                        {
                            Report.Warning(lexer, pre, node, Report.MISSING_ENDTAG_BEFORE);

                            lexer.ungetToken();
                            Node.trimSpaces(lexer, pre);
                            Node.trimEmptyElement(lexer, pre);
                            return;
                        }
                    }
                }

                /* what about head content, HEAD, BODY tags etc? */
                if (!((node.tag.model & Dict.CM_INLINE) != 0))
                {
                    if (node.type != Node.StartTag)
                    {
                        Report.Warning(lexer, pre, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    Report.Warning(lexer, pre, node, Report.MISSING_ENDTAG_BEFORE);
                    lexer.excludeBlocks = true;

                    /* check if we need to infer a container */
                    if ((node.tag.model & Dict.CM_LIST) != 0)
                    {
                        lexer.ungetToken();
                        node = lexer.inferredTag("ul");
                        Node.addClass(node, "noindent");
                    }
                    else if ((node.tag.model & Dict.CM_DEFLIST) != 0)
                    {
                        lexer.ungetToken();
                        node = lexer.inferredTag("dl");
                    }
                    else if ((node.tag.model & Dict.CM_TABLE) != 0)
                    {
                        lexer.ungetToken();
                        node = lexer.inferredTag("table");
                    }

                    Node.insertNodeAfterElement(pre, node);
                    pre = lexer.inferredTag("pre");
                    Node.insertNodeAfterElement(node, pre);
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
                    lexer.excludeBlocks = false;
                    continue;
                }
                /*
                if (!((node.tag.model & Dict.CM_INLINE) != 0))
                {
                Report.warning(lexer, pre, node, Report.MISSING_ENDTAG_BEFORE);
                lexer.ungetToken();
                return;
                }
                */
                if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                {
                    /* trim white space before <br/> */
                    if (node.tag == tt.tagBr)
                        Node.trimSpaces(lexer, pre);

                    Node.insertNodeAtEnd(pre, node);
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.Preformatted);
                    continue;
                }

                /* discard unexpected tags */
                Report.Warning(lexer, pre, node, Report.DISCARDING_UNEXPECTED);
            }

            Report.Warning(lexer, pre, node, Report.MISSING_ENDTAG_FOR);
            Node.trimEmptyElement(lexer, pre);
        }
    }



    public class ParseBlock : Parser
    {

        public virtual void Parse(Lexer lexer, Node element, short mode)
        /*
        element is node created by the lexer
        upon seeing the start tag, or by the
        parser when the start tag is inferred
        */
        {
            Node node, parent;
            bool checkstack;
            int istackbase = 0;
            TagTable tt = lexer.configuration.tt;

            checkstack = true;

            if ((element.tag.model & Dict.CM_EMPTY) != 0)
                return;

            if (element.tag == tt.tagForm && element.isDescendantOf(tt.tagForm))
                Report.Warning(lexer, element, null, Report.ILLEGAL_NESTING);

            /*
            InlineDup() asks the lexer to insert inline emphasis tags
            currently pushed on the istack, but take care to avoid
            propagating inline emphasis inside OBJECT or APPLET.
            For these elements a fresh inline stack context is created
            and disposed of upon reaching the end of the element.
            They thus behave like table cells in this respect.
            */
            if ((element.tag.model & Dict.CM_OBJECT) != 0)
            {
                istackbase = lexer.istackbase;
                lexer.istackbase = lexer.istack.Count;
            }

            if (!((element.tag.model & Dict.CM_MIXED) != 0))
                lexer.inlineDup(null);

            mode = Lexer.IgnoreWhitespace;

            while (true)
            {
                node = lexer.GetToken(mode);
                if (node == null)
                    break;
                /* end tag for this element */
                if (node.type == Node.EndTag && node.tag != null && (node.tag == element.tag || element.was == node.tag))
                {

                    if ((element.tag.model & Dict.CM_OBJECT) != 0)
                    {
                        /* pop inline stack */
                        while (lexer.istack.Count > lexer.istackbase)
                            lexer.popInline(null);
                        lexer.istackbase = istackbase;
                    }

                    element.closed = true;
                    Node.trimSpaces(lexer, element);
                    Node.trimEmptyElement(lexer, element);
                    return;
                }

                if (node.tag == tt.tagHtml || node.tag == tt.tagHead || node.tag == tt.tagBody)
                {
                    if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                        Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);

                    continue;
                }

                if (node.type == Node.EndTag)
                {
                    if (node.tag == null)
                    {
                        Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);

                        continue;
                    }
                    else if (node.tag == tt.tagBr)
                        node.type = Node.StartTag;
                    else if (node.tag == tt.tagP)
                    {
                        Node.coerceNode(lexer, node, tt.tagBr);
                        Node.insertNodeAtEnd(element, node);
                        node = lexer.inferredTag("br");
                    }
                    else
                    {
                        /* 
                        if this is the end tag for an ancestor element
                        then infer end tag for this element
                        */
                        for (parent = element.parent; parent != null; parent = parent.parent)
                        {
                            if (node.tag == parent.tag)
                            {
                                if (!((element.tag.model & Dict.CM_OPT) != 0))
                                    Report.Warning(lexer, element, node, Report.MISSING_ENDTAG_BEFORE);

                                lexer.ungetToken();

                                if ((element.tag.model & Dict.CM_OBJECT) != 0)
                                {
                                    /* pop inline stack */
                                    while (lexer.istack.Count > lexer.istackbase)
                                        lexer.popInline(null);
                                    lexer.istackbase = istackbase;
                                }

                                Node.trimSpaces(lexer, element);
                                Node.trimEmptyElement(lexer, element);
                                return;
                            }
                        }
                        /* special case </tr> etc. for stuff moved in front of table */
                        if (lexer.exiled && node.tag.model != 0 && (node.tag.model & Dict.CM_TABLE) != 0)
                        {
                            lexer.ungetToken();
                            Node.trimSpaces(lexer, element);
                            Node.trimEmptyElement(lexer, element);
                            return;
                        }
                    }
                }

                /* mixed content model permits text */
                if (node.type == Node.TextNode)
                {
                    bool iswhitenode = false;

                    if (node.type == Node.TextNode && node.end <= node.start + 1 && lexer.lexbuf[node.start] == (sbyte)' ')
                        iswhitenode = true;

                    if (lexer.configuration.EncloseBlockText && !iswhitenode)
                    {
                        lexer.ungetToken();
                        node = lexer.inferredTag("p");
                        Node.insertNodeAtEnd(element, node);
                        Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.MixedContent);
                        continue;
                    }

                    if (checkstack)
                    {
                        checkstack = false;

                        if (!((element.tag.model & Dict.CM_MIXED) != 0))
                        {
                            if (lexer.inlineDup(node) > 0)
                                continue;
                        }
                    }

                    Node.insertNodeAtEnd(element, node);
                    mode = Lexer.MixedContent;
                    /*
                    HTML4 strict doesn't allow mixed content for
                    elements with %block; as their content model
                    */
                    lexer.versions &= ~Dict.VERS_HTML40_STRICT;
                    continue;
                }

                if (Node.insertMisc(element, node))
                    continue;

                /* allow PARAM elements? */
                if (node.tag == tt.tagParam)
                {
                    if (((element.tag.model & Dict.CM_PARAM) != 0) && (node.type == Node.StartTag || node.type == Node.StartEndTag))
                    {
                        Node.insertNodeAtEnd(element, node);
                        continue;
                    }

                    /* otherwise discard it */
                    Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* allow AREA elements? */
                if (node.tag == tt.tagArea)
                {
                    if ((element.tag == tt.tagMap) && (node.type == Node.StartTag || node.type == Node.StartEndTag))
                    {
                        Node.insertNodeAtEnd(element, node);
                        continue;
                    }

                    /* otherwise discard it */
                    Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* ignore unknown start/end tags */
                if (node.tag == null)
                {
                    Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /*
                Allow Dict.CM_INLINE elements here.
					
                Allow Dict.CM_BLOCK elements here unless
                lexer.excludeBlocks is yes.
					
                LI and DD are special cased.
					
                Otherwise infer end tag for this element.
                */

                if (!((node.tag.model & Dict.CM_INLINE) != 0))
                {
                    if (node.type != Node.StartTag && node.type != Node.StartEndTag)
                    {
                        Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    if (element.tag == tt.tagTd || element.tag == tt.tagTh)
                    {
                        /* if parent is a table cell, avoid inferring the end of the cell */

                        if ((node.tag.model & Dict.CM_HEAD) != 0)
                        {
                            Comzept.Genesis.Tidy.ParserImpl.moveToHead(lexer, element, node);
                            continue;
                        }

                        if ((node.tag.model & Dict.CM_LIST) != 0)
                        {
                            lexer.ungetToken();
                            node = lexer.inferredTag("ul");
                            Node.addClass(node, "noindent");
                            lexer.excludeBlocks = true;
                        }
                        else if ((node.tag.model & Dict.CM_DEFLIST) != 0)
                        {
                            lexer.ungetToken();
                            node = lexer.inferredTag("dl");
                            lexer.excludeBlocks = true;
                        }

                        /* infer end of current table cell */
                        if (!((node.tag.model & Dict.CM_BLOCK) != 0))
                        {
                            lexer.ungetToken();
                            Node.trimSpaces(lexer, element);
                            Node.trimEmptyElement(lexer, element);
                            return;
                        }
                    }
                    else if ((node.tag.model & Dict.CM_BLOCK) != 0)
                    {
                        if (lexer.excludeBlocks)
                        {
                            if (!((element.tag.model & Dict.CM_OPT) != 0))
                                Report.Warning(lexer, element, node, Report.MISSING_ENDTAG_BEFORE);

                            lexer.ungetToken();

                            if ((element.tag.model & Dict.CM_OBJECT) != 0)
                                lexer.istackbase = istackbase;

                            Node.trimSpaces(lexer, element);
                            Node.trimEmptyElement(lexer, element);
                            return;
                        }
                    }
                    /* things like list items */
                    else
                    {
                        if (!((element.tag.model & Dict.CM_OPT) != 0) && !element.implicit_Renamed)
                            Report.Warning(lexer, element, node, Report.MISSING_ENDTAG_BEFORE);

                        if ((node.tag.model & Dict.CM_HEAD) != 0)
                        {
                            Comzept.Genesis.Tidy.ParserImpl.moveToHead(lexer, element, node);
                            continue;
                        }

                        lexer.ungetToken();

                        if ((node.tag.model & Dict.CM_LIST) != 0)
                        {
                            if (element.parent != null && element.parent.tag != null && element.parent.tag.parser == Comzept.Genesis.Tidy.ParserImpl.ParseList)
                            {
                                Node.trimSpaces(lexer, element);
                                Node.trimEmptyElement(lexer, element);
                                return;
                            }

                            node = lexer.inferredTag("ul");
                            Node.addClass(node, "noindent");
                        }
                        else if ((node.tag.model & Dict.CM_DEFLIST) != 0)
                        {
                            if (element.parent.tag == tt.tagDl)
                            {
                                Node.trimSpaces(lexer, element);
                                Node.trimEmptyElement(lexer, element);
                                return;
                            }

                            node = lexer.inferredTag("dl");
                        }
                        else if ((node.tag.model & Dict.CM_TABLE) != 0 || (node.tag.model & Dict.CM_ROW) != 0)
                        {
                            node = lexer.inferredTag("table");
                        }
                        else if ((element.tag.model & Dict.CM_OBJECT) != 0)
                        {
                            /* pop inline stack */
                            while (lexer.istack.Count > lexer.istackbase)
                                lexer.popInline(null);
                            lexer.istackbase = istackbase;
                            Node.trimSpaces(lexer, element);
                            Node.trimEmptyElement(lexer, element);
                            return;
                        }
                        else
                        {
                            Node.trimSpaces(lexer, element);
                            Node.trimEmptyElement(lexer, element);
                            return;
                        }
                    }
                }

                /* parse known element */
                if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                {
                    if ((node.tag.model & Dict.CM_INLINE) != 0)
                    {
                        if (checkstack && !node.implicit_Renamed)
                        {
                            checkstack = false;

                            if (lexer.inlineDup(node) > 0)
                                continue;
                        }

                        mode = Lexer.MixedContent;
                    }
                    else
                    {
                        checkstack = true;
                        mode = Lexer.IgnoreWhitespace;
                    }

                    /* trim white space before <br/> */
                    if (node.tag == tt.tagBr)
                        Node.trimSpaces(lexer, element);

                    Node.insertNodeAtEnd(element, node);

                    if (node.implicit_Renamed)
                        Report.Warning(lexer, element, node, Report.INSERTING_TAG);

                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
                    continue;
                }

                /* discard unexpected tags */
                if (node.type == Node.EndTag)
                    lexer.popInline(node); /* if inline end tag */

                Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
            }

            if (!((element.tag.model & Dict.CM_OPT) != 0))
                Report.Warning(lexer, element, node, Report.MISSING_ENDTAG_FOR);

            if ((element.tag.model & Dict.CM_OBJECT) != 0)
            {
                /* pop inline stack */
                while (lexer.istack.Count > lexer.istackbase)
                    lexer.popInline(null);
                lexer.istackbase = istackbase;
            }

            Node.trimSpaces(lexer, element);
            Node.trimEmptyElement(lexer, element);
        }
    }


    public class ParseTableTag : Parser
    {

        public virtual void Parse(Lexer lexer, Node table, short mode)
        {
            Node node, parent;
            int istackbase;
            TagTable tt = lexer.configuration.tt;

            lexer.deferDup();
            istackbase = lexer.istackbase;
            lexer.istackbase = lexer.istack.Count;

            while (true)
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);
                if (node == null)
                    break;
                if (node.tag == table.tag && node.type == Node.EndTag)
                {
                    lexer.istackbase = istackbase;
                    table.closed = true;
                    Node.trimEmptyElement(lexer, table);
                    return;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(table, node))
                    continue;

                /* discard unknown tags */
                if (node.tag == null && node.type != Node.TextNode)
                {
                    Report.Warning(lexer, table, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* if TD or TH or text or inline or block then infer <TR> */

                if (node.type != Node.EndTag)
                {
                    if (node.tag == tt.tagTd || node.tag == tt.tagTh || node.tag == tt.tagTable)
                    {
                        lexer.ungetToken();
                        node = lexer.inferredTag("tr");
                        Report.Warning(lexer, table, node, Report.MISSING_STARTTAG);
                    }
                    else if (node.type == Node.TextNode || (node.tag.model & (Dict.CM_BLOCK | Dict.CM_INLINE)) != 0)
                    {
                        Node.insertNodeBeforeElement(table, node);
                        Report.Warning(lexer, table, node, Report.TAG_NOT_ALLOWED_IN);
                        lexer.exiled = true;

                        /* AQ: TODO
                        Line 2040 of parser.c (13 Jan 2000) reads as follows:
                        if (!node->type == TextNode)
                        This will always evaluate to false.
                        This has been reported to Dave Raggett dsr@w3.org
                        */
                        //Should be?: if (!(node.type == Node.TextNode))
                        if (false)
                            Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);

                        lexer.exiled = false;
                        continue;
                    }
                    else if ((node.tag.model & Dict.CM_HEAD) != 0)
                    {
                        Comzept.Genesis.Tidy.ParserImpl.moveToHead(lexer, table, node);
                        continue;
                    }
                }

                /* 
                if this is the end tag for an ancestor element
                then infer end tag for this element
                */
                if (node.type == Node.EndTag)
                {
                    if (node.tag == tt.tagForm)
                    {
                        lexer.badForm = 1;
                        Report.Warning(lexer, table, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    if (node.tag != null && (node.tag.model & (Dict.CM_TABLE | Dict.CM_ROW)) != 0)
                    {
                        Report.Warning(lexer, table, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    for (parent = table.parent; parent != null; parent = parent.parent)
                    {
                        if (node.tag == parent.tag)
                        {
                            Report.Warning(lexer, table, node, Report.MISSING_ENDTAG_BEFORE);
                            lexer.ungetToken();
                            lexer.istackbase = istackbase;
                            Node.trimEmptyElement(lexer, table);
                            return;
                        }
                    }
                }

                if (!((node.tag.model & Dict.CM_TABLE) != 0))
                {
                    lexer.ungetToken();
                    Report.Warning(lexer, table, node, Report.TAG_NOT_ALLOWED_IN);
                    lexer.istackbase = istackbase;
                    Node.trimEmptyElement(lexer, table);
                    return;
                }

                if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                {
                    Node.insertNodeAtEnd(table, node); ;
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
                    continue;
                }

                /* discard unexpected text nodes and end tags */
                Report.Warning(lexer, table, node, Report.DISCARDING_UNEXPECTED);
            }

            Report.Warning(lexer, table, node, Report.MISSING_ENDTAG_FOR);
            Node.trimEmptyElement(lexer, table);
            lexer.istackbase = istackbase;
        }
    }


    public class ParseColGroup : Parser
    {

        public virtual void Parse(Lexer lexer, Node colgroup, short mode)
        {
            Node node, parent;
            TagTable tt = lexer.configuration.tt;

            if ((colgroup.tag.model & Dict.CM_EMPTY) != 0)
                return;

            while (true)
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);
                if (node == null)
                    break;
                if (node.tag == colgroup.tag && node.type == Node.EndTag)
                {
                    colgroup.closed = true;
                    return;
                }

                /* 
                if this is the end tag for an ancestor element
                then infer end tag for this element
                */
                if (node.type == Node.EndTag)
                {
                    if (node.tag == tt.tagForm)
                    {
                        lexer.badForm = 1;
                        Report.Warning(lexer, colgroup, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    for (parent = colgroup.parent; parent != null; parent = parent.parent)
                    {

                        if (node.tag == parent.tag)
                        {
                            lexer.ungetToken();
                            return;
                        }
                    }
                }

                if (node.type == Node.TextNode)
                {
                    lexer.ungetToken();
                    return;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(colgroup, node))
                    continue;

                /* discard unknown tags */
                if (node.tag == null)
                {
                    Report.Warning(lexer, colgroup, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                if (node.tag != tt.tagCol)
                {
                    lexer.ungetToken();
                    return;
                }

                if (node.type == Node.EndTag)
                {
                    Report.Warning(lexer, colgroup, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* node should be <COL> */
                Node.insertNodeAtEnd(colgroup, node);
                Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
            }
        }
    }


    public class ParseRowGroup : Parser
    {

        public virtual void Parse(Lexer lexer, Node rowgroup, short mode)
        {
            Node node, parent;
            TagTable tt = lexer.configuration.tt;

            if ((rowgroup.tag.model & Dict.CM_EMPTY) != 0)
                return;

            while (true)
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);
                if (node == null)
                    break;
                if (node.tag == rowgroup.tag)
                {
                    if (node.type == Node.EndTag)
                    {
                        rowgroup.closed = true;
                        Node.trimEmptyElement(lexer, rowgroup);
                        return;
                    }

                    lexer.ungetToken();
                    return;
                }

                /* if </table> infer end tag */
                if (node.tag == tt.tagTable && node.type == Node.EndTag)
                {
                    lexer.ungetToken();
                    Node.trimEmptyElement(lexer, rowgroup);
                    return;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(rowgroup, node))
                    continue;

                /* discard unknown tags */
                if (node.tag == null && node.type != Node.TextNode)
                {
                    Report.Warning(lexer, rowgroup, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /*
                if TD or TH then infer <TR>
                if text or inline or block move before table
                if head content move to head
                */

                if (node.type != Node.EndTag)
                {
                    if (node.tag == tt.tagTd || node.tag == tt.tagTh)
                    {
                        lexer.ungetToken();
                        node = lexer.inferredTag("tr");
                        Report.Warning(lexer, rowgroup, node, Report.MISSING_STARTTAG);
                    }
                    else if (node.type == Node.TextNode || (node.tag.model & (Dict.CM_BLOCK | Dict.CM_INLINE)) != 0)
                    {
                        Node.moveBeforeTable(rowgroup, node, tt);
                        Report.Warning(lexer, rowgroup, node, Report.TAG_NOT_ALLOWED_IN);
                        lexer.exiled = true;

                        if (node.type != Node.TextNode)
                            Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);

                        lexer.exiled = false;
                        continue;
                    }
                    else if ((node.tag.model & Dict.CM_HEAD) != 0)
                    {
                        Report.Warning(lexer, rowgroup, node, Report.TAG_NOT_ALLOWED_IN);
                        Comzept.Genesis.Tidy.ParserImpl.moveToHead(lexer, rowgroup, node);
                        continue;
                    }
                }

                /* 
                if this is the end tag for ancestor element
                then infer end tag for this element
                */
                if (node.type == Node.EndTag)
                {
                    if (node.tag == tt.tagForm)
                    {
                        lexer.badForm = 1;
                        Report.Warning(lexer, rowgroup, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    if (node.tag == tt.tagTr || node.tag == tt.tagTd || node.tag == tt.tagTh)
                    {
                        Report.Warning(lexer, rowgroup, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    for (parent = rowgroup.parent; parent != null; parent = parent.parent)
                    {
                        if (node.tag == parent.tag)
                        {
                            lexer.ungetToken();
                            Node.trimEmptyElement(lexer, rowgroup);
                            return;
                        }
                    }
                }

                /*
                if THEAD, TFOOT or TBODY then implied end tag
					
                */
                if ((node.tag.model & Dict.CM_ROWGRP) != 0)
                {
                    if (node.type != Node.EndTag)
                        lexer.ungetToken();

                    Node.trimEmptyElement(lexer, rowgroup);
                    return;
                }

                if (node.type == Node.EndTag)
                {
                    Report.Warning(lexer, rowgroup, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                if (!(node.tag == tt.tagTr))
                {
                    node = lexer.inferredTag("tr");
                    Report.Warning(lexer, rowgroup, node, Report.MISSING_STARTTAG);
                    lexer.ungetToken();
                }

                /* node should be <TR> */
                Node.insertNodeAtEnd(rowgroup, node);
                Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
            }

            Node.trimEmptyElement(lexer, rowgroup);
        }
    }


    public class ParseRow : Parser
    {

        public virtual void Parse(Lexer lexer, Node row, short mode)
        {
            Node node, parent;
            bool exclude_state;
            TagTable tt = lexer.configuration.tt;

            if ((row.tag.model & Dict.CM_EMPTY) != 0)
                return;

            while (true)
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);
                if (node == null)
                    break;
                if (node.tag == row.tag)
                {
                    if (node.type == Node.EndTag)
                    {
                        row.closed = true;
                        Node.fixEmptyRow(lexer, row);
                        return;
                    }

                    lexer.ungetToken();
                    Node.fixEmptyRow(lexer, row);
                    return;
                }

                /* 
                if this is the end tag for an ancestor element
                then infer end tag for this element
                */
                if (node.type == Node.EndTag)
                {
                    if (node.tag == tt.tagForm)
                    {
                        lexer.badForm = 1;
                        Report.Warning(lexer, row, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    if (node.tag == tt.tagTd || node.tag == tt.tagTh)
                    {
                        Report.Warning(lexer, row, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    for (parent = row.parent; parent != null; parent = parent.parent)
                    {
                        if (node.tag == parent.tag)
                        {
                            lexer.ungetToken();
                            Node.trimEmptyElement(lexer, row);
                            return;
                        }
                    }
                }

                /* deal with comments etc. */
                if (Node.insertMisc(row, node))
                    continue;

                /* discard unknown tags */
                if (node.tag == null && node.type != Node.TextNode)
                {
                    Report.Warning(lexer, row, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* discard unexpected <table> element */
                if (node.tag == tt.tagTable)
                {
                    Report.Warning(lexer, row, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* THEAD, TFOOT or TBODY */
                if (node.tag != null && (node.tag.model & Dict.CM_ROWGRP) != 0)
                {
                    lexer.ungetToken();
                    Node.trimEmptyElement(lexer, row);
                    return;
                }

                if (node.type == Node.EndTag)
                {
                    Report.Warning(lexer, row, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /*
                if text or inline or block move before table
                if head content move to head
                */

                if (node.type != Node.EndTag)
                {
                    if (node.tag == tt.tagForm)
                    {
                        lexer.ungetToken();
                        node = lexer.inferredTag("td");
                        Report.Warning(lexer, row, node, Report.MISSING_STARTTAG);
                    }
                    else if (node.type == Node.TextNode || (node.tag.model & (Dict.CM_BLOCK | Dict.CM_INLINE)) != 0)
                    {
                        Node.moveBeforeTable(row, node, tt);
                        Report.Warning(lexer, row, node, Report.TAG_NOT_ALLOWED_IN);
                        lexer.exiled = true;

                        if (node.type != Node.TextNode)
                            Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);

                        lexer.exiled = false;
                        continue;
                    }
                    else if ((node.tag.model & Dict.CM_HEAD) != 0)
                    {
                        Report.Warning(lexer, row, node, Report.TAG_NOT_ALLOWED_IN);
                        Comzept.Genesis.Tidy.ParserImpl.moveToHead(lexer, row, node);
                        continue;
                    }
                }

                if (!(node.tag == tt.tagTd || node.tag == tt.tagTh))
                {
                    Report.Warning(lexer, row, node, Report.TAG_NOT_ALLOWED_IN);
                    continue;
                }

                /* node should be <TD> or <TH> */
                Node.insertNodeAtEnd(row, node);
                exclude_state = lexer.excludeBlocks;
                lexer.excludeBlocks = false;
                Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
                lexer.excludeBlocks = exclude_state;

                /* pop inline stack */

                while (lexer.istack.Count > lexer.istackbase)
                    lexer.popInline(null);
            }

            Node.trimEmptyElement(lexer, row);
        }
    }


    public class ParseNoFrames : Parser
    {

        public virtual void Parse(Lexer lexer, Node noframes, short mode)
        {
            Node node;
            bool checkstack;
            TagTable tt = lexer.configuration.tt;

            lexer.badAccess |= Report.USING_NOFRAMES;
            mode = Lexer.IgnoreWhitespace;
            checkstack = true;

            while (true)
            {
                node = lexer.GetToken(mode);
                if (node == null)
                    break;
                if (node.tag == noframes.tag && node.type == Node.EndTag)
                {
                    noframes.closed = true;
                    Node.trimSpaces(lexer, noframes);
                    return;
                }

                if ((node.tag == tt.tagFrame || node.tag == tt.tagFrameset))
                {
                    Report.Warning(lexer, noframes, node, Report.MISSING_ENDTAG_BEFORE);
                    Node.trimSpaces(lexer, noframes);
                    lexer.ungetToken();
                    return;
                }

                if (node.tag == tt.tagHtml)
                {
                    if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                        Report.Warning(lexer, noframes, node, Report.DISCARDING_UNEXPECTED);

                    continue;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(noframes, node))
                    continue;

                if (node.tag == tt.tagBody && node.type == Node.StartTag)
                {
                    Node.insertNodeAtEnd(noframes, node);
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
                    continue;
                }

                /* implicit body element inferred */
                if (node.type == Node.TextNode || node.tag != null)
                {
                    lexer.ungetToken();
                    node = lexer.inferredTag("body");
                    if (lexer.configuration.XmlOut)
                        Report.Warning(lexer, noframes, node, Report.INSERTING_TAG);
                    Node.insertNodeAtEnd(noframes, node);
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
                    continue;
                }
                /* discard unexpected end tags */
                Report.Warning(lexer, noframes, node, Report.DISCARDING_UNEXPECTED);
            }

            Report.Warning(lexer, noframes, node, Report.MISSING_ENDTAG_FOR);
        }
    }

    /// <summary>
    /// Parse selected parts.
    /// </summary>
    public class ParseSelect : Parser
    {

        /// <summary>
        /// Start Parsing using given lexer.
        /// </summary>
        /// <param name="lexer"></param>
        /// <param name="field"></param>
        /// <param name="mode"></param>
        public virtual void Parse(Lexer lexer, Node field, short mode)
        {
            Node node;
            TagTable tt = lexer.configuration.tt;

            lexer.insert = -1; /* defer implicit inline start tags */

            while (true)
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);
                if (node == null)
                    break;
                if (node.tag == field.tag && node.type == Node.EndTag)
                {
                    field.closed = true;
                    Node.trimSpaces(lexer, field);
                    return;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(field, node))
                    continue;

                if (node.type == Node.StartTag && (node.tag == tt.tagOption || node.tag == tt.tagOptgroup || node.tag == tt.tagScript))
                {
                    Node.insertNodeAtEnd(field, node);
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
                    continue;
                }

                /* discard unexpected tags */
                Report.Warning(lexer, field, node, Report.DISCARDING_UNEXPECTED);
            }

            Report.Warning(lexer, field, node, Report.MISSING_ENDTAG_FOR);
        }
    }

    /// <summary>
    /// Parse text 
    /// </summary>
    public class ParseText : Parser
    {

        /// <summary>
        /// Start text parser.
        /// </summary>
        /// <param name="lexer"></param>
        /// <param name="field"></param>
        /// <param name="mode"></param>
        public virtual void Parse(Lexer lexer, Node field, short mode)
        {
            Node node;
            TagTable tt = lexer.configuration.tt;

            lexer.insert = -1; /* defer implicit inline start tags */

            if (field.tag == tt.tagTextarea)
                mode = Lexer.Preformatted;

            while (true)
            {
                node = lexer.GetToken(mode);
                if (node == null)
                    break;
                if (node.tag == field.tag && node.type == Node.EndTag)
                {
                    field.closed = true;
                    Node.trimSpaces(lexer, field);
                    return;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(field, node))
                    continue;

                if (node.type == Node.TextNode)
                {
                    /* only called for 1st child */
                    if (field.content == null && !((mode & Lexer.Preformatted) != 0))
                        Node.trimSpaces(lexer, field);

                    if (node.start >= node.end)
                    {
                        continue;
                    }

                    Node.insertNodeAtEnd(field, node);
                    continue;
                }

                if (node.tag == tt.tagFont)
                {
                    Report.Warning(lexer, field, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* terminate element on other tags */
                if (!((field.tag.model & Dict.CM_OPT) != 0))
                    Report.Warning(lexer, field, node, Report.MISSING_ENDTAG_BEFORE);

                lexer.ungetToken();
                Node.trimSpaces(lexer, field);
                return;
            }

            if (!((field.tag.model & Dict.CM_OPT) != 0))
                Report.Warning(lexer, field, node, Report.MISSING_ENDTAG_FOR);
        }
    }

    /// <summary>
    /// Parse an option group.
    /// </summary>
    public class ParseOptGroup : Parser
    {

        /// <summary>
        /// Start parsing.
        /// </summary>
        /// <param name="lexer"></param>
        /// <param name="field"></param>
        /// <param name="mode"></param>
        public virtual void Parse(Lexer lexer, Node field, short mode)
        {
            Node node;
            TagTable tt = lexer.configuration.tt;

            lexer.insert = -1; /* defer implicit inline start tags */

            while (true)
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);
                if (node == null)
                    break;
                if (node.tag == field.tag && node.type == Node.EndTag)
                {
                    field.closed = true;
                    Node.trimSpaces(lexer, field);
                    return;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(field, node))
                    continue;

                if (node.type == Node.StartTag && (node.tag == tt.tagOption || node.tag == tt.tagOptgroup))
                {
                    if (node.tag == tt.tagOptgroup)
                        Report.Warning(lexer, field, node, Report.CANT_BE_NESTED);

                    Node.insertNodeAtEnd(field, node);
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.MixedContent);
                    continue;
                }

                /* discard unexpected tags */
                Report.Warning(lexer, field, node, Report.DISCARDING_UNEXPECTED);
            }
        }
    }

    public class ParseHTML : Parser
    {

        public virtual void Parse(Lexer lexer, Node html, short mode)
        {
            Node node, head;
            Node frameset = null;
            Node noframes = null;

            lexer.configuration.XmlTags = false;
            lexer.seenBodyEndTag = 0;
            TagTable tt = lexer.configuration.tt;

            for (; ; )
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);

                if (node == null)
                {
                    node = lexer.inferredTag("head");
                    break;
                }

                if (node.tag == tt.tagHead)
                    break;

                if (node.tag == html.tag && node.type == Node.EndTag)
                {
                    Report.Warning(lexer, html, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(html, node))
                    continue;

                lexer.ungetToken();
                node = lexer.inferredTag("head");
                break;
            }

            head = node;
            Node.insertNodeAtEnd(html, head);
            Comzept.Genesis.Tidy.ParserImpl.getParseHead().Parse(lexer, head, mode);

            for (; ; )
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);

                if (node == null)
                {
                    if (frameset == null)
                        /* create an empty body */
                        node = lexer.inferredTag("body");

                    return;
                }

                /* robustly handle html tags */
                if (node.tag == html.tag)
                {
                    if (node.type != Node.StartTag && frameset == null)
                        Report.Warning(lexer, html, node, Report.DISCARDING_UNEXPECTED);

                    continue;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(html, node))
                    continue;

                /* if frameset document coerce <body> to <noframes> */
                if (node.tag == tt.tagBody)
                {
                    if (node.type != Node.StartTag)
                    {
                        Report.Warning(lexer, html, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    if (frameset != null)
                    {
                        lexer.ungetToken();

                        if (noframes == null)
                        {
                            noframes = lexer.inferredTag("noframes");
                            Node.insertNodeAtEnd(frameset, noframes);
                            Report.Warning(lexer, html, noframes, Report.INSERTING_TAG);
                        }

                        Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, noframes, mode);
                        continue;
                    }

                    break; /* to parse body */
                }

                /* flag an error if we see more than one frameset */
                if (node.tag == tt.tagFrameset)
                {
                    if (node.type != Node.StartTag)
                    {
                        Report.Warning(lexer, html, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    if (frameset != null)
                        Report.error(lexer, html, node, Report.DUPLICATE_FRAMESET);
                    else
                        frameset = node;

                    Node.insertNodeAtEnd(html, node);
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, mode);

                    /*
                    see if it includes a noframes element so
                    that we can merge subsequent noframes elements
                    */

                    for (node = frameset.content; node != null; node = node.next)
                    {
                        if (node.tag == tt.tagNoframes)
                            noframes = node;
                    }
                    continue;
                }

                /* if not a frameset document coerce <noframes> to <body> */
                if (node.tag == tt.tagNoframes)
                {
                    if (node.type != Node.StartTag)
                    {
                        Report.Warning(lexer, html, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    if (frameset == null)
                    {
                        Report.Warning(lexer, html, node, Report.DISCARDING_UNEXPECTED);
                        node = lexer.inferredTag("body");
                        break;
                    }

                    if (noframes == null)
                    {
                        noframes = node;
                        Node.insertNodeAtEnd(frameset, noframes);
                    }

                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, noframes, mode);
                    continue;
                }

                if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                {
                    if (node.tag != null && (node.tag.model & Dict.CM_HEAD) != 0)
                    {
                        Comzept.Genesis.Tidy.ParserImpl.moveToHead(lexer, html, node);
                        continue;
                    }
                }

                lexer.ungetToken();

                /* insert other content into noframes element */

                if (frameset != null)
                {
                    if (noframes == null)
                    {
                        noframes = lexer.inferredTag("noframes");
                        Node.insertNodeAtEnd(frameset, noframes);
                    }
                    else
                        Report.Warning(lexer, html, node, Report.NOFRAMES_CONTENT);

                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, noframes, mode);
                    continue;
                }

                node = lexer.inferredTag("body");
                break;
            }

            /* node must be body */

            Node.insertNodeAtEnd(html, node);
            Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, mode);
        }
    }


    public class ParseHead : Parser
    {

        public virtual void Parse(Lexer lexer, Node head, short mode)
        {
            Node node;
            int HasTitle = 0;
            int HasBase = 0;
            TagTable tt = lexer.configuration.tt;

            while (true)
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);
                if (node == null)
                    break;
                if (node.tag == head.tag && node.type == Node.EndTag)
                {
                    head.closed = true;
                    break;
                }

                if (node.type == Node.TextNode)
                {
                    lexer.ungetToken();
                    break;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(head, node))
                    continue;

                if (node.type == Node.DocTypeTag)
                {
                    Node.insertDocType(lexer, head, node);
                    continue;
                }

                /* discard unknown tags */
                if (node.tag == null)
                {
                    Report.Warning(lexer, head, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                if (!((node.tag.model & Dict.CM_HEAD) != 0))
                {
                    lexer.ungetToken();
                    break;
                }

                if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                {
                    if (node.tag == tt.tagTitle)
                    {
                        ++HasTitle;

                        if (HasTitle > 1)
                            Report.Warning(lexer, head, node, Report.TOO_MANY_ELEMENTS);
                    }
                    else if (node.tag == tt.tagBase)
                    {
                        ++HasBase;

                        if (HasBase > 1)
                            Report.Warning(lexer, head, node, Report.TOO_MANY_ELEMENTS);
                    }
                    else if (node.tag == tt.tagNoscript)
                        Report.Warning(lexer, head, node, Report.TAG_NOT_ALLOWED_IN);

                    Node.insertNodeAtEnd(head, node);
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
                    continue;
                }

                /* discard unexpected text nodes and end tags */
                Report.Warning(lexer, head, node, Report.DISCARDING_UNEXPECTED);
            }

            if (HasTitle == 0)
            {
                Report.Warning(lexer, head, null, Report.MISSING_TITLE_ELEMENT);
                Node.insertNodeAtEnd(head, lexer.inferredTag("title"));
            }
        }
    }


    public class ParseTitle : Parser
    {

        public virtual void Parse(Lexer lexer, Node title, short mode)
        {
            Node node;

            while (true)
            {
                node = lexer.GetToken(Lexer.MixedContent);
                if (node == null)
                    break;
                if (node.tag == title.tag && node.type == Node.EndTag)
                {
                    title.closed = true;
                    Node.trimSpaces(lexer, title);
                    return;
                }

                if (node.type == Node.TextNode)
                {
                    /* only called for 1st child */
                    if (title.content == null)
                        Node.trimInitialSpace(lexer, title, node);

                    if (node.start >= node.end)
                    {
                        continue;
                    }

                    Node.insertNodeAtEnd(title, node);
                    continue;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(title, node))
                    continue;

                /* discard unknown tags */
                if (node.tag == null)
                {
                    Report.Warning(lexer, title, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* pushback unexpected tokens */
                Report.Warning(lexer, title, node, Report.MISSING_ENDTAG_BEFORE);
                lexer.ungetToken();
                Node.trimSpaces(lexer, title);
                return;
            }

            Report.Warning(lexer, title, node, Report.MISSING_ENDTAG_FOR);
        }
    }


    public class ParseScript : Parser
    {

        public virtual void Parse(Lexer lexer, Node script, short mode)
        {
            /*
            This isn't quite right for CDATA content as it recognises
            tags within the content and parses them accordingly.
            This will unfortunately screw up scripts which include
            < + letter,  < + !, < + ?  or  < + / + letter
            */

            Node node;

            node = lexer.getCDATA(script);

            if (node != null)
                Node.insertNodeAtEnd(script, node);
        }
    }


    public class ParseBody : Parser
    {

        public virtual void Parse(Lexer lexer, Node body, short mode)
        {
            Node node;
            bool checkstack, iswhitenode;

            mode = Lexer.IgnoreWhitespace;
            checkstack = true;
            TagTable tt = lexer.configuration.tt;

            while (true)
            {
                node = lexer.GetToken(mode);
                if (node == null)
                    break;
                if (node.tag == body.tag && node.type == Node.EndTag)
                {
                    body.closed = true;
                    Node.trimSpaces(lexer, body);
                    lexer.seenBodyEndTag = 1;
                    mode = Lexer.IgnoreWhitespace;

                    if (body.parent.tag == tt.tagNoframes)
                        break;

                    continue;
                }

                if (node.tag == tt.tagNoframes)
                {
                    if (node.type == Node.StartTag)
                    {
                        Node.insertNodeAtEnd(body, node);
                        Comzept.Genesis.Tidy.ParserImpl.getParseBlock().Parse(lexer, node, mode);
                        continue;
                    }

                    if (node.type == Node.EndTag && body.parent.tag == tt.tagNoframes)
                    {
                        Node.trimSpaces(lexer, body);
                        lexer.ungetToken();
                        break;
                    }
                }

                if ((node.tag == tt.tagFrame || node.tag == tt.tagFrameset) && body.parent.tag == tt.tagNoframes)
                {
                    Node.trimSpaces(lexer, body);
                    lexer.ungetToken();
                    break;
                }

                if (node.tag == tt.tagHtml)
                {
                    if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                        Report.Warning(lexer, body, node, Report.DISCARDING_UNEXPECTED);

                    continue;
                }

                iswhitenode = false;

                if (node.type == Node.TextNode && node.end <= node.start + 1 && node.textarray[node.start] == (sbyte)' ')
                    iswhitenode = true;

                /* deal with comments etc. */
                if (Node.insertMisc(body, node))
                    continue;

                if (lexer.seenBodyEndTag == 1 && !iswhitenode)
                {
                    ++lexer.seenBodyEndTag;
                    Report.Warning(lexer, body, node, Report.CONTENT_AFTER_BODY);
                }

                /* mixed content model permits text */
                if (node.type == Node.TextNode)
                {
                    if (iswhitenode && mode == Lexer.IgnoreWhitespace)
                    {
                        continue;
                    }

                    if (lexer.configuration.EncloseBodyText && !iswhitenode)
                    {
                        Node para;

                        lexer.ungetToken();
                        para = lexer.inferredTag("p");
                        Node.insertNodeAtEnd(body, para);
                        Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, para, mode);
                        mode = Lexer.MixedContent;
                        continue;
                    }
                    /* strict doesn't allow text here */
                    else
                        lexer.versions &= ~(Dict.VERS_HTML40_STRICT | Dict.VERS_HTML20);

                    if (checkstack)
                    {
                        checkstack = false;

                        if (lexer.inlineDup(node) > 0)
                            continue;
                    }

                    Node.insertNodeAtEnd(body, node);
                    mode = Lexer.MixedContent;
                    continue;
                }

                if (node.type == Node.DocTypeTag)
                {
                    Node.insertDocType(lexer, body, node);
                    continue;
                }
                /* discard unknown  and PARAM tags */
                if (node.tag == null || node.tag == tt.tagParam)
                {
                    Report.Warning(lexer, body, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /*
                Netscape allows LI and DD directly in BODY
                We infer UL or DL respectively and use this
                boolean to exclude block-level elements so as
                to match Netscape's observed behaviour.
                */
                lexer.excludeBlocks = false;

                if (!((node.tag.model & Dict.CM_BLOCK) != 0) && !((node.tag.model & Dict.CM_INLINE) != 0))
                {
                    /* avoid this error message being issued twice */
                    if (!((node.tag.model & Dict.CM_HEAD) != 0))
                        Report.Warning(lexer, body, node, Report.TAG_NOT_ALLOWED_IN);

                    if ((node.tag.model & Dict.CM_HTML) != 0)
                    {
                        /* copy body attributes if current body was inferred */
                        if (node.tag == tt.tagBody && body.implicit_Renamed && body.attributes == null)
                        {
                            body.attributes = node.attributes;
                            node.attributes = null;
                        }

                        continue;
                    }

                    if ((node.tag.model & Dict.CM_HEAD) != 0)
                    {
                        Comzept.Genesis.Tidy.ParserImpl.moveToHead(lexer, body, node);
                        continue;
                    }

                    if ((node.tag.model & Dict.CM_LIST) != 0)
                    {
                        lexer.ungetToken();
                        node = lexer.inferredTag("ul");
                        Node.addClass(node, "noindent");
                        lexer.excludeBlocks = true;
                    }
                    else if ((node.tag.model & Dict.CM_DEFLIST) != 0)
                    {
                        lexer.ungetToken();
                        node = lexer.inferredTag("dl");
                        lexer.excludeBlocks = true;
                    }
                    else if ((node.tag.model & (Dict.CM_TABLE | Dict.CM_ROWGRP | Dict.CM_ROW)) != 0)
                    {
                        lexer.ungetToken();
                        node = lexer.inferredTag("table");
                        lexer.excludeBlocks = true;
                    }
                    else
                    {
                        /* AQ: The following line is from the official C
                        version of tidy.  It doesn't make sense to me
                        because the '!' operator has higher precedence
                        than the '&' operator.  It seems to me that the
                        expression always evaluates to 0.
							
                        if (!node->tag->model & (CM_ROW | CM_FIELD))
							
                        AQ: 13Jan2000 fixed in C tidy
                        */
                        if (!((node.tag.model & (Dict.CM_ROW | Dict.CM_FIELD)) != 0))
                        {
                            lexer.ungetToken();
                            return;
                        }

                        /* ignore </td> </th> <option> etc. */
                        continue;
                    }
                }

                if (node.type == Node.EndTag)
                {
                    if (node.tag == tt.tagBr)
                        node.type = Node.StartTag;
                    else if (node.tag == tt.tagP)
                    {
                        Node.coerceNode(lexer, node, tt.tagBr);
                        Node.insertNodeAtEnd(body, node);
                        node = lexer.inferredTag("br");
                    }
                    else if ((node.tag.model & Dict.CM_INLINE) != 0)
                        lexer.popInline(node);
                }

                if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                {
                    if (((node.tag.model & Dict.CM_INLINE) != 0) && !((node.tag.model & Dict.CM_MIXED) != 0))
                    {
                        /* HTML4 strict doesn't allow inline content here */
                        /* but HTML2 does allow img elements as children of body */
                        if (node.tag == tt.tagImg)
                            lexer.versions &= ~Dict.VERS_HTML40_STRICT;
                        else
                            lexer.versions &= ~(Dict.VERS_HTML40_STRICT | Dict.VERS_HTML20);

                        if (checkstack && !node.implicit_Renamed)
                        {
                            checkstack = false;

                            if (lexer.inlineDup(node) > 0)
                                continue;
                        }

                        mode = Lexer.MixedContent;
                    }
                    else
                    {
                        checkstack = true;
                        mode = Lexer.IgnoreWhitespace;
                    }

                    if (node.implicit_Renamed)
                        Report.Warning(lexer, body, node, Report.INSERTING_TAG);

                    Node.insertNodeAtEnd(body, node);
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, mode);
                    continue;
                }

                /* discard unexpected tags */
                Report.Warning(lexer, body, node, Report.DISCARDING_UNEXPECTED);
            }
        }
    }


    public class ParseFrameSet : Parser
    {

        public virtual void Parse(Lexer lexer, Node frameset, short mode)
        {
            Node node;
            TagTable tt = lexer.configuration.tt;

            lexer.badAccess |= Report.USING_FRAMES;

            while (true)
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);
                if (node == null)
                    break;
                if (node.tag == frameset.tag && node.type == Node.EndTag)
                {
                    frameset.closed = true;
                    Node.trimSpaces(lexer, frameset);
                    return;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(frameset, node))
                    continue;

                if (node.tag == null)
                {
                    Report.Warning(lexer, frameset, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                {
                    if (node.tag != null && (node.tag.model & Dict.CM_HEAD) != 0)
                    {
                        Comzept.Genesis.Tidy.ParserImpl.moveToHead(lexer, frameset, node);
                        continue;
                    }
                }

                if (node.tag == tt.tagBody)
                {
                    lexer.ungetToken();
                    node = lexer.inferredTag("noframes");
                    Report.Warning(lexer, frameset, node, Report.INSERTING_TAG);
                }

                if (node.type == Node.StartTag && (node.tag.model & Dict.CM_FRAMES) != 0)
                {
                    Node.insertNodeAtEnd(frameset, node);
                    lexer.excludeBlocks = false;
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.MixedContent);
                    continue;
                }
                else if (node.type == Node.StartEndTag && (node.tag.model & Dict.CM_FRAMES) != 0)
                {
                    Node.insertNodeAtEnd(frameset, node);
                    continue;
                }

                /* discard unexpected tags */
                Report.Warning(lexer, frameset, node, Report.DISCARDING_UNEXPECTED);
            }

            Report.Warning(lexer, frameset, node, Report.MISSING_ENDTAG_FOR);
        }
    }


    public class ParseInline : Parser
    {

        public virtual void Parse(Lexer lexer, Node element, short mode)
        {
            Node node, parent;
            TagTable tt = lexer.configuration.tt;

            if ((element.tag.model & Dict.CM_EMPTY) != 0)
                return;

            if (element.tag == tt.tagA)
            {
                if (element.attributes == null)
                {
                    Report.Warning(lexer, element.parent, element, Report.DISCARDING_UNEXPECTED);
                    Node.discardElement(element);
                    return;
                }
            }

            /*
            ParseInline is used for some block level elements like H1 to H6
            For such elements we need to insert inline emphasis tags currently
            on the inline stack. For Inline elements, we normally push them
            onto the inline stack provided they aren't implicit or OBJECT/APPLET.
            This test is carried out in PushInline and PopInline, see istack.c
            We don't push A or SPAN to replicate current browser behavior
            */
            if (((element.tag.model & Dict.CM_BLOCK) != 0) || (element.tag == tt.tagDt))
                lexer.inlineDup(null);
            else if ((element.tag.model & Dict.CM_INLINE) != 0 && element.tag != tt.tagA && element.tag != tt.tagSpan)
                lexer.PushInline(element);

            if (element.tag == tt.tagNobr)
                lexer.badLayout |= Report.USING_NOBR;
            else if (element.tag == tt.tagFont)
                lexer.badLayout |= Report.USING_FONT;

            /* Inline elements may or may not be within a preformatted element */
            if (mode != Lexer.Preformatted)
                mode = Lexer.MixedContent;

            while (true)
            {
                node = lexer.GetToken(mode);
                if (node == null)
                    break;
                /* end tag for current element */
                if (node.tag == element.tag && node.type == Node.EndTag)
                {
                    if ((element.tag.model & Dict.CM_INLINE) != 0 && element.tag != tt.tagA)
                        lexer.popInline(node);

                    if (!((mode & Lexer.Preformatted) != 0))
                        Node.trimSpaces(lexer, element);
                    /*
                    if a font element wraps an anchor and nothing else
                    then move the font element inside the anchor since
                    otherwise it won't alter the anchor text color
                    */
                    if (element.tag == tt.tagFont && element.content != null && element.content == element.last)
                    {
                        Node child = element.content;

                        if (child.tag == tt.tagA)
                        {
                            child.parent = element.parent;
                            child.next = element.next;
                            child.prev = element.prev;

                            if (child.prev != null)
                                child.prev.next = child;
                            else
                                child.parent.content = child;

                            if (child.next != null)
                                child.next.prev = child;
                            else
                                child.parent.last = child;

                            element.next = null;
                            element.prev = null;
                            element.parent = child;
                            element.content = child.content;
                            element.last = child.last;
                            child.content = element;
                            child.last = element;
                            for (child = element.content; child != null; child = child.next)
                                child.parent = element;
                        }
                    }
                    element.closed = true;
                    Node.trimSpaces(lexer, element);
                    Node.trimEmptyElement(lexer, element);
                    return;
                }

                /* <u>...<u>  map 2nd <u> to </u> if 1st is explicit */
                /* otherwise emphasis nesting is probably unintentional */
                /* big and small have cumulative effect to leave them alone */
                if (node.type == Node.StartTag && node.tag == element.tag && lexer.isPushed(node) && !node.implicit_Renamed && !element.implicit_Renamed && node.tag != null && ((node.tag.model & Dict.CM_INLINE) != 0) && node.tag != tt.tagA && node.tag != tt.tagFont && node.tag != tt.tagBig && node.tag != tt.tagSmall)
                {
                    if (element.content != null && node.attributes == null)
                    {
                        Report.Warning(lexer, element, node, Report.COERCE_TO_ENDTAG);
                        node.type = Node.EndTag;
                        lexer.ungetToken();
                        continue;
                    }

                    Report.Warning(lexer, element, node, Report.NESTED_EMPHASIS);
                }

                if (node.type == Node.TextNode)
                {
                    /* only called for 1st child */
                    if (element.content == null && !((mode & Lexer.Preformatted) != 0))
                        Node.trimSpaces(lexer, element);

                    if (node.start >= node.end)
                    {
                        continue;
                    }

                    Node.insertNodeAtEnd(element, node);
                    continue;
                }

                /* mixed content model so allow text */
                if (Node.insertMisc(element, node))
                    continue;

                /* deal with HTML tags */
                if (node.tag == tt.tagHtml)
                {
                    if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                    {
                        Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    /* otherwise infer end of inline element */
                    lexer.ungetToken();
                    if (!((mode & Lexer.Preformatted) != 0))
                        Node.trimSpaces(lexer, element);
                    Node.trimEmptyElement(lexer, element);
                    return;
                }

                /* within <dt> or <pre> map <p> to <br/> */
                if (node.tag == tt.tagP && node.type == Node.StartTag && ((mode & Lexer.Preformatted) != 0 || element.tag == tt.tagDt || element.isDescendantOf(tt.tagDt)))
                {
                    node.tag = tt.tagBr;
                    node.element = "br";
                    Node.trimSpaces(lexer, element);
                    Node.insertNodeAtEnd(element, node);
                    continue;
                }

                /* ignore unknown and PARAM tags */
                if (node.tag == null || node.tag == tt.tagParam)
                {
                    Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                if (node.tag == tt.tagBr && node.type == Node.EndTag)
                    node.type = Node.StartTag;

                if (node.type == Node.EndTag)
                {
                    /* coerce </br> to <br/> */
                    if (node.tag == tt.tagBr)
                        node.type = Node.StartTag;
                    else if (node.tag == tt.tagP)
                    {
                        /* coerce unmatched </p> to <br/><br/> */
                        if (!element.isDescendantOf(tt.tagP))
                        {
                            Node.coerceNode(lexer, node, tt.tagBr);
                            Node.trimSpaces(lexer, element);
                            Node.insertNodeAtEnd(element, node);
                            node = lexer.inferredTag("br");
                            continue;
                        }
                    }
                    else if ((node.tag.model & Dict.CM_INLINE) != 0 && node.tag != tt.tagA && !((node.tag.model & Dict.CM_OBJECT) != 0) && (element.tag.model & Dict.CM_INLINE) != 0)
                    {
                        /* allow any inline end tag to end current element */
                        lexer.popInline(element);

                        if (element.tag != tt.tagA)
                        {
                            if (node.tag == tt.tagA && node.tag != element.tag)
                            {
                                Report.Warning(lexer, element, node, Report.MISSING_ENDTAG_BEFORE);
                                lexer.ungetToken();
                            }
                            else
                            {
                                Report.Warning(lexer, element, node, Report.NON_MATCHING_ENDTAG);
                            }

                            if (!((mode & Lexer.Preformatted) != 0))
                                Node.trimSpaces(lexer, element);
                            Node.trimEmptyElement(lexer, element);
                            return;
                        }

                        /* if parent is <a> then discard unexpected inline end tag */
                        Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }
                    /* special case </tr> etc. for stuff moved in front of table */
                    else if (lexer.exiled && node.tag.model != 0 && (node.tag.model & Dict.CM_TABLE) != 0)
                    {
                        lexer.ungetToken();
                        Node.trimSpaces(lexer, element);
                        Node.trimEmptyElement(lexer, element);
                        return;
                    }
                }

                /* allow any header tag to end current header */
                if ((node.tag.model & Dict.CM_HEADING) != 0 && (element.tag.model & Dict.CM_HEADING) != 0)
                {
                    if (node.tag == element.tag)
                    {
                        Report.Warning(lexer, element, node, Report.NON_MATCHING_ENDTAG);
                    }
                    else
                    {
                        Report.Warning(lexer, element, node, Report.MISSING_ENDTAG_BEFORE);
                        lexer.ungetToken();
                    }
                    if (!((mode & Lexer.Preformatted) != 0))
                        Node.trimSpaces(lexer, element);
                    Node.trimEmptyElement(lexer, element);
                    return;
                }

                /*
                an <A> tag to ends any open <A> element
                but <A href=...> is mapped to </A><A href=...>
                */
                if (node.tag == tt.tagA && !node.implicit_Renamed && lexer.isPushed(node))
                {
                    /* coerce <a> to </a> unless it has some attributes */
                    if (node.attributes == null)
                    {
                        node.type = Node.EndTag;
                        Report.Warning(lexer, element, node, Report.COERCE_TO_ENDTAG);
                        lexer.popInline(node);
                        lexer.ungetToken();
                        continue;
                    }

                    lexer.ungetToken();
                    Report.Warning(lexer, element, node, Report.MISSING_ENDTAG_BEFORE);
                    lexer.popInline(element);
                    if (!((mode & Lexer.Preformatted) != 0))
                        Node.trimSpaces(lexer, element);
                    Node.trimEmptyElement(lexer, element);
                    return;
                }

                if ((element.tag.model & Dict.CM_HEADING) != 0)
                {
                    if (node.tag == tt.tagCenter || node.tag == tt.tagDiv)
                    {
                        if (node.type != Node.StartTag && node.type != Node.StartEndTag)
                        {
                            Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
                            continue;
                        }

                        Report.Warning(lexer, element, node, Report.TAG_NOT_ALLOWED_IN);

                        /* insert center as parent if heading is empty */
                        if (element.content == null)
                        {
                            Node.insertNodeAsParent(element, node);
                            continue;
                        }

                        /* split heading and make center parent of 2nd part */
                        Node.insertNodeAfterElement(element, node);

                        if (!((mode & Lexer.Preformatted) != 0))
                            Node.trimSpaces(lexer, element);

                        element = lexer.cloneNode(element);
                        element.start = lexer.lexsize;
                        element.end = lexer.lexsize;
                        Node.insertNodeAtEnd(node, element);
                        continue;
                    }

                    if (node.tag == tt.tagHr)
                    {
                        if (node.type != Node.StartTag && node.type != Node.StartEndTag)
                        {
                            Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
                            continue;
                        }

                        Report.Warning(lexer, element, node, Report.TAG_NOT_ALLOWED_IN);

                        /* insert hr before heading if heading is empty */
                        if (element.content == null)
                        {
                            Node.insertNodeBeforeElement(element, node);
                            continue;
                        }

                        /* split heading and insert hr before 2nd part */
                        Node.insertNodeAfterElement(element, node);

                        if (!((mode & Lexer.Preformatted) != 0))
                            Node.trimSpaces(lexer, element);

                        element = lexer.cloneNode(element);
                        element.start = lexer.lexsize;
                        element.end = lexer.lexsize;
                        Node.insertNodeAfterElement(node, element);
                        continue;
                    }
                }

                if (element.tag == tt.tagDt)
                {
                    if (node.tag == tt.tagHr)
                    {
                        Node dd;

                        if (node.type != Node.StartTag && node.type != Node.StartEndTag)
                        {
                            Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
                            continue;
                        }

                        Report.Warning(lexer, element, node, Report.TAG_NOT_ALLOWED_IN);
                        dd = lexer.inferredTag("dd");

                        /* insert hr within dd before dt if dt is empty */
                        if (element.content == null)
                        {
                            Node.insertNodeBeforeElement(element, dd);
                            Node.insertNodeAtEnd(dd, node);
                            continue;
                        }

                        /* split dt and insert hr within dd before 2nd part */
                        Node.insertNodeAfterElement(element, dd);
                        Node.insertNodeAtEnd(dd, node);

                        if (!((mode & Lexer.Preformatted) != 0))
                            Node.trimSpaces(lexer, element);

                        element = lexer.cloneNode(element);
                        element.start = lexer.lexsize;
                        element.end = lexer.lexsize;
                        Node.insertNodeAfterElement(dd, element);
                        continue;
                    }
                }


                /* 
                if this is the end tag for an ancestor element
                then infer end tag for this element
                */
                if (node.type == Node.EndTag)
                {
                    for (parent = element.parent; parent != null; parent = parent.parent)
                    {
                        if (node.tag == parent.tag)
                        {
                            if (!((element.tag.model & Dict.CM_OPT) != 0) && !element.implicit_Renamed)
                                Report.Warning(lexer, element, node, Report.MISSING_ENDTAG_BEFORE);

                            if (element.tag == tt.tagA)
                                lexer.popInline(element);

                            lexer.ungetToken();

                            if (!((mode & Lexer.Preformatted) != 0))
                                Node.trimSpaces(lexer, element);

                            Node.trimEmptyElement(lexer, element);
                            return;
                        }
                    }
                }

                /* block level tags end this element */
                if (!((node.tag.model & Dict.CM_INLINE) != 0))
                {
                    if (node.type != Node.StartTag)
                    {
                        Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    if (!((element.tag.model & Dict.CM_OPT) != 0))
                        Report.Warning(lexer, element, node, Report.MISSING_ENDTAG_BEFORE);

                    if ((node.tag.model & Dict.CM_HEAD) != 0 && !((node.tag.model & Dict.CM_BLOCK) != 0))
                    {
                        Comzept.Genesis.Tidy.ParserImpl.moveToHead(lexer, element, node);
                        continue;
                    }

                    /*
                    prevent anchors from propagating into block tags
                    except for headings h1 to h6
                    */
                    if (element.tag == tt.tagA)
                    {
                        if (node.tag != null && !((node.tag.model & Dict.CM_HEADING) != 0))
                            lexer.popInline(element);
                        else if (!(element.content != null))
                        {
                            Node.discardElement(element);
                            lexer.ungetToken();
                            return;
                        }
                    }

                    lexer.ungetToken();

                    if (!((mode & Lexer.Preformatted) != 0))
                        Node.trimSpaces(lexer, element);

                    Node.trimEmptyElement(lexer, element);
                    return;
                }

                /* parse inline element */
                if (node.type == Node.StartTag || node.type == Node.StartEndTag)
                {
                    if (node.implicit_Renamed)
                        Report.Warning(lexer, element, node, Report.INSERTING_TAG);

                    /* trim white space before <br/> */
                    if (node.tag == tt.tagBr)
                        Node.trimSpaces(lexer, element);

                    Node.insertNodeAtEnd(element, node);
                    Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, mode);
                    continue;
                }

                /* discard unexpected tags */
                Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
            }

            if (!((element.tag.model & Dict.CM_OPT) != 0))
                Report.Warning(lexer, element, node, Report.MISSING_ENDTAG_FOR);

            Node.trimEmptyElement(lexer, element);
        }
    }


    public class ParseList : Parser
    {

        public virtual void Parse(Lexer lexer, Node list, short mode)
        {
            Node node;
            Node parent;
            TagTable tt = lexer.configuration.tt;

            if ((list.tag.model & Dict.CM_EMPTY) != 0)
                return;

            lexer.insert = -1; /* defer implicit inline start tags */

            while (true)
            {
                node = lexer.GetToken(Lexer.IgnoreWhitespace);
                if (node == null)
                    break;

                if (node.tag == list.tag && node.type == Node.EndTag)
                {
                    if ((list.tag.model & Dict.CM_OBSOLETE) != 0)
                        Node.coerceNode(lexer, list, tt.tagUl);

                    list.closed = true;
                    Node.trimEmptyElement(lexer, list);
                    return;
                }

                /* deal with comments etc. */
                if (Node.insertMisc(list, node))
                    continue;

                if (node.type != Node.TextNode && node.tag == null)
                {
                    Report.Warning(lexer, list, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                /* 
                if this is the end tag for an ancestor element
                then infer end tag for this element
                */
                if (node.type == Node.EndTag)
                {
                    if (node.tag == tt.tagForm)
                    {
                        lexer.badForm = 1;
                        Report.Warning(lexer, list, node, Report.DISCARDING_UNEXPECTED);
                        continue;
                    }

                    if (node.tag != null && (node.tag.model & Dict.CM_INLINE) != 0)
                    {
                        Report.Warning(lexer, list, node, Report.DISCARDING_UNEXPECTED);
                        lexer.popInline(node);
                        continue;
                    }

                    for (parent = list.parent; parent != null; parent = parent.parent)
                    {
                        if (node.tag == parent.tag)
                        {
                            Report.Warning(lexer, list, node, Report.MISSING_ENDTAG_BEFORE);
                            lexer.ungetToken();

                            if ((list.tag.model & Dict.CM_OBSOLETE) != 0)
                                Node.coerceNode(lexer, list, tt.tagUl);

                            Node.trimEmptyElement(lexer, list);
                            return;
                        }
                    }

                    Report.Warning(lexer, list, node, Report.DISCARDING_UNEXPECTED);
                    continue;
                }

                if (node.tag != tt.tagLi)
                {
                    lexer.ungetToken();

                    if (node.tag != null && (node.tag.model & Dict.CM_BLOCK) != 0 && lexer.excludeBlocks)
                    {
                        Report.Warning(lexer, list, node, Report.MISSING_ENDTAG_BEFORE);
                        Node.trimEmptyElement(lexer, list);
                        return;
                    }

                    node = lexer.inferredTag("li");
                    node.addAttribute("style", "list-style: none");
                    Report.Warning(lexer, list, node, Report.MISSING_STARTTAG);
                }

                /* node should be <LI> */
                Node.insertNodeAtEnd(list, node);
                Comzept.Genesis.Tidy.ParserImpl.parseTag(lexer, node, Lexer.IgnoreWhitespace);
            }

            if ((list.tag.model & Dict.CM_OBSOLETE) != 0)
                Node.coerceNode(lexer, list, tt.tagUl);

            Report.Warning(lexer, list, node, Report.MISSING_ENDTAG_FOR);
            Node.trimEmptyElement(lexer, list);
        }
    }



}