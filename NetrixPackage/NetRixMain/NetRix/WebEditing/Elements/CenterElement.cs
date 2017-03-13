using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The CENTER element defines a block whose contents are centered horizontally on visual browsers.
    /// </summary>
    /// <remarks>
    /// &lt;CENTER&gt; is a shorthand for <c>&lt;DIV ALIGN=center&gt;</c>, though CENTER is slightly better supported among browsers. 
    /// Both methods of centering are deprecated in favor of style sheets (CSS).
    /// <para>
    /// If you setup a user interface it is recommended to use <c>&lt;div style="text-align:center"&gt;</c> to center any text
    /// in a container. This class is available to get backwards compatibility with elder HTML pages edited with NetRix.
    /// </para>
    /// </remarks>
    public sealed class CenterElement : StyledElement
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
        public CenterElement(IHtmlEditor editor)
            : base("center", editor)
        {
        }

        internal CenterElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
