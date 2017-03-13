using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.Netrix.WebEditing.Styles
{
    /// <summary>
    /// Alignment of text on the line or within a container.
    /// </summary>
    public enum TextAlign
    {
        /// <summary>
        /// Left alignment (default it not set).
        /// </summary>
        Left,
        /// <summary>
        /// Right alignment.
        /// </summary>
        Right,
        /// <summary>
        /// Center within container or page body.
        /// </summary>
        Center,
        /// <summary>
        /// Justify (full align).
        /// </summary>
        Justify,
        /// <summary>
        /// The value is not set. If assigned as value the attribute is being removed.
        /// </summary>
        NotSet
    }
}
