using System;
using System.Windows.Forms;
namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Allows to attach an delegate which is being called when the PropertyGrid is about to show an error message.
    /// </summary>
    /// <param name="ex">The exception that causes the error.</param>
    /// <param name="message">The message (optional).</param>
    public delegate void ShowErrorDelegate(Exception ex, string message);
    /// <summary>
    /// Allows to attach an delegate which is being called when the PropertyGrid is about to show a message.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="caption"></param>
    /// <param name="buttons"></param>
    /// <returns></returns>
    public delegate DialogResult ShowMessageDelegate(string message, string caption, MessageBoxButtons buttons);
    /// <summary>
    /// Allows to attach an delegate which is being called when the PropertyGrid is about to open an dialog, such as a collection editor.
    /// </summary>
    /// <param name="form">The form which the control whishes to open. Could be replaced by another one.</param>
    /// <returns>The dialog result after calling ShowDialog.</returns>
    public delegate DialogResult ShowDialogDelegate(Form form);

    /// <summary>
    /// An public access to the common IUIService which provides more control over the PropertyGrid and other
    /// design time environments.
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// Whether or not the component editor icon is shown in the PropertyGrid.
        /// </summary>
        bool CanShowComponentEditor { get; set; }
        /// <summary>
        /// Request the owner window handle. Not yet supported.
        /// </summary>
        event EventHandler GetDialogOwner;
        /// <summary>
        /// Called when the user clicks the Component Editor icon of the PropertyGrid.
        /// </summary>
        event EventHandler ShowComponentEditor;
        /// <summary>
        /// Gets or sets the callback delegate for common dialogs.
        /// </summary>
        ShowDialogDelegate ShowDialogDialog { get; set; }
        /// <summary>
        /// Gets or sets the callback delegate for error messages.
        /// </summary>
        ShowErrorDelegate ShowErrorDialog { get; set; }
        /// <summary>
        /// Gets or sets the callback delegate for common messages.
        /// </summary>
        ShowMessageDelegate ShowMessageDialog { get; set; }
        /// <summary>
        /// Shows tool window. Not yet implemented.
        /// </summary>
        event EventHandler ShowToolWindow;
        /// <summary>
        /// Called when the component requests to refresh the UI.
        /// </summary>
        event EventHandler UIDirty;
    }
}