namespace NetrixHtmlEditorDemo.Windows
{
    partial class EventWindow
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Test");
            this.comboBoxEventType = new System.Windows.Forms.ComboBox();
            this.listViewEvents = new System.Windows.Forms.ListView();
            this.columnHeaderEventName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderEventData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSourceElement = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelSelection = new System.Windows.Forms.Label();
            this.buttonClearList = new System.Windows.Forms.Button();
            this.checkBoxStop = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxEventType
            // 
            this.comboBoxEventType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxEventType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEventType.Items.AddRange(new object[] {
            "All (Mouse, Key, Control, Window)",
            "All but Mouse",
            "All but Key",
            "All but Mouse and Key",
            "Mouse only",
            "Key only",
            "Control only",
            "Window only"});
            this.comboBoxEventType.Location = new System.Drawing.Point(185, 2);
            this.comboBoxEventType.Name = "comboBoxEventType";
            this.comboBoxEventType.Size = new System.Drawing.Size(244, 21);
            this.comboBoxEventType.TabIndex = 1;
            this.comboBoxEventType.SelectedIndexChanged += new System.EventHandler(this.comboBoxEventType_SelectedIndexChanged);
            // 
            // listViewEvents
            // 
            this.listViewEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderEventName,
            this.columnHeaderEventData,
            this.columnHeaderSourceElement});
            this.listViewEvents.GridLines = true;
            this.listViewEvents.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listViewEvents.LabelWrap = false;
            this.listViewEvents.Location = new System.Drawing.Point(2, 54);
            this.listViewEvents.MultiSelect = false;
            this.listViewEvents.Name = "listViewEvents";
            this.listViewEvents.ShowGroups = false;
            this.listViewEvents.Size = new System.Drawing.Size(424, 317);
            this.listViewEvents.TabIndex = 2;
            this.listViewEvents.UseCompatibleStateImageBehavior = false;
            this.listViewEvents.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderEventName
            // 
            this.columnHeaderEventName.Text = "Event Name";
            this.columnHeaderEventName.Width = 120;
            // 
            // columnHeaderEventData
            // 
            this.columnHeaderEventData.Text = "Event Arguments";
            this.columnHeaderEventData.Width = 200;
            // 
            // columnHeaderSourceElement
            // 
            this.columnHeaderSourceElement.Text = "Source";
            this.columnHeaderSourceElement.Width = 100;
            // 
            // labelSelection
            // 
            this.labelSelection.AutoSize = true;
            this.labelSelection.Location = new System.Drawing.Point(4, 5);
            this.labelSelection.Name = "labelSelection";
            this.labelSelection.Size = new System.Drawing.Size(175, 13);
            this.labelSelection.TabIndex = 3;
            this.labelSelection.Text = "Select the event categorie to show:";
            // 
            // buttonClearList
            // 
            this.buttonClearList.Location = new System.Drawing.Point(185, 28);
            this.buttonClearList.Name = "buttonClearList";
            this.buttonClearList.Size = new System.Drawing.Size(75, 23);
            this.buttonClearList.TabIndex = 4;
            this.buttonClearList.Text = "Clear &List";
            this.buttonClearList.UseVisualStyleBackColor = true;
            this.buttonClearList.Click += new System.EventHandler(this.buttonClearList_Click);
            // 
            // checkBoxStop
            // 
            this.checkBoxStop.AutoSize = true;
            this.checkBoxStop.Location = new System.Drawing.Point(267, 30);
            this.checkBoxStop.Name = "checkBoxStop";
            this.checkBoxStop.Size = new System.Drawing.Size(129, 17);
            this.checkBoxStop.TabIndex = 5;
            this.checkBoxStop.Text = "&Stop receiving events";
            this.checkBoxStop.UseVisualStyleBackColor = true;
            this.checkBoxStop.CheckedChanged += new System.EventHandler(this.checkBoxStop_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Options to handle the event window:";
            // 
            // EventWindow
            // 
            this.ClientSize = new System.Drawing.Size(429, 373);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxStop);
            this.Controls.Add(this.buttonClearList);
            this.Controls.Add(this.labelSelection);
            this.Controls.Add(this.listViewEvents);
            this.Controls.Add(this.comboBoxEventType);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "EventWindow";
            this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Float;
            this.TabText = "Output";
            this.Text = "Shows Events Fired by the HTML Editor";
            this.Load += new System.EventHandler(this.EventWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private System.Windows.Forms.ComboBox comboBoxEventType;
        private System.Windows.Forms.ListView listViewEvents;
        private System.Windows.Forms.Label labelSelection;
        private System.Windows.Forms.ColumnHeader columnHeaderEventName;
        private System.Windows.Forms.ColumnHeader columnHeaderEventData;
        private System.Windows.Forms.ColumnHeader columnHeaderSourceElement;
        private System.Windows.Forms.Button buttonClearList;
        private System.Windows.Forms.CheckBox checkBoxStop;
        private System.Windows.Forms.Label label1;
    }
}