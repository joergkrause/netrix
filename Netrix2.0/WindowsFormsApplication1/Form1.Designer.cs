namespace WindowsFormsApplication1 {
  partial class Form1 {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      GuruComponents.Netrix.NetrixServiceProvider netrixServiceProvider1 = new GuruComponents.Netrix.NetrixServiceProvider();
      this.htmlEditor1 = new GuruComponents.Netrix.HtmlEditor();
      this.SuspendLayout();
      // 
      // htmlEditor1
      // 
      this.htmlEditor1.BackColor = System.Drawing.SystemColors.Info;
      this.htmlEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.htmlEditor1.DockToolbar = System.Windows.Forms.DockStyle.Left;
      this.htmlEditor1.HtmlFormatterOptions = ((GuruComponents.Netrix.HtmlFormatting.IHtmlFormatterOptions)(resources.GetObject("htmlEditor1.HtmlFormatterOptions")));
      this.htmlEditor1.IsFileBasedDocument = false;
      this.htmlEditor1.Location = new System.Drawing.Point(0, 0);
      this.htmlEditor1.MenuStripVisible = true;
      this.htmlEditor1.Name = "htmlEditor1";
      this.htmlEditor1.Proxy = null;
      this.htmlEditor1.ServiceProvider = netrixServiceProvider1;
      this.htmlEditor1.ShowHorizontalRuler = false;
      this.htmlEditor1.ShowVerticalRuler = false;
      this.htmlEditor1.Size = new System.Drawing.Size(1904, 868);
      this.htmlEditor1.StatusStripVisible = false;
      this.htmlEditor1.TabIndex = 0;
      this.htmlEditor1.TempFile = "";
      this.htmlEditor1.ToolbarVisible = true;
      this.htmlEditor1.UserAgent = null;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1904, 868);
      this.Controls.Add(this.htmlEditor1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private GuruComponents.Netrix.HtmlEditor htmlEditor1;
  }
}

