using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute = GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;

namespace GuruComponents.Netrix.WebEditing.Elements.Html5
{
    /// <summary>
    /// This class represents the &lt;video&gt; element.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The VIDEO tag specifies video, such as a movie clip or other video streams.
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
    public class VideoElement : AudioElement
    {
        /// <summary>
        /// Sets the width of the video player.
        /// </summary>
        [DefaultValueAttribute(100)]
        [CategoryAttribute("Element Layout")]
        [Description("Sets the width of the video player.")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorInt),
             typeof(System.Drawing.Design.UITypeEditor))]
        public int width
        {
            get
            {
                return base.GetIntegerAttribute("width", 0);
            }

            set
            {
                base.SetIntegerAttribute("width", value, 0);
            }
        }

        /// <summary>
        /// Sets the height of the video player.
        /// </summary>
        [DefaultValueAttribute(100)]
        [CategoryAttribute("Element Layout")]
        [Description("Sets the height of the video player.")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorInt),
             typeof(System.Drawing.Design.UITypeEditor))]
        public int height
        {
            get
            {
                return base.GetIntegerAttribute("height", 0);
            }

            set
            {
                base.SetIntegerAttribute("height", value, 0);
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
        public VideoElement(IHtmlEditor editor)
            : base("video", editor)
        {
        }

        internal VideoElement(Interop.IHTMLElement peer, HtmlEditor editor)
            : base(peer, editor)
        {
        }
    }
}
