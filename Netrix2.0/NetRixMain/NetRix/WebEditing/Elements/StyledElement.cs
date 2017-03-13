using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This class is used as a base class for various elements to simplify the definition of
    /// commonly used attributes.
    /// </summary>
    public abstract class StyledElement : SimpleInlineElement
    {

        /// <summary>
        /// onClick sets a script to run when the user clicks on the link.
        /// </summary>
        [Description("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("JavaScript Events")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]
        [ScriptingVisible()] public string ScriptOnClick
        {
            get
            {
                return base.GetStringAttribute("onClick");
            }

            set
            {
                base.SetStringAttribute("onClick", value);
            }
        }

        /// <summary>
        /// onMouseOver is an event handlers that is triggered when the mouse moves over the link. 
        /// </summary>
        /// <remarks>
        /// The most common use for these event handler are rollover images, but you put any kind of program in them you want.
        /// </remarks>
        [CategoryAttribute("JavaScript Events")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        [ScriptingVisible()] 
        public string ScriptOnMouseOver
        {
            get
            {
                return base.GetStringAttribute("onMouseOver");
            }

            set
            {
                base.SetStringAttribute("onMouseOver", value);
            }
        }

        /// <summary>
        /// onMouseOut is an event handler that is triggered when the mouse moves out from the element again.
        /// </summary>
        /// <remarks>
        /// The most common use for these event handler are rollover images, but you put any kind of program in them you want.
        /// </remarks> 
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("JavaScript Events")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        [ScriptingVisible()] 
        public string ScriptOnMouseOut
        {
            get
            {
                return base.GetStringAttribute("onMouseOut");
            }

            set
            {
                base.SetStringAttribute("onMouseOut", value);
            }
        }

        /// <summary>
        /// W3C says that TITLE is "an advisory title for the linked resource".
        /// </summary>
        /// <remarks>
        /// The idea is that TITLE gives a description of the linked resource that is more informative than the URL.
        /// <para>
        /// Most browsers have ignored this useful attribute. One that doesn't ignore it is Opera. Opera displays the value of TITLE in a small box when the pointer is over a link. 
        /// The latest version of MSIE also shows a little box over the link, but otherwise ignores TITLE.
        /// </para>
        /// </remarks>
        [DescriptionAttribute("")]
        [CategoryAttribute("Standard Values")]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public virtual string title
        {
            get
            {
                return base.GetStringAttribute("title");
            }

            set
            {
                base.SetStringAttribute("title", value);
            }
        }

		internal StyledElement(string newTag, IHtmlEditor editor) : base(newTag, editor)
		{
		}

        internal StyledElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
