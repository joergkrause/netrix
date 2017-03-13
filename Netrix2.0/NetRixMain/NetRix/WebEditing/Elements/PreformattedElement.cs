using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The PRE element contains preformatted text.
    /// </summary>
    /// <remarks>
    /// Visual browsers should render preformatted text in a fixed-pitch font, should not collapse whitespace, and should not wrap long lines.
    /// <para>PRE is useful for formatting computer code or poetry where whitespace is important, but since preformatted text is inherently visual, authors should avoid dependence on it wherever possible. When using PRE, authors should avoid altering the element's fixed-pitch font or non-collapsing whitespace properties by means of style sheets.
    /// </para>
    /// </remarks>
    public sealed class PreformattedElement : StyledElement
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
        public PreformattedElement(IHtmlEditor editor)
            : base("pre", editor)
        {
        }

        internal PreformattedElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
