using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The method used to send the form.
    /// </summary>
    public enum FormMethod
    {
        /// <summary>
        /// HTTP-POST, the content is transferred as part of the HTTP body (default).
        /// </summary>
        Post    = 0,
        /// <summary>
        /// HTTP-GET, the content is transferred as part of the URL.
        /// </summary>
        Get     = 1
    }
}
