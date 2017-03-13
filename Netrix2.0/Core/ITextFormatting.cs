using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix
{

    /// <summary>
    /// IHtmlEditor.TextFormatting implements ITextFormatting.
    /// </summary>
    /// <remarks>
    /// The implementing class provides methods to format text selections or text blocks.
    /// <para>
    /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
    /// </para>
    /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
    /// </remarks>
    public interface ITextFormatting 
    {
        /// <summary>
        /// Selects the whole text in the control, normally linked with Ctrl-A key.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        void SelectAll();

        /// <summary>
        /// Removes the selection the user has made but does not delete the content
        /// </summary>
        /// <remarks>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        void ClearSelection();

        /// <summary>
        /// Removes the current inline formatting with &lt;font&gt; tag.
        /// </summary>
        /// <example>
        /// The following code assumes menu item to RemoveInlineFormat and its click event 
        /// is handle like this:
        /// <code>
        /// private void mnuRemoveInlineFormat_Click(object sender, System.EventArgs e)
        /// {
        ///		htmlEditor2.TextFormatting.RemoveInlineFormat();
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="RemoveParagraphFormat"/>
        /// <seealso cref="SetHtmlFormat"/>
        void RemoveInlineFormat();

        /// <summary>
        /// Removes the current paragraph formatting by any kind of block element.
        /// </summary>
        /// <remarks>
        /// This is a shortcut for <c>SetHtmlFormat(HtmlFormat.Normal)</c>.
        /// </remarks>
        /// <example>
        /// The following code assumes menu item to RemoveInlineFormat and its click event 
        /// is handle like this:
        /// <code>
        /// private void mnuRemoveParagraphFormat_Click(object sender, System.EventArgs e)
        /// {
        ///		htmlEditor2.TextFormatting.RemoveParagraphFormat();
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="RemoveInlineFormat"/>
        /// <seealso cref="SetHtmlFormat"/>
        void RemoveParagraphFormat();


		/// <summary>
        /// Indicates if the current text can be indented
        /// </summary>
        /// <remarks>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns>Returns a boolean value indicating the state.</returns>
        bool CanIndent { get; }

        /// <summary>
        /// Indicates if the background color can be set
        /// </summary>
        /// <remarks>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns>Returns a boolean value indicating the state.</returns>
        bool CanSetBackColor { get; }

        /// <summary>
        /// Indicates if the font face can be set.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns>Returns a boolean value indicating the state.</returns>
        bool CanSetFontName { get; }

        /// <summary>
        /// Indicates if the font size can get set.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns>Returns a boolean value indicating the state.</returns>
        bool CanSetFontSize { get; }

        /// <summary>
        /// Indicates if the foreground color can be set.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns>Returns a boolean value indicating the state.</returns>
        bool CanSetForeColor { get; }

        /// <summary>
        /// Indicates if the Html format (eg ordered lists, paragraph, heading) can be set.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns>Returns a boolean value indicating the state.</returns>
        bool CanSetHtmlFormat { get; }

        /// <summary>
        /// Indicates if the current text can be unindented.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns>Returns a boolean value indicating the state.</returns>
        bool CanUnindent { get; }

        /// <summary>
		/// Gets and sets the font name of the current text.
		/// </summary>
		/// <remarks>
		/// This property cannot be applied if no text is selected. 
		/// Any call to the set path will remove empty attributes, if the current element is a font element and
		/// the attribute is empty. If no more attributes left the font tag is removed.  
		/// <para>
		/// The intention if this property is NOT to change the font face of any font attribute which is valid in
		/// the parent hierarchy. This means, that only an active selection with one or more characters will
		/// encapsulated with the font tag. To get the current element just use <c>Selection.Element</c>
		/// or get the current element from HtmlElementChanged event. If the current element is the font
		/// element the property <c>face</c> can be used to set or retrieve the current font face value.
		/// </para>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        string FontName { get; set; }

        /// <summary>
		/// Gets and sets the font size of the current text.
		/// </summary>
		/// <remarks>
		/// This property cannot be applied if no text is selected. 
		/// Any call to the set path will remove empty attributes, if the current element is a font element and
		/// the attribute is empty. If no more attributes left the font tag is removed.  
		/// <para>
		/// The intention if this property is NOT to change the font size of any font attribute which is valid in
		/// the parent hierarchy. This means, that only an active selection with one or more characters will
		/// encapsulated with the font tag. To get the current element just use <c>Selection.Element</c>
		/// or get the current element from HtmlElementChanged event. If the current element is the font
		/// element the property <c>size</c> can be used to set or retrieve the current font size value.
		/// </para>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        [Obsolete("Use CssFontSize from class CssTextFormatting if possible.")]
        GuruComponents.Netrix.WebEditing.FontUnit FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font size using a span tag and local CSS style 'font-size'.
        /// </summary>
        /// <remarks>
        /// Use as a replacement for <see cref="FontSize"/>.
        /// </remarks>
        System.Web.UI.WebControls.Unit CssFontSize { get; set; }

        /// <summary>
        /// The foreground color of the current text.
        /// </summary>
        /// <remarks>
        /// This property cannot be applied if no text is selected. 
        /// Any call to the set path will remove empty attributes, if the current element is a font element and
        /// the attribute is empty. If no more attributes left the font tag is removed.  
        /// <para>
        /// The intention if this property is NOT to change the color of any font attribute which is valid in
        /// the parent hierarchy. This means, that only an active selection with one or more characters will
        /// encapsulated with the font tag. To get the current element just use <c>Selection.Element</c>
        /// or get the current element from HtmlElementChanged event. If the current element is the font
        /// element the property color can be used to set or retrieve the current color value.
        /// </para>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        Color ForeColor { get; set; }

		/// <summary>
		/// Sets the background of the current text selection to the given color. </summary>
		/// <remarks>This property cannot be 
		/// applied if no text is selected. 
		/// Any call to the set path will remove empty attributes, if the current element is a font element and
		/// the attribute is empty. If no more attributes left the font tag is removed.  
		/// <para>
		/// The intention if this property is NOT to change the font face of any font attribute which is valid in
		/// the parent hierarchy. This means, that only an active selection with one or more characters will
		/// encapsulated with the font tag. To get the current element just use <c>Selection.Element</c>
		/// or get the current element from HtmlElementChanged event. If the current element is the font
		/// element the style attribute <c>background-color</c> can be used.
		/// </para>
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        Color BackColor { get; set; }

        /// <summary>
        /// Gets the current state of the bold command (enabled and/or checked).
        /// </summary>
        /// <remarks>This applies always to the whole paragraph.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns></returns>
        HtmlCommandInfo GetBoldInfo();

        /// <summary>
        /// Retrieves the current block format, e.g. "Header 1", "Unordered List", etc. and returns
        /// it in the internal enum format to stay language independent.
        /// </summary>
        /// <remarks>This applies always to the whole paragraph.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns><see cref="GuruComponents.Netrix.HtmlFormat">HtmlFormat</see> enumeration</returns>
        HtmlFormat GetHtmlFormat(); 

        /// <summary>
        /// Gets the current state of the italics command (enabled and/or checked).</summary>
        /// <remarks>This method can only
        /// detect the state if some text is selected. 
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns></returns>
        HtmlCommandInfo GetItalicsInfo();

        /// <summary>
		/// Gets the current state of the strikethrough command (enabled and/or checked).
		/// </summary>
		/// <remarks>
		/// This method can only detect the state if some text is selected. 
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns></returns>
        HtmlCommandInfo GetStrikethroughInfo(); 

        /// <summary>
		/// Gets the current state of the Subscript command (enabled and/or checked).</summary>
		/// <remarks>
		/// This method can only detect the state if some text is selected. 
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns></returns>
        HtmlCommandInfo GetSubscriptInfo(); 

        /// <summary>
		/// Gets the current state of the Superscript command (enabled and/or checked).</summary>
		/// <remarks>
		/// This method can only
		/// detect the state if some text is selected. 
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns></returns>
        HtmlCommandInfo GetSuperscriptInfo();

        /// <summary>
		/// Gets the current state of the Underline command (enabled and/or checked).</summary>
		/// <remarks>
		/// This method can only
		/// detect the state if some text is selected. 
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <returns></returns>
        HtmlCommandInfo GetUnderlineInfo();

        /// <summary>
        /// Sets the HTML format (eg ordered list, paragraph, etc.) of the current text.
        /// </summary>
        /// <remarks>This applies always to the whole paragraph.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <param name="format">The format that is applied to the paragraph.</param>
        void SetHtmlFormat(HtmlFormat format);

        /// <summary>
        /// Set the whole document to LTR (left to right) or RTL (right to left) mode.
        /// </summary>
        /// <remarks>
        /// RTL is used for languages primarily running from right to left, like arabic,
        /// whereas LTR is used for western languages.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <param name="rtl">True set RTL, false LTR. Both cases set the attribute.</param>
        void DirectionRtlDocument(bool rtl);

		/// <summary>
        /// Set a block (paragraph, header) to LTR (left to right) or RTL (right to left) mode.
        /// </summary>
        /// <remarks>
        /// RTL is used for languages primarily running from right to left, like arabic,
        /// whereas LTR is used for western languages.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <param name="rtl">True set RTL, false LTR. Both cases set the attribute.</param>
        void DirectionRtlBlock(bool rtl);

		/// <summary>
        /// Set an inline element to LTR (left to right) or RTL (right to left) mode.
        /// </summary>
        /// <remarks>
        /// RTL is used for languages primarily running from right to left, like arabic,
        /// whereas LTR is used for western languages.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <param name="rtl">True set RTL, false LTR. Both cases set the attribute.</param>
        void DirectionRtlInline(bool rtl);

		/// <summary>
        /// Align the current block selection or current block.
        /// </summary>
        /// <remarks>This applies always to the whole paragraph.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        /// <param name="alignment"></param>
        void SetAlignment(Alignment alignment);


        /// <summary>
        /// Get the current Alignment of the paragraph where the caret resides.
        /// </summary>
        /// <returns>A value of the Alignment enumeration.</returns>
        Alignment GetAlignment();

        /// <summary>
        /// Toggles the current selection with the bold formatting (STRONG tag).
        /// </summary>
        /// <remarks>This applies always to the whole paragraph.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        void ToggleBold(); 

        /// <summary>
		/// Toggles the current selection with the italic formatting (EM tag).
        /// </summary>
        /// <remarks>This applies always to the whole paragraph.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        void ToggleItalics(); 

        /// <summary>
		/// Toggles the current selection with the strikethrough formatting (STRIKE tag).
        /// </summary>
        /// <remarks>This applies always to the whole paragraph.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        void ToggleStrikethrough();

        /// <summary>
		/// Toggles the current selection with the subscript formatting (SUB tag).
        /// </summary>
        /// <remarks>This applies always to the whole paragraph.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        void ToggleSubscript();

        /// <summary>
		/// Toggles the current selection with the superscript formatting (SUP tag).
        /// </summary>
        /// <remarks>This applies always to the whole paragraph.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        void ToggleSuperscript();

        /// <summary>
		/// Toggles the current selection with the underline formatting (U tag).
        /// </summary>
        /// <remarks>This applies always to the whole paragraph.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        void ToggleUnderline();

		/// <summary>
		/// Indents the current text.
		/// </summary>
		/// <remarks>This applies always to the whole paragraph.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        void Indent();

        /// <summary>
        /// Unindents the current text.</summary>
        /// <remarks>
        /// This applies always to the whole paragraph. If there is no outdent
        /// level is available the command will be ignored.
        /// <para>
        /// For examples see the corresponding <see cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting">IHtmlEditor.TextFormatting</see> class.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.IHtmlEditor.TextFormatting"/>
        /// </remarks>
        void UnIndent();

    }
}