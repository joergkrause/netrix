// SAX input source.
// No warranty; no copyright -- use this as you will.
// $Id: InputSource.java,v 1.2 2000/11/07 05:16:47 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	/// <summary> A single input source for an XML entity.</summary>
	/// <remarks>
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p>This class allows a SAX application to encapsulate information
	/// about an input source in a single object, which may include
	/// a public identifier, a system identifier, a byte stream (possibly
	/// with a specified encoding), and/or a character stream.</p>
	/// 
	/// <p>There are two places that the application will deliver this
	/// input source to the parser: as the argument to the Parser.parse
	/// method, or as the return value of the EntityResolver.resolveEntity
	/// method.</p>
	/// 
	/// <p>The SAX parser will use the InputSource object to determine how
	/// to read XML input.  If there is a character stream available, the
	/// parser will read that stream directly; if not, the parser will use
	/// a byte stream, if available; if neither a character stream nor a
	/// byte stream is available, the parser will attempt to open a URI
	/// connection to the resource identified by the system
	/// identifier.</p>
	/// 
	/// <p>An InputSource object belongs to the application: the SAX parser
	/// shall never modify it in any way (it may modify a copy if 
	/// necessary).</p>
    /// </remarks>
	/// 
	/// <since> SAX 1.0
	/// </since>
	/// <author>  David Megginson, 
	/// <a href="mailto:sax@megginson.com">sax@megginson.com</a>
	/// </author>
	/// <version>  2.0
	/// </version>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Parser.Parse">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.EntityResolver.resolveEntity">
	/// </seealso>
	public class InputSource
	{
		/// <summary> Get the public identifier for this input source.</summary>
		/// <remarks>
		/// <p>The public identifier is always optional: if the application
		/// writer includes one, it will be provided as part of the
		/// location information.</p>
        /// </remarks>
        /// <returns> The public identifier, or null if none was supplied.
        /// </returns>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Locator.PublicId">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.SAXParseException.PublicId">
		/// </seealso>
		virtual public System.String PublicId
		{
			get
			{
				return publicId;
			}
			
			set
			{
				this.publicId = value;
			}
			
		}
		/// <summary> Get the system identifier for this input source.</summary>
		/// <remarks>
		/// <p>The getEncoding method will return the character encoding
		/// of the object pointed to, or null if unknown.</p>
		/// <p>If the system ID is a URL, it will be fully resolved.</p>
		/// Set the system identifier for this input source.
		/// <p>The system identifier is optional if there is a byte stream
		/// or a character stream, but it is still useful to provide one,
		/// since the application can use it to resolve relative URIs
		/// and can include it in error messages and warnings (the parser
		/// will attempt to open a connection to the URI only if
		/// there is no byte stream or character stream specified).</p>
		/// 
		/// <p>If the application knows the character encoding of the
		/// object pointed to by the system identifier, it can register
		/// the encoding using the setEncoding method.</p>
		/// 
		/// <p>If the system ID is a URL, it must be fully resolved.</p>
		/// </remarks>
        /// <returns> The system identifier.
        /// </returns>
        /// <seealso cref="Encoding">
        /// </seealso>
        /// <seealso cref="Encoding">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Locator.SystemId">
		/// </seealso>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.SAXParseException.SystemId">
		/// </seealso>
		virtual public System.String SystemId
		{
			get
			{
				return systemId;
			}
			
			set
			{
				this.systemId = value;
			}
			
		}

		/// <summary> Get the byte stream for this input source.</summary>
		/// <remarks>
		/// <p>The getEncoding method will return the character
		/// encoding for this byte stream, or null if unknown.</p>
		/// 
		/// 
        /// Set the byte stream for this input source.
		/// 
		/// <p>The SAX parser will ignore this if there is also a character
		/// stream specified, but it will use a byte stream in preference
		/// to opening a URI connection itself.</p>
		/// 
		/// <p>If the application knows the character encoding of the
		/// byte stream, it should set it with the setEncoding method.</p>
		/// </remarks>
        /// <returns> The byte stream, or null if none was supplied.
        /// </returns>
        /// <seealso cref="Encoding">
        /// </seealso>
        /// <seealso cref="Encoding">
		/// </seealso>
		/// <seealso cref="Encoding">
		/// </seealso>
		virtual public System.IO.Stream ByteStream
		{
			get
			{
				return byteStream;
			}
			
			set
			{
				this.byteStream = value;
			}
			
		}
		/// <summary> Get the character encoding for a byte stream or URI.</summary>
		/// <remarks> Set the character encoding, if known.
		/// 
		/// <p>The encoding must be a string acceptable for an
		/// XML encoding declaration (see section 4.3.3 of the XML 1.0
		/// recommendation).</p>
		/// 
		/// <p>This method has no effect when the application provides a
		/// character stream.</p>
		/// 
        /// </remarks>
        /// <returns> The encoding, or null if none was supplied.
        /// </returns>
        /// <seealso cref="ByteStream">
        /// </seealso>
        /// <seealso cref="SystemId">
        /// </seealso>
        /// <seealso cref="ByteStream">
        /// </seealso>
        /// <seealso cref="SystemId">
		/// </seealso>
		/// <seealso cref="ByteStream">
		/// </seealso>
		virtual public System.String Encoding
		{
			get
			{
				return encoding;
			}
			
			set
			{
				this.encoding = value;
			}
			
		}

		/// <summary> Get the character stream for this input source.</summary>
		/// <remarks>
		/// <p>If there is a character stream specified, the SAX parser
		/// will ignore any byte stream and will not attempt to open
		/// a URI connection to the system identifier.</p>
        /// </remarks>
        /// <returns> The character stream, or null if none was supplied.
        /// </returns>
		virtual public System.IO.StreamReader CharacterStream
		{
			get
			{
				return characterStream;
			}
			
			set
			{
				this.characterStream = value;
			}
			
		}
		
		/// <summary> Zero-argument default constructor.
		/// 
		/// </summary>
		/// <seealso cref="PublicId">
		/// </seealso>
		/// <seealso cref="SystemId">
		/// </seealso>
		/// <seealso cref="ByteStream">
		/// </seealso>
		/// <seealso cref="CharacterStream">
		/// </seealso>
		/// <seealso cref="Encoding">
		/// </seealso>
		public InputSource()
		{
		}
		
		
		/// <summary> Create a new input source with a system identifier.</summary>
		/// <remarks>
		/// <p>Applications may use setPublicId to include a 
		/// public identifier as well, or setEncoding to specify
		/// the character encoding, if known.</p>
		/// 
		/// <p>If the system identifier is a URL, it must be full resolved.</p>
		/// </remarks>
		/// 
		/// <param name="systemId">The system identifier (URI).
		/// </param>
		/// <seealso cref="PublicId">
		/// </seealso>
		/// <seealso cref="SystemId">
		/// </seealso>
		/// <seealso cref="ByteStream">
		/// </seealso>
		/// <seealso cref="Encoding">
		/// </seealso>
		/// <seealso cref="CharacterStream">
		/// </seealso>
		public InputSource(System.String systemId)
		{
			SystemId = systemId;
		}
		
		
		/// <summary> Create a new input source with a byte stream.
		/// 
		/// <p>Application writers may use setSystemId to provide a base 
		/// for resolving relative URIs, setPublicId to include a 
		/// public identifier, and/or setEncoding to specify the object's
		/// character encoding.</p>
		/// 
		/// </summary>
		/// <param name="byteStream">The raw byte stream containing the document.
		/// </param>
		/// <seealso cref="PublicId">
		/// </seealso>
		/// <seealso cref="SystemId">
		/// </seealso>
		/// <seealso cref="Encoding">
		/// </seealso>
		/// <seealso cref="ByteStream">
		/// </seealso>
		/// <seealso cref="CharacterStream">
		/// </seealso>
		public InputSource(System.IO.Stream byteStream)
		{
			ByteStream = byteStream;
		}
		
		
		/// <summary> Create a new input source with a character stream.
		/// 
		/// <p>Application writers may use setSystemId() to provide a base 
		/// for resolving relative URIs, and setPublicId to include a 
		/// public identifier.</p>
		/// 
		/// <p>The character stream shall not include a byte order mark.</p>
		/// 
		/// </summary>
		/// <seealso cref="PublicId">
		/// </seealso>
		/// <seealso cref="SystemId">
		/// </seealso>
		/// <seealso cref="ByteStream">
		/// </seealso>
		/// <seealso cref="CharacterStream">
		/// </seealso>
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		public InputSource(System.IO.StreamReader characterStream)
		{
			CharacterStream = characterStream;
		}
		
		
		
		////////////////////////////////////////////////////////////////////
		// Internal state.
		////////////////////////////////////////////////////////////////////
		
		private System.String publicId;
		private System.String systemId;
		private System.IO.Stream byteStream;
		private System.String encoding;
		//UPGRADE_ISSUE: Class hierarchy differences between 'java.io.Reader' and 'System.IO.StreamReader' may cause compilation errors. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1186'"
		private System.IO.StreamReader characterStream;
	}
	
	// end of InputSource.java
}