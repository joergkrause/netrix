using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.Netrix.WebEditing
{
    /// <summary>
    /// Current loader state.
    /// </summary>
    /// <remarks>
    /// If parsing fails for some reason the "Ready" state is never reached.
    /// When using this state a time out value is required.
    /// </remarks>
    public enum ReadyState
    {
        /// <summary>
        /// Is in loading state, e.g. it's loading, but not yet parsed.
        /// </summary>
        Loading,
        /// <summary>
        /// Is in interactive state, e.g. it's loaded and control is now parsing.
        /// </summary>
        Interactive,
        /// <summary>
        /// Is ready, e.g. parsing is done successfully.
        /// </summary>
        Complete
    }
}
