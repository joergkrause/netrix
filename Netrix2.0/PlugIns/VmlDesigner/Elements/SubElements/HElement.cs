using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// Each handle is specified using a single h sub-element.
	/// </summary>
	/// <remarks>
    /// This defines which pair of adjust values store the position of the handle and how the handle position can vary as the handle is adjusted.  The handle is moved under user control, within the constraints imposed by the handle definition, and the final position is stored back in the adjust values. 
    /// Positions are stored within the shape coordinate space - this means that handle positions are independent of the actual size of the shape.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class HElement : VmlBaseElement
	{
                                        
		public HElement(IHtmlEditor editor) : base("v:h", editor)
		{
		}

        internal HElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
