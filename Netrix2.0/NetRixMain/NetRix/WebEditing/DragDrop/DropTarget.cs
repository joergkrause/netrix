using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Web.UI.WebControls;

namespace GuruComponents.Netrix.WebEditing.DragDrop
{
    /// <summary>
    /// Handles incoming drop events. Caller should prepare objects to insert elements, otherwise text
    /// will inserted as plain ascii into HTML.
    /// </summary>
    /// <remarks>
    /// Rev II: JK
    /// </remarks>
    internal class DropTarget : Interop.IOleDropTarget
    {                                                                                                                     
        private HtmlEditor htmlEditor;
        private Interop.IOleDropTarget _originalDropTarget;
        private DropInfo _dropInfo;
        private DataObject _currentDataObj;
        private IntPtr dataObjectPtr;
        private DragDropCommands dragData;
        private static Hashtable DragIcons;                         
        private static Hashtable DragCursor;
        private int DragPointX, DragPointY;
        private IElement nativeElement = null;
        private static Guid guid = new Guid("0000010E-0000-0000-C000-000000000046");
        
        internal DropTarget(HtmlEditor htmlEditor, DataObjectConverter dataObjectConverter, Interop.IOleDropTarget pDropTarget)
        {
            this.htmlEditor = htmlEditor;
            _originalDropTarget = pDropTarget;
            _dropInfo = new DropInfo();
            _dropInfo.Converter = dataObjectConverter;  
            if (DragIcons == null)
            {
                DragIcons = new Hashtable(21);
                DragIcons.Add(DragDropCommands.Anchor,          "WSIconAnchor.ico");
                DragIcons.Add(DragDropCommands.Break,           "WSIconBreak.ico");
                DragIcons.Add(DragDropCommands.Button,          "WSIconButton.ico");
                DragIcons.Add(DragDropCommands.Div,             "WSIconDIV.ico");                
                DragIcons.Add(DragDropCommands.Form,            "WSIconForm.ico");
                DragIcons.Add(DragDropCommands.HorizontalRule,  "WSIconHRule.ico");
                DragIcons.Add(DragDropCommands.Textbox,         "WSIconInputText.ico");
                DragIcons.Add(DragDropCommands.Checkbox,        "WSIconInputCheckBox.ico");
                DragIcons.Add(DragDropCommands.RadioButton,     "WSIconInputRadioButton.ico");
                DragIcons.Add(DragDropCommands.SubmitButton,    "WSIconOKButton.ico");
                DragIcons.Add(DragDropCommands.ListBox,         "WSIconSelectList.ico");
                DragIcons.Add(DragDropCommands.DropDown,        "WSIconDropDownList.ico");
                DragIcons.Add(DragDropCommands.Paragraph,       "WSIconParagraph.ico");
                DragIcons.Add(DragDropCommands.FileButton,      "WSIconFileButton.ico");
                DragIcons.Add(DragDropCommands.Password,        "WSIconInputPassW.ico");
                DragIcons.Add(DragDropCommands.ResetButton,     "WSIconButton.ico");
                DragIcons.Add(DragDropCommands.ImageButton,     "WSIconInputImage.ico");
                DragIcons.Add(DragDropCommands.HiddenField,     "WSIconInputHidden.ico");
                DragIcons.Add(DragDropCommands.Span,            "WSIconSpan.ico");
                DragIcons.Add(DragDropCommands.Image,           "WSIconImage.ico");
                DragIcons.Add(DragDropCommands.Table,           "WSIconTable.ico");
                DragIcons.Add(DragDropCommands.TextArea,        "WSIconTextArea.ico");
            }
            DragCursor = new Hashtable();
            System.IO.Stream st;
            string s = "GuruComponents.Netrix.Resources.DragCursors.";
            st = this.GetType().Assembly.GetManifestResourceStream(String.Concat(s, "DefaultNot.ico"));
            DragCursor.Add("DefaultNot.ico", new System.Windows.Forms.Cursor(st));
            st = this.GetType().Assembly.GetManifestResourceStream(String.Concat(s, "DefaultMove.ico"));
            DragCursor.Add("DefaultMove.ico", new System.Windows.Forms.Cursor(st));
            st = this.GetType().Assembly.GetManifestResourceStream(String.Concat(s, "DefaultCopy.ico"));
            DragCursor.Add("DefaultCopy.ico", new System.Windows.Forms.Cursor(st));
        }

