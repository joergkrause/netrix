/*
* @(#)Tidy.java   1.11 2000/08/16
*
*/

/*
HTML parser and pretty printer

Copyright (c) 1998-2000 World Wide Web Consortium (Massachusetts
Institute of Technology, Institut National de Recherche en
Informatique et en Automatique, Keio University). All Rights
Reserved.

Contributing Author(s):

Dave Raggett dsr@w3.org
Andy Quick ac.quick@sympatico.ca (translation to Java)

The contributing author(s) would like to thank all those who
helped with testing, bug fixes, and patience.  This wouldn't
have been possible without all of you.

COPYRIGHT NOTICE:

This software and documentation is provided "as is," and
the copyright holders and contributing author(s) make no
representations or warranties, express or implied, including
but not limited to, warranties of merchantability or fitness
for any particular purpose or that the use of the software or
documentation will not infringe any third party patents,
copyrights, trademarks or other rights. 

The copyright holders and contributing author(s) will not be
liable for any direct, indirect, special or consequential damages
arising out of any use of the software or documentation, even if
advised of the possibility of such damage.

Permission is hereby granted to use, copy, modify, and distribute
this source code, or portions hereof, documentation and executables,
for any purpose, without fee, subject to the following restrictions:

1. The origin of this source code must not be misrepresented.
2. Altered versions must be plainly marked as such and must
not be misrepresented as being the original source.
3. This Copyright notice may not be removed or altered from any
source or altered source distribution.

The copyright holders and contributing author(s) specifically
permit, without fee, and encourage the use of this source code
as a component for supporting the Hypertext Markup Language in
commercial products. If you use this source code in a product,
acknowledgment is not required but would be appreciated.*/
using System;
using Comzept.Genesis.Tidy.Xml.Dom;
namespace Comzept.Genesis.Tidy
{
	
