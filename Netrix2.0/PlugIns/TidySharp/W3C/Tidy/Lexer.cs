/*
* @(#)Lexer.java   1.11 2000/08/16
*
*/

/*
Given a file stream fp it returns a sequence of tokens.

GetToken(fp) gets the next token
UngetToken(fp) provides one level undo

The tags include an attribute list:

- linked list of attribute/value nodes
- each node has 2 null-terminated strings.
- entities are replaced in attribute values

white space is compacted if not in preformatted mode
If not in preformatted mode then leading white space
is discarded and subsequent white space sequences
compacted to single space chars.

If XmlTags is no then Tag names are folded to upper
case and attribute names to lower case.

Not yet done:
-   Doctype subset and marked sections*/
using System;

namespace Comzept.Genesis.Tidy
{

    /// <summary> 
    /// Lexer for html parser.
    /// </summary>
    /// <remarks>
    /// (c) 1998-2000 (W3C) MIT, INRIA, Keio University
    /// See Tidy.java for the copyright notice.
    /// Derived from <a href="http://www.w3.org/People/Raggett/tidy">HTML Tidy Release 4 Aug 2000</a>
    /// 
    /// </remarks>
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
    /// </version>
    public class Lexer
    {


        public StreamIn in_Renamed; /* file stream */
        public System.IO.StreamWriter errout; /* error output stream */
        public short badAccess; /* for accessibility errors */
        public short badLayout; /* for bad style errors */
        public short badChars; /* for bad char encodings */
        public short badForm; /* for mismatched/mispositioned form tags */
        public short warnings; /* count of warnings in this document */
        public short errors; /* count of errors */
        public int lines; /* lines seen */
        public int columns; /* at start of current token */
        public bool waswhite; /* used to collapse contiguous white space */
        public bool pushed; /* true after token has been pushed back */
        public bool insertspace; /* when space is moved after end tag */
        public bool excludeBlocks; /* Netscape compatibility */
        public bool exiled; /* true if moved out of table */
        public bool isvoyager; /* true if xmlns attribute on html element */
        public short versions; /* bit vector of HTML versions */
        public int doctype; /* version as given by doctype (if any) */
        public bool badDoctype; /* e.g. if html or PUBLIC is missing */
        public int txtstart; /* start of current node */
        public int txtend; /* end of current node */
        public short state; /* state of lexer's finite state machine */
        public Node token;

        /* 
        lexer character buffer
		
        parse tree nodes span onto this buffer
        which contains the concatenated text
        contents of all of the elements.
		
        lexsize must be reset for each file.
        */
        public sbyte[] lexbuf; /* byte buffer of UTF-8 chars */
        public int lexlength; /* allocated */
        public int lexsize; /* used */

        /* Inline stack for compatibility with Mosaic */
        public Node inode; /* for deferring text node */
        public int insert; /* for inferring inline tags */
        public System.Collections.ArrayList istack;
        public int istackbase; /* start of frame */

        public Style styles; /* used for cleaning up presentation markup */

        public Configuration configuration;
        protected internal int seenBodyEndTag; /* used by parser */
        private System.Collections.ArrayList nodeList;

        public Lexer(StreamIn in_Renamed, Configuration configuration)
        {
            this.in_Renamed = in_Renamed;
            this.lines = 1;
            this.columns = 1;
            this.state = LEX_CONTENT;
            this.badAccess = 0;
            this.badLayout = 0;
            this.badChars = 0;
            this.badForm = 0;
            this.warnings = 0;
            this.errors = 0;
            this.waswhite = false;
            this.pushed = false;
            this.insertspace = false;
            this.exiled = false;
            this.isvoyager = false;
            this.versions = Dict.VERS_EVERYTHING;
            this.doctype = Dict.VERS_UNKNOWN;
            this.badDoctype = false;
            this.txtstart = 0;
            this.txtend = 0;
            this.token = null;
            this.lexbuf = null;
            this.lexlength = 0;
            this.lexsize = 0;
            this.inode = null;
            this.insert = -1;
            this.istack = new System.Collections.ArrayList();
            this.istackbase = 0;
            this.styles = null;
            this.configuration = configuration;
            this.seenBodyEndTag = 0;
            this.nodeList = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
        }

        public virtual Node newNode()
        {
            Node node = new Node();
            nodeList.Add(node);
            return node;
        }

        public virtual Node newNode(short type, sbyte[] textarray, int start, int end)
        {
            Node node = new Node(type, textarray, start, end);
            nodeList.Add(node);
            return node;
        }

        public virtual Node newNode(short type, sbyte[] textarray, int start, int end, System.String element)
        {
            Node node = new Node(type, textarray, start, end, element, configuration.tt);
            nodeList.Add(node);
            return node;
        }

        public virtual Node cloneNode(Node node)
        {
            Node cnode = (Node)node.Clone();
            nodeList.Add(cnode);
            for (AttVal att = cnode.attributes; att != null; att = att.next)
            {
                if (att.asp != null)
                    nodeList.Add(att.asp);
                if (att.php != null)
                    nodeList.Add(att.php);
            }
            return cnode;
        }

        public virtual AttVal cloneAttributes(AttVal attrs)
        {
            AttVal cattrs = (AttVal)attrs.Clone();
            for (AttVal att = cattrs; att != null; att = att.next)
            {
                if (att.asp != null)
                    nodeList.Add(att.asp);
                if (att.php != null)
                    nodeList.Add(att.php);
            }
            return cattrs;
        }

        protected internal virtual void updateNodeTextArrays(sbyte[] oldtextarray, sbyte[] newtextarray)
        {
            Node node;
            for (int i = 0; i < nodeList.Count; i++)
            {
                node = (Node)(nodeList[i]);
                if (node.textarray == oldtextarray)
                    node.textarray = newtextarray;
            }
        }

        /* used for creating preformatted text from Word2000 */
        public virtual Node newLineNode()
        {
            Node node = newNode();

            node.textarray = this.lexbuf;
            node.start = this.lexsize;
            addCharToLexer((int)'\n');
            node.end = this.lexsize;
            return node;
        }

        // Should always be able convert to/from UTF-8, so encoding exceptions are
        // converted to an Error to avoid adding throws declarations in
        // lots of methods.

        public static sbyte[] getBytes(System.String str)
        {
            try
            {
                //UPGRADE_TODO: Method 'java.lang.String.getBytes' was converted to 'System.Text.Encoding.GetEncoding(string).GetBytes(string)' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangStringgetBytes_javalangString'"
                return SupportClass.ToSByteArray(System.Text.Encoding.GetEncoding("UTF8").GetBytes(str));
            }
            catch (System.IO.IOException e)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                throw new System.ApplicationException("string to UTF-8 conversion failed: " + e.Message);
            }
        }

