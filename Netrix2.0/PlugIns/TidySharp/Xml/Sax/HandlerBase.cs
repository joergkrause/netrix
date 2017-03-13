// SAX default handler base class.
// No warranty; no copyright -- use this as you will.
// $Id: HandlerBase.java,v 1.1 2000/11/07 05:10:22 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	/// <summary> Default base class for handlers.
	/// 
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p>This class implements the default behaviour for four SAX1
	/// interfaces: EntityResolver, DTDHandler, DocumentHandler,
	/// and ErrorHandler.  It is now obsolete, but is included in SAX2 to
	/// support legacy SAX1 applications.  SAX2 applications should use
	/// the {@link org.Xml.Sax.helpers.DefaultHandler DefaultHandler}
	/// class instead.</p>
	/// 
	/// <p>Application writers can extend this class when they need to
	/// implement only part of an interface; parser writers can
	/// instantiate this class to provide default handlers when the
	/// application has not supplied its own.</p>
	/// 
	/// <p>Note that the use of this class is optional.</p>
	/// 
	/// </summary>
	/// <deprecated> This class works with the deprecated
	/// {@link org.Xml.Sax.DocumentHandler DocumentHandler}
	/// interface.  It has been replaced by the SAX2
	/// {@link org.Xml.Sax.helpers.DefaultHandler DefaultHandler}
	/// class.
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
	public class HandlerBase : EntityResolver, DTDHandler, DocumentHandler, ErrorHandler
	{
		virtual public Locator DocumentLocator
		{
			set
			{
				// no op
			}
			
		}
		
		public virtual InputSource resolveEntity(System.String publicId, System.String systemId)
		{
			return null;
		}
		
		
		public virtual void  notationDecl(System.String name, System.String publicId, System.String systemId)
		{
			// no op
		}
		
		
		/// <summary> Receive notification of an unparsed entity declaration.
		/// 
		/// <p>By default, do nothing.  Application writers may override this
		/// method in a subclass to keep track of the unparsed entities
		/// declared in a document.</p>
		/// 
		/// </summary>
		/// <param name="name">The entity name.
		/// </param>
		/// <param name="publicId">The entity public identifier, or null if not
		/// available.
		/// </param>
		/// <param name="systemId">The entity system identifier.
		/// </param>
		/// <param name="notationName">The name of the associated notation.
		/// </param>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.DTDHandler.unparsedEntityDecl">
		/// </seealso>
		public virtual void  unparsedEntityDecl(System.String name, System.String publicId, System.String systemId, System.String notationName)
		{
			// no op
		}
		
		
		/// <summary> Receive notification of the beginning of the document.
		/// 
		/// <p>By default, do nothing.  Application writers may override this
		/// method in a subclass to take specific actions at the beginning
		/// of a document (such as allocating the root node of a tree or
		/// creating an output file).</p>
		/// 
		/// </summary>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.DocumentHandler.startDocument">
		/// </seealso>
		public virtual void  startDocument()
		{
			// no op
		}
		
		
		/// <summary> Receive notification of the end of the document.
		/// 
		/// <p>By default, do nothing.  Application writers may override this
		/// method in a subclass to take specific actions at the beginning
		/// of a document (such as finalising a tree or closing an output
		/// file).</p>
		/// 
		/// </summary>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.DocumentHandler.endDocument">
		/// </seealso>
		public virtual void  endDocument()
		{
			// no op
		}
		
		
		/// <summary> Receive notification of the start of an element.
		/// 
		/// <p>By default, do nothing.  Application writers may override this
		/// method in a subclass to take specific actions at the start of
		/// each element (such as allocating a new tree node or writing
		/// output to a file).</p>
		/// 
		/// </summary>
		/// <param name="name">The element type name.
		/// </param>
		/// <param name="attributes">The specified or defaulted attributes.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.DocumentHandler.startElement">
		/// </seealso>
		public virtual void  startElement(System.String name, AttributeList attributes)
		{
			// no op
		}
		
		
		/// <summary> Receive notification of the end of an element.
		/// 
		/// <p>By default, do nothing.  Application writers may override this
		/// method in a subclass to take specific actions at the end of
		/// each element (such as finalising a tree node or writing
		/// output to a file).</p>
		/// 
		/// </summary>
		/// <param name="name">The element type name.
		/// </param>
		/// <param name="attributes">The specified or defaulted attributes.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.DocumentHandler.endElement">
		/// </seealso>
		public virtual void  endElement(System.String name)
		{
			// no op
		}
		
		
		/// <summary> Receive notification of character data inside an element.
		/// 
		/// <p>By default, do nothing.  Application writers may override this
		/// method to take specific actions for each chunk of character data
		/// (such as adding the data to a node or buffer, or printing it to
		/// a file).</p>
		/// 
		/// </summary>
		/// <param name="ch">The characters.
		/// </param>
		/// <param name="start">The start position in the character array.
		/// </param>
		/// <param name="length">The number of characters to use from the
		/// character array.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.DocumentHandler.characters">
		/// </seealso>
		public virtual void  characters(char[] ch, int start, int length)
		{
			// no op
		}
		
		
		/// <summary> Receive notification of ignorable whitespace in element content.
		/// 
		/// <p>By default, do nothing.  Application writers may override this
		/// method to take specific actions for each chunk of ignorable
		/// whitespace (such as adding data to a node or buffer, or printing
		/// it to a file).</p>
		/// 
		/// </summary>
		/// <param name="ch">The whitespace characters.
		/// </param>
		/// <param name="start">The start position in the character array.
		/// </param>
		/// <param name="length">The number of characters to use from the
		/// character array.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.DocumentHandler.ignorableWhitespace">
		/// </seealso>
		public virtual void  ignorableWhitespace(char[] ch, int start, int length)
		{
			// no op
		}
		
		
		/// <summary> Receive notification of a processing instruction.
		/// 
		/// <p>By default, do nothing.  Application writers may override this
		/// method in a subclass to take specific actions for each
		/// processing instruction, such as setting status variables or
		/// invoking other methods.</p>
		/// 
		/// </summary>
		/// <param name="target">The processing instruction target.
		/// </param>
		/// <param name="data">The processing instruction data, or null if
		/// none is supplied.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.DocumentHandler.processingInstruction">
		/// </seealso>
		public virtual void  processingInstruction(System.String target, System.String data)
		{
			// no op
		}
		
		
		public virtual void  warning(SAXParseException e)
		{
			// no op
		}
		
		
		/// <summary> Receive notification of a recoverable parser error.
		/// 
		/// <p>The default implementation does nothing.  Application writers
		/// may override this method in a subclass to take specific actions
		/// for each error, such as inserting the message in a log file or
		/// printing it to the console.</p>
		/// 
		/// </summary>
		/// <param name="e">The warning information encoded as an exception.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.ErrorHandler.warning">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.SAXParseException">
		/// </seealso>
		public virtual void  error(SAXParseException e)
		{
			// no op
		}
		
		
		/// <summary> Report a fatal XML parsing error.
		/// 
		/// <p>The default implementation throws a SAXParseException.
		/// Application writers may override this method in a subclass if
		/// they need to take specific actions for each fatal error (such as
		/// collecting all of the errors into a single report): in any case,
		/// the application must stop all regular processing when this
		/// method is invoked, since the document is no longer reliable, and
		/// the parser may no longer report parsing events.</p>
		/// 
		/// </summary>
		/// <param name="e">The error information encoded as an exception.
		/// </param>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.ErrorHandler.fatalError">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.SAXParseException">
		/// </seealso>
		public virtual void  fatalError(SAXParseException e)
		{
			throw e;
		}
	}
	
	// end of HandlerBase.java
}