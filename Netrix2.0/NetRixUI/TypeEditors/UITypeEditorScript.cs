using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
    /// <summary>
    /// This editor let the user editor script areas outside the editor.
    /// </summary>
    /// <remarks>
    /// This is only a rudimentary editor, based on a simple multiline textbox. An external editor
    /// should provide more functionality like line numbering and syntax coloring.
    /// </remarks>
    public class UITypeEditorScript : UITypeEditor
    {
        private IWindowsFormsEditorService service;

        /// <summary>
        /// Callback to a customized script editor.
        /// </summary>
        /// <remarks>
        /// This callback must be <c>null</c> to call standard editor (enhanced text field). If it points
        /// to a method this method should invoke a dialog which is displayed instead of the
        /// integrated TextBox.
        /// </remarks>
        public static UIScriptEditor UIEditorCallback = null;

        /// <summary>
        /// This method calls the editor dialog used in the property grid.
        /// </summary>
        /// <remarks>
        /// This method can invoke an internal dialog or an external one. The external one is called whenever a 
        /// callback is set. To set an external dialog simple set the 
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorScript.UIEditorCallback">UIEditorCallback</see> delegate of type
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UIScriptEditor">UIScriptEditor</see>.
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
                        TextBox tb = new TextBox();
                        tb.Multiline = true;
                        tb.ScrollBars = ScrollBars.Both;
                        tb.ForeColor = Color.Blue;
                        tb.BackColor = SystemColors.Info;
                        tb.Size = new Size(300, 120);
                        tb.KeyDown += new KeyEventHandler(tb_KeyDown);
                        tb.Text = value;
                        tb.SelectionStart = 0;
                        tb.SelectionLength = 0;
                        tb.AcceptsReturn = true;
                        tb.AcceptsTab = true;
                        service.DropDownControl(tb);
                        value = tb.Text;
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

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (service != null)
                {
                    service.CloseDropDown();
                }
            }
        }
    }
}