	/// <summary> 
	/// HTML parser and pretty printer.
	/// </summary>
    /// <remarks>
	/// <p>
	/// (c) 1998-2000 (W3C) MIT, INRIA, Keio University
	/// See Tidy.java for the copyright notice.
	/// Derived from <a href="http://www.w3.org/People/Raggett/tidy">
	/// HTML Tidy Release 4 Aug 2000</a>
	/// </p>
	/// 
	/// <p>
	/// Copyright (c) 1998-2000 World Wide Web Consortium (Massachusetts
	/// Institute of Technology, Institut National de Recherche en
	/// Informatique et en Automatique, Keio University). All Rights
	/// Reserved.
	/// </p>
	/// 
	/// <p>
	/// Contributing Author(s):<br/>
	/// <a href="mailto:dsr@w3.org">Dave Raggett</a><br/>
	/// <a href="mailto:ac.quick@sympatico.ca">Andy Quick</a> (translation to Java)
	/// </p>
	/// 
	/// <p>
	/// The contributing author(s) would like to thank all those who
	/// helped with testing, bug fixes, and patience.  This wouldn't
	/// have been possible without all of you.
	/// </p>
	/// 
	/// <p>
	/// COPYRIGHT NOTICE:<br/>
	/// 
	/// This software and documentation is provided "as is," and
	/// the copyright holders and contributing author(s) make no
	/// representations or warranties, express or implied, including
	/// but not limited to, warranties of merchantability or fitness
	/// for any particular purpose or that the use of the software or
	/// documentation will not infringe any third party patents,
	/// copyrights, trademarks or other rights. 
	/// </p>
	/// 
	/// <p>
	/// The copyright holders and contributing author(s) will not be
	/// liable for any direct, indirect, special or consequential damages
	/// arising out of any use of the software or documentation, even if
	/// advised of the possibility of such damage.
	/// </p>
	/// 
	/// <p>
	/// Permission is hereby granted to use, copy, modify, and distribute
	/// this source code, or portions hereof, documentation and executables,
	/// for any purpose, without fee, subject to the following restrictions:
	/// </p>
	/// 
	/// <p>
	/// <ol>
	/// <li>The origin of this source code must not be misrepresented.</li>
	/// <li>Altered versions must be plainly marked as such and must
	/// not be misrepresented as being the original source.</li>
	/// <li>This Copyright notice may not be removed or altered from any
	/// source or altered source distribution.</li>
	/// </ol>
	/// </p>
	/// 
	/// <p>
	/// The copyright holders and contributing author(s) specifically
	/// permit, without fee, and encourage the use of this source code
	/// as a component for supporting the Hypertext Markup Language in
	/// commercial products. If you use this source code in a product,
	/// acknowledgment is not required but would be appreciated.
	/// </p>
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
	/// 
	/// </version>	
	public class Tidy
	{
		virtual public System.IO.StreamWriter Stderr
		{
			get
			{
				return stderr;
			}
			
		}
		/// <summary> ParseErrors - the number of errors that occurred in the most
		/// recent parse operation
		/// </summary>
		virtual public int ParseErrors
		{
			
			
			get
			{
				return parseErrors;
			}
			
		}
		/// <summary> ParseWarnings - the number of warnings that occurred in the most
		/// recent parse operation
		/// </summary>
		virtual public int ParseWarnings
		{
			
			
			get
			{
				return parseWarnings;
			}
			
		}
		/// <summary> Errout - the error output stream</summary>
		virtual public System.IO.StreamWriter Errout
		{
			
			
			get
			{
				return errout;
			}
			
			set
			{
				this.errout = value;
			}
			
		}
		/// <summary> Spaces - default indentation</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.spaces">
		/// </seealso>
		virtual public int Spaces
		{
			get
			{
				return configuration.spaces;
			}
			
			
			
			set
			{
				configuration.spaces = value;
			}
			
		}
		/// <summary> Wraplen - default wrap margin</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.wraplen">
		/// </seealso>
		virtual public int Wraplen
		{
			get
			{
				return configuration.wraplen;
			}
			
			
			
			set
			{
				configuration.wraplen = value;
			}
			
		}
		/// <summary> CharEncoding</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.CharEncoding">
		/// </seealso>
		virtual public int CharEncoding
		{
			get
			{
				return configuration.CharEncoding;
			}
			
			
			
			set
			{
				configuration.CharEncoding = value;
			}
			
		}
		/// <summary> Tabsize</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.tabsize">
		/// </seealso>
		virtual public int Tabsize
		{
			get
			{
				return configuration.tabsize;
			}
			
			
			
			set
			{
				configuration.tabsize = value;
			}
			
		}
		/// <summary> Errfile - file name to write errors to</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.errfile">
		/// </seealso>
		virtual public System.String Errfile
		{
			get
			{
				return configuration.errfile;
			}
			
			
			
			set
			{
				configuration.errfile = value;
			}
			
		}
		/// <summary> Writeback - if true then output tidied markup
		/// NOTE: this property is ignored when parsing from an InputStream.
		/// </summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.writeback">
		/// </seealso>
		virtual public bool Writeback
		{
			get
			{
				return configuration.writeback;
			}
			
			
			
			set
			{
				configuration.writeback = value;
			}
			
		}
		/// <summary> OnlyErrors - if true normal output is suppressed</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.OnlyErrors">
		/// </seealso>
		virtual public bool OnlyErrors
		{
			get
			{
				return configuration.OnlyErrors;
			}
			
			
			
			set
			{
				configuration.OnlyErrors = value;
			}
			
		}
		/// <summary> ShowWarnings - however errors are always shown</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.ShowWarnings">
		/// </seealso>
		virtual public bool ShowWarnings
		{
			get
			{
				return configuration.ShowWarnings;
			}
			
			
			
			set
			{
				configuration.ShowWarnings = value;
			}
			
		}
		/// <summary> Quiet - no 'Parsing X', guessed DTD or summary</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.Quiet">
		/// </seealso>
		virtual public bool Quiet
		{
			get
			{
				return configuration.Quiet;
			}
			
			
			
			set
			{
				configuration.Quiet = value;
			}
			
		}
		/// <summary> IndentContent - indent content of appropriate tags</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.IndentContent">
		/// </seealso>
		virtual public bool IndentContent
		{
			get
			{
				return configuration.IndentContent;
			}
			
			
			
			set
			{
				configuration.IndentContent = value;
			}
			
		}
		/// <summary> SmartIndent - does text/block level content effect indentation</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.SmartIndent">
		/// </seealso>
		virtual public bool SmartIndent
		{
			get
			{
				return configuration.SmartIndent;
			}
			
			
			
			set
			{
				configuration.SmartIndent = value;
			}
			
		}
		/// <summary> HideEndTags - suppress optional end tags</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.HideEndTags">
		/// </seealso>
		virtual public bool HideEndTags
		{
			get
			{
				return configuration.HideEndTags;
			}
			
			
			
			set
			{
				configuration.HideEndTags = value;
			}
			
		}
		/// <summary> XmlTags - treat input as XML</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.XmlTags">
		/// </seealso>
		virtual public bool XmlTags
		{
			get
			{
				return configuration.XmlTags;
			}
			
			
			
			set
			{
				configuration.XmlTags = value;
			}
			
		}
		/// <summary> XmlOut - create output as XML</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.XmlOut">
		/// </seealso>
		virtual public bool XmlOut
		{
			get
			{
				return configuration.XmlOut;
			}
			
			
			
			set
			{
				configuration.XmlOut = value;
			}
			
		}
		/// <summary> XHTML - output extensible HTML</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.xHTML">
		/// </seealso>
		virtual public bool XHTML
		{
			get
			{
				return configuration.xHTML;
			}
			
			
			
			set
			{
				configuration.xHTML = value;
			}
			
		}
		/// <summary> RawOut - avoid mapping values > 127 to entities</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.RawOut">
		/// </seealso>
		virtual public bool RawOut
		{
			get
			{
				return configuration.RawOut;
			}
			
			
			
			set
			{
				configuration.RawOut = value;
			}
			
		}
		/// <summary> UpperCaseTags - output tags in upper not lower case</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.UpperCaseTags">
		/// </seealso>
		virtual public bool UpperCaseTags
		{
			get
			{
				return configuration.UpperCaseTags;
			}
			
			
			
			set
			{
				configuration.UpperCaseTags = value;
			}
			
		}
		/// <summary> UpperCaseAttrs - output attributes in upper not lower case</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.UpperCaseAttrs">
		/// </seealso>
		virtual public bool UpperCaseAttrs
		{
			get
			{
				return configuration.UpperCaseAttrs;
			}
			
			
			
			set
			{
				configuration.UpperCaseAttrs = value;
			}
			
		}
		/// <summary> MakeClean - remove presentational clutter</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.MakeClean">
		/// </seealso>
		virtual public bool MakeClean
		{
			get
			{
				return configuration.MakeClean;
			}
			
			
			
			set
			{
				configuration.MakeClean = value;
			}
			
		}
		/// <summary> BreakBeforeBR - o/p newline before &lt;br&gt; or not?</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.BreakBeforeBR">
		/// </seealso>
		virtual public bool BreakBeforeBR
		{
			get
			{
				return configuration.BreakBeforeBR;
			}
			
			
			
			set
			{
				configuration.BreakBeforeBR = value;
			}
			
		}
		/// <summary> BurstSlides - create slides on each h2 element</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.BurstSlides">
		/// </seealso>
		virtual public bool BurstSlides
		{
			get
			{
				return configuration.BurstSlides;
			}
			
			
			
			set
			{
				configuration.BurstSlides = value;
			}
			
		}
		/// <summary> NumEntities - use numeric entities</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.NumEntities">
		/// </seealso>
		virtual public bool NumEntities
		{
			get
			{
				return configuration.NumEntities;
			}
			
			
			
			set
			{
				configuration.NumEntities = value;
			}
			
		}
		/// <summary> QuoteMarks - output " marks as &amp;quot;</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.QuoteMarks">
		/// </seealso>
		virtual public bool QuoteMarks
		{
			get
			{
				return configuration.QuoteMarks;
			}
			
			
			
			set
			{
				configuration.QuoteMarks = value;
			}
			
		}
		/// <summary> QuoteNbsp - output non-breaking space as entity</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.QuoteNbsp">
		/// </seealso>
		virtual public bool QuoteNbsp
		{
			get
			{
				return configuration.QuoteNbsp;
			}
			
			
			
			set
			{
				configuration.QuoteNbsp = value;
			}
			
		}
		/// <summary> QuoteAmpersand - output naked ampersand as &amp;</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.QuoteAmpersand">
		/// </seealso>
		virtual public bool QuoteAmpersand
		{
			get
			{
				return configuration.QuoteAmpersand;
			}
			
			
			
			set
			{
				configuration.QuoteAmpersand = value;
			}
			
		}
		/// <summary> WrapAttVals - wrap within attribute values</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.WrapAttVals">
		/// </seealso>
		virtual public bool WrapAttVals
		{
			get
			{
				return configuration.WrapAttVals;
			}
			
			
			
			set
			{
				configuration.WrapAttVals = value;
			}
			
		}
		/// <summary> WrapScriptlets - wrap within JavaScript string literals</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.WrapScriptlets">
		/// </seealso>
		virtual public bool WrapScriptlets
		{
			get
			{
				return configuration.WrapScriptlets;
			}
			
			
			
			set
			{
				configuration.WrapScriptlets = value;
			}
			
		}
		/// <summary> WrapSection - wrap within &lt;![ ... ]&gt; section tags</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.WrapSection">
		/// </seealso>
		virtual public bool WrapSection
		{
			get
			{
				return configuration.WrapSection;
			}
			
			
			
			set
			{
				configuration.WrapSection = value;
			}
			
		}
		/// <summary> AltText - default text for alt attribute</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.altText">
		/// </seealso>
		virtual public System.String AltText
		{
			get
			{
				return configuration.altText;
			}
			
			
			
			set
			{
				configuration.altText = value;
			}
			
		}
		/// <summary> Slidestyle - style sheet for slides</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.slidestyle">
		/// </seealso>
		virtual public System.String Slidestyle
		{
			get
			{
				return configuration.slidestyle;
			}
			
			
			
			set
			{
				configuration.slidestyle = value;
			}
			
		}
		/// <summary> XmlPi - add &lt;?xml?&gt; for XML docs</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.XmlPi">
		/// </seealso>
		virtual public bool XmlPi
		{
			get
			{
				return configuration.XmlPi;
			}
			
			
			
			set
			{
				configuration.XmlPi = value;
			}
			
		}
		/// <summary> DropFontTags - discard presentation tags</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.DropFontTags">
		/// </seealso>
		virtual public bool DropFontTags
		{
			get
			{
				return configuration.DropFontTags;
			}
			
			
			
			set
			{
				configuration.DropFontTags = value;
			}
			
		}
		/// <summary> DropEmptyParas - discard empty p elements</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.DropEmptyParas">
		/// </seealso>
		virtual public bool DropEmptyParas
		{
			get
			{
				return configuration.DropEmptyParas;
			}
			
			
			
			set
			{
				configuration.DropEmptyParas = value;
			}
			
		}
		/// <summary> FixComments - fix comments with adjacent hyphens</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.FixComments">
		/// </seealso>
		virtual public bool FixComments
		{
			get
			{
				return configuration.FixComments;
			}
			
			
			
			set
			{
				configuration.FixComments = value;
			}
			
		}
		/// <summary> WrapAsp - wrap within ASP pseudo elements</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.WrapAsp">
		/// </seealso>
		virtual public bool WrapAsp
		{
			get
			{
				return configuration.WrapAsp;
			}
			
			
			
			set
			{
				configuration.WrapAsp = value;
			}
			
		}
		/// <summary> WrapJste - wrap within JSTE pseudo elements</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.WrapJste">
		/// </seealso>
		virtual public bool WrapJste
		{
			get
			{
				return configuration.WrapJste;
			}
			
			
			
			set
			{
				configuration.WrapJste = value;
			}
			
		}
		/// <summary> WrapPhp - wrap within PHP pseudo elements</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.WrapPhp">
		/// </seealso>
		virtual public bool WrapPhp
		{
			get
			{
				return configuration.WrapPhp;
			}
			
			
			
			set
			{
				configuration.WrapPhp = value;
			}
			
		}
		/// <summary> FixBackslash - fix URLs by replacing \ with /</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.FixBackslash">
		/// </seealso>
		virtual public bool FixBackslash
		{
			get
			{
				return configuration.FixBackslash;
			}
			
			
			
			set
			{
				configuration.FixBackslash = value;
			}
			
		}
		/// <summary> IndentAttributes - newline+indent before each attribute</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.IndentAttributes">
		/// </seealso>
		virtual public bool IndentAttributes
		{
			get
			{
				return configuration.IndentAttributes;
			}
			
			
			
			set
			{
				configuration.IndentAttributes = value;
			}
			
		}
		/// <summary> DocType - user specified doctype
		/// omit | auto | strict | loose | <i>fpi</i>
		/// where the <i>fpi</i> is a string similar to
		/// &quot;-//ACME//DTD HTML 3.14159//EN&quot;
		/// Note: for <i>fpi</i> include the double-quotes in the string.
		/// </summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.docTypeStr">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.docTypeMode">
		/// </seealso>
		virtual public System.String DocType
		{
			get
			{
				System.String result = null;
				switch (configuration.docTypeMode)
				{
					
					case Configuration.DOCTYPE_OMIT: 
						result = "omit";
						break;
					
					case Configuration.DOCTYPE_AUTO: 
						result = "auto";
						break;
					
					case Configuration.DOCTYPE_STRICT: 
						result = "strict";
						break;
					
					case Configuration.DOCTYPE_LOOSE: 
						result = "loose";
						break;
					
					case Configuration.DOCTYPE_USER: 
						result = configuration.docTypeStr;
						break;
					}
				return result;
			}
			
			
			
			set
			{
				if (value != null)
					configuration.docTypeStr = configuration.ParseDocType(value, "doctype");
			}
			
		}
		/// <summary> LogicalEmphasis - replace i by em and b by strong</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.LogicalEmphasis">
		/// </seealso>
		virtual public bool LogicalEmphasis
		{
			get
			{
				return configuration.LogicalEmphasis;
			}
			
			
			
			set
			{
				configuration.LogicalEmphasis = value;
			}
			
		}
		/// <summary> XmlPIs - if set to true PIs must end with ?></summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.XmlPIs">
		/// </seealso>
		virtual public bool XmlPIs
		{
			get
			{
				return configuration.XmlPIs;
			}
			
			
			
			set
			{
				configuration.XmlPIs = value;
			}
			
		}
		/// <summary> EncloseText - if true text at body is wrapped in &lt;p&gt;'s</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.EncloseBodyText">
		/// </seealso>
		virtual public bool EncloseText
		{
			get
			{
				return configuration.EncloseBodyText;
			}
			
			
			
			set
			{
				configuration.EncloseBodyText = value;
			}
			
		}
		/// <summary> EncloseBlockText - if true text in blocks is wrapped in &lt;p&gt;'s</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.EncloseBlockText">
		/// </seealso>
		virtual public bool EncloseBlockText
		{
			get
			{
				return configuration.EncloseBlockText;
			}
			
			
			
			set
			{
				configuration.EncloseBlockText = value;
			}
			
		}
		/// <summary> KeepFileTimes - if true last modified time is preserved<br/>
		/// <b>this is NOT supported at this time.</b>
		/// </summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.KeepFileTimes">
		/// </seealso>
		virtual public bool KeepFileTimes
		{
			get
			{
				return configuration.KeepFileTimes;
			}
			
			
			
			set
			{
				configuration.KeepFileTimes = value;
			}
			
		}
		/// <summary> Word2000 - draconian cleaning for Word2000</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.Word2000">
		/// </seealso>
		virtual public bool Word2000
		{
			get
			{
				return configuration.Word2000;
			}
			
			
			
			set
			{
				configuration.Word2000 = value;
			}
			
		}
		/// <summary> TidyMark - add meta element indicating tidied doc</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.TidyMark">
		/// </seealso>
		virtual public bool TidyMark
		{
			get
			{
				return configuration.TidyMark;
			}
			
			
			
			set
			{
				configuration.TidyMark = value;
			}
			
		}
		/// <summary> XmlSpace - if set to yes adds xml:space attr as needed</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.XmlSpace">
		/// </seealso>
		virtual public bool XmlSpace
		{
			get
			{
				return configuration.XmlSpace;
			}
			
			
			
			set
			{
				configuration.XmlSpace = value;
			}
			
		}
		/// <summary> Emacs - if true format error output for GNU Emacs</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.Emacs">
		/// </seealso>
		virtual public bool Emacs
		{
			get
			{
				return configuration.Emacs;
			}
			
			
			
			set
			{
				configuration.Emacs = value;
			}
			
		}
		/// <summary> LiteralAttribs - if true attributes may use newlines</summary>
		/// <seealso cref="Comzept.Genesis.Tidy.Configuration.LiteralAttribs">
		/// </seealso>
		virtual public bool LiteralAttribs
		{
			get
			{
				return configuration.LiteralAttribs;
			}
			
			
			
			set
			{
				configuration.LiteralAttribs = value;
			}
			
		}
		/// <summary> InputStreamName - the name of the input stream (printed in the
		/// header information).
		/// </summary>
		virtual public System.String InputStreamName
		{
			get
			{
				return inputStreamName;
			}
			
			set
			{
				if (value != null)
					inputStreamName = value;
			}
			
		}
		/// <summary> Sets the configuration from a configuration file.</summary>
		virtual public System.String ConfigurationFromFile
		{
			
			
			set
			{
				configuration.ParseFile(value);
			}
			
		}
		/// <summary> Sets the configuration from a properties object.</summary>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.util.Properties' and 'System.Collections.Specialized.NameValueCollection' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		virtual public System.Collections.Specialized.NameValueCollection ConfigurationFromProps
		{
			
			
			set
			{
				configuration.AddProps(value);
			}
			
		}
		
