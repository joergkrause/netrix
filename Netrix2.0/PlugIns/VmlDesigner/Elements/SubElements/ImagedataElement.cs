using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// Native object class for v:imagedata element.
	/// </summary>
	/// <remarks>
	/// This sub-element may appear inside a shape or a shapetype to define a picture to be rendered on top of a shape. There is also a top-level element, image, which has these attributes, along with most of the same attributes as shape.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class ImagedataElement : VmlBaseElement
	{

		public ImagedataElement(IHtmlEditor editor) : base("v:imagedata", editor)
		{
		}

        internal ImagedataElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
