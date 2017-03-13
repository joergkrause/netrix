// SAX exception class.
// No warranty; no copyright -- use this as you will.
// $Id: SAXParseException.java,v 1.1 2000/11/14 16:49:04 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	/// <summary> Encapsulate an XML parse error or warning.
	/// 
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p>This exception will include information for locating the error
	/// in the original XML document.  Note that although the application
	/// will receive a SAXParseException as the argument to the handlers
	/// in the {@link org.Xml.Sax.ErrorHandler ErrorHandler} interface, 
	/// the application is not actually required to throw the exception; 
	/// instead, it can simply read the information in it and take a 
	/// different action.</p>
	/// 
	/// <p>Since this exception is a subclass of {@link org.Xml.Sax.SAXException 
	/// SAXException}, it inherits the ability to wrap another exception.</p>
	/// 
	/// </summary>
	/// <since> SAX 1.0
	/// </since>
	/// <author>  David Megginson, 
	/// <a href="mailto:sax@megginson.com">sax@megginson.com</a>
	/// </author>
	/// <version>  2.0
	/// </version>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Locator">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.ErrorHandler">
	/// </seealso>
	[Serializable]
	public class SAXParseException:SAXException
	{
		/// <summary> Get the public identifier of the entity where the exception occurred.
		/// 
		/// </summary>
		/// <returns> A string containing the public identifier, or null
		/// if none is available.
		/// </returns>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Locator.PublicId">
		/// </seealso>
		virtual public System.String PublicId
		{
			get
			{
				return this.publicId;
			}
			
		}
		/// <summary> Get the system identifier of the entity where the exception occurred.
		/// 
		/// <p>If the system identifier is a URL, it will be resolved
		/// fully.</p>
		/// 
		/// </summary>
		/// <returns> A string containing the system identifier, or null
		/// if none is available.
		/// </returns>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Locator.SystemId">
		/// </seealso>
		virtual public System.String SystemId
		{
			get
			{
				return this.systemId;
			}
			
		}
		/// <summary> The line number of the end of the text where the exception occurred.
		/// 
		/// </summary>
		/// <returns> An integer representing the line number, or -1
		/// if none is available.
		/// </returns>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Locator.LineNumber">
		/// </seealso>
		virtual public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
			
		}
		/// <summary> The column number of the end of the text where the exception occurred.
		/// 
		/// <p>The first column in a line is position 1.</p>
		/// 
		/// </summary>
		/// <returns> An integer representing the column number, or -1
		/// if none is available.
		/// </returns>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Locator.ColumnNumber">
		/// </seealso>
		virtual public int ColumnNumber
		{
			get
			{
				return this.columnNumber;
			}
			
		}
		
		public SAXParseException(System.String message, Locator locator):base(message)
		{
			if (locator != null)
			{
				init(locator.PublicId, locator.SystemId, locator.LineNumber, locator.ColumnNumber);
			}
			else
			{
				init(null, null, - 1, - 1);
			}
		}
		
		
		/// <summary> Wrap an existing exception in a SAXParseException.
		/// 
		/// <p>This constructor is especially useful when an application is
		/// creating its own exception from within a {@link org.Xml.Sax.ContentHandler
		/// ContentHandler} callback, and needs to wrap an existing exception that is not a
		/// subclass of {@link org.Xml.Sax.SAXException SAXException}.</p>
		/// 
		/// </summary>
		/// <param name="message">The error or warning message, or null to
		/// use the message from the embedded exception.
		/// </param>
		/// <param name="locator">The locator object for the error or warning (may be
		/// null).
		/// </param>
		/// <param name="e">Any exception.
		/// </param>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Locator">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Parser.Locale">
		/// </seealso>
		public SAXParseException(System.String message, Locator locator, System.Exception e):base(message, e)
		{
			if (locator != null)
			{
				init(locator.PublicId, locator.SystemId, locator.LineNumber, locator.ColumnNumber);
			}
			else
			{
				init(null, null, - 1, - 1);
			}
		}
		
		
		/// <summary> Create a new SAXParseException.
		/// 
		/// <p>This constructor is most useful for parser writers.</p>
		/// 
		/// <p>If the system identifier is a URL, the parser must resolve it
		/// fully before creating the exception.</p>
		/// 
		/// </summary>
		/// <param name="message">The error or warning message.
		/// </param>
		/// <param name="publicId">The public identifer of the entity that generated
		/// the error or warning.
		/// </param>
		/// <param name="systemId">The system identifer of the entity that generated
		/// the error or warning.
		/// </param>
		/// <param name="lineNumber">The line number of the end of the text that
		/// caused the error or warning.
		/// </param>
		/// <param name="columnNumber">The column number of the end of the text that
		/// cause the error or warning.
		/// </param>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Parser.setLocale">
		/// </seealso>
		public SAXParseException(System.String message, System.String publicId, System.String systemId, int lineNumber, int columnNumber):base(message)
		{
			init(publicId, systemId, lineNumber, columnNumber);
		}
		
		
		/// <summary> Create a new SAXParseException with an embedded exception.
		/// 
		/// <p>This constructor is most useful for parser writers who
		/// need to wrap an exception that is not a subclass of
		/// {@link org.Xml.Sax.SAXException SAXException}.</p>
		/// 
		/// <p>If the system identifier is a URL, the parser must resolve it
		/// fully before creating the exception.</p>
		/// 
		/// </summary>
		/// <param name="message">The error or warning message, or null to use
		/// the message from the embedded exception.
		/// </param>
		/// <param name="publicId">The public identifer of the entity that generated
		/// the error or warning.
		/// </param>
		/// <param name="systemId">The system identifer of the entity that generated
		/// the error or warning.
		/// </param>
		/// <param name="lineNumber">The line number of the end of the text that
		/// caused the error or warning.
		/// </param>
		/// <param name="columnNumber">The column number of the end of the text that
		/// cause the error or warning.
		/// </param>
		/// <param name="e">Another exception to embed in this one.
		/// </param>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Parser.setLocale">
		/// </seealso>
		public SAXParseException(System.String message, System.String publicId, System.String systemId, int lineNumber, int columnNumber, System.Exception e):base(message, e)
		{
			init(publicId, systemId, lineNumber, columnNumber);
		}
		
		
		/// <summary> Internal initialization method.
		/// 
		/// </summary>
		/// <param name="publicId">The public identifier of the entity which generated the exception,
		/// or null.
		/// </param>
		/// <param name="systemId">The system identifier of the entity which generated the exception,
		/// or null.
		/// </param>
		/// <param name="lineNumber">The line number of the error, or -1.
		/// </param>
		/// <param name="columnNumber">The column number of the error, or -1.
		/// </param>
		private void  init(System.String publicId, System.String systemId, int lineNumber, int columnNumber)
		{
			this.publicId = publicId;
			this.systemId = systemId;
			this.lineNumber = lineNumber;
			this.columnNumber = columnNumber;
		}
		
		
		private System.String publicId;
		
		
		/// <serial> The system identifier, or null.
		/// </serial>
		/// <seealso cref="SystemId">
		/// </seealso>
		private System.String systemId;
		
		
		/// <serial> The line number, or -1.
		/// </serial>
		/// <seealso cref="LineNumber">
		/// </seealso>
		private int lineNumber;
		
		
		/// <serial> The column number, or -1.
		/// </serial>
		/// <seealso cref="ColumnNumber">
		/// </seealso>
		private int columnNumber;
	}
	
	// end of SAXParseException.java
}