/*
* @(#)Configuration.java   1.11 2000/08/16
*
*/


/*
Configuration files associate a property name with a value.
The format is that of a Java .properties file.*/
using System;
using System.Runtime.InteropServices;
namespace Comzept.Genesis.Tidy
{
	/// <summary> 
/// Read configuration file and manage configuration properties.
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
	[Serializable]
	public class Configuration
	{
		
		/// <summary>
        /// Character encoding
		/// </summary>
		public const int RAW = 0;
        /// <summary>
        /// Character encoding
        /// </summary>
        public const int ASCII = 1;
        /// <summary>
        /// Character encoding
        /// </summary>
        public const int LATIN1 = 2;
        /// <summary>
        /// Character encoding
        /// </summary>
        public const int UTF8 = 3;
        /// <summary>
        /// Character encoding
        /// </summary>
        public const int ISO2022 = 4;
        /// <summary>
        /// Character encoding
        /// </summary>
        public const int MACROMAN = 5;
		
		/// <summary>
        /// Mode controlling treatment of doctype
		/// </summary>
		public const int DOCTYPE_OMIT = 0;
        /// <summary>
        /// Mode controlling treatment of doctype
        /// </summary>
        public const int DOCTYPE_AUTO = 1;
        /// <summary>
        /// Mode controlling treatment of doctype
        /// </summary>
        public const int DOCTYPE_STRICT = 2;
        /// <summary>
        /// Mode controlling treatment of doctype
        /// </summary>
        public const int DOCTYPE_LOOSE = 3;
        /// <summary>
        /// Mode controlling treatment of doctype
        /// </summary>
        public const int DOCTYPE_USER = 4;
		
        /// <summary>
        /// default indentation
        /// </summary>
		protected internal int spaces = 2; /*  */
        /// <summary>
        /// default wrap margin
        /// </summary>
		protected internal int wraplen = 68; /*  */
        /// <summary>
        /// Default encoding is ASCII.
        /// </summary>
		protected internal int CharEncoding = ASCII;
        /// <summary>
        /// Default tab size (4).
        /// </summary>
		protected internal int tabsize = 4;
		
