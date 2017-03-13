using System;
using System.ComponentModel;

using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.Styles;
using System.Web.UI;

namespace GuruComponents.Netrix.VmlDesigner.Elements
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

        /// <summary>
        /// Access to several additional properties.
        /// </summary>
        public IExtendedProperties ExtendedProperties
        {
            get
            {
                throw new NotSupportedException("This property is not supported on TextNodes");
            }
        }
        
        public string UniqueName
        {
            get { throw new NotImplementedException(""); }
        }

        public Interop.IHTMLElement GetBaseElement() 
        {
            return node as Interop.IHTMLElement;
        }

        #region IElement Member

        public bool IsSelectable()
        {
            return false;
        }


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

        public IElementBehavior ElementBehaviors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool IElement.ContentEditable
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        string IElement.InnerHtml
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        string IElement.OuterHtml
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

        IElement IElement.TagElement
        {
            get
            {
                return null;
            }
        }

        Control IElement.GetChild(int index)
        {
            return null;
        }

        Control IElement.GetChild(string name)
        {
            return null;
        }

        ElementCollection IElement.GetChildren()
        {
            return null;
        }

        public Control GetParent()
        {
            Interop.IHTMLElement el = node.parentNode as Interop.IHTMLElement;
            Control element = null;
            if (el != null)
            {
                element = this.htmleditor.GenericElementFactory.CreateElement(el);
            }
            return element;
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="attribute">Not used.</param>
        void IElement.RemoveAttribute(string attribute)
        {
        }


        public new string ToString()
        {
            return node.nodeValue.ToString();
        }

        string IElement.GetStyle()
        {
            return null;
        }

        void IElement.SetStyle(string CssText)
        {
        }

        string IElement.GetStyleAttribute(string styleName)
        {
            return null;
        }

        void IElement.SetStyleAttribute(string styleName, string styleText)
        {
        }

        void IElement.RemoveStyleAttribute(string styleName)
        {
        }

        void IElement.SetAttribute(string attribute, object value)
        {
        }

        object IElement.GetAttribute(string attribute)
        {
            return null;
        }

        private IElementDom ed;

        IElementDom IElement.ElementDom
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

        #endregion

        #region IElement Member

        event Events.DocumentEventHandler IElement.Blur { add { } remove {  } }
        event Events.DocumentEventHandler IElement.Click { add { } remove {  } }
        event Events.DocumentEventHandler IElement.DblClick { add { } remove {  } }
        event Events.DocumentEventHandler IElement.DragEnter { add { } remove {  } }
        event Events.DocumentEventHandler IElement.DragLeave { add { } remove {  } }
        event Events.DocumentEventHandler IElement.DragOver { add { } remove {  } }
        event Events.DocumentEventHandler IElement.DragStart { add { } remove {  } }
        event Events.DocumentEventHandler IElement.Drop { add { } remove {  } }
        event Events.DocumentEventHandler IElement.Focus { add { } remove {  } }
        event Events.DocumentEventHandler IElement.KeyDown { add { } remove {  } }
        event Events.DocumentEventHandler IElement.KeyPress { add { } remove {  } }
        event Events.DocumentEventHandler IElement.KeyUp { add { } remove {  } }
        event Events.DocumentEventHandler IElement.Move { add { } remove {  } }
        event Events.DocumentEventHandler IElement.MoveEnd { add { } remove {  } }
        event Events.DocumentEventHandler IElement.MoveStart { add { } remove {  } }
        event Events.DocumentEventHandler IElement.PropertyChange { add { } remove {  } }
        
        event Events.DocumentEventHandler IElement.MouseDown { add { } remove {  } }

        event Events.DocumentEventHandler IElement.ResizeStart { add { } remove {  } }

        event Events.DocumentEventHandler IElement.ResizeEnd { add { } remove {  } }

        event Events.DocumentEventHandler IElement.MouseEnter { add { } remove {  } }

        event Events.DocumentEventHandler IElement.MouseLeave { add { } remove {  } }

        event Events.DocumentEventHandler IElement.MouseMove { add { } remove {  } }

        event Events.DocumentEventHandler IElement.MouseOut { add { } remove {  } }

        event Events.DocumentEventHandler IElement.MouseOver { add { } remove {  } }

        event Events.DocumentEventHandler IElement.MouseUp { add { } remove {  } }

        event Events.DocumentEventHandler IElement.SelectStart { add { } remove {  } }

        event Events.DocumentEventHandler IElement.ControlSelect { add { } remove {  } }

        event Events.DocumentEventHandler IElement.LayoutComplete { add { } remove {  } }

        event Events.DocumentEventHandler IElement.FocusOut { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Change { add { } remove {  } }

        event Events.DocumentEventHandler IElement.ContextMenu { add { } remove {  } }

        event Events.DocumentEventHandler IElement.BeforeActivate { add { } remove {  } }

        event Events.DocumentEventHandler IElement.DragEnd { add { } remove {  } }

        event Events.DocumentEventHandler IElement.EditFocus { add { } remove {  } }

        event Events.DocumentEventHandler IElement.BeforeCopy { add { } remove {  } }

        event Events.DocumentEventHandler IElement.BeforePaste { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Paste { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Resize { add { } remove {  } }

        event Events.DocumentEventHandler IElement.SelectionChange { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Drag { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Cut { add { } remove {  } }

        event Events.DocumentEventHandler IElement.LoseCapture { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Load { add { } remove {  } }

        event Events.DocumentEventHandler IElement.FilterChange { add { } remove {  } }

        event Events.DocumentEventHandler IElement.FocusIn { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Stop { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Abort { add { } remove {  } }

        event Events.DocumentEventHandler IElement.BeforeDeactivate { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Select { add { } remove {  } }
        
        event Events.DocumentEventHandler IElement.Paged { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Activate { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Copy { add { } remove {  } }

        event Events.DocumentEventHandler IElement.BeforeCut { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Error { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Scroll { add { } remove {  } }

        event Events.DocumentEventHandler IElement.Deactivate { add { } remove {  } }

        event Events.DocumentEventHandler IElement.MouseWheel { add { } remove {  } }


        [Browsable(false)]
		GuruComponents.Netrix.WebEditing.Styles.IEffectiveStyle IElement.EffectiveStyle
        {
            get
            {
                throw new NotSupportedException("This property is not supported for TextNode Elements");
            }
        }

        [Browsable(false)]
		IElementStyle IElement.RuntimeStyle
        {
            get
            {                
                throw new NotSupportedException("This property is not supported for TextNode Elements");
            }
        }

        [Browsable(false)]
		IElementStyle IElement.CurrentStyle
        {
            get
            {                
                throw new NotSupportedException("This property is not supported for TextNode Elements");
            }
        }


        bool IElement.Unselectable
        {
            get
            {
                throw new NotSupportedException("This property is not supported for TextNode Elements");
            }
            set
            {
                throw new NotSupportedException("This property is not supported for TextNode Elements");
            }
        }

        public System.Drawing.Rectangle GetAbsoluteArea()
        {
            // TODO:  Implementierung von TextNodeElement.GetAbsoluteArea hinzufügen
            return new System.Drawing.Rectangle ();
        }

        public void RemoveAttribute(string attribute)
        {
            // TODO:  Implementierung von TextNodeElement.RemoveAttribute hinzufügen
        }

        public bool AtomicSelection
        {
            get
            {
                // TODO:  Getter-Implementierung für TextNodeElement.AtomicSelection hinzufügen
                return false;
            }
            set
            {
                // TODO:  Getter-Implementierung für TextNodeElement.AtomicSelection hinzufügen
            }
        }

        #endregion

        #region IElement Members


        public string Alias
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public string ElementName
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IElement Members


        public bool IsAbsolutePositioned
        {
            get { return false; }
        }

        public bool IsTextEdit
        {
            get { return true; }
        }

        public System.Web.UI.CssStyleCollection Style
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
        #endregion
    }
}
