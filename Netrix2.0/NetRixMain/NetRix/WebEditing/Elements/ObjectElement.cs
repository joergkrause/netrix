using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The OBJECT element.
    /// </summary>
    public sealed class ObjectElement : EmbeddedBaseElement
    {


        [Category("Element Behavior")]
        [DefaultValue("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [Description("title_Acronym")]
        [DisplayNameAttribute()]
        public string code
        {
            set
            {
                this.SetStringAttribute ("code", value);
                return;
            } 
      
            get
            {
                return this.GetStringAttribute("code");
            } 
      
        }


        [CategoryAttribute("Element Behavior")]
        [DefaultValue("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [Description("title_Acronym")]
        [DisplayName()]
        public string codeBase
        {
            set
            {
                this.SetStringAttribute ("codebase", value);
                return;
            } 
      
            get
            {
                return this.GetStringAttribute("codebase");
            } 
      
        }


        [CategoryAttribute("Element Behavior")]
        [DefaultValue("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [Description("title_Acronym")]
        [DisplayName()]
        public string classid
        {
            set
            {
                this.SetStringAttribute ("classid", value);
                return;
            } 
      
            get
            {
                return this.GetStringAttribute("classid");
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
        public ObjectElement(IHtmlEditor editor)
            : base("object", editor)
        {
        }

        internal ObjectElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
