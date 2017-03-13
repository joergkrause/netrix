using System;
using System.Collections;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.Design;
using GuruComponents.Netrix.ComInterop;
using System.ComponentModel.Design;
using GuruComponents.Netrix.WebEditing.Elements;

# pragma warning disable 0618

namespace GuruComponents.Netrix.Designer
{
    public class AspWebFormsRootDesigner : WebFormsRootDesigner
    {
        IServiceContainer serviceProvider;
        IHtmlEditor htmlEditor;

        public AspWebFormsRootDesigner(IServiceContainer serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.htmlEditor = ((IEditorInstanceService) serviceProvider.GetService(typeof(IEditorInstanceService))).EditorInstance;
        }

        public override void AddClientScriptToDocument(ClientScriptItem scriptItem)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string AddControlToDocument(Control newControl, Control referenceControl, ControlLocation location)
        {
            if (newControl is IElement && referenceControl is IElement)
            {
                switch (location)
                {
                    case ControlLocation.FirstChild:
                        ((IElement)referenceControl).ElementDom.AppendChild(((IElement)newControl));
                        break;
                    case ControlLocation.First:
                        ((IElement)referenceControl).ElementDom.AppendChild(((IElement)newControl));
                        break;
                    case ControlLocation.LastChild:
                        ((IElement)referenceControl).ElementDom.AppendChild(((IElement)newControl));
                        break;
                    case ControlLocation.Last:
                        ((IElement)referenceControl).ElementDom.AppendChild(((IElement)newControl));
                        break;
                    case ControlLocation.After:
                        ((IElement)referenceControl).InsertAdjacentElement(InsertWhere.AfterBegin, (IElement)newControl);
                        break;
                    case ControlLocation.Before:
                        ((IElement)referenceControl).InsertAdjacentElement(InsertWhere.BeforeBegin, (IElement)newControl);
                        break;
                }
            }
            return "";
        }

        public override string DocumentUrl
        {
            get { return htmlEditor.Url; }
        }

        public override ClientScriptItemCollection GetClientScriptsInDocument()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void GetControlViewAndTag(Control control, out IControlDesignerView view, out IControlDesignerTag tag)
        {            
            view = null;
            tag = null;
            IDesignerHost designerHost = ((IDesignerHost) serviceProvider.GetService(typeof(IDesignerHost)));
            IDesigner designer = designerHost.GetDesigner(control);
            
        }

        public override bool IsDesignerViewLocked
        {
            get { return false; }
        }

        public override bool IsLoading
        {
            get { return false; }
        }

        public override System.Web.UI.Design.WebFormsReferenceManager ReferenceManager
        {
            get 
            { 
                IWebFormReferenceManager wrm = serviceProvider.GetService(typeof(System.Web.UI.Design.IWebFormReferenceManager)) as IWebFormReferenceManager;
                return ((System.Web.UI.Design.WebFormsReferenceManager)wrm);
            }
        }

        public override void RemoveClientScriptFromDocument(string clientScriptId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void RemoveControlFromDocument(Control control)
        {
            if (control is IElement)
            {
                ((IElement) control).ElementDom.RemoveElement(true);
            }
        }
    }
}
