using System;

namespace GuruComponents.Netrix.WebEditing.UndoRedo
{
    /// <summary>
    /// Defines which type of stack we use.
    /// </summary>
    public enum BatchedUndoType
    {
        /// <summary>
        /// This is an undo stack.
        /// </summary>
        Undo = 1,
        /// <summary>
        /// This is a redo stack.
        /// </summary>
        Redo = 2
    }
}