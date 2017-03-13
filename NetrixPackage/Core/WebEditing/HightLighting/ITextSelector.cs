using GuruComponents.Netrix.WebEditing.Elements;
namespace GuruComponents.Netrix.WebEditing.HighLighting
{
    /// <summary>
    /// Interface allows access to text selection features.
    /// </summary>
    /// <remarks>
    /// Retrieve an instance by accessing the <see cref="IHtmlEditor.TextSelector">TextSelector</see> property of base control.
    /// </remarks>
    /// <seealso cref="IHtmlEditor.TextSelector"/>
    public interface ITextSelector
    {

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
        /// <param name="withinElement">Element within the selection is searched.</param>
        /// <param name="posStart">Returns the start position within the body range, in characters, zero based.</param>
        /// <param name="posEnd">Returns the start position within the body range, in characters, zero based.</param>
        void GetSelectionPosition(IElement withinElement, out long posStart, out long posEnd);

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
        void GetSelectionPosition(out long posStart, out long posEnd);

        /// <summary>
        /// Sets the gravity attribute of this pointer.
        /// </summary>
        /// <param name="pointer">The Pointer which gravity is to set.</param>
        /// <param name="gravity">The gravity which is to set.</param>
        void SetPointerGravity(PointerSelection pointer, PointerGravity gravity);


        /// <overloads/>
        /// <summary>
        /// Returns a collection of information about both pointers.
        /// </summary>
        /// <remarks>
        /// The number of character the information method retrieves is set to 1.
        /// <seealso cref="GetPointerInformation(bool,int)"/>
        /// </remarks>
        /// <param name="Move">Value that specifies TRUE if the pointer is to move past the content to the left (right respectively), or FALSE otherwise. If TRUE, the pointer will move either to the other side of the tag to its left, or up to the number of characters specified by parameter tryChars, depending on the <see cref="ContextType"/> to the pointer's left (right respectively).</param>
        /// <returns>Returns a structure with basic pointer informations.</returns>
        PointerInformation GetPointerInformation(bool Move);

        /// <summary>
        /// Returns a collection of information about both pointers.
        /// </summary>
        /// <remarks>
        /// <seealso cref="GetPointerInformation(bool)"/>
        /// </remarks>
        /// <param name="Move">Value that specifies TRUE if the pointer is to move past the content to the left (right respectively), or FALSE otherwise. If TRUE, the pointer will move either to the other side of the tag to its left, or up to the number of characters specified by parameter tryChars, depending on the <see cref="ContextType"/> to the pointer's left (right respectively).</param>
        /// <param name="tryChars">Variable that specifies the number of characters to retrieve to XXXRetrieveChars property, if <see cref="ContextType"/> is <see cref="ContextType.Text"/>, 
        /// and receives the actual number of characters the method was able to retrieve. It can be set to -1, 
        /// indicating that the method should retrieve an arbitrary amount of text, up to the next no-scope element 
        /// or element scope transition.</param>
        /// <returns>Returns a structure with basic pointer informations.</returns>
        PointerInformation GetPointerInformation(bool Move, int tryChars);

        /// <summary>
        /// Retrieves the position of a markup pointer. 
        /// </summary>
        /// <remarks>As long as the pointer's positioned, the method can be used to retrieve the carets position at line.</remarks>
        /// <returns>Returns markup pointer's position</returns>
        int GetMarkupPosition(PointerSelection pointer);

        /// <summary>
        /// Determines if the markup pointer is positioned at a word break.
        /// </summary>
        /// <param name="pointer">If <c>False</c> the first Pointer is checked, the second otherwise.</param>
        bool IsAtWordBreak(PointerSelection pointer);

        /// <summary>
        /// Determines if a markup pointer is located inside of, at the beginning of, or at the end of text that is formatted as a URL. 
        /// </summary>
        bool IsInsideURL(PointerSelection pointer);

        /// <summary>
        /// Moves a markup pointer to a particular element in a markup container.
        /// </summary>
        void MoveToElementContent(PointerSelection pointer, GuruComponents.Netrix.WebEditing.Elements.IElement element, bool atStart);

        /// <summary>
        /// Moves a markup pointer to a specified location in a specified direction, but not past the other markup pointer. 
        /// </summary>
        /// <remarks>
        /// This method can simplify the usage of pointers, because it avoids an frequently mistake which results
        /// in some loss of functionality; the positioning of the second pointer before the first one.  
        /// </remarks>
        void MoveUnitBounded(PointerSelection pointer, MoveUnit mu);

        /// <summary>
        /// Set the default highlight style.
        /// </summary>
        /// <remarks>
        /// This methiod pre-sets the highlight style. The current selection will not be changeds.
        /// </remarks>
        /// <param name="HighLightStyle">The highlight style object which is used from now on.</param>
        void SetHighLightStyle(IHighLightStyle HighLightStyle);

        /// <summary>
        /// Highlights the current range.
        /// </summary>
        /// <remarks>
        /// This method cannot work if no text fragment was selected before call. The highlight style should be set
        /// before first call.
        /// </remarks>
        bool HighLightRange();

