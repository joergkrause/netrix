using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This sub-element may appear inside a shape or a shapetype to define formulas.
	/// </summary>
	/// <remarks>
    /// This sub-element may appear inside a shape or a shapetype to define formulas that can vary the path of a shape, 
    /// its inscribed text rectangles, and connection sites.  
    /// Formula values change as the adj values change on the shape.  Formulas can reference other formulas 
    /// defined earlier in the same formulas element.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class FormulasElement : VmlBaseElement
	{

		public FormulasElement(IHtmlEditor editor) : base("v:formulas", editor)
		{
		}

        internal FormulasElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
