using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	public class TextStyleUIEditor : UITypeEditor
	{
		private IWindowsFormsEditorService edSvc = null;

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (context != null && context.Instance != null && provider != null)
			{
				edSvc = (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));


				if (edSvc != null)
				{
					TextStyle style = (TextStyle) value;
					using (TextStyleDesignerDialog tsd = new TextStyleDesignerDialog(style))
					{
						context.OnComponentChanging();
						if (edSvc.ShowDialog(tsd) == DialogResult.OK)
						{
							this.ValueChanged(this, EventArgs.Empty);
							context.OnComponentChanged();
							return style;
						}

					}
				}
			}

			return value;
		}


		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}


		private void ValueChanged(object sender, EventArgs e)
		{
			if (edSvc != null)
			{
			}
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			TextStyle ts = (TextStyle) e.Value;
			using (SolidBrush b = new SolidBrush(ts.BackColor))
			{
				e.Graphics.FillRectangle(b, e.Bounds);
			}

			FontStyle fs = FontStyle.Regular;
			if (ts.Bold)
				fs |= FontStyle.Bold;
			if (ts.Italic)
				fs |= FontStyle.Italic;
			if (ts.Underline)
				fs |= FontStyle.Underline;

			Font f = new Font("arial", 8f, fs);


			using (SolidBrush b = new SolidBrush(ts.ForeColor))
			{
				e.Graphics.DrawString("abc", f, b, e.Bounds);
			}

			f.Dispose();


		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}