        /// <summary>
        /// see doctype property
        /// </summary>
		protected internal int docTypeMode = DOCTYPE_AUTO; /* see doctype property */
        /// <summary>
        /// default text for alt attribute
        /// </summary>
		protected internal System.String altText = null; /* default text for alt attribute */
        /// <summary>
        /// style sheet for slides
        /// </summary>
		protected internal System.String slidestyle = null; /* style sheet for slides */
        /// <summary>
        /// user specified doctype 
        /// </summary>
		protected internal System.String docTypeStr = null; /* user specified doctype */
        /// <summary>
        /// file name to write errors to 
        /// </summary>
		protected internal System.String errfile = null; /* file name to write errors to */
        /// <summary>
        /// if true then output tidied markup
        /// </summary>
		protected internal bool writeback = false; /* if true then output tidied markup */
		/// <summary>
        /// if true normal output is suppressed
		/// </summary>
		protected internal bool OnlyErrors = false; /* if true normal output is suppressed */
        /// <summary>
        /// however errors are always shown
        /// </summary>
		protected internal bool ShowWarnings = true; /* however errors are always shown */
        /// <summary>
        /// no 'Parsing X', guessed DTD or summary
        /// </summary>
		protected internal bool Quiet = false; /* no 'Parsing X', guessed DTD or summary */
        /// <summary>
        /// indent content of appropriate tags
        /// </summary>
		protected internal bool IndentContent = false; /* indent content of appropriate tags */
        /// <summary>
        /// does text/block level content effect indentation
        /// </summary>
		protected internal bool SmartIndent = false; /* does text/block level content effect indentation */
        /// <summary>
        /// suppress optional end tags
        /// </summary>
		protected internal bool HideEndTags = false; /* suppress optional end tags */
        /// <summary>
        /// treat input as XML
        /// </summary>
		protected internal bool XmlTags = false; /* treat input as XML */
        /// <summary>
        /// create output as XML
        /// </summary>
		protected internal bool XmlOut = false; /* create output as XML */
        /// <summary>
        /// output extensible HTML
        /// </summary>
		protected internal bool xHTML = false; /* output extensible HTML */
        /// <summary>
        ///  add &lt;?xml?&gt; for XML docs
        /// </summary>
		protected internal bool XmlPi = false; /* add <?xml?> for XML docs */
        /// <summary>
        /// avoid mapping values &gt; 127 to entities
        /// </summary>
		protected internal bool RawOut = false; /* avoid mapping values > 127 to entities */
        /// <summary>
        /// output tags in upper not lower case
        /// </summary>
		protected internal bool UpperCaseTags = false; /* output tags in upper not lower case */
        /// <summary>
        /// output attributes in upper not lower case
        /// </summary>
		protected internal bool UpperCaseAttrs = false; /* output attributes in upper not lower case */
        /// <summary>
        /// remove presentational clutter
        /// </summary>
		protected internal bool MakeClean = false; /* remove presentational clutter */
        /// <summary>
        /// replace i by em and b by strong
        /// </summary>
		protected internal bool LogicalEmphasis = false; /* replace i by em and b by strong */
        /// <summary>
        /// discard presentation tags
        /// </summary>
		protected internal bool DropFontTags = false; /* discard presentation tags */
        /// <summary>
        /// discard empty p elements
        /// </summary>
		protected internal bool DropEmptyParas = true; /* discard empty p elements */
        /// <summary>
        /// fix comments with adjacent hyphens
        /// </summary>
		protected internal bool FixComments = true; /* fix comments with adjacent hyphens */
        /// <summary>
        /// o/p newline before &lt;br/&gt; or not?
        /// </summary>
		protected internal bool BreakBeforeBR = false; /* o/p newline before <br/> or not? */
        /// <summary>
        /// create slides on each h2 element
        /// </summary>
		protected internal bool BurstSlides = false; /* create slides on each h2 element */
        /// <summary>
        /// use numeric entities
        /// </summary>
		protected internal bool NumEntities = false; /* use numeric entities */
        /// <summary>
        /// output " marks as &amp;quot;
        /// </summary>
		protected internal bool QuoteMarks = false; /* output " marks as &quot; */
        /// <summary>
        /// output non-breaking space as entity 
        /// </summary>
		protected internal bool QuoteNbsp = true; /* output non-breaking space as entity */
        /// <summary>
        /// output naked ampersand as &amp;amp;
        /// </summary>
		protected internal bool QuoteAmpersand = true; /* output naked ampersand as &amp; */
        /// <summary>
        /// wrap within attribute values
        /// </summary>
		protected internal bool WrapAttVals = false; /* wrap within attribute values */
        /// <summary>
        /// wrap within JavaScript string literals
        /// </summary>
		protected internal bool WrapScriptlets = false; /* wrap within JavaScript string literals */
        /// <summary>
        /// wrap within <![ ... ]> section tags
        /// </summary>
		protected internal bool WrapSection = true; /* wrap within <![ ... ]> section tags */
        /// <summary>
        /// wrap within ASP pseudo elements
        /// </summary>
		protected internal bool WrapAsp = true; /* wrap within ASP pseudo elements */
        /// <summary>
        /// wrap within JSTE pseudo elements
        /// </summary>
		protected internal bool WrapJste = true; /* wrap within JSTE pseudo elements */
        /// <summary>
        /// wrap within PHP pseudo elements
        /// </summary>
		protected internal bool WrapPhp = true; /* wrap within PHP pseudo elements */
        /// <summary>
        /// fix URLs by replacing \ with /
        /// </summary>
		protected internal bool FixBackslash = true; /* fix URLs by replacing \ with / */
		protected internal bool IndentAttributes = false; /* newline+indent before each attribute */
        /// <summary>
        /// if set to yes PIs must end with ?&gt;
        /// </summary>
		protected internal bool XmlPIs = false; /* if set to yes PIs must end with ?> */
        /// <summary>
        /// if set to yes adds xml:space attr as needed 
        /// </summary>
		protected internal bool XmlSpace = false; /* if set to yes adds xml:space attr as needed */
        /// <summary>
        ///  if yes text at body is wrapped in &lt;p&gt;'s
        /// </summary>
		protected internal bool EncloseBodyText = false; /* if yes text at body is wrapped in <p>'s */
        /// <summary>
        /// if yes text in blocks is wrapped in &lt;p&gt;'s
        /// </summary>
		protected internal bool EncloseBlockText = false; /* if yes text in blocks is wrapped in <p>'s */
        /// <summary>
        /// if yes last modied time is preserved
        /// </summary>
		protected internal bool KeepFileTimes = true; /* if yes last modied time is preserved */
        /// <summary>
        /// draconian cleaning for Word2000
        /// </summary>
		protected internal bool Word2000 = false; /* draconian cleaning for Word2000 */
        /// <summary>
        /// add meta element indicating tidied doc
        /// </summary>
		protected internal bool TidyMark = true; /* add meta element indicating tidied doc */
        /// <summary>
        /// if true format error output for GNU Emacs
        /// </summary>
		protected internal bool Emacs = false; /* if true format error output for GNU Emacs */
        /// <summary>
        /// if true attributes may use newlines
        /// </summary>
		protected internal bool LiteralAttribs = false; /* if true attributes may use newlines */
		/// <summary>
        /// TagTable associated with this Configuration.
		/// </summary>
		protected internal TagTable tt; /* TagTable associated with this Configuration */
		
