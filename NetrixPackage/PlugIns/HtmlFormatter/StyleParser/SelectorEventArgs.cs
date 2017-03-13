using System;

namespace GuruComponents.Netrix.UserInterface.StyleParser
{
	/// <summary>
	/// SelectorEventArgs are used as event arguments when the parser fires a new style found event.
	/// </summary>
	public class SelectorEventArgs : EventArgs
	{
		/// <summary>
		/// The Name of the Selector regardless the type.
		/// </summary>
		public string Name;
		/// <summary>
		/// The type of selector.
		/// </summary>
		public SelectorType Type;
		/// <summary>
		/// The content of the selector as object.
		/// </summary>
		public StyleObject Selector;
	}
}
