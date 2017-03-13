using System;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
	/// <summary>
	/// The visual appearance of <see cref="IHtmlGrid">Grid</see>.
	/// </summary>
	[Serializable()]
    public enum GridType
	{
		/// <summary>
		/// Draws exactly one point per grid position. Default mode.
		/// </summary>
		Points,
		/// <summary>
		/// Draws lines at grid positions. Fastest mode.
		/// </summary>
		Lines,
		/// <summary>
		/// Draws small cross signs at grid positions. Slowest mode.
		/// </summary>
		Cross
	}


}
