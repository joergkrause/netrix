using System;

namespace GuruComponents.Netrix.WebEditing.Exceptions
{
    /// <summary>
    /// Exception is thrown when a command is issued and the document was not ready yet.
    /// </summary>
    /// <remarks>
    /// To avoid this exception wait for <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> event before issuing any commands.
    /// </remarks>
	public class DocumentNotReadyException : System.Exception
	{
		public DocumentNotReadyException ()
		{
		}
   
		/// <summary>
		/// Constructor accepting a single string message.
		/// </summary>
		/// <param name="message"></param>
		public DocumentNotReadyException (string message) : base(message)
		{
		}
   
		/// <summary>
        ///  Constructor accepting a string message and an 
        ///  inner exception which will be wrapped by this custom exception class.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public DocumentNotReadyException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}

