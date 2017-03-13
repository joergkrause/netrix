using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The MENU element defines a menu list. Deprecated.
    /// </summary>
    /// <remarks>
    /// The element contains one or more LI elements that define the actual items of the list. The LI elements must not contain block-level elements, which prevents MENUs from being nested.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.ListItemElement">ListItemElement (LI)</seealso>
    /// </remarks>
    public sealed class MenuElement : StyledElement
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
        public MenuElement(IHtmlEditor editor)
            : base("menu", editor)
        {
        }

        internal MenuElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
