using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using BorderStyle=System.Windows.Forms.BorderStyle;
using Panel=System.Windows.Forms.Panel;

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
    [ToolboxBitmap(typeof(GuruComponents.Netrix.UserInterface.ResourceManager), "ToolBox.PixelEditor.ico")]
    [Designer(typeof(NetrixUIDesigner))]
    [DefaultEvent("ValueChanged")]
    public class PixelEditor : UserControl
    {
        private NumericUpDown numericUpDown1;
        private Panel panel1;
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <remarks>
        /// Requests internally the localized information from satellite assemblies.
        /// </remarks>
        public PixelEditor()
        {
            InitializeComponent();
            SetUp();
        }

        internal PixelEditor(bool @internal)
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

        #region Vom Komponenten-Designer generierter Code
        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.numericUpDown1 = new NumericUpDown();
            this.panel1 = new Panel();
            ((ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.numericUpDown1.Location = new Point(0, 2);
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
            this.numericUpDown1.Size = new Size(87, 16);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.UpDownAlign = LeftRightAlignment.Left;
            this.numericUpDown1.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new EventHandler(numericUpDown1_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.panel1.BackColor = SystemColors.Window;
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(92, 23);
            this.panel1.TabIndex = 4;
            // 
            // UnitEditor
            // 
            this.Controls.Add(this.panel1);
            this.Name = "UnitEditor";
            this.Size = new Size(95, 23);
            ((ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        /// <summary>
        /// Gets or sets the border style.
        /// </summary>
        /// <remarks>
        /// This allows the developer to set a common border style around the whole control.
        /// The subcontrols will change their behavior accordingly to the outer border.
        /// </remarks>
        [Browsable(true), Category("NetRix"), Description("Gets or sets the border style.")]
        public BorderStyle Border
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
                    case BorderStyle.Fixed3D:
                        this.numericUpDown1.Top = 1;
                        break;
                    case BorderStyle.FixedSingle:
                        this.numericUpDown1.Top = 1;
                        break;
                    case BorderStyle.None:
                        this.numericUpDown1.Top = 3;
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
        public Unit Unit
        {
            get
            {
                return Unit.Pixel((int)this.numericUpDown1.Value);
            }
            set
            {
                if (value.Equals(Unit.Empty))
                {
                    numericUpDown1.Enabled = false;
                }
                else
                {
                    numericUpDown1.DecimalPlaces = 0;
                    numericUpDown1.Value = Convert.ToDecimal(value.Value);
                }
            }
        }

        /// <summary>
        /// Fired if the value has been changed.
        /// </summary>
        [Category("NetRix Events")]
        public event EventHandler ValueChanged;

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(sender, e);
            }
        }

    }
}