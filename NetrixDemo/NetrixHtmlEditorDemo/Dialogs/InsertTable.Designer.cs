namespace GuruComponents.EditorDemo.Dialogs
{
    partial class InsertTable
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsertTable));
            this.unitEditorBorderSize = new GuruComponents.Netrix.UserInterface.UnitEditor();
            this.aeroButtonOK = new System.Windows.Forms.Button();
            this.aeroButtonCancel = new System.Windows.Forms.Button();
            this.aeroTextBoxCol = new System.Windows.Forms.TextBox();
            this.aeroTextBoxRow = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.aeroTextBoxPadding = new System.Windows.Forms.TextBox();
            this.aeroTextBoxSpacing = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // unitEditorBorderSize
            // 
            this.unitEditorBorderSize.Border = System.Windows.Forms.BorderStyle.FixedSingle;
            this.unitEditorBorderSize.Location = new System.Drawing.Point(78, 19);
            this.unitEditorBorderSize.Name = "unitEditorBorderSize";
            this.unitEditorBorderSize.Size = new System.Drawing.Size(175, 23);
            this.unitEditorBorderSize.TabIndex = 2;
            this.unitEditorBorderSize.Unit = new System.Web.UI.WebControls.Unit(1D, System.Web.UI.WebControls.UnitType.Pixel);
            // 
            // aeroButtonOK
            // 
            this.aeroButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("aeroButtonOK.Image")));
            this.aeroButtonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.aeroButtonOK.Location = new System.Drawing.Point(197, 272);
            this.aeroButtonOK.Name = "aeroButtonOK";
            this.aeroButtonOK.Size = new System.Drawing.Size(75, 23);
            this.aeroButtonOK.TabIndex = 3;
            this.aeroButtonOK.Text = "&OK";
            this.aeroButtonOK.UseVisualStyleBackColor = true;
            this.aeroButtonOK.Click += new System.EventHandler(this.aeroButtonOK_Click);
            // 
            // aeroButtonCancel
            // 
            this.aeroButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.aeroButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("aeroButtonCancel.Image")));
            this.aeroButtonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.aeroButtonCancel.Location = new System.Drawing.Point(116, 272);
            this.aeroButtonCancel.Name = "aeroButtonCancel";
            this.aeroButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.aeroButtonCancel.TabIndex = 4;
            this.aeroButtonCancel.Text = "C&ancel";
            this.aeroButtonCancel.UseVisualStyleBackColor = true;
            // 
            // aeroTextBoxCol
            // 
            this.errorProvider1.SetError(this.aeroTextBoxCol, "Must have numeric value");
            this.aeroTextBoxCol.Location = new System.Drawing.Point(19, 28);
            this.aeroTextBoxCol.Name = "aeroTextBoxCol";
            this.aeroTextBoxCol.Size = new System.Drawing.Size(50, 20);
            this.aeroTextBoxCol.TabIndex = 1;
            this.aeroTextBoxCol.Text = "3";
            // 
            // aeroTextBoxRow
            // 
            this.errorProvider1.SetError(this.aeroTextBoxRow, "Must have numeric value");
            this.aeroTextBoxRow.Location = new System.Drawing.Point(135, 28);
            this.aeroTextBoxRow.Name = "aeroTextBoxRow";
            this.aeroTextBoxRow.Size = new System.Drawing.Size(50, 20);
            this.aeroTextBoxRow.TabIndex = 3;
            this.aeroTextBoxRow.Text = "3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Columns";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(191, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Rows";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(259, 42);
            this.label3.TabIndex = 0;
            this.label3.Text = "Use this dialog to create a new table at the current caret position. Set the appr" +
                "opriate values.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.aeroTextBoxCol);
            this.groupBox1.Controls.Add(this.aeroTextBoxRow);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(16, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 70);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Table Properties";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "&Width";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.aeroTextBoxPadding);
            this.groupBox2.Controls.Add(this.aeroTextBoxSpacing);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.unitEditorBorderSize);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(16, 137);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 119);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Border Properties";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(138, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Pixel";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(138, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Pixel";
            // 
            // aeroTextBoxPadding
            // 
            this.aeroTextBoxPadding.Location = new System.Drawing.Point(78, 57);
            this.aeroTextBoxPadding.Name = "aeroTextBoxPadding";
            this.aeroTextBoxPadding.Size = new System.Drawing.Size(50, 20);
            this.aeroTextBoxPadding.TabIndex = 7;
            // 
            // aeroTextBoxSpacing
            // 
            this.aeroTextBoxSpacing.Location = new System.Drawing.Point(78, 91);
            this.aeroTextBoxSpacing.Name = "aeroTextBoxSpacing";
            this.aeroTextBoxSpacing.Size = new System.Drawing.Size(50, 20);
            this.aeroTextBoxSpacing.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Cell&spacing";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Cell&padding";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // InsertTable
            // 
            this.AcceptButton = this.aeroButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.aeroButtonCancel;
            this.ClientSize = new System.Drawing.Size(284, 307);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.aeroButtonCancel);
            this.Controls.Add(this.aeroButtonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "InsertTable";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insert Table";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Netrix.UserInterface.UnitEditor unitEditorBorderSize;
        private System.Windows.Forms.Button aeroButtonOK;
        private System.Windows.Forms.Button aeroButtonCancel;
        private System.Windows.Forms.TextBox aeroTextBoxCol;
        private System.Windows.Forms.TextBox aeroTextBoxRow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox aeroTextBoxPadding;
        private System.Windows.Forms.TextBox aeroTextBoxSpacing;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}