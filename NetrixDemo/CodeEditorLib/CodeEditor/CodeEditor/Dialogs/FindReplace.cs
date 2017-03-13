using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GuruComponents.CodeEditor.CodeEditor
{
	/// <summary>
	/// Summary description for FindReplace.
	/// </summary>
	public class FindReplaceForm : Form
	{
		private Panel pnlButtons;
		private Panel pnlFind;
		private Panel pnlReplace;
		private Panel pnlSettings;
		private Label lblFindWhat;
		private Button btnRegex1;
		private Button button1;
		private Label lblReplaceWith;
		private Panel panel1;
		private Panel panel3;
		private Panel pnlReplaceButtons;
		private GroupBox groupBox1;
		private ComboBox cboFind;
		private ComboBox cboReplace;
		private Button btnReplace;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private Button btnFind;
		private Button btnClose;
		private CheckBox chkWholeWord;
		private CheckBox chkMatchCase;
		private Button btnReplaceAll;
		private CheckBox chkRegEx;
		private Button btnMarkAll;

		private WeakReference _Control = null;

		private EditViewControl mOwner
		{
			get
			{
				if (_Control != null)
					return (EditViewControl) _Control.Target;
				else
					return null;
			}
			set { _Control = new WeakReference(value); }
		}


		private Button btnDoReplace;
		private string _Last = "";

		/// <summary>
		/// Default constructor for the FindReplaceForm.
		/// </summary>
		public FindReplaceForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            btnFind.Text = Localizations.FindNextButtonText;
            btnReplace.Text = Localizations.FindReplaceButtonText;
            btnMarkAll.Text = Localizations.FindMarkAllButtonText;
            btnReplaceAll.Text = Localizations.FindReplaceAllButtonText;
            btnClose.Text = Localizations.FindCloseButtonText;

            lblFindWhat.Text = Localizations.FindWhatLabelText;
            lblReplaceWith.Text = Localizations.FindReplaceWithLabelText;

            chkMatchCase.Text = Localizations.FindMatchCaseLabel;
            chkWholeWord.Text = Localizations.FindMatchWholeWordLabel;
            chkRegEx.Text = Localizations.FindUseRegExLabel;

		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
//				unchecked
//				{
//					int i= (int)0x80000000;
//					cp.Style |=i;
//				}
				return cp;
			}
		}

		/// <summary>
		/// Creates a FindReplaceForm that will be assigned to a specific Owner control.
		/// </summary>
		/// <param name="Owner">The SyntaxBox that will use the FindReplaceForm</param>
		public FindReplaceForm(EditViewControl Owner)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			mOwner = Owner;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Displays the FindReplaceForm and sets it in "Find" mode.
		/// </summary>
		public void ShowFind()
		{
			pnlReplace.Visible = false;
			pnlReplaceButtons.Visible = false;
            this.Text = Localizations.FindDialogText;
			this.Show();
			this.Height = 160;
			btnDoReplace.Visible = false;
			btnReplace.Visible = true;
			_Last = "";
			cboFind.Focus();
		}

		/// <summary>
		/// Displays the FindReplaceForm and sets it in "Replace" mode.
		/// </summary>
		public void ShowReplace()
		{
			pnlReplace.Visible = true;
			pnlReplaceButtons.Visible = true;
            this.Text = Localizations.ReplaceDialogText;
			this.Show();
			this.Height = 200;
			btnDoReplace.Visible = true;
			btnReplace.Visible = false;
			_Last = "";
			cboFind.Focus();
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindReplaceForm));
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMarkAll = new System.Windows.Forms.Button();
            this.pnlReplaceButtons = new System.Windows.Forms.Panel();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDoReplace = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.pnlFind = new System.Windows.Forms.Panel();
            this.cboFind = new System.Windows.Forms.ComboBox();
            this.lblFindWhat = new System.Windows.Forms.Label();
            this.btnRegex1 = new System.Windows.Forms.Button();
            this.pnlReplace = new System.Windows.Forms.Panel();
            this.cboReplace = new System.Windows.Forms.ComboBox();
            this.lblReplaceWith = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkRegEx = new System.Windows.Forms.CheckBox();
            this.chkWholeWord = new System.Windows.Forms.CheckBox();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.pnlButtons.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlReplaceButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlFind.SuspendLayout();
            this.pnlReplace.SuspendLayout();
            this.pnlSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.panel3);
            this.pnlButtons.Controls.Add(this.pnlReplaceButtons);
            this.pnlButtons.Controls.Add(this.panel1);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Location = new System.Drawing.Point(400, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(96, 178);
            this.pnlButtons.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.btnMarkAll);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 96);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(96, 82);
            this.panel3.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(8, 40);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 24);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMarkAll
            // 
            this.btnMarkAll.Location = new System.Drawing.Point(8, 8);
            this.btnMarkAll.Name = "btnMarkAll";
            this.btnMarkAll.Size = new System.Drawing.Size(80, 24);
            this.btnMarkAll.TabIndex = 2;
            this.btnMarkAll.Text = "Mark all";
            this.btnMarkAll.Click += new System.EventHandler(this.btnMarkAll_Click);
            // 
            // pnlReplaceButtons
            // 
            this.pnlReplaceButtons.Controls.Add(this.btnReplaceAll);
            this.pnlReplaceButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReplaceButtons.Location = new System.Drawing.Point(0, 64);
            this.pnlReplaceButtons.Name = "pnlReplaceButtons";
            this.pnlReplaceButtons.Size = new System.Drawing.Size(96, 32);
            this.pnlReplaceButtons.TabIndex = 3;
            this.pnlReplaceButtons.Visible = false;
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Location = new System.Drawing.Point(8, 8);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(80, 24);
            this.btnReplaceAll.TabIndex = 2;
            this.btnReplaceAll.Text = "Replace All";
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDoReplace);
            this.panel1.Controls.Add(this.btnReplace);
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(96, 64);
            this.panel1.TabIndex = 2;
            // 
            // btnDoReplace
            // 
            this.btnDoReplace.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDoReplace.Location = new System.Drawing.Point(8, 40);
            this.btnDoReplace.Name = "btnDoReplace";
            this.btnDoReplace.Size = new System.Drawing.Size(80, 24);
            this.btnDoReplace.TabIndex = 4;
            this.btnDoReplace.Text = "Replace";
            this.btnDoReplace.Click += new System.EventHandler(this.btnDoReplace_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Image = ((System.Drawing.Image)(resources.GetObject("btnReplace.Image")));
            this.btnReplace.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReplace.Location = new System.Drawing.Point(8, 40);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(80, 24);
            this.btnReplace.TabIndex = 3;
            this.btnReplace.Text = "Replace";
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnFind
            // 
            this.btnFind.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnFind.Location = new System.Drawing.Point(8, 8);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 24);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "&FindNext";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // pnlFind
            // 
            this.pnlFind.Controls.Add(this.cboFind);
            this.pnlFind.Controls.Add(this.lblFindWhat);
            this.pnlFind.Controls.Add(this.btnRegex1);
            this.pnlFind.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFind.Location = new System.Drawing.Point(0, 0);
            this.pnlFind.Name = "pnlFind";
            this.pnlFind.Size = new System.Drawing.Size(400, 40);
            this.pnlFind.TabIndex = 1;
            // 
            // cboFind
            // 
            this.cboFind.Location = new System.Drawing.Point(104, 8);
            this.cboFind.Name = "cboFind";
            this.cboFind.Size = new System.Drawing.Size(288, 21);
            this.cboFind.TabIndex = 1;
            // 
            // lblFindWhat
            // 
            this.lblFindWhat.Location = new System.Drawing.Point(8, 8);
            this.lblFindWhat.Name = "lblFindWhat";
            this.lblFindWhat.Size = new System.Drawing.Size(96, 24);
            this.lblFindWhat.TabIndex = 0;
            this.lblFindWhat.Text = "Fi&nd what:";
            // 
            // btnRegex1
            // 
            this.btnRegex1.Image = ((System.Drawing.Image)(resources.GetObject("btnRegex1.Image")));
            this.btnRegex1.Location = new System.Drawing.Point(368, 8);
            this.btnRegex1.Name = "btnRegex1";
            this.btnRegex1.Size = new System.Drawing.Size(21, 21);
            this.btnRegex1.TabIndex = 2;
            this.btnRegex1.Visible = false;
            // 
            // pnlReplace
            // 
            this.pnlReplace.Controls.Add(this.cboReplace);
            this.pnlReplace.Controls.Add(this.lblReplaceWith);
            this.pnlReplace.Controls.Add(this.button1);
            this.pnlReplace.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReplace.Location = new System.Drawing.Point(0, 40);
            this.pnlReplace.Name = "pnlReplace";
            this.pnlReplace.Size = new System.Drawing.Size(400, 40);
            this.pnlReplace.TabIndex = 2;
            this.pnlReplace.Visible = false;
            // 
            // cboReplace
            // 
            this.cboReplace.Location = new System.Drawing.Point(106, 8);
            this.cboReplace.Name = "cboReplace";
            this.cboReplace.Size = new System.Drawing.Size(286, 21);
            this.cboReplace.TabIndex = 4;
            // 
            // lblReplaceWith
            // 
            this.lblReplaceWith.Location = new System.Drawing.Point(10, 8);
            this.lblReplaceWith.Name = "lblReplaceWith";
            this.lblReplaceWith.Size = new System.Drawing.Size(96, 24);
            this.lblReplaceWith.TabIndex = 3;
            this.lblReplaceWith.Text = "Re&place with:";
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(368, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(21, 21);
            this.button1.TabIndex = 5;
            this.button1.Visible = false;
            // 
            // pnlSettings
            // 
            this.pnlSettings.Controls.Add(this.groupBox1);
            this.pnlSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSettings.Location = new System.Drawing.Point(0, 80);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(400, 98);
            this.pnlSettings.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkRegEx);
            this.groupBox1.Controls.Add(this.chkWholeWord);
            this.groupBox1.Controls.Add(this.chkMatchCase);
            this.groupBox1.Location = new System.Drawing.Point(8, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 88);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search";
            // 
            // chkRegEx
            // 
            this.chkRegEx.Location = new System.Drawing.Point(8, 64);
            this.chkRegEx.Name = "chkRegEx";
            this.chkRegEx.Size = new System.Drawing.Size(144, 16);
            this.chkRegEx.TabIndex = 2;
            this.chkRegEx.Text = "Use Regular expressions";
            this.chkRegEx.Visible = false;
            // 
            // chkWholeWord
            // 
            this.chkWholeWord.Location = new System.Drawing.Point(8, 40);
            this.chkWholeWord.Name = "chkWholeWord";
            this.chkWholeWord.Size = new System.Drawing.Size(144, 16);
            this.chkWholeWord.TabIndex = 1;
            this.chkWholeWord.Text = "Match &whole word";
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.Location = new System.Drawing.Point(8, 16);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(144, 16);
            this.chkMatchCase.TabIndex = 0;
            this.chkMatchCase.Text = "Match &case";
            // 
            // FindReplaceForm
            // 
            this.AcceptButton = this.btnFind;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(496, 178);
            this.Controls.Add(this.pnlSettings);
            this.Controls.Add(this.pnlReplace);
            this.Controls.Add(this.pnlFind);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FindReplaceForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.FindReplace_Closing);
            this.pnlButtons.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnlReplaceButtons.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlFind.ResumeLayout(false);
            this.pnlReplace.ResumeLayout(false);
            this.pnlSettings.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private void btnReplace_Click(object sender, EventArgs e)
		{
			ShowReplace();
		}

		private void FindReplace_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
		}

		private void btnFind_Click(object sender, EventArgs e)
		{
			this.FindNext();
			cboFind.Focus();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			mOwner.Focus();
			this.Hide();
		}

		private void btnDoReplace_Click(object sender, EventArgs e)
		{
			this.mOwner.ReplaceSelection(cboReplace.Text);
			btnFind_Click(null, null);
		}

		private void btnReplaceAll_Click(object sender, EventArgs e)
		{
			string text = cboFind.Text;
			if (text == "")
				return;

			bool found = false;
			foreach (string s in cboFind.Items)
			{
				if (s == text)
				{
					found = true;
					break;
				}
			}
			if (!found)
				cboFind.Items.Add(text);

			int x = mOwner.Caret.Position.X;
			int y = mOwner.Caret.Position.Y;
			mOwner.Caret.Position.X = 0;
			mOwner.Caret.Position.Y = 0;
			while (mOwner.SelectNext(cboFind.Text, chkMatchCase.Checked, chkWholeWord.Checked, chkRegEx.Checked))
			{
				this.mOwner.ReplaceSelection(cboReplace.Text);
			}

			mOwner.Selection.ClearSelection();
			//	mOwner.Caret.Position.X=x;
			//	mOwner.Caret.Position.Y=y;
			//	mOwner.ScrollIntoView ();

			cboFind.Focus();
		}

		private void btnMarkAll_Click(object sender, EventArgs e)
		{
			string text = cboFind.Text;
			if (text == "")
				return;

			bool found = false;
			foreach (string s in cboFind.Items)
			{
				if (s == text)
				{
					found = true;
					break;
				}
			}
			if (!found)
				cboFind.Items.Add(text);

			int x = mOwner.Caret.Position.X;
			int y = mOwner.Caret.Position.Y;
			mOwner.Caret.Position.X = 0;
			mOwner.Caret.Position.Y = 0;
			while (mOwner.SelectNext(cboFind.Text, chkMatchCase.Checked, chkWholeWord.Checked, chkRegEx.Checked))
			{
				this.mOwner.Caret.CurrentRow.Bookmarked = true;
			}

			mOwner.Selection.ClearSelection();
			//	mOwner.Caret.Position.X=x;
			//	mOwner.Caret.Position.Y=y;
			//	mOwner.ScrollIntoView ();

			cboFind.Focus();
		}

		public void FindNext()
		{
			string text = cboFind.Text;

			if (_Last != "" && _Last != text)
			{
				this.mOwner.Caret.Position.X = 0;
				this.mOwner.Caret.Position.Y = 0;
				this.mOwner.ScrollIntoView();
			}

			_Last = text;

			if (text == "")
				return;

			bool found = false;
			foreach (string s in cboFind.Items)
			{
				if (s == text)
				{
					found = true;
					break;
				}
			}
			if (!found)
				cboFind.Items.Add(text);

			mOwner.SelectNext(cboFind.Text, chkMatchCase.Checked, chkWholeWord.Checked, chkRegEx.Checked);
		}
	}
}