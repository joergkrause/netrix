using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel.Design;
using System.Web.UI;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.XmlDesigner
{
    /// <summary>
    /// Writes element back as string.
    /// </summary>
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
}