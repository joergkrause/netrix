using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The VAR element is used to markup variables or program arguments.
    /// </summary>
    /// <remarks>
    /// Visual browsers typically render VAR as italic text, but authors can suggest a rendering using style sheets. Since VAR is a structural element, it carries meaning, making it preferable to font style elements such as I when marking up variables.
    /// </remarks>
    public sealed class VariableElement : SimpleInlineElement
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
        public VariableElement(IHtmlEditor editor)
            : base("var", editor)
        {
        }

        internal VariableElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
