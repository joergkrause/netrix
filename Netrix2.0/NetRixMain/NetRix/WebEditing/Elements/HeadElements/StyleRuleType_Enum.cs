using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// Types of Style Rules (Selectors)
	/// </summary>
	public enum StyleRuleType
	{
        /// <summary>
        /// All element variants, like "B", "B > P", "B P", "B+P" and pseudos for elements, like "A:hover"
        /// </summary>
        Standard    = 0,
        /// <summary>
        /// Classes, like ".class"
        /// </summary>
        Class       = 1,
        /// <summary>
        /// IDs, like "#id"
        /// </summary>
        ID          = 2,
        /// <summary>
        /// Pseudor definitions, like @block
        /// </summary>
        Pseudo      = 3,
        /// <summary>
        /// The asterisk *. Not followed by any text.
        /// </summary>
        Global      = 4
	}
}
