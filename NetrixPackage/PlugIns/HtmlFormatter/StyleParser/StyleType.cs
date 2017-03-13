namespace GuruComponents.Netrix.UserInterface.StyleParser
{
	/// <summary>
	/// To check what type of attribute it is.
	/// </summary>
	public enum StyleType
	{
        /// <summary>
        /// Attribute is a color.
        /// </summary>
		Color		= 0,
        /// <summary>
        /// Attribute is a unit.
        /// </summary>
		Unit		= 1,
        /// <summary>
        /// Attribute is a list (any number of values, comma separated).
        /// </summary>
		List		= 2,
        /// <summary>
        /// Attribute is a property (any string value).
        /// </summary>
		Property	= 3,
        /// <summary>
        /// Attribute is a combination of Property, Color and Unit (in any order).
        /// </summary>
		Group		= 4		
	}
}
