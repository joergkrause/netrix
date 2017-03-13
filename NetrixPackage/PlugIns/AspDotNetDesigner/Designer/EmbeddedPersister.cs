using System.ComponentModel.Design;
using System.Web.UI;
using GuruComponents.Netrix.ComInterop;
using System.Web.UI.Design;

namespace GuruComponents.Netrix.AspDotNetDesigner
{
    internal class EmbeddedPersister
    {
        private EmbeddedPersister()
        {
        }

        public static void PersistControl(Interop.IHTMLElement element, Control control)
        {
            EmbeddedSerializer.SerializeControl(control, element);
        }

        public static void PersistControl(Interop.IHTMLElement element, Control control, IDesignerHost host)
        {
            EmbeddedSerializer.SerializeControl(control, host, element);
        }


        public static void PersistInnerProperties(Interop.IHTMLElement element, object component, IDesignerHost host)
        {
            EmbeddedSerializer.SerializeInnerProperties(component, host, element);
        }
        public static void PersistTemplate(Interop.IHTMLElement element, ITemplate template, IDesignerHost host)
        {
            EmbeddedSerializer.SerializeTemplate(template, element, host);
        }
    }

    //public class EmbeddedPersister
    //{
    //    private EmbeddedPersister()
    //    {
    //    }

    //    public static void PersistControl(Interop.IHTMLElement element, Control control)
    //    {
    //        string html = ControlPersister.PersistControl(control);
    //        element.SetOuterHTML(html);
    //    }

    //    public static void PersistControl(Interop.IHTMLElement element, Control control, IDesignerHost host)
    //    {
    //        string html = ControlPersister.PersistControl(control, host).Trim();
    //        element.SetOuterHTML(html);
    //    }


    //    public static void PersistInnerProperties(Interop.IHTMLElement element, object component, IDesignerHost host)
    //    {
    //        string html = ControlPersister.PersistInnerProperties(component, host);
    //        element.InsertAdjacentHTML("afterBegin", html);
    //    }

    //    public static void PersistTemplate(Interop.IHTMLElement element, ITemplate template, IDesignerHost host)
    //    {
    //        string html = ControlPersister.PersistTemplate(template, host);
    //        element.InsertAdjacentHTML("afterBegin", html);
    //    }
    //}
}