        private void GetMouseCursor(DragDropCommands element)
        {
            string s = "GuruComponents.Netrix.Resources.DragCursors.";
            switch (element) 
            {
                case DragDropCommands.DefaultNot:
                    this.htmlEditor.Cursor = (Cursor) DragCursor["DefaultNot.ico"];
                    this.htmlEditor.Exec(Interop.IDM.OVERRIDE_CURSOR, true);
                    break;
                case DragDropCommands.DefaultMove:
                    this.htmlEditor.Cursor = (Cursor) DragCursor["DefaultMove.ico"];
                    this.htmlEditor.Exec(Interop.IDM.OVERRIDE_CURSOR, true);
                    break;
                case DragDropCommands.DefaultCopy:
                    this.htmlEditor.Cursor = (Cursor) DragCursor["DefaultCopy.ico"];
                    this.htmlEditor.Exec(Interop.IDM.OVERRIDE_CURSOR, true);
                    break;
                default:
                    s = String.Concat(s, DragIcons[element].ToString());
                    if (DragIcons.ContainsKey(element))
                    {
                        SetCursorFromRessource(s);
                    }
                    break;
            }
        }

        private void SetCursorFromRessource(string s)
        {
            System.IO.Stream st = this.GetType().Assembly.GetManifestResourceStream(s);
            this.htmlEditor.Cursor = new System.Windows.Forms.Cursor(st);
            this.htmlEditor.Exec(Interop.IDM.OVERRIDE_CURSOR, true);
        }

        #region IOleDropTarget Member
                                       
        public int OleDragLeave()
        {
            this._dropInfo.ConverterInfo = DataObjectConverterInfo.Disabled;            
            int result = Interop.S_OK;
            if (this._currentDataObj != null) 
            {
                this._currentDataObj = null;
                if (!dataObjectPtr.Equals(IntPtr.Zero))
                {
                    Marshal.Release(this.dataObjectPtr);
                }
                this.dataObjectPtr = IntPtr.Zero;
                result = this._originalDropTarget.OleDragLeave();
            }
            this.htmlEditor.OnDragLeave();
            return result;
        }

        public int OleDragEnter(IntPtr pDataObj, int grfKeyState, long pt, ref int pdwEffect)
        {
            // check for AllowDrop
            if (this.htmlEditor.AllowDrop == false) return Interop.S_FALSE;
            int left = (int) (pt & 0xFFFF);
            int top = (int) (pt >> 32) & 0xFFFF;
            this.DragPointX = left;
            this.DragPointY = top;
            int result = Interop.S_OK;
            object o = Marshal.GetObjectForIUnknown(pDataObj);
            _currentDataObj = new DataObject(o);
            IntPtr cptr;
            this._dropInfo.ConverterInfo = this._dropInfo.Converter.CanConvertToHtml(_currentDataObj);
            switch (this._dropInfo.ConverterInfo) 
            {
                case DataObjectConverterInfo.Native:
                    nativeElement = GetElementFromData();
                    if (nativeElement == null)
                    {
                        this._dropInfo.ConverterInfo = DataObjectConverterInfo.Unhandled;
                        goto case DataObjectConverterInfo.Unhandled;
                    }
                    // let's drop HTML but keep the serialized element
                    DataObject foolcurrentDataObj = new DataObject(DataFormats.Html, nativeElement.OuterHtml);
                    IntPtr fooldataObjectPtr = Marshal.GetIUnknownForObject(foolcurrentDataObj);
                    Marshal.QueryInterface(fooldataObjectPtr, ref guid, out cptr);
                    Marshal.Release(fooldataObjectPtr);                
                    result = this._originalDropTarget.OleDragEnter(cptr, grfKeyState, pt, ref pdwEffect);
                    break;
                case DataObjectConverterInfo.CanConvert:
                    object data = _currentDataObj.GetData(DataFormats.Serializable);
                    dragData = (DragDropCommands) data;
                    GetMouseCursor(dragData);
                    this._currentDataObj = new DataObject(DataFormats.Html, String.Empty);
                    dataObjectPtr = Marshal.GetIUnknownForObject(this._currentDataObj);
                    Marshal.QueryInterface(dataObjectPtr, ref guid, out cptr);
                    Marshal.Release(dataObjectPtr);                
                    result = this._originalDropTarget.OleDragEnter(cptr, grfKeyState, pt, ref pdwEffect);
                    break;
                case  DataObjectConverterInfo.Disabled:
                    result = Interop.S_FALSE; 
                    break;
                case DataObjectConverterInfo.Text:
                case DataObjectConverterInfo.Unhandled:                    
                    try
                    {
                        result = this._originalDropTarget.OleDragEnter(pDataObj, grfKeyState, pt, ref pdwEffect);

                        System.Diagnostics.Debug.WriteLine(_currentDataObj.GetData(DataFormats.Html));
                    }
                    catch
                    {
                    }
                    break;
                case DataObjectConverterInfo.Externally:
                    result = Interop.S_OK;
                    break;                            
            }
            DragEventArgs drgevent = CreateEventArgs(grfKeyState, left, top, (DragDropEffects)pdwEffect, (DragDropEffects)pdwEffect);
            this.htmlEditor.OnDragEnter(drgevent);
            return result;
        }

