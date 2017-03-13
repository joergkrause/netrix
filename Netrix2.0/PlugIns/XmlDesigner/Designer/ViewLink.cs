using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

using GuruComponents.Netrix;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Collections.Generic;
using GuruComponents.Netrix.WebEditing.Styles;
using System.Windows.Forms;

namespace GuruComponents.Netrix.XmlDesigner
{

    /// <summary>
    /// This class creates the design time HTML for XML control, which are linked to the underlying HTML designer.
    /// </summary>
    public class ViewLink : IDesignTimeBehavior, IDisposable
    {
        private IHtmlEditor _editor;
        private IDesignerHost _designerHost;
        private Interop.IElementBehaviorSite _behaviorSite;
        private Interop.IHTMLElement _element;
        private Interop.IHTMLElement _viewElement;
        private Interop.IHTMLDocument baseDocument;
        private Interop.IHTMLElementDefaults elementDefaults;
        private ControlDesigner _designer;
        private EventSink _eventSink;
        private XmlControl _control;
        private bool _savingContents;
        private bool _active = false;
        private bool EventsEnabled = true;

        internal bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        /// <summary>
        /// Name of behavior.
        /// </summary>
        [DesignOnly(true), Browsable(false)]
        public string Name
        {
            get
            {
                return "XML#Designer";
            }
        }

        /// <summary>
        /// Editor this element belongs to.
        /// </summary>
        [DesignOnly(true), Browsable(false)]
        public IHtmlEditor Editor
        {
            get
            {
                return _editor;
            }
        }

        internal Interop.IHTMLElement Element
        {
            get
            {
                return _element;
            }
            //set
            //{
            //    _element = value;
            //    if (_control == null)
            //    {
            //        CreateControlAndDesigner();
            //        CreateControlView();
            //    }
            //    ConnectToControlAndDesigner();
            //    if (_control != null)
            //    {
            //        ((Interop.IHTMLDOMNode)_control.GetBaseElement()).replaceNode((Interop.IHTMLDOMNode)_element);
            //    }
            //}
        }

        private IServiceProvider ServiceProvider
        {
            get
            {
                return Editor.ServiceProvider;
            }
        }

        private DesignTimeBehavior _behavior;
        
