using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using GuruComponents.Netrix;
using GuruComponents.Netrix.WebEditing.Elements;
using System.IO;
using GuruComponents.Netrix.WebEditing.DragDrop;

namespace NetrixHtmlEditorDemo.Windows
{
    public partial class ToolWindow : DockContent
    {

        private IHtmlEditor _editor;

        public ToolWindow()
        {
            InitializeComponent();
        }

        private void button_StartDragDrop(object sender, MouseEventArgs e)
        {
            IElement el = null;
            string tagName = ((Label)sender).Tag.ToString();
            DataObject d = new DataObject();
            switch (tagName)
            {
                case "A":
                    el = _editor.CreateElement("A");
                    goto case "INPUT";
                case "IMG":
                    el = new ImageElement(_editor);
                    goto case "INPUT";
                case "BR":
                    el = new BreakElement(_editor);
                    goto case "INPUT";
                case "TEXTAREA":
                    el = _editor.CreateElement("TEXTAREA");
                    ((TextAreaElement)el).cols = 10;
                    ((TextAreaElement)el).rows = 3;
                    d.SetData(typeof(IElement), el);
                    break;
                case "LISTBOX":
                    el = _editor.CreateElement("SELECT");
                    ((SelectElement)el).multiple = true;
                    ((SelectElement)el).size = 3;
                    d.SetData(typeof(IElement), el);
                    break;
                case "HR":
                    el = _editor.CreateElement("HR");
                    d.SetData(typeof(IElement), el);
                    break;
                default:
                    MessageBox.Show("This toolbox item has not been implemented.\n\nCheckout 'ToolWindow.cs' in demo code for implementation details.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case "BUTTON":
                case "INPUT button":
                    el = new InputButtonElement(_editor);
                    goto case "INPUT";
                case "INPUT image":
                    el = new InputImageElement(_editor);
                    goto case "INPUT";
                case "INPUT submit":
                    el = new InputSubmitElement(_editor);
                    goto case "INPUT";
                case "INPUT checkbox":
                    el = new InputCheckboxElement(_editor);
                    goto case "INPUT";
                case "INPUT radio":
                    el = new InputRadioElement(_editor);
                    goto case "INPUT";
                case "INPUT text":
                    el = new InputTextElement(_editor);
                    goto case "INPUT";
                case "INPUT password":
                    el = new InputPasswordElement(_editor);
                    goto case "INPUT";
                case "INPUT hidden":
                    el = new InputHiddenElement(_editor);
                    goto case "INPUT";
                case "INPUT":
                    d.SetData(typeof(IElement), el);
                    break;
            }

            ((Label)sender).DoDragDrop(d, DragDropEffects.Copy);
        }

        public void SetEditorReference(IHtmlEditor editor)
        {
            _editor = editor;
            ((HtmlEditor)_editor).DragDrop += new DragEventHandler(ToolWindow_DragDrop);
        }

        void ToolWindow_DragDrop(object sender, DragEventArgs e)
        {
            foreach (string format in e.Data.GetFormats())
            {
                switch (format)
                {
                    case "GuruComponents.Netrix.WebEditing.Elements.IElement":
                        // usually elements are dropped automatically. This code shows how to handle the element immediately.
                        // here only Anchor elements are handled
                        object o = e.Data.GetData(typeof(IElement));
                        if (o is AnchorElement)
                        {
                            AnchorElement a = (AnchorElement)o;
                            a.href = "http://www.netrixcomponent.net";
                            a.InnerText = "Netrix Component";
                        }
                        break;

                }
            }
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            using (Stream s = this.GetType().Assembly.GetManifestResourceStream("GuruComponents.EditorDemo.Resources.Html.DragDrop.htm"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    _editor.LoadHtml(sr.ReadToEnd());
                }
            }
        }


    }
}