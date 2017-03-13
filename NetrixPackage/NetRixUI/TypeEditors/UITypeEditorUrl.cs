using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
	/// <summary>
	/// UITypeEditorUrl presents a type editor for URL selection. 
	/// </summary>
	/// <remarks>
	/// This editor calls the <see cref="System.Windows.Forms.OpenFileDialog">OpenFileDialog</see> from .NET
	/// framework as a modal dialog.
	/// </remarks>
	public class UITypeEditorUrl : UITypeEditor
	{
		private IWindowsFormsEditorService service;
		private OpenFileDialog openFileDialog = new OpenFileDialog();

        /// <summary>
        /// Callback to a customized url editor.
        /// </summary>
        /// <remarks>
        /// This callback must be <c>null</c> to call standard editor (<see cref="System.Windows.Forms.OpenFileDialog">OpenFileDialog</see>). If it points
        /// to a method this method should invoke a dialog which is displayed instead of the
        /// integrated <see cref="System.Windows.Forms.OpenFileDialog">OpenFileDialog</see>.
        /// </remarks>
        public static UIUrlEditor UIEditorCallback = null;

        /// <summary>
        /// This method calls the editor dialog used in the property grid.
        /// </summary>
        /// <remarks>
        /// This method can invoke an internal dialog or an external one. The external one is called whenever a 
        /// callback is set. To set an external dialog simple set the 
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUrl.UIEditorCallback">UIEditorCallback</see> delegate of type
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UIUrlEditor">UIUrlEditor</see>.
        /// </remarks>
        /// <param name="context">The component which contains the property beeing edited.</param>
        /// <param name="provider">The service provider which invokes the editor.</param>
        /// <param name="val">The value currently beeing set in the property.</param>
        /// <returns>The changed value the user has created using the invoked dialog.</returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object val)
		{
			if (context != null && context.Instance != null && provider != null)
			{                
				service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
				string value = val.ToString();
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
                        // get the string in the property box
                        openFileDialog.Reset();
                        switch (context.Instance.GetType().ToString())
                        {
                            case "GuruComponents.Netrix.WebEditing.Elements.FrameElement":
                            case "GuruComponents.Netrix.WebEditing.Elements.FormElement":
                            case "GuruComponents.Netrix.WebEditing.Elements.IFrameElement":
                            case "GuruComponents.Netrix.WebEditing.Elements.AnchorElement":
                                openFileDialog.Filter = "HTML|*.htm;*.html|Active Scripting|*.php;*.asp;*.aspx;*.php4;;*.cgi;*.pl; *.phtml|All Files (*.*)|*.*";
                                break;
                            default:
                                openFileDialog.Filter = "Web Image files (*.PNG;*.JPG;*.GIF)|*.jpg;*.jpeg;*.gif;*.png|All Files (*.*)|*.*";
                                break;
                        }					
                        openFileDialog.Multiselect = false;
                        openFileDialog.CheckFileExists = false;
                        openFileDialog.FilterIndex = 0;
                        openFileDialog.RestoreDirectory = true;
                        openFileDialog.Title = "File Selection";
                        if (openFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            value = openFileDialog.FileName;
                        }
                    }
				}
				val = value;
			}
			return val;
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
