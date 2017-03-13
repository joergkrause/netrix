using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Web.UI.WebControls;

namespace GuruComponents.Netrix.UserInterface
{
	/// <summary>
	/// This class creates a simple unit editor to allow users selecting either pixel or percentage
	/// values from within a given range.
	/// </summary>
	/// <remarks>
	/// This improves the PropertyGrid usability.
	/// </remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(ResourceManager), "ToolBox.UnitEditor.ico")]
    [DefaultEvent("ValueChanged")]
    [Designer(typeof(GuruComponents.Netrix.UserInterface.NetrixUIDesigner))]
    public class UnitEditor : System.Windows.Forms.UserControl
	{
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Panel panel1;
        private ComboBox comboBox1;
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <remarks>
        /// Requests internally the localized information from satellite assemblies.
        /// </remarks>
		public UnitEditor()
		{
			InitializeComponent();
			SetUp();
		}

        internal UnitEditor(bool @internal)
        {
            InitializeComponent();
            SetUp();
        }

		private void SetUp()
		{
            // TODO: Extend Resources
		}

        /// <summary>
        /// Reset the UI resources after culture changes.
        /// </summary>
        public void ResetUI()
        {
            SetUp();
        }

        /// <summary>
        /// The current Assembly version.
        /// </summary>                   
        /// <remarks>
        /// This property returns the current assembly version to inform during design session about the assembly loaded.
        /// </remarks>
        [Browsable(true), Category("NetRix"), Description("Current Version. ReadOnly.")]
        public string Version
        {
            get
            {
                return this.GetType().Assembly.GetName().Version.ToString();
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

		#region Vom Komponenten-Designer generierter Code
		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Location = new System.Drawing.Point(0, 2);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(83, 16);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.numericUpDown1.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(177, 23);
            this.panel1.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "-",
            "%",
            "px",
            "cm",
            "mm",
            "pt",
            "em",
            "ex",
            "inch",
            "pica"});
            this.comboBox1.Location = new System.Drawing.Point(83, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(93, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // UnitEditor
            // 
            this.Controls.Add(this.panel1);
            this.Name = "UnitEditor";
            this.Size = new System.Drawing.Size(178, 23);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion


        /// <summary>
        /// Controls the resizing behavior.
        /// </summary>
        /// <remarks>
        /// This control will regularly divide the elements on the final width.
        /// </remarks>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {            
            int w = this.Width = Math.Max(175, this.Width);
            int h = this.Height = Math.Max(22, this.Height);
            this.panel1.Height = h;
            this.panel1.Width = w;
            this.numericUpDown1.Height = h;
            this.numericUpDown1.Top = Math.Max((this.panel1.BorderStyle == System.Windows.Forms.BorderStyle.None) ? 5 : 1, h/2 - 12);
            this.numericUpDown1.Width = this.Width / 2;   
            this.comboBox1.Width = this.Width / 2;
            this.comboBox1.Left = numericUpDown1.Right + 1;
            base.OnResize (e);
        }

        /// <summary>
        /// Gets or sets the border style.
        /// </summary>
        /// <remarks>
        /// This allows the developer to set a common border style around the whole control.
        /// The subcontrols will change their behavior accordingly to the outer border.
        /// </remarks>
        [Browsable(true), Category("NetRix"), Description("Gets or sets the border style.")]
        public System.Windows.Forms.BorderStyle Border
        {
            get
            {
                return this.panel1.BorderStyle;
            }
            set
            {
                this.panel1.BorderStyle = value;
                switch (value)
                {
                    case System.Windows.Forms.BorderStyle.Fixed3D:
                        this.numericUpDown1.Top = 1;
                        this.comboBox1.FlatStyle = FlatStyle.System;
                        break;
                    case System.Windows.Forms.BorderStyle.FixedSingle:
                        this.numericUpDown1.Top = 1;
                        this.comboBox1.FlatStyle = FlatStyle.Popup;
                        break;
                    case System.Windows.Forms.BorderStyle.None:
                        this.numericUpDown1.Top = 3;
                        this.comboBox1.FlatStyle = FlatStyle.Flat;
                        break;
                }
                this.numericUpDown1.BorderStyle = value;
            }
        }

        /// <summary>
        /// The unit displayed in the control.
        /// </summary>
        /// <remarks>
        /// Gets or sets the value within the control.
        /// </remarks>
        [Browsable(true), Category("NetRix"), Description("The unit displayed in the control.")]
		public System.Web.UI.WebControls.Unit Unit
		{
            get
            {
                if (comboBox1.SelectedIndex == -1) return Unit.Empty;
                switch (comboBox1.SelectedItem.ToString())
                {
                    default:
                    case "-":
                        return Unit.Empty;
                    case "%":
                        return Unit.Percentage((double)this.numericUpDown1.Value);
                    case "px":
                        return Unit.Pixel((int)this.numericUpDown1.Value);
                    case "mm":
                        return new Unit((double)this.numericUpDown1.Value, UnitType.Mm);
                    case "cm":
                        return new Unit((double)this.numericUpDown1.Value, UnitType.Cm);
                    case "pt":
                        return new Unit((double)this.numericUpDown1.Value, UnitType.Point);
                    case "em":
                        return new Unit((double)this.numericUpDown1.Value, UnitType.Em);
                    case "ex":
                        return new Unit((double)this.numericUpDown1.Value, UnitType.Ex);
                    case "inch":
                        return new Unit((double)this.numericUpDown1.Value, UnitType.Inch);
                    case "pica":
                        return new Unit((double)this.numericUpDown1.Value, UnitType.Pica);
                }
            }
			set
			{
                if (value.Equals(Unit.Empty))
                {
                    numericUpDown1.Enabled = false;
                    comboBox1.SelectedItem = "-";
                }
                else
                {
                    switch (value.Type)
                    {
                        case UnitType.Percentage:
                            comboBox1.SelectedItem = "%";
                            numericUpDown1.DecimalPlaces = 0;
                            break;
                        case UnitType.Pixel:
                            comboBox1.SelectedItem = "px";
                            numericUpDown1.DecimalPlaces = 0;
                            break;
                        case UnitType.Mm:
                            comboBox1.SelectedItem = "mm";
                            numericUpDown1.DecimalPlaces = 2;
                            break;
                        case UnitType.Cm:
                            comboBox1.SelectedItem = "cm";
                            numericUpDown1.DecimalPlaces = 2;
                            break;
                        case UnitType.Point:
                            comboBox1.SelectedItem = "pt";
                            numericUpDown1.DecimalPlaces = 2;
                            break;
                        case UnitType.Em:
                            comboBox1.SelectedItem = "em";
                            numericUpDown1.DecimalPlaces = 2;
                            break;
                        case UnitType.Ex:
                            comboBox1.SelectedItem = "ex";
                            numericUpDown1.DecimalPlaces = 2;
                            break;
                        case UnitType.Inch:
                            comboBox1.SelectedItem = "inch";
                            numericUpDown1.DecimalPlaces = 2;
                            break;
                        case UnitType.Pica:
                            comboBox1.SelectedItem = "pica";
                            numericUpDown1.DecimalPlaces = 2;
                            break;
                    }
                    numericUpDown1.Value = Convert.ToDecimal(value.Value);
                }
			}
		}

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "-":
                    numericUpDown1.Enabled = false;
                    break;
                case "%":
                    numericUpDown1.Enabled = true;
                    numericUpDown1.DecimalPlaces = 0;
                    break;
                case "px":
                    numericUpDown1.Enabled = true;
                    numericUpDown1.DecimalPlaces = 0;
                    break;
                case "mm":
                    numericUpDown1.Enabled = true;
                    numericUpDown1.DecimalPlaces = 2;
                    break;
                case "cm":
                    numericUpDown1.Enabled = true;
                    numericUpDown1.DecimalPlaces = 2;
                    break;
                case "pt":
                    numericUpDown1.Enabled = true;
                    numericUpDown1.DecimalPlaces = 2;
                    break;
                case "em":
                    numericUpDown1.Enabled = true;
                    numericUpDown1.DecimalPlaces = 2;
                    break;
                case "ex":
                    numericUpDown1.Enabled = true;
                    numericUpDown1.DecimalPlaces = 2;
                    break;
                case "inch":
                    numericUpDown1.Enabled = true;
                    numericUpDown1.DecimalPlaces = 2;
                    break;
                case "pica":
                    numericUpDown1.Enabled = true;
                    numericUpDown1.DecimalPlaces = 2;
                    break;
            }
            OnValueChanged(sender);
        }

        /// <summary>
        /// Fired if either the numeric value or the unit type has been changed.
        /// </summary>
        [Category("NetRix Events")]
        public event EventHandler ValueChanged;

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            OnValueChanged(sender);
        }

        /// <summary>
        /// Called if either the numeric value or the unit type has been changed.
        /// </summary>
        /// <param name="sender"></param>
        protected void OnValueChanged(object sender)
        {
            if (ValueChanged != null)
            {
                ValueChanged(sender, EventArgs.Empty);
            }
        }

	}
}
