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
    /// The TEXTAREA element defines a form control for the user to enter multi-line text input.
    /// </summary>
    /// <remarks>
    /// TEXTAREA is most useful within a FORM, HTML 4.0 allows TEXTAREA in any block-level or inline element other than BUTTON. However, 
    /// Netscape Navigator will not display any TEXTAREA elements outside of a FORM.
    /// <para>
    /// The initial value of the TEXTAREA is provided as the content of the element and must not contain any HTML tags. When a form is submitted, 
    /// the current value of any TEXTAREA element within the FORM is sent to the server as a name/value pair. The TEXTAREA element's NAME attribute provides the name used.
    /// </para>
    /// </remarks>
    public sealed class TextAreaElement : SelectableElement
    {

        /// <summary>
        /// The COLS attribute specify the number of visible columns. Required.
        /// </summary>
        /// <remarks>
        /// Set the value to 0 (zero) to remove the attribute. Remember that the attribute is still required if no styles are used.
        /// </remarks>

        [Category("Element Layout")]
        [DefaultValueAttribute(20)]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public int cols
        {
            get
            {
                return base.GetIntegerAttribute("cols", 0);
            }

            set
            {
                base.SetIntegerAttribute("cols", value, 0);
            }
        }

        /// <summary>
        /// The NAME attribute. Recommended.
        /// </summary>
        /// <remarks>
        /// The name attribute is used to transfer the entered data to the server. A server side script will access the data
        /// using that name. The name should be unique within the form.
        /// </remarks>

        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Standard Values")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
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


        [CategoryAttribute("JavaScript Events")]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnChange
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


        [DescriptionAttribute("")]
        [CategoryAttribute("JavaScript Events")]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnScroll
        {
            get
            {
                return base.GetStringAttribute("onScroll");
            }

            set
            {
                base.SetStringAttribute("onScroll", value);
            }
        }

        /// <summary>
        /// Set the element as read-only.
        /// </summary>
        /// <remarks>
        /// The boolean READONLY attribute, new in HTML 4.0 and poorly supported by current browsers, prevents the user from editing the content of the TEXTAREA. 
        /// Read-only elements are still submitted with the form.
        /// </remarks>

        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [DefaultValueAttribute(false)]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

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
        /// The ROWS attribute specify the number of visible rows. Required.
        /// </summary>
        /// <remarks>
        /// Set the value to 0 (zero) to remove the attribute. Remember that the attribute is still required if no styles are used.
        /// </remarks>

        [DescriptionAttribute("")]
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(2)]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int rows
        {
            get
            {
                return base.GetIntegerAttribute("rows", 0);
            }

            set
            {
                base.SetIntegerAttribute("rows", value, 0);
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
        public TextAreaElement(IHtmlEditor editor)
            : base(@"textarea", editor)
        {
        }


        internal TextAreaElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
