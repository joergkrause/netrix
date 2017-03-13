using System;
namespace GuruComponents.Netrix.WebEditing.Styles
{
    /// <summary>
    /// Background style information.
    /// </summary>
    public interface IBackground
    {
        /// <summary>
        /// 
        /// </summary>
        GuruComponents.Netrix.WebEditing.Styles.BackgroundStyles background { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string backgroundAttachment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        System.Drawing.Color backgroundColor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string backgroundImage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        System.Web.UI.WebControls.HorizontalAlign backgroundPositionHorizontalAlign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        System.Web.UI.WebControls.Unit backgroundPositionUnit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        System.Web.UI.WebControls.VerticalAlign backgroundPositionVerticalAlign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        System.Web.UI.WebControls.Unit backgroundPositionX { get; set; }
        /// <summary>
        /// 
        /// </summary>
        System.Web.UI.WebControls.Unit backgroundPositionY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        GuruComponents.Netrix.WebEditing.Styles.BackgroundRepeat backgroundRepeat { get; set; }
    }
}
