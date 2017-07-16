using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.HtmlFormatting;
using GuruComponents.Netrix.HtmlFormatting.Elements;
using GuruComponents.Netrix.Networking;
using GuruComponents.Netrix.PlugIns;
using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.DragDrop;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.Exceptions;
using GuruComponents.Netrix.WebEditing.UndoRedo;
using Control = System.Web.UI.Control;
using EventBindingService = GuruComponents.Netrix.Designer.EventBindingService;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;
using System.Xml.Xsl;

# pragma warning disable 0618

namespace GuruComponents.Netrix {

  /// <summary>
  /// This is the editor control with the basic functionality.</summary>
  /// <remarks>
  /// <para>
  /// The main assembly has a satellite contract attribute to unlink the version relation between
  /// the main and the satellite assemblies. You should set all new satellites always to "1.0.0.0" to
  /// make the resource manager operate properly, even if the version of the main assembly is
  /// much higher.
  /// </para>
  /// <para>
  /// A basic application must only assure that any commands are issued after receiving of 
  /// <see cref="ReadyStateComplete"/> event. When the control loads a document, it starts an
  /// synchronous parse procedure internally. This might take some time, even if the parser is really
  /// fast and common document parse and appear within microseconds, it takes a while and it's
  /// not recommended to assume in code that this procedure is always fast enough. Therefore,
  /// the <see cref="ReadyStateComplete"/> event is the only guarantee that the content is ready
  /// and all commands can be issued safely.
  /// </para>
  /// <para>
  /// The editor has an integrated UI, that is, a toolstrip container with several toolstrips,
  /// a menustrip, and two rulers. Each part can be switched on and off using the Visual Studio.NET
  /// designer within the smart tag dialog. 
  /// Several tools are processed internally, but some are not. In any case the event
  /// <see cref="ToolItemClicked"/> is being fired and this event is cancallable. That means, the
  /// host application can cancel the internal processing in case the click is handled elsewhere.
  /// In fact , for some tools the host application must handle the click.
  /// </para>
  /// <para>
  /// Plug-Ins can extend the control, if they implement the <see cref="IPlugIn"/> interface and
  /// being implemented as ExtenderProvider. This means, they appear on the component tray and extend
  /// the base control implicitly. Additional properties appear within the PropertyGrid.
  /// </para>
  /// <para>
  /// </para>
  /// </remarks>
  /// <example>
  /// To instantiate the control just call the constructor:
  /// <code>
  /// using GuruComponents.Netrix;
  /// 
  /// HtmlEditor htmlEditor1 = new HtmlEditor();
  /// // do some stuff with the editor here</code>
  /// A basic application to implement a HTML editor solution must provide these functions:
  /// <list type="list">
  ///     <item><term>An instance of the component.</term></item>
  ///     <item><term>The <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> event attached to a handler.</term></item>
  ///     <item><term>A handler which sets the control into design mode on request.</term></item>
  /// </list>
  /// After having this the user can put the control into design mode and the application can look for the
  /// ready state complete event. It is important that no function is called before this event was fired.
  /// <para>
  /// Take a look at the various methods and properties to see more examples of how to use it.
  /// </para>
  /// </example>
  [ToolboxItem(true)]
  [ToolboxBitmap(typeof(HtmlEditor), "GuruComponents.Netrix.Resources.ToolBox.ico")]
  [DefaultEvent("ReadyStateComplete")]
  [Serializable()]
  [DesignerAttribute(typeof(NetRixControlDesigner))]
  public partial class HtmlEditor : UserControl, IHtmlEditor, IEditorInstanceService, INotifyPropertyChanged {
    #region Basic Control Variables, not for use from custom code

    #region Interop Stuff
    private Interop.IOleServiceProvider isp;
    private Interop.IHTMLEditServices es;
    private IntPtr streamPtr;

    private Guid IHtmlEditServicesGuid = new Guid("3050f663-98b5-11cf-bb82-00aa00bdce0b");
    private Guid SHtmlEditServicesGuid = new Guid("3050f7f9-98b5-11cf-bb82-00aa00bdce0b");
    private Interop.IHTMLDocument2 _activeDocument = null;
    #endregion
    #region Control Handle Stuff
    private bool _allowActivation;
    internal DropTarget _dropTarget;
    private IDesignerHost designerHost;
    private NamespaceManager _namespaceManager;
    private CommandService menuService;
    List<string> commandList;

    /// <summary>
    /// Stores the last elements hashcode. This is used to avoid multiple element changed event during mouse move
    /// or keypress if the current element is not left.
    /// </summary>
    private int _htmlElementHashCode = 0;
    #endregion
    #region Window Handle Stuff
    private ClientSite _clientSite;
    #endregion
    #region Security Stuff
    private SecurityManager _securityManager = new SecurityManager();
    private string _userName;
    private string _passWord;
    #endregion
    #region Content Stuff
    internal string Content = String.Empty;
    internal StringBuilder MhtBuilder = null;         // protocolhandler will instanciate this
    private string tmpFile = String.Empty;
    private bool tmpHasToBeRemoved = true;
    private string tmpPath = String.Empty;
    private bool _namespaceRegistered = false;
    private ChangeMonitor _changeMonitor;
    private bool _contentModified = false;
    #endregion


    private Container components = null;

    #endregion

    /// <overloads>This method has two overloads.</overloads>
    /// <summary>
    /// This is the base constructor for the control.
    /// </summary>
    /// <remarks>
    /// Calling this constructor will instantiate the
    /// editor and set the default values for all properties. It is necessary to wait for the
    /// <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> event before any action on the control starts. Returning the
    /// object does not mean that the underlying component is ready to receive commands.
    /// <para>
    /// The recommended way to use the control is the VS.NET designer. If the code is written without
    /// a designer the following call is applicable:
    /// <code>
    /// using GuruComponents.Netrix;
    /// 
    /// HtmlEditor htmlEditor1;
    /// 
    /// htmlEditor1 = new HtmlEditor();
    /// </code>
    /// </para>
    /// </remarks>
    public HtmlEditor()
        : this(true) { }

    /// <summary>
    /// This is the second constructor for the control.
    /// </summary>
    /// <remarks>
    /// Calling this constructor will instantiate the
    /// editor and set the default values for all properties. It is necessary to wait for the
    /// <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete"/> event before any action on the control starts. Returning the
    /// object does not mean that the underlying component is ready to receive commands.
    /// <para>
    /// The recommended way to use the control is the VS.NET designer. If the code is written without
    /// a designer the following call is applicable:
    /// <code>
    /// using GuruComponents.Netrix;
    /// 
    /// HtmlEditor htmlEditor1;
    /// 
    /// htmlEditor1 = new HtmlEditor(false);
    /// </code>
    /// </para>
    /// <para>
    /// If the control is put into part document mode the base skeleton of a HTML page is not build 
    /// around elements. This can be used to edit HTML fragments.
    /// </para>
    /// </remarks>
    /// <param name="fullDocumentMode"></param>
    public HtmlEditor(bool fullDocumentMode)
        : this(fullDocumentMode, false) {

    }
    /// <summary>
    /// This is the second constructor for the control.
    /// </summary>
    /// <remarks>
    /// Calling this constructor will instantiate the
    /// editor and set the default values for all properties. It is necessary to wait for the
    /// <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete"/> event before any action on the control starts. Returning the
    /// object does not mean that the underlying component is ready to receive commands.
    /// <para>
    /// The recommended way to use the control is the VS.NET designer. If the code is written without
    /// a designer the following call is applicable:
    /// <code>
    /// using GuruComponents.Netrix;
    /// 
    /// HtmlEditor htmlEditor1;
    /// 
    /// htmlEditor1 = new HtmlEditor(false);
    /// </code>
    /// </para>
    /// <para>
    /// If the control is put into part document mode the base skeleton of a HTML page is not build 
    /// around elements. This can be used to edit HTML fragments.
    /// </para>
    /// </remarks>
    /// <param name="fullDocumentMode"></param>
    /// <param name="showTools">False suppresses the additional UI .</param>
    public HtmlEditor(bool fullDocumentMode, bool showTools) {

#if !LIGHT
      InitializeComponent();
      panelEditContainer.SetEditor(this);
      // by default everything is off
      ShowVerticalRuler = showTools;
      ShowHorizontalRuler = showTools;
      StatusStripVisible = showTools;
      MenuStripVisible = showTools;
      ToolbarVisible = showTools;
      rulerControlV.Visible = showTools;
      rulerControlH.Visible = showTools;
      toolStripFile.Visible = showTools;
      toolStripFormat.Visible = showTools;
      toolStripEdit.Visible = showTools;
      menuStrip1.Visible = showTools;
      panelEditContainer.Resize += new EventHandler(panelEditContainer_Resize);
      panelEditContainer.GotFocus += new EventHandler(panelEditContainer_GotFocus);
      panelEditContainer.VisibleChanged += new EventHandler(panelEditContainer_VisibleChanged);
      panelEditContainer.HandleCreated += new EventHandler(panelEditContainer_HandleCreated);
      panelEditContainer.HandleDestroyed += new EventHandler(panelEditContainer_HandleDestroyed);
      panelEditContainer.ParentChanged += new EventHandler(panelEditContainer_ParentChanged);
      // the toolstrip cont is the outer most control. It receives the focus and we need to send it down to MSHTML
      toolStripContainer1.GotFocus += new EventHandler(toolStripContainer1_GotFocus);

      toolBarDockStyle = DockStyle.Top;
#endif

      commandList = new List<string>(Enum.GetNames(typeof(HtmlCommand)));


      components = new Container();
      // Preparation
      _allowActivation = true;
      _fullDocumentMode = fullDocumentMode;
      _internalShortcutKeys = true;
      // Scroll bars should be enabled by default
      _scrollBarsEnabled = true;
      IsCreated = false;
      // Force control creation before MSHTML is created
      this.CreateControl();
      // put in the desired state
      _designModeDesired = false;
      _designModeDesiredValue = _designModeEnabled;
      this.Exec(_designModeEnabled ? Interop.IDM.EDITMODE : Interop.IDM.BROWSEMODE);
    }

    void toolStripContainer1_GotFocus(object sender, EventArgs e) {
      Focus();
    }


    /// <summary>
    /// Compiled as trial version.
    /// </summary>
    [Category("NetRix Licensing")]
    [ReadOnly(true)]
    public bool IsTrial
    {
      get
      {
#if TRIAL
				return true;
#else
        return false;
#endif

      }
    }

    void panelEditContainer_ParentChanged(object sender, EventArgs e) {
      OnParentChanged(e);
    }

    void panelEditContainer_HandleDestroyed(object sender, EventArgs e) {
      OnHandleDestroyed(e);
    }

    void panelEditContainer_HandleCreated(object sender, EventArgs e) {
      OnPanelCreated(e);
    }

    void panelEditContainer_VisibleChanged(object sender, EventArgs e) {
      OnVisibleChanged(e);
    }

    void panelEditContainer_GotFocus(object sender, EventArgs e) {
      PanelGotFocus(e);
    }

    void panelEditContainer_Resize(object sender, EventArgs e) {
      if (MshtmlSite != null) {
        MshtmlSite.ParentResize();
      }
    }

    #region Creation

    /// <summary>
    /// <para>
    /// We can only activate the MSHTML after our handle has been created,
    /// so upon creating the handle, we create and activate Interop.
    /// </para>
    /// <para>
    /// If LoadHtml was called prior to this, we do the loading now.
    /// </para>
    /// </summary>
    /// <param name="args"></param>
    protected void OnPanelCreated(EventArgs args) {
      if (!IsCreated && !DesignMode) {
        CreateMshtml();
      }
    }

    protected override void OnHandleDestroyed(EventArgs e) {
      if (!DesignMode) {
        NeedActivation = true;
      }
      base.OnHandleDestroyed(e);
    }

    protected override void OnParentChanged(EventArgs e) {
      if (MshtmlSite == null && !DesignMode) {
        CreateMshtml();
      }
      base.OnParentChanged(e);
    }

    protected override void OnVisibleChanged(EventArgs e) {
      if (NeedActivation && !DesignMode) {
        if (MshtmlSite == null) {
          CreateMshtml();
        } else {
          MshtmlSite.ActivateMSHTML();
        }
      }
    }

    private void CreateMshtml() {
      // All Basic Interfaces
      MshtmlSite = new MSHTMLSite(this);
      MshtmlSite.CreateMSHTML();
      if (this.Visible) {
        NeedActivation = false;
        MshtmlSite.ActivateMSHTML();
      } else {
        // This delayes the activation until the control becomes visible
        NeedActivation = true;
      }
      IsCreated = true;
      // set some conditions the control should running in 
      this.Exec(Interop.IDM.PERSISTDEFAULTVALUES, true);
      this.Exec(Interop.IDM.PROTECTMETATAGS, true);
      this.Exec(Interop.IDM.PRESERVEUNDOALWAYS, true);
      this.Exec(Interop.IDM.NOACTIVATENORMALOLECONTROLS, true);
      this.Exec(Interop.IDM.NOACTIVATEDESIGNTIMECONTROLS, true);
      this.Exec(Interop.IDM.NOACTIVATEJAVAAPPLETS, true);
      this.Exec(Interop.IDM.NOFIXUPURLSONPASTE, true);
      // first time set all properties
      ResetDesiredProperties(false);
    }

    #endregion

    #region Native Windows Stuff Option (For Customization Only)

    /// <summary>
    /// The HideCaret function removes the caret from the screen. 
    /// </summary>
    /// <remarks>
    /// Hiding a caret does not destroy its current shape or invalidate the insertion point.
    /// </remarks>
    public void HideCaret() {
      MshtmlSite.HideCaret();
      Interop.IDisplayServices ds = (Interop.IDisplayServices)GetActiveDocument(false);
      Interop.IHTMLCaret c;
      ds.GetCaret(out c);
      c.Hide();
    }

    /// <summary>
    /// The ShowCaret function makes the caret visible on the screen at the caret's current position.
    /// </summary>
    /// <remarks>
    /// When the caret becomes visible, it begins flashing automatically. 
    /// ShowCaret shows the caret only if the specified window owns the caret, the caret has a shape, and the caret has not been hidden two or more times in a row. If one or more of these conditions is not met, ShowCaret does nothing and returns <c>false</c>.
    /// <para>
    /// Hiding is cumulative. If your application calls HideCaret five times in a row, it must also call ShowCaret five times before the caret reappears.
    /// </para>            
    /// <para>
    /// The system provides one caret per queue. A window should create a caret only when it has the keyboard focus or is active. The window should destroy the caret before losing the keyboard focus or becoming inactive.
    /// In NetRix this is the default behavior and there is no need to manipulate the caret directly but a different behavior is required.
    /// </para>
    /// </remarks>
    public void ShowCaret() {
      MshtmlSite.ShowCaret();
      Interop.IDisplayServices ds = (Interop.IDisplayServices)GetActiveDocument(false);
      Interop.IHTMLCaret c;
      ds.GetCaret(out c);
      c.Show(true);
    }

    #endregion

    /// <summary>
    /// This methods returns one of the inner types not available in the Core assembly.
    /// </summary>
    /// <remarks>
    /// This method is used by plug-ins to get access to internal types. The method works lazy and
    /// returns <c>null</c> (<c>Nothing</c> in VB) in case of error.
    /// </remarks>
    /// <param name="typeName">The full qualified name.</param>
    /// <returns>The Type, if the name was found and is declared in 'Guru.Netrix.Professional.Editor.v4.dll'.</returns>
    public Type ReflectInnerType(string typeName) {
      return Type.GetType(typeName, false, true);
    }

    #region Internally used Editor functions


    /// <summary>
    /// Set the editor window zoom level temporarily to given value.
    /// </summary>
    /// <remarks>
    /// This setting does not persist. Reloading document removes zoom level and sets document to 100%.
    /// </remarks>
    /// <seealso cref="IDocument.Zoom"/>
    /// <seealso cref="GetZoom"/>
    /// <param name="ratio">Value for zoom, 1 equals 100%. Set 0.5 for 50% or 2.0 for 200%.</param>
    public void Zoom(decimal ratio) {
      GetBodyElement().RuntimeStyle.Zoom = (((double)ratio * 100.0) + "%").ToString();
    }

    /// <summary>
    /// Returns the current temporary Zoom level.
    /// </summary>
    /// <seealso cref="IDocument.Zoom"/>
    /// <seealso cref="Zoom"/>
    /// <returns>Zoom value, return 1 for 100%, 0.5 for 50% or 2.0 for 200%.</returns>
    public decimal GetZoom() {
      decimal result;
      if (Decimal.TryParse(GetBodyElement().RuntimeStyle.Zoom, out result)) {
        return result;
      } else {
        return 1.0M;
      }
    }

    /// <internalonly/>
    /// <summary>
    /// Set the standard designer which is responsible for all base events.
    /// </summary>
    /// <remarks>
    /// Called in readystate "complete" state and before external event handling.
    /// </remarks>
    internal protected void SetEditDesigner() {
      // prepare add designer methods
      isp = (Interop.IOleServiceProvider)MshtmlSite.MSHTMLDocument;
      Interop.IHtmlBodyElement body = this.GetBodyThreadSafe(false);
      if (isp != null && body != null) {
        IntPtr ppv = IntPtr.Zero;
        try {
          isp.QueryService(ref SHtmlEditServicesGuid, ref IHtmlEditServicesGuid, out ppv);
          es = Marshal.GetObjectForIUnknown(ppv) as Interop.IHTMLEditServices;
          foreach (Interop.IHTMLEditDesigner designer in _editDesigners) {
            es.AddDesigner(designer);
          }
        } catch {
        } finally {
          Marshal.Release(ppv);
        }
      }
    }

    /// <summary>
    /// Add a external designer to the designer chain. 
    /// </summary>
    /// <remarks>
    /// This is done by the various PlugIns the control accepts. The designers attached are loaded during
    /// readystate complete phase. This means, this method takes effect only for new documents.
    /// <para>
    /// Scenario: Create Designer > Add Designer > Load Content > Wait for ReadyStateComplete > Designer receives events
    /// </para>
    /// </remarks>
    /// <param name="designer"></param>
    public void AddEditDesigner(object designer) {
      if (!this._editDesigners.Contains(designer)) {
        _editDesigners.Add(designer);
      }
    }

    /// <summary>
    /// Access to native object of either the current main document or the current active frame document.
    /// </summary>
    /// <remarks>
    /// The purpose of this method is access to underyling COM document for extensibility and
    /// customization. Applications which host the control and deal with the content ususally do not
    /// access the underyling COM wrapper due to some impications required by COM/Interop. 
    /// </remarks>
    public Interop.IHTMLDocument2 GetActiveDocument(bool baseDocument) {
      if (MshtmlSite != null) {
        if (_activeDocument != null && !baseDocument) {
          return _activeDocument;
        } else {
          return MshtmlSite.MSHTMLDocument;
        }
      } else {
        return null;
      }
    }

    /// <summary>
    /// This method sets the current active frame document, so any task starts later 
    /// uses this document instead of the base doc.
    /// </summary>
    /// <param name="document"></param>
    internal void SetActiveFrameDocument(Interop.IHTMLDocument2 document) {
      this._activeDocument = document;
    }

    /// <summary>
    /// Searches the element tree upwards to detect the next element the current element is in.
    /// </summary>
    /// <remarks>
    /// Search stops and fails if body is reached ==> element is not in document tree.
    /// Search returns the first occurence of element, therefore the innermost element of a nested
    /// structure is recognized.
    /// </remarks>
    /// <param name="element">Interop.IHTMLElement element or null if not found.</param>
    /// <param name="tagName">The tag name which the methods searches for.</param>
    internal Interop.IHTMLElement GetParentElement(Interop.IHTMLElement element, string tagName) {
      if (element.GetTagName() == tagName) {
        return element;
      }
      Interop.IHTMLElement parent = element.GetParentElement();
      while (parent != null) {
        if (parent.GetTagName() == tagName.ToUpper())
          break;
        parent = parent.GetParentElement();
      }
      return parent;
    }

    /// <summary>
    /// Searches the element tree upwards to detect the next element the current element is in.
    /// </summary>
    /// <remarks>
    /// Search stops and fails if body is reached ==> element is not in document tree.
    /// Search returns the first occurence of element, therefore the innermost element of a nested
    /// structure is recognized.
    /// </remarks>
    /// <param name="element">IElement element or null if not found.</param>
    /// <param name="tagName">The tag name which the methods searches for.</param>
    /// <returns>Returns the element, if found.</returns>
    public Control GetParentFromHierarchy(IElement element, string tagName) {
      if (element == null)
        return null;
      Interop.IHTMLElement parent = this.GetParentElement(((IElement)element).GetBaseElement(), tagName);
      if (parent != null) {
        return GenericElementFactory.CreateElement(parent);
      }
      return null;
    }

    /// <summary>
    /// Gets the command target to send IDM commands to the component.
    /// </summary>
    internal Interop.IOleCommandTarget CommandTarget
    {
      get
      {
        return (Interop.IOleCommandTarget)MshtmlSite.MSHTMLDocument;
      }
    }

    internal string BaseUrl
    {
      get
      {
        return Url;
      }
    }

    /// <summary>
    /// Set or hide the current mouse pointer. 
    /// </summary>
    /// <remarks>
    /// Hiding does not remove the pointer but display the last standard pointer instead of the current.
    /// </remarks>
    /// <param name="Hide">If true reset to the standard pointer</param>
    public void SetMousePointer(bool Hide) {
      Exec(Interop.IDM.OVERRIDE_CURSOR, Hide);
    }

    /// <summary>
    /// Executes the specified command in MSHTML directly.
    /// </summary>
    /// <param name="command">The command. Not all commands are supported in all situations.</param>
    public void Exec(Interop.IDM command) {
      Exec(command, null);
    }

    /// <summary>
    /// Executes the specified command in MSHTML with the specified arguments.
    /// </summary>
    /// <param name="command">The command. Not all commands are supported in all situations.</param>
    /// <param name="argument">The argument. Supported types are string, bool, int and short.</param>
    public void Exec(Interop.IDM command, object argument) {
      if (CommandTarget == null)
        return;
      Interop.OLEVARIANT nil = new Interop.OLEVARIANT();
      Interop.OLEVARIANT arg = new Interop.OLEVARIANT();
      if (argument != null) {
        switch (argument.GetType().ToString()) {
          case "System.String":
            arg.LoadString((string)argument);
            break;
          case "System.Boolean":
            arg.LoadBoolean((bool)argument);
            break;
          case "System.Int32":
          case "System.Int16":
            arg.LoadInteger((int)argument);
            break;
          default:
            throw new ArgumentException(String.Concat("Unknown Type: ", argument.GetType(), " Argument: ", argument));
        }
      }
      try {
        int hr;
        hr = CommandTarget.Exec(ref Interop.Guid_MSHTML, (int)command, (int)Interop.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, arg, nil);
        if (hr != Interop.S_OK) {
          Debug.WriteLine(hr, "EXEC -> ERROR");
          //throw new CommandUnavailableException(command, "HtmlEditor.Exec: Command fails. Returncode was: " + hr);
        }
      } catch (Exception ex) {
        Debug.WriteLine(ex.Message, "EXEC -> EX -> ERROR");
        //throw new CommandUnavailableException(command, "HtmlEditor.Exec: Command fails. See for more details InnerException", ex);
      }
    }

    /// <summary>
    /// Executes the specified command in MSHTML and returns the result as 
    /// <see cref="GuruComponents.Netrix.ComInterop.Interop.OLEVARIANT">OLEVARIANT</see>. The caller
    /// is responsible for correct preparing and marshaling of the structure.
    /// </summary>
    /// <param name="command">The command send to MSHTML</param>
    /// <param name="retVal">Return value as <see cref="GuruComponents.Netrix.ComInterop.Interop.OLEVARIANT">OLEVARIANT</see></param>
    /// <returns>object result - The result of the command</returns>
    internal void ExecResult(Interop.IDM command, ref Interop.OLEVARIANT retVal) {
      Interop.OLEVARIANT nil = new Interop.OLEVARIANT();
      try {
        int hr;
        hr = CommandTarget.Exec(ref Interop.Guid_MSHTML, (int)command, (int)Interop.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, nil, retVal);
        if (hr != Interop.S_OK) {
          throw new CommandUnavailableException((int)command, "HtmlEditor.Exec: Command fails. Returncode was: " + hr);
          //System.Diagnostics.Debug.WriteLine(command + " fails with code " + hr, "Exec FAIL");
        }
      } catch (Exception) {
        //throw new CommandUnavailableException((int)command, "HtmlEditor.Exec: Command fails. See for more details InnerException", ex);
      }
    }


    /// <summary>
    /// Send a command to MSHTML and excepts to get an bool value back.
    /// If the command does not work or does not return the expected type an exception is thrown.
    /// </summary>
    /// <param name="command">The command</param>
    /// <returns>bool value or false</returns>
    internal bool ExecResultBoolean(Interop.IDM command) {
      Interop.OLEVARIANT oleVar = new Interop.OLEVARIANT();
      ExecResult(command, ref oleVar);
      oleVar.vt = Convert.ToInt16(VarEnum.VT_BOOL);
      object o = oleVar.ToNativeObject();
      oleVar.Clear();
      if (o is bool) {
        return (bool)o;
      } else {
        return false;
      }
    }

    /// <summary>
    /// Send a command to MSHTML and excepts to get an string value back
    /// If the command does not work or does not return the expected type an exception is thrown.
    /// </summary>
    /// <param name="command">The command</param>
    /// <returns>string value or String.Empty</returns>
    internal string ExecResultString(Interop.IDM command) {
      Interop.OLEVARIANT oleVar = new Interop.OLEVARIANT();
      //oleVar.vt = Convert.ToInt16(VarEnum.VT_BSTR);
      ExecResult(command, ref oleVar);
      oleVar.vt = Convert.ToInt16(VarEnum.VT_BSTR);
      object o = oleVar.ToNativeObject();
      oleVar.Clear();
      if (o is string) {
        return (string)o;
      } else {
        return String.Empty;
      }
    }

    /// <summary>
    /// Send a command to MSHTML and excepts to get an string value back
    /// If the command does not work or does not return the expected type an exception is thrown.
    /// </summary>
    /// <param name="command">The command</param>
    /// <returns>object value or String.Object</returns>
    internal object ExecResultObject(Interop.IDM command) {
      Interop.OLEVARIANT oleVar = new Interop.OLEVARIANT();
      //oleVar.vt = Convert.ToInt16(VarEnum.VT_BSTR);
      ExecResult(command, ref oleVar);
      oleVar.vt = Convert.ToInt16(VarEnum.VT_BSTR);
      object o = oleVar.ToNativeObject();
      oleVar.Clear();
      return o;
    }

    /// <summary>
    /// Send a command to MSHTML and excepts to get an integer value back
    /// If the command does not work or does not return the expected type an exception is thrown.
    /// </summary>
    /// <param name="command">The command</param>
    /// <returns>integer value or 0</returns>
    internal int ExecResultInt(Interop.IDM command) {
      Interop.OLEVARIANT oleVar = new Interop.OLEVARIANT();
      ExecResult(command, ref oleVar);
      oleVar.vt = Convert.ToInt16(VarEnum.VT_I4);
      object o = oleVar.ToNativeObject();
      oleVar.Clear();
      if (o is Int32) {
        return (int)o;
      } else {
        return 0;
      }
    }

    /// <summary>
    /// Queries MSHTML for the command info (enabled and checked) for the specified command
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    internal HtmlCommandInfo GetCommandInfo(Interop.IDM command) {
      //Create an OLECMD to store the command and receive the result
      Interop.OLECMD[] oleCommand = new Interop.OLECMD[1];
      oleCommand[0].cmdID = (int)command;
      Interop.OLECMDTEXT oleText = new Interop.OLECMDTEXT();
      oleText.cwActual = 0;
      // possible during shutdown
      if (MshtmlSite == null || CommandTarget == null) {
        return HtmlCommandInfo.Error;
      }
      int hr = CommandTarget.QueryStatus(ref Interop.Guid_MSHTML, 1, oleCommand, oleText);
      if (hr == 0) {
        //Then translate the response from the command status
        //We can just right shift by one to eliminate the supported flag from OLECMDF
        // 1 = supported, 2 = enabled, 4 = latched (is toggle type and currently on)            
        int info;
        info = oleCommand[0].cmdf;
        return (HtmlCommandInfo)(info >> 1);
        //return (HtmlCommandInfo)(info>>1) & (HtmlCommandInfo.Enabled | HtmlCommandInfo.Checked | HtmlCommandInfo.Latched);
      } else {
        return HtmlCommandInfo.Error;
      }
    }

    /// <summary>
    /// Indicates if the specified command is checked
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    internal bool IsCommandChecked(Interop.IDM command) {
      return ((GetCommandInfo(command) & HtmlCommandInfo.Checked) != 0);
    }

    /// <summary>
    /// Indicates if the specified command is enabled
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    internal bool IsCommandEnabled(Interop.IDM command) {
      return ((GetCommandInfo(command) & HtmlCommandInfo.Enabled) != 0);
    }

    /// <summary>
    /// Indicates if the specified command is enabled
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    internal bool IsCommandEnabledAndChecked(Interop.IDM command) {
      HtmlCommandInfo info = GetCommandInfo(command);
      return (((info & HtmlCommandInfo.Enabled) != 0) && ((info & HtmlCommandInfo.Checked) != 0));
    }

    /// <summary>
    /// Gets the current status of any (theoretical) available command.
    /// </summary>
    /// <remarks>
    /// This is a very fast method to check the current state of the control and
    /// the currently available commands, used to update toolbars and menus. 
    /// </remarks>
    /// <param name="command">Any command from the <see cref="GuruComponents.Netrix.HtmlCommand">HtmlCommand</see> enumeration.</param>
    /// <returns>A value that indicates the command is available or not or currently toggled on.</returns>
    public HtmlCommandInfo CommandStatus(HtmlCommand command) {
      try {
        return GetCommandInfo((Interop.IDM)((int)command));
      } catch {
        return HtmlCommandInfo.None;
      }
    }

    /// <summary>
    /// Indicates if the Interop.HTMLDocument2 is created.
    /// </summary>
    /// 
    private bool _iscreated;
    protected bool IsCreated
    {
      get { return _iscreated; }
      set { _iscreated = value; }
    }

