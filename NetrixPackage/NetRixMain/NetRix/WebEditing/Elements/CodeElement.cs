using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The CODE element denotes computer code.
    /// </summary>
    /// <remarks>
    /// Visual browsers typically render CODE as monospaced text, but authors can suggest a rendering using style sheets. 
    /// Since CODE is a structural element, it carries meaning, making it preferable to font style elements such as TT when marking up computer code.
    /// Since spacing is often important when presenting computer code, the PRE element can be useful as a container for CODE elements. 
    /// When used within other containers, a CODE element has multiple spaces collapsed. The following example uses CODE within PRE:
    /// <code>
    /// &lt;PRE&gt;&lt;CODE&gt;
    /// class HelloWorld 
    /// {
    ///   public static void main(String[] args) 
    ///   {
    ///      System.out.println("Hello World!");
    ///   }
    /// }
    /// &lt;/CODE&gt;&lt;/PRE&gt;
    /// </code>       
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TeletypeElement">TeletypeElement (TT)</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.PreformattedElement">PreformattedElement (PRE)</seealso>
    /// </remarks>
    public sealed class CodeElement : StyledElement
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
        public CodeElement(IHtmlEditor editor)
            : base("code", editor)
        {
        }

        internal CodeElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
