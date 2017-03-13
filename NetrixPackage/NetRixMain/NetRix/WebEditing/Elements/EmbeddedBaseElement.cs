using System;
using System.ComponentModel;
using System.Collections;
using System.Web.UI.WebControls;

using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;

using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// This base class supports the NetRix infrastructure.
    /// </summary>
    /// <remarks>
    /// Please do not use this class directly in any application.
    /// </remarks>
    public abstract class EmbeddedBaseElement : SimpleInlineElement
    {

        /// <summary>
        /// This field stores the option elements which are part of the select box.
        /// </summary>
        private ParameterElementsCollection _params;


        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.ParamCollectionEditor),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]

        public ParameterElementsCollection Params
        {
            get
            {
                SynchParams(GetBaseElement());
                return _params;
            }
            set
            {
                _params = value;
            }
        }

        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="HorizontalAlign"]/*'/>
        [DescriptionAttribute("")]
        [DefaultValueAttribute(System.Web.UI.WebControls.HorizontalAlign.Left)]
        [CategoryAttribute("Element Layout")]
        [TypeConverter(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterDropList))]
        [TE.DisplayName()]

        public HorizontalAlign align
        {
            set
            {
                this.SetEnumAttribute ("align", (HorizontalAlign) value, (HorizontalAlign) 0);
                return;
            } 
      
            get
            {
                return (HorizontalAlign) this.GetEnumAttribute ("align", (HorizontalAlign) 0);
            } 
        }

        /// <summary>
        /// Use as alt="text" attribute.</summary>
        /// <remarks>
        /// For user agents that cannot display images, forms, or applets, this 
        /// attribute specifies alternate text. The language of the alternate text is specified by the lang attribute. 
        /// </remarks>

        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]

        public string alt
        {
            get
            {
                return base.GetStringAttribute("alt");
            }

            set
            {
                base.SetStringAttribute("alt", value);
            }
        }


        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]

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


        [DefaultValueAttribute(1)]
        [CategoryAttribute("Element Layout")]
        [Description("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorInt),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName("size_Select")]
        public int width
        {
            get
            {
                return base.GetIntegerAttribute("width", 0);
            }

            set
            {
                base.SetIntegerAttribute("width", value, 0);
            }
        }


        [DefaultValueAttribute(1)]
        [CategoryAttribute("Element Layout")]
        [Description("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorInt),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName("")]
        public int height
        {
            get
            {
                return base.GetIntegerAttribute("height", 0);
            }

            set
            {
                base.SetIntegerAttribute("height", value, 0);
            }
        }


        [DefaultValueAttribute(1)]
        [CategoryAttribute("Element Layout")]
        [Description("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorInt),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName("size_Select")]

        public int vspace
        {
            get
            {
                return base.GetIntegerAttribute("vspace", 0);
            }

            set
            {
                base.SetIntegerAttribute("vspace", value, 0);
            }
        }


        [DefaultValueAttribute(1)]
        [CategoryAttribute("Element Layout")]
        [Description("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorInt),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName("")]

        public int hspace
        {
            get
            {
                return base.GetIntegerAttribute("hspace", 0);
            }

            set
            {
                base.SetIntegerAttribute("hspace", value, 0);
            }
        }


        /// <summary>
        /// Gets or sets the title attribute for this element.
        /// </summary>
        /// <remarks>
        /// Visible elements may render the title as a tooltip.
        /// Invisible elements may not render, but use internally to support design time environments.
        /// Some elements may neither render nor use but still accept the attribute.
        /// </remarks>
        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]
        public string title
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

        private void _param_OnInsertHandler(int index, object value)
        {
            Interop.IHTMLElement objectElement = (Interop.IHTMLElement) base.GetBaseElement();
            Interop.IHTMLDOMNode Node = (Interop.IHTMLDOMNode) objectElement;
            Interop.IHTMLDOMNode optGrpNode = null;
            // add PARAM directly...
            optGrpNode = Node.appendChild((Interop.IHTMLDOMNode) ((ParamElement) value).GetBaseElement());
        }

        private void _param_OnClearHandler()
        {
            Interop.IHTMLElement objectElement = (Interop.IHTMLElement) base.GetBaseElement();
            Interop.IHTMLDOMNode Node = (Interop.IHTMLDOMNode) objectElement;
            Interop.IHTMLDOMChildrenCollection param = (Interop.IHTMLDOMChildrenCollection) ((Interop.IHTMLDOMNode) Node).childNodes;
            for (int i = 0; i < param.length; i++)
            {	
                Interop.IHTMLDOMNode el = (Interop.IHTMLDOMNode) param.item(0);
                Node.removeChild(el);
            }
        }
        internal EmbeddedBaseElement(string tag, IHtmlEditor editor)
            : base(tag, editor)
        {
        }

        internal EmbeddedBaseElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
            Interop.IHTMLObjectElement o = (Interop.IHTMLObjectElement) peer;
            if (_params == null)
            {
                _params = new ParameterElementsCollection(o);
                _params.OnInsertHandler += new CollectionInsertHandler(_param_OnInsertHandler);
                _params.OnClearHandler += new CollectionClearHandler(_param_OnClearHandler);
            } 
            SynchParams(peer);
        }

        private void SynchParams(Interop.IHTMLElement peer)
        {
            _params.Clear();
            Interop.IHTMLDOMChildrenCollection param = (Interop.IHTMLDOMChildrenCollection) ((Interop.IHTMLDOMNode) peer).childNodes;
            for (int i = 0; i < param.length; i++)
            {	
                Interop.IHTMLElement el = (Interop.IHTMLElement) param.item(0); // current is always on pos 0, access reorders list
                object pe = null;
                switch (el.GetTagName())
                {
                    case "PARAM":
                        pe = new ParamElement(el, HtmlEditor);
                        break;
                }
                if (pe != null)
                {
                    _params.Add(pe);
                }
            }
        }
    }
}
