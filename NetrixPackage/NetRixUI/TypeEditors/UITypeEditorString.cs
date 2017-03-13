using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
	/// <summary>
	/// String editor.
	/// </summary>
	/// <remarks>
	/// Uses the standard string editor if no external callback function is assigned.
	/// </remarks>
	public class UITypeEditorString : UITypeEditor
	{
		private IWindowsFormsEditorService service;

        /// <summary>
        /// Callback to a customized string editor.
        /// </summary>
        /// <remarks>
        /// This callback must be <c>null</c> to call standard editor (enhanced text field). If it points
        /// to a method this method should invoke a dialog which is displayed instead of the
        /// integrated TextBox.
        /// </remarks>
        public static UIStringEditor UIEditorCallback = null;

        /// <summary>
        /// This method calls the editor dialog used in the property grid.
        /// </summary>
        /// <remarks>
        /// This method can invoke an internal dialog or an external one. The external one is called whenever a 
        /// callback is set. To set an external dialog simple set the 
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString.UIEditorCallback">UIEditorCallback</see> delegate of type
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UIStringEditor">UIStringEditor</see>.
        /// </remarks>
        /// <param name="context">The component which contains the property beeing edited.</param>
        /// <param name="provider">The service provider which invokes the editor.</param>
        /// <param name="val">The value currently beeing set in the property.</param>
        /// <returns>The changed value the user has created using the invoked dialog.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object val)
		{
            string value = val.ToString();
			if (context != null && context.Instance != null && provider != null)
			{
				service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
				
				if (service != null)
				{
                    // look for a delegate attached by host app
                    if (UIEditorCallback != null)
                    {           
                        string oldVal = value;
                        DialogResult result = UIEditorCallback(context, ref value);
                        if (result != DialogResult.OK)
                        {
                            value = oldVal;
                        }
                    } 
				}				
			}
			return value;
		}

        /// <summary>
        /// Defines the edit style for that control.
        /// </summary>
        /// <remarks>
        /// This component defines the dialog as dropdown. This value cannot be overwritten by a external
        /// delegate which produces a customized dialog.
        /// </remarks>
        /// <param name="context">The calling component.</param>
        /// <returns>The style beeing set; always <see cref="System.Drawing.Design.UITypeEditorEditStyle.DropDown">UITypeEditorEditStyle.DropDwon</see>.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context != null && context.Instance != null)
			{
                if (UIEditorCallback == null)
                {
                    return UITypeEditorEditStyle.None;
                } 
                else 
                {
                    return UITypeEditorEditStyle.DropDown;
                }
			}
			return base.GetEditStyle(context); 
		}
	}
}
