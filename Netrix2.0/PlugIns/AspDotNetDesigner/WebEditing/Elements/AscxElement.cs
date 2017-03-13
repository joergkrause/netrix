using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.Designer;
using System.Collections;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using System.Web.UI.Design;
using System.Web.UI;
using System.Drawing;
using GuruComponents.Netrix.AspDotNetDesigner;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// A generic class for access to methods which can't handle System.Web.UI.Control based elements.
    /// </summary>
    /// <remarks>
    /// Internally this class is used to resolve and show User Controls loaded from ASCX files.
    /// </remarks>
    public class AscxElement : System.Web.UI.UserControl, IElement
    {

        private IHtmlEditor editor;
        private Interop.IHTMLElement peer;
        private Interop.IHTMLElement2 peer2;
        private Interop.IHTMLElement3 peer3;
        private DesignTimeBehavior behavior;
        private IElementBehavior elementBehavior;

        /// <summary>
        /// Behavior 
        /// </summary>
        [Browsable(false)]
        public DesignTimeBehavior Behavior
        {
            get { return behavior; }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public AscxElement() : base()
        {
        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="peer"></param>
        internal AscxElement(IHtmlEditor editor, Interop.IHTMLElement peer) : this(editor, peer, null)
        {

        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="peer"></param>
        /// <param name="behavior"></param>
        internal AscxElement(IHtmlEditor editor, Interop.IHTMLElement peer, DesignTimeBehavior behavior)
        {
            this.editor = editor;
            this.peer = peer;
            this.peer2 = (Interop.IHTMLElement2)peer;
            this.peer3 = (Interop.IHTMLElement3)peer;
            this.behavior = behavior;
        }

        /// <summary>
        /// Add peer. This is internally used und shall never be called from user code.
        /// </summary>
        /// <param name="element"></param>
        protected internal void AssociatePeer(Interop.IHTMLElement element)
        {
            this.peer = element;
            this.peer2 = (Interop.IHTMLElement2)element;
            this.peer3 = (Interop.IHTMLElement3)element;
            this.behavior = null;
        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="element"></param>
        /// <param name="behavior"></param>
        internal protected void AssociatePeer(Interop.IHTMLElement element, DesignTimeBehavior behavior)
        {
            this.peer = element;
            this.peer2 = (Interop.IHTMLElement2)element;
            this.peer3 = (Interop.IHTMLElement3)element;
            this.behavior = behavior;
        }

        #region IElement Members

        event DocumentEventHandler IElement.LoseCapture { add { } remove { } }

        event DocumentEventHandler IElement.Click{ add{} remove{} }

        event DocumentEventHandler IElement.DblClick{ add{} remove{} }

        event DocumentEventHandler IElement.DragStart{ add{} remove{} }

        event DocumentEventHandler IElement.Focus{ add{} remove{} }

        event DocumentEventHandler IElement.Drop{ add{} remove{} }

        event DocumentEventHandler IElement.Blur{ add{} remove{} }

        event DocumentEventHandler IElement.DragOver{ add{} remove{} }

        event DocumentEventHandler IElement.DragEnter{ add{} remove{} }

        event DocumentEventHandler IElement.DragLeave{ add{} remove{} }

        event DocumentEventHandler IElement.KeyDown{ add{} remove{} }

        event DocumentEventHandler IElement.KeyPress{ add{} remove{} }

        event DocumentEventHandler IElement.KeyUp{ add{} remove{} }

        event DocumentEventHandler IElement.MouseDown{ add{} remove{} }

        event DocumentEventHandler IElement.ResizeStart{ add{} remove{} }

        event DocumentEventHandler IElement.ResizeEnd{ add{} remove{} }

        event DocumentEventHandler IElement.MouseEnter{ add{} remove{} }

        event DocumentEventHandler IElement.MouseLeave{ add{} remove{} }

        event DocumentEventHandler IElement.MouseMove{ add{} remove{} }

        event DocumentEventHandler IElement.MouseOut{ add{} remove{} }

        event DocumentEventHandler IElement.MouseOver{ add{} remove{} }

        event DocumentEventHandler IElement.MouseUp{ add{} remove{} }

        event DocumentEventHandler IElement.SelectStart{ add{} remove{} }

        event DocumentEventHandler IElement.LayoutComplete{ add{} remove{} }

        event DocumentEventHandler IElement.Load{ add{} remove{} }

        event DocumentEventHandler IElement.MouseWheel{ add{} remove{} }

        event DocumentEventHandler IElement.MoveEnd{ add{} remove{} }

        event DocumentEventHandler IElement.MoveStart{ add{} remove{} }

        event DocumentEventHandler IElement.Activate{ add{} remove{} }

        event DocumentEventHandler IElement.BeforeActivate{ add{} remove{} }

        event DocumentEventHandler IElement.BeforeCopy{ add{} remove{} }

        event DocumentEventHandler IElement.BeforeCut{ add{} remove{} }

        event DocumentEventHandler IElement.BeforePaste{ add{} remove{} }

        event DocumentEventHandler IElement.ContextMenu{ add{} remove{} }

        event DocumentEventHandler IElement.Copy{ add{} remove{} }

        event DocumentEventHandler IElement.Cut{ add{} remove{} }

        event DocumentEventHandler IElement.Deactivate{ add{} remove{} }

        event DocumentEventHandler IElement.Drag{ add{} remove{} }

        event DocumentEventHandler IElement.DragEnd{ add{} remove{} }

        event DocumentEventHandler IElement.FocusIn{ add{} remove{} }

        event DocumentEventHandler IElement.FocusOut{ add{} remove{} }

        event DocumentEventHandler IElement.FilterChange{ add{} remove{} }

        event DocumentEventHandler IElement.Abort{ add{} remove{} }

        event DocumentEventHandler IElement.Change{ add{} remove{} }

        event DocumentEventHandler IElement.Select{ add{} remove{} }

        event DocumentEventHandler IElement.SelectionChange{ add{} remove{} }

        event DocumentEventHandler IElement.Stop{ add{} remove{} }

        event DocumentEventHandler IElement.BeforeDeactivate{ add{} remove{} }

        event DocumentEventHandler IElement.ControlSelect{ add{} remove{} }

        event DocumentEventHandler IElement.EditFocus{ add{} remove{} }

        event DocumentEventHandler IElement.Error{ add{} remove{} }

        event DocumentEventHandler IElement.Move{ add{} remove{} }

        event DocumentEventHandler IElement.Paste{ add{} remove{} }

        event DocumentEventHandler IElement.PropertyChange{ add{} remove{} }

        event DocumentEventHandler IElement.Resize{ add{} remove{} }

        event DocumentEventHandler IElement.Scroll{ add{} remove{} }

        event DocumentEventHandler IElement.Paged { add{} remove{} }

        [Browsable(false)]
        IExtendedProperties IElement.ExtendedProperties
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        [Browsable(false)]
        string IElement.UniqueName
        {
            get { return ""; }
        }

        /// <summary>
        /// Selectable
        /// </summary>
        /// <returns></returns>
        public bool IsSelectable()
        {
            return editor.Selection.IsSelectableElement(this);
        }

        /// <summary>
        /// Position style.
        /// </summary>
        [Browsable(false)]
        public bool IsAbsolutePositioned
        {
            get { return peer.GetStyle().GetPosition().Equals("absolute"); }
        }

        [Browsable(false)]
        bool IElement.IsTextEdit
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Insert an element
        /// </summary>
        /// <param name="method"></param>
        /// <param name="element"></param>
        public void InsertAdjacentElement(InsertWhere method, IElement element)
        {
            if (element == null) return;
            peer2.InsertAdjacentElement(method.ToString(), element.GetBaseElement());
        }

        /// <summary>
        /// Insert Html
        /// </summary>
        /// <param name="method"></param>
        /// <param name="html"></param>
        public void InsertAdjacentHtml(InsertWhere method, string html)
        {
            peer.InsertAdjacentHTML(method.ToString(), html);
        }

        /// <summary>
        /// Insert text.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="text"></param>
        public void InsertAdjacentText(InsertWhere method, string text)
        {
            peer.InsertAdjacentText(method.ToString(), text);
        }

        /// <summary>
        /// Get area of element.
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Rectangle GetAbsoluteArea()
        {
            return new Rectangle(peer2.GetClientLeft(), peer2.GetClientTop(), peer2.GetClientWidth(), peer2.GetClientHeight());
        }

        /// <summary>
        /// The peer element (generic base element)
        /// </summary>
        /// <returns></returns>
        public Interop.IHTMLElement GetBaseElement()
        {
            return peer;
        }

        /// <summary>
        /// Editor reference.
        /// </summary>
        [Browsable(false)]
        public IHtmlEditor HtmlEditor
        {
            get
            {
                return editor;
            }
            set
            {
                editor = value;
            }
        }

        /// <summary>
        /// Is editable in browse mode.
        /// </summary>
        [Browsable(false)]
        public bool ContentEditable
        {
            get
            {
                return (peer3 == null) ? false : peer3.contentEditable.Equals("true");
            }
            set
            {
                peer3.contentEditable = (value) ? "true" : "false";
            }
        }

        /// <summary>
        /// Supports atomic selection.
        /// </summary>
        public bool AtomicSelection
        {
            get
            {
                string sel = GetStringAttribute("ATOMICSELECTION");
                return (sel.ToLower().Equals("true"));
            }
            set
            {
                SetStringAttribute("ATOMICSELECTION", value ? "true" : "false");
            }
        }

        /// <summary>
        /// Is selectable
        /// </summary>
        public bool Unselectable
        {
            get
            {
                string sel = GetStringAttribute("UNSELECTABLE");
                return (sel.ToLower().Equals("on"));
            }
            set
            {
                SetStringAttribute("UNSELECTABLE", value ? "on" : "off");
            }
        }

        /// <summary>
        /// Access to the binary behavior manager at element level.
        /// </summary>
        [Browsable(false)]
        public IElementBehavior ElementBehaviors
        {
            get
            {
                if (elementBehavior == null)
                {
                    elementBehavior = new ElementBehavior(this);
                }
                return elementBehavior;
            }
        }

        /// <summary>
        /// Not supported
        /// </summary>
        [Browsable(false)]
        string IElement.InnerHtml
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        /// <summary>
        /// Thre outer HTML. Set path not implemented.
        /// </summary>
        [Browsable(false)]
        public string OuterHtml
        {
            get
            {
                return peer.GetOuterHTML();
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        [Browsable(false)]
        string IElement.InnerText
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        /// <summary>
        /// Tag's name.
        /// </summary>
        public string TagName
        {
            get { return (peer == null) ? "UserControl" : peer.GetTagName(); }
        }

        /// <summary>
        /// Self reference.
        /// </summary>
        [Browsable(false)]
        public IElement TagElement
        {
            get { return this; }
        }

        /// <summary>
        /// Alias for designer
        /// </summary>
        public string Alias
        {
            get { return (peer2 == null) ? "uc" : peer2.GetScopeName(); }
        }

        /// <summary>
        /// Name of element
        /// </summary>
        public string ElementName
        {
            get { return String.Format("{0}:{1}", Alias, TagName); }
        }

        Control IElement.GetChild(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        Control IElement.GetChild(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        ElementCollection IElement.GetChildren()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        Control IElement.GetParent()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        public string GetStyle()
        {
            return peer.GetStyle().GetCssText();
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="CssText"></param>
        public void SetStyle(string CssText)
        {
            peer.GetStyle().SetCssText(CssText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleName"></param>
        /// <returns></returns>
        public string GetStyleAttribute(string styleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void IElement.SetStyleAttribute(string styleName, string styleText)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void IElement.RemoveStyleAttribute(string styleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public void SetAttribute(string attribute, object value)
        {
            SetNativeAttribute(attribute, value);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="attribute"></param>
        public void RemoveAttribute(string attribute)
        {
            peer.RemoveAttribute(attribute, 0);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public object GetAttribute(string attribute)
        {
            return GetNativeAttribute(attribute);
        }

        [Browsable(false)]
        IElementDom IElement.ElementDom
        {
            get { throw new NotImplementedException("The method or operation is not implemented."); }
        }

        [Browsable(false)]
        GuruComponents.Netrix.WebEditing.Styles.IEffectiveStyle IElement.EffectiveStyle
        {
            get { throw new NotImplementedException("The method or operation is not implemented."); }
        }

        [Browsable(false)]
        GuruComponents.Netrix.WebEditing.Styles.IElementStyle IElement.RuntimeStyle
        {
            get { throw new NotImplementedException("The method or operation is not implemented."); }
        }

        [Browsable(false)]
        GuruComponents.Netrix.WebEditing.Styles.IElementStyle IElement.CurrentStyle
        {
            get { throw new NotImplementedException("The method or operation is not implemented."); }
        }

        #endregion

        # region internal methods

        /// <summary>
        /// Universal access to any attribute.
        /// </summary>
        /// <remarks>
        /// The type returned may vary depended on the internal type.
        /// </remarks>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>The object which is the value of the attribute.</returns>
        private object GetNativeAttribute(string attribute)
        {
            if (peer == null) return null;
            object local2;
            attribute = (attribute.Equals("http-equiv")) ? "httpequiv" : attribute;
            try
            {
                object[] locals = new object[1];
                locals[0] = null;
                peer.GetAttribute(attribute, 0, locals);
                object local1 = locals[0];
                if (local1 is DBNull)
                {
                    local1 = null;
                }
                local2 = local1;
            }
            catch
            {
                local2 = null;
            }
            return local2;
        }

        private void SetNativeAttribute(string attr, object value)
        {
            peer.SetAttribute(attr, value, 0);
        }

        /// <summary>
        /// Returns an attribute's value which is supposedly string.
        /// </summary>
        /// <param name="attribute">Name of attribute.</param>
        /// <returns>String representation of value.</returns>
        protected internal virtual string GetStringAttribute(string attribute)
        {
            return GetStringAttribute(attribute, String.Empty);
        }

        /// <summary>
        /// Returns an attribute's value which is supposedly string.
        /// </summary>
        /// <param name="attribute">Name of attribute.</param>
        /// <param name="defaultValue"></param>
        /// <returns>String representation of value.</returns>
        protected internal virtual string GetStringAttribute(string attribute, string defaultValue)
        {
            object local = GetAttribute(attribute);
            if (local == null)
            {
                return defaultValue;
            }
            if (local is String)
            {
                return (string)local;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        protected internal void SetStringAttribute(string attribute, string value)
        {
            SetStringAttribute(attribute, value, String.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        protected internal void SetStringAttribute(string attribute, string value, string defaultValue)
        {
            if (value == null || value.Equals(defaultValue))
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, value);
        }
# endregion


        [Browsable(false)]
        CssStyleCollection IElement.Style
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
