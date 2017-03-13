using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;
using System;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This class represents the &lt;applet&gt; element.
    /// </summary>
    /// <remarks>
    /// <para>
    /// APPLET is deprecated (with all its attributes) in favor of OBJECT. See <see cref="GuruComponents.Netrix.WebEditing.Elements.ObjectElement"/>
    /// for more information.
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
    [Obsolete("This element is not supported in HTML 5 and should not be used in future projects")]
    public sealed class AppletElement : EmbeddedBaseElement
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

        public string @object
        {
            set
            {
                this.SetStringAttribute ("object", value);
                return;
            } 
      
            get
            {
                return this.GetStringAttribute("object");
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
        public AppletElement(IHtmlEditor editor)
            : base("applet", editor)
        {
        }

        internal AppletElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
