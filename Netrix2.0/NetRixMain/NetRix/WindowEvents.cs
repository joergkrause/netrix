using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;
using System.Runtime.InteropServices;
using GuruComponents.Netrix.WebEditing.Documents;
using GuruComponents.Netrix.Events;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;

namespace GuruComponents.Netrix
{
    /// <summary>
    /// Handle events on window and optional on document level.
    /// </summary>
    internal class WindowsEvents : Interop.IHTMLWindowEvents, Interop.IHTMLDocumentEvents, IDisposable
    {

        private Interop.IHTMLWindow2 window;
        private IConnectionPoint connectionPointWin, connectionPointDoc;
        private int winCookie, docCookie;
        private static Guid winGuid = typeof(Interop.IHTMLWindowEvents).GUID;
        private static Guid docGuid = typeof(Interop.IHTMLDocumentEvents).GUID;
        private IHtmlEditor editor;
        private GuruComponents.Netrix.WebEditing.Documents.HtmlWindow htmlWindow;

        public WindowsEvents(Interop.IHTMLWindow2 window, IHtmlEditor editor, IHtmlWindow htmlWindow)
        {
            this.window = window;
            this.editor = editor;
            this.htmlWindow = (GuruComponents.Netrix.WebEditing.Documents.HtmlWindow)htmlWindow;
            this.ConnectWindow();
            this.ConnectDocument();
        }

        internal void ConnectWindow()
        {
            // sink the events                    
            IConnectionPointContainer cpc = (IConnectionPointContainer)this.window;
            try
            {
                cpc.FindConnectionPoint(ref winGuid, out connectionPointWin);
                connectionPointWin.Advise(this, out winCookie);
            }
            catch
            {
            }
        }

        private void ConnectDocument()
        {
            // sink the events                    
            IConnectionPointContainer cpc = (IConnectionPointContainer)this.window.document;
            try
            {
                cpc.FindConnectionPoint(ref docGuid, out connectionPointDoc);
                connectionPointDoc.Advise(this as Interop.IHTMLDocumentEvents, out docCookie);
            }
            catch
            {
            }
        }

        private void Disconnect()
        {
            if (winCookie != 0)
            {
                //try { connectionPointWin.Unadvise(winCookie); }
                //catch { }
                winCookie = 0;
            }
            if (docCookie != 0)
            {
                try { connectionPointDoc.Unadvise(docCookie); }
                catch { }
                docCookie = 0;
            }
            htmlWindow = null;
            editor = null;
        }

        # region Inheritable Access

        protected virtual void OnLoad()
        {
            htmlWindow.InvokeLoad(window.@event);
        }

        protected virtual void OnUnload()
        {
        }

        protected virtual bool OnHelp()
        {
            htmlWindow.InvokeHelp(GetEvent());
            return false;
        }

        protected virtual void OnFocus()
        {
            htmlWindow.InvokeFocus(GetEvent());
        }

        protected virtual void OnBlur()
        {
            htmlWindow.InvokeBlur(GetEvent());
        }

        protected virtual void OnError(string description, string url, int line)
        {
            ShowErrorEventArgs e = new ShowErrorEventArgs(description, url, line);
            this.htmlWindow.OnScriptError(e);
            // suppress javascript errors
            Interop.IHTMLEventObj eventObj = GetEvent();
            if (eventObj != null)
            {
                eventObj.returnValue = e.Cancel;
            }
        }

        protected virtual void OnResize()
        {
            Interop.IHTMLEventObj eventObj = GetEvent();
            if (eventObj != null)
            {
                htmlWindow.InvokeResize(eventObj);
            }
        }

        protected virtual void OnScroll()
        {
            Interop.IHTMLEventObj eventObj = GetEvent();
            if (eventObj != null)
            {
                htmlWindow.InvokeScroll(eventObj);
            }
        }

        protected virtual void OnBeforeUnload()
        {
            Interop.IHTMLEventObj eventObj = GetEvent();
            if (eventObj != null)
            {
                htmlWindow.InvokeBeforeUnload(eventObj);
            }
        }

        protected virtual void OnBeforePrint()
        {
            Interop.IHTMLEventObj eventObj = GetEvent();
            if (eventObj != null)
            {
                htmlWindow.InvokeBeforePrint(eventObj);
            }
        }

        protected virtual void OnAfterPrint()
        {
            Interop.IHTMLEventObj eventObj = GetEvent();
            if (eventObj != null)
            {
                htmlWindow.InvokeAfterPrint(eventObj);
            }
        }

        # endregion

