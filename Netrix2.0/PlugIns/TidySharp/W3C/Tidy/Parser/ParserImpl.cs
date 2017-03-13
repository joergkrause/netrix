/*
* @(#)ParserImpl.java   1.11 2000/08/16
*
*/
using System;
namespace Comzept.Genesis.Tidy
{
	
	/// <summary> 
	/// HTML Parser implementation</summary>
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
	/// </version>
	public class ParserImpl
	{
		
		//private static int SeenBodyEndTag;  /* AQ: moved into lexer structure */
		
		internal static void parseTag(Lexer lexer, Node node, short mode)
		{
			// Local fix by GLP 2000-12-21.  Need to reset insertspace if this 
			// is both a non-inline and empty tag (base, link, meta, isindex, hr, area).
			// Remove this code once the fix is made in Tidy.
			
			/******  (Original code follows)
			if ((node.tag.model & Dict.CM_EMPTY) != 0)
			{
			lexer.waswhite = false;
			return;
			}
			else if (!((node.tag.model & Dict.CM_INLINE) != 0))
			lexer.insertspace = false;
			*******/
			
			if (!((node.tag.model & Dict.CM_INLINE) != 0))
				lexer.insertspace = false;
			
			if ((node.tag.model & Dict.CM_EMPTY) != 0)
			{
				lexer.waswhite = false;
				return ;
			}
			
			if (node.tag.parser == null || node.type == Node.StartEndTag)
				return ;
			
			node.tag.parser.Parse(lexer, node, mode);
		}
		
		internal static void moveToHead(Lexer lexer, Node element, Node node)
		{
			Node head;
			TagTable tt = lexer.configuration.tt;
			
			
			if (node.type == Node.StartTag || node.type == Node.StartEndTag)
			{
				Report.Warning(lexer, element, node, Report.TAG_NOT_ALLOWED_IN);
				
				while (element.tag != tt.tagHtml)
					element = element.parent;
				
				for (head = element.content; head != null; head = head.next)
				{
					if (head.tag == tt.tagHead)
					{
						Node.insertNodeAtEnd(head, node);
						break;
					}
				}
				
				if (node.tag.parser != null)
					parseTag(lexer, node, Lexer.IgnoreWhitespace);
			}
			else
			{
				Report.Warning(lexer, element, node, Report.DISCARDING_UNEXPECTED);
			}
		}
				
		public static Parser getParseHTML()
		{
			return _parseHTML;
		}
		
		public static Parser getParseHead()
		{
			return _parseHead;
		}
		
		public static Parser getParseTitle()
		{
			return _parseTitle;
		}
		
		public static Parser getParseScript()
		{
			return _parseScript;
		}
		
		public static Parser getParseBody()
		{
			return _parseBody;
		}
		
		public static Parser getParseFrameSet()
		{
			return _parseFrameSet;
		}
		
		public static Parser getParseInline()
		{
			return _parseInline;
		}
		
		public static Parser ParseList
		{
			get { return _parseList; }
		}
		
		public static Parser ParseDefList
		{
            get { return _parseDefList; }
		}
		
		public static Parser ParsePre
		{
            get { return _parsePre; }
		}
		
		public static Parser getParseBlock()
		{
			return _parseBlock;
		}
		
		public static Parser getParseTableTag()
		{
			return _parseTableTag;
		}
		
		public static Parser getParseColGroup()
		{
			return _parseColGroup;
		}
		
		public static Parser getParseRowGroup()
		{
			return _parseRowGroup;
		}
		
		public static Parser getParseRow()
		{
			return _parseRow;
		}
		
		public static Parser getParseNoFrames()
		{
			return _parseNoFrames;
		}
		
		public static Parser getParseSelect()
		{
			return _parseSelect;
		}
		
		public static Parser getParseText()
		{
			return _parseText;
		}
		
