using System;

namespace GuruComponents.Netrix.SpellChecker
{

    /// <summary>
    /// Public interface a form must implement to be accepted as an internal dialog.
    /// </summary>
    /// <remarks>
    /// The dialog is shown modal.
    /// </remarks>
    interface ISpellcheckerDialog
    {
        /// <summary>
        /// Action fired by the dialog. The form's DialogResult property determines what happens next.
        /// </summary>
        /// <remarks>
        /// Implementers should close the dialog and return one of these dialog results:
        /// <list type="bullet">
        ///     <item>OK: Replace the word with the new one (<see cref="NewWord"/>)</item>
        ///     <item>Abort: Abort and close the dialog without further action.</item>
        ///     <item>Cancel: Same as Abort</item>
        ///     <item>Ignore: Ignore the word and proceed with next one.</item>
        /// </list>
        /// </remarks>
        event EventHandler Action;
        /// <summary>
        /// The word used to replace the current one.
        /// </summary>
        string NewWord { get; set; }
        /// <summary>
        /// A list of suggestions the form can present.
        /// </summary>
        System.Collections.Generic.List<string> Suggestions { set; }
        /// <summary>
        /// The word not found in the dictionary to be shown on the form.
        /// </summary>
        string WrongWord { get; set; }
    }
}
