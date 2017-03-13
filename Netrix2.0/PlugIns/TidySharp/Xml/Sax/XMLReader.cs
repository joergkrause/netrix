// XMLReader.java - read an XML document.
// Written by David Megginson, sax@megginson.com
// NO WARRANTY!  This class is in the Public Domain.

// $Id: XMLReader.java,v 1.1 2000/11/14 16:49:04 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	
	/// <summary> Interface for reading an XML document using callbacks.
	/// 
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p><strong>Note:</strong> despite its name, this interface does 
	/// <em>not</em> extend the standard Java {@link java.io.Reader Reader} 
	/// interface, because reading XML is a fundamentally different activity 
	/// than reading character data.</p>
	/// 
	/// <p>XMLReader is the interface that an XML parser's SAX2 driver must
	/// implement.  This interface allows an application to set and
	/// query features and properties in the parser, to register
	/// event handlers for document processing, and to initiate
	/// a document parse.</p>
	/// 
	/// <p>All SAX interfaces are assumed to be synchronous: the
	/// {@link #parse parse} methods must not return until parsing
	/// is complete, and readers must wait for an event-handler callback
	/// to return before reporting the next event.</p>
	/// 
	/// <p>This interface replaces the (now deprecated) SAX 1.0 {@link
	/// org.Xml.Sax.Parser Parser} interface.  The XMLReader interface
	/// contains two important enhancements over the old Parser
	/// interface:</p>
	/// 
	/// <ol>
	/// <li>it adds a standard way to query and set features and 
	/// properties; and</li>
	/// <li>it adds Namespace support, which is required for many
	/// higher-level XML standards.</li>
	/// </ol>
	/// 
	/// <p>There are adapters available to convert a SAX1 Parser to
	/// a SAX2 XMLReader and vice-versa.</p>
	/// 
	/// </summary>
	/// <since> SAX 2.0
	/// </since>
	/// <author>  David Megginson, 
	/// <a href="mailto:sax@megginson.com">sax@megginson.com</a>
	/// </author>
	/// <version>  2.0
	/// </version>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.XMLFilter">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.helpers.ParserAdapter">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.helpers.XMLReaderAdapter">
	/// </seealso>
	public interface XMLReader
	{
		/// <summary> Return the current entity resolver.
		/// 
		/// </summary>
		/// <returns> The current entity resolver, or null if none
		/// has been registered.
		/// </returns>
		/// <seealso cref="EntityResolver">
		/// </seealso>
		EntityResolver EntityResolver
		{
			get;
			
			set;
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Return the current DTD handler.
		/// 
		/// </summary>
		/// <returns> The current DTD handler, or null if none
		/// has been registered.
		/// </returns>
		/// <seealso cref="DTDHandler">
		/// </seealso>
		/// <summary> Allow an application to register a DTD event handler.
		/// 
		/// <p>If the application does not register a DTD handler, all DTD
		/// events reported by the SAX parser will be silently ignored.</p>
		/// 
		/// <p>Applications may register a new or different handler in the
		/// middle of a parse, and the SAX parser must begin using the new
		/// handler immediately.</p>
		/// 
		/// </summary>
		/// <param name="handler">The DTD handler.
		/// </param>
		/// <exception cref="java.lang.NullPointerException">If the handler 
		/// argument is null.
		/// </exception>
		/// <seealso cref="DTDHandler">
		/// </seealso>
		DTDHandler DTDHandler
		{
			get;
			
			set;
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Return the current content handler.
		/// 
		/// </summary>
		/// <returns> The current content handler, or null if none
		/// has been registered.
		/// </returns>
		/// <seealso cref="ContentHandler">
		/// </seealso>
		/// <summary> Allow an application to register a content event handler.
		/// 
		/// <p>If the application does not register a content handler, all
		/// content events reported by the SAX parser will be silently
		/// ignored.</p>
		/// 
		/// <p>Applications may register a new or different handler in the
		/// middle of a parse, and the SAX parser must begin using the new
		/// handler immediately.</p>
		/// 
		/// </summary>
		/// <param name="handler">The content handler.
		/// </param>
		/// <exception cref="java.lang.NullPointerException">If the handler 
		/// argument is null.
		/// </exception>
		/// <seealso cref="ContentHandler">
		/// </seealso>
		ContentHandler ContentHandler
		{
			get;
			
			set;
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Return the current error handler.
		/// 
		/// </summary>
		/// <returns> The current error handler, or null if none
		/// has been registered.
		/// </returns>
		/// <seealso cref="ErrorHandler">
		/// </seealso>
		/// <summary> Allow an application to register an error event handler.
		/// 
		/// <p>If the application does not register an error handler, all
		/// error events reported by the SAX parser will be silently
		/// ignored; however, normal processing may not continue.  It is
		/// highly recommended that all SAX applications implement an
		/// error handler to avoid unexpected bugs.</p>
		/// 
		/// <p>Applications may register a new or different handler in the
		/// middle of a parse, and the SAX parser must begin using the new
		/// handler immediately.</p>
		/// 
		/// </summary>
		/// <param name="handler">The error handler.
		/// </param>
		/// <exception cref="java.lang.NullPointerException">If the handler 
		/// argument is null.
		/// </exception>
		/// <seealso cref="ErrorHandler">
		/// </seealso>
		ErrorHandler ErrorHandler
		{
			get;
			
			set;
			
		}
		
		bool getFeature(System.String name);
		
		
		/// <summary> Set the state of a feature.
		/// 
		/// <p>The feature name is any fully-qualified URI.  It is
		/// possible for an XMLReader to recognize a feature name but
		/// to be unable to set its value; this is especially true
		/// in the case of an adapter for a SAX1 {@link org.Xml.Sax.Parser Parser},
		/// which has no way of affecting whether the underlying parser is
		/// validating, for example.</p>
		/// 
		/// <p>All XMLReaders are required to support setting
		/// http://xml.org/sax/features/namespaces to true and
		/// http://xml.org/sax/features/namespace-prefixes to false.</p>
		/// 
		/// <p>Some feature values may be immutable or mutable only 
		/// in specific contexts, such as before, during, or after 
		/// a parse.</p>
		/// 
		/// </summary>
		/// <param name="name">The feature name, which is a fully-qualified URI.
		/// </param>
		/// <param name="state">The requested state of the feature (true or false).
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXNotRecognizedException">When the
		/// XMLReader does not recognize the feature name.
		/// </exception>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXNotSupportedException">When the
		/// XMLReader recognizes the feature name but 
		/// cannot set the requested value.
		/// </exception>
		/// <seealso cref="Feature">
		/// </seealso>
		void  setFeature(System.String name, bool value_Renamed);
		
		
		/// <summary> Look up the value of a property.
		/// 
		/// <p>The property name is any fully-qualified URI.  It is
		/// possible for an XMLReader to recognize a property name but
		/// to be unable to return its state; this is especially true
		/// in the case of an adapter for a SAX1 {@link org.Xml.Sax.Parser
		/// Parser}.</p>
		/// 
		/// <p>XMLReaders are not required to recognize any specific
		/// property names, though an initial core set is documented for
		/// SAX2.</p>
		/// 
		/// <p>Some property values may be available only in specific
		/// contexts, such as before, during, or after a parse.</p>
		/// 
		/// <p>Implementors are free (and encouraged) to invent their own properties,
		/// using names built on their own URIs.</p>
		/// 
		/// </summary>
		/// <param name="name">The property name, which is a fully-qualified URI.
		/// </param>
		/// <returns> The current value of the property.
		/// </returns>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXNotRecognizedException">When the
		/// XMLReader does not recognize the property name.
		/// </exception>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXNotSupportedException">When the
		/// XMLReader recognizes the property name but 
		/// cannot determine its value at this time.
		/// </exception>
		/// <seealso cref="Property">
		/// </seealso>
		System.Object getProperty(System.String name);
		
		
		/// <summary> Set the value of a property.
		/// 
		/// <p>The property name is any fully-qualified URI.  It is
		/// possible for an XMLReader to recognize a property name but
		/// to be unable to set its value; this is especially true
		/// in the case of an adapter for a SAX1 {@link org.Xml.Sax.Parser
		/// Parser}.</p>
		/// 
		/// <p>XMLReaders are not required to recognize setting
		/// any specific property names, though a core set is provided with 
		/// SAX2.</p>
		/// 
		/// <p>Some property values may be immutable or mutable only 
		/// in specific contexts, such as before, during, or after 
		/// a parse.</p>
		/// 
		/// <p>This method is also the standard mechanism for setting
		/// extended handlers.</p>
		/// 
		/// </summary>
		/// <param name="name">The property name, which is a fully-qualified URI.
		/// </param>
		/// <param name="state">The requested value for the property.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXNotRecognizedException">When the
		/// XMLReader does not recognize the property name.
		/// </exception>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXNotSupportedException">When the
		/// XMLReader recognizes the property name but 
		/// cannot set the requested value.
		/// </exception>
		void  setProperty(System.String name, System.Object value_Renamed);
		
		
		void  parse(InputSource input);
		
		
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
	
	// end of XMLReader.java
}