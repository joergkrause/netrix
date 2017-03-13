using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using GuruComponents.Netrix.UserInterface.StyleEditor;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
	/// <summary>
	/// This is used to present the style editor for attributes containing inline styles.
	/// </summary>
	/// <remarks>
	/// This editor calls the integrated style editor control, placed on a advanced form surface.
	/// This adds a textbox for style preview to the control.
	/// For more information about this control see
	/// <see cref="StyleEditorForm">StyleEditorForm</see> class.
	/// </remarks>
	public class UITypeEditorStyleStyle : UITypeEditor
	{
		private IWindowsFormsEditorService service;
        private static StyleEditorForm sf = null;

        /// <summary>
        /// Callback to a customized string editor.
        /// </summary>
        /// <remarks>
        /// This callback must be <c>null</c> to call standard editor (enhanced text field). If it points
        /// to a method this method should invoke a dialog which is displayed instead of the
        /// integrated TextBox.
        /// <para>
        /// A customized editor can use the 
        /// <see cref="StyleControl">StyleControl</see>
        /// to provide a graphical interface to the user.
        /// </para>
        /// </remarks>
        public static UIStyleEditor UIEditorCallback = null;

        /// <summary>
        /// This method calls the editor dialog used in the property grid.
        /// </summary>
        /// <remarks>
        /// This method can invoke an internal dialog or an external one. The external one is called whenever a 
        /// callback is set. To set an external dialog simple set the 
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorStyleStyle.UIEditorCallback">UIEditorCallback</see> delegate of type
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UIStyleEditor">UIStyleEditor</see>.
        /// </remarks>
        /// <param name="context">The component which contains the property beeing edited.</param>
        /// <param name="provider">The service provider which invokes the editor.</param>
        /// <param name="val">The value currently beeing set in the property.</param>
        /// <returns>The changed value the user has created using the invoked dialog.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object val)
		{
			string value = (val == null) ? String.Empty : val.ToString();
            if (context != null && context.Instance != null && provider != null)
			{
				service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
				if (service != null)
				{

                    bool StartOver = true;
                    // look for a delegate attached by host app
                    if (UIEditorCallback != null)
                    {           
                        StartOver = false;
                        string oldVal = value;
                        DialogResult result = UIEditorCallback(context, ref value);
                        if (result != DialogResult.OK)
                        {
                            value = oldVal;
                            if (result == DialogResult.Ignore)
                            {
                                StartOver = true;
                            }
                        }
                    } 
                    if (StartOver)
                    {
                        Cursor oldCursor = Cursor.Current;
                        Cursor.Current = Cursors.AppStarting;
                        if (sf == null || sf.IsDisposed)
                        {
                            sf = new StyleEditorForm(true);
                            sf.TabAppearance = TabAppearance.Normal;
                            sf.ButtonAppearance = FlatStyle.System;
                        } 
                        else 
                        {
                            sf.ResetStyleControl();
                        }
                        sf.ShowInTaskbar = false;
                        sf.StartPosition = FormStartPosition.CenterParent;
                        sf.StyleString = (value == null) ? String.Empty : value.ToString();
                        Cursor.Current = oldCursor;
                        sf.ShowDialog();
                        value = sf.StyleString;
                    }
				}
			}
			return value;
		}

        /// <summary>
        /// Defines the edit style for that control.
        /// </summary>
        /// <remarks>
        /// This component defines the dialog as modal. This value cannot be overwritten by a external
        /// delegate which produces a customized dialog.
        /// </remarks>
        /// <param name="context">The calling component.</param>
        /// <returns>The style beeing set; always <see cref="System.Drawing.Design.UITypeEditorEditStyle.Modal">UITypeEditorEditStyle.Modal</see>.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context != null && context.Instance != null)
			{
				return UITypeEditorEditStyle.Modal;
			}
			return base.GetEditStyle(context);
		}
	}
}