		[NonSerialized]
		private System.Collections.Specialized.NameValueCollection _properties = new System.Collections.Specialized.NameValueCollection();
		
        /// <summary>
        /// Create a config instance.
        /// </summary>
		public Configuration()
		{
		}
		
        /// <summary>
        /// Add a property.
        /// </summary>
        /// <param name="p"></param>
		public virtual void  AddProps(System.Collections.Specialized.NameValueCollection p)
		{
			System.Collections.IEnumerator enum_Renamed = p.Keys.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			while (enum_Renamed.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				System.String key = (System.String) enum_Renamed.Current;
				System.String value_Renamed = p.Get(key);
				_properties[(System.String) key] = (System.String) value_Renamed;
			}
			parseProps();
		}
		
        /// <summary>
        /// Parse a configuration file.
        /// </summary>
        /// <param name="filename"></param>
		public virtual void ParseFile(System.String filename)
		{
			try
			{
				new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				_properties = new System.Collections.Specialized.NameValueCollection(System.Configuration.ConfigurationSettings.AppSettings);
			}
			catch (System.IO.IOException e)
			{
				System.Console.Error.WriteLine(filename + e.ToString());
				return ;
			}
			parseProps();
		}
		
		private void  parseProps()
		{
			System.String value_Renamed;
			
			value_Renamed = _properties.Get("indent-spaces");
			if (value_Renamed != null)
				spaces = parseInt(value_Renamed, "indent-spaces");
			
			value_Renamed = _properties.Get("wrap");
			if (value_Renamed != null)
				wraplen = parseInt(value_Renamed, "wrap");
			
			value_Renamed = _properties.Get("wrap-attributes");
			if (value_Renamed != null)
				WrapAttVals = parseBool(value_Renamed, "wrap-attributes");
			
			value_Renamed = _properties.Get("wrap-script-literals");
			if (value_Renamed != null)
				WrapScriptlets = parseBool(value_Renamed, "wrap-script-literals");
			
			value_Renamed = _properties.Get("wrap-sections");
			if (value_Renamed != null)
				WrapSection = parseBool(value_Renamed, "wrap-sections");
			
			value_Renamed = _properties.Get("wrap-asp");
			if (value_Renamed != null)
				WrapAsp = parseBool(value_Renamed, "wrap-asp");
			
			value_Renamed = _properties.Get("wrap-jste");
			if (value_Renamed != null)
				WrapJste = parseBool(value_Renamed, "wrap-jste");
			
			value_Renamed = _properties.Get("wrap-php");
			if (value_Renamed != null)
				WrapPhp = parseBool(value_Renamed, "wrap-php");
			
			value_Renamed = _properties.Get("literal-attributes");
			if (value_Renamed != null)
				LiteralAttribs = parseBool(value_Renamed, "literal-attributes");
			
			value_Renamed = _properties.Get("tab-size");
			if (value_Renamed != null)
				tabsize = parseInt(value_Renamed, "tab-size");
			
			value_Renamed = _properties.Get("markup");
			if (value_Renamed != null)
				OnlyErrors = parseInvBool(value_Renamed, "markup");
			
			value_Renamed = _properties.Get("quiet");
			if (value_Renamed != null)
				Quiet = parseBool(value_Renamed, "quiet");
			
			value_Renamed = _properties.Get("tidy-mark");
			if (value_Renamed != null)
				TidyMark = parseBool(value_Renamed, "tidy-mark");
			
			value_Renamed = _properties.Get("indent");
			if (value_Renamed != null)
				IndentContent = parseIndent(value_Renamed, "indent");
			
			value_Renamed = _properties.Get("indent-attributes");
			if (value_Renamed != null)
				IndentAttributes = parseBool(value_Renamed, "ident-attributes");
			
			value_Renamed = _properties.Get("hide-endtags");
			if (value_Renamed != null)
				HideEndTags = parseBool(value_Renamed, "hide-endtags");
			
			value_Renamed = _properties.Get("input-xml");
			if (value_Renamed != null)
				XmlTags = parseBool(value_Renamed, "input-xml");
			
			value_Renamed = _properties.Get("output-xml");
			if (value_Renamed != null)
				XmlOut = parseBool(value_Renamed, "output-xml");
			
			value_Renamed = _properties.Get("output-xhtml");
			if (value_Renamed != null)
				xHTML = parseBool(value_Renamed, "output-xhtml");
			
			value_Renamed = _properties.Get("add-xml-pi");
			if (value_Renamed != null)
				XmlPi = parseBool(value_Renamed, "add-xml-pi");
			
			value_Renamed = _properties.Get("add-xml-decl");
			if (value_Renamed != null)
				XmlPi = parseBool(value_Renamed, "add-xml-decl");
			
			value_Renamed = _properties.Get("assume-xml-procins");
			if (value_Renamed != null)
				XmlPIs = parseBool(value_Renamed, "assume-xml-procins");
			
			value_Renamed = _properties.Get("raw");
			if (value_Renamed != null)
				RawOut = parseBool(value_Renamed, "raw");
			
			value_Renamed = _properties.Get("uppercase-tags");
			if (value_Renamed != null)
				UpperCaseTags = parseBool(value_Renamed, "uppercase-tags");
			
			value_Renamed = _properties.Get("uppercase-attributes");
			if (value_Renamed != null)
				UpperCaseAttrs = parseBool(value_Renamed, "uppercase-attributes");
			
			value_Renamed = _properties.Get("clean");
			if (value_Renamed != null)
				MakeClean = parseBool(value_Renamed, "clean");
			
			value_Renamed = _properties.Get("logical-emphasis");
			if (value_Renamed != null)
				LogicalEmphasis = parseBool(value_Renamed, "logical-emphasis");
			
			value_Renamed = _properties.Get("word-2000");
			if (value_Renamed != null)
				Word2000 = parseBool(value_Renamed, "word-2000");
			
			value_Renamed = _properties.Get("drop-empty-paras");
			if (value_Renamed != null)
				DropEmptyParas = parseBool(value_Renamed, "drop-empty-paras");
			
			value_Renamed = _properties.Get("drop-font-tags");
			if (value_Renamed != null)
				DropFontTags = parseBool(value_Renamed, "drop-font-tags");
			
			value_Renamed = _properties.Get("enclose-text");
			if (value_Renamed != null)
				EncloseBodyText = parseBool(value_Renamed, "enclose-text");
			
			value_Renamed = _properties.Get("enclose-block-text");
			if (value_Renamed != null)
				EncloseBlockText = parseBool(value_Renamed, "enclose-block-text");
			
			value_Renamed = _properties.Get("alt-text");
			if (value_Renamed != null)
				altText = value_Renamed;
			
			value_Renamed = _properties.Get("add-xml-space");
			if (value_Renamed != null)
				XmlSpace = parseBool(value_Renamed, "add-xml-space");
			
			value_Renamed = _properties.Get("fix-bad-comments");
			if (value_Renamed != null)
				FixComments = parseBool(value_Renamed, "fix-bad-comments");
			
			value_Renamed = _properties.Get("split");
			if (value_Renamed != null)
				BurstSlides = parseBool(value_Renamed, "split");
			
			value_Renamed = _properties.Get("break-before-br");
			if (value_Renamed != null)
				BreakBeforeBR = parseBool(value_Renamed, "break-before-br");
			
			value_Renamed = _properties.Get("numeric-entities");
			if (value_Renamed != null)
				NumEntities = parseBool(value_Renamed, "numeric-entities");
			
			value_Renamed = _properties.Get("quote-marks");
			if (value_Renamed != null)
				QuoteMarks = parseBool(value_Renamed, "quote-marks");
			
			value_Renamed = _properties.Get("quote-nbsp");
			if (value_Renamed != null)
				QuoteNbsp = parseBool(value_Renamed, "quote-nbsp");
			
			value_Renamed = _properties.Get("quote-ampersand");
			if (value_Renamed != null)
				QuoteAmpersand = parseBool(value_Renamed, "quote-ampersand");
			
			value_Renamed = _properties.Get("write-back");
			if (value_Renamed != null)
				writeback = parseBool(value_Renamed, "write-back");
			
			value_Renamed = _properties.Get("keep-time");
			if (value_Renamed != null)
				KeepFileTimes = parseBool(value_Renamed, "keep-time");
			
			value_Renamed = _properties.Get("show-warnings");
			if (value_Renamed != null)
				ShowWarnings = parseBool(value_Renamed, "show-warnings");
			
			value_Renamed = _properties.Get("error-file");
			if (value_Renamed != null)
				errfile = parseName(value_Renamed, "error-file");
			
			value_Renamed = _properties.Get("slide-style");
			if (value_Renamed != null)
				slidestyle = parseName(value_Renamed, "slide-style");
			
			value_Renamed = _properties.Get("new-inline-tags");
			if (value_Renamed != null)
				parseInlineTagNames(value_Renamed, "new-inline-tags");
			
			value_Renamed = _properties.Get("new-blocklevel-tags");
			if (value_Renamed != null)
				parseBlockTagNames(value_Renamed, "new-blocklevel-tags");
			
			value_Renamed = _properties.Get("new-empty-tags");
			if (value_Renamed != null)
				parseEmptyTagNames(value_Renamed, "new-empty-tags");
			
			value_Renamed = _properties.Get("new-pre-tags");
			if (value_Renamed != null)
				parsePreTagNames(value_Renamed, "new-pre-tags");
			
			value_Renamed = _properties.Get("char-encoding");
			if (value_Renamed != null)
				CharEncoding = parseCharEncoding(value_Renamed, "char-encoding");
			
			value_Renamed = _properties.Get("doctype");
			if (value_Renamed != null)
				docTypeStr = ParseDocType(value_Renamed, "doctype");
			
			value_Renamed = _properties.Get("fix-backslash");
			if (value_Renamed != null)
				FixBackslash = parseBool(value_Renamed, "fix-backslash");
			
			value_Renamed = _properties.Get("gnu-emacs");
			if (value_Renamed != null)
				Emacs = parseBool(value_Renamed, "gnu-emacs");
		}
		
