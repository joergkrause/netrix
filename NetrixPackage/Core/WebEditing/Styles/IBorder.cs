using System;
#pragma warning disable 1591
namespace GuruComponents.Netrix.WebEditing.Styles
{
    public interface IBorder
    {
        string border { get; set; }
        string borderBottom { get; set; }
        System.Drawing.Color borderBottomColor { get; set; }
        System.Web.UI.WebControls.BorderStyle borderBottomStyle { get; set; }
        System.Web.UI.WebControls.Unit borderBottomWidth { get; set; }
        bool borderBottomWidthAuto { get; set; }
        System.Drawing.Color borderColor { get; set; }
        string borderLeft { get; set; }
        System.Drawing.Color borderLeftColor { get; set; }
        System.Web.UI.WebControls.BorderStyle borderLeftStyle { get; set; }
        System.Web.UI.WebControls.Unit borderLeftWidth { get; set; }
        bool borderLeftWidthAuto { get; set; }
        string borderRight { get; set; }
        System.Drawing.Color borderRightColor { get; set; }
        System.Web.UI.WebControls.BorderStyle borderRightStyle { get; set; }
        System.Web.UI.WebControls.Unit borderRightWidth { get; set; }
        string borderRightWidthValue { get; set; }
        System.Web.UI.WebControls.BorderStyle borderStyle { get; set; }
        string borderTop { get; set; }
        System.Drawing.Color borderTopColor { get; set; }
        System.Web.UI.WebControls.BorderStyle borderTopStyle { get; set; }
        System.Web.UI.WebControls.Unit borderTopWidth { get; set; }
        string borderTopWidthValue { get; set; }
        System.Web.UI.WebControls.Unit borderWidth { get; set; }
    }
}
