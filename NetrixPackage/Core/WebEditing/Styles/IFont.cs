using System;
namespace GuruComponents.Netrix.WebEditing.Styles
{
    /// <summary>
    /// Store font styles.
    /// </summary>
    public interface IFont
    {
        /// <summary>
        /// Font name
        /// </summary>
        string font { get; set; }
        /// <summary>
        /// Fotn family name.
        /// </summary>
        string fontFamily { get; set; }
        /// <summary>
        /// Size
        /// </summary>
        System.Web.UI.WebControls.Unit fontSize { get; set; }
        /// <summary>
        /// Style
        /// </summary>
        string fontStyle { get; set; }
        /// <summary>
        /// Variant (italic, ...)
        /// </summary>
        string fontVariant { get; set; }
        /// <summary>
        /// Weight (bold, ...)
        /// </summary>
        string fontWeight { get; set; }
    }
}