        #region IHTMLWindowEvents Members

        void Interop.IHTMLWindowEvents.onload()
        {
            OnLoad();
        }

        void Interop.IHTMLWindowEvents.onunload()
        {
            OnUnload();
        }

        bool Interop.IHTMLWindowEvents.onhelp()
        {
            return OnHelp();
        }

        void Interop.IHTMLWindowEvents.onfocus()
        {
            OnFocus();
        }

        void Interop.IHTMLWindowEvents.onblur()
        {
            OnBlur();
        }

        void Interop.IHTMLWindowEvents.onerror(string description, string url, int line)
        {
            OnError(description, url, line);
        }

        void Interop.IHTMLWindowEvents.onresize()
        {
            OnResize();
        }

        void Interop.IHTMLWindowEvents.onscroll()
        {
            OnScroll();
        }

        void Interop.IHTMLWindowEvents.onbeforeunload()
        {
            OnBeforeUnload();
        }

        void Interop.IHTMLWindowEvents.onbeforeprint()
        {
            OnBeforePrint();
        }

        void Interop.IHTMLWindowEvents.onafterprint()
        {
            OnAfterPrint();
        }

        #endregion

        #region IHTMLDocumentEvents Members

        private Interop.IHTMLEventObj GetEvent()
        {
            Interop.IHTMLEventObj e = window.@event;
            if (e == null || editor == null) return null;
            if (editor.EventManager.GetEnabled((EventType)Enum.Parse(typeof(EventType), e.type, true)))
            {
                return e;
            }
            else
            {
                return null;
            }
        }

        private static MouseButtons GetMouseButtonFromNative(Interop.IHTMLEventObj @event)
        {
            if (@event == null) return System.Windows.Forms.Control.MouseButtons;
            switch (@event.button)
            {
                case 0:
                    return MouseButtons.None;
                case 1:
                    return MouseButtons.Left;
                case 2:
                    return MouseButtons.Right;
                case 3:
                    return MouseButtons.Middle;
                default:
                    return MouseButtons.None;
            }
        }

        bool Interop.IHTMLDocumentEvents.onhelp()
        {
            Interop.IHTMLEventObj e = GetEvent();
            ((HtmlEditor)editor).InvokeHelpRequested(new HelpEventArgs(Control.MousePosition));
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeHelp(e); return e.returnValue == null ? true : Convert.ToBoolean(e.returnValue); }
            return true;
        }

        bool Interop.IHTMLDocumentEvents.onclick()
        {
            Interop.IHTMLEventObj e = GetEvent();
            ((HtmlEditor)editor).InvokeClick();
            ((HtmlEditor)editor).InvokeMouseClick(GetMouseButtonFromNative(e));
            if (e != null) 
            {
                ((HtmlDocumentStructure)editor.DocumentStructure).InvokeClick(e); return e.returnValue == null ? true : Convert.ToBoolean(e.returnValue); 
            }
            return true;
        }

