using System;
namespace GuruComponents.Netrix.WebEditing.Styles
{
    /// <summary>
    /// Stores padding style information.
    /// </summary>
    public interface IPadding
    {
        /// <summary>
        /// Padding in unit measures for all sides.
        /// </summary>
        System.Web.UI.WebControls.Unit padding { get; set; }
        /// <summary>
        /// Padding in unit measures for bottom side.
        /// </summary>
        System.Web.UI.WebControls.Unit paddingBottom { get; set; }
        /// <summary>
        /// True if bottom padding is not set.
        /// </summary>
        bool paddingBottomAuto { get; set; }
        /// <summary>
        /// Padding in unit measures for left side.
        /// </summary>
        System.Web.UI.WebControls.Unit paddingLeft { get; set; }
        /// <summary>
        /// True if left padding is not set.
        /// </summary>
        bool paddingLeftAuto { get; set; }
        /// <summary>
        /// Padding in unit measures for right side.
        /// </summary>
        System.Web.UI.WebControls.Unit paddingRight { get; set; }
        /// <summary>
        /// True if right padding is not set.
        /// </summary>
        bool paddingRightAuto { get; set; }
        /// <summary>
        /// Padding in unit measures for top side.
        /// </summary>
        System.Web.UI.WebControls.Unit paddingTop { get; set; }
        /// <summary>
        /// True if top padding is not set.
        /// </summary>
        bool paddingTopAuto { get; set; }
    }
}
