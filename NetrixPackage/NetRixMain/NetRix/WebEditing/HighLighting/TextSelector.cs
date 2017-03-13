using System;
using System.Collections;

using GuruComponents.Netrix;
using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.ComInterop;
using System.Windows.Forms;
using System.Drawing;

namespace GuruComponents.Netrix.WebEditing.HighLighting
{

    /// <summary>
    /// The textselector class selects text and moves the start and end pointers flexible.
    /// </summary>
    /// <remarks>
    /// This class provides a lot of methods to select a text and move the start and endpoint of the selection, store
    /// as many ranges and access the stored ranges by a key value.
    /// </remarks>
    /// <example>
    /// The following code assumes you have two comboboxes which provides all available move unit
    /// options as an entry. It assumes also that a instance of <see cref="GuruComponents.Netrix.WebEditing.HighLighting.ITextSelector">ITextSelector</see> exists which has the name
    /// <i>TextSelector</i>. To get such an instance use the following code:
    /// <code>
    /// using GuruComponents.Netrix.WebEditing.HighLighting;
    /// 
    /// ITextSelector TextSelector = this.htmlEditor1.TextSelector;
    /// </code>
    /// After retrieving the instance any of the commands for text selection can be sent. The following code assumes that
    /// two ComboBox's available on your Form containing valid numbers from the 
    /// <see cref="GuruComponents.Netrix.WebEditing.HighLighting.MoveTextPointer">MoveTextPointer</see> enumeration.
    /// <code>
    /// TextSelector.MoveFirstPointer((MoveTextPointer) this.comboBoxStartP.SelectedIndex);
    /// TextSelector.MoveSecondPointer((MoveTextPointer) this.comboBoxEndP.SelectedIndex);
    /// HighLightStyle hStyle = new HighLightStyle(Color.FromName(this.textBoxColor.Text), (UnderlineStyle) this.comboBoxType.SelectedIndex);
    /// TextSelector.RemoveHighLighting();
    /// TextSelector.SetHighLightStyle(hStyle);
    /// TextSelector.HighLightRange();
    /// </code>
    /// It is important to call <see cref="GuruComponents.Netrix.WebEditing.HighLighting.ITextSelector.RemoveHighLighting">RemoveHighLighting</see> on every selection change to remove the old style. Otherwise the
    /// old selection will stay on the surface and the new selection is added. The 
    /// <see cref="GuruComponents.Netrix.WebEditing.HighLighting.ITextSelector.RemoveHighLighting">RemoveHighLighting</see>
    /// <see cref="GuruComponents.Netrix.WebEditing.HighLighting.ITextSelector.SetHighLightStyle">SetHighLightStyle</see> method must be called once. The example calls it on every
    /// command, but a real life application should avoid this call.
    /// </example>
    public class TextSelector : ITextSelector, IDisposable
    {

        private Interop.IHTMLDocument2 document;
        private Interop.IMarkupServices ims;
        private Interop.IHTMLTxtRange trg;
        private Interop.IMarkupPointer markupPointerStart, markupPointerEnd;
        private Interop.IMarkupPointer searchPointerStart, searchPointerEnd;
        private Interop.IDisplayPointer dpStart, dpEnd;
        private Interop.IHighlightRenderingServices render = null;
        private Interop.IHighlightSegment ppi;
        private Interop.IDisplayServices ds;
        private GuruComponents.Netrix.WebEditing.HighLighting.IHighLightStyle highLightStyle;
        private Hashtable RangeStore;
        private HtmlEditor editor;
        private readonly System.Text.RegularExpressions.Regex fragment;
        private const string pattern = "<!--StartFragment-->(.*)<!--EndFragment-->";

        internal TextSelector(IHtmlEditor editor)
        {
            this.editor = (HtmlEditor)editor;
            document = editor.GetActiveDocument(false);
            ims = document as Interop.IMarkupServices;
            ds = (Interop.IDisplayServices)document;
            ds.CreateDisplayPointer(out dpStart);
            ds.CreateDisplayPointer(out dpEnd);
            ims.CreateMarkupPointer(out markupPointerStart);
            ims.CreateMarkupPointer(out markupPointerEnd);
            ims.CreateMarkupPointer(out searchPointerStart);
            ims.CreateMarkupPointer(out searchPointerEnd);
            // for preselected range don't move the pointers
            this.ResetRangePointers();
            // prepare highlighting
            render = document as Interop.IHighlightRenderingServices;
            fragment = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.Compiled);
        }

        # region Internal Access to Selector parts

        internal Interop.IMarkupPointer StartPointer
        {
            get
            {
                return this.markupPointerStart;
            }
        }

        internal Interop.IMarkupPointer EndPointer
        {
            get
            {
                return this.markupPointerEnd;
            }
        }

        # endregion

        # region Range Operations

        /// <summary>
        /// Reset the range pointer to the beginning of the document and after that set the base range to the pointer location.
        /// </summary>
        /// <remarks>
        /// The method takes the body as range, set the pointers to that range, collapses the pointers
        /// to the beginning and moves the range to the collapsed pointers. As an result, the current
        /// range is at the beginning with a length of 0.
        /// </remarks>
        public void ResetRangePointers()
        {
            trg = ((Interop.IHtmlBodyElement)document.GetBody()).createTextRange();
            trg.Collapse(true);
            ims.MovePointersToRange(trg, markupPointerStart, markupPointerEnd);
        }


        /// <summary>
        /// Selects a previously selected and stored fragment.
        /// </summary>
        /// <remarks>
        /// This method does not scroll into view if the fragment selected was outside the designer surface.
        /// </remarks>
        public void SelectRange(string key)
        {
            SelectRange(key, false);
        }

        /// <summary>
        /// Selects a previously selected and stored fragment.
        /// </summary>
        /// <param name="key">The name of the stored fragment.</param>
        /// <param name="ScrollIntoView">If set to <c>true</c> the control tries to scroll the selected range into the visible area.</param>
        public void SelectRange(string key, bool ScrollIntoView)
        {
            Interop.IHTMLTxtRange range = EnsureRangeStore()[key] as Interop.IHTMLTxtRange;
            if (range != null)
            {
                range.Select();
                if (ScrollIntoView)
                {
                    range.ScrollIntoView(true);
                }
            }
        }

