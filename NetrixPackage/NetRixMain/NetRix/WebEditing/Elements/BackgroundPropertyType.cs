namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// Defines the behavior of the background of the document.
    /// </summary>
    public enum BackgroundPropertyType
    {
        /// <summary>
        /// The background scrolls with the document (Default, recommended).
        /// </summary>
        Scroll = 0,
        /// <summary>
        /// The background is fixed and the text scrolls over the background.
        /// </summary>
        Fixed  = 1,
        /// <summary>
        /// Remove Attribute (used internally for the property support).
        /// </summary>
        None = 9
    }
}
