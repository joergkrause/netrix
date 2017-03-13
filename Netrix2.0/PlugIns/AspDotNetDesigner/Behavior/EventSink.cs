using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using System.Collections.Generic;

namespace GuruComponents.Netrix.AspDotNetDesigner
{
    /// <summary>
    /// The purpose of this class is to deal with the events a control will
    /// fire at design time.
    /// </summary>
    internal sealed class EventSink : Interop.IHTMLGenericEvents
    {

        public static readonly string DesignTimeLockAttribute;

        private bool _allowResize;
        private DesignTimeBehavior _behavior;
        private ControlDesigner _designer;
        private Interop.IHTMLElement _element;
        private Interop.IHTMLStyle _elementStyle;
        private Interop.IHTMLStyle _runTimeStyle;
        private bool _elementLocked;
        private object _elementLockedLeft;
        private object _elementLockedTop;
        private bool _elementMoving;
        private ConnectionPointCookie _eventSinkCookie;
        private int _oldHeight;
        private int _oldWidth;
        private IHtmlEditor _editor;
        private Interop.IHTMLDocument2 _document;
        private Interop.IHTMLEventObj _eventobj;
        private IComponentChangeService _changeService;
        private static List<string> docEvents;

        
        static EventSink()
        {
            DesignTimeLockAttribute = "Design_Time_Lock";
            string[] de = new string[] { "help", "click", "dblclick", "keydown", "keyup", "keypress", "mousedown", "mousemove", "mouseup", "mouseout", "mouseover", "readystatechange", "beforeupdate", "afterupdate", "rowexit", "rowenter", "dragstart", "selectstart", "errorupdate", "contextmenu", "stop", "rowsdelete", "rowsinserted", "cellchange", "propertychange", "datasetchanged", "dataavailable", "datasetcomplete", "beforeeditfocus", "selectionchange", "controlselect", "mousewheel", "focusin", "focusout", "activate", "deactivate", "beforeactivate", "beforedeactivate" };
            docEvents = new List<string>(de);
        }

        public EventSink(DesignTimeBehavior behavior)
        {
            this._behavior = behavior;
            this._changeService = (IComponentChangeService) this._behavior.ServiceProvider.GetService(typeof (IComponentChangeService));
            this._allowResize = true;
        }
        /// <summary>
        /// Connects the specified control and its underlying element to the event sink.
        /// </summary>        
        /// <param name="element">Underlying element of control.</param>
        /// <param name="editor">Reference to editor control.</param>
        public void Connect(Interop.IHTMLElement element, IHtmlEditor editor)
        {
            this._editor = editor;            
            this._designer = (ControlDesigner)this._behavior.Designer;
            try
            {
                this._element = element;
                this._elementStyle = this._element.GetStyle();
                this._runTimeStyle = ((Interop.IHTMLElement2) this._element).GetRuntimeStyle();
                this._eventSinkCookie = new ConnectionPointCookie(this._element, this, typeof(Interop.IHTMLElementEvents));
            }
            catch (Exception)
            {
            }            
            EventsEnabled = true;
        }
        public void Disconnect()
        {
            if (this._eventSinkCookie != null)
            {
                try
                {
                    this._eventSinkCookie.Disconnect();
                    this._eventSinkCookie = null;
                }
                catch
                {
                    // on shut down the RCW might be detached alredy
                }
            }
            this._element = null;
            this._designer = null;
            this._behavior = null;
        }

