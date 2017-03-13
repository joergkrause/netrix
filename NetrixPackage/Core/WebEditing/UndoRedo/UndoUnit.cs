using System;
using GuruComponents.Netrix.ComInterop;
namespace GuruComponents.Netrix.WebEditing.UndoRedo
{

    /// <summary>
    /// Wrapper for IOleUndoUnit.
    /// </summary>
    public class UndoUnit : Interop.IOleUndoUnit
    {

        private Interop.IOleUndoUnit _unit;
        private Interop.IOleUndoManager _undoManager;

        /// <summary>
        /// Ctor for IOleUndoUnit object.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="undoManager"></param>
        public UndoUnit(Interop.IOleUndoUnit unit, Interop.IOleUndoManager undoManager)
        {
            _unit = unit;
            _undoManager = undoManager;
        }

        /// <summary>
        /// Undo a step.
        /// </summary>
        public void Do()
        {
            _unit.Do(_undoManager);
        }

        /// <summary>
        /// Get name of step.
        /// </summary>
        public string Description
        {
            get
            {
                string s;
                GetDescription(out s);
                return s;
            }
        }

        int Interop.IOleUndoUnit.Do(Interop.IOleUndoManager undoManager)
        {
            return _unit.Do(undoManager);
        }
        /// <summary>
        /// Name of an undo step.
        /// </summary>
        /// <param name="bStr"></param>
        /// <returns></returns>
        public int GetDescription(out string bStr)
        {
            if (_unit == null)
            {
                bStr = "Packed Operation";
                return 0;
            }
            return _unit.GetDescription(out bStr);
        }

        /// <summary>
        /// Get COM unit type.
        /// </summary>
        /// <param name="clsid"></param>
        /// <param name="plID"></param>
        /// <returns></returns>
        public int GetUnitType(out int clsid, out int plID)
        {
            return _unit.GetUnitType(out clsid, out plID);
        }
        /// <summary>
        /// Invokes the NextAdd event
        /// </summary>
        /// <returns>returns 1 if the event has been fired, 0 else.</returns>
        public int OnNextAdd()
        {
            if (NextAdd != null)
            {
                NextAdd(this, EventArgs.Empty);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        /// Fired if a new step has been added.
        /// </summary>
        public event EventHandler NextAdd;

    }


}
