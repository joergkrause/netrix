using System;
using System.ComponentModel;
using System.Drawing;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
    /// <summary>
    /// Displays a grid of pixels to help positioning elements in absolute position mode.
    /// </summary>
    /// <remarks>
    /// Optionally implements SnapRect to snap new elements to grid borders. Gridsize defaults to 8.
    /// </remarks> 
    public interface IHtmlGrid : IBaseBehavior
    {

        # region Public Properties

		/// <summary>
		/// Number of pixel the grid will snap.
		/// </summary>
		/// <remarks>
		/// If the grid is every 20 pixels and the zone is 5, the snap feature will work from pixel 15 to 25 on first column.
		/// The pixel between 25 and 35 on second column will not snap. If the zone is equally or greater than grid elements
		/// will always snap.
		/// </remarks>
		int SnapZone { get; set; }

		/// <summary>
		/// Snap will also work on element resize.
		/// </summary>
		/// <remarks>
		/// The <see cref="SnapZone"/> property applies, too.
		/// </remarks>
		bool SnapOnResize { get; set; }

		/// <summary>
		/// The visual appearance of lines, if used instead of points.
		/// </summary>
		GridLineType GridLineVisualisation { get; set; }

		/// <summary>
		/// The width of lines if the grid is shown as lines or the width of crosses in case cross view.
		/// </summary>
		int GridLineWidth { get; set; }


		/// <summary>
		/// The visual appearance of the grid.
		/// </summary>
		GridType GridVisualisation { get; set; }

		/// <summary>
		/// Gets or sets the visibility of the grid.
		/// </summary>
		/// <remarks>
		/// The grid is a good base to build the VS.NET like GridLayout.
		/// <seealso cref="SnapEnabled">SnapEnabled</seealso>
		/// <seealso cref="GuruComponents.Netrix.IHtmlEditor.AbsolutePositioningEnabled">AbsolutePositioningEnabled</seealso>
		/// <seealso cref="GuruComponents.Netrix.IHtmlEditor.MultipleSelectionEnabled">MultipleSelectionEnabled</seealso>
		/// </remarks>
		/// <example>
		/// To simulate the VS.NET GridLayout just set the following properties in one step. This code assumes that
		/// you have set the boolean member variable <c>State</c> before calling the sequence:
		/// <code>
		///this.htmlEditor1.GridVisible = State;
		///this.htmlEditor1.SnapEnabled = State;
		///this.htmlEditor1.AbsolutePositioningEnabled = State;
		///this.htmlEditor1.MultipleSelectionEnabled = State;</code>
		/// </example>
		bool GridVisible { get; set; }

		/// <summary>
		/// Enables snap of elements to grid if absolute positioning is enabled.
		/// </summary>
		/// <remarks>
		/// In flow layout is this function useless, therefore this options activates grid mode, too.
		/// </remarks>
        bool SnapEnabled { get; set; }

		/// <summary>
		/// Gets or sets the gap between pixels shown as grid.
		/// </summary>
		/// <remarks>
		/// Defaults to 8 for both width and height, if no value is set. Setting values lower than 8 will cause performance
		/// problems on large surfaces because to many point are plotted with any screen refresh.
		/// </remarks>
		Size GridSize { get; set; }

		/// <summary>
		/// Gets or sets the color of the grid pixel.
		/// </summary>
		/// <remarks>
		/// Defaults to <c>Color.Black</c>.
		/// </remarks>
		 Color GridColor { get; set; }

		# endregion
		
     }
}