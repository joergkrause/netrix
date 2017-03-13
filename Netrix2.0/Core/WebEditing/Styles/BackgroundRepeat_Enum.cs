using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
	/// Controls how the backgroundImage property of the object is tiled.
	/// </summary>
	public enum BackgroundRepeat
    {
        /// <summary>
        /// Default. Image is repeated horizontally and vertically.
        /// </summary>
        Repeat,
        /// <summary>
        /// Image is not repeated. 
        /// </summary>
        NoRepeat,
        /// <summary>
        /// Image is repeated horizontally. 
        /// </summary>
        RepeatX,
        /// <summary>
        /// Image is repeated vertically. 
        /// </summary>
        RepeatY,
        /// <summary>
        /// The value is not set. If assigned as value the attribute is being removed.
        /// </summary>
        NotSet
	}
}
