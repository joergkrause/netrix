using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// Defines a hidden element in a form.
    /// </summary>
    /// <remarks>
    /// This is one of the classes related to the INPUT element. The type of element ist fixed
    /// by the type property. Hidden elements are used to send additional values to the server without any
    /// user interaction.
    /// <para>
    /// During design time the hidden fields are lay out as textboxes. During run time they remain invisible.
    /// </para>
    /// </remarks>
    public sealed class InputHiddenElement : Element
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
        public string type
		{
			get
			{
				return "hidden";
			}
		}

        /// <summary>
        /// ID of the element.
        /// </summary>
        [CategoryAttribute("Standard Values")]
        [Description("")]
        [DisplayName()]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [MergablePropertyAttribute(false)]
        [ParenthesizePropertyNameAttribute(true)]
        [DefaultValueAttribute("")]
        public string id
        {
            get
            {
                return base.GetBaseElement().GetId();
            }

            set
            {
                base.GetBaseElement().SetId(value);
            }
        }

        /// <summary>
        /// NAME of the element, used to access value on server.
        /// </summary>
        [Description("")]
        [DisplayName()]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [CategoryAttribute("Standard Values")]

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


        [DefaultValueAttribute("")]
        [CategoryAttribute("JavaScript Events")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        [ScriptingVisible()] 
        public string ScriptOnChange
        {
            get
            {
                return base.GetStringAttribute("onChange");
            }

            set
            {
                base.SetStringAttribute("onChange", value);
            }
        }


        [DefaultValueAttribute("")]
        [Description("")]
        [DisplayName()]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [CategoryAttribute("Standard Values")]
        public string value
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

        internal InputHiddenElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
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
        public InputHiddenElement(IHtmlEditor editor)
            : base(@"input type=""hidden""", editor)
        {
        }


        public override string ToString()
        {
            return "<input type=\'hidden\'>";
        }
    }

}
