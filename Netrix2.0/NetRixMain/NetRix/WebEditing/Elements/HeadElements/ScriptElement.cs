using System;
using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.WebEditing.Styles;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The SCRIPT element includes a client-side script in the document.
    /// </summary>
    /// <remarks>
    /// Client-side scripts allow greater interactivity in a document by responding to user events. For example, a script 
    /// could be used to check the user's form input prior to submission to provide immediate notice of any errors by the user.
    /// </remarks>
    public class ScriptElement : Element
    {

        /// <summary>
        /// The Script content as HTML.
        /// </summary>
        [Browsable(false)]
        public new string InnerHtml
        {
            get { return this.ScriptContent; }
        }

        /// <summary>
        /// The script content as Text.
        /// </summary>
        [Browsable(false)]
        public new string InnerText
        {
            get { return this.ScriptContent; }
        }



        [Browsable(false)]
        private new IEffectiveStyle EffectiveStyle
        {
            get
            {
                throw new NotSupportedException("This property is not available for head elements");
            }
        }


        /// <summary>
        /// The CHARSET attribute gives the character encoding. Optional.
        /// </summary>
        /// <remarks>
        /// The character encoding of the external script is typically a string like ISO-8859-1. 
        /// </remarks>

        [CategoryAttribute("Element Behavior")]
        [DefaultValue("")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string charset
        {
            set
            {
                this.SetStringAttribute ("charset", value);
                return;
            } 
      
            get
            {
                return this.GetStringAttribute("charset");
            } 
      
        }

        /// <summary>
        /// The SRC attribute specifies an external script. Optional.
        /// </summary>
        /// <remarks>
        /// The SRC attribute allows authors to reuse code by specifying an external script.
        /// When the SRC attribute is used, the embedded script is ignored.
        /// </remarks>

        [Category("Content")]
        [DefaultValue("")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string src
        {
            set
            {
                this.SetStringAttribute ("src", this.GetRelativeUrl(value));
                return;
            } 
            get
            {
                return this.GetRelativeUrl (this.GetStringAttribute ("src"));
            }          
        }

        /// <summary>
        /// LANGUAGE determines the script language used.
        /// </summary>
        /// <remarks>
        /// Browsers will ignore scripts with LANGUAGE values that they do not support. For example, Netscape Navigator 3.0 will execute scripts with LANGUAGE="JavaScript" or LANGUAGE="JavaScript1.1" but will ignore scripts with LANGUAGE="JavaScript1.2" or LANGUAGE="VBScript".
        /// <para>
        /// In the absence of the LANGUAGE attribute, browsers that do not support the TYPE attribute typically assume that the language is the highest version of JavaScript supported by the browser. Thus authors may safely omit the deprecated LANGUAGE attribute when using JavaScript.
        /// </para>
        /// </remarks>

        [CategoryAttribute("Element Behavior")]
        [DefaultValue("")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string language
        {
            set
            {
                this.SetStringAttribute ("language", value);
                return;
            } 
      
            get
            {
                return this.GetStringAttribute("language");
            } 
      
        }

        /// <summary>
        /// The TYPE attribute of SCRIPT specifies the media type. Required.
        /// </summary>
        /// <remarks>
        /// The required TYPE attribute of SCRIPT specifies the media type of the scripting language, e.g., text/javascript.
        /// However, many browsers only support the deprecated LANGUAGE attribute, which specifies the language name.
        /// </remarks>
        [CategoryAttribute("Element Behavior")]
        [DefaultValue("")]
        [Description("")]
        [DisplayName("type_Script")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        public string type
        {
            set
            {
                this.SetStringAttribute ("type", value);
                return;
            } 
      
            get
            {
                return this.GetStringAttribute("type");
            } 
        }

        /// <summary>
        /// The content of the embedded script.
        /// </summary>
        /// <remarks>
        /// This is used to access the content between the SCRIPT tags. It is recommended to create an
        /// editor for scripting which supports external and internal script blocks in the same way.
        /// </remarks>

        [Category("Content")]
        [DefaultValue("")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorScript),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string ScriptContent
        {
            set
            {
                ((Interop.IHTMLScriptElement)base.GetBaseElement()).text = value;
            } 
      
            get
            {
                return ((Interop.IHTMLScriptElement)base.GetBaseElement()).text;
            }       
        }

 /// <summary>
        /// Creates the specified element.
        /// </summary>
        /// <remarks>
        /// The element is being created and attached to the current document, but nevertheless not visible,
        /// until it's being placed anywhere within the DOM. To attach an element it's possible to either
        /// use the <see cref="ElementDom"/> property of any other already placed element and refer to this
        /// DOM or use the body element (<see cref="IHtmlEditor.GetBodyElement"/>) and add the element there. Also, in 
        /// case of user interactive solutions, it's possible to add an element near the current caret 
        /// position, using <see cref="IHtmlEditor.CreateElementAtCaret(string)"/> method.
        /// <para>
        /// Note: Invisible elements do neither appear in the DOM nor do they get saved.
        /// </para>
        /// </remarks>
        /// <param name="editor">The editor this element belongs to.</param>
        public ScriptElement(IHtmlEditor editor)
            : base("script", editor)
        {
            this.language = "JavaScript";
        }

        internal ScriptElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}