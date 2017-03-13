using System;
using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The BLOCKQUOTE tag defines the start of a long quotation.
    /// </summary>
    /// <remarks>
    /// The BLOCKQUOTE tag is supposed to contain only block-level elements within it, and not just plain text.
    /// To validate the page as strict XHTML, you must add a block-level element around the text within the BLOCKQUOTE tag, 
    /// like this:
    /// <code>
    /// &lt;blockquote&gt;
    /// &lt;p&gt;here is a long quotation here is a long quotation&lt;/p&gt;
    /// &lt;/blockquote&gt;
    /// </code>
    /// The blockquote element creates white space on both sides of the text
    /// </remarks>
    [Obsolete("This element does not conform the HTML 4 definition")]
    public sealed class BlockquoteElement : StyledElement
    {

        /// <summary>
        /// A cite, which this element belongs to.
        /// </summary>
        [Category("Element Behavior")]
        [DefaultValue("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [Description("")]
        [DisplayNameAttribute()]
        public string cite
        {
            set
            {
                this.SetStringAttribute ("cite", value);
            } 
      
            get
            {
                return this.GetStringAttribute("cite");
            }       
        }

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
        public BlockquoteElement(IHtmlEditor editor)
            : base("blockquote", editor)
        {
        }

        internal BlockquoteElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
