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
	/// 
	public interface IStyle
	{
        /// <summary>
        /// Sets or retrieves up to five separate background properties of the object. 
        /// </summary>
        /// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		BackgroundStyles background { get; set;} 
       
		/// <summary>
        /// Sets or retrieves how the background image is attached to the object within the document. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string backgroundAttachment { get; set;} 
        
		/// <summary>
        /// Sets or retrieves the color behind the content of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color backgroundColor { get; set;} 
        
		/// <summary>
        /// Sets or retrieves the background image of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string backgroundImage{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the position of the background of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit backgroundPositionUnit{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the position of the background of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.VerticalAlign backgroundPositionVerticalAlign{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the position of the background of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.HorizontalAlign backgroundPositionHorizontalAlign{ get; set;}

        /// <summary>
        /// Sets or retrieves the x-coordinate of the IHTMLStyle::backgroundPosition property.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit backgroundPositionX{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the y-coordinate of the IHTMLStyle::backgroundPosition property.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit backgroundPositionY{ get; set;}  

        /// <summary>
        /// Sets or retrieves how the backgroundImage property of the object is tiled. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		BackgroundRepeat backgroundRepeat{ get; set;}  

        /// <summary>
        /// Sets or retrieves the properties to draw around the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string border{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the properties of the bottom border of the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string borderBottom{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the color of the bottom border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color borderBottomColor{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the style of the bottom border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.BorderStyle borderBottomStyle{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the width of the bottom border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit borderBottomWidth{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the border color of the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color borderColor{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the properties of the left border of the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string borderLeft{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the color of the left border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color borderLeftColor{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the style of the left border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.BorderStyle borderLeftStyle{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the width of the left border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit borderLeftWidth{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the properties of the right border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string borderRight{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the color of the right border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color borderRightColor{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the style of the right border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.BorderStyle borderRightStyle{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the width of the right border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit borderRightWidth{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the style of the left, right, top, and bottom borders of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.BorderStyle borderStyle{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the properties of the top border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string borderTop{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the color of the top border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color borderTopColor{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the style of the top border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.BorderStyle borderTopStyle{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the width of the top border of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit borderTopWidth{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the width of the left, right, top, and bottom borders of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit borderWidth{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves whether the object allows floating objects on its left side, right side, or both, so that the next text displays past the floating objects. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		ClearStyles clear{ get; set;}  

        /// <summary>
        /// Sets or retrieves which part of a positioned object is visible.  
        /// </summary>
        /// <remarks>
		/// <para>
		/// This property defines the shape and size of the positioned object 
		/// that is visible. The Horizontalposition must be set to absolute. 
		/// Any part of the object that is outside the clipping region is transparent. 
		/// Any coordinate can be replaced by the value auto, which exposes the 
		/// respective side (that is, the side is not clipped).
		/// </para>
		/// <para>
		/// The order of the values rect(0 0 50 50) renders the object invisible 
		/// because it sets the top and right positions of the clipping region to 0. 
		/// To achieve a 50-by-50 view port, use rect(0 50 50 0).
		/// </para>
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </para>
		/// </remarks>
		/// 
		ClipStyle clip{ get; set;}  

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
		/// 
		Color color{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the persisted representation of the style rule. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
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
		/// 
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
		/// 
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
		/// 
		string filter{ get; set;}
        
		/// <summary>
        /// Sets or retrieves a combination of separate IHTMLStyle::font properties of the object. Alternatively, sets or retrieves one or more of six user-preference fonts. 
        /// </summary>
		/// <remarks>
		/// <para>This is a composite property that specifies up to six font values. The font-style, font-variant, and font-weight values may appear in any order before font-size. However, the font-size, line-height, and font-family properties must appear in the order listed. Setting the IHTMLStyle::font property also sets the component properties. In this case, the string must be a combination of valid values for the component properties; only font-family may have more than one value. If the string does not contain a value for a component property, that property is set to its default, regardless of prior settings for that component property.</para>
		/// <para>As of Internet Explorer 6, when you use the !DOCTYPE declaration to specify standards-compliant mode, the following conditions apply to this property. </para>
		/// <para>The font-size and font-family values must be declared. If font-size and font-family are not declared, or are not in the correct order, the IHTMLStyle::font property is ignored.</para>
		/// <para>All specified values must appear in the correct order. Otherwise, the IHTMLStyle::font property is ignored.</para>
		/// <para>In standards-compliant mode, the default font-size is small, not medium. If not explicitly set, font-size returns a point value.</para>
		/// <para>For more information about standards-compliant parsing and the !DOCTYPE declaration, see CSS Enhancements in Internet Explorer 6.</para>
		/// <para>When specifying the user preferences caption, icon, menu, message-box, small-caption, or status-bar for this property, do not set other values for the font property on the same element. If you do, the other values might render, but the user preference value is ignored.</para>
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </para>
		/// </remarks>
		/// 
		string font{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the name of the font used for text in the object.  
        /// </summary>
		/// <remarks>
		/// <para>The value is a prioritized list of font family names and generic family names. 
		/// List items are separated by commas to minimize confusion between multiple-word font 
		/// family names. If the font family name contains white space, it should appear in 
		/// single or double quotation marks; generic font family names are values and cannot 
		/// appear in quotation marks. </para>
		/// <para>Because you do not know which fonts users have installed, you should provide 
		/// a list of alternatives with a generic font family at the end of the list. This list 
		/// can include embedded fonts. For more information about embedding fonts, see the at font-face rule.
		/// </para>
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </para>
		/// </remarks>
		/// 
		string fontFamily{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a value that indicates the font size used for text in the object.  
        /// </summary>
		/// <remarks>
		/// <para>As of Internet Explorer 6, when you use the !DOCTYPE declaration to specify standards-compliant mode, the default value for this property is small, not medium.</para>
		/// <para>Negative values are not allowed. Font sizes using the proportional "em" measure are based on the font size of the parent object.</para>
		/// <para>Possible length values specified in a relative measurement, using the height of the element's font (em) or the height of the letter "x" (ex), are supported in Internet Explorer 4.0 and later.</para>
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </para>
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit fontSize{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the font style of the object as italic, normal, or oblique.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string fontStyle{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves whether the text of the object is in small capital letters. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string fontVariant{ get; set;} 
        
		/// <summary>
        /// Sets or retrieves the weight of the font of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string fontWeight{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the height of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit height{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the position of the object relative to the left edge of the next 
        /// positioned object in the document hierarchy.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit left{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the amount of additional space between letters in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit letterSpacing{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the amount of additional space between letters in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		bool letterSpacingNormal{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the distance between lines in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit lineHeight{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves up to three separate IHTMLStyle::listStyle properties of the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string listStyle{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a value that indicates which image to use as a list-item marker for the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string listStyleImage{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a variable that indicates how the list-item marker is drawn relative to the content of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		ListStylePosition listStylePosition{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the predefined type of the line-item marker for the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		ListStyleType listStyleType{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the width of the top, right, bottom, and left margins of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit margin{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the height of the bottom margin of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit marginBottom{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the width of the left margin of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit marginLeft{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the width of the right margin of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit marginRight{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the height of the top margin of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit marginTop{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a value indicating how to manage the content of the object 
        /// when the content exceeds the height or width of the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string overflow{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the amount of space to insert between the object and its margin or, 
        /// if there is a border, between the object and its border.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit padding{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the amount of space to insert between the bottom border of 
        /// the object and the content.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit paddingBottom{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the amount of space to insert between the left border of the object and the content.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit paddingLeft{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the amount of space to insert between the right border of the object and the content.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit paddingRight{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the amount of space to insert between the top border of the object and the content.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit paddingTop{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a value indicating whether a page break occurs after the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		PageBreakStyles pageBreakAfter{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a string indicating whether a page break occurs before the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		PageBreakStyles pageBreakBefore{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the bottom position of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		int pixelBottom{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the right position of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		int pixelRight{ get; set;}  
        
		/// <summary>
        /// Retrieves the type of positioning used for the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string position { get;}
        
		/// <summary>
        /// Sets or retrieves the bottom position of the object in the units specified by the IHTMLStyle::left attribute.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit posBottom{ get; set;} 
        
		/// <summary>
        /// Sets or retrieves the right position of the object in the units specified by the IHTMLStyle::top attribute.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit posRight{ get; set;}  
        
		/// <summary>
        /// Removes the given attribute from the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		/// <param name="attribute">Specifies the attribute name.</param>
        void RemoveAttribute(string attribute);
        
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
		/// 
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
		/// 
		string styleFloat{ get; set;} 
        
		/// <summary>
        /// Sets or retrieves whether the text in the object is left-aligned, 
        /// right-aligned, centered, or justified.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.HorizontalAlign textAlign{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a value that indicates whether the text in the object 
        /// has blink, line-through, overline, or underline decorations.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string textDecoration{ get; set;}    
        
		/// <summary>
        /// Sets or retrieves a Boolean value that indicates whether the object's 
        /// IHTMLStyle::textDecoration property has a value of "blink." 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		bool textDecorationBlink{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a Boolean value indicating whether the text in the object has a 
        /// line drawn through it.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		bool textDecorationLineThrough{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the Boolean value indicating whether the IHTMLStyle::textDecoration 
        /// property for the object has been set to none.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		bool textDecorationNone{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a Boolean value indicating whether the text in the object 
        /// has a line drawn over it.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		bool textDecorationOverline{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves whether the text in the object is underlined.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		bool textDecorationUnderline{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the indentation of the first line of text in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit textIndent{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the rendering of the text in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string textTransform{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the position of the object relative to the top of the 
        /// next positioned object in the document hierarchy.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit top{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the vertical alignment of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.VerticalAlign verticalAlign{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves whether the content of the object is displayed.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string visibility{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a value that indicates whether lines are automatically broken inside the object. 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		string whiteSpace{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit width{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the amount of additional space between words in the object.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		System.Web.UI.WebControls.Unit wordSpacing{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the amount of additional space between words in the object to the default value.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		bool wordSpacingNormal{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the stacking order of positioned objects as integer value.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		int zIndex{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the stacking order of positioned objects as auto value.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		bool zIndexAuto{ get; set;}  

        /// <summary>
        /// Sets or retrieves the color of the top and left edges of the scroll box and 
        /// scroll arrows of a scroll bar.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color scrollbarArrowColor{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the color of the main elements of a scroll bar, 
        /// which include the scroll box, track, and scroll arrows.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color scrollbarBaseColor{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the color of the gutter of a scroll bar.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color scrollbarDarkShadowColor{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the color of the scroll box and scroll arrows of a scroll bar.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color scrollbarFaceColor{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the color of the top and left edges of the scroll box and 
        /// scroll arrows of a scroll bar.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color scrollbarHighlightColor{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the color of the bottom and right edges of the scroll box and 
        /// scroll arrows of a scroll bar.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color scrollbarShadowColor{ get; set;}
        
		/// <summary>
        /// Sets or retrieves the color of the track element of a scroll bar.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </remarks>
		/// 
		Color scrollbarTrackColor{ get; set;}

        /// <summary>
        /// Sets or retrieves whether to break words when the content exceeds the boundaries of its container.
        /// </summary>
        /// <remarks>
        /// This property applies to elements that have layout. An element has layout when 
        /// it is absolutely positioned, is a block element, or is an inline element with a 
        /// specified height or width.
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.CssEffectiveStyle">CssEffectiveStyle</see> 
		/// class for more details.
		/// </para>
		/// </remarks>
		/// 
		WordWrap wordWrap{ get; set;}

	}
}
