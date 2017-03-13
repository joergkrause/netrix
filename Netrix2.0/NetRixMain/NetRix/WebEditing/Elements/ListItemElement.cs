using System;
using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// The LI element defines a list item. 
    /// </summary>
    /// <remarks>
    /// The element must be contained within DIR, MENU, OL or UL.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.OrderedListElement">OrderedListElement (OL)</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.UnorderedListElement">UnorderedListElement (UL)</seealso>
    /// </remarks>
    public sealed class ListItemElement : StyledElement
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
        /// <item>disc (a filled-in circle) </item>
        /// <item>square (a square outline) </item>
        /// <item>circle (a circle outline) </item>
        /// </list>
        /// The numbering style on an individual list item can be suggested using the TYPE attribute of LI. 
        /// <para>
        /// The list-style-type property of CSS provides greater flexibility in suggesting numbering styles.
        /// </para>
        /// </remarks>

        [Category("Element Layout")]
        [DefaultValueAttribute(ListType.Default)]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DescriptionAttribute("type_LI")]
        [DisplayNameAttribute("type_LI")]

        public ListType type
        {
            get
            {
                string str;

                if ((str = base.GetStringAttribute("type")) != null)
                {
                    str = String.IsInterned(str);
                    if (str == "1")
                    {
                        return ListType.Numeric;
                    }
                    if (str == "a")
                    {
                        return ListType.LowerCaseAlphabetic;
                    }
                    if (str == "A")
                    {
                        return ListType.UpperCaseAlphabetic;
                    }
                    if (str == "i")
                    {
                        return ListType.LowerCaseRoman;
                    }
                    if (str == "I")
                    {
                        return ListType.UpperCaseRoman;
                    }
                    if (str == "Disk")
                    {
                        return ListType.Disk;
                    }
                    if (str == "Circle")
                    {
                        return ListType.Circle;
                    }
                    if (str == "Square")
                    {
                        return ListType.Square;
                    }
                }
                return ListType.Default;
            }

            set
            {
                string str;

                switch (value)
                {
                case ListType.Numeric:
                    str = "1";
                    break;

                case ListType.LowerCaseAlphabetic:
                    str = "a";
                    break;

                case ListType.UpperCaseAlphabetic:
                    str = "A";
                    break;

                case ListType.LowerCaseRoman:
                    str = "i";
                    break;

                case ListType.UpperCaseRoman:
                    str = "I";
                    break;

                case ListType.Disk:
                    str = "Disk";
                    break;

                case ListType.Circle:
                    str = "Circle";
                    break;

                case ListType.Square:
                    str = "Square";
                    break;

                default:
                    str = String.Empty;
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
        public ListItemElement(IHtmlEditor editor)
            : base("li", editor)
        {
        }

        internal ListItemElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
