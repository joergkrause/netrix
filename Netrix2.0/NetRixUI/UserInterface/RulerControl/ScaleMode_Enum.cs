using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.UserInterface.Ruler
{
	/// <summary>
	/// Mode the scale appears.
	/// </summary>
    public enum ScaleMode
	{
        /// <summary>
        /// Use points (pt).
        /// </summary>
		Points = 0,
        /// <summary>
        /// Use pixels (px). Recommended.
        /// </summary>
		Pixels = 1,
        /// <summary>
        /// Use centimeters (cm).
        /// </summary>
		Centimetres = 2,
        /// <summary>
        /// Use inches (in).
        /// </summary>
		Inches = 3
	}

	
}