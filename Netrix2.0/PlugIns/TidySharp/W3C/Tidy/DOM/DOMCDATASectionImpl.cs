/*
* @(#)DOMCDATASectionImpl.java   1.11 2000/08/16
*
*/
using System;
using DOMException = Comzept.Genesis.Tidy.Dom.DOMException;
namespace Comzept.Genesis.Tidy.Xml.Dom
{
	
	/// <summary> 
	/// DOMCDATASectionImpl
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
	/// <author>   Gary L Peskin <garyp@firstech.com>
	/// </author>
	/// <version>  1.11, 2000/08/16 Tidy Release 4 Aug 2000
	/// </version>
	
	public class DOMCDATASectionImpl:DOMTextImpl, Comzept.Genesis.Tidy.Dom.CDATASection
	{
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NodeNodeName">
		/// </seealso>
		override public System.String NodeName
		{
			get
			{
				return "#cdata-section";
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NodeNodeType">
		/// </seealso>
		override public short NodeType
		{
			get
			{
				return Comzept.Genesis.Tidy.Dom.Node_Fields.CDATA_SECTION_NODE;
			}
			
		}
		
		protected internal DOMCDATASectionImpl(Node adaptee):base(adaptee)
		{
		}
		
		
		/* --------------------- DOM ---------------------------- */
	}
}