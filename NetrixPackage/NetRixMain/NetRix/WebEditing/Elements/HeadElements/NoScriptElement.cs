using GuruComponents.Netrix.ComInterop;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The SCRIPT element includes a client-side script in the document.
    /// </summary>
    /// <remarks>
    /// Client-side scripts allow greater interactivity in a document by responding to user events. For example, a script 
    /// could be used to check the user's form input prior to submission to provide immediate notice of any errors by the user.
    /// </remarks>
    public class NoScriptElement : Element
    {

 /// <summary>
        /// Creates the specified element.
        /// </summary>
        /// <remarks>
        /// The element is being created and attached to the current document, but nevertheless not visible,
        /// until it's being placed anywhere within the DOM. To attach an element it's possible to either
        /// use the <see cref="ElementDom"/> property of any other already placed element and refer to this
        /// DOM or use the body element (<see cref="IHtmlEditor.GetBodyElement"/>) and add the element there. Also, in 
        /// case of user interactive solutions, it's possible to add an element near the current caret 
        /// position, using <see cref="IHtmlEditor.CreateElementAtCaret(string)"/> method.
        /// <para>
        /// Note: Invisible elements do neither appear in the DOM nor do they get saved.
        /// </para>
        /// </remarks>
        /// <param name="editor">The editor this element belongs to.</param>
        public NoScriptElement(IHtmlEditor editor)
            : base("noscript", editor)
        {
        }

        internal NoScriptElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}