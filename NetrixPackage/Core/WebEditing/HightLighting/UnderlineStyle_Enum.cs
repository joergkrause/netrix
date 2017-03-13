using System;

namespace GuruComponents.Netrix.WebEditing.HighLighting
{
	/// <summary>
	/// Defines the available Styles for underlining. 
	/// </summary>
	public enum UnderlineStyle
	{
		/// <summary>
		/// Text decoration has no specified style. The value is set automatically; for example, by default or inheritance. 
		/// </summary>
		Undefined = 0,
		/// <summary>
		/// Text has one line drawn below it. 
		/// </summary>
		Single = 1, 
		/// <summary>
		/// Not currently implemented. 
		/// </summary>
		Double  = 2,
		/// <summary>
		/// Not currently implemented. 
		/// </summary>
		Words = 3,
		/// <summary>
		/// Text has a dotted line drawn below it. 
		/// </summary>
		Dotted = 4,
		/// <summary>
		/// Not currently implemented. 
		/// </summary>
		Thick = 5,
		/// <summary>
		/// Not currently implemented. 
		/// </summary>
		Dash = 6,
		/// <summary>
		/// Not currently implemented. 
		/// </summary>
		DotDash = 7,
		/// <summary>
		/// Not currently implemented. 
		/// </summary>
		DotDotDash = 8,	
		/// <summary>
		/// Text has a wavy line drawn below it. 
		/// </summary>
		Wave = 9,
		/// <summary>
		/// Not currently implemented. 
		/// </summary>
		SingleAccounting = 10,
		/// <summary>
		/// Not currently implemented. 
		/// </summary>
		DoubleAccounting = 11,
		/// <summary>
		/// Text has a dashed line drawn below it that has thicker width. 
		/// </summary>
		ThickDash  = 12

	}
}
