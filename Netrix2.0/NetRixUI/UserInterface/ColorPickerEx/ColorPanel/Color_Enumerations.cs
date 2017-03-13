using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{

	/// <summary>
	/// This represents the Z Axis of a RGB color cube.
	/// </summary>
	public enum ZAxis
	{
		/// <summary>The Z Axis is red</summary>
		red,
		/// <summary>The Z Axis is blue</summary>
		blue,
		/// <summary>The Z Axis is green</summary>
		green
	}

	/// <summary>
	/// Specifies the set of colors to be displayed in the color palette.
	/// </summary>
	public enum ColorSet
	{
		/// <summary>Show the system color palette.</summary>
		System,
		/// <summary>Show the web color palette.</summary>
		Web,
		/// <summary>Show Continues palette.</summary>
		Continues,
		/// <summary>Show custom scale.</summary>
		Custom
	}

	/// <summary>
	/// Specifies the order the colors contained in the selected palette should be sorted.
	/// </summary>
	/// <remarks>
	/// Additionally
	/// the grid can be shown in a Dreamwaever like color sorting box using 12 columns to order colors
	/// in fours blocks, each 6x6 cells to have color groups (meaning that all green variants are closed
	/// together).
	/// </remarks>
	public enum ColorSortOrder
	{
		/// <summary>Sort by name.</summary>
		Name,
		/// <summary>Sort by brightness.</summary>
		Brightness,
		/// <summary>Sort as continues colors in 18x12 websafe grid (216 colors only).</summary>
		Continues,
		/// <summary>Sort by saturation.</summary>
		Saturation,
		/// <summary>Sort by linear distance from the origin (0,0,0) of the RGB color space.</summary>
		Distance,
		/// <summary>
		/// Colors are sorted according to their original order.
		/// </summary>
		/// <remarks>
		/// For System and Web color sets this is the same as sort by name.
		/// For custom colors it will be the order of the colors in the array assigned to the CustomColors property
		/// </remarks>
		Unsorted
	}

}
