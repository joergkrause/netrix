using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The INS element contains content that has been inserted.
    /// </summary>
    /// <remarks>
    /// This element is useful in marking changes from one version of a document to the next. Through style sheets, authors can suggest an appropriate rendering, such as rendering the inserted content in italics, a different color, or a different voice.
    /// <para>
    /// INS may be used as either a block-level element or an inline element. If used as an inline element (e.g., within a P), then INS may not contain any block-level elements.
    /// </para>
    /// </remarks>
    public sealed class InsertedElement : StyledElement
    {
        /// <summary>
        /// The CITE attribute of DEL gives a URI with information on why the content was deleted. Optional.
        /// </summary>
        /// <remarks>
        /// A brief explanation for the deletion can be given with the TITLE attribute, which may be rendered as a "tooltip" by some browsers.
        /// </remarks>

        [Category("Runtime Appearance")]
        [DefaultValue("")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string cite
        {
            set
            {
                this.SetStringAttribute ("cite", value);
                return;
            } 
      
            get
            {
                return this.GetStringAttribute("cite");
            } 
      
        }

        /// <summary>
        /// The optional DATETIME attribute specifies the date and time of the deletion.
        /// </summary>
        /// <remarks>
        /// The value is case-sensitive and of the form YYYY-MM-DDThh:mm:ssTZD. See the values section for a full explanation of this format.
        /// The usage is shown by the following example:
        /// <code>
        /// &lt;DEL CITE="http://www.w3.org/TR/REC-html40/appendix/changes.html#h-A.1.3" DATETIME="1997-12-19T00:00:00-05:00" TITLE="XMP is obsolete"&gt;
        /// &lt;P&gt;
        /// The XMP element contains preformatted text in which markup other than an end tag is treated as literal text.
        /// &lt;/P&gt;
        /// &lt;/DEL&gt;
        /// </code>
        /// Since DEL is poorly supported among browsers, authors may wish to use a font style element such as STRIKE (deprecated in HTML 4.0) to attempt to convey the meaning of DEL to non-supporting visual browsers.
        /// </remarks>

        [CategoryAttribute("Runtime Appearance")]
        [DefaultValue("")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string datetime
        {
            set
            {
                this.SetStringAttribute ("cite", value);
                return;
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
        public InsertedElement(IHtmlEditor editor)
            : base("ins", editor)
        {
        }

        internal InsertedElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}