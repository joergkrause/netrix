using System;
using System.Drawing;
using System.Web;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
	/// Base Interface for current styles
	/// </summary>
	public interface IEffectiveStyle
	{
        /// <summary>
        /// Sets or retrieves how the background is styled to the object within the document. 
        /// </summary>
        IBackgroundStyles background { get; } 
        /// <summary>
        /// Sets or retrieves the color of the bottom border of the object.  
        /// </summary>
        IBorderEffective border{ get; }  
        /// <summary>
        /// Sets or retrieves whether the object allows floating objects on its left side, right side, or both, so that the next text displays past the floating objects. 
        /// </summary>
        ClearStyles clear{ get; }  

        /// <summary>
        /// Sets or retrieves which part of a positioned object is visible.  
        /// </summary>
        IClipEffective clip{ get; }  
        /// <summary>
        /// Sets or retrieves the color of the text of the object.  
        /// </summary>
        Color color{ get; }  
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
        /// </remarks>
        string cursor{ get; }  
        /// <summary>
        /// Sets or retrieves whether the object is rendered. 
        /// </summary>
        DisplayStyles display{ get; }  
        /// <summary>
        /// Sets or retrieves the font used for text in the object.  
        /// </summary>
        IFontEffective font{ get; }  
        /// <summary>
        /// Sets or retrieves the height of the object.  
        /// </summary>
        string height{ get; }
        /// <summary>
        /// Sets or retrieves the position of the object relative to the left edge of the next positioned object in the document hierarchy.  
        /// </summary>
        string left{ get; }  
        /// <summary>
        /// Sets or retrieves the amount of additional space between letters in the object.  
        /// </summary>
        string letterSpacing{ get; }          
        /// <summary>
        /// Sets or retrieves the distance between lines in the object.  
        /// </summary>
        string lineHeight{ get; }  
        /// <summary>
        /// Sets or retrieves a value that indicates which image to use as a list-item marker for the object.  
        /// </summary>
        string listStyleImage{ get; }  
        /// <summary>
        /// Sets or retrieves a variable that indicates how the list-item marker is drawn relative to the content of the object.  
        /// </summary>
        ListStylePosition listStylePosition{ get; }  
        /// <summary>
        /// Sets or retrieves the predefined type of the line-item marker for the object.  
        /// </summary>
        ListStyleType listStyleType{ get; }  
        /// <summary>
        /// Sets or retrieves the margin of the object.  
        /// </summary>
        IFourProperties margin{ get; }
        /// <summary>
        /// Sets or retrieves a value indicating how to manage the content of the object when the content exceeds the height or width of the object. 
        /// </summary>
        string overflow{ get; }  
        /// <summary>
        /// Sets or retrieves the amount of space to insert between the border of the object and the content.  
        /// </summary>
        IFourProperties padding{ get; }
        /// <summary>
        /// Sets or retrieves a value indicating whether a page break occurs after the object.  
        /// </summary>
        PageBreakStyles pageBreakAfter{ get; }  
        /// <summary>
        /// Sets or retrieves a string indicating whether a page break occurs before the object. 
        /// </summary>
        PageBreakStyles pageBreakBefore{ get; }  
        /// <summary>
        /// Retrieves the type of positioning used for the object.  
        /// </summary>
        string position { get;}
        /// <summary>
        /// Sets or retrieves on which side of the object the text will flow. 
        /// </summary>
        string styleFloat{ get; } 
        /// <summary>
        /// Sets or retrieves whether the text in the object is left-aligned, right-aligned, centered, or justified.  
        /// </summary>
        System.Web.UI.WebControls.HorizontalAlign textAlign { get; }  
        /// <summary>
        /// Sets or retrieves a value that indicates whether the text in the object has blink, line-through, overline, or underline decorations.
        /// </summary>
        string textDecoration{ get; }    
        /// <summary>
        /// Sets or retrieves the indentation of the first line of text in the object.  
        /// </summary>
        System.Web.UI.WebControls.Unit textIndent{ get; }  
        /// <summary>
        /// Sets or retrieves the position of the object relative to the top of the next positioned object in the document hierarchy.  
        /// </summary>
        string top{ get; }  
        /// <summary>
        /// Sets or retrieves the vertical alignment of the object.  
        /// </summary>
        System.Web.UI.WebControls.VerticalAlign verticalAlign{ get; }  
        /// <summary>
        /// Sets or retrieves whether the content of the object is displayed.  
        /// </summary>
        string visibility{ get; }  
        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        string width{ get; }  
        /// <summary>
        /// Sets or retrieves the stacking order of positioned objects as integer value.
        /// </summary>
        string zIndex{ get; }  
	}
}
