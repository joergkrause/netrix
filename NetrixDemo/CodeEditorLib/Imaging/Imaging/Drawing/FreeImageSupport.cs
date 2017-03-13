using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace Comzept.Library.Drawing
{

	[StructLayout(LayoutKind.Sequential)]
	internal class RGBQUAD
	{
		public byte rgbBlue;
		public byte rgbGreen;
		public byte rgbRed;
		public byte rgbReserved;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal class BITMAPINFOHEADER
	{
		public uint size;
		public int width;
		public int height;
		public ushort biPlanes;
		public ushort biBitCount;
		public uint biCompression;
		public uint biSizeImage;
		public int biXPelsPerMeter;
		public int biYPelsPerMeter;
		public uint biClrUsed;
		public uint biClrImportant;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal class BITMAPINFO
	{
		public BITMAPINFOHEADER bmiHeader;
		public RGBQUAD bmiColors;
	}

}