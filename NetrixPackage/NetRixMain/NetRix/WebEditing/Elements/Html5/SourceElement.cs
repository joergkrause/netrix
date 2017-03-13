using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute = GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;

namespace GuruComponents.Netrix.WebEditing.Elements.Html5
{
    /// <summary>
    /// This class represents the &lt;source&gt; element. (HTML 5)
    /// </summary>
    /// <remarks>
    /// <para>
    /// The SOURCE tag defines media resources for media elements, such as VIDEO and AUDIO.
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
    /// <example>
    /// &lt;audio controls="controls">
    ///   &lt;source src="song.ogg" type="audio/ogg" />
    ///   &lt;source src="song.mp3" type="audio/mpeg" />
    /// Your browser does not support the audio element.
    /// &lt;/audio>
    /// </example>
    /// </remarks>
    /// <seealso cref="AudioElement"/>
    public class SourceElement : Html5Base
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
        [Description("title_Source")]
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
        /// The URL of the media.
        /// </summary>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("The URL of the media.")]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string src
        {
            set
            {
                this.SetStringAttribute("src", this.GetRelativeUrl(value));
                return;
            }
            get
            {
                return this.GetRelativeUrl(this.GetStringAttribute("src"));
            }
        }

        /// <summary>
        /// The TYPE attribute of SOURCE specifies the media type.
        /// </summary>
        /// <remarks>
        /// Specifies the MIME type of the media resource.
        /// The TYPE attribute of SOURCE specifies the media type of the scripting language, e.g., audio/mp3.
        /// </remarks>
        [CategoryAttribute("Element Behavior")]
        [DefaultValue("")]
        [Description("")]
        [DisplayName("Specifies the MIME type of the media resource.")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        public string type
        {
            set
            {
                this.SetStringAttribute("type", value);
                return;
            }

            get
            {
                return this.GetStringAttribute("type");
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
        public SourceElement(IHtmlEditor editor)
            : base("source", editor)
        {
        }

        internal SourceElement(Interop.IHTMLElement peer, HtmlEditor editor)
            : base(peer, editor)
        {
        }
    }
}
