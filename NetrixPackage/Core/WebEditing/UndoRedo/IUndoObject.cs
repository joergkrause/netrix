using System;
namespace GuruComponents.Netrix.WebEditing.UndoRedo
{

    /// <summary>
    /// Defines one managed undo step.
    /// </summary>
    public interface IUndoObject
    {
        /// <summary>
        /// Name, usually the step itself, such as "Typing".
        /// </summary>
        string Description { get; }

        /// <summary>
        /// "Do" this step, hence, this undos the step actually.
        /// </summary>
        void Do();
    }
}
