using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{
    /// <summary>
    /// Defines the type of clip style.
    /// </summary>
    public enum ClipFormat
    {
        /// <summary>
        /// The clip format is auto detected.
        /// </summary>
        Auto,
        /// <summary>
        /// The clip format is a rectangle.
        /// </summary>
        Rectangle,
        /// <summary>
        /// The value is not set. If assigned as value the attribute is being removed.
        /// </summary>
        NotSet
    }
}