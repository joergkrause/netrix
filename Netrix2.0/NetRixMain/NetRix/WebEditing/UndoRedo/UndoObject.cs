using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.UndoRedo
{

    /// <summary>
    /// This class holds one undo step. 
    /// </summary>
    /// <remarks>
    /// The class is used as part of the undo history list and to perform single Undo steps on this list besides the regular
    /// undo chain. To perfom an Undo step just call the <see cref="Do"/> method.
    /// </remarks>
    public sealed class UndoObject : GuruComponents.Netrix.WebEditing.UndoRedo.IUndoObject
    {
        private string description;
        private UndoUnit unit;
        private Interop.IOleUndoManager undoManager;

        internal UndoObject(string description, UndoUnit unit, Interop.IOleUndoManager undoManager)
        {
            this.description = description;
            this.unit = unit;
            this.undoManager = undoManager;
        }

        /// <summary>
        /// The description (internal name) of the Undo step.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// A method to  perform an Undo action specific to this step.
        /// </summary>
        /// <remarks>
        /// In case this is a parent unit it will undo all containing child steps.
        /// </remarks>
        public void Do()
        {
            unit.Do();
        }

    }

}
