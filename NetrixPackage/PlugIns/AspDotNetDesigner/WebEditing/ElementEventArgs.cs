using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.AspDotNetDesigner
{
    internal class ElementEventArgs : EventArgs
    {

        private Interop.IHTMLEventObj e;

        internal ElementEventArgs(Interop.IHTMLEventObj e)
        {
            this.e = e;
        }

        internal Interop.IHTMLEventObj EventObj
        {
            get { return e; }
        }
    }
}
