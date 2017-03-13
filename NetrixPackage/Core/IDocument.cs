using System;
using System.Drawing;
using System.Collections;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix
{

    /// <summary>
    /// HtmlDocument implements implements.</summary>
    /// <remarks>
    /// The implementing class contains base functions to deal with the whole document and most of the insert methods,
    /// creating and wrapping elements and some methods to check whether or not an element can be inserted.
    /// <seealso cref="IHtmlEditor"/>
    /// </remarks>    
    public interface IDocument 
    {

        /// <summary>
        /// Indicates if a button can be inserted.
        /// </summary>
        bool CanInsertButton { get; }        

        /// <summary>
        /// Indicates if a listbox can be inserted.
        /// </summary>
        bool CanInsertListBox { get; }

        /// <summary>
        /// Indicates if HTML can be inserted.
        /// </summary>
        bool CanInsertHtml  { get; }

        /// <summary>                                
        /// Indicates if an hyperlink can be inserted.
        /// </summary>
        bool CanInsertHyperlink { get; }

        /// <summary>
        /// Indicates if a radio button can be inserted.
        /// </summary>
        bool CanInsertRadioButton  { get; }

        /// <summary>
        /// Indicates if a text area can be inserted.
        /// </summary>
        bool CanInsertTextArea  { get; }
        /// <summary>
        /// Indicates if a textbox can be inserted.
        /// </summary>
        /// 
        bool CanInsertTextBox  { get; }

        /// <summary>
        /// Returns the active (last selected) element.
        /// </summary>
        IElement ActiveElement  { get; }

        /// <summary>
        /// Set the active element and make it the current selection. 
        /// </summary>
        /// <remarks>Does nothing if element not exists.</remarks>
        /// <param name="el">The element that will set as active element.</param>
        void SetActiveElement(IElement el);

        /// <summary>
        /// Set the active element and make it the current selection.
        /// </summary>
        /// <remarks>
        /// Does nothing if element not exists.
        /// If the second parameter is true the element becomes a viewable selection and shows handles.
        /// </remarks>
        /// <param name="el">The element which should set</param>
        /// <param name="SelectElement">Activate the handles around the element</param>
        void SetActiveElement(IElement el, bool SelectElement);

        /// <summary>
        /// Synchronizes the current element with the selection. Normally for internal use only.
        /// </summary>
        void SetActiveElement();

        /// <summary>
        /// Sets a list of colors for the customized color tab of color picker control. The host 
        /// application may define a way to choose various specific colors which should used during
        /// design time. 
        /// </summary>
		ArrayList CustomColors { get; set; }

        /// <summary>
        /// Inserts a comment with the given text.
        /// </summary>
        /// <param name="innerText"></param>
        void InsertComment(string innerText);

        /// <summary>
        /// Inserts the specified string into the html over the current selection.
        /// </summary>
        /// <param name="html"></param>
        void InsertHtml(string html);

        /// <summary>
        /// Inserts a FORM tag which acts as a container for form elements.
        /// </summary>
        /// <returns></returns>
        IElement InsertForm();

        /// <summary>
        /// Inserts a button.
        /// </summary>
        IElement InsertInputButton();

		/// <summary>
        /// Inserts a password field.
        /// </summary>
        IElement InsertInputPassword();

		/// <summary>
        /// Inserts a hidden field, in Designmode displayed as textfield
        /// </summary>
        IElement InsertInputHidden();

		/// <summary>
        /// Inserts a list box.
        /// </summary>
        IElement InsertInputListBox();

        /// <summary>
        /// Inserts a radio button.
        /// </summary>
        IElement InsertInputRadioButton();

        /// <summary>
        /// Inserts a checkbox.
        /// </summary>
        IElement InsertInputCheckbox();

        /// <summary>
        /// Inserts a text area.
        /// </summary>
        IElement InsertInputTextArea();

        /// <summary>
        /// Inserts a text box.
        /// </summary>
        IElement InsertInputTextBox();
 
        /// <summary>
        /// Inserts a dropdownlist.
        /// </summary>
        IElement InsertInputDropdownList();

		/// <summary>
        /// Inserts a list box.
        /// </summary>
        IElement InsertInputFileUpload();

        /// <summary>
        /// Creates Horizontal Rule (line), &lt;hr&gt; tag.
        /// </summary>
        IElement InsertHorizontalRule();

        /// <summary>
        /// Inserts a image button that acts as submit button.
        /// </summary>
        IElement InsertInputImage();

        /// <summary>
        /// Inserts a submit button.
        /// </summary>
        IElement InsertInputSubmit();

        /// <summary>
        /// Inserts a reset button.
        /// </summary>
        IElement InsertInputReset();

        /// <summary>
        /// Creates line break, &lt;br&gt; tag.
        /// </summary>
        IElement InsertBreak();

		/// <summary>
        /// Creates line break, &lt;br clear="xxx"&gt; tag.
        /// </summary>
        IElement InsertBreakWithClear(GuruComponents.Netrix.WebEditing.Elements.BreakClearEnum breakClear);

        /// <summary>
        /// Inserts a &lt;script&gt; tag to activate javascript or something like that.
        /// </summary>
        /// <returns></returns>
        IElement InsertScript();

        /// <summary>
        /// Inserts an embedded frame (iframe).
        /// </summary>
        IElement InsertIFrame();

        /// <summary>
        /// Inserts an EMBED element.
        /// </summary>
        /// <returns></returns>
        IElement InsertEmbed();

        /// <summary>
        /// Insert a new table at caret position if possible. Fill string defaults to an empty string. 
        /// Border width defaults to 0.
        /// </summary>
        /// <param name="rows">Number of Rows</param>
        /// <param name="cols">Number of Columns</param>
        /// <returns></returns>
        IElement InsertTable(int rows, int cols);

        /// <summary>
        /// Insert a new table at caret position if possible. Provide all possible parameters except border width.
        /// </summary>
        /// <param name="rows">Number of Rows</param>
        /// <param name="cols">Number of Columns</param>
        /// <param name="preFill">String which prefills the cells, e.g. &amp;nbsp;</param>
        /// <returns></returns>
        IElement InsertTable(int rows, int cols, string preFill);

        /// <summary>
        /// Insert a new table at caret position if possible. Provide all possible parameters.
        /// </summary>
        /// <param name="rows">Number of Rows</param>
        /// <param name="cols">Number of Columns</param>
        /// <param name="preFill">String which prefills the cells, e.g. &amp;nbsp;</param>
        /// <param name="borderWidth">Width of borders</param>
        IElement InsertTable(int rows, int cols, string preFill, int borderWidth);

        /// <summary>
        /// Insert an empty image tag (&lt;img&gt;).
        /// </summary>
        /// <returns></returns>
        IElement InsertImage();

        /// <summary>
        /// Insert (&lt;dl&gt;) tag
        /// </summary>
        IElement InsertDefinitionList();
 
        /// <summary>
        /// Insert (&lt;dd&gt;) tag
        /// </summary>
        IElement InsertDefinitionEntry();
	
        /// <summary>
        /// Insert (&lt;dt&gt;) tag
        /// </summary>
        IElement InsertDefinitionTerm();
    
        /// <summary>
        /// Inserts a ordered list. Does not create the first LI.
        /// </summary>
        IElement InsertOrderedList(); 

        /// <summary>
        /// Inserts a unordered (bulleted) list
        /// </summary>
        IElement InsertUnOrderedList();

        /// <summary>
        /// Inserts a list element
        /// </summary>
        IElement InsertListElement();
 
        /// <summary>
        /// Inserts a hyperlink with the specified URL and description
        /// </summary>
        IElement InsertHyperlink();

        /// <summary>
        /// Inserts a bookmark, e.g. a anchor tag with name attribute.
        /// </summary>
        /// <param name="name">The name of the bookmark</param>
        IElement InsertBookmark(string name);

        /// <summary>
        /// Insert (&lt;pre&gt;) tag
        /// </summary>
        IElement InsertPreformatted();

        /// <summary>
        /// Insert (&lt;address&gt;) tag
        /// </summary>
        IElement InsertAddress();

        /// <summary>
        /// Insert (&lt;acronym&gt;) tag
        /// </summary>
        IElement InsertAcronym();

        /// <summary>
        /// Insert (&lt;bdo&gt;) tag
        /// </summary>
        IElement InsertBidirectionalOverride();

        /// <summary>
        /// Insert (&lt;blockquote&gt;) tag
        /// </summary>
        IElement InsertBlockquote();

        /// <summary>
        /// Insert (&lt;big&gt;) tag
        /// </summary>
        IElement InsertBig();

        /// <summary>
        /// Insert (&lt;bgsound&gt;) tag
        /// </summary>
        IElement InsertBgsound();

        /// <summary>
        /// Insert (&lt;center&gt;) tag
        /// </summary>
        IElement InsertCenter();

        /// <summary>
        /// Insert (&lt;cite&gt;) tag
        /// </summary>
        IElement InsertCite();

        /// <summary>
        /// Insert (&lt;code&gt;) tag
        /// </summary>
        IElement InsertCode();

        /// <summary>
        /// Insert (&lt;del&gt;) tag
        /// </summary>
        IElement InsertDel();

        /// <summary>
        /// Insert (&lt;dfn&gt;) tag
        /// </summary>
        IElement InsertDefinition();

        /// <summary>
        /// Insert (&lt;ins&gt;) tag
        /// </summary>
        IElement InsertIns();

        /// <summary>
        /// Insert (&lt;kbd&gt;) tag
        /// </summary>
        IElement InsertKeyboard();

        /// <summary>
        /// Insert (&lt;em&gt;) tag
        /// </summary>
        IElement InsertEmphasis();

        /// <summary>
        /// Insert (&lt;tt&gt;) tag
        /// </summary>
        IElement InsertTeletype();

        /// <summary>
        /// Insert (&lt;var&gt;) tag
        /// </summary>
        IElement InsertVariable();

        /// <summary>
        /// Create a ordered list without any elements
        /// </summary>
        /// <returns></returns>
        IElement CreateOrderedList();

        /// <summary>
        /// Create an unordered list without any elements
        /// </summary>
        /// <returns></returns>
        IElement CreateUnorderedList();
            
        /// <summary>
        /// Creates &amp;nbsp; entity.
        /// </summary>
        IElement InsertNonBreakableSpace();

        /// <summary>
        /// Insert HTML at caret position and delete any selected text, if the DeleteSelection parameter is true.
        /// </summary>
        /// <param name="Html">HTML text to be inserted.</param>
        /// <param name="DeleteSelection">True, if the current selection should be deleted.</param>
        void InsertHtml(string Html, bool DeleteSelection);

        /// <summary>
        /// Indicates if the current selection can be wrapped in HTML tags.
        /// </summary>
        bool CanWrapSelection  { get; }

        /// <summary>
        /// Creates a new tag around the current HTML selection.</summary><remarks>This method does not check
        /// for the validity of the new HTML. If the generated HTML is not valid the call causes
        /// unexpected results.
        /// </remarks>
        /// <param name="tag">The name of the tag which should wrap the selection.</param>
        bool WrapSelection(string tag);

        /// <summary>
        /// Creates a new tag around the current HTML selection.</summary><remarks>This method does not check
        /// for the validity of the new HTML. If the generated HTML is not valid the call causes
        /// unexpected results. Additonally the given attributes are created.
        /// </remarks>
        /// <param name="tag">The name of the tag which should wrap the selection.</param>
        /// <param name="attributes">Dictionary of string key/value pairs for attribute creation</param>
        bool WrapSelection(string tag, IDictionary attributes);

        /// <summary>
        /// Takes the selection and put an &lt;div&gt; around it
        /// </summary>
        bool WrapSelectionInDiv(); 

        /// <summary>
        /// Takes the selection and put an &lt;span&gt; around it
        /// </summary>
        bool WrapSelectionInSpan();

        /// <summary>
        /// Takes the selection and put an &lt;blockquote&gt; around it
        /// </summary>
        bool WrapSelectionInBlockQuote();

        /// <summary>
        /// Takes the selection and put an &lt;hyperlink&gt; around it
        /// </summary>
        void WrapSelectionInHyperlink(string url);

        /// <summary>
		/// Remove a hyperlink where the caret is placed in.
		/// </summary>
		/// <remarks>
		/// The text which the hyperlink contains will not be removed.
		/// </remarks>
		void RemoveHyperlink();

		/// <summary>
		/// Indicates if the current selection can have it's hyperlink removed.
		/// </summary>
		/// <remarks>
		/// Use this property to update the user interface.
		/// </remarks>
		bool CanRemoveHyperlink { get ; }

        /// <summary>
        /// Gets or set the editor window zoom level and persist it using body's zoom style attribute.
        /// </summary>
        /// <remarks>
        /// This setting persist's by setting bod element's zoom style. 
        /// In design mode, this zooms the scrollbars, too. In browse mode, the behavior of the scrollbars
        /// depends on the current DOCTYPE settings. To get desired behavior (content zoomed and scrollbars
        /// not zoomed, the following DOCTYPE setting is recommended:
        /// <code>
        /// &lt;!doctype html public "-//W3C//DTD HTML 4.01 Strict//EN"&gt;
        /// </code>
        /// Insert this line as first line in each document to get desired zoom behavior.
        /// </remarks>
        decimal Zoom
        {
            set;
            get;
        }
    }
}