		internal const long serialVersionUID = - 2794371560623987718L;
		
		private bool initialized = false;
		private System.IO.StreamWriter errout = null; /* error output stream */
		private System.IO.StreamWriter stderr = null;
		private Configuration configuration = null;
		private System.String inputStreamName = "InputStream";
		private int parseErrors = 0;
		private int parseWarnings = 0;
		
		public Tidy()
		{
			init();
		}
		
		public virtual Configuration getConfiguration()
		{
			return configuration;
		}
		
		/// <summary> first time initialization which should
		/// precede reading the command line
		/// </summary>
		
		private void  init()
		{
			configuration = new Configuration();
			if (configuration == null)
				return ;
			
			AttributeTable at = AttributeTable.DefaultAttributeTable;
			if (at == null)
				return ;
			TagTable tt = new TagTable();
			if (tt == null)
				return ;
			tt.Configuration = configuration;
			configuration.tt = tt;
			EntityTable et = EntityTable.DefaultEntityTable;
			if (et == null)
				return ;
			
			/* Unnecessary - same initial values in Configuration
			Configuration.XmlTags       = false;
			Configuration.XmlOut        = false;
			Configuration.HideEndTags   = false;
			Configuration.UpperCaseTags = false;
			Configuration.MakeClean     = false;
			Configuration.writeback     = false;
			Configuration.OnlyErrors    = false;
			*/
			
			configuration.errfile = null;
			System.IO.StreamWriter temp_writer;
			temp_writer = new System.IO.StreamWriter(System.Console.OpenStandardError(), System.Text.Encoding.Default);
			temp_writer.AutoFlush = true;
			stderr = temp_writer;
			errout = stderr;
			initialized = true;
		}
		
