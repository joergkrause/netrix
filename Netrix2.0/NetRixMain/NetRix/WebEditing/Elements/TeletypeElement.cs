using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The TT element suggests that text be rendered as teletype or monospaced text.
    /// </summary>
    /// <remarks>
    /// In most cases, use of a phrase element such as CODE, SAMP, or KBD is more appropriate since these elements express the meaning of the text more clearly.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.CodeElement">CodeElement</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.KeyboardElement">KeyboardElement</seealso>
    /// </remarks>
    [System.Obsolete("This element is not supported in HTML 5 and should not be used in future projects")]
    public sealed class TeletypeElement : StyledElement
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
        public TeletypeElement(IHtmlEditor editor)
            : base("tt", editor)
        {
        }

        internal TeletypeElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
