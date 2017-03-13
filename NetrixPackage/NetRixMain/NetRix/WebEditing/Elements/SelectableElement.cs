using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using System;
using System.ComponentModel;

using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// A base class used to support the NetRix infrastructure.
    /// </summary>
    public abstract class SelectableElement : StyledElement
    {

        /// <summary>
        /// The ACCESSKEY attribute.
        /// </summary>
        /// <remarks>
        /// By default, the accessKey property sets focus to the object. The object receives focus when the user simultaneously presses the ALT key and the accelerator key assigned to an object. Some controls perform an action after receiving focus. For example, using accessKey on a input type=button causes the onclick event to fire. By comparison, applying the accessKey on a radio button causes the onclick event to fire and toggles the checked property, visibly selecting or deselecting the control.
        /// </remarks>

        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]

        public string accessKey
        {
            get
            {
                return base.GetStringAttribute("accessKey");
            }

            set
            {
                if (value != null && value.Length != 1)
                {
                    throw new ArgumentException("The value mast be a string with lenght 1.");
                }
                base.SetStringAttribute("accessKey", value);
            }
        }

        /// <summary>
        /// The DISABLED attribute, disables the element.
        /// </summary>
        /// <remarks>
        /// The most elements are drawn grayed or with a gray shadow to inform the user that the element is disabled.
        /// Field elements do not except the focus and do not display the caret.
        /// <para>
        /// In HTML the element is disables if the attribute exists and it is enabled if it doesn't exist. The parameter assigned
        /// to the attribute is useless.
        /// </para>
        /// </remarks>

		[CategoryAttribute("Element Behavior")]
		[DefaultValueAttribute(false)]
		[DescriptionAttribute("")]
        [TypeConverter(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterDropList))]
        [TE.DisplayName()]

		public bool disabled
		{
			get
			{
				return base.GetBooleanAttribute("disabled");
			}

			set
			{
				base.SetBooleanAttribute("disabled", value);
			}
		}

        /// <summary>
        /// Sets or retrieves the value indicating whether the object visibly indicates that it has focus.
        /// </summary>
        /// <remarks>
        /// The focus of an object is visibly indicated by a focus rectangle—a dotted rectangle within the boundaries of the object.
        /// This property does not control the ability of an object to receive focus; for that, use the tabIndex property.
        /// </remarks>

        [DescriptionAttribute("")]
        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(false)]
        [TypeConverter(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterDropList))]
        [TE.DisplayName()]

        public bool hideFocus
        {
            get
            {
                return base.GetBooleanAttribute("hideFocus");
            }

            set
            {
                base.SetBooleanAttribute("hideFocus", value);
            }
        }

        /// <summary>
        /// Defines an client site event which is fired if the element receives the focus.
        /// </summary>

        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [CategoryAttribute("JavaScript Events")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]

        [ScriptingVisible()] public string ScriptOnFocus
        {
            get
            {
                return base.GetStringAttribute("onFocus");
            }

            set
            {
                base.SetStringAttribute("onFocus", value);
            }
        }

        /// <summary>
        /// Defines an client site event which is fired if the element registered a key stroke.
        /// </summary>

        [DefaultValueAttribute("")]
        [CategoryAttribute("JavaScript Events")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]

        [ScriptingVisible()] public string ScriptOnKeyPress
        {
            get
            {
                return base.GetStringAttribute("onKeyPress");
            }

            set
            {
                base.SetStringAttribute("onKeyPress", value);
            }
        }

        /// <summary>
        /// Gets or set the TAB index.
        /// </summary>
        /// <remarks>
        /// If the user hits the TAB key the focus moves from block element to block element, beginning with the first element on a page.
        /// To change the default TAB chain the tabIndex can be set to any numeric (integer) value which force the order of the TAB key.
        /// The value must be greater or equal than 0 (zero).
        /// <para>
        /// To remove the attribute set the value to 0 (zero).
        /// </para>
        /// </remarks>

        [DefaultValueAttribute(0)]
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]

        public virtual short tabIndex
        {
            get
            {
                return (short)base.GetIntegerAttribute("tabIndex", 0);
            }

            set
            {
                base.SetIntegerAttribute("tabIndex", value, 0);
            }
        }

        internal SelectableElement(string tag, IHtmlEditor editor) : base (tag, editor)
        {
        }

        internal SelectableElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
