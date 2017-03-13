using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This enumaration is used to determine the rules between cells.
    /// </summary>
    /// <remarks>
    /// Rules are
    /// an HTML 4.0 feature to control lines between rows and columns separatly.
    /// The default value is "All", which is in effect the same behavior as "NotSet".
    /// </remarks>
    public enum RulesType
    {
        /// <summary>
        /// No rules set. Behavior inherited.
        /// </summary>
        NotSet = 0,
        /// <summary>
        /// No rules drawn.
        /// </summary>
        None = 1,
        /// <summary>
        /// Draw rules around column groups.
        /// </summary>
        Groups = 2,
        /// <summary>
        /// Draw rules for rows (horizontal).
        /// </summary>
        Rows = 3,
        /// <summary>
        /// Draw rules for columns (vertical).
        /// </summary>
        Cols = 4,
        /// <summary>
        /// Draw all rules (Default behavior).
        /// </summary>
        All = 5,
    }
}