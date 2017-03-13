
using System;
using Comzept.Library.Win32;

namespace Comzept.Library.Drawing.GDI
{
	public class GDIFont : GDIObject
	{
		public bool Bold;
		public bool Italic;
		public bool Underline;
		public bool Strikethrough;
		public float Size;
		public string FontName;
		public byte Charset;
		public IntPtr hFont;


		public GDIFont()
		{
			Create();
		}

		public GDIFont(string fontname, float size)
		{
			Init(fontname, size, false, false, false, false);
			Create();
		}

		public GDIFont(string fontname, float size, bool bold, bool italic, bool underline, bool strikethrough)
		{
			Init(fontname, size, bold, italic, underline, strikethrough);
			Create();
		}

		protected void Init(string fontname, float size, bool bold, bool italic, bool underline, bool strikethrough)
		{
			FontName = fontname;
			Size = size;
			Bold = bold;
			Italic = italic;
			Underline = underline;
			Strikethrough = strikethrough;

			LogFont tFont = new LogFont();
			tFont.lfItalic = (byte) (this.Italic ? 1 : 0);
			tFont.lfStrikeOut = (byte) (this.Strikethrough ? 1 : 0);
			tFont.lfUnderline = (byte) (this.Underline ? 1 : 0);
			tFont.lfWeight = this.Bold ? 700 : 400;
			tFont.lfWidth = 0;
			tFont.lfHeight = (int) (-this.Size*1.3333333333333);
			tFont.lfCharSet = 1;

			tFont.lfFaceName = this.FontName;


			hFont = NativeMethods.CreateFontIndirect(tFont);
		}

		~GDIFont()
		{
			Destroy();
		}

		protected override void Destroy()
		{
			if (hFont != (IntPtr) 0)
				NativeMethods.DeleteObject(hFont);
			base.Destroy();
			hFont = (IntPtr) 0;
		}

		protected override void Create()
		{
			base.Create();
		}

	}
}