        public static System.String getString(sbyte[] bytes, int offset, int length)
        {
            try
            {
                System.String tempStr;
                //UPGRADE_TODO: The differences in the Format  of parameters for constructor 'java.lang.String.String'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
                tempStr = System.Text.Encoding.GetEncoding("UTF8").GetString(SupportClass.ToByteArray(bytes));
                return new System.String(tempStr.ToCharArray(), offset, length);
            }
            catch (System.IO.IOException e)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                throw new System.ApplicationException("UTF-8 to string conversion failed: " + e.Message);
            }
        }

        public virtual bool EndOfInput
        {
            get
            {
                return this.in_Renamed.IsEndOfStream;
            }
        }

        public virtual void addByte(int c)
        {
            if (this.lexsize + 1 >= this.lexlength)
            {
                while (this.lexsize + 1 >= this.lexlength)
                {
                    if (this.lexlength == 0)
                        this.lexlength = 8192;
                    else
                        this.lexlength = this.lexlength * 2;
                }

                sbyte[] temp = this.lexbuf;
                this.lexbuf = new sbyte[this.lexlength];
                if (temp != null)
                {
                    Array.Copy(temp, 0, this.lexbuf, 0, temp.Length);
                    updateNodeTextArrays(temp, this.lexbuf);
                }
            }

            this.lexbuf[this.lexsize++] = (sbyte)c;
            this.lexbuf[this.lexsize] = (sbyte)'\x0000'; /* debug */
        }

        public virtual void changeChar(sbyte c)
        {
            if (this.lexsize > 0)
            {
                this.lexbuf[this.lexsize - 1] = c;
            }
        }

        /* store char c as UTF-8 encoded byte stream */
        public virtual void addCharToLexer(int c)
        {
            if (c < 128)
                addByte(c);
            else if (c <= 0x7FF)
            {
                addByte(0xC0 | (c >> 6));
                addByte(0x80 | (c & 0x3F));
            }
            else if (c <= 0xFFFF)
            {
                addByte(0xE0 | (c >> 12));
                addByte(0x80 | ((c >> 6) & 0x3F));
                addByte(0x80 | (c & 0x3F));
            }
            else if (c <= 0x1FFFFF)
            {
                addByte(0xF0 | (c >> 18));
                addByte(0x80 | ((c >> 12) & 0x3F));
                addByte(0x80 | ((c >> 6) & 0x3F));
                addByte(0x80 | (c & 0x3F));
            }
            else
            {
                addByte(0xF8 | (c >> 24));
                addByte(0x80 | ((c >> 18) & 0x3F));
                addByte(0x80 | ((c >> 12) & 0x3F));
                addByte(0x80 | ((c >> 6) & 0x3F));
                addByte(0x80 | (c & 0x3F));
            }
        }

        public virtual void addStringToLexer(System.String str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                addCharToLexer((int)str[i]);
            }
        }

        /*
        No longer attempts to insert missing ';' for unknown
        enitities unless one was present already, since this
        gives unexpected results.
		
        For example:   <a href="something.htm?foo&bar&fred">
        was tidied to: <a href="something.htm?foo&amp;bar;&amp;fred;">
        rather than:   <a href="something.htm?foo&amp;bar&amp;fred">
		
        My thanks for Maurice Buxton for spotting this.
        */
        public virtual void parseEntity(short mode)
        {
            short map;
            int start;
            bool first = true;
            bool semicolon = false;
            bool numeric = false;
            int c, ch, startcol;
            System.String str;

            start = this.lexsize - 1; /* to start at "&" */
            startcol = this.in_Renamed.curcol - 1;

            while (true)
            {
                c = this.in_Renamed.ReadChar();
                if (c == StreamIn.EndOfStream)
                    break;
                if (c == ';')
                {
                    semicolon = true;
                    break;
                }

                if (first && c == '#')
                {
                    addCharToLexer(c);
                    first = false;
                    numeric = true;
                    continue;
                }

                first = false;
                map = MAP((char)c);

                /* AQ: Added flag for numeric entities so that numeric entities
                with missing semi-colons are recognized.
                Eg. "&#114e&#112;..." is recognized as "rep"
                */
                if (numeric && ((c == 'x') || ((map & DIGIT) != 0)))
                {
                    addCharToLexer(c);
                    continue;
                }
                if (!numeric && ((map & NAMECHAR) != 0))
                {
                    addCharToLexer(c);
                    continue;
                }

                /* otherwise put it back */

                this.in_Renamed.UngetChar(c);
                break;
            }

            str = getString(this.lexbuf, start, this.lexsize - start);
            ch = EntityTable.DefaultEntityTable.entityCode(str);

            /* deal with unrecognized entities */
            if (ch <= 0)
            {
                /* set error position just before offending chararcter */
                this.lines = this.in_Renamed.curline;
                this.columns = startcol;

                if (this.lexsize > start + 1)
                {
                    Report.entityError(this, Report.UNKNOWN_ENTITY, str, ch);

                    if (semicolon)
                        addCharToLexer(';');
                }
                /* naked & */
                else
                {
                    Report.entityError(this, Report.UNESCAPED_AMPERSAND, str, ch);
                }
            }
            else
            {
                if (c != ';')
                /* issue warning if not terminated by ';' */
                {
                    /* set error position just before offending chararcter */
                    this.lines = this.in_Renamed.curline;
                    this.columns = startcol;
                    Report.entityError(this, Report.MISSING_SEMICOLON, str, c);
                }

                this.lexsize = start;

                if (ch == 160 && (mode & Preformatted) != 0)
                    ch = ' ';

                addCharToLexer(ch);

                if (ch == '&' && !this.configuration.QuoteAmpersand)
                {
                    addCharToLexer('a');
                    addCharToLexer('m');
                    addCharToLexer('p');
                    addCharToLexer(';');
                }
            }
        }

        public virtual char parseTagName()
        {
            short map;
            int c;

            /* fold case of first char in buffer */

            c = this.lexbuf[this.txtstart];
            map = MAP((char)c);

            if (!this.configuration.XmlTags && (map & UPPERCASE) != 0)
            {
                c += (int)((int)'a' - (int)'A');
                this.lexbuf[this.txtstart] = (sbyte)c;
            }

            while (true)
            {
                c = this.in_Renamed.ReadChar();
                if (c == StreamIn.EndOfStream)
                    break;
                map = MAP((char)c);

                if ((map & NAMECHAR) == 0)
                    break;

                /* fold case of subsequent chars */

                if (!this.configuration.XmlTags && (map & UPPERCASE) != 0)
                    c += (int)((int)'a' - (int)'A');

                addCharToLexer(c);
            }

            this.txtend = this.lexsize;
            return (char)c;
        }

        public virtual void addStringLiteral(System.String str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                addCharToLexer((int)str[i]);
            }
        }

        /* choose what version to use for new doctype */
        public virtual short HTMLVersion()
        {
            short versions;

            versions = this.versions;

            if ((versions & Dict.VERS_HTML20) != 0)
                return Dict.VERS_HTML20;

            if ((versions & Dict.VERS_HTML32) != 0)
                return Dict.VERS_HTML32;

            if ((versions & Dict.VERS_HTML40_STRICT) != 0)
                return Dict.VERS_HTML40_STRICT;

            if ((versions & Dict.VERS_HTML40_LOOSE) != 0)
                return Dict.VERS_HTML40_LOOSE;

            if ((versions & Dict.VERS_FRAMES) != 0)
                return Dict.VERS_FRAMES;

            return Dict.VERS_UNKNOWN;
        }

        public virtual System.String HTMLVersionName()
        {
            short guessed;
            int j;

            guessed = apparentVersion();

            for (j = 0; j < W3CVersion.Length; ++j)
            {
                if (guessed == W3CVersion[j].code)
                {
                    if (this.isvoyager)
                        return W3CVersion[j].voyagerName;

                    return W3CVersion[j].name;
                }
            }

            return null;
        }

        /* add meta element for Tidy */
        public virtual bool addGenerator(Node root)
        {
            AttVal attval;
            Node node;
            Node head = root.findHEAD(configuration.tt);

            if (head != null)
            {
                for (node = head.content; node != null; node = node.next)
                {
                    if (node.tag == configuration.tt.tagMeta)
                    {
                        attval = node.getAttrByName("name");

                        if (attval != null && attval.value_Renamed != null && Lexer.wstrcasecmp(attval.value_Renamed, "generator") == 0)
                        {
                            attval = node.getAttrByName("content");

                            if (attval != null && attval.value_Renamed != null && attval.value_Renamed.Length >= 9 && Lexer.wstrcasecmp(attval.value_Renamed.Substring(0, (9) - (0)), "HTML Tidy") == 0)
                            {
                                return false;
                            }
                        }
                    }
                }

                node = this.inferredTag("meta");
                node.addAttribute("content", "HTML Tidy, see www.w3.org");
                node.addAttribute("name", "generator");
                Node.insertNodeAtStart(head, node);
                return true;
            }

            return false;
        }

        /* return true if substring s is in p and isn't all in upper case */
        /* this is used to check the case of SYSTEM, PUBLIC, DTD and EN */
        /* len is how many chars to check in p */
        private static bool findBadSubString(System.String s, System.String p, int len)
        {
            int n = s.Length;
            int i = 0;
            System.String ps;

            while (n < len)
            {
                ps = p.Substring(i, (i + n) - (i));
                if (wstrcasecmp(s, ps) == 0)
                    return (!ps.Equals(s.Substring(0, (n) - (0))));

                ++i;
                --len;
            }

            return false;
        }

        public virtual bool checkDocTypeKeyWords(Node doctype)
        {
            int len = doctype.end - doctype.start;
            System.String s = getString(this.lexbuf, doctype.start, len);

            return !(findBadSubString("SYSTEM", s, len) || findBadSubString("PUBLIC", s, len) || findBadSubString("//DTD", s, len) || findBadSubString("//W3C", s, len) || findBadSubString("//EN", s, len));
        }

        /* examine <!DOCTYPE> to identify version */
        public virtual short findGivenVersion(Node doctype)
        {
            System.String p, s;
            int i, j;
            int len;
            System.String str1;
            System.String str2;

            /* if root tag for doctype isn't html give up now */
            str1 = getString(this.lexbuf, doctype.start, 5);
            if (wstrcasecmp(str1, "html ") != 0)
                return 0;

            if (!checkDocTypeKeyWords(doctype))
                Report.Warning(this, doctype, null, Report.DTYPE_NOT_UPPER_CASE);

            /* give up if all we are given is the system id for the doctype */
            str1 = getString(this.lexbuf, doctype.start + 5, 7);
            if (wstrcasecmp(str1, "SYSTEM ") == 0)
            {
                /* but at least ensure the case is correct */
                if (!str1.Substring(0, (6) - (0)).Equals("SYSTEM"))
                    Array.Copy(getBytes("SYSTEM"), 0, this.lexbuf, doctype.start + 5, 6);
                return 0; /* unrecognized */
            }

            if (wstrcasecmp(str1, "PUBLIC ") == 0)
            {
                if (!str1.Substring(0, (6) - (0)).Equals("PUBLIC"))
                    Array.Copy(getBytes("PUBLIC "), 0, this.lexbuf, doctype.start + 5, 6);
            }
            else
                this.badDoctype = true;

            for (i = doctype.start; i < doctype.end; ++i)
            {
                if (this.lexbuf[i] == (sbyte)'"')
                {
                    str1 = getString(this.lexbuf, i + 1, 12);
                    str2 = getString(this.lexbuf, i + 1, 13);
                    if (str1.Equals("-//W3C//DTD "))
                    {
                        /* compute length of identifier e.g. "HTML 4.0 Transitional" */
                        for (j = i + 13; j < doctype.end && this.lexbuf[j] != (sbyte)'/'; ++j)
                            ;
                        len = j - i - 13;
                        p = getString(this.lexbuf, i + 13, len);

                        for (j = 1; j < W3CVersion.Length; ++j)
                        {
                            s = W3CVersion[j].name;
                            if (len == s.Length && s.Equals(p))
                                return W3CVersion[j].code;
                        }

                        /* else unrecognized version */
                    }
                    else if (str2.Equals("-//IETF//DTD "))
                    {
                        /* compute length of identifier e.g. "HTML 2.0" */
                        for (j = i + 14; j < doctype.end && this.lexbuf[j] != (sbyte)'/'; ++j)
                            ;
                        len = j - i - 14;

                        p = getString(this.lexbuf, i + 14, len);
                        s = W3CVersion[0].name;
                        if (len == s.Length && s.Equals(p))
                            return W3CVersion[0].code;

                        /* else unrecognized version */
                    }
                    break;
                }
            }

            return 0;
        }

        public virtual void fixHTMLNameSpace(Node root, System.String profile)
        {
            Node node;
            AttVal prev, attr;

            for (node = root.content; node != null && node.tag != configuration.tt.tagHtml; node = node.next)
                ;

            if (node != null)
            {
                prev = null;

                for (attr = node.attributes; attr != null; attr = attr.next)
                {
                    if (attr.attribute.Equals("xmlns"))
                        break;

                    prev = attr;
                }

                if (attr != null)
                {
                    if (!attr.value_Renamed.Equals(profile))
                    {
                        Report.Warning(this, node, null, Report.INCONSISTENT_NAMESPACE);
                        attr.value_Renamed = profile;
                    }
                }
                else
                {
                    attr = new AttVal(node.attributes, null, (int)'"', "xmlns", profile);
                    attr.dict = AttributeTable.DefaultAttributeTable.findAttribute(attr);
                    node.attributes = attr;
                }
            }
        }

        public virtual bool setXHTMLDocType(Node root)
        {
            System.String fpi = " ";
            System.String sysid = "";
            System.String namespace_Renamed = XHTML_NAMESPACE;
            Node doctype;

            doctype = root.findDocType();

            if (configuration.docTypeMode == Configuration.DOCTYPE_OMIT)
            {
                if (doctype != null)
                    Node.discardElement(doctype);
                return true;
            }

            if (configuration.docTypeMode == Configuration.DOCTYPE_AUTO)
            {
                /* see what flavor of XHTML this document matches */
                if ((this.versions & Dict.VERS_HTML40_STRICT) != 0)
                {
                    /* use XHTML strict */
                    fpi = "-//W3C//DTD XHTML 1.0 Strict//EN";
                    sysid = voyager_strict;
                }
                else if ((this.versions & Dict.VERS_LOOSE) != 0)
                {
                    fpi = "-//W3C//DTD XHTML 1.0 Transitional//EN";
                    sysid = voyager_loose;
                }
                else if ((this.versions & Dict.VERS_FRAMES) != 0)
                {
                    /* use XHTML frames */
                    fpi = "-//W3C//DTD XHTML 1.0 Frameset//EN";
                    sysid = voyager_frameset;
                }
                /* lets assume XHTML transitional */
                else
                {
                    fpi = "-//W3C//DTD XHTML 1.0 Transitional//EN";
                    sysid = voyager_loose;
                }
            }
            else if (configuration.docTypeMode == Configuration.DOCTYPE_STRICT)
            {
                fpi = "-//W3C//DTD XHTML 1.0 Strict//EN";
                sysid = voyager_strict;
            }
            else if (configuration.docTypeMode == Configuration.DOCTYPE_LOOSE)
            {
                fpi = "-//W3C//DTD XHTML 1.0 Transitional//EN";
                sysid = voyager_loose;
            }

            fixHTMLNameSpace(root, namespace_Renamed);

            if (doctype == null)
            {
                doctype = newNode(Node.DocTypeTag, this.lexbuf, 0, 0);
                doctype.next = root.content;
                doctype.parent = root;
                doctype.prev = null;
                root.content = doctype;
            }

            if (configuration.docTypeMode == Configuration.DOCTYPE_USER && configuration.docTypeStr != null)
            {
                fpi = configuration.docTypeStr;
                sysid = "";
            }

            this.txtstart = this.lexsize;
            this.txtend = this.lexsize;

            /* add public identifier */
            addStringLiteral("html PUBLIC ");

            /* check if the fpi is quoted or not */
            if (fpi[0] == '"')
                addStringLiteral(fpi);
            else
            {
                addStringLiteral("\"");
                addStringLiteral(fpi);
                addStringLiteral("\"");
            }

            if (sysid.Length + 6 >= this.configuration.wraplen)
                addStringLiteral("\n\"");
            else
                addStringLiteral("\n    \"");

            /* add system identifier */
            addStringLiteral(sysid);
            addStringLiteral("\"");

            this.txtend = this.lexsize;

            doctype.start = this.txtstart;
            doctype.end = this.txtend;

            return false;
        }

        public virtual short apparentVersion()
        {
            switch (this.doctype)
            {

                case Dict.VERS_UNKNOWN:
                    return HTMLVersion();


                case Dict.VERS_HTML20:
                    if ((this.versions & Dict.VERS_HTML20) != 0)
                        return Dict.VERS_HTML20;

                    break;


                case Dict.VERS_HTML32:
                    if ((this.versions & Dict.VERS_HTML32) != 0)
                        return Dict.VERS_HTML32;

                    break; /* to replace old version by new */


                case Dict.VERS_HTML40_STRICT:
                    if ((this.versions & Dict.VERS_HTML40_STRICT) != 0)
                        return Dict.VERS_HTML40_STRICT;

                    break;


                case Dict.VERS_HTML40_LOOSE:
                    if ((this.versions & Dict.VERS_HTML40_LOOSE) != 0)
                        return Dict.VERS_HTML40_LOOSE;

                    break; /* to replace old version by new */


                case Dict.VERS_FRAMES:
                    if ((this.versions & Dict.VERS_FRAMES) != 0)
                        return Dict.VERS_FRAMES;

                    break;
            }

            Report.Warning(this, null, null, Report.INCONSISTENT_VERSION);
            return this.HTMLVersion();
        }

        /* fixup doctype if missing */
        public virtual bool fixDocType(Node root)
        {
            Node doctype;
            int guessed = Dict.VERS_HTML40_STRICT, i;

            if (this.badDoctype)
                Report.Warning(this, null, null, Report.MALFORMED_DOCTYPE);

            if (configuration.XmlOut)
                return true;

            doctype = root.findDocType();

            if (configuration.docTypeMode == Configuration.DOCTYPE_OMIT)
            {
                if (doctype != null)
                    Node.discardElement(doctype);
                return true;
            }

            if (configuration.docTypeMode == Configuration.DOCTYPE_STRICT)
            {
                Node.discardElement(doctype);
                doctype = null;
                guessed = Dict.VERS_HTML40_STRICT;
            }
            else if (configuration.docTypeMode == Configuration.DOCTYPE_LOOSE)
            {
                Node.discardElement(doctype);
                doctype = null;
                guessed = Dict.VERS_HTML40_LOOSE;
            }
            else if (configuration.docTypeMode == Configuration.DOCTYPE_AUTO)
            {
                if (doctype != null)
                {
                    if (this.doctype == Dict.VERS_UNKNOWN)
                        return false;

                    switch (this.doctype)
                    {

                        case Dict.VERS_UNKNOWN:
                            return false;


                        case Dict.VERS_HTML20:
                            if ((this.versions & Dict.VERS_HTML20) != 0)
                                return true;

                            break; /* to replace old version by new */


                        case Dict.VERS_HTML32:
                            if ((this.versions & Dict.VERS_HTML32) != 0)
                                return true;

                            break; /* to replace old version by new */


                        case Dict.VERS_HTML40_STRICT:
                            if ((this.versions & Dict.VERS_HTML40_STRICT) != 0)
                                return true;

                            break; /* to replace old version by new */


                        case Dict.VERS_HTML40_LOOSE:
                            if ((this.versions & Dict.VERS_HTML40_LOOSE) != 0)
                                return true;

                            break; /* to replace old version by new */


                        case Dict.VERS_FRAMES:
                            if ((this.versions & Dict.VERS_FRAMES) != 0)
                                return true;

                            break; /* to replace old version by new */
                    }

                    /* INCONSISTENT_VERSION warning is now issued by ApparentVersion() */
                }

                /* choose new doctype */
                guessed = HTMLVersion();
            }

            if (guessed == Dict.VERS_UNKNOWN)
                return false;

            /* for XML use the Voyager system identifier */
            if (this.configuration.XmlOut || this.configuration.XmlTags || this.isvoyager)
            {
                if (doctype != null)
                    Node.discardElement(doctype);

                for (i = 0; i < W3CVersion.Length; ++i)
                {
                    if (guessed == W3CVersion[i].code)
                    {
                        fixHTMLNameSpace(root, W3CVersion[i].profile);
                        break;
                    }
                }

                return true;
            }

            if (doctype == null)
            {
                doctype = newNode(Node.DocTypeTag, this.lexbuf, 0, 0);
                doctype.next = root.content;
                doctype.parent = root;
                doctype.prev = null;
                root.content = doctype;
            }

            this.txtstart = this.lexsize;
            this.txtend = this.lexsize;

            /* use the appropriate public identifier */
            addStringLiteral("html PUBLIC ");

            if (configuration.docTypeMode == Configuration.DOCTYPE_USER && configuration.docTypeStr != null)
                addStringLiteral(configuration.docTypeStr);
            else if (guessed == Dict.VERS_HTML20)
                addStringLiteral("\"-//IETF//DTD HTML 2.0//EN\"");
            else
            {
                addStringLiteral("\"-//W3C//DTD ");

                for (i = 0; i < W3CVersion.Length; ++i)
                {
                    if (guessed == W3CVersion[i].code)
                    {
                        addStringLiteral(W3CVersion[i].name);
                        break;
                    }
                }

                addStringLiteral("//EN\"");
            }

            this.txtend = this.lexsize;

            doctype.start = this.txtstart;
            doctype.end = this.txtend;

            return true;
        }

        /* ensure XML document starts with <?XML version="1.0"?> */
        public virtual bool fixXMLPI(Node root)
        {
            Node xml;
            int s;

            if (root.content != null && root.content.type == Node.ProcInsTag)
            {
                s = root.content.start;

                if (this.lexbuf[s] == (sbyte)'x' && this.lexbuf[s + 1] == (sbyte)'m' && this.lexbuf[s + 2] == (sbyte)'l')
                    return true;
            }

            xml = newNode(Node.ProcInsTag, this.lexbuf, 0, 0);
            xml.next = root.content;

            if (root.content != null)
            {
                root.content.prev = xml;
                xml.next = root.content;
            }

            root.content = xml;

            this.txtstart = this.lexsize;
            this.txtend = this.lexsize;
            addStringLiteral("xml version=\"1.0\"");
            if (this.configuration.CharEncoding == Configuration.LATIN1)
                addStringLiteral(" encoding=\"ISO-8859-1\"");
            this.txtend = this.lexsize;

            xml.start = this.txtstart;
            xml.end = this.txtend;
            return false;
        }

        public virtual Node inferredTag(System.String name)
        {
            Node node;

            node = newNode(Node.StartTag, this.lexbuf, this.txtstart, this.txtend, name);
            node.implicit_Renamed = true;
            return node;
        }

        public static bool expectsContent(Node node)
        {
            if (node.type != Node.StartTag)
                return false;

            /* unknown element? */
            if (node.tag == null)
                return true;

            if ((node.tag.model & Dict.CM_EMPTY) != 0)
                return false;

            return true;
        }

        /*
        create a text node for the contents of
        a CDATA element like style or script
        which ends with </foo> for some foo.
        */
        public virtual Node getCDATA(Node container)
        {
            int c, lastc, start, len, i;
            System.String str;
            bool endtag = false;

            this.lines = this.in_Renamed.curline;
            this.columns = this.in_Renamed.curcol;
            this.waswhite = false;
            this.txtstart = this.lexsize;
            this.txtend = this.lexsize;

            lastc = (int)'\x0000';
            start = -1;

            while (true)
            {
                c = this.in_Renamed.ReadChar();
                if (c == StreamIn.EndOfStream)
                    break;
                /* treat \r\n as \n and \r as \n */

                if (c == (int)'/' && lastc == (int)'<')
                {
                    if (endtag)
                    {
                        this.lines = this.in_Renamed.curline;
                        this.columns = this.in_Renamed.curcol - 3;

                        Report.Warning(this, null, null, Report.BAD_CDATA_CONTENT);
                    }

                    start = this.lexsize + 1; /* to first letter */
                    endtag = true;
                }
                else if (c == (int)'>' && start >= 0)
                {
                    len = this.lexsize - start;
                    if (len == container.element.Length)
                    {
                        str = getString(this.lexbuf, start, len);
                        if (Lexer.wstrcasecmp(str, container.element) == 0)
                        {
                            this.txtend = start - 2;
                            break;
                        }
                    }

                    this.lines = this.in_Renamed.curline;
                    this.columns = this.in_Renamed.curcol - 3;

                    Report.Warning(this, null, null, Report.BAD_CDATA_CONTENT);

                    /* if javascript insert backslash before / */

                    if (ParserImpl.IsJavaScript(container))
                    {
                        for (i = this.lexsize; i > start - 1; --i)
                            this.lexbuf[i] = this.lexbuf[i - 1];

                        this.lexbuf[start - 1] = (sbyte)'\\';
                        this.lexsize++;
                    }

                    start = -1;
                }
                else if (c == (int)'\r')
                {
                    c = this.in_Renamed.ReadChar();

                    if (c != (int)'\n')
                        this.in_Renamed.UngetChar(c);

                    c = (int)'\n';
                }

                addCharToLexer((int)c);
                this.txtend = this.lexsize;
                lastc = c;
            }

            if (c == StreamIn.EndOfStream)
                Report.Warning(this, container, null, Report.MISSING_ENDTAG_FOR);

            if (this.txtend > this.txtstart)
            {
                this.token = newNode(Node.TextNode, this.lexbuf, this.txtstart, this.txtend);
                return this.token;
            }

            return null;
        }

        public virtual void ungetToken()
        {
            this.pushed = true;
        }

        public const short IgnoreWhitespace = 0;
        public const short MixedContent = 1;
        public const short Preformatted = 2;
        public const short IgnoreMarkup = 3;

        /// <summary>
        /// Gets the next token.
        /// </summary>
        /// <remarks>
        /// Modes for GetToken():		
        /// <para>
        /// MixedContent   -- for elements which don't accept PCDATA<br/>
        /// Preformatted   -- white space preserved as is<br/>
        /// IgnoreMarkup   -- for CDATA elements such as script, style<br/>
        /// </para>
        /// </remarks>
        /// <param name="mode"></param>
        /// <returns></returns>
        public virtual Node GetToken(short mode)
        {
            short map;
            int c = 0;
            int lastc;
            int badcomment = 0;
            MutableBoolean isempty = new MutableBoolean();
            AttVal attributes;

            if (this.pushed)
            {
                /* duplicate inlines in preference to pushed text nodes when appropriate */
                if (this.token.type != Node.TextNode || (this.insert == -1 && this.inode == null))
                {
                    this.pushed = false;
                    return this.token;
                }
            }

            /* at start of block elements, unclosed inline
            elements are inserted into the token stream */

            if (this.insert != -1 || this.inode != null)
                return InsertedToken();

            this.lines = this.in_Renamed.curline;
            this.columns = this.in_Renamed.curcol;
            this.waswhite = false;

            this.txtstart = this.lexsize;
            this.txtend = this.lexsize;

            while (true)
            {
                c = this.in_Renamed.ReadChar();
                if (c == StreamIn.EndOfStream)
                    break;
                if (this.insertspace && mode != IgnoreWhitespace)
                {
                    addCharToLexer(' ');
                    this.waswhite = true;
                    this.insertspace = false;
                }

                /* treat \r\n as \n and \r as \n */

                if (c == '\r')
                {
                    c = this.in_Renamed.ReadChar();

                    if (c != '\n')
                        this.in_Renamed.UngetChar(c);

                    c = '\n';
                }

                addCharToLexer(c);

                switch (this.state)
                {

                    case LEX_CONTENT:  /* element content */
                        map = MAP((char)c);

                        /*
                        Discard white space if appropriate. Its cheaper
                        to do this here rather than in parser methods
                        for elements that don't have mixed content.
                        */
                        if (((map & WHITE) != 0) && (mode == IgnoreWhitespace) && this.lexsize == this.txtstart + 1)
                        {
                            --this.lexsize;
                            this.waswhite = false;
                            this.lines = this.in_Renamed.curline;
                            this.columns = this.in_Renamed.curcol;
                            continue;
                        }

                        if (c == '<')
                        {
                            this.state = LEX_GT;
                            continue;
                        }

                        if ((map & WHITE) != 0)
                        {
                            /* was previous char white? */
                            if (this.waswhite)
                            {
                                if (mode != Preformatted && mode != IgnoreMarkup)
                                {
                                    --this.lexsize;
                                    this.lines = this.in_Renamed.curline;
                                    this.columns = this.in_Renamed.curcol;
                                }
                            }
                            /* prev char wasn't white */
                            else
                            {
                                this.waswhite = true;
                                lastc = c;

                                if (mode != Preformatted && mode != IgnoreMarkup && c != ' ')
                                    changeChar((sbyte)' ');
                            }

                            continue;
                        }
                        else if (c == '&' && mode != IgnoreMarkup)
                            parseEntity(mode);

                        /* this is needed to avoid trimming trailing whitespace */
                        if (mode == IgnoreWhitespace)
                            mode = MixedContent;

                        this.waswhite = false;
                        continue;


                    case LEX_GT:  /* < */

                        /* check for endtag */
                        if (c == '/')
                        {
                            c = this.in_Renamed.ReadChar();
                            if (c == StreamIn.EndOfStream)
                            {
                                this.in_Renamed.UngetChar(c);
                                continue;
                            }

                            addCharToLexer(c);
                            map = MAP((char)c);

                            if ((map & LETTER) != 0)
                            {
                                this.lexsize -= 3;
                                this.txtend = this.lexsize;
                                this.in_Renamed.UngetChar(c);
                                this.state = LEX_ENDTAG;
                                this.lexbuf[this.lexsize] = (sbyte)'\x0000'; /* debug */
                                this.in_Renamed.curcol -= 2;

                                /* if some text before the </ return it now */
                                if (this.txtend > this.txtstart)
                                {
                                    /* trim space char before end tag */
                                    if (mode == IgnoreWhitespace && this.lexbuf[this.lexsize - 1] == (sbyte)' ')
                                    {
                                        this.lexsize -= 1;
                                        this.txtend = this.lexsize;
                                    }

                                    this.token = newNode(Node.TextNode, this.lexbuf, this.txtstart, this.txtend);
                                    return this.token;
                                }

                                continue; /* no text so keep going */
                            }

                            /* otherwise treat as CDATA */
                            this.waswhite = false;
                            this.state = LEX_CONTENT;
                            continue;
                        }

                        if (mode == IgnoreMarkup)
                        {
                            /* otherwise treat as CDATA */
                            this.waswhite = false;
                            this.state = LEX_CONTENT;
                            continue;
                        }

                        /*
                        look out for comments, doctype or marked sections
                        this isn't quite right, but its getting there ...
                        */
                        if (c == '!')
                        {
                            c = this.in_Renamed.ReadChar();

                            if (c == '-')
                            {
                                c = this.in_Renamed.ReadChar();

                                if (c == '-')
                                {
                                    this.state = LEX_COMMENT; /* comment */
                                    this.lexsize -= 2;
                                    this.txtend = this.lexsize;

                                    /* if some text before < return it now */
                                    if (this.txtend > this.txtstart)
                                    {
                                        this.token = newNode(Node.TextNode, this.lexbuf, this.txtstart, this.txtend);
                                        return this.token;
                                    }

                                    this.txtstart = this.lexsize;
                                    continue;
                                }

                                Report.Warning(this, null, null, Report.MALFORMED_COMMENT);
                            }
                            else if (c == 'd' || c == 'D')
                            {
                                this.state = LEX_DOCTYPE; /* doctype */
                                this.lexsize -= 2;
                                this.txtend = this.lexsize;
                                mode = IgnoreWhitespace;

                                /* skip until white space or '>' */

                                for (; ; )
                                {
                                    c = this.in_Renamed.ReadChar();

                                    if (c == StreamIn.EndOfStream || c == '>')
                                    {
                                        this.in_Renamed.UngetChar(c);
                                        break;
                                    }

                                    map = MAP((char)c);

                                    if ((map & WHITE) == 0)
                                        continue;

                                    /* and skip to end of whitespace */

                                    for (; ; )
                                    {
                                        c = this.in_Renamed.ReadChar();

                                        if (c == StreamIn.EndOfStream || c == '>')
                                        {
                                            this.in_Renamed.UngetChar(c);
                                            break;
                                        }

                                        map = MAP((char)c);

                                        if ((map & WHITE) != 0)
                                            continue;

                                        this.in_Renamed.UngetChar(c);
                                        break;
                                    }

                                    break;
                                }

                                /* if some text before < return it now */
                                if (this.txtend > this.txtstart)
                                {
                                    this.token = newNode(Node.TextNode, this.lexbuf, this.txtstart, this.txtend);
                                    return this.token;
                                }

                                this.txtstart = this.lexsize;
                                continue;
                            }
                            else if (c == '[')
                            {
                                /* Word 2000 embeds <![if ...]> ... <![endif]> sequences */
                                this.lexsize -= 2;
                                this.state = LEX_SECTION;
                                this.txtend = this.lexsize;

                                /* if some text before < return it now */
                                if (this.txtend > this.txtstart)
                                {
                                    this.token = newNode(Node.TextNode, this.lexbuf, this.txtstart, this.txtend);
                                    return this.token;
                                }

                                this.txtstart = this.lexsize;
                                continue;
                            }

                            /* otherwise swallow chars up to and including next '>' */
                            while (true)
                            {
                                c = this.in_Renamed.ReadChar();
                                if (c == '>')
                                    break;
                                if (c == -1)
                                {
                                    this.in_Renamed.UngetChar(c);
                                    break;
                                }
                            }

                            this.lexsize -= 2;
                            this.lexbuf[this.lexsize] = (sbyte)'\x0000';
                            this.state = LEX_CONTENT;
                            continue;
                        }

                        /*
                        processing instructions
                        */

                        if (c == '?')
                        {
                            this.lexsize -= 2;
                            this.state = LEX_PROCINSTR;
                            this.txtend = this.lexsize;

                            /* if some text before < return it now */
                            if (this.txtend > this.txtstart)
                            {
                                this.token = newNode(Node.TextNode, this.lexbuf, this.txtstart, this.txtend);
                                return this.token;
                            }

                            this.txtstart = this.lexsize;
                            continue;
                        }

                        /* Microsoft ASP's e.g. <% ... server-code ... %> */
                        if (c == '%')
                        {
                            this.lexsize -= 2;
                            this.state = LEX_ASP;
                            this.txtend = this.lexsize;

                            /* if some text before < return it now */
                            if (this.txtend > this.txtstart)
                            {
                                this.token = newNode(Node.TextNode, this.lexbuf, this.txtstart, this.txtend);
                                return this.token;
                            }

                            this.txtstart = this.lexsize;
                            continue;
                        }

                        /* Netscapes JSTE e.g. <# ... server-code ... #> */
                        if (c == '#')
                        {
                            this.lexsize -= 2;
                            this.state = LEX_JSTE;
                            this.txtend = this.lexsize;

                            /* if some text before < return it now */
                            if (this.txtend > this.txtstart)
                            {
                                this.token = newNode(Node.TextNode, this.lexbuf, this.txtstart, this.txtend);
                                return this.token;
                            }

                            this.txtstart = this.lexsize;
                            continue;
                        }

                        map = MAP((char)c);

                        /* check for start tag */
                        if ((map & LETTER) != 0)
                        {
                            this.in_Renamed.UngetChar(c); /* push back letter */
                            this.lexsize -= 2; /* discard "<" + letter */
                            this.txtend = this.lexsize;
                            this.state = LEX_STARTTAG; /* ready to read tag name */

                            /* if some text before < return it now */
                            if (this.txtend > this.txtstart)
                            {
                                this.token = newNode(Node.TextNode, this.lexbuf, this.txtstart, this.txtend);
                                return this.token;
                            }

                            continue; /* no text so keep going */
                        }

                        /* otherwise treat as CDATA */
                        this.state = LEX_CONTENT;
                        this.waswhite = false;
                        continue;


                    case LEX_ENDTAG:  /* </letter */
                        this.txtstart = this.lexsize - 1;
                        this.in_Renamed.curcol += 2;
                        c = parseTagName();
                        this.token = newNode(Node.EndTag, this.lexbuf, this.txtstart, this.txtend, getString(this.lexbuf, this.txtstart, this.txtend - this.txtstart));
                        this.lexsize = this.txtstart;
                        this.txtend = this.txtstart;

                        /* skip to '>' */
                        while (c != '>')
                        {
                            c = this.in_Renamed.ReadChar();

                            if (c == StreamIn.EndOfStream)
                                break;
                        }

                        if (c == StreamIn.EndOfStream)
                        {
                            this.in_Renamed.UngetChar(c);
                            continue;
                        }

                        this.state = LEX_CONTENT;
                        this.waswhite = false;
                        return this.token; /* the endtag token */


                    case LEX_STARTTAG:  /* first letter of tagname */
                        this.txtstart = this.lexsize - 1; /* set txtstart to first letter */
                        c = parseTagName();
                        isempty.value_Renamed = false;
                        attributes = null;
                        this.token = newNode((isempty.value_Renamed ? Node.StartEndTag : Node.StartTag), this.lexbuf, this.txtstart, this.txtend, getString(this.lexbuf, this.txtstart, this.txtend - this.txtstart));

                        /* parse attributes, consuming closing ">" */
                        if (c != '>')
                        {
                            if (c == '/')
                                this.in_Renamed.UngetChar(c);

                            attributes = ParseAttrs(isempty);
                        }

                        if (isempty.value_Renamed)
                            this.token.type = Node.StartEndTag;

                        this.token.attributes = attributes;
                        this.lexsize = this.txtstart;
                        this.txtend = this.txtstart;

                        /* swallow newline following start tag */
                        /* special check needed for CRLF sequence */
                        /* this doesn't apply to empty elements */

                        if (expectsContent(this.token) || this.token.tag == configuration.tt.tagBr)
                        {

                            c = this.in_Renamed.ReadChar();

                            if (c == '\r')
                            {
                                c = this.in_Renamed.ReadChar();

                                if (c != '\n')
                                    this.in_Renamed.UngetChar(c);
                            }
                            else if (c != '\n' && c != '\f')
                                this.in_Renamed.UngetChar(c);

                            this.waswhite = true; /* to swallow leading whitespace */
                        }
                        else
                            this.waswhite = false;

                        this.state = LEX_CONTENT;

                        if (this.token.tag == null)
                            Report.error(this, null, this.token, Report.UNKNOWN_ELEMENT);
                        else if (!this.configuration.XmlTags)
                        {
                            this.versions &= this.token.tag.versions;

                            if ((this.token.tag.versions & Dict.VERS_PROPRIETARY) != 0)
                            {
                                if (!this.configuration.MakeClean && (this.token.tag == configuration.tt.tagNobr || this.token.tag == configuration.tt.tagWbr))
                                    Report.Warning(this, null, this.token, Report.PROPRIETARY_ELEMENT);
                            }

                            if (this.token.tag.chkattrs != null)
                            {
                                this.token.checkUniqueAttributes(this);
                                this.token.tag.chkattrs.Check(this, this.token);
                            }
                            else
                                this.token.checkAttributes(this);
                        }

                        return this.token; /* return start tag */


                    case LEX_COMMENT:  /* seen <!-- so look for --> */

                        if (c != '-')
                            continue;

                        c = this.in_Renamed.ReadChar();
                        addCharToLexer(c);

                        if (c != '-')
                            continue;

                        while (true)
                        {
                            c = this.in_Renamed.ReadChar();

                            if (c == '>')
                            {
                                if (badcomment != 0)
                                    Report.Warning(this, null, null, Report.MALFORMED_COMMENT);

                                this.txtend = this.lexsize - 2; // AQ 8Jul2000
                                this.lexbuf[this.lexsize] = (sbyte)'\x0000';
                                this.state = LEX_CONTENT;
                                this.waswhite = false;
                                this.token = newNode(Node.CommentTag, this.lexbuf, this.txtstart, this.txtend);

                                /* now look for a line break */

                                c = this.in_Renamed.ReadChar();

                                if (c == '\r')
                                {
                                    c = this.in_Renamed.ReadChar();

                                    if (c != '\n')
                                        this.token.linebreak = true;
                                }

                                if (c == '\n')
                                    this.token.linebreak = true;
                                else
                                    this.in_Renamed.UngetChar(c);

                                return this.token;
                            }

                            /* note position of first such error in the comment */
                            if (badcomment == 0)
                            {
                                this.lines = this.in_Renamed.curline;
                                this.columns = this.in_Renamed.curcol - 3;
                            }

                            badcomment++;
                            if (this.configuration.FixComments)
                                this.lexbuf[this.lexsize - 2] = (sbyte)'=';

                            addCharToLexer(c);

                            /* if '-' then look for '>' to end the comment */
                            if (c != '-')
                            {
                                //UPGRADE_NOTE: Labeled break statement was changed to a goto statement. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1012'"
                                goto end_comment_brk;
                            }
                        }
                    //UPGRADE_NOTE: Label 'end_comment_brk' was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1011'"

end_comment_brk: ;

                        /* otherwise continue to look for --> */
                        this.lexbuf[this.lexsize - 2] = (sbyte)'=';
                        continue;


                    case LEX_DOCTYPE:  /* seen <!d so look for '>' munging whitespace */
                        map = MAP((char)c);

                        if ((map & WHITE) != 0)
                        {
                            if (this.waswhite)
                                this.lexsize -= 1;

                            this.waswhite = true;
                        }
                        else
                            this.waswhite = false;

                        if (c != '>')
                            continue;

                        this.lexsize -= 1;
                        this.txtend = this.lexsize;
                        this.lexbuf[this.lexsize] = (sbyte)'\x0000';
                        this.state = LEX_CONTENT;
                        this.waswhite = false;
                        this.token = newNode(Node.DocTypeTag, this.lexbuf, this.txtstart, this.txtend);
                        /* make a note of the version named by the doctype */
                        this.doctype = findGivenVersion(this.token);
                        return this.token;


                    case LEX_PROCINSTR:  /* seen <? so look for '>' */
                        /* check for PHP preprocessor instructions <?php ... ?> */

                        if (this.lexsize - this.txtstart == 3)
                        {
                            if ((getString(this.lexbuf, this.txtstart, 3)).Equals("php"))
                            {
                                this.state = LEX_PHP;
                                continue;
                            }
                        }

                        if (this.configuration.XmlPIs)
                        /* insist on ?> as terminator */
                        {
                            if (c != '?')
                                continue;

                            /* now look for '>' */
                            c = this.in_Renamed.ReadChar();

                            if (c == StreamIn.EndOfStream)
                            {
                                Report.Warning(this, null, null, Report.UNEXPECTED_END_OF_FILE);
                                this.in_Renamed.UngetChar(c);
                                continue;
                            }

                            addCharToLexer(c);
                        }

                        if (c != '>')
                            continue;

                        this.lexsize -= 1;
                        this.txtend = this.lexsize;
                        this.lexbuf[this.lexsize] = (sbyte)'\x0000';
                        this.state = LEX_CONTENT;
                        this.waswhite = false;
                        this.token = newNode(Node.ProcInsTag, this.lexbuf, this.txtstart, this.txtend);
                        return this.token;


                    case LEX_ASP:  /* seen <% so look for "%>" */
                        if (c != '%')
                            continue;

                        /* now look for '>' */
                        c = this.in_Renamed.ReadChar();


                        if (c != '>')
                        {
                            this.in_Renamed.UngetChar(c);
                            continue;
                        }

                        this.lexsize -= 1;
                        this.txtend = this.lexsize;
                        this.lexbuf[this.lexsize] = (sbyte)'\x0000';
                        this.state = LEX_CONTENT;
                        this.waswhite = false;
                        this.token = newNode(Node.AspTag, this.lexbuf, this.txtstart, this.txtend);
                        return this.token;


                    case LEX_JSTE:  /* seen <# so look for "#>" */
                        if (c != '#')
                            continue;

                        /* now look for '>' */
                        c = this.in_Renamed.ReadChar();


                        if (c != '>')
                        {
                            this.in_Renamed.UngetChar(c);
                            continue;
                        }

                        this.lexsize -= 1;
                        this.txtend = this.lexsize;
                        this.lexbuf[this.lexsize] = (sbyte)'\x0000';
                        this.state = LEX_CONTENT;
                        this.waswhite = false;
                        this.token = newNode(Node.JsteTag, this.lexbuf, this.txtstart, this.txtend);
                        return this.token;


                    case LEX_PHP:  /* seen "<?php" so look for "?>" */
                        if (c != '?')
                            continue;

                        /* now look for '>' */
                        c = this.in_Renamed.ReadChar();

                        if (c != '>')
                        {
                            this.in_Renamed.UngetChar(c);
                            continue;
                        }

                        this.lexsize -= 1;
                        this.txtend = this.lexsize;
                        this.lexbuf[this.lexsize] = (sbyte)'\x0000';
                        this.state = LEX_CONTENT;
                        this.waswhite = false;
                        this.token = newNode(Node.PhpTag, this.lexbuf, this.txtstart, this.txtend);
                        return this.token;


                    case LEX_SECTION:  /* seen "<![" so look for "]>" */
                        if (c == '[')
                        {
                            if (this.lexsize == (this.txtstart + 6) && (getString(this.lexbuf, this.txtstart, 6)).Equals("CDATA["))
                            {
                                this.state = LEX_CDATA;
                                this.lexsize -= 6;
                                continue;
                            }
                        }

                        if (c != ']')
                            continue;

                        /* now look for '>' */
                        c = this.in_Renamed.ReadChar();

                        if (c != '>')
                        {
                            this.in_Renamed.UngetChar(c);
                            continue;
                        }

                        this.lexsize -= 1;
                        this.txtend = this.lexsize;
                        this.lexbuf[this.lexsize] = (sbyte)'\x0000';
                        this.state = LEX_CONTENT;
                        this.waswhite = false;
                        this.token = newNode(Node.SectionTag, this.lexbuf, this.txtstart, this.txtend);
                        return this.token;


                    case LEX_CDATA:  /* seen "<![CDATA[" so look for "]]>" */
                        if (c != ']')
                            continue;

                        /* now look for ']' */
                        c = this.in_Renamed.ReadChar();

                        if (c != ']')
                        {
                            this.in_Renamed.UngetChar(c);
                            continue;
                        }

                        /* now look for '>' */
                        c = this.in_Renamed.ReadChar();

                        if (c != '>')
                        {
                            this.in_Renamed.UngetChar(c);
                            continue;
                        }

                        this.lexsize -= 1;
                        this.txtend = this.lexsize;
                        this.lexbuf[this.lexsize] = (sbyte)'\x0000';
                        this.state = LEX_CONTENT;
                        this.waswhite = false;
                        this.token = newNode(Node.CDATATag, this.lexbuf, this.txtstart, this.txtend);
                        return this.token;
                }
            }

            if (this.state == LEX_CONTENT)
            /* text string */
            {
                this.txtend = this.lexsize;

                if (this.txtend > this.txtstart)
                {
                    this.in_Renamed.UngetChar(c);

                    if (this.lexbuf[this.lexsize - 1] == (sbyte)' ')
                    {
                        this.lexsize -= 1;
                        this.txtend = this.lexsize;
                    }

                    this.token = newNode(Node.TextNode, this.lexbuf, this.txtstart, this.txtend);
                    return this.token;
                }
            }
            else if (this.state == LEX_COMMENT)
            /* comment */
            {
                if (c == StreamIn.EndOfStream)
                    Report.Warning(this, null, null, Report.MALFORMED_COMMENT);

                this.txtend = this.lexsize;
                this.lexbuf[this.lexsize] = (sbyte)'\x0000';
                this.state = LEX_CONTENT;
                this.waswhite = false;
                this.token = newNode(Node.CommentTag, this.lexbuf, this.txtstart, this.txtend);
                return this.token;
            }

            return null;
        }

        /// <summary>
        /// parser for ASP within start tags
        /// </summary>
        /// <remarks>
        /// Some people use ASP for to customize attributes
        /// Tidy isn't really well suited to dealing with ASP
        /// This is a workaround for attributes, but won't
        /// deal with the case where the ASP is used to tailor
        /// the attribute value. Here is an example of a work
        /// around for using ASP in attribute values:
        /// <code>
        /// href="&lt;%=rsSchool.Fields("ID").Value%&gt;"
        /// </code>
        /// where the ASP that generates the attribute value
        /// is masked from Tidy by the quotemarks.
        /// </remarks>
        /// <returns></returns>
        public virtual Node ParseAsp()
        {
            int c;
            Node asp = null;

            this.txtstart = this.lexsize;

            for (; ; )
            {
                c = this.in_Renamed.ReadChar();
                addCharToLexer(c);


                if (c != '%')
                    continue;

                c = this.in_Renamed.ReadChar();
                addCharToLexer(c);

                if (c == '>')
                    break;
            }

            this.lexsize -= 2;
            this.txtend = this.lexsize;

            if (this.txtend > this.txtstart)
                asp = newNode(Node.AspTag, this.lexbuf, this.txtstart, this.txtend);

            this.txtstart = this.txtend;
            return asp;
        }

        /*
        PHP is like ASP but is based upon XML
        processing instructions, e.g. <?php ... ?>
        */
        public virtual Node parsePhp()
        {
            int c;
            Node php = null;

            this.txtstart = this.lexsize;

            for (; ; )
            {
                c = this.in_Renamed.ReadChar();
                addCharToLexer(c);


                if (c != '?')
                    continue;

                c = this.in_Renamed.ReadChar();
                addCharToLexer(c);

                if (c == '>')
                    break;
            }

            this.lexsize -= 2;
            this.txtend = this.lexsize;

            if (this.txtend > this.txtstart)
                php = newNode(Node.PhpTag, this.lexbuf, this.txtstart, this.txtend);

            this.txtstart = this.txtend;
            return php;
        }

        /* consumes the '>' terminating start tags */
        public virtual System.String parseAttribute(MutableBoolean isempty, MutableObject asp, MutableObject php)
        {
            int start = 0;
            // int len = 0;   Removed by BUGFIX for 126265
            short map;
            System.String attr;
            int c = 0;

            asp.Object = null; /* clear asp pointer */
            php.Object = null; /* clear php pointer */
            /* skip white space before the attribute */

            for (; ; )
            {
                c = this.in_Renamed.ReadChar();

                if (c == '/')
                {
                    c = this.in_Renamed.ReadChar();

                    if (c == '>')
                    {
                        isempty.value_Renamed = true;
                        return null;
                    }

                    this.in_Renamed.UngetChar(c);
                    c = '/';
                    break;
                }

                if (c == '>')
                    return null;

                if (c == '<')
                {
                    c = this.in_Renamed.ReadChar();

                    if (c == '%')
                    {
                        asp.Object = ParseAsp();
                        return null;
                    }
                    else if (c == '?')
                    {
                        php.Object = parsePhp();
                        return null;
                    }

                    this.in_Renamed.UngetChar(c);
                    Report.attrError(this, this.token, null, Report.UNEXPECTED_GT);
                    return null;
                }

                if (c == '"' || c == '\'')
                {
                    Report.attrError(this, this.token, null, Report.UNEXPECTED_QUOTEMARK);
                    continue;
                }

                if (c == StreamIn.EndOfStream)
                {
                    Report.attrError(this, this.token, null, Report.UNEXPECTED_END_OF_FILE);
                    this.in_Renamed.UngetChar(c);
                    return null;
                }

                map = MAP((char)c);

                if ((map & WHITE) == 0)
                    break;
            }

            start = this.lexsize;

            for (; ; )
            {
                /* but push back '=' for parseValue() */
                if (c == '=' || c == '>')
                {
                    this.in_Renamed.UngetChar(c);
                    break;
                }

                if (c == '<' || c == StreamIn.EndOfStream)
                {
                    this.in_Renamed.UngetChar(c);
                    break;
                }

                map = MAP((char)c);

                if ((map & WHITE) != 0)
                    break;

                /* what should be done about non-namechar characters? */
                /* currently these are incorporated into the attr name */

                if (!this.configuration.XmlTags && (map & UPPERCASE) != 0)
                    c += (int)('a' - 'A');

                //  ++len;    Removed by BUGFIX for 126265 
                addCharToLexer(c);

                c = this.in_Renamed.ReadChar();
            }

            // Following line added by GLP to fix BUG 126265.  This is a temporary comment
            // and should be removed when Tidy is fixed.
            int len = this.lexsize - start;
            attr = (len > 0 ? getString(this.lexbuf, start, len) : null);
            this.lexsize = start;

            return attr;
        }

        /*
        invoked when < is seen in place of attribute value
        but terminates on whitespace if not ASP, PHP or Tango
        this routine recognizes ' and " quoted strings
        */
        public virtual int parseServerInstruction()
        {
            int c, map, delim = '"';
            bool isrule = false;

            c = this.in_Renamed.ReadChar();
            addCharToLexer(c);

            /* check for ASP, PHP or Tango */
            if (c == '%' || c == '?' || c == '@')
                isrule = true;

            for (; ; )
            {
                c = this.in_Renamed.ReadChar();

                if (c == StreamIn.EndOfStream)
                    break;

                if (c == '>')
                {
                    if (isrule)
                        addCharToLexer(c);
                    else
                        this.in_Renamed.UngetChar(c);

                    break;
                }

                /* if not recognized as ASP, PHP or Tango */
                /* then also finish value on whitespace */
                if (!isrule)
                {
                    map = MAP((char)c);

                    if ((map & WHITE) != 0)
                        break;
                }

                addCharToLexer(c);

                if (c == '"')
                {
                    do
                    {
                        c = this.in_Renamed.ReadChar();
                        addCharToLexer(c);
                    }
                    while (c != '"');
                    delim = '\'';
                    continue;
                }

                if (c == '\'')
                {
                    do
                    {
                        c = this.in_Renamed.ReadChar();
                        addCharToLexer(c);
                    }
                    while (c != '\'');
                }
            }

            return delim;
        }

        /// <summary>
        /// Parse a value
        /// </summary>
        /// <remarks>
        /// values start with "=" or " = " etc. doesn't consume the ">" at end of start tag
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="foldCase"></param>
        /// <param name="isempty"></param>
        /// <param name="pdelim"></param>
        /// <returns></returns>
        public virtual System.String ParseValue(System.String name, bool foldCase, MutableBoolean isempty, MutableInteger pdelim)
        {
            int len = 0;
            int start;
            short map;
            bool seen_gt = false;
            bool munge = true;
            int c = 0;
            int lastc, delim, quotewarning;
            System.String value_Renamed;

            delim = 0;
            pdelim.value_Renamed = (int)'"';

            /*
            Henry Zrepa reports that some folk are using the
            embed element with script attributes where newlines
            are significant and must be preserved
            */
            if (configuration.LiteralAttribs)
                munge = false;

            /* skip white space before the '=' */

            for (; ; )
            {
                c = this.in_Renamed.ReadChar();

                if (c == StreamIn.EndOfStream)
                {
                    this.in_Renamed.UngetChar(c);
                    break;
                }

                map = MAP((char)c);

                if ((map & WHITE) == 0)
                    break;
            }

            /*
            c should be '=' if there is a value
            other legal possibilities are white
            space, '/' and '>'
            */

            if (c != '=')
            {
                this.in_Renamed.UngetChar(c);
                return null;
            }

            /* skip white space after '=' */

            for (; ; )
            {
                c = this.in_Renamed.ReadChar();

                if (c == StreamIn.EndOfStream)
                {
                    this.in_Renamed.UngetChar(c);
                    break;
                }

                map = MAP((char)c);

                if ((map & WHITE) == 0)
                    break;
            }

            /* check for quote marks */

            if (c == '"' || c == '\'')
                delim = c;
            else if (c == '<')
            {
                start = this.lexsize;
                addCharToLexer(c);
                pdelim.value_Renamed = parseServerInstruction();
                len = this.lexsize - start;
                this.lexsize = start;
                return (len > 0 ? getString(this.lexbuf, start, len) : null);
            }
            else
                this.in_Renamed.UngetChar(c);

            /*
            and read the value string
            check for quote mark if needed
            */

            quotewarning = 0;
            start = this.lexsize;
            c = '\x0000';

            for (; ; )
            {
                lastc = c; /* track last character */
                c = this.in_Renamed.ReadChar();

                if (c == StreamIn.EndOfStream)
                {
                    Report.attrError(this, this.token, null, Report.UNEXPECTED_END_OF_FILE);
                    this.in_Renamed.UngetChar(c);
                    break;
                }

                if (delim == (char)0)
                {
                    if (c == '>')
                    {
                        this.in_Renamed.UngetChar(c);
                        break;
                    }

                    if (c == '"' || c == '\'')
                    {
                        Report.attrError(this, this.token, null, Report.UNEXPECTED_QUOTEMARK);
                        break;
                    }

                    if (c == '<')
                    {
                        /* this.in.ungetChar(c); */
                        Report.attrError(this, this.token, null, Report.UNEXPECTED_GT);
                        /* break; */
                    }

                    /*
                    For cases like <br clear=all/> need to avoid treating /> as
                    part of the attribute value, however care is needed to avoid
                    so treating <a href=http://www.acme.com/> in this way, which
                    would map the <a> tag to <a href="http://www.acme.com"/>
                    */
                    if (c == '/')
                    {
                        /* peek ahead in case of /> */
                        c = this.in_Renamed.ReadChar();

                        if (c == '>' && !AttributeTable.DefaultAttributeTable.IsUrl(name))
                        {
                            isempty.value_Renamed = true;
                            this.in_Renamed.UngetChar(c);
                            break;
                        }

                        /* unget peeked char */
                        this.in_Renamed.UngetChar(c);
                        c = '/';
                    }
                }
                /* delim is '\'' or '"' */
                else
                {
                    if (c == delim)
                        break;

                    /* treat CRLF, CR and LF as single line break */

                    if (c == '\r')
                    {
                        c = this.in_Renamed.ReadChar();
                        if (c != '\n')
                            this.in_Renamed.UngetChar(c);

                        c = '\n';
                    }

                    if (c == '\n' || c == '<' || c == '>')
                        ++quotewarning;

                    if (c == '>')
                        seen_gt = true;
                }

                if (c == '&')
                {
                    addCharToLexer(c);
                    parseEntity((short)0);
                    continue;
                }

                /*
                kludge for JavaScript attribute values
                with line continuations in string literals
                */
                if (c == '\\')
                {
                    c = this.in_Renamed.ReadChar();

                    if (c != '\n')
                    {
                        this.in_Renamed.UngetChar(c);
                        c = '\\';
                    }
                }

                map = MAP((char)c);

                if ((map & WHITE) != 0)
                {
                    if (delim == (char)0)
                        break;

                    if (munge)
                    {
                        c = ' ';

                        if (lastc == ' ')
                            continue;
                    }
                }
                else if (foldCase && (map & UPPERCASE) != 0)
                    c += (int)('a' - 'A');

                addCharToLexer(c);
            }

            if (quotewarning > 10 && seen_gt && munge)
            {
                /*
                there is almost certainly a missing trailling quote mark
                as we have see too many newlines, < or > characters.
				
                an exception is made for Javascript attributes and the
                javascript URL scheme which may legitimately include < and >
                */
                if (!AttributeTable.DefaultAttributeTable.IsScript(name) && !(AttributeTable.DefaultAttributeTable.IsUrl(name) && (getString(this.lexbuf, start, 11)).Equals("javascript:")))
                    Report.error(this, null, null, Report.SUSPECTED_MISSING_QUOTE);
            }

            len = this.lexsize - start;
            this.lexsize = start;

            if (len > 0 || delim != 0)
                value_Renamed = getString(this.lexbuf, start, len);
            else
                value_Renamed = null;

            /* note delimiter if given */
            if (delim != 0)
                pdelim.value_Renamed = delim;
            else
                pdelim.value_Renamed = (int)'"';

            return value_Renamed;
        }

        /// <summary>
        /// Checks validity.
        /// </summary>
        /// <remarks>Attr must be non-null</remarks>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static bool isValidAttrName(System.String attr)
        {
            short map;
            char c;
            int i;

            /* first character should be a letter */
            c = attr[0];
            map = MAP(c);

            if (!((map & LETTER) != 0))
                return false;

            /* remaining characters should be namechars */
            for (i = 1; i < attr.Length; i++)
            {
                c = attr[i];
                map = MAP(c);

                if ((map & NAMECHAR) != 0)
                    continue;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Swallows closing '>'
        /// </summary>
        /// <param name="isempty"></param>
        /// <returns></returns>
        public virtual AttVal ParseAttrs(MutableBoolean isempty)
        {
            AttVal av, list;
            System.String attribute, value_Renamed;
            MutableInteger delim = new MutableInteger();
            MutableObject asp = new MutableObject();
            MutableObject php = new MutableObject();

            list = null;

            for (; !EndOfInput; )
            {
                attribute = parseAttribute(isempty, asp, php);

                if (attribute == null)
                {
                    /* check if attributes are created by ASP markup */
                    if (asp.Object != null)
                    {
                        av = new AttVal(list, null, (Node)asp.Object, null, '\x0000', null, null);
                        list = av;
                        continue;
                    }

                    /* check if attributes are created by PHP markup */
                    if (php.Object != null)
                    {
                        av = new AttVal(list, null, null, (Node)php.Object, '\x0000', null, null);
                        list = av;
                        continue;
                    }

                    break;
                }

                value_Renamed = ParseValue(attribute, false, isempty, delim);

                if (attribute != null && isValidAttrName(attribute))
                {
                    av = new AttVal(list, null, null, null, delim.value_Renamed, attribute, value_Renamed);
                    av.dict = AttributeTable.DefaultAttributeTable.findAttribute(av);
                    list = av;
                }
                else
                {
                    av = new AttVal(null, null, null, null, 0, attribute, value_Renamed);
                    Report.attrError(this, this.token, value_Renamed, Report.BAD_ATTRIBUTE_VALUE);
                }
            }

            return list;
        }

        /// <summary>
        /// Push a copy of an inline node onto stack but don't push if implicit or OBJECT or APPLET (implicit tags are ones generated from the istack).
        /// </summary>
        /// <remarks>
        /// One issue arises with pushing inlines when the tag is already pushed. For instance:
        /// <code>
        /// &lt;p>&lt;em>text
        /// &lt;p>&lt;em>more text
        /// </code>
        /// Shouldn't be mapped to
        /// <code>
        /// &lt;p>&lt;em>text&lt;/em>&lt;/p>
        /// &lt;p>&lt;em>&lt;em>more text&lt;/em>&lt;/em>
        /// </code>
        /// </remarks>
        /// <param name="node">Node to push</param>
        public virtual void PushInline(Node node)
        {
            IStack is_Renamed;

            if (node.implicit_Renamed)
                return;

            if (node.tag == null)
                return;

            if ((node.tag.model & Dict.CM_INLINE) == 0)
                return;

            if ((node.tag.model & Dict.CM_OBJECT) != 0)
                return;

            if (node.tag != configuration.tt.tagFont && isPushed(node))
                return;

            // make sure there is enough space for the stack
            is_Renamed = new IStack();
            is_Renamed.tag = node.tag;
            is_Renamed.element = node.element;
            if (node.attributes != null)
                is_Renamed.attributes = cloneAttributes(node.attributes);
            this.istack.Add(is_Renamed);
        }

        /// <summary>
        /// Pop the inline stack.
        /// </summary>
        /// <param name="node"></param>
        public virtual void popInline(Node node)
        {
            AttVal av;
            IStack is_Renamed;

            if (node != null)
            {

                if (node.tag == null)
                    return;

                if ((node.tag.model & Dict.CM_INLINE) == 0)
                    return;

                if ((node.tag.model & Dict.CM_OBJECT) != 0)
                    return;

                // if node is </a> then pop until we find an <a>
                if (node.tag == configuration.tt.tagA)
                {

                    while (this.istack.Count > 0)
                    {
                        is_Renamed = (IStack)SupportClass.StackSupport.Pop(this.istack);
                        if (is_Renamed.tag == configuration.tt.tagA)
                        {
                            break;
                        }
                    }

                    if (this.insert >= this.istack.Count)
                        this.insert = -1;
                    return;
                }
            }

            if (this.istack.Count > 0)
            {
                is_Renamed = (IStack)SupportClass.StackSupport.Pop(this.istack);
                if (this.insert >= this.istack.Count)
                    this.insert = -1;
            }
        }

        public virtual bool isPushed(Node node)
        {
            int i;
            IStack is_Renamed;

            for (i = this.istack.Count - 1; i >= 0; --i)
            {
                is_Renamed = (IStack)this.istack[i];
                if (is_Renamed.tag == node.tag)
                    return true;
            }

            return false;
        }

        /*
        This has the effect of inserting "missing" inline
        elements around the contents of blocklevel elements
        such as P, TD, TH, DIV, PRE etc. This procedure is
        called at the start of ParseBlock. when the inline
        stack is not empty, as will be the case in:
		
        <i><h1>italic heading</h1></i>
		
        which is then treated as equivalent to
		
        <h1><i>italic heading</i></h1>
		
        This is implemented by setting the lexer into a mode
        where it gets tokens from the inline stack rather than
        from the input stream.
        */
        public virtual int inlineDup(Node node)
        {
            int n;

            n = this.istack.Count - this.istackbase;
            if (n > 0)
            {
                this.insert = this.istackbase;
                this.inode = node;
            }

            return n;
        }

        public virtual Node InsertedToken()
        {
            Node node;
            IStack is_Renamed;
            int n;

            // this will only be null if inode != null
            if (this.insert == -1)
            {
                node = this.inode;
                this.inode = null;
                return node;
            }

            // is this is the "latest" node then update
            // the position, otherwise use current values

            if (this.inode == null)
            {
                this.lines = this.in_Renamed.curline;
                this.columns = this.in_Renamed.curcol;
            }

            node = newNode(Node.StartTag, this.lexbuf, this.txtstart, this.txtend); // GLP:  Bugfix 126261.  Remove when this change
            //       is fixed in istack.c in the original Tidy
            node.implicit_Renamed = true;
            is_Renamed = (IStack)this.istack[this.insert];
            node.element = is_Renamed.element;
            node.tag = is_Renamed.tag;
            if (is_Renamed.attributes != null)
                node.attributes = cloneAttributes(is_Renamed.attributes);

            // advance lexer to next item on the stack
            n = this.insert;

            // and recover state if we have reached the end
            if (++n < this.istack.Count)
            {
                this.insert = n;
            }
            else
            {
                this.insert = -1;
            }

            return node;
        }

        /* AQ: Try this for speed optimization */
        public static int wstrcasecmp(System.String s1, System.String s2)
        {
            return (s1.ToUpper().Equals(s2.ToUpper()) ? 0 : 1);
        }

        public static int wstrcaselexcmp(System.String s1, System.String s2)
        {
            char c;
            int i = 0;

            while (i < s1.Length && i < s2.Length)
            {
                c = s1[i];
                if (toLower(c) != toLower(s2[i]))
                {
                    break;
                }
                i += 1;
            }
            if (i == s1.Length && i == s2.Length)
            {
                return 0;
            }
            else if (i == s1.Length)
            {
                return -1;
            }
            else if (i == s2.Length)
            {
                return 1;
            }
            else
            {
                return (s1[i] > s2[i] ? 1 : -1);
            }
        }

        public static bool wsubstr(System.String s1, System.String s2)
        {
            int i;
            int len1 = s1.Length;
            int len2 = s2.Length;

            for (i = 0; i <= len1 - len2; ++i)
            {
                if (s2.ToUpper().Equals(s1.Substring(i).ToUpper()))
                    return true;
            }

            return false;
        }

        public virtual bool canPrune(Node element)
        {
            if (element.type == Node.TextNode)
                return true;

            if (element.content != null)
                return false;

            if (element.tag == configuration.tt.tagA && element.attributes != null)
                return false;

            if (element.tag == configuration.tt.tagP && !this.configuration.DropEmptyParas)
                return false;

            if (element.tag == null)
                return false;

            if ((element.tag.model & Dict.CM_ROW) != 0)
                return false;

            if (element.tag == configuration.tt.tagApplet)
                return false;

            if (element.tag == configuration.tt.tagObject)
                return false;

            if (element.attributes != null && (element.getAttrByName("id") != null || element.getAttrByName("name") != null))
                return false;

            return true;
        }

        /* duplicate name attribute as an id */
        public virtual void fixId(Node node)
        {
            AttVal name = node.getAttrByName("name");
            AttVal id = node.getAttrByName("id");

            if (name != null)
            {
                if (id != null)
                {
                    if (!id.value_Renamed.Equals(name.value_Renamed))
                        Report.attrError(this, node, "name", Report.ID_NAME_MISMATCH);
                }
                else if (this.configuration.XmlOut)
                    node.addAttribute("id", name.value_Renamed);
            }
        }

        /*
        defer duplicates when entering a table or other
        element where the inlines shouldn't be duplicated
        */
        public virtual void deferDup()
        {
            this.insert = -1;
            this.inode = null;
        }

        /* Private methods and fields */

        /* lexer char types */
        private const short DIGIT = 1;
        private const short LETTER = 2;
        private const short NAMECHAR = 4;
        private const short WHITE = 8;
        private const short NEWLINE = 16;
        private const short LOWERCASE = 32;
        private const short UPPERCASE = 64;

        /* lexer GetToken states */

        private const short LEX_CONTENT = 0;
        private const short LEX_GT = 1;
        private const short LEX_ENDTAG = 2;
        private const short LEX_STARTTAG = 3;
        private const short LEX_COMMENT = 4;
        private const short LEX_DOCTYPE = 5;
        private const short LEX_PROCINSTR = 6;
        private const short LEX_ENDCOMMENT = 7;
        private const short LEX_CDATA = 8;
        private const short LEX_SECTION = 9;
        private const short LEX_ASP = 10;
        private const short LEX_JSTE = 11;
        private const short LEX_PHP = 12;

        /* used to classify chars for lexical purposes */
        private static short[] lexmap = new short[128];

        private static void mapStr(System.String str, short code)
        {
            int j;

            for (int i = 0; i < str.Length; i++)
            {
                j = (int)str[i];
                lexmap[j] |= code;
            }
        }

        private static short MAP(char c)
        {
            int ic = (int)c;
            return (short)((ic < 128) ? lexmap[ic] : (short)0);
        }

        private static bool isWhite(char c)
        {
            short m = MAP(c);

            return (m & WHITE) != 0;
        }

        private static bool isDigit(char c)
        {
            short m;

            m = MAP(c);

            return (m & DIGIT) != 0;
        }

        private static bool isLetter(char c)
        {
            short m;

            m = MAP(c);

            return (m & LETTER) != 0;
        }

        private static char toLower(char c)
        {
            short m = MAP(c);

            if ((m & UPPERCASE) != 0)
                c = (char)((int)c + (int)'a' - (int)'A');

            return c;
        }

        private static char toUpper(char c)
        {
            short m = MAP(c);

            if ((m & LOWERCASE) != 0)
                c = (char)((int)c + (int)'A' - (int)'a');

            return c;
        }

        public static char foldCase(char c, bool tocaps, bool xmlTags)
        {
            short m;

            if (!xmlTags)
            {
                m = MAP(c);

                if (tocaps)
                {
                    if ((m & LOWERCASE) != 0)
                        c = (char)((int)c + (int)'A' - (int)'a');
                }
                /* force to lower case */
                else
                {
                    if ((m & UPPERCASE) != 0)
                        c = (char)((int)c + (int)'a' - (int)'A');
                }
            }

            return c;
        }


        private class W3CVersionInfo
        {
            internal System.String name;
            internal System.String voyagerName;
            internal System.String profile;
            internal short code;

            public W3CVersionInfo(System.String name, System.String voyagerName, System.String profile, short code)
            {
                this.name = name;
                this.voyagerName = voyagerName;
                this.profile = profile;
                this.code = code;
            }
        }

        /* the 3 URIs  for the XHTML 1.0 DTDs */
        private const System.String voyager_loose = "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd";
        private const System.String voyager_strict = "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd";
        private const System.String voyager_frameset = "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd";

        private const System.String XHTML_NAMESPACE = "http://www.w3.org/1999/xhtml";

        //UPGRADE_NOTE: The initialization of  'W3CVersion' was moved to static method 'Comzept.Genesis.Tidy.Lexer'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
        private static Lexer.W3CVersionInfo[] W3CVersion;

        static Lexer()
        {
            {
                mapStr("\r\n\f", (short)(NEWLINE | WHITE));
                mapStr(" \t", WHITE);
                mapStr("-.:_", NAMECHAR);
                mapStr("0123456789", (short)(DIGIT | NAMECHAR));
                mapStr("abcdefghijklmnopqrstuvwxyz", (short)(LOWERCASE | LETTER | NAMECHAR));
                mapStr("ABCDEFGHIJKLMNOPQRSTUVWXYZ", (short)(UPPERCASE | LETTER | NAMECHAR));
            }
            W3CVersion = new Lexer.W3CVersionInfo[] { new W3CVersionInfo("HTML 4.01", "XHTML 1.0 Strict", voyager_strict, Dict.VERS_HTML40_STRICT), new W3CVersionInfo("HTML 4.01 Transitional", "XHTML 1.0 Transitional", voyager_loose, Dict.VERS_HTML40_LOOSE), new W3CVersionInfo("HTML 4.01 Frameset", "XHTML 1.0 Frameset", voyager_frameset, Dict.VERS_FRAMES), new W3CVersionInfo("HTML 4.0", "XHTML 1.0 Strict", voyager_strict, Dict.VERS_HTML40_STRICT), new W3CVersionInfo("HTML 4.0 Transitional", "XHTML 1.0 Transitional", voyager_loose, Dict.VERS_HTML40_LOOSE), new W3CVersionInfo("HTML 4.0 Frameset", "XHTML 1.0 Frameset", voyager_frameset, Dict.VERS_FRAMES), new W3CVersionInfo("HTML 3.2", "XHTML 1.0 Transitional", voyager_loose, Dict.VERS_HTML32), new W3CVersionInfo("HTML 2.0", "XHTML 1.0 Strict", voyager_strict, Dict.VERS_HTML20) };
        }
    }
}