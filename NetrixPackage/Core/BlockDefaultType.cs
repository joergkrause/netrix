using System;

namespace GuruComponents.Netrix
{
	/// <summary>
	/// Type of paragraphs (block) insertion.
	/// </summary>
	/// <remarks>
	/// To use this enumaration the property <see cref="GuruComponents.Netrix.IHtmlEditor.BlockDefault">BlockDefault</see>
	/// must be set before the document is loaded. The purpose of this definition is to change the
	/// behavior of the ENTER key. The behavior of the various paragraph insertion method will not change.
	/// <seealso cref="GuruComponents.Netrix.IHtmlEditor.BlockDefault"/>
	/// </remarks>
	public enum BlockDefaultType
	{
        /// <summary>
        /// Hitting enter will insert DIV. To insert a break type Shft-Enter.
        /// </summary>
        DIV,
        /// <summary>
        /// Hitting enter will insert P. To insert a break type Shft-Enter.
        /// </summary>
        P
	}
}
