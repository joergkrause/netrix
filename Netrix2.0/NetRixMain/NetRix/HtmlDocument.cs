using System;
using System.Collections;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.Exceptions;
using GuruComponents.Netrix.WebEditing.UndoRedo;
using System.Xml;

namespace GuruComponents.Netrix
{

    /// <summary>
    /// HtmlDocument contains base functions to deal with the whole document.</summary>
    /// <remarks>
    /// The class contains the most of the insert methods,
    /// creating and wrapping elements and some methods to check whether or not an element can be inserted.
    /// This class cannot be instantiated directly. You must use the 
    /// <see cref="GuruComponents.Netrix.HtmlEditor.Document">Document</see> property instead to return
    /// a singleton instance of the class. The HtmlEditor component can only have one document class
    /// at the time.
    /// </remarks>
    public class HtmlDocument : IDocument, IDisposable
    {

        private HtmlEditor htmlEditor;
        private IElement _activeElement;
        # region FormattingHelpers
        private static GuruComponents.Netrix.HtmlFormatting.HtmlFormatter hf = new HtmlFormatting.HtmlFormatter();
        private static GuruComponents.Netrix.HtmlFormatting.HtmlFormatterOptions ho = new HtmlFormatting.HtmlFormatterOptions();
        private static System.Text.StringBuilder sb = new System.Text.StringBuilder();
        private static System.IO.StringWriter sw;
        # endregion

        internal HtmlDocument(IHtmlEditor editor)
        {
            htmlEditor = (HtmlEditor)editor;
        }

        #region Test functions

        /// <summary>
        /// Indicates if a button can be inserted.</summary>
        /// <remarks>It is recommended to use this property to setup toolbars
        /// or activate/deactivate menus according to the current state of the caret.
        /// </remarks>
        public bool CanInsertButton
        {
            get
            {
                return htmlEditor.IsCommandEnabled(Interop.IDM.BUTTON);
            }
        }

        /// <summary>
        /// Indicates if a listbox can be inserted. 
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to setup toolbars
        /// or activate/deactivate menus according to the current state of the caret.
        /// </remarks>
        public bool CanInsertListBox
        {
            get
            {
                return htmlEditor.IsCommandEnabled(Interop.IDM.LISTBOX);
            }
        }

