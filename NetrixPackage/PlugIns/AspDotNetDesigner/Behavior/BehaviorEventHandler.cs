using System;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace GuruComponents.Netrix.AspDotNetDesigner
{

    /// <summary>
    /// Generic handler for all behavior generated element events.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void BehaviorEventHandler(object sender, BehaviorEventArgs e);

    /// <summary>
    /// Event arguments for generic element events.
    /// </summary>
    public class BehaviorEventArgs : EventArgs
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="control"></param>
        public BehaviorEventArgs(Control control)
        {
            CurrentElement = control; // as IElement;
        }
        /// <summary>
        /// element
        /// </summary>
        public Control CurrentElement { get; set; }

        //public Interop.IHTMLElement PeerElement;
    }

}
