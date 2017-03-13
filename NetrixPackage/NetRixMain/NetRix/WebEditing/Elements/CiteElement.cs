using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The CITE element is used to markup citations.
    /// </summary>
    /// <remarks>
    /// Visual browsers typically render CITE as italic text, but authors can suggest a rendering using style sheets. 
    /// Since CITE is a structural element, it carries meaning, making it preferable to font style elements such as I 
    /// when marking up citations.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.ItalicElement">ItalicElement</seealso>
    /// </remarks>
    public sealed class CiteElement : StyledElement
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
        public CiteElement(IHtmlEditor editor)
            : base("cite", editor)
        {
        }

        internal CiteElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
