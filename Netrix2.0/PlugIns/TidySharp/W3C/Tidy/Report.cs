/*
* @(#)Report.java   1.11 2000/08/16
*
*/

using System;
namespace Comzept.Genesis.Tidy
{
	
	/// <summary> 
/// Error/informational message reporter.
/// 
/// You should only need to edit the file TidyMessages.properties
/// to localize HTML tidy.
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
    public class Report
	{
		
		/* used to point to Web Accessibility Guidelines */
		public const System.String ACCESS_URL = "http://www.w3.org/WAI/GL";
		
		public const System.String RELEASE_DATE = "4th August 2000";
		
		public static System.String currentFile; /* sasdjb 01May00 for GNU Emacs error parsing */
		
		/* error codes for entities */
		
		public const short MISSING_SEMICOLON = 1;
		public const short UNKNOWN_ENTITY = 2;
		public const short UNESCAPED_AMPERSAND = 3;
		
		/* error codes for element messages */
		
		public const short MISSING_ENDTAG_FOR = 1;
		public const short MISSING_ENDTAG_BEFORE = 2;
		public const short DISCARDING_UNEXPECTED = 3;
		public const short NESTED_EMPHASIS = 4;
		public const short NON_MATCHING_ENDTAG = 5;
		public const short TAG_NOT_ALLOWED_IN = 6;
		public const short MISSING_STARTTAG = 7;
		public const short UNEXPECTED_ENDTAG = 8;
		public const short USING_BR_INPLACE_OF = 9;
		public const short INSERTING_TAG = 10;
		public const short SUSPECTED_MISSING_QUOTE = 11;
		public const short MISSING_TITLE_ELEMENT = 12;
		public const short DUPLICATE_FRAMESET = 13;
		public const short CANT_BE_NESTED = 14;
		public const short OBSOLETE_ELEMENT = 15;
		public const short PROPRIETARY_ELEMENT = 16;
		public const short UNKNOWN_ELEMENT = 17;
		public const short TRIM_EMPTY_ELEMENT = 18;
		public const short COERCE_TO_ENDTAG = 19;
		public const short ILLEGAL_NESTING = 20;
		public const short NOFRAMES_CONTENT = 21;
		public const short CONTENT_AFTER_BODY = 22;
		public const short INCONSISTENT_VERSION = 23;
		public const short MALFORMED_COMMENT = 24;
		public const short BAD_COMMENT_CHARS = 25;
		public const short BAD_XML_COMMENT = 26;
		public const short BAD_CDATA_CONTENT = 27;
		public const short INCONSISTENT_NAMESPACE = 28;
		public const short DOCTYPE_AFTER_TAGS = 29;
		public const short MALFORMED_DOCTYPE = 30;
		public const short UNEXPECTED_END_OF_FILE = 31;
		public const short DTYPE_NOT_UPPER_CASE = 32;
		public const short TOO_MANY_ELEMENTS = 33;
		
		/* error codes used for attribute messages */
		
		public const short UNKNOWN_ATTRIBUTE = 1;
		public const short MISSING_ATTRIBUTE = 2;
		public const short MISSING_ATTR_VALUE = 3;
		public const short BAD_ATTRIBUTE_VALUE = 4;
		public const short UNEXPECTED_GT = 5;
		public const short PROPRIETARY_ATTR_VALUE = 6;
		public const short REPEATED_ATTRIBUTE = 7;
		public const short MISSING_IMAGEMAP = 8;
		public const short XML_ATTRIBUTE_VALUE = 9;
		public const short UNEXPECTED_QUOTEMARK = 10;
		public const short ID_NAME_MISMATCH = 11;
		
		/* accessibility flaws */
		
		public const short MISSING_IMAGE_ALT = 1;
		public const short MISSING_LINK_ALT = 2;
		public const short MISSING_SUMMARY = 4;
		public const short MISSING_IMAGE_MAP = 8;
		public const short USING_FRAMES = 16;
		public const short USING_NOFRAMES = 32;
		
		/* presentation flaws */
		
