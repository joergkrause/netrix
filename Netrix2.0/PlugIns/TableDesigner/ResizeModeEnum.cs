using System;

namespace GuruComponents.Netrix.TableDesigner
{
    /// <summary>
    /// Currently used resize mode, e.g. the direction of handle the user is moving.
    /// </summary>
    internal enum ResizeModeEnum
    {
        /// <summary>
        /// No resizing
        /// </summary>
        None = 0,
        /// <summary>
        /// West East. Handle: middle right border.
        /// </summary>
        WE   = 1,
        /// <summary>
        /// North South. Handle: middle bottom border.
        /// </summary>
        NS   = 2,
        /// <summary>
        /// Diagonal, from North West to South East. Handle: lower right corner.
        /// </summary>
        NWSE = 3
    }


}

