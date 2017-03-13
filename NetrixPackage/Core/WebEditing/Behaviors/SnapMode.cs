using System;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
    /// <summary>
    /// Determines how the 'snap element' feature behaves against snapzones.
    /// </summary>
    public enum SnapMode
    {

        /// <summary>
        /// Snap element against grid. Doesn't matter if grid is visible or not.
        /// </summary>        
        Grid,
        /// <summary>
        /// Snap element agains helpline. Helpline must be visible to snap.
        /// </summary>
        HelpLine,
        /// <summary>
        /// Snap against both, grid and helpline.
        /// </summary>
        Both,
        /// <summary>
        /// Turn off any snap behavior.
        /// </summary>
        None
    }

}
