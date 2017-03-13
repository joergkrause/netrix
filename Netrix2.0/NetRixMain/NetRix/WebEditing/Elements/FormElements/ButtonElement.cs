using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The BUTTON element defines a submit button, reset button, or push button.
    /// </summary>
    /// <remarks>
    /// Authors can also use INPUT to specify these buttons, but the BUTTON element 
    /// allows richer labels, including images and emphasis. However, BUTTON is new in HTML 4.0 and 
    /// poorly supported among current browsers, so INPUT is a more reliable choice at this time.
    /// </remarks>
    public sealed class ButtonElement : SelectableElement
    {

        /// <summary>
        /// The VALUE attributes determine the value sent to the server.
        /// </summary>
        /// <remarks>
        /// It is recommended to not use the value on the server side to control the behavior of
        /// a script. In case of a button the value is a UI parameter, e.g. it is used to set the
        /// text on the button. Changing the UI causes the script to fail.
        /// </remarks>

        [Browsable(true)]
        [CategoryAttribute("Element Layout")]
        [DescriptionAttribute("")]        
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string @value
        {
            get
            {
                return base.GetStringAttribute("value");
            }

            set
            {
                base.SetStringAttribute("value", value);
            }
        }

        /// <summary>
        /// Disables the element.
        /// </summary>
        /// <remarks>
        /// The most elements are drawn grayed or with a gray shadow to inform the user that the element is disabled.
        /// Field elements do not except the focus and do not display the caret.
        /// <para>
        /// In HTML the element is disables if the attribute exists and it is enabled if it doesn't exist. The parameter assigned
        /// to the attribute is useless.
        /// </para>
        /// </remarks>

        [DefaultValueAttribute(false)]
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [DisplayName()]

        public new bool disabled
        {
            get
            {
                return base.GetBooleanAttribute("disabled");
            }

            set
            {
                base.SetBooleanAttribute("disabled", value);
            }
        }

        /// <summary>
        /// The NAME of the button.
        /// </summary>
        /// <remarks>
        /// The name is used to recognize the value in a server side script.
        /// </remarks>

        [CategoryAttribute("Standard Values")]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DescriptionAttribute("")]
        [DisplayName()]

        public string name
        {
            get
            {
                return base.GetStringAttribute("name");
            }

            set
            {
                base.SetStringAttribute("name", value);
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
        public ButtonElement(IHtmlEditor editor)
            : base(@"button", editor)
        {
        }


        internal ButtonElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
