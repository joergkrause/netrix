using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.UserInterface.Ruler;
using ComponentModel_TypeConverter = System.ComponentModel.TypeConverter;
using Orientation = GuruComponents.Netrix.UserInterface.Ruler.Orientation;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.ComInterop;
using System.Collections.Generic;

namespace GuruComponents.Netrix
{
    partial class HtmlEditor {
# if !LIGHT
        private EditPanel panelEditContainer;
        private StatusStrip statusStrip1;
        private MenuStrip menuStrip1;
        private ToolStripContainer toolStripContainer1;
        private ToolStrip toolStripFile;
        private RulerControl rulerControlV;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripMenuItem printPreviewToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem customizeToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem contentsToolStripMenuItem;
        private ToolStripMenuItem indexToolStripMenuItem;
        private ToolStripMenuItem searchToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripButton newToolStripButton;
        private ToolStripButton openToolStripButton;
        private ToolStripButton saveToolStripButton;
        private ToolStripButton printToolStripButton;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripButton helpToolStripButton;
        private ToolStrip toolStripEdit;
        private ToolStripButton toolStripButtonUndo;
        private ToolStripButton toolStripButtonRedo;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripButton toolStripButtonCut;
        private ToolStripButton toolStripButtonCopy;
        private ToolStripButton toolStripButtonPaste;
        private ToolStrip toolStripFormat;
        private ToolStripButton toolStripButtonFBold;
        private ToolStripButton toolStripButtonFItalic;
        private ToolStripButton toolStripButtonFUnderline;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripButton toolStripButtonFSub;
        private ToolStripButton toolStripButtonFSup;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem subToolStripMenuItem;
        private ToolStripMenuItem superToolStripMenuItem;
        private RulerControl rulerControlH;

        private DockStyle toolBarDockStyle;
# endif
# if !LIGHT
        private void InitializeComponent()
        {
            bool IsValid = true;
# endif

            if (IsValid)
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HtmlEditor));
                this.statusStrip1 = new System.Windows.Forms.StatusStrip();
                this.menuStrip1 = new System.Windows.Forms.MenuStrip();
                this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
                this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
                this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
                this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
                this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
                this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
                this.subToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.superToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
                this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.panelEditContainer = new GuruComponents.Netrix.EditPanel();
                this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
                this.rulerControlV = new GuruComponents.Netrix.UserInterface.Ruler.RulerControl();
                this.rulerControlH = new GuruComponents.Netrix.UserInterface.Ruler.RulerControl();
                this.toolStripFile = new System.Windows.Forms.ToolStrip();
                this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
                this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
                this.toolStripFormat = new System.Windows.Forms.ToolStrip();
                this.toolStripButtonFBold = new System.Windows.Forms.ToolStripButton();
                this.toolStripButtonFItalic = new System.Windows.Forms.ToolStripButton();
                this.toolStripButtonFUnderline = new System.Windows.Forms.ToolStripButton();
                this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
                this.toolStripButtonFSub = new System.Windows.Forms.ToolStripButton();
                this.toolStripButtonFSup = new System.Windows.Forms.ToolStripButton();
                this.toolStripEdit = new System.Windows.Forms.ToolStrip();
                this.toolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
                this.toolStripButtonRedo = new System.Windows.Forms.ToolStripButton();
                this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
                this.toolStripButtonCut = new System.Windows.Forms.ToolStripButton();
                this.toolStripButtonCopy = new System.Windows.Forms.ToolStripButton();
                this.toolStripButtonPaste = new System.Windows.Forms.ToolStripButton();
                this.menuStrip1.SuspendLayout();
                this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
                this.toolStripContainer1.ContentPanel.SuspendLayout();
                this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
                this.toolStripContainer1.SuspendLayout();
                this.toolStripFile.SuspendLayout();
                this.toolStripFormat.SuspendLayout();
                this.toolStripEdit.SuspendLayout();
                this.SuspendLayout();
                // 
                // statusStrip1
                // 
                this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
                this.statusStrip1.Location = new System.Drawing.Point(0, 0);
                this.statusStrip1.Name = "statusStrip1";
                this.statusStrip1.Size = new System.Drawing.Size(439, 22);
                this.statusStrip1.TabIndex = 2;
                this.statusStrip1.Text = "statusStrip1";
                // 
                // menuStrip1
                // 
                this.menuStrip1.AllowMerge = false;
                this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
                this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
                this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
                this.menuStrip1.Location = new System.Drawing.Point(0, 0);
                this.menuStrip1.Name = "menuStrip1";
                this.menuStrip1.Size = new System.Drawing.Size(439, 24);
                this.menuStrip1.TabIndex = 3;
                this.menuStrip1.Text = "menuStrip1";

