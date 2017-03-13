using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using System.Diagnostics;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The purpose of this class is to deal with the events a control will
    /// fire at design time.
    /// </summary>
    internal sealed class EventSink
    {

        private IElement _element;
        private ConnectionPointCookie _eventSinkCookie;
        private Interop.IHTMLElement _baseElement;
        private HTMLSpecialEvents specialEvents;

        internal HTMLSpecialEvents EventSource
        {
            get { return specialEvents; }
        }

        internal Interop.IHTMLEventObj NativeEventObject
        {
            get { return specialEvents.EventObj; }
        }

        internal class HtmlEvents
        {

            private Interop.IHTMLEventObj _eventobj;

            protected internal Interop.IHTMLEventObj EventObj
            {
                get { return _eventobj; }
                set { _eventobj = value; }
            }

            protected IElement _element;
            private Interop.IHTMLWindow2 window;

            protected HtmlEvents(IElement _element)
            {
                this._element = _element;
                Interop.IHTMLDocument2 _document = _element.HtmlEditor.GetActiveDocument(false);
                if (_document == null) return;
                window = _document.GetParentWindow();
            }

            /// <summary>
            /// This method checks whether the element has been disposed during the call.
            /// </summary>
            /// <returns></returns>
            protected bool GetSafeReturn()
            {
                if (((Element)_element).IsDisposed)
                {
                    return true;
                }
                if (_eventobj == null)
                {
                    return true;
                }
                //System.Diagnostics.Debug.WriteLine((_eventobj.returnValue == null ? true : _eventobj.returnValue), _element.ToString());
                return Convert.ToBoolean(_eventobj.returnValue == null ? true : _eventobj.returnValue);
            }

            protected internal bool GetEventObject()
            {
                try
                {
                    if (((Element)_element).IsDisposed) return false;
                    // native event object				
                    _eventobj = window.@event;
                    if (_eventobj != null && _eventobj.srcElement != null)
                    {
                        // check suppressed events
                        EventType type = (EventType)Enum.Parse(typeof(EventType), _eventobj.type, true);
                        if (_element.HtmlEditor.EventManager.GetEnabled(type))
                        {
                            //System.Diagnostics.Debug.WriteLine(_eventobj.type, _element.TagName);
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message, "GetEvent::Exception");
                }
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        internal class HTMLSpecialEvents : HtmlEvents,
            Interop.IHTMLAnchorEvents,
            Interop.IHTMLAreaEvents,
            Interop.IHTMLButtonElementEvents,
            Interop.IHTMLControlElementEvents,
            Interop.IHTMLDocumentEvents,
            Interop.IHTMLElementEvents,
            Interop.IHTMLFormElementEvents,
            Interop.IHTMLFrameSiteEvents,
            Interop.IHTMLImgEvents,
            Interop.IHTMLInputFileElementEvents,
            Interop.IHTMLInputImageEvents,
            Interop.IHTMLInputTextElementEvents,
            Interop.IHTMLLabelEvents,
            Interop.IHTMLLinkElementEvents,
            Interop.IHTMLMapEvents,
            Interop.IHTMLObjectElementEvents,
            Interop.IHTMLOptionButtonElementEvents,
            Interop.IHTMLScriptEvents,
            Interop.IHTMLSelectElementEvents,
            Interop.IHTMLStyleElementEvents,
            Interop.IHTMLTableEvents,
            Interop.IHTMLTextContainerEvents
        {

            public HTMLSpecialEvents(IElement e)
                : base(e)
            {
            }

            #region IHTMLXXXEvents Member

            public Boolean onhelp()
            {
                return false;
            }

            public Boolean onclick()
            {
                ((Element)_element).OnClick();
                return GetSafeReturn();
            }

            public Boolean ondblclick()
            {
                if (GetEventObject())
                {
                    ((Element)_element).OnDblClick();
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean onkeypress()
            {
                if (GetEventObject())
                {
                    ((Element)_element).OnKeyDown();
                }
                if (GetEventObject())
                {
                    ((Element)_element).OnKeyUp();
                }
                if (GetEventObject())
                {
                    ((Element)_element).OnKeyPress();
                    return GetSafeReturn();
                }
                return true;
            }

            public void onkeydown()
            {
                GetEventObject();
            }

            public void onkeyup()
            {
                GetEventObject();
            }

            public void onmouseout()
            {
                ((Element)_element).OnMouseOut();
            }

            public void onmouseover()
            {
                ((Element)_element).OnMouseOver();
            }

            public void onmousemove()
            {
                ((Element)_element).OnMouseMove();
            }

            public void onmousedown()
            {
                ((Element)_element).OnMouseDown();
            }

            public void onmouseup()
            {
                ((Element)_element).OnMouseUp();
            }

            public Boolean onselectstart()
            {
                ((Element)_element).OnSelectStart();
                return GetSafeReturn();
            }

            public void onfilterchange()
            {
                ((Element)_element).OnFilterChange();
            }

            public Boolean ondragstart()
            {
                ((Element)_element).OnDragStart();
                return GetSafeReturn();
            }

            public Boolean onbeforeupdate()
            {
                //((Element) _element).OnBeforeUpdate(new DocumentEventArgs(GetEventObject(), _element));
                return false;
            }

            public void onafterupdate()
            {
                //((Element) _element).OnAfterUpdate(new DocumentEventArgs(GetEventObject(), _element));
            }

            public Boolean onerrorupdate()
            {
                //((Element) _element).OnErrorUpdate(new DocumentEventArgs(GetEventObject(), _element));
                return false;
            }

            public Boolean onrowexit()
            {
                //((Element) _element).OnRowExit(new DocumentEventArgs(GetEventObject(), _element));
                return false;
            }

            public void onrowenter()
            {
                //((Element) _element).OnRowEnter(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void ondatasetchanged()
            {
                //((Element) _element).OnDatasetChanged(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void ondataavailable()
            {
                //((Element) _element).OnDataAvailable(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void ondatasetcomplete()
            {
                //((Element) _element).OnDatasetComplete(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void onlosecapture()
            {
                ((Element)_element).OnLoseCapture();
            }

            public void onpropertychange()
            {
                ((Element)_element).OnPropertyChange();
            }

            public void onscroll()
            {
                ((Element)_element).OnScroll();
            }

            public void onfocus()
            {
                ((Element)_element).OnFocus();
            }

            public void onblur()
            {
                ((Element)_element).OnBlur();
            }

            public void onresize()
            {
                ((Element)_element).OnResize();
            }

            public Boolean ondrag()
            {
                ((Element)_element).OnDrag();
                return GetSafeReturn();
            }

            public void ondragend()
            {
                ((Element)_element).OnDragEnd();
            }

            public Boolean ondragenter()
            {
                ((Element)_element).OnDragEnter();
                return GetSafeReturn();
            }

            public Boolean ondragover()
            {
                ((Element)_element).OnDragOver();
                return GetSafeReturn();
            }

            public void ondragleave()
            {
                ((Element)_element).OnDragLeave();
            }

            public Boolean ondrop()
            {
                ((Element)_element).OnDrop();
                return GetSafeReturn();
            }

            public Boolean onbeforecut()
            {
                ((Element)_element).OnBeforeCut();
                return GetSafeReturn();
            }

            public Boolean oncut()
            {
                ((Element)_element).OnCut();
                return GetSafeReturn();
            }

            public Boolean onbeforecopy()
            {
                ((Element)_element).OnBeforeCopy();
                return GetSafeReturn();
            }

            public Boolean oncopy()
            {
                ((Element)_element).OnCopy();
                return GetSafeReturn();
            }

            public Boolean onbeforepaste()
            {
                ((Element)_element).OnBeforePaste();
                return GetSafeReturn();
            }

            public Boolean onpaste()
            {
                ((Element)_element).OnPaste();
                return GetSafeReturn();
            }

            public Boolean oncontextmenu()
            {
                ((Element)_element).OnContextMenu();
                return GetSafeReturn();
            }

            public void onrowsdelete()
            {
                //((Element) _element).OnRowsDelete(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void onrowsinserted()
            {
                //((Element) _element).OnRowsInserted(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void oncellchange()
            {
                //((Element) _element).OnCellChange(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void onreadystatechange()
            {
                //((Element) _element).OnReadyStateChange(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void onbeforeeditfocus()
            {
                ((Element)_element).OnEditFocus();
            }

            public void onlayoutcomplete()
            {
                ((Element)_element).OnLayoutComplete();
            }

            public void onpage()
            {
                if (((Element)_element).Page != null && GetEventObject())
                {
                    ((Element)_element).OnPage();
                }
            }

            public Boolean onbeforedeactivate()
            {
                ((Element)_element).OnBeforeDeactivate();
                return GetSafeReturn();
            }

            public Boolean onbeforeactivate()
            {
                ((Element)_element).OnBeforeActivate();
                return GetSafeReturn();
            }

            public void onmove()
            {
                ((Element)_element).OnMove();
            }

            public Boolean oncontrolselect()
            {
                ((Element)_element).OnControlSelect();
                return GetSafeReturn();
            }

            public Boolean onmovestart()
            {
                ((Element)_element).OnMoveStart();
                return GetSafeReturn();
            }

            public void onmoveend()
            {
                try
                {
                    ((Element)_element).OnMoveEnd();
                }
                catch (Exception)
                {
                }
            }

            public Boolean onresizestart()
            {
                ((Element)_element).OnResizeStart();
                return GetSafeReturn();
            }

            public void onresizeend()
            {
                ((Element)_element).OnResizeEnd();
            }

            public void onmouseenter()
            {
                ((Element)_element).OnMouseEnter();
            }

            public void onmouseleave()
            {
                ((Element)_element).OnMouseLeave();
            }

            public Boolean onmousewheel()
            {
                ((Element)_element).OnMouseWheel();
                return GetSafeReturn();
            }

            public void onactivate()
            {
                ((Element)_element).OnActivate();
            }

            public void ondeactivate()
            {
                ((Element)_element).OnDeactivate();
            }

            public void onfocusin()
            {
                ((Element)_element).OnFocusIn();
            }

            public void onfocusout()
            {
                ((Element)_element).OnFocusOut();
            }

            public void onload()
            {
                ((Element)_element).OnLoad();
            }

            public void onerror()
            {
                ((Element)_element).OnError();
            }

            bool Interop.IHTMLObjectElementEvents.onerror()
            {
                ((Element)_element).OnError();
                return GetSafeReturn();
            }

            public Boolean onchange()
            {
                ((Element)_element).OnChange();
                return GetSafeReturn();
            }

            void Interop.IHTMLSelectElementEvents.onchange()
            {
                ((Element)_element).OnChange();
            }

            void Interop.IHTMLTextContainerEvents.onchange()
            {
                ((Element)_element).OnChange();
            }


            public void onabort()
            {
                ((Element)_element).OnAbort();
            }


            public void onselect()
            {
                ((Element)_element).OnSelect();
            }

            public void onselectionchange()
            {
                ((Element)_element).OnSelectionChange();
            }

            public bool onstop()
            {
                ((Element)_element).OnStop();
                return GetSafeReturn();
            }

            public bool onreset()
            {
                return false;
            }

            public bool onsubmit()
            {
                return false;
            }

            #endregion

        }

        public EventSink(IElement element)
        {
            this._element = element;
        }

        /// <summary>
        /// Connects the specified control and its underlying element to the event sink.
        /// </summary>
        public void Connect()
        {
            this._baseElement = _element.GetBaseElement();
            string scope = ((Interop.IHTMLElement2)_baseElement).GetScopeName();
            if (!scope.Equals("HTML")) return; // do not attach other than HTML controls
            try
            {
                Guid guid;
                switch (_element.TagName.ToLower())
                {
                    default:
                        // element
                        guid = typeof(Interop.IHTMLElementEvents).GUID;
                        break;
                    case "":
                        // generic/unknown elements
                        guid = Guid.Empty;
                        break;
                    case "body":
                    case "caption":
                    case "textarea":
                    case "td":
                    case "th":
                    case "fieldset":
                    case "legend":
                        guid = typeof(Interop.IHTMLTextContainerEvents).GUID;
                        break;
                    case "hr":
                    case "tr":
                    case "frame":
                        guid = typeof(Interop.IHTMLControlElementEvents).GUID;
                        break;
                    case "a":
                        guid = typeof(Interop.IHTMLAnchorEvents).GUID;
                        break;
                    case "area":
                        guid = typeof(Interop.IHTMLAreaEvents).GUID;
                        break;
                    case "button":
                        guid = typeof(Interop.IHTMLButtonElementEvents).GUID;
                        break;

                    case "form":
                        guid = typeof(Interop.IHTMLFormElementEvents).GUID;
                        break;
                    case "img":
                        guid = typeof(Interop.IHTMLImgEvents).GUID;
                        break;
                    case "label":
                        guid = typeof(Interop.IHTMLLabelEvents).GUID;
                        break;
                    case "link":
                        guid = typeof(Interop.IHTMLLinkElementEvents).GUID;
                        break;
                    case "map":
                        guid = typeof(Interop.IHTMLMapEvents).GUID;
                        break;
                    case "marquee":
                        guid = typeof(Interop.IHTMLMarqueeElementEvents).GUID;
                        break;
                    case "object":
                        guid = typeof(Interop.IHTMLObjectElementEvents).GUID;
                        break;
                    case "script":
                        guid = typeof(Interop.IHTMLScriptEvents).GUID;
                        break;
                    case "select":
                        guid = typeof(Interop.IHTMLSelectElementEvents).GUID;
                        break;
                    case "style":
                        guid = typeof(Interop.IHTMLStyleElementEvents).GUID;
                        break;
                    case "table":
                        guid = typeof(Interop.IHTMLTableEvents).GUID;
                        break;
                    case "input":
                        object att = _element.GetAttribute("type");
                        {
                            switch (att.ToString().ToLower())
                            {
                                case "file":
                                    guid = typeof(Interop.IHTMLInputFileElementEvents).GUID;
                                    break;
                                case "image":
                                    guid = typeof(Interop.IHTMLInputImageEvents).GUID;
                                    break;
                                case "text":
                                case "hidden":
                                case "password":
                                    guid = typeof(Interop.IHTMLInputTextElementEvents).GUID;
                                    break;
                                case "checkbox":
                                case "radio":
                                    guid = typeof(Interop.IHTMLOptionButtonElementEvents).GUID;
                                    break;
                                case "button":
                                case "submit":
                                case "reset":
                                    guid = typeof(Interop.IHTMLButtonElementEvents).GUID;
                                    break;
                                default:
                                    // control                            
                                    guid = typeof(Interop.IHTMLControlElementEvents).GUID;
                                    break;
                            }
                            break;
                        }

                }
                specialEvents = new HTMLSpecialEvents(_element);
                if (!guid.Equals(Guid.Empty))
                {
                    this._eventSinkCookie = new ConnectionPointCookie(this._baseElement, specialEvents, guid, false);
                }
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

    }

}