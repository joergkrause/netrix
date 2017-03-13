using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using GuruComponents.Netrix.Events;

namespace NetrixHtmlEditorDemo.Windows
{
    public partial class ElementEventWindow : DockContent
    {


        public ElementEventWindow()
        {
            InitializeComponent();
        }

        private void buttonClearList_Click(object sender, EventArgs e)
        {
            listViewEvents.Items.Clear();
        }

        public void RegisterEvent(EventGroup eventGroup, string name, object sender, EventArgs e)
        {
            string[] subItems = new string[3] { name, ExamineEventArgs(e), sender.GetType().Name };
            ListViewItem item = new ListViewItem(subItems);
            switch (eventGroup)
            {
                case EventGroup.Control:
                    item.SubItems[0].ForeColor = Color.Red;
                    break;
                case EventGroup.Edit:
                    item.SubItems[0].ForeColor = Color.Blue;
                    break;
                case EventGroup.Mouse:
                    item.SubItems[0].ForeColor = Color.Green;
                    break;
                case EventGroup.Move:
                    item.SubItems[0].ForeColor = Color.Gray;
                    break;
                case EventGroup.Drag:
                    item.SubItems[0].ForeColor = Color.Purple;
                    break;
                case EventGroup.Focus:
                    item.SubItems[0].ForeColor = Color.Brown;
                    break;
                case EventGroup.Resize:
                    item.SubItems[0].ForeColor = Color.DarkOliveGreen;
                    break;
            }
            listViewEvents.Items.Add(item);
            int up = 1;
            GuruComponents.Netrix.ComInterop.Win32.SendMessage(listViewEvents.Handle, 277, (IntPtr)up, IntPtr.Zero);
        }

        private string ExamineEventArgs (EventArgs e)
        {
            PropertyInfo[] pi = e.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<string> infoData = new List<string>();
            if (pi != null && pi.Length > 0)
            {
                foreach (PropertyInfo i in pi)
                {
                    infoData.Add(String.Format("{0}={1}", i.Name, i.GetValue(e, null)));
                }
            }
            return String.Join(", ", infoData.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}