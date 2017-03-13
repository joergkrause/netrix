using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// Defines a checkbox element on a form.
    /// </summary>
    /// <remarks>
    /// This is one of the classes related to the INPUT element. The type of element ist fixed
    /// by the type property.
    /// </remarks>
    public class InputCheckboxElement : InputElement
    {

        /// <summary>
        /// Sets or retrieves the state of the check box or radio button.
        /// </summary>
        /// <remarks>
        /// Check boxes that are not selected do not return their values when the form is submitted. 
        /// </remarks>
        [Category("Element Behavior")]
        [DefaultValueAttribute(false)]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DescriptionAttribute("")]
        [DisplayNameAttribute()]
        public bool @checked
        {
            get
            {
                return base.GetBooleanAttribute("checked");
            }

            set
            {
                base.SetBooleanAttribute("checked", value);
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
                return "checkbox";
            }
        }

        /// <summary>
        /// Sets or retrieves the default or selected value of the control.
        /// </summary>
        /// <remarks>
        /// The default value is "on" which is send, if no value is given.
        /// </remarks>

        [DefaultValueAttribute("on")]
        [DescriptionAttribute("")]
        [CategoryAttribute("Standard Values")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public override string value
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
        public InputCheckboxElement(IHtmlEditor editor)
            : base(@"input type=""checkbox""", editor)
        {
        }

        protected InputCheckboxElement(string tag, IHtmlEditor editor) : base (tag, editor)
        {
        }

        internal InputCheckboxElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
