using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// Native object class for v:textbox element.
	/// </summary>
	/// <remarks>
	/// This sub-element may appear inside a shape or a shapetype to define text that is to appear inside the shape. 
	/// This text may contain rich formatting and will be rendered to fit inside the textboxrect defined by the path 
	/// element.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class TextboxElement : VmlBaseElement
	{

        /// <summary>
        /// Creates an new textbox element.
        /// </summary>
        /// <param name="editor"></param>
		public TextboxElement(IHtmlEditor editor) : base("v:textbox", editor)
		{
		}

        internal TextboxElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
