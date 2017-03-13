using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Behaviors;

using GuruComponents.Netrix.WebEditing.Styles;
using System.Collections;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The base interface for all HTML elements.
    /// </summary>
    public interface IElement
    {

        /// <summary>
        /// Contains the HTML cascading-style sheets (CSS) inline style attributes for a specified HTML server control. This class cannot be inherited.
        /// </summary>
        System.Web.UI.CssStyleCollection Style { get; set; }

        # region Events

        /// <summary>
        /// Fired if the user clicks on the element loses its mouse capture.
        /// </summary>
        event GuruComponents.Netrix.Events.DocumentEventHandler LoseCapture;

        /// <summary>
        /// Fired if the user clicks on the element in design mode.
        /// </summary>
        event DocumentEventHandler Click;

        /// <summary>
        /// Fired if the user double clicks on the element in design mode.
        /// </summary>
        event DocumentEventHandler DblClick;

        /// <summary>
        /// Fired if the user starts dragging the in design mode.
        /// </summary>
        event DocumentEventHandler DragStart;

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        event DocumentEventHandler Focus;

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        event DocumentEventHandler Drop;

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        event DocumentEventHandler Blur;

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        event DocumentEventHandler DragOver;

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        event DocumentEventHandler DragEnter;

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        event DocumentEventHandler DragLeave;

        /// <summary>
        /// Fired if the user hits a key down on the element in design mode.
        /// </summary>
        event DocumentEventHandler KeyDown;

        /// <summary>
        /// Fired if the user pressed a key element in design mode.
        /// </summary>
        event DocumentEventHandler KeyPress;

        /// <summary>
        /// Fired if the user hits and releases a key on the element in design mode.
        /// </summary>
        event DocumentEventHandler KeyUp;
        /// <summary>
        /// Fired if the user clicks a mouse button on the element in design mode.
        /// </summary>
        event DocumentEventHandler MouseDown;

        /// <summary>
        /// Sets or removes an event handler function that fires when the user begins to change the dimensions of the object.
        /// </summary>
        /// <remarks>
        /// This event handler will not suppress the embedded resizing behavior.
        /// </remarks>
        event DocumentEventHandler ResizeStart;

        /// <summary>
        /// Sets or removes an event handler function that fires when the user has finished changing the dimensions of the object.
        /// </summary>
        /// <remarks>
        /// This event handler will not suppress the embedded resizing behavior.
        /// </remarks>
        event DocumentEventHandler ResizeEnd;
        /// <summary>
        /// Sets or removes an event handler function that fires when the user moves the mouse pointer into the object.
        /// </summary>
        /// <remarks>
        /// Unlike the MouseOver event, the MouseEnter event does not bubble. In other words, the MouseEnter 
        /// event does not fire when the user moves the mouse pointer over elements contained by the object, 
        /// whereas <see cref="MouseOver">MouseOver</see> does fire. 
        /// </remarks>
        event DocumentEventHandler MouseEnter;

        /// <summary>
        /// Sets or retrieves a pointer to the event handler function that fires, when the user moves the mouse pointer outside 
        /// the boundaries of the object.</summary>
        /// <remarks>Use in correspondence to OnMouseEnter.</remarks>
        event DocumentEventHandler MouseLeave;

        /// <summary>
        /// Fired if the user moves the mouse over the element area in design mode.
        /// </summary>
        event DocumentEventHandler MouseMove;

        /// <summary>
        /// Fired if the user mouse has left the element area with the mouse in design mode.
        /// </summary>
        /// <example>
        /// To use this event to show the content of a link the mouse pointer is over, use this code:
        /// <code>
        /// ArrayList anchors = this.htmlEditor2.DocumentProperties.GetElementCollection("A") as ArrayList;
        /// if (anchors != null)
        /// {
        ///    foreach (AnchorElement a in anchors)
        ///    {
        ///        a.OnMouseOver -= new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOver);
        ///        a.OnMouseOver += new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOver);
        ///        a.OnMouseOut -= new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOut);
        ///        a.OnMouseOut += new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOut);
        ///    }
        /// }</code>
        /// Place this code in the <see cref="GuruComponents.Netrix.IHtmlEditor.ReadyStateComplete">ReadyStateComplete</see> event.
        /// The handler are now able to do something with the element.
        /// <code>
        ///private void a_OnMouseOut(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///  AnchorElement a = e.SrcElement as AnchorElement;
        ///  if (a != null)
        ///  {
        ///     this.statusBarPanelLinks.Text = "";
        ///  }
        ///}</code>
        /// <code>
        ///private void a_OnMouseOver(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///   AnchorElement a = e.SrcElement as AnchorElement;
        ///   if (a != null)
        ///   {
        ///      this.statusBarPanelLinks.Text = a.href;
        ///   }
        ///}</code>
        /// </example>
        event DocumentEventHandler MouseOut;

        /// <summary>
        /// Fired if the user has entered the element area with the mouse in design mode.
        /// </summary>
        /// <example>
        /// To use this event to show the content of a link the mouse pointer is over, use this code:
        /// <code>
        /// ArrayList anchors = this.htmlEditor2.DocumentProperties.GetElementCollection("A") as ArrayList;
        /// if (anchors != null)
        /// {
        ///    foreach (AnchorElement a in anchors)
        ///    {
        ///        a.OnMouseOver -= new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOver);
        ///        a.OnMouseOver += new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOver);
        ///        a.OnMouseOut -= new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOut);
        ///        a.OnMouseOut += new GuruComponents.Netrix.Events.DocumentEventHandler(a_OnMouseOut);
        ///    }
        /// }</code>
        /// Place this code in the <see cref="IHtmlEditor.ReadyStateComplete">ReadyStateComplete</see> event.
        /// The handler are now able to do something with the element.
        /// <code>
        ///private void a_OnMouseOut(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///  AnchorElement a = e.SrcElement as AnchorElement;
        ///  if (a != null)
        ///  {
        ///     this.statusBarPanelLinks.Text = "";
        ///  }
        ///}</code>
        /// <code>
        ///private void a_OnMouseOver(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///   AnchorElement a = e.SrcElement as AnchorElement;
        ///   if (a != null)
        ///   {
        ///      this.statusBarPanelLinks.Text = a.href;
        ///   }
        ///}</code>
        /// </example>
        event DocumentEventHandler MouseOver;
        /// <summary>
        /// Fired if the user releases the mouse button over the element area in design mode.
        /// </summary>
        event DocumentEventHandler MouseUp;
        /// <summary>
        /// Fired if the user starts selecting the element area in design mode.
        /// </summary>
        event DocumentEventHandler SelectStart;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler LayoutComplete;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Load;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler MouseWheel;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler MoveEnd;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler MoveStart;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Activate;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler BeforeActivate;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler BeforeCopy;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler BeforeCut;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler BeforePaste;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler ContextMenu;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Copy;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Cut;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Deactivate;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Drag;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler DragEnd;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler FocusIn;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler FocusOut;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler FilterChange;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Abort;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Change;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Select;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler SelectionChange;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Stop;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler BeforeDeactivate;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler ControlSelect;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler EditFocus;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Error;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Move;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Paste;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler PropertyChange;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Resize;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Scroll;
        /// <summary>
        /// Fired if the underlying event is being received.
        /// </summary>
        event DocumentEventHandler Paged;
        
        # endregion

        /// <summary>
        /// Access to several extended properties an element can provide.
        /// </summary>
        IExtendedProperties ExtendedProperties { get; }

        /// <summary>
        /// Returns the unique name which is used to hold the element in the designer environment.
        /// </summary>
        string UniqueName
        {
            get;
        }

        /// <summary>
        /// Returns <c>true</c> if the element is selectable.
        /// </summary>
        /// <returns></returns>
        bool IsSelectable();

        /// <summary>
        /// Gets true if the element can positioned freely on the surface by using the mouse.
        /// </summary>
        bool IsAbsolutePositioned
        {
            get;
        }

        /// <summary>
        /// Gets <c>true</c> if the element is in text editing mode.
        /// </summary>
        bool IsTextEdit
        {
            get;
        }

		/// <summary>
		/// Inserts an element at the specified location.
		/// </summary>
		/// <param name="method">Specifies where to insert the HTML element.</param>
		/// <param name="element">Element to be inserted adjacent to the object.</param>
		void InsertAdjacentElement(InsertWhere method, IElement element);

		/// <summary>
		/// Inserts the given HTML text into the element at the location.
		/// </summary>
		/// <param name="method"></param>
		/// <param name="html"></param>
		void InsertAdjacentHtml(InsertWhere method, string html);

		/// <summary>
		/// Inserts the given text into the element at the specified location.
		/// </summary>
		/// <param name="method">Specifies where to insert the text.</param>
		/// <param name="text"></param>
		void InsertAdjacentText(InsertWhere method, string text);

        /// <summary>
        /// Returns the absolute coordinates of the element in pixel.
        /// </summary>
        /// <remarks>
        /// This method works even for non absolute positioned elements. Some elements, which have no rectangle
        /// dimensions, may fail returning any useful values.
        /// </remarks>
        /// <returns>A rectangle which describes the dimensions of the client area of the element.</returns>
        System.Drawing.Rectangle GetAbsoluteArea();

        /// <summary>
        /// Returns the base COM element object.
        /// </summary>
        /// <returns></returns>
        Interop.IHTMLElement GetBaseElement();

        /// <summary>
        /// Reference to the editor that this elements belongs to.
        /// </summary>
        IHtmlEditor HtmlEditor { get; set; }

		/// <summary>
		/// Gets or sets explicitly the design mode for this element.
		/// </summary>
		/// <value>Setting to true will set the attribute "contentEditable", which is visible in the documents DOM.
		/// Returning the value false means that the mode is not explicitly set but may be inherited. Setting a
		/// parent to ContentEditable will not change the value for the children, but in fact, they are now editable. 
		/// </value>
		bool ContentEditable { get; set; }

        /// <summary>
        /// The element with this property set to TRUE will be selectable only as a unit.
        /// </summary>
        /// <remarks>
        /// This property is only available 
        /// </remarks>
        bool AtomicSelection { get; set; }

        /// <summary>
        /// Makes the element unselectable, so the user cannot activate it for resizing or moving.
        /// </summary>
        /// <remarks>
        /// The property is ignored if the element is already an unselectable element. Only block elements
        /// like DIV, TABLE, and IMG can be selectable.
        /// </remarks>
        bool Unselectable { get; set; }

        /// <summary>
        /// Access to the element's behavior 
        /// </summary>
        IElementBehavior ElementBehaviors
        {
            get;
        }
        
//        /// <summary>
//        /// This property contains the root of the project, where the file containing
//        /// the element belongs to.
//        /// </summary>
//        /// <remarks>
//        /// This property is set internally when the control loads either HTML from a string and the
//        /// app gives the path or when it loads from a file. If none of these option happens the
//        /// property is set to an empty string.
//        /// </remarks>
//        string AppRootPath
//        {
//            get;
//        }
        /// <summary>
        /// Gets or sets inner html of the element.
        /// </summary>
        /// <remarks>
        /// The inner html is the complete content between the opening and the closing tag.
        /// If the element is not a container element the property will return <see cref="System.String.Empty">String.Empty</see>.
        /// Any HTML is returned "as is", with the braces and tag names.
        /// </remarks>
        string InnerHtml
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets outer html of the element.
        /// </summary>
        /// <remarks>
        /// The outer html is the complete content including the opening and the closing tag and their braces.
        /// </remarks>
        string OuterHtml
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the inner text of the element.
        /// </summary>
        /// <remarks>
        /// The inner text is the complete content between the opening and the closing tag, with any HTML tags
        /// stripped out.
        /// If the element is not a container element the property will return <see cref="System.String.Empty">String.Empty</see>.
        /// Any HTML is returned "as is", with the braces and tag names.
        /// </remarks>
        string InnerText
        {
            get; set;
        }

        /// <summary>
        /// The name of the element.
        /// </summary>
        /// <remarks>
        /// Used to reference the element if it is still unknown after a insertion operation.
        /// This property should work even if there is no native class available. It does not work
        /// reliable if an element is not HTML 4.0 compliant.
        /// </remarks>
        string TagName
        {
            get;
        }

        /// <summary>
        /// Gives direct access to the element object from datasource or collection based lists.
        /// </summary>
        /// <remarks>
        /// Used for lists which needs a property to access collection members for displaying purposes. To
        /// prevent such lists from deleting the object we send only a shallow copy clone. This
        /// allows the caller to change all properties but deleting does not modify the DOM.
        /// </remarks>
		IElement TagElement
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string Alias
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string ElementName
        {
            get;
        }

        /// <overloads>This method has two overloads.</overloads>
        /// <summary>
        /// Returns the child at the give index. </summary>
        /// <remarks>If a element has 4 children they are indexed zero based.
        /// </remarks>
        /// <param name="index">The Number of child in the children collection.</param>
        /// <returns>Returns the base class of child element or <c>null</c>, if the index was not found.</returns>
        System.Web.UI.Control GetChild(int index);
        /// <summary>
        /// The child of the element or null if it does not have children.
        /// </summary>
        /// <remarks>
        /// This method will return the next child with the given tag name in the collection of children.
        /// </remarks>
        /// <param name="name">Tag name of the child we are searching for.</param>
        /// <returns>Returns the base class of child element or <c>null</c>, if the name was not found.</returns>
        System.Web.UI.Control GetChild(string name);
        
        /// <summary>
        /// Returns the collection of children of the element.
        /// </summary>
        /// <remarks>
        /// The returned collection will always beeing created, but it could be empty if there are
        /// no children.
        /// </remarks>
        /// <returns>Collection of elements of type IElement.</returns>
        ElementCollection GetChildren();
        
        /// <summary>
        /// The parent of the current element or null if no parent.
        /// </summary>
        /// <returns>Returns the base class of child element or <c>null</c>, if there was no parent element.</returns>
        System.Web.UI.Control GetParent();
        /// <summary>
        /// String represention of the element.
        /// </summary>
        /// <returns>Tag form of element without attributes</returns>
        string ToString();

        /// <summary>
        /// This method return the complete CSS inline definition, which the style attribute contains.
        /// </summary>
        /// <remarks>
        /// For easy access to specific styles it is recommended to use the 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement.GetStyleAttribute">GetStyleAttribute</see> and 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement.SetStyleAttribute">SetStyleAttribute</see>
        /// methods. This and both alternative methods will check the content and cannot assign values not
        /// processed by the Internet Explorer Engine. The final behavior may vary depending on the currently
        /// installed IE engine.
        /// </remarks>
        /// <returns>Returns the complete style text (all rules) as one string.</returns>
        string GetStyle();
        /// <summary>
        /// Set the current style by overwriting the complete content of the style attribute.
        /// </summary>
        /// <param name="CssText">The style text; should be CSS compliant.</param>
        void SetStyle(string CssText);
        /// <summary>
        /// Gets a specific part of an inline style.
        /// </summary>
        /// <param name="styleName">The style attribute to retrieve</param>
        /// <returns>The string representation of the style. Returns <see cref="System.String.Empty"/> if the 
        /// style does not exists.</returns>
        string GetStyleAttribute(string styleName);
        /// <summary>
        /// Sets a specific part of an inline style.
        /// </summary>
        /// <remarks>
        /// Setting to <seealso cref="System.String.Empty">String.Empty</seealso> does remove
        /// the style name. For a more intuitive way use <see cref="RemoveStyleAttribute">RemoveStyleAttribute</see> instead.
        /// Setting impossible rule texts will either ignore the command or set unexpected values.
        /// </remarks>
        /// <param name="styleName">The name of the style rule beeing set, e.g. "font-family".</param>
        /// <param name="styleText">The rule content, like "Verdana,Arial" for the "font-family" rule.</param>
        void SetStyleAttribute(string styleName, string styleText);
        /// <summary>
        /// Removes an style attribute and his content from the inline style string.
        /// </summary>
        /// <param name="styleName">Name of style to be removed</param>
        void RemoveStyleAttribute(string styleName);        

        /// <summary>
        /// Sets an attribute to a specific value.
        /// </summary>
        /// <remarks>
        /// The command may does nothing if the value does not correspond with the attribute. E.g. it
        /// is almost impossible to write a pixel value if the attribute expects a font name.
        /// </remarks>
        /// <param name="attribute">The name of the attribute.</param>
        /// <param name="value">The value beeing written.</param>
        void SetAttribute(string attribute, object value);

        /// <summary>
        /// Remove the give attribute from element.
        /// </summary>
        /// <param name="attribute">The name of the attribute which is about to be removed. Case insensitive.</param>
        void RemoveAttribute(string attribute);

        /// <summary>
        /// Universal access to any attribute.
        /// </summary>
        /// <remarks>
        /// The type returned may vary depended on the internal type.
        /// </remarks>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>The object which is the value of the attribute.</returns>
        object GetAttribute(string attribute);

        /// <summary>
        /// Returns the DOM access to the element.
        /// </summary>
        /// <returns>The DOM representation of the element.</returns>
        IElementDom ElementDom { get; }

        /// <summary>
        /// Gets the current effective style, including styles defines internally, in linked sheets as well as through HTML.
        /// </summary>
        /// <remarks>
        /// This property shows the effective style if this element as a cascade of the global
        /// and inline styles defined elsewhere. Readonly.
        /// <para>        
        /// The property returns <c>null</c> (<c>Nothing</c> in VB.NET) if the effective style can not be retrieved.
        /// <para>See implementing classes for details</para> 
        /// </para>
        /// <seealso cref="RuntimeStyle"/>        
        /// <seealso cref="CurrentStyle"/>        
        /// </remarks>
        IEffectiveStyle EffectiveStyle { get; }

        /// <summary>
        /// Gets the current element's runtime style definition and allows interactive access.
        /// </summary>
        /// <remarks>
        /// This property allows access to styles not being persistent within the document. They affect only at runtime
        /// and can change the current appearance of an object. One can use this to add specific effects during user
        /// operation of to customize elements in particular situations.
        /// <para>See implementing classes for details</para>
        /// <seealso cref="EffectiveStyle"/>
        /// <seealso cref="CurrentStyle"/>
        /// <seealso cref="SetStyleAttribute"/>
        /// <seealso cref="RemoveStyleAttribute"/>
        /// <seealso cref="GetStyleAttribute"/>
        /// </remarks>
        IElementStyle RuntimeStyle { get; }

        /// <summary>
        /// Access to the style attribute in an object form.
        /// </summary>
        /// <para>See implementing classes for details</para>
        /// <seealso cref="EffectiveStyle"/>
        /// <seealso cref="RuntimeStyle"/>
        /// <seealso cref="SetStyleAttribute"/>
        /// <seealso cref="RemoveStyleAttribute"/>
        /// <seealso cref="GetStyleAttribute"/>
        IElementStyle CurrentStyle { get; }

    }
}