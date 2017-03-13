/*
* @(#)DOMTextImpl.java   1.11 2000/08/16
*
*/
using System;
using DOMException = Comzept.Genesis.Tidy.Dom.DOMException;
namespace Comzept.Genesis.Tidy.Xml.Dom
{
	
	/// <summary> 
	/// DOMTextImpl
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
	
	public class DOMTextImpl:DOMCharacterDataImpl, Comzept.Genesis.Tidy.Dom.Text
	{
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.NodeName">
		/// </seealso>
		override public System.String NodeName
		{
			get
			{
				return "#text";
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Node.NodeType">
		/// </seealso>
		override public short NodeType
		{
			get
			{
				return Comzept.Genesis.Tidy.Dom.Node_Fields.TEXT_NODE;
			}
			
		}
		
		protected internal DOMTextImpl(Node adaptee):base(adaptee)
		{
		}
		
		
		/* --------------------- DOM ---------------------------- */
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.Text.splitText">
		/// </seealso>
		public virtual Comzept.Genesis.Tidy.Dom.Text splitText(int offset)
		{
			// NOT SUPPORTED
			throw new DOMExceptionImpl(DOMException.NO_MODIFICATION_ALLOWED_ERR, "Not supported");
		}
	}
}