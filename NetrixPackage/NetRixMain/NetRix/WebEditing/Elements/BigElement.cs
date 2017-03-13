using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The BIG element suggests that text be rendered in a larger font.
    /// </summary>
    /// <remarks>
    /// In most cases, use of a structural element such as STRONG or a heading (e.g., H3) is more appropriate since these elements express the meaning 
    /// of the text more clearly. One can suggest that STRONG text be rendered in a larger font with the following Cascading Style Sheet:
    /// <code>
    /// STRONG { font-size: larger }
    /// </code>
    /// Most browsers support nested BIG elements, but authors should be wary of making significant changes to the font size. 
    /// Different users have different font sizes, eyesight, and window sizes. Large changes in font size may look right to the author but ridiculous to some users.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.StrongElement">StrongElement</seealso>
    /// </remarks>
    public sealed class BigElement : SimpleInlineElement
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
        public BigElement(IHtmlEditor editor)
            : base("big", editor)
        {
        }

        internal BigElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
