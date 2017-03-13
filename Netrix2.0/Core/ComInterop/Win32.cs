using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace GuruComponents.Netrix.ComInterop
{
    /// <summary>
    /// This class provides support for Win32 APIs.
    /// </summary>
    public class Win32
    {
        #region  Ole32.dll imports

        /// <exclude/>
        [DllImport("ole32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool OleDraw([MarshalAs(UnmanagedType.IUnknown)] object pUnkn, int dwAspect, IntPtr hDC, ref Rectangle rect);

        /// <exclude/>
        [DllImport("ole32.dll", PreserveSig = false)]
        public static extern void CreateStreamOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease, [Out] out IStream pStream);

        /// <exclude/>
        [DllImport("ole32.dll", PreserveSig = false)]
        public static extern void GetHGlobalFromStream(IStream pStream, [Out] out IntPtr pHGlobal);

        /// <exclude/>
        [DllImport("ole32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int CreateBindCtx(int dwReserved, [Out] out Interop.IBindCtx ppbc);

        /// <exclude/>
        [DllImport("ole32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int OleRun(
            [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown
            );

        /// <exclude/>
        [DllImport("ole32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int OleLockRunning(
            [In, MarshalAs(UnmanagedType.IUnknown)] object pUnknown,
            [In, MarshalAs(UnmanagedType.Bool)] bool flock,
            [In, MarshalAs(UnmanagedType.Bool)] bool fLastUnlockCloses
            );

        /// <exclude/>
        [DllImport("ole32.dll", EntryPoint = "MkParseDisplayName", CharSet = CharSet.Auto)]
        public static extern uint MkParseDisplayName(
            IBindCtx bc,      // bind context object
            string szUserName,    // display name
            out uint chEaten,      // number of characters
            out Interop.IMoniker mk // IMoniker interface
            );

        /// <exclude/>
        [DllImport("oleaut32.dll", PreserveSig = false)]
        public static extern void VariantClear(HandleRef pObject);

        #endregion

        #region URLMON.dll imports

        /// <exclude/>
        [DllImport("urlmon.dll", ExactSpelling = true, CharSet = CharSet.Ansi)]
        public static extern int URLDownloadToFileA(
            [In]Interop.IUnknown pUnk,
            string szURL,
            string szFileName,
            Int32 dwReserved,
            [In] Interop.IBindStatusCallback pbsc);

        /// <exclude/>
        [DllImport("urlmon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int URLDownloadToCacheFile(
            [In]Interop.IUnknown pUnk,
            string szURL,
            string szFileName,
            Int32 dwBufLength,
            Int32 dwReserved,
            [In] Interop.IBindStatusCallback pbsc);

        /// <exclude/>
        [DllImport("urlmon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int CreateURLMoniker(Interop.IMoniker pmkContext, string szURL, [Out] out Interop.IMoniker ppmk);

        /// <exclude/>
        [DllImport("urlmon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int RegisterBindStatusCallback(IBindCtx pbc, [In] Interop.IBindStatusCallback pbsc, [Out] out Interop.IBindStatusCallback ppbsc, int dwReserved);

        /// <exclude/>
        [DllImport("urlmon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int CoInternetCombineUrl(
            [In, MarshalAs(UnmanagedType.LPWStr)]
            string pwzBaseUrl,
            [In, MarshalAs(UnmanagedType.LPWStr)]
            string pwzRelativeUrl,
            [In]
            int dwCombineFlags,
            [Out]
            IntPtr pszResult,
            [In]
            int cchResult,
            [Out]
            out int pcchResult,
            [In]
            int dwReserved
            );

        /// <exclude/>
        [DllImport("urlmon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int CoInternetGetSession(int dwSessionMode, out Interop.IInternetSession session, int dwReserved);

        /// <exclude/>
        [DllImport("urlmon.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int FindMimeFromData(IntPtr bindContext,
            string url,
            [MarshalAs(UnmanagedType.LPArray)] byte[] buffer,
            int bufferSize,
            string proposedMimeType,
            int flags,
            ref string mimeType,
            int reserved);

        #endregion

        #region gdi32.dll imports
        /// <exclude/>
        [DllImport("gdi32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int SetPixel(IntPtr hDC, int x, int y, int crColor);

        /// <exclude/>
        [DllImport("gdi32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool LineTo(IntPtr hdc, int x, int y);

        /// <exclude/>
        [DllImport("gdi32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool MoveToEx(
            IntPtr hdc,					// handle to device context
            int x,						// x-coordinate of new current position
            int y,						// y-coordinate of new current position
            [In, Out] IntPtr lpPoint   // old current position
            );

        /// <exclude/>
        [DllImport("gdi32")]
        public static extern IntPtr CreateSolidBrush(ulong crColor);

        /// <exclude/>
        [DllImport("gdi32")]
        public static extern IntPtr CreatePen(int nPenStyle, int nWidth, int crColor);

        /// <exclude/>
        [DllImport("gdi32")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        /// <exclude/>
        [DllImport("gdi32")]
        public static extern int DeleteObject(IntPtr hObject);

        /// <exclude/>
        [DllImport("gdi32")]
        public static extern int DeleteDC(IntPtr hDC);

        /// <exclude/>
        [DllImport("gdi32.dll")]
        public static extern int BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);


        #endregion

        #region User32.dll imports

        /// <exclude/>
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool DestroyCursor(IntPtr hCursor);

        /// <exclude/>
        [DllImport("user32.dll")]
        public static extern bool HideCaret(IntPtr hWnd);

        /// <exclude/>
        [DllImport("user32.dll")]
        public static extern bool ShowCaret(IntPtr hWnd);


        /// <exclude/>
        [DllImport("user32.dll", EntryPoint = "LoadString")]
        public static extern int LoadStringA(IntPtr hInstance, int wID, StringBuilder lpBuffer, int nBufferMax);

        /// <exclude/>
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool GetClientRect(IntPtr hWnd, [In, Out] Interop.RECT rect);

        /// <exclude/>
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        /// <exclude/>
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool GetCaretPos([Out] out Interop.POINT pPoint);

        /// <exclude/>
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool SetCaretPos([In] int x, [In] int y);

        /// <exclude/>
        [DllImport("user32.Dll")]
        public static extern IntPtr GetFocus();

        /// <exclude/>
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        /// <exclude/>
        [DllImport("user32.dll")]
        public static extern int SendMessage(
            IntPtr hWnd,    // handle to destination window
            int Msg,        // message
            IntPtr wParam,  // first message parameter
            IntPtr lParam   // second message parameter
            );

        /// <exclude/>
        [DllImport("user32.dll")]
        public static extern int MapVirtualKeyEx(int uCode, int uMapType, int dwhkl);

        /// <exclude/>
        [DllImport("user32.dll")]
        public static extern int ToAsciiEx(
            int uVirtKey,
            int uScanCode,
            byte[] lpKeyState,
            out int lpChar,
            int uFlags,
            int dwhkl);

        /// <exclude/>
        [DllImport("user32.dll")]
        public static extern int VkKeyScanEx(byte ch, int dwhkl);

        /// <exclude/>
        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        /// <exclude/>
        [DllImport("user32.dll")]
        public static extern int GetKeyboardLayout(int dwLayout);

        /// <exclude />
        public static char GetAsciiCharacter(int iScancode)
        {
            int lpChar;
            int layout = GetKeyboardLayout(0);
            int iKeyCode = MapVirtualKeyEx(iScancode, 1, layout);
            byte[] bCharData = new byte[256];
            byte[] pByte = bCharData;
            GetKeyboardState(pByte);
            ToAsciiEx(iScancode, iKeyCode, pByte, out lpChar, 0, layout);
            return (char)lpChar;
        }

        #endregion

        #region kern32.dll Imports
        /// <exclude/>
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GlobalLock(IntPtr handle);

        /// <exclude/>
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool GlobalUnlock(IntPtr handle);

        /// <exclude/>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(
          string lpFileName,
          IntPtr hFile,
          uint dwFlags);

        /// <exclude/>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        /// <exclude/>
        [DllImport("kernel32.dll", EntryPoint = "EnumResourceNamesW",
          CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool EnumResourceNamesWithName(
          IntPtr hModule,
          string lpszType,
          EnumResNameDelegate lpEnumFunc,
          IntPtr lParam);

        /// <exclude/>
        [DllImport("kernel32.dll", EntryPoint = "EnumResourceNamesW",
          CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool EnumResourceNamesWithID(
          IntPtr hModule,
          uint lpszType,
          EnumResNameDelegate lpEnumFunc,
          IntPtr lParam);

        /// <exclude/>
        [DllImport("kernel32.dll", EntryPoint = "GetThreadLocale", CharSet = CharSet.Auto)]
        public static extern int GetThreadLCID();

        /// <exclude/>
        public delegate bool EnumResNameDelegate(
          IntPtr hModule,
          IntPtr lpszType,
          IntPtr lpszName,
          IntPtr lParam);

        #endregion

        # region wininet.dll imports

        /// <exclude/>
        [DllImport("wininet.dll")]
        public extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        # endregion

    }
}

