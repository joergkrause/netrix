namespace GuruComponents.EditorDemo.Dialogs
{
    partial class EditStyles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditStyles));
            this.styleControl1 = new GuruComponents.Netrix.UserInterface.StyleEditor.StyleControl();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxStyles = new System.Windows.Forms.TextBox();
            this.labelElement = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // styleControl1
            // 
            this.styleControl1.BorderDesign = System.Windows.Forms.BorderStyle.None;
            this.styleControl1.FontPickerSampleString = " - NET.RIX";
            this.styleControl1.Location = new System.Drawing.Point(12, 12);
            this.styleControl1.Name = "styleControl1";
            this.styleControl1.Size = new System.Drawing.Size(440, 448);
            this.styleControl1.StyleObject = ((GuruComponents.Netrix.UserInterface.StyleParser.StyleObject)(resources.GetObject("styleControl1.StyleObject")));
            this.styleControl1.StyleString = "";
            this.styleControl1.TabAppearance = System.Windows.Forms.TabAppearance.Normal;
            this.styleControl1.TabIndex = 0;
            this.styleControl1.ContentChanged += new System.EventHandler(this.styleControl1_ContentChanged);
            this.styleControl1.ParserReady += new GuruComponents.Netrix.UserInterface.StyleParser.SelectorEventHandler(this.styleControl1_ParserReady);
            // 
            // buttonOK
            // 
            this.buttonOK.Image = ((System.Drawing.Image)(resources.GetObject("buttonOK.Image")));
            this.buttonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOK.Location = new System.Drawing.Point(376, 573);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "    &OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(295, 572);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxStyles);
            this.groupBox1.Controls.Add(this.labelElement);
            this.groupBox1.Location = new System.Drawing.Point(12, 470);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 84);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Style for Element ";
            // 
            // textBoxStyles
            // 
            this.textBoxStyles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxStyles.Location = new System.Drawing.Point(7, 20);
            this.textBoxStyles.Multiline = true;
            this.textBoxStyles.Name = "textBoxStyles";
            this.textBoxStyles.Size = new System.Drawing.Size(427, 58);
            this.textBoxStyles.TabIndex = 5;
            this.textBoxStyles.TextChanged += new System.EventHandler(this.textBoxStyles_TextChanged);
            // 
            // labelElement
            // 
            this.labelElement.AutoSize = true;
            this.labelElement.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelElement.Location = new System.Drawing.Point(93, 0);
            this.labelElement.Name = "labelElement";
            this.labelElement.Size = new System.Drawing.Size(19, 13);
            this.labelElement.TabIndex = 4;
            this.labelElement.Text = "...";
            // 
            // EditStyles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 607);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.styleControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditStyles";
            this.Text = "EditStyles";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Netrix.UserInterface.StyleEditor.StyleControl styleControl1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxStyles;
        private System.Windows.Forms.Label labelElement;
    }
}