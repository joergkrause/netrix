using System;
using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The OL element defines an ordered list.
    /// </summary>                             
    /// <remarks>
    /// The element contains one or more LI elements that define the actual items of the list.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.ListItemElement">ListItemElement (LI)</seealso>
    /// </remarks>
    public sealed class OrderedListElement : StyledElement
    {

        /// <summary>
        /// Defines the TYPE of list. Deprecated.
        /// </summary>
        /// <remarks>
        /// The TYPE attribute of OL suggests the numbering style on visual browsers. The case-sensitive values are as follows:
        /// <list type="bullet">
        /// <item>1 (decimal numbers: 1, 2, 3, 4, 5, ...) </item>
        /// <item>a (lowercase alphabetic: a, b, c, d, e, ...) </item>
        /// <item>A (uppercase alphabetic: A, B, C, D, E, ...) </item>
        /// <item>i (lowercase Roman numerals: i, ii, iii, iv, v, ...) </item>
        /// <item>I (uppercase Roman numerals: I, II, III, IV, V, ...) </item>
        /// </list>
        /// The numbering style on an individual list item can be suggested using the TYPE attribute of LI. 
        /// <para>
        /// The list-style-type property of CSS provides greater flexibility in suggesting numbering styles.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.ListItemElement">ListItemElement (LI)</seealso>
        /// </remarks>
        [DefaultValue(OrderedListType.Default)]
        [DescriptionAttribute("type_Ol")]
        [CategoryAttribute("Element Layout")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayNameAttribute("type_Ol")]    
        public OrderedListType type
        {
            get
            {
                string str;

                if ((str = base.GetStringAttribute("type")) != null)
                {
                    str = String.IsInterned(str);
                    if (str == "1")
                    {
                        return OrderedListType.Numeric;
                    }
                    if (str == "a")
                    {
                        return OrderedListType.LowerCaseAlphabetic;
                    }
                    if (str == "A")
                    {
                        return OrderedListType.UpperCaseAlphabetic;
                    }
                    if (str == "i")
                    {
                        return OrderedListType.LowerCaseRoman;
                    }
                    if (str == "I")
                    {
                        return OrderedListType.UpperCaseRoman;
                    }
                }
                return OrderedListType.Default;
            }

            set
            {
                string str = String.Empty;
                switch (value)
                {
                case OrderedListType.Numeric:
                    str = "1";
                    break;

                case OrderedListType.LowerCaseAlphabetic:
                    str = "a";
                    break;

                case OrderedListType.UpperCaseAlphabetic:
                    str = "A";
                    break;

                case OrderedListType.LowerCaseRoman:
                    str = "i";
                    break;

                case OrderedListType.UpperCaseRoman:
                    str = "I";
                    break;
                }
                if (str.Length == 0)
                {
                    base.RemoveAttribute("type");
                    return;
                }
                base.SetAttribute("type", str);
            }
        }

        /// <summary>
        /// List of Items this OL element contains.
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Browsable(true)]
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
        public OrderedListElement(IHtmlEditor editor)
            : base("ol", editor)
        {
        }

        internal OrderedListElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
