/*
* @(#)Node.java   1.11 2000/08/16
*
*/
using System;
using Comzept.Genesis.Tidy.Xml.Dom;
namespace Comzept.Genesis.Tidy
{
	
	/// <summary> 
	/// Node
	/// 
	/// (c) 1998-2000 (W3C) MIT, INRIA, Keio University
	/// See Tidy.java for the copyright notice.
	/// Derived from <a href="http://www.w3.org/People/Raggett/tidy">
	/// HTML Tidy Release 4 Aug 2000</a>
	/// 
	/// </summary>
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
	
	/*
	Used for elements and text nodes
	element name is null for text nodes
	start and end are offsets into lexbuf
	which contains the textual content of
	all elements in the parse tree.
	
	parent and content allow traversal
	of the parse tree in any direction.
	attributes are represented as a linked
	list of AttVal nodes which hold the
	strings for attribute/value pairs.*/
	
	public class Node : System.ICloneable
	{
		virtual public bool Element
		{
			get
			{
				return (this.type == StartTag || this.type == StartEndTag?true:false);
			}
			
		}
		virtual protected internal Comzept.Genesis.Tidy.Dom.Node Adapter
		{
			get
			{
				if (adapter == null)
				{
					switch (this.type)
					{
						
						case RootNode: 
							adapter = new DOMDocumentImpl(this);
							break;
						
						case StartTag: 
						case StartEndTag: 
							adapter = new DOMElementImpl(this);
							break;
						
						case DocTypeTag: 
							adapter = new DOMDocumentTypeImpl(this);
							break;
						
						case CommentTag: 
							adapter = new DOMCommentImpl(this);
							break;
						
						case TextNode: 
							adapter = new DOMTextImpl(this);
							break;
						
						case CDATATag: 
							adapter = new DOMCDATASectionImpl(this);
							break;
						
						case ProcInsTag: 
							adapter = new DOMProcessingInstructionImpl(this);
							break;
						
						default: 
							adapter = new DOMNodeImpl(this);
							break;
						
					}
				}
				return adapter;
			}
			
		}
		virtual protected internal short Type
		{
			set
			{
				this.type = value;
			}
			
			/* --------------------- END DOM ------------------------ */
			
		}
		/// <summary>
		/// Node type
		/// </summary>
		public const short RootNode = 0;
        /// <summary>
        /// Node type
        /// </summary>
        public const short DocTypeTag = 1;
        /// <summary>
        /// Node type
        /// </summary>
        public const short CommentTag = 2;
        /// <summary>
        /// Node type
        /// </summary>
        public const short ProcInsTag = 3;
        /// <summary>
        /// Node type
        /// </summary>
        public const short TextNode = 4;
        /// <summary>
        /// Node type
        /// </summary>
        public const short StartTag = 5;
        /// <summary>
        /// Node type
        /// </summary>
        public const short EndTag = 6;
        /// <summary>
        /// Node type
        /// </summary>
        public const short StartEndTag = 7;
        /// <summary>
        /// Node type
        /// </summary>
        public const short CDATATag = 8;
        /// <summary>
        /// Node type
        /// </summary>
        public const short SectionTag = 9;
        /// <summary>
        /// Node type
        /// </summary>
        public const short AspTag = 10;
        /// <summary>
        /// Node type
        /// </summary>
        public const short JsteTag = 11;
        /// <summary>
        /// Node type
        /// </summary>
        public const short PhpTag = 12;

