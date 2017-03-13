namespace GuruComponents.CodeEditor.Library.Plugins.Dialogs
{
    partial class PluginConfigurationDialog
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
            this.lstPlugins = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.btnBrowsePlugin = new System.Windows.Forms.Button();
            this.btnLoadAtStart = new System.Windows.Forms.Button();
            this.btnLoadNow = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstPlugins
            // 
            this.lstPlugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstPlugins.FullRowSelect = true;
            this.lstPlugins.HideSelection = false;
            this.lstPlugins.Location = new System.Drawing.Point(12, 12);
            this.lstPlugins.Name = "lstPlugins";
            this.lstPlugins.Size = new System.Drawing.Size(407, 232);
            this.lstPlugins.TabIndex = 0;
            this.lstPlugins.UseCompatibleStateImageBehavior = false;
            this.lstPlugins.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Filename";
            this.columnHeader2.Width = 140;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Load at Start";
            // 
            // btnBrowsePlugin
            // 
            this.btnBrowsePlugin.Location = new System.Drawing.Point(13, 251);
            this.btnBrowsePlugin.Name = "btnBrowsePlugin";
            this.btnBrowsePlugin.Size = new System.Drawing.Size(111, 23);
            this.btnBrowsePlugin.TabIndex = 1;
            this.btnBrowsePlugin.Text = "Browse For Plugin";
            this.btnBrowsePlugin.UseVisualStyleBackColor = true;
            this.btnBrowsePlugin.Click += new System.EventHandler(this.btnBrowsePlugin_Click);
            // 
            // btnLoadAtStart
            // 
            this.btnLoadAtStart.Location = new System.Drawing.Point(130, 251);
            this.btnLoadAtStart.Name = "btnLoadAtStart";
            this.btnLoadAtStart.Size = new System.Drawing.Size(92, 23);
            this.btnLoadAtStart.TabIndex = 2;
            this.btnLoadAtStart.Text = "Load at Start";
            this.btnLoadAtStart.UseVisualStyleBackColor = true;
            this.btnLoadAtStart.Click += new System.EventHandler(this.btnLoadAtStart_Click);
            // 
            // btnLoadNow
            // 
            this.btnLoadNow.Location = new System.Drawing.Point(228, 250);
            this.btnLoadNow.Name = "btnLoadNow";
            this.btnLoadNow.Size = new System.Drawing.Size(92, 23);
            this.btnLoadNow.TabIndex = 2;
            this.btnLoadNow.Text = "Load Now";
            this.btnLoadNow.UseVisualStyleBackColor = true;
            this.btnLoadNow.Click += new System.EventHandler(this.btnLoadNow_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(326, 250);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(92, 23);
            this.btnSaveConfig.TabIndex = 2;
            this.btnSaveConfig.Text = "Save Config";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(172, 280);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // PluginConfigurationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 311);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.btnLoadNow);
            this.Controls.Add(this.btnLoadAtStart);
            this.Controls.Add(this.btnBrowsePlugin);
            this.Controls.Add(this.lstPlugins);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginConfigurationDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Plugins";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        protected System.Windows.Forms.Button btnLoadAtStart;
        protected System.Windows.Forms.Button btnLoadNow;
        protected System.Windows.Forms.ListView lstPlugins;
        protected System.Windows.Forms.Button btnBrowsePlugin;
        protected System.Windows.Forms.Button btnSaveConfig;
        protected System.Windows.Forms.Button btnClose;
    }
}