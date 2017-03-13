/*
* @(#)Dict.java   1.11 2000/08/16
*
*/
using System;
namespace Comzept.Genesis.Tidy
{
	
	/// <summary> 
	/// Tag dictionary node</summary>
	/// <remarks>
	/// (c) 1998-2000 (W3C) MIT, INRIA, Keio University
	/// See Tidy.java for the copyright notice.
	/// Derived from <a href="http://www.w3.org/People/Raggett/tidy">
	/// HTML Tidy Release 4 Aug 2000</a>
	/// 
	/// </remarks>
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
	public class Dict
	{
		
		/* content model shortcut encoding */
		/// <summary>
        /// Content model shortcut encoding.
		/// </summary>
		public const int CM_UNKNOWN = 0;
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_EMPTY = (1 << 0);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_HTML = (1 << 1);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_HEAD = (1 << 2);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_BLOCK = (1 << 3);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_INLINE = (1 << 4);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_LIST = (1 << 5);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_DEFLIST = (1 << 6);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_TABLE = (1 << 7);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_ROWGRP = (1 << 8);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_ROW = (1 << 9);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_FIELD = (1 << 10);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_OBJECT = (1 << 11);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_PARAM = (1 << 12);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_FRAMES = (1 << 13);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_HEADING = (1 << 14);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_OPT = (1 << 15);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_IMG = (1 << 16);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_MIXED = (1 << 17);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_NO_INDENT = (1 << 18);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_OBSOLETE = (1 << 19);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_NEW = (1 << 20);
        /// <summary>
        /// Content model shortcut encoding.
        /// </summary>
        public const int CM_OMITST = (1 << 21);
		
		/*
		
		If the document uses just HTML 2.0 tags and attributes described it as HTML 2.0
		Similarly for HTML 3.2 and the 3 flavors of HTML 4.0. If there are proprietary
		tags and attributes then describe it as HTML Proprietary. If it includes the
		xml-lang or xmlns attributes but is otherwise HTML 2.0, 3.2 or 4.0 then describe
		it as one of the flavors of Voyager (strict, loose or frameset).
		*/
		/// <summary>
		/// HTML Version.
		/// </summary>
		public const short VERS_UNKNOWN = 0;
        /// <summary>
        /// HTML Version.
        /// </summary>		
		public const short VERS_HTML20 = 1;
        /// <summary>
        /// HTML Version.
        /// </summary>
        public const short VERS_HTML32 = 2;
        /// <summary>
        /// HTML Version.
        /// </summary>
        public const short VERS_HTML40_STRICT = 4;
        /// <summary>
        /// HTML Version.
        /// </summary>
        public const short VERS_HTML40_LOOSE = 8;
        /// <summary>
        /// HTML Version.
        /// </summary>
        public const short VERS_FRAMES = 16;
        /// <summary>
        /// HTML Version.
        /// </summary>
        public const short VERS_XML = 32;

        /// <summary>
        /// HTML Version.
        /// </summary>
        public const short VERS_NETSCAPE = 64;
        /// <summary>
        /// HTML Version.
        /// </summary>
        public const short VERS_MICROSOFT = 128;
        /// <summary>
        /// HTML Version.
        /// </summary>
        public const short VERS_SUN = 256;

        /// <summary>
        /// HTML Version.
        /// </summary>
        public const short VERS_MALFORMED = 512;
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'VERS_ALL '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly short VERS_ALL = (short) ((VERS_HTML20 | VERS_HTML32 | VERS_HTML40_STRICT | VERS_HTML40_LOOSE | VERS_FRAMES));
		//UPGRADE_NOTE: Final was removed from the declaration of 'VERS_HTML40 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly short VERS_HTML40 = (short) ((VERS_HTML40_STRICT | VERS_HTML40_LOOSE | VERS_FRAMES));
		//UPGRADE_NOTE: Final was removed from the declaration of 'VERS_LOOSE '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly short VERS_LOOSE = (short) ((VERS_HTML32 | VERS_HTML40_LOOSE | VERS_FRAMES));
		//UPGRADE_NOTE: Final was removed from the declaration of 'VERS_IFRAMES '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly short VERS_IFRAMES = (short) ((VERS_HTML40_LOOSE | VERS_FRAMES));
		//UPGRADE_NOTE: Final was removed from the declaration of 'VERS_FROM32 '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly short VERS_FROM32 = (short) ((VERS_HTML40_STRICT | VERS_LOOSE));
		//UPGRADE_NOTE: Final was removed from the declaration of 'VERS_PROPRIETARY '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly short VERS_PROPRIETARY = (short) ((VERS_NETSCAPE | VERS_MICROSOFT | VERS_SUN));
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'VERS_EVERYTHING '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		public static readonly short VERS_EVERYTHING = (short) ((VERS_ALL | VERS_PROPRIETARY));
		
        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="versions"></param>
        /// <param name="model"></param>
        /// <param name="parser"></param>
        /// <param name="chkattrs"></param>
		public Dict(System.String name, short versions, int model, Parser parser, CheckAttribs chkattrs)
		{
			this.name = name;
			this.versions = versions;
			this.model = model;
			this.parser = parser;
			this.chkattrs = chkattrs;
		}
		
		public System.String name;
		public short versions;
		public int model;
		public Parser parser;
		public CheckAttribs chkattrs;
	}
}