        /// <summary>
        /// Node in list
        /// </summary>		
		protected internal Node parent;
        /// <summary>
        /// Node in list
        /// </summary>		
        protected internal Node prev;
        /// <summary>
        /// Node in list
        /// </summary>		
        protected internal Node next;
        /// <summary>
        /// Node in list
        /// </summary>		
        protected internal Node last;
        /// <summary>
        /// start of span onto text array
        /// </summary>		
        protected internal int start; /* start of span onto text array */
        /// <summary>
        /// end of span onto text array
        /// </summary>		
        protected internal int end; /* end of span onto text array */
        /// <summary>
        /// the text array
        /// </summary>
		protected internal sbyte[] textarray; /* the text array */
        /// <summary>
        /// TextNode, StartTag, EndTag etc.
        /// </summary>
		protected internal short type; /* TextNode, StartTag, EndTag etc. */
        /// <summary>
        /// true if closed by explicit end tag
        /// </summary>
		protected internal bool closed; /* true if closed by explicit end tag */
        /// <summary>
        /// true if inferred
        /// </summary>
		protected internal bool implicit_Renamed; /* true if inferred */
        /// <summary>
        ///  true if followed by a line break
        /// </summary>
		protected internal bool linebreak; /* true if followed by a line break */
        /// <summary>
        /// old tag when it was changed
        /// </summary>
		protected internal Dict was; /* old tag when it was changed */
        /// <summary>
        /// tag's dictionary definition
        /// </summary>
		protected internal Dict tag; /* tag's dictionary definition */
        /// <summary>
        /// name (null for text nodes)
        /// </summary>
		protected internal System.String element; /* name (null for text nodes) */
		protected internal AttVal attributes;
		protected internal Node content;
		
        /// <summary>
        /// Ctor
        /// </summary>
		public Node():this(TextNode, null, 0, 0)
		{
		}
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="type"></param>
		/// <param name="textarray"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public Node(short type, sbyte[] textarray, int start, int end)
		{
			this.parent = null;
			this.prev = null;
			this.next = null;
			this.last = null;
			this.start = start;
			this.end = end;
			this.textarray = textarray;
			this.type = type;
			this.closed = false;
			this.implicit_Renamed = false;
			this.linebreak = false;
			this.was = null;
			this.tag = null;
			this.element = null;
			this.attributes = null;
			this.content = null;
		}
		
		public Node(short type, sbyte[] textarray, int start, int end, System.String element, TagTable tt)
		{
			this.parent = null;
			this.prev = null;
			this.next = null;
			this.last = null;
			this.start = start;
			this.end = end;
			this.textarray = textarray;
			this.type = type;
			this.closed = false;
			this.implicit_Renamed = false;
			this.linebreak = false;
			this.was = null;
			this.tag = null;
			this.element = element;
			this.attributes = null;
			this.content = null;
			if (type == StartTag || type == StartEndTag || type == EndTag)
				tt.findTag(this);
		}
		
		/* used to clone heading nodes when split by an <HR> */
		public virtual System.Object Clone()
		{
			Node node = new Node();
			
			node.parent = this.parent;
			if (this.textarray != null)
			{
				node.textarray = new sbyte[this.end - this.start];
				node.start = 0;
				node.end = this.end - this.start;
				if (node.end > 0)
					Array.Copy(this.textarray, this.start, node.textarray, node.start, node.end);
			}
			node.type = this.type;
			node.closed = this.closed;
			node.implicit_Renamed = this.implicit_Renamed;
			node.linebreak = this.linebreak;
			node.was = this.was;
			node.tag = this.tag;
			if (this.element != null)
				node.element = this.element;
			if (this.attributes != null)
				node.attributes = (AttVal) this.attributes.Clone();
			return node;
		}
		
		public virtual AttVal getAttrByName(System.String name)
		{
			AttVal attr;
			
			for (attr = this.attributes; attr != null; attr = attr.next)
			{
				if (name != null && attr.attribute != null && attr.attribute.Equals(name))
					break;
			}
			
			return attr;
		}
		
		/* default method for checking an element's attributes */
		public virtual void  checkAttributes(Lexer lexer)
		{
			AttVal attval;
			
			for (attval = this.attributes; attval != null; attval = attval.next)
				attval.checkAttribute(lexer, this);
		}
		
