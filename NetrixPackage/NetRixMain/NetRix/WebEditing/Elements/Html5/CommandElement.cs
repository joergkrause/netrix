using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute = GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;

namespace GuruComponents.Netrix.WebEditing.Elements.Html5
{
    /// <summary>
    /// This class represents the &lt;command&gt; element. (HTML 5)
    /// </summary>
    /// <remarks>
    /// <para>
    /// The command tag defines a command button, like a radiobutton, a checkbox, or a button.
    /// The command element is only visible if it is inside a menu element. If not, it will not be displayed, but can be used to specify a keyboard shortcut.
    /// </para>
    /// Classes directly or indirectly inherited from 
    /// <see cref="GuruComponents.Netrix.WebEditing.Elements.Element"/> are not intended to be instantiated
    /// directly by the host application. Use the various insertion or creation methods in the base classes
    /// instead. The return values are of type <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement"/>
    /// and can be casted into the element just created.
    /// <para>
    /// Examples of how to create and modify elements with the native element classes can be found in the 
    /// description of other element classes.
    /// </para>
    /// </remarks>
    public class CommandElement : Html5Base
    {

        /// <summary>
        /// In some browsers the title attribute can be used to show the full version of the expression 
        /// when you are holding the mouse over the abbreviation.
        /// </summary>
        [Category("Element Layout")]
        [DefaultValue("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [Description("title_Command")]
        [DisplayNameAttribute()]
        public new string title
        {
            set
            {
                this.SetStringAttribute("title", value);
                return;
            }
            get
            {
                return this.GetStringAttribute("title");
            }
        }


        /// <summary>
        /// Defines if the command is checked or not. Use only if type is radio or checkbox.
        /// </summary>
        [Category("Element Layout")]
        [Description("Defines if the command is checked or not. Use only if type is radio or checkbox.")]
        [DefaultValue(false)]
        [DisplayNameAttribute()]
        public bool @checked
        {
            set
            {
                this.SetBooleanAttribute("checked", value);
                return;
            }
            get
            {
                return this.GetBooleanAttribute("checked");
            }
        }

        /// <summary>
        /// Defines if the command is available or not.
        /// </summary>
        [Category("Element Layout")]
        [Description("Defines if the command is available or not.")]
        [DefaultValue(false)]
        [DisplayNameAttribute()]
        public bool disabled
        {
            set
            {
                this.SetBooleanAttribute("disabled", value);
                return;
            }
            get
            {
                return this.GetBooleanAttribute("disabled");
            }
        }

        /// <summary>
        /// Defines the url of an image to display as the command.
        /// </summary>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("Defines the url of an image to display as the command.")]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string icon
        {
            set
            {
                this.SetStringAttribute("icon", this.GetRelativeUrl(value));
                return;
            }
            get
            {
                return this.GetRelativeUrl(this.GetStringAttribute("icon"));
            }
        }

        /// <summary>
        /// Defines a name for the command. The label is visible.
        /// </summary>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("Defines a name for the command. The label is visible.")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string label
        {
            set
            {
                this.SetStringAttribute("label", this.GetRelativeUrl(value));
                return;
            }
            get
            {
                return this.GetRelativeUrl(this.GetStringAttribute("label"));
            }
        }

        /// <summary>
        /// Defines the name of the radiogroup this command belongs to. Use only if type is radio.
        /// </summary>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("Defines the name of the radiogroup this command belongs to. Use only if type is radio.")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string radiogroup
        {
            set
            {
                this.SetStringAttribute("radiogroup", this.GetRelativeUrl(value));
                return;
            }
            get
            {
                return this.GetRelativeUrl(this.GetStringAttribute("radiogroup"));
            }
        }

        /// <summary>
        /// Defines the type of command. Default value is command.
        /// </summary>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("Defines the type of command. Default value is command.")]
        [DisplayName()]
        public CommandType @type
        {
            set
            {
                this.SetEnumAttribute("type", value, CommandType.Command);
                return;
            }
            get
            {
                return (CommandType) this.GetEnumAttribute("type", CommandType.Command);
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
        public CommandElement(IHtmlEditor editor)
            : base("command", editor)
        {
        }

        internal CommandElement(Interop.IHTMLElement peer, HtmlEditor editor)
            : base(peer, editor)
        {
        }
    }
}