    /// <summary>
    /// Set the caret to an element.
    /// </summary>
    /// <param name="el">The element where the caret is set to.</param>
    /// <param name="adjacency">The position within the elements container.</param>
    internal void SetCaretToElement(Interop.IHTMLElement el, Interop.ELEMENT_ADJACENCY adjacency) {
      try {
        Interop.IMarkupServices ms;
        Interop.IDisplayServices ds;
        ms = (Interop.IMarkupServices)this.GetActiveDocument(false);
        Interop.IMarkupPointer mp;
        ms.CreateMarkupPointer(out mp);
        mp.MoveAdjacentToElement(el, adjacency);
        ds = (Interop.IDisplayServices)this.GetActiveDocument(false);
        Interop.IDisplayPointer dp;
        ds.CreateDisplayPointer(out dp);
        // Move display pointer to markup
        dp.MoveToMarkupPointer(mp, null);
        Interop.IHTMLCaret cr;
        ds.GetCaret(out cr);
        // set the caret at the beginning of the new selection
        cr.MoveCaretToPointer(dp, true, Interop.CARET_DIRECTION.CARET_DIRECTION_SAME);
      } catch {
        // ignore command on fail
      }
    }

    /// <summary>
    /// Checks ready state and throw an exception but in design mode.
    /// </summary>
    /// <returns>False if not ready</returns>
    private bool ThrowDocumentNotReadyException() {
      if (!IsReady) {
        if (!DesignMode) {
          throw new DocumentNotReadyException();
        }
        return false;
      } else {
        return true;
      }
    }

    /// <summary>
    /// Inserts an given element at the current caret position.
    /// </summary>
    /// <remarks>
    /// The caller must assure that only a single element is being inserted, not a structure.
    /// So, an insertion like <table></table> is allowed, whereas <table><tbody></tbody></table> is not.
    /// </remarks>
    /// <param name="tagId"></param>
    /// <param name="name"></param>
    internal Interop.IHTMLElement InsertElementAtCaret(Interop.ELEMENT_TAG_ID tagId, string name) {
      if (!ThrowDocumentNotReadyException())
        return null;
      Interop.IHTMLElement el = null;
      try {
        el = this.GetActiveDocument(false).CreateElement(Helper.GetElementName(tagId));
        el.SetAttribute("id", name, 0);
        Document.InsertHtml(el.GetOuterHTML());
        el = GetElementById(name).GetBaseElement();
        el.RemoveAttribute("id", 0);
      } catch {
      }
      return el;
    }

    /// <summary>
    /// Inserts an given element at the current caret position.
    /// </summary>
    /// <remarks>
    /// The caller must assure that only a single element is being inserted, not a structure.
    /// So, an insertion like <table></table> is allowed, whereas <table><tbody></tbody></table> is not.
    /// </remarks>
    /// <param name="el"></param>
    /// <param name="name"></param>
    internal Interop.IHTMLElement InsertElementAtCaret(Interop.IHTMLElement el, string name) {
      if (!ThrowDocumentNotReadyException())
        return null;
      if (el == null)
        return null;
      try {
        Interop.IHTMLDocument2 doc = this.GetActiveDocument(false);
        Interop.IMarkupServices ms;
        Interop.IDisplayServices ds;
        ds = (Interop.IDisplayServices)doc;
        Interop.IDisplayPointer dp;
        ds.CreateDisplayPointer(out dp);
        Interop.IHTMLCaret cr;
        ds.GetCaret(out cr);
        cr.MoveDisplayPointerToCaret(dp);
        ms = (Interop.IMarkupServices)doc;
        Interop.IMarkupPointer sp, ep;
        ms.CreateMarkupPointer(out sp);   //Create a start markup pointer
        ms.CreateMarkupPointer(out ep);   //Create a start markup pointer
        dp.PositionMarkupPointer(sp);
        dp.PositionMarkupPointer(ep);
        ms.InsertElement(el, sp, ep);
        return el;
      } catch {
      }
      return null;
    }

    /// <summary>
    /// Insert Text at Caret position.
    /// </summary>
    /// <remarks>
    /// This method will always insert any characters als Text, tag definitions appear
    /// as converted text, e.g. &lt;span&gt; is inserted as &amp;lt;span&amp;gt;.
    /// </remarks>
    /// <param name="text">Text that has to be inserted.</param>
    public void InsertTextAtCaret(string text) {
      if (!ThrowDocumentNotReadyException())
        return;
      if (text == null)
        return;
      Interop.IMarkupServices ms;
      Interop.IDisplayServices ds;
      ds = (Interop.IDisplayServices)this.GetActiveDocument(false);
      Interop.IHTMLCaret cr;
      ds.GetCaret(out cr);
      ms = (Interop.IMarkupServices)this.GetActiveDocument(false);
      Interop.IMarkupPointer sp;
      ms.CreateMarkupPointer(out sp);   //Create a start markup pointer
      cr.MoveMarkupPointerToCaret(sp);  //Move the start markup pointer to the 
      ms.InsertText(text, text.Length, sp);
    }

    /// <summary>
    /// Inserts a generic created element at the current caret position.
    /// </summary>
    /// <remarks>
    /// This method internally calls <see cref="CreateElement"/> to add the element to the documents DOM.
    /// After that, the element is inserted. The method may fail if the element cannot be inserted at caret position.
    /// The control will not accept invalid HTML. In case of error an internal exception is thrown and catched. The
    /// method will return <c>null</c> (<c>Nothing</c> in VB.NET) if no success.
    /// </remarks>
    /// <param name="tagName">The name of tag to be inserted. Use "span" to insert &lt;span&gt;, for instance.</param>
    /// <returns>Returns <c>True</c> on success, <c>False</c> otherwise.</returns>
    public IElement CreateElementAtCaret(string tagName) {
      try {
        Interop.IHTMLElement el = null;
        IElement element = null;
        Interop.IMarkupServices ms;
        Interop.IDisplayServices ds;
        ds = (Interop.IDisplayServices)this.GetActiveDocument(false);
        Interop.IHTMLCaret cr;
        ds.GetCaret(out cr);
        ms = (Interop.IMarkupServices)this.GetActiveDocument(false);
        Interop.IMarkupPointer sp;
        ms.CreateMarkupPointer(out sp);   //Create a start markup pointer
        cr.MoveMarkupPointerToCaret(sp);  //Move the start markup pointer to the 
                                          //ms.CreateElement(Helper.GetTagId(tagName), null, out el);
        el = this.GetActiveDocument(false).CreateElement(tagName);
        ms.InsertElement(el, sp, null);
        element = GenericElementFactory.CreateElement(el) as IElement;
        return element;
      } catch {
        // failed for any reason
        return null;
      }
    }


    /// <summary>
    /// Insert the element at the current caret position.
    /// </summary>
    /// <remarks>
    /// This method inserts an element based on a "template" element. It returns the real element instead.
    /// </remarks>
    /// <param name="element">The element which has to be inserted, will be replaced by a clone of the element which points to the inserted one.</param>
    /// <returns>Returns <c>True</c> on success or <c>False</c> if the insertion fails.</returns>
    public bool InsertElementAtCaret(IElement element) {
      try {
        string name = "";
        if (element.GetAttribute("id") == null) {
          name = Helper.BuildUniqueName(element, this);
        } else {
          name = element.GetAttribute("id").ToString();
        }
        this.InsertElementAtCaret(element.GetBaseElement(), name);
        if (name != null) {
          element.SetAttribute("id", name);
        }
        return true;
      } catch {
        return false;
      }
    }

    /// <summary>
    /// Get the element near the last caret position. Does not recognize elements which have handles.
    /// </summary>
    /// <returns>The element</returns>
    [Browsable(false)]
    internal Interop.IHTMLElement CurrentScopeElement
    {
      get
      {
        if (!IsReady)
          return null;
        if (!DesignModeEnabled) {
          return GetBodyElement().GetBaseElement();
        }
        if (currentElement != null) {
          return currentElement;
        } else {
          Interop.IDisplayServices ds;
          ds = (Interop.IDisplayServices)this.GetActiveDocument(false);
          Interop.IHTMLCaret cr;
          ds.GetCaret(out cr);
          Interop.IDisplayPointer dp = null;
          ds.CreateDisplayPointer(out dp);
          cr.MoveDisplayPointerToCaret(dp);
          Interop.IHTMLElement el;
          dp.GetFlowElement(out el);
          return el;
        }
      }
    }

    /// <summary>
    /// Creates an instance of the element for the specified tag.
    /// </summary>
    /// <remarks>
    /// This method does not add the element to the DOM, it makes it available for adding later.
    /// <para>
    /// You can create all elements programmatically, except for &lt;frame&gt; and &lt;iframe&gt;. 
    /// In addition, the properties of these created elements are read/write and can be accessed 
    /// programmatically. Before you use new objects, you must explicitly add them to their respective 
    /// collections or to the document's DOM. To insert new elements into the current document, 
    /// use the <see cref="IElementDom.InsertBefore">InsertBefore</see> or 
    /// <see cref="IElementDom.AppendChild">AppendChild</see> methods. 
    /// </para>
    /// <para>You must perform a second step when using <c>CreateElement</c> to create the &lt;input&gt; element. 
    /// The method generates an <see cref="GuruComponents.Netrix.WebEditing.Elements.InputTextElement">InputTextElement</see>, because that is the default input type property. 
    /// To insert any other kind of input element, first invoke CreateElement for input, then set the 
    /// type property to the appropriate value in the next line of code. 
    /// <code>
    /// inputElement.SetAttribute("type", "radio");
    /// </code>
    /// Attributes can be included with the <c>tagName</c> parameter as long as the entire string is valid HTML. 
    /// You should do this if you wish to include the NAME attribute at run time on objects created with the 
    /// createElement method. Attributes should be included with the sTag when form elements are created that 
    /// are to be reset using the reset method or a BUTTON with a TYPE attribute value of reset.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example creates an <see cref="GuruComponents.Netrix.WebEditing.Elements.InputRadioElement">InputRadioElement</see> directly:
    /// <code>
    /// InputRadioElement radio = (InputRadioElement) editor.CreateElement(String.Format("&lt;input type={0}&gt;", "radio"));
    /// </code>
    /// After creating the element you're supposed to add it to the document:
    /// <code>
    /// masterElement.ElementDom.AppendChild(radio);
    /// </code>
    /// This example assumes that you have an element object <c>masterElement</c> which is a container and can 
    /// hold any sort of form elements.
    /// </example>
    /// <seealso cref="GuruComponents.Netrix.HtmlDocument">HtmlDocument</seealso>
    /// <seealso cref="GuruComponents.Netrix.HtmlEditor.Document">Document</seealso>
    /// <param name="tagName"></param>
    /// <returns></returns>
    public IElement CreateElement(string tagName) {
      return CreateElementAsynch(tagName);
    }

    internal IElement CreateElementAsynch(string tagName) {
      IElement element;
      // look if we deal with namespaces
      if (tagName.IndexOf(":") != -1 && RegisteredPlugIns != null) {
        string ns = tagName.Split(':')[0];
        foreach (IPlugIn p in RegisteredPlugIns) {
          if ((p.Features & Feature.CreateElements) == Feature.CreateElements) {
            ArrayList nss = new ArrayList(p.GetSupportedNamespaces(this).Keys);
            if (nss.Contains(ns)) {
              element = p.CreateElement(tagName, this) as IElement;
              return element;
            }
          }
        }
      }
      // In case the plugin wasn't able to create try directly
      Interop.IHTMLElement el;
      el = this.GetActiveDocument(false).CreateElement(tagName);
      element = GenericElementFactory.CreateElement(el) as IElement;
      return element;
    }

    /// <summary>
    /// Called when a new element has been created successfully.
    /// </summary>
    /// <remarks></remarks>
    /// <param name="element">The element which has been created.</param>
    protected internal void OnElementCreated(Control element) {
      if (ElementCreated != null) {
        ElementCreated(element, EventArgs.Empty);
      }
    }

    private IElementFactory _genericElementFactory = null;

    /// <summary>
    /// This property returns an instance of IElementFactory.
    /// </summary>
    /// <remarks>
    /// Its purpose is to provide access to the element factory for plug-ins and extension modules.
    /// Extension authors can use the factory to add elements to the elements cache. It's not 
    /// recommended to use this property directly from user code, in fact, it supports the NetRix
    /// infrastructure only.
    /// </remarks>
    [Browsable(false)]
    public IElementFactory GenericElementFactory
    {
      get
      {
        if (_genericElementFactory == null) {
          _genericElementFactory = new ElementFactory(this);
        }
        return _genericElementFactory;
      }
    }

    /// <summary>
    /// Get the current element which is in the scope for next editing operation. 
    /// </summary>
    /// <remarks>
    /// Caller can cast the object to the right IElement derived type.
    /// <para>
    /// If it's more important to get the current element, which has fired the last event or
    /// is set by <see cref="HtmlElementChanged"/> event, it's better to use the <see cref="GetCurrentElement"/>
    /// method. Under normal circumstances both method return the same element, but during key or mouse
    /// operations in nested element hierarchies there might be a difference between currency and scope.
    /// </para>
    /// </remarks>
    /// <returns>The element object</returns>
    public IElement GetCurrentScopeElement() {
      IElement e = (IElement)GenericElementFactory.CreateElement(CurrentScopeElement);
      return e;
    }

    private Interop.IHTMLElement currentElement = null;

    /// <summary>
    /// Gets the current element.
    /// </summary>
    /// <remarks>
    /// This method returns the current element, as it is set by <see cref="HtmlElementChanged"/> event.
    /// If the element wasn't recognized for some reason the method returns the Body Element
    /// </remarks>
    /// <seealso cref="GetCurrentScopeElement"/>
    /// <returns>The current element object.</returns>
    public IElement GetCurrentElement() {
      if (currentElement != null) {
        // check whether it's still part of the document
        Interop.IHTMLElement element = GetBodyThreadSafe(false) as Interop.IHTMLElement;
        if (element.Contains(currentElement)) {
          // return only if valid
          return GenericElementFactory.CreateElement(currentElement) as IElement;
        } else {
          return GetBodyElement();
        }
      } else {
        return GetBodyElement();
      }
    }

    public IComponent GetCurrentComponent() {
      if (currentElement != null) {
        // check whether it's still part of the document
        Interop.IHTMLElement element = GetBodyThreadSafe(false) as Interop.IHTMLElement;
        if (element.Contains(currentElement)) {
          // return only if valid
          return GenericElementFactory.CreateElement(currentElement) as IComponent;
        } else {
          return null;
        }
      } else {
        return null;
      }
    }

    #endregion

    #region Internally used Event Invoke Methods

    private void _snapElement_AfterSnapRect(object sender, AfterSnapRectEventArgs e) {
      if (AfterSnapRect != null) {
        AfterSnapRect(this, e);
      }
    }

    private void _snapElement_BeforeSnapRect(object sender, BeforeSnapRectEventArgs e) {
      if (BeforeSnapRect != null) {
        BeforeSnapRect(this, e);
      }
    }

    internal protected void OnPropertyFilterRequest(PropertyFilterEventArgs e) {
      if (PropertyFilterRequest != null) {
        PropertyFilterRequest(e.Element, e);
      }
    }

    internal protected void OnPropertyDisplayRequest(PropertyDisplayEventArgs e) {
      if (PropertyDisplayRequest != null) {
        PropertyDisplayRequest(e.Element, e);
      }
    }

    internal protected void OnEventFilterRequest(EventFilterEventArgs e) {
      if (EventFilterRequest != null) {
        EventFilterRequest(e.Element, e);
      }
    }

    internal protected void OnEventDisplayRequest(EventDisplayEventArgs e) {
      if (EventDisplayRequest != null) {
        EventDisplayRequest(e.Element, e);
      }
    }

    /// <summary>
    /// Fires the AfterSave event.
    /// </summary>
    protected void OnSaved() {
      // In non-full document mode, we don't actually save the IPersistInitStream, so
      // clear the dirty bit here
      if (!IsFullDocumentMode) {
        ClearDirtyState();
      }
      if (Saved != null) {
        Saved(this, new SaveEventArgs(this.Encoding, this.Url));
      }
    }

    /// <summary>
    /// Overridden to remove the grid behavior before loading.
    /// </summary>
    protected void OnLoading() {
      if (Loading != null) {
        Loading(this, new LoadEventArgs(this.Encoding, this.Url));
      }
    }

    #region DesignTime Behavior

    //private Pen p = new Pen(Color.Blue, 1F);
    //private Brush b = new SolidBrush(Color.White);
    //private Brush t = new SolidBrush(Color.Blue);
    //private Font f = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Pixel);

    ///// <summary>
    ///// Draws the design time information.
    ///// </summary>
    ///// <param name="e"></param>
    //protected override void OnPaint(PaintEventArgs e)
    //{
    //    if (DesignMode)
    //    {
    //        base.OnPaint(e);
    //        int wh = Width / 2;
    //        int hh = Height / 2;
    //        // center
    //        e.Graphics.DrawLine(p, 0, hh, Width, hh);
    //        e.Graphics.DrawLine(p, wh, 0, wh, Height);
    //        string s = String.Format(" {0} x {1}", Width, Height);
    //        SizeF sf = e.Graphics.MeasureString(s, f);
    //        e.Graphics.FillRectangle(b, wh - sf.Width / 2, hh - sf.Height / 2, sf.Width, sf.Height);
    //        e.Graphics.DrawRectangle(p, wh - sf.Width / 2, hh - sf.Height / 2, sf.Width, sf.Height);
    //        e.Graphics.DrawString(s, f, t, wh - sf.Width / 2 - 1, hh - sf.Height / 2 + 1);
    //        // location
    //        if (Width > 80 && Height > 80)
    //        {
    //            wh = 30;
    //            hh = 40;
    //            e.Graphics.DrawLine(p, 0, 0, wh, wh);
    //            e.Graphics.DrawLine(p, wh, wh, hh, wh);
    //            s = String.Format(" {0} x {1}", Left, Top);
    //            sf = e.Graphics.MeasureString(s, f);
    //            e.Graphics.FillRectangle(b, hh - sf.Width / 2 + hh / 2, wh - sf.Height / 2, sf.Width, sf.Height);
    //            e.Graphics.DrawRectangle(p, hh - sf.Width / 2 + hh / 2, wh - sf.Height / 2, sf.Width, sf.Height);
    //            e.Graphics.DrawString(s, f, t, hh - sf.Width / 2 - 1 + hh / 2, wh - sf.Height / 2 + 1);
    //        }
    //    }
    //}

    #endregion

    /// <summary>
    /// This method fires the AfterLoad event.
    /// </summary>
    protected void OnLoaded() {
      if (Loaded != null) {
        Loaded(this, new LoadEventArgs(this.Encoding, this.Url));
      }
    }

    /// <summary>
    /// This method fires the BeforeSave event.
    /// </summary>
    protected void OnSaving() {
      if (Saving != null) {
        SaveEventArgs sea = new SaveEventArgs(this.Encoding, this.Url);
        Saving(this, sea);
        Encoding = sea.Encoding;
      }
    }

    #endregion

    #region +++++ Block: Common Control Properties

    #region Highlighting, SpellChecker and Word selection


    public void InvokePostEditorEvent(PostEditorEventArgs e) {
      if (PostEditorEvent != null) {
        PostEditorEvent(this, e);
      }
    }


    public void MoveCaretTo(MoveCaret unit, bool sameColumn) {
      MoveCaretTo((Interop.DISPLAY_MOVEUNIT)(int)unit, sameColumn);
    }

    internal void MoveCaretTo(Interop.DISPLAY_MOVEUNIT unit) {
      MoveCaretTo(unit, true);
    }

    internal void MoveCaretTo(Interop.DISPLAY_MOVEUNIT unit, bool sameColumn) {
      Application.DoEvents();
      try {
        Interop.IHTMLCaret cr;
        Interop.IDisplayServices ds = (Interop.IDisplayServices)this.GetActiveDocument(false);
        Interop.IDisplayPointer dp;
        ds.CreateDisplayPointer(out dp);
        ds.GetCaret(out cr);
        cr.MoveDisplayPointerToCaret(dp);
        dp.MoveUnit(unit, sameColumn ? -1 : 1);
        cr.MoveCaretToPointer(dp, true, Interop.CARET_DIRECTION.CARET_DIRECTION_SAME);
      } catch {
      }
    }

    #endregion

    #region Frames

    /// <summary>
    /// Sets overwrite or insert mode.
    /// </summary>
    /// <remarks>
    /// This methods sets the behavior of the insertion point. The user can independently use
    /// the INS key to switch the behavior, but the host application can synchronize the
    /// UI with the behavior by forcing the state using this method.
    /// </remarks>
    /// <param name="Mode">The mode which sets the override behavior. <c>True</c> turns it on.</param>
    public void OverWrite(bool Mode) {
      this._activeDocument.ExecCommand("OverWrite", Mode, null);
    }

    /// <summary>
    /// Provides information about the current state of the internet connection.
    /// </summary>
    /// <remarks>
    /// Does not check if the connection is established but if it is possible to do so.
    /// </remarks>
    /// <example>
    /// The following code shows how to use the method and how to convert the enum values into readable strings:
    /// <code>
    ///private string GetConnectionStateString()
    ///{
    ///    GuruComponents.Netrix.InternetConnectState t = this.htmlEditor2.GetConnectionState();
    ///    string s = String.Concat("[", (int)t, "] ");
    ///    if ((t &amp; GuruComponents.Netrix.InternetConnectState.ConnectionLAN) == 0 == false)
    ///    {
    ///        s += "LAN";
    ///    }
    ///    if ((t &amp; GuruComponents.Netrix.InternetConnectState.ConnectionModem) == 0 == false)
    ///    {
    ///        s += "Modem";
    ///        if ((t &amp; GuruComponents.Netrix.InternetConnectState.Configured) == 0 == false)
    ///        {
    ///            s += "/Configured";
    ///        } 
    ///        else 
    ///        {
    ///            s += "/Not Configured";
    ///        }
    ///    }
    ///    if ((t &amp; GuruComponents.Netrix.InternetConnectState.ConnectionProxy) == 0 == false)
    ///    {
    ///        s += "Proxy";
    ///    }
    ///    if ((t &amp; GuruComponents.Netrix.InternetConnectState.Offline) == 0 == false)
    ///    {
    ///        s += "/Offline";
    ///    } 
    ///    else 
    ///    {
    ///        s += "/Online";
    ///    }
    ///    return s;
    ///}</code>
    /// </example>
    /// <returns>One or more values of the bitflag 
    /// enumeration <see cref="GuruComponents.Netrix.InternetConnectState">GuruComponents.Netrix.InternetConnectState</see>.</returns>
    public InternetConnectState GetConnectionState() {
      int temp;
      Win32.InternetGetConnectedState(out temp, 0);
      return (InternetConnectState)temp;
    }

    #region Printing

    /// <summary>
    /// Shows the internal Page Setup Dialog.
    /// </summary>
    public void PrintPageSetup() {
      Exec(Interop.IDM.PAGESETUP);
    }

    /// <summary>
    /// Shows the print preview and let the user optionally print.
    /// </summary>
    /// <remarks>
    /// To use the print feature the control must be able to reload the current content internally. 
    /// Therefore the host application must provide a path or URL to the LoadXXX methods before 
    /// a printing process can start. Without a valid path the print command is ignored and the
    /// preview will display a blank page.
    /// </remarks>
    public void PrintWithPreview() {
      Exec(Interop.IDM.PRINTPREVIEW);
    }

    /// <summary>
    /// Prints a document using a customized preview from the given 
    /// print template.
    /// </summary>
    /// <remarks>
    /// To use the print feature the control must be able to reload the current content internally. 
    /// Therefore the host application must provide a path or URL to the LoadXXX methods before 
    /// a printing process can start. Without a valid path the print command is ignored and the
    /// preview will display a blank page.
    /// </remarks>
    /// <param name="PathToPrintTemplate">The path to the HTML code creating the print template.</param>
    public void PrintWithPreview(string PathToPrintTemplate) {
      Exec(Interop.IDM.PRINTPREVIEW, PathToPrintTemplate);
    }

    /// <summary>
    /// Print the current document to the current printer without any user interaction.
    /// </summary>
    /// <remarks>
    /// To use the print feature the control must be able to reload the current content internally. 
    /// Therefore the host application must provide a path or URL to the LoadXXX methods before 
    /// a printing process can start. Without a valid path the print command is ignored and the
    /// method will print a blank page.
    /// </remarks>
    public void PrintImmediately() {
      Exec(Interop.IDM.PRINT);
    }

    #endregion

    #endregion

