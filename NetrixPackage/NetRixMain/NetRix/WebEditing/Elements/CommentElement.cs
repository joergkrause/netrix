using System;
using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// Allows access to a HTML comment.
    /// </summary>
    /// <remarks>
    /// Each comment is treated as an tag object and can contain simple text, which is managed by the one and only
    /// property <see cref="GuruComponents.Netrix.WebEditing.Elements.CommentElement.Content">Content</see>.
    /// </remarks>
    public sealed class CommentElement : Element
    {

        /// <summary>
        /// Sets an attribute to a specific value.
        /// </summary>
        /// <remarks>
        /// Does nothing for TextNodeElement.
        /// </remarks>
        /// <param name="attribute">The name of the attribute.</param>
        /// <param name="value">The value beeing written.</param>
        public override void SetAttribute(string attribute, object value)
        {
            throw new MethodAccessException("This method is not accessible for TextNodeElement");
        }

        /// <summary>
        /// Universal access to any attribute.
        /// </summary>
        /// <remarks>
        /// Returns always <c>null</c> (<c>Nothing</c> in VB) for TextNodeElements.
        /// </remarks>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>The object which is the value of the attribute.</returns>
        public override object GetAttribute(string attribute)
        {
            throw new MethodAccessException("This method is not accessible for TextNodeElement");
        }

        /// <summary>
        /// Remove the give attribute from element.
        /// </summary>
        /// <param name="attribute">The name of the attribute which is about to be removed. Case insensitive.</param>
        public override void RemoveAttribute(string attribute)
        {
            throw new MethodAccessException("This method is not accessible for TextNodeElement");
        }


        /// <summary>
        /// The content of the comment.
        /// </summary>
        /// <remarks>
        /// The content of the comment is not changed or recognize in any way. It is recommended to not use 
        /// any HTML here.
        /// </remarks>
        [Category("Content")]
        [DefaultValue("")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorComment),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]
        public string Content
        {
            set
            {
                ((Interop.IHTMLCommentElement)base.GetBaseElement()).text = String.Concat("<!-- ", value, " -->");
            }       
            get
            {
                
                string content = ((Interop.IHTMLCommentElement)base.GetBaseElement()).text;
                if (content.StartsWith("<!-- "))
                {
                    content = content.Substring(5);
                }
                if (content.EndsWith(" -->"))
                {
                    content = content.Substring(0, content.Length - 4);
                }
                return content;
            } 
      
        }

        /// <summary>
        /// Supports the user interface.
        /// </summary>
        /// <remarks>
        /// This overwritten method is used to support the user interface with a common string representation (the user
        /// interface is not supported in the Light version, but this method will still return a valid string).
        /// </remarks>
        /// <returns>The string representation, &lt;!-- --&gt;</returns>
        public override string ToString()
        {
            return "<!-- -->";
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
        public CommentElement(IHtmlEditor editor)
            : base("<!-- -->", editor)
        {
        }

        internal CommentElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
