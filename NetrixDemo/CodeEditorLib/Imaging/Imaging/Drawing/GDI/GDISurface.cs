
using System;
using System.Drawing;
using System.Windows.Forms;
using Comzept.Library.Win32;

namespace Comzept.Library.Drawing.GDI
{
	public class GDISurface : GDIObject
	{
		protected IntPtr mhDC;
		protected IntPtr mhBMP;
		protected int mWidth;
		protected int mHeight;
		protected int mTabSize = 4;
		protected IntPtr _OldFont = IntPtr.Zero;
		protected IntPtr _OldPen = IntPtr.Zero;
		protected IntPtr _OldBrush = IntPtr.Zero;
		protected IntPtr _OldBmp = IntPtr.Zero;


		private WeakReference _Control = null;

		private Control Control
		{
			get
			{
				if (_Control != null)
					return (Control) _Control.Target;
				else
					return null;
			}
			set { _Control = new WeakReference(value); }
		}

		public GDISurface(IntPtr hDC)
		{
			mhDC = hDC;
		}

		public GDISurface(int width, int height)
		{
            // added: 31/01/06
			//TODO: test it
            IntPtr deskDC = NativeMethods.GetDC(new IntPtr(0));
            Init(width, height, deskDC);
            Create();
		}

		public GDISurface(int width, int height, IntPtr hdc)
		{
			Init(width, height, hdc);
			Create();
		}

		protected void Init(int width, int height, IntPtr hdc)
		{
			mWidth = width;
			mHeight = height;
			mhDC = NativeMethods.CreateCompatibleDC(hdc);

			mhBMP = NativeMethods.CreateCompatibleBitmap(hdc, width, height);

			IntPtr ret = NativeMethods.SelectObject(mhDC, mhBMP);
			_OldBmp = ret;

			if (mhDC == (IntPtr) 0)
				MessageBox.Show("hDC creation FAILED!!");

			if (mhDC == (IntPtr) 0)
				MessageBox.Show("hBMP creation FAILED!!");


		}

		public GDISurface(int width, int height, GDISurface surface)
		{
			Init(width, height, surface.hDC);
			Create();
		}


		public GDISurface(int width, int height, Control CompatibleControl, bool BindControl)
		{
			IntPtr hDCControk = NativeMethods.ControlDC(CompatibleControl);
			Init(width, height, hDCControk);
			NativeMethods.ReleaseDC(CompatibleControl.Handle, hDCControk);

			if (BindControl)
			{
				Control = CompatibleControl;
			}
			else
			{
			}

			Create();
		}


		public IntPtr hDC
		{
			get { return mhDC; }
		}

		public IntPtr hBMP
		{
			get { return mhBMP; }
		}

		public Color TextForeColor
		{
			//map get,settextcolor
			get { return NativeMethods.IntToColor(NativeMethods.GetTextColor(mhDC)); }
			set { NativeMethods.SetTextColor(mhDC, NativeMethods.ColorToInt(value)); }
		}

		public Color TextBackColor
		{
			//map get,setbkcolor
			get { return NativeMethods.IntToColor(NativeMethods.GetBkColor(mhDC)); }
			set { NativeMethods.SetBkColor(mhDC, NativeMethods.ColorToInt(value)); }
		}


		public bool FontTransparent
		{
			//map get,setbkmode
			//1=transparent , 2=solid
			get { return NativeMethods.GetBkMode(mhDC) < 2; }
			set { NativeMethods.SetBkMode(mhDC, value ? 1 : 2); }
		}


		public Size MeasureString(string Text)
		{
			//map GetTabbedTextExtent
			//to be implemented
			return new Size(0, 0);
		}

		public Size MeasureTabbedString(string Text, int tabsize)
		{
			int ret = NativeMethods.GetTabbedTextExtent(mhDC, Text, Text.Length, 1, ref tabsize);
			return new Size(ret & 0xFFFF, (ret >> 16) & 0xFFFF);
		}

		public void DrawString(string Text, int x, int y, int width, int height)
		{
			//to be implemented
			//map DrawText

		}

		public Size DrawTabbedString(string Text, int x, int y, int taborigin, int tabsize)
		{
			int ret = NativeMethods.TabbedTextOut(mhDC, x, y, Text, Text.Length, 1, ref tabsize, taborigin);
			return new Size(ret & 0xFFFF, (ret >> 16) & 0xFFFF);
		}


		//---------------------------------------
		//render methods , 
		//render to dc ,
		//render to control
		//render to gdisurface

		public void RenderTo(IntPtr hdc, int x, int y)
		{
			//map bitblt
			IntPtr ret = NativeMethods.BitBlt(hdc, x, y, mWidth, mHeight, mhDC, 0, 0, (int) GDIRop.SrcCopy);
		}


		public void RenderTo(GDISurface target, int x, int y)
		{
			RenderTo(target.hDC, x, y);
		}

		public void RenderTo(GDISurface target, int SourceX, int SourceY, int Width, int Height, int DestX, int DestY)
		{
			NativeMethods.BitBlt(target.hDC, DestX, DestY, Width, Height, this.hDC, SourceX, SourceY, (int) GDIRop.SrcCopy);
		}

