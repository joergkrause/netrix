using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using GuruComponents.Netrix.UserInterface.FontPicker;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
	/// <summary>
	/// TypeEditor for selecting multiple fonts.
	/// </summary>
	/// <remarks>
	/// Returns list of fonts as comma separated list. Uses the 
    /// <see cref="GuruComponents.Netrix.UserInterface.FontPicker.FontPickerUserControl">FontPickerUserControl</see>
	/// internally.
	/// </remarks>
	public class UITypeEditorFont : UITypeEditor
	{
		private IWindowsFormsEditorService service;
        private static FontPickerUserControl ff = new FontPickerUserControl(true);
        
        /// <summary>
        /// Callback to a customized font editor.
        /// </summary>
        /// <remarks>
        /// This callback must be <c>null</c> to call the internal font editor. If it points
        /// to a method this method should invoke a dialog which is displayed instead of the
        /// integrated color picker control.
        /// <seealso cref="GuruComponents.Netrix.UserInterface.FontPicker.FontPickerUserControl">FontPickerUserControl</seealso>
        /// </remarks>
        public static UIFontEditor UIEditorCallback = null;

        /// <summary>
        /// This method calls the editor dialog used in the property grid.
        /// </summary>
        /// <remarks>
        /// This method can invoke an internal dialog or an external one. The external one is called whenever a 
        /// callback is set. To set an external dialog simple set the 
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorFont.UIEditorCallback">UIEditorCallback</see> delegate of type
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UIFontEditor">UIFontEditor</see>.
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
				if (service != null && ff != null)
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
                        ff.ResetUI();
                        ff.PopulateStringList = val.ToString();
                        service.DropDownControl(ff);
                        val = ff.PopulateStringList;
                    }
				}
			}
			return val;
		}

        /// <summary>
        /// Defines the edit style for that control.
        /// </summary>
        /// <remarks>
        /// This component defines the dialog as dropdown. This value cannot be overwritten by a external
        /// delegate which produces a customized dialog.
        /// </remarks>
        /// <param name="context">The calling component.</param>
        /// <returns>The style beeing set; always <see cref="System.Drawing.Design.UITypeEditorEditStyle.DropDown">UITypeEditorEditStyle.DropDown</see>.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context != null && context.Instance != null)
			{
				return UITypeEditorEditStyle.DropDown;
			}
			return base.GetEditStyle(context);
		}

	}
}
