/*
* @(#)DOMElementImpl.java   1.11 2000/08/16
*
*/
using System;
using DOMException = Comzept.Genesis.Tidy.Dom.DOMException;
namespace Comzept.Genesis.Tidy.Xml.Dom
{
	
	/// <summary> 
	/// DOMElementImpl
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
	
	public class DOMElementImpl:DOMNodeImpl, Comzept.Genesis.Tidy.Dom.Element
	{
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NodeType">
		/// </seealso>
		override public short NodeType
		{
			get
			{
				return Comzept.Genesis.Tidy.Dom.Node_Fields.ELEMENT_NODE;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Element.TagName">
		/// </seealso>
		virtual public System.String TagName
		{
			get
			{
				return base.NodeName;
			}
			
		}
		
		protected internal DOMElementImpl(Node adaptee):base(adaptee)
		{
		}
		
		
		/* --------------------- DOM ---------------------------- */
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Element.getAttribute">
		/// </seealso>
		public virtual System.String getAttribute(System.String name)
		{
			if (this.adaptee == null)
				return null;
			
			AttVal att = this.adaptee.attributes;
			while (att != null)
			{
				if (att.attribute.Equals(name))
					break;
				att = att.next;
			}
			if (att != null)
				return att.value_Renamed;
			else
				return "";
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Element.setAttribute">
		/// </seealso>
		public virtual void  setAttribute(System.String name, System.String value_Renamed)
		{
			if (this.adaptee == null)
				return ;
			
			AttVal att = this.adaptee.attributes;
			while (att != null)
			{
				if (att.attribute.Equals(name))
					break;
				att = att.next;
			}
			if (att != null)
			{
				att.value_Renamed = value_Renamed;
			}
			else
			{
				att = new AttVal(null, null, (int) '"', name, value_Renamed);
				att.dict = AttributeTable.DefaultAttributeTable.findAttribute(att);
				if (this.adaptee.attributes == null)
				{
					this.adaptee.attributes = att;
				}
				else
				{
					att.next = this.adaptee.attributes;
					this.adaptee.attributes = att;
				}
			}
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Element.removeAttribute">
		/// </seealso>
		public virtual void  removeAttribute(System.String name)
		{
			if (this.adaptee == null)
				return ;
			
			AttVal att = this.adaptee.attributes;
			AttVal pre = null;
			while (att != null)
			{
				if (att.attribute.Equals(name))
					break;
				pre = att;
				att = att.next;
			}
			if (att != null)
			{
				if (pre == null)
				{
					this.adaptee.attributes = att.next;
				}
				else
				{
					pre.next = att.next;
				}
			}
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Element.getAttributeNode">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Attr getAttributeNode(System.String name)
		{
			if (this.adaptee == null)
				return null;
			
			AttVal att = this.adaptee.attributes;
			while (att != null)
			{
				if (att.attribute.Equals(name))
					break;
				att = att.next;
			}
			if (att != null)
				return att.Adapter;
			else
				return null;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Element.setAttributeNode">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Attr setAttributeNode(Comzept.Genesis.Tidy.Dom.Attr newAttr)
		{
			if (newAttr == null)
				return null;
			if (!(newAttr is DOMAttrImpl))
			{
				throw new DOMExceptionImpl(DOMException.WRONG_DOCUMENT_ERR, "newAttr not instanceof DOMAttrImpl");
			}
			
			DOMAttrImpl newatt = (DOMAttrImpl) newAttr;
			System.String name = newatt.avAdaptee.attribute;
			Comzept.Genesis.Tidy.Dom.Attr result = null;
			
			AttVal att = this.adaptee.attributes;
			while (att != null)
			{
				if (att.attribute.Equals(name))
					break;
				att = att.next;
			}
			if (att != null)
			{
				result = att.Adapter;
				att.adapter = newAttr;
			}
			else
			{
				if (this.adaptee.attributes == null)
				{
					this.adaptee.attributes = newatt.avAdaptee;
				}
				else
				{
					newatt.avAdaptee.next = this.adaptee.attributes;
					this.adaptee.attributes = newatt.avAdaptee;
				}
			}
			return result;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Element.removeAttributeNode">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Attr removeAttributeNode(Comzept.Genesis.Tidy.Dom.Attr oldAttr)
		{
			if (oldAttr == null)
				return null;
			
			Comzept.Genesis.Tidy.Dom.Attr result = null;
			AttVal att = this.adaptee.attributes;
			AttVal pre = null;
			while (att != null)
			{
				if (att.Adapter == oldAttr)
					break;
				pre = att;
				att = att.next;
			}
			if (att != null)
			{
				if (pre == null)
				{
					this.adaptee.attributes = att.next;
				}
				else
				{
					pre.next = att.next;
				}
				result = oldAttr;
			}
			else
			{
				throw new DOMExceptionImpl(DOMException.NOT_FOUND_ERR, "oldAttr not found");
			}
			return result;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Element.getElementsByTagName">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.NodeList getElementsByTagName(System.String name)
		{
			return new DOMNodeListByTagNameImpl(this.adaptee, name);
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Element.normalize">
		/// </seealso>
		public override void  normalize()
		{
			// NOT SUPPORTED
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		public virtual System.String getAttributeNS(System.String namespaceURI, System.String localName)
		{
			return null;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		/// <exception cref="Comzept.Genesis.Tidy.Dom.DOMException">
		/// </exception>
		public virtual void  setAttributeNS(System.String namespaceURI, System.String qualifiedName, System.String value_Renamed)
		{
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		/// <exception cref="Comzept.Genesis.Tidy.Dom.DOMException">
		/// </exception>
		public virtual void  removeAttributeNS(System.String namespaceURI, System.String localName)
		{
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		public virtual Comzept.Genesis.Tidy.Dom.Attr getAttributeNodeNS(System.String namespaceURI, System.String localName)
		{
			return null;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		/// <exception cref="Comzept.Genesis.Tidy.Dom.DOMException">
		/// </exception>
		public virtual Comzept.Genesis.Tidy.Dom.Attr setAttributeNodeNS(Comzept.Genesis.Tidy.Dom.Attr newAttr)
		{
			return null;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		public virtual Comzept.Genesis.Tidy.Dom.NodeList getElementsByTagNameNS(System.String namespaceURI, System.String localName)
		{
			return null;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		public virtual bool hasAttribute(System.String name)
		{
			return false;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		public virtual bool hasAttributeNS(System.String namespaceURI, System.String localName)
		{
			return false;
		}
	}
}