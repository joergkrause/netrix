/*
* @(#)AttributeTable.java   1.11 2000/08/16
*
*/
using System;
namespace Comzept.Genesis.Tidy
{
	
	/// <summary> 
	/// HTML attribute hash table</summary>
	/// <remarks>
	/// (c) 1998-2000 (W3C) MIT, INRIA, Keio University
	/// See Tidy.java for the copyright notice.
	/// Derived from <a href="http://www.w3.org/People/Raggett/tidy">
	/// HTML Tidy Release 4 Aug 2000</a>
	/// </remarks>
	/// 
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
	public class AttributeTable
	{
        /// <summary>
        /// Table of expected default attributes.
        /// </summary>
		public static AttributeTable DefaultAttributeTable
		{
			get
			{
				if (defaultAttributeTable == null)
				{
					defaultAttributeTable = new AttributeTable();
					for (int i = 0; i < attrs.Length; i++)
					{
						defaultAttributeTable.Install(attrs[i]);
					}
					attrHref = defaultAttributeTable.Lookup("href");
					attrSrc = defaultAttributeTable.Lookup("src");
					attrId = defaultAttributeTable.Lookup("id");
					attrName = defaultAttributeTable.Lookup("name");
					attrSummary = defaultAttributeTable.Lookup("summary");
					attrAlt = defaultAttributeTable.Lookup("alt");
					attrLongdesc = defaultAttributeTable.Lookup("longdesc");
					attrUsemap = defaultAttributeTable.Lookup("usemap");
					attrIsmap = defaultAttributeTable.Lookup("ismap");
					attrLanguage = defaultAttributeTable.Lookup("language");
					attrType = defaultAttributeTable.Lookup("type");
					attrTitle = defaultAttributeTable.Lookup("title");
					attrXmlns = defaultAttributeTable.Lookup("xmlns");
					attrValue = defaultAttributeTable.Lookup("value");
					attrContent = defaultAttributeTable.Lookup("content");
					attrDatafld = defaultAttributeTable.Lookup("datafld"); ;
					attrWidth = defaultAttributeTable.Lookup("width"); ;
					attrHeight = defaultAttributeTable.Lookup("height"); ;
					
					attrAlt.NoWrap = true;
					attrValue.NoWrap = true;
					attrContent.NoWrap = true;
				}
				return defaultAttributeTable;
			}
			
		}
		
		public AttributeTable()
		{
		}
		
		public virtual Attribute Lookup(System.String name)
		{
			return (Attribute) attributeHashtable[name];
		}
		
		public virtual Attribute Install(Attribute attr)
		{
			System.Object tempObject;
			tempObject = attributeHashtable[attr.Name];
			attributeHashtable[attr.Name] = attr;
			return (Attribute) tempObject;
		}
		
        /// <summary>
        /// Public method for finding attribute definition by name
        /// </summary>
        /// <param name="attval"></param>
        /// <returns></returns>
		public virtual Attribute findAttribute(AttVal attval)
		{
			Attribute np;
			
			if (attval.attribute != null)
			{
				np = Lookup(attval.attribute);
				return np;
			}
			
			return null;
		}
		
		public virtual bool IsUrl(System.String attrname)
		{
			Attribute np;
			
			np = Lookup(attrname);
			return (np != null && np.AttrChk == AttrCheckImpl.getCheckUrl());
		}
		
		public virtual bool IsScript(System.String attrname)
		{
			Attribute np;
			
			np = Lookup(attrname);
			return (np != null && np.AttrChk == AttrCheckImpl.getCheckScript());
		}
		
		public virtual bool IsLiteralAttribute(System.String attrname)
		{
			Attribute np;
			
			np = Lookup(attrname);
			return (np != null && np.Literal);
		}
		
