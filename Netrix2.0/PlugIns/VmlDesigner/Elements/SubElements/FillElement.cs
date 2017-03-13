using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// Fills a path.
	/// </summary>
	/// <remarks>
    /// This sub-element may appear inside a shape, shapetype, background or most predefined shape elements to describe how the path should be filled if something beyond a solid color fill is desired.  The attributes of the fill element can used to describe a powerful set of image or gradient based fill patterns.   Extensions to the VML fill definition may be encoded as sub-elements of fill.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class FillElement : VmlBaseElement
	{

		public FillElement(IHtmlEditor editor) : base("v:fill", editor)
		{
		}

        internal FillElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
