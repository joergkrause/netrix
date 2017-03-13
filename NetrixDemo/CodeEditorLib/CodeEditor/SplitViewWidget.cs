
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using GuruComponents.CodeEditor.Library.Win32;

namespace GuruComponents.CodeEditor.Forms
{
	/// <summary>
	/// Summary description for SplitView.
	/// </summary>
	public class SplitViewWidget : Control
	{
		private Panel Vertical;
		private Panel Horizontal;
		private Panel Center;

		private Control _UpperLeft = null;
		private Control _UpperRight = null;
		private Control _LowerLeft = null;
		private Control _LowerRight = null;
		private Point PrevPos = new Point(0);
		private bool FirstTime = false;


		private SizeAction Action = 0;
		private Point StartPos = new Point(0, 0);

		/// <summary>
		/// an event fired when the split view is resized.
		/// </summary>
		public event EventHandler Resizing = null;

		/// <summary>
		/// an event fired when the top views are hidden.
		/// </summary>
		public event EventHandler HideTop = null;

		/// <summary>
		/// an event fired when the left views are hidden.
		/// </summary>
		public event EventHandler HideLeft = null;

		private void OnResizing()
		{
			if (Resizing != null)
				Resizing(this, new EventArgs());
		}

		private void OnHideLeft()
		{
			if (HideLeft != null)
				HideLeft(this, new EventArgs());
		}

		private void OnHideTop()
		{
			if (HideTop != null)
				HideTop(this, new EventArgs());
		}

