using System;
using System.ComponentModel;
namespace GuruComponents.Netrix.WebEditing.HighLighting
{
    /// <summary>
    /// Defines a color with additional properties.
    /// </summary>
    public interface IHighlightColor
    {
        /// <summary>
        /// Color name
        /// </summary>
        string ColorName { get; }
        /// <summary>
        /// Color value
        /// </summary>
        System.Drawing.Color ColorValue { get; set; }
        /// <summary>
        /// Color type.
        /// </summary>
        GuruComponents.Netrix.WebEditing.HighLighting.ColorType Type { get; set; }
    }
}