    /// <summary>
    /// This method sends an IDM command directly to MSHTML.
    /// </summary>
    /// <remarks>
    /// The call is protected by a try/catch block. The 
    /// method does not throw an exception on error and does always return nothing. This method is just for
    /// experimental environments and should not used in production environments.
    /// <para>
    /// This is method is subject to be changed or removed in later versions without further notice.
    /// </para>
    /// <list type="table">
    /// <listheader>
    ///     <term>IDM constant</term><term>Numeric Value</term>
    /// </listheader>
    /// <item><description>Interop.IDM.UNKNOWN                 </description><description>0</description></item>
    /// <item><description>Interop.IDM.ALIGNBOTTOM             </description><description>1</description></item>
    /// <item><description>Interop.IDM.ALIGNHORIZONTALCENTERS  </description><description>2</description></item>
    /// <item><description>Interop.IDM.ALIGNLEFT               </description><description>3</description></item>
    /// <item><description>Interop.IDM.ALIGNRIGHT              </description><description>4</description></item>
    /// <item><description>Interop.IDM.ALIGNTOGRID             </description><description>5</description></item>
    /// <item><description>Interop.IDM.ALIGNTOP                </description><description>6</description></item>
    /// <item><description>Interop.IDM.ALIGNVERTICALCENTERS    </description><description>7</description></item>
    /// <item><description>Interop.IDM.ARRANGEBOTTOM           </description><description>8</description></item>
    /// <item><description>Interop.IDM.ARRANGERIGHT            </description><description>9</description></item>
    /// <item><description>Interop.IDM.BRINGFORWARD            </description><description>10</description></item>
    /// <item><description>Interop.IDM.BRINGTOFRONT            </description><description>11</description></item>
    /// <item><description>Interop.IDM.CENTERHORIZONTALLY      </description><description>12</description></item>
    /// <item><description>Interop.IDM.CENTERVERTICALLY        </description><description>13</description></item>
    /// <item><description>Interop.IDM.CODE                    </description><description>14</description></item>
    /// <item><description>Interop.IDM.DELETE                  </description><description>17</description></item>
    /// <item><description>Interop.IDM.FONTNAME                </description><description>18</description></item>
    /// <item><description>Interop.IDM.FONTSIZE                </description><description>19</description></item>
    /// <item><description>Interop.IDM.GROUP                   </description><description>20</description></item>
    /// <item><description>Interop.IDM.HORIZSPACECONCATENATE   </description><description>21</description></item>
    /// <item><description>Interop.IDM.HORIZSPACEDECREASE      </description><description>22</description></item>
    /// <item><description>Interop.IDM.HORIZSPACEINCREASE      </description><description>23</description></item>
    /// <item><description>Interop.IDM.HORIZSPACEMAKEEQUAL     </description><description>24</description></item>
    /// <item><description>Interop.IDM.INSERTOBJECT            </description><description>25</description></item>
    /// <item><description>Interop.IDM.MULTILEVELREDO          </description><description>30</description></item>
    /// <item><description>Interop.IDM.SENDBACKWARD            </description><description>32</description></item>
    /// <item><description>Interop.IDM.SENDTOBACK              </description><description>33</description></item>
    /// <item><description>Interop.IDM.SHOWTABLE               </description><description>34</description></item>
    /// <item><description>Interop.IDM.SIZETOCONTROL           </description><description>35</description></item>
    /// <item><description>Interop.IDM.SIZETOCONTROLHEIGHT     </description><description>36</description></item>
    /// <item><description>Interop.IDM.SIZETOCONTROLWIDTH      </description><description>37</description></item>
    /// <item><description>Interop.IDM.SIZETOFIT               </description><description>38</description></item>
    /// <item><description>Interop.IDM.SIZETOGRID              </description><description>39</description></item>
    /// <item><description>Interop.IDM.SNAPTOGRID              </description><description>40</description></item>
    /// <item><description>Interop.IDM.TABORDER                </description><description>41</description></item>
    /// <item><description>Interop.IDM.TOOLBOX                 </description><description>42</description></item>
    /// <item><description>Interop.IDM.MULTILEVELUNDO          </description><description>44</description></item>
    /// <item><description>Interop.IDM.UNGROUP                 </description><description>45</description></item>
    /// <item><description>Interop.IDM.VERTSPACECONCATENATE    </description><description>46</description></item>
    /// <item><description>Interop.IDM.VERTSPACEDECREASE       </description><description>47</description></item>
    /// <item><description>Interop.IDM.VERTSPACEINCREASE       </description><description>48</description></item>
    /// <item><description>Interop.IDM.VERTSPACEMAKEEQUAL      </description><description>49</description></item>
    /// <item><description>Interop.IDM.JUSTIFYFULL             </description><description>50</description></item>
    /// <item><description>Interop.IDM.BACKCOLOR               </description><description>51</description></item>
    /// <item><description>Interop.IDM.BOLD                    </description><description>52</description></item>
    /// <item><description>Interop.IDM.BORDERCOLOR             </description><description>53</description></item>
    /// <item><description>Interop.IDM.FLAT                    </description><description>54</description></item>
    /// <item><description>Interop.IDM.FORECOLOR               </description><description>55</description></item>
    /// <item><description>Interop.IDM.ITALIC                  </description><description>56</description></item>
    /// <item><description>Interop.IDM.JUSTIFYCENTER           </description><description>57</description></item>
    /// <item><description>Interop.IDM.JUSTIFYGENERAL          </description><description>58</description></item>
    /// <item><description>Interop.IDM.JUSTIFYLEFT             </description><description>59</description></item>
    /// <item><description>Interop.IDM.JUSTIFYRIGHT            </description><description>60</description></item>
    /// <item><description>Interop.IDM.RAISED                  </description><description>61</description></item>
    /// <item><description>Interop.IDM.SUNKEN                  </description><description>62</description></item>
    /// <item><description>Interop.IDM.UNDERLINE               </description><description>63</description></item>
    /// <item><description>Interop.IDM.CHISELED                </description><description>64</description></item>
    /// <item><description>Interop.IDM.ETCHED                  </description><description>65</description></item>
    /// <item><description>Interop.IDM.SHADOWED                </description><description>66</description></item>
    /// <item><description>Interop.IDM.FIND                    </description><description>67</description></item>
    /// <item><description>Interop.IDM.SHOWGRID                </description><description>69</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST0         </description><description>72</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST1         </description><description>73</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST2         </description><description>74</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST3         </description><description>75</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST4         </description><description>76</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST5         </description><description>77</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST6         </description><description>78</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST7         </description><description>79</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST8         </description><description>80</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST9         </description><description>81</description></item>
    /// <item><description>Interop.IDM.CONVERTOBJECT       </description><description>    82</description></item>
    /// <item><description>Interop.IDM.CUSTOMCONTROL       </description><description>    83</description></item>
    /// <item><description>Interop.IDM.CUSTOMIZEITEM       </description><description>    84</description></item>
    /// <item><description>Interop.IDM.RENAME              </description><description>    85</description></item>
    /// <item><description>Interop.IDM.IMPORT              </description><description>    86</description></item>
    /// <item><description>Interop.IDM.NEWPAGE             </description><description>    87</description></item>
    /// <item><description>Interop.IDM.MOVE                </description><description>    88</description></item>
    /// <item><description>Interop.IDM.CANCEL              </description><description>    89</description></item>
    /// <item><description>Interop.IDM.FONT                </description><description>    90</description></item>
    /// <item><description>Interop.IDM.STRIKETHROUGH       </description><description>    91</description></item>
    /// <item><description>Interop.IDM.DELETEWORD          </description><description>    92</description></item>
    /// <item><description>Interop.IDM.EXECPRINT           </description><description>    93</description></item>
    /// <item><description>Interop.IDM.JUSTIFYNONE         </description><description>    94</description></item>
    /// <item><description>Interop.IDM.TRISTATEBOLD        </description><description>    95</description></item>
    /// <item><description>Interop.IDM.TRISTATEITALIC      </description><description>    96</description></item>
    /// <item><description>Interop.IDM.TRISTATEUNDERLINE   </description><description>    97</description></item>
    /// <item><description>Interop.IDM.FOLLOW_ANCHOR        </description><description>   2008</description></item>
    /// <item><description>Interop.IDM.INSINPUTIMAGE         </description><description>  2114</description></item>
    /// <item><description>Interop.IDM.INSINPUTBUTTON        </description><description>  2115</description></item>
    /// <item><description>Interop.IDM.INSINPUTRESET         </description><description>  2116</description></item>
    /// <item><description>Interop.IDM.INSINPUTSUBMIT        </description><description>  2117</description></item>
    /// <item><description>Interop.IDM.INSINPUTUPLOAD        </description><description>  2118</description></item>
    /// <item><description>Interop.IDM.INSFIELDSET           </description><description>  2119</description></item>
    /// <item><description>Interop.IDM.PASTEINSERT          </description><description>   2120</description></item>
    /// <item><description>Interop.IDM.REPLACE              </description><description>   2121</description></item>
    /// <item><description>Interop.IDM.EDITSOURCE           </description><description>   2122</description></item>
    /// <item><description>Interop.IDM.BOOKMARK             </description><description>   2123</description></item>
    /// <item><description>Interop.IDM.HYPERLINK            </description><description>   2124</description></item>
    /// <item><description>Interop.IDM.UNLINK               </description><description>   2125</description></item>
    /// <item><description>Interop.IDM.BROWSEMODE           </description><description>   2126</description></item>
    /// <item><description>Interop.IDM.EDITMODE             </description><description>   2127</description></item>
    /// <item><description>Interop.IDM.UNBOOKMARK           </description><description>   2128</description></item>
    /// <item><description>Interop.IDM.TOOLBARS             </description><description>   2130</description></item>
    /// <item><description>Interop.IDM.STATUSBAR            </description><description>   2131</description></item>
    /// <item><description>Interop.IDM.FORMATMARK           </description><description>   2132</description></item>
    /// <item><description>Interop.IDM.TEXTONLY             </description><description>   2133</description></item>
    /// <item><description>Interop.IDM.OPTIONS              </description><description>   2135</description></item>
    /// <item><description>Interop.IDM.FOLLOWLINKC          </description><description>   2136</description></item>
    /// <item><description>Interop.IDM.FOLLOWLINKN          </description><description>   2137</description></item>
    /// <item><description>Interop.IDM.VIEWSOURCE           </description><description>   2139</description></item>
    /// <item><description>Interop.IDM.ZOOMPOPUP            </description><description>   2140</description></item>
    /// <item><description>Interop.IDM.BASELINEFONT1       </description><description>    2141</description></item>
    /// <item><description>Interop.IDM.BASELINEFONT2       </description><description>    2142</description></item>
    /// <item><description>Interop.IDM.BASELINEFONT3       </description><description>    2143</description></item>
    /// <item><description>Interop.IDM.BASELINEFONT4       </description><description>    2144</description></item>
    /// <item><description>Interop.IDM.BASELINEFONT5       </description><description>    2145</description></item>
    /// <item><description>Interop.IDM.HORIZONTALLINE      </description><description>    2150</description></item>
    /// <item><description>Interop.IDM.LINEBREAKNORMAL     </description><description>    2151</description></item>
    /// <item><description>Interop.IDM.LINEBREAKLEFT       </description><description>    2152</description></item>
    /// <item><description>Interop.IDM.LINEBREAKRIGHT      </description><description>    2153</description></item>
    /// <item><description>Interop.IDM.LINEBREAKBOTH       </description><description>    2154</description></item>
    /// <item><description>Interop.IDM.NONBREAK            </description><description>    2155</description></item>
    /// <item><description>Interop.IDM.SPECIALCHAR         </description><description>    2156</description></item>
    /// <item><description>Interop.IDM.HTMLSOURCE          </description><description>    2157</description></item>
    /// <item><description>Interop.IDM.IFRAME              </description><description>    2158</description></item>
    /// <item><description>Interop.IDM.HTMLCONTAIN         </description><description>    2159</description></item>
    /// <item><description>Interop.IDM.TEXTBOX             </description><description>    2161</description></item>
    /// <item><description>Interop.IDM.TEXTAREA            </description><description>    2162</description></item>
    /// <item><description>Interop.IDM.CHECKBOX            </description><description>    2163</description></item>
    /// <item><description>Interop.IDM.RADIOBUTTON         </description><description>    2164</description></item>
    /// <item><description>Interop.IDM.DROPDOWNBOX         </description><description>    2165</description></item>
    /// <item><description>Interop.IDM.LISTBOX             </description><description>    2166</description></item>
    /// <item><description>Interop.IDM.BUTTON              </description><description>    2167</description></item>
    /// <item><description>Interop.IDM.IMAGE               </description><description>    2168</description></item>
    /// <item><description>Interop.IDM.OBJECT              </description><description>    2169</description></item>
    /// <item><description>Interop.IDM.1D                  </description><description>    2170</description></item>
    /// <item><description>Interop.IDM.IMAGEMAP            </description><description>    2171</description></item>
    /// <item><description>Interop.IDM.FILE                </description><description>    2172</description></item>
    /// <item><description>Interop.IDM.COMMENT             </description><description>    2173</description></item>
    /// <item><description>Interop.IDM.SCRIPT              </description><description>    2174</description></item>
    /// <item><description>Interop.IDM.JAVAAPPLET          </description><description>    2175</description></item>
    /// <item><description>Interop.IDM.PLUGIN              </description><description>    2176</description></item>
    /// <item><description>Interop.IDM.PAGEBREAK           </description><description>    2177</description></item>
    /// <item><description>Interop.IDM.HTMLAREA            </description><description>    2178</description></item>
    /// <item><description>Interop.IDM.PARAGRAPH           </description><description>    2180</description></item>
    /// <item><description>Interop.IDM.FORM                </description><description>    2181</description></item>
    /// <item><description>Interop.IDM.MARQUEE             </description><description>    2182</description></item>
    /// <item><description>Interop.IDM.LIST                </description><description>    2183</description></item>
    /// <item><description>Interop.IDM.ORDERLIST           </description><description>    2184</description></item>
    /// <item><description>Interop.IDM.UNORDERLIST         </description><description>    2185</description></item>
    /// <item><description>Interop.IDM.INDENT              </description><description>    2186</description></item>
    /// <item><description>Interop.IDM.OUTDENT             </description><description>    2187</description></item>
    /// <item><description>Interop.IDM.PREFORMATTED        </description><description>    2188</description></item>
    /// <item><description>Interop.IDM.ADDRESS             </description><description>    2189</description></item>
    /// <item><description>Interop.IDM.BLINK               </description><description>    2190</description></item>
    /// <item><description>Interop.IDM.DIV                 </description><description>    2191</description></item>
    /// <item><description>Interop.IDM.TABLEINSERT         </description><description>    2200</description></item>
    /// <item><description>Interop.IDM.RCINSERT            </description><description>    2201</description></item>
    /// <item><description>Interop.IDM.CELLINSERT          </description><description>    2202</description></item>
    /// <item><description>Interop.IDM.CAPTIONINSERT       </description><description>    2203</description></item>
    /// <item><description>Interop.IDM.CELLMERGE           </description><description>    2204</description></item>
    /// <item><description>Interop.IDM.CELLSPLIT           </description><description>    2205</description></item>
    /// <item><description>Interop.IDM.CELLSELECT          </description><description>    2206</description></item>
    /// <item><description>Interop.IDM.ROWSELECT           </description><description>    2207</description></item>
    /// <item><description>Interop.IDM.COLUMNSELECT        </description><description>    2208</description></item>
    /// <item><description>Interop.IDM.TABLESELECT         </description><description>    2209</description></item>
    /// <item><description>Interop.IDM.TABLEPROPERTIES     </description><description>    2210</description></item>
    /// <item><description>Interop.IDM.CELLPROPERTIES      </description><description>    2211</description></item>
    /// <item><description>Interop.IDM.ROWINSERT           </description><description>    2212</description></item>
    /// <item><description>Interop.IDM.COLUMNINSERT        </description><description>    2213</description></item>
    /// <item><description>Interop.IDM.HELP_CONTENT         </description><description>   2220</description></item>
    /// <item><description>Interop.IDM.HELP_ABOUT           </description><description>   2221</description></item>
    /// <item><description>Interop.IDM.HELP_README          </description><description>   2222</description></item>
    /// <item><description>Interop.IDM.REMOVEFORMAT          </description><description>  2230</description></item>
    /// <item><description>Interop.IDM.PAGEINFO             </description><description>   2231</description></item>
    /// <item><description>Interop.IDM.TELETYPE             </description><description>   2232</description></item>
    /// <item><description>Interop.IDM.GETBLOCKFMTS          </description><description>  2233</description></item>
    /// <item><description>Interop.IDM.BLOCKFMT              </description><description>  2234</description></item>
    /// <item><description>Interop.IDM.SHOWHIDE_CODE         </description><description>  2235</description></item>
    /// <item><description>Interop.IDM.TABLE                 </description><description>  2236</description></item>
    /// <item><description>Interop.IDM.COPYFORMAT             </description><description> 2237</description></item>
    /// <item><description>Interop.IDM.PASTEFORMAT         </description><description>    2238</description></item>
    /// <item><description>Interop.IDM.GOTO                 </description><description>   2239</description></item>
    /// <item><description>Interop.IDM.CHANGEFONT            </description><description>  2240</description></item>
    /// <item><description>Interop.IDM.CHANGEFONTSIZE        </description><description>  2241</description></item>
    /// <item><description>Interop.IDM.CHANGECASE            </description><description>  2246</description></item>
    /// <item><description>Interop.IDM.SHOWSPECIALCHAR       </description><description>  2249</description></item>
    /// <item><description>Interop.IDM.SUBSCRIPT             </description><description>  2247</description></item>
    /// <item><description>Interop.IDM.SUPERSCRIPT           </description><description>  2248</description></item>
    /// <item><description>Interop.IDM.CENTERALIGNPARA       </description><description>  2250</description></item>
    /// <item><description>Interop.IDM.LEFTALIGNPARA         </description><description>  2251</description></item>
    /// <item><description>Interop.IDM.RIGHTALIGNPARA        </description><description>  2252</description></item>
    /// <item><description>Interop.IDM.REMOVEPARAFORMAT      </description><description>  2253</description></item>
    /// <item><description>Interop.IDM.APPLYNORMAL           </description><description>  2254</description></item>
    /// <item><description>Interop.IDM.APPLYHEADING1         </description><description>  2255</description></item>
    /// <item><description>Interop.IDM.APPLYHEADING2         </description><description>  2256</description></item>
    /// <item><description>Interop.IDM.APPLYHEADING3         </description><description>  2257</description></item>
    /// <item><description>Interop.IDM.DOCPROPERTIES         </description><description>  2260</description></item>
    /// <item><description>Interop.IDM.ADDFAVORITES          </description><description>  2261</description></item>
    /// <item><description>Interop.IDM.COPYSHORTCUT          </description><description>  2262</description></item>
    /// <item><description>Interop.IDM.SAVEBACKGROUND        </description><description>  2263</description></item>
    /// <item><description>Interop.IDM.SETWALLPAPER          </description><description>  2264</description></item>
    /// <item><description>Interop.IDM.COPYBACKGROUND        </description><description>  2265</description></item>
    /// <item><description>Interop.IDM.CREATESHORTCUT        </description><description>  2266</description></item>
    /// <item><description>Interop.IDM.PAGE                  </description><description>  2267</description></item>
    /// <item><description>Interop.IDM.SAVETARGET            </description><description>  2268</description></item>
    /// <item><description>Interop.IDM.SHOWPICTURE           </description><description>  2269</description></item>
    /// <item><description>Interop.IDM.SAVEPICTURE           </description><description>  2270</description></item>
    /// <item><description>Interop.IDM.DYNSRCPLAY            </description><description>  2271</description></item>
    /// <item><description>Interop.IDM.DYNSRCSTOP            </description><description>  2272</description></item>
    /// <item><description>Interop.IDM.PRINTTARGET           </description><description>  2273</description></item>
    /// <item><description>Interop.IDM.IMGARTPLAY            </description><description>  2274</description></item>
    /// <item><description>Interop.IDM.IMGARTSTOP            </description><description>  2275</description></item>
    /// <item><description>Interop.IDM.IMGARTREWIND          </description><description>  2276</description></item>
    /// <item><description>Interop.IDM.PRINTQUERYJOBSPENDING </description><description>  2277</description></item>
    /// <item><description>Interop.IDM.SETDESKTOPITEM        </description><description>  2278</description></item>
    /// <item><description>Interop.IDM.CONTEXTMENU           </description><description>  2280</description></item>
    /// <item><description>Interop.IDM.GOBACKWARD            </description><description>  2282</description></item>
    /// <item><description>Interop.IDM.GOFORWARD             </description><description>  2283</description></item>
    /// <item><description>Interop.IDM.PRESTOP               </description><description>  2284</description></item>
    /// <item><description>Interop.IDM.MP_MYPICS             </description><description>  2287</description></item>
    /// <item><description>Interop.IDM.MP_EMAILPICTURE       </description><description>  2288</description></item>
    /// <item><description>Interop.IDM.MP_PRINTPICTURE       </description><description>  2289</description></item>
    /// <item><description>Interop.IDM.CREATELINK           </description><description>   2290</description></item>
    /// <item><description>Interop.IDM.COPYCONTENT          </description><description>   2291</description></item>
    /// <item><description>Interop.IDM.LANGUAGE             </description><description>   2292</description></item>
    /// <item><description>Interop.IDM.GETPRINTTEMPLATE    </description><description>    2295</description></item>
    /// <item><description>Interop.IDM.SETPRINTTEMPLATE    </description><description>    2296</description></item>
    /// <item><description>Interop.IDM.TEMPLATE_PAGESETUP  </description><description>    2298</description></item>
    /// <item><description>Interop.IDM.REFRESH              </description><description>   2300</description></item>
    /// <item><description>Interop.IDM.STOPDOWNLOAD         </description><description>   2301</description></item>
    /// <item><description>Interop.IDM.ENABLE_INTERACTION    </description><description>  2302</description></item>
    /// <item><description>Interop.IDM.LAUNCHDEBUGGER         </description><description> 2310</description></item>
    /// <item><description>Interop.IDM.BREAKATNEXT            </description><description> 2311</description></item>
    /// <item><description>Interop.IDM.INSINPUTHIDDEN        </description><description>  2312</description></item>
    /// <item><description>Interop.IDM.INSINPUTPASSWORD      </description><description>  2313</description></item>
    /// <item><description>Interop.IDM.OVERWRITE          </description><description>     2314</description></item>
    /// <item><description>Interop.IDM.PARSECOMPLETE       </description><description>    2315</description></item>
    /// <item><description>Interop.IDM.HTMLEDITMODE       </description><description>     2316</description></item>
    /// <item><description>Interop.IDM.REGISTRYREFRESH      </description><description>   2317</description></item>
    /// <item><description>Interop.IDM.COMPOSESETTINGS      </description><description>   2318</description></item>
    /// <item><description>Interop.IDM.SHOWALLTAGS           </description><description>  2327</description></item>
    /// <item><description>Interop.IDM.SHOWALIGNEDSITETAGS   </description><description>  2321</description></item>
    /// <item><description>Interop.IDM.SHOWSCRIPTTAGS        </description><description>  2322</description></item>
    /// <item><description>Interop.IDM.SHOWSTYLETAGS         </description><description>  2323</description></item>
    /// <item><description>Interop.IDM.SHOWCOMMENTTAGS       </description><description>  2324</description></item>
    /// <item><description>Interop.IDM.SHOWAREATAGS          </description><description>  2325</description></item>
    /// <item><description>Interop.IDM.SHOWUNKNOWNTAGS       </description><description>  2326</description></item>
    /// <item><description>Interop.IDM.SHOWMISCTAGS          </description><description>  2320</description></item>
    /// <item><description>Interop.IDM.SHOWZEROBORDERATDESIGNTIME  </description><description>       2328</description></item>
    /// <item><description>Interop.IDM.AUTODETECT         </description><description>     2329</description></item>
    /// <item><description>Interop.IDM.SCRIPTDEBUGGER     </description><description>     2330</description></item>
    /// <item><description>Interop.IDM.GETBYTESDOWNLOADED  </description><description>    2331</description></item>
    /// <item><description>Interop.IDM.NOACTIVATENORMALOLECONTROLS   </description><description>     2332</description></item>
    /// <item><description>Interop.IDM.NOACTIVATEDESIGNTIMECONTROLS  </description><description>     2333</description></item>
    /// <item><description>Interop.IDM.NOACTIVATEJAVAAPPLETS          </description><description>    2334</description></item>
    /// <item><description>Interop.IDM.NOFIXUPURLSONPASTE              </description><description>   2335</description></item>
    /// <item><description>Interop.IDM.EMPTYGLYPHTABLE   </description><description>      2336</description></item>
    /// <item><description>Interop.IDM.ADDTOGLYPHTABLE   </description><description>      2337</description></item>
    /// <item><description>Interop.IDM.REMOVEFROMGLYPHTABLE </description><description>   2338</description></item>
    /// <item><description>Interop.IDM.REPLACEGLYPHCONTENTS  </description><description>  2339</description></item>
    /// <item><description>Interop.IDM.SHOWWBRTAGS            </description><description> 2340</description></item>
    /// <item><description>Interop.IDM.PERSISTSTREAMSYNC      </description><description> 2341</description></item>
    /// <item><description>Interop.IDM.SETDIRTY              </description><description>  2342</description></item>
    /// <item><description>Interop.IDM.RUNURLSCRIPT       </description><description>     2343</description></item>
    /// <item><description>Interop.IDM.ZOOMRATIO          </description><description>     2344</description></item>
    /// <item><description>Interop.IDM.GETZOOMNUMERATOR    </description><description>    2345</description></item>
    /// <item><description>Interop.IDM.GETZOOMDENOMINATOR   </description><description>   2346</description></item>
    /// <item><description>Interop.IDM.DIRLTR                </description><description>  2350</description></item>
    /// <item><description>Interop.IDM.DIRRTL               </description><description>   2351</description></item>
    /// <item><description>Interop.IDM.BLOCKDIRLTR          </description><description>   2352</description></item>
    /// <item><description>Interop.IDM.BLOCKDIRRTL          </description><description>   2353</description></item>
    /// <item><description>Interop.IDM.INLINEDIRLTR         </description><description>   2354</description></item>
    /// <item><description>Interop.IDM.INLINEDIRRTL         </description><description>   2355</description></item>
    /// <item><description>Interop.IDM.ISTRUSTEDDLG     </description><description>       2356</description></item>
    /// <item><description>Interop.IDM.INSERTSPAN        </description><description>      2357</description></item>
    /// <item><description>Interop.IDM.LOCALIZEEDITOR     </description><description>     2358</description></item>
    /// <item><description>Interop.IDM.SAVEPRETRANSFORMSOURCE </description><description> 2370</description></item>
    /// <item><description>Interop.IDM.VIEWPRETRANSFORMSOURCE </description><description> 2371</description></item>
    /// <item><description>Interop.IDM.SCROLL_HERE            </description><description> 2380</description></item>
    /// <item><description>Interop.IDM.SCROLL_TOP             </description><description> 2381</description></item>
    /// <item><description>Interop.IDM.SCROLL_BOTTOM          </description><description> 2382</description></item>
    /// <item><description>Interop.IDM.SCROLL_PAGEUP          </description><description> 2383</description></item>
    /// <item><description>Interop.IDM.SCROLL_PAGEDOWN        </description><description> 2384</description></item>
    /// <item><description>Interop.IDM.SCROLL_UP              </description><description> 2385</description></item>
    /// <item><description>Interop.IDM.SCROLL_DOWN            </description><description> 2386</description></item>
    /// <item><description>Interop.IDM.SCROLL_LEFTEDGE        </description><description> 2387</description></item>
    /// <item><description>Interop.IDM.SCROLL_RIGHTEDGE       </description><description> 2388</description></item>
    /// <item><description>Interop.IDM.SCROLL_PAGELEFT        </description><description> 2389</description></item>
    /// <item><description>Interop.IDM.SCROLL_PAGERIGHT       </description><description> 2390</description></item>
    /// <item><description>Interop.IDM.SCROLL_LEFT            </description><description> 2391</description></item>
    /// <item><description>Interop.IDM.SCROLL_RIGHT           </description><description> 2392</description></item>
    /// <item><description>Interop.IDM.MULTIPLESELECTION      </description><description> 2393</description></item>
    /// <item><description>Interop.IDM.TWOD_POSITION            </description><description> 2394</description></item>
    /// <item><description>Interop.IDM.TWOD_ELEMENT             </description><description> 2395</description></item>
    /// <item><description>Interop.IDM.ONED_ELEMENT             </description><description> 2396</description></item>
    /// <item><description>Interop.IDM.ABSOLUTE_POSITION      </description><description> 2397</description></item>
    /// <item><description>Interop.IDM.LIVERESIZE             </description><description> 2398</description></item>
    /// <item><description>Interop.IDM.ATOMICSELECTION	</description><description>		2399</description></item>
    /// <item><description>Interop.IDM.AUTOURLDETECT_MODE  </description><description>    2400</description></item>
    /// <item><description>Interop.IDM.IE50_PASTE          </description><description>    2401</description></item>
    /// <item><description>Interop.IDM.IE50_PASTE_MODE      </description><description>   2402</description></item>
    /// <item><description>Interop.IDM.DISABLE_EDITFOCUS_UI   </description><description> 2404</description></item>
    /// <item><description>Interop.IDM.RESPECTVISIBILITY_INDESIGN </description><description> 2405</description></item>
    /// <item><description>Interop.IDM.CSSEDITING_LEVEL         </description><description>   2406</description></item>
    /// <item><description>Interop.IDM.UI_OUTDENT                </description><description>  2407</description></item>
    /// <item><description>Interop.IDM.UPDATEPAGESTATUS           </description><description> 2408</description></item>
    /// <item><description>Interop.IDM.IME_ENABLE_RECONVERSION	</description><description>	2409</description></item>
    /// <item><description>Interop.IDM.KEEPSELECTION			</description><description>	2410</description></item>
    /// <item><description>Interop.IDM.UNLOADDOCUMENT             </description><description> 2411</description></item>
    /// <item><description>Interop.IDM.OVERRIDE_CURSOR            </description><description> 2420</description></item>
    /// <item><description>Interop.IDM.PEERHITTESTSAMEINEDIT      </description><description> 2423</description></item>
    /// <item><description>Interop.IDM.TRUSTAPPCACHE              </description><description> 2425</description></item>
    /// <item><description>Interop.IDM.BACKGROUNDIMAGECACHE       </description><description> 2430</description></item>
    /// <item><description>Interop.IDM.DEFAULTBLOCK           </description><description>     6046</description></item>
    /// <item><description>Interop.IDM.MIMECSET__FIRST__       </description><description>    3609</description></item>
    /// <item><description>Interop.IDM.MIMECSET__LAST__         </description><description>   3699</description></item>
    /// <item><description>Interop.IDM.MENUEXT_FIRST__   </description><description>    3700</description></item>
    /// <item><description>Interop.IDM.MENUEXT_LAST__     </description><description>   3732</description></item>
    /// <item><description>Interop.IDM.MENUEXT_COUNT       </description><description>  3733</description></item>
    /// <item><description>Interop.IDM.OPEN                </description><description>    2000</description></item>
    /// <item><description>Interop.IDM.NEW                 </description><description>    2001</description></item>
    /// <item><description>Interop.IDM.SAVE                </description><description>    70</description></item>
    /// <item><description>Interop.IDM.SAVEAS              </description><description>    71</description></item>
    /// <item><description>Interop.IDM.SAVECOPYAS          </description><description>    2002</description></item>
    /// <item><description>Interop.IDM.PRINTPREVIEW        </description><description>    2003</description></item>
    /// <item><description>Interop.IDM.SHOWPRINT           </description><description>    2010</description></item>
    /// <item><description>Interop.IDM.SHOWPAGESETUP       </description><description>    2011</description></item>
    /// <item><description>Interop.IDM.PRINT               </description><description>    27</description></item>
    /// <item><description>Interop.IDM.PAGESETUP           </description><description>    2004</description></item>
    /// <item><description>Interop.IDM.SPELL               </description><description>    2005</description></item>
    /// <item><description>Interop.IDM.PASTESPECIAL        </description><description>    2006</description></item>
    /// <item><description>Interop.IDM.CLEARSELECTION      </description><description>    2007</description></item>
    /// <item><description>Interop.IDM.PROPERTIES          </description><description>    28</description></item>
    /// <item><description>Interop.IDM.REDO                </description><description>    29</description></item>
    /// <item><description>Interop.IDM.UNDO                </description><description>    43</description></item>
    /// <item><description>Interop.IDM.SELECTALL           </description><description>    31</description></item>
    /// <item><description>Interop.IDM.ZOOMPERCENT         </description><description>    50</description></item>
    /// <item><description>Interop.IDM.GETZOOM             </description><description>    68</description></item>
    /// <item><description>Interop.IDM.STOP                </description><description>    2138</description></item>
    /// <item><description>Interop.IDM.COPY                </description><description>    15</description></item>
    /// <item><description>Interop.IDM.CUT                 </description><description>    16</description></item>
    /// <item><description>Interop.IDM.PASTE               </description><description>    26</description></item>
    /// <item><description>Interop.IDM.PERSISTDEFAULTVALUES </description><description> 7100</description></item>
    /// <item><description>Interop.IDM.PROTECTMETATAGS </description><description>  7101</description></item>
    /// <item><description>Interop.IDM.PRESERVEUNDOALWAYS </description><description> 6049</description></item>
    /// </list>
    /// Note: The appearance in this list does not mean that a particular command works either in NetRix or
    /// MSHTML.
    /// </remarks>
    /// <param name="command">The IDM command ID sent to the control.</param>
    public void SendIDMCommand(int command) {
      if (command < 1 || command > 7101)
        return;
      try {
        this.Exec((Interop.IDM)command);
      } catch (Exception ex) {
        MessageBox.Show(ex.Message, "Error sending IDM");
      }
    }

