
using System;
using System.Drawing;
using GuruComponents.CodeEditor.Library.Win32;

namespace GuruComponents.CodeEditor.Library.Drawing.GDI
{
	public class GDIPen : GDIObject
	{
		public IntPtr hPen;

		public GDIPen(Color color, int width)
		{
			hPen = NativeMethods.CreatePen(0, width, NativeMethods.ColorToInt(color));
			Create();
		}

		protected override void Destroy()
		{
			if (hPen != (IntPtr) 0)
				NativeMethods.DeleteObject(hPen);
			base.Destroy();
			hPen = (IntPtr) 0;
		}

		protected override void Create()
		{
			base.Create();
		}
	}
}