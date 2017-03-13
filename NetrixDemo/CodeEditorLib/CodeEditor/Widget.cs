
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using GuruComponents.CodeEditor.Library.Drawing;
using GuruComponents.CodeEditor.Library.Win32;

namespace GuruComponents.CodeEditor.Forms
{
	[ToolboxItem(true)]
	public class Widget : Control
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private ControlBorderStyle borderStyle;

		private Color borderColor = Color.Black;
		private Container components = null;
		private bool RunOnce = true;

		public event EventHandler Load = null;


		public Widget()
		{
			SetStyle(ControlStyles.EnableNotifyMessage, true);
			this.BorderStyle = ControlBorderStyle.FixedSingle;
			InitializeComponent();
		}

		[Browsable(false)]
		public Size WindowSize
		{
			get
			{
				APIRect s = new APIRect();
				NativeMethods.GetWindowRect(this.Handle, ref s);
				return new Size(s.Width, s.Height);
			}
		}

		[Category("Appearance - Borders"), Description("The border color")]
		[DefaultValue(typeof (Color), "Black")]
		public Color BorderColor
		{
			get { return borderColor; }

			set
			{
				borderColor = value;
				this.Refresh();
				this.Invalidate();
				UpdateStyles();
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private const int WS_EX_CLIENTEDGE = unchecked((int) 0x00000200);
		private const int WS_BORDER = unchecked((int) 0x00800000);

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;

				if (BorderStyle == ControlBorderStyle.None)
					return cp;

				cp.ExStyle &= (~WS_EX_CLIENTEDGE);
				cp.Style &= (~WS_BORDER);

				return cp;
			}
		}

		[Browsable(true),
			EditorBrowsable(EditorBrowsableState.Always)]
		[Category("Appearance - Borders"), Description("The border style")]
		[DefaultValue(ControlBorderStyle.None)]
		public ControlBorderStyle BorderStyle
		{
			get { return borderStyle; }
			set
			{
				if (borderStyle != value)
				{
					if (!Enum.IsDefined(typeof (ControlBorderStyle), value))
					{
						throw new InvalidEnumArgumentException("value", (int) value, typeof (ControlBorderStyle));
					}
					borderStyle = value;
					UpdateStyles();
					this.Refresh();
				}
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Do not use!", true)]
		public override Image BackgroundImage
		{
			get { return base.BackgroundImage; }
			set { base.BackgroundImage = value; }
		}


		[Browsable(false)]
		public int ClientWidth
		{
			get { return this.WindowSize.Width - (this.BorderWidth*2); }
		}

		[Browsable(false)]
		public int ClientHeight
		{
			get { return this.WindowSize.Height - (this.BorderWidth*2); }
		}

		[Browsable(false)]
		public int BorderWidth
		{
			get
			{
				switch (this.borderStyle)
				{
					case ControlBorderStyle.None:
						{
							return 0;
						}
					case ControlBorderStyle.Sunken:
						{
							return 2;
						}
					case ControlBorderStyle.SunkenThin:
						{
							return 1;
						}
					case ControlBorderStyle.Raised:
						{
							return 2;
						}

					case ControlBorderStyle.Etched:
						{
							return 2;
						}
					case ControlBorderStyle.Bump:
						{
							return 6;
						}
					case ControlBorderStyle.FixedSingle:
						{
							return 1;
						}
					case ControlBorderStyle.FixedDouble:
						{
							return 2;
						}
					case ControlBorderStyle.RaisedThin:
						{
							return 1;
						}
					case ControlBorderStyle.Dotted:
						{
							return 1;
						}
					case ControlBorderStyle.Dashed:
						{
							return 1;
						}
				}


				return this.Height;
			}
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// BaseControl
			// 
			this.Size = new System.Drawing.Size(272, 264);


		}

		#endregion

		protected virtual void OnLoad(EventArgs e)
		{
			if (Load != null)
				Load(this, e);
			this.Refresh();
		}

		protected override unsafe void WndProc(ref Message m)
		{
			if (m.Msg == (int) WindowMessage.WM_NCPAINT)
			{
				RenderBorder();
			}
			else if (m.Msg == (int) WindowMessage.WM_SHOWWINDOW)
			{
				if (RunOnce)
				{
					RunOnce = false;
					OnLoad(null);
					base.WndProc(ref m);
					UpdateStyles();
				}
				else
				{
					UpdateStyles();
					base.WndProc(ref m);
				}

			}
			else if (m.Msg == (int) WindowMessage.WM_NCCREATE)
			{
				base.WndProc(ref m);
			}
			else if (m.Msg == (int) WindowMessage.WM_NCCALCSIZE)
			{
				if (m.WParam == (IntPtr) 0)
				{
					APIRect* pRC = (APIRect*) m.LParam;
					//pRC->left -=3;
					base.WndProc(ref m);
				}
				else if (m.WParam == (IntPtr) 1)
				{
					_NCCALCSIZE_PARAMS* pNCP = (_NCCALCSIZE_PARAMS*) m.LParam;


					int t = pNCP->NewRect.top + this.BorderWidth;
					int l = pNCP->NewRect.left + this.BorderWidth;
					int b = pNCP->NewRect.bottom - this.BorderWidth;
					int r = pNCP->NewRect.right - this.BorderWidth;

					base.WndProc(ref m);

					pNCP->NewRect.top = t;
					pNCP->NewRect.left = l;
					pNCP->NewRect.right = r;
					pNCP->NewRect.bottom = b;

					return;
				}
			}
			else
			{
				base.WndProc(ref m);
			}
		}

		private void RenderBorder()
		{
			IntPtr hdc = NativeMethods.GetWindowDC(this.Handle);
			APIRect s = new APIRect();
			NativeMethods.GetWindowRect(this.Handle, ref s);

			using (Graphics g = Graphics.FromHdc(hdc))
			{
				DrawingTools.DrawBorder((ControlBorderStyle) (int) this.BorderStyle, this.BorderColor, g, new Rectangle(0, 0, s.Width, s.Height));
			}
			NativeMethods.ReleaseDC(this.Handle, hdc);
		}


		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
		}

//		protected override void OnHandleCreated(System.EventArgs e)
//		{
//			base.OnHandleCreated (e);
//		//	this.UpdateStyles ();
//			Console.WriteLine ("gapa");
//		}
//
//		protected override void OnHandleDestroyed(System.EventArgs e)
//		{			
//			base.OnHandleDestroyed (e);
//			Console.WriteLine ("apa");
//		}
//
//		protected override void OnParentChanged(System.EventArgs e)
//		{
//			base.OnParentChanged (e);
//		}
	}
}