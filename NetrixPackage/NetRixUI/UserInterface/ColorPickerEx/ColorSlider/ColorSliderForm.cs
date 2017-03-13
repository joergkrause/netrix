using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{
	/// <summary>
	/// The class for the ColorSlider.
	/// </summary>
	/// <remarks>
	/// The slider can select any value from the RGB color space. Display devices can only 
	/// display a limited number of colors from that space.
	/// </remarks>
	[ToolboxItem(false)]
	public class ColorSliderForm : Form
	{
		private ColorSlider colorSlider;
		private Button buttonCancel;
		private Button buttonOK;
		private Panel panel1;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private Container components = null;

        /// <summary>
        /// The constructor initializes a new instance of the slider.
        /// </summary>
        /// <remarks>
        /// The slider is use internally as a replacement for the color grid. It can be used 
        /// externally but is needs some infrastructure around to make sense.
        /// </remarks>
		public ColorSliderForm()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Gets or sets the currently selected color.
        /// </summary>
		public Color Color
		{
			set
			{
				this.colorSlider.Color = value;
			}
			get
			{
				return this.colorSlider.Color;
			}
		}

        /// <summary>
        /// Called on deactivation of the control.
        /// </summary>
        /// <remarks>
        /// Used to set the return value to a definitive state.
        /// </remarks>
        /// <param name="e">The event arguments beeing sent from the caller.</param>
		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);

			if( this.DialogResult != DialogResult.OK )
			{
				this.DialogResult = DialogResult.Cancel;
			}
		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.colorSlider = new ColorSlider();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// colorSlider
			// 
			this.colorSlider.Location = new System.Drawing.Point(-32, 0);
			this.colorSlider.Name = "colorSlider";
			this.colorSlider.Size = new System.Drawing.Size(428, 256);
			this.colorSlider.TabIndex = 0;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonCancel.Location = new System.Drawing.Point(264, 232);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(57, 22);
			this.buttonCancel.TabIndex = 16;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.buttonOK.Location = new System.Drawing.Point(335, 232);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(57, 22);
			this.buttonOK.TabIndex = 15;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.buttonOK,
																				 this.buttonCancel,
																				 this.colorSlider});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(399, 258);
			this.panel1.TabIndex = 17;
			// 
			// ColorSliderForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleMode = AutoScaleMode.None;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(399, 258);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panel1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(405, 282);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(405, 282);
			this.Name = "ColorSliderForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Color Slider Box";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Color = this.colorSlider.Color;
			this.Close();
		}
	}
}
