using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The B element suggests that text be rendered as bold text.
    /// </summary>    
    /// <remarks>
    /// In most cases, use of a phrase element such as STRONG is more appropriate since such elements express the meaning of the text more clearly.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.StrongElement"/>
    /// </remarks>
    public sealed class BoldElement : SimpleInlineElement
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
        public BoldElement(IHtmlEditor editor)
            : base("b", editor)
        {
        }

        internal BoldElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
