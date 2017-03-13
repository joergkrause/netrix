using System;
using System.Windows.Forms;

namespace GuruComponents.CodeEditor.Library.Win32
{

	#region params

	public class NativeMessageArgs : EventArgs
	{
		public Message Message;
		public bool Cancel;
	}

	public delegate void NativeMessageHandler(object s, NativeMessageArgs e);

	#endregion

	public class NativeSubclasser : NativeWindow
	{
		public event NativeMessageHandler Message = null;

		protected virtual void OnMessage(NativeMessageArgs e)
		{
			if (Message != null)
				Message(this, e);
		}

		public NativeSubclasser()
		{
		}

		public NativeSubclasser(Control Target)
		{
			this.AssignHandle(Target.Handle);
			Target.HandleCreated += new EventHandler(this.Handle_Created);
			Target.HandleDestroyed += new EventHandler(this.Handle_Destroyed);
		}

		private void Handle_Created(object o, EventArgs e)
		{
			this.AssignHandle(((Control) o).Handle);
		}

		private void Handle_Destroyed(object o, EventArgs e)
		{
			this.ReleaseHandle();
		}

		public NativeSubclasser(IntPtr hWnd)
		{
			this.AssignHandle(hWnd);
		}

		public void Detatch()
		{
			//	this.ReleaseHandle ();
		}

		protected override void WndProc(ref Message m)
		{
			try
			{
				NativeMessageArgs e = new NativeMessageArgs();
				e.Message = m;
				e.Cancel = false;

				OnMessage(e);

				if (!e.Cancel)
					base.WndProc(ref m);
			}
			catch (Exception x)
			{
				Console.WriteLine(x.Message);
			}
		}
	}
}