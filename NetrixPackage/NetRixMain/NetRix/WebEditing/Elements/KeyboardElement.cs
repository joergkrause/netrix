using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The KBD element denotes text to be entered by the user.
    /// </summary>
    /// <remarks>
    /// Visual browsers typically render KBD as monospaced text, but authors can suggest a rendering using style sheets. 
    /// Since KBD is a structural element, it carries meaning, making it preferable to font style elements such as TT when marking up text to be entered by the user.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TeletypeElement">TeletypeElement (TT)</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.PreformattedElement">PreformattedElement (PRE)</seealso>
    /// </remarks>
    public sealed class KeyboardElement : SimpleInlineElement
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
        public KeyboardElement(IHtmlEditor editor)
            : base("kbd", editor)
        {
        }

        internal KeyboardElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
