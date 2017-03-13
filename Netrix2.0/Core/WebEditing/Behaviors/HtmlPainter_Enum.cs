using System;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
	/// <summary>
	/// Determines the behavior of drawing process of binary behaviors against the underlying element.
	/// </summary>
	[Flags()]
    public enum HtmlPainter
	{
        /// <summary>
        /// The behavior requires a Microsoft Direct3D® surface.
        /// </summary>
        ThreedSurface = 0x200,
        /// <summary>
        /// Not implemented.
        /// </summary>
        Alpha = 0x4,
        /// <summary>
        /// Complex
        /// </summary>
        Complex = 0x8,
        /// <summary>
        /// The behavior's rendering will be opaque; the behavior will change all pixels in its update rectangle or region and MSHTML does not need to render anything below it.
        /// </summary>
        Opaque = 0x1,
        /// <summary>
        /// The behavior uses Microsoft® DirectDraw® hardware overlay planes.
        /// </summary>
        Overlay = 0x10,
        /// <summary>
        /// The behavior supports hit testing; MSHTML will call the IHTMLPainter.HitTestPoint method when relevant events fire on the element to which the behavior is attached.
        /// </summary>
        HitTest = 0x20,
        /// <summary>
        /// Not implemented.
        /// </summary>
        Noband = 0x400,
        /// <summary>
        /// No device context is required.
        /// </summary>
        NoDC = 0x1000,
        /// <summary>
        /// The behavior will draw entirely within its bounding area; MSHTML does not have to apply clipping.
        /// </summary>
        NoPhysicalClip = 0x2000,
        /// <summary>
        /// The behavior will return the device context to the state in which it received it; MSHTML does not need to save the device context's state prior to passing it to the IHTMLPainter::Draw method.
        /// </summary>
        NoSaveDC = 0x4000,
        /// <summary>
        /// The behavior supports transformations. If a behavior does not support transformations and MSHTML is applying a transformation to its output (for instance, in a zoomed page layout view), its IHTMLPainter::Draw method will not be called and the behavior will not be rendered.
        /// </summary>
        SupportsXForm = 0x8000,
        /// <summary>
        /// The behavior requires a DirectDraw surface.
        /// </summary>
        Surface = 0x100,
        /// <summary>
        /// The behavior's rendering will be transparent; the behavior might not change all the pixels in its update rectangle or region and MSHTML needs to render content below it.
        /// </summary>
        Transparent = 0x2
	}
}