		public static Parser ParseOptGroup
		{
			get { return _parseOptGroup; }
		}
		
		
		private static Parser _parseHTML = new ParseHTML();
		private static Parser _parseHead = new ParseHead();
		private static Parser _parseTitle = new ParseTitle();
		private static Parser _parseScript = new ParseScript();
		private static Parser _parseBody = new ParseBody();
		private static Parser _parseFrameSet = new ParseFrameSet();
		private static Parser _parseInline = new ParseInline();
		private static Parser _parseList = new ParseList();
		private static Parser _parseDefList = new ParseDefList();
		private static Parser _parsePre = new ParsePre();
		private static Parser _parseBlock = new ParseBlock();
		private static Parser _parseTableTag = new ParseTableTag();
		private static Parser _parseColGroup = new ParseColGroup();
		private static Parser _parseRowGroup = new ParseRowGroup();
		private static Parser _parseRow = new ParseRow();
		private static Parser _parseNoFrames = new ParseNoFrames();
		private static Parser _parseSelect = new ParseSelect();
		private static Parser _parseText = new ParseText();
		private static Parser _parseOptGroup = new ParseOptGroup();
		
		/*
		HTML is the top level element
		*/
		public static Node parseDocument(Lexer lexer)
		{
			Node node, document, html;
			Node doctype = null;
			TagTable tt = lexer.configuration.tt;
			
			document = lexer.newNode();
			document.type = Node.RootNode;
			
			while (true)
			{
				node = lexer.GetToken(Lexer.IgnoreWhitespace);
				if (node == null)
					break;
				
				/* deal with comments etc. */
				if (Node.insertMisc(document, node))
					continue;
				
				if (node.type == Node.DocTypeTag)
				{
					if (doctype == null)
					{
						Node.insertNodeAtEnd(document, node);
						doctype = node;
					}
					else
						Report.Warning(lexer, document, node, Report.DISCARDING_UNEXPECTED);
					continue;
				}
				
				if (node.type == Node.EndTag)
				{
					Report.Warning(lexer, document, node, Report.DISCARDING_UNEXPECTED); //TODO?
					continue;
				}
				
				if (node.type != Node.StartTag || node.tag != tt.tagHtml)
				{
					lexer.ungetToken();
					html = lexer.inferredTag("html");
				}
				else
					html = node;
				
				Node.insertNodeAtEnd(document, html);
				getParseHTML().Parse(lexer, html, (short) 0); // TODO?
				break;
			}
			
			return document;
		}
		
		/// <summary>  Indicates whether or not whitespace should be preserved for this element.
		/// If an <code>xml:space</code> attribute is found, then if the attribute value is
		/// <code>preserve</code>, returns <code>true</code>.  For any other value, returns
		/// <code>false</code>.  If an <code>xml:space</code> attribute was <em>not</em>
		/// found, then the following element names result in a return value of <code>true:
		/// pre, script, style,</code> and <code>xsl:text</code>.  Finally, if a
		/// <code>TagTable</code> was passed in and the element appears as the "pre" element
		/// in the <code>TagTable</code>, then <code>true</code> will be returned.
		/// Otherwise, <code>false</code> is returned.
		/// </summary>
		/// <param name="element">The <code>Node</code> to test to see if whitespace should be
		/// preserved.
		/// </param>
		/// <param name="tt">The <code>TagTable</code> to test for the <code>getNodePre()</code>
		/// function.  This may be <code>null</code>, in which case this test
		/// is bypassed.
		/// </param>
		/// <returns> <code>true</code> or <code>false</code>, as explained above.
		/// </returns>
		
		public static bool XMLPreserveWhiteSpace(Node element, TagTable tt)
		{
			AttVal attribute;
			
			/* search attributes for xml:space */
			for (attribute = element.attributes; attribute != null; attribute = attribute.next)
			{
				if (attribute.attribute.Equals("xml:space"))
				{
					if (attribute.value_Renamed.Equals("preserve"))
						return true;
					
					return false;
				}
			}
			
			/* kludge for html docs without explicit xml:space attribute */
			if (Lexer.wstrcasecmp(element.element, "pre") == 0 || Lexer.wstrcasecmp(element.element, "script") == 0 || Lexer.wstrcasecmp(element.element, "style") == 0)
				return true;
			
			if ((tt != null) && (tt.findParser(element) == ParsePre))
				return true;
			
			/* kludge for XSL docs */
			if (Lexer.wstrcasecmp(element.element, "xsl:text") == 0)
				return true;
			
			return false;
		}
		
