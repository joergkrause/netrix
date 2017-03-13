using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The SELECT element defines a form control for the selection of options.
    /// </summary>
    /// <remarks>
    /// While SELECT is most useful within a FORM, HTML 4.0 allows SELECT in any block-level or inline element other than BUTTON. However, Netscape Navigator will not display any SELECT elements outside of a FORM.
    /// <para>
    /// The SELECT element contains one or more OPTGROUP or OPTION elements to provide a menu of choices for the user. Each choice is contained within an OPTION element. Choices can be grouped logically through the OPTGROUP element. SELECT's NAME attribute provides the key sent to the server with the value of the selected option.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.OptionElement">OptionElement (OPTION)</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.OptGroupElement">OptGroupElement (OPTGROUP)</seealso>
    /// </remarks>
    public class SelectElement : SelectableElement
    {

        /// <summary>
        /// This field stores the option elements which are part of the select box.
        /// </summary>
		private OptionElementsCollection _options;

        /// <summary>
        /// Gets or sets a collection of OPTION or OPTGROUP elements.
        /// </summary>
        /// <remarks>
        /// Any OPTGROUP element is rendered as bold and italic cannot be selected by the user. This is an
        /// HTML 4 enhancement and not widely supported by elder browsers.
        /// </remarks>
        [Category("Element Behavior")]
        [Description("")]
        [EditorAttribute(
            typeof(OptionCollectionEditor),
            typeof(UITypeEditor))]
        [DisplayNameAttribute()]
		public OptionElementsCollection Options
		{
			get
			{
                CreateOptionList((Interop.IHTMLSelectElement)base.GetBaseElement());
				return _options;
			}
		}

        /// <summary>
        /// The boolean MULTIPLE attribute allows the user to select multiple options.
        /// </summary>
        /// <remarks>
        /// By default, the user can only select one option. The boolean MULTIPLE attribute allows the user to select multiple options, which are submitted as separate name/value pairs. 
        /// </remarks>

		[DefaultValueAttribute(false)]
		[TypeConverter(typeof(UITypeConverterDropList))]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [DisplayName()]

        public bool multiple
		{
			get
			{
				return base.GetBooleanAttribute("multiple");
			}

			set
			{
				base.SetBooleanAttribute("multiple", value);
			}
		}


        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string name
        {
            get
            {
                return base.GetStringAttribute("name");
            }

            set
            {
                base.SetStringAttribute("name", value);
            }
        }


        [CategoryAttribute("JavaScript Events")]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnChange
        {
            get
            {
                return base.GetStringAttribute("onChange");
            }

            set
            {
                base.SetStringAttribute("onChange", value);
            }
        }


		[DefaultValueAttribute(1)]
        [CategoryAttribute("Element Layout")]
        [Description("size_Select")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName("size_Select")]

		public int size
		{
			get
			{
				return base.GetIntegerAttribute("size", 1);
			}

			set
			{
				base.SetIntegerAttribute("size", value, 0);
			}
		}


		[DefaultValueAttribute(0)]
        [CategoryAttribute("Element Behavior")]
        [Description("The internal index of the selection, in case of a unique selection.")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName("Selected Index")]

		public int SelectedIndex
		{
			get
			{
				return ((Interop.IHTMLSelectElement) GetBaseElement()).selectedIndex;
			}
			set
			{
				((Interop.IHTMLSelectElement) GetBaseElement()).selectedIndex = value;
			}
		}

        private void _options_OnInsertHandler(int index, object value)
        {
            Interop.IHTMLElement selectElement = (Interop.IHTMLElement) base.GetBaseElement();
            Interop.IHTMLDOMNode selNode = (Interop.IHTMLDOMNode) selectElement;
            Interop.IHTMLDOMNode optGrpNode = null;
            switch (value.GetType().Name)
            {
                case "OptionElement":
                    // add OPTION directly...
                    optGrpNode = selNode.appendChild((Interop.IHTMLDOMNode) ((OptionElement) value).GetBaseElement());
                    // ...and handle InnerText separatly because this is not a real attribute
                    //((Interop.IHTMLElement) optGrpNode).SetInnerText(((OptionElement) value).text);
                    //optGrpNode.nodeValue = ((OptionElement) value).text;
                    break;
                case "OptGroupElement":
                    // add OPTGROUP and the OPTION children the element already has
                    OptGroupElement optGrpObj = (OptGroupElement) value;
                    optGrpNode = selNode.appendChild((Interop.IHTMLDOMNode) (optGrpObj).GetBaseElement());
                    if (optGrpObj.Options != null && optGrpObj.Options.Count > 0)
                    {
                        foreach (OptionElement oe in optGrpObj.Options)
                        {
                            optGrpNode.appendChild((Interop.IHTMLDOMNode) oe.GetBaseElement());
                        }
                    }
                    break;
            }            
        }

        private void _options_OnClearHandler()
        {
            // we do not clear the collection here to protect the underlying conjunction between collection and element
            Interop.IHTMLElement selectElement = (Interop.IHTMLElement) base.GetBaseElement();
            Interop.IHTMLDOMNode selNode = (Interop.IHTMLDOMNode) selectElement;
            if (selNode.hasChildNodes())
            {
                Interop.IHTMLElementCollection options = (Interop.IHTMLElementCollection) selectElement.GetChildren();                
                for (int o = 0; o < options.GetLength(); o++)
                {
                    Interop.IHTMLDOMNode childElement = (Interop.IHTMLDOMNode) options.Item(0, 0);
                    // remove first node, because the collection will change immediately after removeChild call
                    selNode.removeChild(childElement);
                }
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
        public SelectElement(IHtmlEditor editor)
            : base("select", editor)
        {
        }

        internal SelectElement(string tag, IHtmlEditor editor)
            : base(tag, editor)
        {
        }

        internal SelectElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
            Interop.IHTMLSelectElement s = (Interop.IHTMLSelectElement) peer;            
            CreateOptionList(s);
        }

        private void CreateOptionList(Interop.IHTMLSelectElement s)
        {
            if (_options == null)
            {
                _options = new OptionElementsCollection(s, base.HtmlEditor);
                _options.OnInsertHandler += new CollectionInsertHandler(_options_OnInsertHandler);
                _options.OnClearHandler += new CollectionClearHandler(_options_OnClearHandler);
            }
            else
            {
                Debug.WriteLine("Recreate List");
                _options.OnInsertHandler -= new CollectionInsertHandler(_options_OnInsertHandler);
                _options.OnClearHandler -= new CollectionClearHandler(_options_OnClearHandler);
                _options.Clear();
                Interop.IHTMLElement selectElement = (Interop.IHTMLElement)s;
                Interop.IHTMLElementCollection options = (Interop.IHTMLElementCollection)selectElement.GetChildren();
                for (int i = 0; i < options.GetLength(); i++)
                {
                    Interop.IHTMLElement el = (Interop.IHTMLElement)options.Item(i, i);
                    IOptionElement oe = null;
                    switch (el.GetTagName())
                    {
                        case "OPTION":
                            oe = new OptionElement(el, base.HtmlEditor);
                            break;
                        case "OPTGROUP":
                            oe = new OptGroupElement(el, base.HtmlEditor);
                            break;
                    }
                    if (oe != null)
                    {
                        _options.Add(oe);
                    }
                }
                _options.OnInsertHandler += new CollectionInsertHandler(_options_OnInsertHandler);
                _options.OnClearHandler += new CollectionClearHandler(_options_OnClearHandler);
            }
        }


    }
}
