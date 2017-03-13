namespace GuruComponents.EditorDemo.Dialogs
{
    partial class Spellchecker
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.buttonIgnore = new System.Windows.Forms.Button();
            this.listBoxSuggestions = new System.Windows.Forms.ListBox();
            this.buttonChange = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelNotFound = new System.Windows.Forms.Label();
            this.labelSuggestions = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.labelRepl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBox1.Location = new System.Drawing.Point(13, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(337, 20);
            this.textBox1.TabIndex = 0;
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(367, 31);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(75, 23);
            this.buttonProceed.TabIndex = 1;
            this.buttonProceed.Text = "&Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.buttonProceed_Click);
            // 
            // buttonIgnore
            // 
            this.buttonIgnore.Location = new System.Drawing.Point(367, 123);
            this.buttonIgnore.Name = "buttonIgnore";
            this.buttonIgnore.Size = new System.Drawing.Size(75, 23);
            this.buttonIgnore.TabIndex = 2;
            this.buttonIgnore.Text = "&Ignore";
            this.buttonIgnore.UseVisualStyleBackColor = true;
            this.buttonIgnore.Click += new System.EventHandler(this.buttonIgnore_Click);
            // 
            // listBoxSuggestions
            // 
            this.listBoxSuggestions.FormattingEnabled = true;
            this.listBoxSuggestions.Location = new System.Drawing.Point(13, 144);
            this.listBoxSuggestions.Name = "listBoxSuggestions";
            this.listBoxSuggestions.Size = new System.Drawing.Size(337, 82);
            this.listBoxSuggestions.TabIndex = 3;
            this.listBoxSuggestions.SelectedIndexChanged += new System.EventHandler(this.listBoxSuggestions_SelectedIndexChanged);
            // 
            // buttonChange
            // 
            this.buttonChange.Enabled = false;
            this.buttonChange.Location = new System.Drawing.Point(367, 81);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(75, 23);
            this.buttonChange.TabIndex = 4;
            this.buttonChange.Text = "Change";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.buttonChange_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(367, 203);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelNotFound
            // 
            this.labelNotFound.AutoSize = true;
            this.labelNotFound.Location = new System.Drawing.Point(13, 13);
            this.labelNotFound.Name = "labelNotFound";
            this.labelNotFound.Size = new System.Drawing.Size(228, 13);
            this.labelNotFound.TabIndex = 7;
            this.labelNotFound.Text = "This word has not been found in the dictionary:";
            // 
            // labelSuggestions
            // 
            this.labelSuggestions.AutoSize = true;
            this.labelSuggestions.Location = new System.Drawing.Point(16, 123);
            this.labelSuggestions.Name = "labelSuggestions";
            this.labelSuggestions.Size = new System.Drawing.Size(280, 13);
            this.labelSuggestions.TabIndex = 8;
            this.labelSuggestions.Text = "These suggestion have been found (click to copy above):";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(16, 83);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(334, 20);
            this.textBox2.TabIndex = 9;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // labelRepl
            // 
            this.labelRepl.AutoSize = true;
            this.labelRepl.Location = new System.Drawing.Point(16, 66);
            this.labelRepl.Name = "labelRepl";
            this.labelRepl.Size = new System.Drawing.Size(181, 13);
            this.labelRepl.TabIndex = 10;
            this.labelRepl.Text = "Replace with the following correction";
            // 
            // Spellchecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 241);
            this.Controls.Add(this.labelRepl);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.labelSuggestions);
            this.Controls.Add(this.labelNotFound);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonChange);
            this.Controls.Add(this.listBoxSuggestions);
            this.Controls.Add(this.buttonIgnore);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Spellchecker";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Spellchecker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonProceed;
        private System.Windows.Forms.Button buttonIgnore;
        private System.Windows.Forms.ListBox listBoxSuggestions;
        private System.Windows.Forms.Button buttonChange;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelNotFound;
        private System.Windows.Forms.Label labelSuggestions;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label labelRepl;
    }
}