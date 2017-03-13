using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute = GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;

namespace GuruComponents.Netrix.WebEditing.Elements.Html5
{
    /// <summary>
    /// This class represents the &lt;article&gt; element.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This element indicates an article. The tag specifies independent, self-contained content.
    /// An article should make sense on its own and it should be possible to distribute it independently from the rest of the site.
    ///
    ///Examples of possible articles:
    ///<list type="bullet">
    ///<term>forum post</term>
    ///<term>newspaper article</term>
    ///<term>blog entry</term>
    ///<term>user comment</term>
    ///</list>
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
    public class AudioElement : Html5Base
    {

        /// <summary>
        /// In some browsers the title attribute can be used to show the full version of the expression 
        /// when you are holding the mouse over the abbreviation.
        /// </summary>
        [Category("Element Layout")]
        [DefaultValue("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [Description("title_Article")]
        [DisplayNameAttribute()]
        public new string title
        {
            set
            {
                this.SetStringAttribute("title", value);
                return;
            }
            get
            {
                return this.GetStringAttribute("title");
            }
        }

        /// <summary>
        /// This field stores the option elements which are part of the select box.
        /// </summary>
        private SourceElementsCollection _sources;

        /// <summary>
        /// Gets a collection of SOURCE elements. Optional.
        /// </summary>
        [Category("Element Behavior")]
        [Description("")]
        [EditorAttribute(
            typeof(OptionCollectionEditor),
            typeof(UITypeEditor))]
        [DisplayNameAttribute()]
        public SourceElementsCollection Options
        {
            get
            {
                CreateSourceList(base.GetBaseElement());
                return _sources;
            }
        }

        private void CreateSourceList(Interop.IHTMLElement s)
        {
            if (_sources == null)
            {
                _sources = new SourceElementsCollection(s, base.HtmlEditor);
                _sources.OnInsertHandler += new CollectionInsertHandler(_sources_OnInsertHandler);
                _sources.OnClearHandler += new CollectionClearHandler(_sources_OnClearHandler);
            }
            else
            {
                _sources.OnInsertHandler -= new CollectionInsertHandler(_sources_OnInsertHandler);
                _sources.OnClearHandler -= new CollectionClearHandler(_sources_OnClearHandler);
                _sources.Clear();
                Interop.IHTMLElement selectElement = (Interop.IHTMLElement)s;
                Interop.IHTMLElementCollection options = (Interop.IHTMLElementCollection)selectElement.GetChildren();
                for (int i = 0; i < options.GetLength(); i++)
                {
                    Interop.IHTMLElement el = (Interop.IHTMLElement)options.Item(i, i);
                    object oe = new SourceElement(el, (HtmlEditor)base.HtmlEditor);
                    _sources.Add(oe);
                }
                _sources.OnInsertHandler += new CollectionInsertHandler(_sources_OnInsertHandler);
                _sources.OnClearHandler += new CollectionClearHandler(_sources_OnClearHandler);
            }
        }

        private void _sources_OnInsertHandler(int index, object value)
        {
            Interop.IHTMLElement selectElement = (Interop.IHTMLElement)base.GetBaseElement();
            Interop.IHTMLDOMNode selNode = (Interop.IHTMLDOMNode)selectElement;
            selNode.appendChild((Interop.IHTMLDOMNode)((IElement)value).GetBaseElement());
        }

        private void _sources_OnClearHandler()
        {
            // we do not clear the collection here to protect the underlying conjunction between collection and element
            Interop.IHTMLElement selectElement = (Interop.IHTMLElement)base.GetBaseElement();
            Interop.IHTMLDOMNode selNode = (Interop.IHTMLDOMNode)selectElement;
            if (selNode.hasChildNodes())
            {
                Interop.IHTMLElementCollection options = (Interop.IHTMLElementCollection)selectElement.GetChildren();
                for (int o = 0; o < options.GetLength(); o++)
                {
                    Interop.IHTMLDOMNode childElement = (Interop.IHTMLDOMNode)options.Item(0, 0);
                    // remove first node, because the collection will change immediately after removeChild call
                    selNode.removeChild(childElement);
                }
            }
        }

        /// <summary>
        /// SRC tells where to get the audio source that should be put on the page.
        /// </summary>
        /// <remarks>
        /// SRC is the one required attribute. It is recommended to use relative paths. If a filename is given the property will recognize and set
        /// the relative path automatically.
        /// </remarks>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string src
        {
            set
            {
                this.SetStringAttribute("src", this.GetRelativeUrl(value));
                return;
            }
            get
            {
                return this.GetRelativeUrl(this.GetStringAttribute("src"));
            }
        }

        /// <summary>
        /// Specifies that playback controls should be displayed.
        /// </summary>
        [DefaultValueAttribute("")]
        [DescriptionAttribute("Specifies that playback controls should be displayed.")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string controls
        {
            set
            {
                this.SetStringAttribute("controls", value);
                return;
            }
            get
            {
                return this.GetStringAttribute("controls");
            }
        }

        /// <summary>
        /// Specifies that the audio should start over again, when it is finished.
        /// </summary>
        [DefaultValueAttribute("")]
        [DescriptionAttribute("Specifies that the audio should start over again, when it is finished.")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string loop
        {
            set
            {
                this.SetStringAttribute("loop", value);
                return;
            }
            get
            {
                return this.GetStringAttribute("loop");
            }
        }

        /// <summary>
        /// Specifies that the audio should start playing as soon as it is ready
        /// </summary>
        [DefaultValueAttribute("")]
        [DescriptionAttribute("Specifies that the audio should start playing as soon as it is ready.")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]
        public string autoplay
        {
            set
            {
                this.SetStringAttribute("autoplay", value);
                return;
            }
            get
            {
                return this.GetStringAttribute("autoplay");
            }
        }

        /// <summary>
        /// Specifies whether or not the audio should be loaded when the page loads.
        /// </summary>
        [DefaultValueAttribute("")]
        [DescriptionAttribute("Specifies whether or not the audio should be loaded when the page loads.")]
        [DisplayName()]
        public Preload preload
        {
            set
            {
                this.SetEnumAttribute("preload", value, Preload.None);
                return;
            }
            get
            {
                return (Preload) this.GetEnumAttribute("preload", Preload.None);
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
        public AudioElement(IHtmlEditor editor)
            : base("audio", editor)
        {
        }

        /// <summary>
        /// Supports VIDEO inheritance only.
        /// </summary>
        /// <param name="tagname"></param>
        /// <param name="editor"></param>
        protected AudioElement(string tagname, IHtmlEditor editor)
            : base(tagname, editor)
        {
            if (!tagname.Equals("video")) throw new System.ArgumentException();
        }

        internal AudioElement(Interop.IHTMLElement peer, HtmlEditor editor)
            : base(peer, editor)
        {
        }
    }
}
