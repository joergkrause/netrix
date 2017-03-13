using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;


namespace GuruComponents.Netrix.XmlDesigner
{
    

    internal class GlobalEvents : Interop.IHTMLEditDesigner
    {

        internal event ElementEventHandler PreHandleEvent;
        internal event ElementEventHandler PostHandleEvent;
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
            if (PreHandleEvent != null)
            {
                PreHandleEvent(htmlEditor, eventObj);
                //if (eventObj.cancelBubble) return Interop.S_OK;
            }
            return Interop.S_FALSE;
        }

        int Interop.IHTMLEditDesigner.PostHandleEvent(int dispId, Interop.IHTMLEventObj eventObj)
        {
            if (PostHandleEvent != null)
            {
                PostHandleEvent(htmlEditor, eventObj);
                //if (eventObj.cancelBubble) return Interop.S_OK;
            }
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
