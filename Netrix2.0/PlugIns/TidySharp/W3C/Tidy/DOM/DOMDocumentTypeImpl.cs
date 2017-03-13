/*
* @(#)DOMDocumentTypeImpl.java   1.11 2000/08/16
*
*/
using System;
using DOMException = Comzept.Genesis.Tidy.Dom.DOMException;
namespace Comzept.Genesis.Tidy.Xml.Dom
{
	
	/// <summary> 
	/// DOMDocumentTypeImpl
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
	
	public class DOMDocumentTypeImpl:DOMNodeImpl, Comzept.Genesis.Tidy.Dom.DocumentType
	{
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.NodeType">
		/// </seealso>
		override public short NodeType
		{
			get
			{
				return Comzept.Genesis.Tidy.Dom.Node_Fields.DOCUMENT_TYPE_NODE;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.NodeName">
		/// </seealso>
		override public System.String NodeName
		{
			get
			{
				return Name;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.DocumentType.Name">
		/// </seealso>
		virtual public System.String Name
		{
			get
			{
				System.String value_Renamed = null;
				if (adaptee.type == Node.DocTypeTag)
				{
					
					if (adaptee.textarray != null && adaptee.start < adaptee.end)
					{
						value_Renamed = Lexer.getString(adaptee.textarray, adaptee.start, adaptee.end - adaptee.start);
					}
				}
				return value_Renamed;
			}
			
		}
        /// <summary>
        /// Not supported.
        /// </summary>
		virtual public Comzept.Genesis.Tidy.Dom.NamedNodeMap Entities
		{
			get
			{
				// NOT SUPPORTED
				return null;
			}
			
		}
        /// <summary>
        /// Not supported
        /// </summary>
		virtual public Comzept.Genesis.Tidy.Dom.NamedNodeMap Notations
		{
			get
			{
				// NOT SUPPORTED
				return null;
			}
			
		}
		/// <summary> DOM2 - not implemented.</summary>
		virtual public System.String PublicId
		{
			get
			{
				return null;
			}
			
		}
		/// <summary> DOM2 - not implemented.</summary>
		virtual public System.String SystemId
		{
			get
			{
				return null;
			}
			
		}
		/// <summary> DOM2 - not implemented.</summary>
		virtual public System.String InternalSubset
		{
			get
			{
				return null;
			}
			
		}
		
        /// <summary>
        /// Base class call only.
        /// </summary>
        /// <param name="adaptee"></param>
		protected internal DOMDocumentTypeImpl(Node adaptee):base(adaptee)
		{
		}
		
		
		/* --------------------- DOM ---------------------------- */
	}
}