        /// <summary>Declare attribt as literal</summary>
		/// <remarks>
		/// Henry Zrepa reports that some folk are
		/// using embed with script attributes where
		/// newlines are signficant. These need to be
        /// declared and handled specially!</remarks>
		/// <param name="name"></param>
		public virtual void  declareLiteralAttrib(System.String name)
		{
			Attribute attrib = Lookup(name);
			
			if (attrib == null)
				attrib = Install(new Attribute(name, Dict.VERS_PROPRIETARY, null));
			
			attrib.Literal = true;
		}
		
		private System.Collections.Hashtable attributeHashtable = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
		
		private static AttributeTable defaultAttributeTable = null;
		
		//UPGRADE_NOTE: The initialization of  'attrs' was moved to static method 'Comzept.Genesis.Tidy.AttributeTable'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private static Attribute[] attrs;
		/// <summary>
		/// Attribute href
		/// </summary>
		public static Attribute attrHref = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrSrc = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrId = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrName = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrSummary = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrAlt = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrLongdesc = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrUsemap = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrIsmap = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrLanguage = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrType = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrTitle = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrXmlns = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrValue = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrContent = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrDatafld = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrWidth = null;
        /// <summary>
        /// Attribute 
        /// </summary>
        public static Attribute attrHeight = null;

