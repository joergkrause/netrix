
using System;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using GuruComponents.CodeEditor.CodeEditor.Syntax;

namespace GuruComponents.CodeEditor.CodeEditor
{
	/// <summary>
	/// Designer for the SyntaxBoxControl
	/// </summary>
	public class CodeEditorDesigner : ControlDesigner
	{
		public CodeEditorDesigner() : base()
		{
		}

		protected ISelectionService SelectionService
		{
			get { return (ISelectionService) this.GetService(typeof (ISelectionService)); }
		}

		protected void OnActivate(object s, EventArgs e)
		{
		}

		protected virtual IDesignerHost DesignerHost
		{
			get { return (IDesignerHost) base.GetService(typeof (IDesignerHost)); }
		}

        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            if (DesignerHost != null)
            {
                DesignerTransaction trans = DesignerHost.CreateTransaction("Adding Syntaxdocument");
                SyntaxDocument sd = DesignerHost.CreateComponent(typeof(SyntaxDocument)) as SyntaxDocument;

                CodeEditorControl sb = this.Control as CodeEditorControl;
                sb.Document = sd;
                trans.Commit();
            }
        }
	}
}