        /// <summary>
        /// Attached behavior.
        /// </summary>
        public DesignTimeBehavior Behavior
        {
            get { return _behavior; }
            set { _behavior = value; }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="behavior"></param>
        public ViewLink(IHtmlEditor editor, DesignTimeBehavior behavior)
        {
            this._editor = editor;
            this._behavior = behavior;
            _designerHost = editor.ServiceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost; 

        }
        
        private void ConnectToControlAndDesigner()
        {
            // Connect event sink
            if (_eventSink == null && _control != null)
            {
                _eventSink = new EventSink(this);
                _eventSink.Connect(_control, Element, _editor); 
                _eventSink.ElementEvent += new ElementEventHandler(_eventSink_ElementEvent);
            }
            _designerHost.LoadComplete -= new EventHandler(this.OnDesignerHostLoadComplete);
            _designerHost.LoadComplete += new EventHandler(this.OnDesignerHostLoadComplete);
        }


        void _eventSink_ElementEvent(object sender, Interop.IHTMLEventObj e)
        {
            if (!EventsEnabled) return;
            //System.Diagnostics.Debug.WriteLine(e.type, e.srcElement.GetTagName());
            switch (e.type)
            {
                case "controlselect":
                    break;
                case "dragstart":
                    isDragging = true;
                    break;
                case "dragend":
                    isDragging = false;
                    break;
            }
            // cancelling the bubble prevents us from getting this for the parent too.
            //e.cancelBubble = true;
        }

        private void CreateControlAndDesigner()
        {
            bool parseError;
            XmlControl c = ParseControl(out parseError) as XmlControl;
            if (!parseError && c != null)
            {                
                _control = (XmlControl) c;
                //_control.PropertyChanged += new PropertyChangedEventHandler(_control_PropertyChanged);                
                IDesigner d = _designerHost.GetDesigner(_control);
                if (d == null)
                {
                    _designerHost.Container.Add(_control, _control.ID);
                    d = _designerHost.GetDesigner(_control);
                }
                if (d is ControlDesigner)
                {
                    Designer = (ControlDesigner) d;                
                    Designer.Behavior = ((IHtmlControlDesignerBehavior) this);
                }
            }
        }

        /// <summary>
        /// Attached element designer.
        /// </summary>
        [DesignOnly(true), Browsable(false)]
        public HtmlControlDesigner Designer
        {
            get
            {
                return _designer;
            }
            set
            {
                _designer = (ControlDesigner)value;
            }
        }

        // CommandTarget
        Interop.OLEVARIANT nil = new Interop.OLEVARIANT();
        Interop.OLEVARIANT arg = new Interop.OLEVARIANT();

        private void ExecuteCommand(object state)
        {
            int commandId = Convert.ToInt32(state);
            Interop.IOleCommandTarget CommandTarget = (Interop.IOleCommandTarget) baseDocument;
            CommandTarget.Exec(ref Interop.Guid_MSHTML, commandId, (int)Interop.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, arg, nil);
        }

        // respect core controls designmodeenabled prop
        private bool InheritedDesignModeEnabled(bool designModeRequested)
        {
            return designModeRequested && Editor.DesignModeEnabled;
        }

        /// <summary>
        /// Creates the viewlink document.
        /// </summary>
        private void CreateControlView()
        {
            Interop.IHTMLDocument2 originDocument = (Interop.IHTMLDocument2)Element.GetDocument();
            Interop.IHTMLElement htmlElement = originDocument.CreateElement("HTML");
            Interop.IHTMLElement headElement = originDocument.CreateElement("HEAD");
            Interop.IHTMLElement bodyElement = originDocument.CreateElement("BODY");
            ((Interop.IHTMLElement2)htmlElement).InsertAdjacentElement("beforeBegin", headElement);
            ((Interop.IHTMLElement2)htmlElement).InsertAdjacentElement("afterBegin", bodyElement);
            _viewElement = bodyElement;
            _viewElement.SetAttribute("tabIndex", 1000, 0);
            baseDocument = (Interop.IHTMLDocument)_viewElement.GetDocument();
            try
            {
                elementDefaults = ((Interop.IElementBehaviorSiteOM2)_behaviorSite).GetDefaults();
            }
            catch
            {
                throw new ApplicationException("Wrong usage - acces to not properly attached master element");
            }
            XmlElementDesigner elementDesigner = Designer as XmlElementDesigner;
            if (_control == null || elementDesigner == null)
            {
                _viewElement.SetInnerHTML(String.Format(@"<div style=""border:dotted 1px red;background:Silver;width:220px;height:40px""><b>Error</b>:&nbsp;Element does not have a Designer attached (Element {0}:{1})</div>",
                    ((Interop.IHTMLElement2)_element).GetScopeName(),
                    _element.GetTagName()));
                ((Interop.IHTMLDocument2)baseDocument).SetDesignMode("Off");
                elementDefaults.SetViewLink(baseDocument);
                return;
            }
            else
            {
                _viewElement.SetInnerHTML(elementDesigner.GetDesignTimeHtml());
                elementDefaults.SetViewLink(baseDocument);
            }
            //done, set reference
            _control.ViewElementDefaults = elementDefaults;
            // end commands do basic document
            if (elementDesigner.DisableEditFocus)
            {
                //System.Threading.ThreadPool.QueueUserWorkItem(ExecuteCommand, (int)Interop.IDM.DISABLE_EDITFOCUS_UI);
            }

            // get viewlink specific properties from control element
            elementDefaults.SetFrozen(elementDesigner.FrozenEvents);    // true = event handler returns only master; false = event returns designtime html
            ((Interop.IHTMLElement3)_viewElement).contentEditable = InheritedDesignModeEnabled(_control.ContentEditable) ? "true" : "false";
            elementDefaults.SetViewMasterTab(elementDesigner.ViewMasterTab);
            elementDefaults.SetTabStop(elementDesigner.TabStop);
            elementDefaults.SetCanHaveHTML(elementDesigner.CanHaveHtml);
            elementDefaults.SetIsMultiLine(true);

            Interop.IHTMLStyle style = ((Interop.IHTMLElement2)_viewElement).GetRuntimeStyle();
            ElementStyle runtimeStyle = new ElementStyle(style);
            elementDesigner.SetRuntimeStyle(runtimeStyle);
            // assure that we don't see any unexpected scrollbars during resize operations
            ((Interop.IHTMLElement2)_element).GetRuntimeStyle().SetOverflow("hidden");
            object w = ((Interop.IHTMLElement)_element).GetStyle().GetWidth();
            elementDesigner.OnSetComponentDefaults();
            try
            {
                Interop.IHTMLDocument2 baseDocument2 = (Interop.IHTMLDocument2)baseDocument;
                int numSheets = 0;
                Interop.IHTMLStyleSheetsCollection baseDocumentStylesheets = originDocument.GetStyleSheets();
                if (baseDocumentStylesheets != null)
                {
                    numSheets = baseDocumentStylesheets.Length;
                }
                for (int j = 0; j < numSheets; j++)
                {
                    object local = j;
                    Interop.IHTMLStyleSheet sheetItem = (Interop.IHTMLStyleSheet)baseDocumentStylesheets.Item(local);
                    if (sheetItem != null)
                    {
                        int k = 0;
                        Interop.IHTMLStyleSheetRulesCollection rules = sheetItem.GetRules();
                        if (rules != null)
                        {
                            k = rules.GetLength();
                        }
                        if (k != 0)
                        {
                            Interop.IHTMLStyleSheet newSheet = baseDocument2.CreateStyleSheet(String.Empty, 0);
                            for (int i2 = 0; i2 < k; i2++)
                            {
                                Interop.IHTMLStyleSheetRule newRule = rules.Item(i2);
                                if (newRule != null)
                                {
                                    string selector = newRule.GetSelectorText();
                                    string content = newRule.GetStyle().cssText;
                                    newSheet.AddRule(selector, content, i2);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            // once we have the viewlink created, establish a backlink
            elementDesigner.AssociatedViewLink = this;
        }

        internal void OnBehaviorDetach()
        {
            if (_eventSink != null)
            {
                _eventSink.Disconnect();
                _eventSink = null;
            }
            _element = null;
            _viewElement = null;
            _behaviorSite = null;
            //_editor = null;
        }


        internal void OnBehaviorInit(Interop.IElementBehaviorSite behaviorSite)
        {
            _behaviorSite = behaviorSite;
        }

        bool _isReady = false;

        /// <summary>
        /// True if element rendering is done.
        /// </summary>
        public bool IsReady
        {
            get { return _isReady; }
            set { _isReady = value; }
        }

        internal void OnContentReady(Interop.IHTMLElement element)
        {
            _element = element;
            //            System.Diagnostics.Debug.WriteLine("OnContentReady: " + _element.GetTagName());
            Interop.IHTMLElement parentElement = Element.GetParentElement();
            if (_parent == null && parentElement != null)
            {
                string str = (String)this.GetAttribute("id", true);
                if (str != null)
                {
                    IComponent iComponent = _designerHost.Container.Components[str];
                    if (iComponent != null && iComponent is XmlControl)
                    {
                        XmlControl control = (XmlControl)iComponent;
                        ControlDesigner controlDesigner = (ControlDesigner)_designerHost.GetDesigner(_control);
                        if (controlDesigner != null)
                        {
                            DesignTimeBehavior behavior = (DesignTimeBehavior)controlDesigner.Behavior;
                            if (behavior != null && IsDragging())
                            {
                                this.EndDrag();
                                //_control.WebControl = control;
                                Designer = controlDesigner;
                                CreateControlView();
                                SetControlParent(parentElement);
                                ConnectToControlAndDesigner();
                            }
                        }
                    }
                }
                if (!isDragging)
                {
                    CreateControlAndDesigner();
                    CreateControlView();
                    SetControlParent(parentElement);
                    ConnectToControlAndDesigner();
                }
            }
        }

        private bool isDragging;
        private bool IsDragging()
        {
            return isDragging;
        }
        private void EndDrag()
        {
            isDragging = false;
        }

        internal void OnContentSave()
        {
            if (_savingContents || _viewElement == null || DesignTimeElementView == null || _designerHost == null || _control == null)
            {
                return;
            }
            try
            {
            	_savingContents = true;
                _designer.UpdateDesignTimeHtml();                
                EmbeddedPersister.PersistControl(Element, _control, _designerHost);
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
            _designerHost.LoadComplete -= new EventHandler(this.OnDesignerHostLoadComplete);            
        }

        internal void OnDocumentContextChanged()
        {
        }

        private System.Web.UI.Control ParseControl(out bool parseError)
        {            
            string str1 = _element.GetOuterHTML();
            //System.Diagnostics.Debug.WriteLine(str1, "ParseControl: ");
            System.Web.UI.Control control = null;
            parseError = false;
            try
            {
                // take element and let factory create element object from registration
                control = Editor.GenericElementFactory.CreateElement(_element);
            }
            catch (Exception)
            {
                parseError = true;
            }
            if (!(control is XmlControl))
            {
                parseError = true;
            }
            return control;
        }

        private void SetContainedControlsParent(Interop.IHTMLElement element)
        {
            Interop.IHTMLElementCollection elementChildren = (Interop.IHTMLElementCollection)element.GetChildren();
            if (elementChildren != null)
            {
                int i = elementChildren.GetLength();
                for (int j = 0; j < i; j++)
                {
                    Interop.IHTMLElement childElement = (Interop.IHTMLElement)elementChildren.Item(j, j);
                    if (childElement != null)
                    {
                        object[] locals = new object[1];
                        childElement.GetAttribute("id", 0, locals);
                        string str2 = locals[0] as String;
                        System.Web.UI.Control control = _designerHost.Container.Components[str2] as System.Web.UI.Control;
                        if (control != null && control.Parent != _control)
                        {
                            _control.Controls.Add(control);
                            ControlDesigner controlDesigner = _designerHost.GetDesigner(control) as ControlDesigner;
                            if (controlDesigner != null)
                            {
                                controlDesigner.OnSetParent();
                            }
                        }
                        else
                        {
                            SetContainedControlsParent(childElement);
                        }
                    }
                }
            }
        }

        Interop.IHTMLElement _parent;

        private void SetControlParent(Interop.IHTMLElement newParent)
        {
            //return;
            try
            {
                XmlControl control;

                _parent = newParent;
                Interop.IHTMLElement parentElement = newParent;
                object[] locals = new object[1];
                for (control = null; control == null && parentElement != null; parentElement = parentElement.GetParentElement())
                {                    
                    parentElement.GetAttribute("id", 0, locals);
                    string str2 = locals[0] as String;
                    control = _designerHost.Container.Components[str2] as XmlControl;
                }
                if (_control != null)
                {
                    if (_control.Parent != control)
                    {
                        control.Controls.Add(_control);
                        if (((ControlDesigner)Designer) != null)
                        {
                            ((ControlDesigner)Designer).OnSetParent();                            
                            if (!((ControlDesigner)Designer).ReadOnly)                                
                            {
                                _control.Controls.Clear();
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

        # region IControlDesignerBehavior

        /// <summary>
        /// Returns the view element.
        /// </summary>
        [DesignOnly(true), Browsable(false)]
        public object DesignTimeElementView
        {
            get
            {
				if (_viewElement == null) return null;
                object o = ((Interop.IHTMLDOMNode)_viewElement).firstChild;
                return o;
            }
        }

        /// <summary>
        /// Returns the design time HTML.
        /// </summary>
        [DesignOnly(true), Browsable(false)]
        public string DesignTimeHtml
        {
            get
            {
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

        public void OnTemplateModeChanged()
        {
        }

        # endregion

        # region IHtmlControlDesignerBehavior Member

        /// <summary>
        /// Returns the design time element.
        /// </summary>
        [DesignOnly(true), Browsable(false)]        
        public object DesignTimeElement
        {
            get
            {
                return _viewElement;
            }
        }

        /// <summary>
        /// Get attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public object GetAttribute(string attribute, bool ignoreCase)
        {
            Interop.IHTMLElement element = (Interop.IHTMLElement)Element;  //DesignTimeElementView; // (Interop.IHTMLElement) ((Interop.IHTMLDOMNode) _viewElement).firstChild;
            if (element == null)
            {
                return null;
            }
            object[] locals1 = new object[1];
            try
            {
                element.GetAttribute(attribute, !ignoreCase ? 1 : 0, locals1);
                object local = locals1[0];
				if (local is Interop.IHTMLStyle)
				{
					return ((Interop.IHTMLStyle) local).GetCssText();
				}
                return local;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get style
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="designTimeOnly"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public object GetStyleAttribute(string attribute, bool designTimeOnly, bool ignoreCase)
        {
            Interop.IHTMLStyle style = GetExactStyle(designTimeOnly);
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
            return null;
        }

        private Interop.IHTMLStyle GetExactStyle(bool designTimeOnly)
        {
            Interop.IHTMLElement element = (Interop.IHTMLElement)DesignTimeElementView;
            if (element != null)
            {

                if (designTimeOnly)
                {
                    return ((Interop.IHTMLElement2)element).GetRuntimeStyle();
                }
                else
                {
                    return element.GetStyle();
                }
            }
            return null;
        }

        /// <summary>
        /// Remove attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="ignoreCase"></param>
        public void RemoveAttribute(string attribute, bool ignoreCase)
        {
			Interop.IHTMLElement element = (Interop.IHTMLElement)DesignTimeElementView; // (Interop.IHTMLElement) ((Interop.IHTMLDOMNode) _viewElement).firstChild;
            if (element != null)
            {
                try
                {
                    element.RemoveAttribute(attribute, !ignoreCase ? 1 : 0);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Remove style attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="designTimeOnly"></param>
        /// <param name="ignoreCase"></param>
        public void RemoveStyleAttribute(string attribute, bool designTimeOnly, bool ignoreCase)
        {
            Interop.IHTMLStyle style = GetExactStyle(designTimeOnly);
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

        /// <summary>
        /// Set attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="val"></param>
        /// <param name="ignoreCase"></param>
        public void SetAttribute(string attribute, object val, bool ignoreCase)
        {
			Interop.IHTMLElement element = (Interop.IHTMLElement)DesignTimeElementView; // (Interop.IHTMLElement) ((Interop.IHTMLDOMNode) _viewElement).firstChild;
            if (element != null)
            {
                try
                {
                    element.SetAttribute(attribute, val, !ignoreCase ? 1 : 0);
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// Set style attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="designTimeOnly"></param>
        /// <param name="val"></param>
        /// <param name="ignoreCase"></param>
        public void SetStyleAttribute(string attribute, bool designTimeOnly, object val, bool ignoreCase)
        {
            Interop.IHTMLStyle style = GetExactStyle(designTimeOnly);
            if (style != null)
            {
                try
                {
                    style.SetAttribute(attribute, val, !ignoreCase ? 1 : 0);
                }
                catch
                {
                }
            }
        }

        # endregion

        #region IDisposable Members

        public void Dispose()
        {
             _editor = null;
             _designerHost = null;
             _behaviorSite = null;
             _element = null;
             _viewElement = null;
             _designer = null;
             _eventSink = null;
             _control = null; 
        }

        #endregion

        # region Binary Behavior Support

        # region Binary Bahavior Features Offered to Designers

        public void InvalidateRegion()
        {
            ((Interop.IHTMLPaintSite)_behaviorSite).InvalidateRegion(IntPtr.Zero);
        }

        /// <summary>
        /// Converts a point's coordinates, expressed relative to the top left corner of the client area, to coordinates relative to the top left of the element to which a rendering behavior is attached.
        /// </summary>
        /// <param name="global">Structure that specifies the point to convert to the element's coordinate system.</param>
        /// <returns></returns>
        public Point TransformGlobalToLocal(Point global)
        {
            Interop.POINT pLocal, pGlobal;
            pGlobal = new Interop.POINT();
            pLocal = new Interop.POINT();
            pGlobal.x = global.X;
            pGlobal.y = global.Y;
            ((Interop.IHTMLPaintSite)_behaviorSite).TransformGlobalToLocal(pGlobal, out pLocal);
            return new Point(pLocal.x, pLocal.y);
        }

        /// <summary>
        /// Converts a point's coordinates, expressed relative to the top left corner of the element to which a rendering behavior is attached, to coordinates relative to the top left of the client area.
        /// </summary>
        /// <param name="local">Structure that specifies the point to convert to the client area's coordinate system.</param>
        /// <returns></returns>
        public Point TransformLocalToGlobal(Point local)
        {
            Interop.POINT pLocal, pGlobal;
            pGlobal = new Interop.POINT();
            pLocal = new Interop.POINT();
            pLocal.x = local.X;
            pLocal.y = local.Y;
            ((Interop.IHTMLPaintSite)_behaviorSite).TransformLocalToGlobal(pLocal, out pGlobal);
            return new Point(pGlobal.x, pGlobal.y);
        }

        # endregion Binary Bahavior Features Offered to Designers

        # region Binary Bahavior Features implemented by Designers

        internal void Draw(Graphics gr, Rectangle area)
        {
            ((XmlElementDesigner)Designer).Draw(gr, area);
        }

        internal void OnResize(int cx, int cy)
        {
            if (Designer == null) return; // during drag drop it's null but still this callback gets called
            ((XmlElementDesigner)Designer).OnResize(new Size(cx, cy));
        }

        internal Interop.HTML_PAINTER_INFO GetPainterInfo()
        {
            if (Designer == null) return null; // during drag drop it's null but still this callback gets called
            Interop.HTML_PAINTER_INFO htmlPainterInfo = new Interop.HTML_PAINTER_INFO();
            htmlPainterInfo.lFlags = (int) ((XmlElementDesigner)Designer).HtmlPaintFlag;
            htmlPainterInfo.lZOrder = (int) ((XmlElementDesigner)Designer).HtmlZOrderFlag;
            htmlPainterInfo.iidDrawObject = Guid.Empty;
            htmlPainterInfo.rcBounds = ((XmlElementDesigner)Designer).BorderZoneInternal(); ;
            return htmlPainterInfo;
        }

        internal void HitTestPoint(int ptx, int pty, out bool pbHit, out int plPartID)
        {
            Point hitTest = new Point(ptx, pty);
            hitTest.X -= ((XmlElementDesigner)Designer).BorderZone().Location.X;
            hitTest.Y -= ((XmlElementDesigner)Designer).BorderZone().Location.Y;
            pbHit = ((XmlElementDesigner)Designer).HitTestPoint(hitTest, out plPartID);
        }

        internal Rectangle BorderZone
        {
            get { return ((XmlElementDesigner)Designer).BorderZone(); }
        }

        internal void SetMousePointer(int lPartID)
        {
            Cursor.Current = ((XmlElementDesigner)Designer).SetMousePointer(lPartID);
        }

        # endregion Binary Bahavior Features implemented by Designers

        # endregion
   }

}

