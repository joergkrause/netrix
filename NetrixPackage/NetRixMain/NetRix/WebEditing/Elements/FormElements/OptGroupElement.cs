using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using System;
using System.ComponentModel;

using TE = GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    public sealed class OptGroupElement : StyledElement, IOptionElement
    {
        /// <summary>
        /// This field stores the option elements which are part of this option group.
        /// </summary>
        private OptionOnlyElementsCollection _options;


        /// <summary>
        /// Gets or sets the options within this OPTGROUP.
        /// </summary>
        /// <remarks>
        /// This property supports the usage of collections for the options (OPTION elements). The intention is to support the
        /// NetRix UI (not applicable in the Light version).
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.OptionElement">OptionElement (OPTION)</seealso>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.SelectElement">SelectElement (SELECT)</seealso>
        /// </remarks>

        [CategoryAttribute("Content")]
        [Description("")]
        [EditorAttribute(
             typeof(UserInterface.TypeEditors.OptionOnlyCollectionEditor),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]

        public OptionOnlyElementsCollection Options
        {
            get
            {
                return _options;
            }
            set
            {
                _options = value;
            }
        }

        /// <summary>
        /// The LABEL attribute specifies the group label presented to the user. Required.
        /// </summary>
        /// <remarks>
        /// The LABEL should describe the group of choices available through the OPTGROUP's OPTIONs. Each OPTION generally uses a LABEL attribute as well to provide a shortened label that, together with the OPTGROUP's LABEL, gives a complete description of the option. 
        /// </remarks>

        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute("")]
        [Description("")]
        [EditorAttribute(
             typeof(UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]

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

        /// <summary>
        /// DO NOT USE!
        /// </summary>
        /// <remarks>
        /// This non browsable property is only implemented to allow the usage of the interface <see cref="IOptionElement"/>
        /// which is used to group OPTION and OPTGROUP in the same collection.
        /// <para>
        /// <b>Warning:</b> This property is for the NetRix infrastructure only. Please do NEVER call from own code.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public string text
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        /// <summary>
        /// DO NOT USE!
        /// </summary>
        /// <remarks>
        /// This non browsable property is only implemented to allow the usage of the interface <see cref="IOptionElement"/>
        /// which is used to group OPTION and OPTGROUP in the same collection.
        /// <para>
        /// <b>Warning:</b> This property is for the NetRix infrastructure only. Please do NEVER call from own code.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public string value
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        /// <summary>
        /// DO NOT USE!
        /// </summary>
        /// <remarks>
        /// This non browsable property is only implemented to allow the usage of the interface <see cref="IOptionElement"/>
        /// which is used to group OPTION and OPTGROUP in the same collection.
        /// <para>
        /// <b>Warning:</b> This property is for the NetRix infrastructure only. Please do NEVER call from own code.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public bool selected
        {
            get
            {
                return false;
            }
            set
            {
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
        [TypeConverter(typeof(UserInterface.TypeConverters.UITypeConverterDropList))]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
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

        public override string ToString()
        {
            return String.Concat("<OPTGROUP> ", ((_options == null) ? 0 : _options.Count), " OPTIONs");
        }

        private void BuildCollection(Interop.IHTMLElement peer)
        {
            if (_options == null)
            {
                Interop.IHTMLElement select = peer.GetParentElement();
                _options = new OptionOnlyElementsCollection(peer, base.HtmlEditor);
                Interop.IHTMLElementCollection coll = select.GetChildren() as Interop.IHTMLElementCollection;
                if (coll != null && coll.GetLength() > 0)
                {
                    bool beginBlock = false;
                    for (int i = 0; i < coll.GetLength(); i++)
                    {
                        Interop.IHTMLElement option = coll.Item(i, i) as Interop.IHTMLElement;
                        if (option != null)
                        {
                            if (beginBlock && option.GetTagName().Equals("OPTGROUP"))
                            {
                                break;
                            }
                            if (beginBlock)
                            {
                                _options.Add(base.HtmlEditor.GenericElementFactory.CreateElement(option) as OptionElement);
                            }
                            if (option.Equals(peer))
                            {
                                beginBlock = true;
                                continue;
                            }
                        }
                    }
                }
                _options.OnInsertHandler += new CollectionInsertHandler(_options_OnInsertHandler);
                _options.OnClearHandler += new CollectionClearHandler(_options_OnClearHandler);
            }
            else
            {
                _options.Clear();
            }
        }

        /// <summary>
        /// For use with the collection editor.
        /// </summary>
        /// <remarks>
        /// Callers should be use the collections to add elements
        /// instead of creating them directly under normal circumstances.
        /// </remarks>
        public OptGroupElement()
            : base("optgroup", null)
        {
            BuildCollection(base.GetBaseElement());
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
        public OptGroupElement(IHtmlEditor editor)
            : base("optgroup", editor)
        {
        }

        /// <summary>
        /// For internal use only. Please do not call from own code.
        /// </summary>
        internal OptGroupElement(Interop.IHTMLElement peer, IHtmlEditor editor)
            : base(peer, editor)
        {
            BuildCollection(peer);
        }

        private void _options_OnInsertHandler(int index, object value)
        {
            Interop.IHTMLElement optgroup = base.GetBaseElement();
            Interop.IHTMLDOMNode optNode = (Interop.IHTMLDOMNode)optgroup;
            optNode.appendChild((Interop.IHTMLDOMNode)((OptionElement)value).GetBaseElement());
        }

        private void _options_OnClearHandler()
        {
            Interop.IHTMLElement optgroup = base.GetBaseElement();
            Interop.IHTMLDOMNode optNode = (Interop.IHTMLDOMNode)optgroup;
            Interop.IHTMLElementCollection subNodes = (Interop.IHTMLElementCollection)optgroup.GetChildren();
            for (int i = 0; i < subNodes.GetLength(); i++)
            {
                optNode.removeChild((Interop.IHTMLDOMNode)subNodes.Item(i, i));
            }
        }

    }
}
