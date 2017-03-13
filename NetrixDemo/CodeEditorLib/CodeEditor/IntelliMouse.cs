using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using ScrollEventArgs = GuruComponents.CodeEditor.Forms.IntelliMouse.ScrollEventArgs;
using ScrollEventHandler = GuruComponents.CodeEditor.Forms.IntelliMouse.ScrollEventHandler;
using GuruComponents.CodeEditor.Library.Timers;
using GuruComponents.CodeEditor.Library.Win32;

namespace GuruComponents.CodeEditor.Forms
{
	/// <summary>
	/// Summary description for IntelliMouseControl.
	/// </summary>
	public class IntelliMouseControl : Control
	{
		protected const int WM_MBUTTONDOWN = 0x0207;
		protected const int WM_MBUTTONUP = 0x0208;
		protected const int WM_LBUTTONDOWN = 0x0201;
		protected const int WM_RBUTTONDOWN = 0x0204;
		protected const int WM_CAPTURECHANGED = 0x0215;
		protected const int WM_MOUSELEAVE = 0x02A3;


		protected IContainer components;
		protected bool Active = false;

		#region GENERAL DECLARATIONS

		protected Point ActivationPoint = new Point(0, 0);
		protected Point CurrentDelta = new Point(0, 0);

		protected WeakReference _CurrentParent = null;

		protected Control CurrentParent
		{
			get
			{
				if (_CurrentParent != null)
					return (Control) _CurrentParent.Target;
				else
					return null;
			}
			set { _CurrentParent = new WeakReference(value); }
		}

		#endregion

		#region EVENTS

		public event EventHandler BeginScroll = null;
		public event EventHandler EndScroll = null;
		public event ScrollEventHandler Scroll = null;

		#endregion

		#region PUBLIC PROPERTY IMAGE

		protected Bitmap _Image;
		protected WeakTimer tmrFeedback;
		protected PictureBox picImage;
		protected RegionHandler regionHandler1;


		public Bitmap Image
		{
			get { return _Image; }
			set
			{
				_Image = value;
				IsDirty = true;
			}
		}

		#endregion

		#region PUBLIC PROPERTY TRANSPARENCYKEY

		protected Color _TransparencyKey = Color.FromArgb(255, 0, 255);

		public Color TransparencyKey
		{
			get { return _TransparencyKey; }
			set
			{
				_TransparencyKey = value;
				IsDirty = true;
			}
		}

		#endregion

		#region CONSTRUCTOR

		public IntelliMouseControl()
		{
			InitializeComponent();
//			SetStyle(ControlStyles.Selectable,false);			
//			this.Image = (Bitmap)this.picImage.Image;
//			this.Visible =false;
		}

		#endregion

		#region DISPOSE

		protected override void Dispose(bool disposing)
		{
#if DEBUG
			try
			{
				Console.WriteLine("disposing intellimouse");
			}
			catch
			{
			}
#endif
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region FINALIZE

		~IntelliMouseControl()
		{
#if DEBUG
			try
			{
				Console.WriteLine("finalizing intellimouse");
			}
			catch
			{
			}
#endif
		}

		#endregion

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof (IntelliMouseControl));
			this.tmrFeedback = new GuruComponents.CodeEditor.Library.Timers.WeakTimer(this.components);
			this.picImage = new System.Windows.Forms.PictureBox();
            this.regionHandler1 = new GuruComponents.CodeEditor.Forms.RegionHandler(this.components);
			// 
			// tmrFeedback
			// 
			this.tmrFeedback.Enabled = true;
			this.tmrFeedback.Interval = 10;
			this.tmrFeedback.Tick += new System.EventHandler(this.tmrFeedback_Tick);
			// 
			// picImage
			// 
			this.picImage.Image = ((System.Drawing.Bitmap) (resources.GetObject("picImage.Image")));
			this.picImage.Location = new System.Drawing.Point(17, 17);
			this.picImage.Name = "picImage";
			this.picImage.TabIndex = 0;
			this.picImage.TabStop = false;
			// 
			// regionHandler1
			// 
			this.regionHandler1.Control = null;
			this.regionHandler1.MaskImage = null;
			this.regionHandler1.TransparencyKey = System.Drawing.Color.FromArgb(((System.Byte) (255)), ((System.Byte) (0)), ((System.Byte) (255)));
			// 
			// IntelliMouseControl
			// 
			this.ParentChanged += new System.EventHandler(this.IntelliMouseControl_ParentChanged);

		}

