using System;
using System.Collections;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix
{

    /// <summary>
    /// HtmlSelection implements this interface.
    /// </summary>
    /// <remarks>
    /// It provides methods and properties for element selections and layer operations.
    /// <seealso cref="IHtmlEditor"/>
    /// </remarks>
	public interface ISelection
	{

		/// <summary>
		/// Convenience method for checking if the specified element is absolutely positioned
		/// </summary>
		/// <param name="element">The element to check.</param>
		/// <returns>Returns <c>true</c> if "position:absolute" style is set.</returns>
		bool IsElement2DPositioned(IElement element);

		/// <summary>
		/// Determines if the element can have handles, e.g. is a selectable element.
		/// </summary>
		/// <param name="element">The element to check.</param>
		/// <returns>Returns <c>true</c> if the element can have handles.</returns>
		bool IsSelectableElement(IElement element);

		/// <summary>
		/// Checks if the element is currently selected.
		/// </summary>
		/// <remarks>
		/// This methods returns always <c>False</c> if the element isn't a selectable block element.
		/// One can check the selectable property by calling <see cref="IsSelectableElement"/> method.
		/// The method does not change the current selection.
		/// </remarks>
		/// <param name="element">The element to be checked.</param>
		/// <returns>Either <c>True</c> or <c>False</c> depending on current state.</returns>
		bool IsSelected(IElement element);

		/// <summary>
		/// Indicates if the current selection of absolutely positionable elements can be aligned to 
		/// any of the borders.
		/// </summary>
		/// <remarks>
		/// This is possible if two or more elements are selected and all of them
		/// have the position:absolute style attribute.
		/// </remarks>
		bool CanAlign {   get;  }

		/// <summary>
		/// This method returns true, if all selected elements have the same parent. </summary>
		/// <remarks>Under
		/// some circumstances it is recommended not to align floating elements if they are
		/// defined under different parent elements.
		/// </remarks>
		bool HaveSameParent { get; }

		/// <summary>
		/// Assign bottom alignment to current selection.
		/// </summary>
		void AlignBottom();

		/// <summary>
		/// Align selected elements horizontal centered.
		/// </summary>
		void AlignHorizontalCenter();

		/// <summary>
		/// Align selection left.
		/// </summary>
		void AlignLeft();

		/// <summary>
		/// Align selection to the right.
		/// </summary>
		void AlignRight();

		/// <summary>
		/// Align elements vertically at top of containing container.
		/// </summary>
		void AlignTop();

		/// <summary>
		/// Align elements vertically centered if inside a block element.
		/// </summary>
		void AlignVerticalCenter();

		/// <summary>
		/// Toggle the design time lock state of the selected items. Locked elements cannot be changed in 
		/// design mode.
		/// </summary>
		void ToggleLock(); 

		/// <summary>
		/// Returns info about the design time lock state of the selection.
		/// </summary>
		/// <returns></returns>
		HtmlCommandInfo GetLockInfo();

		/// <summary>
		/// Indicates if the current selection have it's z-index modified.
		/// </summary>
		bool CanChangeZIndex { get; }

		/// <summary>
		/// Returns info about the absolute positioning of the selection.
		/// </summary>
		/// <returns></returns>
		HtmlCommandInfo GetAbsolutePositionInfo();

        /// <summary>
        /// Gets the last not synchronized element.
        /// </summary>
        /// <returns></returns>
        IElement GetUnsynchronizedElement();

		/// <summary>
		/// Toggle the absolute positioning state of the selected items.
		/// </summary>
		void ToggleAbsolutePosition();

        /// <summary>
		/// Sends given item to the back.
		/// </summary>
        /// <param name="element">The element which has to be send to back.</param>
		void SendToBack(IElement element);
        
        /// <summary>
		/// Bring the given element to front by changing the z.order in a layered document.
		/// </summary>
        /// <param name="element">The element which has to be bring to front.</param>
		void BringToFront(IElement element);        

		/// <summary>
		/// Bring element to front by changing the z.order in a layered document.
		/// </summary>
		void BringToFront();

		/// <summary>
		/// Sends all selected items to the back.
		/// </summary>
		void SendToBack();

		/// <summary>
		/// Number of selected items.
		/// </summary>
		int Length { get; }

		/// <summary>
		/// Get the current element after synchronizing the current selection.
		/// </summary>
		IElement Element { get; }

		/// <summary>
		/// Gets the list of selected elements as Native Type. Normally, if only one element is 
		/// selected, use the <see cref="Element"/> method instead.
		/// </summary>
		ElementCollection Elements  { get; }

		/// <summary>
		/// Change the complete HTML including the outer HTML
		/// </summary>
		/// <param name="outerHtml"></param>
		void SetOuterHtml(string outerHtml); 

		/// <summary>
		/// Returns the outer HTML of the current selection. This includes the tag of he outermost element themselfes.
		/// </summary>
		/// <returns>HTML code as string</returns>
		string GetOuterHtml();

		/// <summary>
		/// Gets the list of direct children of the given element. The collection does not reflect the current 
		/// selection to make this function usable for all elements.
		/// </summary>
		/// <param name="o">IElement element</param>
		/// <returns><see cref="ElementCollection"/> of children, can be empty if there are no children or null if element was not recognized</returns>
		ElementCollection GetChildHierarchy(IElement o);

		/// <summary>
		/// Get the list of elements building the parent of the given element. The collection does not reflect the current 
		/// selection to make this function usable for all elements.
		/// </summary>
		/// <param name="o">Element as IElement derived object</param>
		/// <returns><see cref="ElementCollection"/> of parent element or null</returns>
		ElementCollection GetParentHierarchy(IElement o);

		/// <summary>
		/// Indicates if the current selection can be used to assign new values using any other operation.
		/// </summary>
		bool CanMatchSize { get; }

		/// <summary>
		/// Returns the text contained in the selection if there is a text selection
		/// </summary>
		string Text { get; } 

		/// <summary>
		/// Returns the html text contained in the selection if there is a text selection. The
		/// html tags are not stripped out. See <see cref="Text"/> for a version which strips tags.
		/// </summary>
		string Html { get; }

		/// <summary>
		/// This propertys returns true, if the control has currently selected text in it.
		/// </summary>
		bool HasTextSelection { get; }

		/// <summary>
		/// The HtmlSelectionType of the selection
		/// </summary>
		HtmlSelectionType SelectionType  { get; }

		/// <summary>
		/// Selects all elements in the document if they are absolutely positioned.
		/// </summary>
		bool SelectAll();

        /// <overloads>This method has two overloads.</overloads>
		/// <summary>
		/// Searches for an element and returns true, if the element exists within the page, and selects the element, if possible.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		bool SelectElement(IElement o);

        /// <summary>
        /// Searches for an element collection and returns true, if all elements exists within the page.
        /// The collection is selected, if possible.
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
		bool SelectElements(ICollection elements);

		/// <summary>
		/// Removes the selection the user has made but does not delete the content.
		/// </summary>
		void ClearSelection();

        /// <summary>
        /// Removes the selected elements and all its content.
        /// </summary>
        void DeleteSelection();

		/// <summary>
		/// Synchronizes the selection state held in this object with the selection state in MSHTML.</summary>
		/// <remarks>This method fires the
		/// OnSelectionChanged event if the current selection is different from the previous one.
		/// </remarks>
		/// <returns>Returns <c>true</c> if the selection has changed since last synchronization.</returns>
		bool SynchronizeSelection();

		/// <summary>
		/// This method remove the current element and preserves all child elements and textual content, if
		/// parameter preserveContent is set to true.
		/// </summary>
		/// <param name="preserveContent">True if the content and children should not be deleted.</param>
		void RemoveCurrentElement(bool preserveContent);

        /// <summary>
        /// Fired if the selection has changed. 
        /// </summary>
        /// <remarks>
        /// If element selections are made which cannot recognized as selection the event may return only the body element.
        /// </remarks>
        event GuruComponents.Netrix.Events.SelectionChangedEventHandler SelectionChanged;


	}

}
