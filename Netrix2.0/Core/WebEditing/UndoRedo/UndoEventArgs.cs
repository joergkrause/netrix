using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.Netrix.WebEditing.UndoRedo
{

    /// <summary>
    /// Contains the current undo step.
    /// </summary>
    /// <remarks>The property <see cref="System.Web.UI.WebControls.Unit">Unit</see> contains information about was has been added to the undo step recently.</remarks>
    public class UndoEventArgs : EventArgs
    {
        /// <summary>
        /// Ctor for event args.
        /// </summary>
        /// <param name="uu"></param>
        public UndoEventArgs(IUndoStack uu)
        {
            Stack = uu;
            
        }

        private IUndoStack _iundoStack;
         /// <summary>
        /// The current undo step.
        /// </summary>
        /// <seealso cref="UndoUnit"/>
       public IUndoStack Stack { 
            get{return _iundoStack;}
            set {_iundoStack=value ;}
        }
    }

}
