using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{
	/// <summary>
	/// A control that allows the user to select any color from the
	/// RGB color space.
	/// </summary>
	/// <remarks>
	/// The control presents a 2 dimensional slice through the 3D color cube.  The user
	/// may select Red, Green or Blue to be the z-axis where the z-axis extends into the screen and the
	/// x and y-axes are the remaining pair of colors.
	/// The control displays a 256x256 color palette calculated over the x and y axes combined
	/// with the selected z-axis value.  This palette is varied continuously as the user changes the 
	/// selected z-axis value.
	/// <br></br><br></br>
	/// The control is inpired from <a href="http://dotnet.securedomains.com/colorpicker/default.aspx">
	/// Peter McMahon's ColorPicker.NET</a>.
	/// </remarks>
	[ToolboxItem(false)]
    [ToolboxBitmap(typeof(ResourceManager), "ToolBox.ColorEditor.ico")]
    [Designer(typeof(NetrixUIDesigner))]
    public class ColorSlider : UserControl
	{
		static readonly Color defaultColor = Color.FromArgb(127,127,127);
		const bool defaultContinuousScroll = true;

		private TrackBar trackBarRed;
		private IContainer components;
		private NumericUpDown numericUpDownRed;
		private Panel panel1;
		private RadioButton radioButtonBlue;
		private RadioButton radioButtonRed;
		private RadioButton radioButtonGreen;
		private PictureBox pictureBox;
		private TrackBar trackBarGreen;
		private TrackBar trackBarBlue;
		private NumericUpDown numericUpDownGreen;
		private NumericUpDown numericUpDownBlue;

		private Bitmap imgColors = new Bitmap(256, 256);

		private ZAxis zaxis = ZAxis.red;
		private bool bContinuousScrollZAxis = defaultContinuousScroll;

		private int x_val = 0;
		private int y_val = 0;
		private bool bMouseDown = false;
		private Label label1;
		private TrackBar trackBarBrightness;
		private Panel panel2;
		private Panel panel3;
		private ToolTip toolTip1;
        /// <summary>
		/// Creates a CustomColorPicker control.
		/// It defaults to the blue/green side of the color cube (z-axis=red).
		/// </summary>
		public ColorSlider()
        {
            // This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			pictureBox.Image = imgColors;
			ResetColor();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.radioButtonBlue = new System.Windows.Forms.RadioButton();
            this.numericUpDownRed = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownGreen = new System.Windows.Forms.NumericUpDown();
            this.radioButtonGreen = new System.Windows.Forms.RadioButton();
            this.trackBarRed = new System.Windows.Forms.TrackBar();
            this.trackBarBlue = new System.Windows.Forms.TrackBar();
            this.radioButtonRed = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trackBarGreen = new System.Windows.Forms.TrackBar();
            this.numericUpDownBlue = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarBrightness = new System.Windows.Forms.TrackBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBlue)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(32, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(256, 256);
            this.pictureBox.TabIndex = 11;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            // 
            // radioButtonBlue
            // 
            this.radioButtonBlue.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonBlue.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(192)));
            this.radioButtonBlue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonBlue.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radioButtonBlue.Location = new System.Drawing.Point(99, 0);
            this.radioButtonBlue.Name = "radioButtonBlue";
            this.radioButtonBlue.Size = new System.Drawing.Size(40, 24);
            this.radioButtonBlue.TabIndex = 3;
            this.radioButtonBlue.Text = "Fix";
            this.radioButtonBlue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.radioButtonBlue, "Set the Z axis to blue");
            this.radioButtonBlue.CheckedChanged += new System.EventHandler(this.radioButtonBlue_CheckedChanged);
            // 
            // numericUpDownRed
            // 
            this.numericUpDownRed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownRed.Hexadecimal = true;
            this.numericUpDownRed.Location = new System.Drawing.Point(288, 186);
            this.numericUpDownRed.Maximum = new System.Decimal(new int[] {
                                                                             255,
                                                                             0,
                                                                             0,
                                                                             0});
            this.numericUpDownRed.Name = "numericUpDownRed";
            this.numericUpDownRed.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownRed.TabIndex = 7;
            this.numericUpDownRed.ValueChanged += new System.EventHandler(this.numericUpDownRed_ValueChanged);
            this.numericUpDownRed.Leave += new System.EventHandler(this.numericUpDownRed_Leave);
            // 
            // numericUpDownGreen
            // 
            this.numericUpDownGreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownGreen.Hexadecimal = true;
            this.numericUpDownGreen.Location = new System.Drawing.Point(336, 186);
            this.numericUpDownGreen.Maximum = new System.Decimal(new int[] {
                                                                               255,
                                                                               0,
                                                                               0,
                                                                               0});
            this.numericUpDownGreen.Name = "numericUpDownGreen";
            this.numericUpDownGreen.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownGreen.TabIndex = 8;
            this.numericUpDownGreen.ValueChanged += new System.EventHandler(this.numericUpDownGreen_ValueChanged);
            // 
            // radioButtonGreen
            // 
            this.radioButtonGreen.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonGreen.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
            this.radioButtonGreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonGreen.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radioButtonGreen.Location = new System.Drawing.Point(51, 0);
            this.radioButtonGreen.Name = "radioButtonGreen";
            this.radioButtonGreen.Size = new System.Drawing.Size(40, 24);
            this.radioButtonGreen.TabIndex = 2;
            this.radioButtonGreen.Text = "Fix";
            this.radioButtonGreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.radioButtonGreen, "Set the Z axis to green");
            this.radioButtonGreen.CheckedChanged += new System.EventHandler(this.radioButtonGreen_CheckedChanged);
            // 
            // trackBarRed
            // 
            this.trackBarRed.LargeChange = 16;
            this.trackBarRed.Location = new System.Drawing.Point(288, 24);
            this.trackBarRed.Maximum = 255;
            this.trackBarRed.Name = "trackBarRed";
            this.trackBarRed.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarRed.Size = new System.Drawing.Size(45, 160);
            this.trackBarRed.TabIndex = 4;
            this.trackBarRed.TickFrequency = 16;
            this.trackBarRed.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarRed.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trackBarRed_KeyUp);
            this.trackBarRed.Leave += new System.EventHandler(this.trackBarRed_Leave);
            this.trackBarRed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarRed_MouseUp);
            this.trackBarRed.ValueChanged += new System.EventHandler(this.trackBarRed_ValueChanged);
            // 
            // trackBarBlue
            // 
            this.trackBarBlue.LargeChange = 16;
            this.trackBarBlue.Location = new System.Drawing.Point(384, 24);
            this.trackBarBlue.Maximum = 255;
            this.trackBarBlue.Name = "trackBarBlue";
            this.trackBarBlue.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarBlue.Size = new System.Drawing.Size(45, 160);
            this.trackBarBlue.TabIndex = 6;
            this.trackBarBlue.TickFrequency = 16;
            this.trackBarBlue.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarBlue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trackBarBlue_KeyUp);
            this.trackBarBlue.Leave += new System.EventHandler(this.trackBarBlue_Leave);
            this.trackBarBlue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarBlue_MouseUp);
            this.trackBarBlue.ValueChanged += new System.EventHandler(this.trackBarBlue_ValueChanged);
            // 
            // radioButtonRed
            // 
            this.radioButtonRed.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButtonRed.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(0)), ((System.Byte)(0)));
            this.radioButtonRed.Checked = true;
            this.radioButtonRed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonRed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.radioButtonRed.Location = new System.Drawing.Point(3, 0);
            this.radioButtonRed.Name = "radioButtonRed";
            this.radioButtonRed.Size = new System.Drawing.Size(40, 24);
            this.radioButtonRed.TabIndex = 1;
            this.radioButtonRed.TabStop = true;
            this.radioButtonRed.Text = "Fix";
            this.radioButtonRed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.radioButtonRed, "Set the Z axis to red");
            this.radioButtonRed.CheckedChanged += new System.EventHandler(this.radioButtonRed_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButtonBlue);
            this.panel1.Controls.Add(this.radioButtonRed);
            this.panel1.Controls.Add(this.radioButtonGreen);
            this.panel1.Location = new System.Drawing.Point(288, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(144, 24);
            this.panel1.TabIndex = 10;
            // 
            // trackBarGreen
            // 
            this.trackBarGreen.LargeChange = 16;
            this.trackBarGreen.Location = new System.Drawing.Point(336, 24);
            this.trackBarGreen.Maximum = 255;
            this.trackBarGreen.Name = "trackBarGreen";
            this.trackBarGreen.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarGreen.Size = new System.Drawing.Size(45, 160);
            this.trackBarGreen.TabIndex = 5;
            this.trackBarGreen.TickFrequency = 16;
            this.trackBarGreen.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarGreen.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trackBarGreen_KeyUp);
            this.trackBarGreen.Leave += new System.EventHandler(this.trackBarGreen_Leave);
            this.trackBarGreen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarGreen_MouseUp);
            this.trackBarGreen.ValueChanged += new System.EventHandler(this.trackBarGreen_ValueChanged);
            // 
            // numericUpDownBlue
            // 
            this.numericUpDownBlue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownBlue.Hexadecimal = true;
            this.numericUpDownBlue.Location = new System.Drawing.Point(384, 186);
            this.numericUpDownBlue.Maximum = new System.Decimal(new int[] {
                                                                              255,
                                                                              0,
                                                                              0,
                                                                              0});
            this.numericUpDownBlue.Name = "numericUpDownBlue";
            this.numericUpDownBlue.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownBlue.TabIndex = 9;
            this.numericUpDownBlue.ValueChanged += new System.EventHandler(this.numericUpDownBlue_ValueChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(296, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "No HTML Color selected";
            // 
            // trackBarBrightness
            // 
            this.trackBarBrightness.LargeChange = 16;
            this.trackBarBrightness.Location = new System.Drawing.Point(0, 24);
            this.trackBarBrightness.Maximum = 255;
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarBrightness.Size = new System.Drawing.Size(45, 208);
            this.trackBarBrightness.TabIndex = 13;
            this.trackBarBrightness.TickFrequency = 8;
            this.trackBarBrightness.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarBrightness.ValueChanged += new System.EventHandler(this.trackBarBrightness_ValueChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(32, 24);
            this.panel2.TabIndex = 14;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Location = new System.Drawing.Point(0, 232);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(32, 24);
            this.panel3.TabIndex = 15;
            // 
            // ColorSlider
            // 
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.trackBarBrightness);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownRed);
            this.Controls.Add(this.trackBarRed);
            this.Controls.Add(this.trackBarGreen);
            this.Controls.Add(this.trackBarBlue);
            this.Controls.Add(this.numericUpDownGreen);
            this.Controls.Add(this.numericUpDownBlue);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Name = "ColorSlider";
            this.Size = new System.Drawing.Size(432, 256);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBlue)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		private void MakePalette()
		{
			switch( zaxis )
			{
				case ZAxis.red:
					MakeRedPalette();
					break;
				case ZAxis.blue:
					MakeBluePalette();
					break;
				case ZAxis.green:
					MakeGreenPalette();
					break;
			}
		}

		private void MakeRedPalette()
		{
# if UNSAFE
			byte red_value = Convert.ToByte(trackBarRed.Value);
            BitmapUnsafe ub = new BitmapUnsafe(imgColors);
            ub.MakeRedPalette(red_value);
# else
			int red_value = trackBarRed.Value;
			for( int i=0; i<255; i++ )
			{
				for( int j=0; j<255; j++ )
				{
					imgColors.SetPixel(i, j, Color.FromArgb(i, j, red_value) );
				}
			}
#endif
		}

		private void MakeBluePalette()
		{
# if UNSAFE
			byte blue_value = Convert.ToByte(trackBarBlue.Value);
            BitmapUnsafe ub = new BitmapUnsafe(imgColors);
            ub.MakeBluePalette(blue_value);
# else
			int blue_value = trackBarBlue.Value;

			for( int i=0; i<255; i++ )
			{
				for( int j=0; j<255; j++ )
				{
					imgColors.SetPixel(i, j, Color.FromArgb(i, j, blue_value) );
				}
			}
#endif
      }

		private void MakeGreenPalette()
		{
# if UNSAFE
			byte green_value = Convert.ToByte(trackBarGreen.Value);
            BitmapUnsafe ub = new BitmapUnsafe(imgColors);
            ub.MakeGreenPalette(green_value);
# else

            int green_value = trackBarGreen.Value;

			for( int i=0; i<255; i++ )
			{
				for( int j=0; j<255; j++ )
				{
					imgColors.SetPixel(i, j, Color.FromArgb(i, green_value, j) );
				}
			}
#endif
        }

		private void trackBarRedReleased()
		{
			if( (zaxis == ZAxis.red) && !bContinuousScrollZAxis)
			{
				MakeRedPalette();
				pictureBox.Refresh();
			}
		}

		// Leave not receive for UpDown controls - beta2 bug?
		private void numericUpDownRed_Leave(object sender, EventArgs e)
		{
			trackBarRedReleased();
		}

		private void trackBarRed_Leave(object sender, EventArgs e)
		{
			trackBarRedReleased();
		}

		private void trackBarRed_KeyUp(object sender, KeyEventArgs e)
		{
			trackBarRedReleased();
		}

		private void trackBarRed_MouseUp(object sender, MouseEventArgs e)
		{
			trackBarRedReleased();
		}

		private void trackBarRed_ValueChanged(object sender, EventArgs e)
		{
			numericUpDownRed.Value = trackBarRed.Value;

			Application.DoEvents();

			if( (zaxis == ZAxis.red) && bContinuousScrollZAxis )
			{
				MakeRedPalette();
			}

			GetCoords();

			pictureBox.Refresh();

			FireColorChangedEvent();
		}

		private void numericUpDownRed_ValueChanged(object sender, EventArgs e)
		{
			trackBarRed.Value = (int)numericUpDownRed.Value;
		}

		private void trackBarBlueReleased()
		{
			if( zaxis == ZAxis.blue && !bContinuousScrollZAxis )
			{
				MakeBluePalette();
				pictureBox.Refresh();
			}
		}

		private void trackBarBlue_Leave(object sender, EventArgs e)
		{
			trackBarBlueReleased();
		}
		
		private void trackBarBlue_KeyUp(object sender, KeyEventArgs e)
		{
			trackBarBlueReleased();
		}

		private void trackBarBlue_MouseUp(object sender, MouseEventArgs e)
		{
			trackBarBlueReleased();
		}

		private void trackBarBlue_ValueChanged(object sender, EventArgs e)
		{
			numericUpDownBlue.Value = trackBarBlue.Value;
			Application.DoEvents();

			if( zaxis == ZAxis.blue && bContinuousScrollZAxis )
			{
				MakeBluePalette();
			}

			GetCoords();
			pictureBox.Refresh();

			FireColorChangedEvent();
		}

		private void numericUpDownBlue_ValueChanged(object sender, EventArgs e)
		{
			trackBarBlue.Value = (int)numericUpDownBlue.Value;
		}

		private void trackBarGreenReleased()
		{
			if( zaxis == ZAxis.green && !bContinuousScrollZAxis )
			{
				MakeGreenPalette();
				pictureBox.Refresh();
			}
		}
		
		private void trackBarGreen_Leave(object sender, EventArgs e)
		{
			trackBarGreenReleased();
		}

		private void trackBarGreen_KeyUp(object sender, KeyEventArgs e)
		{
			trackBarGreenReleased();
		}

		private void trackBarGreen_MouseUp(object sender, MouseEventArgs e)
		{
			trackBarGreenReleased();
		}

		private void trackBarGreen_ValueChanged(object sender, EventArgs e)
		{
			numericUpDownGreen.Value = trackBarGreen.Value;
			Application.DoEvents();

			if( zaxis == ZAxis.green && bContinuousScrollZAxis )
			{
				MakeGreenPalette();
			}

			GetCoords();
			pictureBox.Refresh();

			FireColorChangedEvent();
		}

		private void GetCoords()
		{
			switch( zaxis )
			{
				case ZAxis.green:
					x_val = trackBarRed.Value;
					y_val = trackBarBlue.Value;
					break;
				case ZAxis.red:
					x_val = trackBarGreen.Value;
					y_val = trackBarBlue.Value;
					break;
				case ZAxis.blue:
					x_val = trackBarRed.Value;
					y_val = trackBarGreen.Value;
					break;
			}
		}

		private static void SetCoordBound( ref int coord )
		{
			if( coord < 0 )
				coord = 0;
			if( coord > 255 )
				coord = 255;
		}

		private void SetCoords(int x, int y)
		{
			SetCoordBound( ref x );
			SetCoordBound( ref y );

			switch( zaxis )
			{
				case ZAxis.green:
					trackBarRed.Value   = x;
					trackBarBlue.Value  = y;
					break;
				case ZAxis.red:
					trackBarGreen.Value = x;
					trackBarBlue.Value  = y;
					break;
				case ZAxis.blue:
					trackBarRed.Value   = x;
					trackBarGreen.Value = y;
					break;
			}
		}

		private void numericUpDownGreen_ValueChanged(object sender, EventArgs e)
		{
			trackBarGreen.Value = (int)numericUpDownGreen.Value;
		}

		private void radioButtonRed_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonRed.Checked)
			{
				zaxis = ZAxis.red;
				MakeRedPalette();
				GetCoords();
				pictureBox.Refresh();
			}
		}

		private void radioButtonGreen_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonGreen.Checked)
			{
				zaxis = ZAxis.green;
				MakeGreenPalette();
				GetCoords();
				pictureBox.Refresh();
			}
		}

		private void radioButtonBlue_CheckedChanged(object sender, EventArgs e)
		{
			if(radioButtonBlue.Checked)
			{
				zaxis = ZAxis.blue;
				MakeBluePalette();
				GetCoords();
				pictureBox.Refresh();
			}
		}

		private void pictureBox_Paint(object sender, PaintEventArgs e)
		{
			if( Enabled )
			{
				Pen p = new Pen(Color.Gray);

				const int offset = 5;

				e.Graphics.DrawLine( p, x_val, 0, x_val, y_val-offset );
				e.Graphics.DrawLine( p, x_val, y_val+offset, x_val, 255 );

				e.Graphics.DrawLine( p, 0, y_val, x_val-offset, y_val );
				e.Graphics.DrawLine( p, x_val+offset, y_val, 255, y_val );

				e.Graphics.DrawRectangle( p, x_val-offset, y_val-offset, 2*offset, 2*offset );

				p.Dispose();
			}
		}

		private void pictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			if( bMouseDown )
			{
				SetCoords(e.X,e.Y);
			}
		}

		private void pictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			bMouseDown = true;
			SetCoords(e.X,e.Y);
		}

		private void pictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			bMouseDown = false;
			this.label1.Text = ColorTranslator.ToHtml(this.Color);
		}

		/// <summary>
		/// The ColorChangedEvent event handler.
		/// </summary>
		[Browsable(true), Category("ColorPicker")]
		public event ColorChangedEventHandler     ColorChanged;

		private void FireColorChangedEvent()
		{
			if( null != ColorChanged )
			{
				Color color = Color.FromArgb(
					trackBarRed.Value,
					trackBarGreen.Value,
					trackBarBlue.Value );

				OnColorChanged(new ColorChangedEventArgs(color));
			}
		}

		/// <summary>
		/// Raises the ColorChanged event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnColorChanged(ColorChangedEventArgs e)
		{
			if( null != ColorChanged )
			{
			    ColorChanged(this, e);
			}
		}


		/// <summary>
		/// Sets/gets the pick color.
		/// </summary>
		[Browsable(true), Category("ColorPanel"), Description("Get/set the pick color.")]
		public Color Color
		{
			get
			{
				return Color.FromArgb(
					trackBarRed.Value,
					trackBarGreen.Value,
					trackBarBlue.Value );
			}
			set
			{
				if( value != Color.FromArgb(
					trackBarRed.Value,
					trackBarGreen.Value,
					trackBarBlue.Value ) )
				{
					trackBarRed.Value   = value.R;
					trackBarBlue.Value  = value.B;
					trackBarGreen.Value = value.G;

					FireColorChangedEvent();
				}
			}
		}

		/// <summary>
		/// Design time support to reset the Color property to it's default value.
		/// </summary>
		public void ResetColor()
		{
			Color = defaultColor;
		}

		/// <summary>
		/// Design time support to indicate whether the Color property should be serialized.
		/// </summary>
		/// <returns></returns>
		public bool ShouldSerializeColor()
		{
			return Color != defaultColor;
		}

		/// <summary>
		/// Enable/disable smooth scrolling of the z-axis of the color cube.
		/// You may want to disable this on slower systems since continuously recomputing
		/// the color palette is fairly processor intensive.
		/// </summary>
		[Browsable(true), Category("ColorPanel")]
		[Description("Enable/disable continuous z-axis color."), DefaultValue(defaultContinuousScroll)]
		public bool EnableContinuousScrollZ
		{
			get
			{
				return bContinuousScrollZAxis;
			}
			set
			{
				bContinuousScrollZAxis = value;
			}
		}

		/// <summary>
		/// Overrides OnEnabledChanged so the control can redraw itself 
		/// enabled/disabled.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);

			trackBarBlue.Enabled       = Enabled;
			trackBarRed.Enabled        = Enabled;
			trackBarGreen.Enabled      = Enabled;

			numericUpDownBlue.Enabled  = Enabled;
			numericUpDownRed.Enabled   = Enabled;
			numericUpDownGreen.Enabled = Enabled;

			radioButtonBlue.Enabled    = Enabled;
			radioButtonRed.Enabled     = Enabled;
			radioButtonGreen.Enabled   = Enabled;

			pictureBox.Enabled = Enabled;

			Refresh();
		}

		/// <summary>
		/// Overrides OnLoad.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			MakePalette();
			Refresh();

			base.OnLoad(e);
		}

		private void trackBarBrightness_ValueChanged(object sender, EventArgs e)
		{
			/*
			double y = Convert.ToDouble(this.trackBarBrightness.Value)/255;
			int r = Convert.ToInt32((y*255.0)*.299);
			int g = Convert.ToInt32((y*255.0)*.587);
			int b = Convert.ToInt32((y*255.0)*.114);
			this.numericUpDownRed.Value = r;
			this.numericUpDownGreen.Value = g;
			this.numericUpDownBlue.Value = b;
			this.trackBarRed.Value = r;
			this.trackBarGreen.Value = g;
			this.trackBarBlue.Value = b;
			*/
		}
	}
}