		public virtual void  checkUniqueAttributes(Lexer lexer)
		{
			AttVal attval;
			
			for (attval = this.attributes; attval != null; attval = attval.next)
			{
				if (attval.asp == null && attval.php == null)
					attval.checkUniqueAttribute(lexer, this);
			}
		}
		
		public virtual void  addAttribute(System.String name, System.String value_Renamed)
		{
			AttVal av = new AttVal(null, null, null, null, '"', name, value_Renamed);
			av.dict = AttributeTable.DefaultAttributeTable.findAttribute(av);
			
			if (this.attributes == null)
				this.attributes = av;
			/* append to end of attributes */
			else
			{
				AttVal here = this.attributes;
				
				while (here.next != null)
					here = here.next;
				
				here.next = av;
			}
		}
		
		/* remove attribute from node then free it */
		public virtual void  removeAttribute(AttVal attr)
		{
			AttVal av;
			AttVal prev = null;
			AttVal next;
			
			for (av = this.attributes; av != null; av = next)
			{
				next = av.next;
				
				if (av == attr)
				{
					if (prev != null)
						prev.next = next;
					else
						this.attributes = next;
				}
				else
					prev = av;
			}
		}
		
		/* find doctype element */
		public virtual Node findDocType()
		{
			Node node;
			
			for (node = this.content; node != null && node.type != DocTypeTag; node = node.next)
				;
			
			return node;
		}
		
		public virtual void  discardDocType()
		{
			Node node;
			
			node = findDocType();
			if (node != null)
			{
				if (node.prev != null)
					node.prev.next = node.next;
				else
					node.parent.content = node.next;
				
				if (node.next != null)
					node.next.prev = node.prev;
				
				node.next = null;
			}
		}
		
		/* remove node from markup tree and discard it */
		public static Node discardElement(Node element)
		{
			Node next = null;
			
			if (element != null)
			{
				next = element.next;
				removeNode(element);
			}
			
			return next;
		}
		
		/* insert node into markup tree */
		public static void  insertNodeAtStart(Node element, Node node)
		{
			node.parent = element;
			
			if (element.content == null)
				element.last = node;
			else
				element.content.prev = node; // AQ added 13 Apr 2000
			
			node.next = element.content;
			node.prev = null;
			element.content = node;
		}
		
		/* insert node into markup tree */
		public static void  insertNodeAtEnd(Node element, Node node)
		{
			node.parent = element;
			node.prev = element.last;
			
			if (element.last != null)
				element.last.next = node;
			else
				element.content = node;
			
			element.last = node;
		}
		
		/*
		insert node into markup tree in pace of element
		which is moved to become the child of the node
		*/
		public static void  insertNodeAsParent(Node element, Node node)
		{
			node.content = element;
			node.last = element;
			node.parent = element.parent;
			element.parent = node;
			
			if (node.parent.content == element)
				node.parent.content = node;
			
			if (node.parent.last == element)
				node.parent.last = node;
			
			node.prev = element.prev;
			element.prev = null;
			
			if (node.prev != null)
				node.prev.next = node;
			
			node.next = element.next;
			element.next = null;
			
			if (node.next != null)
				node.next.prev = node;
		}
		
		/* insert node into markup tree before element */
		public static void  insertNodeBeforeElement(Node element, Node node)
		{
			Node parent;
			
			parent = element.parent;
			node.parent = parent;
			node.next = element;
			node.prev = element.prev;
			element.prev = node;
			
			if (node.prev != null)
				node.prev.next = node;
			
			if (parent.content == element)
				parent.content = node;
		}
		
		/* insert node into markup tree after element */
		public static void  insertNodeAfterElement(Node element, Node node)
		{
			Node parent;
			
			parent = element.parent;
			node.parent = parent;
			
			// AQ - 13Jan2000 fix for parent == null
			if (parent != null && parent.last == element)
				parent.last = node;
			else
			{
				node.next = element.next;
				// AQ - 13Jan2000 fix for node.next == null
				if (node.next != null)
					node.next.prev = node;
			}
			
			element.next = node;
			node.prev = element;
		}
		
