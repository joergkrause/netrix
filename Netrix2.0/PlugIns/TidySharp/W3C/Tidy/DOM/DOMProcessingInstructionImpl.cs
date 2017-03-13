/*
* @(#)DOMProcessingInstructionImpl.java   1.11 2000/08/16
*
*/
using System;
using DOMException = Comzept.Genesis.Tidy.Dom.DOMException;
namespace Comzept.Genesis.Tidy.Xml.Dom
{
	
	/// <summary> 
	/// DOMProcessingInstructionImpl
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
	
	public class DOMProcessingInstructionImpl:DOMNodeImpl, Comzept.Genesis.Tidy.Dom.ProcessingInstruction
	{
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.NodeNodeType">
		/// </seealso>
		override public short NodeType
		{
			get
			{
				return Comzept.Genesis.Tidy.Dom.Node_Fields.PROCESSING_INSTRUCTION_NODE;
			}
			
		}
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.ProcessingInstruction.getTarget">
		/// </seealso>
		virtual public System.String Target
		{
			get
			{
				// TODO
				return null;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.ProcessingInstruction.getData">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.ProcessingInstruction.setData">
		/// </seealso>
		virtual public System.String Data
		{
			get
			{
				return NodeValue;
			}
			
			set
			{
				// NOT SUPPORTED
				throw new DOMExceptionImpl(DOMException.NO_MODIFICATION_ALLOWED_ERR, "Not supported");
			}
			
		}
		
		protected internal DOMProcessingInstructionImpl(Node adaptee):base(adaptee)
		{
		}
		
		
		/* --------------------- DOM ---------------------------- */
	}
}