		static AttributeTable()
		{
			attrs = new Attribute[]{new Attribute("abbr", Dict.VERS_HTML40, null), new Attribute("accept-charset", Dict.VERS_HTML40, null), new Attribute("accept", Dict.VERS_ALL, null), new Attribute("accesskey", Dict.VERS_HTML40, null), new Attribute("action", Dict.VERS_ALL, AttrCheckImpl.getCheckUrl()), new Attribute("add_date", Dict.VERS_NETSCAPE, null), new Attribute("align", Dict.VERS_ALL, AttrCheckImpl.getCheckAlign()), new Attribute("alink", Dict.VERS_LOOSE, null), new Attribute("alt", Dict.VERS_ALL, null), new Attribute("archive", Dict.VERS_HTML40, null), new Attribute("axis", Dict.VERS_HTML40, null), new Attribute("background", Dict.VERS_LOOSE, AttrCheckImpl.getCheckUrl()), new Attribute("bgcolor", Dict.VERS_LOOSE, null), new Attribute("bgproperties", Dict.VERS_PROPRIETARY, null), new Attribute("border", Dict.VERS_ALL, AttrCheckImpl.getCheckBool()), new Attribute("bordercolor", Dict.VERS_MICROSOFT, null), new Attribute("bottommargin", Dict.VERS_MICROSOFT, null), new Attribute("cellpadding", Dict.VERS_FROM32, null), new Attribute("cellspacing", Dict.VERS_FROM32, null), new Attribute("char", Dict.VERS_HTML40, null), new Attribute("charoff", Dict.VERS_HTML40, null), new Attribute("charset", Dict.VERS_HTML40, null), new Attribute("checked", Dict.VERS_ALL, AttrCheckImpl.getCheckBool()), new Attribute("cite", Dict.VERS_HTML40, AttrCheckImpl.getCheckUrl()), new Attribute("class", Dict.VERS_HTML40, null), new Attribute("classid", Dict.VERS_HTML40, AttrCheckImpl.getCheckUrl()), new Attribute("clear", Dict.VERS_LOOSE, null), new Attribute("code", Dict.VERS_LOOSE, null), new Attribute("codebase", Dict.VERS_HTML40, AttrCheckImpl.getCheckUrl()), new Attribute("codetype", Dict.VERS_HTML40, null), new Attribute("color", Dict.VERS_LOOSE, null), new Attribute("cols", Dict.VERS_IFRAMES, null), new Attribute("colspan", Dict.VERS_FROM32, null), new Attribute("compact", Dict.VERS_ALL, AttrCheckImpl.getCheckBool()), new Attribute("content", Dict.VERS_ALL, null), new Attribute("coords", Dict.VERS_FROM32, null), new 
				Attribute("data", Dict.VERS_HTML40, AttrCheckImpl.getCheckUrl()), new Attribute("datafld", Dict.VERS_MICROSOFT, null), new Attribute("dataformatas", Dict.VERS_MICROSOFT, null), new Attribute("datapagesize", Dict.VERS_MICROSOFT, null), new Attribute("datasrc", Dict.VERS_MICROSOFT, AttrCheckImpl.getCheckUrl()), new Attribute("datetime", Dict.VERS_HTML40, null), new Attribute("declare", Dict.VERS_HTML40, AttrCheckImpl.getCheckBool()), new Attribute("defer", Dict.VERS_HTML40, AttrCheckImpl.getCheckBool()), new Attribute("dir", Dict.VERS_HTML40, null), new Attribute("disabled", Dict.VERS_HTML40, AttrCheckImpl.getCheckBool()), new Attribute("enctype", Dict.VERS_ALL, null), new Attribute("face", Dict.VERS_LOOSE, null), new Attribute("for", Dict.VERS_HTML40, null), new Attribute("frame", Dict.VERS_HTML40, null), new Attribute("frameborder", Dict.VERS_FRAMES, null), new Attribute("framespacing", Dict.VERS_PROPRIETARY, null), new Attribute("gridx", Dict.VERS_PROPRIETARY, null), new Attribute("gridy", Dict.VERS_PROPRIETARY, null), new Attribute("headers", Dict.VERS_HTML40, null), new Attribute("height", Dict.VERS_ALL, null), new Attribute("href", Dict.VERS_ALL, AttrCheckImpl.getCheckUrl()), new Attribute("hreflang", Dict.VERS_HTML40, null), new Attribute("hspace", Dict.VERS_ALL, null), new Attribute("http-equiv", Dict.VERS_ALL, null), new Attribute("id", Dict.VERS_HTML40, AttrCheckImpl.getCheckId()), new Attribute("ismap", Dict.VERS_ALL, AttrCheckImpl.getCheckBool()), new Attribute("label", Dict.VERS_HTML40, null), new Attribute("lang", Dict.VERS_HTML40, null), new Attribute("language", Dict.VERS_LOOSE, null), new Attribute("last_modified", Dict.VERS_NETSCAPE, null), new Attribute("last_visit", Dict.VERS_NETSCAPE, null), new Attribute("leftmargin", Dict.VERS_MICROSOFT, null), new Attribute("link", Dict.VERS_LOOSE, null), new Attribute("longdesc", Dict.VERS_HTML40, AttrCheckImpl.getCheckUrl()), new Attribute("lowsrc", Dict.VERS_PROPRIETARY, AttrCheckImpl.getCheckUrl()), new Attribute("marginheight", 
				Dict.VERS_IFRAMES, null), new Attribute("marginwidth", Dict.VERS_IFRAMES, null), new Attribute("maxlength", Dict.VERS_ALL, null), new Attribute("media", Dict.VERS_HTML40, null), new Attribute("method", Dict.VERS_ALL, null), new Attribute("multiple", Dict.VERS_ALL, AttrCheckImpl.getCheckBool()), new Attribute("name", Dict.VERS_ALL, AttrCheckImpl.getCheckName()), new Attribute("nohref", Dict.VERS_FROM32, AttrCheckImpl.getCheckBool()), new Attribute("noresize", Dict.VERS_FRAMES, AttrCheckImpl.getCheckBool()), new Attribute("noshade", Dict.VERS_LOOSE, AttrCheckImpl.getCheckBool()), new Attribute("nowrap", Dict.VERS_LOOSE, AttrCheckImpl.getCheckBool()), new Attribute("object", Dict.VERS_HTML40_LOOSE, null), new Attribute("onblur", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onchange", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onclick", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("ondblclick", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onkeydown", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onkeypress", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onkeyup", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onload", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onmousedown", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onmousemove", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onmouseout", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onmouseover", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onmouseup", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onsubmit", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onreset", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onselect", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript()), new Attribute("onunload", Dict.VERS_HTML40, AttrCheckImpl.getCheckScript
				()), new Attribute("onafterupdate", Dict.VERS_MICROSOFT, AttrCheckImpl.getCheckScript()), new Attribute("onbeforeupdate", Dict.VERS_MICROSOFT, AttrCheckImpl.getCheckScript()), new Attribute("onerrorupdate", Dict.VERS_MICROSOFT, AttrCheckImpl.getCheckScript()), new Attribute("onrowenter", Dict.VERS_MICROSOFT, AttrCheckImpl.getCheckScript()), new Attribute("onrowexit", Dict.VERS_MICROSOFT, AttrCheckImpl.getCheckScript()), new Attribute("onbeforeunload", Dict.VERS_MICROSOFT, AttrCheckImpl.getCheckScript()), new Attribute("ondatasetchanged", Dict.VERS_MICROSOFT, AttrCheckImpl.getCheckScript()), new Attribute("ondataavailable", Dict.VERS_MICROSOFT, AttrCheckImpl.getCheckScript()), new Attribute("ondatasetcomplete", Dict.VERS_MICROSOFT, AttrCheckImpl.getCheckScript()), new Attribute("profile", Dict.VERS_HTML40, AttrCheckImpl.getCheckUrl()), new Attribute("prompt", Dict.VERS_LOOSE, null), new Attribute("readonly", Dict.VERS_HTML40, AttrCheckImpl.getCheckBool()), new Attribute("rel", Dict.VERS_ALL, null), new Attribute("rev", Dict.VERS_ALL, null), new Attribute("rightmargin", Dict.VERS_MICROSOFT, null), new Attribute("rows", Dict.VERS_ALL, null), new Attribute("rowspan", Dict.VERS_ALL, null), new Attribute("rules", Dict.VERS_HTML40, null), new Attribute("scheme", Dict.VERS_HTML40, null), new Attribute("scope", Dict.VERS_HTML40, null), new Attribute("scrolling", Dict.VERS_IFRAMES, null), new Attribute("selected", Dict.VERS_ALL, AttrCheckImpl.getCheckBool()), new Attribute("shape", Dict.VERS_FROM32, null), new Attribute("showgrid", Dict.VERS_PROPRIETARY, AttrCheckImpl.getCheckBool()), new Attribute("showgridx", Dict.VERS_PROPRIETARY, AttrCheckImpl.getCheckBool()), new Attribute("showgridy", Dict.VERS_PROPRIETARY, AttrCheckImpl.getCheckBool()), new Attribute("size", Dict.VERS_LOOSE, null), new Attribute("span", Dict.VERS_HTML40, null), new Attribute("src", (short) (Dict.VERS_ALL | Dict.VERS_FRAMES), AttrCheckImpl.getCheckUrl()), new Attribute("standby", Dict.VERS_HTML40, null), new Attribute("start", 
				Dict.VERS_ALL, null), new Attribute("style", Dict.VERS_HTML40, null), new Attribute("summary", Dict.VERS_HTML40, null), new Attribute("tabindex", Dict.VERS_HTML40, null), new Attribute("target", Dict.VERS_HTML40, null), new Attribute("text", Dict.VERS_LOOSE, null), new Attribute("title", Dict.VERS_HTML40, null), new Attribute("topmargin", Dict.VERS_MICROSOFT, null), new Attribute("type", Dict.VERS_FROM32, null), new Attribute("usemap", Dict.VERS_ALL, AttrCheckImpl.getCheckBool()), new Attribute("valign", Dict.VERS_FROM32, AttrCheckImpl.getCheckValign()), new Attribute("value", Dict.VERS_ALL, null), new Attribute("valuetype", Dict.VERS_HTML40, null), new Attribute("version", Dict.VERS_ALL, null), new Attribute("vlink", Dict.VERS_LOOSE, null), new Attribute("vspace", Dict.VERS_LOOSE, null), new Attribute("width", Dict.VERS_ALL, null), new Attribute("wrap", Dict.VERS_NETSCAPE, null), new Attribute("xml:lang", Dict.VERS_XML, null), new Attribute("xmlns", Dict.VERS_ALL, null)};
		}
	}
}