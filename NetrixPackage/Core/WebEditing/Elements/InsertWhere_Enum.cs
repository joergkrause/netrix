using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// specifies where to insert the HTML text.
	/// </summary>
	public enum InsertWhere
	{
		/// <summary>
		/// Inserts text immediately before the object.
		/// </summary>
		BeforeBegin,
		/// <summary>
		/// Inserts text after the start of the object but before all other content in the object.
		/// </summary>
		AfterBegin,
		/// <summary>
		/// Inserts text immediately before the end of the object but after all other content in the object.
		/// </summary>
		BeforeEnd,
		/// <summary>
		/// Inserts text immediately after the end of the object.
		/// </summary>
		AfterEnd
	}
}
