using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.CodeEditor.Library.Win32;

namespace GuruComponents.CodeEditor.Library.Drawing.GDI
{
    public class GDIBitmap:GDIObject
    {
        public IntPtr _hBmp;

        public GDIBitmap(IntPtr hBmp)
        {
            _hBmp = hBmp;
        }
        public GDIBitmap(string filename)
        {
            FreeImage img = new FreeImage(filename);
            IntPtr deskDC = NativeMethods.GetDC(new IntPtr(0));
            IntPtr destDC = NativeMethods.CreateCompatibleDC(deskDC);
            IntPtr oldBmp = NativeMethods.SelectObject(destDC, _hBmp);
            _hBmp = NativeMethods.CreateCompatibleBitmap(destDC, img.Width, img.Height);
            img.PaintToDevice(destDC, 0, 0, img.Width, img.Height, 0, 0, 0, img.Height, 0);
            NativeMethods.SelectObject(destDC, oldBmp);

            NativeMethods.DeleteDC(deskDC);
            NativeMethods.DeleteDC(destDC);
            img.DisposeAndSetHandle(0);

        }

        public IntPtr Handle
        {
            get
            {
                return _hBmp;
            }
        }

        protected override void Destroy()
        {
            if (_hBmp != (IntPtr)0)
            {
                NativeMethods.DeleteObject(_hBmp);
            }

            base.Destroy();
            _hBmp = (IntPtr)0;
        }
        protected override void Create()
        {
            base.Create();
        }
    }


}
