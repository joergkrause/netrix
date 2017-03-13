using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// Gives direct access to the underlying DOM of any element.  
	/// </summary>
	/// <remarks>
	/// To access the object model of an element just query the <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement.ElementDom"/> property from element.
	/// To access the object model of the whole document's body just query the <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement.ElementDom"/> property from body element,
	/// available from <see cref="GuruComponents.Netrix.IHtmlEditor.GetBodyElement"/> method.
	/// To access the object model of the complete document just query IElementDom property from editor class,
	/// available through <see cref="GuruComponents.Netrix.IHtmlEditor.DocumentDom"/> method.
	/// </remarks>
	public interface IElementDom
    {

		/// <summary>
		/// Exchanges the location of two objects in the document hierarchy.
		/// </summary>
		/// <remarks>If elements are removed at run time, before the closing tag is parsed, areas of the document might not render.</remarks>
		/// <param name="node">Object that specifies the existing element.</param>
		/// <returns>Returns a reference to the object that invoked the method.</returns>
		IElement SwapNode(IElement node);
        
        /// <summary>
        /// Returns the direct descendent children elements.
        /// </summary>
        /// <remarks>
        /// Doe snot returns any text nodes between the elements.
        /// </remarks>
        /// <returns></returns>
        ElementCollection GetChildNodes();

        /// <summary>
        /// Returns the parent if there is any, or <c>null</c> otherwise.
        /// </summary>
        /// <returns></returns>
        IElement GetParent();

		/// <overloads/>
		/// <summary>
		/// Returns the direct descendent children elements, including text nodes.
		/// </summary>
		/// <remarks>
		/// This method returns text portions as TextNodeElement objects.
		/// </remarks>
		/// <param name="includeTextNodes">If <c>True</c> the collection will include text parts as TextNodeElement objects.</param>
		/// <returns></returns>
		ElementCollection GetChildNodes(bool includeTextNodes);

        /// <summary>
        /// Returns the last child of the current element.
        /// </summary>
        IElement LastChild { get; }

        /// <summary>
        /// Returns the first child of the current element.
        /// </summary>
        IElement FirstChild { get; }

        /// <summary>
        /// Gets <c>true</c>, if the element has child elements.
        /// </summary>
        bool HasChildNodes { get; } 
        
        /// <summary>
        /// Return the next sibling of this element.
        /// </summary>
        IElement NextSibling { get; }

        /// <summary>
        /// Return the previous sibling of the element.
        /// </summary>
        IElement PreviousSibling { get; }

        /// <summary>
        /// Insert a child before the given element.
        /// </summary>
        /// <param name="newChild"></param>
        /// <param name="refChild"></param>
        /// <returns></returns>
        IElement InsertBefore (IElement newChild, IElement refChild);

        /// <summary>
        /// Replace the element with another one.
        /// </summary>
        /// <param name="newChild"></param>
        /// <param name="oldChild"></param>
        /// <returns></returns>
        IElement ReplaceChild (IElement newChild, IElement oldChild);

        /// <summary>
        /// Removes the element from the collection.
        /// </summary>
        /// <param name="oldChild"></param>
        /// <returns></returns>
        IElement RemoveChild(IElement oldChild);

        /// <summary>
        /// Appends a new child to the children collection.
        /// </summary>
        /// <param name="newChild"></param>
        /// <returns></returns>
        IElement AppendChild (IElement newChild);

        /// <summary>
        /// Removes the current element from document
        /// </summary>
        /// <param name="deep">If <c>true</c>, all child element will be removed too. Otherwise the children will be preserved.</param>
        void RemoveElement(bool deep);

    }
}