        public int OleDragOver(int grfKeyState, long pt, ref int pdwEffect)
        {

            int result = Interop.S_OK;
            int x = (int) (pt & 0xFFFF);
            int y = (int) (pt >> 32) & 0xFFFF;
            // supress flicker
            if (x != this.DragPointX || y != this.DragPointY)
            {
                GetMouseCursor(dragData);
                this.DragPointX = x;
                this.DragPointY = y;
                switch (this._dropInfo.ConverterInfo)
                {
                    case DataObjectConverterInfo.Native:
                    case DataObjectConverterInfo.CanConvert:
                        long pt2 = (x - 4) + ((long)(y - 16) << 32);
                        result = this._originalDropTarget.OleDragOver(grfKeyState, pt2, ref pdwEffect);
                        break;
                    case DataObjectConverterInfo.Text:
                    case DataObjectConverterInfo.Unhandled:
                    {
                        this.htmlEditor.Focus();
                        result = this._originalDropTarget.OleDragOver(grfKeyState, pt, ref pdwEffect);                     
                        break;
                    }
                    case DataObjectConverterInfo.Externally:
                        result = Interop.S_OK;
                        this.htmlEditor.Exec(Interop.IDM.OVERRIDE_CURSOR, false);
                        break;
                }
            }
            DragEventArgs drgevent = CreateEventArgs(grfKeyState, x, y, (DragDropEffects)pdwEffect, (DragDropEffects)pdwEffect);
            pdwEffect = (int)drgevent.AllowedEffect;
            this.htmlEditor.OnDragOver(drgevent);
            return result;
        }
        
        public int OleDrop(IntPtr pDataObj, int grfKeyState, long pt, ref int pdwEffect)
        {
            int result = Interop.S_OK;
            Control hostControl;
            Form hostForm;
            hostControl = this.htmlEditor;
            while (hostControl != null) 
            {
                hostControl = hostControl.Parent;
                hostForm = hostControl as Form;
                if (hostForm == null)
                    continue;
                hostForm.BringToFront();
                break;
            }
            int left = (int)(pt & 0xFFFF);
            int top = (int)(pt >> 32) & 0xFFFF;
            long pt2;
            object o = Marshal.GetObjectForIUnknown(pDataObj);
            _currentDataObj = new DataObject(o);
            IntPtr cptr;
            switch (this._dropInfo.ConverterInfo)
            {
                case DataObjectConverterInfo.Native:
                    // Create new DataObject                
                    if (nativeElement == null)
                    {
                        this._dropInfo.ConverterInfo = DataObjectConverterInfo.Unhandled;
                        goto case DataObjectConverterInfo.Unhandled;
                    }
                    string id = nativeElement.GetAttribute("id") as string;
                    string tempId = Guid.NewGuid().ToString();
                    nativeElement.SetAttribute("id", tempId);
                    DataObject foolcurrentDataObj = new DataObject(DataFormats.Html, nativeElement.OuterHtml);
                    IntPtr fooldataObjectPtr = Marshal.GetIUnknownForObject(foolcurrentDataObj);
                    Marshal.QueryInterface(fooldataObjectPtr, ref guid, out cptr);
                    Marshal.Release(fooldataObjectPtr);                
                    pt2 = (left - 4) + ((long)(top - 16) << 32);
                    result = this._originalDropTarget.OleDrop(cptr, grfKeyState, pt, ref pdwEffect);
                    nativeElement = htmlEditor.GetElementById(tempId);
                    nativeElement.SetAttribute("id", id);
                    break;
                case DataObjectConverterInfo.CanConvert:
                    object data = _currentDataObj.GetData(DataFormats.Serializable);
                    dragData = (DragDropCommands)data;
                    this._currentDataObj = new DataObject(DataFormats.Html, String.Empty);
                    pDataObj = Marshal.GetIUnknownForObject(this._currentDataObj);
                    Marshal.QueryInterface(dataObjectPtr, ref guid, out cptr);
                    Marshal.Release(dataObjectPtr);
                    pt2 = (left - 4) + ((long)(top - 16) << 32);
                    result = this._originalDropTarget.OleDrop(cptr, grfKeyState, pt2, ref pdwEffect);
                    break;
                case DataObjectConverterInfo.Text:
                case DataObjectConverterInfo.Unhandled:
                    result = this._originalDropTarget.OleDrop(pDataObj, grfKeyState, pt, ref pdwEffect);
                    break;
                case DataObjectConverterInfo.Externally:
                    result = Interop.S_OK;
                    break;
            }
            if (nativeElement != null)
            {
                SetElementPositioning(nativeElement, left, top);
                _currentDataObj.SetData(typeof(IElement), nativeElement);
            }
            DragEventArgs drgevent = CreateEventArgs(grfKeyState, left, top, (DragDropEffects)pdwEffect, (DragDropEffects)pdwEffect);
            this.htmlEditor.OnDragDrop(drgevent);
            pdwEffect = (int)drgevent.AllowedEffect;
            return result;
        }

