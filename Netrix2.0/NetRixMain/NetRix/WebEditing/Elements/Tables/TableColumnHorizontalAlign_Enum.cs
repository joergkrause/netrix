using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This enumaration is used for the TBODY element to align the content. The only
    /// difference to the embedded align enumeration is char type.
    /// </summary>
    public enum TableColumnHorizontalAlign
    {
        /// <summary>
        /// Align left.
        /// </summary>
        Left    = 1,
        /// <summary>
        /// Align right.
        /// </summary>
        Right   = 2,
        /// <summary>
        /// Align center.
        /// </summary>
        Center  = 3,
        /// <summary>
        /// Align at the character defined with the char attribute.
        /// </summary>
        Char    = 4,
        /// <summary>
        /// Justify.
        /// </summary>
        Justify = 5,
        /// <summary>
        /// Default behavior. Most browsers align left by default.
        /// </summary>
        NotSet  = 0,
    }
}