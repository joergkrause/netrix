using System;
using System.Drawing;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.Events;
using System.Web.UI;
using System.Runtime.InteropServices;

namespace GuruComponents.Netrix.WebEditing.Documents
{
	/// <summary>
	/// HtmlWindow allows basic access to window and document related features outside the designer scope.
	/// </summary>
	/// <remarks>
	/// The class is being used to access scripting sections, execute JavaScript and open embedded 
	/// scripting dialogs.
	/// </remarks>
	public class HtmlTimer
	{
		private Interop.IHTMLWindow2 window;
		private Interop.IHTMLDocument2 doc;
		private IHtmlEditor editor;

		internal HtmlTimer(Interop.IHTMLWindow2 window, IHtmlEditor editor)
		{
			this.editor = editor;
			try
			{
				this.window = window.parent;
				doc = window.document;            
			}
			catch
			{
			}
		}

		private Interop.IHTMLWindow3 window3
		{
			get
			{
				return ((Interop.IHTMLWindow3) this.window);
			}
		}

		private Interop.IHTMLWindow4 window4
		{
			get
			{
				return ((Interop.IHTMLWindow4) this.window);
			}
		}

        public void SetInterval(int msec, Delegate callBack)
        {
            window3.setInterval(callBack, msec, "");
        }

        public void SetCallBack(int msec, HandlerDelegate callBack)
        {
            HandlerClass hc = new HandlerClass();
            hc.Handler = callBack;
            IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            Marshal.StructureToPtr(hc, p, false);
            window3.setTimeout(p, msec, "JScript");
        }

    }

    public delegate void HandlerDelegate(object sender);

    [StructLayout(LayoutKind.Sequential)]
    public struct HandlerClass
    {

        public HandlerDelegate Handler;

        [DispId(0)]
        public void Call()
        {
            Handler(this);
        }

    }

}