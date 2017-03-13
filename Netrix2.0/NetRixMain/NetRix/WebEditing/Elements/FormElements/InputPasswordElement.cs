using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// Defines a password text element in a form.
    /// </summary>
    /// <remarks>
    /// This is one of the classes related to the INPUT element. The type of element ist fixed
    /// by the type property.
    /// </remarks>
    public sealed class InputPasswordElement : InputTextElement
    {

        /// <summary>
        /// The VALUE attributes determine the value sent to the server.
        /// </summary>
        /// <remarks>
        /// The value cannot be set due to security restricition. 
        /// </remarks>

        [Browsable(false)]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public override string @value
        {
            get
            {
                return base.value;
            }
            set
            {
                base.value = value;
            }
        }

        /// <summary>
        /// Gets the type of input element.
        /// </summary>
        /// <remarks>
        /// This property is for information only and cannot be changed.
        /// </remarks>

		[BrowsableAttribute(true)]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public override string type
        {
            get
            {
                return "password";
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
        public InputPasswordElement(IHtmlEditor editor)
            : base(@"input type=""password""", editor)
        {
        }


        internal InputPasswordElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