        /// <summary>
        /// This method deletes the content of the current base range.
        /// </summary>
        void DeleteRangeText();

        /// <summary>
        /// Reset the range pointer to the beginning of the document. 
        /// </summary>
        void ResetRangePointers();

        /// <summary>
        /// Returns the last added highlight segment.
        /// </summary>
        /// <remarks>
        /// This segment contains a method which can be used to remove the segment later.
        /// </remarks>
        IHighLightSegment LastSegment { get; }

        /// <summary>
        /// Remove all highlighted fragments.
        /// </summary>
        void RemoveHighLighting();

        /// <overloads>This method has two overloads.</overloads>
        /// <summary>
        /// Select the current range between two pointers.
        /// </summary>
        /// <remarks>
        /// This method does not scroll into view if the fragment selected was outside the designer surface.
        /// </remarks>
        void SelectPointerRange();

        /// <summary>
        /// Select the current range between two pointers.
        /// </summary>
        /// <remarks>
        /// Assure to set the text pointers before calling this method.
        /// </remarks>
        /// <param name="ScrollIntoView">If set to <c>true</c> the control tries to scroll the selected range into the visible area.</param>
        void SelectPointerRange(bool ScrollIntoView);

        /// <overloads>This method has two overloads.</overloads>
        /// <summary>
        /// Selects a previously selected and stored fragment.
        /// </summary>
        /// <remarks>
        /// This method does not scroll into view if the fragment selected was outside the designer surface.
        /// </remarks>
        void SelectRange(string key);

        /// <summary>
        /// Selects a previously selected and stored fragment.
        /// </summary>
        /// <param name="key">The name of the stored fragment.</param>
        /// <param name="ScrollIntoView">If set to <c>true</c> the control tries to scroll the selected range into the visible area.</param>
        void SelectRange(string key, bool ScrollIntoView);

        /// <summary>
        /// Returns the text between two valid pointers.
        /// </summary>
        /// <returns>The text between the pointers. HTML tags are stripped out.</returns>
        string GetTextBetweenPointers();

        /// <summary>
        /// Returns the HTML between two valid pointers.
        /// </summary>
        /// <returns>The text between the pointers. HTML tags are preserved.</returns>
        string GetHtmlBetweenPointers();

        /// <summary>
        /// Set (overwrite) all text between current pointers.
        /// </summary>
        /// <param name="text">Text to be written.</param>
        void SetTextBetweenPointers(string text);
        /// <summary>
        /// Set (overwrite) all text between current pointers with HTML.
        /// </summary>
        /// <param name="html">Html to be written.</param>
        void SetHtmlBetweenPointers(string html);

        /// <summary>
        /// Deletes the text between the pointers. 
        /// </summary>
        /// <remarks>If the selection is fragmented HTML a valid structure will be rebuild.</remarks>
        void DeleteTextBetweenPointers();

        /// <summary>
        /// Searches a text between the pointers.
        /// </summary>
        /// <remarks>
        /// This is a nice enhancement of the regular find function to narrow a search. It searches the whole document,
        /// but starts searching using the current pointer position. So in case you want to restart search from the
        /// beginning, just set the pointers back by calling <see cref="MovePointersToElement"/>, <see cref="MoveFirstPointer"/>,
        /// or <see cref="MoveSecondPointer"/>.
        /// </remarks>
        /// <param name="searchString">A string to search for.</param>
        /// <param name="upWards">Search upwards.</param>
        /// <param name="matchCase">Must be set to <c>true</c> if match case is required.</param>
        /// <param name="wholeWord">Must set to <c>true</c> if only whole words should be found.</param>
        /// <returns>Returns <c>true</c> if something was found, <c>false</c> otherwise. Also returns <c>false</c> in case document ends.</returns>
        bool FindTextBetweenPointers(string searchString, bool upWards, bool matchCase, bool wholeWord);

        /// <summary>
        /// Call this to reset the pointers to catch the whole document.
        /// </summary>
        void ResetFindWordPointer();

        /// <summary>
        /// Move the first pointer.
        /// </summary>
        /// <remarks>
        /// The pointers can be used to create a range. The range can be moved too, using the
        /// <see cref="MoveCurrentRange">MoveCurrentRange</see>
        /// method.
        /// </remarks>
        /// <param name="Move">The first pointer will be moved to the given destination.</param>
        void MoveFirstPointer(MoveTextPointer Move);

        /// <summary>
        /// Move the pointers to the caret to start next positioning around the caret.
        /// </summary>
        /// <remarks>
        /// It is recommended to set the pointers actively before any caret related move operation starts.
        /// </remarks>
        void MovePointersToCaret();

        /// <summary>
        /// Move the second pointer.
        /// </summary>
        /// <remarks>
        /// The pointers can be used to create a range. The range can be moved too, using the
        /// <see cref="MoveCurrentRange">MoveCurrentRange</see>
        /// method.
        /// </remarks>
        /// <param name="Move">The second pointer will be moved to the given destination.</param>
        void MoveSecondPointer(MoveTextPointer Move);

        /// <summary>
        /// Move the current range completely.
        /// </summary>
        /// <param name="Move"></param>
        /// <param name="Count"></param>
        int MoveCurrentRange(MoveUnit Move, int Count);

