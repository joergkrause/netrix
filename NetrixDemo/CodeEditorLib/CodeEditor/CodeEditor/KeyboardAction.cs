

using System.Windows.Forms;

namespace GuruComponents.CodeEditor.CodeEditor
{
	/// <summary>
	/// Delegate used when triggering keyboard actions
	/// </summary>
	public delegate void ActionDelegate();

	/// <summary>
	/// Instances of this class represents a relation between pressed keys and a delegate
	/// </summary>
	public class KeyboardAction
	{
		/// <summary>
		/// Determines if "SHIFT" be pressed to invoke this action
		/// </summary>
		private bool _Shift = false;

		/// <summary>
		/// Determines if "ALT" be pressed to invoke this action
		/// </summary>
		private bool _Alt = false;

		/// <summary>
		/// Determines if "CONTROL" be pressed to invoke this action
		/// </summary>
		private bool _Control = false;

		/// <summary>
		/// Determines if this action allowed in readonly mode
		/// </summary>
		private bool _AllowReadOnly = false;

		/// <summary>
		/// Determines what key to associate with the action
		/// </summary>
		private Keys _Key = 0;

		/// <summary>
		/// Instance to a delegate to be invoked for this action
		/// </summary>
		private ActionDelegate _Action = null;

		public bool Shift
		{
			get { return _Shift; }
			set { _Shift = value; }
		}

		public bool Alt
		{
			get { return _Alt; }
			set { _Alt = value; }
		}

		public bool Control
		{
			get { return _Control; }
			set { _Control = value; }
		}

		public bool AllowReadOnly
		{
			get { return _AllowReadOnly; }
			set { _AllowReadOnly = value; }
		}

		public Keys Key
		{
			get { return _Key; }
			set { _Key = value; }
		}

		public ActionDelegate Action
		{
			get { return _Action; }
			set { _Action = value; }
		}


		public KeyboardAction()
		{
		}

		public KeyboardAction(Keys key, bool shift, bool control, bool alt, bool allowreadonly, ActionDelegate actionDelegate)
		{
			this.Key = key;
			this.Control = control;
			this.Alt = alt;
			this.Shift = shift;
			this.Action = actionDelegate;
			this.AllowReadOnly = allowreadonly;
		}
	}
}