		public const short USING_SPACER = 1;
		public const short USING_LAYER = 2;
		public const short USING_NOBR = 4;
		public const short USING_FONT = 8;
		public const short USING_BODY = 16;
		
		/* character encoding errors */
		public const short WINDOWS_CHARS = 1;
		public const short NON_ASCII = 2;
		public const short FOUND_UTF16 = 4;
		
		private static short optionerrors;
		
		private static System.Resources.ResourceManager res = null;
		
		public static void  tidyPrint(System.IO.StreamWriter p, System.String msg)
		{
			p.Write(msg);
		}
		
		public static void  tidyPrintln(System.IO.StreamWriter p, System.String msg)
		{
			//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
			p.WriteLine(msg);
		}
		
		public static void  tidyPrintln(System.IO.StreamWriter p)
		{
			//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln'"
			p.WriteLine();
		}
		
		public static void  showVersion(System.IO.StreamWriter p)
		{
			tidyPrintln(p, "Java HTML Tidy release date: " + RELEASE_DATE);
			tidyPrintln(p, "See http://www.w3.org/People/Raggett for details");
		}
		
		public static void  tag(Lexer lexer, Node tag)
		{
			if (tag != null)
			{
				if (tag.type == Node.StartTag)
					tidyPrint(lexer.errout, "<" + tag.element + ">");
				else if (tag.type == Node.EndTag)
					tidyPrint(lexer.errout, "</" + tag.element + ">");
				else if (tag.type == Node.DocTypeTag)
					tidyPrint(lexer.errout, "<!DOCTYPE>");
				else if (tag.type == Node.TextNode)
					tidyPrint(lexer.errout, "plain text");
				else
					tidyPrint(lexer.errout, tag.element);
			}
		}
		
		/* lexer is not defined when this is called */
		public static void  unknownOption(System.String option)
		{
			optionerrors++;
			try
			{
				//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				System.Console.Error.WriteLine(System.String.Format(res.GetString("unknown_option"), new System.Object[]{option}));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Error.WriteLine(e.ToString());
			}
		}
		
		/* lexer is not defined when this is called */
		public static void  badArgument(System.String option)
		{
			optionerrors++;
			try
			{
				//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				System.Console.Error.WriteLine(System.String.Format(res.GetString("bad_argument"), new System.Object[]{option}));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Error.WriteLine(e.ToString());
			}
		}
		
		
		public static void  position(Lexer lexer)
		{
			try
			{
				/* Change formatting to be parsable by GNU Emacs */
				if (lexer.configuration.Emacs)
				{
					//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(lexer.errout, System.String.Format(res.GetString("emacs_format"), new System.Object[]{currentFile, (System.Int32) lexer.lines, (System.Int32) lexer.columns}));
					tidyPrint(lexer.errout, " ");
				}
				/* traditional format */
				else
				{
					//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(lexer.errout, System.String.Format(res.GetString("line_column"), new System.Object[]{(System.Int32) lexer.lines, (System.Int32) lexer.columns}));
				}
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				lexer.errout.WriteLine(e.ToString());
			}
		}
		
		public static void  encodingError(Lexer lexer, short code, int c)
		{
			lexer.warnings++;
			
			if (lexer.configuration.ShowWarnings)
			{
				position(lexer);
				
				if (code == WINDOWS_CHARS)
				{
					lexer.badChars |= WINDOWS_CHARS;
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("illegal_char"), new System.Object[]{(System.Int32) c}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				tidyPrintln(lexer.errout);
			}
		}
		
		public static void  entityError(Lexer lexer, short code, System.String entity, int c)
		{
			lexer.warnings++;
			
			if (lexer.configuration.ShowWarnings)
			{
				position(lexer);
				
				
				if (code == MISSING_SEMICOLON)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("missing_semicolon"), new System.Object[]{entity}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == UNKNOWN_ENTITY)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("unknown_entity"), new System.Object[]{entity}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == UNESCAPED_AMPERSAND)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("unescaped_ampersand"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				tidyPrintln(lexer.errout);
			}
		}
		