		/// <summary>
        /// Ensure that config is self consistent.
		/// </summary>
		public virtual void  Adjust()
		{
			if (EncloseBlockText)
				EncloseBodyText = true;
			
			/* avoid the need to set IndentContent when SmartIndent is set */
			
			if (SmartIndent)
				IndentContent = true;
			
			/* disable wrapping */
			if (wraplen == 0)
				wraplen = 0x7FFFFFFF;
			
			/* Word 2000 needs o:p to be declared as inline */
			if (Word2000)
			{
				tt.DefineInlineTag("o:p");
			}
			
			/* XHTML is written in lower case */
			if (xHTML)
			{
				XmlOut = true;
				UpperCaseTags = false;
				UpperCaseAttrs = false;
			}
			
			/* if XML in, then XML out */
			if (XmlTags)
			{
				XmlOut = true;
				XmlPIs = true;
			}
			
			/* XML requires end tags */
			if (XmlOut)
			{
				QuoteAmpersand = true;
				HideEndTags = false;
			}
		}
		
		private static int parseInt(System.String s, System.String option)
		{
			int i = 0;
			try
			{
				i = System.Int32.Parse(s);
			}
			catch (System.FormatException)
			{
				Report.badArgument(option);
				i = - 1;
			}
			return i;
		}
		
