using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The DT element defines a term in a definition list.
    /// </summary>
    /// <remarks>
    /// The closing tag for DT is optional, but its use prevents common browser bugs with style sheets. Note that DT cannot contain block-level elements such as P and H2.
    /// <para>A DT element should generally be followed by a DD element that provides the definition for the term given by the DT. A single definition term 
    /// may have multiple definitions associated with it, and a single definition may have multiple terms.
    /// </para>
    /// </remarks>
    public sealed class DefinitionListTermElement : StyledElement
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
        public DefinitionListTermElement(IHtmlEditor editor)
            : base("dt", editor)
        {
        }

        internal DefinitionListTermElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