        bool Interop.IHTMLDocumentEvents.ondblclick()
        {
            Interop.IHTMLEventObj e = GetEvent();
            ((HtmlEditor)editor).InvokeDoubleClick();
            ((HtmlEditor)editor).InvokeMouseDoubleClick(GetMouseButtonFromNative(e));
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeDblClick(e); return e.returnValue == null ? true : Convert.ToBoolean(e.returnValue); }
            return true;
        }

        void Interop.IHTMLDocumentEvents.onkeydown()
        {
            Interop.IHTMLEventObj e = GetEvent();            
            if (e != null) 
            { 
                ((HtmlDocumentStructure)editor.DocumentStructure).InvokeKeyDown(e); 
            }
        }

        void Interop.IHTMLDocumentEvents.onkeyup()
        {
            Interop.IHTMLEventObj e = GetEvent();            
            if (e != null) 
            { 
                ((HtmlDocumentStructure)editor.DocumentStructure).InvokeKeyUp(e); 
            }
        }

        bool Interop.IHTMLDocumentEvents.onkeypress()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null)
            {
                ((HtmlDocumentStructure)editor.DocumentStructure).InvokeKeyPress(e); return e.returnValue == null ? true : Convert.ToBoolean(e.returnValue);
            }
            return true;
        }

        void Interop.IHTMLDocumentEvents.onmousedown()
        {
            Interop.IHTMLEventObj e = GetEvent();
            ((HtmlEditor)editor).InvokeMouseDown(GetMouseButtonFromNative(e));
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeMouseDown(e); }
        }

        void Interop.IHTMLDocumentEvents.onmousemove()
        {
            Interop.IHTMLEventObj e = GetEvent();
            ((HtmlEditor)editor).InvokeMouseMove(GetMouseButtonFromNative(e));
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeMouseMove(e); }
        }

        void Interop.IHTMLDocumentEvents.onmouseup()
        {
            Interop.IHTMLEventObj e = GetEvent();
            ((HtmlEditor)editor).InvokeMouseUp(GetMouseButtonFromNative(e));
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeMouseUp(e); }
        }

        void Interop.IHTMLDocumentEvents.onmouseout()
        {
            ((HtmlEditor)editor).InvokeMouseOut();
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeMouseOut(e); }
        }

        void Interop.IHTMLDocumentEvents.onmouseover()
        {
            ((HtmlEditor)editor).InvokeMouseOver();
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeMouseOver(e); }
        }

        void Interop.IHTMLDocumentEvents.onreadystatechange()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeReadystateChange(e); }
        }

        bool Interop.IHTMLDocumentEvents.onbeforeupdate()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeBeforeUpdate(e); }
            return true;
        }

        void Interop.IHTMLDocumentEvents.onafterupdate()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeAfterupdate(e); }
        }

        bool Interop.IHTMLDocumentEvents.onrowexit()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeRowExit(e); }
            return true;
        }

        void Interop.IHTMLDocumentEvents.onrowenter()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeRowEnter(e); }
        }

        bool Interop.IHTMLDocumentEvents.ondragstart()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeDragstart(e); return e.returnValue == null ? true : Convert.ToBoolean(e.returnValue); }
            return true;
        }

        bool Interop.IHTMLDocumentEvents.onselectstart()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeSelectstart(e); }
            return true;
        }

        bool Interop.IHTMLDocumentEvents.onerrorupdate()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeErrorUpdate(e); }
            return true;
        }

        bool Interop.IHTMLDocumentEvents.oncontextmenu()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeContextMenu(e); }
            return true;
        }

        bool Interop.IHTMLDocumentEvents.onstop()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeStop(e); }
            return true;
        }

        void Interop.IHTMLDocumentEvents.onrowsdelete()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeRowsDelete(e); }
        }

        void Interop.IHTMLDocumentEvents.onrowsinserted()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeRowsInserted(e); }
        }

        void Interop.IHTMLDocumentEvents.oncellchange()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeCellchange(e); }
        }

        void Interop.IHTMLDocumentEvents.onpropertychange()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokePropertyChange(e); }
        }

        void Interop.IHTMLDocumentEvents.ondatasetchanged()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeDatasetChanged(e); }
        }

        void Interop.IHTMLDocumentEvents.ondataavailable()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeDataavailable(e); }
        }

        void Interop.IHTMLDocumentEvents.ondatasetcomplete()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeDatasetComplete(e); }
        }

        void Interop.IHTMLDocumentEvents.onbeforeeditfocus()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeBeforeeditfocus(e); }
        }

        void Interop.IHTMLDocumentEvents.onselectionchange()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeSelectionchange(e); }
        }

        bool Interop.IHTMLDocumentEvents.oncontrolselect()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeControlselect(e); return e.returnValue == null ? true : Convert.ToBoolean(e.returnValue); }
            return true;
        }

        bool Interop.IHTMLDocumentEvents.onmousewheel()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeMousewheel(e); return e.returnValue == null ? true : Convert.ToBoolean(e.returnValue); }
            return true;
        }

        void Interop.IHTMLDocumentEvents.onfocusin()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeFocusin(e); }
        }

        void Interop.IHTMLDocumentEvents.onfocusout()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeFocusout(e); }
        }

        void Interop.IHTMLDocumentEvents.onactivate()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeActivate(e); }
            return;
        }

        void Interop.IHTMLDocumentEvents.ondeactivate()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeDeactivate(e); }
            return;
        }

        bool Interop.IHTMLDocumentEvents.onbeforeactivate()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null) { ((HtmlDocumentStructure)editor.DocumentStructure).InvokeBeforeActivate(e); return e.returnValue == null ? true : Convert.ToBoolean(e.returnValue); }
            return true;
        }

        bool Interop.IHTMLDocumentEvents.onbeforedeactivate()
        {
            Interop.IHTMLEventObj e = GetEvent();
            if (e != null)
            {
                ((HtmlDocumentStructure)editor.DocumentStructure).InvokeBeforeDeactivate(e);
                return e.returnValue == null ? true : Convert.ToBoolean(e.returnValue);
            }
            return true;
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            Disconnect();
        }

        #endregion
    }

}
