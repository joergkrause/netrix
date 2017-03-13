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
	/// The ColorChangedEvent delegate.
	/// </summary>
	public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs e);

		/// <summary>
	/// Provides data for the <c>ColorChanged</c> event.
	/// </summary>
	/// <remarks>
	/// The ColorChanged event occurs when a user selects a new color in the
	/// ColorPicker, ColorPanel or CustomColorPicker controls.
	/// </remarks>
	public class ColorChangedEventArgs : System.EventArgs
	{
		private System.Drawing.Color color;

		/// <summary>
		/// Initializes a new instance of the <c>ColorChangedEventArgs</c> class.
		/// </summary>
		/// <param name="color">
		/// The selected color.
		/// </param>
		internal ColorChangedEventArgs(System.Drawing.Color color)
		{
			this.color = color;
		}

		/// <summary>
		/// Gets the selected color.
		/// </summary>
		/// <value>
		/// The color which set by the user.
		/// </value>
		public System.Drawing.Color Color
		{
			get
			{
				return this.color;
			}
		}
	}
}
