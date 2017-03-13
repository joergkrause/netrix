using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.StyleEditor
{
	/// <summary>
	/// StyleEditorForm contains the StyleEditor control and a simple textbox for direct style editing.
	/// </summary>
	/// <remarks>
	/// This form is called from the TypeEditor for styles.
	/// </remarks>
    [ToolboxItem(false)]
    [ToolboxBitmap(typeof(GuruComponents.Netrix.UserInterface.ResourceManager), "ToolBox.StyleEditorForm.ico")]
    [Designer(typeof(GuruComponents.Netrix.UserInterface.NetrixUIDesigner))]
    public sealed class StyleEditorForm : System.Windows.Forms.Form
	{

        private System.Windows.Forms.TextBox textBoxStyleString;
        private System.Windows.Forms.Button buttonOK;
        private GuruComponents.Netrix.UserInterface.StyleEditor.StyleControl styleUserControl1;
        private int CaretPosition = 0;

		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// Instantiates the editor form.
        /// </summary>
        /// <remarks>
        /// This sets the localizable strings using the resource in the satellite assemblies.
        /// </remarks>
		public StyleEditorForm()
		{
            this.styleUserControl1 = new GuruComponents.Netrix.UserInterface.StyleEditor.StyleControl();
			InitializeComponent();
            this.styleUserControl1.ContentChanged += new EventHandler(styleUserControl1_ContentChanged);
            ResetStyleControl();
		}

        internal StyleEditorForm(bool @internal)
        {
            this.styleUserControl1 = new GuruComponents.Netrix.UserInterface.StyleEditor.StyleControl(true);
            InitializeComponent();
            this.styleUserControl1.ContentChanged += new EventHandler(styleUserControl1_ContentChanged);
            ResetStyleControl();
        }

        /// <summary>
        /// Reset the style control to update UI.
        /// </summary>
        /// <remarks>
        /// This method is public to renew the localizable strings during run time to support 
        /// changing of UI culture on the fly.
        /// </remarks>
        public void ResetStyleControl()
        {
            this.styleUserControl1.SetUp();
            this.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditorForm.Text");
            this.buttonOK.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditorForm.buttonOK.Text");
            this.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditorForm.Text");
        }

        /// <summary>
        /// Sets the appereance of the TABs.
        /// </summary>
        public System.Windows.Forms.TabAppearance TabAppearance 
        {
            set
            {
                this.styleUserControl1.TabAppearance = value;
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

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{            
            this.textBoxStyleString = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();            
            this.SuspendLayout();
            // 
            // textBoxStyleString
            // 
            this.textBoxStyleString.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxStyleString.Location = new System.Drawing.Point(1, 449);
            this.textBoxStyleString.Multiline = true;
            this.textBoxStyleString.Name = "textBoxStyleString";
            this.textBoxStyleString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStyleString.Size = new System.Drawing.Size(383, 48);
            this.textBoxStyleString.TabIndex = 1;
            this.textBoxStyleString.Text = "";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonOK.Location = new System.Drawing.Point(387, 450);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(40, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            // 
            // styleUserControl1
            // 
            this.styleUserControl1.BackColor = System.Drawing.SystemColors.Control;
            this.styleUserControl1.Location = new System.Drawing.Point(0, 0);
            this.styleUserControl1.Name = "styleUserControl1";
            this.styleUserControl1.Size = new System.Drawing.Size(440, 448);
            this.styleUserControl1.StyleString = "";
            this.styleUserControl1.TabIndex = 0;
            // 
            // StyleEditorForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonOK;
            this.ClientSize = new System.Drawing.Size(435, 498);
            this.Controls.Add(this.styleUserControl1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxStyleString);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StyleEditorForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Style Editor";
            this.ResumeLayout(false);

        }
		#endregion

        private void styleUserControl1_ContentChanged(object sender, System.EventArgs e)
        {
            this.textBoxStyleString.TextChanged -= new System.EventHandler(this.textBoxStyles_TextChanged);
            this.textBoxStyleString.Text = this.styleUserControl1.StyleString;
            this.textBoxStyleString.TextChanged += new System.EventHandler(this.textBoxStyles_TextChanged);
        }

        private void textBoxStyles_TextChanged(object sender, System.EventArgs e)
        {
            this.CaretPosition = this.textBoxStyleString.SelectionStart;
            this.styleUserControl1.ContentChanged -= new EventHandler(styleUserControl1_ContentChanged);
            this.styleUserControl1.StyleString = this.textBoxStyleString.Text;
            this.styleUserControl1.ContentChanged += new EventHandler(styleUserControl1_ContentChanged);
            this.textBoxStyleString.SelectionStart = this.CaretPosition;
        }

        /// <summary>
        /// Sets the appereance of the buttons.
        /// </summary>
        public FlatStyle ButtonAppearance
        {
            set
            {
                this.styleUserControl1.ButtonAppearance = value;
                this.buttonOK.FlatStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets the current style as string.
        /// </summary>
        public string StyleString
        {
            get
            {
                return this.styleUserControl1.StyleString;
            }
            set
            {
                this.styleUserControl1.StyleString = value;
                // prevent from recursing events
                this.textBoxStyleString.TextChanged -= new System.EventHandler(this.textBoxStyles_TextChanged);
                this.textBoxStyleString.Text = value;
                this.textBoxStyleString.TextChanged += new System.EventHandler(this.textBoxStyles_TextChanged);
            }
        }
	}
}