    /// <summary>
    /// This method sends an IDM command directly to MSHTML.
    /// </summary>
    /// <remarks>
    /// The call is protected by a try/catch block. The 
    /// method does not throw an exception on error and does always return nothing. This method is just for
    /// experimental environments and should not used in production environments.
    /// <para>
    /// This is method is subject to be changed or removed in later versions without further notice.
    /// </para>
    /// <list type="table">
    /// <listheader>
    ///     <term>IDM constant</term><term>Numeric Value</term>
    /// </listheader>
    /// <item><description>Interop.IDM.UNKNOWN                 </description><description>0</description></item>
    /// <item><description>Interop.IDM.ALIGNBOTTOM             </description><description>1</description></item>
    /// <item><description>Interop.IDM.ALIGNHORIZONTALCENTERS  </description><description>2</description></item>
    /// <item><description>Interop.IDM.ALIGNLEFT               </description><description>3</description></item>
    /// <item><description>Interop.IDM.ALIGNRIGHT              </description><description>4</description></item>
    /// <item><description>Interop.IDM.ALIGNTOGRID             </description><description>5</description></item>
    /// <item><description>Interop.IDM.ALIGNTOP                </description><description>6</description></item>
    /// <item><description>Interop.IDM.ALIGNVERTICALCENTERS    </description><description>7</description></item>
    /// <item><description>Interop.IDM.ARRANGEBOTTOM           </description><description>8</description></item>
    /// <item><description>Interop.IDM.ARRANGERIGHT            </description><description>9</description></item>
    /// <item><description>Interop.IDM.BRINGFORWARD            </description><description>10</description></item>
    /// <item><description>Interop.IDM.BRINGTOFRONT            </description><description>11</description></item>
    /// <item><description>Interop.IDM.CENTERHORIZONTALLY      </description><description>12</description></item>
    /// <item><description>Interop.IDM.CENTERVERTICALLY        </description><description>13</description></item>
    /// <item><description>Interop.IDM.CODE                    </description><description>14</description></item>
    /// <item><description>Interop.IDM.DELETE                  </description><description>17</description></item>
    /// <item><description>Interop.IDM.FONTNAME                </description><description>18</description></item>
    /// <item><description>Interop.IDM.FONTSIZE                </description><description>19</description></item>
    /// <item><description>Interop.IDM.GROUP                   </description><description>20</description></item>
    /// <item><description>Interop.IDM.HORIZSPACECONCATENATE   </description><description>21</description></item>
    /// <item><description>Interop.IDM.HORIZSPACEDECREASE      </description><description>22</description></item>
    /// <item><description>Interop.IDM.HORIZSPACEINCREASE      </description><description>23</description></item>
    /// <item><description>Interop.IDM.HORIZSPACEMAKEEQUAL     </description><description>24</description></item>
    /// <item><description>Interop.IDM.INSERTOBJECT            </description><description>25</description></item>
    /// <item><description>Interop.IDM.MULTILEVELREDO          </description><description>30</description></item>
    /// <item><description>Interop.IDM.SENDBACKWARD            </description><description>32</description></item>
    /// <item><description>Interop.IDM.SENDTOBACK              </description><description>33</description></item>
    /// <item><description>Interop.IDM.SHOWTABLE               </description><description>34</description></item>
    /// <item><description>Interop.IDM.SIZETOCONTROL           </description><description>35</description></item>
    /// <item><description>Interop.IDM.SIZETOCONTROLHEIGHT     </description><description>36</description></item>
    /// <item><description>Interop.IDM.SIZETOCONTROLWIDTH      </description><description>37</description></item>
    /// <item><description>Interop.IDM.SIZETOFIT               </description><description>38</description></item>
    /// <item><description>Interop.IDM.SIZETOGRID              </description><description>39</description></item>
    /// <item><description>Interop.IDM.SNAPTOGRID              </description><description>40</description></item>
    /// <item><description>Interop.IDM.TABORDER                </description><description>41</description></item>
    /// <item><description>Interop.IDM.TOOLBOX                 </description><description>42</description></item>
    /// <item><description>Interop.IDM.MULTILEVELUNDO          </description><description>44</description></item>
    /// <item><description>Interop.IDM.UNGROUP                 </description><description>45</description></item>
    /// <item><description>Interop.IDM.VERTSPACECONCATENATE    </description><description>46</description></item>
    /// <item><description>Interop.IDM.VERTSPACEDECREASE       </description><description>47</description></item>
    /// <item><description>Interop.IDM.VERTSPACEINCREASE       </description><description>48</description></item>
    /// <item><description>Interop.IDM.VERTSPACEMAKEEQUAL      </description><description>49</description></item>
    /// <item><description>Interop.IDM.JUSTIFYFULL             </description><description>50</description></item>
    /// <item><description>Interop.IDM.BACKCOLOR               </description><description>51</description></item>
    /// <item><description>Interop.IDM.BOLD                    </description><description>52</description></item>
    /// <item><description>Interop.IDM.BORDERCOLOR             </description><description>53</description></item>
    /// <item><description>Interop.IDM.FLAT                    </description><description>54</description></item>
    /// <item><description>Interop.IDM.FORECOLOR               </description><description>55</description></item>
    /// <item><description>Interop.IDM.ITALIC                  </description><description>56</description></item>
    /// <item><description>Interop.IDM.JUSTIFYCENTER           </description><description>57</description></item>
    /// <item><description>Interop.IDM.JUSTIFYGENERAL          </description><description>58</description></item>
    /// <item><description>Interop.IDM.JUSTIFYLEFT             </description><description>59</description></item>
    /// <item><description>Interop.IDM.JUSTIFYRIGHT            </description><description>60</description></item>
    /// <item><description>Interop.IDM.RAISED                  </description><description>61</description></item>
    /// <item><description>Interop.IDM.SUNKEN                  </description><description>62</description></item>
    /// <item><description>Interop.IDM.UNDERLINE               </description><description>63</description></item>
    /// <item><description>Interop.IDM.CHISELED                </description><description>64</description></item>
    /// <item><description>Interop.IDM.ETCHED                  </description><description>65</description></item>
    /// <item><description>Interop.IDM.SHADOWED                </description><description>66</description></item>
    /// <item><description>Interop.IDM.FIND                    </description><description>67</description></item>
    /// <item><description>Interop.IDM.SHOWGRID                </description><description>69</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST0         </description><description>72</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST1         </description><description>73</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST2         </description><description>74</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST3         </description><description>75</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST4         </description><description>76</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST5         </description><description>77</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST6         </description><description>78</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST7         </description><description>79</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST8         </description><description>80</description></item>
    /// <item><description>Interop.IDM.OBJECTVERBLIST9         </description><description>81</description></item>
    /// <item><description>Interop.IDM.CONVERTOBJECT       </description><description>    82</description></item>
    /// <item><description>Interop.IDM.CUSTOMCONTROL       </description><description>    83</description></item>
    /// <item><description>Interop.IDM.CUSTOMIZEITEM       </description><description>    84</description></item>
    /// <item><description>Interop.IDM.RENAME              </description><description>    85</description></item>
    /// <item><description>Interop.IDM.IMPORT              </description><description>    86</description></item>
    /// <item><description>Interop.IDM.NEWPAGE             </description><description>    87</description></item>
    /// <item><description>Interop.IDM.MOVE                </description><description>    88</description></item>
    /// <item><description>Interop.IDM.CANCEL              </description><description>    89</description></item>
    /// <item><description>Interop.IDM.FONT                </description><description>    90</description></item>
    /// <item><description>Interop.IDM.STRIKETHROUGH       </description><description>    91</description></item>
    /// <item><description>Interop.IDM.DELETEWORD          </description><description>    92</description></item>
    /// <item><description>Interop.IDM.EXECPRINT           </description><description>    93</description></item>
    /// <item><description>Interop.IDM.JUSTIFYNONE         </description><description>    94</description></item>
    /// <item><description>Interop.IDM.TRISTATEBOLD        </description><description>    95</description></item>
    /// <item><description>Interop.IDM.TRISTATEITALIC      </description><description>    96</description></item>
    /// <item><description>Interop.IDM.TRISTATEUNDERLINE   </description><description>    97</description></item>
    /// <item><description>Interop.IDM.FOLLOW_ANCHOR        </description><description>   2008</description></item>
    /// <item><description>Interop.IDM.INSINPUTIMAGE         </description><description>  2114</description></item>
    /// <item><description>Interop.IDM.INSINPUTBUTTON        </description><description>  2115</description></item>
    /// <item><description>Interop.IDM.INSINPUTRESET         </description><description>  2116</description></item>
    /// <item><description>Interop.IDM.INSINPUTSUBMIT        </description><description>  2117</description></item>
    /// <item><description>Interop.IDM.INSINPUTUPLOAD        </description><description>  2118</description></item>
    /// <item><description>Interop.IDM.INSFIELDSET           </description><description>  2119</description></item>
    /// <item><description>Interop.IDM.PASTEINSERT          </description><description>   2120</description></item>
    /// <item><description>Interop.IDM.REPLACE              </description><description>   2121</description></item>
    /// <item><description>Interop.IDM.EDITSOURCE           </description><description>   2122</description></item>
    /// <item><description>Interop.IDM.BOOKMARK             </description><description>   2123</description></item>
    /// <item><description>Interop.IDM.HYPERLINK            </description><description>   2124</description></item>
    /// <item><description>Interop.IDM.UNLINK               </description><description>   2125</description></item>
    /// <item><description>Interop.IDM.BROWSEMODE           </description><description>   2126</description></item>
    /// <item><description>Interop.IDM.EDITMODE             </description><description>   2127</description></item>
    /// <item><description>Interop.IDM.UNBOOKMARK           </description><description>   2128</description></item>
    /// <item><description>Interop.IDM.TOOLBARS             </description><description>   2130</description></item>
    /// <item><description>Interop.IDM.STATUSBAR            </description><description>   2131</description></item>
    /// <item><description>Interop.IDM.FORMATMARK           </description><description>   2132</description></item>
    /// <item><description>Interop.IDM.TEXTONLY             </description><description>   2133</description></item>
    /// <item><description>Interop.IDM.OPTIONS              </description><description>   2135</description></item>
    /// <item><description>Interop.IDM.FOLLOWLINKC          </description><description>   2136</description></item>
    /// <item><description>Interop.IDM.FOLLOWLINKN          </description><description>   2137</description></item>
    /// <item><description>Interop.IDM.VIEWSOURCE           </description><description>   2139</description></item>
    /// <item><description>Interop.IDM.ZOOMPOPUP            </description><description>   2140</description></item>
    /// <item><description>Interop.IDM.BASELINEFONT1       </description><description>    2141</description></item>
    /// <item><description>Interop.IDM.BASELINEFONT2       </description><description>    2142</description></item>
    /// <item><description>Interop.IDM.BASELINEFONT3       </description><description>    2143</description></item>
    /// <item><description>Interop.IDM.BASELINEFONT4       </description><description>    2144</description></item>
    /// <item><description>Interop.IDM.BASELINEFONT5       </description><description>    2145</description></item>
    /// <item><description>Interop.IDM.HORIZONTALLINE      </description><description>    2150</description></item>
    /// <item><description>Interop.IDM.LINEBREAKNORMAL     </description><description>    2151</description></item>
    /// <item><description>Interop.IDM.LINEBREAKLEFT       </description><description>    2152</description></item>
    /// <item><description>Interop.IDM.LINEBREAKRIGHT      </description><description>    2153</description></item>
    /// <item><description>Interop.IDM.LINEBREAKBOTH       </description><description>    2154</description></item>
    /// <item><description>Interop.IDM.NONBREAK            </description><description>    2155</description></item>
    /// <item><description>Interop.IDM.SPECIALCHAR         </description><description>    2156</description></item>
    /// <item><description>Interop.IDM.HTMLSOURCE          </description><description>    2157</description></item>
    /// <item><description>Interop.IDM.IFRAME              </description><description>    2158</description></item>
    /// <item><description>Interop.IDM.HTMLCONTAIN         </description><description>    2159</description></item>
    /// <item><description>Interop.IDM.TEXTBOX             </description><description>    2161</description></item>
    /// <item><description>Interop.IDM.TEXTAREA            </description><description>    2162</description></item>
    /// <item><description>Interop.IDM.CHECKBOX            </description><description>    2163</description></item>
    /// <item><description>Interop.IDM.RADIOBUTTON         </description><description>    2164</description></item>
    /// <item><description>Interop.IDM.DROPDOWNBOX         </description><description>    2165</description></item>
    /// <item><description>Interop.IDM.LISTBOX             </description><description>    2166</description></item>
    /// <item><description>Interop.IDM.BUTTON              </description><description>    2167</description></item>
    /// <item><description>Interop.IDM.IMAGE               </description><description>    2168</description></item>
    /// <item><description>Interop.IDM.OBJECT              </description><description>    2169</description></item>
    /// <item><description>Interop.IDM.1D                  </description><description>    2170</description></item>
    /// <item><description>Interop.IDM.IMAGEMAP            </description><description>    2171</description></item>
    /// <item><description>Interop.IDM.FILE                </description><description>    2172</description></item>
    /// <item><description>Interop.IDM.COMMENT             </description><description>    2173</description></item>
    /// <item><description>Interop.IDM.SCRIPT              </description><description>    2174</description></item>
    /// <item><description>Interop.IDM.JAVAAPPLET          </description><description>    2175</description></item>
    /// <item><description>Interop.IDM.PLUGIN              </description><description>    2176</description></item>
    /// <item><description>Interop.IDM.PAGEBREAK           </description><description>    2177</description></item>
    /// <item><description>Interop.IDM.HTMLAREA            </description><description>    2178</description></item>
    /// <item><description>Interop.IDM.PARAGRAPH           </description><description>    2180</description></item>
    /// <item><description>Interop.IDM.FORM                </description><description>    2181</description></item>
    /// <item><description>Interop.IDM.MARQUEE             </description><description>    2182</description></item>
    /// <item><description>Interop.IDM.LIST                </description><description>    2183</description></item>
    /// <item><description>Interop.IDM.ORDERLIST           </description><description>    2184</description></item>
    /// <item><description>Interop.IDM.UNORDERLIST         </description><description>    2185</description></item>
    /// <item><description>Interop.IDM.INDENT              </description><description>    2186</description></item>
    /// <item><description>Interop.IDM.OUTDENT             </description><description>    2187</description></item>
    /// <item><description>Interop.IDM.PREFORMATTED        </description><description>    2188</description></item>
    /// <item><description>Interop.IDM.ADDRESS             </description><description>    2189</description></item>
    /// <item><description>Interop.IDM.BLINK               </description><description>    2190</description></item>
    /// <item><description>Interop.IDM.DIV                 </description><description>    2191</description></item>
    /// <item><description>Interop.IDM.TABLEINSERT         </description><description>    2200</description></item>
    /// <item><description>Interop.IDM.RCINSERT            </description><description>    2201</description></item>
    /// <item><description>Interop.IDM.CELLINSERT          </description><description>    2202</description></item>
    /// <item><description>Interop.IDM.CAPTIONINSERT       </description><description>    2203</description></item>
    /// <item><description>Interop.IDM.CELLMERGE           </description><description>    2204</description></item>
    /// <item><description>Interop.IDM.CELLSPLIT           </description><description>    2205</description></item>
    /// <item><description>Interop.IDM.CELLSELECT          </description><description>    2206</description></item>
    /// <item><description>Interop.IDM.ROWSELECT           </description><description>    2207</description></item>
    /// <item><description>Interop.IDM.COLUMNSELECT        </description><description>    2208</description></item>
    /// <item><description>Interop.IDM.TABLESELECT         </description><description>    2209</description></item>
    /// <item><description>Interop.IDM.TABLEPROPERTIES     </description><description>    2210</description></item>
    /// <item><description>Interop.IDM.CELLPROPERTIES      </description><description>    2211</description></item>
    /// <item><description>Interop.IDM.ROWINSERT           </description><description>    2212</description></item>
    /// <item><description>Interop.IDM.COLUMNINSERT        </description><description>    2213</description></item>
    /// <item><description>Interop.IDM.HELP_CONTENT         </description><description>   2220</description></item>
    /// <item><description>Interop.IDM.HELP_ABOUT           </description><description>   2221</description></item>
    /// <item><description>Interop.IDM.HELP_README          </description><description>   2222</description></item>
    /// <item><description>Interop.IDM.REMOVEFORMAT          </description><description>  2230</description></item>
    /// <item><description>Interop.IDM.PAGEINFO             </description><description>   2231</description></item>
    /// <item><description>Interop.IDM.TELETYPE             </description><description>   2232</description></item>
    /// <item><description>Interop.IDM.GETBLOCKFMTS          </description><description>  2233</description></item>
    /// <item><description>Interop.IDM.BLOCKFMT              </description><description>  2234</description></item>
    /// <item><description>Interop.IDM.SHOWHIDE_CODE         </description><description>  2235</description></item>
    /// <item><description>Interop.IDM.TABLE                 </description><description>  2236</description></item>
    /// <item><description>Interop.IDM.COPYFORMAT             </description><description> 2237</description></item>
    /// <item><description>Interop.IDM.PASTEFORMAT         </description><description>    2238</description></item>
    /// <item><description>Interop.IDM.GOTO                 </description><description>   2239</description></item>
    /// <item><description>Interop.IDM.CHANGEFONT            </description><description>  2240</description></item>
    /// <item><description>Interop.IDM.CHANGEFONTSIZE        </description><description>  2241</description></item>
    /// <item><description>Interop.IDM.CHANGECASE            </description><description>  2246</description></item>
    /// <item><description>Interop.IDM.SHOWSPECIALCHAR       </description><description>  2249</description></item>
    /// <item><description>Interop.IDM.SUBSCRIPT             </description><description>  2247</description></item>
    /// <item><description>Interop.IDM.SUPERSCRIPT           </description><description>  2248</description></item>
    /// <item><description>Interop.IDM.CENTERALIGNPARA       </description><description>  2250</description></item>
    /// <item><description>Interop.IDM.LEFTALIGNPARA         </description><description>  2251</description></item>
    /// <item><description>Interop.IDM.RIGHTALIGNPARA        </description><description>  2252</description></item>
    /// <item><description>Interop.IDM.REMOVEPARAFORMAT      </description><description>  2253</description></item>
    /// <item><description>Interop.IDM.APPLYNORMAL           </description><description>  2254</description></item>
    /// <item><description>Interop.IDM.APPLYHEADING1         </description><description>  2255</description></item>
    /// <item><description>Interop.IDM.APPLYHEADING2         </description><description>  2256</description></item>
    /// <item><description>Interop.IDM.APPLYHEADING3         </description><description>  2257</description></item>
    /// <item><description>Interop.IDM.DOCPROPERTIES         </description><description>  2260</description></item>
    /// <item><description>Interop.IDM.ADDFAVORITES          </description><description>  2261</description></item>
    /// <item><description>Interop.IDM.COPYSHORTCUT          </description><description>  2262</description></item>
    /// <item><description>Interop.IDM.SAVEBACKGROUND        </description><description>  2263</description></item>
    /// <item><description>Interop.IDM.SETWALLPAPER          </description><description>  2264</description></item>
    /// <item><description>Interop.IDM.COPYBACKGROUND        </description><description>  2265</description></item>
    /// <item><description>Interop.IDM.CREATESHORTCUT        </description><description>  2266</description></item>
    /// <item><description>Interop.IDM.PAGE                  </description><description>  2267</description></item>
    /// <item><description>Interop.IDM.SAVETARGET            </description><description>  2268</description></item>
    /// <item><description>Interop.IDM.SHOWPICTURE           </description><description>  2269</description></item>
    /// <item><description>Interop.IDM.SAVEPICTURE           </description><description>  2270</description></item>
    /// <item><description>Interop.IDM.DYNSRCPLAY            </description><description>  2271</description></item>
    /// <item><description>Interop.IDM.DYNSRCSTOP            </description><description>  2272</description></item>
    /// <item><description>Interop.IDM.PRINTTARGET           </description><description>  2273</description></item>
    /// <item><description>Interop.IDM.IMGARTPLAY            </description><description>  2274</description></item>
    /// <item><description>Interop.IDM.IMGARTSTOP            </description><description>  2275</description></item>
    /// <item><description>Interop.IDM.IMGARTREWIND          </description><description>  2276</description></item>
    /// <item><description>Interop.IDM.PRINTQUERYJOBSPENDING </description><description>  2277</description></item>
    /// <item><description>Interop.IDM.SETDESKTOPITEM        </description><description>  2278</description></item>
    /// <item><description>Interop.IDM.CONTEXTMENU           </description><description>  2280</description></item>
    /// <item><description>Interop.IDM.GOBACKWARD            </description><description>  2282</description></item>
    /// <item><description>Interop.IDM.GOFORWARD             </description><description>  2283</description></item>
    /// <item><description>Interop.IDM.PRESTOP               </description><description>  2284</description></item>
    /// <item><description>Interop.IDM.MP_MYPICS             </description><description>  2287</description></item>
    /// <item><description>Interop.IDM.MP_EMAILPICTURE       </description><description>  2288</description></item>
    /// <item><description>Interop.IDM.MP_PRINTPICTURE       </description><description>  2289</description></item>
    /// <item><description>Interop.IDM.CREATELINK           </description><description>   2290</description></item>
    /// <item><description>Interop.IDM.COPYCONTENT          </description><description>   2291</description></item>
    /// <item><description>Interop.IDM.LANGUAGE             </description><description>   2292</description></item>
    /// <item><description>Interop.IDM.GETPRINTTEMPLATE    </description><description>    2295</description></item>
    /// <item><description>Interop.IDM.SETPRINTTEMPLATE    </description><description>    2296</description></item>
    /// <item><description>Interop.IDM.TEMPLATE_PAGESETUP  </description><description>    2298</description></item>
    /// <item><description>Interop.IDM.REFRESH              </description><description>   2300</description></item>
    /// <item><description>Interop.IDM.STOPDOWNLOAD         </description><description>   2301</description></item>
    /// <item><description>Interop.IDM.ENABLE_INTERACTION    </description><description>  2302</description></item>
    /// <item><description>Interop.IDM.LAUNCHDEBUGGER         </description><description> 2310</description></item>
    /// <item><description>Interop.IDM.BREAKATNEXT            </description><description> 2311</description></item>
    /// <item><description>Interop.IDM.INSINPUTHIDDEN        </description><description>  2312</description></item>
    /// <item><description>Interop.IDM.INSINPUTPASSWORD      </description><description>  2313</description></item>
    /// <item><description>Interop.IDM.OVERWRITE          </description><description>     2314</description></item>
    /// <item><description>Interop.IDM.PARSECOMPLETE       </description><description>    2315</description></item>
    /// <item><description>Interop.IDM.HTMLEDITMODE       </description><description>     2316</description></item>
    /// <item><description>Interop.IDM.REGISTRYREFRESH      </description><description>   2317</description></item>
    /// <item><description>Interop.IDM.COMPOSESETTINGS      </description><description>   2318</description></item>
    /// <item><description>Interop.IDM.SHOWALLTAGS           </description><description>  2327</description></item>
    /// <item><description>Interop.IDM.SHOWALIGNEDSITETAGS   </description><description>  2321</description></item>
    /// <item><description>Interop.IDM.SHOWSCRIPTTAGS        </description><description>  2322</description></item>
    /// <item><description>Interop.IDM.SHOWSTYLETAGS         </description><description>  2323</description></item>
    /// <item><description>Interop.IDM.SHOWCOMMENTTAGS       </description><description>  2324</description></item>
    /// <item><description>Interop.IDM.SHOWAREATAGS          </description><description>  2325</description></item>
    /// <item><description>Interop.IDM.SHOWUNKNOWNTAGS       </description><description>  2326</description></item>
    /// <item><description>Interop.IDM.SHOWMISCTAGS          </description><description>  2320</description></item>
    /// <item><description>Interop.IDM.SHOWZEROBORDERATDESIGNTIME  </description><description>       2328</description></item>
    /// <item><description>Interop.IDM.AUTODETECT         </description><description>     2329</description></item>
    /// <item><description>Interop.IDM.SCRIPTDEBUGGER     </description><description>     2330</description></item>
    /// <item><description>Interop.IDM.GETBYTESDOWNLOADED  </description><description>    2331</description></item>
    /// <item><description>Interop.IDM.NOACTIVATENORMALOLECONTROLS   </description><description>     2332</description></item>
    /// <item><description>Interop.IDM.NOACTIVATEDESIGNTIMECONTROLS  </description><description>     2333</description></item>
    /// <item><description>Interop.IDM.NOACTIVATEJAVAAPPLETS          </description><description>    2334</description></item>
    /// <item><description>Interop.IDM.NOFIXUPURLSONPASTE              </description><description>   2335</description></item>
    /// <item><description>Interop.IDM.EMPTYGLYPHTABLE   </description><description>      2336</description></item>
    /// <item><description>Interop.IDM.ADDTOGLYPHTABLE   </description><description>      2337</description></item>
    /// <item><description>Interop.IDM.REMOVEFROMGLYPHTABLE </description><description>   2338</description></item>
    /// <item><description>Interop.IDM.REPLACEGLYPHCONTENTS  </description><description>  2339</description></item>
    /// <item><description>Interop.IDM.SHOWWBRTAGS            </description><description> 2340</description></item>
    /// <item><description>Interop.IDM.PERSISTSTREAMSYNC      </description><description> 2341</description></item>
    /// <item><description>Interop.IDM.SETDIRTY              </description><description>  2342</description></item>
    /// <item><description>Interop.IDM.RUNURLSCRIPT       </description><description>     2343</description></item>
    /// <item><description>Interop.IDM.ZOOMRATIO          </description><description>     2344</description></item>
    /// <item><description>Interop.IDM.GETZOOMNUMERATOR    </description><description>    2345</description></item>
    /// <item><description>Interop.IDM.GETZOOMDENOMINATOR   </description><description>   2346</description></item>
    /// <item><description>Interop.IDM.DIRLTR                </description><description>  2350</description></item>
    /// <item><description>Interop.IDM.DIRRTL               </description><description>   2351</description></item>
    /// <item><description>Interop.IDM.BLOCKDIRLTR          </description><description>   2352</description></item>
    /// <item><description>Interop.IDM.BLOCKDIRRTL          </description><description>   2353</description></item>
    /// <item><description>Interop.IDM.INLINEDIRLTR         </description><description>   2354</description></item>
    /// <item><description>Interop.IDM.INLINEDIRRTL         </description><description>   2355</description></item>
    /// <item><description>Interop.IDM.ISTRUSTEDDLG     </description><description>       2356</description></item>
    /// <item><description>Interop.IDM.INSERTSPAN        </description><description>      2357</description></item>
    /// <item><description>Interop.IDM.LOCALIZEEDITOR     </description><description>     2358</description></item>
    /// <item><description>Interop.IDM.SAVEPRETRANSFORMSOURCE </description><description> 2370</description></item>
    /// <item><description>Interop.IDM.VIEWPRETRANSFORMSOURCE </description><description> 2371</description></item>
    /// <item><description>Interop.IDM.SCROLL_HERE            </description><description> 2380</description></item>
    /// <item><description>Interop.IDM.SCROLL_TOP             </description><description> 2381</description></item>
    /// <item><description>Interop.IDM.SCROLL_BOTTOM          </description><description> 2382</description></item>
    /// <item><description>Interop.IDM.SCROLL_PAGEUP          </description><description> 2383</description></item>
    /// <item><description>Interop.IDM.SCROLL_PAGEDOWN        </description><description> 2384</description></item>
    /// <item><description>Interop.IDM.SCROLL_UP              </description><description> 2385</description></item>
    /// <item><description>Interop.IDM.SCROLL_DOWN            </description><description> 2386</description></item>
    /// <item><description>Interop.IDM.SCROLL_LEFTEDGE        </description><description> 2387</description></item>
    /// <item><description>Interop.IDM.SCROLL_RIGHTEDGE       </description><description> 2388</description></item>
    /// <item><description>Interop.IDM.SCROLL_PAGELEFT        </description><description> 2389</description></item>
    /// <item><description>Interop.IDM.SCROLL_PAGERIGHT       </description><description> 2390</description></item>
    /// <item><description>Interop.IDM.SCROLL_LEFT            </description><description> 2391</description></item>
    /// <item><description>Interop.IDM.SCROLL_RIGHT           </description><description> 2392</description></item>
    /// <item><description>Interop.IDM.MULTIPLESELECTION      </description><description> 2393</description></item>
    /// <item><description>Interop.IDM.TWOD_POSITION            </description><description> 2394</description></item>
    /// <item><description>Interop.IDM.TWOD_ELEMENT             </description><description> 2395</description></item>
    /// <item><description>Interop.IDM.ONED_ELEMENT             </description><description> 2396</description></item>
    /// <item><description>Interop.IDM.ABSOLUTE_POSITION      </description><description> 2397</description></item>
    /// <item><description>Interop.IDM.LIVERESIZE             </description><description> 2398</description></item>
    /// <item><description>Interop.IDM.ATOMICSELECTION	</description><description>		2399</description></item>
    /// <item><description>Interop.IDM.AUTOURLDETECT_MODE  </description><description>    2400</description></item>
    /// <item><description>Interop.IDM.IE50_PASTE          </description><description>    2401</description></item>
    /// <item><description>Interop.IDM.IE50_PASTE_MODE      </description><description>   2402</description></item>
    /// <item><description>Interop.IDM.DISABLE_EDITFOCUS_UI   </description><description> 2404</description></item>
    /// <item><description>Interop.IDM.RESPECTVISIBILITY_INDESIGN </description><description> 2405</description></item>
    /// <item><description>Interop.IDM.CSSEDITING_LEVEL         </description><description>   2406</description></item>
    /// <item><description>Interop.IDM.UI_OUTDENT                </description><description>  2407</description></item>
    /// <item><description>Interop.IDM.UPDATEPAGESTATUS           </description><description> 2408</description></item>
    /// <item><description>Interop.IDM.IME_ENABLE_RECONVERSION	</description><description>	2409</description></item>
    /// <item><description>Interop.IDM.KEEPSELECTION			</description><description>	2410</description></item>
    /// <item><description>Interop.IDM.UNLOADDOCUMENT             </description><description> 2411</description></item>
    /// <item><description>Interop.IDM.OVERRIDE_CURSOR            </description><description> 2420</description></item>
    /// <item><description>Interop.IDM.PEERHITTESTSAMEINEDIT      </description><description> 2423</description></item>
    /// <item><description>Interop.IDM.TRUSTAPPCACHE              </description><description> 2425</description></item>
    /// <item><description>Interop.IDM.BACKGROUNDIMAGECACHE       </description><description> 2430</description></item>
    /// <item><description>Interop.IDM.DEFAULTBLOCK           </description><description>     6046</description></item>
    /// <item><description>Interop.IDM.MIMECSET__FIRST__       </description><description>    3609</description></item>
    /// <item><description>Interop.IDM.MIMECSET__LAST__         </description><description>   3699</description></item>
    /// <item><description>Interop.IDM.MENUEXT_FIRST__   </description><description>    3700</description></item>
    /// <item><description>Interop.IDM.MENUEXT_LAST__     </description><description>   3732</description></item>
    /// <item><description>Interop.IDM.MENUEXT_COUNT       </description><description>  3733</description></item>
    /// <item><description>Interop.IDM.OPEN                </description><description>    2000</description></item>
    /// <item><description>Interop.IDM.NEW                 </description><description>    2001</description></item>
    /// <item><description>Interop.IDM.SAVE                </description><description>    70</description></item>
    /// <item><description>Interop.IDM.SAVEAS              </description><description>    71</description></item>
    /// <item><description>Interop.IDM.SAVECOPYAS          </description><description>    2002</description></item>
    /// <item><description>Interop.IDM.PRINTPREVIEW        </description><description>    2003</description></item>
    /// <item><description>Interop.IDM.SHOWPRINT           </description><description>    2010</description></item>
    /// <item><description>Interop.IDM.SHOWPAGESETUP       </description><description>    2011</description></item>
    /// <item><description>Interop.IDM.PRINT               </description><description>    27</description></item>
    /// <item><description>Interop.IDM.PAGESETUP           </description><description>    2004</description></item>
    /// <item><description>Interop.IDM.SPELL               </description><description>    2005</description></item>
    /// <item><description>Interop.IDM.PASTESPECIAL        </description><description>    2006</description></item>
    /// <item><description>Interop.IDM.CLEARSELECTION      </description><description>    2007</description></item>
    /// <item><description>Interop.IDM.PROPERTIES          </description><description>    28</description></item>
    /// <item><description>Interop.IDM.REDO                </description><description>    29</description></item>
    /// <item><description>Interop.IDM.UNDO                </description><description>    43</description></item>
    /// <item><description>Interop.IDM.SELECTALL           </description><description>    31</description></item>
    /// <item><description>Interop.IDM.ZOOMPERCENT         </description><description>    50</description></item>
    /// <item><description>Interop.IDM.GETZOOM             </description><description>    68</description></item>
    /// <item><description>Interop.IDM.STOP                </description><description>    2138</description></item>
    /// <item><description>Interop.IDM.COPY                </description><description>    15</description></item>
    /// <item><description>Interop.IDM.CUT                 </description><description>    16</description></item>
    /// <item><description>Interop.IDM.PASTE               </description><description>    26</description></item>
    /// <item><description>Interop.IDM.PERSISTDEFAULTVALUES </description><description> 7100</description></item>
    /// <item><description>Interop.IDM.PROTECTMETATAGS </description><description>  7101</description></item>
    /// <item><description>Interop.IDM.PRESERVEUNDOALWAYS </description><description> 6049</description></item>
    /// </list>
    /// Note: The appearance in this list does not mean that a particular command works either in NetRix or
    /// MSHTML.
    /// </remarks>
    /// <param name="command">The IDM command ID sent to the control.</param>
    /// <param name="argument">Argument object specific to the command parameter.</param>
    public void SendIDMCommand(int command, object argument) {
      if (command < 1 || command > 7101)
        return;
      try {
        this.Exec((Interop.IDM)command, argument);
      } catch (Exception ex) {
        MessageBox.Show(ex.Message, "Error sending IDM");
      }
    }

