using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using GuruComponents.CodeEditor.Forms;
using GuruComponents.CodeEditor.Library.Win32;

namespace GuruComponents.CodeEditor.Library.Drawing
{
	public class DesktopGraphics
	{
		protected IntPtr hdc = new IntPtr(0);
		protected IntPtr handle = new IntPtr(0);
		public readonly Graphics Graphics;

		public DesktopGraphics()
		{
			handle = NativeMethods.GetDesktopWindow();
			hdc = NativeMethods.GetWindowDC(hdc);
			this.Graphics = Graphics.FromHdc(hdc);
		}

		public void Dispose()
		{
			NativeMethods.ReleaseDC(handle, hdc);
		}
	}

	public sealed class DrawingTools
	{
		private DrawingTools()
		{
		}

		public static void ResetGraphics(Graphics g)
		{
			g.ReleaseHdc(g.GetHdc());
		}

		public static Color MixColors(Color c1, Color c2, double mix)
		{
			double d = mix;
			return Color.FromArgb((int) (c1.R*(1 - d) + c2.R*d), (int) (c1.G*(1 - d) + c2.G*d), (int) (c1.B*(1 - d) + c2.B*d));
		}

		private static void DrawBorder(Border3DStyle Style, Color BorderColor, Graphics g, Rectangle r)
		{
			SolidBrush normal = new SolidBrush(BorderColor);

			SolidBrush light;

			if (BorderColor.GetBrightness() > 0.6)
				light = new SolidBrush(MixColors(BorderColor, Color.White, 1));
			else
				light = new SolidBrush(MixColors(BorderColor, Color.White, 0.5));

			SolidBrush dark;
			SolidBrush darkdark;

			if (BorderColor.GetBrightness() < 0.5)
			{
				dark = new SolidBrush(MixColors(BorderColor, Color.Black, 0.7));
				darkdark = new SolidBrush(MixColors(BorderColor, Color.Black, 1));
			}
			else
			{
				dark = new SolidBrush(MixColors(BorderColor, Color.Black, 0.4));
				darkdark = new SolidBrush(MixColors(BorderColor, Color.Black, 0.6));
			}

			switch (Style)
			{
				case Border3DStyle.Sunken:
					{
						g.FillRectangle(dark, r.Left, r.Top, r.Width, 1);
						g.FillRectangle(dark, r.Left, r.Top, 1, r.Height);
						g.FillRectangle(darkdark, r.Left + 1, r.Top + 1, r.Width - 2, 1);
						g.FillRectangle(darkdark, r.Left + 1, r.Top + 1, 1, r.Height - 2);

						g.FillRectangle(light, r.Right - 1, r.Top + 1, 1, r.Height - 1);
						g.FillRectangle(light, r.Left + 1, r.Bottom - 1, r.Width - 1, 1);
						g.FillRectangle(normal, r.Right - 2, r.Top + 2, 1, r.Height - 3);
						g.FillRectangle(normal, r.Left + 2, r.Bottom - 2, r.Width - 3, 1);
						break;
					}
				case Border3DStyle.Raised:
					{
						g.FillRectangle(normal, r.Left, r.Top, r.Width - 1, 1);
						g.FillRectangle(normal, r.Left, r.Top, 1, r.Height - 1);
						g.FillRectangle(light, r.Left + 1, r.Top + 1, r.Width - 2, 1);
						g.FillRectangle(light, r.Left + 1, r.Top + 1, 1, r.Height - 2);

						g.FillRectangle(darkdark, r.Right - 1, r.Top, 1, r.Height);
						g.FillRectangle(darkdark, r.Left, r.Bottom - 1, r.Width, 1);
						g.FillRectangle(dark, r.Right - 2, r.Top + 1, 1, r.Height - 2);
						g.FillRectangle(dark, r.Left + 1, r.Bottom - 2, r.Width - 2, 1);

						break;
					}
				case Border3DStyle.RaisedInner:
					{
						g.FillRectangle(light, r.Left, r.Top, r.Width - 1, 1);
						g.FillRectangle(light, r.Left, r.Top, 1, r.Height - 1);
						//					g.FillRectangle (light,r.Left+1,r.Top+1,r.Width -2,1);
						//					g.FillRectangle (light,r.Left+1,r.Top+1,1,r.Height -2);

						g.FillRectangle(dark, r.Right - 1, r.Top, 1, r.Height);
						g.FillRectangle(dark, r.Left, r.Bottom - 1, r.Width, 1);
						//					g.FillRectangle (dark,r.Right -2,r.Top+1,1,r.Height -2);
						//					g.FillRectangle (dark,r.Left  +1,r.Bottom-2,r.Width -2,1);

						break;
					}
				case Border3DStyle.SunkenOuter:
					{
						g.FillRectangle(dark, r.Left, r.Top, r.Width, 1);
						g.FillRectangle(dark, r.Left, r.Top, 1, r.Height);
						//					g.FillRectangle (darkdark,r.Left+1,r.Top+1,r.Width -2,1);
						//					g.FillRectangle (darkdark,r.Left+1,r.Top+1,1,r.Height -2);

						g.FillRectangle(light, r.Right - 1, r.Top + 1, 1, r.Height - 1);
						g.FillRectangle(light, r.Left + 1, r.Bottom - 1, r.Width - 1, 1);
						//				g.FillRectangle (normal,r.Right -2,r.Top+2,1,r.Height -3);
						//				g.FillRectangle (normal,r.Left  +2,r.Bottom-2,r.Width -3,1);

						break;
					}
				case Border3DStyle.Etched:
					{
						break;
					}
				default:
					break;
			}

			normal.Dispose();
			light.Dispose();
			dark.Dispose();
			darkdark.Dispose();
		}


