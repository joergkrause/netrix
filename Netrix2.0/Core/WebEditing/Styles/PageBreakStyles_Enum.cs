using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{

    /// <summary>
    /// Defines a value indicating whether a page break occurs before or after the object.
    /// </summary>
    public enum PageBreakStyles
    {
        /// <summary>
        /// Always insert a page break after the object. 
        /// </summary>
        Always,
        /// <summary>
        /// Default. Neither force nor forbid a page break after the object. 
        /// </summary>
        Auto,
        /// <summary>
        /// string Do not insert a page break. 
        /// </summary>
        Empty,
        /// <summary>
        /// Currently works the same as always. 
        /// </summary>
        Left,
        /// <summary>
        /// Currently works the same as always. 
        /// </summary>
        Right,
        /// <summary>
        /// The value is not set. If assigned as value the attribute is being removed.
        /// </summary>
        NotSet
    }
}
