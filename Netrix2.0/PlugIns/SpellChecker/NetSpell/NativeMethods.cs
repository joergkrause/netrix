using System;
using System.Runtime.InteropServices;

namespace GuruComponents.Netrix.SpellChecker.NetSpell.Controls
{
	/// <summary>
	/// Summary description for NativeMethods.
	/// </summary>
	internal sealed class NativeMethods
	{

		private NativeMethods()
		{
		}

        [StructLayout(LayoutKind.Sequential)]
        internal struct CHARFORMAT2
        {
            internal uint cbSize;
            internal int dwMask;
            internal int dwEffects;
            internal long yHeight;
            internal long yOffset;
            internal int crTextColor;
            internal byte bCharSet;
            internal byte bPitchAndFamily;
            internal string szFaceName;
            internal int wWeight;
            internal short sSpacing;
            internal int crBackColor;
            internal int lcid;
            internal int dwReserved;
            internal short sStyle;
            internal int wKerning;
            internal byte bUnderlineType;
            internal byte bAnimation;
            internal byte bRevAuthor;
            internal byte bReserved1;
        }

		// Windows Messages 
		internal const int WM_SETREDRAW				= 0x000B; 

		internal const int WM_PAINT					= 0x000F;
		internal const int WM_ERASEBKGND			= 0x0014;
		
		internal const int WM_NOTIFY				= 0x004E;
		
		internal const int WM_HSCROLL				= 0x0114;
		internal const int WM_VSCROLL				= 0x0115;

		internal const int WM_CAPTURECHANGED		= 0x0215;

		internal const int WM_USER					= 0x0400;
		
		// Win API declaration
		[DllImport("user32.dll", EntryPoint="SendMessage", CharSet=CharSet.Auto)]
		internal static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam); 
 

	}
}