        private Control Control
        {
            get
            {
                return _behavior.Control;
            }
        }
        private Interop.IHTMLEventObj GetEventObject()
        {
            _document = (Interop.IHTMLDocument2)this._element.GetDocument();
            try
            {
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

        public bool EventsEnabled;

        //void Interop.IHTMLElementEvents.Invoke(int dispid, ref Guid g, int lcid, int dwFlags, ref Interop.DISPPARAMS pdp, object pvarRes, ref Interop.EXCEPINFO pei, out int nArgError)
        void Interop.IHTMLGenericEvents.Invoke(int dispid, ref Guid g, int lcid, int dwFlags, Interop.DISPPARAMS pdp, object[] pvarRes, Interop.EXCEPINFO pei, int[] nArgError)
        {
            nArgError = new int[] { Interop.S_FALSE };
            if (!EventsEnabled) return;
            if (_element == null) return;
            _eventobj = this.GetEventObject();
            //System.Diagnostics.Debug.WriteLine(_eventobj.type, _eventobj.srcElement.GetTagName());
            if (_eventobj == null) return;
            if (_eventobj.srcElement != null)
            {
                // refire on document level to support global events in NetRix main
                if (docEvents.Contains(_eventobj.type))
                {
                    Interop.IHTMLDocument4 doc4 = (Interop.IHTMLDocument4)_element.GetDocument();
                    Interop.IHTMLEventObj newObj = doc4.createEventObject(_eventobj);
                    doc4.fireEvent(String.Concat("on", _eventobj.type), newObj);
                }
            }
            _editor.SetMousePointer(false);

            if (ElementEvent != null)
            {
                ElementEvent(this, _eventobj);
            }
            # region Event Type Switch Block
            Interop.IHTMLElement ele = _eventobj.srcElement;
            switch (_eventobj.type)
            {
                case "resizestart":
                    HandleResizeStart(_eventobj);
                    break;
                case "resize":
                    HandleResize(_eventobj);
                    break;
                case "resizeend":
                    HandleResizeEnd(_eventobj);
                    break;
                case "mousemove":
                    //if (_elementMoving)
                    //{
                    //    ((Interop.IHTMLElement3)_element).fireEvent("onmove", ref _eventobj);
                    //    //HandleMove(_eventobj);
                    //}
                    break;
                case "moveend":
                    HandleMoveEnd();
                    break;
                case "movestart":
                    HandleMoveStart();
                    break;
                case "dragstart":
                    this._behavior.StartDrag();
                    break;
                case "dragend":
                    this._behavior.EndDrag();
                    break;
                case "move":
                    HandleMove(_eventobj);
                    break;
                case "dblclick":
                    HandleDblClick(_eventobj);
                    break;
                case "propertychange":
                    break;
            }

            # endregion Event Type Switch Block
        }

        internal event ElementEventHandler ElementEvent;
        string oldStyle;

        private void HandleResizeStart(Interop.IHTMLEventObj e)
        {
            bool flag1 = this.AllowResize;
            if (!flag1)
            {
                e.cancelBubble = true;
            }
            else
            {
                _runTimeStyle.SetOverflow("hidden");
                this._oldWidth = 0; // ((Interop.IHTMLElement2)this._element).GetClientWidth();
                this._oldHeight = 0; // ((Interop.IHTMLElement2)this._element).GetClientHeight();
                e.returnValue = flag1;
                oldStyle = _elementStyle.GetCssText();
                if (_changeService != null)
                {
                    _changeService.OnComponentChanging(Control, null);
                }
            }
        }

        private void HandleResizeEnd(Interop.IHTMLEventObj e)
        {
            HandleResize();
            if (_changeService != null)
            {
                _changeService.OnComponentChanged(Control, null, oldStyle, _elementStyle.GetCssText());
            }
        }

        private void HandleResize(Interop.IHTMLEventObj e)
        {
        }

        private void HandleResize()
        {
            // handle controls only
            if (!(Control is WebControl)) return;
            WebControl wc = (WebControl)Control;
            EventsEnabled = false;
            // watch for externally set attributes
            object[] pvars = new object[1] { null };
            _element.GetAttribute("width", 0, pvars);
            if (pvars[0] != null && pvars[0].ToString().EndsWith("%"))
            {
                // percentage width
                Interop.IHTMLElement parent = _element.GetParentElement();
                int pwidth = ((Interop.IHTMLElement2)parent).GetBoundingClientRect().right -
                             ((Interop.IHTMLElement2)parent).GetBoundingClientRect().left;
                if (!this._designer.ReadOnly)
                {
                    Unit percentage = new Unit(pvars[0].ToString());
                    int px = Convert.ToInt32 (pwidth * (percentage.Value / 100));
                    _elementStyle.SetPixelWidth(px);
                    wc.Width = Unit.Pixel(px);
                }
            }
            else
            {
                if (this._designer != null)
                {
                    int width = _elementStyle.GetPixelWidth();
                    if (!this._designer.ReadOnly && (width == 0))
                    {
                        //width = this._element.GetOffsetWidth();
                    }
                    if (width != 0)
                    {
                        _elementStyle.RemoveAttribute("width", 0);
                        if (width <= 0) width = _oldWidth;
                        //System.Web.UI.WebControls.WebControl control1 = this._behavior.Control as System.Web.UI.WebControls.WebControl;
                        if (!this._designer.ReadOnly)
                        {
                            if (width != this._oldWidth)
                            {
                                wc.Width = Math.Max(0, (width));
                                _elementStyle.SetPixelWidth(width);
                                _runTimeStyle.SetPixelWidth(width + 1);
                                _oldWidth = width;
                            }
                        }
                    }
                }
            }
            _element.GetAttribute("height", 0, pvars);
            if (pvars[0] != null && pvars[0].ToString().EndsWith("%"))
            {
                // percentage height
                Interop.IHTMLElement parent = _element.GetParentElement();
                int pheight = ((Interop.IHTMLElement2)parent).GetBoundingClientRect().bottom -
                              ((Interop.IHTMLElement2)parent).GetBoundingClientRect().top;
                if (this._designer != null && !this._designer.ReadOnly)
                {
                    Unit percentage = new Unit(pvars[0].ToString());
                    int px = Convert.ToInt32(pheight * (percentage.Value / 100));
                    _elementStyle.SetPixelHeight(px);
                    wc.Height = Unit.Pixel(px);
                }
            }
            else
            {
                int height = _elementStyle.GetPixelHeight();
                if (this._designer != null)
                {
                    if ((!this._designer.ReadOnly) && height == 0)
                    {
                        //height = this._element.GetOffsetHeight();
                    }
                    if ((height != 0))
                    {
                        _elementStyle.RemoveAttribute("height", 0);
                        if (height <= 0) height = _oldHeight;
                        //System.Web.UI.WebControls.WebControl control1 = this._behavior.Control as System.Web.UI.WebControls.WebControl;
                        if (!this._designer.ReadOnly)
                        {
                            if (height != this._oldHeight)
                            {
                                wc.Height = Math.Max(0, (height));
                                _elementStyle.SetPixelHeight(height);
                                _runTimeStyle.SetPixelHeight(height + 1);
                                _oldHeight = height;
                            }
                        }
                    }
                }
            }
            _behavior.PersistProperties();
            if (_changeService != null)
            {
                //_changeService.OnComponentChanged(Control, null, oldStyle, _elementStyle.GetCssText());
            }
            EventsEnabled = true;
        }


        private void HandleDblClick(Interop.IHTMLEventObj e)
        {
            e.cancelBubble = true;
            //Control control1 = this._behavior.Control;
            EventDescriptor descriptor = TypeDescriptor.GetDefaultEvent(this._behavior.Control);
            IEventBindingService service1 = (IEventBindingService)this._behavior.ServiceProvider.GetService(typeof(IEventBindingService));
            if (descriptor != null)
            {
                string text1 = "On" + descriptor.DisplayName;
                string text2 = null;
                object[] objArray1 = new object[1];
                this._element.GetAttribute(text1, 0, objArray1);
                if (objArray1[0] != null)
                {
                    text2 = objArray1[0] as string;
                }
                if (text2 == null)
                {
                    text2 = service1.CreateUniqueMethodName(this.Control, descriptor);
                    this._element.SetAttribute(text1, text2, 0);
                }
                service1.ShowCode(this._behavior.Control, descriptor);
            }
        }

        private void HandleMove(Interop.IHTMLEventObj e)
        {
            if (this._elementMoving)
            {
                _elementMoving = false;
                int left = _elementStyle.GetPixelLeft();
                int top = _elementStyle.GetPixelTop();
                if ((!this._designer.ReadOnly && (top == 0)) && (left == 0))
                {
                    left = this._element.GetOffsetLeft();
                    top = this._element.GetOffsetTop();
                }
                if ((top != 0) || (left != 0))
                {
                    string oldStyle = _elementStyle.GetCssText();
                    if (_changeService != null)
                    {
                        _changeService.OnComponentChanging(Control, null);
                    }
                    _elementStyle.RemoveAttribute("left", 0);
                    _elementStyle.RemoveAttribute("top", 0);
                    //System.Web.UI.WebControls.WebControl control1 = this._behavior.Control as System.Web.UI.WebControls.WebControl;
                    if (this.Control is WebControl)
                    {
                        ((WebControl)this.Control).Style["left"] = left.ToString();//Unit.Pixel(e.x).ToString(); // left.ToString();
                        ((WebControl)this.Control).Style["top"] = top.ToString();//Unit.Pixel(e.y).ToString(); // top.ToString();
                    }
                    if (!this._designer.ReadOnly)
                    {
                        _elementStyle.SetPixelLeft(left);
                        _elementStyle.SetPixelTop(top);
                        Interop.IHTMLStyle runtTimeStyle = ((Interop.IHTMLElement2)this._element).GetRuntimeStyle();
                        runtTimeStyle.SetOverflow("hidden");                        
                    }
                    if (_changeService != null)
                    {
                        _changeService.OnComponentChanged(Control, null, oldStyle, _elementStyle.GetCssText());
                    }
                }
                _elementMoving = true;
            }
        }

        private void HandleMoveEnd()
        {
            //this._behavior.ElementDefaults.SetFrozen(true);
            if (this._elementMoving)
            {
                this._elementMoving = false;
                if (this._elementLocked)
                {
                    _elementStyle.SetTop(this._elementLockedTop);
                    _elementStyle.SetLeft(this._elementLockedLeft);
                    this._elementLocked = false;
                }
            }
        }
        private void HandleMoveStart()
        {
            //this._behavior.ElementDefaults.SetFrozen(false);
            Interop.IHTMLElement2 element2 = (Interop.IHTMLElement2)this._element;
            Interop.IHTMLCurrentStyle currentStyle = element2.GetCurrentStyle();
            string position = currentStyle.position;
            if ((position != null) && (string.Compare(position, "absolute", true) == 0))
            {
                this._elementMoving = true;
                int left = this._element.GetStyle().GetPixelLeft();
                if (left == 0)
                {
                    left = this._element.GetOffsetWidth();
                }
                int top = this._element.GetStyle().GetPixelTop();
                if (top == 0)
                {
                    this._element.GetOffsetHeight();
                }
                _elementStyle.SetPixelLeft(left);
                _elementStyle.SetPixelTop(top);
            }
            else
            {
                if (_editor.AbsolutePositioningEnabled)
                {
                    _elementStyle.SetAttribute("position", "absolute", 0);
                    this._elementMoving = true;
                }
            }
            if (this._elementMoving)
            {
                object[] objArray1 = new object[1];
                this._element.GetAttribute(DesignTimeLockAttribute, 0, objArray1);
                if (objArray1[0] == null)
                {
                    objArray1[0] = currentStyle.getAttribute(DesignTimeLockAttribute, 0);
                }
                if ((objArray1[0] != null) && (objArray1[0] is string))
                {
                    this._elementLocked = true;
                    this._elementLockedTop = currentStyle.top;
                    this._elementLockedLeft = currentStyle.left;
                }
            }
        }

        /// <summary>
        /// Gets <c>true</c>, when resizing is allowed. The property responds
        /// to the internally forced setting, if set to <c>false</c> . Otherwise it
        /// calls the underlying <see cref="System.Windows.Forms.Design.ControlDesigner">ControlDesigner</see> to 
        /// get the appropriate property.
        /// </summary>
        internal bool AllowResize
        {
            get
            {   
                bool ar = true;
                if (this._designer != null)
                {
                    ar = this._designer.AllowResize;
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