using System;
using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.WebEditing.Styles;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The PARAM element provides parameters for the OBJECT and APPLET elements.
    /// </summary>
    /// <remarks>
    /// An OBJECT or APPLET may contain any number of PARAM elements prior to the alternate content that is also contained within the OBJECT or APPLET element.
    /// </remarks>
    public sealed class ParamElement : Element
    {


        /// <summary>
        /// This member overwrites the existings member to correct the PropertyGrid display.
        /// </summary>
        /// <remarks>
        /// This property cannot be used in user code. It supports the NetRix infrastructure only.
        /// </remarks>
        /// <exception cref="System.NotImplementedException">Always fired on call.</exception>
        [Browsable(false)]
        public new IEffectiveStyle EffectiveStyle
        {
            get
            {
                throw new NotImplementedException("Effective Style not available for that kind of element");
            }
        }


        /// <summary>
        /// The NAME of the parameter. Required.
        /// </summary>
        /// <remarks>
        /// The name is used within the applet to access the named parameter.
        /// </remarks>

        [CategoryAttribute("Common")]
        [DefaultValue("")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string name
        {
            set
            {
                this.SetStringAttribute ("name", value);
            } 
      
            get
            {
                return this.GetStringAttribute("name");
            } 
      
        }

        /// <summary>
        /// The VALUE of the parameter. Required.
        /// </summary>
        /// <remarks>
        /// The value is used within the applet to access the parameter value.
        /// </remarks>        

        [CategoryAttribute("Common")]
        [DefaultValue("")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string @value
        {
            set
            {
                this.SetStringAttribute ("value", value);
            } 
      
            get
            {
                return this.GetStringAttribute("value");
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
        public ParamElement(IHtmlEditor editor)
            : base("param", editor)
        {
        }
        internal ParamElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
