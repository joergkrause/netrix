using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{
	/// <summary>
	/// ColorPanelUserControl is used to host an instance of ColorPanel together with various options.
	/// </summary>	
	/// <remarks>
	/// This is the base control which is placed into the 
	/// <see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorPanelForm">ColorPanelForm</see>
	/// form to be dropped from the 
	/// <see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl">ColorPickerUserControl</see>.
	/// Normally this control should not be placed as a standalone user control onto a form. It is public to
	/// be used if a replacement of the <see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorPanelForm">ColorPanelForm</see> form
	/// is requiered.
	/// <para>
	/// This control uses the satellite assemblies to localize the labels. For that reason, the control
	/// should not be used without the satellites for all languages used in the final application. Otherwise
	/// a replacement string is used as the embedded default resource.
	/// </para>
	/// </remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(GuruComponents.Netrix.UserInterface.ResourceManager), "ToolBox.ColorEditor.ico")]
    [Designer(typeof(GuruComponents.Netrix.UserInterface.NetrixUIDesigner))]
    public class ColorPanelUserControl : System.Windows.Forms.UserControl
	{
		private ColorPanel colorPanel = null;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioButtonNo;
		private System.Windows.Forms.RadioButton radioButtonSaturation;
		private System.Windows.Forms.RadioButton radioButtonDistance;
		private System.Windows.Forms.RadioButton radioButtonName;
		private System.Windows.Forms.RadioButton radioButtonBrightness;
		private System.Windows.Forms.RadioButton radioButtonCont;
        private System.Windows.Forms.Button buttonWeb;
        private System.Windows.Forms.Button buttonCustom;
        private System.Windows.Forms.Button buttonSystem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonNoColor;
        private System.Windows.Forms.PictureBox pictureBox1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// The contructor which instantiate the control.
        /// </summary>
        /// <remarks>
        /// Build the control using this constructor when used on a form.
        /// </remarks>
		public ColorPanelUserControl()
		{
			InitializeComponent();
			SetUp();
			this.buttonWeb.BackColor = SystemColors.Control;
            this.buttonWeb.Focus();
		}

        private static Stream s = typeof(ColorPanelUserControl).Assembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.NoColor.ico");

        internal void SetUp()
        {
            this.radioButtonNo.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.radioButtonNo.Text"); //"Standard";
            this.radioButtonCont.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.radioButtonCont.Text"); //"Continued";
            this.groupBox2.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.groupBox2.Text"); //"Sort Options";
            this.radioButtonBrightness.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.radioButtonBrightness.Text"); //"Brightness";
            this.radioButtonName.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.radioButtonName.Text"); //"Name";
            this.radioButtonDistance.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.radioButtonDistance.Text"); //"Distance";
            this.radioButtonSaturation.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.radioButtonSaturation.Text"); //"Saturation";
            this.buttonWeb.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.buttonWeb.Text"); //"Web";
            this.buttonCustom.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.buttonCustom.Text"); //"Custom";
            this.buttonSystem.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.buttonSystem.Text"); //"System";
            this.label1.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.label1.Text"); //"Hit ESC to cancel";
            this.buttonNoColor.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("ColorPanelUserControl.buttonNoColor.Text"); //"No Color";
                this.buttonNoColor.Image = Image.FromStream(s);
                this.pictureBox1.Image = Image.FromStream(s);
             
            ArrangeButtons();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ColorPanelUserControl));
            this.colorPanel = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPanel();
            this.radioButtonNo = new System.Windows.Forms.RadioButton();
            this.radioButtonCont = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonBrightness = new System.Windows.Forms.RadioButton();
            this.radioButtonName = new System.Windows.Forms.RadioButton();
            this.radioButtonDistance = new System.Windows.Forms.RadioButton();
            this.radioButtonSaturation = new System.Windows.Forms.RadioButton();
            this.buttonWeb = new System.Windows.Forms.Button();
            this.buttonCustom = new System.Windows.Forms.Button();
            this.buttonSystem = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonNoColor = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // colorPanel
            // 
            this.colorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.colorPanel.ColorWellSize = new System.Drawing.Size(12, 12);
            this.colorPanel.Location = new System.Drawing.Point(0, 0);
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.Size = new System.Drawing.Size(220, 148);
            this.colorPanel.TabIndex = 13;
            this.colorPanel.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.colorPanel_ColorChanged);
            // 
            // radioButtonNo
            // 
            this.radioButtonNo.Checked = true;
            this.radioButtonNo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonNo.Location = new System.Drawing.Point(8, 18);
            this.radioButtonNo.Name = "radioButtonNo";
            this.radioButtonNo.Size = new System.Drawing.Size(111, 16);
            this.radioButtonNo.TabIndex = 4;
            this.radioButtonNo.TabStop = true;
            this.radioButtonNo.Tag = "No";
            this.radioButtonNo.Text = "Standard";
            this.radioButtonNo.CheckedChanged += new System.EventHandler(this.radioButtonNo_CheckedChanged);
            // 
            // radioButtonCont
            // 
            this.radioButtonCont.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonCont.Location = new System.Drawing.Point(8, 34);
            this.radioButtonCont.Name = "radioButtonCont";
            this.radioButtonCont.Size = new System.Drawing.Size(111, 16);
            this.radioButtonCont.TabIndex = 5;
            this.radioButtonCont.Tag = "continues";
            this.radioButtonCont.Text = "Continued";
            this.radioButtonCont.CheckedChanged += new System.EventHandler(this.radioButtonNo_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.radioButtonBrightness);
            this.groupBox2.Controls.Add(this.radioButtonName);
            this.groupBox2.Controls.Add(this.radioButtonDistance);
            this.groupBox2.Controls.Add(this.radioButtonSaturation);
            this.groupBox2.Controls.Add(this.radioButtonCont);
            this.groupBox2.Controls.Add(this.radioButtonNo);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(224, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(127, 121);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sort Options";
            // 
            // radioButtonBrightness
            // 
            this.radioButtonBrightness.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonBrightness.Location = new System.Drawing.Point(8, 98);
            this.radioButtonBrightness.Name = "radioButtonBrightness";
            this.radioButtonBrightness.Size = new System.Drawing.Size(111, 16);
            this.radioButtonBrightness.TabIndex = 9;
            this.radioButtonBrightness.Tag = "Brightness";
            this.radioButtonBrightness.Text = "Brightness";
            this.radioButtonBrightness.CheckedChanged += new System.EventHandler(this.radioButtonNo_CheckedChanged);
            // 
            // radioButtonName
            // 
            this.radioButtonName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonName.Location = new System.Drawing.Point(8, 82);
            this.radioButtonName.Name = "radioButtonName";
            this.radioButtonName.Size = new System.Drawing.Size(111, 16);
            this.radioButtonName.TabIndex = 8;
            this.radioButtonName.Tag = "Name";
            this.radioButtonName.Text = "Name";
            this.radioButtonName.CheckedChanged += new System.EventHandler(this.radioButtonNo_CheckedChanged);
            // 
            // radioButtonDistance
            // 
            this.radioButtonDistance.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonDistance.Location = new System.Drawing.Point(8, 66);
            this.radioButtonDistance.Name = "radioButtonDistance";
            this.radioButtonDistance.Size = new System.Drawing.Size(111, 16);
            this.radioButtonDistance.TabIndex = 7;
            this.radioButtonDistance.Tag = "Distance";
            this.radioButtonDistance.Text = "Distance";
            this.radioButtonDistance.CheckedChanged += new System.EventHandler(this.radioButtonNo_CheckedChanged);
            // 
            // radioButtonSaturation
            // 
            this.radioButtonSaturation.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonSaturation.Location = new System.Drawing.Point(8, 50);
            this.radioButtonSaturation.Name = "radioButtonSaturation";
            this.radioButtonSaturation.Size = new System.Drawing.Size(111, 16);
            this.radioButtonSaturation.TabIndex = 6;
            this.radioButtonSaturation.Tag = "Saturation";
            this.radioButtonSaturation.Text = "Saturation";
            this.radioButtonSaturation.CheckedChanged += new System.EventHandler(this.radioButtonNo_CheckedChanged);
            // 
            // buttonWeb
            // 
            this.buttonWeb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonWeb.BackColor = System.Drawing.SystemColors.ControlDark;
            this.buttonWeb.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonWeb.Location = new System.Drawing.Point(1, 146);
            this.buttonWeb.Name = "buttonWeb";
            this.buttonWeb.Size = new System.Drawing.Size(72, 22);
            this.buttonWeb.TabIndex = 8;
            this.buttonWeb.Tag = "0";
            this.buttonWeb.Text = "Web";
            this.buttonWeb.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonCustom
            // 
            this.buttonCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCustom.BackColor = System.Drawing.SystemColors.ControlDark;
            this.buttonCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCustom.Location = new System.Drawing.Point(74, 146);
            this.buttonCustom.Name = "buttonCustom";
            this.buttonCustom.Size = new System.Drawing.Size(72, 22);
            this.buttonCustom.TabIndex = 9;
            this.buttonCustom.Tag = "1";
            this.buttonCustom.Text = "Custom";
            this.buttonCustom.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonSystem
            // 
            this.buttonSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSystem.BackColor = System.Drawing.SystemColors.ControlDark;
            this.buttonSystem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonSystem.Location = new System.Drawing.Point(149, 146);
            this.buttonSystem.Name = "buttonSystem";
            this.buttonSystem.Size = new System.Drawing.Size(72, 22);
            this.buttonSystem.TabIndex = 10;
            this.buttonSystem.Tag = "2";
            this.buttonSystem.Text = "System";
            this.buttonSystem.Click += new System.EventHandler(this.button_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Info;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label1.Location = new System.Drawing.Point(224, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Hit ESC to cancel";
            // 
            // buttonNoColor
            // 
            this.buttonNoColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNoColor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            
            this.buttonNoColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonNoColor.Location = new System.Drawing.Point(224, 0);
            this.buttonNoColor.Name = "buttonNoColor";
            this.buttonNoColor.Size = new System.Drawing.Size(128, 23);
            this.buttonNoColor.TabIndex = 12;
            this.buttonNoColor.Text = "No Color";
            this.buttonNoColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonNoColor.Click += new System.EventHandler(this.buttonNoColor_Click);
            // 
            // pictureBox1
            //             
            this.pictureBox1.Location = new System.Drawing.Point(313, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 15);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // ColorPanelUserControl
            // 
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.colorPanel);
            this.Controls.Add(this.buttonNoColor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonSystem);
            this.Controls.Add(this.buttonCustom);
            this.Controls.Add(this.buttonWeb);
            this.Name = "ColorPanelUserControl";
            this.Size = new System.Drawing.Size(357, 170);
            this.Tag = "0";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ColorPanelForm_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        /// <summary>
        /// This method internally sets the names of buttons
        /// </summary>
        /// <param name="standard"></param>
        /// <param name="cont"></param>
        /// <param name="saturation"></param>
        /// <param name="bright"></param>
        /// <param name="name"></param>
        /// <param name="distance"></param>
        internal void SetColorSortNames(string standard, string cont, string saturation, string bright, string name, string distance)
        {
            this.radioButtonBrightness.Text = bright;
            this.radioButtonCont.Text = cont;
            this.radioButtonDistance.Text = distance;
            this.radioButtonNo.Text = standard;
            this.radioButtonSaturation.Text = saturation;
            this.radioButtonName.Text = name;
        }
        
        /// <summary>
        /// Sets the control in an undefined state.
        /// </summary>
        /// <remarks>
        /// This is used to inform other controls to not rewrite the value in the control.
        /// </remarks>
        public void ResetControl()
        {
            ResetControl(new List<Color>());
            SetUp();
        }

        /// <summary>
        /// Sets the control in an undefined state.
        /// </summary>
        /// <remarks>
        /// This is used to inform other controls
        /// to not rewrite the value in the control. Additionally the custom color table 
        /// is filled with a array of colors.
        /// </remarks>
        /// <param name="customiser">Array of colors to customize the palette.</param>
        public void ResetControl(List<Color> customiser)
        {
            this.CustomColors = customiser;
        }

        private void colorPanel_ColorChanged(object sender, ColorChangedEventArgs e)
		{
			this.Color = this.colorPanel.Color;
			if (ColorChanged != null)
			{
				ColorChanged(sender, e);
			}
		}

		private void ColorPanelForm_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				e.Handled = true;
				this.colorPanel.ResetColor();
				return;
			}
			if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				return;
			}
		}

		private void radioButtonNo_CheckedChanged(object sender, System.EventArgs e)
		{
			RadioButton r = (RadioButton) sender;
			switch (r.Tag.ToString())
			{
				case "No":
					if (this.buttonWeb.BackColor == SystemColors.Control)
					{
						this.colorPanel.ColorSet = GuruComponents.Netrix.UserInterface.ColorPicker.ColorSet.Web;
					}
					this.colorPanel.ColorSortOrder = GuruComponents.Netrix.UserInterface.ColorPicker.ColorSortOrder.Unsorted;
					break;
				case "continues":
					this.colorPanel.ColorSet = GuruComponents.Netrix.UserInterface.ColorPicker.ColorSet.Continues;
					this.buttonWeb.BackColor = SystemColors.Control;
					this.colorPanel.ColorSortOrder = GuruComponents.Netrix.UserInterface.ColorPicker.ColorSortOrder.Unsorted;
					break;
				case "Saturation":
					this.colorPanel.ColorSortOrder = GuruComponents.Netrix.UserInterface.ColorPicker.ColorSortOrder.Saturation;
					break;
				case "Distance":
					this.colorPanel.ColorSortOrder = GuruComponents.Netrix.UserInterface.ColorPicker.ColorSortOrder.Distance;
					break;
				case "Brightness":
					this.colorPanel.ColorSortOrder = GuruComponents.Netrix.UserInterface.ColorPicker.ColorSortOrder.Brightness;
					break;
				case "Name":
					this.colorPanel.ColorSortOrder = GuruComponents.Netrix.UserInterface.ColorPicker.ColorSortOrder.Name;
					break;
			}
		}

        private void button_Click(object sender, System.EventArgs e)
        {
            Button b = (Button) sender;
            this.buttonWeb.BackColor = SystemColors.ControlDark;
            this.buttonCustom.BackColor = SystemColors.ControlDark;
            this.buttonSystem.BackColor = SystemColors.ControlDark;
            switch (b.Tag.ToString())
            {
                case "0":
                    this.buttonWeb.BackColor = SystemColors.Control;
                    if (this.radioButtonCont.Checked)
                    {
                        this.colorPanel.ColorSet = ColorSet.Continues;
                    } 
                    else 
                    {
                        this.colorPanel.ColorSet = ColorSet.Web;
                    }
                    this.radioButtonCont.Enabled = true;
                    break;
                case "1":
                    this.buttonCustom.BackColor = SystemColors.Control;
                    this.colorPanel.ColorSet = ColorSet.Custom;
                    //this.radioButtonNo.Checked = true;
                    this.radioButtonCont.Enabled = false;
                    break;
                case "2":
                    this.buttonSystem.BackColor = SystemColors.Control;
                    this.colorPanel.ColorSet = ColorSet.System;
                    //this.radioButtonNo.Checked = true;
                    this.radioButtonCont.Enabled = false;
                    break;
            }        
        }


		/// <summary>
		/// Gets or sets the Border of the panel.
		/// </summary>
		/// <remarks>
		/// The value of type <see cref="System.Windows.Forms.BorderStyle"/> controls the style around
		/// the embedded color panel, not of the whole control.
		/// </remarks>
		public System.Windows.Forms.BorderStyle PanelBorderStyle
		{
			get
			{
				return colorPanel.BorderStyle;
			}
			set
			{
				colorPanel.BorderStyle = value;
			}
		}

		/// <summary>
		/// The current selected color.
		/// </summary>
		/// <remarks>
		/// Gets or sets the color which becomes selected if found on the currently opened panel.
		/// </remarks>
        public System.Drawing.Color Color
		{
			get
			{
				return colorPanel.Color;
			}
			set
			{
				colorPanel.Color = value;
			}
		}

        /// <summary>
        /// Gets or sets the color panel.
        /// </summary>
        /// <remarks>
        /// This controls the color display and switches the buttons accordingly the current setting.
        /// The possible values are:
        /// <list type="bullet">
        /// <item>Web</item>
        /// <item>System</item>
        /// <item>Custom</item>
        /// </list>
        /// </remarks>
		public ColorSet ColorSet
		{
			get
			{
				return colorPanel.ColorSet;
			}
			set
			{
				colorPanel.ColorSet = value;
                switch (value)
                {
                    case ColorSet.Custom:
                        this.buttonCustom.BackColor = SystemColors.Control;
                        this.buttonSystem.BackColor = SystemColors.ControlDark;
                        this.buttonWeb.BackColor = SystemColors.ControlDark;                        
                        break;
                    case ColorSet.System:
                        this.buttonCustom.BackColor = SystemColors.ControlDark;
                        this.buttonSystem.BackColor = SystemColors.Control;
                        this.buttonWeb.BackColor = SystemColors.ControlDark;
                        break;
                    case ColorSet.Web:
                        this.buttonCustom.BackColor = SystemColors.ControlDark;
                        this.buttonSystem.BackColor = SystemColors.ControlDark;
                        this.buttonWeb.BackColor = SystemColors.Control;
                        break;
                }
			}
		}

        /// <summary>
        /// The size of one colored square in pixels.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
		public System.Drawing.Size ColorWellSize
		{
			get
			{
				return colorPanel.ColorWellSize;
			}
			set
			{
				colorPanel.ColorWellSize = value;
			}
		}

        /// <summary>
        /// Gets or sets the color sort technique.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
		public ColorSortOrder ColorSortOrder
		{
			get
			{
				return colorPanel.ColorSortOrder;
			}
			set
			{
				colorPanel.ColorSortOrder = value;
			}
		}

        /// <summary>
        /// Gets or sets the number of columns displayed.
        /// </summary>
        /// <remarks>
        /// This value should
        /// not be changed because the sorted color patterns does not work
        /// with different column numbers.
        /// </remarks>
		public int Columns
		{
			get
			{
				return colorPanel.Columns;
			}
			set
			{
				colorPanel.Columns = value;
			}
		}

        /// <summary>
        /// Gets or sets the custom colors.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public List<Color> CustomColors
		{
			get
			{
				return colorPanel.CustomColors;
			}
			set
			{
				colorPanel.CustomColors = value;
			}
		}

        /// <summary>
        /// Gets or sets the current color.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        public System.Drawing.Color CurrentColor
        {
            get
            {
                return colorPanel.Color;
            }
            set
            {
                colorPanel.Color = value;
            }
        }

		/// <summary>
		/// Fired if the color has changed.
		/// </summary>
        public event ColorChangedEventHandler ColorChanged;

		/// <summary>
		/// Event informs the caller that the user will reset the color.
		/// </summary>
		public event ColorCancelEventHandler ColorCancel;

        private void colorPanel_Resize(object sender, System.EventArgs e)
        {
            // make the form the same size as the panel
            colorPanel.Top  = 0;
            colorPanel.Left = 0;

            this.Width = colorPanel.Width;
            this.Height = colorPanel.Height;
        }

        private void colorPanel_PanelClosing(object sender, EventArgs e)
        {
        }

        private void buttonNoColor_Click(object sender, System.EventArgs e)
        {
            colorPanel.Color = Color.Empty;
            this.Color = Color.Empty;
            if (ColorCancel != null)
            {
                ColorCancel(this, new ColorChangedEventArgs(Color.Empty));
            }
        }

        /// <summary>
        /// Controls visiblity of "Web" button.
        /// </summary>
        public bool ButtonWebVisible
        {
            set
            {
                this.buttonWeb.Visible = value;
                ArrangeButtons();
            }
        }
        /// <summary>
        /// Controls visiblity of "Custom" button.
        /// </summary>
        public bool ButtonCustomVisible
        {
            set
            {
                this.buttonCustom.Visible = value;
                ArrangeButtons();
            }
        }
        /// <summary>
        /// Controls visiblity of "System" button.
        /// </summary>
        public bool ButtonSystemVisible
        {
            set
            {
                this.buttonSystem.Visible = value;
                ArrangeButtons();
            }
        }

        private void ArrangeButtons()
        {
            int mode = (this.buttonWeb.Visible ? 1 : 0) + (this.buttonCustom.Visible ? 2 : 0) + (this.buttonSystem.Visible ? 4 : 0);
            this.buttonWeb.Enabled  = true;
            this.buttonCustom.Enabled  = true;
            this.buttonSystem.Enabled  = true;
            switch (mode)
            {
                case 0:
                    break;
                case 1:
                    this.buttonWeb.Width    = colorPanel.Width - 2;
                    this.buttonWeb.Left     = 2;
                    this.buttonWeb.Enabled = false;
                    break;
                case 2:
                    this.buttonCustom.Width = colorPanel.Width - 2;
                    this.buttonCustom.Left  = 2;
                    this.buttonCustom.Enabled = false;
                    break;
                case 3:
                    this.buttonWeb.Width    = colorPanel.Width / 2 - 2;
                    this.buttonWeb.Left     = 2;
                    this.buttonCustom.Width = colorPanel.Width / 2 - 2;
                    this.buttonCustom.Left  = this.buttonWeb.Right + 2;
                    break;
                case 4:
                    this.buttonSystem.Width = colorPanel.Width - 2;
                    this.buttonSystem.Left  = 2;
                    this.buttonSystem.Enabled = false;
                    break;
                case 5:
                    this.buttonWeb.Width    = colorPanel.Width / 2 - 2;
                    this.buttonWeb.Left     = 2;
                    this.buttonSystem.Width = colorPanel.Width / 2 - 2;
                    this.buttonSystem.Left  = this.buttonWeb.Right + 2;
                    break;
                case 6:
                    this.buttonCustom.Width = colorPanel.Width / 2 - 2;
                    this.buttonCustom.Left  = 2;
                    this.buttonSystem.Width = colorPanel.Width / 2 - 2;
                    this.buttonSystem.Left  = this.buttonCustom.Right + 2;
                    break;
                case 7:
                    this.buttonWeb.Width    = colorPanel.Width / 3 - 2;
                    this.buttonWeb.Left     = 2;
                    this.buttonCustom.Width = colorPanel.Width / 3 - 2;
                    this.buttonCustom.Left  = buttonWeb.Right + 2;
                    this.buttonSystem.Width = colorPanel.Width / 3 - 2;
                    this.buttonSystem.Left  = buttonCustom.Right + 2; 
                    break;
            }
        }

    }
}