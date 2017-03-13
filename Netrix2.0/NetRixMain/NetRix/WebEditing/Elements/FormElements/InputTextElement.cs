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
    /// Defines the start of an input field where the user can enter data.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="LabelElement"/> element to define a label to a form control.
    /// You can also group fields with the <see cref="FieldSetElement"/> element.
    /// </remarks>
    public class InputTextElement : InputElement
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
				return "text";
			}
		}


        /// <summary>
        /// Defines the maximum number of characters allowed in a text field.
        /// </summary>
        /// <remarks>
        /// <seealso cref="size"/>
        /// </remarks>
        [DefaultValueAttribute(int.MaxValue)]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]
        public int maxLength
        {
            get
            {
                return base.GetIntegerAttribute("maxLength", int.MaxValue);
            }

            set
            {
                if (value < 0)
                {
                    value = int.MaxValue;
                }
                base.SetIntegerAttribute("maxLength", value, int.MaxValue);
            }
        }


        /// <summary>
        /// Indicates that the value of this field cannot be modified.
        /// </summary>
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [DisplayName()]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DefaultValueAttribute(false)]
        public bool readOnly
        {
            get
            {
                return base.GetBooleanAttribute("readOnly");
            }

            set
            {
                base.SetBooleanAttribute("readOnly", value);
            }
        }


        /// <summary>
        /// Defines the size of the input element (e.g. the number of chars).
        /// </summary>
        /// <remarks>
        /// This refers to the width, in standard font chars. It does not limit the number of chars which the
        /// user could enter.
        /// <seealso cref="maxLength"/>
        /// </remarks>
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(20)]
        public int size
        {
            get
            {
                return base.GetIntegerAttribute("size", 20);
            }

            set
            {
                base.SetIntegerAttribute("size", value, 20);
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
        public InputTextElement(IHtmlEditor editor)
            : base(@"input type=""text""", editor)
        {
        }

        protected InputTextElement(string tag, IHtmlEditor editor) : base (tag, editor)
        {
        }

        internal InputTextElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
