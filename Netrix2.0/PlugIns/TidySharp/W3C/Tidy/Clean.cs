/*
* @(#)Clean.java   1.11 2000/08/16
*
*/
using System;
namespace Comzept.Genesis.Tidy
{
	
	/// <summary> 
	/// Clean up misuse of presentation markup
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
	Filters from other formats such as Microsoft Word
	often make excessive use of presentation markup such
	as font tags, B, I, and the align attribute. By applying
	a set of production rules, it is straight forward to
	transform this to use CSS.
	
	Some rules replace some of the children of an element by
	style properties on the element, e.g.
	
	<p><b>...</b></p> -> <p style="font-weight: bold">...</p>
	
	Such rules are applied to the element's content and then
	to the element itself until none of the rules more apply.
	Having applied all the rules to an element, it will have
	a style attribute with one or more properties. 
	
	Other rules strip the element they apply to, replacing
	it by style properties on the contents, e.g.
	
	<dir><li><p>...</li></dir> -> <p style="margin-left 1em">...
	
	These rules are applied to an element before processing
	its content and replace the current element by the first
	element in the exposed content.
	
	After applying both sets of rules, you can replace the
	style attribute by a class value and style rule in the
	document head. To support this, an association of styles
	and class names is built.
	
	A naive approach is to rely on string matching to test
	when two property lists are the same. A better approach
	would be to first sort the properties before matching.*/
	
	public class Clean
	{
		
		private int classNum = 1;
		
		private TagTable tt;
		
		public Clean(TagTable tt)
		{
			this.tt = tt;
		}
		
		private StyleProp insertProperty(StyleProp props, System.String name, System.String value_Renamed)
		{
			StyleProp first, prev, prop;
			int cmp;
			
			prev = null;
			first = props;
			
			while (props != null)
			{
				cmp = String.CompareOrdinal(props.Name, name);
				
				if (cmp == 0)
				{
					/* this property is already defined, ignore new value */
					return first;
				}
				
				if (cmp > 0)
				// props.name > name
				{
					/* insert before this */
					
					prop = new StyleProp(name, value_Renamed, props);
					
					if (prev != null)
						prev.Next = prop;
					else
						first = prop;
					
					return first;
				}
				
				prev = props;
				props = props.Next;
			}
			
			prop = new StyleProp(name, value_Renamed);
			
			if (prev != null)
				prev.Next = prop;
			else
				first = prop;
			
			return first;
		}
		
		/*
		Create sorted linked list of properties from style string
		It temporarily places nulls in place of ':' and ';' to
		delimit the strings for the property name and value.
		Some systems don't allow you to null literal strings,
		so to avoid this, a copy is made first.
		*/
		private StyleProp createProps(StyleProp prop, System.String style)
		{
			int name_end;
			int value_end;
			int value_start = 0;
			int name_start = 0;
			bool more;
			
			name_start = 0;
			while (name_start < style.Length)
			{
				while (name_start < style.Length && style[name_start] == ' ')
					++name_start;
				
				name_end = name_start;
				
				while (name_end < style.Length)
				{
					if (style[name_end] == ':')
					{
						value_start = name_end + 1;
						break;
					}
					
					++name_end;
				}
				
				if (name_end >= style.Length || style[name_end] != ':')
					break;
				
				while (value_start < style.Length && style[value_start] == ' ')
					++value_start;
				
				value_end = value_start;
				more = false;
				
				while (value_end < style.Length)
				{
					if (style[value_end] == ';')
					{
						more = true;
						break;
					}
					
					++value_end;
				}
				
				prop = insertProperty(prop, style.Substring(name_start, (name_end) - (name_start)), style.Substring(value_start, (value_end) - (value_start)));
				
				if (more)
				{
					name_start = value_end + 1;
					continue;
				}
				
				break;
			}
			
			return prop;
		}
		
		private System.String createPropString(StyleProp props)
		{
			System.String style = "";
			int len;
			StyleProp prop;
			
			/* compute length */
			
			for (len = 0, prop = props; prop != null; prop = prop.Next)
			{
				len += prop.Name.Length + 2;
				len += prop.ValueRenamed.Length + 2;
			}
			
			for (prop = props; prop != null; prop = prop.Next)
			{
				style = System.String.Concat(style, prop.Name);
				style = System.String.Concat(style, ": ");
				
				style = System.String.Concat(style, prop.ValueRenamed);
				
				if (prop.Next == null)
					break;
				
				style = System.String.Concat(style, "; ");
			}
			
			return style;
		}
		
		/*
		create string with merged properties
		*/
		private System.String addProperty(System.String style, System.String property)
		{
			StyleProp prop;
			
			prop = createProps(null, style);
			prop = createProps(prop, property);
			style = createPropString(prop);
			return style;
		}
		
		private System.String gensymClass(System.String tag)
		{
			System.String str;
			
			str = "c" + classNum;
			classNum++;
			return str;
		}
		
