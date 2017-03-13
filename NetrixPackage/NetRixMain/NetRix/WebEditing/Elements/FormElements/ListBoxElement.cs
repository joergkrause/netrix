using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// A specialized version of the SELECT element.
    /// </summary>
    /// <remarks>
    /// This class realizes the SELECT element which is rendered as a list instead of a drop down box.
    /// The default value of the number of elements visible is 3.
    /// </remarks>
    public class ListBoxElement : SelectElement
    {

        /// <summary>
        /// The SIZE attribute. Required.
        /// </summary>
        /// <remarks>
        /// If the value is bigger than 1 the box is rendered as a list. The value defaults to 3.
        /// </remarks>

        [DefaultValue(3)]
        [CategoryAttribute("Element Layout")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
		[DisplayNameAttribute()]

        public new int size
        {
            get
            {
                return base.GetIntegerAttribute("size", 3);
            }

            set
            {
                base.SetIntegerAttribute("size", value, 3);
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
        public ListBoxElement(IHtmlEditor editor)
            : base("select", editor)
        {
            size = 3;
        }

        internal ListBoxElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
			Interop.IHTMLSelectElement s = (Interop.IHTMLSelectElement) peer;
			s.size = this.size;
        }
    }

}
