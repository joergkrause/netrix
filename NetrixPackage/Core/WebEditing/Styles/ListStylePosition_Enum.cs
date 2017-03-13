using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
    /// Defines a variable that indicates how the list-item marker is drawn relative to the content of the object. 
	/// </summary>
	public enum ListStylePosition
    {
        /// <summary>
        /// Default. Marker is placed outside the text, and any wrapping text is not aligned under the marker.
        /// </summary>
        Outside,
        /// <summary>
        /// Marker is placed inside the text, and any wrapping text is aligned under the marker. 
        /// </summary>
        Inside,
        /// <summary>
        /// The value is not set. If assigned as value the attribute is being removed.
        /// </summary>
        NotSet
	}
}
