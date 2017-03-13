using System;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{

	/// <summary>
	/// If grid uses lines, the enum values determin how the line is drawn.
	/// </summary>
	[Serializable()]
    public enum GridLineType
	{
        /// <summary>
        /// Solid lines
        /// </summary>
		Solid = 0,
        /// <summary>
        /// Dashed lines
        /// </summary>
		Dash = 1,
        /// <summary>
        /// Dotted line
        /// </summary>
		Dot = 2,
        /// <summary>
        /// Pattern of dots and dashes
        /// </summary>
		Dashdot = 3,
        /// <summary>
        /// Pattern of dashe followed by two dots.
        /// </summary>
		Dashdotdot = 4,
        /// <summary>
        /// Nothing, no grid
        /// </summary>
		Null = 5,
        /// <summary>
        /// Not yet implemented.
        /// </summary>
		Insideframe = 6
	}

}
