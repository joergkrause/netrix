using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The DL element defines a definition list.
    /// </summary>
    /// <remarks>
    /// An entry in the list is created using the DT element for the term being defined and the DD element for the definition of the term.
    /// <para>
    /// A definition list can have multiple terms for a given definition as well as multiple definitions for a given term. 
    /// Authors can also give a term without a corresponding definition, and vice versa, but such a structure rarely makes sense.
    /// </para>
    /// </remarks>
    public sealed class DefinitionListDefinitionElement : StyledElement
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
        public DefinitionListDefinitionElement(IHtmlEditor editor)
            : base("dl", editor)
        {
        }

        internal DefinitionListDefinitionElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
