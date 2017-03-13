using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// Table header cell. 
    /// </summary>
    /// <remarks>
    /// TH defines a column or row header that is not table data itself. If a cell acts as both header and data, 
    /// TD should be used instead. th must be used inside a TR element.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableRowElement">TableRowElement</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableCellElement">TableCellrElement</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableElement">TableElement</seealso>
    /// </remarks>
	public class TableHeaderElement : TableCellElement
	{
        
        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="PublicElementConstructor"]'/>
		public TableHeaderElement() : base("th")
		{
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
        public TableHeaderElement(IHtmlEditor editor)
            : base("th", editor)
        {
        }

		internal TableHeaderElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
		}
	}
}