		/// <summary> Parses InputStream in and returns the root Node.
		/// If out is non-null, pretty prints to OutputStream out.
		/// </summary>
		
		public virtual Node parse(System.IO.Stream in_Renamed, System.IO.Stream out_Renamed)
		{
			Node document = null;
			
			try
			{
				document = parse(in_Renamed, null, out_Renamed);
			}
			catch (System.IO.FileNotFoundException)
			{
			}
			catch (System.IO.IOException)
			{
			}
			
			return document;
		}
		
		
		/// <summary> Internal routine that actually does the parsing.  The caller
		/// can pass either an InputStream or file name.  If both are passed,
		/// the file name is preferred.
		/// </summary>
		
		private Node parse(System.IO.Stream in_Renamed, System.String file, System.IO.Stream out_Renamed)
		{
			Lexer lexer;
			Node document = null;
			Node doctype;
			Out o = new OutImpl(); /* normal output stream */
			PPrint pprint;
			
			if (!initialized)
				return null;
			
			if (errout == null)
				return null;
			
			parseErrors = 0;
			parseWarnings = 0;
			
			/* ensure config is self-consistent */
			configuration.Adjust();
			
			if (file != null)
			{
				in_Renamed = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				inputStreamName = file;
			}
			else if (in_Renamed == null)
			{
				in_Renamed = System.Console.OpenStandardInput();
				inputStreamName = "stdin";
			}
			
			if (in_Renamed != null)
			{
				lexer = new Lexer(new StreamInImpl(in_Renamed, configuration.CharEncoding, configuration.tabsize), configuration);
				lexer.errout = errout;
				
				/*
				store pointer to lexer in input stream
				to allow character encoding errors to be
				reported
				*/
				lexer.in_Renamed.lexer = lexer;
				
				/* Tidy doesn't alter the doctype for generic XML docs */
				if (configuration.XmlTags)
					document = ParserImpl.ParseXMLDocument(lexer);
				else
				{
					lexer.warnings = 0;
					if (!configuration.Quiet)
						Report.helloMessage(errout, Report.RELEASE_DATE, inputStreamName);
					
					document = ParserImpl.parseDocument(lexer);
					
					if (!document.checkNodeIntegrity())
					{
						Report.badTree(errout);
						return null;
					}
					
					Clean cleaner = new Clean(configuration.tt);
					
					/* simplifies <b><b> ... </b> ...</b> etc. */
					cleaner.nestedEmphasis(document);
					
					/* cleans up <dir>indented text</dir> etc. */
					cleaner.list2BQ(document);
					cleaner.bQ2Div(document);
					
					/* replaces i by em and b by strong */
					if (configuration.LogicalEmphasis)
						cleaner.emFromI(document);
					
					if (configuration.Word2000 && cleaner.isWord2000(document, configuration.tt))
					{
						/* prune Word2000's <![if ...]> ... <![endif]> */
						cleaner.dropSections(lexer, document);
						
						/* drop style & class attributes and empty p, span elements */
						cleaner.cleanWord2000(lexer, document);
					}
					
					/* replaces presentational markup by style rules */
					if (configuration.MakeClean || configuration.DropFontTags)
						cleaner.cleanTree(lexer, document);
					
					if (!document.checkNodeIntegrity())
					{
						Report.badTree(errout);
						return null;
					}
					doctype = document.findDocType();
					if (document.content != null)
					{
						if (configuration.xHTML)
							lexer.setXHTMLDocType(document);
						else
							lexer.fixDocType(document);
						
						if (configuration.TidyMark)
							lexer.addGenerator(document);
					}
					
					/* ensure presence of initial <?XML version="1.0"?> */
					if (configuration.XmlOut && configuration.XmlPi)
						lexer.fixXMLPI(document);
					
					if (!configuration.Quiet && document.content != null)
					{
						Report.reportVersion(errout, lexer, inputStreamName, doctype);
						Report.reportNumWarnings(errout, lexer);
					}
				}
				
				parseWarnings = lexer.warnings;
				parseErrors = lexer.errors;
				
				// Try to close the InputStream but only if if we created it.
				
				if ((file != null) && (in_Renamed != null))
				{
					try
					{
						in_Renamed.Close();
					}
					catch (System.IO.IOException)
					{
					}
				}
				
				if (lexer.errors > 0)
					Report.needsAuthorIntervention(errout);
				
				o.state = StreamIn.FSM_ASCII;
				o.encoding = configuration.CharEncoding;
				
				if (!configuration.OnlyErrors && lexer.errors == 0)
				{
					if (configuration.BurstSlides)
					{
						Node body;
						
						body = null;
						/*
						remove doctype to avoid potential clash with
						markup introduced when bursting into slides
						*/
						/* discard the document type */
						doctype = document.findDocType();
						
						if (doctype != null)
							Node.discardElement(doctype);
						
						/* slides use transitional features */
						lexer.versions |= Dict.VERS_HTML40_LOOSE;
						
						/* and patch up doctype to match */
						if (configuration.xHTML)
							lexer.setXHTMLDocType(document);
						else
							lexer.fixDocType(document);
						
						/* find the body element which may be implicit */
						body = document.findBody(configuration.tt);
						
						if (body != null)
						{
							pprint = new PPrint(configuration);
							Report.reportNumberOfSlides(errout, pprint.countSlides(body));
							pprint.createSlides(lexer, document);
						}
						else
							Report.missingBody(errout);
					}
					else if (configuration.writeback && (file != null))
					{
						try
						{
							pprint = new PPrint(configuration);
							//UPGRADE_TODO: Constructor 'java.io.FileOutputStream.FileOutputStream' was converted to 'System.IO.FileStream.FileStream' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioFileOutputStreamFileOutputStream_javalangString'"
							o.out_Renamed = new System.IO.FileStream(file, System.IO.FileMode.Create);
							
							if (configuration.XmlTags)
								pprint.printXMLTree(o, (short) 0, 0, lexer, document);
							else
								pprint.printTree(o, (short) 0, 0, lexer, document);
							
							pprint.flushLine(o, 0);
							o.out_Renamed.Close();
						}
						catch (System.IO.IOException e)
						{
							//UPGRADE_TODO: Method 'java.io.PrintWriter.println' was converted to 'System.IO.TextWriter.WriteLine' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioPrintWriterprintln_javalangString'"
							//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
							errout.WriteLine(file + e.ToString());
						}
					}
					else if (out_Renamed != null)
					{
						pprint = new PPrint(configuration);
						o.out_Renamed = out_Renamed;
						
						if (configuration.XmlTags)
							pprint.printXMLTree(o, (short) 0, 0, lexer, document);
						else
							pprint.printTree(o, (short) 0, 0, lexer, document);
						
						pprint.flushLine(o, 0);
					}
				}
				
				Report.errorSummary(lexer);
			}
			return document;
		}
		
		
		/// <summary> Parses InputStream in and returns a DOM Document node.
		/// If out is non-null, pretty prints to OutputStream out.
		/// </summary>
		
