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
    internal sealed class EventSink : Interop.IHTMLGenericEvents
    {

        public static readonly string DesignTimeLockAttribute;
        internal event ElementEventHandler ElementEvent;
        private ViewLink _behavior;
        private Interop.IHTMLElement _element;
        private XmlControl _control;
        private ConnectionPointCookie _eventSinkCookie;
        private IHtmlEditor _editor;
        private Interop.IHTMLDocument2 _document;
        private int _oldWidth;
        private int _oldHeight;
        private Interop.IHTMLEventObj _eventobj;
        private bool _elementMoving = false;
        private bool _elementLocked = false;
        private object _elementLockedTop;
        private object _elementLockedLeft;
        private bool _allowResize = true;

        static EventSink()
        {
            EventSink.DesignTimeLockAttribute = "Design_Time_Lock";
        }
        public EventSink(ViewLink behavior)
        {
            this._behavior = behavior;
        }

        private ControlDesigner Designer
        {
            get
            {
                return (ControlDesigner)_behavior.Designer;
            }
        }

        /// <summary>
        /// Connects the specified control and its underlying element to the event sink.
        /// </summary>
        /// <param name="control">Control to connect.</param>
        /// <param name="element">Underlying element of control.</param>
        /// <param name="editor"></param>
        public void Connect(XmlControl control, Interop.IHTMLElement element, IHtmlEditor editor)
        {
            if (editor == null || control == null)
            {
                throw new NullReferenceException("Parameter does not allow null values");
            }
            this._editor = editor;
            this._control = control;
            try
            {
                this._element = element;
                this._eventSinkCookie = new ConnectionPointCookie(this._element, this, typeof(Interop.IHTMLElementEvents));
                //this._eventSinkCookie = new ConnectionPointCookie(this._element, this, typeof(Interop.IHTMLTextContainerEvents));
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
            this._behavior = null;
        }
        private Interop.IHTMLEventObj GetEventObject()
        {
            if (_element == null) return null;
            try
            {
                _document = (Interop.IHTMLDocument2)this._element.GetDocument();
                Interop.IHTMLWindow2 window1 = _document.GetParentWindow();
                return window1.@event;
            }
            catch
            {
                return null;
            }
        }
        void Interop.IHTMLGenericEvents.Bogus1()
        {
        }
        void Interop.IHTMLGenericEvents.Bogus2()
        {
        }
        void Interop.IHTMLGenericEvents.Bogus3()
        {
        }

        void Interop.IHTMLGenericEvents.Invoke(int dispid, ref Guid g, int lcid, int dwFlags, Interop.DISPPARAMS pdp, object[] pvarRes, Interop.EXCEPINFO pei, int[] nArgError)
        {
            nArgError = new int[] { Interop.S_FALSE };
            _eventobj = this.GetEventObject();
            if (_eventobj != null && _eventobj.srcElement != null)
            {

                //System.Diagnostics.Debug.WriteLineIf(_eventobj.srcElement != null, _eventobj.type, " EVENTSINK " + _eventobj.srcElement.GetTagName());

                if (ElementEvent != null) {
                    ElementEvent(this, _eventobj);
                }
                switch (_eventobj.type)
                {
                    case "help":
                        break;

                    case "click":
                        _control.InvokeClick(_eventobj);
                        break;
                    case "dblclick":
                        _control.InvokeDblClick(_eventobj);
                        break;

                    case "keypress":
                        _control.InvokeKeyPress(_eventobj);
                        break;

                    case "keydown":
                        break;
                    case "keyup":
                        break;
                    case "mouseout":
                        _control.InvokeMouseOut(_eventobj);
                        break;
                    case "mouseover":
                        _control.InvokeMouseOver(_eventobj);
                        break;
                    case "mousemove":
                        _control.InvokeMouseMove(_eventobj);
                        break;
                    case "mousedown":
                        _control.InvokeMouseDown(_eventobj);
                        break;
                    case "mouseup":
                        _control.InvokeMouseUp(_eventobj);
                        break;
                    case "selectstart":
                        _control.InvokeSelectStart(_eventobj);
                        break;
                    case "filterchange":
                        _control.InvokeFilterChange(_eventobj);
                        break;
                    case "dragstart":
                        _control.InvokeDragStart(_eventobj);
                        break;
                    case "beforeupdate":
                        break;
                    case "afterupdate":
                        break;
                    case "errorupdate":
                        break;
                    case "rowexit":
                        break;
                    case "rowenter":
                        break;
                    case "datasetchanged":
                        break;
                    case "dataavailable":
                        break;
                    case "datasetcomplete":
                        break;
                    case "losecapture":
                        _control.InvokeLoseCapture(_eventobj);
                        break;
                    case "propertychange":
                        _control.InvokePropertyChange(_eventobj);
                        break;
                    case "scroll":
                        _control.InvokeScroll(_eventobj);
                        break;
                    case "focus":
                        _control.InvokeFocus(_eventobj);
                        break;
                    case "blur":
                        _control.InvokeBlur(_eventobj);
                        break;
                    case "resize":
                        _control.InvokeResize(_eventobj);
                        OnResize();
                        break;
                    case "drag":
                        _control.InvokeDrag(_eventobj);
                        break;
                    case "dragend":
                        _control.InvokeDragEnd(_eventobj);
                        break;
                    case "dragenter":
                        _control.InvokeDragEnter(_eventobj);
                        break;
                    case "dragover":
                        _control.InvokeDragOver(_eventobj);
                        break;

                    case "dragleave":
                        _control.InvokeDragLeave(_eventobj);
                        break;
                    case "drop":
                        _control.InvokeDrop(_eventobj);
                        break;
                    case "beforecut":
                        _control.InvokeBeforeCut(_eventobj);
                        break;
                    case "cut":
                        _control.InvokeCut(_eventobj);
                        break;
                    case "beforecopy":
                        _control.InvokeBeforeCopy(_eventobj);
                        break;
                    case "copy":
                        _control.InvokeCopy(_eventobj);
                        break;
                    case "beforepaste":
                        _control.InvokeBeforePaste(_eventobj);
                        break;
                    case "paste":
                        _control.InvokePaste(_eventobj);
                        break;
                    case "contextmenu":
                        _control.InvokeContextMenu(_eventobj);
                        break;


                    case "rowsdelete":
                        break;
                    case "rowsinserted":
                        break;
                    case "cellchange":
                        break;
                    case "readystatechange":
                        break;
                    case "beforeeditfocus":
                        _control.InvokeEditFocus(_eventobj);
                        break;
                    case "layoutcomplete":
                        _control.InvokeLayoutComplete(_eventobj);
                        break;
                    case "page":
                        _control.InvokePage(_eventobj);
                        break;
                    case "beforedeactivate":
                        _control.InvokeBeforeDeactivate(_eventobj);
                        break;
                    case "beforeactivate":
                        _control.InvokeBeforeActivate(_eventobj);
                        break;
                    case "move":
                        _control.InvokeMove(_eventobj);
                        break;
                    case "controlselect":
                        _control.InvokeControlSelect(_eventobj);
                        break;
                    case "movestart":
                        _control.InvokeMoveStart(_eventobj);
                        OnMoveStart();
                        break;
                    case "moveend":
                        _control.InvokeMoveEnd(_eventobj);
                        OnMoveEnd();
                        break;
                    case "resizestart":
                        _control.InvokeResizeStart(_eventobj);
                        OnResizeStart();
                        break;
                    case "resizeend":
                        _control.InvokeResizeEnd(_eventobj);
                        break;
                    case "mouseenter":
                        _control.InvokeMouseEnter(_eventobj);
                        break;
                    case "mouseleave":
                        _control.InvokeMouseLeave(_eventobj);
                        break;
                    case "mousewheel":
                        _control.InvokeMouseWheel(_eventobj);
                        break;
                    case "activate":
                        _control.InvokeActivate(_eventobj);
                        break;
                    case "deactivate":
                        _control.InvokeDeactivate(_eventobj);
                        break;
                    case "focusin":
                        _control.InvokeFocusIn(_eventobj);
                        break;
                    case "focusout":
                        _control.InvokeFocusOut(_eventobj);
                        break;
                    case "load":
                        _control.InvokeLoad(_eventobj);
                        break;
                    case "error":
                        _control.InvokeError(_eventobj);
                        break;
                    case "change":
                        _control.InvokeChange(_eventobj);
                        break;
                    case "abort":
                        _control.InvokeAbort(_eventobj);
                        break;
                    case "select":
                        _control.InvokeSelect(_eventobj);
                        break;
                    case "selectionchange":
                        _control.InvokeSelectionChange(_eventobj);
                        break;
                    case "stop":
                        _control.InvokeStop(_eventobj);
                        break;
                    case "reset":
                        break;
                    case "submit":
                        break;
                }
            }
        }

        private void OnMoveEnd()
        {
            if (this._elementMoving)
            {
                this._elementMoving = false;
                if (this._elementLocked)
                {
                    Interop.IHTMLStyle style1 = this._element.GetStyle();
                    if (style1 != null)
                    {
                        style1.SetTop(this._elementLockedTop);
                        style1.SetLeft(this._elementLockedLeft);
                    }
                    this._elementLocked = false;
                }
                DocumentEventArgs e = new DocumentEventArgs(_eventobj, _control);
                _control.OnMoveEnd(e);
            }
        }

        private void OnMoveStart()
        {
            Interop.IHTMLElement2 element1 = (Interop.IHTMLElement2)this._element;
            Interop.IHTMLCurrentStyle style1 = element1.GetCurrentStyle();
            string text1 = style1.position;
            if ((text1 != null) && (string.Compare(text1, "absolute", true) == 0))
            {
                this._elementMoving = true;
            }
            if (this._elementMoving)
            {
                object[] objArray1 = new object[1];
                this._element.GetAttribute(EventSink.DesignTimeLockAttribute, 0, objArray1);
                if (objArray1[0] == null)
                {
                    objArray1[0] = style1.getAttribute(EventSink.DesignTimeLockAttribute, 0);
                }
                if ((objArray1[0] != null) && (objArray1[0] is string))
                {
                    this._elementLocked = true;
                    this._elementLockedTop = style1.top;
                    this._elementLockedLeft = style1.left;
                }
            }
        }

        private void OnResize()
        {
            if (!this.AllowResize) return;
            Interop.IHTMLStyle style1 = this._element.GetStyle();
            int width = style1.GetPixelWidth();
            int height = style1.GetPixelHeight();
            if (height == 0)
            {
                height = this._element.GetOffsetHeight();
            }
            if (width == 0)
            {
                width = this._element.GetOffsetWidth();
            }
            if ((height != 0) || (width != 0))
            {
                //style1.RemoveAttribute("width", 0);
                //style1.RemoveAttribute("height", 0);
                //System.Web.UI.WebControls.WebControl control1 = this._behavior.Control as System.Web.UI.WebControls.WebControl;
                if (this._control != null)
                {
                    if (height != this._oldHeight)
                    {
                        this._control.Height = System.Web.UI.WebControls.Unit.Pixel(height);
                    }
                    if (width != this._oldWidth)
                    {
                        this._control.Width = System.Web.UI.WebControls.Unit.Pixel(width);
                    }

                }
                this._oldHeight = height;
                this._oldWidth = width;
                IComponentChangeService service1 = (IComponentChangeService)this._editor.ServiceProvider.GetService(typeof(IComponentChangeService));
                if (service1 != null)
                {
                    service1.OnComponentChanged(this._control, null, null, null);
                }
                //if (!this.Designer.ReadOnly)
                {                    
                    style1.SetWidth(width);
                    style1.SetHeight(height);
                }
                //((Interop.IHTMLElement2)_element).GetRuntimeStyle().SetOverflow("visible");
                //_behavior.OnContentSave();
            }
        }

        private void OnResizeStart()
        {
            bool canresize = this.AllowResize;
            if (!canresize)
            {
                _eventobj.cancelBubble = true;
                _eventobj.returnValue = false;
            }
            this._oldWidth = ((Interop.IHTMLElement2)this._element).GetClientWidth();
            this._oldHeight = ((Interop.IHTMLElement2)this._element).GetClientHeight();

        }

        /// <summary>
        /// Gets <c>true</c>, when resizing is allowed. The property responds
        /// to the internally forced setting, if set to <c>false</c>. Otherwise it
        /// calls the underlying ControlDesigner to 
        /// get the appropriate property.
        /// </summary>
        internal bool AllowResize
        {
            get
            {
                bool ar = true;
                if (this.Designer != null)
                {
                    ar = this.Designer.AllowResize;
                }
                if (this._allowResize)
                {
                    return ar;
                }
                return false;
            }
            set
            {
                this._allowResize = value;
            }
        }

    }

}