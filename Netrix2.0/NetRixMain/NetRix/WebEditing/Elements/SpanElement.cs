using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The SPAN element is a generic inline container.
    /// </summary>
    /// <remarks>
    /// SPAN carries no structural meaning itself, but it can be used to provide extra structure through its LANG, DIR, CLASS, and ID attributes. 
    /// Style sheets are often used to suggest a presentation for a given class or ID.
    /// <para>SPAN should only be used where no other HTML inline element provides a suitable meaning. If a presentation such as bold or italic text would be 
    /// suitable on visual browsers, authors may prefer to use an appropriate font style element.
    /// </para>
    /// </remarks>
    public sealed class SpanElement : SimpleInlineElement
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
        public SpanElement(IHtmlEditor editor)
            : base("span", editor)
        {
        }
        
        internal SpanElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
