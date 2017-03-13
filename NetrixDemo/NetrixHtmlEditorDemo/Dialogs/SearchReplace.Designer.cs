namespace GuruComponents.EditorDemo.Dialogs
{
    partial class SearchReplace
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchReplace));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.textBoxReplace = new System.Windows.Forms.TextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.labelReplace = new System.Windows.Forms.Label();
            this.checkBoxWholeWord = new System.Windows.Forms.CheckBox();
            this.checkBoxCaseSensitive = new System.Windows.Forms.CheckBox();
            this.checkBoxSearchUp = new System.Windows.Forms.CheckBox();
            this.checkBoxReplace = new System.Windows.Forms.CheckBox();
            this.buttonNext = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(378, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This dialog shows how to implement and use the search and replace functions.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelReplace);
            this.groupBox1.Controls.Add(this.labelSearch);
            this.groupBox1.Controls.Add(this.textBoxReplace);
            this.groupBox1.Controls.Add(this.textBoxSearch);
            this.groupBox1.Location = new System.Drawing.Point(17, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 64);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonNext);
            this.groupBox2.Controls.Add(this.checkBoxSearchUp);
            this.groupBox2.Controls.Add(this.checkBoxCaseSensitive);
            this.groupBox2.Controls.Add(this.checkBoxWholeWord);
            this.groupBox2.Location = new System.Drawing.Point(17, 123);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(414, 76);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
            this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOK.Location = new System.Drawing.Point(356, 215);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "     &OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCancel.Location = new System.Drawing.Point(275, 215);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "     &Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(82, 25);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(100, 20);
            this.textBoxSearch.TabIndex = 0;
            // 
            // textBoxReplace
            // 
            this.textBoxReplace.Location = new System.Drawing.Point(291, 25);
            this.textBoxReplace.Name = "textBoxReplace";
            this.textBoxReplace.Size = new System.Drawing.Size(100, 20);
            this.textBoxReplace.TabIndex = 1;
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(20, 28);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(56, 13);
            this.labelSearch.TabIndex = 2;
            this.labelSearch.Text = "Search f&or";
            // 
            // labelReplace
            // 
            this.labelReplace.AutoSize = true;
            this.labelReplace.Location = new System.Drawing.Point(204, 28);
            this.labelReplace.Name = "labelReplace";
            this.labelReplace.Size = new System.Drawing.Size(69, 13);
            this.labelReplace.TabIndex = 3;
            this.labelReplace.Text = "Replace wit&h";
            // 
            // checkBoxWholeWord
            // 
            this.checkBoxWholeWord.AutoSize = true;
            this.checkBoxWholeWord.Location = new System.Drawing.Point(23, 19);
            this.checkBoxWholeWord.Name = "checkBoxWholeWord";
            this.checkBoxWholeWord.Size = new System.Drawing.Size(110, 17);
            this.checkBoxWholeWord.TabIndex = 0;
            this.checkBoxWholeWord.Text = "&Whole Word Only";
            this.checkBoxWholeWord.UseVisualStyleBackColor = true;
            // 
            // checkBoxCaseSensitive
            // 
            this.checkBoxCaseSensitive.AutoSize = true;
            this.checkBoxCaseSensitive.Location = new System.Drawing.Point(23, 42);
            this.checkBoxCaseSensitive.Name = "checkBoxCaseSensitive";
            this.checkBoxCaseSensitive.Size = new System.Drawing.Size(96, 17);
            this.checkBoxCaseSensitive.TabIndex = 1;
            this.checkBoxCaseSensitive.Text = "Case &Sensitive";
            this.checkBoxCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // checkBoxSearchUp
            // 
            this.checkBoxSearchUp.AutoSize = true;
            this.checkBoxSearchUp.Location = new System.Drawing.Point(207, 19);
            this.checkBoxSearchUp.Name = "checkBoxSearchUp";
            this.checkBoxSearchUp.Size = new System.Drawing.Size(77, 17);
            this.checkBoxSearchUp.TabIndex = 2;
            this.checkBoxSearchUp.Text = "Search &Up";
            this.checkBoxSearchUp.UseVisualStyleBackColor = true;
            // 
            // checkBoxReplace
            // 
            this.checkBoxReplace.AutoSize = true;
            this.checkBoxReplace.Location = new System.Drawing.Point(17, 220);
            this.checkBoxReplace.Name = "checkBoxReplace";
            this.checkBoxReplace.Size = new System.Drawing.Size(66, 17);
            this.checkBoxReplace.TabIndex = 5;
            this.checkBoxReplace.Text = "&Replace";
            this.checkBoxReplace.UseVisualStyleBackColor = true;
            this.checkBoxReplace.CheckedChanged += new System.EventHandler(this.checkBoxReplace_CheckedChanged);
            // 
            // buttonNext
            // 
            this.buttonNext.Image = ((System.Drawing.Image)(resources.GetObject("buttonNext.Image")));
            this.buttonNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonNext.Location = new System.Drawing.Point(207, 42);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(126, 23);
            this.buttonNext.TabIndex = 3;
            this.buttonNext.Text = "&Find next word";
            this.buttonNext.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // SearchReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 259);
            this.Controls.Add(this.checkBoxReplace);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SearchReplace";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search & Replace";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchReplace_FormClosing);
            this.Load += new System.EventHandler(this.SearchReplace_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelReplace;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.TextBox textBoxReplace;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.CheckBox checkBoxSearchUp;
        private System.Windows.Forms.CheckBox checkBoxCaseSensitive;
        private System.Windows.Forms.CheckBox checkBoxWholeWord;
        private System.Windows.Forms.CheckBox checkBoxReplace;
        private System.Windows.Forms.Button buttonNext;
    }
}