		public static void  trimEmptyElement(Lexer lexer, Node element)
		{
			TagTable tt = lexer.configuration.tt;
			
			if (lexer.canPrune(element))
			{
				if (element.type != TextNode)
					Report.Warning(lexer, element, null, Report.TRIM_EMPTY_ELEMENT);
				
				discardElement(element);
			}
			else if (element.tag == tt.tagP && element.content == null)
			{
				/* replace <p></p> by <br/><br/> to preserve formatting */
				Node node = lexer.inferredTag("br");
				Node.coerceNode(lexer, element, tt.tagBr);
				Node.insertNodeAfterElement(element, node);
			}
		}
		
		/*
		This maps 
		<em>hello </em><strong>world</strong>
		to
		<em>hello</em> <strong>world</strong>
		
		If last child of element is a text node
		then trim trailing white space character
		moving it to after element's end tag.
		*/
		public static void  trimTrailingSpace(Lexer lexer, Node element, Node last)
		{
			sbyte c;
			TagTable tt = lexer.configuration.tt;
			
			if (last != null && last.type == Node.TextNode && last.end > last.start)
			{
				c = lexer.lexbuf[last.end - 1];
				
				if (c == 160 || c == (sbyte) ' ')
				{
					/* take care with <td>&nbsp;</td> */
					if (element.tag == tt.tagTd || element.tag == tt.tagTh)
					{
						if (last.end > last.start + 1)
							last.end -= 1;
					}
					else
					{
						last.end -= 1;
						
						if (((element.tag.model & Dict.CM_INLINE) != 0) && !((element.tag.model & Dict.CM_FIELD) != 0))
							lexer.insertspace = true;
						
						/* if empty string then delete from parse tree */
						if (last.start == last.end)
							trimEmptyElement(lexer, last);
					}
				}
			}
		}
		
		/*
		This maps 
		<p>hello<em> world</em>
		to
		<p>hello <em>world</em>
		
		Trims initial space, by moving it before the
		start tag, or if this element is the first in
		parent's content, then by discarding the space
		*/
		public static void  trimInitialSpace(Lexer lexer, Node element, Node text)
		{
			Node prev, node;
			
			// GLP: Local fix to Bug 119789. Remove this comment when parser.c is updated.
			//      31-Oct-00. 
			if (text.type == TextNode && text.textarray[text.start] == (sbyte) ' ' && (text.start < text.end))
			{
				if (((element.tag.model & Dict.CM_INLINE) != 0) && !((element.tag.model & Dict.CM_FIELD) != 0) && element.parent.content != element)
				{
					prev = element.prev;
					
					if (prev != null && prev.type == TextNode)
					{
						if (prev.textarray[prev.end - 1] != (sbyte) ' ')
							prev.textarray[prev.end++] = (sbyte) ' ';
						
						++element.start;
					}
					/* create new node */
					else
					{
						node = lexer.newNode();
						// Local fix for bug 228486 (GLP).  This handles the case
						// where we need to create a preceeding text node but there are
						// no "slots" in textarray that we can steal from the current
						// element.  Therefore, we create a new textarray containing
						// just the blank.  When Tidy is fixed, this should be removed.
						if (element.start >= element.end)
						{
							node.start = 0;
							node.end = 1;
							node.textarray = new sbyte[1];
						}
						else
						{
							node.start = element.start++;
							node.end = element.start;
							node.textarray = element.textarray;
						}
						node.textarray[node.start] = (sbyte) ' ';
						node.prev = prev;
						if (prev != null)
							prev.next = node;
						node.next = element;
						element.prev = node;
						node.parent = element.parent;
					}
				}
				
				/* discard the space  in current node */
				++text.start;
			}
		}
		
