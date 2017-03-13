using System;

namespace GuruComponents.Netrix.WebEditing.HighLighting
{
	/// <summary>
	/// Defines the available Styles for underlining. 
	/// </summary>
	public enum LineType : int
	{
		/// <summary>
		/// Text decoration has no specified style. The value is set automatically; for example, by default or inheritance. 
		/// </summary>
		None = 0,
		/// <summary>
		/// Text has one line drawn below it. 
		/// </summary>
		Auto = 1, 
		/// <summary>
		/// Not currently implemented. 
		/// </summary>
		Overline  = 2,
		/// <summary>
		/// Not currently implemented. 
		/// </summary>
		Underline = 3,
		/// <summary>
		/// Text has a dotted line drawn below it. 
		/// </summary>
		LineThrough = 4,
	}
}
