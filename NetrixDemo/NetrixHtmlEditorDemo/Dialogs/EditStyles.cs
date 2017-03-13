using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.UserInterface.StyleParser;

namespace GuruComponents.EditorDemo.Dialogs
{
    public partial class EditStyles : Form
    {
        public EditStyles()
        {
            InitializeComponent();
        }

public string CssText
{
    get
    {
        return styleControl1.StyleString;
    }
    set
    {
        styleControl1.StyleString = value;
    }
}

public IElement Element
{
    set
    {
        if (value != null)
        {
            labelElement.Text = value.TagName.ToUpper();
            styleControl1.Enabled = true;
            textBoxStyles.Enabled = false;
        }
        else
        {
            labelElement.Text = "";
            styleControl1.Enabled = false;
            textBoxStyles.Enabled = false;
        }
    }
}

private void buttonOK_Click(object sender, EventArgs e)
{
    DialogResult = System.Windows.Forms.DialogResult.OK;
    Close();
}

private void buttonCancel_Click(object sender, EventArgs e)
{
    DialogResult = System.Windows.Forms.DialogResult.Cancel;
    Close();

}

private void styleControl1_ParserReady(object sender, SelectorEventArgs e)
{
    textBoxStyles.Text = e.Selector.ToString();
}

private void textBoxStyles_TextChanged(object sender, EventArgs e)
{
    // you may handle it both ways here
}

private void styleControl1_ContentChanged(object sender, EventArgs e)
{
    textBoxStyles.Text = styleControl1.StyleString;
}
    }
}
