using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The SUP element is used for superscripts.
    /// </summary>
    /// <remarks>
    /// Since SUP is inherently presentational, it should not be relied upon to express a given meaning. However, it can be useful for mathematical exponents where the context implies the meaning of the exponent, as well as other cases where superscript presentation is helpful but not required.
    /// </remarks>
    public sealed class SupElement : SimpleInlineElement
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
        public SupElement(IHtmlEditor editor)
            : base("sup", editor)
        {
        }

        internal SupElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
