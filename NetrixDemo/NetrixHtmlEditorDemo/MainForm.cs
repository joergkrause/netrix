using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RibbonLib;
using WeifenLuo.WinFormsUI.Docking;
using NetrixHtmlEditorDemo.Windows;
using System.IO;
using RibbonLib.Controls;
using GuruComponents.EditorDemo.Commands;
using RibbonLib.Controls.Events;

namespace NetrixHtmlEditorDemo
{
    public partial class MainForm : Form
    {
        // Docklayout
        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent;
        private HelpAndExplainWindow m_helpExplorer = new HelpAndExplainWindow();
        private PropertyWindow m_propertyWindow = new PropertyWindow();
        private ToolWindow m_toolbox = new ToolWindow();
        private OutlineWindow m_outlineWindow = new OutlineWindow();
        private HtmlDoc m_docWindow= null;

        public MainForm()
        {
            InitializeComponent();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            // load ribbon controls
            AttachRibbonEvents();
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(HelpAndExplainWindow).ToString())
                return m_helpExplorer;
            else if (persistString == typeof(PropertyWindow).ToString())
                return m_propertyWindow;
            else if (persistString == typeof(ToolWindow).ToString())
                return m_toolbox;
            else
            {
                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                if (parsedStrings[0] != typeof(HtmlDoc).ToString())
                    return null;
                // assure that we deal with just one instance only
                m_docWindow = EditorDocument;
                if (parsedStrings[1] != string.Empty)
                    m_docWindow.FileName = parsedStrings[1];
                if (parsedStrings[2] != string.Empty)
                    m_docWindow.Text = parsedStrings[2];

                if (String.IsNullOrEmpty(m_docWindow.FileName) || m_docWindow.Text.Equals("New Document"))
                {
                    ShowCommonHelp();
                }

                return m_docWindow;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            dockPanel.Parent.Top = panel2.Height + 2;
            dockPanel.Parent.Height = this.Height - panel2.Height - 34;
            if (File.Exists(configFile))
            {
                dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
            }
            else
            {
                // preload default windows
                m_helpExplorer.Show(dockPanel);
                m_propertyWindow.Show(dockPanel);
                m_toolbox.Show(dockPanel);
                ShowCommonHelp();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (m_bSaveLayout)
                dockPanel.SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);

        }
    }
}
