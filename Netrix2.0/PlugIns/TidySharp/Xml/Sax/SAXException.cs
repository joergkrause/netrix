// SAX exception class.
// No warranty; no copyright -- use this as you will.
// $Id: SAXException.java,v 1.1 2000/11/14 16:49:04 garypeskin Exp $
using System;
namespace Comzept.Genesis.Tidy.Xml.Sax
{
	
	/// <summary> Encapsulate a general SAX error or warning.
	/// 
	/// <blockquote>
	/// <em>This module, both source code and documentation, is in the
	/// Public Domain, and comes with <strong>NO WARRANTY</strong>.</em>
	/// </blockquote>
	/// 
	/// <p>This class can contain basic error or warning information from
	/// either the XML parser or the application: a parser writer or
	/// application writer can subclass it to provide additional
	/// functionality.  SAX handlers may throw this exception or
	/// any exception subclassed from it.</p>
	/// 
	/// <p>If the application needs to pass through other types of
	/// exceptions, it must wrap those exceptions in a SAXException
	/// or an exception derived from a SAXException.</p>
	/// 
	/// <p>If the parser or application needs to include information about a
	/// specific location in an XML document, it should use the
	/// {@link org.Xml.Sax.SAXParseException SAXParseException} subclass.</p>
	/// 
	/// </summary>
	/// <since> SAX 1.0
	/// </since>
	/// <author>  David Megginson, 
	/// <a href="mailto:sax@megginson.com">sax@megginson.com</a>
	/// </author>
	/// <version>  2.0
	/// </version>
	/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.SAXParseException">
	/// </seealso>
	[Serializable]
	public class SAXException:System.Exception
	{
		/// <summary> Return a detail message for this exception.
		/// 
		/// <p>If there is an embedded exception, and if the SAXException
		/// has no detail message of its own, this method will return
		/// the detail message from the embedded exception.</p>
		/// 
		/// </summary>
		/// <returns> The error or warning message.
		/// </returns>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Parser.setLocale">
		/// </seealso>
		public override System.String Message
		{
			get
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.String message = base.Message;
				
				if (message == null && exception != null)
				{
					//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
					return exception.Message;
				}
				else
				{
					return message;
				}
			}
			
		}
		/// <summary> Return the embedded exception, if any.
		/// 
		/// </summary>
		/// <returns> The embedded exception, or null if there is none.
		/// </returns>
		virtual public System.Exception Exception
		{
			get
			{
				return exception;
			}
			
		}
		
		
		/// <summary> Create a new SAXException.
		/// 
		/// </summary>
		/// <param name="message">The error or warning message.
		/// </param>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Parser.setLocale">
		/// </seealso>
		public SAXException(System.String message):base(message)
		{
			this.exception = null;
		}
		
		
		/// <summary> Create a new SAXException wrapping an existing exception.
		/// 
		/// <p>The existing exception will be embedded in the new
		/// one, and its message will become the default message for
		/// the SAXException.</p>
		/// 
		/// </summary>
		/// <param name="e">The exception to be wrapped in a SAXException.
		/// </param>
		public SAXException(System.Exception e):base()
		{
			this.exception = e;
		}
		
		
		/// <summary> Create a new SAXException from an existing exception.
		/// 
		/// <p>The existing exception will be embedded in the new
		/// one, but the new exception will have its own message.</p>
		/// 
		/// </summary>
		/// <param name="message">The detail message.
		/// </param>
		/// <param name="e">The exception to be wrapped in a SAXException.
		/// </param>
		/// <seealso cref="Comzept.Genesis.Tidy.Xml.Sax.Parser.setLocale">
		/// </seealso>
		public SAXException(System.String message, System.Exception e):base(message)
		{
			this.exception = e;
		}
		
		
		/// <summary> Override toString to pick up any embedded exception.
		/// 
		/// </summary>
		/// <returns> A string representation of this exception.
		/// </returns>
		public override System.String ToString()
		{
			if (exception != null)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				return exception.ToString();
			}
			else
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				return base.ToString();
			}
		}
		
		
		private System.Exception exception;
	}
	
	// end of SAXException.java
}