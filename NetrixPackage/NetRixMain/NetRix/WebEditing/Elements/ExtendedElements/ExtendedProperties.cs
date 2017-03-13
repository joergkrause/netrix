using System;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.ComponentModel;

namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// This class allows access to additional information about an element and advanced element manipulation.
	/// </summary>
	/// <remarks>
	/// This class enhances the basic Element class. It was introduced to avoid
	/// the ability of non-HTML attributes for propertygrid access. The ScrollXX properties allow the
	/// detection of the current scroll position to synchronize external controls like ruler or bars.
	/// <para>
	/// The positions are in pixel and relative to the control window.
	/// </para>
	/// </remarks>
	public sealed class ExtendedProperties : IExtendedProperties
	{


        private Interop.IHTMLElement element;

        /// <summary>
        /// Constructor to create an instance of the class.
        /// </summary>
        /// <remarks>
        /// THIS CTOR SUPPORTS THE NETRIX INFRASTRUCTURE AND IS NIT INTENDET TO BEING USED FROM USER CODE.
        /// </remarks>
        /// <param name="peer"></param>
        public ExtendedProperties(Interop.IHTMLElement peer)
        {
            element = peer;
        }

		private Interop.IHTMLElement2 el2
		{
            get
            {
               return (Interop.IHTMLElement2) element;
            }
		}

        private Interop.IHTMLElement3 el3
        {
            get
            {
                return (Interop.IHTMLElement3) element;
            }
        }

        private Interop.IHTMLElement4 el4
        {
            get
            {
                return (Interop.IHTMLElement4) element;
            }
        }

        /// <summary>
        /// Simulates a click on a scroll-bar component.
        /// </summary>
        /// <remarks>
        /// Cascading Style Sheets (CSS) allow you to scroll on all objects through the IHTMLStyle::overflow property.
        /// <para>When the content of an element changes and causes scroll bars to display, the IHTMLElement2::doScroll method might not work correctly immediately following the content update. When this happens, you can use the IHTMLWindow2::setTimeout method to enable the browser to recognize the dynamic changes that affect scrolling.</para>
        /// </remarks>
        /// <param name="scroll"></param>
        public void DoScroll(ScrollAction scroll)
        {
            el2.DoScroll(scroll.ToString());
        }

        /// <summary>
        /// Returns the component located at the specified coordinates via certain events. 
        /// </summary>
        /// <remarks>
        /// A component is a part of the element, which acts as an specific interaction point. Typically these
        /// are the scrollbars as well as parts of the scrollbar, and the resizing handles. The property can
        /// be used to determine such an element at a specific client location.
        /// <para>The ComponentFromPoint method, available as of MSHTML 5, is applicable to any object that can be given scroll bars 
        /// through Cascading Style Sheets (CSS).</para> 
        /// <para>The ComponentFromPoint method may not consistently return the same object when used with the <c>MouseOver</c> event. Because 
        /// a user's mouse speed and entry point can vary, different components of an element can fire the <c>MouseOver</c> event. 
        /// For example, when a user moves the cursor over a <see cref="AreaElement">AreaElement</see> object with scroll bars, the event might fire when the mouse 
        /// enters the component border, the scroll bars, or the client region. Once the event fires, the expected element may not 
        /// be returned unless the scroll bars were the point of entry for the mouse. In this case, the onmousemove event can be 
        /// used to provide more consistent results.</para>
        /// <para>
        /// A good idea to get the current handle is the <c>ResizeStart</c> event, which is fired when the mouse is on top of one
        /// of the handles of an selected element. However, this will never return components other than "HandleXX" or nothing.
        /// </para>
        /// </remarks>
        /// <param name="loc">Location in client coordinates of the point for which the component is retrieved.</param>
        public ElementComponent ComponentFromPoint(System.Drawing.Point loc)
        {
            string c = el2.ComponentFromPoint(loc.X, loc.Y);
            if (c == null || c.Equals(String.Empty))
            {
                return ElementComponent.ClientArea;
            } 
            else 
            {
                return (ElementComponent) Enum.Parse(typeof(ElementComponent), c, true);
            }
        }

        /// <summary>
        /// Sets or retrieves the distance between the left edge of the object and the leftmost portion of the content currently visible in the window.
        /// </summary>
        /// <remarks>
        /// This property's value equals the current horizontal offset of the content within the scrollable range. Although you can set this property to any value, if you assign a value less than 0, the property is set to 0. If you assign a value greater than the maximum value, the property is set to the maximum value.
        /// <para>This property is always 0 for objects that do not have scroll bars. For these objects, setting the property has no effect.</para>
        /// <para>When a marquee object scrolls vertically, its ScrollLeft extended property is set to 0.</para>
        /// </remarks>
        [Browsable(false)]
        public int ScrollLeft
        {
            get
            {                
                return el2.GetScrollLeft();
            }
        }

        /// <summary>
        /// Sets or retrieves the distance between the top of the object and the topmost portion of the content currently visible in the window.
        /// </summary>
        /// <remarks>
        /// This property's value equals the current vertical offset of the content within the scrollable range. Although you can set this property to any value, if you assign a value less than 0, the property is set to 0. If you assign a value greater than the maximum value, the property is set to the maximum value.
        /// <para>This property is always 0 for objects that do not have scroll bars. For these objects, setting the property has no effect.</para>
        /// <para>When a marquee object scrolls horizontally, its ScrollTop extended property is set to 0.</para>
        /// </remarks>
        [Browsable(false)]
        public int ScrollTop
        {
            get
            {
                return el2.GetScrollTop();
            }
        }

        /// <summary>
        /// Retrieves the scrolling height of the object.
        /// </summary>
        /// <remarks>
        /// The height is the distance between the top and bottom edges of the object's content.
        /// </remarks>
        [Browsable(false)]
        public int ScrollHeight
        {
            get
            {
                return el2.GetScrollHeight();
            }
        }

        /// <summary>
        /// Retrieves the scrolling width of the object.
        /// </summary>
        /// <remarks>
        /// The width is the distance between the left and right edges of the object's visible content.
        /// </remarks>
        [Browsable(false)]
        public int ScrollWidth
        {
            get
            {
                return el2.GetScrollWidth();
            }
        }

        /// <summary>
        /// Retrieves the client area of the object including padding, but not including margin, border, or scroll bar.
        /// </summary>
        [Browsable(false)]
        public System.Drawing.Rectangle ClientArea
        {
            get
            {
                System.Drawing.Rectangle r = new System.Drawing.Rectangle(
                    el2.GetClientLeft(),
                    el2.GetClientTop(),
                    el2.GetClientWidth(),
                    el2.GetClientHeight()
                    );
                return r;
            }
        }

        /// <summary>
        /// Retrieves an object that specifies the bounds of the elements.
        /// </summary>
        [Browsable(false)]
        public System.Drawing.Rectangle AbsoluteArea
        {
            get
            {
                Interop.IHTMLRect rect = el2.GetBoundingClientRect();
                System.Drawing.Rectangle r = new System.Drawing.Rectangle(
                    rect.left,
                    rect.top,
                    rect.right - rect.left,
                    rect.bottom - rect.top
                    );
                return r;
            }
        }

        /// <summary>
        /// Merges adjacent <see cref="TextNodeElement"/> objects to produce a normalized document object model.
        /// </summary>
        /// <remarks>
        /// Calling Normalize before manipulating the subelements of an object ensures that the document object model has a consistent structure. The normal form is useful for operations that require a consistent document tree structure, and it ensures that the document object model view is identical when saved and reloaded.
        /// </remarks>
        public void Normalize()
        {
            el4.normalize();
        }
        
        /// <overloads/>
        /// <summary>
        /// Causes the object to scroll into view, aligning at the top of the window.
        /// </summary>
        /// <remarks>
        /// The ScrollIntoView method is useful for immediately showing the user the result of some action without requiring the user to manually scroll through the document to find the result.
        /// <para>Depending on the size of the given object and the current window, this method might not be able to put the item at the very top or very bottom, but will position the object as close to the requested position as possible.</para>
        /// </remarks>
        public void ScrollIntoView()
        {
            ScrollIntoView(true);
        }

        /// <summary>
        /// Causes the object to scroll into view, aligning it either at the top or bottom of the window.
        /// </summary>
        /// <remarks>
        /// The ScrollIntoView method is useful for immediately showing the user the result of some action without requiring the user to manually scroll through the document to find the result.
        /// <para>Depending on the size of the given object and the current window, this method might not be able to put the item at the very top or very bottom, but will position the object as close to the requested position as possible.</para>
        /// </remarks>
        /// <param name="top">Specifies where to scroll to. If <c>true</c> the element is scrolled to top position.</param>
        public void ScrollIntoView(bool top)
        {
            element.ScrollIntoView(top);
        }

        /// <summary>
        /// Retrieves the ordinal position of the object, in source order, as the object appears in the document's all collection.
        /// </summary>
        [Browsable(false)]
        public int DOMIndex
        {
            get
            {
                return element.GetSourceIndex();
            }
        }


        /// <summary>
        /// Sets the element as active without setting focus to the element.
        /// </summary>
        /// <remarks>The setActive method does not cause the document to scroll to the active object in the current page.</remarks>
        public void SetActive()
        {
            el3.setActive();
        }

        /// <summary>
        /// Set the ability to capture the focus.
        /// </summary>
        /// <param name="containerCapture"></param>
        public void SetCapture(bool containerCapture)
        {
            el2.SetCapture(containerCapture);
        }

        /// <summary>
        /// Release the ability to capture the focus.
        /// </summary>
        public void ReleaseCapture()
        {
            el2.ReleaseCapture();
        }

        /// <summary>
        /// Sets or retrieves the value indicating whether the object visibly indicates that it has focus.
        /// </summary>
        public bool HideFocus
        {
            get { return el3.hideFocus; }
            set { el3.hideFocus = value; }
        }

    }
}