		private System.String findStyle(Lexer lexer, System.String tag, System.String properties)
		{
			Style style;
			
			for (style = lexer.styles; style != null; style = style.next)
			{
				if (style.tag.Equals(tag) && style.properties.Equals(properties))
					return style.tagClass;
			}
			
			style = new Style(tag, gensymClass(tag), properties, lexer.styles);
			lexer.styles = style;
			return style.tagClass;
		}
		
		/*
		Find style attribute in node, and replace it
		by corresponding class attribute. Search for
		class in style dictionary otherwise gensym
		new class and add to dictionary.
		
		Assumes that node doesn't have a class attribute
		*/
		private void  style2Rule(Lexer lexer, Node node)
		{
			AttVal styleattr, classattr;
			System.String classname;
			
			styleattr = node.getAttrByName("style");
			
			if (styleattr != null)
			{
				classname = findStyle(lexer, node.element, styleattr.value_Renamed);
				classattr = node.getAttrByName("class");
				
				/*
				if there already is a class attribute
				then append class name after a space
				*/
				if (classattr != null)
				{
					classattr.value_Renamed = classattr.value_Renamed + " " + classname;
					node.removeAttribute(styleattr);
				}
				/* reuse style attribute for class attribute */
				else
				{
					styleattr.attribute = "class";
					styleattr.value_Renamed = classname;
				}
			}
		}
		
		private void  addColorRule(Lexer lexer, System.String selector, System.String color)
		{
			if (color != null)
			{
				lexer.addStringLiteral(selector);
				lexer.addStringLiteral(" { color: ");
				lexer.addStringLiteral(color);
				lexer.addStringLiteral(" }\n");
			}
		}
		
		/*
		move presentation attribs from body to style element
		
		background="foo" ->  body { background-image: url(foo) }
		bgcolor="foo"    ->  body { background-color: foo }
		text="foo"       ->  body { color: foo }
		link="foo"       ->  :link { color: foo }
		vlink="foo"      ->  :visited { color: foo }
		alink="foo"      ->  :active { color: foo }
		*/
		private void  cleanBodyAttrs(Lexer lexer, Node body)
		{
			AttVal attr;
			System.String bgurl = null;
			System.String bgcolor = null;
			System.String color = null;
			
			attr = body.getAttrByName("background");
			
			if (attr != null)
			{
				bgurl = attr.value_Renamed;
				attr.value_Renamed = null;
				body.removeAttribute(attr);
			}
			
			attr = body.getAttrByName("bgcolor");
			
			if (attr != null)
			{
				bgcolor = attr.value_Renamed;
				attr.value_Renamed = null;
				body.removeAttribute(attr);
			}
			
			attr = body.getAttrByName("text");
			
			if (attr != null)
			{
				color = attr.value_Renamed;
				attr.value_Renamed = null;
				body.removeAttribute(attr);
			}
			
			if (bgurl != null || bgcolor != null || color != null)
			{
				lexer.addStringLiteral(" body {\n");
				
				if (bgurl != null)
				{
					lexer.addStringLiteral("  background-image: url(");
					lexer.addStringLiteral(bgurl);
					lexer.addStringLiteral(");\n");
				}
				
				if (bgcolor != null)
				{
					lexer.addStringLiteral("  background-color: ");
					lexer.addStringLiteral(bgcolor);
					lexer.addStringLiteral(";\n");
				}
				
				if (color != null)
				{
					lexer.addStringLiteral("  color: ");
					lexer.addStringLiteral(color);
					lexer.addStringLiteral(";\n");
				}
				
				lexer.addStringLiteral(" }\n");
			}
			
			attr = body.getAttrByName("link");
			
			if (attr != null)
			{
				addColorRule(lexer, " :link", attr.value_Renamed);
				body.removeAttribute(attr);
			}
			
			attr = body.getAttrByName("vlink");
			
			if (attr != null)
			{
				addColorRule(lexer, " :visited", attr.value_Renamed);
				body.removeAttribute(attr);
			}
			
			attr = body.getAttrByName("alink");
			
			if (attr != null)
			{
				addColorRule(lexer, " :active", attr.value_Renamed);
				body.removeAttribute(attr);
			}
		}
		
		private bool niceBody(Lexer lexer, Node doc)
		{
			Node body = doc.findBody(lexer.configuration.tt);
			
			if (body != null)
			{
				if (body.getAttrByName("background") != null || body.getAttrByName("bgcolor") != null || body.getAttrByName("text") != null || body.getAttrByName("link") != null || body.getAttrByName("vlink") != null || body.getAttrByName("alink") != null)
				{
					lexer.badLayout |= Report.USING_BODY;
					return false;
				}
			}
			
			return true;
		}
		
