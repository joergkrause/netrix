using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The FIELDSET element defines a form control group.
    /// </summary>                                        
    /// <remarks>
    /// By grouping related form controls, authors can divide a form into smaller, more manageable parts, improving the usability disaster that can strike when confronting users with too many form controls. The grouping provided by FIELDSET also helps the accessibility of forms to those using aural browsers 
    /// by allowing these users to more easily orient themselves when filling in a large form.
    /// <para>While FIELDSET is not widely supported by current browsers, it can be used safely by explicitly 
    /// closing any preceding P element with &lt;/P&gt; or by including an empty P prior to the FIELDSET. This causes 
    /// non-supporting browsers to infer the start of a block-level element even though they ignore the 
    /// block-level FIELDSET element.
    /// </para>
    /// </remarks>
    public class FieldSetElement : StyledElement
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
        public FieldSetElement(IHtmlEditor editor)
            : base(@"fieldset", editor)
        {
        }


        internal FieldSetElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
