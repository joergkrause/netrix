using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GuruComponents.Netrix;
using GuruComponents.Netrix.PlugIns;

namespace GuruComponents.EditorDemo.Dialogs
{
    public partial class Properties : Form
    {

        private IHtmlEditor _editor;


        public Properties()
        {
            InitializeComponent();
        }

        public void SetEditor(IHtmlEditor editor)
        {
            _editor = editor;
            _editor.ReadyStateComplete += new EventHandler(_editor_ReadyStateComplete);
            propertyGrid1.SelectedObject = _editor;
        }

        void _editor_ReadyStateComplete(object sender, EventArgs e)
        {
            LoadPlugIns();
        }

        private void LoadPlugIns()
        {
            foreach (IPlugIn pi in _editor.RegisteredPlugIns)
            {
                listBoxPlugIns.Items.Add(pi.Name);
            }
        }

        private void radioButtonPlugin_CheckedChanged(object sender, EventArgs e)
        {
            listBoxPlugIns.Enabled = true;
            listBoxPlugIns.Items.Clear();
            if (_editor.IsReady)
            {
                LoadPlugIns();
            }
        }

        private void listBoxPlugIns_SelectedIndexChanged(object sender, EventArgs e)
        {
            IPlugIn pi = _editor.RegisteredPlugIns[listBoxPlugIns.SelectedIndex];
            propertyGrid1.SelectedObject = pi;
        }

        private void radioButtonHtml_CheckedChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = _editor;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
