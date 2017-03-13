using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// ENCTYPE determines how the form data is encoded.
    /// </summary>
    /// <remarks>
    /// Whenever data is transmitted from one place to another, 
    /// there needs to be an agreed upon means of representing that data.
    /// </remarks>
    public enum EncodingType
    {
        /// <summary>
        /// For sending form data.
        /// </summary>
        UrlEncoded = 0,
        /// <summary>
        /// For uploading files.
        /// </summary>
        Multipart = 1,
        /// <summary>
        /// For sending form data as plain text (depreciated).
        /// </summary>
        TextPlain = 2
    }
}
