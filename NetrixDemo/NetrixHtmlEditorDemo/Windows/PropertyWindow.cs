using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using GuruComponents.Netrix;

namespace NetrixHtmlEditorDemo.Windows
{
    public partial class PropertyWindow : DockContent
	{

        private IHtmlEditor _editor;

		public PropertyWindow()
		{
			InitializeComponent();
            
		}

        public void SetEditor(IHtmlEditor editor)
        {
            _editor = editor;
            GuruComponents.Netrix.UserInterface.ResourceManager.Initialize(_editor.GetActiveDocument(true), "de-DE");
        }

		public void SetObject(object obj)
		{
            propertyGrid.SelectedObject = obj;
            if (obj != null)
            {
                propertyGrid.Site = ((IComponent)obj).Site;
                labelObject.Text = obj.GetType().Name;
            }
            else
            {
                labelObject.Text = "No Object";
            }
		}

	}
}