using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GuruComponents.EditorDemo.Dialogs
{
    public partial class Spellchecker : Form
    {

        public Spellchecker()
        {
            InitializeComponent();
        }

        private void buttonProceed_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Abort;
            Action(sender, e);
        }

        private void buttonIgnore_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Ignore;
            Action(sender, e);
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Action(sender, e);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        public event EventHandler Action;

        public string WrongWord
        {
            get { return textBox1.Text; }
            set 
            {
                if (InvokeRequired)
                {
                    textBox1.Invoke(new MethodInvoker(delegate { textBox1.Text = value; }));
                }else {
                    textBox1.Text = value;
                }
            }
        }

        public string NewWord
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        public List<string> Suggestions
        {
            set
            {
                if (InvokeRequired)
                {
                    listBoxSuggestions.Invoke(new MethodInvoker(delegate { listBoxSuggestions.Items.Clear(); }));
                    listBoxSuggestions.Invoke(new MethodInvoker(delegate { listBoxSuggestions.Items.AddRange(value.ToArray()); }));
                }
                else
                {
                    listBoxSuggestions.Items.Clear();
                    listBoxSuggestions.Items.AddRange(value.ToArray());
                }
            }
        }

        private void listBoxSuggestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSuggestions.SelectedIndex >= 0)
            {
                textBox2.Text = listBoxSuggestions.SelectedItem.ToString();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            buttonChange.Enabled = (textBox2.Text.Trim().Length > 0);
        }

    }
}
