using System;

namespace GuruComponents.Netrix.WebEditing.HighLighting
{
    /// <summary>
    /// Compares the end point of a text range with the end point of another range.
    /// </summary>
    public enum CompareUnit
    {
        /// <summary>
        /// Compares the position of the start with the end of the other range.
        /// </summary>
        StartToEnd,
        /// <summary>
        /// Compares the position of the start with the start of the other range.
        /// </summary>
        StartToStart,
        /// <summary>
        /// Compares the position of the end with the start of the other range.
        /// </summary>
        EndToStart,
        /// <summary>
        /// Compares the position of the end with the end of the other range.
        /// </summary>
        EndToEnd
    }
}