		#endregion

		private bool IsDirty = false;

		protected void CreateRegion()
		{
			this.regionHandler1.ApplyRegion(this, this.Image, this.TransparencyKey);
			IsDirty = false;
		}

		public void Activate(int x, int y)
		{
			if (IsDirty)
				CreateRegion();

			this.Size = new Size(Image.Width, Image.Height);
			this.Location = new Point(x - Image.Width/2, y - Image.Height/2);
			this.ActivationPoint.X = x;
			this.ActivationPoint.Y = y;
			this.BringToFront();
			this.Visible = true;
			this.Focus();
			Active = false;
			Application.DoEvents();
			SetCursor(0, 0);
			tmrFeedback.Enabled = true;
			onBeginScroll(new EventArgs());
			NativeMethods.SendMessage(this.Handle, WM_MBUTTONDOWN, 0, 0);
			Active = true;
		}

		protected void SetCursor(int x, int y)
		{
			Assembly assembly = GetType().Assembly;

			int dY = y;
			int dX = x;

			CurrentDelta.X = dX;
			CurrentDelta.Y = dY;

			if (dY > 16)
			{
				this.Cursor = new Cursor(assembly.GetManifestResourceStream("MoveDown.cur"));
				CurrentDelta.Y -= 16;

			}
			else if (dY < -16)
			{
				this.Cursor = new Cursor(assembly.GetManifestResourceStream("MoveUp.cur"));
				CurrentDelta.Y += 16;

			}
			else
			{
				this.Cursor = new Cursor(assembly.GetManifestResourceStream("MoveUpDown.cur"));
				CurrentDelta = new Point(0, 0);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (Active)
			{
				if (e.Button != MouseButtons.None && (e.Button != MouseButtons.Middle && e.X != 0 && e.Y != 0))
				{
					Deactivate();
					Point p = new Point(e.X + this.Left, e.Y + this.Top);
					NativeMethods.SendMessage(this.Parent.Handle, WM_LBUTTONDOWN, 0, p.Y*0x10000 + p.X);
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (Active)
			{
				if (e.Button != MouseButtons.Middle && e.Button != MouseButtons.None)
				{
					Deactivate();
				}
				else
				{
					int x = e.X;
					int y = e.Y;
					x -= Image.Width/2;
					y -= Image.Height/2;
					SetCursor(x, y);
					NativeMethods.SendMessage(this.Handle, WM_MBUTTONDOWN, 0, 0);
				}
			}
			else
			{
				base.OnMouseMove(e);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
		}

		protected void Deactivate()
		{
			NativeMethods.SendMessage(this.Handle, WM_MBUTTONUP, 0, 0);
			Active = false;
			tmrFeedback.Enabled = false;
			this.Hide();
			onEndScroll(new EventArgs());
			this.Parent.Focus();
		}

		protected override void OnResize(EventArgs e)
		{
			if (this.Image != null)
				this.Size = new Size(Image.Width, Image.Height);
			else
				this.Size = new Size(32, 32);

		}

		protected void Parent_MouseDown(object s, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				this.Activate(e.X, e.Y);
			}
		}

		protected void tmrFeedback_Tick(object sender, EventArgs e)
		{
			ScrollEventArgs a = new ScrollEventArgs();
			a.DeltaX = CurrentDelta.X;
			a.DeltaY = CurrentDelta.Y;
			onScroll(a);
		}

		protected virtual void onBeginScroll(EventArgs e)
		{
			if (BeginScroll != null)
				BeginScroll(this, e);
		}

		protected virtual void onEndScroll(EventArgs e)
		{
			if (EndScroll != null)
				EndScroll(this, e);
		}

		protected virtual void onScroll(ScrollEventArgs e)
		{
			if (Scroll != null)
				Scroll(this, e);
		}

		protected void IntelliMouseControl_ParentChanged(object sender, EventArgs e)
		{
			if (CurrentParent != null)
			{
				CurrentParent.MouseDown -= new MouseEventHandler(this.Parent_MouseDown);
			}
			if (this.Parent != null)
			{
				this.Parent.MouseDown += new MouseEventHandler(this.Parent_MouseDown);
				this.Deactivate();
			}


		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			Deactivate();
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			Deactivate();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			Deactivate();
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_MOUSELEAVE)
			{
				base.WndProc(ref m);
				this.Deactivate();
				return;
			}

			base.WndProc(ref m);
		}
	}
}