		public void RenderToControl(int x, int y)
		{
			IntPtr hdc = NativeMethods.ControlDC(Control);

			RenderTo(hdc, x, y);
			NativeMethods.ReleaseDC(Control.Handle, hdc);
		}

		//---------------------------------------

		public Graphics CreateGraphics()
		{
			return Graphics.FromHdc(mhDC);
		}

		//---------------------------------------

		public GDIFont Font
		{
			get
			{
				GDITextMetric tm = new GDITextMetric();
				string fontname = "                                                ";

				NativeMethods.GetTextMetrics(mhDC, ref tm);
				NativeMethods.GetTextFace(mhDC, 79, fontname);

				GDIFont gf = new GDIFont();
				gf.FontName = fontname;
				gf.Bold = (tm.tmWeight > 400); //400=fw_normal
				gf.Italic = (tm.tmItalic != 0);
				gf.Underline = (tm.tmUnderlined != 0);
				gf.Strikethrough = (tm.tmStruckOut != 0);

				gf.Size = (int) (((double) (tm.tmMemoryHeight)/(double) tm.tmDigitizedAspectY)*72);
				return gf;
			}
			set
			{
				IntPtr res = NativeMethods.SelectObject(mhDC, value.hFont);
				if (_OldFont == IntPtr.Zero)
					_OldFont = res;
			}
		}

		public void FillRect(GDIBrush brush, int x, int y, int width, int height)
		{
			APIRect gr;
			gr.top = y;
			gr.left = x;
			gr.right = width + x;
			gr.bottom = height + y;

			NativeMethods.FillRect(mhDC, ref gr, brush.hBrush);
		}

		public void DrawFocusRect(int x, int y, int width, int height)
		{
			APIRect gr;
			gr.top = y;
			gr.left = x;
			gr.right = width + x;
			gr.bottom = height + y;

			NativeMethods.DrawFocusRect(mhDC, ref gr);
		}

		public void FillRect(Color color, int x, int y, int width, int height)
		{
			GDIBrush b = new GDIBrush(color);
			FillRect(b, x, y, width, height);
			b.Dispose();
		}

		public void InvertRect(int x, int y, int width, int height)
		{
			APIRect gr;
			gr.top = y;
			gr.left = x;
			gr.right = width + x;
			gr.bottom = height + y;

			NativeMethods.InvertRect(mhDC, ref gr);
		}

		public void DrawLine(GDIPen pen, Point p1, Point p2)
		{
			IntPtr oldpen = NativeMethods.SelectObject(mhDC, pen.hPen);
			APIPoint gp;
			gp.x = 0;
			gp.y = 0;
			NativeMethods.MoveToEx(mhDC, p1.X, p1.Y, ref gp);
			NativeMethods.LineTo(mhDC, p2.X, p2.Y);
			IntPtr crap = NativeMethods.SelectObject(mhDC, oldpen);
		}

		public void DrawLine(Color color, Point p1, Point p2)
		{
			GDIPen p = new GDIPen(color, 1);
			DrawLine(p, p1, p2);
			p.Dispose();
		}

		public void DrawRect(Color color, int left, int top, int width, int height)
		{
			GDIPen p = new GDIPen(color, 1);
			this.DrawRect(p, left, top, width, height);
			p.Dispose();
		}

		public void DrawRect(GDIPen pen, int left, int top, int width, int height)
		{
			this.DrawLine(pen, new Point(left, top), new Point(left + width, top));
			this.DrawLine(pen, new Point(left, top + height), new Point(left + width, top + height));
			this.DrawLine(pen, new Point(left, top), new Point(left, top + height));
			this.DrawLine(pen, new Point(left + width, top), new Point(left + width, top + height + 1));
		}

		public void Clear(Color color)
		{
			GDIBrush b = new GDIBrush(color);
			Clear(b);
			b.Dispose();
		}

		public void Clear(GDIBrush brush)
		{
			FillRect(brush, 0, 0, mWidth, mHeight);
		}

		public void Flush()
		{
			NativeMethods.GdiFlush();
		}

		protected override void Destroy()
		{
			if (_OldBmp != IntPtr.Zero)
				NativeMethods.SelectObject(this.hDC, _OldBmp);

			if (_OldFont != IntPtr.Zero)
				NativeMethods.SelectObject(this.hDC, _OldFont);

			if (_OldPen != IntPtr.Zero)
				NativeMethods.SelectObject(this.hDC, _OldPen);

			if (_OldBrush != IntPtr.Zero)
				NativeMethods.SelectObject(this.hDC, _OldBrush);

			if (mhBMP != (IntPtr) 0)
				NativeMethods.DeleteObject(mhBMP);

			if (mhDC != (IntPtr) 0)
				NativeMethods.DeleteDC(mhDC);

			mhBMP = (IntPtr) 0;
			mhDC = (IntPtr) 0;


			base.Destroy();
		}

		public void SetBrushOrg(int x, int y)
		{
			APIPoint p;
			p.x = 0;
			p.y = 0;
			NativeMethods.SetBrushOrgEx(mhDC, x, y, ref p);
		}

		protected override void Create()
		{
			base.Create();
		}

	}
}