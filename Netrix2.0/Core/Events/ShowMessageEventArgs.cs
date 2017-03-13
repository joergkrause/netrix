using System;
using System.Windows.Forms;
using System.Drawing;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This class defines data for the <see cref="GuruComponents.Netrix.WebEditing.Documents.IHtmlWindow.ScriptMessage">ScriptMessage</see> event.
    /// </summary>
    /// <remarks>
    /// This class allows to cancel the popup box.
    /// </remarks>
    public class ShowMessageEventArgs : System.ComponentModel.CancelEventArgs
    {
        private string msg;
        private string cpt;
        private Interop.MB buttonflag;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="dwType"></param>
        public ShowMessageEventArgs(string message, string caption, uint dwType)
        {
            msg = message;
            cpt = caption;
            buttonflag = (Interop.MB) dwType;
        }

        /// <summary>
        /// The Message in the script alert box.
        /// </summary>
        public string Message
        {
            get
            {
                return msg;
            }
        }

        /// <summary>
        /// The caption for the script alert box.
        /// </summary>
        public string Caption
        {
            get
            {
                return cpt;
            }
        }

    }
}