		/* create style element using rules from dictionary */
		private void  createStyleElement(Lexer lexer, Node doc)
		{
			Node node, head, body;
			Style style;
			AttVal av;
			
			if (lexer.styles == null && niceBody(lexer, doc))
				return ;
			
			node = lexer.newNode(Node.StartTag, null, 0, 0, "style");
			node.implicit_Renamed = true;
			
			/* insert type attribute */
			av = new AttVal(null, null, '"', "type", "text/css");
			av.dict = AttributeTable.DefaultAttributeTable.findAttribute(av);
			node.attributes = av;
			
			body = doc.findBody(lexer.configuration.tt);
			
			lexer.txtstart = lexer.lexsize;
			
			if (body != null)
				cleanBodyAttrs(lexer, body);
			
			for (style = lexer.styles; style != null; style = style.next)
			{
				lexer.addCharToLexer(' ');
				lexer.addStringLiteral(style.tag);
				lexer.addCharToLexer('.');
				lexer.addStringLiteral(style.tagClass);
				lexer.addCharToLexer(' ');
				lexer.addCharToLexer('{');
				lexer.addStringLiteral(style.properties);
				lexer.addCharToLexer('}');
				lexer.addCharToLexer('\n');
			}
			
			lexer.txtend = lexer.lexsize;
			
			Node.insertNodeAtEnd(node, lexer.newNode(Node.TextNode, lexer.lexbuf, lexer.txtstart, lexer.txtend));
			
			/*
			now insert style element into document head
			
			doc is root node. search its children for html node
			the head node should be first child of html node
			*/
			
			head = doc.findHEAD(lexer.configuration.tt);
			
			if (head != null)
				Node.insertNodeAtEnd(head, node);
		}
		
		/* ensure bidirectional links are consistent */
		private void  fixNodeLinks(Node node)
		{
			Node child;
			
			if (node.prev != null)
				node.prev.next = node;
			else
				node.parent.content = node;
			
			if (node.next != null)
				node.next.prev = node;
			else
				node.parent.last = node;
			
			for (child = node.content; child != null; child = child.next)
				child.parent = node;
		}
		
		/*
		used to strip child of node when
		the node has one and only one child
		*/
		private void  stripOnlyChild(Node node)
		{
			Node child;
			
			child = node.content;
			node.content = child.content;
			node.last = child.last;
			child.content = null;
			
			for (child = node.content; child != null; child = child.next)
				child.parent = node;
		}
		
		/* used to strip font start and end tags */
		private void  discardContainer(Node element, MutableObject pnode)
		{
			Node node;
			Node parent = element.parent;
			
			if (element.content != null)
			{
				element.last.next = element.next;
				
				if (element.next != null)
				{
					element.next.prev = element.last;
					element.last.next = element.next;
				}
				else
					parent.last = element.last;
				
				if (element.prev != null)
				{
					element.content.prev = element.prev;
					element.prev.next = element.content;
				}
				else
					parent.content = element.content;
				
				for (node = element.content; node != null; node = node.next)
					node.parent = parent;
				
				pnode.Object = element.content;
			}
			else
			{
				if (element.next != null)
					element.next.prev = element.prev;
				else
					parent.last = element.prev;
				
				if (element.prev != null)
					element.prev.next = element.next;
				else
					parent.content = element.next;
				
				pnode.Object = element.next;
			}
			
			element.next = null;
			element.content = null;
		}
		
		/*
		Add style property to element, creating style
		attribute as needed and adding ; delimiter
		*/
		private void  addStyleProperty(Node node, System.String property)
		{
			AttVal av;
			
			for (av = node.attributes; av != null; av = av.next)
			{
				if (av.attribute.Equals("style"))
					break;
			}
			
			/* if style attribute already exists then insert property */
			
			if (av != null)
			{
				System.String s;
				
				s = addProperty(av.value_Renamed, property);
				av.value_Renamed = s;
			}
			/* else create new style attribute */
			else
			{
				av = new AttVal(node.attributes, null, '"', "style", property);
				av.dict = AttributeTable.DefaultAttributeTable.findAttribute(av);
				node.attributes = av;
			}
		}
		
		/*
		Create new string that consists of the
		combined style properties in s1 and s2
		
		To merge property lists, we build a linked
		list of property/values and insert properties
		into the list in order, merging values for
		the same property name.
		*/
		private System.String mergeProperties(System.String s1, System.String s2)
		{
			System.String s;
			StyleProp prop;
			
			prop = createProps(null, s1);
			prop = createProps(prop, s2);
			s = createPropString(prop);
			return s;
		}
		
		private void  mergeStyles(Node node, Node child)
		{
			AttVal av;
			System.String s1, s2, style;
			
			for (s2 = null, av = child.attributes; av != null; av = av.next)
			{
				if (av.attribute.Equals("style"))
				{
					s2 = av.value_Renamed;
					break;
				}
			}
			
			for (s1 = null, av = node.attributes; av != null; av = av.next)
			{
				if (av.attribute.Equals("style"))
				{
					s1 = av.value_Renamed;
					break;
				}
			}
			
			if (s1 != null)
			{
				if (s2 != null)
				/* merge styles from both */
				{
					style = mergeProperties(s1, s2);
					av.value_Renamed = style;
				}
			}
			else if (s2 != null)
			/* copy style of child */
			{
				av = new AttVal(node.attributes, null, '"', "style", s2);
				av.dict = AttributeTable.DefaultAttributeTable.findAttribute(av);
				node.attributes = av;
			}
		}
		