        private DragEventArgs CreateEventArgs(int grfKeyState, int left, int top, DragDropEffects pdw1, DragDropEffects pdw2)
        {
            DragEventArgs drgevent = new DragEventArgs(_currentDataObj, grfKeyState, left, top, pdw1, pdw2);
            return drgevent;
        }

        #endregion

        //private int SetCaretToPointer(int grfKeyState, int x, int y, ref int pdwEffect)
        //{
        //    if (IsInAbsolutePositionMode) return Interop.S_OK;
        //    // Set caret only if not in absolute position mode...
        //    // Keystate: 4 = SHFT, 8 = CTRL, 32 = ALT, Bit 1 is set Left Mouse, 2 is Right Mouse
        //    //            if ((grfKeyState & 0x4) == 0x4)
        //    //            {
        //    //                pdwEffect = (int) Interop.DROPEFFECTS.DROPEFFECT_COPY;
        //    //            }
        //    //            else
        //    //            {
        //    //                pdwEffect = (int) Interop.DROPEFFECTS.DROPEFFECT_MOVE;
        //    //            }
        //    Interop.IDisplayServices ds = (Interop.IDisplayServices)this.htmlEditor.GetActiveDocument(true);
        //    Interop.IDisplayPointer pDispPointer;
        //    Interop.IHTMLElement pElement;
        //    ds.CreateDisplayPointer(out pDispPointer);
        //    Interop.POINT ptPoint = new GuruComponents.Netrix.ComInterop.Interop.POINT();
        //    ptPoint.x = x - 4;
        //    ptPoint.y = y - 16;
        //    uint res;
        //    try
        //    {
        //        pDispPointer.MoveToPoint(ptPoint, Interop.COORD_SYSTEM.COORD_SYSTEM_CONTAINER, null, 0, out res);
        //    }
        //    catch
        //    {
        //        // Need to catch cause some regions fail
        //    }
        //    Interop.IHTMLCaret cr;
        //    ds.GetCaret(out cr);
        //    cr.MoveDisplayPointerToCaret(pDispPointer);
        //    pDispPointer.GetFlowElement(out pElement);
        //    Interop.IHTMLElement3 p3Element = (Interop.IHTMLElement3)pElement;
        //    if (p3Element.contentEditable.Equals("false") || !p3Element.canHaveHTML)
        //    {
        //        return Interop.S_FALSE;
        //    }
        //    cr.MoveCaretToPointerEx(pDispPointer, true, true, Interop.CARET_DIRECTION.CARET_DIRECTION_SAME);
        //    cr.Show(true);
        //    return Interop.S_OK;
        //}

