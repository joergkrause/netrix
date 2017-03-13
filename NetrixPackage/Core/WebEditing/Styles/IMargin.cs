using System;
namespace GuruComponents.Netrix.WebEditing.Styles
{
    /// <summary>
    /// CSS margin 
    /// </summary>
    public interface IMargin
    {
        /// <summary>
        /// Margin for all sides.
        /// </summary>
        System.Web.UI.WebControls.Unit margin { get; set; }
        /// <summary>
        /// Nottom margin
        /// </summary>
        System.Web.UI.WebControls.Unit marginBottom { get; set; }
        /// <summary>
        /// Is set to auto
        /// </summary>
        bool marginBottomAuto { get; set; }
        /// <summary>
        /// Left margin
        /// </summary>
        System.Web.UI.WebControls.Unit marginLeft { get; set; }
        /// <summary>
        /// Left is set to auto
        /// </summary>
        bool marginLeftAuto { get; set; }
        /// <summary>
        /// Right margin
        /// </summary>
        System.Web.UI.WebControls.Unit marginRight { get; set; }
        /// <summary>
        /// Right is set to auto
        /// </summary>
        bool marginRightAuto { get; set; }
        /// <summary>
        /// Top margin
        /// </summary>
        System.Web.UI.WebControls.Unit marginTop { get; set; }
        /// <summary>
        /// Top is set to auto
        /// </summary>
        bool marginTopAuto { get; set; }
    }
}
