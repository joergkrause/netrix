/*
* @(#)AttrCheckImpl.java   1.11 2000/08/16
*
*/
using System;
namespace Comzept.Genesis.Tidy
{
	
	/// <summary> 
	/// Check attribute values implementations
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
	
	public class AttrCheckImpl
	{
		
		public class CheckUrl : AttrCheck
		{
			
			public virtual void  check(Lexer lexer, Node node, AttVal attval)
			{
				if (attval.value_Renamed == null)
					Report.attrError(lexer, node, attval.attribute, Report.MISSING_ATTR_VALUE);
				else if (lexer.configuration.FixBackslash)
				{
					attval.value_Renamed = attval.value_Renamed.Replace('\\', '/');
				}
			}
		}
		
		
		public class CheckScript : AttrCheck
		{
			
			public virtual void  check(Lexer lexer, Node node, AttVal attval)
			{
			}
		}
		
		
		public class CheckAlign : AttrCheck
		{
			
			public virtual void  check(Lexer lexer, Node node, AttVal attval)
			{
				System.String value_Renamed;
				
				/* IMG, OBJECT, APPLET and EMBED use align for vertical position */
				if (node.tag != null && ((node.tag.model & Dict.CM_IMG) != 0))
				{
					Comzept.Genesis.Tidy.AttrCheckImpl.getCheckValign().check(lexer, node, attval);
					return ;
				}
				
				value_Renamed = attval.value_Renamed;
				
				if (value_Renamed == null)
					Report.attrError(lexer, node, attval.attribute, Report.MISSING_ATTR_VALUE);
				else if (!(Lexer.wstrcasecmp(value_Renamed, "left") == 0 || Lexer.wstrcasecmp(value_Renamed, "center") == 0 || Lexer.wstrcasecmp(value_Renamed, "right") == 0 || Lexer.wstrcasecmp(value_Renamed, "justify") == 0))
					Report.attrError(lexer, node, attval.value_Renamed, Report.BAD_ATTRIBUTE_VALUE);
			}
		}
		
		
		public class CheckValign : AttrCheck
		{
			
			public virtual void  check(Lexer lexer, Node node, AttVal attval)
			{
				System.String value_Renamed;
				
				value_Renamed = attval.value_Renamed;
				
				if (value_Renamed == null)
					Report.attrError(lexer, node, attval.attribute, Report.MISSING_ATTR_VALUE);
				else if (Lexer.wstrcasecmp(value_Renamed, "top") == 0 || Lexer.wstrcasecmp(value_Renamed, "middle") == 0 || Lexer.wstrcasecmp(value_Renamed, "bottom") == 0 || Lexer.wstrcasecmp(value_Renamed, "baseline") == 0)
				{
					/* all is fine */
				}
				else if (Lexer.wstrcasecmp(value_Renamed, "left") == 0 || Lexer.wstrcasecmp(value_Renamed, "right") == 0)
				{
					if (!(node.tag != null && ((node.tag.model & Dict.CM_IMG) != 0)))
						Report.attrError(lexer, node, value_Renamed, Report.BAD_ATTRIBUTE_VALUE);
				}
				else if (Lexer.wstrcasecmp(value_Renamed, "texttop") == 0 || Lexer.wstrcasecmp(value_Renamed, "absmiddle") == 0 || Lexer.wstrcasecmp(value_Renamed, "absbottom") == 0 || Lexer.wstrcasecmp(value_Renamed, "textbottom") == 0)
				{
					lexer.versions &= Dict.VERS_PROPRIETARY;
					Report.attrError(lexer, node, value_Renamed, Report.PROPRIETARY_ATTR_VALUE);
				}
				else
					Report.attrError(lexer, node, value_Renamed, Report.BAD_ATTRIBUTE_VALUE);
			}
		}
		
		
		public class CheckBool : AttrCheck
		{
			
			public virtual void  check(Lexer lexer, Node node, AttVal attval)
			{
			}
		}
		
		
		public class CheckId : AttrCheck
		{
			
			public virtual void  check(Lexer lexer, Node node, AttVal attval)
			{
			}
		}
		
		
		public class CheckName : AttrCheck
		{
			
			public virtual void  check(Lexer lexer, Node node, AttVal attval)
			{
			}
		}
		
		
		public static AttrCheck getCheckUrl()
		{
			return _checkUrl;
		}
		
		public static AttrCheck getCheckScript()
		{
			return _checkScript;
		}
		
		public static AttrCheck getCheckAlign()
		{
			return _checkAlign;
		}
		
		public static AttrCheck getCheckValign()
		{
			return _checkValign;
		}
		
		public static AttrCheck getCheckBool()
		{
			return _checkBool;
		}
		
		public static AttrCheck getCheckId()
		{
			return _checkId;
		}
		
		public static AttrCheck getCheckName()
		{
			return _checkName;
		}
		
		
		private static AttrCheck _checkUrl = new CheckUrl();
		private static AttrCheck _checkScript = new CheckScript();
		private static AttrCheck _checkAlign = new CheckAlign();
		private static AttrCheck _checkValign = new CheckValign();
		private static AttrCheck _checkBool = new CheckBool();
		private static AttrCheck _checkId = new CheckId();
		private static AttrCheck _checkName = new CheckName();
	}
}