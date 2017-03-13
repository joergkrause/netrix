using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The BR element forces a break in the current line of text.
    /// </summary>
    /// <remarks>
    /// BR can be useful in formatting addresses within the ADDRESS element, but it is often misused to break lines of text in a paragraph or table cell when it looks "nice" to the author. This usually results in an awkward presentation when viewed with a font size other than that used by the author.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.ParagraphElement">ParagraphElement</seealso>
    /// </remarks>
    public sealed class BreakElement : Element
	{

        /// <summary>
        /// The CLEAR attribute of BR is used to move below floating objects.
        /// </summary>
        /// <remarks>
        /// Floating objects are typically images or tables.
        /// In the following example, the second paragraph should be rendered below the floating image:
        /// <code>
        /// &lt;P&gt;&lt;IMG SRC="toronto.jpg" ALIGN=left ALT="" TITLE="Toronto's CN Tower"&gt;Toronto is the largest city in Canada and the fourth largest in North America.&lt;/P&gt;
        /// &lt;BR CLEAR=left&gt;
        /// &lt;P&gt;The city is highly multicultural, with over 80 ethnic communities from Africa, Asia, and Europe...
        /// &lt;/Pgt;
        /// </code>
        /// Style sheets provide more flexibility in controlling text flow around objects and eliminate the need to use BR for this purpose since CSS1's clear property can be applied to any element (such as the second paragraph in the preceding example).
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.BreakClearEnum">BreakClearEnum</seealso>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.ParagraphElement">ParagraphElement</seealso>
        /// </remarks>

		[Description("")]
		[DefaultValueAttribute(BreakClearEnum.None)]
		[CategoryAttribute("Element Behavior")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayNameAttribute()]

		public BreakClearEnum clear
		{
			set
			{
				this.SetEnumAttribute ("clear", value, (BreakClearEnum) 0);
				return;
			} 
      
			get
			{
				return (BreakClearEnum) this.GetEnumAttribute ("clear", (BreakClearEnum) 0);
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
        public BreakElement(IHtmlEditor editor)
            : base("br", editor)
        {
        }

		internal BreakElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
		}
	}

}