		private enum SizeAction
		{
			None = 0,
			SizeH = 1,
			SizeV = 2,
			SizeA = 3
		}


		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		/// <summary>
		/// Default constructor for the splitview control
		/// </summary>
		public SplitViewWidget()
		{
			// This call is required by the Windows.Forms Form Designer.
			this.SetStyle(ControlStyles.ContainerControl, true);
//			this.SetStyle(ControlStyles.AllPaintingInWmPaint ,false);
//			this.SetStyle(ControlStyles.DoubleBuffer ,false);
//			this.SetStyle(ControlStyles.Selectable,true);
//			this.SetStyle(ControlStyles.ResizeRedraw ,true);
//			this.SetStyle(ControlStyles.Opaque ,true);			
//			this.SetStyle(ControlStyles.UserPaint,true);
			//SetStyle(ControlStyles.Selectable ,true);

			InitializeComponent();


			DoResize();
			ReSize(0, 0);
			//this.Refresh ();
			// TODO: Add any initialization after the InitForm call

		}

//		protected override void OnLoad(EventArgs e)
//		{
//			
//		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
#if DEBUG
			try
			{
				Console.WriteLine("disposing splitview");
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

		/// <summary>
		/// Gets or Sets the control that should be confined to the upper left view.
		/// </summary>
		public Control UpperLeft
		{
			get { return _UpperLeft; }
			set
			{
				_UpperLeft = value;
				DoResize();
			}
		}

		/// <summary>
		/// Gets or Sets the control that should be confined to the upper right view.
		/// </summary>
		public Control UpperRight
		{
			get { return _UpperRight; }
			set
			{
				_UpperRight = value;
				DoResize();
			}
		}

		/// <summary>
		/// Gets or Sets the control that should be confined to the lower left view.
		/// </summary>
		public Control LowerLeft
		{
			get { return _LowerLeft; }
			set
			{
				_LowerLeft = value;
				DoResize();
			}
		}

		/// <summary>
		/// Gets or Sets the control that should be confined to the lower right view.
		/// </summary>
		public Control LowerRight
		{
			get { return _LowerRight; }
			set
			{
				_LowerRight = value;
				DoResize();
			}
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Vertical = new System.Windows.Forms.Panel();
			this.Horizontal = new System.Windows.Forms.Panel();
			this.Center = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// Vertical
			// 
			this.Vertical.BackColor = System.Drawing.SystemColors.Control;
			this.Vertical.Cursor = System.Windows.Forms.Cursors.VSplit;
			this.Vertical.Name = "Vertical";
			this.Vertical.Size = new System.Drawing.Size(4, 264);
			this.Vertical.TabIndex = 0;
			this.Vertical.Resize += new System.EventHandler(this.Vertical_Resize);
			this.Vertical.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Vertical_MouseUp);
			this.Vertical.DoubleClick += new System.EventHandler(this.Vertical_DoubleClick);
			this.Vertical.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Vertical_MouseMove);
			this.Vertical.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Vertical_MouseDown);
			// 
			// Horizontal
			// 
			this.Horizontal.BackColor = System.Drawing.SystemColors.Control;
			this.Horizontal.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.Horizontal.Name = "Horizontal";
			this.Horizontal.Size = new System.Drawing.Size(320, 4);
			this.Horizontal.TabIndex = 1;
			this.Horizontal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Horizontal_MouseUp);
			this.Horizontal.DoubleClick += new System.EventHandler(this.Horizontal_DoubleClick);
			this.Horizontal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Horizontal_MouseMove);
			this.Horizontal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Horizontal_MouseDown);
			// 
			// Center
			// 
			this.Center.BackColor = System.Drawing.SystemColors.Control;
			this.Center.Cursor = System.Windows.Forms.Cursors.SizeAll;
			this.Center.Location = new System.Drawing.Point(146, 69);
			this.Center.Name = "Center";
			this.Center.Size = new System.Drawing.Size(24, 24);
			this.Center.TabIndex = 2;
			this.Center.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Center_MouseUp);
			this.Center.DoubleClick += new System.EventHandler(this.Center_DoubleClick);
			this.Center.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Center_MouseMove);
			this.Center.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Center_MouseDown);
			// 
			// SplitViewControl
			// 
			this.BackColor = System.Drawing.Color.Magenta;
			this.Controls.AddRange(new System.Windows.Forms.Control[]
				{
					this.Center,
					this.Horizontal,
					this.Vertical
				});
			this.Size = new System.Drawing.Size(200, 200);
			this.VisibleChanged += new System.EventHandler(this.SplitViewControl_VisibleChanged);
			this.Enter += new System.EventHandler(this.SplitViewControl_Enter);
			this.Leave += new System.EventHandler(this.SplitViewControl_Leave);
			this.ResumeLayout(false);

		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			DoResize();
		}

		private void DoResize()
		{
			//			int OldWidth=Horizontal.Width ;
			//			int OldHeight=Vertical.Height;
			int NewHeight = this.Height;
			int NewWidth = this.Width;

			if (NewHeight != 0 && NewWidth != 0)
			{
				this.SuspendLayout();
				//				Horizontal.Top = (int)(NewHeight*HorizontalPos);
				//				Vertical.Left =(int)(NewWidth*VerticalPos);
				//
				//				int CenterY=(Horizontal.Top+Horizontal.Height /2)-Center.Height/2;
				//				int CenterX=(Vertical.Left+Vertical.Width /2)-Center.Width /2;
				//
				//				Center.Location =new Point (CenterX,CenterY);


				//ReSize (0,0);
				ReSize2();
				OnResizing();

				if (Horizontal.Top < 15)
				{
					Horizontal.Top = 0 - Horizontal.Height;
					OnHideTop();
				}

				if (Vertical.Left < 15)
				{
					Vertical.Left = 0 - Vertical.Width;
					OnHideLeft();
				}


				Horizontal.Width = this.Width;
				Vertical.Height = this.Height;
				Horizontal.SendToBack();
				Vertical.SendToBack();
				Horizontal.BackColor = SystemColors.Control;
				Vertical.BackColor = SystemColors.Control;


				//this.SendToBack ();
				int RightLeft = Vertical.Left + Vertical.Width;
				int RightLowerTop = Horizontal.Top + Horizontal.Height;
				int RightWidth = this.Width - RightLeft;
				int LowerHeight = this.Height - RightLowerTop;
				int UpperHeight = Horizontal.Top;
				int LeftWidth = Vertical.Left;

				if (LowerRight != null)
				{
					LowerRight.BringToFront();
					LowerRight.SetBounds(RightLeft, RightLowerTop, RightWidth, LowerHeight);
				}
				if (UpperRight != null)
				{
					UpperRight.BringToFront();
					UpperRight.SetBounds(RightLeft, 0, RightWidth, UpperHeight);
				}


				if (LowerLeft != null)
				{
					LowerLeft.BringToFront();
					LowerLeft.SetBounds(0, RightLowerTop, LeftWidth, LowerHeight);
				}
				if (UpperLeft != null)
				{
					UpperLeft.BringToFront();
					UpperLeft.SetBounds(0, 0, LeftWidth, UpperHeight);
				}
				this.ResumeLayout(); //ggf
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			this.DoResize();
		}


		private void Vertical_Resize(object sender, EventArgs e)
		{
		}

		private void Horizontal_MouseDown(object sender, MouseEventArgs e)
		{
			Action = SizeAction.SizeH;
			StartPos = new Point(e.X, e.Y);
			Horizontal.BringToFront();
			Horizontal.BackColor = SystemColors.ControlDark;
			FirstTime = true;
		}

		private void Vertical_MouseDown(object sender, MouseEventArgs e)
		{
			Action = SizeAction.SizeV;
			StartPos = new Point(e.X, e.Y);
			Vertical.BringToFront();
			Vertical.BackColor = SystemColors.ControlDark;
			FirstTime = true;
		}

		private void Center_MouseDown(object sender, MouseEventArgs e)
		{
			Action = SizeAction.SizeA;
			StartPos = new Point(e.X, e.Y);
			Vertical.BringToFront();
			Horizontal.BringToFront();
			Vertical.BackColor = SystemColors.ControlDark;
			Horizontal.BackColor = SystemColors.ControlDark;
			FirstTime = true;
		}

		private void Horizontal_MouseUp(object sender, MouseEventArgs e)
		{
			int xDiff = 0;
			int yDiff = StartPos.Y - e.Y;
			//	StartPos=new Point (e.X,e.Y);
			ReSize(xDiff, yDiff);

			Action = SizeAction.None;
			DoResize();
		}

		private void Vertical_MouseUp(object sender, MouseEventArgs e)
		{
			int xDiff = StartPos.X - e.X;
			int yDiff = 0;
			//	StartPos=new Point (e.X,e.Y);
			ReSize(xDiff, yDiff);

			Action = SizeAction.None;
			DoResize();
			this.Refresh();
		}

		private void Center_MouseUp(object sender, MouseEventArgs e)
		{
			int xDiff = StartPos.X - e.X;
			int yDiff = StartPos.Y - e.Y;
			//	StartPos=new Point (e.X,e.Y);
			ReSize(xDiff, yDiff);

			Action = SizeAction.None;
			DoResize();
		}

		private void Horizontal_MouseMove(object sender, MouseEventArgs e)
		{
			if (Action == SizeAction.SizeH && e.Button == MouseButtons.Left)
			{
				Point start;
				int x = e.X;
				int y = e.Y;


				if (y + Horizontal.Top > this.Height - 4)
					y = this.Height - 4 - Horizontal.Top;
				if (y + Horizontal.Top < 0)
					y = 0 - Horizontal.Top;

				if (!FirstTime)
				{
					start = this.PointToScreen(this.Location);
					start.Y += PrevPos.Y + Horizontal.Location.Y;
					ControlPaint.FillReversibleRectangle(new Rectangle(start.X, start.Y, this.Width, 3), Color.Black);
				}
				else
					FirstTime = false;

				start = this.PointToScreen(this.Location);
				start.Y += y + Horizontal.Location.Y;
				ControlPaint.FillReversibleRectangle(new Rectangle(start.X, start.Y, this.Width, 3), Color.Black);

				PrevPos = new Point(x, y);
			}
		}

		private void Vertical_MouseMove(object sender, MouseEventArgs e)
		{
			if (Action == SizeAction.SizeV && e.Button == MouseButtons.Left)
			{
				Point start;
				int x = e.X;
				int y = e.Y;

				if (x + Vertical.Left > this.Width - 4)
					x = this.Width - 4 - Vertical.Left;
				if (x + Vertical.Left < 0)
					x = 0 - Vertical.Left;


				if (!FirstTime)
				{
					start = this.PointToScreen(this.Location);
					start.X += PrevPos.X + Vertical.Location.X;
					ControlPaint.FillReversibleRectangle(new Rectangle(start.X, start.Y, 3, this.Height), Color.Black);
				}
				else
					FirstTime = false;

				start = this.PointToScreen(this.Location);
				start.X += x + Vertical.Location.X;
				ControlPaint.FillReversibleRectangle(new Rectangle(start.X, start.Y, 3, this.Height), Color.Black);

				PrevPos = new Point(x, y);
			}
		}

		private void Center_MouseMove(object sender, MouseEventArgs e)
		{
			if (Action == SizeAction.SizeA && e.Button == MouseButtons.Left)
			{
				Point start;
				int x = e.X - Center.Width/2;
				int y = e.Y - Center.Height/2;

				if (!FirstTime)
				{
					start = this.PointToScreen(this.Location);
					start.X += PrevPos.X + Vertical.Location.X;
					ControlPaint.FillReversibleRectangle(new Rectangle(start.X, start.Y, 3, this.Height), Color.Black);

					start = this.PointToScreen(this.Location);
					start.Y += PrevPos.Y + Horizontal.Location.Y;
					ControlPaint.FillReversibleRectangle(new Rectangle(start.X, start.Y, this.Width, 3), SystemColors.ControlDark);

				}
				else
					FirstTime = false;

				start = this.PointToScreen(this.Location);
				start.X += x + Vertical.Location.X;
				ControlPaint.FillReversibleRectangle(new Rectangle(start.X, start.Y, 3, this.Height), Color.Black);

				start = this.PointToScreen(this.Location);
				start.Y += y + Horizontal.Location.Y;
				ControlPaint.FillReversibleRectangle(new Rectangle(start.X, start.Y, this.Width, 3), SystemColors.ControlDark);


				PrevPos = new Point(x, y);
			}
		}


		private void ReSize(int x, int y)
		{
			//if (x==0 && y==0)
			//	return;


			this.SuspendLayout();

			int xx = Vertical.Left - x;
			int yy = Horizontal.Top - y;

			if (xx < 0)
				xx = 0;

			if (yy < 0)
				yy = 0;

			if (yy > this.Height - Horizontal.Height - SystemInformation.VerticalScrollBarWidth*3)
				yy = this.Height - Horizontal.Height - SystemInformation.VerticalScrollBarWidth*3;


			if (xx > this.Width - Vertical.Width - SystemInformation.VerticalScrollBarWidth*3)
				xx = this.Width - Vertical.Width - SystemInformation.VerticalScrollBarWidth*3;

			if (xx != Vertical.Left)
				Vertical.Left = xx;

			if (yy != Horizontal.Top)
				Horizontal.Top = yy;


			int CenterY = (Horizontal.Top + Horizontal.Height/2) - Center.Height/2;
			int CenterX = (Vertical.Left + Vertical.Width/2) - Center.Width/2;

			Center.Location = new Point(CenterX, CenterY);
			this.ResumeLayout();
			this.Invalidate();

			try
			{
				if (UpperLeft != null)
					UpperLeft.Refresh();
				if (UpperLeft != null)
					UpperLeft.Refresh();
				if (UpperLeft != null)
					UpperLeft.Refresh();
				if (UpperLeft != null)
					UpperLeft.Refresh();
			}
			catch
			{
			}
			OnResizing();
			//DoResize();	
			//this.Refresh ();
		}

		private void ReSize2()
		{
			int xx = Vertical.Left;
			int yy = Horizontal.Top;

			if (xx < 0)
				xx = 0;

			if (yy < 0)
				yy = 0;

			if (yy > this.Height - Horizontal.Height - SystemInformation.VerticalScrollBarWidth*3)
			{
				yy = this.Height - Horizontal.Height - SystemInformation.VerticalScrollBarWidth*3;
				if (yy != Horizontal.Top)
					Horizontal.Top = yy;
			}


			if (xx > this.Width - Vertical.Width - SystemInformation.VerticalScrollBarWidth*3)
			{
				xx = this.Width - Vertical.Width - SystemInformation.VerticalScrollBarWidth*3;
				if (xx != Vertical.Left)
					Vertical.Left = xx;
			}

			int CenterY = (Horizontal.Top + Horizontal.Height/2) - Center.Height/2;
			int CenterX = (Vertical.Left + Vertical.Width/2) - Center.Width/2;

			Center.Location = new Point(CenterX, CenterY);
		}

		private void Center_DoubleClick(object sender, EventArgs e)
		{
			Horizontal.Top = -100;
			Vertical.Left = -100;
			DoResize();
		}

		private void Vertical_DoubleClick(object sender, EventArgs e)
		{
			Vertical.Left = -100;
			DoResize();
		}

		private void Horizontal_DoubleClick(object sender, EventArgs e)
		{
			Horizontal.Top = -100;
			DoResize();
		}

		/// <summary>
		/// Splits the view horiziontally.
		/// </summary>
		public void Split5050h()
		{
			Horizontal.Top = this.Height/2;
			DoResize();
		}

		/// <summary>
		/// Splits teh view vertically.
		/// </summary>
		public void Split5050v()
		{
			Vertical.Left = this.Width/2;
			DoResize();
		}

		public int SplitviewV
		{
			get { return Vertical.Left; }
			set
			{
				Vertical.Left = value;
				DoResize();
			}
		}

		public int SplitviewH
		{
			get { return Horizontal.Top; }
			set
			{
				Horizontal.Top = value;
				DoResize();
			}
		}


		public void ResetSplitview()
		{
			Vertical.Left = 0;
			Horizontal.Top = 0;
			DoResize();
		}

		/// <summary>
		/// Start dragging the horizontal splitter.
		/// </summary>
		public void InvokeMouseDownh()
		{
			IntPtr hwnd = Horizontal.Handle;
			NativeMethods.SendMessage(hwnd, (int) WindowMessage.WM_LBUTTONDOWN, 0, 0);
		}

		/// <summary>
		/// Start dragging the vertical splitter.
		/// </summary>
		public void InvokeMouseDownv()
		{
			IntPtr hwnd = Vertical.Handle;
			NativeMethods.SendMessage(hwnd, (int) WindowMessage.WM_LBUTTONDOWN, 0, 0);
		}

		private void SplitViewControl_Leave(object sender, EventArgs e)
		{
		}

		private void SplitViewControl_Enter(object sender, EventArgs e)
		{
		}


		private void SplitViewControl_VisibleChanged(object sender, EventArgs e)
		{
		}
	}
}