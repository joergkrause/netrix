using System;

namespace GuruComponents.Netrix.WebEditing.DragDrop
{
	/// <summary>
	/// All possible commands that can be send as drop event data.
	/// </summary>
	public enum DragDropCommands
	{
        /// <summary>
        /// Presents the move icon if action is not allowed.
        /// </summary>
        DefaultNot     = -1,
        /// <summary>
        /// Presents the move icon if no element was detected and Move operation is running.
        /// </summary>
        DefaultMove     = 0,
        /// <summary>
        /// Presents the copy icon if no element was detected and Copy operation is running.
        /// </summary>
        DefaultCopy     = 1,
        /// <summary>
        /// The attached data represents an anchor.
        /// </summary>
		Anchor          = 101,
        /// <summary>
        /// The attached data represents a break.
        /// </summary>
        Break           = 102,
        /// <summary>
        /// The attached data represents a button.
        /// </summary>
        Button          = 103,
        /// <summary>
        /// The attached data represents a div element.
        /// </summary>
        Div             = 104,
        /// <summary>
        /// The attached data represents a form element.
        /// </summary>
        Form            = 105,
        /// <summary>
        /// The attached data represents a horizontal rule.
        /// </summary>
        HorizontalRule  = 106,
        /// <summary>
        /// The attached data represents a textbox.
        /// </summary>
        Textbox         = 107,
        /// <summary>
        /// The attached data represents a textarea.
        /// </summary>
        TextArea        = 108,
        /// <summary>
        /// The attached data represents a checkbox.
        /// </summary>
        Checkbox        = 109,
        /// <summary>
        /// The attached data represents a radiobutton.
        /// </summary>
        RadioButton     = 110,
        /// <summary>
        /// The attached data represents an image button.
        /// </summary>
        ImageButton     = 111,
        /// <summary>
        /// The attached data represents a file button.
        /// </summary>
        FileButton      = 112,
        /// <summary>
        /// The attached data represents a password field.
        /// </summary>
        Password        = 113,
        /// <summary>
        /// The attached data represents a reset button.
        /// </summary>
        ResetButton     = 114,
        /// <summary>
        /// The attached data represents a submit button.
        /// </summary>
        SubmitButton    = 115,
        /// <summary>
        /// The attached data represents a listbox.
        /// </summary>
        ListBox         = 116,
        /// <summary>
        /// The attached data represents a dropdown field.
        /// </summary>
        DropDown        = 117,
        /// <summary>
        /// The attached data represents a paragraph tag.
        /// </summary>
        Paragraph       = 118,
        /// <summary>
        /// The attached data represents an image tag.
        /// </summary>
        Image           = 119,
        /// <summary>
        /// The attached data represents a table.
        /// </summary>
        Table           = 120,
        /// <summary>
        /// The attached data represents a hidden field.
        /// </summary>
        HiddenField     = 121,
        /// <summary>
        /// The attached data represents a Span tag.
        /// </summary>
        Span            = 122
	}
}
