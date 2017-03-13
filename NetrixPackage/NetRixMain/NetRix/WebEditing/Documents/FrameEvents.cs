using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;
using System.Runtime.InteropServices;
using GuruComponents.Netrix.WebEditing.Documents;
using GuruComponents.Netrix.Events;

namespace GuruComponents.Netrix
{
    /// <summary>
    /// Handle events on frame windows and optional on frame's document level.
    /// </summary>
    internal class FrameEvents : WindowsEvents
    {

        public FrameEvents(Interop.IHTMLWindow2 window, IHtmlEditor editor)
            : base(window, editor, new HtmlWindow(window, editor))
        {
        }

        protected override void OnFocus()
        {
            InvokeActivate();
        }

        protected override void OnBlur()
        {
            InvokeDeActivate();
        }

        public event EventHandler Activate;

        public event EventHandler DeActivate;

        private void InvokeActivate()
        {
            if (Activate != null)
            {
                Activate(this, EventArgs.Empty);
            }
        }

        private void InvokeDeActivate()
        {
            if (DeActivate != null)
            {
                DeActivate(this, EventArgs.Empty);
            }
        }

    }

}
