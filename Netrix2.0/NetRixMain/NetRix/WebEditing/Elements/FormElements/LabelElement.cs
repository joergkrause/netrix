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
    /// The LABEL element associates a label with a form control.
    /// </summary>
    /// <remarks>
    /// By associating labels with form controls, authors give important hints to users of speech browsers while also allowing visual browsers to duplicate common GUI features (e.g., the ability to click on a text label to select a radio button or checkbox).
    /// </remarks>
    public sealed class LabelElement : StyledElement
    {

        /// <summary>
        /// The ACCESSKEY attribute specifies a single Unicode character as a shortcut key for giving focus to the LABEL.
        /// </summary>
        /// <remarks>
        /// If the assigned key is hit the control passes the focus on to the associated form control. Entities (e.g. &amp;eacute;) may be used as the ACCESSKEY value.
        /// </remarks>

        [Category("Element Behavior")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string accessKey
        {
            get
            {
                return base.GetStringAttribute("accessKey");
            }

            set
            {
                if (value != null && value.Length != 1)
                {
                    throw new ArgumentException("AccessKey cannot be longer than one character");
                }
                base.SetStringAttribute("accessKey", value);
            }
        }

        /// <summary>
        /// The FOR attribute explicitly specifies the control associated with the LABEL.
        /// </summary>
        /// <remarks>
        /// The value of the FOR attribute must match the value of the associated form control's ID attribute. In the absence of the FOR attribute, the LABEL must contain the associated form control. This method of implicit association is convenient in many cases, but not an option when the form control and its label are in different table cells, paragraphs, or divisions.
        /// </remarks>

        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string @for
        {
            get
            {
                return base.GetStringAttribute("htmlFor");
            }

            set
            {
                base.SetStringAttribute("htmlFor", value);
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
        public LabelElement(IHtmlEditor editor)
            : base("label", editor)
        {
        }

        internal LabelElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
