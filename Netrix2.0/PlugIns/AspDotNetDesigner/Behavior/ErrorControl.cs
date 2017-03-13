using System;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.AspDotNetDesigner
{
        /// <summary>
        /// The purpose of this class is the creation of some HTML for design time
        /// error messages.
        /// </summary>
        [DesignerAttribute(typeof(ErrorDesigner), typeof(IDesigner))]
        internal sealed class ErrorControl : WebControl
        {
            private string _controlTag;
            private Exception _errorException;
            private static string errorGlyph;
            private static readonly string RenderTemplate;

            static ErrorControl()
            {
                RenderTemplate = "\r\n<table cellpadding=4 cellspacing=0 style=\"font-family:Tahoma;font-size:8pt;color:buttontext;background-color:buttonface;border: solid 1px;border-top-color:buttonhighlight;border-left-color:buttonhighlight;border-bottom-color:buttonshadow;border-right-color:buttonshadow\">\r\n                <tr><td><img src=\"{1}\" width=\"16\" height=\"16\" align=absmiddle>&nbsp;&nbsp;&lt;{2}&gt;</td></tr>\r\n                <tr><td>{0}</td></tr>\r\n              </table>";
                errorGlyph = null;
            }

            public ErrorControl(string controlTag, Exception e)
            {
                this._controlTag = controlTag;
                this._errorException = e;
                ID = "ErrorCtrl";
            }
            public string GetDesignTimeHtml()
            {				
                string html = String.Format(RenderTemplate, this._errorException.Message, ErrorGlyph, this._controlTag);
                return html;
            }

            public string Error
            {
                get
                {
                    return this._errorException.ToString();
                }
            }
            private static string ErrorGlyph
            {
                get
                {
                    if (errorGlyph == null)
                    {
                        string pth = Path.GetDirectoryName(typeof(ErrorControl).Assembly.Location);
                        errorGlyph = String.Format("res://{0}\\{1}{2}{3}", pth, "GuruComponents.Netrix.Resources.dll", @"/ASPNET/", "ERROR_GLYPH");
                    }
                    return errorGlyph;
                }
            }

            public override string ID
            {
                get
                {
                    return base.ID;
                }
                set
                {
                    base.ID = value;
                }
            }
        }

        internal sealed class ErrorDesigner : ControlDesigner
        {
            private ErrorControl ec;

            public override void Initialize(IComponent component)
            {
                if (component is ErrorControl)
                {
                    ec = (ErrorControl) component;
                }
                
            }

            public override bool AllowResize
            {
                get
                {
                    return false;
                }
            }

            public override string GetDesignTimeHtml()
            {
                return ec.GetDesignTimeHtml();
            }
   

        }

}
