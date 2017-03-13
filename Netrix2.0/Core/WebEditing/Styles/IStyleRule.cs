using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{
    /// <summary>
    /// Allows access to the specific style rule and there definition.
    /// </summary>
    /// <remarks>
    /// StyleRule class implements IStyleRule interface.
    /// <para>
    /// See StyleRule class for more details.
    /// </para>
    /// </remarks>
    public interface IStyleRule
    {
        /// <summary>
        /// Gets or sets the name of the rule, including the preceding type sign.
        /// </summary>
		string RuleName { get; set; }
        
		/// <summary>
        /// Gives access to the style definition.
        /// </summary>
		IStyle StyleDefinition { get; }
    }
}
