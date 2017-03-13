using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// Each f element defines a single value as the result of the evaluation of an expression.
	/// </summary>
	/// <remarks>
    /// Each f element defines a single value as the result of the evaluation of an expression. The expression is defined by the cdata content of the eqn attribute and has the general form of an operation followed by up to three arguments, which may be adjust handle values, the results of earlier guide formulas, fixed numbers or pre-defined values.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class FElement : VmlBaseElement
	{

		public FElement(IHtmlEditor editor) : base("v:f", editor)
		{
		}

        internal FElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
