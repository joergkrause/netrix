using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.AspDotNetDesigner
{
    /// <summary>
    /// This class creates the design time HTML for ASP.NET control,
    /// which are linked to the underlying HTML designer.
    /// </summary>
    public class DesignTimeBehavior :
        IBaseBehavior,
        Interop.IElementBehaviorLayout,
        Interop.IHTMLPainterEventInfo,
        //IControlDesignerTag,
        //IControlDesignerView,
        IHtmlControlDesignerBehavior,
        IControlDesignerBehavior,
        IDisposable
    {
        private static Bitmap _controlGlyph;
        private static Stream stream;

        private IHtmlEditor _editor;
        private IDesignerHost _designerHost;
        private IComponentChangeService _changeService;
        private Interop.IElementBehaviorSite _behaviorSite;
        private Interop.IHTMLPaintSite _paintSite;
        private Interop.IHTMLElement _element;
        private Interop.IHTMLElement _parent;
        private Interop.IHTMLElement _viewElement;
        private EventSink _eventSink;
        private Control _genericWebControl;
        private IElement _nativeElement; // wrapped _element !
        private HtmlControlDesigner _designer;
        private bool _savingContents;
        private bool _controlDown;
        private static bool _dragging;
        private static bool _isResizing;

        /// <summary>
        /// Unique name of behavior.
        /// </summary>
        public string Name
        {
            get
            {
                return "ASP#Designer";
            }
        }

        /// <summary>
        /// Current webcontrol handled by this design time behavior.
        /// </summary>
        public Control Control
        {
            get
            {
                if (_genericWebControl == null && this.Designer != null && this.Designer.Component != null)
                {
                    _genericWebControl = this.Designer.Component as Control;
                }
                return _genericWebControl;
            }
        }

        /// <summary>
        /// Related to editor that contains the element.
        /// </summary>
        public IHtmlEditor Editor
        {
            get
            {
                return _editor;
            }
        }

        internal Interop.IHTMLElement Parent
        {
            get
            {
                return _parent;
            }
        }

        internal IServiceProvider ServiceProvider
        {
            get
            {
                return Editor.ServiceProvider;
            }
        }

        static DesignTimeBehavior()
        {
            stream = typeof(DesignTimeBehavior).Assembly.GetManifestResourceStream("GuruComponents.Netrix.AspDotNetDesigner.Resources.Behavior.ico");
            _controlGlyph = new Bitmap(stream);
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="editor"></param>
        public DesignTimeBehavior(IHtmlEditor editor)
        {
            this._editor = editor;
            this._designerHost = editor.ServiceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
            this._changeService = editor.ServiceProvider.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            this._changeService.ComponentChanged += new ComponentChangedEventHandler(_changeService_ComponentChanged);
        }

        void _changeService_ComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            if (e.Component.Equals(_genericWebControl))
            {
                PersistProperties();
            }
        }

        private void ConnectToControlAndDesigner()
        {
            Debug.WriteLine("Connect", this.GetHashCode().ToString());
            //SetAttribute("RuntimeComponent", _genericNetRixControl, false);
            if (_eventSink == null)
            {
                _eventSink = new EventSink(this);
                _eventSink.Connect(Element, _editor);
                _eventSink.ElementEvent += new ElementEventHandler(_eventSink_ElementEvent);
            }
            if (Control is AscxElement)
            {
                string tagPrefix = ((AscxElement)Control).Alias;
                string tagName = ((AscxElement)Control).TagName;
                IReferenceManager wrm = _editor.ServiceProvider.GetService(typeof(IWebFormReferenceManager)) as IReferenceManager;
                string basePath = (_editor.Url != null) ? Path.GetDirectoryName(_editor.Url) : Directory.GetCurrentDirectory();
                RegisterDirective rd = wrm.GetRegisterDirective(tagPrefix, tagName) as RegisterDirective;
                if (rd != null && rd.ExpandUserControl)
                {
                    string ascxFile = ((GuruComponents.Netrix.Designer.WebFormsReferenceManager)wrm).GetUserControlPath(tagPrefix, tagName);
                    string ascxPath = Path.Combine(basePath, ascxFile);
                    if (File.Exists(ascxPath))
                    {
                        string ucContent = File.ReadAllText(ascxPath);
                        Control[] uc = System.Web.UI.Design.ControlParser.ParseControls(this._designerHost, ucContent);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (Control childControl in uc)
                        {
                            INameCreationService ns = _designerHost.GetService(typeof(INameCreationService)) as INameCreationService;
                            ((DesignerHost)_designerHost).Add(childControl, ns.CreateName(_designerHost.Container, childControl.GetType()));
                            IDesigner ucDesigner = _designerHost.GetDesigner(childControl);
                            if (ucDesigner != null)
                            {
                                sb.Append(((ControlDesigner)ucDesigner).GetDesignTimeHtml());
                            }
                        }
                        DesignTimeHtml = sb.ToString();
                        return;
                    }
                }
            }
            if (Control is ErrorControl)
            {
                DesignTimeHtml = ((ErrorControl)Control).GetDesignTimeHtml();
                return;
            }
            if (Designer != null)
            {
                Designer.Behavior = this;
                if (Designer is ControlDesigner)
                {
                    if (((ControlDesigner)Designer).ReadOnly)
                    {
                        if (((ControlDesigner)Designer).DesignTimeHtmlRequiresLoadComplete && _designerHost.Loading)
                        {
                            _designerHost.LoadComplete += new EventHandler(this.OnDesignerHostLoadComplete);
                            return;
                        }
                        DesignTimeHtml = ((ControlDesigner)Designer).GetDesignTimeHtml();
                    }
                }
            }
        }

        /// <summary>
        /// Redirect internal events from sink to outer control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _eventSink_ElementEvent(object sender, Interop.IHTMLEventObj e)
        {
            System.Diagnostics.Debug.WriteLineIf(!e.type.Contains("mouse") && !e.type.Contains("prop"), e.type);
            switch (e.type)
            {
                //    case "click":
                //        _genericWebControl.InvokeClick(e);
                //        break;
                case "dblclick":
                    //((IElement)NativeElement).InvokeDblClick(e);
                    break;
                //    case "keypress":
                //        _genericWebControl.InvokeKeyPress(e);
                //        break;
                //    case "keydown":
                //        _genericWebControl.InvokeKeyDown(e);
                //        break;
                //    case "keyup":
                //        _genericWebControl.InvokeKeyUp(e);
                //        break;
                //    case "mouseout":
                //        _genericWebControl.InvokeMouseOut(e);
                //        break;
                //    case "mouseover":
                //        _genericWebControl.InvokeMouseOver(e);
                //        break;
                //    case "mousemove":
                //        _genericWebControl.InvokeMouseMove(e);
                //        break;
                //    case "mousedown":
                //        _genericWebControl.InvokeMouseDown(e);
                //        break;
                //    case "mouseup":
                //        _genericWebControl.InvokeMouseUp(e);
                //        break;
                //    case "selectstart":
                //        _genericWebControl.InvokeSelectStart(e);
                //        break;
                //    case "filterchange":
                //        _genericWebControl.InvokeFilterChange(e);
                //        break;
                //    case "losecapture":
                //        _genericWebControl.InvokeLoseCapture(e);
                //        break;
                //    case "propertychange":
                //        if (_genericWebControl != null)
                //        {
                //            _genericWebControl.InvokePropertyChange(e);
                //        }
                //        break;
                //    case "scroll":
                //        _genericWebControl.InvokeScroll(e);
                //        break;
                //    case "focus":
                //        _genericWebControl.InvokeFocus(e);
                //        break;
                //    case "blur":
                //        _genericWebControl.InvokeBlur(e);
                //        break;
                case "dragstart":
                    _dragging = true;
                    break;
                case "drag":
                    break;
                case "dragend":
                    _dragging = false;
                    OnContentSave();
                    break;
                //    case "dragenter":
                //        _genericWebControl.InvokeDragEnter(e);
                //        break;
                //    case "dragover":
                //        _genericWebControl.InvokeDragOver(e);
                //        break;
                //    case "dragleave":
                //        _genericWebControl.InvokeDragLeave(e);
                //        break;
                //    case "drop":
                //        _genericWebControl.InvokeDrop(e);
                //        break;
                //    case "beforecut":
                //        _genericWebControl.InvokeBeforeCut(e);
                //        break;
                //    case "cut":
                //        _genericWebControl.InvokeCut(e);
                //        break;
                //    case "beforecopy":
                //        _genericWebControl.InvokeBeforeCopy(e);
                //        break;
                //    case "copy":
                //        _genericWebControl.InvokeCopy(e);
                //        break;
                //    case "beforepaste":
                //        _genericWebControl.InvokeBeforePaste(e);
                //        break;
                //    case "paste":
                //        _genericWebControl.InvokePaste(e);
                //        break;
                //    case "contextmenu":
                //        _genericWebControl.InvokeContextMenu(e);
                //        break;
                //    case "beforeeditfocus":
                //        //
                //        break;
                //    case "layoutcomplete":
                //        _genericWebControl.InvokeLayoutComplete(e);
                //        break;
                //    case "page":
                //        _genericWebControl.InvokePage(e);
                //        break;
                //    case "beforedeactivate":
                //        _genericWebControl.InvokeBeforeDeactivate(e);
                //        break;
                //    case "beforeactivate":
                //        _genericWebControl.InvokeBeforeActivate(e);
                //        break;
                //    case "controlselect":
                //        _genericWebControl.InvokeControlSelect(e);
                //        break;
                //    case "move":
                //        _genericWebControl.InvokeMove(e);
                //        break;
                //    case "movestart":
                //        _genericWebControl.InvokeMoveStart(e);
                //        break;
                //    case "moveend":
                //        _genericWebControl.InvokeMoveEnd(e);
                //        break;
                case "resize":
                    break;
                case "resizestart":
                    _isResizing = true;
                    break;
                case "resizeend":
                    _isResizing = false;
                    break;
                //    case "mouseenter":
                //        _genericWebControl.InvokeMouseEnter(e);
                //        break;
                //    case "mouseleave":
                //        _genericWebControl.InvokeMouseLeave(e);
                //        break;
                //    case "mousewheel":
                //        _genericWebControl.InvokeMouseWheel(e);
                //        break;
                //    case "activate":
                //        _genericWebControl.InvokeActivate(e);
                //        break;
                //    case "deactivate":
                //        _genericWebControl.InvokeDeactivate(e);
                //        break;
                //    case "focusin":
                //        _genericWebControl.InvokeFocusIn(e);
                //        break;
                //    case "focusout":
                //        _genericWebControl.InvokeFocusOut(e);
                //        break;
                //    case "load":
                //        _genericWebControl.InvokeLoad(e);
                //        break;
                //    case "error":
                //        _genericWebControl.InvokeError(e);
                //        break;
                //    case "change":
                //        _genericWebControl.InvokeChange(e);
                //        break;
                //    case "abort":
                //        _genericWebControl.InvokeAbort(e);
                //        break;
                //    case "select":
                //        _genericWebControl.InvokeSelect(e);
                //        break;
                //    case "selectionchange":
                //        _genericWebControl.InvokeSelectionChange(e);
                //        break;
                //    case "stop":
                //        _genericWebControl.InvokeStop(e);
                //        break;
            }
        }

        private void CreateControlAndDesigner()
        {
            Debug.WriteLine("CreateControlAndDesigner", this.GetHashCode().ToString());
            bool parseError;
            _genericWebControl = ParseControl(out parseError);

            if (!parseError && _genericWebControl != null)
            {
                // Add the editor ref if not already there
                if (_genericWebControl is Element)
                {
                    if (((Element)_genericWebControl).HtmlEditor == null)
                    {

                        ((Element)_genericWebControl).HtmlEditor = Editor;
                    }
                }

                // Unique name is unique amongst all controls, including IElement based
                string uniqueName;
                ((Interop.IHTMLUniqueName)Element).uniqueID(out uniqueName);
                // Control IS is ID="" attribute based and not equal to uniqueName
                if (EnsureControlIDAndSearchUniqueNameinContainer(_genericWebControl, uniqueName) == false)
                {
                    _designerHost.Container.Add(_genericWebControl, uniqueName);
                }
                else
                {
                    _designerHost.Container.Remove(_genericWebControl);
                    _designerHost.Container.Add(_genericWebControl, uniqueName);
                }
                IDesigner d = _designerHost.GetDesigner(_genericWebControl);
                if (d == null)
                {
                    d = TypeDescriptor.CreateDesigner(_genericWebControl, typeof(IDesigner));
                }
                if (d is ControlDesigner)
                {
                    Designer = (ControlDesigner)d;
                    if (((ControlDesigner)Designer).ViewControl == null)
                    {
                        ((ControlDesigner)Designer).ViewControl = _genericWebControl;
                        ((ControlDesigner)Designer).ViewControlCreated = true;
                    }
                    ((ControlDesigner)Designer).Behavior = this;
                }
                //Debug.Assert(Designer != null, "Designer error 2");
                //Debug.Assert(_genericWebControl.Site != null);
                ((ControlDesigner)Designer).ReadOnly = false;
                return;
            }
            else
            {
                if (_eventSink != null)
                {
                    _eventSink.AllowResize = false;
                }
            }
        }

        private void CreateControlView()
        {
            Debug.WriteLine("CreateContolView", this.GetHashCode().ToString());
            if (Designer != null && Designer is ControlDesigner)
            {
                Interop.IHTMLDocument2 originDocument = (Interop.IHTMLDocument2)Element.GetParentElement().GetDocument();
                Interop.IHTMLElementDefaults elementDefaults = ((Interop.IElementBehaviorSiteOM2)_behaviorSite).GetDefaults();
                Interop.IHTMLElement htmlElement = originDocument.CreateElement("HTML");
                Interop.IHTMLElement headElement = originDocument.CreateElement("HEAD");
                Interop.IHTMLElement bodyElement = originDocument.CreateElement("BODY");
                //Interop.IHTMLElement spanElement = originDocument.CreateElement("SPAN");
                ((Interop.IHTMLElement2)htmlElement).InsertAdjacentElement("afterBegin", headElement);
                ((Interop.IHTMLElement2)htmlElement).InsertAdjacentElement("beforeEnd", bodyElement);
                //((Interop.IHTMLElement2)bodyElement).InsertAdjacentElement("afterBegin", spanElement);
                //_viewElement = spanElement;
                _viewElement = bodyElement;
                string html = ((ControlDesigner)Designer).GetDesignTimeHtml();
                _viewElement.SetInnerHTML(html);
                //Interop.IHTMLDocument baseDocument = (Interop.IHTMLDocument)spanElement.GetDocument();
                Interop.IHTMLDocument baseDocument = (Interop.IHTMLDocument)bodyElement.GetDocument();
                elementDefaults.SetViewLink(baseDocument);
                elementDefaults.SetViewInheritStyle(true); // on false dblClick shows hatch border
                elementDefaults.SetViewMasterTab(true);
                elementDefaults.SetFrozen(true);   // handle events as unit, otherwise internal controls will fire events too
                try
                {
                    Interop.IHTMLDocument2 baseDocument2 = (Interop.IHTMLDocument2)baseDocument;
                    int numStyles = 0;
                    Interop.IHTMLStyleSheetsCollection baseDocumentStylesheets = originDocument.GetStyleSheets();
                    if (baseDocumentStylesheets != null)
                    {
                        numStyles = baseDocumentStylesheets.Length;
                        for (int j = 0; j < numStyles; j++)
                        {
                            object local = j;
                            Interop.IHTMLStyleSheet sheet1 =
                                (Interop.IHTMLStyleSheet)baseDocumentStylesheets.Item(local);
                            if (sheet1 != null)
                            {
                                int k = 0;
                                Interop.IHTMLStyleSheetRulesCollection rules = sheet1.GetRules();
                                if (rules != null)
                                {
                                    k = rules.GetLength();
                                }
                                if (k != 0)
                                {
                                    Interop.IHTMLStyleSheet newSheet = baseDocument2.CreateStyleSheet(String.Empty, 0);
                                    for (int i2 = 0; i2 < k; i2++)
                                    {
                                        Interop.IHTMLStyleSheetRule rule = rules.Item(i2);
                                        if (rule != null)
                                        {
                                            string str1 = rule.GetSelectorText();
                                            string str2 = rule.GetStyle().cssText;
                                            if (str2 != null)
                                            {
                                                newSheet.AddRule(str1, str2, i2);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }

            }
        }

        // return true if exists, false if not. Ensures always element has ID.
        // To check whether the element is already in the container, we look for the uniquename
        private bool EnsureControlIDAndSearchUniqueNameinContainer(Control c, string uniqueName)
        {
            if (_genericWebControl == null) return false;
            string str = c.ID;
            bool elementHasNoID = str == null;
            INameCreationService iNameCreationService = null;
            if (elementHasNoID == true)
            {
                // has no ID, so we need to create one
                iNameCreationService = (INameCreationService)ServiceProvider.GetService(typeof(INameCreationService));
                // assure we take the base type
                str = iNameCreationService.CreateName(_designerHost.Container, _genericWebControl.GetType());
                // set the ID
                c.ID = str;
                Element.SetAttribute("id", str, 0);
            }
            // check the container for element
            return (_designerHost.Container.Components[uniqueName] != null);
        }

        internal bool IsControlDown()
        {
            return _controlDown;
        }

        internal bool IsDragging()
        {
            return _dragging;
        }

        internal bool IsResizing()
        {
            return _isResizing;
        }

        private void OnBehaviorDetach()
        {
            Debug.WriteLine("Detach", this.GetHashCode().ToString());
            if (Designer != null)
            {
                Designer.Behavior = null;
                Designer = null;
            }
            if (_eventSink != null)
            {
                _eventSink.Disconnect();
                _eventSink = null;
            }
            _controlDown = false;
            _behaviorSite = null;
        }

        private void OnBehaviorInit(Interop.IElementBehaviorSite behaviorSite)
        {
            _behaviorSite = behaviorSite;
            _paintSite = (Interop.IHTMLPaintSite)_behaviorSite;
            Element = _behaviorSite.GetElement();
            Debug.WriteLine("OnBehaviorInit " + _behaviorSite.GetElement().GetTagName());

            behaviorSite.RegisterNotification(0);

            behaviorSite.RegisterNotification((int)Interop.BEHAVIOR_EVENT.CONTENTREADY);
            behaviorSite.RegisterNotification((int)Interop.BEHAVIOR_EVENT.APPLYSTYLE);
            behaviorSite.RegisterNotification((int)Interop.BEHAVIOR_EVENT.DOCUMENTREADY);
            behaviorSite.RegisterNotification((int)Interop.BEHAVIOR_EVENT.DOCUMENTCONTEXTCHANGE);
            behaviorSite.RegisterNotification((int)Interop.BEHAVIOR_EVENT.CONTENTSAVE);

        }

        private void OnBehaviorNotify(Interop.BEHAVIOR_EVENT eventId)
        {
            if (_behaviorSite == null) return; // probably disposed
            switch (eventId)
            {
                case Interop.BEHAVIOR_EVENT.CONTENTREADY:

                    OnContentReady();
                    return;

                case Interop.BEHAVIOR_EVENT.DOCUMENTCONTEXTCHANGE:
                    OnDocumentContextChanged();
                    return;

                case Interop.BEHAVIOR_EVENT.CONTENTSAVE:
                    OnContentSave();
                    return;

                case Interop.BEHAVIOR_EVENT.DOCUMENTREADY:
                case Interop.BEHAVIOR_EVENT.APPLYSTYLE:
                    return;

            }
        }

        private void OnContentReady()
        {
            if (_designerHost.Loading)
            {
                CreateControlAndDesigner();
                SetControlParent(Element.GetParentElement());
                CreateControlView();
                ConnectToControlAndDesigner();
            }
            else
            {
                Interop.IHTMLElement parentElement = Element.GetParentElement();
                if (_parent == null && parentElement != null)
                {
                    bool flag = false;
                    string str = (String)this.GetAttribute("id", true);
                    if (!String.IsNullOrEmpty(str))
                    {
                        IComponent iComponent = null;
                        string ms_id;
                        ((GuruComponents.Netrix.ComInterop.Interop.IHTMLUniqueName)Element).uniqueID(out ms_id);
                        if (_designerHost.Container.Components.Count > 0)
                        {
                            iComponent = _designerHost.Container.Components[ms_id];
                            foreach (IComponent c in _designerHost.Container.Components)
                            {
                                if (!(c is Control)) continue;
                                if (((Control)c).ID == str)
                                {
                                    iComponent = c;
                                    break;
                                }
                            }
                        }
                        if (iComponent != null && iComponent is Control)
                        {
                            Control control = (Control)iComponent;
                            HtmlControlDesigner controlDesigner = _designerHost.GetDesigner(control) as HtmlControlDesigner;
                            if (controlDesigner != null)
                            {
                                DesignTimeBehavior behavior = (DesignTimeBehavior)controlDesigner.Behavior;
                                if (behavior != null && behavior.IsDragging() && !behavior.IsControlDown())
                                {
                                    ((DesignTimeBehavior)controlDesigner.Behavior).EndDrag();
                                    flag = true;
                                    Designer = controlDesigner;
                                    SetControlParent(parentElement);
                                    CreateControlView();
                                    ConnectToControlAndDesigner();
                                }
                            }
                        }
                    }
                    else
                    {
                        // Create Name
                    }
                    if (!flag)
                    {
                        CreateControlAndDesigner();
                        SetControlParent(parentElement);
                        CreateControlView();
                        ConnectToControlAndDesigner();
                    }
                }
            }
            Interop.IHTMLElement2 el2 = (Interop.IHTMLElement2)Element;
            Interop.IHTMLStyle el2RunStyle = el2.GetRuntimeStyle();
            Interop.IHTMLStyle elStyle = Element.GetStyle();
            if (el2RunStyle != null)
            {
                el2RunStyle.SetDisplay("inline-block");
                int width = elStyle.GetPixelWidth();
                int height = elStyle.GetPixelHeight();
                if (width > 0)
                {
                    el2RunStyle.SetWidth(width + 1);
                }
                if (height > 0)
                {
                    el2RunStyle.SetHeight(height + 1);
                }
                el2RunStyle.SetOverflow("hidden");
                if (_editor.AbsolutePositioningEnabled)
                {
                    ((Interop.IHTMLStyle2)el2RunStyle).SetPosition("absolute");
                }
                else
                {
                    el2RunStyle.RemoveAttribute("position", 0);
                }
            }
        }

        /// <summary>
        /// Let the view follow the properties.
        /// </summary>
        private void OnContentRefresh()
        {
            Debug.WriteLine("OnContentRefresh", this.GetHashCode().ToString());
            if (Designer != null)
            {
                string str = DesignTimeHtml;
                if (str != null && _viewElement != null)
                {
                    _eventSink.EventsEnabled = false;
                    try
                    {
                        _viewElement.SetInnerHTML(str);
                        Interop.IHTMLStyle style1 = ((Interop.IHTMLElement2)this.Element).GetRuntimeStyle();
                        if (Control is WebControl)
                        {
                            WebControl wc = (WebControl)Control;
                            if (wc.Width != Unit.Empty)
                            {
                                style1.SetPixelWidth(Convert.ToInt32(wc.Width.Value + 1));
                            }
                            if (wc.Height != Unit.Empty)
                            {
                                style1.SetPixelHeight(Convert.ToInt32(wc.Height.Value + 1));
                            }
                            wc.Width = wc.Width;
                        }
                    }
                    finally
                    {
                        _eventSink.EventsEnabled = true;
                    }
                }
            }
        }

        private void PropertyChangedHandler(object sender, EventArgs e)
        {
            PersistProperties();
        }

        internal void PersistProperties()
        {
            OnContentRefresh();
            //OnContentSave();
        }

        private void OnContentSave()
        {
            if (_savingContents)
            {
                return;
            }
            try
            {
                _savingContents = true;
                System.Diagnostics.Debug.WriteLine(IsDragging(), "OnSave");
                if (Designer != null && Control != null && !IsDragging())
                {
                    EmbeddedPersister.PersistControl(Element, Control, _designerHost);
                    //OnContentRefresh();
                }
            }
            catch
            {
            }
            finally
            {
                _savingContents = false;
            }
        }

        private void OnDesignerHostLoadComplete(object sender, EventArgs e)
        {
            if (Designer != null)
            {
                this.DesignTimeHtml = ((ControlDesigner)Designer).GetDesignTimeHtml();
                _designerHost.LoadComplete -= new EventHandler(this.OnDesignerHostLoadComplete);
            }
        }

        private void OnDocumentContextChanged()
        {
            Interop.IHTMLElement _newElement = null;
            try
            {
                _newElement = (Element != null) ? Element.GetParentElement() : null;
                //_parent = _newElement;
            }
            catch
            {
            }
            if (_parent != null)
            {
                if (_genericWebControl == null)
                {
                    CreateControlAndDesigner();
                    SetControlParent(_parent);
                    CreateControlView();
                    ConnectToControlAndDesigner();
                    return;
                }
                else
                {
                    IDesigner d = _designerHost.GetDesigner(_genericWebControl);
                    if (d == null)
                    {
                        d = TypeDescriptor.CreateDesigner(_genericWebControl, typeof(IDesigner));
                    }
                    if (d is ControlDesigner)
                    {
                        Designer = (ControlDesigner)d;
                        if (((ControlDesigner)Designer).ViewControl == null)
                        {
                            ((ControlDesigner)Designer).ViewControl = _genericWebControl;
                            ((ControlDesigner)Designer).ViewControlCreated = true;
                        }
                        ((ControlDesigner)Designer).Behavior = this;
                    }
                }
                SetControlParent(_parent);
            }
            else if (_newElement != null && _genericWebControl == null)
            {
                CreateControlAndDesigner();
                SetControlParent(_newElement);
                CreateControlView();
                ConnectToControlAndDesigner();
            }
        }

        private Control ParseControl(out bool parseError)
        {
            string str1 = Element.GetOuterHTML();
            Control control = null;
            parseError = false;
            try
            {
                object[] locals = new object[1];
                Element.GetAttribute("runat", 0, locals);
                if (true) // TODO: element type reading
                {
                    if (String.Compare(locals[0].ToString(), 0, "server", 0, 6, true, CultureInfo.InvariantCulture) != 0)
                    {
                        if (Parent != null)
                        {
                            Parent.GetAttribute("runat", 0, locals);
                            if (String.Compare(locals[0].ToString(), 0, "server", 0, 6, true, CultureInfo.InvariantCulture) != 0)
                            {
                                // assume that the parent must have the attribute
                                throw new Exception(String.Format("{0} is missing the runat=\"server\" attribute.", Element.GetTagName()));
                            }
                        }
                    }
                }
                control = ControlParser.ParseControl(_designerHost, str1);
                if (control is AscxElement)
                {
                    ((AscxElement)control).AssociatePeer(Element, this);
                    //GuruComponents.Netrix.Designer.WebFormsReferenceManager wrm = _designerHost.GetService(typeof(IWebFormReferenceManager)) as GuruComponents.Netrix.Designer.WebFormsReferenceManager;
                    //IRegisterDirective rd = wrm.GetRegisterDirective(((Interop.IHTMLElement2)Element).GetScopeName(), Element.GetTagName());
                    //string vpath = rd.SourceFile;
                    //Control ascx = ((AscxElement)control).LoadControl();
                }
                if (control == null)
                {
                    control = new ErrorControl(String.Concat(((Interop.IHTMLElement2)Element).GetScopeName(), ":", Element.GetTagName()), new ArgumentException("Invalid tag or missing attribute"));
                    _designerHost.Container.Add(control);
                    _designer = _designerHost.GetDesigner(control) as HtmlControlDesigner;
                }
            }
            catch (Exception e)
            {
                control = new ErrorControl(String.Concat(((Interop.IHTMLElement2)Element).GetScopeName(), ":", Element.GetTagName()), e);
                _designerHost.Container.Add(control);
                _designer = _designerHost.GetDesigner(control) as HtmlControlDesigner;
                parseError = true;
            }
            return control;
        }

        private void SetContainedControlsParent(Interop.IHTMLElement element)
        {
            Interop.IHTMLElementCollection interop_IHTMLElementCollection = (Interop.IHTMLElementCollection)element.GetChildren();
            if (interop_IHTMLElementCollection != null)
            {
                int i = interop_IHTMLElementCollection.GetLength();
                for (int j = 0; j < i; j++)
                {
                    Interop.IHTMLElement interop_IHTMLElement = (Interop.IHTMLElement)interop_IHTMLElementCollection.Item(j, j);
                    if (interop_IHTMLElement != null)
                    {
                        object[] locals = new object[1];
                        interop_IHTMLElement.GetAttribute("runat", 0, locals);
                        if (String.Compare(locals[0] as String, "server", true) == 0)
                        {
                            interop_IHTMLElement.GetAttribute("id", 0, locals);
                            string str2 = locals[0] as String;
                            Control control = _designerHost.Container.Components[str2] as Control;
                            if (control != null && control.Parent != Control && !(control is ErrorControl))
                            {
                                Control.Controls.Add(control);
                                ControlDesigner controlDesigner = _designerHost.GetDesigner(control) as ControlDesigner;
                                if (controlDesigner != null)
                                {
                                    controlDesigner.OnSetParent();
                                }
                            }
                        }
                        else
                        {
                            SetContainedControlsParent(interop_IHTMLElement);
                        }
                    }
                }
            }
        }

        private void SetControlParent(Interop.IHTMLElement newParent)
        {
            try
            {
                Control control;

                _parent = newParent;
                Interop.IHTMLElement parentElement = newParent;
                for (control = null; control == null && parentElement != null; parentElement = parentElement.GetParentElement())
                {
                    object[] locals = new object[1];
                    parentElement.GetAttribute("runat", 0, locals);
                    string str1 = locals[0] as String;
                    if (String.Compare("server", str1, true) == 0)
                    {
                        parentElement.GetAttribute("id", 0, locals);
                        string str2 = locals[0] as String;
                        control = _designerHost.Container.Components[str2] as Control;
                    }
                }
                if (control == null)
                {
                    control = (Control)_designerHost.RootComponent;
                }
                if (Control != null && !(Control is ErrorControl))
                {
                    if (Control.Parent != control)
                    {
                        control.Controls.Add(Control);
                        if (Designer != null)
                        {
                            Designer.OnSetParent();
                            if (!((ControlDesigner)Designer).ReadOnly)
                            {
                                Control.Controls.Clear();
                                SetContainedControlsParent(Element);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        internal void SetControlDown(bool val)
        {
            _controlDown = val;
        }

        internal void StartDrag()
        {
            Debug.WriteLine("START");
            _dragging = true;
        }

        internal void EndDrag()
        {
            Debug.WriteLine("END");
            _dragging = false;
        }

        # region IElementBehavior

        void Interop.IElementBehavior.Init(Interop.IElementBehaviorSite behaviorSite)
        {
            OnBehaviorInit(behaviorSite);
        }

        void Interop.IElementBehavior.Notify(int eventId, IntPtr pVar)
        {
            OnBehaviorNotify((Interop.BEHAVIOR_EVENT)eventId);
        }

        void Interop.IElementBehavior.Detach()
        {
            OnBehaviorDetach();
        }

        # endregion

        # region IControlDesignerBehavior

        /// <summary>
        /// View Element
        /// </summary>
        public object DesignTimeElementView
        {
            get
            {
                return _viewElement;
            }
        }
        /// <summary>
        /// Design time HTML
        /// </summary>
        public string DesignTimeHtml
        {
            get
            {
                if (Designer != null && Designer is ControlDesigner && !(Designer is UserControlDesigner))
                {
                    //System.Reflection.MemberInfo mi = Designer.GetType().GetMethod("GetViewRendering");
                    //if (mi != null)
                    //{
                    //    Designer.GetType().InvokeMember("GetViewRendering", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, Designer, null);
                    //}
                    ((ControlDesigner)Designer).GetViewRendering();
                    ((ControlDesigner)Designer).UpdateDesignTimeHtml();
                    return ((ControlDesigner)Designer).GetDesignTimeHtml();
                }
                string str = String.Empty;
                if (_viewElement != null)
                {
                    str = _viewElement.GetInnerHTML();
                }
                return str;
            }
            set
            {
                if (value == null || _viewElement == null)
                {
                    return;
                }
                _viewElement.SetInnerHTML(value);
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public void OnTemplateModeChanged()
        {
        }

        # endregion

        internal Interop.IHTMLElement Element
        {
            get { return (Interop.IHTMLElement)DesignTimeElement; }
            set { _element = value; }
        }

        # region IHtmlControlDesignerBehavior Member

        /// <summary>
        /// Designer instance.
        /// </summary>
        public HtmlControlDesigner Designer
        {
            get
            {
                return _designer;
            }
            set
            {
                if (value == null) return;
                _designer = value;
            }
        }

        /// <summary>
        /// Native (MSHTML) element that's behind the ASP.NET control.
        /// </summary>
        public IElement NativeElement
        {
            get
            {
                if (_nativeElement == null)
                {
                    _nativeElement = new GenericElement(this.Editor, _element, this);
                }
                return _nativeElement;
            }
        }

        /// <summary>
        /// design time instance.
        /// </summary>
        public object DesignTimeElement
        {
            get
            {
                return _element;
            }
        }
        /// <summary>
        /// Gets an attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public object GetAttribute(string attribute, bool ignoreCase)
        {
            if (DesignTimeElement == null)
            {
                return null;
            }
            object[] locals1 = new object[1];
            try
            {
                Element.GetAttribute(attribute, !ignoreCase ? 1 : 0, locals1);
                object local = locals1[0];
                return local;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get Style
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="designTimeOnly"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public object GetStyleAttribute(string attribute, bool designTimeOnly, bool ignoreCase)
        {
            if (Element != null)
            {
                Interop.IHTMLStyle style = null;
                if (designTimeOnly)
                {
                    style = ((Interop.IHTMLElement2)Element).GetRuntimeStyle();
                }
                else
                {
                    style = Element.GetStyle();
                }
                if (style != null)
                {
                    try
                    {
                        object local = style.GetAttribute(attribute, !ignoreCase ? 1 : 0);
                        return local;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="designTimeOnly"></param>
        /// <returns></returns>
        public string GetStyle(bool designTimeOnly)
        {
            if (Element != null)
            {
                Interop.IHTMLStyle style = null;
                if (designTimeOnly)
                {
                    style = ((Interop.IHTMLElement2)Element).GetRuntimeStyle();
                }
                else
                {
                    style = Element.GetStyle();
                }
                if (style != null)
                {
                    try
                    {
                        object local = style.GetCssText();
                        return (local == null) ? "" : local.ToString();
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="ignoreCase"></param>
        public void RemoveAttribute(string attribute, bool ignoreCase)
        {
            if (Element != null)
            {
                try
                {
                    Element.RemoveAttribute(attribute, !ignoreCase ? 1 : 0);
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// Removes a style attribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="designTimeOnly"></param>
        /// <param name="ignoreCase"></param>
        public void RemoveStyleAttribute(string attribute, bool designTimeOnly, bool ignoreCase)
        {
            if (Element != null)
            {
                Interop.IHTMLStyle style = null;
                if (designTimeOnly)
                {
                    style = ((Interop.IHTMLElement2)Element).GetRuntimeStyle();
                }
                else
                {
                    style = Element.GetStyle();
                }
                if (style != null)
                {
                    try
                    {
                        style.RemoveAttribute(attribute, !ignoreCase ? 1 : 0);
                    }
                    catch
                    {
                    }
                }
            }
        }
        /// <summary>
        ///  Set Attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="val"></param>
        /// <param name="ignoreCase"></param>
        public void SetAttribute(string attribute, object val, bool ignoreCase)
        {
            if (Element != null)
            {
                try
                {
                    if (this.Control != null)
                    {
                        // sync value with control's value
                        PropertyInfo pi = this.Control.GetType().GetProperty(attribute);
                        if (pi != null)
                        {
                            pi.SetValue(this.Control, val, null);
                        }
                        Element.SetAttribute(attribute, val.ToString(), !ignoreCase ? 1 : 0);
                        // special treatment for resizing
                        if (attribute.ToLower().Equals("width") || attribute.ToLower().Equals("height") && val is Unit)
                        {
                            if (val.ToString().EndsWith("%"))
                            {
                                Unit percentage = (Unit)val;
                                Interop.IHTMLElement parent = _element.GetParentElement();
                                int pwidth = ((Interop.IHTMLElement2)parent).GetBoundingClientRect().right - ((Interop.IHTMLElement2)parent).GetBoundingClientRect().left;
                                if (!((ControlDesigner)Designer).ReadOnly)
                                {
                                    string px = Unit.Pixel((int)(pwidth * (percentage.Value / 100))).ToString();
                                    ((WebControl)this.Control).Width = Unit.Parse(px);
                                }
                            }
                        }
                        OnContentSave();
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// SetStyleAttribute.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="designTimeOnly"></param>
        /// <param name="val"></param>
        /// <param name="ignoreCase"></param>
        public void SetStyleAttribute(string attribute, bool designTimeOnly, object val, bool ignoreCase)
        {
            if (Element != null)
            {
                Interop.IHTMLStyle interop_IHTMLStyle = null;
                if (designTimeOnly)
                {
                    interop_IHTMLStyle = ((Interop.IHTMLElement2)Element).GetRuntimeStyle();
                }
                else
                {
                    interop_IHTMLStyle = Element.GetStyle();
                }
                if (interop_IHTMLStyle != null)
                {
                    try
                    {
                        interop_IHTMLStyle.SetAttribute(attribute, val, !ignoreCase ? 1 : 0);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Set all styles as string.
        /// </summary>
        /// <param name="cssText"></param>
        /// <param name="designTimeOnly"></param>
        public void SetStyle(string cssText, bool designTimeOnly)
        {
            if (Element != null)
            {
                Interop.IHTMLStyle interop_IHTMLStyle = null;
                if (designTimeOnly)
                {
                    interop_IHTMLStyle = ((Interop.IHTMLElement2)Element).GetRuntimeStyle();
                }
                else
                {
                    interop_IHTMLStyle = Element.GetStyle();
                }
                if (interop_IHTMLStyle != null)
                {
                    try
                    {
                        interop_IHTMLStyle.SetCssText(cssText);
                    }
                    catch
                    {
                    }
                }
            }
        }

        # endregion

        # region IHTMLPainter Member

        void Interop.IHTMLPainter.Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, int leftUpdate, int topUpdate, int rightUpdate, int bottomUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
        {
            using (Graphics graphics = Graphics.FromHdc(hdc))
            {

                graphics.DrawImage(_controlGlyph, leftBounds - 5, topBounds + 1, 10, 10);
                if (Element != null)
                {
                    switch (Element.GetTagName())
                    {
                        case "Panel":
                            graphics.DrawRectangle(new Pen(Color.Gray), leftBounds, topBounds, rightBounds - leftBounds, bottomBounds - topBounds);
                            if (!IsControlDown())
                            {
                                //graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Blue), leftBounds - 1, topBounds - 1, rightBounds - leftBounds + 2, bottomBounds - topBounds + 2);
                                //graphics.DrawImage(_selectedGlyph, leftBounds + 16, topBounds -12, 16, 16);
                            }
                            break;
                    }
                }
            }
        }

        void Interop.IHTMLPainter.OnResize(int cx, int cy)
        {
        }

        void Interop.IHTMLPainter.GetPainterInfo(Interop.HTML_PAINTER_INFO htmlPainterInfo)
        {
            htmlPainterInfo.lFlags = 8194;
            htmlPainterInfo.lZOrder = 8;
            htmlPainterInfo.iidDrawObject = Guid.Empty;
            htmlPainterInfo.rcBounds = new Interop.RECT(0, 0, 0, 0);
        }

        void Interop.IHTMLPainter.HitTestPoint(int ptx, int pty, out bool pbHit, out int plPartID)
        {
            pbHit = false;
            plPartID = 0;
        }

        # endregion

        /// <summary>
        /// Get the elements container.
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        public Control GetElement(IHtmlEditor editor)
        {
            return _genericWebControl;
        }

        # region Behavior

        /// <summary>
        /// Defaults. Not for user code, just public to support the infrastructure.
        /// </summary>
        public Interop.IHTMLElementDefaults ElementDefaults
        {
            get
            {
                return ((Interop.IElementBehaviorSiteOM2)this._behaviorSite).GetDefaults();
            }
        }

        #region IControlDesignerTag Members

        ///<summary>
        ///Retrieves the value of the identified attribute on the tag.
        ///</summary>
        ///
        ///<returns>
        ///A string containing the value of the attribute.
        ///</returns>
        ///
        ///<param name="name">The name of the attribute.</param>
        public string GetAttribute(string name)
        {
            object oVal = GetAttribute(name, true);
            return (oVal == null) ? String.Empty : oVal.ToString();
        }

        ///<summary>
        ///Retrieves the HTML markup for the content of the tag.
        ///</summary>
        ///
        ///<returns>
        ///The HTML markup for the content of the tag.
        ///</returns>
        ///
        public string GetContent()
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///Deletes the specified attribute from the tag.
        ///</summary>
        ///
        ///<param name="name">The name of the attribute.</param>
        public void RemoveAttribute(string name)
        {
            RemoveAttribute(name, true);
        }

        ///<summary>
        ///Sets the value of the specified attribute and creates the attribute, if necessary.
        ///</summary>
        ///
        ///<param name="name">The attribute name.</param>
        ///<param name="value">The attribute value.</param>
        public void SetAttribute(string name, string value)
        {
            SetAttribute(name, value, true);
        }

        ///<summary>
        ///Sets the HTML markup for the content of the tag.
        ///</summary>
        ///
        ///<param name="content">The HTML markup for the content of the tag.</param>
        public void SetContent(string content)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///Sets the <see cref="P:System.Web.UI.Design.IControlDesignerTag.IsDirty"></see> property of the tag.
        ///</summary>
        ///
        ///<param name="dirty">The value for the <see cref="P:System.Web.UI.Design.IControlDesignerTag.IsDirty"></see> property.</param>
        public void SetDirty(bool dirty)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///Retrieves the complete HTML markup for the control, including the outer tags.
        ///</summary>
        ///
        ///<returns>
        ///The outer HTML markup for the control.
        ///</returns>
        ///
        public string GetOuterContent()
        {
            throw new NotImplementedException();
        }

        /////<summary>
        /////Gets a value indicating whether or not an attribute or the content of a tag has changed.
        /////</summary>
        /////
        /////<returns>
        /////true if the tag has changed; otherwise, false.
        /////</returns>
        /////
        //public bool IsDirty
        //{
        //    get { return isDirty; }
        //}

        #endregion

        /// <summary>
        /// Freezes the element so it's content is no longer editable.
        /// </summary>
        protected virtual void FreezeElement()
        {
            Interop.IHTMLElementDefaults defaults = this.ElementDefaults;
            defaults.SetFrozen(true);
            defaults.SetContentEditable("false");
            this._behaviorSite.RegisterNotification(4);
        }


        # endregion

        /// <summary>
        /// Forces refresh of current display.
        /// </summary>
        public void RefreshWebControl()
        {
            OnContentRefresh();
        }


        #region IDisposable Members

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _behaviorSite = null;
            _parent = null;
            _element = null;
            _genericWebControl = null;
        }

        #endregion

        #region IElementBehaviorLayout Members

        void Interop.IElementBehaviorLayout.GetSize(Interop.BEHAVIOR_LAYOUT_MODE dwFlags, Interop.tagSIZE sizeContent, ref Interop.POINT pptTranslateB, ref Interop.POINT pptTopLeft, Interop.tagSIZE psizeProposed)
        {

        }

        int Interop.IElementBehaviorLayout.GetLayoutInfo()
        {
            return (int)(Interop.BEHAVIOR_LAYOUT_INFO.BEHAVIORLAYOUTINFO_MAPSIZE | Interop.BEHAVIOR_LAYOUT_INFO.BEHAVIORLAYOUTINFO_MODIFYNATURAL); //: Modifyslightly
        }

        void Interop.IElementBehaviorLayout.GetPosition(Interop.BEHAVIOR_LAYOUT_MODE lFlags, ref Interop.POINT pptTopLeft)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void Interop.IElementBehaviorLayout.MapSize(Interop.tagSIZE pSizeIn, Interop.RECT prcOut)
        {
            prcOut.top = 0;
            prcOut.left = 0;
            prcOut.bottom = pSizeIn.cy;
            prcOut.right = pSizeIn.cx;
        }

        #endregion

        #region IHTMLPainterEventInfo Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plEventInfoFlags"></param>
        public void GetEventInfoFlags(out int plEventInfoFlags)
        {
            plEventInfoFlags = 0;
        }

        /// <summary>
        /// The method or operation is not implemented.
        /// </summary>
        /// <param name="ppElement"></param>
        public void GetEventTarget(Interop.IHTMLElement ppElement)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }
        /// <summary>
        /// The method or operation is not implemented.
        /// </summary>
        /// <param name="lPartID"></param>
        public void SetCursor(int lPartID)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// The method or operation is not implemented.
        /// </summary>
        /// <param name="lPartID"></param>
        /// <param name="pbstrPart"></param>
        public void StringFromPartID(int lPartID, out string pbstrPart)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        #endregion
    }

}