		private System.String fontSize2Name(System.String size)
		{
			/*
			String[] sizes =
			{
			"50%",
			"60%",
			"80%",
			null,
			"120%",
			"150%",
			"200%"
			};
			*/
			
			System.String[] sizes = new System.String[]{"60%", "70%", "80%", null, "120%", "150%", "200%"};
			System.String buf;
			
			if (size.Length > 0 && '0' <= size[0] && size[0] <= '6')
			{
				int n = size[0] - '0';
				return sizes[n];
			}
			
			if (size.Length > 0 && size[0] == '-')
			{
				if (size.Length > 1 && '0' <= size[1] && size[1] <= '6')
				{
					int n = size[1] - '0';
					double x;
					
					for (x = 1.0; n > 0; --n)
						x *= 0.8;
					
					x *= 100.0;
					//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
					buf = "" + (int) x + "%";
					
					return buf;
				}
				
				return "smaller"; /*"70%"; */
			}
			
			if (size.Length > 1 && '0' <= size[1] && size[1] <= '6')
			{
				int n = size[1] - '0';
				double x;
				
				for (x = 1.0; n > 0; --n)
					x *= 1.2;
				
				x *= 100.0;
				//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
				buf = "" + (int) x + "%";
				
				return buf;
			}
			
			return "larger"; /* "140%" */
		}
		
		private void  addFontFace(Node node, System.String face)
		{
			addStyleProperty(node, "font-family: " + face);
		}
		
		private void  addFontSize(Node node, System.String size)
		{
			System.String value_Renamed;
			
			if (size.Equals("6") && node.tag == tt.tagP)
			{
				node.element = "h1";
				tt.findTag(node);
				return ;
			}
			
			if (size.Equals("5") && node.tag == tt.tagP)
			{
				node.element = "h2";
				tt.findTag(node);
				return ;
			}
			
			if (size.Equals("4") && node.tag == tt.tagP)
			{
				node.element = "h3";
				tt.findTag(node);
				return ;
			}
			
			value_Renamed = fontSize2Name(size);
			
			if (value_Renamed != null)
			{
				addStyleProperty(node, "font-size: " + value_Renamed);
			}
		}
		
		private void  addFontColor(Node node, System.String color)
		{
			addStyleProperty(node, "color: " + color);
		}
		
		private void  addAlign(Node node, System.String align)
		{
			/* force alignment value to lower case */
			addStyleProperty(node, "text-align: " + align.ToLower());
		}
		
		/*
		add style properties to node corresponding to
		the font face, size and color attributes
		*/
		private void  addFontStyles(Node node, AttVal av)
		{
			while (av != null)
			{
				if (av.attribute.Equals("face"))
					addFontFace(node, av.value_Renamed);
				else if (av.attribute.Equals("size"))
					addFontSize(node, av.value_Renamed);
				else if (av.attribute.Equals("color"))
					addFontColor(node, av.value_Renamed);
				
				av = av.next;
			}
		}
		
		/*
		Symptom: <p align=center>
		Action: <p style="text-align: center">
		*/
		private void  textAlign(Lexer lexer, Node node)
		{
			AttVal av, prev;
			
			prev = null;
			
			for (av = node.attributes; av != null; av = av.next)
			{
				if (av.attribute.Equals("align"))
				{
					if (prev != null)
						prev.next = av.next;
					else
						node.attributes = av.next;
					
					if (av.value_Renamed != null)
					{
						addAlign(node, av.value_Renamed);
					}
					
					break;
				}
				
				prev = av;
			}
		}
		
		/*
		The clean up rules use the pnode argument to return the
		next node when the orignal node has been deleted
		*/
		
		/*
		Symptom: <dir> <li> where <li> is only child
		Action: coerce <dir> <li> to <div> with indent.
		*/
		
		private bool dir2Div(Lexer lexer, Node node, MutableObject pnode)
		{
			Node child;
			
			if (node.tag == tt.tagDir || node.tag == tt.tagUl || node.tag == tt.tagOl)
			{
				child = node.content;
				
				if (child == null)
					return false;
				
				/* check child has no peers */
				
				if (child.next != null)
					return false;
				
				if (child.tag != tt.tagLi)
					return false;
				
				if (!child.implicit_Renamed)
					return false;
				
				/* coerce dir to div */
				
				node.tag = tt.tagDiv;
				node.element = "div";
				addStyleProperty(node, "margin-left: 2em");
				stripOnlyChild(node);
				return true;
				
				//#if 0
				//Node content;
				//Node last;
				//content = child.content;
				//last = child.last;
				//child.content = null;
				
				/* adjust parent and set margin on contents of <li> */
				
				//for (child = content; child != null; child = child.next)
				//{
				//    child.parent = node.parent;
				//    addStyleProperty(child, "margin-left: 1em");
				//}
				
				/* hook first/last into sequence */
				
				//if (content != null)
				//{
				//    content.prev = node.prev;
				//    last.next = node.next;
				//    fixNodeLinks(content);
				//    fixNodeLinks(last);
				//}
				
				//node.next = null;
				
				/* ensure that new node is cleaned */
				//pnode.setObject(cleanNode(lexer, content));
				//return true;
				//#endif
			}
			
			return false;
		}
		
