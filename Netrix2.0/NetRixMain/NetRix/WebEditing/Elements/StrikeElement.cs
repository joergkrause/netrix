using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The STRIKE element suggests that text be rendered with a strike-through style. Deprecated.
    /// </summary>
    /// <remarks>
    /// In many cases, use of a phrase element such as DEL is more appropriate since such elements express the meaning of the text more clearly. However, since support for DEL among browsers is weak, STRIKE is useful in combination with DEL.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.DeletedElement">DeletedElement (DEL)</seealso>
    /// </remarks>
    [System.Obsolete("This element is not supported in HTML 5 and should not be used in future projects")]
    public sealed class StrikeElement : SimpleInlineElement
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
        public StrikeElement(IHtmlEditor editor)
            : base("strike", editor)
        {
        }

        internal StrikeElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
