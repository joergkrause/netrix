namespace GuruComponents.Netrix
{
    /// <summary>
    /// Controls the alignment of text within a block element.
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// Align text to the left (default).
        /// </summary>
        Left    = System.Windows.Forms.HorizontalAlignment.Left,
        /// <summary>
        /// Center text within the container.
        /// </summary>
        Center  = System.Windows.Forms.HorizontalAlignment.Center,
        /// <summary>
        /// Align text to the right.
        /// </summary>
        Right   = System.Windows.Forms.HorizontalAlignment.Right,
        /// <summary>
        /// Justify text if justification is supported by the container.
        /// </summary>
        Full    = 3,
        /// <summary>
        /// Remove the alignment attribute.
        /// </summary>
        None    = 9
    }

}