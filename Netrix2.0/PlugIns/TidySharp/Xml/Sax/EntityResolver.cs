// SAX entity resolver.
// No warranty; no copyright -- use this as you will.
// $Id: EntityResolver.java,v 1.1 2000/11/07 02:14:45 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	
	/// <summary> Basic interface for resolving entities.
	/// 
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p>If a SAX application needs to implement customized handling
	/// for external entities, it must implement this interface and
	/// register an instance with the SAX driver using the
	/// {@link org.Xml.Sax.XMLReader#setEntityResolver setEntityResolver}
	/// method.</p>
	/// 
	/// <p>The XML reader will then allow the application to intercept any
	/// external entities (including the external DTD subset and external
	/// parameter entities, if any) before including them.</p>
	/// 
	/// <p>Many SAX applications will not need to implement this interface,
	/// but it will be especially useful for applications that build
	/// XML documents from databases or other specialised input sources,
	/// or for applications that use URI types other than URLs.</p>
	/// 
	/// <p>The following resolver would provide the application
	/// with a special character stream for the entity with the system
	/// identifier "http://www.myhost.com/today":</p>
	/// 
	/// <pre>
	/// import Comzept.Genesis.Tidy.Xml.Sax.EntityResolver;
	/// import Comzept.Genesis.Tidy.Xml.Sax.InputSource;
	/// 
	/// public class MyResolver implements EntityResolver {
	/// public InputSource resolveEntity (String publicId, String systemId)
	/// {
	/// if (systemId.equals("http://www.myhost.com/today")) {
	/// // return a special input source
	/// MyReader reader = new MyReader();
	/// return new InputSource(reader);
	/// } else {
	/// // use the default behaviour
	/// return null;
	/// }
	/// }
	/// }
	/// </pre>
	/// 
	/// <p>The application can also use this interface to redirect system
	/// identifiers to local URIs or to look up replacements in a catalog
	/// (possibly by using the public identifier).</p>
	/// 
	/// </summary>
	/// <since> SAX 1.0
	/// </since>
	/// <author>  David Megginson, 
	/// <a href="mailto:sax@megginson.com">sax@megginson.com</a>
	/// </author>
	/// <version>  2.0
	/// </version>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Parser.setEntityResolver">
	/// </seealso>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.InputSource">
	/// </seealso>
	public interface EntityResolver
	{
		
		
		/// <summary> Allow the application to resolve external entities.
		/// 
		/// <p>The Parser will call this method before opening any external
		/// entity except the top-level document entity (including the
		/// external DTD subset, external entities referenced within the
		/// DTD, and external entities referenced within the document
		/// element): the application may request that the parser resolve
		/// the entity itself, that it use an alternative URI, or that it
		/// use an entirely different input source.</p>
		/// 
		/// <p>Application writers can use this method to redirect external
		/// system identifiers to secure and/or local URIs, to look up
		/// public identifiers in a catalogue, or to read an entity from a
		/// database or other input source (including, for example, a dialog
		/// box).</p>
		/// 
		/// <p>If the system identifier is a URL, the SAX parser must
		/// resolve it fully before reporting it to the application.</p>
		/// 
		/// </summary>
		/// <param name="publicId">The public identifier of the external entity
		/// being referenced, or null if none was supplied.
		/// </param>
		/// <param name="systemId">The system identifier of the external entity
		/// being referenced.
		/// </param>
		/// <returns> An InputSource object describing the new input source,
		/// or null to request that the parser open a regular
		/// URI connection to the system identifier.
		/// </returns>
		/// <exception cref="Comzept.Genesis.Tidy.Xml.Sax.SAXException">Any SAX exception, possibly
		/// wrapping another exception.
		/// </exception>
		/// <exception cref="java.io.IOException">A Java-specific IO exception,
		/// possibly the result of creating a new InputStream
		/// or Reader for the InputSource.
		/// </exception>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.InputSource">
		/// </seealso>
		InputSource resolveEntity(System.String publicId, System.String systemId);
	}
	
	// end of EntityResolver.java
}