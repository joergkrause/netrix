using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
 
    /// <summary>
    /// Defines a submit button element in a form.
    /// </summary>
    /// <remarks>
    /// This is one of the classes related to the INPUT element. The type of element ist fixed
    /// by the type property.
    /// <para>
    /// Once the button is hit by the user the form is sent to the server.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.FormElement">FormElement (FORM)</seealso>
    /// </para>
    /// </remarks>
    public sealed class InputSubmitElement : InputButtonElement
    {

        /// <summary>
        /// Gets the type of input element.
        /// </summary>
        /// <remarks>
        /// This property is for information only and cannot be changed.
        /// </remarks>

		[Browsable(true)]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public override string type
		{
			get
			{
				return "submit";
			}
		}

        /// <summary>
        /// The VALUE attributes determine the value sent to the server.
        /// </summary>
        /// <remarks>
        /// It is recommended to not use the value on the server side to control the behavior of
        /// a script. In case of a button the value is a UI parameter, e.g. it is used to set the
        /// text on the button. Changing the UI causes the script to fail.
        /// </remarks>

        [DefaultValueAttribute("Submit")]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public override string @value
        {
            get
            {
                return base.GetStringAttribute("value", "Submit");
            }

            set
            {
                base.SetStringAttribute("value", value, "Submit");
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
        public InputSubmitElement(IHtmlEditor editor)
            : base(@"input type=""submit""", editor)
        {
        }


        internal InputSubmitElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
