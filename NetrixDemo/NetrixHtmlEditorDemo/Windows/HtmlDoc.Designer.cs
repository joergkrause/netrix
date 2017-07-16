using GuruComponents.Netrix.WebEditing.UndoRedo;
namespace NetrixHtmlEditorDemo.Windows {
  partial class HtmlDoc {
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
      if (!base.IsDisposed) {
        try {
          base.Dispose(disposing);
        }
        catch (System.Exception) {
        }
      }
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HtmlDoc));
      this.contextMenuTabPage = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.menuItem3 = new System.Windows.Forms.ToolStripMenuItem();
      this.menuItem4 = new System.Windows.Forms.ToolStripMenuItem();
      this.menuItem5 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.tabControlDoc = new System.Windows.Forms.TabControl();
      this.tabPageHtml = new System.Windows.Forms.TabPage();
      this.htmlEditor1 = new GuruComponents.Netrix.HtmlEditor();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabelFilename = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabelParentChain = new System.Windows.Forms.ToolStripStatusLabel();
      this.tabPageCode = new System.Windows.Forms.TabPage();
      this.buttonHelp = new System.Windows.Forms.Button();
      this.buttonFOAdvanced = new System.Windows.Forms.Button();
      this.groupBoxFOSet = new System.Windows.Forms.GroupBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.numericUpDownBreak = new System.Windows.Forms.NumericUpDown();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.numericUpDownIndent = new System.Windows.Forms.NumericUpDown();
      this.buttonApplyFO = new System.Windows.Forms.Button();
      this.groupBoxFO = new System.Windows.Forms.GroupBox();
      this.checkBoxPreserve = new System.Windows.Forms.CheckBox();
      this.checkBoxXhtml = new System.Windows.Forms.CheckBox();
      this.codeEditorControl1 = new GuruComponents.CodeEditor.CodeEditor.CodeEditorControl();
      this.syntaxDocument1 = new GuruComponents.CodeEditor.CodeEditor.Syntax.SyntaxDocument(this.components);
      this.imageListContext = new System.Windows.Forms.ImageList(this.components);
      this.contextMenuTabPage.SuspendLayout();
      this.tabControlDoc.SuspendLayout();
      this.tabPageHtml.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.tabPageCode.SuspendLayout();
      this.groupBoxFOSet.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBreak)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIndent)).BeginInit();
      this.groupBoxFO.SuspendLayout();
      this.SuspendLayout();
      // 
      // contextMenuTabPage
      // 
      this.contextMenuTabPage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem3,
            this.menuItem4,
            this.menuItem5});
      this.contextMenuTabPage.Name = "contextMenuTabPage";
      this.contextMenuTabPage.Size = new System.Drawing.Size(153, 92);
      this.contextMenuTabPage.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuTabPage_Opening);
      // 
      // menuItem3
      // 
      this.menuItem3.Name = "menuItem3";
      this.menuItem3.Size = new System.Drawing.Size(152, 22);
      this.menuItem3.Text = "Option &1";
      // 
      // menuItem4
      // 
      this.menuItem4.Name = "menuItem4";
      this.menuItem4.Size = new System.Drawing.Size(152, 22);
      this.menuItem4.Text = "Option &2";
      // 
      // menuItem5
      // 
      this.menuItem5.Name = "menuItem5";
      this.menuItem5.Size = new System.Drawing.Size(152, 22);
      this.menuItem5.Text = "Option &3";
      // 
      // tabControlDoc
      // 
      this.tabControlDoc.Controls.Add(this.tabPageHtml);
      this.tabControlDoc.Controls.Add(this.tabPageCode);
      this.tabControlDoc.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControlDoc.Location = new System.Drawing.Point(0, 4);
      this.tabControlDoc.Name = "tabControlDoc";
      this.tabControlDoc.SelectedIndex = 0;
      this.tabControlDoc.Size = new System.Drawing.Size(663, 551);
      this.tabControlDoc.TabIndex = 1;
      this.tabControlDoc.SelectedIndexChanged += new System.EventHandler(this.tabControlDoc_SelectedIndexChanged);
      // 
      // tabPageHtml
      // 
      this.tabPageHtml.Controls.Add(this.htmlEditor1);
      this.tabPageHtml.Controls.Add(this.statusStrip1);
      this.tabPageHtml.Location = new System.Drawing.Point(4, 22);
      this.tabPageHtml.Name = "tabPageHtml";
      this.tabPageHtml.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageHtml.Size = new System.Drawing.Size(838, 512);
      this.tabPageHtml.TabIndex = 0;
      this.tabPageHtml.Text = "HTML Editor";
      this.tabPageHtml.UseVisualStyleBackColor = true;
      // 
      // htmlEditor1
      // 
      this.htmlEditor1.AllowDrop = true;
      this.htmlEditor1.AutoWordSelection = false;
      this.htmlEditor1.BackColor = System.Drawing.SystemColors.Info;
      this.htmlEditor1.ContextMenuStrip = this.contextMenuTabPage;
      this.htmlEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.htmlEditor1.DockToolbar = System.Windows.Forms.DockStyle.Top;
      this.htmlEditor1.HtmlFormatterOptions = ((GuruComponents.Netrix.HtmlFormatting.IHtmlFormatterOptions)(resources.GetObject("htmlEditor1.HtmlFormatterOptions")));
      this.htmlEditor1.IsFileBasedDocument = false;
      this.htmlEditor1.Location = new System.Drawing.Point(3, 3);
      this.htmlEditor1.MenuStripVisible = false;
      this.htmlEditor1.Name = "htmlEditor1";
      this.htmlEditor1.Proxy = null;
      this.htmlEditor1.ShowHorizontalRuler = false;
      this.htmlEditor1.ShowVerticalRuler = false;
      this.htmlEditor1.Size = new System.Drawing.Size(832, 484);
      this.htmlEditor1.StatusStripVisible = false;
      this.htmlEditor1.TabIndex = 1;
      this.htmlEditor1.TempFile = "";
      this.htmlEditor1.ToolbarVisible = false;
      this.htmlEditor1.UserAgent = null;
      this.htmlEditor1.BeforeNavigate += new GuruComponents.Netrix.Events.BeforeNavigateEventHandler(this.htmlEditor1_BeforeNavigate);
      this.htmlEditor1.DragDrop += new System.Windows.Forms.DragEventHandler(this.htmlEditor1_DragDrop);
      this.htmlEditor1.DragOver += new System.Windows.Forms.DragEventHandler(this.htmlEditor1_DragOver);
      this.htmlEditor1.DragEnter += new System.Windows.Forms.DragEventHandler(this.htmlEditor1_DragEnter);
      this.htmlEditor1.BeforeShortcut += new GuruComponents.Netrix.Events.BeforeShortcutEventHandler(this.htmlEditor1_BeforeShortcut);
      this.htmlEditor1.NextOperationAdded += new System.EventHandler<GuruComponents.Netrix.WebEditing.UndoRedo.UndoEventArgs>(this.htmlEditor1_NextOperationAdded);
      this.htmlEditor1.Loading += new GuruComponents.Netrix.Events.LoadEventHandler(this.htmlEditor1_Loading);
      this.htmlEditor1.Loaded += new GuruComponents.Netrix.Events.LoadEventHandler(this.htmlEditor1_Loaded);
      this.htmlEditor1.Saving += new GuruComponents.Netrix.Events.SaveEventHandler(this.htmlEditor1_Saving);
      this.htmlEditor1.Saved += new GuruComponents.Netrix.Events.SaveEventHandler(this.htmlEditor1_Saved);
      this.htmlEditor1.ElementCreated += new GuruComponents.Netrix.Events.CreatedEventHandler(this.htmlEditor1_ElementCreated);
      this.htmlEditor1.BeforeResourceLoad += new GuruComponents.Netrix.Events.BeforeResourceLoadEventHandler(this.htmlEditor1_BeforeResourceLoad);
      this.htmlEditor1.FindHasReachedEnd += new System.EventHandler(this.htmlEditor1_FindHasReachedEnd);
      this.htmlEditor1.BeforeReplace += new GuruComponents.Netrix.Events.BeforeReplaceEventHandler(this.htmlEditor1_BeforeReplace);
      this.htmlEditor1.UpdateUI += new GuruComponents.Netrix.Events.UpdateUIHandler(this.htmlEditor1_UpdateUI);
      this.htmlEditor1.WebException += new GuruComponents.Netrix.Events.WebExceptionEventHandler(this.htmlEditor1_WebException);
      this.htmlEditor1.BeforeSnapRect += new GuruComponents.Netrix.Events.BeforeSnapRectEventHandler(this.htmlEditor1_BeforeSnapRect);
      this.htmlEditor1.AfterSnapRect += new GuruComponents.Netrix.Events.AfterSnapRectEventHandler(this.htmlEditor1_AfterSnapRect);
      this.htmlEditor1.ContentModified += new GuruComponents.Netrix.Events.ContentModifiedHandler(this.htmlEditor1_ContentModified);
      this.htmlEditor1.ContentChanged += new System.EventHandler(this.htmlEditor1_ContentChanged);
      this.htmlEditor1.HtmlMouseOperation += new GuruComponents.Netrix.Events.HtmlMouseEventHandler(this.htmlEditor1_HtmlMouseOperation);
      this.htmlEditor1.HtmlKeyOperation += new GuruComponents.Netrix.Events.HtmlKeyEventHandler(this.htmlEditor1_HtmlKeyOperation);
      this.htmlEditor1.HtmlElementChanged += new GuruComponents.Netrix.Events.HtmlElementChangedHandler(this.htmlEditor1_HtmlElementChanged);
      this.htmlEditor1.ReadyStateComplete += new System.EventHandler(this.htmlEditor1_ReadyStateComplete);
      this.htmlEditor1.ReadyStateChanged += new GuruComponents.Netrix.Events.ReadyStateChangedHandler(this.htmlEditor1_ReadyStateChanged);
      // 
      // statusStrip1
      // 
      this.statusStrip1.Enabled = false;
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelFilename,
            this.toolStripStatusLabelParentChain});
      this.statusStrip1.Location = new System.Drawing.Point(3, 487);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(832, 22);
      this.statusStrip1.TabIndex = 0;
      this.statusStrip1.Text = "statusStripInformation";
      // 
      // toolStripStatusLabelFilename
      // 
      this.toolStripStatusLabelFilename.Name = "toolStripStatusLabelFilename";
      this.toolStripStatusLabelFilename.Size = new System.Drawing.Size(44, 17);
      this.toolStripStatusLabelFilename.Text = "No File";
      // 
      // toolStripStatusLabelParentChain
      // 
      this.toolStripStatusLabelParentChain.Name = "toolStripStatusLabelParentChain";
      this.toolStripStatusLabelParentChain.Size = new System.Drawing.Size(23, 17);
      this.toolStripStatusLabelParentChain.Text = "<>";
      // 
      // tabPageCode
      // 
      this.tabPageCode.Controls.Add(this.buttonHelp);
      this.tabPageCode.Controls.Add(this.buttonFOAdvanced);
      this.tabPageCode.Controls.Add(this.groupBoxFOSet);
      this.tabPageCode.Controls.Add(this.buttonApplyFO);
      this.tabPageCode.Controls.Add(this.groupBoxFO);
      this.tabPageCode.Controls.Add(this.codeEditorControl1);
      this.tabPageCode.Location = new System.Drawing.Point(4, 22);
      this.tabPageCode.Name = "tabPageCode";
      this.tabPageCode.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageCode.Size = new System.Drawing.Size(655, 525);
      this.tabPageCode.TabIndex = 1;
      this.tabPageCode.Text = "Code";
      this.tabPageCode.UseVisualStyleBackColor = true;
      // 
      // buttonHelp
      // 
      this.buttonHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonHelp.Image = ((System.Drawing.Image)(resources.GetObject("buttonHelp.Image")));
      this.buttonHelp.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
      this.buttonHelp.Location = new System.Drawing.Point(7, 13);
      this.buttonHelp.Name = "buttonHelp";
      this.buttonHelp.Size = new System.Drawing.Size(49, 75);
      this.buttonHelp.TabIndex = 8;
      this.buttonHelp.Text = "Help\r\n\r\n";
      this.buttonHelp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.buttonHelp.UseVisualStyleBackColor = true;
      this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
      // 
      // buttonFOAdvanced
      // 
      this.buttonFOAdvanced.Location = new System.Drawing.Point(459, 20);
      this.buttonFOAdvanced.Name = "buttonFOAdvanced";
      this.buttonFOAdvanced.Size = new System.Drawing.Size(75, 23);
      this.buttonFOAdvanced.TabIndex = 7;
      this.buttonFOAdvanced.Text = "A&dvanced...";
      this.buttonFOAdvanced.UseVisualStyleBackColor = true;
      this.buttonFOAdvanced.Click += new System.EventHandler(buttonFOAdvanced_Click);
      // 
      // groupBoxFOSet
      // 
      this.groupBoxFOSet.Controls.Add(this.label3);
      this.groupBoxFOSet.Controls.Add(this.label4);
      this.groupBoxFOSet.Controls.Add(this.numericUpDownBreak);
      this.groupBoxFOSet.Controls.Add(this.label2);
      this.groupBoxFOSet.Controls.Add(this.label1);
      this.groupBoxFOSet.Controls.Add(this.numericUpDownIndent);
      this.groupBoxFOSet.Location = new System.Drawing.Point(235, 7);
      this.groupBoxFOSet.Name = "groupBoxFOSet";
      this.groupBoxFOSet.Size = new System.Drawing.Size(200, 82);
      this.groupBoxFOSet.TabIndex = 6;
      this.groupBoxFOSet.TabStop = false;
      this.groupBoxFOSet.Text = "Formatter Settings";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(153, 50);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(33, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "chars";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(7, 50);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(78, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Linebreak after";
      // 
      // numericUpDownBreak
      // 
      this.numericUpDownBreak.Location = new System.Drawing.Point(87, 49);
      this.numericUpDownBreak.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.numericUpDownBreak.Name = "numericUpDownBreak";
      this.numericUpDownBreak.Size = new System.Drawing.Size(59, 20);
      this.numericUpDownBreak.TabIndex = 3;
      this.numericUpDownBreak.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(153, 19);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(33, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "chars";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 18);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(37, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Indent";
      // 
      // numericUpDownIndent
      // 
      this.numericUpDownIndent.Location = new System.Drawing.Point(87, 18);
      this.numericUpDownIndent.Name = "numericUpDownIndent";
      this.numericUpDownIndent.Size = new System.Drawing.Size(59, 20);
      this.numericUpDownIndent.TabIndex = 0;
      this.numericUpDownIndent.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
      // 
      // buttonApplyFO
      // 
      this.buttonApplyFO.Location = new System.Drawing.Point(459, 53);
      this.buttonApplyFO.Name = "buttonApplyFO";
      this.buttonApplyFO.Size = new System.Drawing.Size(75, 23);
      this.buttonApplyFO.TabIndex = 5;
      this.buttonApplyFO.Text = "&Apply";
      this.buttonApplyFO.UseVisualStyleBackColor = true;
      this.buttonApplyFO.Click += new System.EventHandler(this.buttonApplyFO_Click);
      // 
      // groupBoxFO
      // 
      this.groupBoxFO.Controls.Add(this.checkBoxPreserve);
      this.groupBoxFO.Controls.Add(this.checkBoxXhtml);
      this.groupBoxFO.Location = new System.Drawing.Point(62, 7);
      this.groupBoxFO.Name = "groupBoxFO";
      this.groupBoxFO.Size = new System.Drawing.Size(163, 82);
      this.groupBoxFO.TabIndex = 4;
      this.groupBoxFO.TabStop = false;
      this.groupBoxFO.Text = "Formatter Options";
      // 
      // checkBoxPreserve
      // 
      this.checkBoxPreserve.AutoSize = true;
      this.checkBoxPreserve.Checked = true;
      this.checkBoxPreserve.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxPreserve.Location = new System.Drawing.Point(16, 19);
      this.checkBoxPreserve.Name = "checkBoxPreserve";
      this.checkBoxPreserve.Size = new System.Drawing.Size(133, 17);
      this.checkBoxPreserve.TabIndex = 1;
      this.checkBoxPreserve.Text = "Preserve Whitespaces";
      this.checkBoxPreserve.UseVisualStyleBackColor = true;
      // 
      // checkBoxXhtml
      // 
      this.checkBoxXhtml.AutoSize = true;
      this.checkBoxXhtml.Checked = true;
      this.checkBoxXhtml.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxXhtml.Location = new System.Drawing.Point(16, 50);
      this.checkBoxXhtml.Name = "checkBoxXhtml";
      this.checkBoxXhtml.Size = new System.Drawing.Size(97, 17);
      this.checkBoxXhtml.TabIndex = 2;
      this.checkBoxXhtml.Text = "Create XHTML";
      this.checkBoxXhtml.UseVisualStyleBackColor = true;
      // 
      // codeEditorControl1
      // 
      this.codeEditorControl1.ActiveView = GuruComponents.CodeEditor.CodeEditor.ActiveView.BottomRight;
      this.codeEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.codeEditorControl1.AutoListPosition = null;
      this.codeEditorControl1.AutoListSelectedText = "a123";
      this.codeEditorControl1.AutoListVisible = false;
      this.codeEditorControl1.CopyAsRTF = false;
      this.codeEditorControl1.Document = this.syntaxDocument1;
      this.codeEditorControl1.InfoTipCount = 1;
      this.codeEditorControl1.InfoTipPosition = null;
      this.codeEditorControl1.InfoTipSelectedIndex = 1;
      this.codeEditorControl1.InfoTipVisible = false;
      this.codeEditorControl1.Location = new System.Drawing.Point(3, 95);
      this.codeEditorControl1.LockCursorUpdate = false;
      this.codeEditorControl1.Name = "codeEditorControl1";
      this.codeEditorControl1.Saved = false;
      this.codeEditorControl1.ShowScopeIndicator = false;
      this.codeEditorControl1.Size = new System.Drawing.Size(652, 430);
      this.codeEditorControl1.SmoothScroll = false;
      this.codeEditorControl1.SplitviewH = -4;
      this.codeEditorControl1.SplitviewV = -4;
      this.codeEditorControl1.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(233)))));
      this.codeEditorControl1.TabIndex = 0;
      this.codeEditorControl1.Text = "codeEditorControl1";
      this.codeEditorControl1.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
      // 
      // syntaxDocument1
      // 
      this.syntaxDocument1.Lines = new string[] {
        ""};
      this.syntaxDocument1.MaxUndoBufferSize = 1000;
      this.syntaxDocument1.Modified = false;
      this.syntaxDocument1.UndoStep = 0;
      // 
      // imageListContext
      // 
      this.imageListContext.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListContext.ImageStream")));
      this.imageListContext.TransparentColor = System.Drawing.Color.Transparent;
      this.imageListContext.Images.SetKeyName(0, "undo.png");
      this.imageListContext.Images.SetKeyName(1, "copy.png");
      this.imageListContext.Images.SetKeyName(2, "paste.png");
      this.imageListContext.Images.SetKeyName(3, "cut.png");
      // 
      // HtmlDoc
      // 
      this.ClientSize = new System.Drawing.Size(663, 555);
      this.Controls.Add(this.tabControlDoc);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "HtmlDoc";
      this.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
      this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
      this.TabPageContextMenuStrip = this.contextMenuTabPage;
      this.contextMenuTabPage.ResumeLayout(false);
      this.tabControlDoc.ResumeLayout(false);
      this.tabPageHtml.ResumeLayout(false);
      this.tabPageHtml.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.tabPageCode.ResumeLayout(false);
      this.groupBoxFOSet.ResumeLayout(false);
      this.groupBoxFOSet.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBreak)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIndent)).EndInit();
      this.groupBoxFO.ResumeLayout(false);
      this.groupBoxFO.PerformLayout();
      this.ResumeLayout(false);

    }
    #endregion

    private System.Windows.Forms.ContextMenuStrip contextMenuTabPage;
    private System.Windows.Forms.ToolStripMenuItem menuItem3;
    private System.Windows.Forms.ToolStripMenuItem menuItem4;
    private System.Windows.Forms.ToolStripMenuItem menuItem5;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.TabControl tabControlDoc;
    private System.Windows.Forms.TabPage tabPageHtml;
    private System.Windows.Forms.TabPage tabPageCode;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private GuruComponents.Netrix.HtmlEditor htmlEditor1;
    private GuruComponents.Netrix.HelpLine.HelpLine helpLine1;
    private GuruComponents.Netrix.HelpLine.HelpLine helpLine2;
    private GuruComponents.Netrix.SpellChecker.Speller speller1;
    private GuruComponents.Netrix.TableDesigner.TableDesigner tableDesigner1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFilename;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelParentChain;
    private GuruComponents.CodeEditor.CodeEditor.CodeEditorControl codeEditorControl1;
    private GuruComponents.CodeEditor.CodeEditor.Syntax.SyntaxDocument syntaxDocument1;
    private System.Windows.Forms.ImageList imageListContext;
    private System.Windows.Forms.GroupBox groupBoxFOSet;
    private System.Windows.Forms.Button buttonApplyFO;
    private System.Windows.Forms.GroupBox groupBoxFO;
    private System.Windows.Forms.CheckBox checkBoxPreserve;
    private System.Windows.Forms.CheckBox checkBoxXhtml;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.NumericUpDown numericUpDownBreak;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown numericUpDownIndent;
    private System.Windows.Forms.Button buttonFOAdvanced;
    private System.Windows.Forms.Button buttonHelp;
  }
}