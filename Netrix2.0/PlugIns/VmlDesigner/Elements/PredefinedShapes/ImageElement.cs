using System;  
using System.ComponentModel;  
using GuruComponents.Netrix.ComInterop;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix;
namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This element is used to draw a bitmap that has been loaded from an external source.
	/// </summary>
	/// <remarks>
    /// There is an implied rectangle that is the same size as the image. Any stroke or fill will be applied to this 
    /// implied rectangle.  The fill will be behind the image and it will therefore only be visible through transparent areas 
    /// of the bitmap.  The stroke is drawn on top of the image.  The bitmap may have transparency encoded in the 
    /// file (if it is a PNG bitmap) or a chromakey color value may be specified using the <c>chromakey</c> attribute.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class ImageElement : PredefinedElement
	{

        /// <summary>
        /// SRC tells where to get the picture that should be put on the page.
        /// </summary>
        /// <remarks>
        /// SRC is the one required attribute. It is recommended to use relative paths. If a filename is given the property will recognize and set
        /// the relative path automatically.
        /// </remarks>
        [CategoryAttribute("Element Layout")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
		[EditorAttribute(
			 typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUrl),
			 typeof(System.Drawing.Design.UITypeEditor))]
		[TE.DisplayName()]
        public string src
        {
            set
            {
                base.SetStringAttribute ("src", this.GetRelativeUrl(value));
                return;
            } 
            get
            {
                return base.GetRelativeUrl (this.GetStringAttribute ("src"));
            }       
        }

		public ImageElement(IHtmlEditor editor) : base("v:image", editor)
		{
		}

        internal ImageElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
