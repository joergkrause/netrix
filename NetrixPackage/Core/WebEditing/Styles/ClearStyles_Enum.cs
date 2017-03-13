using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
    /// Defines whether the object allows floating objects on its left side, right side, or both, so that the next text displays past the floating objects.
	/// </summary>
	public enum ClearStyles
    {
        /// <summary>
        /// Default. Floating objects are allowed on both sides. 
        /// </summary>
        None,
        /// <summary>
        /// Object is moved below any floating object on the left side. 
        /// </summary>
        Left,
        /// <summary>
        /// Object is moved below any floating object on the right side. 
        /// </summary>
        Right,
        /// <summary>
        /// Object is moved below any floating object. 
        /// </summary>
        Both,
        /// <summary>
        /// The value is not set. If assigned as value the attribute is being removed.
        /// </summary>
        NotSet
}
}