    /// <summary>
    /// Fires the <see cref="ContentChanged"/> event.
    /// </summary>
    protected void OnContentChanged() {

      if (!_contentModified) {
        _contentModified = true;
        if (ContentModified != null) {
          ContentModified(this, new ContentModifiedEventArgs(GetCurrentScopeElement()));
        }
      }
      if (ContentChanged != null) {
        ContentChanged(this, EventArgs.Empty);
      }
    }

    /// <summary>
    /// Gets the full decoded path to the file this document is load locally from.
    /// </summary>
    /// <remarks>
    /// This can be an URL, but under most circumstances this may be a Path. Any monikers of type "file://" are removed.
    /// </remarks>
    /// <returns></returns>
    public string GetFullPathUrl() {
      return HttpUtility.UrlDecode(this.GetActiveDocument(false).GetURL().Replace("file://", ""));
    }

    #endregion

    #region +++++ Block: Find and Replace

    private void InvokeFindHasReachedEnd() {
      if (FindHasReachedEnd != null) {
        FindHasReachedEnd(this, EventArgs.Empty);
      }
    }

    private DialogResult InvokeBeforeReplace(string word, int counter) {
      if (BeforeReplace != null) {
        BeforeReplaceEventArgs args = new BeforeReplaceEventArgs(word, counter);
        BeforeReplace(this, args);
        return args.GetResult();
      }
      return DialogResult.OK;
    }


    /// <overloads/>
    /// <summary>
    /// Searches and replaces a string.
    /// </summary>
    /// <param name="searchString">String to search for</param>
    /// <param name="replaceString">String to replace with</param>
    /// <param name="matchCase">true if case sensitive</param>
    /// <param name="wholeWord">true if only whole words (search string matches word boundaries) found</param>
    /// <param name="searchUp">true if search upwards, false otherwise</param>
    /// <returns>Number of replaces</returns>
    public int Replace(string searchString, string replaceString, bool matchCase, bool wholeWord, bool searchUp) {
      return Replace(searchString, replaceString, matchCase, wholeWord, searchUp, -1);
    }

    /// <summary>
    /// Searches and replaces a string.
    /// </summary>
    /// <remarks>
    /// The method uses the callback to <see cref="BeforeReplace"/> event to inform the host application about
    /// thew next replacement. If no handler present, the method will continuously replace all occurences of the
    /// string. The event handler can set a property of type <see cref="System.Windows.Forms.DialogResult">DialogResult</see>
    /// to control the behavior of the Replace method. The various field of the 
    /// <see cref="System.Windows.Forms.DialogResult">DialogResult</see> enumeration have the following meaning:
    /// <list type="bullet">
    /// <item><term>Yes</term><description>Replace</description></item>
    /// <item><term>OK</term><description>Replace</description></item>
    /// <item><term>None</term><description>End method and stop all replacements (e.g. Cancel)</description></item>
    /// <item><term>Cancel</term><description>End method and stop all replacements (e.g. Cancel)</description></item>
    /// <item><term>Abort</term><description>End method and stop all replacements (e.g. Cancel)</description></item>
    /// <item><term>No</term><description>Skip this (current) replacement, but continue.</description></item>
    /// <item><term>Ignore</term><description>Skip this (current) replacement, but continue.</description></item>
    /// <item><term>Retry</term><description>Start replacement at the beginning. (Helpful after some words were skipped)</description></item>
    /// </list>
    /// <para>
    /// Version 1.6.2008 (Aug 2008): Added support for spell checking within TextArea Elements.
    /// </para>
    /// </remarks>
    /// <param name="searchString">String to search for</param>
    /// <param name="replaceString">String to replace with</param>
    /// <param name="matchCase"><c>True</c> if case sensitive</param>
    /// <param name="wholeWord"><c>True</c> if only whole words (search string matches word boundaries) found</param>
    /// <param name="searchUp"><c>True</c> if search upwards, false otherwise</param>
    /// <param name="maxReplacements">Limits the number of replacements.</param>
    /// <returns>Number of replaces</returns>
    public int Replace(string searchString, string replaceString, bool matchCase, bool wholeWord, bool searchUp, int maxReplacements) {
      int counter = 0;
      Interop.IHTMLTxtRange range;
      while (FindForReplace(searchString, matchCase, wholeWord, searchUp, out range)) {
        RESTART:
        counter++;
        if ((_selection.SelectionType & HtmlSelectionType.TextSelection) == HtmlSelectionType.TextSelection // regular
                ||
                (_selection.SelectionType == HtmlSelectionType.ElementSelection && _selection.Element is TextAreaElement) // textarea
                &&
                range != null //condition
                &&
                range.GetText().Length > 0 //condition
                ) {
          range = this._selection.MSHTMLSelection as Interop.IHTMLTxtRange;
          int i = (!matchCase ? 0 : 4) | (!wholeWord ? 0 : 2);
          int j = !searchUp ? 10000000 : -10000000;
          if (range != null && range.FindText(searchString, j, i)) {
            string word = range.GetText();
            switch (InvokeBeforeReplace(word, counter)) {
              case DialogResult.Yes:
              case DialogResult.OK:
                range.SetText(replaceString);
                break;
              case DialogResult.None:
              case DialogResult.Cancel:
              case DialogResult.Abort:
                return counter;
              case DialogResult.No:
              case DialogResult.Ignore:
                break;
              case DialogResult.Retry:
                if (counter == maxReplacements)
                  break;
                if (String.Compare(replaceString, word, !matchCase) == 0)
                  break;
                goto RESTART;
            }
            if (String.Compare(replaceString, word, !matchCase) == 0)
              break;
          }
        }
        if (counter == maxReplacements)
          break;
      }
      return counter;
    }

    public void ReplaceSelection(string replaceString) {
      Interop.IHTMLSelectionObject selectionObject = this.GetActiveDocument(false).GetSelection();
      bool IsTextSelection = false;
      if (selectionObject != null) {
        IsTextSelection = selectionObject.GetSelectionType().Equals("Text");
      }
      if (IsTextSelection) {
        // the last selection, made by Find, will give the starting point defined by the range
        Interop.IHTMLTxtRange textRange;
        textRange = selectionObject.CreateRange() as Interop.IHTMLTxtRange;
        if (textRange != null) {
          textRange.SetText(replaceString);
        }
      }
    }

    public void ReplaceNext(string searchString, string replaceString, bool matchCase, bool wholeWord, bool backwards) {
      bool found;
      Interop.IHTMLTxtRange range;
      found = FindForReplace(searchString, matchCase, wholeWord, backwards, out range);
      if (found && range != null) {
        int i = (!matchCase ? 0 : 4) | (!wholeWord ? 0 : 2);
        int j = !backwards ? 10000000 : -10000000;
        if (range.FindText(searchString, j, i)) {
          range.SetText(replaceString);
        }
      }
    }

    private bool FindForReplace(string searchString, bool matchCase, bool wholeWord, bool searchUp, out Interop.IHTMLTxtRange range) {
      range = null;
      if (searchString == null)
        return false;
      Interop.IHTMLSelectionObject selectionObject = this.GetActiveDocument(false).GetSelection();
      bool IsTextSelection = false;
      if (selectionObject != null) {
        IsTextSelection = selectionObject.GetSelectionType().Equals("Text");
      }
      Interop.IHTMLTxtRange textRange = null;
      if (IsTextSelection) {
        // the last selection, made by Find, will give the starting point defined by the range
        textRange = selectionObject.CreateRange() as Interop.IHTMLTxtRange;
      }
      Interop.IHtmlBodyElement bodyElement = this.GetBodyThreadSafe(false);
      if (textRange == null) {
        // no range, create a range spanning the whole document
        IsTextSelection = false;
        textRange = bodyElement.createTextRange();
      }
      if (searchUp) {
        if (IsTextSelection) {
          textRange.MoveEnd("character", -1);
        }
        for (int i1 = 1; i1 == 1; i1 = textRange.MoveStart("textedit", -1)) {
        }
      } else {
        if (IsTextSelection) {
          textRange.MoveStart("character", 1);
        }
        for (int j1 = 1; j1 == 1; j1 = textRange.MoveEnd("textedit", 1)) {
        }
      }
      int searchConditionFlag = (!matchCase ? 0 : 4) | (!wholeWord ? 0 : 2);
      int i2 = !searchUp ? 10000000 : -10000000;
      bool FoundString = textRange.FindText(searchString, i2, searchConditionFlag);
      if (FoundString) {
        textRange.Select();
        textRange.ScrollIntoView(true);
        range = textRange.Duplicate();
        return true;
      }
      if (IsTextSelection) {
        textRange = selectionObject.CreateRange() as Interop.IHTMLTxtRange;
        if (searchUp) {
          if (textRange != null) {
            textRange.MoveStart("character", 1);
            for (int j2 = 1; j2 == 1; j2 = textRange.MoveEnd("textedit", 1)) {
            }
          }
        } else {
          if (textRange != null) {
            textRange.MoveEnd("character", -1);
            for (int k2 = 1; k2 == 1; k2 = textRange.MoveStart("textedit", -1)) {
            }
          }
        }
        if (textRange != null)
          FoundString = textRange.FindText(searchString, i2, searchConditionFlag);
        if (FoundString) {
          textRange.Select();
          textRange.ScrollIntoView(true);
          range = textRange.Duplicate();
          return true;
        }
      }
      return false;
    }


    /// <overloads/>
    /// <summary>
    /// Searches for a text string in the whole document.
    /// </summary>
    /// <remarks>
    /// Searches document for a given string and, if the string was found, returns true. The search stops at the first 
    /// hit and starts from the last stop. Subsequent calls jump from hit to hit. The document will automatically 
    /// scroll to the marked position.
    /// <para>The method does not stop at end. After the end of document has been reached the method returns to the document 
    /// start. See other overloads for information about the <c>StopAtEnd</c> parameter.</para>
    /// <para>The method uses the whole document as search range. To move the range a specific number of characters see
    /// other overloads.</para>
    /// </remarks>
    /// <example>
    /// The following code assumes that you have a dialog created which provides the following properties:
    /// <list type="bullet">
    /// <item>
    ///     <term>WithReplace</term>
    ///     <description>A boolean value (supported by a checkbox) which turn replacement on.</description>
    /// </item>
    /// <item>
    ///     <term>Search</term>
    ///     <description>A string value (supported by a textbox) which returns the phrase we're searching for.</description>
    /// </item>
    /// <item>
    ///     <term>Replace</term>
    ///     <description>A string value (supported by a textbox) which returns the characters we use for replacement.</description>
    /// </item>
    /// <item>
    ///     <term>Match</term>
    ///     <description>A boolean value (supported by a checkbox) which indicates that the search is case sensitive.</description>
    /// </item>
    /// <item>
    ///     <term>Word</term>
    ///     <description>A boolean value (supported by a checkbox) which delimites the search to whole words.</description>
    /// </item>
    /// <item>
    ///     <term>Up</term>
    ///     <description>A boolean value (supported by a checkbox). The search runs upwards if turned on.</description>
    /// </item>
    /// </list>
    /// <code>
    /// if (SearchReplaceDialog.ShowDialog() == DialogResult.OK)
    /// {        
    ///     if (SearchReplaceDialog.WithReplace)
    ///     {
    ///         int c = this.htmlEditor1.Replace(SearchReplaceDialog.Search, SearchReplaceDialog.Replace, SearchReplaceDialog.Match, SearchReplaceDialog.Word, SearchReplaceDialog.Up);
    ///         MessageBox.Show(c.ToString() + " strings found and replaced");
    ///     } 
    ///     else 
    ///     {
    ///         bool r = this.htmlEditor1.Find(SearchReplaceDialog.Search, SearchReplaceDialog.Match, SearchReplaceDialog.Word, SearchReplaceDialog.Up);
    ///         if (!r)
    ///         {
    ///             MessageBox.Show("String '"+SearchReplaceDialog.Search+"' not found");
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <param name="searchString">A string to search for.</param>
    /// <param name="matchCase">Must be set to <c>true</c> if match case is required.</param>
    /// <param name="wholeWord">Must set to <c>true</c> if only whole words should be found.</param>
    /// <param name="searchUp">Must set to <c>true</c> to search backwards.</param>
    /// <returns>Returns <c>true</c> if something was found, <c>false</c> otherwise.</returns>
    public bool Find(string searchString, bool matchCase, bool wholeWord, bool searchUp) {
      return Find(searchString, matchCase, wholeWord, searchUp, false, 0);
    }

    /// <overloads/>
    /// <summary>
    /// Searches for a text string in the whole document.
    /// </summary>
    /// <remarks>
    /// Searches document for a given string and, if the string was found, returns true. The search stops at the first 
    /// hit and starts from the last stop. Subsequent calls jump from hit to hit. The document will automatically 
    /// scroll to the marked position.
    /// </remarks>
    /// <example>
    /// The following code assumes that you have a dialog created which provides the following properties:
    /// <list type="bullet">
    /// <item>
    ///     <term>WithReplace</term>
    ///     <description>A boolean value (supported by a checkbox) which turn replacement on.</description>
    /// </item>
    /// <item>
    ///     <term>Search</term>
    ///     <description>A string value (supported by a textbox) which returns the phrase we're searching for.</description>
    /// </item>
    /// <item>
    ///     <term>Replace</term>
    ///     <description>A string value (supported by a textbox) which returns the characters we use for replacement.</description>
    /// </item>
    /// <item>
    ///     <term>Match</term>
    ///     <description>A boolean value (supported by a checkbox) which indicates that the search is case sensitive.</description>
    /// </item>
    /// <item>
    ///     <term>Word</term>
    ///     <description>A boolean value (supported by a checkbox) which delimites the search to whole words.</description>
    /// </item>
    /// <item>
    ///     <term>Up</term>
    ///     <description>A boolean value (supported by a checkbox). The search runs upwards if turned on.</description>
    /// </item>
    /// </list>
    /// <code>
    /// if (SearchReplaceDialog.ShowDialog() == DialogResult.OK)
    /// {        
    ///     if (SearchReplaceDialog.WithReplace)
    ///     {
    ///         int c = this.htmlEditor1.Replace(SearchReplaceDialog.Search, SearchReplaceDialog.Replace, SearchReplaceDialog.Match, SearchReplaceDialog.Word, SearchReplaceDialog.Up);
    ///         MessageBox.Show(c.ToString() + " strings found and replaced");
    ///     } 
    ///     else 
    ///     {
    ///         bool r = this.htmlEditor1.Find(SearchReplaceDialog.Search, SearchReplaceDialog.Match, SearchReplaceDialog.Word, SearchReplaceDialog.Up);
    ///         if (!r)
    ///         {
    ///             MessageBox.Show("String '"+SearchReplaceDialog.Search+"' not found");
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <param name="searchString">A string to search for.</param>
    /// <param name="matchCase">Must be set to <c>true</c> if match case is required.</param>
    /// <param name="wholeWord">Must set to <c>true</c> if only whole words should be found.</param>
    /// <param name="searchUp">Must set to <c>true</c> to search backwards.</param>
    /// <param name="stopAtEnd">By default the Find method "loops", e.g. it doesn't stop after reaching the end of document. If set to <c>true</c> the method stops at end.</param>
    /// <returns>Returns <c>true</c> if something was found, <c>false</c> otherwise.</returns>
    public bool Find(string searchString, bool matchCase, bool wholeWord, bool searchUp, bool stopAtEnd) {
      return Find(searchString, matchCase, wholeWord, searchUp, stopAtEnd, 0);
    }

    /// <summary>
    /// Searches for a text string in the whole document.
    /// </summary>
    /// <remarks>
    /// Searches document for a given string and, if the string was found, returns true. The search stops at the first 
    /// hit and starts from the last stop. Subsequent calls jump from hit to hit. The document will automatically 
    /// scroll to the marked position.
    /// </remarks>
    /// <example>
    /// The following code assumes that you have a dialog created which provides the following properties:
    /// <list type="bullet">
    /// <item>
    ///     <term>WithReplace</term>
    ///     <description>A boolean value (supported by a checkbox) which turn replacement on.</description>
    /// </item>
    /// <item>
    ///     <term>Search</term>
    ///     <description>A string value (supported by a textbox) which returns the phrase we're searching for.</description>
    /// </item>
    /// <item>
    ///     <term>Replace</term>
    ///     <description>A string value (supported by a textbox) which returns the characters we use for replacement.</description>
    /// </item>
    /// <item>
    ///     <term>Match</term>
    ///     <description>A boolean value (supported by a checkbox) which indicates that the search is case sensitive.</description>
    /// </item>
    /// <item>
    ///     <term>Word</term>
    ///     <description>A boolean value (supported by a checkbox) which delimites the search to whole words.</description>
    /// </item>
    /// <item>
    ///     <term>Up</term>
    ///     <description>A boolean value (supported by a checkbox). The search runs upwards if turned on.</description>
    /// </item>
    /// </list>
    /// <code>
    /// if (SearchReplaceDialog.ShowDialog() == DialogResult.OK)
    /// {        
    ///     if (SearchReplaceDialog.WithReplace)
    ///     {
    ///         int c = this.htmlEditor1.Replace(SearchReplaceDialog.Search, SearchReplaceDialog.Replace, SearchReplaceDialog.Match, SearchReplaceDialog.Word, SearchReplaceDialog.Up);
    ///         MessageBox.Show(c.ToString() + " strings found and replaced");
    ///     } 
    ///     else 
    ///     {
    ///         bool r = this.htmlEditor1.Find(SearchReplaceDialog.Search, SearchReplaceDialog.Match, SearchReplaceDialog.Word, SearchReplaceDialog.Up);
    ///         if (!r)
    ///         {
    ///             MessageBox.Show("String '"+SearchReplaceDialog.Search+"' not found");
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <param name="searchString">A string to search for.</param>
    /// <param name="matchCase">Must be set to <c>true</c> if match case is required.</param>
    /// <param name="wholeWord">Must set to <c>true</c> if only whole words should be found.</param>
    /// <param name="searchUp">Must set to <c>true</c> to search backwards.</param>
    /// <param name="stopAtEnd">If set to <c>true</c> the process stops if the end of document is reached.</param>
    /// <param name="startPosition">Number of characters the search range is supposed to move before Find starts. Default is 0 (no move).</param>
    /// <returns>Returns <c>true</c> if something was found, <c>false</c> otherwise.</returns>
    public bool Find(string searchString, bool matchCase, bool wholeWord, bool searchUp, bool stopAtEnd, int startPosition) {
      if (String.IsNullOrEmpty(searchString))
        return false;
      Interop.IHTMLSelectionObject selectionObject = this.GetActiveDocument(false).GetSelection();
      bool IsTextSelection = false;
      if (selectionObject != null) {
        IsTextSelection = selectionObject.GetSelectionType().Equals("Text");
      }
      Interop.IHTMLTxtRange textRange = null;
      if (IsTextSelection) {
        // the last selection, made by Find, will give the starting point defined by the range
        textRange = selectionObject.CreateRange() as Interop.IHTMLTxtRange;
      }
      Interop.IHtmlBodyElement bodyElement = this.GetBodyThreadSafe(false);
      if (textRange == null) {
        // no range, create a range spanning the whole document
        IsTextSelection = false;
        textRange = bodyElement.createTextRange();
        if (startPosition != 0) {
          textRange.Move("character", startPosition);
        }
      }
      if (searchUp) {
        if (IsTextSelection) {
          textRange.MoveEnd("character", -1);
        }
        for (int i1 = 1; i1 == 1; i1 = textRange.MoveStart("textedit", -1)) {
        }
      } else {
        if (IsTextSelection) {
          textRange.MoveStart("character", 1);
        }
        for (int j1 = 1; j1 == 1; j1 = textRange.MoveEnd("textedit", 1)) {
        }
      }
      int searchConditionFlag = (!matchCase ? 0 : 4) | (!wholeWord ? 0 : 2);
      int i2 = !searchUp ? 10000000 : -10000000;
      bool FoundString = textRange.FindText(searchString, i2, searchConditionFlag);
      // Stop at end and return false            
      //if (FindCheckEndPoint(textRange)) return false;
      // Do selection
      if (FoundString) {
        textRange.Select();
        textRange.ScrollIntoView(true);
        FindCheckEndPoint(textRange, true);
        return true;
      } else {
        if (stopAtEnd) {
          return false;
        }
      }
      if (IsTextSelection) {
        textRange = selectionObject.CreateRange() as Interop.IHTMLTxtRange;
        if (textRange != null) {
          if (searchUp) {
            textRange.MoveStart("character", 1);
            for (int j2 = 1; j2 == 1; j2 = textRange.MoveEnd("textedit", 1)) {
            }
          } else {
            textRange.MoveEnd("character", -1);
            for (int k2 = 1; k2 == 1; k2 = textRange.MoveStart("textedit", -1)) {
            }
          }
          FoundString = textRange.FindText(searchString, i2, searchConditionFlag);
          if (FoundString) {
            textRange.Select();
            textRange.ScrollIntoView(true);
            FindCheckEndPoint(textRange, true);
            return true;
          }
        }
      }
      FindCheckEndPoint(textRange, false);
      return false;
    }

    private Interop.IHTMLTxtRange _lastFindRange;

    /// <summary>
    /// Checks for reaching the document end compared with the current find range.
    /// </summary>
    /// <param name="currentRange">Current Range</param>
    /// <param name="foundString">Hint that the string was found.</param>
    /// <returns>Returns <c>true</c> when the end point was reached.</returns>
    private bool FindCheckEndPoint(Interop.IHTMLTxtRange currentRange, bool foundString) {
      if (_lastFindRange == null) {
        _lastFindRange = GetBodyThreadSafe(false).createTextRange();
      }
      //lastFindRange = currentRange.Duplicate();

      // Interop.IHTMLTxtRange bodyRange = GetBodyThreadSafe(false).createTextRange();

      //            System.Diagnostics.Debug.Write(currentRange.CompareEndPoints("StartToStart", lastFindRange), " SS ");
      //            System.Diagnostics.Debug.Write(currentRange.CompareEndPoints("EndToStart", lastFindRange), " ES ");
      //            System.Diagnostics.Debug.Write(currentRange.CompareEndPoints("StartToEnd", lastFindRange), " SE ");
      //            System.Diagnostics.Debug.WriteLine(currentRange.CompareEndPoints("EndToEnd", lastFindRange), " EE ");
      //            System.Diagnostics.Debug.Write(currentRange.CompareEndPoints("StartToStart", bodyRange), " B SS ");
      //            System.Diagnostics.Debug.Write(currentRange.CompareEndPoints("EndToStart", bodyRange), " B ES ");
      //            System.Diagnostics.Debug.Write(currentRange.CompareEndPoints("StartToEnd", bodyRange), " B SE ");
      //            System.Diagnostics.Debug.WriteLine(currentRange.CompareEndPoints("EndToEnd", bodyRange), " B EE ");

      if (currentRange.CompareEndPoints("StartToStart", _lastFindRange) <= 0) {
        if (!foundString && currentRange.CompareEndPoints("StartToStart", _lastFindRange) != 0) {
          currentRange.Collapse(true);
          this.Exec(Interop.IDM.CLEARSELECTION);
        }
        InvokeFindHasReachedEnd();
        _lastFindRange = null;
        return true;
      }
      _lastFindRange = currentRange;
      return false;
    }


    #endregion

    #region +++++ Block: Load, Save and Authentication

    /// <summary>
    /// Reset the dirty state flag to inform the control that the current state is
    /// used as unchanged state.</summary>
    /// <remarks>This may result in removing the Undo history under some circumstances.
    /// </remarks>
    public void ClearDirtyState() {
      if (!ThrowDocumentNotReadyException())
        return;
      Exec(Interop.IDM.SETDIRTY, false);
    }

    /// <summary>
    /// Sets username and password for silent authentication on protected sites.
    /// </summary>
    /// <remarks>
    /// If these parameters are set the control tries to authenticate using these
    /// values if a site requests authentication. This in fact suppresses the 
    /// typical authentication dialog. After changing the values it is necessary to
    /// call LoadHtml or LoadUrl again.
    /// </remarks>
    /// <param name="userName">The username the control should send on request.</param>
    /// <param name="passWord">The password the control should send on request.</param>
    public void SetAuthentication(string userName, string passWord) {
      this._userName = userName;
      this._passWord = passWord;
    }

    [Browsable(false)]
    internal string UserName { get { return _userName; } }
    [Browsable(false)]
    internal string Password { get { return _passWord; } }

    private string userAgent;

    /// <summary>
    /// User Agent string used to identify the control when browsing the web.
    /// </summary>
    [Browsable(true), Category("Netrix Component"), Description("User Agent string used to identify the control when browsing the web.")]
    public string UserAgent
    {
      get
      {
        return userAgent;
      }
      set
      {
        userAgent = value;
      }
    }

    private WebProxy webProxy;

    /// <summary>
    /// Gets or sets the proxy used to support download beyond a proxy.
    /// </summary>
    /// <remarks>
    /// Set this property to <c>null</c> if no proxy is required. If the internal loader is being used the 
    /// proxy settings of IE or operating system are bypassed. Restrictive security settings may make 
    /// the operation fail, though.
    /// </remarks>
    [Browsable(false)]
    public WebProxy Proxy
    {
      get
      {
        return webProxy;
      }
      set
      {
        webProxy = value;
      }
    }

    /// <summary>
    /// Called if an exception occured during loading resources from web.
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="url"></param>
    protected internal void OnWebException(WebException ex, ref string url) {
      if (WebException != null) {
        WebExceptionEventArgs args = new WebExceptionEventArgs(ex, url);
        WebException(this, args);
        url = args.Url;
      }
    }

    /// <overloads>There are two overloads of this method.</overloads>
    /// <summary>
    /// This method resets the current credentials applied to the control.
    /// </summary>
    /// <remarks>
    /// After successfull authentication to a secure site the control will reuse the 
    /// credentials on any further access. To avoid this and to re-force the authentication,
    /// a call to this method is applicable. Calling this method will not reset the
    /// username and password set with <see cref="GuruComponents.Netrix.HtmlEditor.SetAuthentication">SetAuthentication</see>.
    /// </remarks>
    public void ClearAuthenticationCache() {
      this._activeDocument.ExecCommand("ClearAuthenticationCache", false, null);
    }


    /// <summary>
    /// This method resets the current credentials applied to the control and remove the username and password.
    /// </summary>
    /// <remarks>
    /// After successfull authentication to a secure site the control will reuse the 
    /// credentials on any further access. To avoid this and to re-force the authentication,
    /// a call to this method is applicable. Calling this method will clear the
    /// username and password set with <see cref="GuruComponents.Netrix.HtmlEditor.SetAuthentication">SetAuthentication</see>.
    /// Further calls to a protected site will always fail if no new username and password beeing set before.
    /// </remarks>
    public void ClearAuthenticationCache(bool reset) {
      ClearAuthenticationCache();
      this._userName = null;
      this._passWord = null;
    }

    /// <summary>
    /// A helper method used to save uncomplete documents. If the host application
    /// needs the body content it is recommended to grab for the Body element and
    /// retrieve the InnerText or InnerHtml property.
    /// </summary>
    /// <param name="bodyElement"></param>
    /// <returns></returns>
    protected virtual string SavePartialHtml(Element bodyElement) {
      return bodyElement.InnerHtml;
    }

    /// <overloads>This method has two overloads.</overloads>
    /// <summary>
    /// Retrieves the HTML contained in control to a string and return it.
    /// </summary>
    /// <remarks>
    /// The content is unchanged "as is" the MSHTML control provides it. 
    /// <para>
    /// This method returns always the content of the base document. In case of a framed document this is the
    /// document containing the frameset definitions. The host application must use the HtmlFrameSet and 
    /// related objects to call the save methods there to save the content of each frame document separatly.
    /// </para>
    /// </remarks>
    /// <returns>The HTML in the control</returns>
    public string GetRawHtml() {
      return GetRawHtml(false);
    }