		public static void DrawBorder(ControlBorderStyle Style, Color BorderColor, Graphics g, Rectangle r)
		{
			switch (Style)
			{
				case ControlBorderStyle.Dotted:
					{
						r.Width --;
						r.Height --;
						g.DrawRectangle(new Pen(SystemColors.Control), r);
						Pen p = new Pen(BorderColor);
						p.DashStyle = DashStyle.Dot;
						g.DrawRectangle(p, r);
						break;
					}
				case ControlBorderStyle.Dashed:
					{
						r.Width --;
						r.Height --;
						g.DrawRectangle(new Pen(SystemColors.Control), r);
						Pen p = new Pen(BorderColor);
						p.DashStyle = DashStyle.Dash;
						g.DrawRectangle(p, r);

						break;
					}
				case ControlBorderStyle.Sunken:
					{
						if (BorderColor == Color.Black)
							BorderColor = SystemColors.Control;
						//System.Windows.Forms.ControlPaint.DrawBorder3D (g,r,Border3DStyle.Sunken);
						DrawingTools.DrawBorder(Border3DStyle.Sunken, BorderColor, g, r);
						break;
					}
				case ControlBorderStyle.FixedSingle:
					{
						r.Width --;
						r.Height --;
						g.DrawRectangle(new Pen(BorderColor), r);
						break;
					}
				case ControlBorderStyle.FixedDouble:
					{
						g.DrawRectangle(new Pen(BorderColor), r.Left, r.Top, r.Width - 1, r.Height - 1);
						g.DrawRectangle(new Pen(BorderColor), r.Left + 1, r.Top + 1, r.Width - 3, r.Height - 3);
						break;
					}
				case ControlBorderStyle.Raised:
					{
						if (BorderColor == Color.Black)
							BorderColor = SystemColors.Control;

						DrawingTools.DrawBorder(Border3DStyle.Raised, BorderColor, g, r);
						//System.Windows.Forms.ControlPaint.DrawBorder3D (g,r,Border3DStyle.Raised);
						break;
					}
				case ControlBorderStyle.RaisedThin:
					{
						if (BorderColor == Color.Black)
							BorderColor = Color.FromArgb(SystemColors.Control.R, SystemColors.Control.G, SystemColors.Control.B);

						DrawingTools.DrawBorder(Border3DStyle.RaisedInner, BorderColor, g, r);
						//System.Windows.Forms.ControlPaint.DrawBorder3D (g,r,Border3DStyle.Raised);
						break;
					}
				case ControlBorderStyle.SunkenThin:
					{
						if (BorderColor == Color.Black)
							BorderColor = Color.FromArgb(SystemColors.Control.R, SystemColors.Control.G, SystemColors.Control.B);

						DrawingTools.DrawBorder(Border3DStyle.SunkenOuter, BorderColor, g, r);
						//System.Windows.Forms.ControlPaint.DrawBorder3D (g,r,Border3DStyle.Raised);
						break;
					}
				case ControlBorderStyle.Etched:
					{
						if (BorderColor == Color.Black)
							BorderColor = SystemColors.Control;

						System.Windows.Forms.ControlPaint.DrawBorder3D(g, r, Border3DStyle.Etched);

						break;
					}
				case ControlBorderStyle.Bump:
					{
						if (BorderColor == Color.Black)
							BorderColor = SystemColors.Control;

						SolidBrush b = new SolidBrush(BorderColor);
						g.FillRectangle(b, r.Left, r.Top, r.Width, 4);
						g.FillRectangle(b, r.Left, r.Bottom - 4, r.Width, 4);

						g.FillRectangle(b, r.Left, r.Top, 4, r.Height);
						g.FillRectangle(b, r.Right - 4, r.Top, 4, r.Height);
						b.Dispose();

						DrawingTools.DrawBorder(Border3DStyle.Raised, BorderColor, g, r);
						DrawingTools.DrawBorder(Border3DStyle.Sunken, BorderColor, g, new Rectangle(r.Left + 4, r.Top + 4, r.Width - 8, r.Height - 8));
						break;
					}
				case ControlBorderStyle.Column:
					{
						SolidBrush normal = new SolidBrush(BorderColor);
						SolidBrush light = new SolidBrush(MixColors(BorderColor, Color.White, 1));
						SolidBrush dark = new SolidBrush(MixColors(BorderColor, Color.Black, 0.4));

						SolidBrush darkdark = new SolidBrush(Color.FromArgb(BorderColor.A, System.Windows.Forms.ControlPaint.DarkDark(BorderColor)));


						g.FillRectangle(light, r.Left, r.Top, r.Width, 1);
						g.FillRectangle(light, r.Left, r.Top + 3, 1, r.Height - 1 - 6);
						g.FillRectangle(dark, r.Right - 1, r.Top + 3, 1, r.Height - 6);
						g.FillRectangle(dark, r.Left, r.Bottom - 1, r.Width, 1);
						break;
					}
				case ControlBorderStyle.Row:
					{
						SolidBrush normal = new SolidBrush(BorderColor);
						SolidBrush light = new SolidBrush(MixColors(BorderColor, Color.White, 1));
						SolidBrush dark = new SolidBrush(MixColors(BorderColor, Color.Black, 0.4));

						SolidBrush darkdark = new SolidBrush(Color.FromArgb(BorderColor.A, System.Windows.Forms.ControlPaint.DarkDark(BorderColor)));


						g.FillRectangle(light, r.Left + 3, r.Top, r.Width - 6, 1);
						g.FillRectangle(light, r.Left, r.Top, 1, r.Height - 1);
						g.FillRectangle(dark, r.Right - 1, r.Top, 1, r.Height);
						g.FillRectangle(dark, r.Left + 3, r.Bottom - 1, r.Width - 6, 1);
						break;
					}
			}
		}