		public virtual Comzept.Genesis.Tidy.Dom.Document parseDOM(System.IO.Stream in_Renamed, System.IO.Stream out_Renamed)
		{
			Node document = parse(in_Renamed, out_Renamed);
			if (document != null)
				return (Comzept.Genesis.Tidy.Dom.Document) document.Adapter;
			else
				return null;
		}
		
		/// <summary> Creates an empty DOM Document.</summary>
		
		public static Comzept.Genesis.Tidy.Dom.Document createEmptyDocument()
		{
			Node document = new Node(Node.RootNode, new sbyte[0], 0, 0);
			Node node = new Node(Node.StartTag, new sbyte[0], 0, 0, "html", new TagTable());
			if (document != null && node != null)
			{
				Node.insertNodeAtStart(document, node);
				return (Comzept.Genesis.Tidy.Dom.Document) document.Adapter;
			}
			else
			{
				return null;
			}
		}
		
		/// <summary> Pretty-prints a DOM Document.</summary>
		
		public virtual void  pprint(Comzept.Genesis.Tidy.Dom.Document doc, System.IO.Stream out_Renamed)
		{
			Out o = new OutImpl();
			PPrint pprint;
			Node document;
			
			if (!(doc is DOMDocumentImpl))
			{
				return ;
			}
			document = ((DOMDocumentImpl) doc).adaptee;
			
			o.state = StreamIn.FSM_ASCII;
			o.encoding = configuration.CharEncoding;
			
			if (out_Renamed != null)
			{
				pprint = new PPrint(configuration);
				o.out_Renamed = out_Renamed;
				
				if (configuration.XmlTags)
					pprint.printXMLTree(o, (short) 0, 0, null, document);
				else
					pprint.printTree(o, (short) 0, 0, null, document);
				
				pprint.flushLine(o, 0);
			}
		}
		