        /// <summary>
        /// Moves the current range start point.
        /// </summary>
        /// <remarks>
        /// This method may fail if the start point is beyond the end point.
        /// </remarks>
        /// <param name="Move"></param>
        /// <param name="Count"></param>
        int MoveCurrentRangeStart(MoveUnit Move, int Count);

        /// <summary>
        /// Moves the current range end point.
        /// </summary>
        /// <remarks>
        /// This method may fail if the end point is before the start point.
        /// </remarks>
        int MoveCurrentRangeEnd(MoveUnit Move, int Count);

        /// <summary>
        /// Stores the current range for later access.
        /// </summary>
        /// <remarks>
        /// This method can be used with the highlighting feature to highlight a prepared range later.
        /// </remarks>
        /// <param name="key"></param>
        void SaveCurrentRange(string key);

        /// <summary>
        /// Clears all stored ranges.
        /// </summary>
        /// <remarks>
        /// This removes highlighting and stored range pointers, but preserves the text selected with the range.
        /// </remarks>
        void ClearRangeStore();

        /// <summary>
        /// This method checks if a stored range is inside the current range.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsInCurrentRange(string key);

        /// <summary>
        /// This method checks if a stored range equals the current range.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool EqualsCurrentRange(string key);

        /// <summary>
        /// This method compares the endpoints.
        /// </summary>
        /// <param name="compare"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        int CompareEndPoints(CompareUnit compare, string key);

        /// <summary>
        /// Causes the object to scroll into view, aligning either to top or bottom of the window.
        /// </summary>
        /// <remarks>
        /// Depending on the size of the given range the control may not be able to scroll exactly to the very top or 
        /// very bottom of the window, but will position the object as close to the requested position as possible.
        /// </remarks>
        /// <param name="atStart"></param>
        void ScrollIntoView(bool atStart);

        /// <summary>
        /// Moves the caret (insertion point) to the beginning or end of the current range.
        /// </summary>
        /// <param name="atStart"></param>
        void MoveCaretToPointer(bool atStart);

        /// <summary>
        /// Expand the current range to that partial units are completely contained.
        /// </summary>
        /// <param name="ExpandTo"></param>
        /// <returns></returns>
        bool ExpandRange(MoveUnit ExpandTo);

        /// <summary>
        /// Selects the whole text of the given element.
        /// </summary>
        /// <remarks>
        /// This is a mixed mode command, uses internally Pointers and Ranges. The command will move both 
        /// pointers to the elements boundaries. Then the current base range is set to these pointers and finally
        /// used to set the elements content. After all the changed content is selected and scrolled into view.
        /// </remarks>
        /// <param name="element">Any element object that implements <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement">IElement</see> and that does contain selectable text.</param>
        void SelectElementText(GuruComponents.Netrix.WebEditing.Elements.IElement element);

        /// <summary>
        /// Set the pointers to the inner border of the tag.
        /// </summary>
        /// <param name="element">The tag from which the content should be highlighted. </param>
        /// <param name="withRange">Whether or not the current base range should follow the pointers immediataly.</param>
        void MovePointersToElement(GuruComponents.Netrix.WebEditing.Elements.IElement element, bool withRange);
        /// <summary>
        /// Move the range to current pointers.
        /// </summary>
        void MoveRangeToPointers();
        /// <summary>
        /// Returns the text available in the current range.
        /// </summary>
        /// <returns></returns>
        string GetTextInRange();
        /// <summary>
        /// Returns the HTML available in the current range.
        /// </summary>
        /// <returns></returns>
        string GetHtmlInRange();
        /// <summary>
        /// Overwrite all text in range.
        /// </summary>
        /// <param name="text"></param>
        void SetTextInRange(string text);
        /// <summary>
        /// Return text of a stored range.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetTextInStoredRange(string key);
        /// <summary>
        /// Return HTML of the stored range.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetHtmlInStoredRange(string key);

        /// <summary>
        /// Select the current range.
        /// </summary>
        void SelectRange();

        /// <summary>
        /// Paste the HTML text into the element.
        /// </summary>
        /// <remarks>
        /// Element must be of type container. The HTML must be valid after pasting.
        /// </remarks>
        /// <seealso cref="Paste(IElement)"/>
        /// <param name="element">The element in which the HTML has to be pasted.</param>
        /// <param name="html">Any valid HTML text.</param>
        void PasteHtml(IElement element, string html);

        /// <summary>
        /// Paste the Clipboard text into the element.
        /// </summary>
        /// <remarks>
        /// Element must be of type container. The HTML must be valid after pasting.
        /// </remarks>
        /// <seealso cref="PasteHtml"/>
        /// <exception cref="System.ArgumentException">Thrown if the clipboard has no valid data. Expected formats are HTML and Text.</exception>
        /// <param name="element">The element in which the HTML has to be pasted.</param>
        void Paste(IElement element);

        /// <summary>
        /// Paste the current content of clipboard at current caret position and let pointers and range follow the insertion point.
        /// </summary>
        void Paste();

    }
}