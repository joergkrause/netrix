using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.UndoRedo
{

    /// <summary>
    /// This class realises a simple batched undo manager.
    /// </summary>
    /// <remarks>
    /// Its purpose is to pack multiple action in one stack operation to simplify undo for the user. 
    /// </remarks>
    public class BatchedUndoUnit : Interop.IOleUndoUnit, IUndoStack
    {
        private string _description;
        private int _startIndex;
        private int _numUndos;
        private Interop.IOleUndoUnit[] _undoUnits;
        private BatchedUndoType _type;
        private IHtmlEditor _editor;

        internal BatchedUndoUnit(string description, IHtmlEditor editor, BatchedUndoType type)
        {
            _description = description;
            _type = type;
            _editor = editor;
        }

        private Interop.IOleUndoManager undoManager
        {
            get { return ((HtmlEditor)_editor).UndoManager; }
        }

        /// <summary>
        /// Return the type of this unit, either Undo or Redo.
        /// </summary>
        public BatchedUndoType Type
        {
            get
            {
                return _type;
            }
        }

		/// <summary>
		/// Returns the collection of Undo/Redo objects available.
		/// </summary>
        /// <remarks>
        /// The current type of unit object decided whether this method returns the Undo or Redo history.
        /// The collections consists of <see cref="UndoObject"/> objects, which implement <see cref="IUndoObject"/>.
        /// These objects may have information about subobjects, which the undo manager creates if the user 
        /// requests packed undo sequences.
        /// </remarks>
        /// <seealso cref="NumChildUndos"/>
        /// <seealso cref="HasChildUndos"/>
        /// <seealso cref="UndoObject"/>
        /// <seealso cref="Type"/>
        /// <returns>Returns a collection of objects of type <see cref="IUndoObject"/>.</returns>
        public System.Collections.Generic.List<IUndoObject> GetUndoHistory()
        {
            int i = 0;
            //IntPtr k = IntPtr.Zero;
            Interop.IOleUndoUnit unit;
            Interop.IEnumOleUndoUnits undoUnits = null;
            if (_type == BatchedUndoType.Undo)
            {
                undoUnits = ((HtmlEditor)_editor).UndoManager.EnumUndoable();
            }
            else
            {
                undoUnits = ((HtmlEditor)_editor).UndoManager.EnumRedoable();
            }
            System.Collections.Generic.List<IUndoObject> undos = new System.Collections.Generic.List<IUndoObject>();
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
                        }                        //Interop.IOleUndoUnit unit = (Interop.IOleUndoUnit) Marshal.GetObjectForIUnknown(k);
                        string s;
                        unit.GetDescription(out s);
                        UndoUnit managedUndo = null;
                        if (unit is UndoUnit)
                        {
                            managedUndo = (UndoUnit)unit;
                        }
                        else
                        {
                            managedUndo = new UndoUnit(unit, ((HtmlEditor)_editor).UndoManager);
                        }
                        UndoObject wrappedObject = new UndoObject(s, managedUndo, ((HtmlEditor)_editor).UndoManager);
                        undos.Add(wrappedObject);
                        i = 0;
                    }
                }
                catch
                {

                }
            }
            return undos;
        }

        /// <summary>
        /// Returns <c>true</c> if there are child undo objects available.
        /// </summary>
        /// <seealso cref="NumChildUndos"/>
        /// <seealso cref="Type"/>
        public bool HasChildUndos
        {
            get
            {
                return (NumChildUndos > 0);
            }
        }

        /// <summary>
        /// Returns the number of child undos available.
        /// </summary>
        /// <seealso cref="HasChildUndos"/>
        /// <seealso cref="Type"/>
        public int NumChildUndos
        {
            get
            {
                if (_undoUnits != null)
                    return _undoUnits.Length;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns the array of child undo objects, if this is privatly packed stack.
        /// </summary>
        /// <seealso cref="IUndoObject"/>
        /// <seealso cref="Type"/>
        public UndoObject[] ChildUndos
        {
            get
            {
                UndoObject[] childUndos = new UndoObject[0];
                if (_undoUnits != null && _undoUnits.Length > 0)
                {
                    childUndos = new UndoObject[_undoUnits.Length];
                    for (int i = 0; i < _undoUnits.Length; i++)
                    {
                        Interop.IOleUndoUnit childUnit = _undoUnits[i];
                        string s;
                        childUnit.GetDescription(out s);
                        UndoUnit managedUnit = new UndoUnit(childUnit, ((HtmlEditor)_editor).UndoManager);
                        childUndos[i] = new UndoObject(s, managedUnit, ((HtmlEditor)_editor).UndoManager);
                    }
                }
                return childUndos;
            }

        }

		/// <summary>
		/// Closes the current stack.
		/// </summary>
		/// <remarks>
		/// Using this method will pack all previously collected operations into one undo steps. Next time the 
		/// Undo command is send the control will undo all packed steps.
		/// </remarks>
        public void Close()
        {
            Interop.IEnumOleUndoUnits undoUnits, redoUnits;
            try
            {
                int i = 0;
                if (_type == BatchedUndoType.Undo)
                {
                    undoUnits = undoManager.EnumUndoable();
                    i = CountUndos(undoUnits);
                    _numUndos = Math.Max(0, i - _startIndex);
                    Pack(undoManager.EnumUndoable());
                }
                else
                {
                    redoUnits = undoManager.EnumRedoable();
                    i = CountUndos(redoUnits);
                    _numUndos = Math.Max(0, i - _startIndex);
                    Pack(undoManager.EnumRedoable());
                }
            }
            catch
            {
            }
        }

		/// <summary>
		/// Clears the complete collection of undo/redo stacks.
		/// </summary>
		/// <remarks>
		/// After issuing this method the control will lose all undo stacks and reset the Undo command
		/// to unavailable state. The next operation will become the first undoable one.
		/// </remarks>
        public void Reset()
        {
            try
            {
                this.undoManager.Enable(false);
                this.undoManager.Enable(true);
                Close();
            }
            catch
            {
            }
        }

		/// <summary>
		/// Enables or Disables the undo manager.
		/// </summary>
		/// <remarks>
		/// If the undo manager is disabled, any following operation will not become part of the undo stack and 
		/// cannot be "undone" by any action, whether it is sent by user or code.
		/// </remarks>
		/// <param name="enabled">Specifies <c>True</c> for enable and <c>False</c> for disable.</param>
        public void Enable(bool enabled)
        {
            this.undoManager.Enable(enabled);
        }
        
		/// <summary>
		/// Gets the number of undo/redo steps currently in the stack.
		/// </summary>
		/// <remarks>
		/// This property returns the number of undo/redo steps that can be issued by the undo command, NOT the
		/// number of packed steps, each undo manager instance covers. When one did five operations, then
		/// packed six operations into one stack and sent then two single operations, this property will
		/// returns eight (five, one for the packed stack and two).
		/// </remarks>
        /// <seealso cref="Type"/>
        public int UndoSteps
        {
            get
            { 
                Interop.IEnumOleUndoUnits undoUnits;
                if (_type == BatchedUndoType.Undo)
                {
                    undoUnits = undoManager.EnumUndoable();
                }
                else
                {
                    undoUnits = undoManager.EnumRedoable();
                }
                int i = CountUndos(undoUnits);
                return i;
            }
        }
        
		/// <summary>
		/// Counts number of undo operations.
		/// </summary>
		/// <param name="enumerator"></param>
		/// <returns>Number of undo operations.</returns>
        private int CountUndos(Interop.IEnumOleUndoUnits enumerator)
        {
            int j = 0;
            int i = 0;
            //IntPtr k = IntPtr.Zero;
            Interop.IOleUndoUnit unit;
			if (enumerator == null) return 0;            
			try
			{
				enumerator.Reset();
			}
			catch
			{
				return 0;
			}
			try
			{
                while (i == 0)
                {
                    enumerator.Next(1, out unit, out i);
                    if (unit != null)
                    {
                        if (unit is BatchedUndoUnit)
                        {
                            // packed steps 
                        }
                        else
                        {
                            Marshal.ReleaseComObject(unit);
                        }
                    }
                    if (enumerator == null || i == 0)
                    {
                        break;
                    }
                    i = 0;
                    j++;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message, "UNDO->COUNTUNDOS->ERROR");
            }
            finally 
            {
                enumerator.Reset();
            }
            return j;
        }

		/// <summary>
		/// Instructs the undo unit to carry out its action. Note that if it 
		/// contains child undo units, it must call their Do methods as well.
		/// </summary>
		/// <param name="undoManager">Specifies pointer to the undo manager.</param>
		/// <returns>Returns non zero interger value if the undo unit 
		/// successfully carried out its action</returns>
        public virtual int Do(Interop.IOleUndoManager undoManager)
        {
            try
            {
                for (int i = _numUndos - 1 ; i >= 0; i--)
                {
                    _undoUnits[i].Do(undoManager);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message, "UNDO->DO->ERROR");
            }
            return Interop.S_OK;
        }

		/// <summary>
		/// Gets the name of the undo manager.
		/// </summary>
        public string Name
        {
            get
            {
                string s;
                GetDescription(out s);
                return s;
            }
        }

		/// <summary>
		/// Gets the description of the last operation collected by the undo/redo manager.
		/// </summary>
		/// <returns>Returns description of last operation.</returns>
        public string GetLastDescription()
        {
            try
            {
                if (_type == BatchedUndoType.Undo)
                {
                    if (this._numUndos > 0)
                        return this.undoManager.GetLastUndoDescription();
                    else
                        return String.Empty;
                }
                else
                {
                    return this.undoManager.GetLastRedoDescription();
                }
            }
            catch
            {
                return String.Empty;
            }
        }

		/// <summary>
		/// Returns a string that describes the undo unit and can be used in the undo 
		/// or redo user interface.
		/// </summary>
		/// <param name="s">Specifies the string describing this undo unit.</param>
		/// <returns>Returns non zero value if the string was successfully returned. </returns>
        public virtual int GetDescription(out string s)
        {
            s = _description;
            return Interop.S_OK;
        }

		/// <summary>
		/// Returns the CLSID and a type identifier for the undo unit
		/// </summary>
		/// <param name="clsid">Specifies CLSID for the undo unit.</param>
		/// <param name="plID">Specifies the type identifier for the undo unit. </param>
		/// <returns></returns>
        public virtual int GetUnitType(out int clsid, out int plID)
        {
            clsid = 0;
            plID = (int) _type;
            return Interop.S_OK;
        }

        public void Open()
        {
            try
            {
                Interop.IEnumOleUndoUnits units = null;
                if (_type == BatchedUndoType.Undo)
                {
                    undoManager.Enable(true);
                    units = undoManager.EnumUndoable();
                    if (units != null)
                    {
                        _startIndex = CountUndos(units);
                    }
                }
                else
                {
                    undoManager.Enable(true);
                    units = undoManager.EnumRedoable();
                    if (units != null)
                    {
                        _startIndex = CountUndos(units);
                    }
                }
            }
            catch
            {
                _startIndex = 0; // restart in case of error
            }
        }

        /// <summary>
        /// Notifies that a new unit has been added.
        /// </summary>
        public event EventHandler NextOperationAdded;

        /// <summary>
        /// Notifies the last undo unit in the collection that a new unit has been added.
        /// </summary>
        /// <remarks>
        /// An object can create an undo unit for an action and add it to the undo manager 
        /// but can continue inserting data into it through private interfaces. When the undo 
        /// unit receives a call to this method, it communicates back to the creating object 
        /// that the context has changed. Then, the creating object stops inserting data into 
        /// the undo unit.
        /// The parent undo unit calls this method on its most recently added child undo unit 
        /// to notify the child unit that the context has changed and a new undo unit has been 
        /// added.
        /// For example, this method is used for supporting fuzzy actions, like typing, which 
        /// do not have a clear point of termination but instead are terminated only when 
        /// something else happens.
        /// </remarks>
        /// <returns>Returns always non zero value. This return type is provided only for remotability.</returns>
        public virtual int OnNextAdd()
        {
            if (NextOperationAdded != null)
            {
                NextOperationAdded(this as IUndoStack, new EventArgs());
            }
            return Interop.S_OK;
        }

        private void Pack(Interop.IEnumOleUndoUnits enumerator)
        {
            enumerator.Reset();
            Interop.IOleUndoUnit[] units = new Interop.IOleUndoUnit[(uint)_startIndex];
            _undoUnits = new Interop.IOleUndoUnit[(uint)_numUndos];
            //IntPtr j1 = IntPtr.Zero;
            Interop.IOleUndoUnit unit;
            int i1 = 0;
            for (int k = 0; k < _startIndex; k++)
            {                               
                enumerator.Next(1, out unit, out i1);
                units[k] = unit; // (Interop.IOleUndoUnit)Marshal.GetObjectForIUnknown(unit);
                //Marshal.Release(unit);
            }
            for (int i2 = 0; i2 < _numUndos; i2++)
            {
                enumerator.Next(1, out unit, out i1);
                _undoUnits[i2] = unit; // (Interop.IOleUndoUnit)Marshal.GetObjectForIUnknown(unit);
                //Marshal.Release(unit);
            }
            undoManager.DiscardFrom(null);
            for (int j2 = 0; j2 < _startIndex; j2++)
            {
                if (units[j2] == null) continue;
                undoManager.Add(units[j2]);
            }
            undoManager.Add(this);
           
        }
    }
}