		/// <summary> Command line interface to parser and pretty printer.</summary>
		
		[STAThread]
		public static void  Main(System.String[] argv)
		{
			int totalerrors = 0;
			int totalwarnings = 0;
			System.String file;
			System.IO.Stream in_Renamed;
			System.String prog = "Tidy";
			Node document;
			Node doctype;
			Lexer lexer;
			System.String s;
			Out out_Renamed = new OutImpl(); /* normal output stream */
			PPrint pprint;
			int argc = argv.Length + 1;
			int argIndex = 0;
			Tidy tidy;
			Configuration configuration;
			System.String arg;
			System.String current_errorfile = "stderr";
			
			tidy = new Tidy();
			configuration = tidy.getConfiguration();
			
			/* read command line */
			
			while (argc > 0)
			{
				if (argc > 1 && argv[argIndex].StartsWith("-"))
				{
					/* support -foo and --foo */
					arg = argv[argIndex].Substring(1);
					
					if (arg.Length > 0 && arg[0] == '-')
						arg = arg.Substring(1);
					
					if (arg.Equals("xml"))
						configuration.XmlTags = true;
					else if (arg.Equals("asxml") || arg.Equals("asxhtml"))
						configuration.xHTML = true;
					else if (arg.Equals("indent"))
					{
						configuration.IndentContent = true;
						configuration.SmartIndent = true;
					}
					else if (arg.Equals("omit"))
						configuration.HideEndTags = true;
					else if (arg.Equals("upper"))
						configuration.UpperCaseTags = true;
					else if (arg.Equals("clean"))
						configuration.MakeClean = true;
					else if (arg.Equals("raw"))
						configuration.CharEncoding = Configuration.RAW;
					else if (arg.Equals("ascii"))
						configuration.CharEncoding = Configuration.ASCII;
					else if (arg.Equals("latin1"))
						configuration.CharEncoding = Configuration.LATIN1;
					else if (arg.Equals("utf8"))
						configuration.CharEncoding = Configuration.UTF8;
					else if (arg.Equals("iso2022"))
						configuration.CharEncoding = Configuration.ISO2022;
					else if (arg.Equals("mac"))
						configuration.CharEncoding = Configuration.MACROMAN;
					else if (arg.Equals("numeric"))
						configuration.NumEntities = true;
					else if (arg.Equals("modify"))
						configuration.writeback = true;
					else if (arg.Equals("change"))
					/* obsolete */
						configuration.writeback = true;
					else if (arg.Equals("update"))
					/* obsolete */
						configuration.writeback = true;
					else if (arg.Equals("errors"))
						configuration.OnlyErrors = true;
					else if (arg.Equals("quiet"))
						configuration.Quiet = true;
					else if (arg.Equals("slides"))
						configuration.BurstSlides = true;
					else if (arg.Equals("help") || argv[argIndex][1] == '?' || argv[argIndex][1] == 'h')
					{
						System.IO.StreamWriter temp_writer;
						temp_writer = new System.IO.StreamWriter(System.Console.OpenStandardOutput(), System.Text.Encoding.Default);
						temp_writer.AutoFlush = true;
						Report.helpText(temp_writer, prog);
						System.Environment.Exit(1);
					}
					else if (arg.Equals("config"))
					{
						if (argc >= 3)
						{
							configuration.ParseFile(argv[argIndex + 1]);
							--argc;
							++argIndex;
						}
					}
					else if (argv[argIndex].Equals("-file") || argv[argIndex].Equals("--file") || argv[argIndex].Equals("-f"))
					{
						if (argc >= 3)
						{
							configuration.errfile = argv[argIndex + 1];
							--argc;
							++argIndex;
						}
					}
					else if (argv[argIndex].Equals("-wrap") || argv[argIndex].Equals("--wrap") || argv[argIndex].Equals("-w"))
					{
						if (argc >= 3)
						{
							configuration.wraplen = System.Int32.Parse(argv[argIndex + 1]);
							--argc;
							++argIndex;
						}
					}
					else if (argv[argIndex].Equals("-version") || argv[argIndex].Equals("--version") || argv[argIndex].Equals("-v"))
					{
						Report.showVersion(tidy.Errout);
						System.Environment.Exit(0);
					}
					else
					{
						s = argv[argIndex];
						
						for (int i = 1; i < s.Length; i++)
						{
							if (s[i] == 'i')
							{
								configuration.IndentContent = true;
								configuration.SmartIndent = true;
							}
							else if (s[i] == 'o')
								configuration.HideEndTags = true;
							else if (s[i] == 'u')
								configuration.UpperCaseTags = true;
							else if (s[i] == 'c')
								configuration.MakeClean = true;
							else if (s[i] == 'n')
								configuration.NumEntities = true;
							else if (s[i] == 'm')
								configuration.writeback = true;
							else if (s[i] == 'e')
								configuration.OnlyErrors = true;
							else if (s[i] == 'q')
								configuration.Quiet = true;
							else
								Report.unknownOption(tidy.Errout, s[i]);
						}
					}
					
					--argc;
					++argIndex;
					continue;
				}
				
				/* ensure config is self-consistent */
				configuration.Adjust();
				
				/* user specified error file */
				if (configuration.errfile != null)
				{
					/* is it same as the currently opened file? */
					if (!configuration.errfile.Equals(current_errorfile))
					{
						/* no so close previous error file */
						
						if (tidy.Errout != tidy.Stderr)
							tidy.Errout.Close();
						
						/* and try to open the new error file */
						try
						{
							System.IO.StreamWriter temp_writer2;
							temp_writer2 = new System.IO.StreamWriter(new System.IO.StreamWriter(configuration.errfile, false, System.Text.Encoding.Default).BaseStream, new System.IO.StreamWriter(configuration.errfile, false, System.Text.Encoding.Default).Encoding);
							temp_writer2.AutoFlush = true;
							tidy.Errout = temp_writer2;
							current_errorfile = configuration.errfile;
						}
						catch (System.IO.IOException)
						{
							/* can't be opened so fall back to stderr */
							current_errorfile = "stderr";
							tidy.Errout = tidy.Stderr;
						}
					}
				}
				
				if (argc > 1)
				{
					file = argv[argIndex];
				}
				else
				{
					file = "stdin";
				}
				
				try
				{
					document = tidy.parse(null, file, System.Console.OpenStandardOutput());
					totalwarnings += tidy.parseWarnings;
					totalerrors += tidy.parseErrors;
				}
				catch (System.IO.FileNotFoundException fnfe)
				{
					Report.unknownFile(tidy.Errout, prog, file);
				}
				catch (System.IO.IOException ioe)
				{
					Report.unknownFile(tidy.Errout, prog, file);
				}
				
				--argc;
				++argIndex;
				
				if (argc <= 1)
					break;
			}
			
			if (totalerrors + totalwarnings > 0)
				Report.generalInfo(tidy.Errout);
			
			if (tidy.Errout != tidy.Stderr)
				tidy.Errout.Close();
			
			/* return status can be used by scripts */
			
			if (totalerrors > 0)
				System.Environment.Exit(2);
			
			if (totalwarnings > 0)
				System.Environment.Exit(1);
			
			/* 0 signifies all is ok */
			System.Environment.Exit(0);
		}
	}
}