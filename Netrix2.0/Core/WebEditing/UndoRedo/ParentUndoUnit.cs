using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.UndoRedo
{
    /// <summary>
    /// Wrapper for an undo operation's parent object. Currently not used.
    /// </summary>
    public class ParentUndoUnit : Interop.IOleParentUndoUnit
    {
        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="undoManager"></param>
        /// <returns></returns>
        public int Do(Interop.IOleUndoManager undoManager)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="bStr"></param>
        /// <returns></returns>
        public int GetDescription(out string bStr)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="clsid"></param>
        /// <param name="plID"></param>
        /// <returns></returns>
        public int GetUnitType(out int clsid, out int plID)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <returns></returns>
        public int OnNextAdd()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="parentUnit"></param>
        /// <returns></returns>
        public int Open(Interop.IOleParentUndoUnit parentUnit)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="parentUnit"></param>
        /// <param name="fCommit"></param>
        /// <returns></returns>
        public int Close(Interop.IOleParentUndoUnit parentUnit, bool fCommit)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="undoUnit"></param>
        /// <returns></returns>
        public int Add(Interop.IOleUndoUnit undoUnit)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="undoUnit"></param>
        /// <returns></returns>
        public int FindUnit(Interop.IOleUndoUnit undoUnit)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public int GetParentState(out long state)
        {
            throw new NotImplementedException();
        }
    }

}