		/* 
		Move initial and trailing space out.
		This routine maps:
		
		hello<em> world</em>
		to
		hello <em>world</em>
		and
		<em>hello </em><strong>world</strong>
		to
		<em>hello</em> <strong>world</strong>
		*/
		public static void  trimSpaces(Lexer lexer, Node element)
		{
			Node text = element.content;
			TagTable tt = lexer.configuration.tt;
			
			if (text != null && text.type == Node.TextNode && element.tag != tt.tagPre)
				trimInitialSpace(lexer, element, text);
			
			text = element.last;
			
			if (text != null && text.type == Node.TextNode)
				trimTrailingSpace(lexer, element, text);
		}
		
		public virtual bool isDescendantOf(Dict tag)
		{
			Node parent;
			
			for (parent = this.parent; parent != null; parent = parent.parent)
			{
				if (parent.tag == tag)
					return true;
			}
			
			return false;
		}
		
		/*
		the doctype has been found after other tags,
		and needs moving to before the html element
		*/
		public static void  insertDocType(Lexer lexer, Node element, Node doctype)
		{
			TagTable tt = lexer.configuration.tt;
			
			Report.Warning(lexer, element, doctype, Report.DOCTYPE_AFTER_TAGS);
			
			while (element.tag != tt.tagHtml)
				element = element.parent;
			
			insertNodeBeforeElement(element, doctype);
		}
		
		public virtual Node findBody(TagTable tt)
		{
			Node node;
			
			node = this.content;
			
			while (node != null && node.tag != tt.tagHtml)
				node = node.next;
			
			if (node == null)
				return null;
			
			node = node.content;
			
			while (node != null && node.tag != tt.tagBody)
				node = node.next;
			
			return node;
		}
		
		/*
		unexpected content in table row is moved to just before
		the table in accordance with Netscape and IE. This code
		assumes that node hasn't been inserted into the row.
		*/
		public static void  moveBeforeTable(Node row, Node node, TagTable tt)
		{
			Node table;
			
			/* first find the table element */
			for (table = row.parent; table != null; table = table.parent)
			{
				if (table.tag == tt.tagTable)
				{
					if (table.parent.content == table)
						table.parent.content = node;
					
					node.prev = table.prev;
					node.next = table;
					table.prev = node;
					node.parent = table.parent;
					
					if (node.prev != null)
						node.prev.next = node;
					
					break;
				}
			}
		}
		
		/*
		if a table row is empty then insert an empty cell
		this practice is consistent with browser behavior
		and avoids potential problems with row spanning cells
		*/
		public static void  fixEmptyRow(Lexer lexer, Node row)
		{
			Node cell;
			
			if (row.content == null)
			{
				cell = lexer.inferredTag("td");
				insertNodeAtEnd(row, cell);
				Report.Warning(lexer, row, cell, Report.MISSING_STARTTAG);
			}
		}
		
		public static void  coerceNode(Lexer lexer, Node node, Dict tag)
		{
			Node tmp = lexer.inferredTag(tag.name);
			Report.Warning(lexer, node, tmp, Report.OBSOLETE_ELEMENT);
			node.was = node.tag;
			node.tag = tag;
			node.type = StartTag;
			node.implicit_Renamed = true;
			node.element = tag.name;
		}
		
		/* extract a node and its children from a markup tree */
		public static void  removeNode(Node node)
		{
			if (node.prev != null)
				node.prev.next = node.next;
			
			if (node.next != null)
				node.next.prev = node.prev;
			
			if (node.parent != null)
			{
				if (node.parent.content == node)
					node.parent.content = node.next;
				
				if (node.parent.last == node)
					node.parent.last = node.prev;
			}
			
			node.parent = node.prev = node.next = null;
		}
		
		public static bool insertMisc(Node element, Node node)
		{
			if (node.type == CommentTag || node.type == ProcInsTag || node.type == CDATATag || node.type == SectionTag || node.type == AspTag || node.type == JsteTag || node.type == PhpTag)
			{
				insertNodeAtEnd(element, node);
				return true;
			}
			
			return false;
		}
		
