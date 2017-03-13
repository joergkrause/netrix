using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// &lt;BASE ...&gt; tells the browser to pretend that the current page is located at some URL other than where the browser found it. 
    /// </summary>
    /// <remarks>
    /// Any relative reference will be calculated from the URL given by &lt;BASE HREF="..."> instead of the actual URL. &lt;BASE ...> goes in the &lt;HEAD> section. 
    /// <para>
    /// Generally it's best to avoid using &lt;BASE ...>. It usually just restricts the ability to move a set of web pages from one location to another 
    /// (say, from your computer where you are developing them to the server where they publicly reside). 
    /// However, &lt;BASE ...> can come in handy in development situations where the final version of the page will make relative references 
    /// to resources that aren't on the development machine. 
    /// </para>
    /// </remarks>
    public sealed class BaseElement : Element
    {

        /// <summary>
        /// The default address for hypertext links.
        /// </summary>

        [Category("Element Behavior")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string href
        {
            set
            {
                this.SetStringAttribute ("href", this.GetRelativeUrl(value));
                return;
            } 
            get
            {
                return this.GetRelativeUrl (this.GetStringAttribute ("href"));
            }  
        }

        /// <include file='DocumentorIncludes.xml' path='WebEditing/Elements[@name="TargetAttribute"]'/>
        [CategoryAttribute("Element Behavior")]
        [TypeConverterAttribute(typeof(TargetConverter))]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [DisplayName()]

        public string target
        {
            get
            {
                return base.GetStringAttribute("target");
            }

            set
            {
                base.SetStringAttribute("target", value);
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
        public BaseElement(IHtmlEditor editor)
            : base("base", editor)
        {
        }
        internal BaseElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
