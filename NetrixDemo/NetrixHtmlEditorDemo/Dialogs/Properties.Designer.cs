namespace GuruComponents.EditorDemo.Dialogs
{
    partial class Properties
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxPlugIns = new System.Windows.Forms.ListBox();
            this.radioButtonPlugin = new System.Windows.Forms.RadioButton();
            this.radioButtonHtml = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(261, 12);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(331, 265);
            this.propertyGrid1.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(517, 297);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxPlugIns);
            this.groupBox1.Controls.Add(this.radioButtonPlugin);
            this.groupBox1.Controls.Add(this.radioButtonHtml);
            this.groupBox1.Location = new System.Drawing.Point(12, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 219);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available Modules";
            // 
            // listBoxPlugIns
            // 
            this.listBoxPlugIns.Enabled = false;
            this.listBoxPlugIns.FormattingEnabled = true;
            this.listBoxPlugIns.Location = new System.Drawing.Point(29, 68);
            this.listBoxPlugIns.Name = "listBoxPlugIns";
            this.listBoxPlugIns.Size = new System.Drawing.Size(175, 134);
            this.listBoxPlugIns.TabIndex = 2;
            this.listBoxPlugIns.SelectedIndexChanged += new System.EventHandler(this.listBoxPlugIns_SelectedIndexChanged);
            // 
            // radioButtonPlugin
            // 
            this.radioButtonPlugin.AutoSize = true;
            this.radioButtonPlugin.Location = new System.Drawing.Point(7, 44);
            this.radioButtonPlugin.Name = "radioButtonPlugin";
            this.radioButtonPlugin.Size = new System.Drawing.Size(162, 17);
            this.radioButtonPlugin.TabIndex = 1;
            this.radioButtonPlugin.Text = "Plug-Ins (select one from list):";
            this.radioButtonPlugin.UseVisualStyleBackColor = true;
            this.radioButtonPlugin.CheckedChanged += new System.EventHandler(this.radioButtonPlugin_CheckedChanged);
            // 
            // radioButtonHtml
            // 
            this.radioButtonHtml.AutoSize = true;
            this.radioButtonHtml.Checked = true;
            this.radioButtonHtml.Location = new System.Drawing.Point(7, 20);
            this.radioButtonHtml.Name = "radioButtonHtml";
            this.radioButtonHtml.Size = new System.Drawing.Size(142, 17);
            this.radioButtonHtml.TabIndex = 0;
            this.radioButtonHtml.TabStop = true;
            this.radioButtonHtml.Text = "HTML Editor Component";
            this.radioButtonHtml.UseVisualStyleBackColor = true;
            this.radioButtonHtml.CheckedChanged += new System.EventHandler(this.radioButtonHtml_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select a module you want to modify properties for";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 297);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(277, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Changes made apply immediately and cannot be undone.";
            // 
            // Properties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 334);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.propertyGrid1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Properties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Properties";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxPlugIns;
        private System.Windows.Forms.RadioButton radioButtonPlugin;
        private System.Windows.Forms.RadioButton radioButtonHtml;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}