		/*
		Symptom: <center>
		Action: replace <center> by <div style="text-align: center">
		*/
		
		private bool center2Div(Lexer lexer, Node node, MutableObject pnode)
		{
			if (node.tag == tt.tagCenter)
			{
				if (lexer.configuration.DropFontTags)
				{
					if (node.content != null)
					{
						Node last = node.last;
						Node parent = node.parent;
						
						discardContainer(node, pnode);
						
						node = lexer.inferredTag("br");
						
						if (last.next != null)
							last.next.prev = node;
						
						node.next = last.next;
						last.next = node;
						node.prev = last;
						
						if (parent.last == last)
							parent.last = node;
						
						node.parent = parent;
					}
					else
					{
						Node prev = node.prev;
						Node next = node.next;
						Node parent = node.parent;
						discardContainer(node, pnode);
						
						node = lexer.inferredTag("br");
						node.next = next;
						node.prev = prev;
						node.parent = parent;
						
						if (next != null)
							next.prev = node;
						else
							parent.last = node;
						
						if (prev != null)
							prev.next = node;
						else
							parent.content = node;
					}
					
					return true;
				}
				node.tag = tt.tagDiv;
				node.element = "div";
				addStyleProperty(node, "text-align: center");
				return true;
			}
			
			return false;
		}
		
		/*
		Symptom <div><div>...</div></div>
		Action: merge the two divs
		
		This is useful after nested <dir>s used by Word
		for indenting have been converted to <div>s
		*/
		private bool mergeDivs(Lexer lexer, Node node, MutableObject pnode)
		{
			Node child;
			
			if (node.tag != tt.tagDiv)
				return false;
			
			child = node.content;
			
			if (child == null)
				return false;
			
			if (child.tag != tt.tagDiv)
				return false;
			
			if (child.next != null)
				return false;
			
			mergeStyles(node, child);
			stripOnlyChild(node);
			return true;
		}
		
		/*
		Symptom: <ul><li><ul>...</ul></li></ul>
		Action: discard outer list
		*/
		
		private bool nestedList(Lexer lexer, Node node, MutableObject pnode)
		{
			Node child, list;
			
			if (node.tag == tt.tagUl || node.tag == tt.tagOl)
			{
				child = node.content;
				
				if (child == null)
					return false;
				
				/* check child has no peers */
				
				if (child.next != null)
					return false;
				
				list = child.content;
				
				if (list == null)
					return false;
				
				if (list.tag != node.tag)
					return false;
				
				pnode.Object = node.next;
				
				/* move inner list node into position of outer node */
				list.prev = node.prev;
				list.next = node.next;
				list.parent = node.parent;
				fixNodeLinks(list);
				
				/* get rid of outer ul and its li */
				child.content = null;
				node.content = null;
				node.next = null;
				
				/*
				If prev node was a list the chances are this node
				should be appended to that list. Word has no way of
				recognizing nested lists and just uses indents
				*/
				
				if (list.prev != null)
				{
					node = list;
					list = node.prev;
					
					if (list.tag == tt.tagUl || list.tag == tt.tagOl)
					{
						list.next = node.next;
						
						if (list.next != null)
							list.next.prev = list;
						
						child = list.last; /* <li> */
						
						node.parent = child;
						node.next = null;
						node.prev = child.last;
						fixNodeLinks(node);
					}
				}
				
				cleanNode(lexer, node);
				return true;
			}
			
			return false;
		}
		
		/*
		Symptom: the only child of a block-level element is a
		presentation element such as B, I or FONT
		
		Action: add style "font-weight: bold" to the block and
		strip the <b> element, leaving its children.
		
		example:
		
		<p>
		<b><font face="Arial" size="6">Draft Recommended Practice</font></b>
		</p>
		
		becomes:
		
		<p style="font-weight: bold; font-family: Arial; font-size: 6">
		Draft Recommended Practice
		</p>
		
		This code also replaces the align attribute by a style attribute.
		However, to avoid CSS problems with Navigator 4, this isn't done
		for the elements: caption, tr and table
		*/
		private bool blockStyle(Lexer lexer, Node node, MutableObject pnode)
		{
			Node child;
			
			if ((node.tag.model & (Dict.CM_BLOCK | Dict.CM_LIST | Dict.CM_DEFLIST | Dict.CM_TABLE)) != 0)
			{
				if (node.tag != tt.tagTable && node.tag != tt.tagTr && node.tag != tt.tagLi)
				{
					/* check for align attribute */
					if (node.tag != tt.tagCaption)
						textAlign(lexer, node);
					
					child = node.content;
					
					if (child == null)
						return false;
					
					/* check child has no peers */
					
					if (child.next != null)
						return false;
					
					if (child.tag == tt.tagB)
					{
						mergeStyles(node, child);
						addStyleProperty(node, "font-weight: bold");
						stripOnlyChild(node);
						return true;
					}
					
					if (child.tag == tt.tagI)
					{
						mergeStyles(node, child);
						addStyleProperty(node, "font-style: italic");
						stripOnlyChild(node);
						return true;
					}
					
					if (child.tag == tt.tagFont)
					{
						mergeStyles(node, child);
						addFontStyles(node, child.attributes);
						stripOnlyChild(node);
						return true;
					}
				}
			}
			
			return false;
		}
		
