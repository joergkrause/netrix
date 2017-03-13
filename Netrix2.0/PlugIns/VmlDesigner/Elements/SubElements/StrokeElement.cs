using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// Describes how to draw path.
	/// </summary>
	/// <remarks>
    /// The sub-element may appear inside a shape, shapetype or any predefined shape element to describe how to draw the 
    /// path if something beyond a solid line with a solid color is desired.  The attributes of the stroke element can 
    /// used to describe a powerful set of stroke properties.  Extensions to the VML stroke definition may be encoded as 
    /// sub-elements of the stroke element.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class StrokeElement : VmlBaseElement
	{
        /// <summary>
        /// Creates a new STROKE element.
        /// </summary>
        /// <param name="editor"></param>
		public StrokeElement(IHtmlEditor editor) : base("v:stroke", editor)
		{
		}

        internal StrokeElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
