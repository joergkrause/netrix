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
    /// BDO overrides the current direction information.
    /// </summary>
    /// <remarks>
    /// The BDO element is a directional override feature needed to deal with unusual pieces of text in which 
    /// directionality cannot be resolved from context in an unambiguous fashion. It requires the DIR attribute. 
    /// The meaning of DIR is different on BDO than on inline text markup elements. For BDO the DIR attribute is a 
    /// bidi override, forcing the directionnality of even those characters that have strong directionnality. On 
    /// inline elements, DIR indicates a new directional embedding level, affecting mostly the neutrals and the overall layout.
    /// <para>
    /// To insert BDO elements just use 
    /// <see cref="GuruComponents.Netrix.HtmlDocument.InsertBidirectionalOverride">InsertBidirectionalOverride</see>.
    /// </para>
    /// <para>
    /// Another way to deal woth text direction features is the usage of the direct formatting features, which applies
    /// to text selection or - in case of of paragraphs - whole paragraphs. For more information about this
    /// topic see <see cref="GuruComponents.Netrix.HtmlTextFormatting.DirectionRtlBlock">DirectionRtlBlock</see>, 
    /// <see cref="GuruComponents.Netrix.HtmlTextFormatting.DirectionRtlInline">DirectionRtlInline</see>, and
    /// <see cref="GuruComponents.Netrix.HtmlTextFormatting.DirectionRtlDocument">DirectionRtlDocument</see>. The BDO
    /// element is used to format inline only.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.HtmlTextFormatting.DirectionRtlInline">DirectionRtlInline</seealso>
    /// </remarks>
    /// <example>
    /// The following code shows how to insert and use this element:
/// <code>
/// IElement bdo = this.htmlEditor1.Document.InsertBidirectionalOverride();
/// ((BdoElement) bdo).dir = "rtl";</code>
    /// The code assumes that any text is selected in the document where the insertion applies to.
    /// </example>
    public sealed class BdoElement : StyledElement
    {

        /// <summary>
        /// The Direction, which the text follows.
        /// </summary>

        [Category("Element Behavior")]
        [DefaultValue("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [Description("")]
        [DisplayNameAttribute()]

        public string dir
        {
            set
            {
                if (value.ToLower().Equals("rtl") || value.ToLower().Equals("ltr"))
                {
                    this.SetStringAttribute ("dir", value);
                } 
                else
                {
                    throw new ArgumentException("Only the values RTL or LTR allowed for this property");
                }
            } 
      
            get
            {
                return this.GetStringAttribute("dir");
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
        public BdoElement(IHtmlEditor editor)
            : base("bdo", editor)
        {
        }

        internal BdoElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