		/* the only child of table cell or an inline element such as em */
		private bool inlineStyle(Lexer lexer, Node node, MutableObject pnode)
		{
			Node child;
			
			if (node.tag != tt.tagFont && (node.tag.model & (Dict.CM_INLINE | Dict.CM_ROW)) != 0)
			{
				child = node.content;
				
				if (child == null)
					return false;
				
				/* check child has no peers */
				
				if (child.next != null)
					return false;
				
				if (child.tag == tt.tagB && lexer.configuration.LogicalEmphasis)
				{
					mergeStyles(node, child);
					addStyleProperty(node, "font-weight: bold");
					stripOnlyChild(node);
					return true;
				}
				
				if (child.tag == tt.tagI && lexer.configuration.LogicalEmphasis)
				{
					mergeStyles(node, child);
					addStyleProperty(node, "font-style: italic");
					stripOnlyChild(node);
					return true;
				}
				
				if (child.tag == tt.tagFont)
				{
					mergeStyles(node, child);
					addFontStyles(node, child.attributes);
					stripOnlyChild(node);
					return true;
				}
			}
			
			return false;
		}
		
		/*
		Replace font elements by span elements, deleting
		the font element's attributes and replacing them
		by a single style attribute.
		*/
		private bool font2Span(Lexer lexer, Node node, MutableObject pnode)
		{
			AttVal av, style, next;
			
			if (node.tag == tt.tagFont)
			{
				if (lexer.configuration.DropFontTags)
				{
					discardContainer(node, pnode);
					return false;
				}
				
				/* if FONT is only child of parent element then leave alone */
				if (node.parent.content == node && node.next == null)
					return false;
				
				addFontStyles(node, node.attributes);
				
				/* extract style attribute and free the rest */
				av = node.attributes;
				style = null;
				
				while (av != null)
				{
					next = av.next;
					
					if (av.attribute.Equals("style"))
					{
						av.next = null;
						style = av;
					}
					
					av = next;
				}
				
				node.attributes = style;
				
				node.tag = tt.tagSpan;
				node.element = "span";
				
				return true;
			}
			
			return false;
		}
		
		/*
		Applies all matching rules to a node.
		*/
		private Node cleanNode(Lexer lexer, Node node)
		{
			Node next = null;
			MutableObject o = new MutableObject();
			bool b = false;
			
			for (next = node; node.Element; node = next)
			{
				o.Object = next;
				
				b = dir2Div(lexer, node, o);
				next = (Node) o.Object;
				if (b)
					continue;
				
				b = nestedList(lexer, node, o);
				next = (Node) o.Object;
				if (b)
					continue;
				
				b = center2Div(lexer, node, o);
				next = (Node) o.Object;
				if (b)
					continue;
				
				b = mergeDivs(lexer, node, o);
				next = (Node) o.Object;
				if (b)
					continue;
				
				b = blockStyle(lexer, node, o);
				next = (Node) o.Object;
				if (b)
					continue;
				
				b = inlineStyle(lexer, node, o);
				next = (Node) o.Object;
				if (b)
					continue;
				
				b = font2Span(lexer, node, o);
				next = (Node) o.Object;
				if (b)
					continue;
				
				break;
			}
			
			return next;
		}
		
		private Node createStyleProperties(Lexer lexer, Node node)
		{
			Node child;
			
			if (node.content != null)
			{
				for (child = node.content; child != null; child = child.next)
				{
					child = createStyleProperties(lexer, child);
				}
			}
			
			return cleanNode(lexer, node);
		}
		
		private void  defineStyleRules(Lexer lexer, Node node)
		{
			Node child;
			
			if (node.content != null)
			{
				for (child = node.content; child != null; child = child.next)
				{
					defineStyleRules(lexer, child);
				}
			}
			
			style2Rule(lexer, node);
		}
		
		public virtual void  cleanTree(Lexer lexer, Node doc)
		{
			doc = createStyleProperties(lexer, doc);
			
			if (!lexer.configuration.MakeClean)
			{
				defineStyleRules(lexer, doc);
				createStyleElement(lexer, doc);
			}
		}
		
