/*
* @(#)CheckAttribsImpl.java   1.11 2000/08/16
*
*/
using System;
namespace Comzept.Genesis.Tidy
{
	
	/// <summary> 
	/// Check HTML attributes implementation
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
	
	public class CheckAttribsImpl
	{
		
		public class CheckHTML : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				AttVal attval;
				Attribute attribute;
				
				node.checkUniqueAttributes(lexer);
				
				for (attval = node.attributes; attval != null; attval = attval.next)
				{
					attribute = attval.checkAttribute(lexer, node);
					
					if (attribute == AttributeTable.attrXmlns)
						lexer.isvoyager = true;
				}
			}
		}
		
		
		public class CheckSCRIPT : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				Attribute attribute;
				AttVal lang, type;
				
				node.checkUniqueAttributes(lexer);
				
				lang = node.getAttrByName("language");
				type = node.getAttrByName("type");
				
				if (type == null)
				{
					Report.attrError(lexer, node, "type", Report.MISSING_ATTRIBUTE);
					
					/* check for javascript */
					
					if (lang != null)
					{
						System.String str = lang.value_Renamed;
						if (str.Length > 10)
							str = str.Substring(0, (10) - (0));
						if ((Lexer.wstrcasecmp(str, "javascript") == 0) || (Lexer.wstrcasecmp(str, "jscript") == 0))
						{
							node.addAttribute("type", "text/javascript");
						}
					}
					else
						node.addAttribute("type", "text/javascript");
				}
			}
		}
		
		
		public class CheckTABLE : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				AttVal attval;
				Attribute attribute;
				bool hasSummary = false;
				
				node.checkUniqueAttributes(lexer);
				
				for (attval = node.attributes; attval != null; attval = attval.next)
				{
					attribute = attval.checkAttribute(lexer, node);
					
					if (attribute == AttributeTable.attrSummary)
						hasSummary = true;
				}
				
				/* suppress warning for missing summary for HTML 2.0 and HTML 3.2 */
				if (!hasSummary && lexer.doctype != Dict.VERS_HTML20 && lexer.doctype != Dict.VERS_HTML32)
				{
					lexer.badAccess |= Report.MISSING_SUMMARY;
					Report.attrError(lexer, node, "summary", Report.MISSING_ATTRIBUTE);
				}
				
				/* convert <table border> to <table border="1"> */
				if (lexer.configuration.XmlOut)
				{
					attval = node.getAttrByName("border");
					if (attval != null)
					{
						if (attval.value_Renamed == null)
							attval.value_Renamed = "1";
					}
				}
			}
		}
		
		
		public class CheckCaption : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				AttVal attval;
				System.String value_Renamed = null;
				
				node.checkUniqueAttributes(lexer);
				
				for (attval = node.attributes; attval != null; attval = attval.next)
				{
					if (Lexer.wstrcasecmp(attval.attribute, "align") == 0)
					{
						value_Renamed = attval.value_Renamed;
						break;
					}
				}
				
				if (value_Renamed != null)
				{
					if (Lexer.wstrcasecmp(value_Renamed, "left") == 0 || Lexer.wstrcasecmp(value_Renamed, "right") == 0)
						lexer.versions &= (short) (Dict.VERS_HTML40_LOOSE | Dict.VERS_FRAMES);
					else if (Lexer.wstrcasecmp(value_Renamed, "top") == 0 || Lexer.wstrcasecmp(value_Renamed, "bottom") == 0)
						lexer.versions &= Dict.VERS_FROM32;
					else
						Report.attrError(lexer, node, value_Renamed, Report.BAD_ATTRIBUTE_VALUE);
				}
			}
		}
		
		
		public class CheckHR : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				if (node.getAttrByName("src") != null)
					Report.attrError(lexer, node, "src", Report.PROPRIETARY_ATTR_VALUE);
			}
		}
		
		
		public class CheckIMG : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				AttVal attval;
				Attribute attribute;
				bool hasAlt = false;
				bool hasSrc = false;
				bool hasUseMap = false;
				bool hasIsMap = false;
				bool hasDataFld = false;
				
				node.checkUniqueAttributes(lexer);
				
				for (attval = node.attributes; attval != null; attval = attval.next)
				{
					attribute = attval.checkAttribute(lexer, node);
					
					if (attribute == AttributeTable.attrAlt)
						hasAlt = true;
					else if (attribute == AttributeTable.attrSrc)
						hasSrc = true;
					else if (attribute == AttributeTable.attrUsemap)
						hasUseMap = true;
					else if (attribute == AttributeTable.attrIsmap)
						hasIsMap = true;
					else if (attribute == AttributeTable.attrDatafld)
						hasDataFld = true;
					else if (attribute == AttributeTable.attrWidth || attribute == AttributeTable.attrHeight)
						lexer.versions &= ~ Dict.VERS_HTML20;
				}
				
				if (!hasAlt)
				{
					lexer.badAccess |= Report.MISSING_IMAGE_ALT;
					Report.attrError(lexer, node, "alt", Report.MISSING_ATTRIBUTE);
					if (lexer.configuration.altText != null)
						node.addAttribute("alt", lexer.configuration.altText);
				}
				
				if (!hasSrc && !hasDataFld)
					Report.attrError(lexer, node, "src", Report.MISSING_ATTRIBUTE);
				
				if (hasIsMap && !hasUseMap)
					Report.attrError(lexer, node, "ismap", Report.MISSING_IMAGEMAP);
			}
		}
		
		
		public class CheckAREA : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				AttVal attval;
				Attribute attribute;
				bool hasAlt = false;
				bool hasHref = false;
				
				node.checkUniqueAttributes(lexer);
				
				for (attval = node.attributes; attval != null; attval = attval.next)
				{
					attribute = attval.checkAttribute(lexer, node);
					
					if (attribute == AttributeTable.attrAlt)
						hasAlt = true;
					else if (attribute == AttributeTable.attrHref)
						hasHref = true;
				}
				
				if (!hasAlt)
				{
					lexer.badAccess |= Report.MISSING_LINK_ALT;
					Report.attrError(lexer, node, "alt", Report.MISSING_ATTRIBUTE);
				}
				if (!hasHref)
					Report.attrError(lexer, node, "href", Report.MISSING_ATTRIBUTE);
			}
		}
		
		
		public class CheckAnchor : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				node.checkUniqueAttributes(lexer);
				
				lexer.fixId(node);
			}
		}
		
		
		public class CheckMap : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				node.checkUniqueAttributes(lexer);
				
				lexer.fixId(node);
			}
		}
		
		public class CheckSTYLE : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				AttVal type = node.getAttrByName("type");
				
				node.checkUniqueAttributes(lexer);
				
				if (type == null)
				{
					Report.attrError(lexer, node, "type", Report.MISSING_ATTRIBUTE);
					
					node.addAttribute("type", "text/css");
				}
			}
		}
		
		public class CheckTableCell : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				node.checkUniqueAttributes(lexer);
				
				/*
				HTML4 strict doesn't allow mixed content for
				elements with %block; as their content model
				*/
				if (node.getAttrByName("width") != null || node.getAttrByName("height") != null)
					lexer.versions &= ~ Dict.VERS_HTML40_STRICT;
			}
		}
		
		/* add missing type attribute when appropriate */
		public class CheckLINK : CheckAttribs
		{
			
			public virtual void  Check(Lexer lexer, Node node)
			{
				AttVal rel = node.getAttrByName("rel");
				
				node.checkUniqueAttributes(lexer);
				
				if (rel != null && rel.value_Renamed != null && rel.value_Renamed.Equals("stylesheet"))
				{
					AttVal type = node.getAttrByName("type");
					
					if (type == null)
					{
						Report.attrError(lexer, node, "type", Report.MISSING_ATTRIBUTE);
						
						node.addAttribute("type", "text/css");
					}
				}
			}
		}
		
		public static CheckAttribs getCheckHTML()
		{
			return _checkHTML;
		}
		
		public static CheckAttribs getCheckSCRIPT()
		{
			return _checkSCRIPT;
		}
		
		public static CheckAttribs getCheckTABLE()
		{
			return _checkTABLE;
		}
		
		public static CheckAttribs getCheckCaption()
		{
			return _checkCaption;
		}
		
		public static CheckAttribs getCheckIMG()
		{
			return _checkIMG;
		}
		
		public static CheckAttribs getCheckAREA()
		{
			return _checkAREA;
		}
		
		public static CheckAttribs getCheckAnchor()
		{
			return _checkAnchor;
		}
		
		public static CheckAttribs getCheckMap()
		{
			return _checkMap;
		}
		
		public static CheckAttribs getCheckSTYLE()
		{
			return _checkStyle;
		}
		
		public static CheckAttribs getCheckTableCell()
		{
			return _checkTableCell;
		}
		
		public static CheckAttribs getCheckLINK()
		{
			return _checkLINK;
		}
		
		public static CheckAttribs getCheckHR()
		{
			return _checkHR;
		}
		
		
		private static CheckAttribs _checkHTML = new CheckHTML();
		private static CheckAttribs _checkSCRIPT = new CheckSCRIPT();
		private static CheckAttribs _checkTABLE = new CheckTABLE();
		private static CheckAttribs _checkCaption = new CheckCaption();
		private static CheckAttribs _checkIMG = new CheckIMG();
		private static CheckAttribs _checkAREA = new CheckAREA();
		private static CheckAttribs _checkAnchor = new CheckAnchor();
		private static CheckAttribs _checkMap = new CheckMap();
		private static CheckAttribs _checkStyle = new CheckSTYLE();
		private static CheckAttribs _checkTableCell = new CheckTableCell();
		private static CheckAttribs _checkLINK = new CheckLINK();
		private static CheckAttribs _checkHR = new CheckHR();
	}
}