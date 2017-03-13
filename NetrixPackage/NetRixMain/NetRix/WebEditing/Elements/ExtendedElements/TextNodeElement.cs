using System;
using System.ComponentModel;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Styles;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This class represents a Text node for usage with the <see cref="ElementDom"/> class.
    /// </summary>
    public class TextNodeElement : IElement
    {

        Interop.IHTMLDOMNode node;
        IHtmlEditor htmleditor;

        internal TextNodeElement(Interop.IHTMLDOMNode node, IHtmlEditor htmlEditor)
        {
            this.node = node;
            this.htmleditor = htmlEditor;
        }

        /// <summary>
        /// For text nodes styles are not defined. FOR INTERNAL USE ONLY.
        /// </summary>
        /// <remarks>This property always throws a <see cref="NotImplementedException"/>. Property is there just to satisfy the interface.</remarks>
        System.Web.UI.CssStyleCollection IElement.Style
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

        public Interop.IHTMLElement GetBaseElement() 
        {
            return (Interop.IHTMLElement) node;
        }

        IElementBehavior IElement.ElementBehaviors { get { throw new MethodAccessException(); } }

        [Browsable(false)]
        public IHtmlEditor HtmlEditor
        {
            get
            {
                return htmleditor;
            }
            set
            {
                htmleditor = value;
            }
        }

        #region IElement Member

		/// <summary>
		/// Inserts an element at the specified location.
		/// </summary>
		/// <param name="method">Specifies where to insert the HTML element.</param>
		/// <param name="element">Element to be inserted adjacent to the object.</param>
		void IElement.InsertAdjacentElement(InsertWhere method, IElement element)
		{
			throw new NotImplementedException("");
		}

		/// <summary>
		/// Inserts the given HTML text into the element at the location.
		/// </summary>
		/// <param name="method"></param>
		/// <param name="html"></param>
		void IElement.InsertAdjacentHtml(InsertWhere method, string html)
		{
			throw new NotImplementedException("");
		}

		/// <summary>
		/// Inserts the given text into the element at the specified location.
		/// </summary>
		/// <param name="method">Specifies where to insert the text.</param>
		/// <param name="text"></param>
		void IElement.InsertAdjacentText(InsertWhere method, string text)
		{
			throw new NotImplementedException("");
		}

        public System.Drawing.Rectangle GetAbsoluteArea()
        {
            return System.Drawing.Rectangle.Empty;
        }

        [Browsable(false)]
        bool GuruComponents.Netrix.WebEditing.Elements.IElement.ContentEditable
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        [Browsable(false)]
        string GuruComponents.Netrix.WebEditing.Elements.IElement.InnerHtml
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        [Browsable(false)]
        string GuruComponents.Netrix.WebEditing.Elements.IElement.OuterHtml
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public string InnerText
        {
            get
            {
                return node.nodeValue.ToString();
            }
            set
            {
                node.nodeValue = value;
            }
        }

        public string TagName
        {
            get
            {
                return "#text";
            }
        }

        [Browsable(false), DesignOnly(true)]
        public string Alias
        {
            get
            {
                return String.Empty;
            }
        }

        [Browsable(false), DesignOnly(true)]
        public string ElementName
        {
            get
            {
                return TagName;
            }
        }

        [Browsable(false)]
        IElement GuruComponents.Netrix.WebEditing.Elements.IElement.TagElement
        {
            get
            {
                return null;
            }
        }

        System.Web.UI.Control GuruComponents.Netrix.WebEditing.Elements.IElement.GetChild(int index)
        {
            return null;
        }

        System.Web.UI.Control GuruComponents.Netrix.WebEditing.Elements.IElement.GetChild(string name)
        {
            return null;
        }

        ElementCollection GuruComponents.Netrix.WebEditing.Elements.IElement.GetChildren()
        {
            return null;
        }

        public System.Web.UI.Control GetParent()
        {
            Interop.IHTMLElement el = node.parentNode as Interop.IHTMLElement;
            System.Web.UI.Control element = null;
            if (el != null)
            {
                element = HtmlEditor.GenericElementFactory.CreateElement(el);
            }
            return element;
        }

        public new string ToString()
        {
            return node.nodeValue.ToString();
        }

        string GuruComponents.Netrix.WebEditing.Elements.IElement.GetStyle()
        {
            return null;
        }

        void GuruComponents.Netrix.WebEditing.Elements.IElement.SetStyle(string CssText)
        {
        }

        string GuruComponents.Netrix.WebEditing.Elements.IElement.GetStyleAttribute(string styleName)
        {
            return null;
        }

        void GuruComponents.Netrix.WebEditing.Elements.IElement.SetStyleAttribute(string styleName, string styleText)
        {
        }

        void GuruComponents.Netrix.WebEditing.Elements.IElement.RemoveStyleAttribute(string styleName)
        {
        }

        void GuruComponents.Netrix.WebEditing.Elements.IElement.SetAttribute(string attribute, object value)
        {
        }

        object GuruComponents.Netrix.WebEditing.Elements.IElement.GetAttribute(string attribute)
        {
            return null;
        }

        private ElementDom ed;

        [Browsable(false)]
        IElementDom GuruComponents.Netrix.WebEditing.Elements.IElement.ElementDom
        {
            get
            {
                if (ed == null)
                {
                    ed = new ElementDom(this.node, this.htmleditor);
                }
                return ed;
            }
        }

        /// <summary>
        /// Textnodes does not support styles.
        /// </summary>
        [Browsable(false)]
        IEffectiveStyle IElement.EffectiveStyle
        {
            get
            {
                return null; // textnodes does not have style
            }
        }

        /// <summary>
        /// Textnodes does not support styles.
        /// </summary>
        [Browsable(false)]
        IElementStyle IElement.RuntimeStyle
        {
            get
            {
                return null; // textnodes does not have style
            }
        }

        /// <summary>
        /// Textnodes does not support styles.
        /// </summary>
        [Browsable(false)]
        IElementStyle IElement.CurrentStyle
        {
            get
            {
                return null; // textnodes does not have style
            }
        }

        #endregion

        void IElement.RemoveAttribute(string attribute)
        {
        }

        [Browsable(false)]
        public IExtendedProperties ExtendedProperties
        {
            get
            {
                throw new NotSupportedException("TextNodeElement does not support extended properties.");
            }
        }

        # region Events

        event DocumentEventHandler IElement.LoseCapture{ add{} remove{} }
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
        event DocumentEventHandler IElement.Paged{ add{} remove{} }

        # endregion

        /// <summary>
        /// TextNodes are never selectable, this method will always return <c>false</c>.
        /// </summary>
        /// <returns>Returns <c>false</c>.</returns>
        [Browsable(false)]
        public bool IsSelectable()
        { 
            return false;
        }

        [Browsable(false)]
        bool IElement.AtomicSelection 
        {
            get
            {
                throw new NotSupportedException();
            } 
            set
            { 
                throw new NotSupportedException();
            }
        }

        [Browsable(false)]
        bool IElement.Unselectable 
        { 
            get
            {
                throw new NotSupportedException();
            } 
            set
            { 
                throw new NotSupportedException();
            }
        }

        #region IElement Members

        [Browsable(false)]
        public string UniqueName
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IElement Members

        [Browsable(false)]        
        bool IElement.IsAbsolutePositioned
        {
            get { return false; }
        }

        [Browsable(false)]
        bool IElement.IsTextEdit
        {
            get { return true; }
        }
       
        #endregion
    }
}
