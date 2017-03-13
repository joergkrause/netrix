namespace GuruComponents.Netrix.WebEditing.DragDrop
{
    /// <summary>
    /// The kind of drag conversion detected at drag enter event. Only CanConvert is handled
    /// internally, otherwise the Drop event will fired and the ConvertInfo will provided together
    /// with the dragged data.
    /// </summary>
    public enum DataObjectConverterInfo
    {
        /// <summary>
        /// Drag drop disabled
        /// </summary>
        Disabled    = 0,
        /// <summary>
        /// Drag content determined as HTML ans dropping is possible
        /// </summary>
        CanConvert  = 1,
        /// <summary>
        /// Dropping possible but not handled by NetRix, may be, MSHTML is handle this internally.
        /// </summary>
        Unhandled   = 2,
        /// <summary>
        /// FileDrop from operating system. Not handled but events are fired to handle this in the
        /// host application.
        /// </summary>
        Externally  = 3,
        /// <summary>
        /// Text, HTML
        /// </summary>
        Text = 4,
        /// <summary>
        /// Expects an object of type IElement
        /// </summary> 
        Native = 5
    }
}