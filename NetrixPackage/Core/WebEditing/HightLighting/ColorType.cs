namespace GuruComponents.Netrix.WebEditing.HighLighting
{
    /// <summary>
    /// Type of Color in HighLightColor object.
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// Any color value.
        /// </summary>
        Color,
        /// <summary>
        /// Color of the parent object.
        /// </summary>
        Inherit,
        /// <summary>
        /// Color of the text is determined by the browser; for example, by default or inheritance. Default.
        /// </summary>
        Auto,
        /// <summary>
        /// Sets the color to transparent color.
        /// </summary>
        Transparent
    }
}