    /// <summary>
    /// Retrieves the HTML contained in control to a string and return it.
    /// </summary>
    /// <remarks>
    /// The content is unchanged "as is" the MSHTML control provides it. 
    /// <para>
    /// This method returns always the content of the base document. In case of a framed document this is the
    /// document containing the frameset definitions. The host application must use the HtmlFrameSet and 
    /// related objects to call the save methods there to save the content of each frame document separatly.
    /// </para>
    /// </remarks>
    /// <param name="ClearDirty">If set to true the call of this method will reset the <see cref="GuruComponents.Netrix.HtmlEditor.IsDirty">IsDirty</see> flag.</param>
    /// <returns>The HTML in the control</returns>
    public string GetRawHtml(bool ClearDirty) {
      return GetRawHtml(ClearDirty, false);
    }

    /// <summary>
    /// Retrieves the HTML contained in control to a string and return it.
    /// </summary>
    /// <remarks>
    /// The content is unchanged "as is" the MSHTML control provides it. 
    /// <para>
    /// This method returns always the content of the base document. In case of a framed document this is the
    /// document containing the frameset definitions. The host application must use the HtmlFrameSet and 
    /// related objects to call the save methods there to save the content of each frame document separatly.
    /// </para>
    /// </remarks>
    /// <param name="ClearDirty">If set to true the call of this method will reset the <see cref="GuruComponents.Netrix.HtmlEditor.IsDirty">IsDirty</see> flag.</param>
    /// <param name="fromActiveFrame">If set to <c>true</c>, the currently active frame is the source, otherwise the frame definition document. This parameter is ignored, if the document has no frames.</param>
    /// <returns>The HTML in the control</returns>
    public string GetRawHtml(bool ClearDirty, bool fromActiveFrame) {
      if (!IsCreated) {
        throw new HtmlControlException("HtmlEditor.GetRawHtml: Document not created");
      }
      string content = String.Empty;
      Encoding tmpEnc = Encoding;
      try {
        OnSaving();
        if (_fullDocumentMode) {
          Interop.IPersistStreamInit persistStream;
          IStream uStream;
          persistStream = (Interop.IPersistStreamInit)this.GetActiveDocument(!fromActiveFrame);
          // Use COM streams to get content
          Win32.CreateStreamOnHGlobal(Interop.NullIntPtr, true, out uStream);
          persistStream.Save(uStream, ClearDirty ? 1 : 0);
          STATSTG iStatStg;
          uStream.Stat(out iStatStg, 1);
          int i = (int)iStatStg.cbSize;
          byte[] bs = new byte[(uint)i];
          IntPtr j;
          Win32.GetHGlobalFromStream(uStream, out j);
          IntPtr k = Win32.GlobalLock(j);
          if (k != Interop.NullIntPtr) {
            Marshal.Copy(k, bs, 0, i);
            Win32.GlobalUnlock(j);

            int index = 0;
            int blocks = (int)Math.Floor((decimal)bs.Length / 8192);
            Encoding _enc;
            while (true) {
              if (bs[0] == Encoding.Unicode.GetPreamble()[0]
                  &&
                  bs[1] == Encoding.Unicode.GetPreamble()[1]) {
                _enc = Encoding.Unicode;
                break;
              }
              if (bs[0] == Encoding.UTF8.GetPreamble()[0]
                  &&
                  bs[1] == Encoding.UTF8.GetPreamble()[1]
                  &&
                  bs[2] == Encoding.UTF8.GetPreamble()[2]) {
                _enc = Encoding.UTF8;
                break;
              }
              if (bs[0] == Encoding.BigEndianUnicode.GetPreamble()[0]
                  &&
                  bs[1] == Encoding.BigEndianUnicode.GetPreamble()[1]) {
                _enc = Encoding.BigEndianUnicode;
                break;
              }
              _enc = this.Encoding;
              break;
            }
            while (index <= blocks) {
              content += _enc.GetString(bs, index * 8192, (index < blocks) ? 8192 : bs.Length - blocks * 8192);
              if ((byte)content[0] == 255) {
                content = content.Substring(1);
              }
              index++;
            }

            // HACK: A very dirty hack to remove the double http-equiv we get here; I've no idea what the reason is..
            int i1 = 0;
            while ((i1 = content.IndexOf(String.Concat(Environment.NewLine, "http-equiv"), i1)) > 0) {
              int i2 = content.IndexOf("\"", i1 + 14);
              if (i1 > 0 && i2 > 0) {
                content = String.Concat(content.Substring(0, i1), content.Substring(++i2));
              }
              i1 = i2;
            }
          }
          // Remove internally set temp file support base tag 
          Regex rx = new Regex(@"<BASE\s+href=""?(?<href>[^"">]*)""?/?>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
          Match baseTag;
          if ((baseTag = rx.Match(content)).Success && baseTag.Groups["href"].Value.Equals(TempPath)) {
            content = rx.Replace(content, "");
          }
        } else {
          Interop.IHTMLElement body = this.GetBodyThreadSafe(false) as Interop.IHTMLElement;
          if (body != null) {
            content = body.GetInnerHTML();
          }
        }
        if (ClearDirty) {
          // TODO: Implement!
        }
      } catch {
        content = String.Empty;
      } finally {
        OnSaved();
        Encoding = tmpEnc;
      }
      if (content == null) {
        content = String.Empty;
      }
      return content;
    }

    /// <summary>
    /// Returns a well formatted representation of the current document.
    /// </summary>
    /// <remarks>Uses the default formatter, see <see cref="HtmlFormatterOptions"/>. This formatter is set to single space as indent
    /// char, 4 spaces indent size, 128 characters line length, and produces XHTML compliant code. Once the <see cref="HtmlFormatterOptions"/>
    /// property is set for the very first time the defaults are no longer valid and any further call will use the custom settings.
    /// </remarks>
    /// <exception cref="System.ArgumentException">Thrown if either the indent size or max line length is less than 0.</exception>
    /// <returns>Returns the formatted string.</returns>
    public string GetFormattedHtml() {
      if (HtmlFormatterOptions == null) {
        // set defaults
        HtmlFormatterOptions = new HtmlFormatterOptions(' ', 4, 128, true);
      }
      return GetFormattedHtml(HtmlFormatterOptions, false);
    }

    /// <summary>
    /// Returns a well formatted representation of the current document.
    /// </summary>
    /// <param name="fo"><see cref="IHtmlFormatterOptions"/> for formatting this streams content.</param>
    /// <exception cref="System.ArgumentException">Thrown if either the indent size or max line length is less than 0.</exception>
    /// <returns>Returns the formatted string.</returns>
    public string GetFormattedHtml(IHtmlFormatterOptions fo) {
      return GetFormattedHtml(fo, false);
    }

    /// <summary>
    /// Returns a well formatted representation of the current document.
    /// </summary>
    /// <param name="fo"><see cref="IHtmlFormatterOptions"/> for formatting this streams content.</param>
    /// <param name="ClearDirty">Resets the dirty flag.</param>
    /// <exception cref="System.ArgumentException">Thrown if either the indent size or max line length is less than 0.</exception>
    /// <returns>Returns the formatted string.</returns>
    public string GetFormattedHtml(IHtmlFormatterOptions fo, bool ClearDirty) {
      return GetFormattedHtml(fo, ClearDirty, false);
    }

    /// <summary>
    /// Returns a well formatted representation of the current document.
    /// </summary>
    /// <param name="fo">See <see cref="IHtmlFormatterOptions"/> for formatting this streams content.</param>
    /// <param name="ClearDirty">If set to true the call of this method will reset the <see cref="GuruComponents.Netrix.HtmlEditor.IsDirty">IsDirty</see> flag.</param>
    /// <param name="fromActiveFrame">For framed documents loaded at once, this parameter can be used to get the content of the current (active) frame instead the base document.</param>
    /// <exception cref="System.ArgumentException">Thrown if either the indent size or max line length is less than 0.</exception>
    /// <returns>Returns the formatted string.</returns>
    public string GetFormattedHtml(IHtmlFormatterOptions fo, bool ClearDirty, bool fromActiveFrame) {
      string content = GetRawHtml(ClearDirty, fromActiveFrame);
      if (fo.IndentSize < 0) {
        throw new ArgumentException("Indentsize must be greater or equal than 0", "IndentSize in HtmlFormatterOptions");
      }
      if (fo.MaxLineLength < 1) {
        throw new ArgumentException("MaxLineLenght must be greater than 0", "MaxLineLength in HtmlFormatterOptions");
      }
      StringBuilder sb = new StringBuilder();
      StringWriter target = new StringWriter(sb);
      Formatter.Format(content, target, fo);
      content = sb.ToString();
      target.Close();
      return content;
    }


    /// <summary>
    /// Saves the HTML contained in the control to a stream.
    /// </summary>
    /// <remarks>
    /// Throws an <exception cref="ArgumentNullException"/> if the parameter is <c>null</c>.
    /// Uses the global Encoding property for document encoding.
    /// </remarks>
    /// <param name="stream"></param>
    public void SaveHtml(Stream stream) {
      if (stream == null) {
        throw new ArgumentNullException("SaveHtml : Must specify a non-null stream to which to save");
      }
      string content;
      content = GetRawHtml(true);
      Save(stream, content);
    }

    /// <summary>
    /// Saves the HTML contained in the control to a stream.
    /// </summary>
    /// <remarks>
    /// Throws an <exception cref="ArgumentNullException"/> if the parameter is <c>null</c>.
    /// Uses the global Encoding property for document encoding.
    /// Another method uses the <see cref="GuruComponents.Netrix.HtmlEditor.GetRawHtml()">GetRawHtml</see> or <see cref="GuruComponents.Netrix.HtmlEditor.GetFormattedHtml()">GetFormattedHtml</see> 
    /// methods, which return content of the editor as string. It is up to the host application to use the common .NET file classes to save the content.
    /// The class <see cref="GuruComponents.Netrix.HtmlFormatting.HtmlFormatterOptions">HtmlFormatterOptions</see> provides a way to control the beautifier and formatter. The various properties are:
    /// <list type="bullet">
    ///     <item>IndentChar</item>
    ///     <item>IndentSize</item>
    ///     <item>MaxLineLength</item>
    ///     <item>AsXhtml</item>
    ///     <item>ElementCasing</item>
    ///     <item>AttributeCasing</item>
    /// </list>
    /// <para>
    /// Note: Both save methods throw an <see cref="System.ArgumentNullException">ArgumentNullException</see> if the stream parameter is null. If it is possible that the stream is null under some circumstances, we should put a try/catch block around the call.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.HtmlFormatting.HtmlFormatterOptions">HtmlFormatterOptions</seealso>
    /// </remarks>       
    /// <exception cref="System.ArgumentNullException">Fired if stream parameter is <c>null</c>.</exception>
    /// <example>
    /// Assuming a menu exists and a menu item fires a click event, the following event handler will save the content of the base document to file in well formatted XHTML compatible format.
    /// <code>
    /// private void menuItem_Click(object sender, System.EventArgs e)
    /// {
    ///     // Build Formatting Options in variable fo here
    ///     if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
    ///     {
    ///         string fileName = this.saveFileDialog1.FileName;
    ///         FileStream fs = new FileStream(fileName, FileMode.Create);
    ///         this.htmlEditor1.SaveFormattedHtml(fs, fo);
    ///         fs.Close();
    ///     }
    /// }
    /// </code>
    /// </example> 
    /// <param name="stream"></param>
    /// <param name="fo"> for formatting this streams content.</param>
    public void SaveFormattedHtml(Stream stream, IHtmlFormatterOptions fo) {
      if (stream == null) {
        throw new ArgumentNullException("SaveHtml : Must specify a non-null stream to which to save");
      }
      string content;
      content = GetFormattedHtml(fo, true);
      Save(stream, content);
    }

    /// <summary>
    /// Saves the HTML contained in the control to a stream using the default formatter.
    /// </summary>
    /// <seealso cref="HtmlFormatterOptions"/>
    /// <param name="stream"></param>
    public void SaveFormattedHtml(Stream stream) {
      SaveFormattedHtml(stream, HtmlFormatterOptions);
    }

    private void Save(Stream stream, string content) {
      try {
        StreamWriter writer = new StreamWriter(stream, this.Encoding);
        writer.Write(content);
        writer.Flush();
        writer.Close();
        if (stream is FileStream) {
          Url = ((FileStream)stream).Name;
        }
      } catch {
        throw new ArgumentException("SaveHtml : Cannot save to the given location.");
      } finally {
        stream.Close();
      }
    }

    /// <overloads>This method has three overloads.</overloads>
    /// <summary>
    /// Saves the previously loaded content as MHTML stream.
    /// </summary>
    /// <remarks>
    /// If the file does not exist the method will create it. If the file exists the method will overwrite
    /// it. 
    /// <para>
    /// The main idea of the MHTML standard is that you send a HTML document, together with in-line graphics, 
    /// applets, etc., and also other linked documents if you so wish, in a MIME multipart/related body part.
    /// The content of the HTML part is coded in quoted printable format. The images and binary objects are
    /// coded in Base64, always chunked in segments to 76 characters per line.
    /// </para>
    /// <para>
    /// The IE can save and view MHTML. The preferred extension is MHT. NetRix works similiar to the basic
    /// MHTML save routine, but does not use the internal interfaces. 
    /// </para>
    /// <para>
    /// <b>Attention:</b> The method returns the content as it if was loaded the first time. ANY CHANGE DURING DESIGNTIME IS
    /// NOT RECOGNIZED. To save the current content the host application MUST save and reload the content AND
    /// wait for the next ReadyStateComplete event before this method can work properly.
    /// </para>
    /// <para>
    /// <b>Know  Issues:</b> This method is EXPERIMENTAL. It does not save frames properly and it does not handle
    /// local absolute paths correctly.
    /// </para>
    /// </remarks>
    /// <param name="fileName">The full path the content is saved into.</param>
    public void SaveMht(string fileName) {
      if (this.MhtBuilder != null) {
        string mht = this.MhtBuilder.ToString();
        StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding(1252));
        sw.Write(mht);
        sw.Close();
      }
    }

    /// <summary>
    /// Saves the previously loaded content as MHTML stream.
    /// </summary>
    /// <remarks>
    /// If the file does not exist the method will create it. If the file exists the method will overwrite
    /// it. 
    /// <para>
    /// The main idea of the MHTML standard is that you send a HTML document, together with in-line graphics, 
    /// applets, etc., and also other linked documents if you so wish, in a MIME multipart/related body part.
    /// The content of the HTML part is coded in quoted printable format. The images and binary objects are
    /// coded in Base64, always chunked in segments to 76 characters per line.
    /// </para>
    /// <para>
    /// The IE can save and view MHTML. The preferred extension is MHT. NetRix works similiar to the basic
    /// MHTML save routine, but does not use the internal interfaces. 
    /// </para>
    /// <para>
    /// The method fails if the document is not ready yet.
    /// </para>
    /// <para>
    /// <b>Attention:</b> The method returns the content as it if was loaded the first time. ANY CHANGE DURING DESIGNTIME IS
    /// NOT RECOGNIZED. To save the current content the host application MUST save and reload the content AND
    /// wait for the next ReadyStateComplete event before this method can work properly.
    /// </para>
    /// <para>
    /// <b>Know  Issues:</b> This method is EXPERIMENTAL. It does not save frames properly and it does not handle
    /// local absolute paths correctly.
    /// </para>
    /// </remarks>
    /// <param name="fileStream">A stream (primarily a FileStream) to which the content is saved.</param>
    public void SaveMht(Stream fileStream) {
      if (this.MhtBuilder != null) {
        string mht = this.MhtBuilder.ToString();
        fileStream.Seek(0, SeekOrigin.Begin);
        StreamWriter sw = new StreamWriter(fileStream, Encoding.GetEncoding(1252));
        sw.Write(mht);
        sw.Close();
      }
    }

    /// <summary>
    /// Saves the previously loaded content as MHTML string.
    /// </summary>
    /// <remarks>
    /// Be aware that the string contains the page and all embedded resources as one block of coded data.
    /// This is approximatly 33% more than the sum of all file sizes (due to the Base64 coding). A normal
    /// page can produce a huge string and the processing may take a while.
    /// <para>
    /// The main idea of the MHTML standard is that you send a HTML document, together with in-line graphics, 
    /// applets, etc., and also other linked documents if you so wish, in a MIME multipart/related body part.
    /// The content of the HTML part is coded in quoted printable format. The images and binary objects are
    /// coded in Base64, always chunked in segments to 76 characters per line.
    /// </para>
    /// <para>
    /// The IE can save and view MHTML. The preferred extension is MHT. NetRix works similiar to the basic
    /// MHTML save routine, but does not use the internal interfaces. 
    /// </para>
    /// <para>
    /// <b>Attention:</b> The method returns the content as it if was loaded the first time. ANY CHANGE DURING DESIGNTIME IS
    /// NOT RECOGNIZED. To save the current content the host application MUST save and reload the content AND
    /// wait for the next ReadyStateComplete event before this method can work properly.
    /// </para>
    /// </remarks>
    /// <seealso cref="CanSaveMht"/>
    /// <returns>The string to which the content was written or <c>null</c>, if there is no content or the document is not ready.</returns>
    public string SaveMht() {
      if (this.MhtBuilder != null) {
        string mht = this.MhtBuilder.ToString();
        return mht;
      } else {
        return null;
      }
    }


    private IElementDom _documentDom;

    /// <summary>
    /// Returns the DOM (document object model) of the current loaded document.
    /// </summary>
    /// <remarks>
    /// The property may return <c>null</c> (<c>Nothing</c> in Visual Basic) if the document is 
    /// either not ready or doesn't contain a HTML element.
    /// <para>
    /// After first call the reference to the object is being cached, so a direct and extensive access
    /// using this property does not cause a performance flaw. Any LoadXXX method will reset cache
    /// and the first subsequent call will recreate the new document structure. Changes to the
    /// document are reflected synchrounously.
    /// </para>
    /// </remarks>
    [Browsable(false)]
    public IElementDom DocumentDom
    {
      get
      {
        if (!ThrowDocumentNotReadyException())
          return null;
        if (_documentDom == null) {
          ElementCollection ec = this.GetElementsByTagName("html");
          if (ec.Count == 1) {
            Interop.IHTMLDOMNode html = ec[0].GetBaseElement() as Interop.IHTMLDOMNode;
            if (html != null) {
              _documentDom = new ElementDom(html, this);
            }
          }
        }
        return _documentDom;
      }
    }

    /// <overloads/>
    /// <summary>
    /// Returns the current document as <see cref="System.Xml.XmlDocument"/> formatted object.
    /// </summary>
    /// <remarks>
    /// This method should be
    /// covered by a try/catch structure because it rethrows internal exceptions occuring during the parse process.
    /// </remarks>
    /// <returns></returns>
    public XmlDocument GetXmlDocument() {
      return GetXmlDocument(false);
    }

    /// <overloads/>
    /// <summary>
    /// Returns the current document as <see cref="System.Xml.XmlDocument"/> formatted object.
    /// </summary>
    /// <remarks>
    /// This method should be
    /// covered by a try/catch structure because it rethrows internal exceptions occuring during the parse process.
    /// </remarks>
    /// <param name="AddXmlDeclaration">
    /// If <c>true</c> the module tries to add the &lt;?xml version="1.0" ?&gt; declaration in front of the document.
    /// </param>
    /// <returns></returns>
    public XmlDocument GetXmlDocument(bool AddXmlDeclaration) {
      return GetXmlDocument(AddXmlDeclaration, false);
    }

    /// <summary>
    /// Returns the current document as <see cref="System.Xml.XmlDocument"/> formatted object.
    /// </summary>
    /// <remarks>
    /// This method should be
    /// covered by a try/catch structure because it rethrows internal exceptions occuring during the parse process.
    /// </remarks>
    /// <param name="AddXmlDeclaration">
    /// If <c>true</c> the module tries to add the &lt;?xml version="1.0" ?&gt; declaration in front of the document.
    /// </param>
    /// <param name="fromActiveFrame"><c>True</c> if content is read from active frame, otherwise the content of the frame definition file is read.</param>
    /// <returns></returns>
    public XmlDocument GetXmlDocument(bool AddXmlDeclaration, bool fromActiveFrame) {
      XmlDocument xDoc = new XmlDocument();
      try {
        IHtmlFormatterOptions fo = new HtmlFormatterOptions(' ', 1, 2048, true);
        // TODO: Create configurable DOCTYPES here
        // <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
        // <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 
        // <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Frameset//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd">
        string dtd = @"";
        string coreDoc = this.GetFormattedHtml(fo, false, fromActiveFrame);
        if (!coreDoc.StartsWith("<?xml") && AddXmlDeclaration) {
          coreDoc = String.Concat(@"<?xml version=""1.0"" ?>", "\r\n", dtd, coreDoc);
        }
        xDoc.LoadXml(coreDoc);
      } catch (Exception ex) {
        throw new XmlException("The document cannot be formatted as XML. Error was : " + ex.Message, ex);
      }
      return xDoc;
    }


    /// <summary>
    /// This method replaces the content of the whole designer.
    /// </summary>
    /// <remarks>
    /// The relation to the previously loaded URL or filename is left untouched. The content is also
    /// beeing rewritten to the file and reloaded from that location. This assures that the paths to 
    /// relative embedded objects are still valid.
    /// <seealso cref="GuruComponents.Netrix.HtmlEditor.ReLoadHtml">ReLoadHtml</seealso>
    /// </remarks>
    /// <param name="content">Content which replaces the current content</param>
    public void ReLoadHtml(string content) {
      ReLoadUrl(content, true);
    }

    /// <overloads>This method has to overloads.</overloads>
    /// <summary>
    /// This method replaces the content of the whole designer.
    /// </summary>
    /// <remarks>
    /// This is the preferred methos to refresh the
    /// content. If the content was externally changed, it is necessary to save the content before the reload
    /// takes place. If saving is not done, the old content will replace all changes.
    /// <para>
    /// It is up to the host application to decide where to save the content. If the parameter <c>saveContent</c>
    /// is set to <c>true</c>, the component will save the content internally and overwrite the original file without
    /// further questions.
    /// </para>
    /// </remarks>
    /// <exception cref="System.IO.FileNotFoundException">Thrown if the path is not valid or not a file.</exception>
    /// <param name="content">Content which replaces the current content</param>
    /// <param name="saveContent">If <c>true</c>, the component will save the the current content to 
    /// file and reload from file then. If <c>false</c> the component will load the last not saved content from 
    /// the related URL. This means, that any unsaved content will be lost. There is no UNDO on this action available.</param>
    public void ReLoadUrl(string content, bool saveContent) {
      if (Url == null) {
        throw new ArgumentNullException("Cannot reload as document was never loaded.");
      }
      if (Url.StartsWith("http://")) {
        throw new ArgumentException("Remote documents must be saved locally before reloading.");
      }
      if (saveContent) {
        try {
          FileStream fs = new FileStream(Url, FileMode.Create, FileAccess.Write);
          StreamWriter sw = new StreamWriter(fs, this.Encoding);
          sw.Write(content);
          sw.Close();
          fs.Close();
        } catch (Exception ex) {
          throw new FileNotFoundException("Cannot access the file or local path " + Url + ".", ex);
        }
      }
      LoadUrl(Url);
    }

    /// <summary>
    /// This method simply refreshes the content to rebuild all relations, like linked stylesheets.
    /// </summary>
    /// <remarks>
    /// A call to this method can force the save operation on request. 
    /// </remarks>
    public void ReLoadUrl(bool SaveContent) {
      ReLoadUrl(this.GetRawHtml(), SaveContent);
    }

    /// <summary>
    /// If loading parts of HTML create a skeleton to get a full DOM.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    internal virtual string CreateHtmlContent(string content) {
      string doctype = "";
      if (AntiQuirks) {
        doctype = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">";
      }
      return String.Concat(doctype, @"  <html><body>", content, "</body></html>");
    }

    /// <summary>
    /// Loads the content of a local file into the control.
    /// </summary>
    /// <remarks>
    /// This method is simply a wrapper for <see cref="LoadUrl"/>. It checks the existence of the file on the local system
    /// and loads it if possible. 
    /// </remarks>
    /// <exception cref="FileNotFoundException">Fires this exception if the file was not found.</exception>
    /// <param name="fileName"></param>
    public void LoadFile(string fileName) {
      if (File.Exists(fileName)) {
        ResetDesiredProperties(false);
        Url = fileName;
        // Start loading
        OnLoading();
        try {
          this.IsFileBasedDocument = true;
          if (!_namespaceRegistered) {
            InternetSessionRegistry.UnregisterAll();
            InternetSessionRegistry.Register("file", new FileProtocolHandlerFactory(this));
            InternetSessionRegistry.RegisterMime("text/plain", "file", new InternetSessionFactory(this));
            _namespaceRegistered = true;
          }
          if (ServiceProvider.GetService(typeof(Interop.IAuthenticate)) != null) {
            ServiceProvider.RemoveService(typeof(Interop.IAuthenticate));
          }
          if (ServiceProvider.GetService(typeof(Interop.IHttpSecurity)) != null) {
            ServiceProvider.RemoveService(typeof(Interop.IHttpSecurity));
          }
          Interop.IOleObject ob = (Interop.IOleObject)MshtmlSite.MSHTMLDocument; //this.ActiveDocument;
          ob.SetClientSite((Interop.IOleClientSite)MshtmlSite);
          Interop.IPersistMoniker persistMoniker;
          Interop.IBindCtx bindContext;
          Interop.IMoniker moniker;
          persistMoniker = (Interop.IPersistMoniker)MshtmlSite.MSHTMLDocument; //this.ActiveDocument;
          Win32.CreateURLMoniker(null, "file:///" + Url, out moniker);
          Win32.CreateBindCtx(0, out bindContext);
          persistMoniker.Load(1, moniker, bindContext, (int)Interop.STGM.STGM_READ);
        } catch (Exception ex) {
          throw new FileLoadException("Cannot load from the given file.", fileName, ex);
        }
        OnLoaded();
      } else {
        throw new FileNotFoundException("File not found", fileName);
      }
    }

    /// <summary>
    /// Saves the content as a file to the location the content was previously loaded from.
    /// </summary>
    /// <remarks>
    /// The method checks whether the file already exists and tries to create the file if it does not exist. The name of
    /// the <see cref="Url"/> method is used as a file name. If the path does not point to a filesystem with write 
    /// permissions an exception occurs.
    /// </remarks>
    public void SaveFile() {
      if (File.Exists(Url)) {
        using (FileStream stream = new FileStream(Url, FileMode.Open, FileAccess.Write)) {
          this.Save(stream, this.GetRawHtml());
          stream.Close();
        }
      } else {
        SaveFile(Url);
      }
    }

