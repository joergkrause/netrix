using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;
using System.Web.UI.Design;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.XmlDesigner
{
    /// <summary>
    /// The purpose of this class is to deal with the events a control will
    /// fire at design time.
    /// </summary>
    internal sealed class BogusSink : Interop.IHTMLTextContainerEvents
    {

        private Interop.IHTMLElement _element;
        private ConnectionPointCookie _eventSinkCookie;
        private Interop.IHTMLDocument2 _document;
        private Interop.IHTMLEventObj _eventobj;

        public BogusSink()
        {
        }

        /// <summary>
        /// Connects the specified control and its underlying element to the event sink.
        /// </summary>
        /// <param name="_control">Control to connect.</param>
        /// <param name="element">Underlying element of control.</param>
        public void Connect(Interop.IHTMLElement element)
        {
            try
            {
                this._element = element;
                this._eventSinkCookie = new ConnectionPointCookie(this._element, this, typeof(Interop.IHTMLTextContainerEvents));
            }
            catch (Exception)
            {
            }
        }
        public void Disconnect()
        {
            if (this._eventSinkCookie != null)
            {
                this._eventSinkCookie.Disconnect();
                this._eventSinkCookie = null;
            }
            this._element = null;
        }
        private Interop.IHTMLEventObj GetEventObject()
        {
            if (_element == null) return null;
            _document = (Interop.IHTMLDocument2)this._element.GetDocument();
            Interop.IHTMLWindow2 window1 = _document.GetParentWindow();
            return window1.@event;
        }

        void Invoke()
        {
            //nArgError = new int[] { Interop.S_FALSE };
            _eventobj = this.GetEventObject();
            if (_eventobj != null && _eventobj.srcElement != null)
            {

                System.Diagnostics.Debug.WriteLineIf(_eventobj.srcElement != null, _eventobj.type, _eventobj.srcElement.GetTagName());
                _eventobj.cancelBubble = true;
            }
        }

        #region IHTMLTextContainerEvents Members

        bool Interop.IHTMLTextContainerEvents.onhelp()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.onclick()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.ondblclick()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.onkeypress()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        void Interop.IHTMLTextContainerEvents.onkeydown()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onkeyup()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onmouseout()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onmouseover()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onmousemove()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onmousedown()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onmouseup()
        {
            Invoke();
        }

        bool Interop.IHTMLTextContainerEvents.onselectstart()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        void Interop.IHTMLTextContainerEvents.onfilterchange()
        {
            Invoke();
        }

        bool Interop.IHTMLTextContainerEvents.ondragstart()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.onbeforeupdate()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        void Interop.IHTMLTextContainerEvents.onafterupdate()
        {
            Invoke();
        }

        bool Interop.IHTMLTextContainerEvents.onerrorupdate()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.onrowexit()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        void Interop.IHTMLTextContainerEvents.onrowenter()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.ondatasetchanged()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.ondataavailable()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.ondatasetcomplete()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onlosecapture()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onpropertychange()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onscroll()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onfocus()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onblur()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onresize()
        {
            Invoke();
        }

        bool Interop.IHTMLTextContainerEvents.ondrag()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        void Interop.IHTMLTextContainerEvents.ondragend()
        {
            Invoke();
        }

        bool Interop.IHTMLTextContainerEvents.ondragenter()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.ondragover()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        void Interop.IHTMLTextContainerEvents.ondragleave()
        {
            Invoke();
        }

        bool Interop.IHTMLTextContainerEvents.ondrop()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.onbeforecut()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.oncut()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.onbeforecopy()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.oncopy()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.onbeforepaste()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.onpaste()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.oncontextmenu()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        void Interop.IHTMLTextContainerEvents.onrowsdelete()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onrowsinserted()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.oncellchange()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onreadystatechange()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onbeforeeditfocus()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onlayoutcomplete()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onpage()
        {
            Invoke();
        }

        bool Interop.IHTMLTextContainerEvents.onbeforedeactivate()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.onbeforeactivate()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        void Interop.IHTMLTextContainerEvents.onmove()
        {
            Invoke();
        }

        bool Interop.IHTMLTextContainerEvents.oncontrolselect()
        {
            Invoke();
            ((Interop.IHTMLElement3)_eventobj.srcElement).setActive();
            return false; // Convert.ToBoolean(_eventobj.returnValue);
        }

        bool Interop.IHTMLTextContainerEvents.onmovestart()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        void Interop.IHTMLTextContainerEvents.onmoveend()
        {
            Invoke();
        }

        bool Interop.IHTMLTextContainerEvents.onresizestart()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        void Interop.IHTMLTextContainerEvents.onresizeend()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onmouseenter()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onmouseleave()
        {
            Invoke();
        }

        bool Interop.IHTMLTextContainerEvents.onmousewheel()
        {
            Invoke();
            return Convert.ToBoolean(_eventobj.returnValue);
        }

        void Interop.IHTMLTextContainerEvents.onactivate()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.ondeactivate()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onfocusin()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onfocusout()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onchange()
        {
            Invoke();
        }

        void Interop.IHTMLTextContainerEvents.onselect()
        {
            Invoke();
        }

        #endregion
    }
}