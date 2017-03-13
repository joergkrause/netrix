/*
* @(#)DOMDocumentImpl.java   1.11 2000/08/16
*
*/
using System;
using DOMException = Comzept.Genesis.Tidy.Dom.DOMException;
namespace Comzept.Genesis.Tidy.Xml.Dom
{
	
	/// <summary> 
	/// DOMDocumentImpl
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
	
	public class DOMDocumentImpl:DOMNodeImpl, Comzept.Genesis.Tidy.Dom.Document
	{
		virtual public TagTable TagTable
		{
			set
			{
				this.tt = value;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NodeNodeName">
		/// </seealso>
		override public System.String NodeName
		{
			get
			{
				return "#document";
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NodeNodeType">
		/// </seealso>
		override public short NodeType
		{
			get
			{
				return Comzept.Genesis.Tidy.Dom.Node_Fields.DOCUMENT_NODE;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.getDoctype">
		/// </seealso>
		virtual public Comzept.Genesis.Tidy.Dom.DocumentType Doctype
		{
			get
			{
				Node node = adaptee.content;
				while (node != null)
				{
					if (node.type == Node.DocTypeTag)
						break;
					node = node.next;
				}
				if (node != null)
					return (Comzept.Genesis.Tidy.Dom.DocumentType) node.Adapter;
				else
					return null;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.getImplementation">
		/// </seealso>
		virtual public Comzept.Genesis.Tidy.Dom.DOMImplementation Implementation
		{
			get
			{
				// NOT SUPPORTED
				return null;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.getDocumentElement">
		/// </seealso>
		virtual public Comzept.Genesis.Tidy.Dom.Element DocumentElement
		{
			get
			{
				Node node = adaptee.content;
				while (node != null)
				{
					if (node.type == Node.StartTag || node.type == Node.StartEndTag)
						break;
					node = node.next;
				}
				if (node != null)
					return (Comzept.Genesis.Tidy.Dom.Element) node.Adapter;
				else
					return null;
			}
			
		}
		
		private TagTable tt; // a DOM Document has its own TagTable.
		
		protected internal DOMDocumentImpl(Node adaptee):base(adaptee)
		{
			tt = new TagTable();
		}
		
		/* --------------------- DOM ---------------------------- */
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.createElement">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Element createElement(System.String tagName)
		{
			Node node = new Node(Node.StartEndTag, null, 0, 0, tagName, tt);
			if (node != null)
			{
				if (node.tag == null)
				// Fix Bug 121206
					node.tag = tt.xmlTags;
				return (Comzept.Genesis.Tidy.Dom.Element) node.Adapter;
			}
			else
				return null;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.createDocumentFragment">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.DocumentFragment createDocumentFragment()
		{
			// NOT SUPPORTED
			return null;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.createTextNode">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Text createTextNode(System.String data)
		{
			sbyte[] textarray = Lexer.getBytes(data);
			Node node = new Node(Node.TextNode, textarray, 0, textarray.Length);
			if (node != null)
				return (Comzept.Genesis.Tidy.Dom.Text) node.Adapter;
			else
				return null;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.createComment">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Comment createComment(System.String data)
		{
			sbyte[] textarray = Lexer.getBytes(data);
			Node node = new Node(Node.CommentTag, textarray, 0, textarray.Length);
			if (node != null)
				return (Comzept.Genesis.Tidy.Dom.Comment) node.Adapter;
			else
				return null;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.createCDATASection">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.CDATASection createCDATASection(System.String data)
		{
			// NOT SUPPORTED
			return null;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.createProcessingInstruction">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.ProcessingInstruction createProcessingInstruction(System.String target, System.String data)
		{
			throw new DOMExceptionImpl(DOMException.NOT_SUPPORTED_ERR, "HTML document");
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.createAttribute">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Attr createAttribute(System.String name)
		{
			AttVal av = new AttVal(null, null, (int) '"', name, null);
			if (av != null)
			{
				av.dict = AttributeTable.DefaultAttributeTable.findAttribute(av);
				return (Comzept.Genesis.Tidy.Dom.Attr) av.Adapter;
			}
			else
			{
				return null;
			}
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.createEntityReference">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.EntityReference createEntityReference(System.String name)
		{
			// NOT SUPPORTED
			return null;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Document.getElementsByTagName">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.NodeList getElementsByTagName(System.String tagname)
		{
			return new DOMNodeListByTagNameImpl(this.adaptee, tagname);
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		/// <exception cref="Comzept.Genesis.Tidy.Dom.DOMException">
		/// </exception>
		public virtual Comzept.Genesis.Tidy.Dom.Node importNode(Comzept.Genesis.Tidy.Dom.Node importedNode, bool deep)
		{
			return null;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		/// <exception cref="Comzept.Genesis.Tidy.Dom.DOMException">
		/// </exception>
		public virtual Comzept.Genesis.Tidy.Dom.Attr createAttributeNS(System.String namespaceURI, System.String qualifiedName)
		{
			return null;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		/// <exception cref="Comzept.Genesis.Tidy.Dom.DOMException">
		/// </exception>
		public virtual Comzept.Genesis.Tidy.Dom.Element createElementNS(System.String namespaceURI, System.String qualifiedName)
		{
			return null;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		public virtual Comzept.Genesis.Tidy.Dom.NodeList getElementsByTagNameNS(System.String namespaceURI, System.String localName)
		{
			return null;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		public virtual Comzept.Genesis.Tidy.Dom.Element getElementById(System.String elementId)
		{
			return null;
		}
	}
}