    /// <summary>
    /// Save the current content into a file.
    /// </summary>
    /// <remarks>
    /// This method saves the "raw" content which consists of unformatted HTML. 
    /// This method requires Write access to the file. The file will be created if it doesn't exists.
    /// <para>
    /// The method checks whether the file already exists and tries to create the file if it does not exist. The name of
    /// the <see cref="Url"/> method is used as a file name. If the path does not point to a filesystem with write 
    /// permissions an exception occurs.
    /// </para>
    /// </remarks>
    /// <param name="fileName">The filename optionally including the full path.</param>
    public void SaveFile(string fileName) {
      using (FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)) {
        this.Save(stream, this.GetRawHtml());
        stream.Close();
      }
    }

    /// <summary>
    /// Saves the current "raw" content into a stream. The stream is not closed and remains open.
    /// </summary>
    /// <param name="stream">The stream the content is written into.</param>
    public void SaveFile(Stream stream) {
      this.Save(stream, this.GetRawHtml());
    }

    /// <summary>
    /// Loads HTML content from a URL or local path.
    /// </summary>
    /// <remarks>
    /// This is the preferred load method. It can take HTML content from the following sources:
    /// <list type="bullet">
    ///     <item>A locally stored file</item>
    ///     <item>An URL, with the leading "http://" (required)</item>
    /// </list>
    /// The method will fire the <see cref="GuruComponents.Netrix.HtmlEditor.Loading">Loading</see> before
    /// the load process starts, but not when the URL or filename was not accepted. This can be used to check
    /// the processing, e.g. if the event does not fire the URL is wrong or the filename was not found. 
    /// If the processing is done the <see cref="GuruComponents.Netrix.HtmlEditor.Loaded">Loaded</see> event
    /// will fired. Remember that the next editing command MUST wait until the
    /// <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> was fired, which
    /// takes a bit more time used by the control to render and display of the content.
    /// <para>
    /// Use <see cref="GuruComponents.Netrix.HtmlEditor.LoadHtml(string)">LoadHtml</see> to load memory based strings only.
    /// </para>
    /// <para>
    /// If MSHTML has not yet been created, the loading is postponed until MSHTML has been created.
    /// The method uses internally IMoniker to set the current base path to resolve relative paths
    /// in src and href attributes without changing the parameters.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example shows how to load a file into NetRix using a menu entry event handler. The
    /// code assumes that a main menu extists with a entry <c>menuItem</c> in it and a event handler
    /// <c>menuItem_Click</c> attached to the Click event.
    /// <code>
    /// private void menuItem_Click(object sender, System.EventArgs e)
    /// {
    ///    if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
    ///    {
    ///       string fileName = this.openFileDialog1.FileName;
    ///       if (File.Exists(fileName))
    ///       {
    ///          this.htmlEditor1.LoadUrl(this.openFileDialog1.FileName);
    ///       }
    ///    }
    /// }
    /// </code>
    /// If is recommended to check whether the file exists. However, the method will not throw an exception
    /// but ignores the command, which makes it impossible to inform the user that the action fails. 
    /// <para>
    /// From build 1015 on the control accepts SSL protected connections too.
    /// </para>
    /// </example>
    /// <param name="url">URL (http://any.domain.com/file.html) or file (C:\whatever\path\file.html) to load from.</param>
    public void LoadUrl(string url) {
      // Set the current name to static field, to let the protocol handler access the right URL
      Uri uri;
      try {
        uri = new Uri(url);
      } catch (Exception ex) {
        throw new ArgumentException("This URI Format is not supported: " + url, "url", ex);
      }
      Url = url;
      tmpFile = String.Empty;
      if (uri.Scheme.Equals(Uri.UriSchemeFile)) {
        LoadFile(url);
        return;
      }
      if ((uri.Scheme.Equals(Uri.UriSchemeHttp) || uri.Scheme.Equals(Uri.UriSchemeHttps)) || !url.Equals("localhost")) {
        _stringLoadDesired = false;
        IsFileBasedDocument = false;
        ResetDesiredProperties(false);
        // Start loading
        try {
          OnLoading();
          //if (!ComplexContent)
          {
            if (!_namespaceRegistered) {
              InternetSessionRegistry.UnregisterAll();
              InternetSessionRegistry.Register("http", new HttpProtocolHandlerFactory(this));
              InternetSessionRegistry.Register("https", new HttpProtocolHandlerFactory(this));
              _namespaceRegistered = true;
            }
          }
          if (ServiceProvider.GetService(typeof(Interop.IAuthenticate)) != null) {
            ServiceProvider.RemoveService(typeof(Interop.IAuthenticate));
          }
          if (ServiceProvider.GetService(typeof(Interop.IHttpSecurity)) != null) {
            ServiceProvider.RemoveService(typeof(Interop.IHttpSecurity));
          }
          // security
          _clientSite = new ClientSite(this._userName, this._passWord);
          ServiceProvider.AddService(typeof(Interop.IAuthenticate), _clientSite);
          ServiceProvider.AddService(typeof(Interop.IHttpSecurity), _clientSite);
          // loader
          Interop.IOleObject ob = (Interop.IOleObject)MshtmlSite.MSHTMLDocument;
          ob.SetClientSite((Interop.IOleClientSite)MshtmlSite);
          Interop.IPersistMoniker persistMoniker;
          Interop.IBindCtx bindContext;
          Interop.IMoniker moniker;
          persistMoniker = (Interop.IPersistMoniker)MshtmlSite.MSHTMLDocument;
          Win32.CreateURLMoniker(null, Url, out moniker);
          Win32.CreateBindCtx(0, out bindContext);
          Content = String.Empty;
          persistMoniker.Load(1, moniker, bindContext, (int)Interop.STGM.STGM_READ);
          // Done
          OnLoaded();
        } catch (Exception ex) {
          throw new FileLoadException("Cannot load from the given URL.", url, ex);
        }
      }
    }

    /// <summary>
    /// Delete temporarily used file. Called on each LoadHtml and finally with Dispose().
    /// </summary>
    protected void RemoveTempFile() {
      // remove tmp file, if supported and if it is a true temp file in temp file storage
      if (this.tmpFile != null
          &&
          !this.tmpFile.Equals(String.Empty)
          &&
          File.Exists(this.tmpFile)
          &&
          tmpHasToBeRemoved
          &&
          Path.GetDirectoryName(Path.GetTempFileName()).Equals(Path.GetDirectoryName(tmpFile))
          ) {
        try {
          File.Delete(this.tmpFile);
          this.tmpFile = String.Empty;
        } catch {
        }
      }
      tmpHasToBeRemoved = false;
    }

    private void SetDesiredProperties() {
      if (_disableEditFocusDesired) {
        DisableEditFocus = _disableEditFocus;
        _disableEditFocusDesired = false;
      }
      if (_respectVisiblityDesired) {
        RespectVisibility = _respectVisiblityDesiredValue;
        _respectVisiblityDesired = false;
      }
      if (_absolutePositioningDesired) {
        AbsolutePositioningEnabled = _absolutePositioningDesiredValue;
        _absolutePositioningDesired = false;
      }
      if (_autoUrlModeDesired) {
        AutoUrlModeEnabled = _autoUrlModeDesiredValue;
        _autoUrlModeDesired = false;
      }
      if (_multipleSelectionDesired) {
        MultipleSelectionEnabled = _multipleSelectionDesiredValue;
        _multipleSelectionDesired = false;
      }
      if (_liveResizeDesired) {
        LiveResize = _liveResizeDesiredValue;
        _liveResizeDesired = false;
      }
      if (_atomicDesired) {
        AtomicSelection = _atomicDesiredValue;
        _atomicDesired = false;
      }
      if (_glyphsDesired) {
        Glyphs.GlyphsVisible = Glyphs.GlyphsVisible;
        _glyphsDesired = false;
      }
      if (_gridVisibleDesired) {
        Grid.GridVisible = _gridVisibleDesiredValue;
        _gridVisibleDesired = false;
      }
      if (_keepSelectionDesired) {
        KeepSelection = _keepSelectionDesiredValue;
        _keepSelectionDesired = false;
      }
    }

    /// <summary>
    /// Bring control back into virgin state.
    /// </summary>
    protected void ResetDesiredProperties(bool disposing) {
      if (!disposing) {
        if (MshtmlSite == null)
          throw new ApplicationException("Initialization failed.");
      }
      // Add primary designer before plugIns can attach themselves 
      // Prepare desired parameters to be replaced after content ready
      //_designModeDesired = true;
      _linkedStylesheetsDesired = true;
      _disableEditFocusDesired = true;
      _respectVisiblityDesired = true;
      _bordersDesired = true;
      _absolutePositioningDesired = true;
      _autoUrlModeDesired = true;
      _multipleSelectionDesired = true;
      _glyphsDesired = true;
      _gridVisibleDesired = true;
      _liveResizeDesired = true;
      _keepSelectionDesired = true;
      _atomicDesired = true;
      _grid = null;
      // reset global state variables
      _lastFindRange = null;
      _documentDom = null;
      // dispose attached element behaviors
      if (_namespaceManager != null) {
        _namespaceManager.ClearBehaviors();
      }
      _contentModified = false;
      if (_undoUnit != null) {
        oleUndoManagerInstance = null;
        _undoUnit.Reset();
        _undoUnit = null;
      }
      _eventManager = null;
      // clean up child objects
      if (_changeMonitor != null)
        _changeMonitor.Dispose();
      _changeMonitor = null;
      if (_htmlWindow != null)
        _htmlWindow.Dispose();
      _htmlWindow = null;
      if (_textSelector != null)
        _textSelector.Dispose();
      _textSelector = null;
      if (_document != null)
        _document.Dispose();
      _document = null;
      if (_selection != null)
        _selection.Dispose();
      _selection = null;
      if (_textFormatting != null)
        ((HtmlTextFormatting)_textFormatting).Dispose();
      _textFormatting = null;
      if (_cssTextFormatting != null)
        ((CssTextFormatting) _cssTextFormatting).Dispose();
      _textFormatting = null;
      if (_documentStructure != null)
        _documentStructure.Dispose();
      _documentStructure = null;

      if (!disposing) {
        // this clears the service containers!
        ServiceProvider = new NetrixServiceProvider();
        // Make this sited at runtime to support PropertyGrid with default ctors
        HtmlEditorSite = new DesignSite(ServiceProvider, Name);

        // Designer Services
        designerHost = new DesignerHost(this);
        // Add various external Services we need during design time
        ServiceProvider.AddService(typeof(IMenuCommandService), MenuService);

        ServiceProvider.AddService(typeof(IEditorInstanceService), this);
        ServiceProvider.AddService(typeof(IEventBindingService), new EventBindingService(this));

        _namespaceManager = new NamespaceManager(this);
        ServiceProvider.AddService(typeof(INamespaceManager), _namespaceManager);

        // register for internal service calls
        ServiceProvider.AddService(typeof(Interop.IOleCommandTarget), MshtmlSite.MSHTMLDocument as Interop.IOleCommandTarget);

        // implements both SelectionService and HtmlSelection, handles the element change invoker
        ServiceProvider.AddService(typeof(ISelectionService), Selection as ISelectionService);

      }
      GenericElementFactory.ResetElementCache();
    }

    /// <summary>
    /// Load a docx document from given full path.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Docx is being converted into HTML and loaded after transformation. The control does not write
    /// the document back as Docx. Instead, the converted and loaded HTML is being written back.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if filePath is null or empty.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown if folder does not exists.</exception>
    /// <exception cref="FileNotFoundException">Thrown if file does not exists.</exception>
    /// <exception cref="XmlException">Cannot transform DOCX into HTML for some reason. See inner exception.</exception>
    /// <param name="filePath">The path and filename the file is loaded from.</param>
    public void LoadDocx(string filePath) {
#if DOTNET20
            throw new NotImplementedException("V 2.0 of .Net Framework is not supported.");
#else
      if (String.IsNullOrEmpty(filePath)) {
        throw new ArgumentNullException("filePath");
      }
      if (!Directory.Exists(Path.GetDirectoryName(filePath))) {
        throw new DirectoryNotFoundException();
      }
      if (!File.Exists(filePath)) {
        throw new FileNotFoundException("File not found", Path.GetFileName(filePath));
      }
      try {
        // prepare and load XSLT
        using (Stream xslt = typeof(HtmlEditor).Assembly.GetManifestResourceStream("GuruComponents.Netrix.Resources.Loader.DocX2Html.xslt")) {
          XmlReader xr = new XmlTextReader(xslt);
          XslTransform xt = new XslTransform();
          xt.Load(xr);
          // prepare and load DOCX content
          using (Stream documentStream = new FileStream(filePath, FileMode.Open)) {
            System.IO.Packaging.Package package = System.IO.Packaging.Package.Open(documentStream,
                                   FileMode.Open,
                                   FileAccess.ReadWrite);
            // get main document part (document.xml) from package
            Uri uri = new Uri("/word/document.xml", UriKind.Relative);
            System.IO.Packaging.PackagePart part = package.GetPart(uri);
            // get stream for document.xml
            using (Stream partStream = part.GetStream(FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
              XmlReader xmlReader = XmlReader.Create(partStream);
              XmlDocument doc = new XmlDocument();
              using (StreamReader sr = new StreamReader(partStream)) {
                string s = sr.ReadToEnd();
                partStream.Seek(0, SeekOrigin.Begin);
                doc.Load(partStream);
                xmlReader.Close();
                StringBuilder sb = new StringBuilder();
                using (TextWriter tw = new StringWriter(sb)) {
                  XmlResolver resolver = new DocxResolver(package);
                  xt.Transform(doc, null, tw, resolver);
                  LoadHtml(sb.ToString());
                }
              }
            }
          }
        }
        Url = filePath;
      } catch (Exception ex) {
        Url = null;
        throw new XmlException("Cannot transform document. See inner exception for details.", ex);
      }
#endif
    }

#if !DOTNET20
    private class DocxResolver : XmlResolver {

      System.IO.Packaging.Package package;

      public DocxResolver(System.IO.Packaging.Package package) {
        this.package = package;
      }

      public override ICredentials Credentials
      {
        set { throw new NotImplementedException(); }
      }

      public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn) {
        System.IO.Packaging.PackagePart part = package.GetPart(absoluteUri);
        // get stream for for uri
        Stream partStream = part.GetStream(FileMode.Open, FileAccess.Read);
        return partStream;
      }

      public override Uri ResolveUri(Uri baseUri, string relativeUri) {
        Uri uri = new Uri("/" + relativeUri, UriKind.Relative);
        return uri;
      }
    }
