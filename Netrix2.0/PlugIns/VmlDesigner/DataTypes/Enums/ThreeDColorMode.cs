using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
    /// <summary>
    /// Type of fill of a shape.
    /// </summary>
    /// <remarks>
    /// Tile, Pattern, and Frame require the image attributes to be supplied. Gradient and GradientRadial 
    /// require the gradient attributes to be supplied.
    /// </remarks>
    public enum ThreeDColorMode
    {
        Mixed = -2,
        Auto = 1,
        Custom = 2,
    }
}
