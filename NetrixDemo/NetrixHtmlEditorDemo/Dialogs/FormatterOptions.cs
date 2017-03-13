using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GuruComponents.Netrix.HtmlFormatting;

namespace GuruComponents.EditorDemo.Dialogs
{
    public partial class FormatterOptions : Form
    {
        public FormatterOptions()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public IHtmlFormatterOptions OptionsObject
        {
            get { return propertyGridFO.SelectedObject as IHtmlFormatterOptions; }
            set { propertyGridFO.SelectedObject = value;  }
        }

    }
}
