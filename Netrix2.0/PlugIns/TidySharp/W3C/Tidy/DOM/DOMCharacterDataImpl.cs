/*
* @(#)DOMCharacterDataImpl.java   1.11 2000/08/16
*
*/
using System;
using DOMException = Comzept.Genesis.Tidy.Dom.DOMException;
namespace Comzept.Genesis.Tidy.Xml.Dom
{
	
	/// <summary> 
	/// DOMCharacterDataImpl
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
	
	public class DOMCharacterDataImpl:DOMNodeImpl, Comzept.Genesis.Tidy.Dom.CharacterData
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.CharacterData.Data">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.CharacterData.Data">
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
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.CharacterData.Length">
		/// </seealso>
		virtual public int Length
		{
			get
			{
				int len = 0;
				if (adaptee.textarray != null && adaptee.start < adaptee.end)
					len = adaptee.end - adaptee.start;
				return len;
			}
			
		}
		
		protected internal DOMCharacterDataImpl(Node adaptee):base(adaptee)
		{
		}
		
		
		/* --------------------- DOM ---------------------------- */
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.CharacterData.substringData">
		/// </seealso>
		public virtual System.String substringData(int offset, int count)
		{
			int len;
			System.String value_Renamed = null;
			if (count < 0)
			{
				throw new DOMExceptionImpl(DOMException.INDEX_SIZE_ERR, "Invalid length");
			}
			if (adaptee.textarray != null && adaptee.start < adaptee.end)
			{
				if (adaptee.start + offset >= adaptee.end)
				{
					throw new DOMExceptionImpl(DOMException.INDEX_SIZE_ERR, "Invalid offset");
				}
				len = count;
				if (adaptee.start + offset + len - 1 >= adaptee.end)
					len = adaptee.end - adaptee.start - offset;
				
				value_Renamed = Lexer.getString(adaptee.textarray, adaptee.start + offset, len);
			}
			return value_Renamed;
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.CharacterData.appendData">
		/// </seealso>
		public virtual void  appendData(System.String arg)
		{
			// NOT SUPPORTED
			throw new DOMExceptionImpl(DOMException.NO_MODIFICATION_ALLOWED_ERR, "Not supported");
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.CharacterData.insertData">
		/// </seealso>
		public virtual void  insertData(int offset, System.String arg)
		{
			// NOT SUPPORTED
			throw new DOMExceptionImpl(DOMException.NO_MODIFICATION_ALLOWED_ERR, "Not supported");
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.CharacterData.deleteData">
		/// </seealso>
		public virtual void  deleteData(int offset, int count)
		{
			// NOT SUPPORTED
			throw new DOMExceptionImpl(DOMException.NO_MODIFICATION_ALLOWED_ERR, "Not supported");
		}
		
		/// <seealso cref="Comzept.Genesis.Tidy.Dom.CharacterData.replaceData">
		/// </seealso>
		public virtual void  replaceData(int offset, int count, System.String arg)
		{
			// NOT SUPPORTED
			throw new DOMExceptionImpl(DOMException.NO_MODIFICATION_ALLOWED_ERR, "Not supported");
		}
	}
}