		/* simplifies <b><b> ... </b> ...</b> etc. */
		public virtual void  nestedEmphasis(Node node)
		{
			MutableObject o = new MutableObject();
			Node next;
			
			while (node != null)
			{
				next = node.next;
				
				if ((node.tag == tt.tagB || node.tag == tt.tagI) && node.parent != null && node.parent.tag == node.tag)
				{
					/* strip redundant inner element */
					o.Object = next;
					discardContainer(node, o);
					next = (Node) o.Object;
					node = next;
					continue;
				}
				
				if (node.content != null)
					nestedEmphasis(node.content);
				
				node = next;
			}
		}
		
		/* replace i by em and b by strong */
		public virtual void  emFromI(Node node)
		{
			while (node != null)
			{
				if (node.tag == tt.tagI)
				{
					node.element = tt.tagEm.name;
					node.tag = tt.tagEm;
				}
				else if (node.tag == tt.tagB)
				{
					node.element = tt.tagStrong.name;
					node.tag = tt.tagStrong;
				}
				
				if (node.content != null)
					emFromI(node.content);
				
				node = node.next;
			}
		}
		
		/*
		Some people use dir or ul without an li
		to indent the content. The pattern to
		look for is a list with a single implicit
		li. This is recursively replaced by an
		implicit blockquote.
		*/
		public virtual void  list2BQ(Node node)
		{
			while (node != null)
			{
				if (node.content != null)
					list2BQ(node.content);
				
				if (node.tag != null && node.tag.parser == ParserImpl.ParseList && node.hasOneChild() && node.content.implicit_Renamed)
				{
					stripOnlyChild(node);
					node.element = tt.tagBlockquote.name;
					node.tag = tt.tagBlockquote;
					node.implicit_Renamed = true;
				}
				
				node = node.next;
			}
		}
		
		/*
		Replace implicit blockquote by div with an indent
		taking care to reduce nested blockquotes to a single
		div with the indent set to match the nesting depth
		*/
		public virtual void  bQ2Div(Node node)
		{
			int indent;
			System.String indent_buf;
			
			while (node != null)
			{
				if (node.tag == tt.tagBlockquote && node.implicit_Renamed)
				{
					indent = 1;
					
					while (node.hasOneChild() && node.content.tag == tt.tagBlockquote && node.implicit_Renamed)
					{
						++indent;
						stripOnlyChild(node);
					}
					
					if (node.content != null)
						bQ2Div(node.content);
					
					indent_buf = "margin-left: " + ((System.Int32) (2 * indent)).ToString() + "em";
					
					node.element = tt.tagDiv.name;
					node.tag = tt.tagDiv;
					node.addAttribute("style", indent_buf);
				}
				else if (node.content != null)
					bQ2Div(node.content);
				
				
				node = node.next;
			}
		}
		
		/* node is <![if ...]> prune up to <![endif]> */
		public virtual Node pruneSection(Lexer lexer, Node node)
		{
			for (; ; )
			{
				/* discard node and returns next */
				node = Node.discardElement(node);
				
				if (node == null)
					return null;
				
				if (node.type == Node.SectionTag)
				{
					if ((Lexer.getString(node.textarray, node.start, 2)).Equals("if"))
					{
						node = pruneSection(lexer, node);
						continue;
					}
					
					if ((Lexer.getString(node.textarray, node.start, 5)).Equals("endif"))
					{
						node = Node.discardElement(node);
						break;
					}
				}
			}
			
			return node;
		}
		
		public virtual void  dropSections(Lexer lexer, Node node)
		{
			while (node != null)
			{
				if (node.type == Node.SectionTag)
				{
					/* prune up to matching endif */
					if ((Lexer.getString(node.textarray, node.start, 2)).Equals("if"))
					{
						node = pruneSection(lexer, node);
						continue;
					}
					
					/* discard others as well */
					node = Node.discardElement(node);
					continue;
				}
				
				if (node.content != null)
					dropSections(lexer, node.content);
				
				node = node.next;
			}
		}
		
		public virtual void  purgeAttributes(Node node)
		{
			AttVal attr = node.attributes;
			AttVal next = null;
			AttVal prev = null;
			
			while (attr != null)
			{
				next = attr.next;
				
				/* special check for class="Code" denoting pre text */
				if (attr.attribute != null && attr.value_Renamed != null && attr.attribute.Equals("class") && attr.value_Renamed.Equals("Code"))
				{
					prev = attr;
				}
				else if (attr.attribute != null && (attr.attribute.Equals("class") || attr.attribute.Equals("style") || attr.attribute.Equals("lang") || attr.attribute.StartsWith("x:") || ((attr.attribute.Equals("height") || attr.attribute.Equals("width")) && (node.tag == tt.tagTd || node.tag == tt.tagTr || node.tag == tt.tagTh))))
				{
					if (prev != null)
						prev.next = next;
					else
						node.attributes = next;
				}
				else
					prev = attr;
				
				attr = next;
			}
		}
		
