using System;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.UndoRedo
{

    /// <summary>
    /// Supports the UNDO infrastructure. Do not implement.
    /// </summary>
    public interface IUndoStack
    {
        /// <summary>
        /// Return the type of this unit stack, either Undo or Redo.
        /// </summary>
        BatchedUndoType Type { get; }

        /// <summary>
        /// List all names of available Undo steps in the undo stack.
        /// </summary>
        System.Collections.Generic.List<IUndoObject> GetUndoHistory();

        /// <summary>
        /// Tells whether this stack has already gathered at least one step.
        /// </summary>
        bool HasChildUndos { get; }

        /// <summary>
        /// Number of steps.
        /// </summary>
        int NumChildUndos { get; }

        /// <summary>
        /// Closes the instance and packs the steps into one that's added to the parent stack.
        /// </summary>
        void Close();

        /// <summary>
        /// Open the instance to gather steps.
        /// </summary>
        void Open();

        /// <summary>
        /// The stack's name, a summary of all internal steps.
        /// </summary>
        string Name { get; }

    }
}