using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.UI.WebControls;

namespace GuruComponents.EditorDemo.Dialogs
{
    public partial class InsertTable : Form
    {
        public InsertTable()
        {
            InitializeComponent();
        }

        private void aeroButtonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        public int Rows
        {
            get
            {
                return Convert.ToInt32(aeroTextBoxRow.Text);
            }
        }

        public int Columns
        {
            get
            {
                return Convert.ToInt32(aeroTextBoxCol.Text);
            }
        }

        public int CellPadding
        {
            get
            {
                int cp = 0;
                if (Int32.TryParse(aeroTextBoxPadding.Text, out cp))
                    return cp;
                else
                    return 0;
            }
        }

        public int CellSpacing
        {
            get
            {
                int cp = 0;
                if (Int32.TryParse(aeroTextBoxSpacing.Text, out cp))
                    return cp;
                else
                    return 0;
            }
        }

        public Unit BorderWidth
        {
            get
            {
                return unitEditorBorderSize.Unit;
            }
        }

    }
}
