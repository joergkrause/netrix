using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.WebEditing.Behaviors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The MAP element defines an image map containing several areas.
    /// </summary>
    public sealed class MapElement : Element
    {

        /// <summary>
        /// Stores the actual area element this map is build from.
        /// </summary>
        private AreaElementsCollection _areaElements;

        private void FillAreaCollection()
        {
            Interop.IHTMLMapElement mapNode = (Interop.IHTMLMapElement) base.GetBaseElement();
            Interop.IHTMLAreasCollection areas = mapNode.areas;
            if (areas.length > 0)
            {
                _areaElements.Clear();
                for (int i = 0; i < areas.length; i++)
                {
                    Interop.IHTMLElement area = (Interop.IHTMLElement) areas.item(i, i);
                    AreaElement o = HtmlEditor.GenericElementFactory.CreateElement(area) as AreaElement;
                    _areaElements.Add(o);
                }
            }
        }

        /// <summary>
        /// Collection of AREA tags.
        /// </summary>
        /// <remarks>
        /// This collection should be used to add or remove 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.AreaElement">AreaElement</see> objects.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.AreaElement"/>
        /// </remarks>

        [Category(""), Browsable(true),
        Description("Edit the Area elements this Map is build from."),
        EditorAttribute(
            typeof(AreaCollectionEditor),
            typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public AreaElementsCollection AreaTags
        {
            get
            {
                // stop handler to prevent firing events during filling the target collection 
                _areaElements.OnClearHandler -= new CollectionClearHandler(_areaElements_OnClearHandler);
                _areaElements.OnInsertHandler -= new CollectionInsertHandler(_areaElements_OnInsertHandler);
                FillAreaCollection();
                _areaElements.OnClearHandler += new CollectionClearHandler(_areaElements_OnClearHandler);
                _areaElements.OnInsertHandler += new CollectionInsertHandler(_areaElements_OnInsertHandler);
                return _areaElements;
            }
        }

        /// <summary>
        /// Name of this map.
        /// </summary>
        /// <remarks>
        /// MAP creates the overall image map element. &lt;MAP ...&gt;> requires the 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.MapElement.name">name</see> attribute.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.MapElement.name"/>
        /// </remarks>

        [DescriptionAttribute("")]
        [CategoryAttribute("Element Behavior")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string name
        {
            set
            {
                this.SetStringAttribute("name", value);
                return;
            } 
      
            get
            {
                return this.GetStringAttribute("name");
            } 
      
        }

        private void _areaElements_OnClearHandler()
        {
            Interop.IHTMLMapElement mapNode = (Interop.IHTMLMapElement) base.GetBaseElement();
            Interop.IHTMLAreasCollection areas = mapNode.areas;
            while (areas.length > 0)
            {
                areas.remove(0);
            }
        }

        private void _areaElements_OnInsertHandler(int index, object value)
        {
            Interop.IHTMLElement area = ((AreaElement) value).GetBaseElement();
            Interop.IHTMLMapElement mapNode = (Interop.IHTMLMapElement) base.GetBaseElement();
            Interop.IHTMLAreasCollection areas = mapNode.areas;
            areas.add(area, 0);
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
        public MapElement(IHtmlEditor editor)
            : base("map", editor)
        {
        }

        internal MapElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
            _areaElements = new AreaElementsCollection();
            FillAreaCollection();
            MapAreaBehavior b = new MapAreaBehavior(editor);
            base.ElementBehaviors.AddBehavior(b);
        }

    }

}