		public static void DrawDesignTimeLine(Graphics g, int x1, int y1, int x2, int y2)
		{
			Pen p = new Pen(SystemColors.ControlDarkDark);
			p.DashOffset = 10;
			p.DashStyle = DashStyle.Dash;
			g.DrawLine(p, x1, y1, x2, y2);
			p.Dispose();
		}

		public static void DrawGrayImage(Graphics g, Image Image, int X, int Y, float TransparencyFactor)
		{
			ColorMatrix cm = new ColorMatrix();
			ImageAttributes ia = new ImageAttributes();

			cm.Matrix33 = TransparencyFactor;

			cm.Matrix00 = 0.33333334F;
			cm.Matrix01 = 0.33333334F;
			cm.Matrix02 = 0.33333334F;
			cm.Matrix10 = 0.33333334F;
			cm.Matrix11 = 0.33333334F;
			cm.Matrix12 = 0.33333334F;
			cm.Matrix20 = 0.33333334F;
			cm.Matrix21 = 0.33333334F;
			cm.Matrix22 = 0.33333334F;

			ia.SetColorMatrix(cm);
			g.DrawImage(Image, new Rectangle(X, Y, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, ia);
		}

		public static void DrawTransparentImage(Graphics g, Image Image, int X, int Y, float TransparencyFactor)
		{
			ImageAttributes ia = new ImageAttributes();
			ColorMatrix cm = new ColorMatrix();

			cm.Matrix33 = TransparencyFactor;

			cm.Matrix00 = 1.0F;
			cm.Matrix11 = 1.0F;
			cm.Matrix22 = 1.0F;

			ia.SetColorMatrix(cm);
			g.DrawImage(Image, new Rectangle(X, Y, Image.Width, Image.Height), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, ia);
		}

		public static void DrawDesignTimeBorder(Graphics g, Rectangle rect)
		{
			rect.Width --;
			rect.Height --;
			Pen p = new Pen(SystemColors.ControlDarkDark);
			p.DashStyle = DashStyle.Dash;
			g.DrawRectangle(p, rect);
			p.Dispose();
		}

		public static void DrawInsertIndicatorH(int x, int y, int width, Graphics g, Color c)
		{
			y -= 3;
			x -= 2;

			System.Windows.Forms.ControlPaint.FillReversibleRectangle(new Rectangle(x, y, 2, 7), c);
			System.Windows.Forms.ControlPaint.FillReversibleRectangle(new Rectangle(x + 2, y + 1, width, 5), c);
			System.Windows.Forms.ControlPaint.FillReversibleRectangle(new Rectangle(width + 2 + x, y, 2, 7), c);
		}

		public static void DrawInsertIndicatorV(int x, int y, int height, Graphics g, Color c)
		{
			x -= 3;
			y -= 2;

			System.Windows.Forms.ControlPaint.FillReversibleRectangle(new Rectangle(x, y, 7, 2), c);
			System.Windows.Forms.ControlPaint.FillReversibleRectangle(new Rectangle(x + 1, y + 2, 5, height), c);
			System.Windows.Forms.ControlPaint.FillReversibleRectangle(new Rectangle(x, height + 2 + y, 7, 2), c);

		}


		public static Bitmap DrawControl(Control control)
		{
			Bitmap b = new Bitmap(control.Width, control.Height);
			Graphics g = Graphics.FromImage(b);
			IntPtr hdc = g.GetHdc();
			int i = NativeMethods.SendMessage(control.Handle, (int) WindowMessage.WM_PRINT, (int) hdc, (int) (WMPrintFlags.PRF_CLIENT | WMPrintFlags.PRF_ERASEBKGND));
			g.ReleaseHdc(hdc);
			g.Dispose();

			return b;
		}

		public static bool DrawControl(Control control, Bitmap b)
		{
			Graphics g = Graphics.FromImage(b);
			IntPtr hdc = g.GetHdc();
			int i = NativeMethods.SendMessage(control.Handle, (int) WindowMessage.WM_PRINT, (int) hdc, (int) (WMPrintFlags.PRF_CLIENT | WMPrintFlags.PRF_ERASEBKGND));
			g.ReleaseHdc(hdc);
			g.Dispose();
			return i != 0;
		}


		public static void DrawSortArrow(int x, int y, Graphics g, bool Ascending)
		{
			Color c1 = Color.FromArgb(220, 255, 255, 255);
			Color c2 = Color.FromArgb(140, 0, 0, 0);

			Pen p1 = new Pen(c1);
			Pen p2 = new Pen(c2);

			if (Ascending)
			{
				g.DrawLine(p1, x, y + 6, x + 7, y + 6);
				g.DrawLine(p2, x + 3, y, x, y + 6);
				g.DrawLine(p1, x + 4, y, x + 7, y + 6);
			}
			else
			{
				g.DrawLine(p2, x, y, x + 7, y);
				g.DrawLine(p2, x, y, x + 3, y + 6);
				g.DrawLine(p1, x + 7, y, x + 4, y + 6);
			}
		}
	}
}