		/*
		used to determine how attributes
		without values should be printed
		this was introduced to deal with
		user defined tags e.g. Cold Fusion
		*/
		public static bool isNewNode(Node node)
		{
			if (node != null && node.tag != null)
			{
				return ((node.tag.model & Dict.CM_NEW) != 0);
			}
			
			return true;
		}
		
		public virtual bool hasOneChild()
		{
			return (this.content != null && this.content.next == null);
		}
		
		/* find html element */
		public virtual Node findHTML(TagTable tt)
		{
			Node node;
			
			for (node = this.content; node != null && node.tag != tt.tagHtml; node = node.next)
				;
			
			return node;
		}
		
		public virtual Node findHEAD(TagTable tt)
		{
			Node node;
			
			node = this.findHTML(tt);
			
			if (node != null)
			{
				for (node = node.content; node != null && node.tag != tt.tagHead; node = node.next)
					;
			}
			
			return node;
		}
		
		public virtual bool checkNodeIntegrity()
		{
			Node child;
			bool found = false;
			
			if (this.prev != null)
			{
				if (this.prev.next != this)
					return false;
			}
			
			if (this.next != null)
			{
				if (this.next.prev != this)
					return false;
			}
			
			if (this.parent != null)
			{
				if (this.prev == null && this.parent.content != this)
					return false;
				
				if (this.next == null && this.parent.last != this)
					return false;
				
				for (child = this.parent.content; child != null; child = child.next)
					if (child == this)
					{
						found = true;
						break;
					}
				
				if (!found)
					return false;
			}
			
			for (child = this.content; child != null; child = child.next)
				if (!child.checkNodeIntegrity())
					return false;
			
			return true;
		}
		
		/*
		Add class="foo" to node
		*/
		public static void  addClass(Node node, System.String classname)
		{
			AttVal classattr = node.getAttrByName("class");
			
			/*
			if there already is a class attribute
			then append class name after a space
			*/
			if (classattr != null)
			{
				classattr.value_Renamed = classattr.value_Renamed + " " + classname;
			}
			/* create new class attribute */
			else
				node.addAttribute("class", classname);
		}
		
		/* --------------------- DEBUG -------------------------- */
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'nodeTypeString'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly System.String[] nodeTypeString = new System.String[]{"RootNode", "DocTypeTag", "CommentTag", "ProcInsTag", "TextNode", "StartTag", "EndTag", "StartEndTag", "SectionTag", "AspTag", "PhpTag"};
		
		public override System.String ToString()
		{
			System.String s = "";
			Node n = this;
			
			while (n != null)
			{
				s += "[Node type=";
				s += nodeTypeString[n.type];
				s += ",element=";
				if (n.element != null)
					s += n.element;
				else
					s += "null";
				if (n.type == TextNode || n.type == CommentTag || n.type == ProcInsTag)
				{
					s += ",text=";
					if (n.textarray != null && n.start <= n.end)
					{
						s += "\"";
						s += Lexer.getString(n.textarray, n.start, n.end - n.start);
						s += "\"";
					}
					else
					{
						s += "null";
					}
				}
				s += ",content=";
				if (n.content != null)
				{
					s += n.content.ToString();
				}
				else
					s += "null";
				s += "]";
				if (n.next != null)
					s += ",";
				n = n.next;
			}
			return s;
		}
		/* --------------------- END DEBUG ---------------------- */
		
		
		/* --------------------- DOM ---------------------------- */
		
		protected internal Comzept.Genesis.Tidy.Dom.Node adapter = null;
		
		protected internal virtual Node cloneNode(bool deep)
		{
			Node node = (Node) this.Clone();
			if (deep)
			{
				Node child;
				Node newChild;
				for (child = this.content; child != null; child = child.next)
				{
					newChild = child.cloneNode(deep);
					insertNodeAtEnd(node, newChild);
				}
			}
			return node;
		}
	}
}