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
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix;

namespace NetrixHtmlEditorDemo.Windows
{
    public partial class OutlineWindow : DockContent
    {

        private IHtmlEditor _editor;

        public OutlineWindow()
        {
            InitializeComponent();
        }

        public override void Refresh()
        {
            FillNodes();
            base.Refresh();
        }

        public void SetEditor(IHtmlEditor editor)
        {
            _editor = editor;
            ((HtmlEditor)_editor).ContentChanged += new EventHandler(OutlineWindow_ContentChanged);
        }

        void OutlineWindow_ContentChanged(object sender, EventArgs e)
        {
            FillNodes();
        }

        private void FillNodes()
        {
            IElement body = _editor.GetBodyElement();
            treeViewOutline.Nodes.Clear();
            TreeNode rootNode = treeViewOutline.Nodes.Add("DOCUMENT ROOT");
            RetrieveChildren(body, rootNode);
            treeViewOutline.ExpandAll();
        }

        private void RetrieveChildren(IElement element, TreeNode currentNode)
        {
          if (element == null) return;
            string marker = "|";
            if (element.ElementDom.FirstChild == null || element.ElementDom.FirstChild.Equals(element))
                marker = "^";
            if (element.ElementDom.LastChild == null || element.ElementDom.LastChild.Equals(element))
                marker = (marker == "^") ? "()" : "v";

            // add current node
            TreeNode curNode = currentNode.Nodes.Add(String.Format("<{0}>", element.TagName.ToUpper()));
            curNode.Tag = element;
            curNode.ToolTipText = element.OuterHtml.Trim();
            // loop further through children
            if (element.ElementDom.GetChildNodes().Count > 0)
            {
                foreach (IElement child in element.GetChildren())
                {
                    RetrieveChildren(child, curNode);
                }
            }
        }

   
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void treeViewOutline_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is IElement)
            {
                if (((IElement)e.Node.Tag).IsSelectable())
                {
                    _editor.Selection.SelectElement(((IElement)e.Node.Tag));
                }
            }
        }

        private void OutlineWindow_Load(object sender, EventArgs e)
        {

        }


    }
}