        /// <summary>
        /// Moves the current range to the pointers, but does not select the range.
        /// </summary>
        /// <remarks>
        /// This method checks whether the first pointer is left from or equal to the second one. 
        /// If not, the command will be ignored.
        /// </remarks>
        public void MoveRangeToPointers()
        {
            try
            {
                int result;
                markupPointerStart.IsLeftOfOrEqualTo(markupPointerEnd, out result);
                if (result == 1)
                {
                    ims.MoveRangeToPointers(markupPointerStart, markupPointerEnd, trg);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Move the current range completely the given number of units.
        /// </summary>
        /// <remarks>
        /// The command will ignore the number if there are no more units left in the document.
        /// </remarks>
        /// <param name="Move">The kind of unit the command use to move.</param>
        /// <param name="Count">The number of units the command is supposed to move.</param>
        /// <returns>Returns the number of units the command has moved. The command will return -1 in case of an error.</returns>
        public int MoveCurrentRange(MoveUnit Move, int Count)
        {
            try
            {
                return this.trg.Move(Enum.GetName(typeof(MoveUnit), Move).ToLower(), Count);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Moves the current range start point.
        /// </summary>
        /// <remarks>
        /// This method may fail if the start point is beyond the end point.
        /// </remarks>
        /// <param name="Move">The kind of unit the command use to move.</param>
        /// <param name="Count">The number of units the command is supposed to move.</param>
        /// <returns>Returns the number of units the command has moved. The command will return -1 in case of an error.</returns>
        public int MoveCurrentRangeStart(MoveUnit Move, int Count)
        {
            try
            {
                return this.trg.MoveStart(Enum.GetName(typeof(MoveUnit), Move).ToLower(), Count);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Moves the current range end point.
        /// </summary>
        /// <remarks>
        /// This method may fail if the end point is before the start point.
        /// </remarks>
        /// <param name="Move">The kind of unit the command use to move.</param>
        /// <param name="Count">The number of units the command is supposed to move.</param>
        /// <returns>Returns the number of units the command has moved. The command will return -1 in case of an error.</returns>
        public int MoveCurrentRangeEnd(MoveUnit Move, int Count)
        {
            try
            {
                return this.trg.MoveEnd(Enum.GetName(typeof(MoveUnit), Move).ToLower(), Count);
            }
            catch
            {
                return -1;
            }
        }

        public void SelectRange()
        {
            try
            {
                if (trg != null && trg.GetText() != null)
                {
                    trg.Select();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets the text in the current range.
        /// </summary>
        /// <returns>Returns the text or an empty string, if no selection.</returns>
        public string GetTextInRange()
        {
            if (trg != null && trg.GetText() != null)
            {
                return trg.GetText();
            }
            return String.Empty;
        }

        /// <summary>
        /// Gets the HTML in the current range.
        /// </summary>
        /// <remarks>
        /// If the range is not valid or does not span any characters the method returns an empty string.
        /// </remarks>
        /// <returns>Returns the HTML in the range, if the range contains valid HTML, otherwise the text content.</returns>
        public string GetHtmlInRange()
        {
            if (trg != null && trg.GetHtmlText() != null)
            {
                return trg.GetHtmlText();
            }
            return String.Empty;
        }

        /// <summary>
        /// Set text withing the current range.
        /// </summary>
        /// <remarks>
        /// If there is no valid range to set text the method will do nothing.
        /// </remarks>
        /// <param name="text">The text to be set as a replacement of the current range content.</param>
        public void SetTextInRange(string text)
        {
            try
            {
                if (trg != null && trg.GetText() != null)
                {
                    trg.SetText(text);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Gets the Text in a stored range.
        /// </summary>
        /// <param name="key">The key of the stored range.</param>
        /// <remarks>
        /// The method will strip out any HTML tags before returning the content.
        /// If the range is not valid or does not span any characters the method returns an empty string.
        /// </remarks>
        /// <returns>Returns the HTML in the range, if the range contains valid HTML, otherwise the text content.</returns>
        public string GetTextInStoredRange(string key)
        {
            Interop.IHTMLTxtRange trg = this.EnsureRangeStore()[key] as Interop.IHTMLTxtRange;
            if (trg != null && trg.GetText() != null)
            {
                return trg.GetText();
            }
            return String.Empty;
        }

        /// <summary>
        /// Gets the HTML in a stored range.
        /// </summary>
        /// <param name="key">The key of the stored range.</param>
        /// <remarks>
        /// If the range is not valid or does not span any characters the method returns an empty string.
        /// </remarks>
        /// <returns>Returns the HTML in the range, if the range contains valid HTML, otherwise the text content.</returns>
        public string GetHtmlInStoredRange(string key)
        {
            Interop.IHTMLTxtRange trg = this.EnsureRangeStore()[key] as Interop.IHTMLTxtRange;
            if (trg != null && trg.GetHtmlText() != null)
            {
                return trg.GetHtmlText();
            }
            return String.Empty;
        }


        private Hashtable EnsureRangeStore()
        {
            if (RangeStore == null)
            {
                RangeStore = new Hashtable();
            }
            return RangeStore;
        }

        /// <summary>
        /// Stores the current range for later access.
        /// </summary>
        /// <remarks>
        /// This method can be used with the highlighting feature to highlight a prepared range later.
        /// </remarks>
        /// <param name="key"></param>
        public void SaveCurrentRange(string key)
        {
            EnsureRangeStore().Add(key, trg);
        }

        /// <summary>
        /// Clears all stored ranges.
        /// </summary>
        /// <remarks>
        /// This removes highlighting and stored range pointers, but preserves the text selected with the range.
        /// </remarks>
        public void ClearRangeStore()
        {
            EnsureRangeStore().Clear();
        }

        /// <summary>
        /// This method checks if a stored range is inside the current range.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsInCurrentRange(string key)
        {
            return trg.InRange(EnsureRangeStore()[key] as GuruComponents.Netrix.ComInterop.Interop.IHTMLTxtRange);
        }

        /// <summary>
        /// This method checks if a stored range equals the current range.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool EqualsCurrentRange(string key)
        {
            return trg.IsEqual(EnsureRangeStore()[key] as GuruComponents.Netrix.ComInterop.Interop.IHTMLTxtRange);
        }

        /// <summary>
        /// This method compares the endpoints.
        /// </summary>
        /// <param name="compare"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public int CompareEndPoints(CompareUnit compare, string key)
        {
            return trg.CompareEndPoints(Enum.GetName(typeof(CompareUnit), compare), EnsureRangeStore()[key] as GuruComponents.Netrix.ComInterop.Interop.IHTMLTxtRange);
        }

        /// <summary>
        /// Causes the object to scroll into view, aligning either to top or bottom of the window.
        /// </summary>
        /// <remarks>
        /// Depending on the size of the given range the control may not be able to scroll exactly to the very top or 
        /// very bottom of the window, but will position the object as close to the requested position as possible.
        /// </remarks>
        /// <param name="atStart"></param>
        public void ScrollIntoView(bool atStart)
        {
            try
            {
                // if there is no text, there is no way to scroll
                if (trg.GetText() != null)
                {
                    trg.ScrollIntoView(atStart);
                }
            }
            catch
            {
                // ignore errors, some selection are not viewable
            }
        }

        /// <summary>
        /// Moves the caret (insertion point) to the beginning or end of the current range.
        /// </summary>
        /// <remarks>
        /// If it appears that the caret is at the end of the visible range and the parameter
        /// <c>atStart</c> was <c>true</c>, the pointers are exchanged. That means that the start pointer
        /// is beyond the end pointer. This is still a valid range, but some commands may fail or
        /// behave differently in that case. The host application should be aware about the current
        /// pointer positions and should provide a way to reset the pointers to the document top. This
        /// can be done using the various Move commands accordingly.
        /// </remarks>
        /// <param name="atStart"></param>
        public void MoveCaretToPointer(bool atStart)
        {
            try
            {
                //                Interop.IHTMLCaret cr;
                //                ds.GetCaret(out cr);
                //                Interop.IHTMLElement element;
                //                ds.CreateDisplayPointer(out dpStart);
                //                ds.CreateDisplayPointer(out dpEnd);                
                //                int i;
                //                this.markupPointerStart.IsLeftOfOrEqualTo(this.markupPointerEnd, out i);
                //                if (i == 1)
                //                {
                //                    dpStart.MoveToMarkupPointer(this.markupPointerStart, null);
                //                    dpEnd.MoveToMarkupPointer(this.markupPointerEnd, null);                
                //                    dpEnd.GetFlowElement(out element);
                //                    if (element != null)
                //                    {
                //                        cr.MoveCaretToPointerEx(atStart ? this.dpStart : this.dpEnd, 1, 1, Interop.CARET_DIRECTION.CARET_DIRECTION_SAME);
                //                        cr.Show(1);           
                //                    }
                //                }
                Interop.IHTMLCaret cr;
                ds.GetCaret(out cr);
                ds.CreateDisplayPointer(out dpStart);
                dpStart.MoveToMarkupPointer(markupPointerStart, null);
                cr.MoveCaretToPointer(dpStart, true, Interop.CARET_DIRECTION.CARET_DIRECTION_SAME);

            }
            catch
            {
            }
        }

        /// <overloads/>
        /// <summary>
        /// Returns a collection of information about both pointers.
        /// </summary>
        /// <remarks>
        /// The number of character the information method retrieves is set to 1.
        /// </remarks>
        /// <param name="Move">Value that specifies TRUE if the pointer is to move past the content to the left (right respectively), or FALSE otherwise. If TRUE, the pointer will move either to the other side of the tag to its left, or up to the number of characters specified by parameter tryChars, depending on the <see cref="ContextType"/> to the pointer's left (right respectively).</param>
        /// <returns>Returns a structure with basic pointer informations.</returns>
        public PointerInformation GetPointerInformation(bool Move)
        {
            return GetPointerInformation(Move, 1);
        }

        /// <summary>
        /// Returns a collection of information about both pointers.
        /// </summary>
        /// <param name="Move">Value that specifies TRUE if the pointer is to move past the content to the left (right respectively), or FALSE otherwise. If TRUE, the pointer will move either to the other side of the tag to its left, or up to the number of characters specified by parameter tryChars, depending on the <see cref="ContextType"/> to the pointer's left (right respectively).</param>
        /// <param name="tryChars">Variable that specifies the number of characters to retrieve to XXXRetrieveChars property, if <see cref="ContextType"/> is <see cref="ContextType.Text"/>, 
        /// and receives the actual number of characters the method was able to retrieve. It can be set to -1, 
        /// indicating that the method should retrieve an arbitrary amount of text, up to the next no-scope element 
        /// or element scope transition.</param>
        /// <returns>Returns a structure with basic pointer informations.</returns>
        public PointerInformation GetPointerInformation(bool Move, int tryChars)
        {
            PointerInformation pi = new PointerInformation();
            int i, cling;
            this.markupPointerStart.IsPositioned(out i);
            pi.IsPositioned = (i == 0) ? false : true;

            this.markupPointerStart.IsRightOf(this.markupPointerEnd, out i);
            pi.FirstIsRightOfSecond = (i == 0) ? false : true;

            this.markupPointerStart.IsRightOfOrEqualTo(this.markupPointerEnd, out i);
            pi.FirstIsRightOfSecond = (i == 0) ? false : true;

            this.markupPointerStart.IsLeftOf(this.markupPointerEnd, out i);
            pi.FirstIsLeftOfSecond = (i == 0) ? false : true;

            this.markupPointerStart.IsLeftOfOrEqualTo(this.markupPointerEnd, out i);
            pi.FirstIsLefttOfOrEqualToSecond = (i == 0) ? false : true;

            this.markupPointerStart.Cling(out cling);
            pi.FirstPointerCling = (cling == 0) ? false : true;
            this.markupPointerEnd.Cling(out cling);
            pi.SecondPointerCling = (cling == 0) ? false : true;

            Interop.POINTER_GRAVITY gravity;

            this.markupPointerStart.Gravity(out gravity);
            pi.FirstPointerGravity = (PointerGravity)(int)gravity;
            this.markupPointerEnd.Gravity(out gravity);
            pi.SecondPointerGravity = (PointerGravity)(int)gravity;

            this.markupPointerStart.IsEqualTo(this.markupPointerEnd, out i);
            pi.PointersAreEqual = (i == 0) ? false : true;

            Interop.MARKUP_CONTEXT_TYPE pContext = Interop.MARKUP_CONTEXT_TYPE.CONTEXT_TYPE_None;
            Interop.IHTMLElement element = null;
            int chars = 0;
            string pText = String.Empty;
            chars = tryChars;
            Interop.IMarkupPointer mpFirst, mpSecond;
            ims.CreateMarkupPointer(out mpFirst);
            ims.CreateMarkupPointer(out mpSecond);

            mpFirst.MoveToPointer(this.markupPointerStart);
            mpSecond.MoveToPointer(this.markupPointerEnd);

            try
            {
                mpFirst.Left(Move ? 1 : 0, out pContext, out element, ref chars, out pText);
            }
            catch
            {
            }
            pi.FirstPointerLeftRetrievedLength = chars;
            pi.FirstPointerLeftRetrievedChars = pText == null ? String.Empty : pText;
            pi.FirstPointerLeftElementScope = element != null ? this.editor.GenericElementFactory.CreateElement(element) as IElement : null;
            pi.FirstPointerLeftContext = (ContextType)(int)pContext;
            chars = tryChars;
            try
            {
                mpFirst.Right(Move ? 1 : 0, out pContext, out element, ref chars, out pText);
            }
            catch
            {
            }
            pi.FirstPointerRightRetrievedLength = chars;
            pi.FirstPointerRightRetrievedChars = pText == null ? String.Empty : pText;
            pi.FirstPointerRightElementScope = element != null ? this.editor.GenericElementFactory.CreateElement(element) as IElement : null;
            pi.FirstPointerRightContext = (ContextType)(int)pContext;
            chars = tryChars;
            try
            {
                mpSecond.Left(Move ? 1 : 0, out pContext, out element, ref chars, out pText);
            }
            catch
            {
            }
            pi.SecondPointerLeftRetrievedLength = chars;
            pi.SecondPointerLeftRetrievedChars = pText == null ? String.Empty : pText;
            pi.SecondPointerLeftElementScope = element != null ? this.editor.GenericElementFactory.CreateElement(element) as IElement : null;
            pi.SecondPointerLeftContext = (ContextType)(int)pContext;
            chars = tryChars;
            try
            {
                mpSecond.Right(Move ? 1 : 0, out pContext, out element, ref chars, out pText);
            }
            catch
            {
            }
            pi.SecondPointerRightRetrievedLength = chars;
            pi.SecondPointerRightRetrievedChars = pText == null ? String.Empty : pText;
            pi.SecondPointerRightElementScope = element != null ? this.editor.GenericElementFactory.CreateElement(element) as IElement : null;
            pi.SecondPointerRightContext = (ContextType)(int)pContext;

            return pi;
        }


        /// <summary>
        /// Sets the gravity attribute of this pointer.
        /// </summary>
        /// <param name="pointer">The Pointer which gravity is to set.</param>
        /// <param name="gravity">The gravity which is to set.</param>
        public void SetPointerGravity(PointerSelection pointer, PointerGravity gravity)
        {
            if (pointer == PointerSelection.First)
            {
                this.markupPointerStart.SetGravity((Interop.POINTER_GRAVITY)(int)gravity);
            }
            else
            {
                this.markupPointerEnd.SetGravity((Interop.POINTER_GRAVITY)(int)gravity);
            }
        }

        /// <summary>
        /// Expand the current range to that partial units are completely contained.
        /// </summary>
        /// <param name="ExpandTo"></param>
        /// <returns></returns>
        public bool ExpandRange(MoveUnit ExpandTo)
        {
            return this.trg.Expand(Enum.GetName(typeof(MoveUnit), ExpandTo).ToLower());
        }

        /// <summary>
        /// Selects the whole text of the given element.
        /// </summary>
        /// <remarks>
        /// This is a mixed mode command, uses internally Pointers and Ranges. The command will move both 
        /// pointers to the elements boundaries. Then the current base range is set to these pointers and finally
        /// used to set the elements content. After all the changed content is selected and scrolled into view.
        /// </remarks>
        /// <param name="element">Any element object that implements <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement">IElement</see> and that does contain selectable text.</param>
        public void SelectElementText(GuruComponents.Netrix.WebEditing.Elements.IElement element)
        {
            try
            {
                Interop.IHTMLElement baseElement = ((GuruComponents.Netrix.WebEditing.Elements.Element)element).GetBaseElement();
                if (baseElement.GetInnerText() != null)
                {
                    //trg.MoveToElementText((Interop.IHTMLElement)baseElement);
                    this.markupPointerStart.MoveAdjacentToElement(baseElement, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterBegin);
                    this.markupPointerEnd.MoveAdjacentToElement(baseElement, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeEnd);
                    ims.MoveRangeToPointers(markupPointerStart, markupPointerEnd, trg);
                    trg.Select();
                    trg.ScrollIntoView(true);
                }
                else
                {
                    this.markupPointerStart.MoveAdjacentToElement(baseElement, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeBegin);
                    this.markupPointerEnd.MoveAdjacentToElement(baseElement, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterEnd);
                    ims.MoveRangeToPointers(markupPointerStart, markupPointerEnd, trg);
                    trg.Select();
                    trg.ScrollIntoView(true);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// This method deletes the content of the current base range.
        /// </summary>
        public void DeleteRangeText()
        {
            if (trg != null && trg.GetText() != null)
            {
                trg.ExecCommand("Delete", false, 1);
            }
        }

        # endregion

        # region Display Operations

        public void GetCaretPosition()
        {

        }

        # endregion

        # region Pointer Operations

        private void GetPosPointerToPointer(Interop.IMarkupPointer pStart, Interop.IMarkupPointer pEnd, out long numbChar)
        {
            numbChar = 0;
            int bIsEqual = 0;
            int l = 1;
            pStart.IsEqualTo(pEnd, out bIsEqual);
            if (bIsEqual == 1)
            {
                numbChar = 0;
            }
            else
            {
                while (l == 1)
                {
                    pStart.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTCHAR);
                    pStart.IsLeftOfOrEqualTo(pEnd, out l);
                    numbChar++;
                }
            }
            --numbChar;
        }

        /// <summary>
        /// Returns the current selection positions, based on markup pointers, within a given element.
        /// </summary>
        /// <remarks>
        /// The values ignore all HTML formatting and count just characters. Returns -1 for both if there is no selection.
        /// <para>
        /// The method expects an element object. See other overload to retrieve within the body.
        /// </para>
        /// <para>
        /// This method does not interfer other method in this class. It's deterministic in any way.
        /// </para>
        /// </remarks>
        /// <param name="posStart">Returns the start position within the body range, in characters, zero based.</param>
        /// <param name="posEnd">Returns the start position within the body range, in characters, zero based.</param>
        public void GetSelectionPosition(IElement withinElement, out long posStart, out long posEnd)
        {
            posStart = -1;
            posEnd = -1;
            if (ims != null)
            {
                Interop.IHTMLTxtRange trg1 = ((Interop.IHTMLDocument2)document).GetSelection().CreateRange() as Interop.IHTMLTxtRange;
                if (trg1 != null)
                {
                    Interop.IMarkupPointer pFirst, pSecond;

                    ims.CreateMarkupPointer(out pFirst);
                    ims.CreateMarkupPointer(out pSecond);
                    // pSecond is the end of the range that is the same as the beginning of selection
                    ims.MovePointersToRange(trg1, pFirst, pSecond);
                    // pFirst ist the selections start point, pSecond is now the parent element's beginning
                    pSecond.MoveAdjacentToElement(withinElement.GetBaseElement(), Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterBegin);
                    // we calculate the chars from the parent's beginning to the selection's start
                    GetPosPointerToPointer(pSecond, pFirst, out posStart);
                    // from this on, we add the length of the selection
                    posEnd = posStart + ((trg1.GetText() == null) ? 0 : trg1.GetText().Length);
                    posEnd--;
                    posStart--;
                }
            }
        }

        /// <summary>
        /// Returns the current selection positions, based on markup pointers.
        /// </summary>
        /// <remarks>
        /// The values ignore all HTML formatting and count just characters. Returns -1 for both if there is no selection.
        /// <para>
        /// This method does not interfer other method in this class. It's deterministic in any way.
        /// </para>
        /// </remarks>
        /// <param name="posStart">Returns the start position within the body range, in characters, zero based.</param>
        /// <param name="posEnd">Returns the start position within the body range, in characters, zero based.</param>
        public void GetSelectionPosition(out long posStart, out long posEnd)
        {
            GetSelectionPosition(editor.GetBodyElement(), out posStart, out posEnd);
        }

        /// <summary>
        /// Set the current selection based on positions of previously read  markup pointers. 
        /// </summary>
        /// <param name="posStart"></param>
        /// <param name="posEnd"></param>
        public void SetSelectionPosition(long posStart, long posEnd)
        {
            if (ims != null)
            {
                Interop.IHTMLTxtRange trg1 = ((Interop.IHtmlBodyElement)document.GetBody()).createTextRange();
                if (trg1 != null)
                {
                    Interop.IMarkupPointer pStart, pEnd;

                    ims.CreateMarkupPointer(out pStart);
                    ims.CreateMarkupPointer(out pEnd);

                    ims.MovePointersToRange(trg1, pStart, pEnd);

                    while (posStart > 0)
                    {
                        pStart.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTCHAR);
                        posStart--;
                    }
                    pEnd.MoveToPointer(pStart);
                    while (posEnd > 0)
                    {
                        pEnd.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTCHAR);
                        posEnd--;
                    }
                    ims.MoveRangeToPointers(pStart, pEnd, trg1);
                    string dummy = trg1.GetText();
                    if (dummy != null)
                    {
                        try
                        {
                            trg1.Select();
                        }
                        catch (Exception)
                        { }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the position of a markup pointer. 
        /// </summary>
        /// <remarks>As long as the pointer's positioned, the method can be used to retrieve the carets position at line.</remarks>
        /// <returns>Returns markup pointer's position</returns>
        public int GetMarkupPosition(PointerSelection pointer)
        {
            int pos;
            if (pointer == PointerSelection.Second)
            {
                ((Interop.IMarkupPointer2)this.markupPointerEnd).GetMarkupPosition(out pos);
            }
            else
            {
                ((Interop.IMarkupPointer2)this.markupPointerStart).GetMarkupPosition(out pos);
            }
            return pos;
        }

        /// <summary>
        /// Determines if the markup pointer is positioned at a word break.
        /// </summary>
        /// <param name="pointer">If <c>False</c> the first Pointer is checked, the second otherwise.</param>
        public bool IsAtWordBreak(PointerSelection pointer)
        {
            int atBreak = 0;
            if (pointer == PointerSelection.Second)
            {
                ((Interop.IMarkupPointer2)this.markupPointerEnd).IsAtWordBreak(out atBreak);
            }
            else
            {
                ((Interop.IMarkupPointer2)this.markupPointerStart).IsAtWordBreak(out atBreak);
            }
            return atBreak == 0 ? false : true;
        }

        /// <summary>
        /// Determines if a markup pointer is located inside of, at the beginning of, or at the end of text that is formatted as a URL. 
        /// </summary>
        public bool IsInsideURL(PointerSelection pointer)
        {
            int inUrl = 0;
            if (pointer == PointerSelection.Second)
            {
                ((Interop.IMarkupPointer2)this.markupPointerEnd).IsInsideURL(this.markupPointerEnd, out inUrl);
            }
            else
            {
                ((Interop.IMarkupPointer2)this.markupPointerStart).IsInsideURL(this.markupPointerStart, out inUrl);
            }
            return inUrl == 0 ? false : true;
        }

        /// <summary>
        /// Moves a markup pointer to a particular element in a markup container.
        /// </summary>
        public void MoveToElementContent(PointerSelection pointer, IElement element, bool atStart)
        {
            int fAtStart = atStart ? 1 : 0;
            Interop.IHTMLElement el = ((Element)element).GetBaseElement();
            if (pointer == PointerSelection.Second)
            {
                ((Interop.IMarkupPointer2)this.markupPointerEnd).MoveToContent(el, fAtStart);
            }
            else
            {
                ((Interop.IMarkupPointer2)this.markupPointerStart).MoveToContent(el, fAtStart);
            }
        }

        /// <summary>
        /// Moves a markup pointer to a specified location in a specified direction, but not past the other markup pointer. 
        /// </summary>
        /// <remarks>
        /// This method can simplify the usage of pointers, because it avoids an frequently mistake which results
        /// in some loss of functionality; the positioning of the second pointer before the first one.  
        /// </remarks>
        public void MoveUnitBounded(PointerSelection pointer, MoveUnit mu)
        {
            Interop.MOVEUNIT_ACTION muAction = (Interop.MOVEUNIT_ACTION)(int)mu;
            if (pointer == PointerSelection.Second)
            {
                ((Interop.IMarkupPointer2)this.markupPointerEnd).MoveUnitBounded(muAction, markupPointerStart);
            }
            else
            {
                ((Interop.IMarkupPointer2)this.markupPointerEnd).MoveUnitBounded(muAction, markupPointerEnd);
            }
        }

        /// <overloads>This method has two overloads.</overloads>
        /// <summary>
        /// Select the current range between two pointers.
        /// </summary>
        /// <remarks>
        /// This method moves the current base range to the current pointers and selects the range, 
        /// when the first pointer was left of the second one.
        /// <para>
        /// The method does not scroll the range into view.
        /// </para>
        /// </remarks>
        public void SelectPointerRange()
        {
            SelectPointerRange(false);
        }

        /// <summary>
        /// Select the current range between two pointers.
        /// </summary>
        /// <remarks>
        /// This method moves the current base range to the current pointers and selects the range, 
        /// when the first pointer was left of the second one.
        /// </remarks>
        /// <param name="ScrollIntoView">If set to <c>true</c> the control tries to scroll the selected range into the visible area.</param>
        public void SelectPointerRange(bool ScrollIntoView)
        {
            ims.MoveRangeToPointers(markupPointerStart, markupPointerEnd, trg);
            try
            {
                int result;
                markupPointerStart.IsLeftOf(markupPointerEnd, out result);
                if (result == 1)
                {
                    trg.Select();
                    if (ScrollIntoView)
                    {
                        trg.ScrollIntoView(true);
                    }
                }
            }
            catch
            {
            }

        }

        /// <summary>
        /// Deletes the text between the pointers. 
        /// </summary>
        /// <remarks>If the selection is fragmented HTML a valid structure will be rebuild.</remarks>

        public void DeleteTextBetweenPointers()
        {
            Interop.IHTMLTxtRange range = ((Interop.IHtmlBodyElement)document.GetBody()).createTextRange();
            if (range != null)
            {
                int result;
                markupPointerStart.IsLeftOf(markupPointerEnd, out result);
                if (result == 1)
                {
                    ims.MoveRangeToPointers(markupPointerStart, markupPointerEnd, range);
                    range.ExecCommand("Delete", false, 1);
                }
            }
        }


        /// <summary>
        /// Returns the text between two valid pointers.
        /// </summary>
        /// <returns>The text between the pointers. HTML tags are stripped out.</returns>
        public string GetTextBetweenPointers()
        {
            Interop.IHTMLTxtRange range = ((Interop.IHtmlBodyElement)document.GetBody()).createTextRange();
            if (range != null)
            {
                int result;
                markupPointerStart.IsLeftOf(markupPointerEnd, out result);
                if (result == 1)
                {
                    ims.MoveRangeToPointers(markupPointerStart, markupPointerEnd, range);
                    return range.GetText();
                }
            }
            return String.Empty;
        }

        public void SetTextBetweenPointers(string text)
        {
            Interop.IHTMLTxtRange range = ((Interop.IHtmlBodyElement)document.GetBody()).createTextRange();
            if (range != null)
            {
                try
                {
                    int result;
                    markupPointerStart.IsLeftOf(markupPointerEnd, out result);
                    if (result == 1)
                    {
                        ims.MoveRangeToPointers(markupPointerStart, markupPointerEnd, range);
                        range.Select();
                        range.SetText(text);
                        range.Collapse(true);
                    }
                }
                catch
                {
                }
            }
        }

        public void SetHtmlBetweenPointers(string html)
        {
            Interop.IHTMLTxtRange range = ((Interop.IHtmlBodyElement)document.GetBody()).createTextRange();
            if (range != null)
            {
                try
                {
                    int result;
                    markupPointerStart.IsLeftOf(markupPointerEnd, out result);
                    if (result == 1)
                    {
                        ims.MoveRangeToPointers(markupPointerStart, markupPointerEnd, range);
                        range.PasteHTML(html);
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Paste the HTML text into the element.
        /// </summary>
        /// <remarks>
        /// Element must be of type container. The HTML must be valid after pasting.
        /// </remarks>
        /// <seealso cref="Paste(IElement)"/>
        /// <param name="element">The element in which the HTML has to be pasted.</param>
        /// <param name="html">Any valid HTML text.</param>
        public void PasteHtml(IElement element, string html)
        {
            Interop.IHTMLTxtRange range = ((Interop.IHtmlBodyElement)document.GetBody()).createTextRange();
            range.MoveToElementText(element.GetBaseElement());
            range.PasteHTML(html);
        }

        /// <summary>
        /// Paste the Clipboard text into the element.
        /// </summary>
        /// <remarks>
        /// Element must be of type container. The HTML must be valid after pasting.
        /// </remarks>
        /// <seealso cref="PasteHtml"/>
        /// <exception cref="ArgumentException">Thrown if the clipboard has no valid data. Expected formats are HTML and Text.</exception>
        /// <param name="element">The element in which the HTML has to be pasted.</param>
        public void Paste(IElement element)
        {
            object html = Clipboard.GetData(DataFormats.Html);
            if (html == null)
            {
                html = Clipboard.GetData(DataFormats.Text);
            }
            if (html != null)
            {
                // if we see a clipboard fragment we extract it
                System.Text.RegularExpressions.Match match = fragment.Match(html.ToString());
                if (match.Success)
                {
                    html = match.Groups[1].Value;
                }
                Interop.IHTMLTxtRange range = ((Interop.IHtmlBodyElement)document.GetBody()).createTextRange();
                range.MoveToElementText(element.GetBaseElement());
                range.Collapse(false);
                range.PasteHTML(html.ToString());
            }
            else
            {
                throw new ArgumentException("The clipboard did neither contain HTML nor Text");
            }
        }

        /// <summary>
        /// Paste the current content of clipboard at current caret position and let pointers and range follow the insertion point.
        /// </summary>
        public void Paste()
        {
            object html = Clipboard.GetData(DataFormats.Html);
            if (html == null)
            {
                html = Clipboard.GetData(DataFormats.Text);
            }
            if (html != null)
            {
                // if we see a clipboard fragment we extract it
                System.Text.RegularExpressions.Match match = fragment.Match(html.ToString());
                if (match.Success)
                {
                    html = match.Groups[1].Value;
                }
                MovePointersToCaret();
                MoveRangeToPointers();
                this.trg.PasteHTML(html.ToString());
            }
            else
            {
                throw new ArgumentException("The clipboard did neither contain HTML nor Text");
            }
        }

        /// <summary>
        /// Returns the HTML between two valid pointers.
        /// </summary>
        /// <returns>The text between the pointers. HTML tags are preserved.</returns>
        public string GetHtmlBetweenPointers()
        {
            Interop.IHTMLTxtRange range = ((Interop.IHtmlBodyElement)document.GetBody()).createTextRange();
            if (range != null)
            {
                int result;
                markupPointerStart.IsLeftOf(markupPointerEnd, out result);
                if (result == 1)
                {
                    ims.MoveRangeToPointers(markupPointerStart, markupPointerEnd, range);
                    return range.GetHtmlText();
                }
            }
            return String.Empty;
        }

        Interop.IHTMLTxtRange range = null;

        /// <summary>
        /// Call this to reset the pointers to catch the whole document.
        /// </summary>
        public void ResetFindWordPointer()
        {
            Interop.IHTMLElement body = document.GetBody();
            range = ((Interop.IHtmlBodyElement)body).createTextRange();
            try
            {
                this.searchPointerStart.MoveAdjacentToElement(body, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeBegin);
                this.searchPointerEnd.MoveAdjacentToElement(body, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterEnd);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Searches a text and move the pointers to the result.
        /// </summary>
        /// <remarks>
        /// This is a nice enhancement of the regular find function to narrow a search. It searches the whole document,
        /// but starts searching using the current pointer position. So in case you want to restart search from the
        /// beginning, just set the pointers back by calling <see cref="MovePointersToElement"/>, <see cref="MoveFirstPointer"/>,
        /// or <see cref="MoveSecondPointer"/>.
        /// If you seek a word and highlight is, you must assure that the pointers get collapsed to avoid seeking the same
        /// word again. If the find method would to this, you would have no range afterwards. 
        /// </remarks>
        /// <example>
        /// htmlEditor1.TextSelector.SetHighLightStyle(highLightStyle);
        /// htmlEditor1.TextSelector.MovePointersToElement(htmlEditor1.GetBodyElement(), true);
        /// // assume textBoxSearch is a TextBox with the search word
        /// while (htmlEditor1.TextSelector.FindTextBetweenPointers(textBoxSearch.Text, false, true, true))
        /// {
        ///     htmlEditor1.TextSelector.HighLightRange();
        ///     highLightList.Add(htmlEditor1.TextSelector.LastSegment);
        ///     // checkBoxAll is a CheckBox that lets the user select all words with one shot
        ///     if (!checkBoxAll.Checked) break;
        /// }
        /// RefreshList();
        /// </example>
        /// <param name="searchString">A string to search for.</param>
        /// <param name="upWards">The direction in which the search takes place.</param>
        /// <param name="matchCase">Must be set to <c>true</c> if match case is required.</param>
        /// <param name="wholeWord">Must set to <c>true</c> if only whole words should be found.</param>
        /// <returns>Returns <c>true</c> if something was found, <c>false</c> otherwise.</returns>
        public bool FindTextBetweenPointers(string searchString, bool upWards, bool matchCase, bool wholeWord)
        {
            if (range == null)
            {
                ResetFindWordPointer();
            }
            bool found = false;
            if (range != null)
            {
                int result;
                // these pointers control the search area
                searchPointerStart.IsLeftOf(searchPointerEnd, out result);
                int searchConditionFlag = (!matchCase ? 0 : 4) | (!wholeWord ? 0 : 2);
                if (result == 1)
                {
                    // these pointers manage search
                    ims.MoveRangeToPointers(searchPointerStart, searchPointerEnd, range);
                    found = range.FindText(searchString, upWards ? -1000000 : 1000000, searchConditionFlag);
                    if (found)
                    {
                        // Move public pointers to let user do anything with the pointers, such as highlighting
                        ims.MovePointersToRange(range, markupPointerStart, markupPointerEnd);
                        // false avoids searching same word again
                        range.Collapse(false);
                        ims.MovePointersToRange(range, searchPointerStart, searchPointerEnd);
                        // assure that the end pointer stays at the end of the document to find all instances
                        Interop.IHTMLElement body = document.GetBody();
                        Interop.IHTMLTxtRange tmpRange = ((Interop.IHtmlBodyElement)body).createTextRange();
                        this.searchPointerEnd.MoveAdjacentToElement(body, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterEnd);
                    }
                }
                else
                {
                    // check whether there is room to the right
                    searchPointerStart.IsLeftOf(searchPointerEnd, out result);
                    if (result == 1)
                    {
                        ims.MoveRangeToPointers(searchPointerStart, searchPointerEnd, range);
                        found = range.FindText(searchString, upWards ? -1000000 : 1000000, searchConditionFlag);
                        if (found)
                        {
                            // Make them equally to recognize recall
                            ims.MovePointersToRange(range, markupPointerStart, markupPointerEnd);
                            // false avoids searching same word again
                            range.Collapse(false);
                            // assure that the end pointer stays at the end of the document to find all instances
                            Interop.IHTMLElement body = document.GetBody();
                            Interop.IHTMLTxtRange tmpRange = ((Interop.IHtmlBodyElement)body).createTextRange();
                            this.searchPointerEnd.MoveAdjacentToElement(body, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterEnd);
                        }
                    }
                    else
                    {
                        found = false; // reached end of document
                    }
                }
            }
            // Reset if reached end of text with no results
            if (!found) ResetFindWordPointer();
            return found;
        }

        /// <summary>
        /// Move the first pointer.
        /// </summary>
        /// <remarks>
        /// The pointers can be used to create a range. The range can be moved too, using the
        /// <see cref="GuruComponents.Netrix.WebEditing.HighLighting.TextSelector.MoveCurrentRange">MoveCurrentRange</see>
        /// method.
        /// </remarks>
        /// <param name="Move">The first pointer will be moved to the given destination.</param>
        public void MoveFirstPointer(MoveTextPointer Move)
        {
            try
            {
                this.markupPointerStart.MoveUnit((GuruComponents.Netrix.ComInterop.Interop.MOVEUNIT_ACTION)(int)Move);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Move the second pointer.
        /// </summary>
        /// <remarks>
        /// The pointers can be used to create a range. The range can be moved too, using the
        /// <see cref="GuruComponents.Netrix.WebEditing.HighLighting.TextSelector.MoveCurrentRange">MoveCurrentRange</see>
        /// method.
        /// </remarks>
        /// <param name="Move">The second pointer will be moved to the given destination.</param>
        public void MoveSecondPointer(MoveTextPointer Move)
        {
            try
            {
                this.markupPointerEnd.MoveUnit((GuruComponents.Netrix.ComInterop.Interop.MOVEUNIT_ACTION)(int)Move);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Move the pointers to the caret to start next positioning around the caret.
        /// </summary>
        /// <remarks>
        /// It is recommended to set the pointers actively before any caret related move operation starts.
        /// </remarks>
        public void MovePointersToCaret()
        {
            Interop.IHTMLCaret cr;
            ds.GetCaret(out cr);
            cr.MoveMarkupPointerToCaret(this.markupPointerStart);
            cr.MoveMarkupPointerToCaret(this.markupPointerEnd);
            cr.MoveDisplayPointerToCaret(this.dpStart);
            cr.MoveDisplayPointerToCaret(this.dpEnd);
        }

        /// <summary>
        /// Selects the whole text of the given element.
        /// </summary>
        /// <param name="element">Any element object that implements <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement">IElement</see> and that does contain selectable text.</param>
        /// <param name="withRange">Whether or not the current base range should follow the pointers immediataly.</param>
        public void MovePointersToElement(GuruComponents.Netrix.WebEditing.Elements.IElement element, bool withRange)
        {
            try
            {
                Interop.IHTMLElement baseElement = ((GuruComponents.Netrix.WebEditing.Elements.Element)element).GetBaseElement();
                if (IsNotAContainer(baseElement))
                {
                    this.markupPointerStart.MoveAdjacentToElement(baseElement, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterBegin);
                    this.markupPointerEnd.MoveAdjacentToElement(baseElement, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeEnd);
                }
                else
                {
                    this.markupPointerStart.MoveAdjacentToElement(baseElement, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_BeforeBegin);
                    this.markupPointerEnd.MoveAdjacentToElement(baseElement, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterEnd);
                }
                if (withRange)
                {
                    ims.MoveRangeToPointers(markupPointerStart, markupPointerEnd, trg);
                }
            }
            catch
            {
            }
        }


        # endregion

        # region Highlighting Operations

        /// <summary>
        /// Set the default highlight style.
        /// </summary>
        /// <remarks>
        /// This methiod pre-sets the highlight style. The current selection will not be changeds.
        /// </remarks>
        /// <param name="HighLightStyle">The highlight style object which is used from now on.</param>
        public void SetHighLightStyle(GuruComponents.Netrix.WebEditing.HighLighting.IHighLightStyle HighLightStyle)
        {
            this.highLightStyle = HighLightStyle;
        }

        /// <summary>
        /// Highlights the current range.
        /// </summary>
        /// <remarks>
        /// This method cannot work if no text fragment was selected before call. The highlight style should be set
        /// before first call.
        /// </remarks>
        /// <returns>The method returns <c>true</c> if the selection was highlighted, <c>false</c> otherwise. It is possible that </returns> the method fails if the start and end pointer do not define a valid range, e.g. the end pointer is on or before the start pointer.
        public bool HighLightRange()
        {
            try
            {
                dpStart.MoveToMarkupPointer(markupPointerStart, null);
                dpEnd.MoveToMarkupPointer(markupPointerEnd, null);
                Interop.IHTMLRenderStyle renderStyle;
                if (this.highLightStyle == null)
                {
                    HighLightStyle tempStyle = new HighLightStyle(HighlightColor.Color(Color.White), HighlightColor.Color(Color.Black));
                    renderStyle = tempStyle.GetRenderStyle(document);
                }
                else
                {
                    renderStyle = ((HighLightStyle)this.highLightStyle).GetRenderStyle(document);
                }
                //render.AddSegment(dpStart, dpEnd, renderStyle, out ppi);
                Interop.IHTMLTxtRange range = ((Interop.IHtmlBodyElement)document.GetBody()).createTextRange();
                ims.MoveRangeToPointers(markupPointerStart, markupPointerEnd, range);
                lastSegment = new HighLightSegment(render, renderStyle, dpStart, dpEnd, range);
                return true;
            }
            catch
            {
                return false;
            }
        }


        private IHighLightSegment lastSegment;

        /// <summary>
        /// Returns the last added highlight segment.
        /// </summary>
        /// <remarks>
        /// This segment contains a method which can be used to remove the segment later.
        /// </remarks>
        public IHighLightSegment LastSegment
        {
            get
            {
                return lastSegment;
            }
        }

        /// <summary>
        /// Remove the last range segment used to highlight a single range.
        /// </summary>
        /// <remarks>
        /// The method ignores the command if no fragment is available.
        /// <para>
        /// If you need to remove the segments highlighted in previous steps you have to store these segments on your own and remove one by one
        /// using a loop.
        /// <example>
        /// private List&lt;IHighLightSegment&gt; textHighlightSegments;
        ///  
        /// void _buttonTextHighLightRemove_ExecuteEvent(object sender, ExecuteEventArgs e)
        /// {
        ///    if (textHighlightSegments != null &amp;&amp; textHighlightSegments.Count &gt; 0)
        ///    {
        ///        foreach (HighLightSegment s in textHighlightSegments)
        ///        {
        ///            s.RemoveSegment();
        ///        }
        ///    }
        /// }
        /// </example>
        /// The underlying object for each segment is COM and hence not properly serializable.
        /// </para>
        /// </remarks>
        public void RemoveHighLighting()
        {
            if (ppi != null)
            {
                render.RemoveSegment(ppi);
            }
        }


        # endregion

        # region Private Helper Methods

        private bool IsNotAContainer(Interop.IHTMLElement element)
        {
            string tn = element.GetTagName().ToUpper();
            if (tn == "HR" || tn == "BR" || tn == "IMG" || tn == "INPUT" || tn == "BUTTON")
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        # endregion

        # region internal Helper methods


        /// <summary>
        /// Move the pointers to other from same scope.
        /// </summary>
        /// <param name="pStart"></param>
        /// <param name="pEnd"></param>
        internal void MovePointers(Interop.IMarkupPointer pStart, Interop.IMarkupPointer pEnd)
        {
            markupPointerStart.MoveToPointer(pStart);
            markupPointerEnd.MoveToPointer(pEnd);
        }

        # endregion


        #region IDisposable Members

        public void Dispose()
        {
            document = null;
            ims = null;
            trg = null;
            markupPointerStart = null;
            markupPointerEnd = null;
            dpStart = null;
            dpEnd = null;
            render = null;
            ppi = null;
            ds = null;
        }

        #endregion
    }
}
