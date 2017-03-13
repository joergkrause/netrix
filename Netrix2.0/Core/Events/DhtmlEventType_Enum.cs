namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// DHTML events fired by document in design mode. This list applies on document level, however, events might fire on element level and appear on document level if not handled elsewhere.
    /// </summary>
    public enum DocumentEventType
    {
        /// <summary>
        /// The element lost the capture (focus) after beeing selected.
        /// </summary>
        LoseCapture = 0,
        /// <summary>
        /// A single mouse click.
        /// </summary>
        Click       = 1,
        /// <summary>
        /// A double mouse click.
        /// </summary>
        DblClick    = 2,
        /// <summary>
        /// The element beguns to start dragging.
        /// </summary>
        DragStart   = 3,
        /// <summary>
        /// A key was pressed. This happens before the key handler has processed the key.
        /// </summary>
        KeyDown     = 4,
        /// <summary>
        /// The key was pressed and the key handler has processed the message.
        /// </summary>
        KeyPress    = 5,
        /// <summary>
        /// The key was pressed and released.
        /// </summary>
        KeyUp       = 6,
        /// <summary>
        /// The mouse key comes down and has not yet been released.
        /// </summary>
        MouseDown   = 7,
        /// <summary>
        /// The element starts resizing. This happens if the user move the handles around the
        /// elements rectangle area with the mouse.
        /// </summary>
        ResizeStart = 8,
        /// <summary>
        /// The element was resized. This happens if the user stops resizing by releasing the mouse
        /// button.
        /// </summary>
        ResizeEnd   = 9,
        /// <summary>
        /// The mouse enters the elements boundaries during a mouse move operation.
        /// </summary>
        MouseEnter  = 10,
        /// <summary>
        /// The mouse leaves the elements boundaries during a mouse move operation.
        /// </summary>
        MouseLeave  = 11,
        /// <summary>
        /// The mosue moves over that element. This is fired multiple times and the handler
        /// should be aware about performance reducing operations. 
        /// </summary>
        MouseMove   = 12,
        /// <summary>
        /// The mouse has left the elements boundaries during a mouse move operation.
        /// </summary>
        MouseOut    = 13,
        /// <summary>
        /// The mouse has entered the elements boundaries during a mouse move operation.
        /// </summary>
        MouseOver   = 14,
        /// <summary>
        /// The mouse button has been released
        /// </summary>
        MouseUp     = 15,
        /// <summary>
        /// The element was selected either by mouse or key operation. This normally follows
        /// a click operation on a selectable element (image, link, ...).
        /// </summary>
        SelectStart = 16,
        /// <summary>
        /// A scroll event occured, either user has pulled scrollbar or element scrolled by program action.
        /// </summary>
        Scroll = 17,
        /// <summary>
        /// Any other operation currently not recognized from within the NetRix environment.
        /// </summary>
        Move,
        /// <summary>
        /// Start moving...
        /// </summary>
        MoveStart,
        /// <summary>
        /// End moving...
        /// </summary>
        MoveEnd,
        /// <summary>
        /// Resize
        /// </summary>
        Resize,
        /// <summary>
        /// Control being selected...
        /// </summary>
        ControlSelect,
        /// <summary>
        /// Drop
        /// </summary>
        Drop,
        /// <summary>
        /// Fired if the selection of a text portion changes.
        /// </summary>
        SelectionChange,
        /// <summary>
        /// Any other
        /// </summary>
        Unknown = 99
    }
}
