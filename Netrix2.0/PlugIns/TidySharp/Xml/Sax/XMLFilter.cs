// XMLFilter.java - filter SAX2 events.
// Written by David Megginson, sax@megginson.com
// NO WARRANTY!  This class is in the Public Domain.

// $Id: XMLFilter.java,v 1.1 2000/11/14 16:49:04 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	
	/// <summary> Interface for an XML filter.
	/// 
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p>An XML filter is like an XML reader, except that it obtains its
	/// events from another XML reader rather than a primary source like
	/// an XML document or database.  Filters can modify a stream of
	/// events as they pass on to the final application.</p>
	/// 
	/// <p>The XMLFilterImpl helper class provides a convenient base
	/// for creating SAX2 filters, by passing on all {@link org.Xml.Sax.EntityResolver
	/// EntityResolver}, {@link org.Xml.Sax.DTDHandler DTDHandler},
	/// {@link org.Xml.Sax.ContentHandler ContentHandler} and {@link org.Xml.Sax.ErrorHandler
	/// ErrorHandler} events automatically.</p>
	/// 
	/// </summary>
	/// <since> SAX 2.0
	/// </since>
	/// <author>  David Megginson, 
	/// <a href="mailto:sax@megginson.com">sax@megginson.com</a>
	/// </author>
	/// <version>  2.0
	/// </version>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.helpers.XMLFilterImpl">
	/// </seealso>
	public interface XMLFilter:XMLReader
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Get the parent reader.
		/// 
		/// <p>This method allows the application to query the parent
		/// reader (which may be another filter).  It is generally a
		/// bad idea to perform any operations on the parent reader
		/// directly: they should all pass through this filter.</p>
		/// 
		/// </summary>
		/// <returns> The parent filter, or null if none has been set.
		/// </returns>
		/// <summary> Set the parent reader.
		/// 
		/// <p>This method allows the application to link the filter to
		/// a parent reader (which may be another filter).  The argument
		/// may not be null.</p>
		/// 
		/// </summary>
		/// <param name="parent">The parent reader.
		/// </param>
		XMLReader Parent
		{
			get;
			
			set;
			
		}
	}
	
	// end of XMLFilter.java
}