		/*
		XML documents
		*/
		public static void  ParseXMLElement(Lexer lexer, Node element, short mode)
		{
			Node node;
			
			/* Jeff Young's kludge for XSL docs */
			
			if (Lexer.wstrcasecmp(element.element, "xsl:text") == 0)
				return ;
			
			/* if node is pre or has xml:space="preserve" then do so */
			
			if (XMLPreserveWhiteSpace(element, lexer.configuration.tt))
				mode = Lexer.Preformatted;
			
			while (true)
			{
				node = lexer.GetToken(mode);
				if (node == null)
					break;
				if (node.type == Node.EndTag && node.element.Equals(element.element))
				{
					element.closed = true;
					break;
				}
				
				/* discard unexpected end tags */
				if (node.type == Node.EndTag)
				{
					Report.error(lexer, element, node, Report.UNEXPECTED_ENDTAG);
					continue;
				}
				
				/* parse content on seeing start tag */
				if (node.type == Node.StartTag)
					ParseXMLElement(lexer, node, mode);
				
				Node.insertNodeAtEnd(element, node);
			}
			
			/*
			if first child is text then trim initial space and
			delete text node if it is empty.
			*/
			
			node = element.content;
			
			if (node != null && node.type == Node.TextNode && mode != Lexer.Preformatted)
			{
				if (node.textarray[node.start] == (sbyte) ' ')
				{
					node.start++;
					
					if (node.start >= node.end)
						Node.discardElement(node);
				}
			}
			
			/*
			if last child is text then trim final space and
			delete the text node if it is empty
			*/
			
			node = element.last;
			
			if (node != null && node.type == Node.TextNode && mode != Lexer.Preformatted)
			{
				if (node.textarray[node.end - 1] == (sbyte) ' ')
				{
					node.end--;
					
					if (node.start >= node.end)
						Node.discardElement(node);
				}
			}
		}
		
		public static Node ParseXMLDocument(Lexer lexer)
		{
			Node node, document, doctype;
			
			document = lexer.newNode();
			document.type = Node.RootNode;
			doctype = null;
			lexer.configuration.XmlTags = true;
			
			while (true)
			{
				node = lexer.GetToken(Lexer.IgnoreWhitespace);
				if (node == null)
					break;
				/* discard unexpected end tags */
				if (node.type == Node.EndTag)
				{
					Report.Warning(lexer, null, node, Report.UNEXPECTED_ENDTAG);
					continue;
				}
				
				/* deal with comments etc. */
				if (Node.insertMisc(document, node))
					continue;
				
				if (node.type == Node.DocTypeTag)
				{
					if (doctype == null)
					{
						Node.insertNodeAtEnd(document, node);
						doctype = node;
					}
					else
						Report.Warning(lexer, document, node, Report.DISCARDING_UNEXPECTED); // TODO
					continue;
				}
				
				/* if start tag then parse element's content */
				if (node.type == Node.StartTag)
				{
					Node.insertNodeAtEnd(document, node);
					ParseXMLElement(lexer, node, Lexer.IgnoreWhitespace);
				}
			}
			
			if (false)
			{
				//#if 0
				/* discard the document type */
				node = document.findDocType();
				
				if (node != null)
					Node.discardElement(node);
			} // #endif
			
			if (doctype != null && !lexer.checkDocTypeKeyWords(doctype))
				Report.Warning(lexer, doctype, null, Report.DTYPE_NOT_UPPER_CASE);
			
			/* ensure presence of initial <?XML version="1.0"?> */
			if (lexer.configuration.XmlPi)
				lexer.fixXMLPI(document);
			
			return document;
		}
		
		public static bool IsJavaScript(Node node)
		{
			bool result = false;
			AttVal attr;
			
			if (node.attributes == null)
				return true;
			
			for (attr = node.attributes; attr != null; attr = attr.next)
			{
				if ((Lexer.wstrcasecmp(attr.attribute, "language") == 0 || Lexer.wstrcasecmp(attr.attribute, "type") == 0) && Lexer.wsubstr(attr.value_Renamed, "javascript"))
					result = true;
			}
			
			return result;
		}
	}
}