		/* Word2000 uses span excessively, so we strip span out */
		public virtual Node stripSpan(Lexer lexer, Node span)
		{
			Node node;
			Node prev = null;
			Node content;
			
			/*
			deal with span elements that have content
			by splicing the content in place of the span
			after having processed it
			*/
			
			cleanWord2000(lexer, span.content);
			content = span.content;
			
			if (span.prev != null)
				prev = span.prev;
			else if (content != null)
			{
				node = content;
				content = content.next;
				Node.removeNode(node);
				Node.insertNodeBeforeElement(span, node);
				prev = node;
			}
			
			while (content != null)
			{
				node = content;
				content = content.next;
				Node.removeNode(node);
				Node.insertNodeAfterElement(prev, node);
				prev = node;
			}
			
			if (span.next == null)
				span.parent.last = prev;
			
			node = span.next;
			span.content = null;
			Node.discardElement(span);
			return node;
		}
		
		/* map non-breaking spaces to regular spaces */
		private void  normalizeSpaces(Lexer lexer, Node node)
		{
			while (node != null)
			{
				if (node.content != null)
					normalizeSpaces(lexer, node.content);
				
				if (node.type == Node.TextNode)
				{
					int i;
					MutableInteger c = new MutableInteger();
					int p = node.start;
					
					for (i = node.start; i < node.end; ++i)
					{
						c.value_Renamed = (int) node.textarray[i];
						
						/* look for UTF-8 multibyte character */
						if (c.value_Renamed > 0x7F)
							i += PPrint.GetUTF8(node.textarray, i, c);
						
						if (c.value_Renamed == 160)
							c.value_Renamed = ' ';
						
						p = PPrint.PutUTF8(node.textarray, p, c.value_Renamed);
					}
				}
				
				node = node.next;
			}
		}
		
		/*
		This is a major clean up to strip out all the extra stuff you get
		when you save as web page from Word 2000. It doesn't yet know what
		to do with VML tags, but these will appear as errors unless you
		declare them as new tags, such as o:p which needs to be declared
		as inline.
		*/
		public virtual void  cleanWord2000(Lexer lexer, Node node)
		{
			/* used to a list from a sequence of bulletted p's */
			Node list = null;
			
			while (node != null)
			{
				/* discard Word's style verbiage */
				if (node.tag == tt.tagStyle || node.tag == tt.tagMeta || node.type == Node.CommentTag)
				{
					node = Node.discardElement(node);
					continue;
				}
				
				/* strip out all span tags Word scatters so liberally! */
				if (node.tag == tt.tagSpan)
				{
					node = stripSpan(lexer, node);
					continue;
				}
				
				/* get rid of Word's xmlns attributes */
				if (node.tag == tt.tagHtml)
				{
					/* check that it's a Word 2000 document */
					if (node.getAttrByName("xmlns:o") == null)
						return ;
				}
				
				if (node.tag == tt.tagLink)
				{
					AttVal attr = node.getAttrByName("rel");
					
					if (attr != null && attr.value_Renamed != null && attr.value_Renamed.Equals("File-List"))
					{
						node = Node.discardElement(node);
						continue;
					}
				}
				
				/* discard empty paragraphs */
				if (node.content == null && node.tag == tt.tagP)
				{
					node = Node.discardElement(node);
					continue;
				}
				
				if (node.tag == tt.tagP)
				{
					AttVal attr = node.getAttrByName("class");
					
					/* map sequence of <p class="MsoListBullet"> to <ul>...</ul> */
					if (attr != null && attr.value_Renamed != null && attr.value_Renamed.Equals("MsoListBullet"))
					{
						Node.coerceNode(lexer, node, tt.tagLi);
						
						if (list == null || list.tag != tt.tagUl)
						{
							list = lexer.inferredTag("ul");
							Node.insertNodeBeforeElement(node, list);
						}
						
						purgeAttributes(node);
						
						if (node.content != null)
							cleanWord2000(lexer, node.content);
						
						/* remove node and append to contents of list */
						Node.removeNode(node);
						Node.insertNodeAtEnd(list, node);
						node = list.next;
					}
					/* map sequence of <p class="Code"> to <pre>...</pre> */
					else if (attr != null && attr.value_Renamed != null && attr.value_Renamed.Equals("Code"))
					{
						Node br = lexer.newLineNode();
						normalizeSpaces(lexer, node);
						
						if (list == null || list.tag != tt.tagPre)
						{
							list = lexer.inferredTag("pre");
							Node.insertNodeBeforeElement(node, list);
						}
						
						/* remove node and append to contents of list */
						Node.removeNode(node);
						Node.insertNodeAtEnd(list, node);
						stripSpan(lexer, node);
						Node.insertNodeAtEnd(list, br);
						node = list.next;
					}
					else
						list = null;
				}
				else
					list = null;
				
				/* strip out style and class attributes */
				if (node.type == Node.StartTag || node.type == Node.StartEndTag)
					purgeAttributes(node);
				
				if (node.content != null)
					cleanWord2000(lexer, node.content);
				
				node = node.next;
			}
		}
		
		public virtual bool isWord2000(Node root, TagTable tt)
		{
			Node html = root.findHTML(tt);
			
			return (html != null && html.getAttrByName("xmlns:o") != null);
		}
	}
}