                // 
                // fileToolStripMenuItem
                // 
                this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
                this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
                this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
                this.fileToolStripMenuItem.Text = "&File";
                // 
                // newToolStripMenuItem
                // 
                this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
                this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.newToolStripMenuItem.Name = "newToolStripMenuItem";
                this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.newToolStripMenuItem.Tag = "New";
                this.newToolStripMenuItem.Text = "&New";
                this.newToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // openToolStripMenuItem
                // 
                this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
                this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.openToolStripMenuItem.Name = "openToolStripMenuItem";
                this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.openToolStripMenuItem.Tag = "Open";
                this.openToolStripMenuItem.Text = "&Open";
                this.openToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // toolStripSeparator
                // 
                this.toolStripSeparator.Name = "toolStripSeparator";
                this.toolStripSeparator.Size = new System.Drawing.Size(149, 6);
                // 
                // saveToolStripMenuItem
                // 
                this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
                this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
                this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.saveToolStripMenuItem.Tag = "Save";
                this.saveToolStripMenuItem.Text = "&Save";
                this.saveToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // saveAsToolStripMenuItem
                // 
                this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
                this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.saveAsToolStripMenuItem.Tag = "SaveAs";
                this.saveAsToolStripMenuItem.Text = "Save &As";
                this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // toolStripSeparator1
                // 
                this.toolStripSeparator1.Name = "toolStripSeparator1";
                this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
                // 
                // printToolStripMenuItem
                // 
                this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
                this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.printToolStripMenuItem.Name = "printToolStripMenuItem";
                this.printToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.printToolStripMenuItem.Tag = "Print";
                this.printToolStripMenuItem.Text = "&Print";
                this.printToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // printPreviewToolStripMenuItem
                // 
                this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
                this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
                this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.printPreviewToolStripMenuItem.Tag = "PrintPreview";
                this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
                this.printPreviewToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // toolStripSeparator2
                // 
                this.toolStripSeparator2.Name = "toolStripSeparator2";
                this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
                // 
                // exitToolStripMenuItem
                // 
                this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
                this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.exitToolStripMenuItem.Tag = "Exit";
                this.exitToolStripMenuItem.Text = "E&xit";
                this.exitToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // editToolStripMenuItem
                // 
                this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
                this.editToolStripMenuItem.Name = "editToolStripMenuItem";
                this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
                this.editToolStripMenuItem.Text = "&Edit";
                // 
                // undoToolStripMenuItem
                // 
                this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
                this.undoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.undoToolStripMenuItem.Tag = "Undo";
                this.undoToolStripMenuItem.Text = "&Undo";
                this.undoToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // redoToolStripMenuItem
                // 
                this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
                this.redoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.redoToolStripMenuItem.Tag = "Redo";
                this.redoToolStripMenuItem.Text = "&Redo";
                this.redoToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // toolStripSeparator3
                // 
                this.toolStripSeparator3.Name = "toolStripSeparator3";
                this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
                // 
                // cutToolStripMenuItem
                // 
                this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
                this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
                this.cutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.cutToolStripMenuItem.Tag = "Cut";
                this.cutToolStripMenuItem.Text = "Cu&t";
                this.cutToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // copyToolStripMenuItem
                // 
                this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
                this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
                this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.copyToolStripMenuItem.Tag = "Copy";
                this.copyToolStripMenuItem.Text = "&Copy";
                this.copyToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // pasteToolStripMenuItem
                // 
                this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
                this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
                this.pasteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.pasteToolStripMenuItem.Tag = "Paste";
                this.pasteToolStripMenuItem.Text = "&Paste";
                this.pasteToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // toolStripSeparator4
                // 
                this.toolStripSeparator4.Name = "toolStripSeparator4";
                this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
                // 
                // selectAllToolStripMenuItem
                // 
                this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
                this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.selectAllToolStripMenuItem.Tag = "SelectAll";
                this.selectAllToolStripMenuItem.Text = "Select &All";
                this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // toolsToolStripMenuItem
                // 
                this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.subToolStripMenuItem,
            this.superToolStripMenuItem});
                this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
                this.toolsToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
                this.toolsToolStripMenuItem.Text = "Forma&t";
                // 
                // customizeToolStripMenuItem
                // 
                this.customizeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("customizeToolStripMenuItem.Image")));
                this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
                this.customizeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.customizeToolStripMenuItem.Tag = "Bold";
                this.customizeToolStripMenuItem.Text = "&Bold";
                this.customizeToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // optionsToolStripMenuItem
                // 
                this.optionsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("optionsToolStripMenuItem.Image")));
                this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
                this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.optionsToolStripMenuItem.Tag = "Italic";
                this.optionsToolStripMenuItem.Text = "&Italic";
                this.optionsToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // toolStripMenuItem1
                // 
                this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem1.Image")));
                this.toolStripMenuItem1.Name = "toolStripMenuItem1";
                this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
                this.toolStripMenuItem1.Tag = "Underline";
                this.toolStripMenuItem1.Text = "&Underline";
                this.toolStripMenuItem1.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // toolStripMenuItem2
                // 
                this.toolStripMenuItem2.Name = "toolStripMenuItem2";
                this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
                // 
                // subToolStripMenuItem
                // 
                this.subToolStripMenuItem.Name = "subToolStripMenuItem";
                this.subToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.subToolStripMenuItem.Text = "&Sub";
                this.subToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // superToolStripMenuItem
                // 
                this.superToolStripMenuItem.Name = "superToolStripMenuItem";
                this.superToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.superToolStripMenuItem.Text = "Su&per";
                this.superToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // helpToolStripMenuItem
                // 
                this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
                this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
                this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
                this.helpToolStripMenuItem.Text = "&Help";
                // 
                // contentsToolStripMenuItem
                // 
                this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
                this.contentsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.contentsToolStripMenuItem.Text = "&Contents";
                this.contentsToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // indexToolStripMenuItem
                // 
                this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
                this.indexToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.indexToolStripMenuItem.Text = "&Index";
                this.indexToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // searchToolStripMenuItem
                // 
                this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
                this.searchToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.searchToolStripMenuItem.Text = "&Search";
                this.searchToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // toolStripSeparator5
                // 
                this.toolStripSeparator5.Name = "toolStripSeparator5";
                this.toolStripSeparator5.Size = new System.Drawing.Size(149, 6);
                // 
                // aboutToolStripMenuItem
                // 
                this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
                this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
                this.aboutToolStripMenuItem.Text = "&About...";
                this.aboutToolStripMenuItem.Click += new System.EventHandler(this.menuItem_Click);
                // 
                // panelEditContainer
                // 
                this.panelEditContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.panelEditContainer.BackColor = System.Drawing.SystemColors.Control;
                this.panelEditContainer.Location = new System.Drawing.Point(32, 32);
                this.panelEditContainer.Name = "panelEditContainer";
                this.panelEditContainer.Size = new System.Drawing.Size(404, 266);
                this.panelEditContainer.TabIndex = 1;
                // 
                // toolStripContainer1
                // 
                // 
                // toolStripContainer1.BottomToolStripPanel
                // 
                this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
                // 
                // toolStripContainer1.ContentPanel
                // 
                this.toolStripContainer1.ContentPanel.AutoScroll = true;
                this.toolStripContainer1.ContentPanel.Controls.Add(this.rulerControlV);
                this.toolStripContainer1.ContentPanel.Controls.Add(this.rulerControlH);
                this.toolStripContainer1.ContentPanel.Controls.Add(this.panelEditContainer);
                this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(439, 301);
                this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
                this.toolStripContainer1.Name = "toolStripContainer1";
                this.toolStripContainer1.Size = new System.Drawing.Size(439, 397);
                this.toolStripContainer1.TabIndex = 4;
                this.toolStripContainer1.Text = "toolStripContainer1";
                // 
                // toolStripContainer1.TopToolStripPanel
                // 
                this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
                this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripFile);
                this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripFormat);
                this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripEdit);
                // 
                // rulerControlV
                // 
                this.rulerControlV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)));
                this.rulerControlV.DivisionMarkFactor = 5;
                this.rulerControlV.Divisions = 10;
                this.rulerControlV.ForeColor = System.Drawing.Color.Black;
                this.rulerControlV.Location = new System.Drawing.Point(0, 32);
                this.rulerControlV.MajorInterval = 100;
                this.rulerControlV.MiddleMarkFactor = 3;
                this.rulerControlV.MouseTrackingOn = false;
                this.rulerControlV.Name = "rulerControlV";
                this.rulerControlV.Orientation = GuruComponents.Netrix.UserInterface.Ruler.Orientation.Vertical;
                this.rulerControlV.RulerAlignment = GuruComponents.Netrix.UserInterface.Ruler.RulerAlignment.BottomOrRight;
                this.rulerControlV.ScaleMode = GuruComponents.Netrix.UserInterface.Ruler.ScaleMode.Pixels;
                this.rulerControlV.ScrollBarOffset = 16;
                this.rulerControlV.Segments = null;
                this.rulerControlV.Size = new System.Drawing.Size(32, 268);
                this.rulerControlV.StartValue = 0D;
                this.rulerControlV.TabIndex = 3;
                this.rulerControlV.Text = "rulerControl2";
                this.rulerControlV.VerticalNumbers = true;
                this.rulerControlV.ZoomFactor = 1D;
                this.rulerControlV.Resize += new System.EventHandler(this.rulerControlV_Resize);
                // 
                // rulerControlH
                // 
                this.rulerControlH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                this.rulerControlH.DivisionMarkFactor = 5;
                this.rulerControlH.Divisions = 10;
                this.rulerControlH.ForeColor = System.Drawing.Color.Black;
                this.rulerControlH.Location = new System.Drawing.Point(32, 0);
                this.rulerControlH.MajorInterval = 100;
                this.rulerControlH.MiddleMarkFactor = 3;
                this.rulerControlH.MouseTrackingOn = false;
                this.rulerControlH.Name = "rulerControlH";
                this.rulerControlH.Orientation = GuruComponents.Netrix.UserInterface.Ruler.Orientation.Horizontal;
                this.rulerControlH.RulerAlignment = GuruComponents.Netrix.UserInterface.Ruler.RulerAlignment.BottomOrRight;
                this.rulerControlH.ScaleMode = GuruComponents.Netrix.UserInterface.Ruler.ScaleMode.Pixels;
                this.rulerControlH.ScrollBarOffset = 16;
                this.rulerControlH.Segments = null;
                this.rulerControlH.Size = new System.Drawing.Size(407, 32);
                this.rulerControlH.StartValue = 0D;
                this.rulerControlH.TabIndex = 2;
                this.rulerControlH.Text = "rulerControl1";
                this.rulerControlH.VerticalNumbers = true;
                this.rulerControlH.ZoomFactor = 1D;
                this.rulerControlH.Resize += new System.EventHandler(this.rulerControlH_Resize);
                // 
                // toolStripFile
                // 
                this.toolStripFile.Dock = System.Windows.Forms.DockStyle.None;
                this.toolStripFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator6,
            this.helpToolStripButton});
                this.toolStripFile.Location = new System.Drawing.Point(3, 24);
                this.toolStripFile.Name = "toolStripFile";
                this.toolStripFile.Size = new System.Drawing.Size(131, 25);
                this.toolStripFile.TabIndex = 4;
                this.toolStripFile.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tool_Click);
                // 
                // newToolStripButton
                // 
                this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
                this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.newToolStripButton.Name = "newToolStripButton";
                this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.newToolStripButton.Tag = "New";
                this.newToolStripButton.Text = "&New";
                // 
                // openToolStripButton
                // 
                this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
                this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.openToolStripButton.Name = "openToolStripButton";
                this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.openToolStripButton.Tag = "Open";
                this.openToolStripButton.Text = "&Open";
                // 
                // saveToolStripButton
                // 
                this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
                this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.saveToolStripButton.Name = "saveToolStripButton";
                this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.saveToolStripButton.Tag = "Save";
                this.saveToolStripButton.Text = "&Save";
                // 
                // printToolStripButton
                // 
                this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
                this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.printToolStripButton.Name = "printToolStripButton";
                this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.printToolStripButton.Tag = "Print";
                this.printToolStripButton.Text = "&Print";
                // 
                // toolStripSeparator6
                // 
                this.toolStripSeparator6.Name = "toolStripSeparator6";
                this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
                // 
                // helpToolStripButton
                // 
                this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
                this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.helpToolStripButton.Name = "helpToolStripButton";
                this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
                this.helpToolStripButton.Tag = "Help";
                this.helpToolStripButton.Text = "He&lp";
                // 
                // toolStripFormat
                // 
                this.toolStripFormat.Dock = System.Windows.Forms.DockStyle.None;
                this.toolStripFormat.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonFBold,
            this.toolStripButtonFItalic,
            this.toolStripButtonFUnderline,
            this.toolStripSeparator9,
            this.toolStripButtonFSub,
            this.toolStripButtonFSup});
                this.toolStripFormat.Location = new System.Drawing.Point(134, 24);
                this.toolStripFormat.Name = "toolStripFormat";
                this.toolStripFormat.Size = new System.Drawing.Size(131, 25);
                this.toolStripFormat.TabIndex = 6;
                this.toolStripFormat.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tool_Click);
                // 
                // toolStripButtonFBold
                // 
                this.toolStripButtonFBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonFBold.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFBold.Image")));
                this.toolStripButtonFBold.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.toolStripButtonFBold.Name = "toolStripButtonFBold";
                this.toolStripButtonFBold.Size = new System.Drawing.Size(23, 22);
                this.toolStripButtonFBold.Tag = "Bold";
                this.toolStripButtonFBold.Text = "Bold";
                // 
                // toolStripButtonFItalic
                // 
                this.toolStripButtonFItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonFItalic.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFItalic.Image")));
                this.toolStripButtonFItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.toolStripButtonFItalic.Name = "toolStripButtonFItalic";
                this.toolStripButtonFItalic.Size = new System.Drawing.Size(23, 22);
                this.toolStripButtonFItalic.Tag = "Italic";
                this.toolStripButtonFItalic.Text = "Italic";
                // 
                // toolStripButtonFUnderline
                // 
                this.toolStripButtonFUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonFUnderline.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFUnderline.Image")));
                this.toolStripButtonFUnderline.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.toolStripButtonFUnderline.Name = "toolStripButtonFUnderline";
                this.toolStripButtonFUnderline.Size = new System.Drawing.Size(23, 22);
                this.toolStripButtonFUnderline.Tag = "Underline";
                this.toolStripButtonFUnderline.Text = "Underline";
                // 
                // toolStripSeparator9
                // 
                this.toolStripSeparator9.Name = "toolStripSeparator9";
                this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
                // 
                // toolStripButtonFSub
                // 
                this.toolStripButtonFSub.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonFSub.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFSub.Image")));
                this.toolStripButtonFSub.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.toolStripButtonFSub.Name = "toolStripButtonFSub";
                this.toolStripButtonFSub.Size = new System.Drawing.Size(23, 22);
                this.toolStripButtonFSub.Tag = "SubScript";
                this.toolStripButtonFSub.Text = "Sub";
                // 
                // toolStripButtonFSup
                // 
                this.toolStripButtonFSup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonFSup.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFSup.Image")));
                this.toolStripButtonFSup.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.toolStripButtonFSup.Name = "toolStripButtonFSup";
                this.toolStripButtonFSup.Size = new System.Drawing.Size(23, 22);
                this.toolStripButtonFSup.Tag = "SuperScript";
                this.toolStripButtonFSup.Text = "Sup";
                // 
                // toolStripEdit
                // 
                this.toolStripEdit.Dock = System.Windows.Forms.DockStyle.None;
                this.toolStripEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonUndo,
            this.toolStripButtonRedo,
            this.toolStripSeparator8,
            this.toolStripButtonCut,
            this.toolStripButtonCopy,
            this.toolStripButtonPaste});
                this.toolStripEdit.Location = new System.Drawing.Point(3, 49);
                this.toolStripEdit.Name = "toolStripEdit";
                this.toolStripEdit.Size = new System.Drawing.Size(131, 25);
                this.toolStripEdit.TabIndex = 5;
                this.toolStripEdit.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tool_Click);
                // 
                // toolStripButtonUndo
                // 
                this.toolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonUndo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUndo.Image")));
                this.toolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.toolStripButtonUndo.Name = "toolStripButtonUndo";
                this.toolStripButtonUndo.Size = new System.Drawing.Size(23, 22);
                this.toolStripButtonUndo.Tag = "Undo";
                this.toolStripButtonUndo.Text = "Undo";
                this.toolStripButtonUndo.ToolTipText = "Undo";
                // 
                // toolStripButtonRedo
                // 
                this.toolStripButtonRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonRedo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRedo.Image")));
                this.toolStripButtonRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.toolStripButtonRedo.Name = "toolStripButtonRedo";
                this.toolStripButtonRedo.Size = new System.Drawing.Size(23, 22);
                this.toolStripButtonRedo.Tag = "Redo";
                this.toolStripButtonRedo.Text = "Redo";
                // 
                // toolStripSeparator8
                // 
                this.toolStripSeparator8.Name = "toolStripSeparator8";
                this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
                // 
                // toolStripButtonCut
                // 
                this.toolStripButtonCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonCut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCut.Image")));
                this.toolStripButtonCut.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.toolStripButtonCut.Name = "toolStripButtonCut";
                this.toolStripButtonCut.Size = new System.Drawing.Size(23, 22);
                this.toolStripButtonCut.Tag = "Cut";
                this.toolStripButtonCut.Text = "Cut";
                // 
                // toolStripButtonCopy
                // 
                this.toolStripButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCopy.Image")));
                this.toolStripButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.toolStripButtonCopy.Name = "toolStripButtonCopy";
                this.toolStripButtonCopy.Size = new System.Drawing.Size(23, 22);
                this.toolStripButtonCopy.Tag = "Copy";
                this.toolStripButtonCopy.Text = "Copy";
                // 
                // toolStripButtonPaste
                // 
                this.toolStripButtonPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.toolStripButtonPaste.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPaste.Image")));
                this.toolStripButtonPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.toolStripButtonPaste.Name = "toolStripButtonPaste";
                this.toolStripButtonPaste.Size = new System.Drawing.Size(23, 22);
                this.toolStripButtonPaste.Tag = "Paste";
                this.toolStripButtonPaste.Text = "Paste";
                // 
                // HtmlEditor
                // 
                this.BackColor = System.Drawing.SystemColors.Info;
                this.Controls.Add(this.toolStripContainer1);
                this.Name = "HtmlEditor";
                this.Size = new System.Drawing.Size(439, 397);
                this.menuStrip1.ResumeLayout(false);
                this.menuStrip1.PerformLayout();
                this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
                this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
                this.toolStripContainer1.ContentPanel.ResumeLayout(false);
                this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
                this.toolStripContainer1.TopToolStripPanel.PerformLayout();
                this.toolStripContainer1.ResumeLayout(false);
                this.toolStripContainer1.PerformLayout();
                this.toolStripFile.ResumeLayout(false);
                this.toolStripFile.PerformLayout();
                this.toolStripFormat.ResumeLayout(false);
                this.toolStripFormat.PerformLayout();
                this.toolStripEdit.ResumeLayout(false);
                this.toolStripEdit.PerformLayout();
                this.ResumeLayout(false);
            }
            else
            {
                MessageBox.Show("This component needs a license but the license cannot be found");
            }
        }


        /// <summary>
        /// Shows the vertical ruler. Off by default.
        /// </summary>
        /// <remarks>
        /// The property <see cref="VerticalRuler"/> returns the ruler object and allows
        /// setting several properties at design and run time.
        /// </remarks>
        [Browsable(false)]
        public bool ShowVerticalRuler
        {
            get
            {
                return rulerControlV.Visible;
            }
            set
            {
                rulerControlV.Visible = value;
                ResizeEditContainer();
            }
        }

        /// <summary>
        /// Shows the horizontal ruler. Off by default.
        /// </summary>
        /// <remarks>
        /// The property <see cref="HorizontalRuler"/> returns the ruler object and allows
        /// setting several properties at design and run time.
        /// </remarks>
        [Browsable(false)]
        public bool ShowHorizontalRuler
        {
            get
            {
                return rulerControlH.Visible;
            }
            set
            {
                rulerControlH.Visible = value;
                ResizeEditContainer();
            }
        }

        private void ResizeEditContainer()
        {
            if (rulerControlH.Visible)
            {
                panelEditContainer.Location = new Point((ShowVerticalRuler) ? rulerControlV.Width : 0, rulerControlH.Height);
                panelEditContainer.Size = new Size(toolStripContainer1.Width - rulerControlV.Width, InnerHeight);
            }
            else
            {
                panelEditContainer.Location = new Point((ShowVerticalRuler) ? rulerControlV.Width : 0, 0);
                panelEditContainer.Size = new Size(toolStripContainer1.Width - ((ShowVerticalRuler) ? rulerControlV.Width : 0), InnerHeight);
            }
            if (rulerControlV.Visible)
            {
                panelEditContainer.Location = new Point(rulerControlV.Width, (ShowHorizontalRuler) ? rulerControlH.Height : 0);
                panelEditContainer.Size = new Size(toolStripContainer1.Width - rulerControlV.Width, InnerHeight);
            }
            else
            {
                panelEditContainer.Location = new Point(0, (ShowHorizontalRuler) ? rulerControlH.Height : 0);
                panelEditContainer.Size = new Size(toolStripContainer1.Width, InnerHeight);
            }
            panelEditContainer.Invalidate();
        }

        private int InnerHeight
        {
            get { return toolStripContainer1.ContentPanel.Height - (((ShowHorizontalRuler) ? rulerControlH.Height : 0)); }
        }


        [Browsable(false)]
        public DockStyle DockToolbar
        {
            get
            {
                return toolBarDockStyle;
            }
            set
            {
                //switch (toolBarDockStyle)
                //{
                //    case DockStyle.Top:
                //        this.toolStripContainer1.TopToolStripPanel.Controls.Remove(this.toolStripFormat);
                //        this.toolStripContainer1.TopToolStripPanel.Controls.Remove(this.toolStripEdit);
                //        this.toolStripContainer1.TopToolStripPanel.Controls.Remove(this.toolStripFile);
                //        break;
                //    case DockStyle.Left:
                //        this.toolStripContainer1.LeftToolStripPanel.Controls.Remove(this.toolStripFormat);
                //        this.toolStripContainer1.LeftToolStripPanel.Controls.Remove(this.toolStripEdit);
                //        this.toolStripContainer1.LeftToolStripPanel.Controls.Remove(this.toolStripFile);
                //        break;
                //    case DockStyle.Right:
                //        this.toolStripContainer1.RightToolStripPanel.Controls.Remove(this.toolStripFormat);
                //        this.toolStripContainer1.RightToolStripPanel.Controls.Remove(this.toolStripEdit);
                //        this.toolStripContainer1.RightToolStripPanel.Controls.Remove(this.toolStripFile);
                //        break;
                //    case DockStyle.Bottom:
                //        this.toolStripContainer1.BottomToolStripPanel.Controls.Remove(this.toolStripFormat);
                //        this.toolStripContainer1.BottomToolStripPanel.Controls.Remove(this.toolStripEdit);
                //        this.toolStripContainer1.BottomToolStripPanel.Controls.Remove(this.toolStripFile);
                //        break;
                //    case DockStyle.Fill:
                //        this.toolStripContainer1.ContentPanel.Controls.Remove(this.toolStripFormat);
                //        this.toolStripContainer1.ContentPanel.Controls.Remove(this.toolStripEdit);
                //        this.toolStripContainer1.ContentPanel.Controls.Remove(this.toolStripFile);
                //        break;
                //    case DockStyle.None:
                //        break;
                //}

                toolBarDockStyle = value;
                switch (value)
                {
                    case DockStyle.Top:
                        int row = this.toolStripContainer1.TopToolStripPanel.Rows.Length - 1;
                        this.toolStripContainer1.TopToolStripPanel.Join(this.toolStripFormat, row);
                        this.toolStripContainer1.TopToolStripPanel.Join(this.toolStripEdit, row);
                        this.toolStripContainer1.TopToolStripPanel.Join(this.toolStripFile, row);
                        this.toolStripContainer1.TopToolStripPanel.Join(this.menuStrip1, 0);
                        ToolbarVisible = true;
                        break;
                    case DockStyle.Left:
                        this.toolStripContainer1.LeftToolStripPanel.Join(this.toolStripFormat, 0);
                        this.toolStripContainer1.LeftToolStripPanel.Join(this.toolStripEdit, 0);
                        this.toolStripContainer1.LeftToolStripPanel.Join(this.toolStripFile, 0);
                        ToolbarVisible = true;
                        break;
                    case DockStyle.Right:
                        this.toolStripContainer1.RightToolStripPanel.Join(this.toolStripFormat);
                        this.toolStripContainer1.RightToolStripPanel.Join(this.toolStripEdit);
                        this.toolStripContainer1.RightToolStripPanel.Join(this.toolStripFile);
                        ToolbarVisible = true;
                        break;
                    case DockStyle.Bottom:
                        this.toolStripContainer1.BottomToolStripPanel.Join(this.toolStripFormat, 0);
                        this.toolStripContainer1.BottomToolStripPanel.Join(this.toolStripEdit, 0);
                        this.toolStripContainer1.BottomToolStripPanel.Join(this.toolStripFile, 0);
                        ToolbarVisible = true;
                        break;
                    case DockStyle.Fill:
                        this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStripFormat);
                        this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStripEdit);
                        this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStripFile);
                        ToolbarVisible = true;
                        break;
                    case DockStyle.None:
                        this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStripFormat);
                        this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStripEdit);
                        this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStripFile);
                        ToolbarVisible = false;
                        break;
                }
                this.toolStripFormat.Dock = DockStyle.Bottom;
                this.toolStripEdit.Dock = DockStyle.Bottom;
                this.toolStripFile.Dock = DockStyle.Bottom;
                ResizeEditContainer();
            }
        }

        /// <summary>
        /// Makes all toolbars visible or hides them.
        /// </summary>
        /// <remarks>
        /// This property controls all toolbars, whether they are integrated in the base control
        /// or provided by a plug-in.
        /// </remarks>
        [Browsable(false)]
        public bool ToolbarVisible
        {
            get
            {
                return toolStripFile.Visible;
            }
            set
            {
                toolStripFile.Visible = value;
                toolStripEdit.Visible = value;
                toolStripFormat.Visible = value;
                ResizeEditContainer();
            }
        }

        /// <summary>
        /// Makes all main menustrip visible or hides it.
        /// </summary>
        /// <remarks>
        /// By default the menustrip is hidden.
        /// </remarks>
        [Browsable(false)]
        public bool MenuStripVisible
        {
            get
            {
                return menuStrip1.Visible;
            }
            set
            {
                this.menuStrip1.Visible = value;
                this.menuStrip1.Dock = DockStyle.Top;
                this.toolStripFormat.Dock = DockStyle.Bottom;
                this.toolStripEdit.Dock = DockStyle.Bottom;
                this.toolStripFile.Dock = DockStyle.Bottom;
                ResizeEditContainer();
            }
        }

        /// <summary>
        /// Makes all statusstrip visible or hides it.
        /// </summary>
        /// <remarks>
        /// By default the statusstrip is hidden. The statusstrip has no integrated features
        /// and appears empty. See <see cref="StatusStrip"/> property to get the statusstrip
        /// object and set properties and panes at design or runtime.
        /// </remarks>
        [Browsable(false)]
        public bool StatusStripVisible
        {
            get
            {
                return statusStrip1.Visible;
            }
            set
            {
                statusStrip1.Visible = value;
                ResizeEditContainer();
            }
        }

        /// <summary>
        /// Returns the vertical ruler object.
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter)), Category("NetRix UserInterface")]
        public RulerControl VerticalRuler
        {
            get { return rulerControlV; }
        }

        /// <summary>
        /// Returns the horizontal ruler object.
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter)), Category("NetRix UserInterface")]
        public RulerControl HorizontalRuler
        {
            get { return rulerControlH; }
        }

        /// <summary>
        /// Returns the main menustrip object.
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter)), Category("NetRix UserInterface")]
        public MenuStrip MenuStrip
        {
            get { return menuStrip1; }
            set { menuStrip1 = value; }
        }

        /// <summary>
        /// Returns the statusstrip object.
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter)), Category("NetRix UserInterface")]
        public StatusStrip StatusStrip
        {
            get { return statusStrip1; }
            set { statusStrip1 = value; }
        }

        /// <summary>
        /// Returns the toolstrip container to allow access to whole UI.
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter)), Category("NetRix UserInterface")]
        [Description("Returns the toolstrip container to allow access to whole UI.")]
        public ToolStripContainer ToolStripContainer
        {
            get { return toolStripContainer1; }
            set { toolStripContainer1 = value; }
        }

        private void rulerControlH_Resize(object sender, EventArgs e)
        {
            ResizeEditContainer();
        }

        private void rulerControlV_Resize(object sender, EventArgs e)
        {
            ResizeEditContainer();
        }

        private void OnRulerScroll()
        {
            Interop.IHTMLElement element = GetBodyElement().GetBaseElement();
            if (element != null)
            {
                Interop.IHtmlBodyElement body = (Interop.IHtmlBodyElement)element;

                VerticalRuler.StartValue = ((Interop.IHTMLElement2)body).GetScrollTop();
                HorizontalRuler.StartValue = ((Interop.IHTMLElement2)body).GetScrollLeft();

                HorizontalRuler.RightBottomMargin = Convert.ToInt32(body.rightMargin);
                HorizontalRuler.LeftTopMargin = Convert.ToInt32(body.leftMargin);
                VerticalRuler.RightBottomMargin = Convert.ToInt32(body.topMargin);
                VerticalRuler.LeftTopMargin = Convert.ToInt32(body.bottomMargin);

                if (((Interop.IHTMLElement2)body).GetScrollHeight() > ((Interop.IHTMLElement)body).GetOffsetHeight())
                {
                    // Vertical bar visible
                }
                if (((Interop.IHTMLElement2)body).GetScrollWidth() > ((Interop.IHTMLElement)body).GetOffsetLeft())
                {
                    // Horizontal bar visible
                }
            }
        }

        private void tool_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolClickedCancelEventArgs ce = new ToolClickedCancelEventArgs(e.ClickedItem);
            OnToolItemClicked(sender, ce);
            if (e.ClickedItem.Tag == null || ce.Cancel) return;
            switch (e.ClickedItem.Tag.ToString())
            {
                case "New":
                    EditorInstance.NewDocument();
                    break;
                case "Open":
                    break;
                case "Save":
                    break;
                case "SaveAs":
                    break;
                case "Print":
                    EditorInstance.PrintImmediately();
                    break;
                case "PrintPreview":
                    EditorInstance.PrintWithPreview();
                    break;
                case "Undo":
                    EditorInstance.Undo();
                    break;
                case "Redo":
                    EditorInstance.Redo();
                    break;
                case "Cut":
                    EditorInstance.Cut();
                    break;
                case "Copy":
                    EditorInstance.Copy();
                    break;
                case "Paste":
                    EditorInstance.Paste();
                    break;
                case "SelectAll":
                    EditorInstance.TextFormatting.SelectAll();
                    break;
                case "Bold":
                    EditorInstance.TextFormatting.ToggleBold();
                    break;
                case "Italic":
                    EditorInstance.TextFormatting.ToggleItalics();
                    break;
                case "Underline":
                    EditorInstance.TextFormatting.ToggleUnderline();
                    break;
                case "SubScript":
                    EditorInstance.TextFormatting.ToggleSubscript();
                    break;
                case "SuperScript":
                    EditorInstance.TextFormatting.ToggleSuperscript();
                    break;
                case "Help":
                    break;
            }
            UpdateUIItems();
        }

        private void UpdateUIItems()
        {
            if (toolStripContainer1.Visible)
            {
                UpdateToolPanel(toolStripContainer1.TopToolStripPanel);
                UpdateToolPanel(toolStripContainer1.LeftToolStripPanel);
                UpdateToolPanel(toolStripContainer1.RightToolStripPanel);
                UpdateToolPanel(toolStripContainer1.BottomToolStripPanel);
            }
        }

        private void UpdateToolPanel(ToolStripPanel panel)
        {
            foreach (Control c in panel.Controls)
            {
                if (c is ToolStrip)
                {
                    foreach (ToolStripItem item in ((ToolStrip)c).Items)
                    {
                        if (item.Tag == null) continue;
                        UpdateToolItem(item);
                    }
                }
            }
        }

        private void UpdateToolItem(ToolStripItem item)
        {
            try
            {
                if (commandList.Contains(item.Tag.ToString()))
                {
                    HtmlCommand command = (HtmlCommand)Enum.Parse(typeof(HtmlCommand), item.Tag.ToString());
                    switch (EditorInstance.CommandStatus(command))
                    {
                        case HtmlCommandInfo.Both:
                            item.Enabled = true;
                            if (item is ToolStripButton)
                            {
                                ((ToolStripButton)item).Checked = true;
                            }
                            break;
                        case HtmlCommandInfo.Enabled:
                            item.Enabled = true;
                            if (item is ToolStripButton)
                            {
                                ((ToolStripButton)item).Checked = false;
                            }
                            break;
                        case HtmlCommandInfo.Checked:
                            item.Enabled = false;
                            if (item is ToolStripButton)
                            {
                                ((ToolStripButton)item).Checked = true;
                            }
                            break;
                        default:
                            item.Enabled = false;
                            if (item is ToolStripButton)
                            {
                                ((ToolStripButton)item).Checked = false;
                            }
                            break;
                    }
                }
            }
            catch
            {
                return;
            }

        }

        private void menuItem_Click(object sender, EventArgs e)
        {
            tool_Click(sender, new ToolStripItemClickedEventArgs(sender as ToolStripItem));
        }

    }
}
