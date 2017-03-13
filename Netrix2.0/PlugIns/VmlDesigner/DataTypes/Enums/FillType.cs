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
    public enum FillType
    {
        Mixed = -2,
        Solid = 1,
        Pattern = 2,
        Tile = 3,
        Frame = 4,
        Gradient = 5,
        GradientUnscaled = 6,
        GradientCenter = 7,
        GradientRadial = 8,
        GradientTitle = 9,
        Background = 10,
    }
}
