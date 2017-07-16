using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.Documents;
using GuruComponents.Netrix.WebEditing.DragDrop;
using GuruComponents.Netrix.WebEditing.Elements;
using Control = System.Web.UI.Control;
using HtmlWindow = GuruComponents.Netrix.WebEditing.Documents.HtmlWindow;
using System.Runtime.InteropServices.ComTypes;
using GuruComponents.Netrix.WebEditing.UndoRedo;

namespace GuruComponents.Netrix {

  /// <summary>
  /// This is the basic implementation of the MSHTML host.
  /// </summary>
  /// <remarks>
  /// This class implements the interfaces building
  /// the base services and the basic editor host, which implements TAB key, table recognition and DEL
  /// key support.
  /// </remarks>
  [ClassInterface(ClassInterfaceType.None)]
  internal class MSHTMLSite :
      Interop.IOleClientSite,
      Interop.IOleContainer,
      Interop.IOleDocumentSite,
      Interop.IOleInPlaceSite,
      Interop.IOleInPlaceSiteEx,
      Interop.IOleInPlaceFrame,
      Interop.IDocHostUIHandler,
      Interop.IDocHostShowUI,
      Interop.IPropertyNotifySink,
      Interop.IAdviseSink,
      Interop.IOleServiceProvider,
      Interop.IHTMLEditDesigner,
      IDisposable {

    /// the Control used to host (and parent) the mshtml window
    private HtmlEditor htmlEditor;

    /// <summary>
    /// the mshtml instance and various related objects
    /// </summary>
    private Interop.IOleObject oleDocumentObject;
    private Interop.IHTMLDocument2 htmlbaseDocument;
    private Interop.IOleDocumentView interopDocumentView;
    private Interop.IOleInPlaceActiveObject activeObject;
    private string _readyStateString;

    /// <summary>
    /// Show UI on start?
    /// </summary>
    private bool WithUI;

    /// <summary>
    /// cookie representing our sink
    /// </summary>
    private ConnectionPointCookie propNotifyCookie;
    private int adviseSinkCookie;
    private IntPtr windowHandle = IntPtr.Zero;


    private DataObjectConverter _dataobjectconverter;

    // Delete Key Code
    private const int DEL = 46;

    /// <summary>
    /// </summary>
    public MSHTMLSite(HtmlEditor htmlEditor) {
      if ((htmlEditor == null))// || (htmlEditor.IsHandleCreated == false))
      {
        throw new ArgumentException();
      }
      WithUI = false;
      this.htmlEditor = htmlEditor;
      this._readyStateString = String.Empty;
    }

    #region Internal used methods

    internal bool PreTranslateMessage(Message msg) {
      Interop.COMMSG lpmsg = new Interop.COMMSG();
      lpmsg.hwnd = msg.HWnd;
      lpmsg.lParam = msg.LParam;
      lpmsg.wParam = msg.WParam;
      lpmsg.message = msg.Msg;
      if (this.activeObject != null && this.activeObject.TranslateAccelerator(lpmsg) == Interop.S_OK) {
        return true;
      }
      else {
        return false;
      }
    }

    internal Interop.IOleObject OleDocument {
      get {
        return this.oleDocumentObject;
      }
    }

    public IntPtr DocumentHandle {
      get {
        return windowHandle;
      }
    }

    /// <summary>
    /// Access to drop information after dragdrop operations. The object
    /// converter contains the dropped element.
    /// </summary>
    public DataObjectConverter DataObjectConverter {
      get {
        if (_dataobjectconverter == null) {
          _dataobjectconverter = new DataObjectConverter();
        }
        return _dataobjectconverter;
      }
      set {
        _dataobjectconverter = value;
      }
    }

    /// <summary>
    /// Access to current document. The set accessor should only set to null during cleanup.
    /// </summary>
    public Interop.IHTMLDocument2 MSHTMLDocument {
      get {
        return htmlbaseDocument;
      }
      set {
        htmlbaseDocument = value;
      }
    }

    /// <overloads/>
    /// <summary>
    /// Activate with UI activation. 
    /// </summary>
    /// <remarks>
    /// UI activation means that the caret appears immediately after the designer surface appears, whether or not the
    /// control has the focus.
    /// </remarks>
    public void ActivateMSHTML() {
      ActivateMSHTML(true);
    }

    /// <summary>
    /// Activate the editor
    /// </summary>
    /// <param name="withUI">Activates the UI of the control immediately after start up.</param>
    /// <remarks>
    /// UI activation means that the caret appears immediately after the designer surface appears, whether or not the
    /// control has the focus.
    /// </remarks>
    public void ActivateMSHTML(bool withUI) {
      try {
        this.WithUI = withUI;
        Interop.RECT r = EditorRect;
        int result = OleDocument.DoVerb((int)Interop.OLE.OLEIVERB_UIACTIVATE, Interop.NullIntPtr, this, 0, EditorHandle, r);
        if (result == Interop.S_OK) {
          this.htmlEditor.NeedActivation = false;
        }
        else {
          throw new ApplicationException("Activate UI in ActivateMSHTML failed with result " + result);
        }
        htmlEditor.AddEditDesigner(this);
      }
      catch (Exception e) {
        Debug.Fail(e.ToString());
      }
    }

    private IntPtr EditorHandle {
      get {
        try {
          if (htmlEditor.IsDisposed)
            return IntPtr.Zero;
          else
            return htmlEditor.PanelHandle;
        }
        catch (Exception) {
          // Object disposed?
        }
        return IntPtr.Zero;
      }
    }

    private Interop.RECT EditorRect {
      get {
        Interop.RECT r = new Interop.RECT();
        Win32.GetClientRect(EditorHandle, r);
        return r;
      }
    }

    /// <summary>
    /// </summary>
    public void Dispose() {
      try {
        int RefCount;
        if (propNotifyCookie != null) {
          propNotifyCookie.Dispose();
          propNotifyCookie = null;
        }
        if (winEvents != null) {
          winEvents.Dispose();
          winEvents = null;
        }
        try {
          Marshal.ReleaseComObject(window);
        }
        catch {
        }
        try {
          if (interopDocumentView != null) {
            try {
              interopDocumentView.Show(0);
            }
            catch {
            }
            try {
              interopDocumentView.UIActivate(0);
            }
            catch {
            }
            try {
              interopDocumentView.SetInPlaceSite(null);
            }
            catch {
            }
            long nullParam = 0L;
            try {
              interopDocumentView.Close(nullParam);
              do {
                RefCount = Marshal.ReleaseComObject(interopDocumentView);
              } while (RefCount >= 0);
            }
            catch {
            }
            finally {
              Marshal.FinalReleaseComObject(interopDocumentView);
              interopDocumentView = null;
            }
          }
        }
        catch {
        }
        if (oleDocumentObject != null) {
          try {
            if (htmlEditor.Site == null || !htmlEditor.Site.DesignMode) {
              Marshal.FinalReleaseComObject(oleDocumentObject);
              oleDocumentObject = null;
            }
          }
          catch {
          }
        }
        if (htmlbaseDocument != null) {
          do {
            RefCount = Marshal.ReleaseComObject(htmlbaseDocument);
          } while (RefCount >= 0);
          Marshal.FinalReleaseComObject(htmlbaseDocument);
          htmlbaseDocument = null;
        }
        if (interopDocumentView != null) {
          do {
            RefCount = Marshal.ReleaseComObject(interopDocumentView);
          } while (RefCount >= 0);
        }
        if (activeObject != null) {
          do {
            RefCount = Marshal.ReleaseComObject(activeObject);
          } while (RefCount >= 0);
          Marshal.FinalReleaseComObject(activeObject);
          activeObject = null;
        }
        interopDocumentView = null;
        htmlbaseDocument = null;
        activeObject = null;
      }
      catch (Exception ex) {
        Debug.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// </summary>
    public void CreateMSHTML() {
      bool created = false;
      try {
        // create our base instance
        this.htmlbaseDocument = (Interop.IHTMLDocument2)new Interop.HTMLDocument();

        this.activeObject = (Interop.IOleInPlaceActiveObject)htmlbaseDocument;
        this.windowHandle = new IntPtr();
        this.activeObject.GetWindow(out this.windowHandle);

        oleDocumentObject = (Interop.IOleObject)htmlbaseDocument;
        if (oleDocumentObject == null) {
          throw new ApplicationException("InteropOleObject not created. No document available.");
        }
        // hand it our Interop.IOleClientSite implementation
        Win32.OleRun(htmlbaseDocument);
        oleDocumentObject.SetClientSite(this);
        Win32.OleLockRunning(htmlbaseDocument, true, false);
        created = true;
        // attach document and window base events
        propNotifyCookie = new ConnectionPointCookie(htmlbaseDocument, this, typeof(Interop.IPropertyNotifySink), false);
        // set document properties
        oleDocumentObject.SetHostNames("NetRix", "NetRix");
        // set ole events
        oleDocumentObject.Advise(this, out adviseSinkCookie);
        // set 
        IConnectionPointContainer icpc = (IConnectionPointContainer)htmlbaseDocument;
        //find the source interface
        ////get IPropertyNotifySink interface
        //Guid g = new Guid("9BFBBC02-EFF1-101A-84ED-00AA00341D07");
        //icpc.FindConnectionPoint(ref g, out icp);
        ////pass a pointer to the host to the connection point
        //icp.Advise(this._site, out this._cookie);
      }
      catch (Exception ex) {
        Debug.Fail("CreateHtml failed", ex.Message);
      }
      finally {
        if (created == false) {
          htmlbaseDocument = null;
          oleDocumentObject = null;
        }
      }
    }

    internal void SetFocus() {
      if (activeObject != null) {
        IntPtr hWnd;
        if (activeObject.GetWindow(out hWnd) == Interop.S_OK) {
          Win32.SetFocus(hWnd);
        }
      }
    }

    #endregion

    #region Internal used event fire methods

    /// <summary>
    /// </summary>
    internal void ParentResize() {
      if (interopDocumentView != null) {
        Interop.RECT r = EditorRect;
        interopDocumentView.SetRect(r);
      }
    }

    internal void ExpandView(Rectangle r) {
      Interop.RECT rect = new Interop.RECT();
      rect.right = r.Right;
      rect.bottom = r.Bottom;
      interopDocumentView.SetRect(rect);
    }

    #endregion

    #region Interop.IOleClientSite Implementation

    public int SaveObject() {
      return Interop.S_OK;
    }

    public int GetMoniker(int dwAssign, int dwWhichMoniker, out object ppmk) {
      ppmk = null;
      return Interop.E_NOTIMPL;
    }

    public int GetContainer(out Interop.IOleContainer ppContainer) {
      ppContainer = (Interop.IOleContainer)this;
      return Interop.S_OK;
    }

    public int ShowObject() {
      return Interop.S_OK;
    }

    public int OnShowWindow(int fShow) {
      return Interop.S_OK;
    }

    public int RequestNewObjectLayout() {
      return Interop.S_OK;
    }
    #endregion

    #region Interop.IOleContainer Implementation

    public void ParseDisplayName(object pbc, string pszDisplayName, int[] pchEaten, object[] ppmkOut) {
      Debug.Fail("ParseDisplayName - " + pszDisplayName);
      throw new COMException(String.Empty, Interop.E_NOTIMPL);
    }

    public void EnumObjects(int grfFlags, out Interop.IEnumUnknown ppenum) {
      ppenum = null;
      throw new COMException(String.Empty, Interop.E_NOTIMPL);
    }
    //        public void EnumObjects(int grfFlags, object[] ppenum) 
    //        {
    //            throw new COMException(String.Empty, Interop.E_NOTIMPL);
    //        }

    public void LockContainer(int fLock) {
    }

    #endregion

    #region Interop.IOleDocumentSite Implementation

    public int ActivateMe(Interop.IOleDocumentView pViewToActivate) {
      if (pViewToActivate == null)
        return Interop.E_INVALIDARG;

      Interop.RECT r = EditorRect;

      interopDocumentView = pViewToActivate;
      interopDocumentView.SetInPlaceSite(this);
      interopDocumentView.UIActivate(WithUI ? 1 : 0);
      interopDocumentView.SetRect(r);
      interopDocumentView.Show(1);

      return Interop.S_OK;
    }
    #endregion

    internal void HideCaret() {
      Win32.HideCaret(windowHandle);
    }

    internal void ShowCaret() {
      Win32.ShowCaret(windowHandle);
    }


    #region Interop.IOleInPlaceSiteEx Implementation

    int Interop.IOleInPlaceSiteEx.CanInPlaceActivate() {
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSiteEx.OnInPlaceActivate() {
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSiteEx.ContextSensitiveHelp(bool fEnterMode) {
      return Interop.E_NOTIMPL;
    }

    int Interop.IOleInPlaceSiteEx.GetWindow(ref IntPtr hwnd) {
      hwnd = IntPtr.Zero;
      if (this.htmlEditor != null) {
        hwnd = EditorHandle;
        return Interop.S_OK;
      }
      else {
        return Interop.E_FAIL;
      }
    }

    int Interop.IOleInPlaceSiteEx.OnInPlaceActivateEx(out bool pfNoRedraw, int dwFlags) {
      pfNoRedraw = false; //false means object needs to redraw
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSiteEx.OnInPlaceDeactivateEx(bool fNoRedraw) {
      //Debug.WriteLine(fNoRedraw, "OnInPlaceDeactivateEx::Enter");
      if (!fNoRedraw) {
        //redraw container
        this.htmlEditor.Invalidate();
      }
      Debug.WriteLine("OnInPlaceDeactivateEx::Leave");
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSiteEx.RequestUIActivate() {
      //Debug.WriteLine("RequestUIActivate::Enter");
      if (this.htmlEditor.Visible && this.htmlEditor.ActivationEnabled && !this.htmlEditor.StopFocusOnLoad || htmlEditor.IsReady) {
        return Interop.S_OK;
      }
      else {
        return Interop.S_FALSE;
      }
    }

    int Interop.IOleInPlaceSiteEx.OnUIActivate() {
      //Debug.WriteLine("OnUIActivate::Enter");
      //return HESULT.S_FALSE prevents focus grab
      //but means no caret
      if (this.htmlEditor.Visible && this.htmlEditor.ActivationEnabled && !this.htmlEditor.StopFocusOnLoad) {
        return Interop.S_OK;
      }
      else {
        return Interop.S_FALSE;
      }
    }

    int Interop.IOleInPlaceSiteEx.GetWindowContext(out Interop.IOleInPlaceFrame ppFrame, out Interop.IOleInPlaceUIWindow ppDoc, Interop.RECT lprcPosRect, Interop.RECT lprcClipRect, Interop.tagOIFI lpFrameInfo) {
      //Debug.WriteLine("GetWindowContext::Enter");
      ppDoc = null; //XX set to null because same as Frame window
      ppFrame = this;
      if (lprcPosRect != null) {
        Win32.GetClientRect(EditorHandle, lprcPosRect);
      }

      if (lprcClipRect != null) {
        Win32.GetClientRect(EditorHandle, lprcClipRect);
      }
      //lpFrameInfo.cb = Marshal.SizeOf(typeof(tagOIFI));
      //This value is set by the caller
      lpFrameInfo.fMDIApp = 0;
      lpFrameInfo.hwndFrame = EditorHandle;
      lpFrameInfo.hAccel = IntPtr.Zero;
      lpFrameInfo.cAccelEntries = 0;
      //Debug.WriteLine("GetWindowContext::Leave");
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSiteEx.Scroll(Interop.tagSIZE scrollExtant) {
      return Interop.E_NOTIMPL;
    }

    int Interop.IOleInPlaceSiteEx.OnUIDeactivate(int fUndoable) {
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSiteEx.OnInPlaceDeactivate() {
      activeObject = null;
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSiteEx.DiscardUndoState() {
      return Interop.E_NOTIMPL;
    }

    int Interop.IOleInPlaceSiteEx.DeactivateAndUndo() {
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSiteEx.OnPosRectChange(ref Interop.RECT lprcPosRect) {
      return Interop.S_OK;
    }

    #endregion

    #region Interop.IOleInPlaceSite Implementation

    int Interop.IOleInPlaceSite.DiscardUndoState() {
      return Interop.E_NOTIMPL;
    }

    int Interop.IOleInPlaceSite.DeactivateAndUndo() {
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSite.OnInPlaceDeactivate() {
      activeObject = null;
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSite.OnUIDeactivate(int fUndoable) {
      return Interop.S_OK;
    }

    IntPtr Interop.IOleInPlaceSite.GetWindow() {
      IntPtr hwnd = IntPtr.Zero;
      if (this.htmlEditor != null) {
        hwnd = EditorHandle;
      }
      return hwnd;
    }

    int Interop.IOleInPlaceSite.ContextSensitiveHelp(int fEnterMode) {
      return Interop.E_NOTIMPL;
    }

    int Interop.IOleInPlaceSite.CanInPlaceActivate() {
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSite.OnInPlaceActivate() {
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSite.OnUIActivate() {
      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSite.GetWindowContext(out Interop.IOleInPlaceFrame ppFrame, out Interop.IOleInPlaceUIWindow ppDoc, Interop.RECT lprcPosRect, Interop.RECT lprcClipRect, Interop.tagOIFI lpFrameInfo) {
      Debug.WriteLine("GetWindowContext2::Enter");
      ppFrame = this;
      ppDoc = null;

      Win32.GetClientRect(EditorHandle, lprcPosRect);
      Win32.GetClientRect(EditorHandle, lprcClipRect);

      lpFrameInfo.cb = Marshal.SizeOf(typeof(Interop.tagOIFI));
      lpFrameInfo.fMDIApp = 0;
      lpFrameInfo.hwndFrame = EditorHandle;
      lpFrameInfo.hAccel = Interop.NullIntPtr;
      lpFrameInfo.cAccelEntries = 0;
      Debug.WriteLine("GetWindowContext2::Leave");

      return Interop.S_OK;
    }

    int Interop.IOleInPlaceSite.Scroll(Interop.tagSIZE scrollExtant) {
      return Interop.E_NOTIMPL;
    }

    int Interop.IOleInPlaceSite.OnPosRectChange(Interop.RECT lprcPosRect) {
      return Interop.S_OK;
    }

    #endregion

    #region Interop.IOleInPlaceFrame Implementation

    IntPtr Interop.IOleInPlaceFrame.GetWindow() {
      return EditorHandle;
    }

    void Interop.IOleInPlaceFrame.ContextSensitiveHelp(int fEnterMode) {
      throw new COMException(String.Empty, Interop.E_NOTIMPL);
    }

    void Interop.IOleInPlaceFrame.GetBorder(Interop.RECT lprectBorder) {
      throw new COMException(String.Empty, Interop.E_NOTIMPL);
    }

    void Interop.IOleInPlaceFrame.RequestBorderSpace(Interop.RECT pborderwidths) {
      throw new COMException(String.Empty, Interop.E_NOTIMPL);
    }

    void Interop.IOleInPlaceFrame.SetBorderSpace(Interop.RECT pborderwidths) {
      throw new COMException(String.Empty, Interop.E_NOTIMPL);
    }

    void Interop.IOleInPlaceFrame.SetActiveObject(Interop.IOleInPlaceActiveObject pActiveObject, string pszObjName) {
      try {
        if (pActiveObject == null) {
          if (this.activeObject != null) {
            Marshal.ReleaseComObject(this.activeObject);
          }
          this.activeObject = null;
          this.windowHandle = IntPtr.Zero;
        }
        else {
          this.activeObject = pActiveObject;
          this.windowHandle = new IntPtr();
          pActiveObject.GetWindow(out this.windowHandle);
        }
      }
      catch {
      }
    }

    public void InsertMenus(IntPtr hmenuShared, Interop.tagOleMenuGroupWidths lpMenuWidths) {
      throw new COMException(String.Empty, Interop.E_NOTIMPL);
    }

    public void SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject) {
      throw new COMException(String.Empty, Interop.E_NOTIMPL);
    }

    public void RemoveMenus(IntPtr hmenuShared) {
      throw new COMException(String.Empty, Interop.E_NOTIMPL);
    }

    public void SetStatusText(string pszStatusText) {
    }

    public void EnableModeless(int fEnable) {
    }

    public int TranslateAccelerator(Interop.COMMSG lpmsg, short wID) {
      return Interop.S_FALSE;
    }
    #endregion

    #region IDocHostUIHandler Implementation

    public int ShowContextMenu(int dwID, ref Interop.POINT pt, object pcmdtReserved, object pdispReserved) {
      Point location = htmlEditor.PointToClient(new Point(pt.x, pt.y));
      Interop.IHTMLElement element = this.MSHTMLDocument.ElementFromPoint(location.X, location.Y);
      Control ielement = this.htmlEditor.GenericElementFactory.CreateElement(element);
      ShowContextMenuEventArgs e = new ShowContextMenuEventArgs(location, false, dwID, ielement);
      try {
        htmlEditor.OnShowContextMenu(e);
      }
      catch {
        // Make sure we return Interop.S_OK
      }
      return Interop.S_OK;
    }

    public int GetHostInfo(Interop.DOCHOSTUIINFO info) {
      info.dwDoubleClick = (int)Interop.DOCHOSTUIDBLCLICK.DEFAULT;
      int flags = 0;
      if (htmlEditor.NoTextSelection) {
        flags |= (int)Interop.DOCHOSTUIFLAG.DIALOG;
      }
      if (htmlEditor.AllowInPlaceNavigation) {
        flags |= (int)Interop.DOCHOSTUIFLAG.ENABLE_INPLACE_NAVIGATION;
      }
      if (htmlEditor.ImeReconversion) {
        flags |= (int)Interop.DOCHOSTUIFLAG.IME_ENABLE_RECONVERSION;
      }
      if (!htmlEditor.Border3d) {
        flags |= (int)Interop.DOCHOSTUIFLAG.NO3DBORDER;
      }
      if (!htmlEditor.ScriptEnabled) {
        flags |= (int)Interop.DOCHOSTUIFLAG.DISABLE_SCRIPT_INACTIVE;
      }
      if (!htmlEditor.ScrollBarsEnabled) {
        flags |= (int)Interop.DOCHOSTUIFLAG.SCROLL_NO;
      }
      if (htmlEditor.FlatScrollBars) {
        flags |= (int)Interop.DOCHOSTUIFLAG.FLAT_SCROLLBAR;
      }
      if (htmlEditor.BlockDefault == BlockDefaultType.DIV) {
        flags |= (int)Interop.DOCHOSTUIFLAG.DIV_BLOCKDEFAULT;
      }
      if (htmlEditor.XPTheming) {
        flags |= (int)Interop.DOCHOSTUIFLAG.THEME;
      }
      else {
        flags |= (int)Interop.DOCHOSTUIFLAG.NOTHEME;
      }
      // IE 6 Enhancements
      flags |= (int)Interop.DOCHOSTUIFLAG.DISABLE_EDIT_NS_FIXUP;
      flags |= (int)Interop.DOCHOSTUIFLAG.DISABLE_UNTRUSTEDPROTOCOL;
      // IE 7 Enhancements
      flags |= (int)Interop.DOCHOSTUIFLAG.USE_WINDOWLESS_SELECTCONTROL;
      // IE 8 Enhancements
      if (htmlEditor.AutoWordSelection) {
        //flags |= (int)Interop.DOCHOSTUIFLAG.AUTOWORD;
      }
      info.dwFlags = flags;
      return Interop.S_OK;
    }

    public int EnableModeless(bool fEnable) {
      return fEnable ? Interop.S_OK : Interop.S_FALSE;
    }

    public int ShowUI(int dwID, Interop.IOleInPlaceActiveObject activeObject, Interop.IOleCommandTarget commandTarget, Interop.IOleInPlaceFrame frame, Interop.IOleInPlaceUIWindow doc) {
      return Interop.S_FALSE;
    }

    public int HideUI() {
      return Interop.S_OK;
    }

    public int UpdateUI() {
      this.htmlEditor.OnUpdateUI(lastEventType);
      return Interop.S_OK;
    }

    public int OnDocWindowActivate(bool fActivate) {
      return Interop.E_NOTIMPL;
    }

    public int OnFrameWindowActivate(bool fActivate) {
      return Interop.E_NOTIMPL;
    }

    public int ResizeBorder(Interop.RECT rect, Interop.IOleInPlaceUIWindow doc, bool fFrameWindow) {
      return Interop.E_NOTIMPL;
    }

    public int GetOptionKeyPath(string[] pbstrKey, int dw) {
      pbstrKey[0] = null;
      return Interop.S_OK;
    }

    public int GetDropTarget(Interop.IOleDropTarget pDropTarget, out Interop.IOleDropTarget ppDropTarget) {
      if (this.htmlEditor._dropTarget == null) {
        this.htmlEditor._dropTarget = new DropTarget(this.htmlEditor, DataObjectConverter, pDropTarget);
        ppDropTarget = this.htmlEditor._dropTarget;
        return Interop.S_OK;
      }
      else {
        ppDropTarget = null; //pDropTarget;
        return Interop.S_FALSE;
      }
    }

    /// <summary>
    /// Called if in JScript windows.external.WhatEver is being executed.
    /// </summary>
    /// <remarks>
    /// E_NOTIMPL = fires native error window
    /// E_DEFAULTACTION = security exception
    /// E_FAIL = unspecified error
    /// E_ABORT = suppress an native window
    /// E_HANDLE = provide valid handle to invoke code
    /// E_UNEXPECTED = unexpected error
    /// E_POINTER = pointer expected
    /// E_NOINTERFACE = null or not object
    /// E_ACCESSDENIED = security error
    /// E_OUTOFMEMORY = out of mem error
    /// </remarks>
    /// <param name="ppDispatch"></param>
    /// <returns></returns>
    public int GetExternal(out object ppDispatch) {
      ppDispatch = ((HtmlWindow)htmlEditor.Window).ObjectForScripting;
      ScriptExternalEventArgs args = new ScriptExternalEventArgs();
      if (ppDispatch == null) {
        args.ExternalError = ScriptExternalEventArgs.ExternalErrorCode.E_ABORT;
      }
      else {
        args.ExternalError = ScriptExternalEventArgs.ExternalErrorCode.S_OK;
      }
        ((HtmlWindow)htmlEditor.Window).OnScriptExternal(args);
      return (int)args.ExternalError;
    }

    public int TranslateAccelerator(Interop.COMMSG msg, ref Guid group, int nCmdID) {
      return Interop.S_FALSE;
    }

    public int TranslateUrl(int dwTranslate, string strURLIn, out string pstrURLOut) {
      BeforeNavigateEventArgs args = new BeforeNavigateEventArgs(strURLIn);
      this.htmlEditor.OnBeforeNavigate(args);
      if (args.Cancel) {
        // This is how we cancel it, a bit weird to provide a blank, but String.Empty will not work!
        pstrURLOut = " ";
      }
      else {
        pstrURLOut = args.Url;
      }
      return pstrURLOut.Equals(strURLIn) ? Interop.S_FALSE : Interop.S_OK;
    }

    public int FilterDataObject(Interop.IOleDataObject pDO, out Interop.IOleDataObject ppDORet) {
      ppDORet = null;
      return Interop.E_NOTIMPL;
    }
    #endregion

    #region IAdviseSink Implementation

    public void OnDataChange(Interop.FORMATETC pFormat, Interop.STGMEDIUM pStg) {
    }

    public void OnViewChange(int dwAspect, int index) {
    }

    public void OnRename(object pmk) {
    }

    public void OnSave() {
    }

    public void OnClose() {
    }

    #endregion

    #region Interop.IOleServiceProvider

    public int QueryService(ref Guid sid, ref Guid iid, out IntPtr ppvObject) {
      int hr = Interop.E_NOINTERFACE;
      ppvObject = Interop.NullIntPtr;
      // ask our explicit services container
      Type type = GetTypeFromIID(sid);
      if (type != null && htmlEditor.ServiceProvider != null) {
        object service = htmlEditor.ServiceProvider.GetService(type);
        if (service != null) {
          if (iid.Equals(Interop.IID_IUnknown)) {
            ppvObject = Marshal.GetIUnknownForObject(service);
          }
          else {
            IntPtr pUnk = Marshal.GetIUnknownForObject(service);
            Marshal.QueryInterface(pUnk, ref iid, out ppvObject);
            Marshal.Release(pUnk);
            return Interop.S_OK;
          }
        }
      }
      return hr;
    }

    private static readonly Guid IUnknowGuid = new Guid("00000118-0000-0000-C000-000000000046");
    private static readonly Guid IHTMLEditDesignerGuid = new Guid("3050f662-98b5-11cf-bb82-00aa00bdce0b");
    private static readonly Guid IHTMLEditHostGuid = new Guid("3050f6a0-98b5-11cf-bb82-00aa00bdce0b");
    private static readonly Guid IAuthenticateGuid = new Guid("79EAC9D0-BAF9-11CE-8C82-00AA004BA90B");
    private static readonly Guid IHttpSecurityGuid = new Guid("79eac9d7-bafa-11ce-8c82-00aa004ba90b");
    //private static readonly Guid IOleCommandTargetGuid = new Guid("b722bccb-4e68-101b-a2bc-00aa00404770");
    private static readonly Guid IOleCommandTargetGuid = new Guid("3050f4b5-98b5-11cf-bb82-00aa00bdce0b");
    private static readonly Guid IOleUndoManagerGuid = new Guid("d001f200-ef97-11ce-9bc9-00aa00608e01");
    private static readonly Guid IInternetSecurityManagerGuid = new Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b");

    private Type GetTypeFromIID(Guid iid) {
      if (iid.Equals(IUnknowGuid))
        return null;
      if (iid.Equals(IHTMLEditDesignerGuid))
        return typeof(Interop.IHTMLEditDesigner);
      if (iid.Equals(IHTMLEditHostGuid))
        return typeof(Interop.IHTMLEditHost);
      if (iid.Equals(IAuthenticateGuid))
        return typeof(Interop.IAuthenticate);
      if (iid.Equals(IHttpSecurityGuid))
        return typeof(Interop.IHttpSecurity);
      if (iid.Equals(IOleCommandTargetGuid))
        return typeof(Interop.IOleCommandTarget);
      if (iid.Equals(IOleUndoManagerGuid))
        return typeof(Interop.IOleUndoManager);
      if (iid.Equals(IInternetSecurityManagerGuid))
        return typeof(Interop.IInternetSecurityManager);
      return null;
    }

    #endregion

    #region Interop.IPropertyNotifySink Implementation

    bool _firstChanged = false;

    public int OnChanged(int dispID) {
      try {
        switch (dispID) {
          case 1005 /*DISPID_FRAMECHANGE*/:
            if (!_firstChanged) {
              _firstChanged = true;
            }
            //                    string readyState = MSHTMLDocument.GetReadyState();
            //                    // the method will called after initialisation, this activates the site
            //                    // for the first time. Subsequent calls does not fire the ready event again.
            break;
          case DispId.READYSTATE:
            string newReadyState = this.MSHTMLDocument.GetReadyState();
            if (newReadyState != this._readyStateString) {
              _readyStateString = newReadyState;
              if (_readyStateString.Equals("complete")) {
                if (winEvents != null) {
                  winEvents.Dispose();
                  winEvents = null;
                }
                // global events
                window = htmlbaseDocument.GetParentWindow();
                winEvents = new WindowsEvents(window, htmlEditor, htmlEditor.Window);
              }
              this.htmlEditor.OnReadyStateChanged(newReadyState);
            }
            break;
        }
      }
      catch {
        return Interop.S_FALSE;
      }
      return Interop.S_OK;
    }

    public int OnRequestEdit(int dispID) {
      return Interop.S_OK;
    }

    #endregion

    #region IHtmlEditDesigner

    internal HtmlFrameSet.FrameWindow RelatedFrameWindow = null;

    private string lastFrameName = String.Empty;
    private Interop.IHTMLWindow2 window = null;
    private int returnCode = Interop.S_FALSE;
    private WindowsEvents winEvents;
    private string lastEventType;

    public int PreHandleEvent(int dispId, Interop.IHTMLEventObj e) {
      returnCode = Interop.S_FALSE;
      Interop.IHTMLElement el = e.srcElement;
      if (e.srcElement != null) {
        lastEventType = e.type;
        Control element = htmlEditor.GenericElementFactory.CreateElement(el);
        returnCode = this.htmlEditor.InvokeHtmlEvent(e, element);
        if (returnCode == Interop.S_OK || (element is IElement && !htmlEditor.DesignModeEnabled)) {
          e.cancelBubble = true;
          e.returnValue = Interop.S_OK;
        }
        else {
          if (returnCode == Interop.S_FALSE && dispId == DispId.KEYDOWN && htmlEditor.DesignModeEnabled) {
            switch (e.keyCode) {
              case DEL:
                if (this.htmlEditor.InternalShortcutKeys) {
                  try {

                    this.htmlEditor.Exec(Interop.IDM.DELETE);

                  }
                  finally {
                    returnCode = Interop.S_OK;
                  }
                }
                break;
              default:

                break;
            }
          }
        }
      }
      return returnCode;
    }

    public int PostHandleEvent(int dispId, Interop.IHTMLEventObj e) {
      return returnCode;
    }

    public int TranslateAccelerator(int dispId, Interop.IHTMLEventObj e) {
      return Interop.S_FALSE;
    }

    public int PostEditorEventNotify(int dispId, Interop.IHTMLEventObj e) {
      HandlePostEvents(dispId, e);
      return Interop.S_FALSE;
    }
    private void HandlePostEvents(int dispId, Interop.IHTMLEventObj e) {
      // For spellchecker and other late bound event sinks
      htmlEditor.InvokePostEditorEvent(new PostEditorEventArgs(e));
      if (e.srcElement != null) {
        if (dispId == DispId.KEYDOWN || dispId == DispId.MOUSEUP) {
          // We check the current scope only if the caret is visible for the user
          if (dispId == DispId.KEYDOWN) {
            //Application.DoEvents();
            IElement currentElement = this.htmlEditor.Window.GetElementFromCaret() as IElement;
            if (currentElement != null) {
              this.htmlEditor.InvokeHtmlElementChanged(currentElement.GetBaseElement(), HtmlElementChangedType.Key);
            }
            else {
              this.htmlEditor.InvokeHtmlElementChanged(htmlEditor.GetBodyElement().GetBaseElement(), HtmlElementChangedType.Key);
            }
          }
          // if a mouse click was handled the event source has the element
          if (dispId == DispId.MOUSEUP) {
            this.htmlEditor.InvokeHtmlElementChanged(e.srcElement, HtmlElementChangedType.Mouse);

          }
        }
      }

    }

    #endregion

    #region IDocHostShowUI Members

    int Interop.IDocHostShowUI.ShowMessage(IntPtr hwnd, string lpStrText, string lpstrCaption, uint dwType,
                                           string lpStrHelpFile, uint dwHelpContext, out uint lpresult) {
      // dwType 48 == Alert()
      //        33 == confirm()
      lpresult = (uint)Interop.MBID.OK;
      ShowMessageEventArgs e = new ShowMessageEventArgs(lpStrText, lpstrCaption, dwType);
      ((HtmlWindow)htmlEditor.Window).OnScriptMessage(e);
      if (e.Cancel)
        return Interop.S_OK;
      else
        return Interop.S_FALSE;
    }

    int Interop.IDocHostShowUI.ShowHelp(IntPtr hwnd, string lpHelpFile, uint uCommand, uint dwData,
                                        Interop.POINT ptMouse, object pDispatchObjectHit) {
      return Interop.S_OK;
    }

    #endregion

  }
}