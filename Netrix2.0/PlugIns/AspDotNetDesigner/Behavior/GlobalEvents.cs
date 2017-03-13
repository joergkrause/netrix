using System.Collections;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.AspDotNetDesigner
{
    /// <summary>
    /// Supports events an a very low level to achieve global event handling.
    /// </summary>
    /// <remarks>This class supports the infrastructure and is not intended to being used from custom code.</remarks>
    public class GlobalEvents : Interop.IHTMLEditDesigner
    {

        internal static event ElementEventHandler PreHandleEvent;
        internal static event ElementEventHandler PostHandleEvent;
        internal static Hashtable ge;
        private IHtmlEditor htmlEditor;

        static GlobalEvents()
        {
            ge = new Hashtable();
        }

        private GlobalEvents()
        {
        }

        internal static GlobalEvents GetGlobalEventsFactory(IHtmlEditor htmlEditor)
        {
            if (ge[htmlEditor] == null)
            {
                ge[htmlEditor] = new GlobalEvents();
                ((GlobalEvents) ge[htmlEditor]).htmlEditor = htmlEditor;
            }
            return ge[htmlEditor] as GlobalEvents;
        }

        #region IHTMLEditDesigner Members

        int Interop.IHTMLEditDesigner.PreHandleEvent(int dispId, Interop.IHTMLEventObj eventObj)
        {
            if (PreHandleEvent != null) PreHandleEvent(htmlEditor, eventObj);
            return Interop.S_FALSE;
        }

        int Interop.IHTMLEditDesigner.PostHandleEvent(int dispId, Interop.IHTMLEventObj eventObj)
        {
            if (PostHandleEvent != null) PostHandleEvent(htmlEditor, eventObj);
            return Interop.S_FALSE;
        }

        int Interop.IHTMLEditDesigner.TranslateAccelerator(int dispId, Interop.IHTMLEventObj eventObj)
        {
            return Interop.S_FALSE;
        }

        int Interop.IHTMLEditDesigner.PostEditorEventNotify(int dispId, Interop.IHTMLEventObj eventObj)
        {
            return Interop.S_FALSE;
        }

        #endregion

    }

}
