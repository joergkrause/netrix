using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The LEGEND element defines a caption for form controls grouped by the FIELDSET element.
    /// </summary>
    /// <remarks>
    /// The LEGEND element must be at the start of a FIELDSET, before any other elements.
    /// <para>While the LEGEND element is not widely supported by current browsers, it can still be used safely 
    /// if a block-level element immediately follows the LEGEND. Combined with careful use of FIELDSET, this will 
    /// cause non-supporting browsers to render the caption as its own paragraph. Elements such as STRONG, B, and BIG 
    /// could also be used to help express the meaning of LEGEND to non-supporting browsers.
    /// </para>
    /// </remarks>
    public sealed class LegendElement : StyledElement
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
        public LegendElement(IHtmlEditor editor)
            : base("legend", editor)
        {
        }

        internal LegendElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
