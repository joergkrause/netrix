/*
* @(#)MutableObject.java   1.11 2000/08/16
*
*/
using System;
namespace Comzept.Genesis.Tidy
{
	
	/// <summary> 
	/// Mutable Object
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
	
	public class MutableObject
	{
		virtual public System.Object Object
		{
			get
			{
				return value_Renamed;
			}
			
			set
			{
				value_Renamed = value;
			}
			
		}
		
		public MutableObject():this(null)
		{
		}
		
		public MutableObject(System.Object o)
		{
			this.value_Renamed = o;
		}
		
		private System.Object value_Renamed;
	}
}