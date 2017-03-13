using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Windows.Forms;

namespace GuruComponents.Netrix.Designer
{
	/// <summary>
	/// UIService.
	/// </summary>
	public class UIService : System.Windows.Forms.Design.IUIService, GuruComponents.Netrix.Designer.IUIService
	{

        IHtmlEditor editor;

        public UIService(IHtmlEditor editor)
        {
            this.editor = editor;
            this.canShowComponentEditor = true;
        }        

        internal static UIService GetInstance(IHtmlEditor editor)
        {
            return editor.ServiceProvider.GetService(typeof(System.Windows.Forms.Design.IUIService)) as UIService;
        }

        public event EventHandler ShowComponentEditor;
        public event EventHandler GetDialogOwner;
        public event EventHandler UIDirty;
        public event EventHandler ShowToolWindow;

        private ShowErrorDelegate showErrorDialog;
        private ShowMessageDelegate showMessageDialog;
        private ShowDialogDelegate showDialogDialog;

        public ShowDialogDelegate ShowDialogDialog
        {
            get { return showDialogDialog; }
            set { showDialogDialog = value; }
        }

        public ShowMessageDelegate ShowMessageDialog
        {
            get { return showMessageDialog; }
            set { showMessageDialog = value; }
        }

        public ShowErrorDelegate ShowErrorDialog
        {
            get { return showErrorDialog; }
            set { showErrorDialog = value; }
        }

        private bool canShowComponentEditor;

        public bool CanShowComponentEditor
        {
            get { return canShowComponentEditor; }
            set { canShowComponentEditor = value; }
        }


        #region IUIService Members

        bool System.Windows.Forms.Design.IUIService.CanShowComponentEditor(object component)
        {
            return CanShowComponentEditor;
        }

        System.Windows.Forms.IWin32Window System.Windows.Forms.Design.IUIService.GetDialogOwnerWindow()
        {
            if (GetDialogOwner != null)
            {
                GetDialogOwner(this, null);
                return null;
            }
            return null;
        }

        void System.Windows.Forms.Design.IUIService.SetUIDirty()
        {
            if (UIDirty != null)
            {
                UIDirty(this, null);
            }
        }

        bool System.Windows.Forms.Design.IUIService.ShowComponentEditor(object component, System.Windows.Forms.IWin32Window parent)
        {
            if (ShowComponentEditor != null)
            {
                CancelEventArgs args = new CancelEventArgs();
                ShowComponentEditor(this, args);
                return args.Cancel;
            }
            return false;
        }

        System.Windows.Forms.DialogResult System.Windows.Forms.Design.IUIService.ShowDialog(System.Windows.Forms.Form form)
        {
            if (ShowDialogDialog != null)
            {
                return ShowDialogDialog(form);
            }
            else
            {
                return form.ShowDialog();
            }            
        }

        void System.Windows.Forms.Design.IUIService.ShowError(Exception ex, string message)
        {
            if (ShowErrorDialog != null)
            {
                ShowErrorDialog(ex, message);
            }
        }

        void System.Windows.Forms.Design.IUIService.ShowError(Exception ex)
        {
            if (ShowErrorDialog != null)
            {
                ShowErrorDialog(ex, "");
            }
        }

        void System.Windows.Forms.Design.IUIService.ShowError(string message)
        {
            if (ShowErrorDialog != null)
            {
                ShowErrorDialog(null, message);
            }
        }

        System.Windows.Forms.DialogResult System.Windows.Forms.Design.IUIService.ShowMessage(string message, string caption, System.Windows.Forms.MessageBoxButtons buttons)
        {
            if (ShowMessageDialog != null)
            {
                return ShowMessageDialog(message, caption, buttons);
            }
            return DialogResult.OK;
        }

        void System.Windows.Forms.Design.IUIService.ShowMessage(string message, string caption)
        {
            if (ShowMessageDialog != null)
            {
                ShowMessageDialog(message, caption, MessageBoxButtons.OK);
            }
        }

        void System.Windows.Forms.Design.IUIService.ShowMessage(string message)
        {
            if (ShowMessageDialog != null)
            {
                ShowMessageDialog(message, "", MessageBoxButtons.OK);
            }
        }

        bool System.Windows.Forms.Design.IUIService.ShowToolWindow(Guid toolWindow)
        {
            if (ShowToolWindow != null)
            {
                EventArgs args = new EventArgs();
                ShowToolWindow(this, null);
                return true; // TODO: args.Arg
            }
            return false;
        }

        IDictionary System.Windows.Forms.Design.IUIService.Styles
        {
            get 
            { 
                Hashtable ht = new Hashtable();
                ht.Add("DialogFont", new Font("Arial", 12.0F));
                ht.Add("HighlightColor", Color.Red);
                return ht; 
            }
        }

        #endregion
    }
}
