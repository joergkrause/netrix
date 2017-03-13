using System;
using System.Drawing;
using System.Web;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
	/// IStyle gives interactive access to each property of a style rule.
	/// </summary>
	/// <remarks>
	/// CssStyle class implements IStyle interface.
	/// A style definition is either an inline style (attribute style of any stylable element) or the
	/// definition part of a style rule. A rule is a combination of a descriptor and a style.
	/// <para>
	/// The purpose of this class is to have an isolated store for CSS definitions. This behaves slightly
	/// different from the styles defined as part of the elements. The native class CssStyle allows access
	/// to style definitions using the <see cref="System.Windows.Forms.PropertyGrid">PropertyGrid</see> and
	/// not the NetRix StyleEditor. 
	/// </para>
	/// <para>
	/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
	/// class for more details.
	/// </para>
	/// </remarks>
	/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
	public interface IElementStyle
	{
        /// <summary>
        /// Access the background styles.
        /// </summary>
        IBackground BackgroundStyle { get; }
        /// <summary>
        /// Access the border styles.
        /// </summary>
        IBorder BorderStyle { get; }
        /// <summary>
        /// Access the font styles.
        /// </summary>
        IFont FontStyle { get; }
        /// <summary>
        /// Access the margin styles.
        /// </summary>
        IMargin MarginStyle { get; }
        /// <summary>
        /// Access the padding styles.
        /// </summary>
        IPadding PaddingStyle { get; }
        /// <summary>
        /// Access the text decoration styles.
        /// </summary>
        ITextDecoration TextDecorationStyle { get; }

		/// <summary>
        /// Sets or retrieves whether the object allows floating objects on its left side, right side, or both, so that the next text displays past the floating objects. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		ClearStyles clear{ get; set;}  

        /// <summary>
        /// Sets or retrieves the color of the text of the object.  
        /// </summary>
		/// <remarks>
		/// Some browsers do not recognize color names, but all browsers should 
		/// recognize RGB color values and display them correctly.
		/// Therefore the property returns always RGB values, even if the value 
		/// was previously set using names. This is a different behavior compared to the original MSHTML.
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </para>
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		Color color{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the persisted representation of the style rule. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string cssText{ get; set;}  
        
        /// <summary>
        /// Sets or retrieves the type of cursor to display as the mouse pointer moves over the object.  
        /// </summary>
        /// <remarks>
        /// <list type="table">
        /// <listheader><item>Cursor String</item><item>Definition</item></listheader>
        /// <item>all-scroll</item><item> Internet Explorer 6 and later. Arrows pointing up, down, left, and right with a dot in the middle, indicating that the page can be scrolled in any direction. </item>
        /// <item>auto</item><item>Default. Browser determines which cursor to display based on the current context. </item>
        /// <item>col-resize</item><item>Internet Explorer 6 and later. Arrows pointing left and right with a vertical bar separating them, indicating that the item/column can be resized horizontally. </item>
        /// <item>crosshair</item><item>Simple cross hair. </item>
        /// <item>default</item><item>Platform-dependent default cursor; usually an arrow. </item>
        /// <item>hand</item><item>Hand with the first finger pointing up, as when the user moves the pointer over a link. </item>
        /// <item>help</item><item>Arrow with question mark, indicating help is available. </item>
        /// <item>move</item><item>Crossed arrows, indicating something is to be moved.           </item>
        /// <item>no-drop</item><item>Internet Explorer 6 and later. Hand with a small circle with a line through it, indicating that the dragged item cannot be dropped at the current cursor location. </item>
        /// <item>not-allowed</item><item>Internet Explorer 6 and later. Circle with a line through it, indicating that the requested action will not be carried out. </item>
        /// <item>pointer</item><item>Internet Explorer 6 and later. Hand with the first finger pointing up, as when the user moves the pointer over a link. Identical to hand. </item>
        /// <item>progress</item><item>Internet Explorer 6 and later. Arrow with an hourglass next to it, indicating that a process is running in the background. User interaction with the page is unaffected. </item>
        /// <item>row-resize</item><item>Internet Explorer 6 and later. Arrows pointing up and down with a horizontal bar separating them, indicating that the item/row can be resized vertically. </item>
        /// <item>text</item><item>Editable text; usually an I-bar. </item>
        /// <item>url(uri)</item><item>Internet Explorer 6 and later. Cursor is defined by the author, using a custom Uniform Resource Identifier (URI), such as url('mycursor.cur'). Cursors of type .CUR and .ANI are the only supported cursor types. </item>
        /// <item>vertical-text</item><item>Internet Explorer 6 and later. Editable vertical text, indicated by a horizontal I-bar. </item>
        /// <item>wait</item><item>Hourglass or watch, indicating that the program is busy and the user should wait. </item>
        /// <item>*-resize</item><item>Arrows, indicating an edge is to be moved; the asterisk (*) can be N, NE, NW, S, SE, SW, E, or W—each representing a compass direction. </item>
        /// </list>
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </para>
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string cursor{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves whether the object is rendered. 
        /// </summary>
		/// <remarks>
		/// In Internet Explorer 4.0, the block, inline, and list-item values are not supported explicitly, but do render the element. 
		/// <para>The block and inline values are supported explicitly as of Internet Explorer 5.</para>
		/// <para>In Internet Explorer 5.5 and earlier, the default value of this property for LI elements is block.</para>
		/// <para>The inline-block value is supported as of Internet Explorer 5.5. You can use this value to give an object a layout without specifying the object's height or width.</para>
		/// <para>All visible HTML objects are block or inline. For example, a div object is a block element, and a span object is an inline element. Block elements typically start a new line and can contain other block elements and inline elements. Inline elements do not typically start a new line and can contain other inline elements or data. Changing the values for the display property affects the layout of the surrounding content by:
		/// <list type="bullet">
		/// <item>Adding a new line after the element with the value block.</item> 
		/// <item>Removing a line from the element with the value inline. </item>
		/// <item>Hiding the data for the element with the value none. </item>
		/// </list>
		/// </para>
		/// <para>
		/// In contrast to the visibility property, display = none reserves no space for the object on the screen.
		/// </para>
		/// <para>
		/// The table-header-group and table-footer-group values can be used to specify that the contents of the tHead and tFoot objects are displayed on every page for a table that spans multiple pages.
		/// </para>
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </para>
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		DisplayStyles display{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the filter or collection of filters applied to the object.  
        /// </summary>
		/// <remarks>
		/// <para>An object must have layout for the filter to render. A simple way to 
		/// accomplish this is to give the element a specified height and/or width. 
		/// However, there are several other properties that can give an element layout. 
		/// For more information on these other properties, see the hasLayout property.</para>
		/// <para>The shadow filter can be applied to the img object by setting the filter 
		/// on the image's parent container.</para>
		/// <para>The filter mechanism is extensible and enables you to develop and add 
		/// additional filters later. For more information about filters, see 
		/// <see href="http://msdn.microsoft.com/workshop/author/filter/filters.asp">Introduction to Filters and Transitions</see> 
		/// on MSDN (online connection required).</para>
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </para>
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string filter{ get; set;}
        
        
		/// <summary>
        /// Sets or retrieves the height of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.Unit height{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the position of the object relative to the left edge of the next 
        /// positioned object in the document hierarchy.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.Unit left{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the amount of additional space between letters in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.Unit letterSpacing{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the amount of additional space between letters in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		bool letterSpacingNormal{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the distance between lines in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.Unit lineHeight{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves up to three separate IHTMLStyle::listStyle properties of the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string listStyle{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a value that indicates which image to use as a list-item marker for the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string listStyleImage{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a variable that indicates how the list-item marker is drawn relative to the content of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		ListStylePosition listStylePosition{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the predefined type of the line-item marker for the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		ListStyleType listStyleType{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a value indicating how to manage the content of the object 
        /// when the content exceeds the height or width of the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string overflow{ get; set;}  
        
        
		/// <summary>
        /// Sets or retrieves a value indicating whether a page break occurs after the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		PageBreakStyles pageBreakAfter{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a string indicating whether a page break occurs before the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		PageBreakStyles pageBreakBefore{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the bottom position of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		int pixelBottom{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the right position of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		int pixelRight{ get; set;}  
        
		/// <summary>
        /// Retrieves the type of positioning used for the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string position { get;}
        
		/// <summary>
        /// Sets or retrieves the bottom position of the object in the units specified by the IHTMLStyle::left attribute.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.Unit posBottom{ get; set;} 
        
		/// <summary>
        /// Sets or retrieves the right position of the object in the units specified by the IHTMLStyle::top attribute.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.Unit posRight{ get; set;}  
        
		/// <summary>
        /// Removes the given attribute from the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		/// <param name="attribute">Specifies the attribute name.</param>
        void RemoveAttribute(string attribute);

        /// <summary>
        /// Get the given attribute from the object. 
        /// </summary>
        /// <remarks>
        /// The caller is supposed to convert to appropriate type.
        /// </remarks>
        /// <param name="attribute">Specifies the attribute name.</param>
        object GetAttribute(string attribute);

		/// <summary>
        /// Sets the value of the specified attribute. 
        /// </summary>
		/// <remarks>
		/// If the attribute not exists the value wil be created and placed at the first 
		/// position in the collection.
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </para>
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		/// <param name="attribute">Specifies the name of the attribute.</param>
        /// <param name="value">Variant that specifies the string, number, or Boolean to assign to the attribute. </param>
        void SetAttribute(string attribute, object value);
        
		/// <summary>
        /// Sets or retrieves on which side of the object the text will flow. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string styleFloat{ get; set;} 
        
		/// <summary>
        /// Sets or retrieves whether the text in the object is left-aligned, 
        /// right-aligned, centered, or justified.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.HorizontalAlign textAlign{ get; set;}  
       		 
        
		/// <summary>
        /// Sets or retrieves the indentation of the first line of text in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.Unit textIndent{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the rendering of the text in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string textTransform{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the position of the object relative to the top of the 
        /// next positioned object in the document hierarchy.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.Unit top{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the vertical alignment of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.VerticalAlign verticalAlign{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves whether the content of the object is displayed.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string visibility{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a value that indicates whether lines are automatically broken inside the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string whiteSpace{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.Unit width{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the amount of additional space between words in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		System.Web.UI.WebControls.Unit wordSpacing{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the amount of additional space between words in the object to the default value.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		bool wordSpacingNormal{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the stacking order of positioned objects as integer value.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		int zIndex{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the stacking order of positioned objects as auto value.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		bool zIndexAuto{ get; }  

        /// <summary>
        /// Sets or retrieves the documents zoom level.  
        /// </summary>
        string Zoom
        {
            get;
            set;
        }

	}
}
