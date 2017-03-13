using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
    /// Controls how the object is rendered.
	/// </summary>
	/// <remarks>
    /// In Internet Explorer 4.0, the block, inline, and list-item values are not supported explicitly, but do render the element. 
    /// <para>The block and inline values are supported explicitly as of Internet Explorer 5.</para>
    /// <para>In Internet Explorer 5.5 and earlier, the default value of this property for li elements is block.</para>
    /// <para>The inline-block value is supported as of Internet Explorer 5.5. You can use this value to give an object a layout without specifying the object's height or width.</para>
    /// <para>All visible HTML objects are block or inline. For example, a div object is a block element, and a span object is an inline element. Block elements typically start a new line and can contain other block elements and inline elements. Inline elements do not typically start a new line and can contain other inline elements or data. Changing the values for the IHTMLStyle::display property affects the layout of the surrounding content by: </para>
    /// <list type="bullet">
    ///     <item>Adding a new line after the element with the value block. </item>
    ///     <item>Removing a line from the element with the value inline.   </item>
    ///     <item>Hiding the data for the element with the value none.             </item>
    ///     </list>
    /// <para>In contrast to the IHTMLStyle::visibility property, IHTMLStyle::display = none reserves no space for the object on the screen.</para>
    /// <para>The table-header-group and table-footer-group values can be used to specify that the contents of the tHead and tFoot objects are displayed on every page for a table that spans multiple pages.</para>
	/// </remarks>
    public enum DisplayStyles
    {
        /// <summary>
        /// Object is rendered as a block element. 
        /// </summary>
        Block,
        /// <summary>
        /// Object is not rendered. 
        /// </summary>
        None ,
        /// <summary>
        /// Default. Object is rendered as an inline element sized by the dimensions of the content. 
        /// </summary>
        Inline, 
        /// <summary>
        /// Object is rendered inline, but the contents of the object are rendered as a block element. Adjacent inline elements are rendered on the same line, space permitting. 
        /// </summary>
        InlineBlock ,
        /// <summary>
        /// Internet Explorer 6 and later. Object is rendered as a block element, and a list-item marker is added. 
        /// </summary>
        ListItem,
        /// <summary>
        /// Table header is always displayed before all other rows and row groups, and after any top captions. The header is displayed on each page spanned by a table.  
        /// </summary>
        TableHeaderGroup,
        /// <summary>
        /// Table footer is always displayed after all other rows and row groups, and before any bottom captions. The footer is displayed on each page spanned by a table.  
        /// </summary>
        TableFooterGroup 

    }
}
