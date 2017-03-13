// ContentHandler.java - handle main document content.
// Written by David Megginson, sax@megginson.com
// NO WARRANTY!  This class is in the public domain.

// $Id: ContentHandler.java,v 1.1 2000/11/06 15:47:43 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	
	/// <summary> Receive notification of the logical content of a document.
	/// 
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p>This is the main interface that most SAX applications
	/// implement: if the application needs to be informed of basic parsing 
	/// events, it implements this interface and registers an instance with 
	/// the SAX parser using the {@link org.Xml.Sax.XMLReader#setContentHandler 
	/// setContentHandler} method.  The parser uses the instance to report 
	/// basic document-related events like the start and end of elements 
	/// and character data.</p>
	/// 
	/// <p>The order of events in this interface is very important, and
	/// mirrors the order of information in the document itself.  For
	/// example, all of an element's content (character data, processing
	/// instructions, and/or subelements) will appear, in order, between
	/// the startElement event and the corresponding endElement event.</p>
	/// 
	/// <p>This interface is similar to the now-deprecated SAX 1.0
	/// DocumentHandler interface, but it adds support for Namespaces
	/// and for reporting skipped entities (in non-validating XML
	/// processors).</p>
	/// 
	/// <p>Implementors should note that there is also a Java class
	/// {@link java.net.ContentHandler ContentHandler} in the java.net
	/// package; that means that it's probably a bad idea to do</p>
	/// 
	/// <blockquote>
	/// import java.net.*;
	/// import Comzept.Genesis.Tidy.Xml.Sax.*;
	/// </blockquote>
	/// 
	/// <p>In fact, "import ...*" is usually a sign of sloppy programming
	/// anyway, so the user should consider this a feature rather than a
	/// bug.</p>
	/// 
	/// </summary>
	/// <since> SAX 2.0
	/// </since>
	/// <author>  David Megginson, 
	/// <a href="mailto:sax@megginson.com">sax@megginson.com</a>
	/// </author>
	/// <version>  2.0
	/// </version>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.XMLReader">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.DTDHandler">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.ErrorHandler">
	/// </seealso>
	public interface ContentHandler
	{
		/// <summary> Receive an object for locating the origin of SAX document events.
		/// 
		/// <p>SAX parsers are strongly encouraged (though not absolutely
		/// required) to supply a locator: if it does so, it must supply
		/// the locator to the application by invoking this method before
		/// invoking any of the other methods in the ContentHandler
		/// interface.</p>
		/// 
		/// <p>The locator allows the application to determine the end
		/// position of any document-related event, even if the parser is
		/// not reporting an error.  Typically, the application will
		/// use this information for reporting its own errors (such as
		/// character content that does not match an application's
		/// business rules).  The information returned by the locator
		/// is probably not sufficient for use with a search engine.</p>
		/// 
		/// <p>Note that the locator will return correct information only
		/// during the invocation of the events in this interface.  The
		/// application should not attempt to use it at any other time.</p>
		/// 
		/// </summary>
		/// <param name="locator">An object that can return the location of
		/// any SAX document event.
		/// </param>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Locator">
		/// </seealso>
		Locator DocumentLocator
		{
			set;
			
		}
		
		
		/// <summary> Receive notification of the beginning of a document.
		/// 
		/// <p>The SAX parser will invoke this method only once, before any
		/// other methods in this interface or in {@link org.Xml.Sax.DTDHandler
		/// DTDHandler} (except for {@link #setDocumentLocator 
		/// setDocumentLocator}).</p>
		/// 
		/// </summary>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="endDocument">
		/// </seealso>
		void  startDocument();
		
		
		/// <summary> Receive notification of the end of a document.
		/// 
		/// <p>The SAX parser will invoke this method only once, and it will
		/// be the last method invoked during the parse.  The parser shall
		/// not invoke this method until it has either abandoned parsing
		/// (because of an unrecoverable error) or reached the end of
		/// input.</p>
		/// 
		/// </summary>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="startDocument">
		/// </seealso>
		void  endDocument();
		
		
		/// <summary> Begin the scope of a prefix-URI Namespace mapping.
		/// 
		/// <p>The information from this event is not necessary for
		/// normal Namespace processing: the SAX XML reader will 
		/// automatically replace prefixes for element and attribute
		/// names when the <code>http://xml.org/sax/features/namespaces</code>
		/// feature is <var>true</var> (the default).</p>
		/// 
		/// <p>There are cases, however, when applications need to
		/// use prefixes in character data or in attribute values,
		/// where they cannot safely be expanded automatically; the
		/// start/endPrefixMapping event supplies the information
		/// to the application to expand prefixes in those contexts
		/// itself, if necessary.</p>
		/// 
		/// <p>Note that start/endPrefixMapping events are not
		/// guaranteed to be properly nested relative to each-other:
		/// all startPrefixMapping events will occur before the
		/// corresponding {@link #startElement startElement} event, 
		/// and all {@link #endPrefixMapping endPrefixMapping}
		/// events will occur after the corresponding {@link #endElement
		/// endElement} event, but their order is not otherwise 
		/// guaranteed.</p>
		/// 
		/// <p>There should never be start/endPrefixMapping events for the
		/// "xml" prefix, since it is predeclared and immutable.</p>
		/// 
		/// </summary>
		/// <param name="prefix">The Namespace prefix being declared.
		/// </param>
		/// <param name="uri">The Namespace URI the prefix is mapped to.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">The client may throw
		/// an exception during processing.
		/// </exception>
		/// <seealso cref="endPrefixMapping">
		/// </seealso>
		/// <seealso cref="startElement">
		/// </seealso>
		void  startPrefixMapping(System.String prefix, System.String uri);
		
		
		/// <summary> End the scope of a prefix-URI mapping.
		/// 
		/// <p>See {@link #startPrefixMapping startPrefixMapping} for 
		/// details.  This event will always occur after the corresponding 
		/// {@link #endElement endElement} event, but the order of 
		/// {@link #endPrefixMapping endPrefixMapping} events is not otherwise
		/// guaranteed.</p>
		/// 
		/// </summary>
		/// <param name="prefix">The prefix that was being mapping.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">The client may throw
		/// an exception during processing.
		/// </exception>
		/// <seealso cref="startPrefixMapping">
		/// </seealso>
		/// <seealso cref="endElement">
		/// </seealso>
		void  endPrefixMapping(System.String prefix);
		
		
		/// <summary> Receive notification of the beginning of an element.
		/// 
		/// <p>The Parser will invoke this method at the beginning of every
		/// element in the XML document; there will be a corresponding
		/// {@link #endElement endElement} event for every startElement event
		/// (even when the element is empty). All of the element's content will be
		/// reported, in order, before the corresponding endElement
		/// event.</p>
		/// 
		/// <p>This event allows up to three name components for each
		/// element:</p>
		/// 
		/// <ol>
		/// <li>the Namespace URI;</li>
		/// <li>the local name; and</li>
		/// <li>the qualified (prefixed) name.</li>
		/// </ol>
		/// 
		/// <p>Any or all of these may be provided, depending on the
		/// values of the <var>http://xml.org/sax/features/namespaces</var>
		/// and the <var>http://xml.org/sax/features/namespace-prefixes</var>
		/// properties:</p>
		/// 
		/// <ul>
		/// <li>the Namespace URI and local name are required when 
		/// the namespaces property is <var>true</var> (the default), and are
		/// optional when the namespaces property is <var>false</var> (if one is
		/// specified, both must be);</li>
		/// <li>the qualified name is required when the namespace-prefixes property
		/// is <var>true</var>, and is optional when the namespace-prefixes property
		/// is <var>false</var> (the default).</li>
		/// </ul>
		/// 
		/// <p>Note that the attribute list provided will contain only
		/// attributes with explicit values (specified or defaulted):
		/// #IMPLIED attributes will be omitted.  The attribute list
		/// will contain attributes used for Namespace declarations
		/// (xmlns* attributes) only if the
		/// <code>http://xml.org/sax/features/namespace-prefixes</code>
		/// property is true (it is false by default, and support for a 
		/// true value is optional).</p>
		/// 
		/// </summary>
		/// <param name="uri">The Namespace URI, or the empty string if the
		/// element has no Namespace URI or if Namespace
		/// processing is not being performed.
		/// </param>
		/// <param name="localName">The local name (without prefix), or the
		/// empty string if Namespace processing is not being
		/// performed.
		/// </param>
		/// <param name="qName">The qualified name (with prefix), or the
		/// empty string if qualified names are not available.
		/// </param>
		/// <param name="atts">The attributes attached to the element.  If
		/// there are no attributes, it shall be an empty
		/// Attributes object.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="endElement">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Attributes">
		/// </seealso>
		void  startElement(System.String namespaceURI, System.String localName, System.String qName, Attributes atts);
		
		
		/// <summary> Receive notification of the end of an element.
		/// 
		/// <p>The SAX parser will invoke this method at the end of every
		/// element in the XML document; there will be a corresponding
		/// {@link #startElement startElement} event for every endElement 
		/// event (even when the element is empty).</p>
		/// 
		/// <p>For information on the names, see startElement.</p>
		/// 
		/// </summary>
		/// <param name="uri">The Namespace URI, or the empty string if the
		/// element has no Namespace URI or if Namespace
		/// processing is not being performed.
		/// </param>
		/// <param name="localName">The local name (without prefix), or the
		/// empty string if Namespace processing is not being
		/// performed.
		/// </param>
		/// <param name="qName">The qualified XML 1.0 name (with prefix), or the
		/// empty string if qualified names are not available.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		void  endElement(System.String namespaceURI, System.String localName, System.String qName);
		
		
		/// <summary> Receive notification of character data.
		/// 
		/// <p>The Parser will call this method to report each chunk of
		/// character data.  SAX parsers may return all contiguous character
		/// data in a single chunk, or they may split it into several
		/// chunks; however, all of the characters in any single event
		/// must come from the same external entity so that the Locator
		/// provides useful information.</p>
		/// 
		/// <p>The application must not attempt to read from the array
		/// outside of the specified range.</p>
		/// 
		/// <p>Note that some parsers will report whitespace in element
		/// content using the {@link #ignorableWhitespace ignorableWhitespace}
		/// method rather than this one (validating parsers <em>must</em> 
		/// do so).</p>
		/// 
		/// </summary>
		/// <param name="ch">The characters from the XML document.
		/// </param>
		/// <param name="start">The start position in the array.
		/// </param>
		/// <param name="length">The number of characters to read from the array.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="ignorableWhitespace">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Locator">
		/// </seealso>
		void  characters(char[] ch, int start, int length);
		
		
		/// <summary> Receive notification of ignorable whitespace in element content.
		/// 
		/// <p>Validating Parsers must use this method to report each chunk
		/// of whitespace in element content (see the W3C XML 1.0 recommendation,
		/// section 2.10): non-validating parsers may also use this method
		/// if they are capable of parsing and using content models.</p>
		/// 
		/// <p>SAX parsers may return all contiguous whitespace in a single
		/// chunk, or they may split it into several chunks; however, all of
		/// the characters in any single event must come from the same
		/// external entity, so that the Locator provides useful
		/// information.</p>
		/// 
		/// <p>The application must not attempt to read from the array
		/// outside of the specified range.</p>
		/// 
		/// </summary>
		/// <param name="ch">The characters from the XML document.
		/// </param>
		/// <param name="start">The start position in the array.
		/// </param>
		/// <param name="length">The number of characters to read from the array.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="characters">
		/// </seealso>
		void  ignorableWhitespace(char[] ch, int start, int length);
		
		
		/// <summary> Receive notification of a processing instruction.
		/// 
		/// <p>The Parser will invoke this method once for each processing
		/// instruction found: note that processing instructions may occur
		/// before or after the main document element.</p>
		/// 
		/// <p>A SAX parser must never report an XML declaration (XML 1.0,
		/// section 2.8) or a text declaration (XML 1.0, section 4.3.1)
		/// using this method.</p>
		/// 
		/// </summary>
		/// <param name="target">The processing instruction target.
		/// </param>
		/// <param name="data">The processing instruction data, or null if
		/// none was supplied.  The data does not include any
		/// whitespace separating it from the target.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		void  processingInstruction(System.String target, System.String data);
		
		
		/// <summary> Receive notification of a skipped entity.
		/// 
		/// <p>The Parser will invoke this method once for each entity
		/// skipped.  Non-validating processors may skip entities if they
		/// have not seen the declarations (because, for example, the
		/// entity was declared in an external DTD subset).  All processors
		/// may skip external entities, depending on the values of the
		/// <code>http://xml.org/sax/features/external-general-entities</code>
		/// and the
		/// <code>http://xml.org/sax/features/external-parameter-entities</code>
		/// properties.</p>
		/// 
		/// </summary>
		/// <param name="name">The name of the skipped entity.  If it is a 
		/// parameter entity, the name will begin with '%', and if
		/// it is the external DTD subset, it will be the string
		/// "[dtd]".
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		void  skippedEntity(System.String name);
	}
	
	// end of ContentHandler.java
}