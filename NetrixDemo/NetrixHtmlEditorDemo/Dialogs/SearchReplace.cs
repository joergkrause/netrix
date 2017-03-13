using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GuruComponents.EditorDemo.Dialogs
{
    public partial class SearchReplace : Form
    {
        public SearchReplace()
        {
            InitializeComponent();
        }

        private void SearchReplace_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = buttonOK.DialogResult;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = buttonCancel.DialogResult;
            Close();
        }

        private bool _useReplace;
        public bool UseReplace
        {
            get
            {
                return _useReplace;
            }
            set
            {
                _useReplace = value;
                if (_useReplace)
                {
                    groupBox1.Text = "Search & Replace";
                }
                else
                {
                    groupBox1.Text = "Search";
                }
                textBoxReplace.Visible = _useReplace;
                labelReplace.Visible = _useReplace;
            }
        }

        private void SearchReplace_Load(object sender, EventArgs e)
        {
            UseReplace = _useReplace;
        }

        private void checkBoxReplace_CheckedChanged(object sender, EventArgs e)
        {
            UseReplace = checkBoxReplace.Checked;
        }

        public bool WholeWord
        {
            get { return checkBoxWholeWord.Checked;  }
        }

        public bool CaseSensitive
        {
            get { return checkBoxCaseSensitive.Checked;  }
        }

        public bool SearchUp
        {
            get { return checkBoxSearchUp.Checked; }
        }

        public string SearchFor
        {
            get { return textBoxSearch.Text.Trim();  }
        }

        public string ReplaceWith
        {
            get { return textBoxReplace.Text.Trim();  }
        }

        public event EventHandler NextWord;

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (NextWord != null)
            {
                NextWord(sender, e);
            }
        }
    }
}
