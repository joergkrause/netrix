using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;

namespace NetrixHtmlEditorDemo.Windows
{
    public partial class EventWindow : DockContent
    {
        [Flags]
        public enum EventType
        {
            None = 0,
            Mouse = 1,
            Key = 2,
            Control = 4,
            Element = 8,
            Window = 16
        }


        public EventWindow()
        {
            InitializeComponent();
            comboBoxEventType.SelectedIndex = 1;
        }

        private bool registerEvents = true;
        private EventType showEvents = EventType.Key | EventType.Control | EventType.Window | EventType.Element;

        private void comboBoxEventType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxEventType.SelectedItem.ToString())
            {
                case "All (Mouse, Key, Control, Window)":
                    showEvents = EventType.Mouse | EventType.Key | EventType.Control | EventType.Window | EventType.Element;
                    break;
                case "All but Mouse":
                    showEvents = EventType.Key | EventType.Control | EventType.Window | EventType.Element;
                    break;
                case "All but Key":
                    showEvents = EventType.Mouse | EventType.Control | EventType.Window | EventType.Element;
                    break;
                case "All but Mouse and Key":
                    showEvents = EventType.Control | EventType.Window | EventType.Element;
                    break;
                case "Mouse only":
                    showEvents = EventType.Mouse;
                    break;
                case "Key only":
                    showEvents = EventType.Key;
                    break;
                case "Control only":
                    showEvents = EventType.Control;
                    break;
                case "Window only":
                    showEvents = EventType.Window;
                    break;
            }
        }

        private void buttonClearList_Click(object sender, EventArgs e)
        {
            listViewEvents.Items.Clear();
        }

        private void checkBoxStop_CheckedChanged(object sender, EventArgs e)
        {
            registerEvents = checkBoxStop.Checked;
        }

        public void RegisterEvent(EventType eventType, string name, object sender, EventArgs e)
        {
            if (registerEvents && ((showEvents & eventType) == eventType))
            {
                string[] subItems = new string[3] { name, ExamineEventArgs(e), sender.GetType().Name };
                ListViewItem item = new ListViewItem(subItems);
                switch ((showEvents & eventType))
                {
                    case EventType.Control:
                        item.SubItems[0].ForeColor = Color.Red;
                        break;
                    case EventType.Key:
                        item.SubItems[0].ForeColor = Color.Blue;
                        break;
                    case EventType.Window:
                        item.SubItems[0].ForeColor = Color.Green;
                        break;
                }
                listViewEvents.Items.Add(item);
                int up = 1;
                GuruComponents.Netrix.ComInterop.Win32.SendMessage(listViewEvents.Handle, 277, (IntPtr)up, IntPtr.Zero);
            }
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

        private void EventWindow_Load(object sender, EventArgs e)
        {
            // dock outside
            if (this.Parent != null)
            {
                Form parent = this.Parent.FindForm();
                this.Location = this.PointToScreen(new Point(parent.Right + 2, parent.Top));
                this.Size = new Size(this.Width, parent.Height);
            }
        }

    }
}