		public static void  attrError(Lexer lexer, Node node, System.String attr, short code)
		{
			lexer.warnings++;
			
			/* keep quiet after 6 errors */
			if (lexer.errors > 6)
				return ;
			
			if (lexer.configuration.ShowWarnings)
			{
				/* on end of file adjust reported position to end of input */
				if (code == UNEXPECTED_END_OF_FILE)
				{
					lexer.lines = lexer.in_Renamed.curline;
					lexer.columns = lexer.in_Renamed.curcol;
				}
				
				position(lexer);
				
				if (code == UNKNOWN_ATTRIBUTE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("unknown_attribute"), new System.Object[]{attr}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == MISSING_ATTRIBUTE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
						tag(lexer, node);
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("missing_attribute"), new System.Object[]{attr}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == MISSING_ATTR_VALUE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
						tag(lexer, node);
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("missing_attr_value"), new System.Object[]{attr}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == MISSING_IMAGEMAP)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
						tag(lexer, node);
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("missing_imagemap"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					lexer.badAccess |= MISSING_IMAGE_MAP;
				}
				else if (code == BAD_ATTRIBUTE_VALUE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
						tag(lexer, node);
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("bad_attribute_value"), new System.Object[]{attr}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == XML_ATTRIBUTE_VALUE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
						tag(lexer, node);
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("xml_attribute_value"), new System.Object[]{attr}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == UNEXPECTED_GT)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("error"));
						tag(lexer, node);
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("unexpected_gt"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					lexer.errors++; ;
				}
				else if (code == UNEXPECTED_QUOTEMARK)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
						tag(lexer, node);
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("unexpected_quotemark"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == REPEATED_ATTRIBUTE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
						tag(lexer, node);
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("repeated_attribute"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == PROPRIETARY_ATTR_VALUE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
						tag(lexer, node);
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("proprietary_attr_value"), new System.Object[]{attr}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == UNEXPECTED_END_OF_FILE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("unexpected_end_of_file"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == ID_NAME_MISMATCH)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
						tag(lexer, node);
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("id_name_mismatch"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				tidyPrintln(lexer.errout);
			}
			else if (code == UNEXPECTED_GT)
			{
				position(lexer);
				try
				{
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(lexer.errout, res.GetString("error"));
					tag(lexer, node);
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(lexer.errout, res.GetString("unexpected_gt"));
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					lexer.errout.WriteLine(e.ToString());
				}
				tidyPrintln(lexer.errout);
				lexer.errors++; ;
			}
		}
		
        /// <summary>
        /// Create a warning
        /// </summary>
        /// <param name="lexer"></param>
        /// <param name="element"></param>
        /// <param name="node"></param>
        /// <param name="code"></param>
		public static void Warning(Lexer lexer, Node element, Node node, short code)
		{
			
			TagTable tt = lexer.configuration.tt;
			
			lexer.warnings++;
			
			/* keep quiet after 6 errors */
			if (lexer.errors > 6)
				return ;
			
			if (lexer.configuration.ShowWarnings)
			{
				/* on end of file adjust reported position to end of input */
				if (code == UNEXPECTED_END_OF_FILE)
				{
					lexer.lines = lexer.in_Renamed.curline;
					lexer.columns = lexer.in_Renamed.curcol;
				}
				
				position(lexer);
				
				if (code == MISSING_ENDTAG_FOR)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("missing_endtag_for"), new System.Object[]{element.element}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == MISSING_ENDTAG_BEFORE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("missing_endtag_before"), new System.Object[]{element.element}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, node);
				}
				else if (code == DISCARDING_UNEXPECTED)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("discarding_unexpected"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, node);
				}
				else if (code == NESTED_EMPHASIS)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("nested_emphasis"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, node);
				}
				else if (code == COERCE_TO_ENDTAG)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("coerce_to_endtag"), new System.Object[]{element.element}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == NON_MATCHING_ENDTAG)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("non_matching_endtag_1"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, node);
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("non_matching_endtag_2"), new System.Object[]{element.element}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == TAG_NOT_ALLOWED_IN)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, node);
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("tag_not_allowed_in"), new System.Object[]{element.element}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == DOCTYPE_AFTER_TAGS)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("doctype_after_tags"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == MISSING_STARTTAG)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("missing_starttag"), new System.Object[]{node.element}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == UNEXPECTED_ENDTAG)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("unexpected_endtag"), new System.Object[]{node.element}));
						if (element != null)
						{
							//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
							//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
							tidyPrint(lexer.errout, System.String.Format(res.GetString("unexpected_endtag_suffix"), new System.Object[]{element.element}));
						}
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == TOO_MANY_ELEMENTS)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("too_many_elements"), new System.Object[]{node.element}));
						if (element != null)
						{
							//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
							//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
							tidyPrint(lexer.errout, System.String.Format(res.GetString("too_many_elements_suffix"), new System.Object[]{element.element}));
						}
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == USING_BR_INPLACE_OF)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("using_br_inplace_of"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, node);
				}
				else if (code == INSERTING_TAG)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("inserting_tag"), new System.Object[]{node.element}));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == CANT_BE_NESTED)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, node);
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("cant_be_nested"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == PROPRIETARY_ELEMENT)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, node);
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("proprietary_element"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					
					if (node.tag == tt.tagLayer)
						lexer.badLayout |= USING_LAYER;
					else if (node.tag == tt.tagSpacer)
						lexer.badLayout |= USING_SPACER;
					else if (node.tag == tt.tagNobr)
						lexer.badLayout |= USING_NOBR;
				}
				else if (code == OBSOLETE_ELEMENT)
				{
					try
					{
						if (element.tag != null && (element.tag.model & Dict.CM_OBSOLETE) != 0)
						{
							//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
							tidyPrint(lexer.errout, res.GetString("obsolete_element"));
						}
						else
						{
							//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
							tidyPrint(lexer.errout, res.GetString("replacing_element"));
						}
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, element);
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("by"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, node);
				}
				else if (code == TRIM_EMPTY_ELEMENT)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("trim_empty_element"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, element);
				}
				else if (code == MISSING_TITLE_ELEMENT)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("missing_title_element"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == ILLEGAL_NESTING)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, element);
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("illegal_nesting"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == NOFRAMES_CONTENT)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("warning"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, node);
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("noframes_content"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == INCONSISTENT_VERSION)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("inconsistent_version"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == MALFORMED_DOCTYPE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("malformed_doctype"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == CONTENT_AFTER_BODY)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("content_after_body"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == MALFORMED_COMMENT)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("malformed_comment"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == BAD_COMMENT_CHARS)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("bad_comment_chars"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == BAD_XML_COMMENT)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("bad_xml_comment"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == BAD_CDATA_CONTENT)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("bad_cdata_content"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == INCONSISTENT_NAMESPACE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("inconsistent_namespace"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == DTYPE_NOT_UPPER_CASE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("dtype_not_upper_case"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				else if (code == UNEXPECTED_END_OF_FILE)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("unexpected_end_of_file"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
					tag(lexer, element);
				}
				
				tidyPrintln(lexer.errout);
			}
		}
		
		public static void  error(Lexer lexer, Node element, Node node, short code)
		{
			lexer.warnings++;
			
			/* keep quiet after 6 errors */
			if (lexer.errors > 6)
				return ;
			
			lexer.errors++;
			
			position(lexer);
			
			if (code == SUSPECTED_MISSING_QUOTE)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(lexer.errout, res.GetString("suspected_missing_quote"));
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					lexer.errout.WriteLine(e.ToString());
				}
			}
			else if (code == DUPLICATE_FRAMESET)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(lexer.errout, res.GetString("duplicate_frameset"));
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					lexer.errout.WriteLine(e.ToString());
				}
			}
			else if (code == UNKNOWN_ELEMENT)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(lexer.errout, res.GetString("error"));
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					lexer.errout.WriteLine(e.ToString());
				}
				tag(lexer, node);
				try
				{
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(lexer.errout, res.GetString("unknown_element"));
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					lexer.errout.WriteLine(e.ToString());
				}
			}
			else if (code == UNEXPECTED_ENDTAG)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(lexer.errout, System.String.Format(res.GetString("unexpected_endtag"), new System.Object[]{node.element}));
					if (element != null)
					{
						//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, System.String.Format(res.GetString("unexpected_endtag_suffix"), new System.Object[]{element.element}));
					}
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					lexer.errout.WriteLine(e.ToString());
				}
			}
			
			tidyPrintln(lexer.errout);
		}
		
		public static void  errorSummary(Lexer lexer)
		{
			/* adjust badAccess to that its null if frames are ok */
			if ((lexer.badAccess & (USING_FRAMES | USING_NOFRAMES)) != 0)
			{
				if (!(((lexer.badAccess & USING_FRAMES) != 0) && ((lexer.badAccess & USING_NOFRAMES) == 0)))
					lexer.badAccess &= ~ (USING_FRAMES | USING_NOFRAMES);
			}
			
			if (lexer.badChars != 0)
			{
				if ((lexer.badChars & WINDOWS_CHARS) != 0)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("badchars_summary"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
			}
			
			if (lexer.badForm != 0)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(lexer.errout, res.GetString("badform_summary"));
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					lexer.errout.WriteLine(e.ToString());
				}
			}
			
			if (lexer.badAccess != 0)
			{
				if ((lexer.badAccess & MISSING_SUMMARY) != 0)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("badaccess_missing_summary"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				if ((lexer.badAccess & MISSING_IMAGE_ALT) != 0)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("badaccess_missing_image_alt"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				if ((lexer.badAccess & MISSING_IMAGE_MAP) != 0)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("badaccess_missing_image_map"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				if ((lexer.badAccess & MISSING_LINK_ALT) != 0)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("badaccess_missing_link_alt"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				if (((lexer.badAccess & USING_FRAMES) != 0) && ((lexer.badAccess & USING_NOFRAMES) == 0))
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("badaccess_frames"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				try
				{
					//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(lexer.errout, System.String.Format(res.GetString("badaccess_summary"), new System.Object[]{ACCESS_URL}));
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					lexer.errout.WriteLine(e.ToString());
				}
			}
			
			if (lexer.badLayout != 0)
			{
				if ((lexer.badLayout & USING_LAYER) != 0)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("badlayout_using_layer"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				if ((lexer.badLayout & USING_SPACER) != 0)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("badlayout_using_spacer"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				if ((lexer.badLayout & USING_FONT) != 0)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("badlayout_using_font"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				if ((lexer.badLayout & USING_NOBR) != 0)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("badlayout_using_nobr"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
				
				if ((lexer.badLayout & USING_BODY) != 0)
				{
					try
					{
						//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
						tidyPrint(lexer.errout, res.GetString("badlayout_using_body"));
					}
					catch (System.Resources.MissingManifestResourceException e)
					{
						//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
						//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
						lexer.errout.WriteLine(e.ToString());
					}
				}
			}
		}
		
		public static void  unknownOption(System.IO.StreamWriter errout, char c)
		{
			try
			{
				//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				tidyPrintln(errout, System.String.Format(res.GetString("unrecognized_option"), new System.Object[]{new System.String(new char[]{c})}));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errout.WriteLine(e.ToString());
			}
		}
		
		public static void  unknownFile(System.IO.StreamWriter errout, System.String program, System.String file)
		{
			try
			{
				//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				tidyPrintln(errout, System.String.Format(res.GetString("unknown_file"), new System.Object[]{program, file}));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errout.WriteLine(e.ToString());
			}
		}
		
		public static void  needsAuthorIntervention(System.IO.StreamWriter errout)
		{
			try
			{
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				tidyPrintln(errout, res.GetString("needs_author_intervention"));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errout.WriteLine(e.ToString());
			}
		}
		
		public static void  missingBody(System.IO.StreamWriter errout)
		{
			try
			{
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				tidyPrintln(errout, res.GetString("missing_body"));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errout.WriteLine(e.ToString());
			}
		}
		
		public static void  reportNumberOfSlides(System.IO.StreamWriter errout, int count)
		{
			try
			{
				//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				tidyPrintln(errout, System.String.Format(res.GetString("slides_found"), new System.Object[]{(System.Int32) count}));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errout.WriteLine(e.ToString());
			}
		}
		
		public static void  generalInfo(System.IO.StreamWriter errout)
		{
			try
			{
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				tidyPrintln(errout, res.GetString("general_info"));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errout.WriteLine(e.ToString());
			}
		}
		
		public static void  helloMessage(System.IO.StreamWriter errout, System.String date, System.String filename)
		{
			currentFile = filename; /* for use with Gnu Emacs */
			
			try
			{
				//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				tidyPrintln(errout, System.String.Format(res.GetString("hello_message"), new System.Object[]{date, filename}));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errout.WriteLine(e.ToString());
			}
		}
		
		public static void  reportVersion(System.IO.StreamWriter errout, Lexer lexer, System.String filename, Node doctype)
		{
			int i, c;
			int state = 0;
			System.String vers = lexer.HTMLVersionName();
			MutableInteger cc = new MutableInteger();
			
			try
			{
				if (doctype != null)
				{
					//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrint(errout, System.String.Format(res.GetString("doctype_given"), new System.Object[]{filename}));
					
					for (i = doctype.start; i < doctype.end; ++i)
					{
						c = (int) doctype.textarray[i];
						
						/* look for UTF-8 multibyte character */
						if (c < 0)
						{
							i += PPrint.GetUTF8(doctype.textarray, i, cc);
							c = cc.value_Renamed;
						}
						
						if (c == (char) '"')
							++state;
						else if (state == 1)
							errout.Write((char) c);
					}
					
					errout.Write('"');
				}
				
				//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				tidyPrintln(errout, System.String.Format(res.GetString("report_version"), new System.Object[]{filename, (vers != null?vers:"HTML proprietary")}));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errout.WriteLine(e.ToString());
			}
		}
		
		public static void  reportNumWarnings(System.IO.StreamWriter errout, Lexer lexer)
		{
			if (lexer.warnings > 0)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrintln(errout, System.String.Format(res.GetString("num_warnings"), new System.Object[]{(System.Int32) lexer.warnings}));
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					errout.WriteLine(e.ToString());
				}
			}
			else
			{
				try
				{
					//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
					tidyPrintln(errout, res.GetString("no_warnings"));
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					errout.WriteLine(e.ToString());
				}
			}
		}
		
		public static void  helpText(System.IO.StreamWriter out_Renamed, System.String prog)
		{
			try
			{
				//UPGRADE_TODO: Method 'java.text.MessageFormat.format' was converted to 'System.String.Format' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073'"
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				tidyPrintln(out_Renamed, System.String.Format(res.GetString("help_text"), new System.Object[]{prog, RELEASE_DATE}));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				out_Renamed.WriteLine(e.ToString());
			}
		}
		
		public static void  badTree(System.IO.StreamWriter errout)
		{
			try
			{
				//UPGRADE_TODO: Method 'java.util.ResourceBundle.getString' was converted to 'System.Resources.ResourceManager.GetString()' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilResourceBundlegetString_javalangString'"
				tidyPrintln(errout, res.GetString("bad_tree"));
			}
			catch (System.Resources.MissingManifestResourceException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				errout.WriteLine(e.ToString());
			}
		}
		static Report()
		{
			{
				try
				{
					//UPGRADE_TODO: Make sure that resources used in this class are valid resource files. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1078'"
					res = System.Resources.ResourceManager.CreateFileBasedResourceManager("org/w3c/tidy/TidyMessages", "", null);
				}
				catch (System.Resources.MissingManifestResourceException e)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					throw new System.ApplicationException(e.ToString());
				}
			}
		}
	}
}