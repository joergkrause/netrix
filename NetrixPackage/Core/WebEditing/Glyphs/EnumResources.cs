  using System;
  using System.Runtime.InteropServices;
using System.Diagnostics;
  using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Glyphs
{
    class EnumResources
    {
        private const uint RT_CURSOR = 0x00000001;
        private const uint RT_BITMAP = 0x00000002;
        private const uint RT_ICON = 0x00000003;
        private const uint RT_MENU = 0x00000004;
        private const uint RT_DIALOG = 0x00000005;
        private const uint RT_STRING = 0x00000006;
        private const uint RT_FONTDIR = 0x00000007;
        private const uint RT_FONT = 0x00000008;
        private const uint RT_ACCELERATOR = 0x00000009;
        private const uint RT_RCDATA = 0x00000010;
        private const uint RT_MESSAGETABLE = 0x00000011;

        private const uint LOAD_LIBRARY_AS_DATAFILE = 0x00000002;

        private static bool IS_INTRESOURCE(IntPtr value)
        {
            if (((uint)value) > ushort.MaxValue)
                return false;
            return true;
        }
        private static uint GET_RESOURCE_ID(IntPtr value)
        {
            if (IS_INTRESOURCE(value))
                return (uint)value;
            throw new NotSupportedException("value is not an ID!");
        }
        private static string GET_RESOURCE_NAME(IntPtr value)
        {
            if (IS_INTRESOURCE(value))
                return value.ToString();
            return Marshal.PtrToStringUni(value);
        }

        [STAThread]
        public static void Load(string path)
        {
            IntPtr hMod = Win32.LoadLibraryEx(path, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);
            if (Win32.EnumResourceNamesWithID(hMod, RT_BITMAP, new Win32.EnumResNameDelegate(EnumRes), IntPtr.Zero) == false)
            {
                Debug.WriteLine(String.Format("GlyphLib Access Failed by Reason: {0}", Marshal.GetLastWin32Error()));
            }
            Win32.FreeLibrary(hMod);
        }

        static bool EnumRes(IntPtr hModule,
          IntPtr lpszType,
          IntPtr lpszName,
          IntPtr lParam)
        {
            Debug.WriteLine("Type: {0}", GET_RESOURCE_NAME(lpszType));
            Debug.WriteLine("Name: {0}", GET_RESOURCE_NAME(lpszName));
            return true;
        }
    }
}