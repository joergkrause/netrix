using System.ComponentModel;
using System.Drawing;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This element describes the fill of the background of a page using vector graphics fills.
	/// </summary>
	/// <remarks>
	/// This illustrates how the rendering description of VML can be extended to existing and new HTML objects.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class BackgroundElement : VmlBaseElement
	{

        /// <summary>
        /// String with command set describing a path.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public string Id
        {
            get
            {
                return base.GetStringAttribute("id");
            }
            set
            {
                base.SetStringAttribute("id", value);
            }
        }

        /// <summary>
        /// RGB color to use for the fill.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public Color FillColor
        {
            get
            {
                return base.GetColorAttribute("fillcolor");
            }
            set
            {
                base.SetColorAttribute("fillcolor", value);
            }
        }

        /// <summary>
        /// Boolean whether to fill the shape or not.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public bool Fill
        {
            get
            {
                return base.GetBooleanAttribute("fill");
            }
            set
            {
                base.SetBooleanAttribute("fill", value);
            }
        }

		public BackgroundElement(IHtmlEditor editor) : base("v:background", editor)
		{
		}

        internal BackgroundElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
