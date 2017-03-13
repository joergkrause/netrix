using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;

namespace GuruComponents.Netrix.WebEditing.Elements.Html5
{
    public  class EmbedElement : EmbeddedBaseElement
    {

	
        /// <summary>
        /// SRC tells where to get the picture that should be put on the page.
        /// </summary>
        /// <remarks>
        /// SRC is the one required attribute. It is recommended to use relative paths. If a filename is given the property will recognize and set
        /// the relative path automatically.
        /// </remarks>

        [Category("Element Layout")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
		[EditorAttribute(
			 typeof(UITypeEditorUrl),
			 typeof(UITypeEditor))]
		[DisplayNameAttribute()]

        public string src
		{
			set
			{
				this.SetStringAttribute ("src", this.GetRelativeUrl(value));
				return;
			} 
			get
			{
				return this.GetRelativeUrl (this.GetStringAttribute ("src"));
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
        public EmbedElement(IHtmlEditor editor)
            : base("embed", editor)
        {
        }

        internal EmbedElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
