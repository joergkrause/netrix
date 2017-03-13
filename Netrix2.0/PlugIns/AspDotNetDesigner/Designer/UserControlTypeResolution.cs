using System;
using System.Web.UI;
using System.Web.UI.Design;
using GuruComponents.Netrix.Designer;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.AspDotNetDesigner
{
    /// <summary>
    /// Resolves types of user controls from directives.
    /// </summary>
    public class UserControlTypeResolution : IUserControlTypeResolutionService
    {
        private IHtmlEditor htmlEditor;
        private DesignerHost host;

        /// <summary>
        /// Resolves an user control's type.
        /// </summary>
        /// <param name="htmlEditor"></param>
        public UserControlTypeResolution(IHtmlEditor htmlEditor)
        {
            this.htmlEditor = htmlEditor;
            host = (DesignerHost)htmlEditor.ServiceProvider.GetService(typeof(IDesignerHost));
        }


        #region IUserControlTypeResolutionService Members

        Type IUserControlTypeResolutionService.GetType(string tagPrefix, string tagName)
        {
            IReferenceManager wrm = htmlEditor.ServiceProvider.GetService(typeof(IWebFormReferenceManager)) as IReferenceManager;
            if (wrm != null)
            {
                IRegisterDirective directive = wrm.GetRegisterDirective(tagPrefix, tagName);
                return directive.ObjectType;
            }
            return null;
        }

        #endregion

    }
}
