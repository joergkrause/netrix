using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This sub-element may appear inside a shape or a shapetype to define user interface elements.
	/// </summary>
	/// <remarks>
	/// This sub-element may appear inside a shape or a shapetype to define user interface elements which can vary the adj values on the shape, thereby changing the value of formulas and the rendering of a path based on formulas and adj values.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class HandleElement : VmlBaseElement
	{

		public HandleElement(IHtmlEditor editor) : base("v:handle", editor)
		{
		}

        internal HandleElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
