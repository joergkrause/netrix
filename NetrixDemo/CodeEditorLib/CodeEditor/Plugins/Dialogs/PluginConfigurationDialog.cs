using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace GuruComponents.CodeEditor.Library.Plugins.Dialogs
{
    public partial class PluginConfigurationDialog : Form
    {
        public PluginConfigurationDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (PluginLoadInfo current in 
                PluginApplication.Istance.Manager.Plugins)
            {
                AddItem(current);
            }
        }

        void AddItem(PluginLoadInfo pluginInfo)
        {
            ListViewItem item = new ListViewItem(pluginInfo.ClassName);
            ListViewItem.ListViewSubItem sub1 = new ListViewItem.ListViewSubItem(item, pluginInfo.Filename);
            ListViewItem.ListViewSubItem sub2 = new ListViewItem.ListViewSubItem(item, pluginInfo.Loaded.ToString());

            item.SubItems.Add(sub1);
            item.SubItems.Add(sub2);

            item.Tag = pluginInfo;

            lstPlugins.Items.Add(item);
        }

        private void btnBrowsePlugin_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();

            opf.Filter = "File Assembly .NET|*.dll";

            if (opf.ShowDialog(this) == DialogResult.OK)
            {
                Assembly assembly = Assembly.LoadFile(opf.FileName);

                Type[] tps = assembly.GetTypes();

                bool havePlugin = false;

                foreach (Type current in tps)
                {
                    if (current.IsSubclassOf(PluginApplication.Istance.Manager.PluginBaseType))
                    {
                        havePlugin = true;                        

                        if (!opf.FileName.Contains(PluginApplication.Istance.
                            Manager.AssemblySearchPath))
                        {
                            File.Copy(opf.FileName,
                                Path.Combine(PluginApplication.Istance.Manager.AssemblySearchPath,
                                Path.GetFileName(opf.FileName)),true);
                        }

                        string file = Path.GetFileName(opf.FileName);

                        string className = current.FullName;

                        PluginLoadInfo info = new PluginLoadInfo(false, file, className);

                        PluginApplication.Istance.Manager.Plugins.Add(info.ClassName, info);

                        AddItem(info);
                    }
                }

                if (!havePlugin)
                {
                    MessageBox.Show("The assembly not contain a valid plugin");
                }
            }
        }

        private void btnLoadAtStart_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem current in lstPlugins.SelectedItems)
            {
                PluginLoadInfo info = (PluginLoadInfo)current.Tag;

                info.Loaded = !info.Loaded;

                current.SubItems[2].Text = info.Loaded.ToString();
            }
        }

        private void btnLoadNow_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem current in lstPlugins.SelectedItems)
            {
                PluginLoadInfo info = (PluginLoadInfo)current.Tag;

                PluginApplication.Istance.Manager.LoadPlugin(info);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            PluginApplication.Istance.Manager.Save();
        }
    }
}