		private static bool parseBool(System.String s, System.String option)
		{
			bool b = false;
			if (s != null && s.Length > 0)
			{
				char c = s[0];
				if ((c == 't') || (c == 'T') || (c == 'Y') || (c == 'y') || (c == '1'))
					b = true;
				else if ((c == 'f') || (c == 'F') || (c == 'N') || (c == 'n') || (c == '0'))
					b = false;
				else
					Report.badArgument(option);
			}
			return b;
		}
		
		private static bool parseInvBool(System.String s, System.String option)
		{
			bool b = false;
			if (s != null && s.Length > 0)
			{
				char c = s[0];
				if ((c == 't') || (c == 'T') || (c == 'Y') || (c == 'y'))
					b = true;
				else if ((c == 'f') || (c == 'F') || (c == 'N') || (c == 'n'))
					b = false;
				else
					Report.badArgument(option);
			}
			return !b;
		}
		
		private static System.String parseName(System.String s, System.String option)
		{
			SupportClass.Tokenizer t = new SupportClass.Tokenizer(s);
			System.String rs = null;
			if (t.Count >= 1)
				rs = t.NextToken();
			else
				Report.badArgument(option);
			return rs;
		}
		
		private static int parseCharEncoding(System.String s, System.String option)
		{
			int result = ASCII;
			
			if (Lexer.wstrcasecmp(s, "ascii") == 0)
				result = ASCII;
			else if (Lexer.wstrcasecmp(s, "latin1") == 0)
				result = LATIN1;
			else if (Lexer.wstrcasecmp(s, "raw") == 0)
				result = RAW;
			else if (Lexer.wstrcasecmp(s, "utf8") == 0)
				result = UTF8;
			else if (Lexer.wstrcasecmp(s, "iso2022") == 0)
				result = ISO2022;
			else if (Lexer.wstrcasecmp(s, "mac") == 0)
				result = MACROMAN;
			else
				Report.badArgument(option);
			
			return result;
		}
		
