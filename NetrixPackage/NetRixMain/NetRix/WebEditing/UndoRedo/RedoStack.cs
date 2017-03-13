using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.UndoRedo
{

   
    public class RedoStack : UndoStack
    {

        public RedoStack(string description, UndoManager manager, IHtmlEditor editor)
            : base(description, manager, editor)
        {
        }

        public override BatchedUndoType Type
        {
            get
            {
                return BatchedUndoType.Redo;
            }
        }
    }

    }
