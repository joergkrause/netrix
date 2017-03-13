using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.UndoRedo
{
    
    public class UndoStack : IUndoStack
    {
        private int _startIndex;
        private int _numUndos;

        private string _description;
        private List<UndoUnit> _childUndos;
        private UndoManager _undoManager;
        Interop.IEnumOleUndoUnits _undoUnits;

        public UndoStack(string description, UndoManager manager, IHtmlEditor editor)
        {
            _description = description;
            _childUndos = new List<UndoUnit>();
            _startIndex = 0;
            _numUndos = 0;
            _undoManager = manager;
            
        }

        public virtual BatchedUndoType Type
        {
            get { return BatchedUndoType.Undo; }
        }

        public bool HasChildUndos
        {
            get { return NumChildUndos > 0; }
        }

        public int NumChildUndos
        {
            get { return _childUndos.Count; }
        }

        public UndoUnit[] ChildUndos
        {
            get { return _childUndos.ToArray(); }
        }

        public List<IUndoObject> GetUndoHistory()
        {
            int i = 0;
            Interop.IOleUndoUnit unit;
            Interop.IEnumOleUndoUnits undoUnits = null;
            undoUnits = ((Interop.IOleUndoManager)_undoManager).EnumUndoable();
            System.Collections.Generic.List<UndoUnit> undos = new List<UndoUnit>();
            if (undoUnits != null)
            {
                undoUnits.Reset();
                try
                {
                    while (i == 0)
                    {
                        undoUnits.Next(1, out unit, out i);
                        if (undoUnits == null || i == 0)
                        {
                            break;
                        }
                        undos.Add(new UndoUnit(unit, ((Interop.IOleUndoManager)_undoManager)));
                        i = 0;
                    }
                }
                catch
                {

                }
            }
            List<IUndoObject> undo2 = new List<IUndoObject>();
            foreach (UndoUnit u1 in undos) {
                undo2.Add(new UndoObject(u1.Description, u1, _undoManager));
            }
            return undo2;
        }

        public void Open()
        {
            ((Interop.IOleUndoManager)_undoManager).Enable(true);
            _undoUnits = ((Interop.IOleUndoManager)_undoManager).EnumUndoable();
            if (_undoUnits != null)
            {
                _startIndex = CountUndos();
            }
            else
            {
                _startIndex = 0;
            }
        }

        public void Close()
        {
            try
            {
                int i = 0;
                _undoUnits = ((Interop.IOleUndoManager)_undoManager).EnumUndoable();
                i = CountUndos();
                _numUndos = Math.Max(0, i - _startIndex);
                PackThis();
            }
            catch
            {
            }
        }

        public string Name
        {
            get { return _description; }
        }

        private void PackThis()
        {
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
            ((Interop.IOleUndoManager)_undoManager).DiscardFrom(null);
            for (int j = 0; j < _startIndex; j++)
            {
                if (startunits[j] == null) continue;
                ((Interop.IOleUndoManager)_undoManager).Add(startunits[j]);
            }
            if (unit != null)
            {
                UndoUnit uu = new UndoUnit(unit, ((Interop.IOleUndoManager)_undoManager));
                ((Interop.IOleUndoManager)_undoManager).Add(uu);
            }
        }

        private int CountUndos()
        {
            int j = 0;
            int i = 0;
            //IntPtr k = IntPtr.Zero;
            Interop.IOleUndoUnit unit;
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

    }
}