		/* slight hack to avoid changes to pprint.c */
		private bool parseIndent(System.String s, System.String option)
		{
			bool b = IndentContent;
			
			if (Lexer.wstrcasecmp(s, "yes") == 0)
			{
				b = true;
				SmartIndent = false;
			}
			else if (Lexer.wstrcasecmp(s, "true") == 0)
			{
				b = true;
				SmartIndent = false;
			}
			else if (Lexer.wstrcasecmp(s, "no") == 0)
			{
				b = false;
				SmartIndent = false;
			}
			else if (Lexer.wstrcasecmp(s, "false") == 0)
			{
				b = false;
				SmartIndent = false;
			}
			else if (Lexer.wstrcasecmp(s, "auto") == 0)
			{
				b = true;
				SmartIndent = true;
			}
			else
				Report.badArgument(option);
			return b;
		}
		
		private void  parseInlineTagNames(System.String s, System.String option)
		{
			SupportClass.Tokenizer t = new SupportClass.Tokenizer(s, " \t\n\r,");
			while (t.HasMoreTokens())
			{
				tt.DefineInlineTag(t.NextToken());
			}
		}
		
		private void  parseBlockTagNames(System.String s, System.String option)
		{
			SupportClass.Tokenizer t = new SupportClass.Tokenizer(s, " \t\n\r,");
			while (t.HasMoreTokens())
			{
				tt.DefineBlockTag(t.NextToken());
			}
		}
		
		private void  parseEmptyTagNames(System.String s, System.String option)
		{
			SupportClass.Tokenizer t = new SupportClass.Tokenizer(s, " \t\n\r,");
			while (t.HasMoreTokens())
			{
				tt.DefineEmptyTag(t.NextToken());
			}
		}
		
		private void  parsePreTagNames(System.String s, System.String option)
		{
			SupportClass.Tokenizer t = new SupportClass.Tokenizer(s, " \t\n\r,");
			while (t.HasMoreTokens())
			{
				tt.DefinePreTag(t.NextToken());
			}
		}
		
        /// <summary>
        /// doctype: omit | auto | strict | loose | &lt;fpi&gt;, where the fpi is a string similar to "-//ACME//DTD HTML 3.14159//EN".
        /// </summary>
        /// <param name="s"></param>
        /// <param name="option"></param>
        /// <returns></returns>
		protected internal virtual System.String ParseDocType(System.String s, System.String option)
		{
			s = s.Trim();
			
			/* "-//ACME//DTD HTML 3.14159//EN" or similar */
			
			if (s.StartsWith("\""))
			{
				docTypeMode = DOCTYPE_USER;
				return s;
			}
			
			/* read first word */
			System.String word = "";
			SupportClass.Tokenizer t = new SupportClass.Tokenizer(s, " \t\n\r,");
			if (t.HasMoreTokens())
				word = t.NextToken();
			
			if (Lexer.wstrcasecmp(word, "omit") == 0)
				docTypeMode = DOCTYPE_OMIT;
			else if (Lexer.wstrcasecmp(word, "strict") == 0)
				docTypeMode = DOCTYPE_STRICT;
			else if (Lexer.wstrcasecmp(word, "loose") == 0 || Lexer.wstrcasecmp(word, "transitional") == 0)
				docTypeMode = DOCTYPE_LOOSE;
			else if (Lexer.wstrcasecmp(word, "auto") == 0)
				docTypeMode = DOCTYPE_AUTO;
			else
			{
				docTypeMode = DOCTYPE_AUTO;
				Report.badArgument(option);
			}
			return null;
		}
	}
}