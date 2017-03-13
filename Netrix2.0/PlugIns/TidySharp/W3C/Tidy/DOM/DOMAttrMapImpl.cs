/*
* @(#)DOMAttrMapImpl.java   1.11 2000/08/16
*
*/
using System;
using DOMException = Comzept.Genesis.Tidy.Dom.DOMException;
namespace Comzept.Genesis.Tidy.Xml.Dom
{
	
	/// <summary> 
	/// DOMAttrMapImpl
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
	
	public class DOMAttrMapImpl : Comzept.Genesis.Tidy.Dom.NamedNodeMap
	{
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NamedNodeMap.getLength">
		/// </seealso>
		virtual public int Length
		{
			get
			{
				int len = 0;
				AttVal att = this.first;
				while (att != null)
				{
					len++;
					att = att.next;
				}
				return len;
			}
			
		}
		
		private AttVal first = null;
		
		protected internal DOMAttrMapImpl(AttVal first)
		{
			this.first = first;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NamedNodeMap.getNamedItem">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Node getNamedItem(System.String name)
		{
			AttVal att = this.first;
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
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NamedNodeMap.setNamedItem">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Node setNamedItem(Comzept.Genesis.Tidy.Dom.Node arg)
		{
			// NOT SUPPORTED
			return null;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NamedNodeMap.removeNamedItem">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Node removeNamedItem(System.String name)
		{
			// NOT SUPPORTED
			return null;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NamedNodeMap.item">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Node item(int index)
		{
			int i = 0;
			AttVal att = this.first;
			while (att != null)
			{
				if (i >= index)
					break;
				i++;
				att = att.next;
			}
			if (att != null)
				return att.Adapter;
			else
				return null;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		public virtual Comzept.Genesis.Tidy.Dom.Node getNamedItemNS(System.String namespaceURI, System.String localName)
		{
			return null;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		/// <exception cref="Comzept.Genesis.Tidy.Dom.DOMException">
		/// </exception>
		public virtual Comzept.Genesis.Tidy.Dom.Node setNamedItemNS(Comzept.Genesis.Tidy.Dom.Node arg)
		{
			return null;
		}
		
		/// <summary> DOM2 - not implemented.</summary>
		/// <exception cref="Comzept.Genesis.Tidy.Dom.DOMException">
		/// </exception>
		public virtual Comzept.Genesis.Tidy.Dom.Node removeNamedItemNS(System.String namespaceURI, System.String localName)
		{
			return null;
		}
	}
}