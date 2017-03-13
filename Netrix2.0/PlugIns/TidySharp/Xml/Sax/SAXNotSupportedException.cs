// SAXNotSupportedException.java - unsupported feature or value.
// Written by David Megginson, sax@megginson.com
// NO WARRANTY!  This class is in the Public Domain.

// $Id: SAXNotSupportedException.java,v 1.1 2000/11/14 16:49:04 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	/// <summary> Exception class for an unsupported operation.
	/// 
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p>An XMLReader will throw this exception when it recognizes a
	/// feature or property identifier, but cannot perform the requested
	/// operation (setting a state or value).  Other SAX2 applications and
	/// extensions may use this class for similar purposes.</p>
	/// 
	/// </summary>
	/// <since> SAX 2.0
	/// </since>
	/// <author>  David Megginson, 
	/// <a href="mailto:sax@megginson.com">sax@megginson.com</a>
	/// </author>
	/// <version>  2.0
	/// </version>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.SAXNotRecognizedException">
	/// </seealso>
	[Serializable]
	public class SAXNotSupportedException:SAXException
	{
		
		/// <summary> Construct a new exception with the given message.
		/// 
		/// </summary>
		/// <param name="message">The text message of the exception.
		/// </param>
		public SAXNotSupportedException(System.String message):base(message)
		{
		}
	}
	
	// end of SAXNotSupportedException.java
}