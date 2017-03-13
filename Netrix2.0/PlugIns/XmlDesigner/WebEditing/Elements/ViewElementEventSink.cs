using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.Documents;
using GuruComponents.Netrix.WebEditing.Styles;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using System.ComponentModel.Design;
using GuruComponents.Netrix.Designer;
using System.Web.UI;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.XmlDesigner
{


    /// <summary>
    /// The purpose of this class is to deal with the events a control will
    /// fire at design time.
    /// </summary>
    sealed class ViewElementEventSink
    {

        private IElement _element;
        private ConnectionPointCookie _eventSinkCookie;
        private Interop.IHTMLElement _baseElement;

        private class HtmlEvents
        {

            protected Interop.IHTMLEventObj _eventobj;
            protected Interop.IHTMLElement _baseElement;
            protected IElement _element;
            private Interop.IHTMLWindow2 window;

            protected HtmlEvents(IElement _element)
            {
                this._baseElement = ((ViewElement)_element).GetBaseElement();
                this._element = _element;
                Interop.IHTMLDocument2 _document = _element.HtmlEditor.GetActiveDocument(false);
                if (_document == null) return;
                window = _document.GetParentWindow();
            }

            /// <summary>
            /// This method checks whether the element has been disposed during the call.
            /// </summary>
            /// <returns>Returns the return value set within the event handler, or false if already disposed.</returns>
            protected bool GetSafeReturn()
            {
                if (((ViewElement)_element).IsDisposed)
                    return false;
                else
                    return Convert.ToBoolean(_eventobj.returnValue == null ? true : _eventobj.returnValue);
            }

            protected bool GetEventObject()
            {
                try
                {
                    // native event object				
                    _eventobj = window.@event;
                    if (_eventobj != null && _eventobj.srcElement != null && (((ViewElement)_element).IsDisposed) == false)
                    {
                        System.Diagnostics.Debug.WriteLine(_eventobj.srcElement.GetTagName(), _eventobj.type);
                        _eventobj.cancelBubble = true;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message, "GetEvent::Exception");
                }
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private class HTMLSpecialEvents : HtmlEvents,
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

            # region DataSet Events -- not implemented

            public void onrowsdelete()
            {
                //((ViewElement) _element).OnRowsDelete(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void onrowsinserted()
            {
                //((ViewElement) _element).OnRowsInserted(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void oncellchange()
            {
                //((ViewElement) _element).OnCellChange(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void onreadystatechange()
            {
                //((ViewElement) _element).OnReadyStateChange(new DocumentEventArgs(GetEventObject(), _element));
            }
            public Boolean onbeforeupdate()
            {
                //((ViewElement) _element).OnBeforeUpdate(new DocumentEventArgs(GetEventObject(), _element));
                return false;
            }

            public void onafterupdate()
            {
                //((ViewElement) _element).OnAfterUpdate(new DocumentEventArgs(GetEventObject(), _element));
            }

            public Boolean onerrorupdate()
            {
                //((ViewElement) _element).OnErrorUpdate(new DocumentEventArgs(GetEventObject(), _element));
                return false;
            }

            public Boolean onrowexit()
            {
                //((ViewElement) _element).OnRowExit(new DocumentEventArgs(GetEventObject(), _element));
                return false;
            }

            public void onrowenter()
            {
                //((ViewElement) _element).OnRowEnter(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void ondatasetchanged()
            {
                //((ViewElement) _element).OnDatasetChanged(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void ondataavailable()
            {
                //((ViewElement) _element).OnDataAvailable(new DocumentEventArgs(GetEventObject(), _element));
            }

            public void ondatasetcomplete()
            {
                //((ViewElement) _element).OnDatasetComplete(new DocumentEventArgs(GetEventObject(), _element));
            }
            # endregion

            # region IE related high level events -- not supported

            public Boolean onhelp()
            {
                return false;
            }
            public bool onstop()
            {
                return true;
            }
            public void onpage()
            {                
            }

            public void onabort()
            {
            }
            public bool onreset()
            {
                return false;
            }

            public bool onsubmit()
            {
                return false;
            }
            # endregion

            # region Supported Events -- Canncellable
            public Boolean onclick()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeClick(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean ondblclick()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeDblClick(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean onkeypress()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeKeyPress(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean ondragstart()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeDragStart(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean ondrag()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeDrag(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }
            public Boolean ondragenter()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeDragEnter(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean ondragover()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeDragOver(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean ondrop()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeDrop(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean onbeforecut()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeBeforeCut(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean oncut()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeCut(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean onbeforecopy()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeBeforeCopy(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean oncopy()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeCopy(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean onbeforepaste()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeBeforePaste(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean onpaste()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokePaste(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean oncontextmenu()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeContextMenu(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean onbeforedeactivate()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeBeforeDeactivate(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean onbeforeactivate()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeBeforeActivate(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean oncontrolselect()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeControlSelect(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }

            public Boolean onmovestart()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeMoveStart(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }


            public Boolean onselectstart()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeSelectStart(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }
            public Boolean onresizestart()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeResizeStart(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }
            public Boolean onmousewheel()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeMouseWheel(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }
            bool Interop.IHTMLObjectElementEvents.onerror()
            {
                return true;
            }

            public Boolean onchange()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeChange(_eventobj);
                    return GetSafeReturn();
                }
                return true;
            }
            # endregion

            # region Supported Events -- Not canncellable

            public void onkeydown()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeKeyDown(_eventobj);
                }
            }

            public void onkeyup()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeKeyUp(_eventobj);
                }
            }
            public void onmouseout()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeMouseOut(_eventobj);
                }
            }

            public void onmouseover()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeMouseOver(_eventobj);
                }
            }

            public void onmousemove()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeMouseMove(_eventobj);
                }
            }

            public void onmousedown()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeMouseDown(_eventobj);
                }
            }

            public void onmouseup()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeMouseUp(_eventobj);
                }
            }

            public void onfilterchange()
            {
            }

            public void onlosecapture()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeLoseCapture(_eventobj);
                }
            }

            public void onpropertychange()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokePropertyChange(_eventobj);
                }
            }

            public void onscroll()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeScroll(_eventobj);
                }
            }

            public void onfocus()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeFocus(_eventobj);
                }
            }

            public void onblur()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeBlur(_eventobj);
                }
            }

            public void onresize()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeResize(_eventobj);
                }
            }

            public void ondragend()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeDragEnd(_eventobj);
                }
            }


            public void ondragleave()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeDragLeave(_eventobj);
                }
            }
            public void onbeforeeditfocus()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeEditFocus(_eventobj);
                }
            }

            public void onlayoutcomplete()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeLayoutComplete(_eventobj);
                }
            }
            public void onmove()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeMove(_eventobj);
                }
            }
            public void onmoveend()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeMoveEnd(_eventobj);
                }
            }
            public void onresizeend()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeResizeEnd(_eventobj);
                }
            }
            public void onmouseenter()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeMouseEnter(_eventobj);
                }
            }

            public void onmouseleave()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeMouseLeave(_eventobj);
                }
            }

            public void onactivate()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeActivate(_eventobj);
                }
            }

            public void ondeactivate()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeDeactivate(_eventobj);
                }
            }

            public void onfocusin()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeFocusIn(_eventobj);
                }
            }

            public void onfocusout()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeFocusOut(_eventobj);
                }
            }

            public void onload()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeLoad(_eventobj);
                }
            }

            public void onerror()
            {
            }


            void Interop.IHTMLSelectElementEvents.onchange()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeChange(_eventobj);
                }
            }

            void Interop.IHTMLTextContainerEvents.onchange()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeChange(_eventobj);
                }
            }

            public void onselect()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeSelect(_eventobj);
                }
            }

            public void onselectionchange()
            {
                if (GetEventObject())
                {
                    ((ViewElement)_element).InvokeSelectionChange(_eventobj);
                }
            }

            # endregion Supported Events -- Not canncellable

            #endregion

        }

        public ViewElementEventSink(IElement element)
        {
            this._element = element;
        }

        /// <summary>
        /// Connects the specified control and its underlying element to the event sink.
        /// </summary>
        public void Connect()
        {
            this._baseElement = ((ViewElement)this._element).GetBaseElement();
            string scope = ((Interop.IHTMLElement2)_baseElement).GetScopeName();
            if (!scope.Equals("HTML")) return; // do not attach other than HTML controls
            try
            {
                Guid guid = Guid.Empty;
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
                    case "fieldset":
                        guid = typeof(Interop.IHTMLTextContainerEvents).GUID;
                        break;
                    case "hr":
                    case "tr":
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
                HTMLSpecialEvents specialEvents = new HTMLSpecialEvents(_element);
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
