using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;
using System.Diagnostics;

namespace GuruComponents.Netrix.WebEditing.UndoRedo
{

    /// <summary>
    /// This class contains the undo manager, used internally to manage undo and redpo steps.
    /// </summary>
    public sealed class UndoManager : Interop.IOleUndoManager, IDisposable
    {
        private IHtmlEditor _editor;
        //private IUndoStack _stackContainer;
        private static Interop.IOleUndoManager oleUndoManagerInstance;
        private static Dictionary<string, UndoUnit> _stack = new Dictionary<string,UndoUnit>();
        private string _stackName;

        public UndoManager(IHtmlEditor editor)
        {
            _editor = editor;
            GetUndoManager();
        } 
        
        public UndoManager(string stackName, IHtmlEditor editor)
        {
            if (oleUndoManagerInstance == null)
                throw new ArgumentNullException("Improper Undo Manager call");
            _stackName = stackName;
            _startIndex = CountUndos();
        }

        // used internally for the base undo manager (parent for all others)
        private void GetUndoManager()
        {
            if (oleUndoManagerInstance == null)
            {
                Interop.IOleServiceProvider serviceProvider = _editor.GetActiveDocument(false) as Interop.IOleServiceProvider;
                Guid undoManagerGuid = typeof(Interop.IOleUndoManager).GUID;
                Guid undoManagerGuid2 = typeof(Interop.IOleUndoManager).GUID;
                IntPtr undoManagerPtr;
                int hr = serviceProvider.QueryService(ref undoManagerGuid2, ref undoManagerGuid, out undoManagerPtr);
                if ((hr == Interop.S_OK) && (undoManagerPtr != Interop.NullIntPtr))
                {
                    oleUndoManagerInstance = (Interop.IOleUndoManager)Marshal.GetObjectForIUnknown(undoManagerPtr);
                    Debug.WriteLine(oleUndoManagerInstance.GetHashCode(), "MANAGER INSTANCE");
                    Marshal.Release(undoManagerPtr);
                }
                else
                {
                    throw new ExecutionEngineException("Component threw an internal exception creating Undo manager.");
                }
            }
        }

        /// <summary>
        /// Return the stack that contains the unique or packed undo units this manager holds.
        /// </summary>
        public IUndoStack UndoStack
        {
            get { return null; } // _stackContainer; }
        }

        # region public members

        public event EventHandler<UndoEventArgs> NextAdded;

        Interop.IEnumOleUndoUnits _undoUnits;
        int _startIndex;
        int _numUndos;

        public void Close()
        {
            // number of elements within
            _numUndos = CountUndos();
            _numUndos = Math.Max(0, _numUndos - _startIndex);
            PackThis();

        }

