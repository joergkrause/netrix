namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// Defines possible shapes for Area elements.
	/// </summary>
	public enum AreaShape
	{
        /// <summary>
        /// No explicit definition for the shape. Applies to the whole image.
        /// </summary>
        Default = 0,
        /// <summary>
        /// The shape is a rectangle.
        /// </summary>
        Rect    = 1,
        /// <summary>
        /// The shape is a circle.
        /// </summary>
        Circle  = 2,
        /// <summary>
        /// The shape is a polygon.
        /// </summary>
        Poly    = 3        
	}
}
