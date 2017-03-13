using System;
using System.Runtime.InteropServices;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.UndoRedo
{

    /// <summary>
    /// Implements <see cref="Interop.IHTMLChangeSink"/> to receive Notify events in case of document changes.
    /// </summary>
    [ComVisible(true)]
    public class ChangeSink : Interop.IHTMLChangeSink, IDisposable
    {

        internal Interop.IHTMLChangeSink Native
        {
            get { return this as Interop.IHTMLChangeSink; }
        }

        /// <summary>
        /// Handle change event on higher level.
        /// </summary>
        public event EventHandler Change;

        #region IHTMLChangeSink Members

        void Interop.IHTMLChangeSink.Notify()
        {
            Change(this, null);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            IntPtr ptr = Marshal.GetComInterfaceForObject(this, typeof(Interop.IHTMLChangeSink));
            Marshal.Release(ptr);
        }

        #endregion
    }
}