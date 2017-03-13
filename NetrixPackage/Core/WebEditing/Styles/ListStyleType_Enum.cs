using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
    /// Defines a variable that indicates how the list-item marker is drawn relative to the content of the object. 
	/// </summary>
    public enum ListStyleType
    {
        /// <summary>
        /// Default. Solid circles.
        /// </summary>
        disc,  
        /// <summary>
        /// Outlined circles.
        /// </summary>
        Circle, 
        /// <summary>
        /// Solid squares.
        /// </summary>
        Square,  
        /// <summary>
        /// 1, 2, 3, 4, and so on. 
        /// </summary>
        Decimal, 
        /// <summary>
        /// i, ii, iii, iv, and so on. 
        /// </summary>
        LowerRoman, 
        /// <summary>
        /// I, II, III, IV, and so on.
        /// </summary>
        UpperRoman, 
        /// <summary>
        /// a, b, c, d, and so on.
        /// </summary>
        LowerAlpha, 
        /// <summary>
        /// A, B, C, D, and so on. 
        /// </summary>
        UpperAlpha, 
        /// <summary>
        /// No marker is shown. 
        /// </summary>
        None,
        /// <summary>
        /// The value is not set. If assigned as value the attribute is being removed.
        /// </summary>
        NotSet        
    }
}
