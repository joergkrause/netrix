/*
* @(#)DOMNodeImpl.java   1.11 2000/08/16
*
*/
using System;
using DOMException = Comzept.Genesis.Tidy.Dom.DOMException;
namespace Comzept.Genesis.Tidy.Xml.Dom
{
	
	/// <summary> 
	/// DOMNodeImpl</summary>
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
	/// <version>  1.4, 1999/09/04 DOM Support
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
	
	public class DOMNodeImpl : Comzept.Genesis.Tidy.Dom.Node
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		virtual public System.String NodeValue
		{
			get
			{
				System.String value_Renamed = ""; //BAK 10/10/2000 replaced null
				if (adaptee.type == Node.TextNode || adaptee.type == Node.CDATATag || adaptee.type == Node.CommentTag || adaptee.type == Node.ProcInsTag)
				{
					
					if (adaptee.textarray != null && adaptee.start < adaptee.end)
					{
						value_Renamed = Lexer.getString(adaptee.textarray, adaptee.start, adaptee.end - adaptee.start);
					}
				}
				return value_Renamed;
			}
			
			set
			{
				if (adaptee.type == Node.TextNode || adaptee.type == Node.CDATATag || adaptee.type == Node.CommentTag || adaptee.type == Node.ProcInsTag)
				{
					sbyte[] textarray = Lexer.getBytes(value);
					adaptee.textarray = textarray;
					adaptee.start = 0;
					adaptee.end = textarray.Length;
				}
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NodeNodeName">
		/// </seealso>
		virtual public System.String NodeName
		{
			get
			{
				return adaptee.element;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NodeNodeType">
		/// </seealso>
		virtual public short NodeType
		{
			get
			{
				short result = - 1;
				switch (adaptee.type)
				{
					
					case Node.RootNode: 
						result = Comzept.Genesis.Tidy.Dom.Node_Fields.DOCUMENT_NODE;
						break;
					
					case Node.DocTypeTag: 
						result = Comzept.Genesis.Tidy.Dom.Node_Fields.DOCUMENT_TYPE_NODE;
						break;
					
					case Node.CommentTag: 
						result = Comzept.Genesis.Tidy.Dom.Node_Fields.COMMENT_NODE;
						break;
					
					case Node.ProcInsTag: 
						result = Comzept.Genesis.Tidy.Dom.Node_Fields.PROCESSING_INSTRUCTION_NODE;
						break;
					
					case Node.TextNode: 
						result = Comzept.Genesis.Tidy.Dom.Node_Fields.TEXT_NODE;
						break;
					
					case Node.CDATATag: 
						result = Comzept.Genesis.Tidy.Dom.Node_Fields.CDATA_SECTION_NODE;
						break;
					
					case Node.StartTag: 
					case Node.StartEndTag: 
						result = Comzept.Genesis.Tidy.Dom.Node_Fields.ELEMENT_NODE;
						break;
					}
				return result;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.ParentNode">
		/// </seealso>
		virtual public Comzept.Genesis.Tidy.Dom.Node ParentNode
		{
			get
			{
				if (adaptee.parent != null)
					return adaptee.parent.Adapter;
				else
					return null;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.ChildNodes">
		/// </seealso>
		virtual public Comzept.Genesis.Tidy.Dom.NodeList ChildNodes
		{
			get
			{
				return new DOMNodeListImpl(adaptee);
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.FirstChild">
		/// </seealso>
		virtual public Comzept.Genesis.Tidy.Dom.Node FirstChild
		{
			get
			{
				if (adaptee.content != null)
					return adaptee.content.Adapter;
				else
					return null;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.LastChild">
		/// </seealso>
		virtual public Comzept.Genesis.Tidy.Dom.Node LastChild
		{
			get
			{
				if (adaptee.last != null)
					return adaptee.last.Adapter;
				else
					return null;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.PreviousSibling">
		/// </seealso>
		virtual public Comzept.Genesis.Tidy.Dom.Node PreviousSibling
		{
			get
			{
				if (adaptee.prev != null)
					return adaptee.prev.Adapter;
				else
					return null;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.NextSibling">
		/// </seealso>
		virtual public Comzept.Genesis.Tidy.Dom.Node NextSibling
		{
			get
			{
				if (adaptee.next != null)
					return adaptee.next.Adapter;
				else
					return null;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.Attributes">
		/// </seealso>
		virtual public Comzept.Genesis.Tidy.Dom.NamedNodeMap Attributes
		{
			get
			{
				return new DOMAttrMapImpl(adaptee.attributes);
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.OwnerDocument">
		/// </seealso>
		virtual public Comzept.Genesis.Tidy.Dom.Document OwnerDocument
		{
			get
			{
				Node node;
				
				node = this.adaptee;
				if (node != null && node.type == Node.RootNode)
					return null;
				
				for (node = this.adaptee; node != null && node.type != Node.RootNode; node = node.parent)
					;
				
				if (node != null)
					return (Comzept.Genesis.Tidy.Dom.Document) node.Adapter;
				else
					return null;
			}
			
		}
		/// <summary> DOM2 - not implemented.</summary>
		virtual public System.String NamespaceURI
		{
			get
			{
				return null;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> DOM2 - not implemented.</summary>
		/// <summary> DOM2 - not implemented.</summary>
		virtual public System.String Prefix
		{
			get
			{
				return null;
			}
			
			set
			{
			}
			
		}
		/// <summary> DOM2 - not implemented.</summary>
		virtual public System.String LocalName
		{
			get
			{
				return null;
			}
			
		}
		
		protected internal Node adaptee;
		
		protected internal DOMNodeImpl(Node adaptee)
		{
			this.adaptee = adaptee;
		}
		
		
		/* --------------------- DOM ---------------------------- */
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.insertBefore">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Node insertBefore(Comzept.Genesis.Tidy.Dom.Node newChild, Comzept.Genesis.Tidy.Dom.Node refChild)
		{
			// TODO - handle newChild already in tree
			
			if (newChild == null)
				return null;
			if (!(newChild is DOMNodeImpl))
			{
				throw new DOMExceptionImpl(DOMException.WRONG_DOCUMENT_ERR, "newChild not instanceof DOMNodeImpl");
			}
			DOMNodeImpl newCh = (DOMNodeImpl) newChild;
			
			if (this.adaptee.type == Node.RootNode)
			{
				if (newCh.adaptee.type != Node.DocTypeTag && newCh.adaptee.type != Node.ProcInsTag)
				{
					throw new DOMExceptionImpl(DOMException.HIERARCHY_REQUEST_ERR, "newChild cannot be a child of this node");
				}
			}
			else if (this.adaptee.type == Node.StartTag)
			{
				if (newCh.adaptee.type != Node.StartTag && newCh.adaptee.type != Node.StartEndTag && newCh.adaptee.type != Node.CommentTag && newCh.adaptee.type != Node.TextNode && newCh.adaptee.type != Node.CDATATag)
				{
					throw new DOMExceptionImpl(DOMException.HIERARCHY_REQUEST_ERR, "newChild cannot be a child of this node");
				}
			}
			if (refChild == null)
			{
				Node.insertNodeAtEnd(this.adaptee, newCh.adaptee);
				if (this.adaptee.type == Node.StartEndTag)
				{
					this.adaptee.Type = Node.StartTag;
				}
			}
			else
			{
				Node ref_Renamed = this.adaptee.content;
				while (ref_Renamed != null)
				{
					if (ref_Renamed.Adapter == refChild)
						break;
					ref_Renamed = ref_Renamed.next;
				}
				if (ref_Renamed == null)
				{
					throw new DOMExceptionImpl(DOMException.NOT_FOUND_ERR, "refChild not found");
				}
				Node.insertNodeBeforeElement(ref_Renamed, newCh.adaptee);
			}
			return newChild;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.replaceChild">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Node replaceChild(Comzept.Genesis.Tidy.Dom.Node newChild, Comzept.Genesis.Tidy.Dom.Node oldChild)
		{
			// TODO - handle newChild already in tree
			
			if (newChild == null)
				return null;
			if (!(newChild is DOMNodeImpl))
			{
				throw new DOMExceptionImpl(DOMException.WRONG_DOCUMENT_ERR, "newChild not instanceof DOMNodeImpl");
			}
			DOMNodeImpl newCh = (DOMNodeImpl) newChild;
			
			if (this.adaptee.type == Node.RootNode)
			{
				if (newCh.adaptee.type != Node.DocTypeTag && newCh.adaptee.type != Node.ProcInsTag)
				{
					throw new DOMExceptionImpl(DOMException.HIERARCHY_REQUEST_ERR, "newChild cannot be a child of this node");
				}
			}
			else if (this.adaptee.type == Node.StartTag)
			{
				if (newCh.adaptee.type != Node.StartTag && newCh.adaptee.type != Node.StartEndTag && newCh.adaptee.type != Node.CommentTag && newCh.adaptee.type != Node.TextNode && newCh.adaptee.type != Node.CDATATag)
				{
					throw new DOMExceptionImpl(DOMException.HIERARCHY_REQUEST_ERR, "newChild cannot be a child of this node");
				}
			}
			if (oldChild == null)
			{
				throw new DOMExceptionImpl(DOMException.NOT_FOUND_ERR, "oldChild not found");
			}
			else
			{
				Node n;
				Node ref_Renamed = this.adaptee.content;
				while (ref_Renamed != null)
				{
					if (ref_Renamed.Adapter == oldChild)
						break;
					ref_Renamed = ref_Renamed.next;
				}
				if (ref_Renamed == null)
				{
					throw new DOMExceptionImpl(DOMException.NOT_FOUND_ERR, "oldChild not found");
				}
				newCh.adaptee.next = ref_Renamed.next;
				newCh.adaptee.prev = ref_Renamed.prev;
				newCh.adaptee.last = ref_Renamed.last;
				newCh.adaptee.parent = ref_Renamed.parent;
				newCh.adaptee.content = ref_Renamed.content;
				if (ref_Renamed.parent != null)
				{
					if (ref_Renamed.parent.content == ref_Renamed)
						ref_Renamed.parent.content = newCh.adaptee;
					if (ref_Renamed.parent.last == ref_Renamed)
						ref_Renamed.parent.last = newCh.adaptee;
				}
				if (ref_Renamed.prev != null)
				{
					ref_Renamed.prev.next = newCh.adaptee;
				}
				if (ref_Renamed.next != null)
				{
					ref_Renamed.next.prev = newCh.adaptee;
				}
				for (n = ref_Renamed.content; n != null; n = n.next)
				{
					if (n.parent == ref_Renamed)
						n.parent = newCh.adaptee;
				}
			}
			return oldChild;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.removeChild">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Node removeChild(Comzept.Genesis.Tidy.Dom.Node oldChild)
		{
			if (oldChild == null)
				return null;
			
			Node ref_Renamed = this.adaptee.content;
			while (ref_Renamed != null)
			{
				if (ref_Renamed.Adapter == oldChild)
					break;
				ref_Renamed = ref_Renamed.next;
			}
			if (ref_Renamed == null)
			{
				throw new DOMExceptionImpl(DOMException.NOT_FOUND_ERR, "refChild not found");
			}
			Node.discardElement(ref_Renamed);
			
			if (this.adaptee.content == null && this.adaptee.type == Node.StartTag)
			{
				this.adaptee.Type = Node.StartEndTag;
			}
			
			return oldChild;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.appendChild">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Node appendChild(Comzept.Genesis.Tidy.Dom.Node newChild)
		{
			// TODO - handle newChild already in tree
			
			if (newChild == null)
				return null;
			if (!(newChild is DOMNodeImpl))
			{
				throw new DOMExceptionImpl(DOMException.WRONG_DOCUMENT_ERR, "newChild not instanceof DOMNodeImpl");
			}
			DOMNodeImpl newCh = (DOMNodeImpl) newChild;
			
			if (this.adaptee.type == Node.RootNode)
			{
				if (newCh.adaptee.type != Node.DocTypeTag && newCh.adaptee.type != Node.ProcInsTag)
				{
					throw new DOMExceptionImpl(DOMException.HIERARCHY_REQUEST_ERR, "newChild cannot be a child of this node");
				}
			}
			else if (this.adaptee.type == Node.StartTag)
			{
				if (newCh.adaptee.type != Node.StartTag && newCh.adaptee.type != Node.StartEndTag && newCh.adaptee.type != Node.CommentTag && newCh.adaptee.type != Node.TextNode && newCh.adaptee.type != Node.CDATATag)
				{
					throw new DOMExceptionImpl(DOMException.HIERARCHY_REQUEST_ERR, "newChild cannot be a child of this node");
				}
			}
			Node.insertNodeAtEnd(this.adaptee, newCh.adaptee);
			
			if (this.adaptee.type == Node.StartEndTag)
			{
				this.adaptee.Type = Node.StartTag;
			}
			
			return newChild;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.hasChildNodes">
		/// </seealso>
		public virtual bool hasChildNodes()
		{
			return (adaptee.content != null);
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.cloneNode">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Node cloneNode(bool deep)
		{
			Node node = adaptee.cloneNode(deep);
			node.parent = null;
			return node.Adapter;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		public virtual void  normalize()
		{
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		public virtual bool supports(System.String feature, System.String version)
		{
			return isSupported(feature, version);
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		public virtual bool isSupported(System.String feature, System.String version)
		{
			return false;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.hasAttributes">
		/// contributed by dlp@users.sourceforge.net
		/// </seealso>
		public virtual bool hasAttributes()
		{
			return adaptee.attributes != null;
		}
	}
}