#endif

    /// <overloads>This method has three overloads.</overloads>
    /// <summary>
    /// Loads HTML content from a stream into this control.
    /// </summary>
    /// <remarks>
    /// It is recommended to use a <see cref="System.IO.FileStream">FileStream</see> for that procedure, because
    /// internally the name is extracted from the file name of the underlying stream. Otherwise any stream is
    /// possible which can be used with the <see cref="System.IO.StreamReader">StreamReader</see> class.
    /// The method will fire the <see cref="GuruComponents.Netrix.HtmlEditor.Loading">Loading</see> before
    /// the load process starts, but not when the URL or filename was not accepted. This can be used to check
    /// the processing, e.g. if the event does not fire the URL is wrong or the filename was not found. 
    /// If the processing is done the <see cref="GuruComponents.Netrix.HtmlEditor.Loaded">Loaded</see> event
    /// will fired. Remember that the next editing command MUST wait until the
    /// <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> was fired, which
    /// takes a bit more time used by the control to render and display of the content.
    /// <para>
    /// If MSHTML has not yet been created, the loading is postponed until MSHTML has been created.
    /// The method uses internally IMoniker to set the current base path to resolve relative paths
    /// in src and href attributes without changing the parameters.
    /// </para>
    /// </remarks>
    /// <param name="stream"></param>
    public void LoadHtml(FileStream stream) {
      if (stream != null) {
        Url = stream.Name;
        LoadFile(Url);
      } else {
        throw new ArgumentNullException("HtmlEditor.LoadHtml: You must specify a non-null stream for content");
      }
    }

    /// <summary>
    /// Loads HTML content from a string into this control.
    /// </summary>
    /// <remarks>
    /// If previously used stream or file load process was used the existing URL (e.g. the filename) remains.
    /// If in a previous operation was no name set the default name "localhost" was set. 
    /// The method will fire the <see cref="GuruComponents.Netrix.HtmlEditor.Loading">Loading</see> before
    /// the load process starts, but not when the URL or filename was not accepted. This can be used to check
    /// the processing, e.g. if the event does not fire the URL is wrong or the filename was not found. 
    /// If the processing is done the <see cref="GuruComponents.Netrix.HtmlEditor.Loaded">Loaded</see> event
    /// will fired. Remember that the next editing command MUST wait until the
    /// <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> was fired, which
    /// takes a bit more time used by the control to render and display of the content.
    /// <para>
    /// If MSHTML has not yet been created, the loading is postponed until MSHTML has been created.
    /// The method uses internally IMoniker to set the current base path to resolve relative paths
    /// in src and href attributes without changing the parameters.
    /// </para>
    /// <para>If content is empty or null, the procedure is passed but does nothing but firing the load events. 
    /// In that case old content might stay not be removed. Call <see cref="NewDocument"/> instead.</para>
    /// <para>This method will set the control into non file based mode, as there is no filename provided.</para>
    /// </remarks>
    /// <seealso cref="NewDocument"/>
    /// <param name="content"></param>
    public void LoadHtml(string content) {
      ResetDesiredProperties(false);
      OnLoading();
      if (!String.IsNullOrEmpty(content)) {
        IsFileBasedDocument = false;
        // we don't provide these services, because string loading has no authentication capability
        if (ServiceProvider.GetService(typeof(Interop.IAuthenticate)) != null) {
          ServiceProvider.RemoveService(typeof(Interop.IAuthenticate));
        }
        if (ServiceProvider.GetService(typeof(Interop.IHttpSecurity)) != null) {
          ServiceProvider.RemoveService(typeof(Interop.IHttpSecurity));
        }
        StringLoader(content);
      }
      OnLoaded();
    }

    /// <summary>
    /// The tempfile option has been removed is no longer available. Obsolete.
    /// </summary>
    /// <remarks>Property returns always <c>false</c>.</remarks>
    [Browsable(false), Obsolete("The tempfile option has been removed is no longer available.")]
    public bool UseTempFile
    {
      get
      {
        return false;
      }
    }

    private void StringLoader(string content) {
      if (String.IsNullOrEmpty(content)) {
        return;
      }
      _stringLoadDesired = false;
      try {
        // add a leading byte order mark here to allow MSHTML to recognize extended (Asian) characters well
        // MSHTML internally ignores the given encoding and tries always to recognize this by checking the byte order mark
        string byteOrderMark;
        byte[] preamble = UnicodeEncoding.Unicode.GetPreamble();
        byteOrderMark = UnicodeEncoding.Unicode.GetString(preamble, 0, preamble.Length);
        long thesize = preamble.Length + UnicodeEncoding.Unicode.GetByteCount(content);
        Interop.IPersistStreamInit persistStream = (Interop.IPersistStreamInit)this.GetActiveDocument(true);
        IStream uStream;
        streamPtr = Marshal.StringToHGlobalUni(String.Concat(byteOrderMark, content));
        Win32.CreateStreamOnHGlobal(streamPtr, true, out uStream);
        IntPtr ptr = IntPtr.Zero;
        uStream.SetSize(thesize);
        //2nd param, 0 is beginning, 1 is current, 2 is end
        uStream.Seek(0, 0, ptr);
        persistStream.InitNew();
        persistStream.Load(uStream);
        // we save the currently loaded stream to reset the IsDirty flag here
        persistStream.Save(uStream, 1);
        Marshal.ReleaseComObject(persistStream);
      } catch {

      }
    }

    #endregion

    private void SetBaseTag(ref string content) {
      // Set BasePath to help file less loading using temp files
      Regex rx = new Regex(@"<base\s[^>]*/?>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      if (!TempPath.Equals(String.Empty) && !rx.Match(content).Success) {
        Regex headRx = new Regex(@"<head\s*[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Match head = headRx.Match(content);
        if (head.Success) {
          content = String.Concat(content.Substring(0, head.Index + head.Length), String.Format(@"<base href=""{0}"">", TempPath), content.Substring(head.Index + head.Length));
        } else {
          Regex htmlRx = new Regex(@"<html\s*[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
          Match html = htmlRx.Match(content);
          if (html.Success) {
            content = String.Concat(content.Substring(0, html.Index + html.Length), String.Format(@"<head><base href=""{0}""></head>", TempPath), content.Substring(html.Index + html.Length));
          }
        }
      }
    }

    private string _desiredInnerHtml;

    private bool _InnerTextIsDesired;
    private bool _InnerHtmlIsDesired;

    /// <summary>
    /// This property changes the content of the body. 
    /// </summary>
    /// <remarks>Its purpose is to support simple databindings.</remarks>
    /// <seealso cref="OuterHtml"/>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false), DefaultValue("")]
    [Bindable(true)]
    public string InnerHtml
    {
      get
      {
        return (IsReady ? this.GetBodyElement().InnerHtml : String.Empty);
      }
      set
      {
        GenericElementFactory.ResetElementCache();
        IElement body = this.GetBodyElement();
        if (IsReady && body != null) {
          string html = body.InnerHtml;
          body.InnerHtml = value;
          if (PropertyChanged != null && !html.Equals(value)) {
            PropertyChanged(this, new PropertyChangedEventArgs("InnerHtml"));
          }
        } else {
          _desiredInnerHtml = value;
          _InnerHtmlIsDesired = true;
        }
      }
    }


    /// <summary>
    /// This property changes the content of the body and strips out all HTML tags.
    /// </summary>
    /// <remarks>
    /// This is for spellchecking and other basic text functions.
    /// <para>
    /// Setting this value will result in lost of all HTML based formatting, as the InnerText property
    /// internally strips out any HTML and does not preserve any formatting. Reading will not hurt
    /// the content.
    /// </para>
    /// </remarks>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false), DefaultValue("")]
    [Bindable(true)]
    public string InnerText
    {
      get
      {
        return (IsReady ? this.GetBodyElement().InnerText : String.Empty);
      }
      set
      {
        if (IsReady && this.GetBodyElement() != null) {
          string oldValue = this.GetBodyElement().InnerText;
          this.GetBodyElement().InnerText = value;
          if (!value.Equals(oldValue)) {
            if (PropertyChanged != null) {
              PropertyChanged(this, new PropertyChangedEventArgs("InnerText"));
            }
          }
        }
      }
    }

    /// <summary>
    /// Disables the internal monikers to support very complex content.
    /// </summary>
    /// <remarks>
    /// The internal moniker procedure helps building MHT and deals with
    /// local files faster. This handling may fail if complex documents, like
    /// the one exported by PowerPoint or Word are loaded. 
    /// </remarks>
    [Browsable(true), DefaultValue(true), Category("Netrix Component"), Description("Disables the internal monikers to support very complex content.")]
    public bool ComplexContent
    {
      get
      {
        return _complexContent;
      }
      set
      {
        _complexContent = value;
      }
    }

    /// <summary>
    /// This property returns the document's content in raw (unformatted) format.
    /// </summary>
    /// <seealso cref="GetRawHtml()"/>
    /// <seealso cref="InnerHtml"/>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false), DefaultValue("")]
    public string OuterHtml
    {
      get
      {
        if (IsReady) {
          return ((Interop.IHTMLDocument3)GetActiveDocument(false)).GetDocumentElement().GetOuterHTML();
        } else {
          return String.Empty;
        }
      }
    }

    /// <summary>
    /// Creates a new document.
    /// </summary>
    /// <remarks>
    /// Used to reset the control into a neutral state.
    /// </remarks>
    public void NewDocument() {
      Url = null;
      LoadHtml(CreateHtmlContent(""));
    }

    #region +++++ Block: UNDO

    /// <summary>
    /// Undo the last action.
    /// </summary>
    /// <remarks>
    /// The command does nothing if the <see cref="CanUndo"/> property returns false.
    /// </remarks>
    public void Undo() {
      if (CanUndo) {
        Exec(Interop.IDM.UNDO);
      }
    }

    /// <summary>
    /// Redo the last operation or - in case of a batched operation - the whole batch.
    /// </summary>
    /// <remarks>
    /// The command does nothing if the <see cref="CanRedo"/> property returns false.
    /// </remarks>
    public void Redo() {
      if (CanRedo) {
        Exec(Interop.IDM.REDO);
      }
    }

    /// <summary>
    /// Gets a new, opened undo manager. 
    /// </summary>
    /// <remarks>
    /// Until the <see cref="IUndoStack.Close">Close</see> method is called all
    /// following operations become part of one single undo step.
    /// </remarks>
    /// <seealso cref="NextOperationAdded"/>
    /// <param name="Name">Provide a specific name to distinguish this manager instance from other parallel opened ones.</param>
    /// <returns>The undo stack, that contains all current undo steps the undo manager currently helds.</returns>
    public IUndoStack GetUndoManager(string Name) {
      if (!ThrowDocumentNotReadyException())
        return null;
      if (Name.Equals(String.Empty)) {
        return _undoUnit;
      } else {
        return OpenBatchUndo(Name);
      }
    }

    /// <summary>
    /// Gets the default, internally used undo manager.
    /// </summary>
    /// <returns></returns>
    public IUndoStack GetUndoManager() {
      return GetUndoManager(String.Empty);
    }

    /// <summary>
    /// Opens a new Undo section in the UndoManager.
    /// </summary>
    /// <param name="description">used to handle nested undo processes</param>
    /// <returns>The undo object. Provides a Close method to finish the undo section.</returns>
    internal BatchedUndoUnit OpenBatchUndo(string description) {
      BatchedUndoUnit bU = new BatchedUndoUnit(description, this, BatchedUndoType.Undo);
      bU.Open();
      return bU;
    }



    private void bu_NextOperationAdded(object sender, EventArgs e) {
      OnNextOperationAdded(new UndoEventArgs(GetUndoManager()));
    }

    private Interop.IOleUndoManager oleUndoManagerInstance;

    internal Interop.IOleUndoManager UndoManager
    {
      get
      {
        if (oleUndoManagerInstance == null) {
          Interop.IOleServiceProvider serviceProvider = GetActiveDocument(false) as Interop.IOleServiceProvider;
          Guid undoManagerGuid = typeof(Interop.IOleUndoManager).GUID;
          Guid undoManagerGuid2 = typeof(Interop.IOleUndoManager).GUID;
          IntPtr undoManagerPtr;
          int hr = serviceProvider.QueryService(ref undoManagerGuid2, ref undoManagerGuid, out undoManagerPtr);
          if ((hr == Interop.S_OK) && (undoManagerPtr != Interop.NullIntPtr)) {
            oleUndoManagerInstance = (Interop.IOleUndoManager)Marshal.GetObjectForIUnknown(undoManagerPtr);
            Marshal.Release(undoManagerPtr);
          } else {
            throw new ExecutionEngineException("Component threw an internal exception creating Undo manager.");
          }
          if (ServiceProvider.GetService(typeof(Interop.IOleUndoManager)) != null) {
            ServiceProvider.RemoveService(typeof(Interop.IOleUndoManager));
          }
          ServiceProvider.AddService(typeof(Interop.IOleUndoManager), oleUndoManagerInstance);
        }
        return oleUndoManagerInstance;
      }
    }

    /// <summary>
    /// Called if the undo manager adds a new undo unit to the current stack.
    /// </summary>
    /// <seealso cref="GetUndoManager()"/>
    /// <param name="e"></param>
    protected void OnNextOperationAdded(UndoEventArgs e) {
      if (NextOperationAdded != null) {
        NextOperationAdded(this, e);
      }
    }

    /// <summary>
    /// Gets a new, already opened redo manager. 
    /// </summary>
    /// <remarks>
    /// Until the <see cref="IUndoStack.Close">Close</see> method is called all
    /// following operations become part of one single redo step.
    /// <para>
    /// The host application should assure that only operations become part of the redo stack which can be 
    /// reliable "redone" in any situation. It is not recommended to use this in conjunction with table operations.
    /// </para>
    /// <para>If the name parameter is empty the method returns the current internal undo stack.</para>
    /// <para>Redo steps are not being notified.</para>
    /// </remarks>
    /// <param name="description">Provide a specific name to distinguish this manager instance from other parallel opened ones.</param>
    /// <returns>Returns an object which provides the method and properties to control the redo manager.</returns>
    public IUndoStack GetRedoManager(string description) {
      if (String.IsNullOrEmpty(description))
        throw new ArgumentNullException("description");
      BatchedUndoUnit rs = new BatchedUndoUnit(description, this, BatchedUndoType.Redo);
      return rs;
    }

    /// <summary>
    /// Opens a new Undo section in the UndoManager.
    /// </summary>
    /// <param name="description">Provide a specific name to distinguish this manager instance from other parallel opened ones.</param>
    /// <returns>The undo object. Provides a Close method to finish the undo section.</returns>
    internal IUndoStack InternalUndoStack(string description) {
      if (String.IsNullOrEmpty(description))
        throw new ArgumentNullException("description");
      BatchedUndoUnit unit = new BatchedUndoUnit(description, this, BatchedUndoType.Undo);
      unit.Open();
      return unit;
    }

    #endregion

    #region +++++ Block: Public Events and Overwritten Events


    /// <summary>
    /// Calls a handler with which the host application can decide, where the resource is really loaded from.
    /// </summary>
    /// <param name="e">URL of resource. The handler may change the URL in the event arguments to change the behavior.</param>
    /// <returns><c>True</c>, if the resource should still be loaded. <c>False</c>, if the resource should never load.</returns>
    internal protected void OnBeforeResourceLoad(BeforeResourceLoadEventArgs e) {
      if (BeforeResourceLoad != null) {
        BeforeResourceLoad(this, e);
      }
    }

    /// <summary>
    /// Fire internal context menu, if assigned AND external event handler, if
    /// used.
    /// </summary>
    /// <param name="e"></param>
    protected internal void OnShowContextMenu(ShowContextMenuEventArgs e) {
      if (this.ContextMenu != null) {
        // we have an assigned menu
        ContextMenu cm = this.ContextMenu;
        cm.Show(this, e.Location);
      }
      if (this.ContextMenuStrip != null) {
        // we have an assigned menu
        ContextMenuStrip cs = this.ContextMenuStrip;
        cs.Show(this, e.Location);
      }
      if (ShowContextMenu != null) {
        ShowContextMenu(this, e);
      }
    }

    /// <summary>
    /// Called from plug-ins using reflection. DO NOT USE IN PUBLIC CODE!
    /// </summary>
    /// <remarks>
    /// This methods supports the infrastructure and should not called from user code.
    /// </remarks>
    /// <param name="element"></param>
    /// <param name="type"></param>
    public void InvokeExternalHtmlElementChanged(IComponent element, HtmlElementChangedType type) {
      ISelectionService sel = (ISelectionService)ServiceProvider.GetService(typeof(ISelectionService));
      if (sel != null) {
        switch (type) {
          case HtmlElementChangedType.Mouse:
            sel.SetSelectedComponents(new IComponent[] { element }, SelectionTypes.Click);
            break;
          case HtmlElementChangedType.Key:
            sel.SetSelectedComponents(new IComponent[] { element }, SelectionTypes.Normal);
            break;
          case HtmlElementChangedType.Unknown:
            sel.SetSelectedComponents(new IComponent[] { element }, SelectionTypes.Auto);
            break;
        }
      }
      if (HtmlElementChanged != null) {
        if (_htmlElementHashCode != element.GetHashCode()) {
          HtmlElementChanged(this, new HtmlElementChangedEventArgs(element, type));
        }
        _htmlElementHashCode = element.GetHashCode();
      }
      // sync the html selection class to represent current selection
      Selection.SynchronizeSelection();
    }

    internal void InvokeHtmlElementChanged(Interop.IHTMLElement el, HtmlElementChangedType type) {
      if (el != null) {
        if (currentElement == null || el.GetHashCode() != currentElement.GetHashCode()) {
          currentElement = el;
          Control element = this.GenericElementFactory.CreateElement(el);
          if (element != null) {
            InvokeExternalHtmlElementChanged(element, type);
          }
        }
      }
    }

    internal protected new void OnDragEnter(DragEventArgs drgevent) {
      if (DragEnter != null) {
        DragEnter(this, drgevent);
      }
    }

    internal protected void OnDragLeave() {
      if (DragLeave != null) {
        DragLeave(this, EventArgs.Empty);
      }
    }

    internal protected new void OnDragOver(DragEventArgs drgevent) {
      if (DragOver != null) {
        DragOver(this, drgevent);
      }
    }

    internal protected new void OnDragDrop(DragEventArgs drgevent) {
      if (DragDrop != null) {
        DragDrop(this, drgevent);
      }
    }

    internal protected void OnBeforeNavigate(BeforeNavigateEventArgs args) {
      if (BeforeNavigate != null) {
        BeforeNavigate(this, args);
      }
    }

    private Interop.IHTMLElement uiScopeElement = null;
    private IElement lastCachedElement = null;
    private UpdateUIEventArgs uiuea = null;

    /// <summary>
    /// Calls the event handler for UpdateUI.
    /// </summary>
    protected internal void OnUpdateUI(string eventType) {
      UpdateUIItems();
      if (UpdateUI != null && eventType != null) {
        IElement element;
        if (lastCachedElement == null || lastCachedElement.GetBaseElement() != uiScopeElement) {
          element = GenericElementFactory.CreateElement(uiScopeElement) as IElement;
          lastCachedElement = element;
          UpdateReason r = UpdateReason.Other;
          if (eventType.StartsWith("mouse"))
            r = UpdateReason.Mouse;
          else
              if (eventType.StartsWith("key"))
            r = UpdateReason.Key;
          uiuea = new UpdateUIEventArgs(element, r);
          UpdateUI(this, uiuea);
        } else {
          element = lastCachedElement;
        }
      }
    }

    /// <summary>
    /// Called if the toolstrips are active and user clicked a tool.
    /// </summary>
    /// <remarks>
    /// Event is being fired before any internal action to allow the host application to stop or skip internal operations.
    /// </remarks>
    /// <param name="sender">Toolstrip which causes the event.</param>
    /// <param name="e">Parameter contains information about clicked tool.</param>
    protected void OnToolItemClicked(object sender, ToolClickedCancelEventArgs e) {
      if (ToolItemClicked != null) {
        ToolItemClicked(sender, e);
      }
    }

    internal int InvokeHtmlEvent(Interop.IHTMLEventObj eventObj, Control element) {
      if (element is IElement) {
        uiScopeElement = ((IElement)element).GetBaseElement();
      }
      return InvokeExternalHtmlEvent(eventObj.type, element, eventObj);
    }

    internal void InvokeClick() {
      base.OnClick(EventArgs.Empty);
    }

    internal void InvokeDoubleClick() {
      base.OnDoubleClick(EventArgs.Empty);
    }

    internal void InvokeMouseClick(MouseButtons b) {
      MouseEventArgs e = new MouseEventArgs(b, 1, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0);
      base.OnMouseClick(e);
    }

    internal void InvokeMouseDoubleClick(MouseButtons b) {
      MouseEventArgs e = new MouseEventArgs(b, 2, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0);
      base.OnMouseDoubleClick(e);
    }

    internal void InvokeMouseUp(MouseButtons b) {
      base.OnMouseUp(new MouseEventArgs(b, 1, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0));
    }

    internal void InvokeMouseDown(MouseButtons b) {
      base.OnMouseDown(new MouseEventArgs(b, 1, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0));
    }
    internal void InvokeMouseMove(MouseButtons b) {
      base.OnMouseMove(new MouseEventArgs(b, 1, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0));
    }
    private Point deltaWheel = Point.Empty;
    internal void InvokeMouseWheel() {
      int delta = (System.Windows.Forms.Control.MousePosition.X - deltaWheel.X) + (System.Windows.Forms.Control.MousePosition.Y - deltaWheel.Y);
      deltaWheel = System.Windows.Forms.Control.MousePosition;
      base.OnMouseWheel(new MouseEventArgs(System.Windows.Forms.Control.MouseButtons, 1, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0));
    }
    internal void InvokeMouseOver() {
      base.OnMouseHover(EventArgs.Empty);
    }
    internal void InvokeMouseOut() {
      base.OnMouseLeave(EventArgs.Empty);
    }
    internal void InvokeMouseEnter() {
      base.OnMouseEnter(EventArgs.Empty);
    }

    internal void InvokeKeyDown(KeyEventArgs e) {
      base.OnKeyDown(e);
    }

    internal void InvokeKeyUp(KeyEventArgs e) {
      base.OnKeyUp(e);
    }

    internal void InvokeKeyPress(KeyPressEventArgs e) {
      base.OnKeyPress(e);
    }
    internal void InvokeHelpRequested(HelpEventArgs e) {
      base.OnHelpRequested(e);
    }

    //internal void MouseEnter

    /// <summary>
    /// This method supports the NETRIX infrastructure and is not intended being called from host application directly.
    /// </summary>
    /// <remarks>
    /// Called from plug-ins using Reflection only. To force any of the events override the appropriate "Onxxx" method
    /// to invoke own event methods.
    /// </remarks>
    /// <param name="eventType">Event type.</param>
    /// <param name="element">Element that causes the event.</param>
    /// <param name="eventObj">Native (COM) event object.</param>
    /// <returns></returns>
    public int InvokeExternalHtmlEvent(string eventType, Control element, Interop.IHTMLEventObj eventObj) {
      int returnCode = Interop.S_FALSE;
      if (eventType.StartsWith("mouse")) {
        if (HtmlMouseOperation != null) {
          HtmlMouseEventArgs meargs = new HtmlMouseEventArgs(element, eventObj, this);
          HtmlMouseOperation(this, meargs);
          returnCode = meargs.Handled ? Interop.S_OK : Interop.S_FALSE;
        }
        return returnCode;
      }
      if (eventType.StartsWith("key")) {
        // Simulate the missing KeyUp event here
        KeyEventType kevt = KeyEventType.Unknown;
        switch (eventType) {
          case "keydown":
            kevt = KeyEventType.KeyDown;
            Keys key;
            key = (Keys)eventObj.keyCode;
            if (key != Keys.ShiftKey && key != Keys.ControlKey) {
              key |= eventObj.ctrlKey ? Keys.Control : Keys.None; // 
              key |= eventObj.shiftKey ? Keys.Shift : Keys.None; // 
              key |= eventObj.altKey ? Keys.Alt : Keys.None; // 
            }
            KeyEventArgs kdargs = new KeyEventArgs(key);
            InvokeKeyDown(kdargs);
            returnCode = kdargs.Handled ? Interop.S_OK : Interop.S_FALSE;
            break;
          case "keypress":
            kevt = KeyEventType.KeyPress;
            int keycode = (eventObj.keyCode >= 97 && eventObj.keyCode <= 122) ? eventObj.keyCode - 32 : eventObj.keyCode;
            key = (Keys)keycode;
            if (key != Keys.ShiftKey && key != Keys.ControlKey) {
              key |= eventObj.ctrlKey ? Keys.Control : Keys.None; // 
              key |= eventObj.shiftKey ? Keys.Shift : Keys.None; // 
              key |= eventObj.altKey ? Keys.Alt : Keys.None; // 
            }
            KeyEventArgs keargs = new KeyEventArgs(key);
            InvokeKeyUp(keargs);
            returnCode = keargs.Handled ? Interop.S_OK : Interop.S_FALSE;
            // Simulate the missing KeyPress event here
            char kChar;
            if ((int)key < 128)
              kChar = Win32.GetAsciiCharacter((int)key);
            else
              kChar = (char)(int)key;
            KeyPressEventArgs kpargs = new KeyPressEventArgs(kChar);
            InvokeKeyPress(kpargs);
            returnCode = kpargs.Handled ? Interop.S_OK : Interop.S_FALSE;
            break;
        }
        if (HtmlKeyOperation != null) {
          HtmlKeyEventArgs fargs = new HtmlKeyEventArgs(eventObj, this);
          fargs.EventType = kevt;
          HtmlKeyOperation(this, fargs);
          returnCode = fargs.Handled ? Interop.S_OK : Interop.S_FALSE;
        }
        return returnCode;
      }
      return returnCode;
    }

    private EventManager _eventManager;
    /// <summary>
    /// Allows the management of events on element level.
    /// </summary>
    [Browsable(false)]
    public EventManager EventManager
    {
      get
      {
        if (_eventManager == null) {
          _eventManager = new EventManager();
        }
        return _eventManager;
      }
    }

    /// <summary>
    /// Gets access to the event binding service.
    /// </summary>
    /// <remarks>
    /// Provides a service for registering event handlers for component events. This allows implementers
    /// to show events in the PropertyGrid's and attach tasks as events of EventBindingService.
    /// <example>
    /// To show the event tab in PropertyGrid attach the Site of the current component and refresh the tabs:
    /// <code>
    /// propertyGrid1.SelectedObject = e.CurrentElement;
    /// IComponent c = ((IComponent)e.CurrentElement);
    /// this.propertyGrid1.Site = c.Site;
    /// this.propertyGrid1.RefreshTabs(PropertyTabScope.Component);
    /// </code>
    /// This code assumes to be within an event handler which provides the event argument <c>e</c>. 
    /// From main control you can access the EventBinding events:
    /// 
    /// </example>
    /// </remarks>
    [Browsable(false)]
    public IEventBinding EventBinding
    {
      get
      {
        return GuruComponents.Netrix.Events.EventBinding.GetInstance(this);
      }
    }

    /// <summary>
    /// Binding to the user interface service.
    /// </summary>
    /// <remarks>
    /// The purpose is to have total control over the way designer aware components, like the
    /// PropertyGrid, operate and interact with the control and elements. This allows you to
    /// add component editors, catch internal errors thrown by the PropertyGrid, add styles
    /// or show private dialogs instead of the common one for collection editors or other internally
    /// invoked dialogs.
    /// <para>
    /// Several events can be used to interact with the service.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.Designer.UIService">UIService</seealso>
    /// <seealso cref="GuruComponents.Netrix.Designer.IUIService">IUIService (Netrix' implementation)</seealso>
    /// <seealso cref="System.Windows.Forms.Design.IUIService">IUIService (System's implementation)</seealso>
    /// </remarks>
    [Browsable(false)]
    public IUIService UIBinding
    {
      get
      {
        return UIService.GetInstance(this);
      }
    }

    /// <summary>
    /// Set the focus to the control.
    /// </summary>
    public new void Focus() {
      panelEditContainer.Focus();
    }

    internal void InvokeBeforeShortcut(BeforeShortcutEventArgs e) {
      if (BeforeShortcut != null) {
        BeforeShortcut(this, e);
      }
    }

    /// <summary>
    /// Informs whether the control has the focus.
    /// </summary>
    [DefaultValue(false)]
    [Browsable(false)]
    public override bool Focused
    {
      get
      {
        return ((Interop.IHTMLDocument4)MshtmlSite.MSHTMLDocument).hasFocus();
      }
    }

    /// <summary>
    /// If set to <c>true</c> the focus will not set on load.
    /// </summary>
    /// <remarks>
    /// The default behavior of the focus will set the focus on the control and
    /// activates the caret after loading a document or HTML. This property suppresses
    /// this behavior if set.
    /// <para>
    /// It is important to set (or let) this property to <c>false</c>, if content is loaded, which
    /// has client side scripts in it or which has forms where the user can enter values. If the
    /// document cannot receive the focus, the user cannot enter anything, the content is totally
    /// blocked. 
    /// </para>
    /// The intention of this property is to allow the subsequential re-loading of the content using
    /// <see cref="LoadHtml(string)">LoadHtml</see> whereas the user enters data into another control on the form. If, under
    /// any circumstances, the user is allows to enter anything or copy anything from the control, the
    /// property must turned off (set to <c>false</c> again.
    /// <para>
    /// This property was introduced in the second edition of the 1016 Build, June 8, 2004. The normal
    /// behavior was not changed if the property is not set explicitly and the control runs in design
    /// mode. In browse mode the control runs with getting the focus, unless this property is
    /// explicitly set.
    /// </para>
    /// </remarks>
    [Category("Netrix Component")]
    [DefaultValue(false)]
    [Description("Normally the focus will catched if a new document is loaded. This let the user immediataly start typing. Setting this property to TRUE prevents this behavior. Default is FALSE.")]
    [Browsable(true)]
    public bool StopFocusOnLoad
    {
      set
      {
        _stopFocusOnLoad = value;
      }
      get
      {
        return _stopFocusOnLoad;
      }
    }

    /// <summary>
    /// Exchanges the behavior of Enter key. If active, Enter inserts BR instead of P.
    /// </summary>
    /// <remarks>
    /// This method allows the editor to behave differently if the user hits the Enter key.
    /// By default, enter inserts a new paragraph. If this property is <c>true</c>, the Enter key will
    /// insert a break (BR tag). The opposite insertion method, Shft-Enter, will insert a paragraph.
    /// The type of paragraph depends on the the current <see cref="BlockDefault"/> setting.
    /// </remarks>
    /// <seealso cref="BlockDefault"/>
    [Browsable(true), Category("Netrix Component"), Description("Exchanges the behavior of Enter key. If active, Enter inserts BR instead of P.")]
    [DefaultValue(false)]
    public bool TransposeEnterBehavior
    {
      get { return _transposeEnterBehavior; }
      set { _transposeEnterBehavior = value; }
    }

    /// <summary>
    /// On focus, we have to also return focus to MSHTML.
    /// </summary>
    protected void PanelGotFocus(EventArgs e) {
      if (!StopFocusOnLoad && IsReady) {
        MshtmlSite.SetFocus();
      }
    }

    /// <summary>
    /// Called when the control has just become ready.
    /// </summary>
    /// <param name="e"></param>
    internal protected void OnReadyStateComplete(EventArgs e) {
      if (_designModeDesired) {
        _designModeDesired = false;
        _designModeEnabled = _designModeDesiredValue;
        if (IsFileBasedDocument) {
          _desiredUrl = Url;
          this.Exec(_designModeEnabled ? Interop.IDM.EDITMODE : Interop.IDM.BROWSEMODE);
          LoadUrl(_desiredUrl);
        } else {
          string c = GetRawHtml();
          this.Exec(_designModeEnabled ? Interop.IDM.EDITMODE : Interop.IDM.BROWSEMODE);
          StringLoader(c);
        }
        return;
      }
      // Set BasePath to help file less loading using temp files, when any document exists
      if (!this.TempFile.Equals(String.Empty) && !DesignModeEnabled) {
        this.DocumentStructure.SetBase(this.TempFile);
      }
      Interop.IHtmlBodyElement bodyElement = this.GetBodyThreadSafe(false);
      // the element is null if it is a frameset, where each frame document must set into design view individually
      if (bodyElement != null) {
        if (_InnerTextIsDesired) {
          InnerText = _desiredInnerHtml;
          _InnerTextIsDesired = false;
        }
        if (_InnerHtmlIsDesired) {
          InnerHtml = _desiredInnerHtml;
          _InnerHtmlIsDesired = false;
        }
        //Set the design mode to the last desired design mode options
        if (_bordersDesired) {
          BordersVisible = _bordersDesiredValue;
          _bordersDesired = false;
        }
        if (_linkedStylesheetsDesired) {
          LinkedStylesheetsEnabled = _linkedStylesheetsDesiredValue;
          _linkedStylesheetsDesired = false;
        }
        if (this.DesignModeEnabled) {
          // control's properties
          SetDesiredProperties();
          // attach edit designer
          this.SetEditDesigner();
          // fire final document created event
          if (DocumentCreated != null) {
            DocumentCreated(this, e);
          }
          // finally we set the caret (in designmode only) to the beginning of the document and scroll it to the window top border
          Interop.IHTMLTxtRange range = bodyElement.createTextRange();
          range.Collapse(true);
          range.Select();
          range.ScrollIntoView(true);
          // Set gravity to improve users's editing experience
          Interop.IDisplayServices ds = (Interop.IDisplayServices)MshtmlSite.MSHTMLDocument;
          Interop.IDisplayPointer dp;
          ds.CreateDisplayPointer(out dp);
          dp.SetDisplayGravity(Interop.DISPLAY_GRAVITY.DISPLAY_GRAVITY_NextLine);
          dp.SetPointerGravity(Interop.POINTER_GRAVITY.POINTER_GRAVITY_Left);

          // Create and start Undo manager and corresponding stack, empty name is the master stack
          // Start Undo manager
          _undoUnit = new BatchedUndoUnit("", this, BatchedUndoType.Undo);
          _undoUnit.Open();
          _undoUnit.NextOperationAdded += new EventHandler(bu_NextOperationAdded);
          // Add Services we need in designmode only
          ServiceProvider.RemoveService(typeof(Interop.IHTMLEditDesigner));
          ServiceProvider.AddService(typeof(Interop.IHTMLEditDesigner), MshtmlSite);
          // Support snap elements in absolute position mode
          _snapElement = new SnapElement(this);
          ServiceProvider.RemoveService(typeof(Interop.IHTMLEditHost));
          ServiceProvider.AddService(typeof(Interop.IHTMLEditHost), _snapElement);
          _snapElement.AfterSnapRect += new AfterSnapRectEventHandler(_snapElement_AfterSnapRect);
          _snapElement.BeforeSnapRect += new BeforeSnapRectEventHandler(_snapElement_BeforeSnapRect);
          // Change management
          Interop.IMarkupContainer2 mc2 = (Interop.IMarkupContainer2)GetActiveDocument(false);
          _changeMonitor = new ChangeMonitor(mc2, this);
          _changeMonitor.Change += new EventHandler(changeSink_Change);

        }
      }
        // Loading completed
        ((DesignerHost)this.designerHost).OnLoadComplete();

      // Inform Plugins that content is ready
      foreach (IPlugIn plugin in plugIns) {
        plugin.NotifyReadyStateCompleted(this);
      }

      // inform host that we're ready
      if (ReadyStateComplete != null) {
        ReadyStateComplete(this, e);
      }

      ResizeEditContainer();
      Window.Scroll += new DocumentEventHandler(Window_Scroll);

    }

    void Window_Scroll(object sender, DocumentEventArgs e) {
      OnRulerScroll();
    }

    /// <summary>
    /// Allows access to the change monitor.
    /// </summary>
    /// <seealso cref="ContentChanged"/>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ChangeMonitor ChangeManager
    {
      get { return _changeMonitor; }
      set { _changeMonitor = value; }
    }

    void changeSink_Change(object sender, EventArgs e) {
      OnContentChanged();
    }


    /// <summary>
    /// Determines that the document is loaded from and saved into a file.
    /// </summary>
    /// <remarks>
    /// This property is set automatically if the document is loaded from an URL or file
    /// using the <see cref="LoadUrl"/> methods. If the <see cref="LoadHtml(string)">LoadHtml</see> methods are
    /// used, the host application can determine how the load/save process works. To
    /// work with files the property is set to <c>true</c>. This is the default value.
    /// To work without any file relation, the property should set to <c>false</c>, before
    /// issuing a <see cref="LoadHtml(string)">LoadHtml</see> method to load content.
    /// </remarks>
    [Browsable(false), DefaultValue(true)]


    public bool IsFileBasedDocument
    {
      get
      {
        return _isFileBasedDocument;
      }
      set
      {
        _isFileBasedDocument = value;
      }
    }

    #endregion

    #region +++++ Block: Window Handler, Control Handler, Process Message (Internals)

    #region ----- Control Container Methods

    /// <summary>
    /// We check the ready state and fire the complete event if the incoming event is "complete".
    /// </summary>
    /// <remarks>
    /// Before the
    /// native complete event is fired the universal <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateChanged"/>
    /// event is fired.
    /// </remarks>
    internal protected void OnReadyStateChanged(string newReadyState) {
      if (ReadyStateChanged != null) {
        ReadyStateChanged(this, new ReadyStateChangedEventArgs(newReadyState));
      }
      if (String.Compare(newReadyState, "complete", true) == 0) {
        try {
          this.OnReadyStateComplete(EventArgs.Empty);
        } catch {
        }
      }
    }

    #endregion

    #region ----- Internal Control Methods

    internal IntPtr PanelHandle
    {
      get { return panelEditContainer.IsDisposed ? IntPtr.Zero : panelEditContainer.Handle; }
    }

    /// <internalonly/>
    protected override void Dispose(bool disposing) {
      panelEditContainer.Resize -= new EventHandler(panelEditContainer_Resize);
      panelEditContainer.GotFocus -= new EventHandler(panelEditContainer_GotFocus);
      panelEditContainer.VisibleChanged -= new EventHandler(panelEditContainer_VisibleChanged);
      panelEditContainer.HandleCreated -= new EventHandler(panelEditContainer_HandleCreated);
      panelEditContainer.HandleDestroyed -= new EventHandler(panelEditContainer_HandleDestroyed);
      panelEditContainer.ParentChanged -= new EventHandler(panelEditContainer_ParentChanged);

      InternetSessionRegistry.UnregisterAll();
      if (_namespaceManager != null) {
        _namespaceManager.Clear();
      }
      if (disposing) {
        if (MhtBuilder != null)
          MhtBuilder = null;
        foreach (System.Windows.Forms.Control c in Controls)
          c.Dispose();
        foreach (IPlugIn plugin in RegisteredPlugIns)
          plugin.Dispose();
        ResetDesiredProperties(disposing);
        GenericElementFactory.ResetElementCache();
        if (components != null) {
          components.Dispose();
        }
        if (Url != null) {
          Url = null;
        }
        if (MshtmlSite != null) {
          IntPtr ptr = Marshal.GetIDispatchForObject(MshtmlSite.OleDocument);
          int i = Marshal.Release(ptr);
          while (i > 0) {
            i = Marshal.Release(ptr);
          }
          MshtmlSite.Dispose();
          MshtmlSite = null;
        }
      }
      RemoveTempFile();
      base.Dispose(disposing);
      GC.Collect();
    }

    /// <summary>
    /// Called if a frame becomes the active frame in a frameset.
    /// </summary>
    /// <param name="e">Argument which has information about the frame.</param>
    internal void OnFrameActivated(FrameEventArgs e) {
      if (e.FrameWindow == null)
        return;
      if (FrameActivated != null) {
        FrameActivated(this, e);
      }
    }

    private CommandService MenuService
    {
      get
      {
        if (menuService == null) {
          menuService = new CommandService(this);
        }
        return menuService;
      }
    }


    private List<IPlugIn> plugIns = new List<IPlugIn>();

    /// <summary>
    /// Gives direct access to the list of registered plug-ins.
    /// </summary>
    [Browsable(false)]
    public List<IPlugIn> RegisteredPlugIns
    {
      get { return plugIns; }
    }

    /// <summary>
    /// Plugins can call this method to register itself as callback for certain functions.
    /// </summary>
    /// <exception cref="ArgumentNullException">Argument <paramref name="plugin"/> must not be <c>null</c>.</exception>
    /// <param name="plugin">The Plugin object which user wish to register.</param>
    public void RegisterPlugIn(IPlugIn plugin) {
      if (plugin == null) {
        throw new ArgumentNullException("Must provide an instance of a plugin.");
      }
      if (plugIns == null) {
        plugIns = new List<IPlugIn>();
      }
      // do not re-register
      if (!plugIns.Contains(plugin)) {
        plugIns.Add(plugin);
      }
    }

    /// <summary>
    /// Adds an externally provided command to the list of available menu commands.
    /// </summary>
    /// <param name="wrapper">The command object, which provides callback information.</param>
    public void AddCommand(CommandWrapper wrapper) {
      wrapper.TargetEditor = this;        // this is important to handle multiple controls with extenders
      MenuService.AddCommand(wrapper);
    }

    /// <summary>
    /// Registers a namespace alias for the namespace manager.
    /// </summary>
    /// <remarks>
    /// The purpose of this method is to add a namespace of elements, for which any plug-in
    /// is available that provides design and formatter information. 
    /// </remarks>
    /// <param name="alias">The alias of the namespace, like "asp" for ASP.NET controls.</param>
    /// <param name="namespaceName">URN of namespace</param>
    /// <param name="behavior">The type of behavior the namespace manager uses to resolve elements for rendering. Must inherit from IBaseBehavior.</param>
    public void RegisterNamespace(string alias, string namespaceName, Type behavior) {
      NamespaceManager.RegisterNamespace(alias, namespaceName, behavior);
    }

    /// <summary>
    /// Registers a namespace alias for the namespace manager.
    /// </summary>
    /// <remarks>
    /// The purpose of this method is to add a namespace of elements, for which any plug-in
    /// is available that provides design and formatter information. 
    /// </remarks>
    /// <param name="alias">The alias of the namespace, like "asp" for ASP.NET controls.</param>
    /// <param name="behavior">The type of behavior the namespace manager uses to resolve elements for rendering. Must inherit from IBaseBehavior.</param>
    public void RegisterNamespace(string alias, Type behavior) {
      RegisterNamespace(alias, String.Empty, behavior);
    }

    /// <summary>
    /// Returns the list of registered namespaces as dictionary with alias:ns pairs.
    /// </summary>
    /// <returns>A list of registered namespaces as dictionary with alias:ns pairs</returns>
    public IDictionary GetRegisteredNamespaces() {
      return NamespaceManager.RegisteredNamespaces;
    }

    /// <summary>
    /// Registers an element for internal processing.
    /// </summary>
    /// <remarks>
    /// The type class must have a public constructor which must accept exact one parameter of
    /// type <see cref="GuruComponents.Netrix.ComInterop.Interop.IHTMLElement">IHTMLElement</see>.
    /// Once the element is registered, the control tries to get the constructor, instantiate an
    /// object and return the object to the caller. Some of the events the control fires use these
    /// generic object creation to expose native element objects to the host application.
    /// <para>
    /// To obtain the <see cref="GuruComponents.Netrix.HtmlFormatting.Elements.ITagInfo">ITagInfo</see>
    /// object instantiate it using the <see cref="GuruComponents.Netrix.HtmlFormatting.Elements.TagInfo">TagInfo</see> class or
    /// any class which implements <see cref="GuruComponents.Netrix.HtmlFormatting.Elements.ITagInfo">ITagInfo</see>.
    /// When creating an <see cref="GuruComponents.Netrix.HtmlFormatting.Elements.ITagInfo">ITagInfo</see> object for
    /// elements which resides in a specific namespace, one must include the namespace alias which
    /// registered whithin the control. The namespace must have been registered before the first element is added.
    /// </para>
    /// <para>
    /// The type the element object is created from must implement <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement">IElement</see>.
    /// </para>
    /// <seealso cref="RegisterNamespace(string, Type)">RegisterNamespace</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.IElement"/>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.Element"/>
    /// <seealso cref="GuruComponents.Netrix.HtmlFormatting.Elements.ITagInfo"/>
    /// <seealso cref="GuruComponents.Netrix.HtmlFormatting.Elements.TagInfo"/>
    /// </remarks>
    /// <param name="tagInfo">The formatting information.</param>
    /// <param name="elementType">The type of element.</param>
    public void RegisterElement(
        ITagInfo tagInfo,
        Type elementType) {
      Formatter.AddCustomElement(tagInfo);
      ((ElementFactory)GenericElementFactory).RegisterElement(tagInfo.TagName, elementType);
    }

    /// <summary>
    /// Try to find the next parent element in the hierarchy which belongs to one of the registered namespaces.
    /// </summary>
    /// <remarks>
    /// This method is used to redirect base events from viewlink elements to its parents. The pre event handler can
    /// call this method to see any parents and stop firing base events. Then the event chain is redirected to the
    /// plugged designers, which in turn re-fire the base events.
    /// </remarks>
    /// <returns>The generic parent element, if one exists, or <c>null</c> in case of failure.</returns>
    private Interop.IHTMLElement GetRegisteredNamespaceParent(Interop.IHTMLElement currentElement) {
      if (NamespaceManager.RegisteredNamespaces.Contains(((Interop.IHTMLElement2)currentElement).GetScopeName())) {
        return currentElement;
      } else {
        Interop.IHTMLElement parent = currentElement.GetParentElement();
        if (parent != null) {
          return GetRegisteredNamespaceParent(parent);
        }
      }
      return null;
    }

    /// <summary>
    /// Invoke a command with the specific Command ID.
    /// </summary>
    /// <remarks>
    /// Plug-Ins can extend the list of available commands at any time and use this method to
    /// issue their own (or any other registered command).
    /// <seealso cref="AddCommand"/>
    /// </remarks>
    /// <param name="id">The ID of any internal or externally registered command.</param>
    public void InvokeCommand(CommandID id) {
      menuService.GlobalInvoke(id);
    }

    internal NamespaceManager NamespaceManager
    {
      get
      {
        // added as a service
        return _namespaceManager;
      }
    }
    private Control _defaultcontrolParent;
    internal Control DefaultControlParent
    {
      get { return _defaultcontrolParent; }
      set { _defaultcontrolParent = value; }
    }
    private bool _needactivation;
    internal bool NeedActivation
    {
      get { return _needactivation; }
      set { _needactivation = value; }
    }

    #endregion

    #endregion

    /// <summary>
    /// Provides access for sited components.
    /// </summary>
    /// <remarks>
    /// In case you work with a complete design time environment, including usage of PropertyGrid and services,
    /// the design time code needs access to the internal services the NetRix component provides. HtmlEditorSite
    /// gives that access by offering its own ISite implementation.
    /// <para>
    /// This is important, if you use the PropertyGrid to create new objects. For instance, if you have a
    /// &lt;select&gt; element and wish to add &lt;option&gt; elements, these elements need a reference to the
    /// underlying editor. The internal service is aware of this and the design time host is supposed to use these
    /// services as well. 
    /// </para>
    /// <example>
    /// The following examples assumes you have a PropertyGrid and HtmlEditor component on a form:
    /// <code>
    /// private void htmlEditor_HtmlElementChanged(object sender, HtmlElementChangedEventArgs e)
    /// {            
    ///     this.propertyGrid1.Site = htmlEditor.HtmlEditorSite; // comment out and ctor calls will fail!
    ///     this.propertyGrid1.SelectedObject = e.CurrentElement;
    /// }
    /// </code>
    /// This assignes the internal Site to the PropertyGrid. And the grid will call the appropriate services, then.
    /// </example>
    /// <para>
    /// Internally NetRix Pro provides a fully developed design time environment, which is responsible for all 
    /// particular needs the HTML elements may have. Even if public constructors exists the call must match specific
    /// conditions to have valid references between the element and the editor.
    /// </para>
    /// <para>
    /// In case you don't use the PropertyGrid you can cast <c>HtmlEditorSite</c> to <c>ISite</c> and use 
    /// <c>GetService</c> to create elements within an design time environment. For instance, IDesignerHost ist
    /// available to create elements.
    /// </para>
    /// </remarks>
    /// <seealso cref="DesignerHost"/>
    /// <seealso cref="DesignSite"/>
    /// <seealso cref="CommandService"/>
    /// <seealso cref="UIService"/>
    /// <seealso cref="HtmlElementChangedEventArgs"/>
    /// <seealso cref="HtmlElementChanged"/>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IDesignSite HtmlEditorSite
    {
      get { return _htmlEditorsite; }
      set { _htmlEditorsite = value; }
    }
    private IDesignSite _htmlEditorsite;
    internal MSHTMLSite MshtmlSite
    {
      get { return _mshtmlsite; }
      set { _mshtmlsite = value; }
    }
    private MSHTMLSite _mshtmlsite;
    internal IntPtr DocumentHandle
    {
      get { return _documenthandle; }
      set { _documenthandle = value; }
    }
    private IntPtr _documenthandle;

    /// <summary>
    /// Gets the service provide for all internal and private services.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <seealso cref="HtmlEditorSite"/>
    /// <seealso cref="ServiceContainer"/>
    /// <seealso cref="NetrixServiceProvider"/>
    [Browsable(false)]
    public NetrixServiceProvider ServiceProvider
    {
      get { return _serviceprovider; }
      set { _serviceprovider = value; }
    }
    private NetrixServiceProvider _serviceprovider;

    #region IEditorInstanceService Members

    [Browsable(false)]
    public IHtmlEditor EditorInstance
    {
      get { return this; }
    }

    #endregion

    /// <summary>
    /// Event raised when a property is changed on the component.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
  }
}
