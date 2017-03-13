using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using GuruComponents.Netrix.UserInterface.ColorPicker;

namespace GuruComponents.Netrix.UserInterface.TypeEditors
{
    /// <summary>
    /// Color TypeEditor for PropertyGrid, which used a direct inctance of the ColorPickerForm.
    /// </summary>
    /// <remarks>
    /// This editor can be replaced by a customized editor using the static callback function.
    /// See <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor.UIEditorCallback">UIEditorCallback</see> for
    /// more information.
    /// </remarks>
    public class UITypeEditorColor : UITypeEditor
    {
        private IWindowsFormsEditorService service;
        private Color colorVal;
        private ColorPanelUserControl ColorControl;

        /// <summary>
        /// Callback to a customized color editor.
        /// </summary>
        /// <remarks>
        /// This callback must be <c>null</c> to call the internal color editor. If it points
        /// to a method this method should invoke a dialog which is displayed instead of the
        /// integrated color picker control.
        /// </remarks>
        public static UIColorEditor UIEditorCallback = null;

        /// <summary>
        /// The contructor of the color editor dialog.
        /// </summary>
        /// <remarks>
        /// This constructor is used to set various values on first call. This assignes the 
        /// event handler beeing used and the custom colors set in the custom color tab.
        /// Using this constructor to set the values results in better perfomance an call but 
        /// means that the colors cannot be changed if the grid is open. You must close the grid to
        /// reset the colors.
        /// </remarks>
        public UITypeEditorColor()
        {
            ColorControl = new ColorPanelUserControl();
            ColorControl.CurrentColor = colorVal;
            ColorControl.BackColor = SystemColors.Control;
            ColorControl.CustomColors = ResourceManager.CustomColors;
            ColorControl.ColorChanged += new ColorChangedEventHandler(this.ValueChanged);
            ColorControl.ColorCancel += new ColorCancelEventHandler(ColorControl_ColorCancel);
        }

        /// <summary>
        /// This editor paints its own thumbnail.
        /// </summary>
        /// <remarks>
        /// This method supports the NetRix infrastructure and cannot be called directly from user code.
        /// </remarks>
        /// <param name="context">Context from which the editor is called.</param>
        /// <returns>Always <c>true</c>.</returns>
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            ISelectionService ss = context.GetService(typeof(ISelectionService)) as ISelectionService;
            if (ss != null && ss.GetSelectedComponents().Count > 1)
                return false;
            else
                return true;
        }

        private static Stream s1 = typeof(UITypeEditorColor).Assembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.NoColor.ico");
        private static Stream s2 = typeof(UITypeEditorColor).Assembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.WrongColor.icoo");

        /// <summary>
        /// This method generates the colored thumbnail.
        /// </summary>
        /// <param name="e"></param>
        public override void PaintValue(PaintValueEventArgs e)
        {
            Rectangle rect = e.Bounds;
            try
            {
                if (e.Value == null)
                {
                    return;
                }
                Color color;
                color = (Color)e.Value;
                if (color.Equals(Color.Empty))
                {
                    s1.Seek(0, SeekOrigin.Begin);
                    e.Graphics.DrawIcon(new Icon(s1), 0, 0);
                }
                else
                {
                    SolidBrush brush;
                    brush = new SolidBrush(color);
                    e.Graphics.FillRectangle(brush, rect);
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 1.0F), rect);
                }
            }
            catch
            {
                s2.Seek(0, SeekOrigin.Begin);
                e.Graphics.DrawIcon(new Icon(s2), 0, 0);
            }
        }

        /// <summary>
        /// This method calls the editor dialog used in the property grid.
        /// </summary>
        /// <remarks>
        /// This method can invoke an internal dialog or an external one. The external one is called whenever a 
        /// callback is set. To set an external dialog simple set the 
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor.UIEditorCallback">UIEditorCallback</see> delegate of type
        /// <see cref="GuruComponents.Netrix.UserInterface.TypeEditors.UIColorEditor">UIColorEditor</see>.
        /// </remarks>
        /// <param name="context">The component which contains the property beeing edited.</param>
        /// <param name="provider">The service provider which invokes the editor.</param>
        /// <param name="val">The value currently beeing set in the property.</param>
        /// <returns>The changed value the user has created using the invoked dialog.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object val)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                colorVal = (val is Color) ? (Color)val : Color.Empty;
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (service != null)
                {
                    bool StartOver = true;
                    // look for a delegate attached by host app
                    if (UIEditorCallback != null)
                    {
                        StartOver = false;
                        Color oldVal = colorVal;
                        DialogResult result = UIEditorCallback(context, ref colorVal);
                        if (result != DialogResult.OK)
                        {
                            colorVal = oldVal;
                            if (result == DialogResult.Ignore)
                            {
                                StartOver = true;
                            }
                        }
                    }
                    if (StartOver)
                    {
                        ColorControl.ResetControl(ResourceManager.CustomColors);
                        ColorControl.ButtonSystemVisible = ResourceManager.ColorPickerButtonSystemVisible;
                        service.DropDownControl(ColorControl);
                    }
                }
            }
            return colorVal;
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

        private void ValueChanged(object sender, ColorChangedEventArgs e)
        {
            if (service != null)
            {
                colorVal = ColorControl.CurrentColor;
                service.CloseDropDown();
            }
        }

        private void ColorControl_ColorCancel(object sender, ColorChangedEventArgs e)
        {
            if (service != null)
            {
                colorVal = Color.Empty;
                service.CloseDropDown();
            }
        }
    }
}
