using System;

namespace GuruComponents.Netrix.WebEditing.Exceptions
{
    /// <summary>
    /// Standard exception thrown if any not command based internal operation fails.
    /// </summary>
	public class HtmlControlException : System.Exception
	{
		public HtmlControlException ()
		{
		}
   
		/// <summary>
		/// Constructor accepting a single string message
		/// </summary>
		/// <param name="message"></param>
		public HtmlControlException (string message) : base(message)
		{
		}
   
		/// <summary>
        ///  Constructor accepting a string message and an 
        ///  inner exception which will be wrapped by this custom exception class
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public HtmlControlException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}