        /// <summary>
        /// Indicates if HTML can be inserted.
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to setup toolbars
        /// or activate/deactivate menus according to the current state of the caret.
        /// </remarks>
        public bool CanInsertHtml
        {
            get
            {
                if (Selection.SelectionType == HtmlSelectionType.ElementSelection)
                {
                    //If this is a control range, we can only insert HTML if we're in a div or span
                    Interop.IHTMLControlRange controlRange = (Interop.IHTMLControlRange)Selection.MSHTMLSelection;
                    int selectedItemCount = controlRange.length;
                    if (selectedItemCount == 1)
                    {
                        Interop.IHTMLElement element = controlRange.item(0);
                        if ((String.Compare(element.GetTagName(), "div", true) == 0) ||
                            (String.Compare(element.GetTagName(), "td", true) == 0))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    //If this is a text range, we can definitely insert HTML
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Indicates if an hyperlink can be inserted.
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to setup toolbars
        /// or activate/deactivate menus according to the current state of the caret.
        /// </remarks>
        public bool CanInsertHyperlink
        {
            get
            {
                if (((Selection.SelectionType == HtmlSelectionType.TextSelection) || (Selection.SelectionType == HtmlSelectionType.Empty)) &&
                    (Selection.Length == 0))
                {
                    return CanInsertHtml;
                }
                else
                {
                    return htmlEditor.IsCommandEnabled(Interop.IDM.HYPERLINK);
                }
            }
        }

        /// <summary>
        /// Indicates if a radio button can be inserted.
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to setup toolbars
        /// or activate/deactivate menus according to the current state of the caret.
        /// </remarks>  
        public bool CanInsertRadioButton
        {
            get
            {

                return htmlEditor.IsCommandEnabled(Interop.IDM.RADIOBUTTON);
            }
        }

        /// <summary>
        /// Indicates if a text area can be inserted.
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to setup toolbars
        /// or activate/deactivate menus according to the current state of the caret.
        /// </remarks>
        public bool CanInsertTextArea
        {
            get
            {
                return htmlEditor.IsCommandEnabled(Interop.IDM.TEXTAREA);
            }
        }

        /// <summary>
        /// Indicates if a textbox can be inserted.
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to setup toolbars
        /// or activate/deactivate menus according to the current state of the caret.
        /// </remarks>
        public bool CanInsertTextBox
        {
            get
            {
                return htmlEditor.IsCommandEnabled(Interop.IDM.TEXTBOX);
            }
        }

        /// <summary>
        /// The current selection in the editor.
        /// </summary>
        protected HtmlSelection Selection
        {
            get
            {
                return (HtmlSelection)htmlEditor.Selection;
            }
        }

        /// <summary>
        /// Returns the currently active element.</summary>
        /// <remarks>
        /// This is the selected element, if a selection was made. This property is only useful if
        /// the editor is in absolute positioning mode and the user is able to made element
        /// selections. It does not refer to text selections.
        /// </remarks>
        public IElement ActiveElement
        {
            get
            {
                return _activeElement;
            }
        }

        #endregion

        /// <summary>
        /// Removes mouse capture from the object in the current document.
        /// </summary>
        /// <remarks>
        /// For ReleaseCapture to have an effect, you must set mouse capture through the 
        /// <see cref="IExtendedProperties.SetCapture">SetCapture</see> method
        /// to any of the elements in the document one can capture.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.IElement">IElement</seealso>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.Element">Element</seealso>
        /// <seealso cref="IExtendedProperties.ReleaseCapture">ReleaseCapture</seealso>
        /// </remarks>
        public void ReleaseCapture()
        {
            ((Interop.IHTMLElement2)htmlEditor.GetActiveDocument(false)).ReleaseCapture();
        }

        /// <overloads>Force setting active element. There are three overloads.</overloads>
        /// <summary>
        /// Set the active element and make it the current selection.
        /// </summary>
        /// <remarks> Does nothing if element not exists.</remarks>
        /// <param name="el">The element which should set</param>
        public void SetActiveElement(IElement el)
        {
            SetActiveElement(el, false);
        }

        /// <summary>
        /// Set the active element and make it the current selection.
        /// </summary>
        /// <remarks>
        /// Does nothing if element not exists.
        /// If the second parameter is true the element becomes a viewable selection and shows handles.
        /// </remarks>
        /// <param name="el">The element which should set</param>
        /// <param name="SelectElement">Activate the handles around the element</param>
        public void SetActiveElement(IElement el, bool SelectElement)
        {
            _activeElement = el;
            if (SelectElement)
            {
                Interop.IHTMLElement2 el2 = (Interop.IHTMLElement2)_activeElement.GetBaseElement();
                try
                {
                    // TODO: Real selection, if possible
                    Interop.IHTMLControlRange elRange = (Interop.IHTMLControlRange)el2.CreateControlRange();
                    if (elRange != null)
                    {
                        elRange.select();
                    }
                }
                catch
                {
                    //As always: ignore failures
                    //throw new GuruComponents.Netrix.WebEditing.Exceptions.SelectionNotPossibleException(el.GetType().ToString(), "Cannot select this type of element");
                }
            }
            Selection.SynchronizeSelection();
        }

        /// <summary>
        /// Synchronizes the current element with the selection.
        /// </summary>
        /// <remarks>
        /// Normally for internal use only. This method supports the NetRix infrastructure only. 
        /// It is public to allow developers to create and attach plug-ins.
        /// </remarks>
        public void SetActiveElement()
        {
            Interop.IMarkupServices ims = (Interop.IMarkupServices)this.htmlEditor.GetActiveDocument(false);
            Interop.IDisplayServices ds = (Interop.IDisplayServices)this.htmlEditor.GetActiveDocument(false);
            Interop.IHTMLCaret cr;
            ds.GetCaret(out cr);
            Interop.IMarkupPointer mp;
            ims.CreateMarkupPointer(out mp);
            cr.MoveMarkupPointerToCaret(mp);
            Interop.IHTMLElement el;
            mp.CurrentScope(out el);
            _activeElement = htmlEditor.GenericElementFactory.CreateElement(el) as IElement;
        }

        /// <summary>
        /// Sets a list of colors for the customized color tab of color picker control.
        /// </summary>
        /// <remarks>
        /// The host application may define a way to choose various specific colors which should used during
        /// design time. 
        /// </remarks>
        public ArrayList CustomColors
        {
            get
            {
                return Element.CustomColors;
            }
            set
            {
                Element.CustomColors = value;
            }
        }

        internal IElement InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID tagID, bool WithHtml)
        {
            return InsertWithOptionalSelection(tagID, WithHtml, false);
        }

        internal IElement InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID tagID, bool WithHtml, bool keepOuterElement)
        {
            IUndoStack wrapUnit = this.htmlEditor.InternalUndoStack("Insert Selection");
            IElement ielement = null;
            try
            {
                string selection = WithHtml ? Selection.Html : Selection.Text;
                Interop.IHTMLDocument2 doc = htmlEditor.GetActiveDocument(false);
                Interop.IMarkupServices ms = (Interop.IMarkupServices)doc;
                Interop.IHTMLElement el = null;
                if ((Selection.SelectionType & HtmlSelectionType.TextSelection) == HtmlSelectionType.TextSelection && Selection.Length > 0)
                {
                    // Get a new selection object
                    Interop.IHTMLSelectionObject selectionObj = htmlEditor.GetActiveDocument(true).GetSelection();
                    Interop.IHTMLTxtRange tr = (Interop.IHTMLTxtRange)selectionObj.CreateRange();
                    // move range into new element
                    Interop.IMarkupPointer m1, m2;
                    ms.CreateMarkupPointer(out m1);
                    ms.CreateMarkupPointer(out m2);
                    // outer selection might has same element already
                    if (keepOuterElement)
                    {
                        Interop.IHTMLElement master = tr.ParentElement();
                        if (master != null)
                        {
                            // we need to normalize to have it more robust against line breaks etc.
                            string outer = master.GetOuterHTML();
                            ClearHtmlFragment(ref outer);
                            string former = tr.GetHtmlText();
                            ClearHtmlFragment(ref former);
                            if (outer.Equals(former))
                            {
                                el = master;
                            }
                        }
                    }
                    if (el == null)
                    {
                        ms.CreateElement(tagID, "", out el);
                        ms.MovePointersToRange(tr, m1, m2);
                        ms.InsertElement(el, m1, m2);
                    }
                    // set the old selection into the elements container, if it fails, paste html directly
                    ClearHtmlFragment(ref selection);
                    string newOuter = el.GetOuterHTML();
                    ClearHtmlFragment(ref newOuter);
                    if (!selection.Equals(newOuter))
                    {
                        try
                        {
                            el.SetInnerHTML(selection);
                        }
                        catch
                        {
                            tr.PasteHTML(selection);
                        }
                    }
                    // create native object
                    ielement = htmlEditor.GenericElementFactory.CreateElement(el) as IElement;
                }
                else
                {
                    Selection.SynchronizeSelection();
                    if (keepOuterElement && Selection.Element != null && Selection.Element is SpanElement)
                    {
                        el = Selection.Element.GetBaseElement();
                        ielement = htmlEditor.GenericElementFactory.CreateElement(el) as IElement;
                    }
                    else
                    {
                        // still no selection, just create element...
                        ms.CreateElement(tagID, "", out el);
                        ielement = htmlEditor.GenericElementFactory.CreateElement(el) as IElement;
                        htmlEditor.InsertElementAtCaret(ielement);
                    }
                    //... and place caret inside the element
                    Interop.IDisplayServices ds;
                    Interop.IDisplayPointer dp;
                    Interop.IMarkupPointer mp;
                    Interop.IHTMLCaret cr;

                    ds = (Interop.IDisplayServices)doc;
                    ds.CreateDisplayPointer(out dp);
                    ds.GetCaret(out cr);
                    ms.CreateMarkupPointer(out mp);

                    mp.MoveAdjacentToElement(el, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterBegin);
                    dp.MoveToMarkupPointer(mp, null);
                    cr.MoveCaretToPointer(dp, true, Interop.CARET_DIRECTION.CARET_DIRECTION_SAME);
                }
            }
            finally
            {
                wrapUnit.Close();
            }
            return ielement;
        }

        private static void ClearHtmlFragment(ref string html)
        {
            sb = new System.Text.StringBuilder();
            sw = new System.IO.StringWriter(sb);
            hf.Format(html, sw, ho);
            html = sb.ToString();
            sw.Dispose();
        }

        private IElement InsertWithOptionalSelection(Interop.IDM idm)
        {
            return InsertWithOptionalSelection(idm, null);
        }

        private IElement InsertWithOptionalSelection(Interop.IDM idm, string parameter)
        {
            IUndoStack undo = htmlEditor.GetUndoManager("Insert Element");
            IElement element = null;
            try
            {
                Interop.IHTMLSelectionObject selectionObj = htmlEditor.GetActiveDocument(false).GetSelection();
                object current = selectionObj.CreateRange();
                //  Create selection from current word
                Interop.IHTMLTxtRange baseRange = current as Interop.IHTMLTxtRange;
                if (baseRange != null && (baseRange.GetText() == null || baseRange.GetText().Equals(String.Empty)))
                {
                    baseRange.Collapse(false);
                    AnchorElement anchor;
                    Interop.IMarkupServices ims = this.htmlEditor.GetActiveDocument(false) as Interop.IMarkupServices;
                    Interop.IMarkupPointer pCursor1, pCursor2;
                    ims.CreateMarkupPointer(out pCursor1);
                    ims.CreateMarkupPointer(out pCursor2);
                    Interop.IHTMLCaret cr;
                    Interop.IDisplayServices ds = (Interop.IDisplayServices)this.htmlEditor.GetActiveDocument(false);
                    ds.GetCaret(out cr);
                    cr.MoveMarkupPointerToCaret(pCursor1);
                    cr.MoveMarkupPointerToCaret(pCursor2);
                    pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_PREVWORDEND);
                    pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDBEGIN);
                    pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDBEGIN);
                    pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDEND);
                    if (baseRange.GetText() != null && Char.IsPunctuation(baseRange.GetText()[baseRange.GetText().Length - 1]))
                    {
                        pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_PREVCHAR);
                    }
                    else
                    {
                        pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_PREVWORDEND);
                    }
                    ims.MoveRangeToPointers(pCursor1, pCursor2, baseRange);
                    // special treatment for empty selections
                    if (baseRange.GetText() != null && baseRange.GetText().Trim().Length > 0)
                    {
                        try
                        {
                            baseRange.Select();
                        }
                        catch
                        {
                            anchor = htmlEditor.CreateElementAtCaret("a") as AnchorElement;
                            if (anchor != null)
                            {
                                anchor.href = parameter;
                                anchor.InnerHtml = parameter;
                                return anchor;
                            }
                        }
                    }
                    else
                    {
                        anchor = htmlEditor.CreateElementAtCaret("a") as AnchorElement;
                        if (anchor != null)
                        {
                            anchor.href = parameter;
                            anchor.InnerHtml = parameter;
                            return anchor;
                        }
                    }
                }
                if (parameter == null)
                {
                    this.htmlEditor.Exec(idm);
                }
                else
                {
                    this.htmlEditor.Exec(idm, parameter);
                }
                switch (selectionObj.GetSelectionType())
                {
                    case "Text":
                        Interop.IHTMLTxtRange textRange = current as Interop.IHTMLTxtRange;
                        textRange.Collapse(true);
                        Interop.IHTMLElement parentElement = textRange.ParentElement();
                        if (parentElement != null)
                        {
                            element = htmlEditor.GenericElementFactory.CreateElement(parentElement) as IElement;
                        }
                        break;
                    case "None":
                        return null;
                    default:
                        Interop.IHTMLControlRange controlRange = current as Interop.IHTMLControlRange;
                        if (controlRange != null)
                        {
                            Interop.IHTMLElement currentElement = controlRange.item(0);
                            if (currentElement != null)
                            {
                                element = htmlEditor.GenericElementFactory.CreateElement(currentElement) as IElement;
                            }
                        }
                        break;
                }
            }
            finally
            {
                undo.Close();
            }
            return element;
        }

        /// <summary>
        /// Inserts the element and places it right from the current selection.
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns>The element object that represents the created element.</returns>
        private IElement InsertElement(Interop.ELEMENT_TAG_ID tagID)
        {
            IElement ielement = htmlEditor.CreateElement(Helper.GetElementName(tagID));
            htmlEditor.InsertElementAtCaret(ielement);
            return ielement;
        }

        #region Form Elements

        /// <summary>
        /// Inserts a FORM tag which acts as a container for form elements.
        /// </summary>
        /// <returns></returns>
        public IElement InsertForm()
        {
            IElement el = this.InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.FORM, true);
            return el;
        }

        /// <summary>
        /// Inserts a button. Needs JavaScript to work.
        /// </summary>
        public IElement InsertInputButton()
        {
            htmlEditor.Exec(Interop.IDM.BUTTON);
            this.SetActiveElement();
            return this.Selection.GetCurrentElement();
        }

        /// <summary>
        /// Inserts a image button that acts as submit button.
        /// </summary>
        public IElement InsertInputImage()
        {
            htmlEditor.Exec(Interop.IDM.INSINPUTIMAGE);
            this.SetActiveElement();
            InputImageElement se = this.Selection.GetCurrentElement() as InputImageElement;
            return se;
        }

        /// <summary>
        /// Inserts a submit button.
        /// </summary>
        public IElement InsertInputSubmit()
        {
            htmlEditor.Exec(Interop.IDM.INSINPUTSUBMIT);
            this.SetActiveElement();
            InputSubmitElement se = this.Selection.GetCurrentElement() as InputSubmitElement;
            return se;
        }

        /// <summary>
        /// Inserts a reset button.
        /// </summary>
        public IElement InsertInputReset()
        {
            htmlEditor.Exec(Interop.IDM.INSINPUTRESET);
            this.SetActiveElement();
            InputResetElement se = this.Selection.GetCurrentElement() as InputResetElement;
            return se;
        }

        /// <summary>
        /// Inserts a password field.
        /// </summary>
        public IElement InsertInputPassword()
        {
            htmlEditor.Exec(Interop.IDM.INSINPUTPASSWORD);
            this.SetActiveElement();
            return this.Selection.GetCurrentElement();
        }
        /// <summary>
        /// Inserts a hidden field, in Designmode displayed as text field.
        /// </summary>
        /// <remarks>
        /// To make the hidden field invisible or smaller at design time set the width and height style attributes.
        /// </remarks>
        public IElement InsertInputHidden()
        {
            htmlEditor.Exec(Interop.IDM.INSINPUTHIDDEN);
            this.SetActiveElement();
            return this.Selection.GetCurrentElement();
        }
        /// <summary>
        /// Inserts a list box.
        /// </summary>
        public IElement InsertInputListBox()
        {
            htmlEditor.Exec(Interop.IDM.LISTBOX);
            this.SetActiveElement();
            return this.Selection.GetCurrentElement();
        }

        /// <summary>
        /// Inserts a radio button.
        /// </summary>
        public IElement InsertInputRadioButton()
        {
            htmlEditor.Exec(Interop.IDM.RADIOBUTTON);
            this.SetActiveElement();
            return this.Selection.GetCurrentElement();
        }

        /// <summary>
        /// Inserts a checkbox.
        /// </summary>
        public IElement InsertInputCheckbox()
        {
            htmlEditor.Exec(Interop.IDM.CHECKBOX);
            this.SetActiveElement();
            return this.Selection.GetCurrentElement();
        }

        /// <summary>
        /// Inserts a text area.
        /// </summary>
        public IElement InsertInputTextArea()
        {
            htmlEditor.Exec(Interop.IDM.TEXTAREA);
            this.SetActiveElement();
            return this.Selection.GetCurrentElement();
        }

        /// <summary>
        /// Inserts a text box.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <example>
        /// The following code assumes that the form has a button and its click event is handled by this method:
        /// <code>
        ///using GuruComponents.Netrix.WebEditing.Elements;
        ///
        ///private void button1_Click(object sender, System.EventArgs e)
        ///{
        ///    IElement f = this.htmlEditor1.Document.InsertInputTextBox();
        ///    f.SetStyleAttribute("BORDER-STYLE", "dashed");
        ///    f.SetStyleAttribute("BORDER-WIDTH", "1px");
        ///    f.SetStyleAttribute("BORDER-COLOR", "#000000");
        ///}</code>
        /// The method inserts a textbox at the current caret position and assigns immediately various
        /// styles to format the box.
        /// </example>
        public IElement InsertInputTextBox()
        {
            htmlEditor.Exec(Interop.IDM.TEXTBOX);
            this.SetActiveElement();
            return this.Selection.GetCurrentElement();
        }

        /// <summary>
        /// Inserts a dropdownlist.
        /// </summary>
        public IElement InsertInputDropdownList()
        {
            htmlEditor.Exec(Interop.IDM.DROPDOWNBOX);
            this.SetActiveElement();
            return this.Selection.GetCurrentElement();
        }
        /// <summary>
        /// Inserts a list box.
        /// </summary>
        public IElement InsertInputFileUpload()
        {
            htmlEditor.Exec(Interop.IDM.INSINPUTUPLOAD);
            this.SetActiveElement();
            return this.Selection.GetCurrentElement();
        }

        #endregion

        #region Insert Elements do not contain any selected text

        /// <summary>
        /// Creates Horizontal Rule (line), &lt;hr&gt; tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertHorizontalRule()
        {
            IElement hr = InsertElement(Interop.ELEMENT_TAG_ID.HR);
            this.SetActiveElement();
            return hr;
        }

        /// <summary>
        /// Creates line break, &lt;br&gt; tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertBreak()
        {
            IElement br = InsertElement(Interop.ELEMENT_TAG_ID.BR);
            this.SetActiveElement();
            return br;
        }

        /// <summary>
        /// Creates a line break (&lt;br&gt;) tag and move the caret to the next line.
        /// </summary>
        /// <remarks>
        /// The purpose of this method is for replacement of default enter key behavior, which normally
        /// inserts &lt;p&gt; tags. Together with the replacement code this method is helpful to have
        /// the best caret control.
        /// </remarks>
        /// <example>
        /// This example shows how to use the KeyPressed event to look for the Enter key. Then, if Enter was
        /// pressed, the code looks if the caret is already inside a paragraph. If so, the code inserts a 
        /// special break and moves the caret immediataly to the next line. The Handled property is set to
        /// inform the control that no paragraph should inserted.
        /// <code>
        ///private void htmlEditor1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        ///{
        ///    if (e.KeyChar == (char)13)
        ///    {
        ///        if (this.htmlEditor1.Selection.Element is ParagraphElement)
        ///        {
        ///            e.Handled = true;
        ///            this.htmlEditor1.Document.InsertBreakWithCaret();
        ///        }
        ///
        ///    }
        ///}</code>
        /// </example>
        /// <returns>Returns the BR tag which was inserted.</returns>
        public IElement InsertBreakWithCaret()
        {
            IElement br = this.InsertBreak();
            this.InsertHtml("&nbsp;");
            this.htmlEditor.MoveCaretTo(Interop.DISPLAY_MOVEUNIT.DISPLAY_MOVEUNIT_CurrentLineStart);
            this.htmlEditor.Delete();
            return br;
        }

        /// <summary>
        /// Creates line break, &lt;br clear="xxx"&gt; tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertBreakWithClear(BreakClearEnum breakClear)
        {
            Interop.IHTMLElement el = htmlEditor.CreateElement("br").GetBaseElement();
            BreakElement br = (BreakElement)htmlEditor.GenericElementFactory.CreateElement(el);
            switch (breakClear)
            {
                case BreakClearEnum.Left:
                    br.clear = BreakClearEnum.Left;
                    break;
                case BreakClearEnum.Right:
                    br.clear = BreakClearEnum.Right;
                    break;
                case BreakClearEnum.All:
                    br.clear = BreakClearEnum.All;
                    break;
                case BreakClearEnum.None:
                default:
                    br.clear = BreakClearEnum.None;
                    break;
            }
            return br;
        }

        /// <summary>
        /// Inserts a &lt;script&gt; tag to activate javascript or something like that
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertScript()
        {
            return InsertElement(Interop.ELEMENT_TAG_ID.SCRIPT);
        }

        /// <summary>
        /// Inserts an embedded frame (iframe)
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertIFrame()
        {
            return InsertElement(Interop.ELEMENT_TAG_ID.IFRAME);
        }

        /// <summary>
        /// Inserts an EMBED element.
        /// </summary>
        /// <returns></returns>
        public IElement InsertEmbed()
        {
            return InsertElement(Interop.ELEMENT_TAG_ID.EMBED);
        }

        /// <summary>
        /// Insert a new table at caret position if possible. Fill string defaults to an empty string. 
        /// Border width defaults to 0.
        /// </summary>
        /// <param name="rows">Number of Rows</param>
        /// <param name="cols">Number of Columns</param>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertTable(int rows, int cols)
        {
            return InsertTable(rows, cols, String.Empty, 0);
        }

        /// <summary>
        /// Insert a new table at caret position if possible. Provide all possible parameters except border width.
        /// </summary>
        /// <param name="rows">Number of Rows</param>
        /// <param name="cols">Number of Columns</param>
        /// <param name="preFill">String which prefills the cells, e.g. &amp;nbsp;</param>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertTable(int rows, int cols, string preFill)
        {
            return InsertTable(rows, cols, preFill, 0);
        }

        /// <summary>
        /// Insert a new table at caret position if possible. Provide all possible parameters.
        /// </summary>
        /// <param name="rows">Number of Rows</param>
        /// <param name="cols">Number of Columns</param>
        /// <param name="preFill">String which prefills the cells, e.g. &amp;nbsp;</param>
        /// <param name="borderWidth">Width of borders</param>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertTable(int rows, int cols, string preFill, int borderWidth)
        {
            Interop.IHTMLTable t;
            IUndoStack unit = this.htmlEditor.InternalUndoStack("New Table");
            try
            {
                // must insert before adding any HTML content or childs!
                Interop.IHTMLElement el;
                el = this.htmlEditor.CreateElementAtCaret("table").GetBaseElement();
                t = (Interop.IHTMLTable)el; //InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.TABLE, false);
                t.cols = cols;
                t.border = borderWidth;
                for (int i = 0; i < rows; i++)
                {
                    Interop.IHTMLTableRow tr = (Interop.IHTMLTableRow)t.insertRow(-1);
                    for (int j = 0; j < cols; j++)
                    {
                        Interop.IHTMLElement c = (Interop.IHTMLElement)tr.insertCell(-1);
                        c.SetInnerHTML(preFill);
                    }
                }

            }
            catch (Exception exc)
            {
                throw new HtmlControlException("Cannot insert table here", exc);
            }
            finally
            {
                unit.Close();
            }
            // reactivate zero borders
            if (this.htmlEditor.BordersVisible)
            {
                this.htmlEditor.BordersVisible = false;
                this.htmlEditor.BordersVisible = true;
            }
            IElement e = htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement)t) as IElement;
            return e;
        }

        /// <summary>
        /// Insert an empty image tag (&lt;img&gt;).
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertImage()
        {
            return InsertElement(Interop.ELEMENT_TAG_ID.IMG);
        }

        #endregion

        # region Insert Complex Elements and there sub elements

        /// <summary>
        /// Insert (&lt;dl&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertDefinitionList()
        {
            return InsertElement(Interop.ELEMENT_TAG_ID.DL);
        }

        /// <summary>
        /// Insert (&lt;dd&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertDefinitionEntry()
        {
            return InsertElement(Interop.ELEMENT_TAG_ID.DD);
        }

        /// <summary>
        /// Insert (&lt;dt&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertDefinitionTerm()
        {
            return InsertElement(Interop.ELEMENT_TAG_ID.DT);
        }

        /// <summary>
        /// Inserts a ordered list. Does not create the first LI.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertOrderedList()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.OL, true);
        }

        /// <summary>
        /// Inserts a unordered (bulleted) list.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertUnOrderedList()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.UL, true);
        }

        /// <summary>
        /// Inserts a list element.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertListElement()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.LI, true);
        }

        # endregion

        # region Elements can contain a selection

        /// <summary>
        /// Inserts a hyperlink with the specified href attribute and the current selection in it.
        /// </summary>
        public IElement InsertHyperlink()
        {
            return InsertHyperlink(String.Empty);
        }

        /// <summary>
        /// Inserts a hyperlink around the currently selected text.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertHyperlink(string href)
        {
            return this.InsertWithOptionalSelection(Interop.IDM.HYPERLINK, href);
        }

        /// <summary>
        /// Inserts a bookmark, e.g. a anchor tag with name attribute.
        /// </summary>
        /// <param name="name">The name of the bookmark</param>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertBookmark(string name)
        {
            AnchorElement a = this.InsertElement(Interop.ELEMENT_TAG_ID.A) as AnchorElement;
            if (a != null)
            {
                a.href = String.Empty;
                a.name = name;
            }
            return a;
        }

        /// <summary>
        /// Insert (&lt;pre&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertPreformatted()
        {
            //return this.InsertWithOptionalSelection(Interop.IDM.PREFORMATTED);
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.PRE, true);
        }

        /// <summary>
        /// Insert (&lt;address&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertAddress()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.ADDRESS, true);
        }

        /// <summary>
        /// Insert (&lt;acronym&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertAcronym()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.ACRONYM, true);
        }

        /// <summary>
        /// Insert (&lt;bdo&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertBidirectionalOverride()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.BDO, true);
        }

        /// <summary>
        /// Insert (&lt;blockquote&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertBlockquote()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.BLOCKQUOTE, true);
        }

        /// <summary>
        /// Insert (&lt;big&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertBig()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.BIG, true);
        }

        /// <summary>
        /// Insert (&lt;bgsound&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertBgsound()
        {
            return InsertElement(Interop.ELEMENT_TAG_ID.BGSOUND);
        }

        /// <summary>
        /// Insert (&lt;center&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertCenter()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.CENTER, true);
        }

        /// <summary>
        /// Insert (&lt;cite&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertCite()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.CITE, true);
        }

        /// <summary>
        /// Insert (&lt;code&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertCode()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.CODE, true);
        }

        /// <summary>
        /// Insert (&lt;del&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertDel()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.DEL, true);
        }

        /// <summary>
        /// Insert (&lt;dfn&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertDefinition()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.DFN, true);
        }

        /// <summary>
        /// Insert (&lt;ins&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertIns()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.INS, true);
        }

        /// <summary>
        /// Insert (&lt;kbd&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertKeyboard()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.KBD, true);
        }

        /// <summary>
        /// Insert (&lt;em&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertEmphasis()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.EM, true);
        }

        /// <summary>              
        /// Insert (&lt;tt&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertTeletype()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.TT, true);
        }

        /// <summary>
        /// Insert (&lt;var&gt;) tag.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertVariable()
        {
            return InsertWithOptionalSelection(Interop.ELEMENT_TAG_ID.VAR, true);
        }


        # endregion

        # region Macro Elements

        /// <summary>
        /// Create an ordered list &lt;OL&gt; element and the first list element in it.
        /// </summary>
        /// <remarks>
        /// The method returns that element and the host application
        /// is responsible to set items in it.
        /// </remarks>
        /// <returns>The element object that represents the created element.</returns>
        public IElement CreateOrderedList()
        {
            htmlEditor.Exec(Interop.IDM.ORDERLIST);
            this.SetActiveElement();
            // active is "LI", so we return Parent
            return this._activeElement.GetParent() as IElement;
        }

        /// <summary>
        /// Create an unordered list &lt;UL&gt; element and the first list element in it.
        /// </summary>
        /// <remarks>
        /// The method returns that element and the host application
        /// is responsible to set items in it.
        /// </remarks>
        /// <returns>The element object that represents the created element.</returns>
        public IElement CreateUnorderedList()
        {
            htmlEditor.Exec(Interop.IDM.UNORDERLIST);
            this.SetActiveElement();
            // active is "LI", so we return Parent
            return this._activeElement.GetParent() as IElement;
        }


        # endregion

        # region Insert Entities

        /// <summary>
        /// Creates an &amp;nbsp; entity within the document at the current caret position.
        /// </summary>
        /// <returns>The element object that represents the created element.</returns>
        public IElement InsertNonBreakableSpace()
        {
            htmlEditor.Exec(Interop.IDM.NONBREAK);
            this.SetActiveElement();
            return this.Selection.GetCurrentElement();
        }

        /// <summary>
        /// Inserts a HTML comment with the given text.
        /// </summary>
        /// <remarks>
        /// It is allowed to provide an empty string. Anyway, to access the comment later
        /// it is necessary to switch glyphs on and point the caret or cursor to the comment glyph.
        /// This will return the <see cref="GuruComponents.Netrix.WebEditing.Elements.CommentElement">CommentElement</see>
        /// object, which can be used to manipulate the content.
        /// </remarks>
        /// <param name="innerText">The text which is written into the comment.</param>
        public void InsertComment(string innerText)
        {
            IUndoStack batchedUndoUnit = htmlEditor.InternalUndoStack("Insert Comment");
            try
            {
                this.InsertHtml(@"<span id=""Netrix_TCS"" style=""visibility:hidden"">TCS</span><!-- " + innerText + @" -->");
                IElement tcsSpan = this.htmlEditor.GetElementById("Netrix_TCS");
                ((Element)tcsSpan).ElementDom.RemoveElement(true);
            }
            finally
            {
                batchedUndoUnit.Close();
            }
        }

        /// <summary>
        /// Insert HTML at caret position and delete any selected text, if the DeleteSelection parameter is true.
        /// </summary>
        /// <remarks>
        /// Inserting comments using this method is not possible. One should use 
        /// <see cref="GuruComponents.Netrix.HtmlDocument.InsertComment">InsertComment</see> instead.
        /// This is due to limitations of the underlying MSHTML engine, that doesn't allow insertion
        /// of invisible elements using standard commands.
        /// <para>
        /// This method inserts the given text as HTML, but it does not transform the DOM (Document Object Model) and
        /// does not add the inserted element to the current element hierarchy. If the element was previously created as
        /// a native object, any access to <see cref="GuruComponents.Netrix.WebEditing.Elements.ElementDom">ElementDom</see>
        /// will fail, as well as attaching behaviors and other internal operations with elements.</para>
        /// <para>
        /// The main usage of this method should be the insertion of entities (&amp;ntilde; &amp;nbsp; etc.) and inline
        /// HTML tags, to which further access doesn't make sense. 
        /// For instance, in a text like "This is &lt;b&gt;not&lt;/b&gt; allowed" the further access to the Bold tag is never
        /// necessary and building the structure using the DOM will simple raise the effort without any advantage.
        /// </para>
        /// </remarks>
        /// <param name="Html">HTML text to be inserted.</param>
        /// <param name="DeleteSelection">True, if the current selection should be deleted.</param>
        public void InsertHtml(string Html, bool DeleteSelection)
        {
            if (DeleteSelection)
            {
                htmlEditor.Selection.SynchronizeSelection();
                // avoid deleting the next character if no selection is present
                if (htmlEditor.Selection.Length > 0)
                {
                    htmlEditor.Delete();
                }
            }
            InsertHtml(Html);
        }

        /// <summary>
        /// Inserts the specified string into the HTML over the current selection.
        /// </summary>
        /// <remarks>
        /// Inserting comments using this method is not possible. One should use 
        /// <see cref="GuruComponents.Netrix.HtmlDocument.InsertComment">InsertComment</see> instead.
        /// This is due to limitations of the underlying MSHTML engine, that doesn't allow insertion
        /// of invisible elements using standard commands.
        /// <para>
        /// This method inserts the given text as HTML, but it does not transform the DOM (Document Object Model) and
        /// does not add the inserted element to the current element hierarchy. If the element was previously created as
        /// a native object, any access to <see cref="GuruComponents.Netrix.WebEditing.Elements.ElementDom">ElementDom</see>
        /// will fail, as well as attaching behaviors and other internal operations with elements.</para>
        /// <para>
        /// The main usage of this method should be the insertion of entities (&amp;ntilde; &amp;nbsp; etc.) and inline
        /// HTML tags, to which further access doesn't make sense. 
        /// For instance, in a text like "This is &lt;b&gt;not&lt;/b&gt; allowed" the further access to the Bold tag is never
        /// necessary and building the structure using the DOM will simple raise the effort without any advantage.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">For inserting SCRIPT tags you must add the DEFER attribute. See http://msdn.microsoft.com/library/default.asp?url=/workshop/author/dhtml/reference/properties/innerhtml.asp for details.</exception>
        /// <param name="html"></param>
        public void InsertHtml(string html)
        {
            Selection.SynchronizeSelection();
            if (Selection.SelectionType == HtmlSelectionType.ElementSelection)
            {
                try
                {
                    //If it's a control range, we can only insert if we are in a div or td
                    Interop.IHTMLControlRange controlRange = Selection.MSHTMLSelection as Interop.IHTMLControlRange;
                    if (controlRange == null)
                    {
                        InsertHtmlInRange(html);
                    }
                    else
                    {
                        int selectedItemCount = controlRange.length;
                        if (selectedItemCount == 1)
                        {
                            Interop.IHTMLElement element = controlRange.item(0);
                            if ((String.Compare(element.GetTagName(), "div", true) == 0) ||
                                (String.Compare(element.GetTagName(), "td", true) == 0))
                            {
                                element.InsertAdjacentHTML("beforeEnd", html);
                            }
                        }
                    }
                }
                catch
                {
                    InsertHtmlInRange(html);
                }
            }
            else
            {
                InsertHtmlInRange(html);
            }
        }

        private void InsertHtmlInRange(string html)
        {
            Interop.IHTMLSelectionObject selectionObj = htmlEditor.GetActiveDocument(false).GetSelection();
            //Get the current selection from that selection object
            try
            {
                object currentSelection;
                currentSelection = selectionObj.CreateRange();
                if (currentSelection is Interop.IHTMLTxtRange)
                {
                    // TODO: fix <script> insertion issue add DEFER to script!
                    // http://msdn.microsoft.com/library/default.asp?url=/workshop/author/dhtml/reference/properties/innerhtml.asp
                    Interop.IHTMLTxtRange textRange = (Interop.IHTMLTxtRange)currentSelection;
                    if (html.StartsWith("<script") && !html.ToLower().Contains(" defer"))
                    {
                        throw new ArgumentException("For inserting SCRIPT tags you must add the DEFER attribute. See http://msdn.microsoft.com/library/default.asp?url=/workshop/author/dhtml/reference/properties/innerhtml.asp for details.", "html");
                    }
                    else
                    {
                        textRange.PasteHTML(html);
                        textRange.Collapse(true);
                        ActivateNewElement();
                    }
                }
            }
            catch
            {
            }
        }

        # endregion

        # region Remove/Delete methods

        /// <summary>
        /// Remove a hyperlink where the caret is placed in.
        /// </summary>
        /// <remarks>
        /// The text which the hyperlink contains will not be removed.
        /// </remarks>
        public void RemoveHyperlink()
        {
            htmlEditor.Exec(Interop.IDM.UNLINK);
            htmlEditor.InvokeHtmlElementChanged(htmlEditor.CurrentScopeElement, HtmlElementChangedType.Unknown);
        }

        /// <summary>
        /// Indicates if the current selection can have it's hyperlink removed.
        /// </summary>
        /// <remarks>
        /// Use this property to update the user interface.
        /// </remarks>
        public bool CanRemoveHyperlink
        {
            get
            {
                return htmlEditor.IsCommandEnabled(Interop.IDM.UNLINK);
            }
        }

        # endregion

        # region Wrap

        /// <summary>
        /// Indicates whether the current selection can be wrapped in HTML tags.
        /// </summary>
        public bool CanWrapSelection
        {
            get
            {
                return (htmlEditor.Selection.SelectionType == HtmlSelectionType.TextSelection);
            }
        }

        /// <summary>
        /// Creates a new tag around the current HTML selection.
        /// </summary>
        /// <remarks>
        /// This method does not check for the validity of the new HTML. If the generated HTML is not valid the 
        /// call causes unexpected results, including but not limited to ignoring the call.
        /// </remarks>
        /// <param name="tag">The tag name which is used to build the tag, e.g. "DIV" for a division.</param>
        public bool WrapSelection(string tag)
        {
            return WrapSelection(tag, null);
        }

        /// <summary>
        /// Creates a new tag around the current HTML selection.
        /// </summary>
        /// <remarks>
        /// The textselection method used internally is "clever" in some ways and tries to keep the HTML valid
        /// and set the new element as expected. This can result in element duplication.
        /// <para>
        /// This HTML <c>&lt;b>stro[ng&lt;/b>&lt;i>ita]lic&lt;</c>, where [] covers the selected text range, 
        /// would become <c>&lt;b>stro&lt;/b>&lt;span>&lt;b>ng&lt;/b>&lt;i>ita&lt;/span>lic&lt;</c> after using the
        /// method with the tag SPAN. As you can see, the &lt;b> tag is duplicated to keep the HTML valid.         
        /// </para>
        /// After the selection has been changed, the method removes the selection and sets the caret to the
        /// end of range.
        /// </remarks>
        /// <param name="tag">The tag name which is used to build the tag, e.g. "DIV" for a division.</param>
        /// <param name="attributes">A collection of attributes, expected as name/value pairs. Can be <c>null</c> (<c>Nothing</c> in VB.NET) if no attributes needed.</param>
        public bool WrapSelection(string tag, IDictionary attributes)
        {
            //Create a string for all the attributes
            string attributeString = String.Empty;
            if (attributes != null)
            {
                attributeString = " ";
                foreach (string key in attributes.Keys)
                {
                    attributeString += key + "=\"" + attributes[key] + "\" ";
                }
                attributeString = attributeString.TrimEnd(' ');
            }
            // Get the selection object from the MSHTML document
            Interop.IHTMLSelectionObject selectionObj = htmlEditor.GetActiveDocument(false).GetSelection();
            //Get the current selection from that selection object
            object currentSelection;
            try
            {
                currentSelection = selectionObj.CreateRange();
            }
            catch
            {
                return false;
            }

            IUndoStack wrapUnit = this.htmlEditor.InternalUndoStack("Wrap Element");
            try
            {
                switch (htmlEditor.Selection.SelectionType)
                {
                    case HtmlSelectionType.Empty:
                        break;
                    case HtmlSelectionType.TextSelection:
                        Interop.IHTMLTxtRange textRange = currentSelection as Interop.IHTMLTxtRange;
                        if (textRange != null)
                        {
                            string oldText = textRange.GetHtmlText();
                            if (oldText == null)
                            {
                                oldText = String.Empty;
                            }
                            Interop.IHTMLTxtRange rightRange = textRange.Duplicate();
                            Interop.IHTMLTxtRange leftRange = textRange.Duplicate();
                            leftRange.Collapse(true);
                            Interop.IHTMLElement leftElement = leftRange.ParentElement();
                            rightRange.Collapse(false);
                            Interop.IHTMLElement rightElement = rightRange.ParentElement();

                            string newText;
                            newText = String.Format("<{0}{1}>{2}</{0}>", tag, attributeString, oldText);

                            if (leftElement != rightElement)
                            {
                                try
                                {
                                    Interop.IHTMLElement rangeParent = textRange.ParentElement();
                                    Interop.IMarkupPointer pL, pR;
                                    Interop.IMarkupServices ims =
                                        this.htmlEditor.GetActiveDocument(false) as Interop.IMarkupServices;
                                    ims.CreateMarkupPointer(out pL);
                                    ims.CreateMarkupPointer(out pR);
                                    // Delete the selection, this does implicitly rebuild valid HTML if any tags partly removed
                                    textRange.ExecCommand("Delete", false, 1);
                                    // The pointer is now placed in the remaining tag
                                    Interop.IHTMLElement remainingElement = textRange.ParentElement();
                                    ims.MovePointersToRange(textRange, pL, pR);
                                    // Use the pointer to set the insertion point between the tags or outside the remaining tag
                                    if (remainingElement == leftElement && remainingElement != rangeParent)
                                    {
                                        pR.MoveAdjacentToElement(leftElement,
                                                                 Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterEnd);
                                        pL.MoveAdjacentToElement(leftElement,
                                                                 Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterEnd);
                                    }
                                    if (remainingElement == rightElement && remainingElement != rangeParent)
                                    {
                                        pR.MoveAdjacentToElement(rightElement,
                                                                 Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeBegin);
                                        pL.MoveAdjacentToElement(rightElement,
                                                                 Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeBegin);
                                    }
                                    if (remainingElement != leftElement && rightElement == rangeParent &&
                                        !remainingElement.GetTagName().Equals("BODY"))
                                    {
                                        pR.MoveAdjacentToElement(remainingElement,
                                                                 Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeBegin);
                                        pL.MoveAdjacentToElement(remainingElement,
                                                                 Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeBegin);
                                    }
                                    ims.MoveRangeToPointers(pL, pR, textRange);
                                }
                                catch
                                {
                                }
                            }
                            textRange.PasteHTML(newText);

                        }
                        // Set the caret to the end of the previous selected text (true would set to the start)
                        textRange.Collapse(true);
                        ActivateNewElement();
                        break;
                    case HtmlSelectionType.ElementSelection:
                        Interop.IHTMLControlRange controlRange = currentSelection as Interop.IHTMLControlRange;
                        if (controlRange != null && controlRange.length > 0)
                        {
                            //controlRange.execCommand("Delete", false, null);
                            string outerHtml = String.Empty;
                            for (int i = 0; i < controlRange.length; i++)
                            {
                                Interop.IHTMLElement el = controlRange.item(i);
                                outerHtml += el.GetOuterHTML();

                            }
                            string newText = String.Format("<{0}{1}>{2}</{0}>", tag, attributeString, outerHtml);
                            InsertHtml(newText, true);
                        }
                        break;
                }
            }
            finally
            {
                wrapUnit.Close();
            }
            return true;
        }

        /// <summary>
        /// Depending on caret position we try to get the new element and invoke HtmlElementChanged event.
        /// </summary>
        private void ActivateNewElement()
        {
            Interop.IMarkupPointer markupPointerStart;
            Interop.IHTMLDocument2 document = htmlEditor.GetActiveDocument(false);
            Interop.IDisplayServices ds = document as Interop.IDisplayServices;
            Interop.IMarkupServices ms = document as Interop.IMarkupServices;
            ms.CreateMarkupPointer(out markupPointerStart);
            Interop.IHTMLCaret cr;
            ds.GetCaret(out cr);
            cr.MoveMarkupPointerToCaret(markupPointerStart);
            Interop.IHTMLElement element;
            Interop.MARKUP_CONTEXT_TYPE pContext;
            int chars = 0;
            string pText;
            markupPointerStart.Left(0, out pContext, out element, ref chars, out pText);
            if (element == null)
            {
                markupPointerStart.Right(0, out pContext, out element, ref chars, out pText);
            }
            if (element != null)
            {
                htmlEditor.InvokeHtmlElementChanged(element, HtmlElementChangedType.Unknown);
            }
        }

        /// <summary>
        /// Takes the selection and put an &lt;div&gt; around it.
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to insert elements around selected text. If there is no selection the
        /// element will still be inserted, but it remains invisible if no styles are applied. The host application
        /// should be aware of inserting empty elements and should provide some help through the user interface
        /// to make such elements visible or reachable.
        /// </remarks>
        public bool WrapSelectionInDiv()
        {
            return WrapSelection("div");
        }

        /// <summary>
        /// Takes the selection and put an &lt;span&gt; around it.
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to insert elements around selected text. If there is no selection the
        /// element will still be inserted, but it remains invisible if no styles are applied. The host application
        /// should be aware of inserting empty elements and should provide some help through the user interface
        /// to make such elements visible or reachable.
        /// </remarks>
        public bool WrapSelectionInSpan()
        {
            return WrapSelection("span");
        }

        /// <summary>
        /// Takes the selection and put an &lt;blockquote&gt; around it.
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to insert elements around selected text. If there is no selection the
        /// element will still be inserted, but it remains invisible if no styles are applied. The host application
        /// should be aware of inserting empty elements and should provide some help through the user interface
        /// to make such elements visible or reachable.
        /// </remarks>
        public bool WrapSelectionInBlockQuote()
        {
            return WrapSelection("blockquote");
        }

        /// <summary>
        /// Takes the selection and put an &lt;hyperlink&gt; around it.
        /// </summary>
        /// <remarks>
        /// The purpose of this method is to insert elements around selected text. If there is no selection the
        /// element will still be inserted, but it remains invisible if no styles are applied. The host application
        /// should be aware of inserting empty elements and should provide some help through the user interface
        /// to make such elements visible or reachable.
        /// </remarks>
        public void WrapSelectionInHyperlink(string url)
        {
            htmlEditor.Exec(Interop.IDM.HYPERLINK, url);
        }

        # endregion

        # region Document Methods

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
        /// <value>Value for zoom, 1 equals 100%. Set 0.5 for 50% or 2.0 for 200%.</value>
        public decimal Zoom
        {
            set
            {
                htmlEditor.GetBodyElement().SetStyleAttribute("zoom", String.Concat((double)value * 100.0, "%"));
            }
            get
            {
                string zoom = htmlEditor.GetBodyElement().GetStyleAttribute("zoom");
                decimal returnValue;
                if (Decimal.TryParse(zoom, out returnValue))
                {
                    return returnValue;
                }
                else
                {
                    return 1.0M;
                }
            }
        }

        # endregion

        #region IDisposable Members

        public void Dispose()
        {
            _activeElement = null;
        }

        #endregion
    }
}

