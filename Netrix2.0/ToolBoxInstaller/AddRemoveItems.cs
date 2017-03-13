using System;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using EnvDTE80;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Comzept.Genesis.Licensing;
using EnvDTE;

namespace Comzept.Genesis.Licensing
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class AddRemoveItems : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.TextBox txtTabName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel1;

		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.Windows.Forms.Button button1;
		private string assemblyName = String.Empty;
		private string installPath = String.Empty;
		private string controltype = String.Empty;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox checkBox2003;
		private System.Windows.Forms.CheckBox checkBox2005;
		private System.Windows.Forms.Label labelHeader;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label labelNotice;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBox1;

        ToolBoxTab2 tlbTabs;
	    ToolBoxTab2 tlbTab;
	    Window2 ToolBoxWnd;
	    DTE2 dteObjectVS8 = null;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private string strAssemblyFiles;

        public AddRemoveItems(string _strAssemblyFiles)
		{

			InitializeComponent();
            this.strAssemblyFiles = _strAssemblyFiles;
		}

		public string InstallPath
		{
			get
			{
				return installPath;
			}
			set
			{
				installPath = value;
				labelNotice.Text += Environment.NewLine + "Current Installation Path: " + Environment.NewLine + value;
			}
		}

		public string ControlType
		{
			get
			{
				return controltype;
			}
			set
			{

			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddRemoveItems));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelNotice = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTabName = new System.Windows.Forms.TextBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox2003 = new System.Windows.Forms.CheckBox();
            this.checkBox2005 = new System.Windows.Forms.CheckBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Controls to Add to ToolBox :";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.labelNotice);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.checkBox2005);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.checkBox2003);
            this.panel1.Controls.Add(this.txtTabName);
            this.panel1.Controls.Add(this.checkedListBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(-8, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(512, 248);
            this.panel1.TabIndex = 7;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // labelNotice
            // 
            this.labelNotice.Location = new System.Drawing.Point(48, 168);
            this.labelNotice.Name = "labelNotice";
            this.labelNotice.Size = new System.Drawing.Size(432, 75);
            this.labelNotice.TabIndex = 11;
            this.labelNotice.Text = "Please select options and start installation by clicking the [Add Items] button.";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(16, 168);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 40);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label5.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label5.Location = new System.Drawing.Point(40, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(432, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "Select options to install Components in VS.NET toolbox:";
            // 
            // txtTabName
            // 
            this.txtTabName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTabName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTabName.Location = new System.Drawing.Point(245, 72);
            this.txtTabName.MaxLength = 50;
            this.txtTabName.Name = "txtTabName";
            this.txtTabName.ReadOnly = true;
            this.txtTabName.Size = new System.Drawing.Size(232, 20);
            this.txtTabName.TabIndex = 6;
            this.txtTabName.Text = "NetRix Components";
            this.txtTabName.MouseHover += new System.EventHandler(this.txtTabName_MouseHover);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Location = new System.Drawing.Point(245, 104);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(232, 62);
            this.checkedListBox1.TabIndex = 8;
            this.checkedListBox1.MouseHover += new System.EventHandler(this.checkedListBox1_MouseHover);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(40, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "New Tab Name:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Location = new System.Drawing.Point(416, 320);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 24);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "&Add Items";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(338, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 24);
            this.button1.TabIndex = 9;
            this.button1.Text = "&Cancel";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(48, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(408, 40);
            this.label3.TabIndex = 10;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(32, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(180, 24);
            this.label4.TabIndex = 11;
            this.label4.Text = "Check the VS.NET versions:";
            // 
            // checkBox2003
            // 
            this.checkBox2003.Checked = true;
            this.checkBox2003.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2003.Enabled = false;
            this.checkBox2003.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox2003.Location = new System.Drawing.Point(43, 121);
            this.checkBox2003.Name = "checkBox2003";
            this.checkBox2003.Size = new System.Drawing.Size(104, 24);
            this.checkBox2003.TabIndex = 12;
            this.checkBox2003.Text = "VS.NET 2003";
            this.checkBox2003.Visible = false;
            this.checkBox2003.MouseHover += new System.EventHandler(this.checkBox2003_MouseHover);
            // 
            // checkBox2005
            // 
            this.checkBox2005.Checked = true;
            this.checkBox2005.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2005.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBox2005.Location = new System.Drawing.Point(245, 34);
            this.checkBox2005.Name = "checkBox2005";
            this.checkBox2005.Size = new System.Drawing.Size(104, 24);
            this.checkBox2005.TabIndex = 13;
            this.checkBox2005.Text = "VS.NET 2005";
            this.checkBox2005.MouseHover += new System.EventHandler(this.checkBox2005_MouseHover);
            // 
            // labelHeader
            // 
            this.labelHeader.BackColor = System.Drawing.SystemColors.Control;
            this.labelHeader.Font = new System.Drawing.Font("Arial", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.ForeColor = System.Drawing.SystemColors.Highlight;
            this.labelHeader.Location = new System.Drawing.Point(0, 320);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(312, 23);
            this.labelHeader.TabIndex = 14;
            this.labelHeader.Text = "NetRix Toolbox Installer";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 40);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // AddRemoveItems
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(494, 352);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AddRemoveItems";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetRix VS.NET Toolbox Installer";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AddRemoveItems_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        //[STAThread]
        //static void Main()
        //{
        //    Application.Run(new AddRemoveItems("khkhjhj"));
        //}
		private void AddRemoveItems_Load(object sender, System.EventArgs e)
		{
			//EnvDTE class cannot be created using the new keyword, it will throw an error
			//if an instance is created directly.
			//We need to use Reflection to get an instance of the DTE Object.
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(InstallerSupport.GetProperty("RegKey"));

            installPath = Path.Combine(rk.GetValue(InstallerSupport.GetProperty("InstallPathKey")).ToString(), "Control");
			try
			{
                foreach (string strAssemblyFile in strAssemblyFiles.Split('|')) {
                    try {
                        string[] strAssembly = strAssemblyFile.Split('*');
                        Assembly assembly = Assembly.LoadFile(Path.Combine(InstallPath, strAssembly[0]));
                        Type[] assemblyControls = assembly.GetTypes();
                        foreach (Type type in assemblyControls) {
                            try {
                                if (type.BaseType.ToString().Equals("System.Windows.Forms.Design.ControlDesigner") || type.BaseType.ToString().Equals("System.ComponentModel.Component")) {
                                    this.checkedListBox1.Items.Add(type.Name, true);
                                }
                            

                            }
                            catch {}
                        }
                    } catch { }
                }

				for (int i = 0; i < checkedListBox1.Items.Count; i++)
				{
					checkedListBox1.SetItemChecked(i, true);
				}

				Type t = System.Type.GetTypeFromProgID("VisualStudio.DTE.8.0");
				if (t != null)
				{
					checkBox2005.Enabled = true;
					object obj = System.Activator.CreateInstance(t, true);
					dteObjectVS8 = (EnvDTE80.DTE2)obj;
				} 
				else 
				{
					checkBox2005.Checked = false;
					checkBox2005.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Cannot find VS.NET on this system, skip toolbox installation step.\n\n" + ex.Message, "Toolbox Installer");
				Close();
			}
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{			

			if (dteObjectVS8 != null)
			{
				InstallTab(dteObjectVS8);
			}
		}

		private void InstallTab(EnvDTE80.DTE2 dteObject)
		{
            ToolBox tlBox = null;
            ToolBoxTabs tbxTabs = null;
            ToolBoxTab2 tbxTab = null;

            try {
                // Create an object reference to the IDE's ToolBox object and
                // its tabs.
                tlBox = (ToolBox)(dteObject.Windows.Item(Constants.vsWindowKindToolbox).Object);
                tbxTabs = tlBox.ToolBoxTabs;

                bool tabExists = false;

                tlbTab = null;
                if (!tabExists) {
                    try {

                        // Add a new tab to the Toolbox and select it.
                        tbxTab = (ToolBoxTab2)tbxTabs.Add(txtTabName.Text);
                        tbxTab.Activate();
                    }
                    catch {

                        MessageBox.Show("An error occured during toolbox tab installation: Tab not created.", "Add Tab", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnAdd.Text = "&Finish";
                        return;
                    }

 
                    bool success = true;
                    assemblyName = Path.Combine(InstallPath, "Comzept.NetRix.Professional.UI.dll");
                    string error = "";
                    if (File.Exists(assemblyName)) {
                        tbxTabs.Item(txtTabName.Text).Activate();

  
                        tbxTabs.Item(txtTabName.Text).ToolBoxItems.Add(txtTabName.Text,assemblyName, vsToolBoxItemFormat.vsToolBoxItemFormatDotNETComponent);
                      
                    } else {
                        success = false;
                        error += "\nError description:\nPath (1) not found: " + assemblyName;
                    }

                    if (success) {
                        MessageBox.Show(txtTabName.Text + " and selected controls successfully added to ToolBox.", "Add/Remove", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnAdd.Text = "&Next >";
                    } else {
                        MessageBox.Show("An error occured during installation: " + error, "Add/Remove", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnAdd.Text = "&Finish";
                    }
                    Close();
                }



            } catch (System.Exception ex) {
                MessageBox.Show("An unexpected error occurred during installation:\n\n" + ex.Message,"Add/Remove", MessageBoxButtons.OK,MessageBoxIcon.Error);
                
            } 



		}

		public void RemoveItem()
		{
			if (dteObjectVS8 != null)
			{
				RemoveItem(dteObjectVS8);
			}
		}

		private void RemoveItem(EnvDTE80.DTE2 dteObject)
		{
			int tabIndex;
			bool tabExists = true;
			//Get the window object corresponding to the Visual Studio ToolBox
			ToolBoxWnd = (EnvDTE80.Window2)dteObject.Windows.Item(EnvDTE.Constants.vsWindowKindToolbox);
			tlbTabs = (EnvDTE80.ToolBoxTab2)((EnvDTE.ToolBox)ToolBoxWnd.Object).ToolBoxTabs;
			tlbTab = null;	

			//loop through all the tabs and match the tab name entered by the user.
			for(tabIndex = 1; tabIndex <= tlbTabs.Collection.Count; tabIndex++)
			{
				tlbTab = (EnvDTE80.ToolBoxTab2)tlbTabs.Collection.Item(tabIndex);
				if(tlbTab.Name.Equals(txtTabName.Text))
				{
					tlbTab.Delete();
					tabExists = false;
					MessageBox.Show(txtTabName.Text + " successfully deleted from ToolBox.","Remove toolbox entry", MessageBoxButtons.OK,MessageBoxIcon.Information);
					break;
				}
			}
			if(tabExists)
			{
				MessageBox.Show(txtTabName.Text + " does not exist in the ToolBox. Please remove entries manually.","Remove toolbox entry", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void label2_Click(object sender, System.EventArgs e)
		{
		
		}

		private void panel1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void checkedListBox1_MouseHover(object sender, System.EventArgs e)
		{
			labelNotice.Text = "Select the component you wish to install in the VS.NET toolbox. If both visual studio versions are available, both installation get the new components.";
		}

		private void txtTabName_MouseHover(object sender, System.EventArgs e)
		{
			labelNotice.Text = "The name of the new Tab within the toolbox. This name is fixed during installation, but can be changed later. Changed tabs will not remove during uninstallation";
		}

		private void checkBox2003_MouseHover(object sender, System.EventArgs e)
		{
			labelNotice.Text = "Check to install in VS.NET 2003. If the field is disabled, the installer didn't recognized the IDE.";
		}

		private void checkBox2005_MouseHover(object sender, System.EventArgs e)
		{
			labelNotice.Text = "Check to install in VS.NET 2005. If the field is disabled, the installer didn't recognized the IDE.";
		}
	}
}
