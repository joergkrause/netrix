// Attributes.java - attribute list with Namespace support
// Written by David Megginson, sax@megginson.com
// NO WARRANTY!  This class is in the public domain.

// $Id: Attributes.java,v 1.1 2000/11/05 20:24:39 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	
	/// <summary> Interface for a list of XML attributes.
	/// 
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p>This interface allows access to a list of attributes in
	/// three different ways:</p>
	/// 
	/// <ol>
	/// <li>by attribute index;</li>
	/// <li>by Namespace-qualified name; or</li>
	/// <li>by qualified (prefixed) name.</li>
	/// </ol>
	/// 
	/// <p>The list will not contain attributes that were declared
	/// #IMPLIED but not specified in the start tag.  It will also not
	/// contain attributes used as Namespace declarations (xmlns*) unless
	/// the <code>http://xml.org/sax/features/namespace-prefixes</code> 
	/// feature is set to <var>true</var> (it is <var>false</var> by 
	/// default).</p>
	/// 
	/// <p>If the namespace-prefixes feature (see above) is <var>false</var>, 
	/// access by qualified name may not be available; if the 
	/// <code>http://xml.org/sax/features/namespaces</code>
	/// feature is <var>false</var>, access by Namespace-qualified names 
	/// may not be available.</p>
	/// 
	/// <p>This interface replaces the now-deprecated SAX1 {@link
	/// org.Xml.Sax.AttributeList AttributeList} interface, which does not 
	/// contain Namespace support.  In addition to Namespace support, it 
	/// adds the <var>getIndex</var> methods (below).</p>
	/// 
	/// <p>The order of attributes in the list is unspecified, and will
	/// vary from implementation to implementation.</p>
	/// 
	/// </summary>
	/// <since> SAX 2.0
	/// </since>
	/// <author>  David Megginson, 
	/// <a href="mailto:sax@megginson.com">sax@megginson.com</a>
	/// </author>
	/// <version>  2.0
	/// </version>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.helpers.AttributeListImpl">
	/// </seealso>
	public interface Attributes
	{
		int Length
		{
			get;
			
		}
		
		
		/// <summary> Look up an attribute's Namespace URI by index.
		/// 
		/// </summary>
		/// <param name="index">The attribute index (zero-based).
		/// </param>
		/// <returns> The Namespace URI, or the empty string if none
		/// is available, or null if the index is out of
		/// range.
		/// </returns>
		/// <seealso cref="Length">
		/// </seealso>
		System.String getURI(int index);
		
		
		/// <summary> Look up an attribute's local name by index.
		/// 
		/// </summary>
		/// <param name="index">The attribute index (zero-based).
		/// </param>
		/// <returns> The local name, or the empty string if Namespace
		/// processing is not being performed, or null
		/// if the index is out of range.
		/// </returns>
		/// <seealso cref="Length">
		/// </seealso>
		System.String getLocalName(int index);
		
		
		/// <summary> Look up an attribute's XML 1.0 qualified name by index.
		/// 
		/// </summary>
		/// <param name="index">The attribute index (zero-based).
		/// </param>
		/// <returns> The XML 1.0 qualified name, or the empty string
		/// if none is available, or null if the index
		/// is out of range.
		/// </returns>
		/// <seealso cref="Length">
		/// </seealso>
		System.String getQName(int index);
		
		
		/// <summary> Look up an attribute's type by index.
		/// 
		/// <p>The attribute type is one of the strings "CDATA", "ID",
		/// "IDREF", "IDREFS", "NMTOKEN", "NMTOKENS", "ENTITY", "ENTITIES",
		/// or "NOTATION" (always in upper case).</p>
		/// 
		/// <p>If the parser has not read a declaration for the attribute,
		/// or if the parser does not report attribute types, then it must
		/// return the value "CDATA" as stated in the XML 1.0 Recommentation
		/// (clause 3.3.3, "Attribute-Value Normalization").</p>
		/// 
		/// <p>For an enumerated attribute that is not a notation, the
		/// parser will report the type as "NMTOKEN".</p>
		/// 
		/// </summary>
		/// <param name="index">The attribute index (zero-based).
		/// </param>
		/// <returns> The attribute's type as a string, or null if the
		/// index is out of range.
		/// </returns>
		/// <seealso cref="Length">
		/// </seealso>
		System.String getType(int index);
		
		
		/// <summary> Look up an attribute's value by index.
		/// 
		/// <p>If the attribute value is a list of tokens (IDREFS,
		/// ENTITIES, or NMTOKENS), the tokens will be concatenated
		/// into a single string with each token separated by a
		/// single space.</p>
		/// 
		/// </summary>
		/// <param name="index">The attribute index (zero-based).
		/// </param>
		/// <returns> The attribute's value as a string, or null if the
		/// index is out of range.
		/// </returns>
		/// <seealso cref="Length">
		/// </seealso>
		System.String getValue(int index);
		
		
		int getIndex(System.String uri, System.String localPart);
		
		
		/// <summary> Look up the index of an attribute by XML 1.0 qualified name.
		/// 
		/// </summary>
		/// <param name="qName">The qualified (prefixed) name.
		/// </param>
		/// <returns> The index of the attribute, or -1 if it does not
		/// appear in the list.
		/// </returns>
		int getIndex(System.String qName);
		
		
		/// <summary> Look up an attribute's type by Namespace name.
		/// 
		/// <p>See {@link #getType(int) getType(int)} for a description
		/// of the possible types.</p>
		/// 
		/// </summary>
		/// <param name="uri">The Namespace URI, or the empty String if the
		/// name has no Namespace URI.
		/// </param>
		/// <param name="localName">The local name of the attribute.
		/// </param>
		/// <returns> The attribute type as a string, or null if the
		/// attribute is not in the list or if Namespace
		/// processing is not being performed.
		/// </returns>
		System.String getType(System.String uri, System.String localName);
		
		
		/// <summary> Look up an attribute's type by XML 1.0 qualified name.
		/// 
		/// <p>See {@link #getType(int) getType(int)} for a description
		/// of the possible types.</p>
		/// 
		/// </summary>
		/// <param name="qName">The XML 1.0 qualified name.
		/// </param>
		/// <returns> The attribute type as a string, or null if the
		/// attribute is not in the list or if qualified names
		/// are not available.
		/// </returns>
		System.String getType(System.String qName);
		
		
		/// <summary> Look up an attribute's value by Namespace name.
		/// 
		/// <p>See {@link #getValue(int) getValue(int)} for a description
		/// of the possible values.</p>
		/// 
		/// </summary>
		/// <param name="uri">The Namespace URI, or the empty String if the
		/// name has no Namespace URI.
		/// </param>
		/// <param name="localName">The local name of the attribute.
		/// </param>
		/// <returns> The attribute value as a string, or null if the
		/// attribute is not in the list.
		/// </returns>
		System.String getValue(System.String uri, System.String localName);
		
		
		/// <summary> Look up an attribute's value by XML 1.0 qualified name.
		/// 
		/// <p>See {@link #getValue(int) getValue(int)} for a description
		/// of the possible values.</p>
		/// 
		/// </summary>
		/// <param name="qName">The XML 1.0 qualified name.
		/// </param>
		/// <returns> The attribute value as a string, or null if the
		/// attribute is not in the list or if qualified names
		/// are not available.
		/// </returns>
		System.String getValue(System.String qName);
	}
	
	// end of Attributes.java
}