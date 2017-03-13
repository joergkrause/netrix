/*
* @(#)StreamIn.java   1.11 2000/08/16
*
*/

using System;
namespace Comzept.Genesis.Tidy
{
	/// <summary> 
/// Input Stream
/// 
/// (c) 1998-2000 (W3C) MIT, INRIA, Keio University
/// Derived from <a href="http://www.w3.org/People/Raggett/tidy">
/// HTML Tidy Release 4 Aug 2000</a>
/// 
/// </summary>
/// <author>   Dave Raggett dsr@w3.org
/// </author>
/// <author>   Andy Quick ac.quick@sympatico.ca (translation to Java)
/// </author>
/// <version>  1.0, 1999/05/22
/// </version>
/// <version>  1.0.1, 1999/05/29
/// </version>
/// <version>  1.1, 1999/06/18 Java Bean
/// </version>
/// <version>  1.2, 1999/07/10 Tidy Release 7 Jul 1999
/// </version>
/// <version>  1.3, 1999/07/30 Tidy Release 26 Jul 1999
/// </version>
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
	public abstract class StreamIn
	{
		/// <summary>
		/// End of stream sign.
		/// </summary>
		public const int EndOfStream = - 1; // EOF
		
		/* states for ISO 2022 
		
		A document in ISO-2022 based encoding uses some ESC sequences called 
		"designator" to switch character sets. The designators defined and 
		used in ISO-2022-JP are:
		
		"ESC" + "(" + ?     for ISO646 variants
		
		"ESC" + "$" + ?     and
		"ESC" + "$" + "(" + ?   for multibyte character sets
		*/
		
		public const int FSM_ASCII = 0;
		public const int FSM_ESC = 1;
		public const int FSM_ESCD = 2;
		public const int FSM_ESCDP = 3;
		public const int FSM_ESCP = 4;
		public const int FSM_NONASCII = 5;
		
		/* non-raw input is cleaned up*/
		public int state; /* FSM for ISO2022 */
		public bool pushed;
		public int c;
		public int tabs;
		public int tabsize;
		public int lastcol;
		public int curcol;
		public int curline;
		public int encoding;
		public System.IO.Stream stream;
		public bool endOfStream;
		public System.Object lexer; /* needed for error reporting */
		
		/// <summary>
        /// read char from stream
		/// </summary>
		/// <returns></returns>
		public abstract int ReadCharFromStream();
		
		public abstract int ReadChar();
		
		public abstract void  UngetChar(int c);

        public abstract bool IsEndOfStream { get; }
	}
}