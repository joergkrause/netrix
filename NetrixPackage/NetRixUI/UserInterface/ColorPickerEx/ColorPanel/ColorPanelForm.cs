using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{
	/// <summary>
	/// ColorPanelForm is used to host an instance of ColorPanelUserControl.
	/// </summary>
	/// <remarks>
	/// This enable the ColorPanel control to be dropped down by the
	/// ColorPicker.
	/// </remarks>
    [ToolboxItem(false)]
    public class ColorPanelForm : System.Windows.Forms.Form
    {

        private GuruComponents.Netrix.UserInterface.ColorPicker.ColorPanelUserControl colorPanelUserControl1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// Creates a new instance of color panel form.
        /// </summary>
        public ColorPanelForm()
        {
            InitializeComponent();
        }

        internal void SetUp()
        {
            this.colorPanelUserControl1.SetUp();            
        }
        

        /// <summary>
        /// Set whether the 'custom' button is visible.
        /// </summary>
        public bool ButtonCustomVisible
        {
            set
            {
                colorPanelUserControl1.ButtonCustomVisible = value;
            }
        }

        /// <summary>
        /// Set whether the 'web' button is visible.
        /// </summary>
        public bool ButtonWebVisible
        {
            set
            {
                colorPanelUserControl1.ButtonWebVisible= value;
            }
        }

        /// <summary>
        /// Set whether the 'system' button is visible.
        /// </summary>
        public bool ButtonSystemVisible
        {
            set
            {
                colorPanelUserControl1.ButtonSystemVisible= value;
            }
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
            colorPanelUserControl1 = new ColorPanelUserControl();
            colorPanelUserControl1.Dock = DockStyle.Fill;
            colorPanelUserControl1.Name = "colorPanelUserControl1";
            colorPanelUserControl1.ColorChanged += new ColorChangedEventHandler(colorPanelUserControl1_ColorChanged);
            colorPanelUserControl1.ColorCancel += new ColorCancelEventHandler(colorPanelUserControl1_ColorCancel);
            colorPanelUserControl1.KeyDown += new KeyEventHandler(colorPanelUserControl1_KeyDown);
            // 
            // ColorPanelForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(345, 172);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorPanelForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "0";
            this.Text = "Color Selection Box";
            this.KeyDown += new KeyEventHandler(ColorPanelForm_KeyDown);
            this.Controls.Add(colorPanelUserControl1);
        }
        #endregion

        /// <summary>
        /// Number of columns of the color grid.
        /// </summary>
        public int Columns
        {
            get
            {
                return colorPanelUserControl1.Columns;
            }
            set
            {
                colorPanelUserControl1.Columns = value;
            }
        }

        /// <summary>
        /// List of custom colors.
        /// </summary>
        public List<Color> CustomColors
        {
            get
            {
                return colorPanelUserControl1.CustomColors;
            }
            set
            {
                colorPanelUserControl1.CustomColors = value;
            }
        }

        /// <summary>
        /// Current color.
        /// </summary>
        public System.Drawing.Color Color
        {
            get
            {
                return colorPanelUserControl1.Color;
            }
            set
            {
                colorPanelUserControl1.Color = value;
            }
        }

        private void colorPanelUserControl1_ColorChanged(object sender, ColorChangedEventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void colorPanelUserControl1_ColorCancel(object sender, ColorChangedEventArgs e)
        {
            DialogResult = DialogResult.OK;
            colorPanelUserControl1.Color = Color.Empty;
            colorPanelUserControl1.ResetControl();
            this.Close();
        }

        private void ColorPanelForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    this.Close();
                    break;
            }
        }

        private void colorPanelUserControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ColorPanelForm_KeyDown(sender, e);
            }
        }
    }
}