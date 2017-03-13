using System;
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
    /// The INPUT element defines a form control for the user to enter input.
    /// </summary>
    /// <remarks>
    /// In NetRix there are some specialised classes representing the various flavors of the INPUT tag, set by the TYPE attribute. 
    /// </remarks>
    public abstract class InputElement : SelectableElement
    {
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

        [Category("Element Behavior")]
        [DefaultValueAttribute(false)]
        [DescriptionAttribute("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayNameAttribute()]

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
        /// Sets or retrieves the name of the object.
        /// </summary>
        /// <remarks>
        /// When submitting a form, use the name property to bind the value of the control. The name is not the value displayed for the input type=button, input type=reset, and input type=submit input types. The internally stored value, not the displayed value, is the one submitted with the form.
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

        /// <summary>
        /// The handler which is called if the onChange event appears.
        /// </summary>
        /// <remarks>
        /// This should be the name of a JSCript, JavaScript or VBScript function or any valid inline code in the
        /// globally selected client site scripting language.
        /// </remarks>

        [DefaultValueAttribute("")]
        [CategoryAttribute("JavaScript Events")]
        [DescriptionAttribute("")]
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

        public abstract string type
        {
            get;
        }

        /// <summary>
        /// Sets or retrieves the default or selected value of the control.
        /// </summary>
        /// <remarks>
        /// The purpose of the value property depends on the type of control as described in the following table.
        /// <list type="table">
        /// <listheader><term>Type</term><term>Description</term></listheader>
        /// <item><term>checkbox</term><term>The selected value. The control submits this value only if the user has selected the control. Otherwise, the control submits no value.</term></item>
        /// <item><term>file</term><term>The value, a file name, typed by the user into the control. Unlike other controls, this value is read-only.</term></item>
        /// <item><term>hidden</term><term>The value that the control submits when the form is submitted.</term></item>
        /// <item><term>option (tag)</term><term>The selected value. The containing list box control submits this value only if the user has selected the option.</term></item>
        /// <item><term>password</term><term>The default value. The control displays this value when it is first created and when the user clicks the reset button.</term></item>
        /// <item><term>radio</term><term>The selected value. The control submits this value only if the user has selected the control. Otherwise, the control submits no value.</term></item>
        /// <item><term>reset</term><term>The button label. If not set, the label defaults to Reset.</term></item>
        /// <item><term>submit</term><term>The button label. If not set, the label defaults to Submit.</term></item>
        /// <item><term>text</term><term>The default value. The control displays this value when it is first created and when the user clicks the reset button.</term></item>
        /// </list>
        /// </remarks>

        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public virtual string value
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
        /// <param name="tag">Tag name including type attribute to create specific input element.</param>
        /// <param name="editor">The editor this element belongs to.</param>
        public InputElement(string tag, HtmlEditor editor)
            : base(tag, editor)
        {
        }

        internal InputElement(string tag, IHtmlEditor editor) : base (tag, editor)
        {
        }

        internal InputElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }

        /// <summary>
        /// Special representation of the string format.
        /// </summary>
        /// <returns>Returns the description of the element in the format &lt;input type="type"&gt;.</returns>
        public override string ToString()
        {
            return String.Concat(new string[]{"<", base.TagName, " type=\"", type, "\">"});
        }
    }

}
