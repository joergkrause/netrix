/*
* @(#)DOMAttrImpl.java   1.11 2000/08/16
*
*/
using System;
using DOMException = Comzept.Genesis.Tidy.Dom.DOMException;
namespace Comzept.Genesis.Tidy.Xml.Dom
{
	
	/// <summary> 
	/// DOMAttrImpl </summary>
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
	
	public class DOMAttrImpl:DOMNodeImpl, Comzept.Genesis.Tidy.Dom.Attr
	{
		override public System.String NodeValue
		{
			/* --------------------- DOM ---------------------------- */
			
			
			get
			{
				return Value;
			}
			
			set
			{
				Value = value;
			}
			
		}
		override public System.String NodeName
		{
			get
			{
				return Name;
			}
			
		}
		override public short NodeType
		{
			get
			{
				return Comzept.Genesis.Tidy.Dom.Node_Fields.ATTRIBUTE_NODE;
			}
			
		}
		override public Comzept.Genesis.Tidy.Dom.Node ParentNode
		{
			get
			{
				return null;
			}
			
		}
		override public Comzept.Genesis.Tidy.Dom.NodeList ChildNodes
		{
			get
			{
				// NOT SUPPORTED
				return null;
			}
			
		}
		override public Comzept.Genesis.Tidy.Dom.Node FirstChild
		{
			get
			{
				// NOT SUPPORTED
				return null;
			}
			
		}
		override public Comzept.Genesis.Tidy.Dom.Node LastChild
		{
			get
			{
				// NOT SUPPORTED
				return null;
			}
			
		}
		override public Comzept.Genesis.Tidy.Dom.Node PreviousSibling
		{
			get
			{
				return null;
			}
			
		}
		override public Comzept.Genesis.Tidy.Dom.Node NextSibling
		{
			get
			{
				return null;
			}
			
		}
		override public Comzept.Genesis.Tidy.Dom.NamedNodeMap Attributes
		{
			get
			{
				return null;
			}
			
		}
		override public Comzept.Genesis.Tidy.Dom.Document OwnerDocument
		{
			get
			{
				return null;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Attr.Name">
		/// </seealso>
		virtual public System.String Name
		{
			get
			{
				return avAdaptee.attribute;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Attr.Specified">
		/// </seealso>
		virtual public bool Specified
		{
			get
			{
				return true;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns value of this attribute.  If this attribute has a null value,
		/// then the attribute name is returned instead.
		/// Thanks to Brett Knights <brett@knightsofthenet.com> for this fix.
		/// </summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Attr.getValue">
		/// 
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Attr.setValue">
		/// </seealso>
		virtual public System.String Value
		{
			get
			{
				return (avAdaptee.value_Renamed == null)?avAdaptee.attribute:avAdaptee.value_Renamed;
			}
			
			set
			{
				avAdaptee.value_Renamed = value;
			}
			
		}
		/// <summary> DOM2 - not implemented.</summary>
		virtual public Comzept.Genesis.Tidy.Dom.Element OwnerElement
		{
			get
			{
				return null;
			}
			
		}
		
		protected internal AttVal avAdaptee;
		
		protected internal DOMAttrImpl(AttVal adaptee):base(null)
		{ // must override all methods of DOMNodeImpl
			this.avAdaptee = adaptee;
		}
		
		public override Comzept.Genesis.Tidy.Dom.Node insertBefore(Comzept.Genesis.Tidy.Dom.Node newChild, Comzept.Genesis.Tidy.Dom.Node refChild)
		{
			throw new DOMExceptionImpl(DOMException.NO_MODIFICATION_ALLOWED_ERR, "Not supported");
		}
		
		public override Comzept.Genesis.Tidy.Dom.Node replaceChild(Comzept.Genesis.Tidy.Dom.Node newChild, Comzept.Genesis.Tidy.Dom.Node oldChild)
		{
			throw new DOMExceptionImpl(DOMException.NO_MODIFICATION_ALLOWED_ERR, "Not supported");
		}
		
		public override Comzept.Genesis.Tidy.Dom.Node removeChild(Comzept.Genesis.Tidy.Dom.Node oldChild)
		{
			throw new DOMExceptionImpl(DOMException.NO_MODIFICATION_ALLOWED_ERR, "Not supported");
		}
		
		public override Comzept.Genesis.Tidy.Dom.Node appendChild(Comzept.Genesis.Tidy.Dom.Node newChild)
		{
			throw new DOMExceptionImpl(DOMException.NO_MODIFICATION_ALLOWED_ERR, "Not supported");
		}
		
		public override bool hasChildNodes()
		{
			return false;
		}
		
		public override Comzept.Genesis.Tidy.Dom.Node cloneNode(bool deep)
		{
			return null;
		}
	}
}