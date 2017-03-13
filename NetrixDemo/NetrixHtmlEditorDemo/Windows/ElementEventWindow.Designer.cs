namespace NetrixHtmlEditorDemo.Windows
{
    partial class ElementEventWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElementEventWindow));
            this.listViewEvents = new System.Windows.Forms.ListView();
            this.columnHeaderEventName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderEventData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSourceElement = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonClearList = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            this.listViewEvents.LabelWrap = false;
            this.listViewEvents.Location = new System.Drawing.Point(2, 33);
            this.listViewEvents.MultiSelect = false;
            this.listViewEvents.Name = "listViewEvents";
            this.listViewEvents.ShowGroups = false;
            this.listViewEvents.Size = new System.Drawing.Size(424, 301);
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
            // buttonClearList
            // 
            this.buttonClearList.Location = new System.Drawing.Point(3, 340);
            this.buttonClearList.Name = "buttonClearList";
            this.buttonClearList.Size = new System.Drawing.Size(75, 23);
            this.buttonClearList.TabIndex = 4;
            this.buttonClearList.Text = "Clear &List";
            this.buttonClearList.UseVisualStyleBackColor = true;
            this.buttonClearList.Click += new System.EventHandler(this.buttonClearList_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(351, 340);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "&Close";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "This windows shows the currently attached element events.";
            // 
            // ElementEventWindow
            // 
            this.ClientSize = new System.Drawing.Size(429, 373);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonClearList);
            this.Controls.Add(this.listViewEvents);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.HideOnClose = true;
            this.Name = "ElementEventWindow";
            this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Float;
            this.TabText = "Output";
            this.Text = "Shows Events Fired by Elements";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private System.Windows.Forms.ListView listViewEvents;
        private System.Windows.Forms.ColumnHeader columnHeaderEventName;
        private System.Windows.Forms.ColumnHeader columnHeaderEventData;
        private System.Windows.Forms.ColumnHeader columnHeaderSourceElement;
        private System.Windows.Forms.Button buttonClearList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
    }
}