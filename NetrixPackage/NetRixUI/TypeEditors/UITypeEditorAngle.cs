using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
	/// <summary>
	/// Editor to create and edit unit values.
	/// </summary>
	/// <remarks>
	/// A unit value is either a percentage or pixel value in HTML. The editor can build the 
	/// right values using the mouse instead of keyboard.
	/// </remarks>
	public class UITypeEditorAngle : UITypeEditor
	{
		private IWindowsFormsEditorService service;
        private AngleEditor AngleControl = new AngleEditor();

        /// <summary>
        /// Callback to a customized unit editor.
        /// </summary>
        /// <remarks>
        /// This callback must be <c>null</c> to call standard editor (integrated unit editor). If it points
        /// to a method this method should invoke a dialog which is displayed instead of the
        /// integrated editor.
        /// </remarks>
        public static UIAngleEditor UIEditorCallback = null;

        /// <summary>
        /// This method calls the editor dialog used in the property grid.
        /// </summary>
        /// <remarks>
        /// This method can invoke an internal dialog or an external one. The external one is called whenever a 
        /// callback is set. To set an external dialog simple set the 
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorAngle.UIEditorCallback">UIEditorCallback</see> delegate of type
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UIAngleEditor">UIAngleEditor</see>.
        /// </remarks>
        /// <param name="context">The component which contains the property beeing edited.</param>
        /// <param name="provider">The service provider which invokes the editor.</param>
        /// <param name="val">The value currently beeing set in the property.</param>
        /// <returns>The changed value the user has created using the invoked dialog.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object val)
		{
            decimal value = Convert.ToDecimal(val);
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
                        decimal oldVal = value;
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
                        AngleControl.BackColor = SystemColors.Control;
                        AngleControl.BorderStyle = BorderStyle.FixedSingle;
                        AngleControl.Angle = value;
                        AngleControl.Size = new Size(100, 100);
                        service.DropDownControl(AngleControl);
                        value = (decimal)AngleControl.Angle;
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
