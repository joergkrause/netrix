using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.FontPicker
{
	/// <summary>
	/// This class builds the form for font selection.
	/// </summary>
	/// <remarks>
	/// The purpose to introduce a new font picker control is the possiblity to select 
	/// multiple font at a time. This is used in styles (<c>font-family</c> attribute) and
	/// the (deprecated) &lt;font&gt; tag. The browser can select any font from list and it will
	/// take the first locally installed one. Such list make sense if we think for font lists
	/// as a fallback instruction. Typically the user will select a list like "Arial, Univers,
	/// Helvetice, Sans Serif". This assumes that the most people have Arial on their system.
	/// Some of us, using a Macintosh computer, have a similar font called Univers. And a few
	/// a professionals working with expensive high quality fonts like Helvetica. Any other
	/// exotic systems are probably able to provide a generic font like Sans Serif, which is in
	/// fact replaced with the best matching font without serifs. 
	/// </remarks>
    [ToolboxItem(false)]
	public class FontPickerBox : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private FontPicker.FontListBox fontListBoxSystem;
		private FontPicker.FontListBox fontListBoxGeneric;
		private FontPicker.FontListBox fontListBoxWeb;
		private System.Collections.Specialized.StringCollection _selectedfont = new System.Collections.Specialized.StringCollection();
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabControl tabControlSchema;
		private System.Windows.Forms.TextBox textBoxFreeFont;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        /// <summary>
        /// Instantiates a new font selection form.
        /// </summary>
        /// <remarks>
        /// This control makes usage of the satellite assemblies to gather localized strings 
        /// for labels and buttons. If the control is used outside of NetRix assure that the 
        /// satellites are in place to avoid embedded replacement strings.
        /// </remarks>
		public FontPickerBox()
		{
			InitializeComponent();
            SetUp();
			FillFontLists();
			this.tabControlSchema.SelectedIndex = 1;
		}

        private void SetUp()
        {
            this.buttonOK.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("FontForm.buttonOK.Text"); // "Add";
            this.buttonCancel.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("FontForm.buttonCancel.Text"); // "Cancel";
            this.label1.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("FontForm.label1.Text"); // "Type as many fonts as necessary line by line.";
            this.tabPage4.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("FontForm.tabPage4.Text"); // "User Fonts";
            this.textBoxFreeFont.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("FontForm.textBoxFreeFont.Text"); // "";
            this.tabPage1.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("FontForm.tabPage1.Text"); // "System";
            this.tabPage2.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("FontForm.tabPage2.Text"); // "Web";
            this.tabPage3.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("FontForm.tabPage3.Text"); // "Generic";
        }

        /// <summary>
        /// Reset the UI to change labels after changing the culture.
        /// </summary>
        /// <remarks>
        /// The call of this method simply reloads the localized strings after the 
        /// current culture has changed.
        /// </remarks>
        public void ResetUI()
        {
            SetUp();
        }

        /// <summary>
        /// Sets the sample string.
        /// </summary>
        /// <remarks>
        /// The font list shows any font with its font name and a small sample. This sample
        /// is formatted with the font themselfes. The sample should have a relevant selection
        /// of letters and numbers.
        /// </remarks>
		public string SampleString
		{
			set
			{
				this.fontListBoxGeneric.SampleString = value;
				this.fontListBoxSystem.SampleString = value;
				this.fontListBoxWeb.SampleString = value;
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.fontListBoxSystem = new GuruComponents.Netrix.UserInterface.FontPicker.FontListBox();
            this.fontListBoxWeb = new GuruComponents.Netrix.UserInterface.FontPicker.FontListBox();
            this.fontListBoxGeneric = new GuruComponents.Netrix.UserInterface.FontPicker.FontListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControlSchema = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textBoxFreeFont = new System.Windows.Forms.TextBox();
            this.tabControlSchema.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(276, 1);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(72, 26);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "Add";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCancel.Location = new System.Drawing.Point(276, 31);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(72, 26);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // fontListBoxSystem
            // 
            this.fontListBoxSystem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fontListBoxSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fontListBoxSystem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.fontListBoxSystem.FamilyType = GuruComponents.Netrix.UserInterface.FontPicker.FontFamilyType.User;
            this.fontListBoxSystem.IntegralHeight = false;
            this.fontListBoxSystem.ListType = GuruComponents.Netrix.UserInterface.FontPicker.ListBoxType.FontNameAndSample;
            this.fontListBoxSystem.Location = new System.Drawing.Point(0, 0);
            this.fontListBoxSystem.Name = "fontListBoxSystem";
            this.fontListBoxSystem.SampleString = " - NET.RIX System";
            this.fontListBoxSystem.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.fontListBoxSystem.Size = new System.Drawing.Size(265, 140);
            this.fontListBoxSystem.Sorted = true;
            this.fontListBoxSystem.TabIndex = 0;
            this.fontListBoxSystem.UserFonts = null;
            this.fontListBoxSystem.DoubleClick += new System.EventHandler(this.listBoxSystemFont_DoubleClick);
            // 
            // fontListBoxWeb
            // 
            this.fontListBoxWeb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fontListBoxWeb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fontListBoxWeb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.fontListBoxWeb.FamilyType = GuruComponents.Netrix.UserInterface.FontPicker.FontFamilyType.User;
            this.fontListBoxWeb.IntegralHeight = false;
            this.fontListBoxWeb.ListType = GuruComponents.Netrix.UserInterface.FontPicker.ListBoxType.FontNameAndSample;
            this.fontListBoxWeb.Location = new System.Drawing.Point(0, 0);
            this.fontListBoxWeb.Name = "fontListBoxWeb";
            this.fontListBoxWeb.SampleString = " - NET.RIX Web";
            this.fontListBoxWeb.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.fontListBoxWeb.Size = new System.Drawing.Size(265, 137);
            this.fontListBoxWeb.Sorted = true;
            this.fontListBoxWeb.TabIndex = 0;
            this.fontListBoxWeb.UserFonts = null;
            this.fontListBoxWeb.DoubleClick += new System.EventHandler(this.listBoxWebFont_DoubleClick);
            // 
            // fontListBoxGeneric
            // 
            this.fontListBoxGeneric.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fontListBoxGeneric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fontListBoxGeneric.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.fontListBoxGeneric.FamilyType = GuruComponents.Netrix.UserInterface.FontPicker.FontFamilyType.User;
            this.fontListBoxGeneric.IntegralHeight = false;
            this.fontListBoxGeneric.ListType = GuruComponents.Netrix.UserInterface.FontPicker.ListBoxType.FontNameAndSample;
            this.fontListBoxGeneric.Location = new System.Drawing.Point(0, 0);
            this.fontListBoxGeneric.Name = "fontListBoxGeneric";
            this.fontListBoxGeneric.SampleString = " - NET.RIX Generic";
            this.fontListBoxGeneric.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.fontListBoxGeneric.Size = new System.Drawing.Size(265, 137);
            this.fontListBoxGeneric.Sorted = true;
            this.fontListBoxGeneric.TabIndex = 0;
            this.fontListBoxGeneric.UserFonts = null;
            this.fontListBoxGeneric.DoubleClick += new System.EventHandler(this.listBoxGenericFont_DoubleClick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type as many fonts as necessary line by line.";
            // 
            // tabControlSchema
            // 
            this.tabControlSchema.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlSchema.Controls.Add(this.tabPage1);
            this.tabControlSchema.Controls.Add(this.tabPage2);
            this.tabControlSchema.Controls.Add(this.tabPage3);
            this.tabControlSchema.Controls.Add(this.tabPage4);
            this.tabControlSchema.HotTrack = true;
            this.tabControlSchema.Location = new System.Drawing.Point(-1, 0);
            this.tabControlSchema.Name = "tabControlSchema";
            this.tabControlSchema.SelectedIndex = 3;
            this.tabControlSchema.Size = new System.Drawing.Size(273, 166);
            this.tabControlSchema.TabIndex = 4;
            this.tabControlSchema.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectionChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fontListBoxSystem);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(265, 140);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "System";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.fontListBoxWeb);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(265, 137);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "Web";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.fontListBoxGeneric);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(265, 137);
            this.tabPage3.TabIndex = 5;
            this.tabPage3.Text = "Generic";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.textBoxFreeFont);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(265, 137);
            this.tabPage4.TabIndex = 6;
            this.tabPage4.Text = "User Fonts";
            // 
            // textBoxFreeFont
            // 
            this.textBoxFreeFont.AcceptsReturn = true;
            this.textBoxFreeFont.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFreeFont.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFreeFont.Location = new System.Drawing.Point(8, 32);
            this.textBoxFreeFont.Multiline = true;
            this.textBoxFreeFont.Name = "textBoxFreeFont";
            this.textBoxFreeFont.Size = new System.Drawing.Size(254, 100);
            this.textBoxFreeFont.TabIndex = 1;
            this.textBoxFreeFont.Text = "";
            // 
            // FontPickerBox
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(352, 164);
            this.Controls.Add(this.tabControlSchema);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FontPickerBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Font Selection Box";
            this.tabControlSchema.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion
	
		private void FillFontLists()
		{
			this.fontListBoxGeneric.FamilyType = FontFamilyType.Generic;
			this.fontListBoxGeneric.ListType = ListBoxType.FontNameAndSample;
			this.fontListBoxGeneric.Populate();
			this.fontListBoxSystem.FamilyType = FontFamilyType.System;
			this.fontListBoxSystem.ListType = ListBoxType.FontNameAndSample;
			this.fontListBoxSystem.Populate();
			this.fontListBoxWeb.FamilyType = FontFamilyType.Web;
			this.fontListBoxWeb.ListType = ListBoxType.FontNameAndSample;
			this.fontListBoxWeb.Populate();
		}

        /// <summary>
        /// Gets or sets the list of fonts.
        /// </summary>
        /// <remarks>
        /// This property gets or sets the list on fonts as a 
        /// <see cref="System.Collections.Specialized.StringCollection">StringCollection</see> collection.
        /// The result is nether <c>null</c>, but the list can be empty.
        /// </remarks>
		public System.Collections.Specialized.StringCollection SelectedFont
		{
			get
			{
				return _selectedfont;
			}
			set
			{
				_selectedfont = value;
			}
		}

		private void listBoxSystemFont_DoubleClick(object sender, System.EventArgs e)
		{
			_selectedfont.Clear();
			_selectedfont.Add((string) this.fontListBoxSystem.SelectedItem);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void listBoxWebFont_DoubleClick(object sender, System.EventArgs e)
		{
			_selectedfont.Clear();
			_selectedfont.Add((string) this.fontListBoxWeb.SelectedItem);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		
		private void listBoxGenericFont_DoubleClick(object sender, System.EventArgs e)
		{
			_selectedfont.Clear();
			_selectedfont.Add((string) this.fontListBoxGeneric.SelectedItem);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}


		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			_selectedfont.Clear();
			switch (this.tabControlSchema.SelectedIndex)
			{
				case 0:
					for (int i = 0; i < this.fontListBoxSystem.SelectedIndices.Count; i++)
					{
						_selectedfont.Add( (string) this.fontListBoxSystem.SelectedItems[i]);
					}
					break;
				case 1:
					for (int i = 0; i < this.fontListBoxWeb.SelectedIndices.Count; i++)
					{
						_selectedfont.Add( (string) this.fontListBoxWeb.SelectedItems[i]);
					}
					break;
				case 2:
					for (int i = 0; i < this.fontListBoxGeneric.SelectedIndices.Count; i++)
					{
						_selectedfont.Add( (string) this.fontListBoxGeneric.SelectedItems[i]);
					}
					break;
				case 3:
					if (this.textBoxFreeFont.TextLength > 1)
					{
						string[] lines = this.textBoxFreeFont.Text.Split(new char[] {'\n'});
						_selectedfont.AddRange(lines);
					}
					break;
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			SelectedFont.Clear();
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void tabControlSchema_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch (this.tabControlSchema.SelectedTab.Tag.ToString())
			{
				case "0":
					this.fontListBoxSystem.Focus();
					break;
				case "1":
					this.fontListBoxGeneric.Focus();
					break;
				case "2":
					this.fontListBoxWeb.Focus();
					break;
				case "9":
					this.textBoxFreeFont.Focus();
					break;
			}
		}

		private void tabControl1_SelectionChanged(object sender, System.EventArgs e)
		{
		
		}
	}
}