        private int CountUndos()
        {
            int j = 0;
            int i = 0;
            //IntPtr k = IntPtr.Zero;
            Interop.IOleUndoUnit unit;
            _undoUnits = oleUndoManagerInstance.EnumUndoable();
            if (_undoUnits == null) return 0;
            try
            {
                _undoUnits.Reset();
            }
            catch
            {
                return 0;
            }
            try
            {
                while (i == 0)
                {
                    _undoUnits.Next(1, out unit, out i);
                    if (unit != null)
                    {
                        string ss;
                        unit.GetDescription(out ss);
                        Debug.WriteLine(unit.GetType().Name, ss);
                        if (unit is UndoStack)
                        {
                            // packed steps 
                        }
                        else
                        {
                            Marshal.ReleaseComObject(unit);
                        }
                    }
                    if (_undoUnits == null || i == 0)
                    {
                        break;
                    }
                    i = 0;
                    j++;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                _undoUnits.Reset();
            }
            return j;
        }

        private void PackThis()
        {
            Debug.WriteLine("PACK", GetHashCode() + " ON " + _stackName);
            Debug.WriteLine("PACK", "FROM " + _startIndex + " TO " + _numUndos);
            _undoUnits.Reset();
            Interop.IOleUndoUnit[] startunits = new Interop.IOleUndoUnit[(uint)_startIndex];
            Interop.IOleUndoUnit[] currentunits = new Interop.IOleUndoUnit[(uint)_numUndos];
            //IntPtr j1 = IntPtr.Zero;
            Interop.IOleUndoUnit unit = null;
            int su = 0;
            for (int k = 0; k < _startIndex; k++)
            {
                _undoUnits.Next(1, out unit, out su);
                startunits[k] = unit;
            }
            for (int i = 0; i < _numUndos; i++)
            {
                _undoUnits.Next(1, out unit, out su);
                currentunits[i] = unit;
            }
            //oleUndoManagerInstance.DiscardFrom(_stack[_stackName]);
            for (int j = 0; j < _startIndex; j++)
            {
                if (startunits[j] == null) continue;
                oleUndoManagerInstance.Add(startunits[j]);
            }

            //if (_stack[_stackName] != null)
            {
                //UndoUnit uu = new UndoUnit(unit, oleUndoManagerInstance);
                // add packed unit
                //oleUndoManagerInstance.Add(_stack[_stackName]);
            }
        }


        # endregion


        internal protected void OnNextAdded(UndoEventArgs e)
        {
            if (NextAdded != null)
            {
                NextAdded(this, e);
            }
        }

        public void Dispose()
        {
            Close();
        }

        string parentDescription = "";

        void Interop.IOleUndoManager.Open(Interop.IOleParentUndoUnit parentUndo)
        {
            oleUndoManagerInstance.Open(parentUndo);
            string ss;
            parentUndo.GetDescription(out ss);
            parentDescription = ss;
            Debug.WriteLine(ss, "UNDO - OPEN parent == " + parentUndo.GetHashCode().ToString() + " MANAGER == " + GetHashCode() + " ON " + _stackName);
        }

        void Interop.IOleUndoManager.Close(Interop.IOleParentUndoUnit parentUndo, bool fCommit)
        {
            oleUndoManagerInstance.Close(parentUndo, fCommit);
            string ss;
            parentUndo.GetDescription(out ss);
            Debug.WriteLine(ss, "UNDO - CLOSE == " + parentUndo.GetHashCode().ToString() + " MANAGER == " + GetHashCode() + " ON " + _stackName);
        }

        void Interop.IOleUndoManager.Add(Interop.IOleUndoUnit undoUnit)
        {
            //UndoUnit lastundoUnit= new UndoUnit(undoUnit, oleUndoManagerInstance);
            //if (!_stack.ContainsKey(_stackName))
            //{
            //    _stack.Add(_stackName, lastundoUnit);
            //}
            //oleUndoManagerInstance.Add(lastundoUnit); // add packed container here
            string s;
            undoUnit.GetDescription(out s);
            if (parentDescription == null) parentDescription = "";
            Debug.WriteLine(s + " for " + parentDescription, "UNDO - ADD unit == " + undoUnit.GetHashCode() + " MANAGER == " + GetHashCode() + " ON ");
            //UndoUnit uu = new UndoUnit(undoUnit, this);
            //OnNextAdded(new UndoEventArgs(uu));
        }

        long Interop.IOleUndoManager.GetOpenParentState()
        {
            Debug.WriteLine("", "UNDO - PARENT");
            // normal = 0, blocked = 1, nonparentable = 2
            return oleUndoManagerInstance.GetOpenParentState();
        }

        void Interop.IOleUndoManager.DiscardFrom(Interop.IOleUndoUnit undoUnit)
        {
            string ss = "NULL";
            if (undoUnit != null)
            {
                undoUnit.GetDescription(out ss);
            }
            Debug.WriteLine(ss, "UNDO - DISCARD");
            oleUndoManagerInstance.DiscardFrom(undoUnit);
        }

        void Interop.IOleUndoManager.UndoTo(Interop.IOleUndoUnit undoUnit)
        {
            string ss;
            undoUnit.GetDescription(out ss);
            Debug.WriteLine(ss, "UNDO - UNDO TO");
            oleUndoManagerInstance.UndoTo(undoUnit);
        }

        void Interop.IOleUndoManager.RedoTo(Interop.IOleUndoUnit undoUnit)
        {
            string ss;
            undoUnit.GetDescription(out ss);
            Debug.WriteLine(ss, "UNDO - REDO TO");
            oleUndoManagerInstance.RedoTo(undoUnit);
        }

        Interop.IEnumOleUndoUnits Interop.IOleUndoManager.EnumUndoable()
        {
            return oleUndoManagerInstance.EnumUndoable();
        }

        Interop.IEnumOleUndoUnits Interop.IOleUndoManager.EnumRedoable()
        {
            throw new NotImplementedException();
        }

        string Interop.IOleUndoManager.GetLastUndoDescription()
        {
            return oleUndoManagerInstance.GetLastUndoDescription();
        }

        string Interop.IOleUndoManager.GetLastRedoDescription()
        {
            throw new NotImplementedException();
        }

        void Interop.IOleUndoManager.Enable(bool fEnable)
        {
            oleUndoManagerInstance.Enable(fEnable);
            Debug.WriteLine("", "UNDO - ENABLE");
        }
    }
}
