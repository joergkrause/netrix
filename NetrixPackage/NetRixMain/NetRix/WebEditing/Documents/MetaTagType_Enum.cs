using System;

namespace GuruComponents.Netrix.WebEditing.Documents
{
	/// <summary>
	/// The type of Meta Tag beeing created for the document.
	/// </summary>
	public enum MetaTagType
	{
        /// <summary>
        /// A named meta tag (witn name attribute).
        /// </summary>
	    Named,
        /// <summary>
        /// A HTTP equiv meta tag (with Http-equiv attribute) that defines HTTP related data.
        /// </summary>
	    HttpEquiv
	}
}
