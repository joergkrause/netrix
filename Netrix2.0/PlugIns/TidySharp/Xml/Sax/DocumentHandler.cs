// SAX document handler.
// No warranty; no copyright -- use this as you will.
// $Id: DocumentHandler.java,v 1.1 2000/11/06 16:18:45 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	/// <summary> Receive notification of general document events.
	/// 
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p>This was the main event-handling interface for SAX1; in
	/// SAX2, it has been replaced by {@link org.Xml.Sax.ContentHandler
	/// ContentHandler}, which provides Namespace support and reporting
	/// of skipped entities.  This interface is included in SAX2 only
	/// to support legacy SAX1 applications.</p>
	/// 
	/// <p>The order of events in this interface is very important, and
	/// mirrors the order of information in the document itself.  For
	/// example, all of an element's content (character data, processing
	/// instructions, and/or subelements) will appear, in order, between
	/// the startElement event and the corresponding endElement event.</p>
	/// 
	/// <p>Application writers who do not want to implement the entire
	/// interface can derive a class from HandlerBase, which implements
	/// the default functionality; parser writers can instantiate
	/// HandlerBase to obtain a default handler.  The application can find
	/// the location of any document event using the Locator interface
	/// supplied by the Parser through the setDocumentLocator method.</p>
	/// 
	/// </summary>
	/// <deprecated> This interface has been replaced by the SAX2
	/// {@link org.Xml.Sax.ContentHandler ContentHandler}
	/// interface, which includes Namespace support.
	/// </deprecated>
	/// <since> SAX 1.0
	/// </since>
	/// <author>  David Megginson, 
	/// <a href="mailto:sax@megginson.com">sax@megginson.com</a>
	/// </author>
	/// <version>  2.0
	/// </version>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Parser.setDocumentHandler">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Locator">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.HandlerBase">
	/// </seealso>
	public interface DocumentHandler
	{
		/// <summary> Receive an object for locating the origin of SAX document events.
		/// 
		/// <p>SAX parsers are strongly encouraged (though not absolutely
		/// required) to supply a locator: if it does so, it must supply
		/// the locator to the application by invoking this method before
		/// invoking any of the other methods in the DocumentHandler
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
		/// other methods in this interface or in DTDHandler (except for
		/// setDocumentLocator).</p>
		/// 
		/// </summary>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
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
		void  endDocument();
		
		
		/// <summary> Receive notification of the beginning of an element.
		/// 
		/// <p>The Parser will invoke this method at the beginning of every
		/// element in the XML document; there will be a corresponding
		/// endElement() event for every startElement() event (even when the
		/// element is empty). All of the element's content will be
		/// reported, in order, before the corresponding endElement()
		/// event.</p>
		/// 
		/// <p>If the element name has a namespace prefix, the prefix will
		/// still be attached.  Note that the attribute list provided will
		/// contain only attributes with explicit values (specified or
		/// defaulted): #IMPLIED attributes will be omitted.</p>
		/// 
		/// </summary>
		/// <param name="name">The element type name.
		/// </param>
		/// <param name="atts">The attributes attached to the element, if any.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="endElement">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.AttributeList">
		/// </seealso>
		void  startElement(System.String name, AttributeList atts);
		
		
		/// <summary> Receive notification of the end of an element.
		/// 
		/// <p>The SAX parser will invoke this method at the end of every
		/// element in the XML document; there will be a corresponding
		/// startElement() event for every endElement() event (even when the
		/// element is empty).</p>
		/// 
		/// <p>If the element name has a namespace prefix, the prefix will
		/// still be attached to the name.</p>
		/// 
		/// </summary>
		/// <param name="name">The element type name
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		void  endElement(System.String name);
		
		
		/// <summary> Receive notification of character data.
		/// 
		/// <p>The Parser will call this method to report each chunk of
		/// character data.  SAX parsers may return all contiguous character
		/// data in a single chunk, or they may split it into several
		/// chunks; however, all of the characters in any single event
		/// must come from the same external entity, so that the Locator
		/// provides useful information.</p>
		/// 
		/// <p>The application must not attempt to read from the array
		/// outside of the specified range.</p>
		/// 
		/// <p>Note that some parsers will report whitespace using the
		/// ignorableWhitespace() method rather than this one (validating
		/// parsers must do so).</p>
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
		/// of ignorable whitespace (see the W3C XML 1.0 recommendation,
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
		/// <p>A SAX parser should never report an XML declaration (XML 1.0,
		/// section 2.8) or a text declaration (XML 1.0, section 4.3.1)
		/// using this method.</p>
		/// 
		/// </summary>
		/// <param name="target">The processing instruction target.
		/// </param>
		/// <param name="data">The processing instruction data, or null if
		/// none was supplied.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		void  processingInstruction(System.String target, System.String data);
	}
	
	// end of DocumentHandler.java
}