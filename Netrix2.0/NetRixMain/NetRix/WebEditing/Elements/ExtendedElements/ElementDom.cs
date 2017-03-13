using System;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// Gives direct access to the underlying DOM of any element.  
	/// </summary>
	/// <remarks>
	/// To access the document just query the ElementDom from body element.
	/// </remarks>
	public class ElementDom : IElementDom
	{
        Interop.IHTMLDOMNode node;
        private IHtmlEditor htmlEditor;
        private System.Xml.XmlNodeType nodeType;

		public ElementDom(Interop.IHTMLDOMNode element, IHtmlEditor htmlEditor) 
		{	
            this.node = element;
            this.htmlEditor = htmlEditor;
            if (element.nodeType == 0) 
            {
                nodeType = System.Xml.XmlNodeType.Attribute;
            } 
            else 
            {
                switch (element.nodeType)
                {
                    case 1:
                        nodeType = System.Xml.XmlNodeType.Element;
                        break;
                    case 3:
                        nodeType = System.Xml.XmlNodeType.Text;
                        break;
                    default:
                        nodeType = System.Xml.XmlNodeType.None;    
                        break;
                }
            }
		}

		
		/// <summary>
		/// Exchanges the location of two objects in the document hierarchy.
		/// </summary>
		/// <remarks>If elements are removed at run time, before the closing tag is parsed, areas of the document might not render.</remarks>
		/// <param name="node">Object that specifies the existing element.</param>
		/// <returns>Returns a reference to the object that invoked the method.</returns>
		public IElement SwapNode(IElement node)
		{
			Interop.IHTMLDOMNode newNode = (this.node).swapNode((Interop.IHTMLDOMNode) node);
			return this.htmlEditor.GenericElementFactory.CreateElement(newNode as Interop.IHTMLElement) as IElement;
		}

		/// <overloads/>
		/// <summary>
		/// Returns the direct descendent children elements, including text nodes.
		/// </summary>
		/// <remarks>
		/// This method returns text portions as <see cref="TextNodeElement"/> objects.
		/// </remarks>
		/// <param name="includeTextNodes">If <c>True</c> the collection will include text parts as <see cref="TextNodeElement"/> objects.</param>
		/// <returns></returns>
		public ElementCollection GetChildNodes(bool includeTextNodes)
		{
			Interop.IHTMLDOMChildrenCollection children = (Interop.IHTMLDOMChildrenCollection) node.childNodes;
			ElementCollection ec = new ElementCollection();
			int length = children.length;
			for (int i = 0; i < length; i++)
			{
				Interop.IHTMLDOMNode element = children.item(i) as Interop.IHTMLDOMNode;
				if (element != null && element != node)
				{
					if (element.nodeName.Equals("#text"))
					{
						ec.Add(new TextNodeElement(element, this.htmlEditor));
					} 
					else 
					{
						ec.Add(this.htmlEditor.GenericElementFactory.CreateElement(element as Interop.IHTMLElement));
					}
				}
			}
			return ec;
		}

        /// <summary>
        /// Returns the direct descendent children elements.
        /// </summary>
        /// <remarks>
        /// Does not returns any text nodes between the elements. They are stripped out from DOM tree before building the collection.
        /// </remarks>
        /// <returns></returns>
        public ElementCollection GetChildNodes()
        {
            Interop.IHTMLDOMChildrenCollection children = (Interop.IHTMLDOMChildrenCollection) node.childNodes;
            ElementCollection ec = new ElementCollection();
            int length = children.length;
            for (int i = 0; i < length; i++)
            {
                Interop.IHTMLDOMNode element = children.item(i) as Interop.IHTMLDOMNode;
                if (element != null && element != node)
                {
                    if (element.nodeName.Equals("#text")) continue;
                    ec.Add(this.htmlEditor.GenericElementFactory.CreateElement(element as Interop.IHTMLElement));
                }
            }
            return ec;
        }

        /// <summary>
        /// Returns the parent if there is any, or <c>null</c> otherwise.
        /// </summary>
        /// <returns>Returns the element as <see cref="IElement"/>, by forcing the cast to avoid access to non-native objects.</returns>
        public IElement GetParent()
        {
            Interop.IHTMLElement parentNode = node.parentNode as Interop.IHTMLElement;
            if (parentNode != null)
            {
                return htmlEditor.GenericElementFactory.CreateElement(parentNode) as IElement;
            }
            else
            {
                return null;
            }
        }

        # region Explicit XmlNode implementation

        public string LocalName
        {
            get
            {
                return ((Interop.IHTMLElement) node).GetTagName();
            }
        }

        public string Name
        {
            get
            {
                string ns = (((Interop.IHTMLElement2) node).GetScopeName() == null) ? String.Empty : ((Interop.IHTMLElement2) node).GetScopeName();
                if (ns.Length > 0) ns += ":";
                return String.Concat(ns, LocalName);
            }
        }

        public System.Xml.XmlNodeType NodeType
        {
            get
            {
                return nodeType;
            }
        }

        public void WriteContentTo(System.Xml.XmlWriter w)
        {
            w.WriteRaw(((Interop.IHTMLElement) node).GetInnerHTML());
        }

        public void WriteTo(System.Xml.XmlWriter w)
        {
            w.WriteRaw(((Interop.IHTMLElement) node).GetInnerHTML());
        }

       
        # endregion

        /// <summary>
        /// Returns the last child of the current element.
        /// </summary>
        public IElement LastChild
        {
            get
            {
                Interop.IHTMLElement el = node.lastChild as Interop.IHTMLElement;
                if (el != null)
                {
                    return this.htmlEditor.GenericElementFactory.CreateElement(el) as IElement;
                }
                else 
                {
                    if (node.lastChild != null && node.lastChild.nodeName == "#text")
                    {
                        return new TextNodeElement(node.lastChild, this.htmlEditor);
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the first child of the current element.
        /// </summary>
        public IElement FirstChild
        {
            get
            {
                Interop.IHTMLElement el = node.firstChild as Interop.IHTMLElement;
                if (el != null)
                {
                    return this.htmlEditor.GenericElementFactory.CreateElement(el) as IElement;
                }
                else 
                {
                    if (node.firstChild != null && node.firstChild.nodeName == "#text")
                    {
                        return new TextNodeElement(node.firstChild, this.htmlEditor);
                    }
                    return null;
                }
            }            
        }

        /// <summary>
        /// Gets <c>true</c>, if the element has child elements.
        /// </summary>
        public bool HasChildNodes
        {
            get
            {
                return node.hasChildNodes();
            }
        }

        /// <summary>
        /// Return the next sibling of this element.
        /// </summary>
        public IElement NextSibling
        {
            get
            {
                Interop.IHTMLElement el = node.nextSibling as Interop.IHTMLElement;
                if (el != null)
                {
                    return this.htmlEditor.GenericElementFactory.CreateElement(el) as IElement;
                }
                else 
                {
                    if (node.nextSibling != null && node.nextSibling.nodeName == "#text")
                    {
                        return new TextNodeElement(node.nextSibling, this.htmlEditor);
                    }
                    return null;
                }                
            }
        }

        /// <summary>
        /// Return the previous sibling of the element.
        /// </summary>
        public IElement PreviousSibling
        {
            get
            {
                Interop.IHTMLElement el = node.previousSibling as Interop.IHTMLElement;
                if (el != null)
                {
                    return this.htmlEditor.GenericElementFactory.CreateElement(el) as IElement;
                }
                else 
                {
                    if (node.previousSibling != null && node.previousSibling.nodeName == "#text")
                    {
                        return new TextNodeElement(node.previousSibling, this.htmlEditor);
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Return the previous sibling of the element.
        /// </summary>
        public IElement Parent
        {
            get
            {
                Interop.IHTMLElement el = node.parentNode as Interop.IHTMLElement;
                if (el != null)
                {
                    return this.htmlEditor.GenericElementFactory.CreateElement(el) as IElement;
                }
                else 
                {
                    if (node.parentNode != null && node.parentNode.nodeName == "#text")
                    {
                        return new TextNodeElement(node.parentNode, this.htmlEditor);
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Insert a new element before the current one.
        /// </summary>
        /// <param name="newChild">The new element, which has to be inserted.</param>
        /// <param name="refChild">The element, before which the new element is inserted.</param>
        /// <returns>The new element, if successful, <c>null</c> (<c>Nothing</c> in VB.NET) else.</returns>
        public IElement InsertBefore (IElement newChild, IElement refChild)
        {
            Interop.IHTMLDOMNode nc, rc;
            if (newChild is TextNodeElement)
            {
                nc = ((TextNodeElement) newChild).GetBaseElement() as Interop.IHTMLDOMNode;                
            }
            else 
            {
                nc = newChild.GetBaseElement() as Interop.IHTMLDOMNode;
            }
            if (refChild is TextNodeElement)
            {
                rc = ((TextNodeElement) refChild).GetBaseElement() as Interop.IHTMLDOMNode;
            } 
            else 
            {
                rc = refChild.GetBaseElement() as Interop.IHTMLDOMNode;
            }
            try
            {
                if (nc != null && rc != null)
                {
                    return this.htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) node.insertBefore(nc, rc)) as IElement;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;                
            }
        }


        /// <summary>
        /// Removes a child element from the collection of children.
        /// </summary>
        /// <param name="oldChild">The element which has to be removed.</param>
        /// <returns>Returns the parent element.</returns>
        public IElement RemoveChild (IElement oldChild)
        {
            Interop.IHTMLDOMNode oc;
            if (oldChild is TextNodeElement)
            {
                oc = ((TextNodeElement) oldChild).GetBaseElement() as Interop.IHTMLDOMNode;                
            }
            else 
            {
                oc = ((IElement) oldChild).GetBaseElement() as Interop.IHTMLDOMNode;
            }
            try
            {
                Interop.IHTMLDOMNode parent = node.removeChild(oc);
                return this.htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) parent) as IElement;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Replaces an element in the children collection with a new one.
        /// </summary>
        /// <param name="oldChild">The child element which should be replaced.</param>
        /// <param name="newChild">The new element which replaces the old one.</param>        
        /// <returns>The IElement object representing the new element.</returns>
        public IElement ReplaceChild (IElement oldChild, IElement newChild)
        {
            Interop.IHTMLDOMNode nc, rc;
            if (newChild is TextNodeElement)
            {
                nc = ((TextNodeElement) newChild).GetBaseElement() as Interop.IHTMLDOMNode;                
            }
            else 
            {
                nc = ((IElement) newChild).GetBaseElement() as Interop.IHTMLDOMNode;
            }
            if (oldChild is TextNodeElement)
            {
                rc = ((TextNodeElement) oldChild).GetBaseElement() as Interop.IHTMLDOMNode;
            } 
            else 
            {
                rc = ((IElement) oldChild).GetBaseElement() as Interop.IHTMLDOMNode;
            }            
            try
            {
                if (rc != null && nc != null)
                {
                    node.replaceChild(rc, nc);
                    return this.htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) node) as IElement;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;                
            }
        }

        /// <summary>
        /// Appends a new child to the children collection.
        /// </summary>
        /// <param name="newChild"></param>
        /// <returns></returns>
        public IElement AppendChild (IElement newChild)
        {
            Interop.IHTMLDOMNode nc;
            if (newChild is TextNodeElement)
            {
                nc = ((TextNodeElement) newChild).GetBaseElement() as Interop.IHTMLDOMNode;                
            }
            else 
            {
                nc = ((IElement) newChild).GetBaseElement() as Interop.IHTMLDOMNode;
            }
            try
            {
                if (nc != null)
                {
                    return this.htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) node.appendChild(nc)) as IElement;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;                                
            }
        }

        /// <summary>
        /// Removes the current element from document
        /// </summary>
        /// <param name="deep">If <c>true</c>, all child element will be removed too. Otherwise the children will be preserved.</param>
        public void RemoveElement(bool deep)
        {
            try
            {
                node.removeNode(deep);
                ((HtmlEditor)htmlEditor).InvokeHtmlElementChanged(((HtmlEditor)htmlEditor).CurrentScopeElement, GuruComponents.Netrix.Events.HtmlElementChangedType.Unknown);
            }
            catch
            {
            }
        }
        

	}
}
