using GuruComponents.Netrix.ComInterop;
using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The NOFRAMES element contains content that should only be rendered when frames are not displayed.
    /// </summary>
    /// <remarks>
    /// NOFRAMES is typically used in a Frameset document to provide alternate content for browsers that do not support frames or have frames disabled.
    /// <para>
    /// When used within a FRAMESET, NOFRAMES must contain a BODY element. There must not be any NOFRAMES elements contained within this BODY element.
    /// </para>
    /// <para>A meaningful NOFRAMES element should always be provided in a Frameset document and should at the very least contain links to the main frame or frames. NOFRAMES should not contain a message telling the user to upgrade his or her browser. Some browsers support frames but allow the user to disable them.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.BodyElement">BodyElement</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.FrameElement">FrameElement</seealso>
    /// </remarks>
    [Obsolete("This element is not supported in HTML 5 and should not be used in future projects")]
    public sealed class NoframeElement : SimpleInlineElement
	{

                /// <summary>
        /// Creates the specified element.
        /// </summary>
        /// <remarks>
        /// The element is being created and attached to the current document, but nevertheless not visible,
        /// until it's being placed anywhere within the DOM. To attach an element it's possible to either
        /// use the <see cref="ElementDom"/> property of any other already placed element and refer to this
        /// DOM or use the body element (<see cref="HtmlEditor.GetBodyElement"/>) and add the element there. Also, in 
        /// case of user interactive solutions, it's possible to add an element near the current caret 
        /// position, using <see cref="HtmlEditor.CreateElementAtCaret(string)"/> method.
        /// <para>
        /// Note: Invisible elements do neither appear in the DOM nor do they get saved.
        /// </para>
        /// </remarks>
        /// <param name="editor">The editor this element belongs to.</param>
        public NoframeElement(IHtmlEditor editor)
            : base("noframe", editor)
        {
        }


		internal NoframeElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
		}

	}
}