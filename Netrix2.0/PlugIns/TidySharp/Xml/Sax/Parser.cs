// SAX parser interface.
// No warranty; no copyright -- use this as you will.
// $Id: Parser.java,v 1.1 2000/11/07 05:29:52 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	
	/// <summary> Basic interface for SAX (Simple API for XML) parsers.
	/// 
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p>This was the main event supplier interface for SAX1; it has
	/// been replaced in SAX2 by {@link org.Xml.Sax.XMLReader XMLReader},
	/// which includes Namespace support and sophisticated configurability
	/// and extensibility.</p>
	/// 
	/// <p>All SAX1 parsers must implement this basic interface: it allows
	/// applications to register handlers for different types of events
	/// and to initiate a parse from a URI, or a character stream.</p>
	/// 
	/// <p>All SAX1 parsers must also implement a zero-argument constructor
	/// (though other constructors are also allowed).</p>
	/// 
	/// <p>SAX1 parsers are reusable but not re-entrant: the application
	/// may reuse a parser object (possibly with a different input source)
	/// once the first parse has completed successfully, but it may not
	/// invoke the parse() methods recursively within a parse.</p>
	/// 
	/// </summary>
	/// <deprecated> This interface has been replaced by the SAX2
	/// {@link org.Xml.Sax.XMLReader XMLReader}
	/// interface, which includes Namespace support.
	/// </deprecated>
	/// <since> SAX 1.0
	/// </since>
	/// <author>  David Megginson, 
	/// <a href="mailto:sax@megginson.com">sax@megginson.com</a>
	/// </author>
	/// <version>  2.0
	/// </version>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.EntityResolver">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.DTDHandler">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.DocumentHandler">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.ErrorHandler">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.HandlerBase">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.InputSource">
	/// </seealso>
	public interface Parser
	{
		/// <summary> Allow an application to request a locale for errors and warnings.
		/// 
		/// <p>SAX parsers are not required to provide localisation for errors
		/// and warnings; if they cannot support the requested locale,
		/// however, they must throw a SAX exception.  Applications may
		/// not request a locale change in the middle of a parse.</p>
		/// 
		/// </summary>
		/// <param name="locale">A Java Locale object.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Throws an exception
		/// (using the previous or default locale) if the 
		/// requested locale is not supported.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.SAXParseException">
		/// </seealso>
		System.Globalization.CultureInfo Locale
		{
			set;
			
		}
		/// <summary> Allow an application to register a custom entity resolver.
		/// 
		/// <p>If the application does not register an entity resolver, the
		/// SAX parser will resolve system identifiers and open connections
		/// to entities itself (this is the default behaviour implemented in
		/// HandlerBase).</p>
		/// 
		/// <p>Applications may register a new or different entity resolver
		/// in the middle of a parse, and the SAX parser must begin using
		/// the new resolver immediately.</p>
		/// 
		/// </summary>
		/// <param name="resolver">The object for resolving entities.
		/// </param>
		/// <seealso cref="EntityResolver">
		/// </seealso>
		/// <seealso cref="HandlerBase">
		/// </seealso>
		EntityResolver EntityResolver
		{
			set;
			
		}
		/// <summary> Allow an application to register a DTD event handler.
		/// 
		/// <p>If the application does not register a DTD handler, all DTD
		/// events reported by the SAX parser will be silently
		/// ignored (this is the default behaviour implemented by
		/// HandlerBase).</p>
		/// 
		/// <p>Applications may register a new or different
		/// handler in the middle of a parse, and the SAX parser must
		/// begin using the new handler immediately.</p>
		/// 
		/// </summary>
		/// <param name="handler">The DTD handler.
		/// </param>
		/// <seealso cref="DTDHandler">
		/// </seealso>
		/// <seealso cref="HandlerBase">
		/// </seealso>
		DTDHandler DTDHandler
		{
			set;
			
		}
		/// <summary> Allow an application to register a document event handler.
		/// 
		/// <p>If the application does not register a document handler, all
		/// document events reported by the SAX parser will be silently
		/// ignored (this is the default behaviour implemented by
		/// HandlerBase).</p>
		/// 
		/// <p>Applications may register a new or different handler in the
		/// middle of a parse, and the SAX parser must begin using the new
		/// handler immediately.</p>
		/// 
		/// </summary>
		/// <param name="handler">The document handler.
		/// </param>
		/// <seealso cref="DocumentHandler">
		/// </seealso>
		/// <seealso cref="HandlerBase">
		/// </seealso>
		DocumentHandler DocumentHandler
		{
			set;
			
		}
		/// <summary> Allow an application to register an error event handler.
		/// 
		/// <p>If the application does not register an error event handler,
		/// all error events reported by the SAX parser will be silently
		/// ignored, except for fatalError, which will throw a SAXException
		/// (this is the default behaviour implemented by HandlerBase).</p>
		/// 
		/// <p>Applications may register a new or different handler in the
		/// middle of a parse, and the SAX parser must begin using the new
		/// handler immediately.</p>
		/// 
		/// </summary>
		/// <param name="handler">The error handler.
		/// </param>
		/// <seealso cref="ErrorHandler">
		/// </seealso>
		/// <seealso cref="SAXException">
		/// </seealso>
		/// <seealso cref="HandlerBase">
		/// </seealso>
		ErrorHandler ErrorHandler
		{
			set;
			
		}
		
		
		/// <summary> Parse an XML document.
		/// 
		/// <p>The application can use this method to instruct the SAX parser
		/// to begin parsing an XML document from any valid input
		/// source (a character stream, a byte stream, or a URI).</p>
		/// 
		/// <p>Applications may not invoke this method while a parse is in
		/// progress (they should create a new Parser instead for each
		/// additional XML document).  Once a parse is complete, an
		/// application may reuse the same Parser object, possibly with a
		/// different input source.</p>
		/// 
		/// </summary>
		/// <param name="source">The input source for the top-level of the
		/// XML document.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <exception cref="java.io.IOException">An IO exception from the parser,
		/// possibly from a byte stream or character stream
		/// supplied by the application.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.InputSource">
		/// </seealso>
		/// <seealso cref="parse(java.lang.String)">
		/// </seealso>
		/// <seealso cref="EntityResolver">
		/// </seealso>
		/// <seealso cref="DTDHandler">
		/// </seealso>
		/// <seealso cref="DocumentHandler">
		/// </seealso>
		/// <seealso cref="ErrorHandler">
		/// </seealso>
		void  parse(InputSource source);
		
		
		/// <summary> Parse an XML document from a system identifier (URI).
		/// 
		/// <p>This method is a shortcut for the common case of reading a
		/// document from a system identifier.  It is the exact
		/// equivalent of the following:</p>
		/// 
		/// <pre>
		/// parse(new InputSource(systemId));
		/// </pre>
		/// 
		/// <p>If the system identifier is a URL, it must be fully resolved
		/// by the application before it is passed to the parser.</p>
		/// 
		/// </summary>
		/// <param name="systemId">The system identifier (URI).
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <exception cref="java.io.IOException">An IO exception from the parser,
		/// possibly from a byte stream or character stream
		/// supplied by the application.
		/// </exception>
		/// <seealso cref="parse(org.Xml.Sax.InputSource)">
		/// </seealso>
		void  parse(System.String systemId);
	}
	
	// end of Parser.java
}