        ///// <summary>
        ///// Fires the Drop event with the current selection.
        ///// </summary>
        //private void OnDragDrop(IntPtr pDataObj, int grfKeyState, long pt, ref int pdwEffect)
        //{
        //    Interop.IHTMLSelectionObject selectionObj = htmlEditor.GetActiveDocument(false).GetSelection();
        //    object currentSelection = null;
        //    //DataObject iData = null;
        //    try 
        //    {
        //        currentSelection = selectionObj.CreateRange();
        //    }
        //    catch 
        //    {
        //        currentSelection = null;
        //    }
        //    // extract the xy coordinates from the screen pointer parameter
        //    int left = (int) (pt & 0xFFFF);
        //    int top = (int) (pt >> 32) & 0xFFFF;
        //    if (currentSelection != null && this._converterInfo != DataObjectConverterInfo.Externally) 
        //    {
        //        Interop.IHTMLElement el;
        //        if (currentSelection is Interop.IHTMLControlRange) 
        //        {
        //            GuruComponents.Netrix.WebEditing.Elements.IElement droppedElement = null;
        //            Interop.IHTMLControlRange ctrRange = (Interop.IHTMLControlRange) currentSelection;
        //            if (ctrRange.length == 1) 
        //            {
        //                el = (Interop.IHTMLElement) ctrRange.item(0);
        //                droppedElement = (IElement) htmlEditor.GenericElementFactory.CreateElement(el);
        //                SetElementPositioning(droppedElement, left, top);
        //                _currentDataObj = new DataObject("GuruComponents.Netrix.WebEditing.Elements.IElement", droppedElement);
        //            }                                                 
        //        }
        //        if (currentSelection is Interop.IHTMLTxtRange) 
        //        {
        //            Interop.IHTMLTxtRange textRange = (Interop.IHTMLTxtRange) currentSelection;
        //            Interop.IHTMLElement textElement = textRange.ParentElement();
        //            if (textElement != null && textElement.GetTagName().ToLower().Equals("body") || textElement == null)
        //            {
        //                string txt = textRange.GetText();
        //                string html = textRange.GetHtmlText();
        //                //if (IsInAbsolutePositionMode)
        //                //{
        //                //    iData = new DataObject(DataFormats.Html, html);
        //                //} 
        //                //else 
        //                //{
        //                //    iData = new DataObject(DataFormats.Text, txt);
        //                //}
        //            } 
        //            else 
        //            {
        //                IElement droppedTextElement = (IElement) htmlEditor.GenericElementFactory.CreateElement(textElement);
        //                SetElementPositioning(droppedTextElement, left, top);
        //                _currentDataObj = new DataObject("GuruComponents.Netrix.WebEditing.Elements.IElement", droppedTextElement);
        //            }
        //        }
        //    } 
        //    //else 
        //    //{
        //    //    object pDataPtr = Marshal.GetObjectForIUnknown(pDataObj);
        //    //    iData = new DataObject(pDataPtr);
        //    //}
        //    DragEventArgs drgevent = new DragEventArgs(_currentDataObj, grfKeyState, left, top, (DragDropEffects)pdwEffect, (DragDropEffects)pdwEffect);
        //    this.htmlEditor.OnDragDrop(drgevent);
        //    pdwEffect = (int) drgevent.AllowedEffect;
        //}

        private IElement GetElementFromData()
        {
                IElement element;
                foreach (string format in _currentDataObj.GetFormats())
                {
                    switch (format)
                    {
                        case "GuruComponents.Netrix.WebEditing.Elements.IElement":
                            element = (IElement)_currentDataObj.GetData("GuruComponents.Netrix.WebEditing.Elements.IElement");
                            return element;
                        default:
                            return null;
                    }
                }
            return null;
        }

        private void SetElementPositioning(IElement droppedElement, int left, int top)
        {
            if (droppedElement == null)
            {
                IElement newElement = htmlEditor.Selection.Element;
                if (htmlEditor.AbsolutePositioningEnabled)
                {
                    htmlEditor.AbsolutePositioningEnabled = false;
                    htmlEditor.AbsolutePositioningEnabled = true;
                    if (newElement != null)
                    {
                        newElement.SetStyleAttribute("position", "absolute");
                        System.Drawing.Point dropPoint = htmlEditor.PointToScreen(System.Drawing.Point.Empty);
                        newElement.CurrentStyle.left = Unit.Pixel(left - dropPoint.X);
                        newElement.CurrentStyle.top = Unit.Pixel(top - dropPoint.Y);
                    }
                }
                else
                {
                    if (newElement != null)
                    {
                        newElement.RemoveStyleAttribute("position");
                    }
                }
            }
            else
            {
                // if absolute positioning is enabled 
                if (htmlEditor.AbsolutePositioningEnabled)
                {
                    IElement element = droppedElement;
                    element.SetStyleAttribute("position", "absolute");
                    // offset from screen
                    System.Drawing.Point dropPoint = htmlEditor.PointToScreen(System.Drawing.Point.Empty);
                    // set element to drop position
                    element.CurrentStyle.left = Unit.Pixel(left - dropPoint.X);
                    element.CurrentStyle.top = Unit.Pixel(top - dropPoint.Y);
                }
                else
                {
                    ((IElement)droppedElement).RemoveStyleAttribute("position");
                }
            }
        }

    }
}
