using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute = GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;

namespace GuruComponents.Netrix.WebEditing.Elements.Html5
{
    /// <summary>
    /// This class represents the &lt;article&gt; element.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This element indicates an article. The tag specifies independent, self-contained content.
    /// An article should make sense on its own and it should be possible to distribute it independently from the rest of the site.
    ///
    ///Examples of possible articles:
    ///<list type="bullet">
    ///<term>forum post</term>
    ///<term>newspaper article</term>
    ///<term>blog entry</term>
    ///<term>user comment</term>
    ///</list>
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
    public class ArticleElement : Html5Base
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
        [Description("title_Article")]
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
        public ArticleElement(IHtmlEditor editor)
            : base("article", editor)
        {
        }

        internal ArticleElement(Interop.IHTMLElement peer, HtmlEditor editor)
            : base(peer, editor)
        {
        }
    }
}
