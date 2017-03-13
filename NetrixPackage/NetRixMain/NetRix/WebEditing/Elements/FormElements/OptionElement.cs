using System;
using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The OPTION element defines a menu choice within a SELECT menu.
    /// </summary>
    /// <remarks>
    /// The value of the option, sent with a submitted form, is specified with the VALUE attribute. In the absence of a VALUE attribute, the value is the content of the OPTION element.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.SelectElement">SelectElement (SELECT)</seealso>
    /// </remarks>
	public sealed class OptionElement : StyledElement, IOptionElement
	{

        /// <summary>
        /// The LABEL attribute specifies the option label presented to the user.
        /// </summary>
        /// <remarks>
        /// This defaults to the content of the OPTION element, but the LABEL attribute allows authors to more easily use OPTGROUP without sacrificing compatibility with browsers that do not support option groups.
        /// </remarks>

		[Category("Element Behavior")]
		[DefaultValueAttribute("")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string label
		{
            get
            {
                return base.GetStringAttribute("label");
            }

            set
            {
                base.SetStringAttribute("label", value);
            }
		}


        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute("")]
        [Description("text_Field")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName("text_Field")]

        public string text
        {
            get
            {
                return base.InnerText;
            }

            set
            {
                base.InnerText = value;
            }
        }


		[CategoryAttribute("Element Behavior")]
		[DefaultValueAttribute("")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

		public string @value
		{
			get
			{
				return base.GetStringAttribute("value");
			}

			set
			{
				base.SetStringAttribute("value", value);
			}
		}

        /// <summary>
        /// The boolean SELECTED attribute defines the OPTION to be initially selected. 
        /// </summary>
        /// <remarks>
        /// A SELECT element can only have one OPTION selected at any time unless the MULTIPLE attribute is present on SELECT.
        /// </remarks>

        [TypeConverter(typeof(UITypeConverterDropList))]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [DisplayName()]

        public bool selected
		{
			get
			{
				return base.GetBooleanAttribute("selected");
			}

			set
			{
                Interop.IHTMLElement selectedElement = base.GetBaseElement().GetParentElement();
                if (selectedElement != null)
                {
                    Interop.IHTMLSelectElement s = selectedElement as Interop.IHTMLSelectElement;
                    if (s != null)
                    {
                        if (!s.multiple && s.size <= 1)
                        {
                            for (int i = 0; i < s.length; i++)
                            {
                                Interop.IHTMLOptionElement option = s.item(i, i) as Interop.IHTMLOptionElement;
                                if (option != null && option.Equals(GetBaseElement()))
                                {                                    
                                    s.selectedIndex = i;
                                    IElement selectWrapped = base.HtmlEditor.GenericElementFactory.CreateElement(s as Interop.IHTMLElement) as IElement;
                                    ((SelectElement)selectWrapped ).SelectedIndex = i;
                                    break;
                                }
                            }
                        }                        
                    }
                }                
				base.SetBooleanAttribute("selected", value);
			}
		}

        /// <summary>
        /// Disables the element.
        /// </summary>
        /// <remarks>
        /// The most elements are drawn grayed or with a gray shadow to inform the user that the element is disabled.
        /// Field elements do not except the focus and do not display the caret.
        /// <para>
        /// In HTML the element is disables if the attribute exists and it is enabled if it doesn't exist. The parameter assigned
        /// to the attribute is useless.
        /// </para>
        /// </remarks>

		[DefaultValueAttribute(false)]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [DisplayName()]

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

        public override string ToString()
        {
            return String.Concat("<OPTION>", this.text);
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
        public OptionElement(IHtmlEditor editor)
            : base("option", editor)
        {
        }

		/// <summary>
		/// For internal use only. Please do not call from own code.
		/// </summary>
		internal OptionElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
        }
        }

}
