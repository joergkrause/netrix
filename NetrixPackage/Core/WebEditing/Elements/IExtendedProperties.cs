using System.Drawing;

namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// Extended properties IElement could provide.
    /// </summary>
    public interface IExtendedProperties
    {
        /// <summary>
        /// Current left scroll position.
        /// </summary>
        int ScrollLeft
        {
            get;
        }

        /// <summary>
        /// Current top scroll position.
        /// </summary>
        int ScrollTop
        {
            get;
        }

        /// <summary>
        /// Current scroll height.
        /// </summary>
        int ScrollHeight
        {
            get;
        }

        /// <summary>
        /// Current scroll width.
        /// </summary>        
        int ScrollWidth
        {
            get;
        }

        /// <summary>
        /// Client area the element needs to render completely.
        /// </summary>
        Rectangle ClientArea
        {
            get;
        }
        
        /// <summary>
        /// Merges adjacent text node objects to produce a normalized document object model.
        /// </summary>
        /// <remarks>
        /// Calling Normalize before manipulating the subelements of an object ensures that the document object model has a consistent structure. The normal form is useful for operations that require a consistent document tree structure, and it ensures that the document object model view is identical when saved and reloaded.
        /// </remarks>
        void Normalize();

        /// <summary>
        /// Simulates a click on a scroll-bar component.
        /// </summary>
        /// <remarks>
        /// Cascading Style Sheets (CSS) allow you to scroll on all objects through the IHTMLStyle::overflow property.
        /// <para>When the content of an element changes and causes scroll bars to display, the IHTMLElement2::doScroll method might not work correctly immediately following the content update. When this happens, you can use the IHTMLWindow2::setTimeout method to enable the browser to recognize the dynamic changes that affect scrolling.</para>
        /// </remarks>
        /// <param name="scroll"></param>
        void DoScroll(ScrollAction scroll);

        /// <summary>
        /// Returns the component located at the specified coordinates via certain events. 
        /// </summary>
        /// <remarks>
        /// A component is a part of the element, which acts as an specific interaction point. Typically these
        /// are the scrollbars as well as parts of the scrollbar, and the resizing handles. The property can
        /// be used to determine such an element at a specific client location.
        /// <para>The ComponentFromPoint method, available as of MSHTML 5, is applicable to any object that can be given scroll bars through Cascading Style Sheets (CSS).</para> 
        /// <para>The ComponentFromPoint method may not consistently return the same object when used with the onmouseover event. Because a user's mouse speed and entry point can vary, different components of an element can fire the onmouseover event. For example, when a user moves the cursor over a textArea object with scroll bars, the event might fire when the mouse enters the component border, the scroll bars, or the client region. Once the event fires, the expected element may not be returned unless the scroll bars were the point of entry for the mouse. In this case, the onmousemove event can be used to provide more consistent results.</para>
        /// </remarks>
        /// <param name="loc">Location in client coordinates of the point for which the component is retrieved.</param>
        ElementComponent ComponentFromPoint(Point loc);

        /// <overloads/>
        /// <summary>
        /// Causes the object to scroll into view, aligning at the top of the window.
        /// </summary>
        /// <remarks>
        /// The ScrollIntoView method is useful for immediately showing the user the result of some action without requiring the user to manually scroll through the document to find the result.
        /// <para>Depending on the size of the given object and the current window, this method might not be able to put the item at the very top or very bottom, but will position the object as close to the requested position as possible.</para>
        /// </remarks>
        void ScrollIntoView();

        /// <summary>
        /// Causes the object to scroll into view, aligning it either at the top or bottom of the window.
        /// </summary>
        /// <remarks>
        /// The ScrollIntoView method is useful for immediately showing the user the result of some action without requiring the user to manually scroll through the document to find the result.
        /// <para>Depending on the size of the given object and the current window, this method might not be able to put the item at the very top or very bottom, but will position the object as close to the requested position as possible.</para>
        /// </remarks>
        /// <param name="top">Specifies where to scroll to. If <c>true</c> the element is scrolled to top position.</param>
        void ScrollIntoView(bool top);

        /// <summary>
        /// Retrieves the ordinal position of the object, in source order, as the object appears in the document's all collection.
        /// </summary>
        int DOMIndex
        {
            get;
        }

        /// <summary>
        /// Sets the element as active without setting focus to the element.
        /// </summary>
        /// <remarks>The setActive method does not cause the document to scroll to the active object in the current page.</remarks>
        void SetActive();

        /// <summary>
        /// Set the ability to capture the focus.
        /// </summary>
        /// <param name="containerCapture"></param>
        void SetCapture(bool containerCapture);

        /// <summary>
        /// Release the ability to capture the focus.
        /// </summary>
        void ReleaseCapture();

        /// <summary>
        /// Sets or retrieves the value indicating whether the object visibly indicates that it has focus.
        /// </summary>
        bool HideFocus { get; set; }
    }
}
