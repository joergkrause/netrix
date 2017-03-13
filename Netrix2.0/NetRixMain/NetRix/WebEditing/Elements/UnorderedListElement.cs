using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The UL element defines an unordered list.
    /// </summary>                             
    /// <remarks>
    /// The element contains one or more LI elements that define the actual items of the list.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.ListItemElement">ListItemElement (LI)</seealso>
    /// </remarks>
    public sealed class UnorderedListElement : StyledElement
    {


        [DefaultValue(UnorderedListType.Default)]
        [CategoryAttribute("Element Layout")]
        [DescriptionAttribute("type_UL")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayNameAttribute("type_UL")]    

        public UnorderedListType type
        {
            get
            {
                return (UnorderedListType)base.GetEnumAttribute("type", UnorderedListType.Default);
            }

            set
            {
                base.SetEnumAttribute("type", value, UnorderedListType.Default);
            }
        }

        /// <summary>
        /// List of Items this UL element contains.
        /// </summary>
        public ListItemElement[] ListItems
        {
            get
            {
                ElementCollection ec = base.GetChildren();
                if (ec == null) return null;
                ElementCollection li = new ElementCollection();
                foreach (IElement el in ec)
                {
                    if (el is ListItemElement) li.Add(el);
                }
                ListItemElement[] array = new ListItemElement[li.Count];
                li.CopyTo(array);
                return array;
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
        public UnorderedListElement(IHtmlEditor editor)
            : base("ul", editor)
        {
        }

        internal UnorderedListElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
