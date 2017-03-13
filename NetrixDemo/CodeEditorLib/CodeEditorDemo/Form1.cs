using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeEditorDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                codeEditorControl1.Open(openFileDialog1.FileName);
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeEditorControl1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeEditorControl1.Redo();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeEditorControl1.Copy();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeEditorControl1.Cut();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeEditorControl1.Paste();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeEditorControl1.ResetText();
        }

        private void removeCurrentRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeEditorControl1.RemoveCurrentRow();
        }

        private void resetSplitViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeEditorControl1.ResetSplitview();
        }

        private void gotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeEditorControl1.ShowGotoLine();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeEditorControl1.ShowFind();
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeEditorControl1.ShowReplace();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void selectHightliteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "Syntax|*.syn|All|*.*";
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                codeEditorControl1.Document.SyntaxFile = openFileDialog2.FileName;
            }
        }
    }
}