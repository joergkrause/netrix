using System;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
	/// This class represents one style definition. This class implements IStyle interface.
	/// </summary>
	/// <remarks>
	/// A style definition is either an inline style (attribute style of any stylable element) or the
	/// definition part of a style rule. A rule is a combination of a descriptor and a style.
	/// <para>
	/// The purpose of this class is to have an isolated store for CSS definitions. This behaves slightly
	/// different from the styles defined as part of the elements. The native class CssStyle allows access
	/// to style definitions using the <see cref="System.Windows.Forms.PropertyGrid">PropertyGrid</see> and
	/// not the NetRix StyleEditor. 
	/// </para>
	/// </remarks>
    public class CssStyle : IStyle, ICustomTypeDescriptor
    {

        private Interop.IHTMLRuleStyle htmlStyle;
        private Interop.IHTMLRuleStyle2 htmlStyle2;
        private Interop.IHTMLRuleStyle3 htmlStyle3;
        private Interop.IHTMLRuleStyle4 htmlStyle4;

        internal CssStyle(Interop.IHTMLRuleStyle hs)
        {
            this.htmlStyle = hs;
            this.htmlStyle2 = (Interop.IHTMLRuleStyle2) hs;
            this.htmlStyle3 = (Interop.IHTMLRuleStyle3) hs;
            this.htmlStyle4 = (Interop.IHTMLRuleStyle4) hs;
        }

        /// <summary>
        /// Sets or retrieves up to five separate background properties of the object. 
        /// </summary>
        public BackgroundStyles background 
        { 
            get
            {
                string bgs = htmlStyle.background;
                return (BackgroundStyles) Enum.Parse(typeof(BackgroundStyles), bgs, true);
            } 
            set
            {
                htmlStyle.background = Enum.GetName(typeof(BackgroundStyles), value);
            }
        } 
        /// <summary>
        /// Sets or retrieves how the background image is attached to the object within the document. 
        /// </summary>
        public string backgroundAttachment 
        { 
            get
            {
                return this.htmlStyle.backgroundAttachment;
            }
            set
            {
                this.htmlStyle.backgroundAttachment = value;
            }
        } 
        /// <summary>
        /// Sets or retrieves the color behind the content of the object.  
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        public Color backgroundColor
        {
            get
            {                                
                if (this.htmlStyle.backgroundColor != null)
                {
                    return RgbColorConverter(this.htmlStyle.backgroundColor.ToString());
                } 
                else 
                {
                    return Color.Empty;
                }
            }
            set
            {
                this.htmlStyle.backgroundColor = (ColorTranslator.ToHtml(value));
            }
        }
        /// <summary>
        /// Sets or retrieves the background image of the object.  
        /// </summary>
        public string backgroundImage
        { 
            get
            {
                return this.htmlStyle.backgroundImage;
            }
            set
            {
                this.htmlStyle.backgroundImage = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves the position of the background of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit backgroundPositionUnit        
        { 
            get
            {
                return Unit.Parse(this.htmlStyle.backgroundPosition, System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                this.htmlStyle.backgroundPosition = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
        }  

        /// <summary>
        /// Sets or retrieves the position of the background of the object.  
        /// </summary>
        public System.Web.UI.WebControls.VerticalAlign backgroundPositionVerticalAlign        
        { 
            get
            {
                VerticalAlign va = (VerticalAlign) Enum.Parse(typeof(VerticalAlign), this.htmlStyle.backgroundPosition, true);
                return va;
            }
            set
            {
                this.htmlStyle.backgroundPosition = (Enum.GetName(typeof(VerticalAlign), value));
            }
        }  

        /// <summary>
        /// Sets or retrieves the position of the background of the object.  
        /// </summary>
        public System.Web.UI.WebControls.HorizontalAlign backgroundPositionHorizontalAlign        
        { 
            get
            {
                HorizontalAlign ha = (HorizontalAlign) Enum.Parse(typeof(HorizontalAlign), this.htmlStyle.backgroundPosition, true);
                return ha;
            }
            set
            {
                this.htmlStyle.backgroundPosition = (Enum.GetName(typeof(HorizontalAlign), value));
            }
        }  


        /// <summary>
        /// Sets or retrieves the x-coordinate of the backgroundPosition property.  
        /// </summary>
        public System.Web.UI.WebControls.Unit backgroundPositionX        
        { 
            get
            {
                return Unit.Parse(this.htmlStyle.backgroundPositionX.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                this.htmlStyle.backgroundPositionX = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
        }  

        /// <summary>
        /// Sets or retrieves the y-coordinate of the backgroundPosition property.  
        /// </summary>
        public System.Web.UI.WebControls.Unit backgroundPositionY        
        { 
            get
            {
                return Unit.Parse(this.htmlStyle.backgroundPositionY.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                this.htmlStyle.backgroundPositionY = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
        }  


        /// <summary>
        /// Sets or retrieves how the backgroundImage property of the object is tiled. 
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint)]
        public BackgroundRepeat backgroundRepeat        
        { 
            get
            {
                return (BackgroundRepeat) ConvertEnumeration(typeof(BackgroundRepeat), this.htmlStyle.backgroundRepeat);
            }
            set
            {
                this.htmlStyle.backgroundRepeat = (ConvertEnumeration(value));
            }
        }  


        /// <summary>
        /// Sets or retrieves the properties to draw around the object. 
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        public string border        
        { 
            get
            {
                return this.htmlStyle.border;
            }
            set
            {
                this.htmlStyle.border = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves the properties of the bottom border of the object. 
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description("Sets or retrieves the properties of the bottom border of the object.")]        
        public string borderBottom
        { 
            get
            {
                return this.htmlStyle.borderBottom;
            }
            set
            {
                this.htmlStyle.borderBottom = (value);
            }
        }

        /// <summary>
        /// Sets or retrieves the color of the bottom border of the object.  
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        public Color borderBottomColor        
        { 
            get
            {
                return RgbColorConverter(this.htmlStyle.borderBottomColor.ToString());
            }
            set
            {
                this.htmlStyle.borderBottomColor = (ColorTranslator.ToHtml(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves the style of the bottom border of the object.  
        /// </summary>
        public System.Web.UI.WebControls.BorderStyle borderBottomStyle        
        { 
            get
            {
                return (System.Web.UI.WebControls.BorderStyle) ConvertEnumeration(typeof(System.Web.UI.WebControls.BorderStyle), this.htmlStyle.borderStyle);
            }
            set
            {
                this.htmlStyle.borderStyle = (ConvertEnumeration(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the bottom border of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit borderBottomWidth        
        { 
            get
            {
                string val = this.htmlStyle.borderBottomWidth.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.borderBottomWidth = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool borderBottomWidthAuto        
        { 
            get
            {
                string val = this.htmlStyle.borderBottomWidth.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.borderBottomWidth = (value ? "auto" : "");
            }
        }    

        /// <summary>
        /// Sets or retrieves the border color of the object. 
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        public Color borderColor        
        { 
            get
            {
                return RgbColorConverter(this.htmlStyle.borderColor.ToLower());
            }
            set
            {
                this.htmlStyle.borderColor = (ColorTranslator.ToHtml(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves the properties of the left border of the object. 
        /// </summary>
        public string borderLeft        
        { 
            get
            {
                return this.htmlStyle.borderLeft;
            }
            set
            {
                this.htmlStyle.borderLeft = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves the color of the left border of the object.  
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        public Color borderLeftColor        
        { 
            get
            {
                return RgbColorConverter(this.htmlStyle.borderLeftColor.ToString());
            }
            set
            {
                this.htmlStyle.borderLeftColor = (ColorTranslator.ToHtml(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves the style of the left border of the object.  
        /// </summary>
        public System.Web.UI.WebControls.BorderStyle borderLeftStyle        
        { 
            get
            {
                return (BorderStyle) ConvertEnumeration(typeof(BorderStyle), this.htmlStyle.borderLeftStyle);
            }
            set
            {
                this.htmlStyle.borderLeftStyle = (ConvertEnumeration(value));
            }

        }  

        /// <summary>
        /// Sets or retrieves the width of the left border of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit borderLeftWidth        
        { 
            get
            {
                string val = this.htmlStyle.borderLeftWidth.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.borderLeftWidth = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool borderLeftWidthAuto        
        { 
            get
            {
                string val = this.htmlStyle.borderLeftWidth.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.borderLeftWidth = (value ? "auto" : "");
            }
        }   

        /// <summary>
        /// Sets or retrieves the properties of the right border of the object.  
        /// </summary>
        public string borderRight        
        { 
            get
            {
                return this.htmlStyle.borderRight.ToString();
            }
            set
            {
                this.htmlStyle.borderRight = (value);
            }
        }   

        /// <summary>
        /// Sets or retrieves the color of the right border of the object.  
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        public Color borderRightColor        
        { 
            get
            {
                return RgbColorConverter(this.htmlStyle.borderRightColor.ToString());
            }
            set
            {
                this.htmlStyle.borderRightColor = (ColorTranslator.ToHtml(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves the style of the right border of the object.  
        /// </summary>
        public System.Web.UI.WebControls.BorderStyle borderRightStyle        
        { 
            get
            {
                return (BorderStyle) ConvertEnumeration(typeof(BorderStyle), this.htmlStyle.borderRightStyle);
            }
            set
            {
                this.htmlStyle.borderRightStyle = (ConvertEnumeration(value));
            }

        }  

        /// <summary>
        /// Sets or retrieves the width of the right border of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit borderRightWidth        
        { 
            get
            {
                return Unit.Parse(this.htmlStyle.borderRightWidth.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                this.htmlStyle.borderRightWidth = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the right border of the object.  
        /// </summary>
        public string borderRightWidthValue        
        { 
            get
            {
                string s = this.htmlStyle.borderRightWidth.ToString().ToLower();
                if (s.Equals("medium") || s.Equals("thin") || s.Equals("thick"))
                {
                    return s;
                } 
                else 
                {
                    return String.Empty;
                }
            }
            set
            {
                this.htmlStyle.borderRightWidth = (value);
            }
        } 

        /// <summary>
        /// Sets or retrieves the style of the left, right, top, and bottom borders of the object.  
        /// </summary>
        public System.Web.UI.WebControls.BorderStyle borderStyle        
        { 
            get
            {
                return (BorderStyle) ConvertEnumeration(typeof(BorderStyle), this.htmlStyle.borderStyle);
            }
            set
            {
                this.htmlStyle.borderStyle = (ConvertEnumeration(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves the properties of the top border of the object.  
        /// </summary>
        public string borderTop        
        { 
            get
            {
                return this.htmlStyle.borderTop;
            }
            set
            {
                this.htmlStyle.borderTop = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves the color of the top border of the object.  
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        public Color borderTopColor        
        { 
            get
            {
                return RgbColorConverter(this.htmlStyle.borderTopColor.ToString());
            }
            set
            {
                this.htmlStyle.borderTopColor = (ColorTranslator.ToHtml(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves the style of the top border of the object.  
        /// </summary>
        public System.Web.UI.WebControls.BorderStyle borderTopStyle        
        { 
            get
            {
                return (BorderStyle) ConvertEnumeration(typeof(BorderStyle), this.htmlStyle.borderTopStyle);
            }
            set
            {
                this.htmlStyle.borderTopStyle = (ConvertEnumeration(value));
            }

        }  

        /// <summary>
        /// Sets or retrieves the width of the top border of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit borderTopWidth        
        { 
            get
            {
                return Unit.Parse(this.htmlStyle.borderTopWidth.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                this.htmlStyle.borderTopWidth = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the right border of the object.  
        /// </summary>
        public string borderTopWidthValue        
        { 
            get
            {
                string s = this.htmlStyle.borderTopWidth.ToString().ToLower();
                if (s.Equals("medium") || s.Equals("thin") || s.Equals("thick"))
                {
                    return s;
                } 
                else 
                {
                    return String.Empty;
                }
            }
            set
            {
                this.htmlStyle.borderTopWidth = (value);
            }
        }   

        /// <summary>
        /// Sets or retrieves the width of the left, right, top, and bottom borders of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit borderWidth        
        { 
            get
            {
                return Unit.Parse(this.htmlStyle.borderWidth, System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                this.htmlStyle.borderWidth = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves whether the object allows floating objects on its left side, right side, or both, so that the next text displays past the floating objects. 
        /// </summary>
        public ClearStyles clear        
        { 
            get
            {
                return (ClearStyles) ConvertEnumeration(typeof(ClearStyles), this.htmlStyle.clear);
            }
            set
            {
                this.htmlStyle.clear = (ConvertEnumeration(value));
            }
        }  


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
        /// </remarks>
        public ClipStyle clip        
        { 
            get
            {
                ClipStyle cs = new ClipStyle();
                cs.ClipString = this.htmlStyle.clip;
                return cs;
            }
            set
            {
                this.htmlStyle.clip = (value.ClipString);
            }
        }  


        /// <summary>
        /// Sets or retrieves the color of the text of the object.  
        /// </summary>
        /// <remarks>
        /// Some browsers do not recognize color names, but all browsers should 
        /// recognize RGB color values and display them correctly.
        /// Therefore the property returns always RGB values, even if the value 
        /// was previously set using names. This is a different behavior compared to the original MSHTML.
        /// </remarks>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        public Color color        
        { 
            get
            {
                if (this.htmlStyle.color != null)
                {
                    return RgbColorConverter(this.htmlStyle.color.ToString());
                } 
                else 
                {
                    return Color.Empty;
                }
            }
            set
            {
                this.htmlStyle.color = (ColorTranslator.ToHtml(value));
            }
        }

        private static Color RgbColorConverter(string cssColor)
        {
            if (cssColor.StartsWith("rgb("))
            {
                string[] rgb = cssColor.Substring(4, cssColor.Length - 5).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                cssColor = String.Format("#{0:X2}{1:X2}{2:X2}",
                    Int32.Parse(rgb[0], System.Globalization.NumberStyles.Number),
                    Int32.Parse(rgb[1], System.Globalization.NumberStyles.Number),
                    Int32.Parse(rgb[2], System.Globalization.NumberStyles.Number)
                    );
            }
            return ColorTranslator.FromHtml(cssColor);
        }

        /// <summary>
        /// Sets or retrieves the persisted representation of the style rule. 
        /// </summary>
        public string cssText        
        { 
            get
            {
                return this.htmlStyle.cssText;
            }
            set
            {
                this.htmlStyle.cssText = value;
            }
        }  

        
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
        public string cursor        
        { 
            get
            {
                return this.htmlStyle.cursor;
            }
            set
            {
                this.htmlStyle.cursor = value;
            }
        }  

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
        /// </remarks>
        public DisplayStyles display        
        { 
            get
            {
                return (DisplayStyles) ConvertEnumeration(typeof(DisplayStyles), this.htmlStyle.display);
            }
            set
            {
                this.htmlStyle.display = (ConvertEnumeration(value));
            }
        }  

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
        /// </remarks>
        public string filter
        { 
            get
            {
                return this.htmlStyle.filter;
            }
            set
            {
                this.htmlStyle.filter = (value);
            }
        }

        /// <summary>
        /// Sets or retrieves a combination of separate Horizontalfont properties of the object. Alternatively, sets or retrieves one or more of six user-preference fonts. 
        /// </summary>
        /// <remarks>
        /// <para>This is a composite property that specifies up to six font values. The font-style, font-variant, and font-weight values may appear in any order before font-size. However, the font-size, line-height, and font-family properties must appear in the order listed. Setting the IHTMLStyle::font property also sets the component properties. In this case, the string must be a combination of valid values for the component properties; only font-family may have more than one value. If the string does not contain a value for a component property, that property is set to its default, regardless of prior settings for that component property.</para>
        /// <para>As of Internet Explorer 6, when you use the !DOCTYPE declaration to specify standards-compliant mode, the following conditions apply to this property. </para>
        /// <para>The font-size and font-family values must be declared. If font-size and font-family are not declared, or are not in the correct order, the IHTMLStyle::font property is ignored.</para>
        /// <para>All specified values must appear in the correct order. Otherwise, the IHTMLStyle::font property is ignored.</para>
        /// <para>In standards-compliant mode, the default font-size is small, not medium. If not explicitly set, font-size returns a point value.</para>
        /// <para>For more information about standards-compliant parsing and the !DOCTYPE declaration, see CSS Enhancements in Internet Explorer 6.</para>
        /// <para>When specifying the user preferences caption, icon, menu, message-box, small-caption, or status-bar for this property, do not set other values for the font property on the same element. If you do, the other values might render, but the user preference value is ignored.</para>
        /// </remarks>
        public string font        
        { 
            get
            {
                return this.htmlStyle.font;
            }
            set
            {
                this.htmlStyle.font = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves the name of the font used for text in the object.  
        /// </summary>
        /// <remarks>
        /// <para>The value is a prioritized list of font family names and generic family names. List items are separated by commas to minimize confusion between multiple-word font family names. If the font family name contains white space, it should appear in single or double quotation marks; generic font family names are values and cannot appear in quotation marks. </para>
        /// <para>Because you do not know which fonts users have installed, you should provide a list of alternatives with a generic font family at the end of the list. This list can include embedded fonts. For more information about embedding fonts, see the @font-face rule.</para>
        /// </remarks>
        public string fontFamily        
        { 
            get
            {
                return this.htmlStyle.fontFamily;
            }
            set
            {
                this.htmlStyle.fontFamily = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves a value that indicates the font size used for text in the object.  
        /// </summary>
        /// <remarks>
        /// <para>As of Internet Explorer 6, when you use the !DOCTYPE declaration to specify standards-compliant mode, the default value for this property is small, not medium.</para>
        /// <para>Negative values are not allowed. Font sizes using the proportional "em" measure are based on the font size of the parent object.</para>
        /// <para>Possible length values specified in a relative measurement, using the height of the element's font (em) or the height of the letter "x" (ex), are supported in Internet Explorer 4.0 and later.</para>
        /// </remarks>
        public System.Web.UI.WebControls.Unit fontSize        
        { 
            get
            {
                return Unit.Parse(this.htmlStyle.fontSize.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                this.htmlStyle.fontSize = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the font style of the object as italic, normal, or oblique.  
        /// </summary>
        public string fontStyle        
        { 
            get
            {
                return this.htmlStyle.fontStyle;
            }
            set
            {
                this.htmlStyle.fontStyle = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves whether the text of the object is in small capital letters. 
        /// </summary>
        public string fontVariant
        { 
            get
            {
                return this.htmlStyle.fontStyle;
            }
            set
            {
                this.htmlStyle.fontStyle = (value);
            }
        } 
        /// <summary>
        /// Sets or retrieves the weight of the font of the object.  
        /// </summary>
        public string fontWeight        
        { 
            get
            {
                return this.htmlStyle.fontWeight;
            }
            set
            {
                this.htmlStyle.fontWeight = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves the height of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit height
        { 
            get
            {
                string val = this.htmlStyle.height.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.height = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool heightAuto        
        { 
            get
            {
                string val = this.htmlStyle.height.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.height = (value ? "auto" : "");
            }
        } 

        /// <summary>
        /// Sets or retrieves the position of the object relative to the left edge of the next 
        /// positioned object in the document hierarchy.  
        /// </summary>
        public System.Web.UI.WebControls.Unit left        
        { 
            get
            {
                string val = this.htmlStyle.left.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.left = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool leftAuto        
        { 
            get
            {
                string val = this.htmlStyle.left.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.left = (value ? "auto" : "");
            }
        }  

        /// <summary>
        /// Sets or retrieves the amount of additional space between letters in the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit letterSpacing        
        { 
            get
            {
                string val = this.htmlStyle.letterSpacing.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.letterSpacing = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

		/// <summary>
		/// Sets or retrieves the amount of additional space between letters in the object.  
		/// </summary>
		public bool letterSpacingNormal
        { 
            get
            {
                string val = this.htmlStyle.letterSpacing.ToString();
                if (val.Equals("normal"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.letterSpacing = (value ? "normal" : "");
            }
        } 
  
        /// <summary>
        /// Sets or retrieves the distance between lines in the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit lineHeight        
        { 
            get
            {
                string val = this.htmlStyle.lineHeight.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.lineHeight = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the distance between lines in the object if set to "normal".
        /// </summary>
        public bool lineHeightNormal
        { 
            get
            {
                string val = this.htmlStyle.lineHeight.ToString();
                if (val.Equals("normal"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.lineHeight = (value ? "normal" : "");
            }
        } 


        /// <summary>
        /// Sets or retrieves up to three separate IHTMLStyle::listStyle properties of the object. 
        /// </summary>
        public string listStyle        
        { 
            get
            {
                return this.htmlStyle.listStyle;
            }
            set
            {
                this.htmlStyle.listStyle = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves a value that indicates which image to use as a list-item marker for the object.  
        /// </summary>
        public string listStyleImage        
        { 
            get
            {
                return this.htmlStyle.listStyleImage;
            }
            set
            {
                this.htmlStyle.listStyleImage = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves a variable that indicates how the list-item marker is drawn relative to the content of the object.  
        /// </summary>
        public ListStylePosition listStylePosition        
        { 
            get
            {
                return (ListStylePosition) ConvertEnumeration(typeof(ListStylePosition), this.htmlStyle.listStylePosition);
            }
            set
            {
                this.htmlStyle.listStylePosition = (ConvertEnumeration(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves the predefined type of the line-item marker for the object.  
        /// </summary>
        public ListStyleType listStyleType        
        { 
            get
            {
                return (ListStyleType) ConvertEnumeration(typeof(ListStyleType), this.htmlStyle.listStyleType);
            }
            set
            {
                this.htmlStyle.listStyleType = (ConvertEnumeration(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the top, right, bottom, and left margins of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit  margin        
        { 
            get
            {
                return Unit.Parse(this.htmlStyle.margin, System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                this.htmlStyle.margin = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the height of the bottom margin of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit marginBottom        
        { 
            get
            {
                string val = this.htmlStyle.marginBottom.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.marginBottom = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool marginBottomAuto        
        { 
            get
            {
                string val = this.htmlStyle.marginBottom.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.marginBottom = (value ? "auto" : "");
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the left margin of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit marginLeft        
        { 
            get
            {
                string val = this.htmlStyle.marginLeft.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.marginLeft = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool marginLeftAuto        
        { 
            get
            {
                string val = this.htmlStyle.marginLeft.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.marginLeft = (value ? "auto" : "");
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the right margin of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit marginRight        
        { 
            get
            {
                string val = this.htmlStyle.marginRight.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.marginRight = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool marginRightAuto        
        { 
            get
            {
                string val = this.htmlStyle.marginRight.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.marginRight = (value ? "auto" : "");
            }
        }  

        /// <summary>
        /// Sets or retrieves the height of the top margin of the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit marginTop        
        { 
            get
            {
                string val = this.htmlStyle.marginTop.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.marginTop = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool marginTopAuto        
        { 
            get
            {
                string val = this.htmlStyle.marginTop.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.marginTop = (value ? "auto" : "");
            }
        }  

        /// <summary>
        /// Sets or retrieves a value indicating how to manage the content of the object 
        /// when the content exceeds the height or width of the object. 
        /// </summary>
        public string overflow        
        { 
            get
            {
                return this.htmlStyle.overflow;
            }
            set
            {
                this.htmlStyle.overflow = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves the amount of space to insert between the object and its margin or, 
        /// if there is a border, between the object and its border.  
        /// </summary>
        public System.Web.UI.WebControls.Unit padding        
        { 
            get
            {
                return Unit.Parse(this.htmlStyle.padding, System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                this.htmlStyle.padding = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the amount of space to insert between the bottom border of the object and 
        /// the content.  
        /// </summary>
        public System.Web.UI.WebControls.Unit paddingBottom        
        { 
            get
            {
                string val = this.htmlStyle.paddingBottom.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.paddingBottom = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool paddingBottomAuto        
        { 
            get
            {
                string val = this.htmlStyle.paddingBottom.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.paddingBottom = (value ? "auto" : "");
            }
        }   

        /// <summary>
        /// Sets or retrieves the amount of space to insert between the left border of the object and the content.  
        /// </summary>
        public System.Web.UI.WebControls.Unit paddingLeft        
        { 
            get
            {
                string val = this.htmlStyle.paddingLeft.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.paddingBottom = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool paddingLeftAuto        
        { 
            get
            {
                string val = this.htmlStyle.paddingLeft.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.paddingLeft = (value ? "auto" : "");
            }
        }

        /// <summary>
        /// Sets or retrieves the amount of space to insert between the right border of the object and the content.  
        /// </summary>
        public System.Web.UI.WebControls.Unit paddingRight        
        { 
            get
            {
                string val = this.htmlStyle.paddingRight.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.paddingRight = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool paddingRightAuto        
        { 
            get
            {
                string val = this.htmlStyle.paddingRight.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.paddingRight = (value ? "auto" : "");
            }
        } 

        /// <summary>
        /// Sets or retrieves the amount of space to insert between the top border of the object and the content.  
        /// </summary>
        public System.Web.UI.WebControls.Unit paddingTop        
        { 
            get
            {
                string val = this.htmlStyle.paddingTop.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.paddingTop = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool paddingTopAuto        
        { 
            get
            {
                string val = this.htmlStyle.paddingTop.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.paddingTop = (value ? "auto" : "");
            }
        }   

        /// <summary>
        /// Sets or retrieves a value indicating whether a page break occurs after the object.  
        /// </summary>
        public PageBreakStyles pageBreakAfter        
        { 
            get
            {
                return (PageBreakStyles) ConvertEnumeration(typeof(PageBreakStyles), this.htmlStyle.pageBreakAfter);
            }
            set
            {
                this.htmlStyle.pageBreakAfter = (ConvertEnumeration(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves a string indicating whether a page break occurs before the object. 
        /// </summary>
        public PageBreakStyles pageBreakBefore        
        { 
            get
            {
                return (PageBreakStyles) ConvertEnumeration(typeof(PageBreakStyles), this.htmlStyle.pageBreakBefore);
            }
            set
            {
                this.htmlStyle.pageBreakBefore = (ConvertEnumeration(value));
            }
        }  

		/// <summary>
		/// Sets or retrieves the bottom position of the object.  
		/// </summary>
		public int pixelBottom
        { 
            get
            {
                return this.htmlStyle2.pixelBottom;
            }
            set
            {
                this.htmlStyle2.pixelBottom = value;
            }
        }  

		/// <summary>
		/// Sets or retrieves the right position of the object.  
		/// </summary>
		public int pixelRight
        { 
            get
            {
                return this.htmlStyle2.pixelRight;
            }
            set
            {
                this.htmlStyle2.pixelRight = value;
            }
        }  

		/// <summary>
		/// Sets or retrieves the bottom position of the object in the units specified by the IHTMLStyle::left attribute.  
		/// </summary>
		public System.Web.UI.WebControls.Unit posBottom        
        { 
            get
            {
                return new Unit(String.Concat(this.htmlStyle2.posBottom, this.top.Type.ToString()));
            }
            set
            {
                this.htmlStyle2.posBottom = (float) value.Value;
            }
        }  

        /// <summary>
        /// Retrieves the type of positioning used for the object.  
        /// </summary>
        public string position        
        { 
            get
            {
                return this.htmlStyle.position;
            }
        }  

		/// <summary>
		/// Sets or retrieves the right position of the object in the units specified by the IHTMLStyle::top attribute.  
		/// </summary>
		public System.Web.UI.WebControls.Unit posRight
        { 
            get
            {
                return new Unit(String.Concat(this.htmlStyle2.posRight, this.top.Type.ToString()));
            }
            set
            {
                this.htmlStyle2.posRight = (float) value.Value;
            }

        }  

        /// <summary>
        /// Removes the given attribute from the object. 
        /// </summary>
        /// <param name="attribute"></param>
        public void RemoveAttribute(string attribute)
        {
            this.htmlStyle.removeAttribute(attribute, 0);
        }

        /// <summary>
        /// Sets the value of the specified attribute. 
        /// </summary>
        /// <remarks>
        /// If the attribute not exists the value wil be created and placed at the first position in the collection.
        /// </remarks>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public void SetAttribute(string attribute, object @value)
        {
            this.htmlStyle.setAttribute(attribute, @value, 0);
        }

        /// <summary>
        /// Sets or retrieves on which side of the object the text will flow. 
        /// </summary>
        public string styleFloat        
        { 
            get
            {
                return this.htmlStyle.styleFloat;
            }
            set
            {
                this.htmlStyle.styleFloat = (value);
            }
        }  
 
        /// <summary>
        /// Sets or retrieves whether the text in the object is left-aligned, 
        /// right-aligned, centered, or justified.  
        /// </summary>
        public System.Web.UI.WebControls.HorizontalAlign textAlign        
        { 
            get
            {
                return (HorizontalAlign) ConvertEnumeration(typeof(HorizontalAlign), this.htmlStyle.textAlign);
            }
            set
            {
                this.htmlStyle.textAlign = (ConvertEnumeration(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves a value that indicates whether the text in the object 
        /// has blink, line-through, overline, or underline decorations.
        /// </summary>
        public string textDecoration        
        { 
            get
            {
                return this.htmlStyle.textDecoration;
            }
            set
            {
                this.htmlStyle.textDecoration = (value);
            }
        }  
  
		/// <summary>
		/// Sets or retrieves a Boolean value that indicates whether the object's 
		/// IHTMLStyle::textDecoration property has a value of "blink." 
		/// </summary>
		public bool textDecorationBlink        
        { 
            get
            {
                return this.htmlStyle.textDecorationBlink;
            }
            set
            {
                this.htmlStyle.textDecorationBlink = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves a Boolean value indicating whether the text in the object has a 
        /// line drawn through it.  
        /// </summary>
        public bool textDecorationLineThrough        
        { 
            get
            {
                return this.htmlStyle.textDecorationLineThrough;
            }
            set
            {
                this.htmlStyle.textDecorationLineThrough = (value);
            }
        }  

		/// <summary>
		/// Sets or retrieves the Boolean value indicating whether the IHTMLStyle::textDecoration 
		/// property for the object has been set to none.  
		/// </summary>
		public bool textDecorationNone        
        { 
            get
            {
                return this.htmlStyle.textDecorationNone;
            }
            set
            {
                this.htmlStyle.textDecorationNone = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves a Boolean value indicating whether the text in the object 
        /// has a line drawn over it.  
        /// </summary>
        public bool textDecorationOverline        
        { 
            get
            {
                return this.htmlStyle.textDecorationOverline;
            }
            set
            {
                this.htmlStyle.textDecorationOverline = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves whether the text in the object is underlined.  
        /// </summary>
        public bool textDecorationUnderline        
        { 
            get
            {
                return this.htmlStyle.textDecorationUnderline;
            }
            set
            {
                this.htmlStyle.textDecorationUnderline = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves the indentation of the first line of text in the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit textIndent        
        { 
            get
            {
                return Unit.Parse(this.htmlStyle.textIndent.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
            set
            {
                this.htmlStyle.textIndent = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the rendering of the text in the object.  
        /// </summary>
        public string textTransform        
        { 
            get
            {
                return this.htmlStyle.textTransform;
            }
            set
            {
                this.htmlStyle.textTransform = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves the position of the object relative to the top 
        /// of the next positioned object in the document hierarchy.  
        /// </summary>
        public System.Web.UI.WebControls.Unit top        
        { 
            get
            {
                string val = this.htmlStyle.top.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.top = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the top of the object.  
        /// </summary>
        public bool topAuto        
        { 
            get
            {
                string val = this.htmlStyle.top.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.top = (value ? "auto" : "");
            }
        }  

        /// <summary>
        /// Sets or retrieves the vertical alignment of the object.  
        /// </summary>
        public System.Web.UI.WebControls.VerticalAlign verticalAlign        
        { 
            get
            {
                return (VerticalAlign) ConvertEnumeration(typeof(VerticalAlign), this.htmlStyle.verticalAlign.ToString());
            }
            set
            {
                this.htmlStyle.verticalAlign = (ConvertEnumeration(value));
            }
        }  

        /// <summary>
        /// Sets or retrieves whether the content of the object is displayed.  
        /// </summary>
        public string visibility        
        { 
            get
            {
                return this.htmlStyle.visibility;
            }
            set
            {
                this.htmlStyle.visibility = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves a value that indicates whether lines are automatically broken inside the object. 
        /// </summary>
        public string whiteSpace        
        { 
            get
            {
                return this.htmlStyle.whiteSpace;
            }
            set
            {
                this.htmlStyle.whiteSpace = (value);
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.
        /// </summary>
        [Description("Sets or retrieves the width of the object.  ")]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit width        
        { 
            get
            {
                string val = this.htmlStyle.width.ToString();
                if (val.Equals("auto"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(val, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.width = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        [Description("Sets or retrieves the width of the object.  ")]
        [DefaultValue(true)]
        public bool widthAuto        
        { 
            get
            {
                string val = this.htmlStyle.width.ToString();
                if (val.Equals("auto"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.width = (value ? "auto" : "");
            }
        } 

        /// <summary>
        /// Sets or retrieves the amount of additional space between words in the object.  
        /// </summary>
        [Description("Sets or retrieves the amount of additional space between words in the object. ")]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit wordSpacing        
        { 
            get
            {
                if (this.htmlStyle.wordSpacing.ToString().ToLower().Equals("normal"))
                {
                    return Unit.Empty;
                } 
                else 
                {
                    return Unit.Parse(this.htmlStyle.wordSpacing.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.wordSpacing = (value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }  

        /// <summary>
        /// 
        /// </summary>
        [Description("Sets or retrieves the amount of additional space between words in the object to the default value. ")]
        [DefaultValue(true)]
        public bool wordSpacingNormal
        { 
            get
            {
                if (this.htmlStyle.wordSpacing.ToString().ToLower().Equals("normal"))
                {
                    return true;
                } 
                else 
                {
                    return false;
                }
            }
            set
            {
                this.htmlStyle.wordSpacing = (value ? "normal" : "");
            }
        }  

        /// <summary>
        ///   
        /// </summary>
        [Description("Sets or retrieves the stacking order of positioned objects as integer value.")]
        [DefaultValue(0)]
        public int zIndex        
        { 
            get
            {
                if (this.htmlStyle.zIndex.ToString().ToLower().Equals("auto"))
                {
                    return 0;
                } 
                else 
                {
                    return Int32.Parse(this.htmlStyle.zIndex.ToString());
                }
            }
            set
            {
                this.htmlStyle.zIndex = (value);
            }
        }
  
        /// <summary>
        /// Sets or retrieves the stacking order of positioned objects as auto value.  
        /// </summary>
        [Description("Sets or retrieves the stacking order of positioned objects as auto value. ")]
        [DefaultValue(true)]
        public bool zIndexAuto
        { 
            get
            {
                return this.htmlStyle.zIndex.ToString().ToLower().Equals("auto");
            }
            set
            {
                this.htmlStyle.zIndex = (value ? "auto" : "");
            }
        }
  
        private string ConvertEnumeration(System.Enum enumeration)
        {
            string enumString = Enum.GetName(enumeration.GetType(), enumeration);
            string s = enumString[0].ToString();
            foreach (char c in enumString.Substring(1))
            {
                if (Char.IsLower(c))
                {
                    s += c.ToString();
                } 
                else 
                {
                    s += String.Concat("-", c.ToString().ToLower());
                }
            }
            return s;
        }

        /// <summary>
        /// Replaces upper chars from enum names into minus signs, ignoring the leading character.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="enumString"></param>
        /// <returns></returns>
        private System.Enum ConvertEnumeration(Type enumType, string enumString)
        {            
            string s = Char.ToUpper(enumString[0]).ToString();
            for (int i = 1; i < enumString.Length; i++)
            {
                Char nextChar = Char.MinValue;
                if (Char.Equals(nextChar, '-'))
                {
                    nextChar = enumString[++i];
                    s += Char.ToUpper(nextChar).ToString();
                } 
                else 
                {
                    nextChar = enumString[i];
                    s += nextChar.ToString();
                }
            }
            return (Enum) Enum.Parse(enumType, s, true);
        }

        /// <summary>
        /// Sets or retrieves the color of the top and left edges of the scroll box and 
        /// scroll arrows of a scroll bar.
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Sets or retrieves the color of the top and left edges of the scroll box and scroll arrows of a scroll bar.")]
        public Color scrollbarArrowColor
        {
            get
            {
                return RgbColorConverter(this.htmlStyle3.scrollbarArrowColor.ToString());
            }
            set
            {
                this.htmlStyle3.scrollbarArrowColor = ColorTranslator.ToHtml(value);
            }
        }
        /// <summary>
        /// Sets or retrieves the color of the main elements of a scroll bar, 
        /// which include the scroll box, track, and scroll arrows.
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Sets or retrieves the color of the main elements of a scroll bar.")]
        public Color scrollbarBaseColor
        {
            get
            {
                return RgbColorConverter(this.htmlStyle3.scrollbarBaseColor.ToString());
            }
            set
            {
                this.htmlStyle3.scrollbarBaseColor = ColorTranslator.ToHtml(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the color of the gutter of a scroll bar.
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Sets or retrieves the color of the gutter of a scroll bar.")]
        public Color scrollbarDarkShadowColor
        {
            get
            {
                return RgbColorConverter(this.htmlStyle3.scrollbarDarkShadowColor.ToString());
            }
            set
            {
                this.htmlStyle3.scrollbarDarkShadowColor = ColorTranslator.ToHtml(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the color of the scroll box and scroll arrows of a scroll bar.
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Sets or retrieves the color of the scroll box and scroll arrows of a scroll bar.")]
        public Color scrollbarFaceColor
        {
            get
            {
                return RgbColorConverter(this.htmlStyle3.scrollbarFaceColor.ToString());
            }
            set
            {
                this.htmlStyle3.scrollbarFaceColor = ColorTranslator.ToHtml(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the color of the top and left edges of the scroll box and 
        /// scroll arrows of a scroll bar.
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Sets or retrieves the color of the top and left edges of the scroll box and scroll arrows of a scroll bar.")]
        public Color scrollbarHighlightColor
        {
            get
            {
                return RgbColorConverter(this.htmlStyle3.scrollbarHighlightColor.ToString());
            }
            set
            {
                this.htmlStyle3.scrollbarHighlightColor = ColorTranslator.ToHtml(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the color of the bottom and right edges of the scroll box and 
        /// scroll arrows of a scroll bar.
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Sets or retrieves the color of the bottom and right edges of the scroll box and scroll arrows of a scroll bar.")]
        public Color scrollbarShadowColor
        {
            get
            {
                return RgbColorConverter(this.htmlStyle3.scrollbarShadowColor.ToString());
            }
            set
            {
                this.htmlStyle3.scrollbarShadowColor = ColorTranslator.ToHtml(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the color of the track element of a scroll bar.
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Sets or retrieves the color of the track element of a scroll bar.")]
        public Color scrollbarTrackColor        
        {
            get
            {
                return RgbColorConverter(this.htmlStyle3.scrollbarTrackColor.ToString());
            }
            set
            {
                this.htmlStyle3.scrollbarTrackColor = ColorTranslator.ToHtml(value);
            }
        }


        /// <summary>
        /// Sets or retrieves whether to break words when the content exceeds the boundaries of its container.
        /// </summary>
        /// <remarks>
        /// This property applies to elements that have layout. An element has layout when it 
        /// is absolutely positioned, is a block element, or is an inline element with a 
        /// specified height or width.
        /// </remarks>
        [Description("Sets or retrieves whether to break words when the content exceeds the boundaries of its container.")]
        public WordWrap wordWrap
        { 
            get
            {
                return (WordWrap) this.ConvertEnumeration(typeof(WordWrap), this.htmlStyle3.wordWrap);
            }
            set
            {
                this.htmlStyle3.wordWrap = this.ConvertEnumeration(value);
            }
        }

        #region ICustomTypeDescriptor Member

        PropertyDescriptorCollection pdc = null;

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] filter) 
        { 
            if (pdc == null)
            {
                PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(GetType(), filter); 
                // notice we use the type here so we don't recurse 
                PropertyDescriptor[] newProps = new PropertyDescriptor[baseProps.Count]; 
                for (int i = 0; i < baseProps.Count; i++) 
                { 
                    newProps[i] = new GuruComponents.Netrix.UserInterface.TypeEditors.CustomPropertyDescriptor(baseProps[i], filter); 
                } 
                // probably wanna cache this... 
                pdc = new PropertyDescriptorCollection(newProps); 
            }
            return pdc;
        } 
 
        AttributeCollection ICustomTypeDescriptor.GetAttributes() 
        { 
            return TypeDescriptor.GetAttributes(this, true); 
        } 
 
        string ICustomTypeDescriptor.GetClassName() 
        { 
            return "CSS Style"; 
        } 
 
        string ICustomTypeDescriptor.GetComponentName() 
        { 
            return TypeDescriptor.GetComponentName(this, true); 
        } 
 
        TypeConverter ICustomTypeDescriptor.GetConverter() 
        { 
            return TypeDescriptor.GetConverter(this, true); 
        } 
 
        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() 
        { 
            return TypeDescriptor.GetDefaultEvent(this, true); 
        } 
 
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(System.Attribute[] attributes) 
        { 
            return TypeDescriptor.GetEvents(this, attributes, true); 
        } 
 
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents() 
        { 
            return TypeDescriptor.GetEvents(this, true); 
        } 
 
        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() 
        { 
            return TypeDescriptor.GetDefaultProperty(this, true); 
        } 
 
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() 
        { 
            return TypeDescriptor.GetProperties(this, true); 
        } 
 
        object ICustomTypeDescriptor.GetEditor(System.Type editorBaseType) 
        { 

            return TypeDescriptor.GetEditor(this, editorBaseType, true); 
        } 
 
        object ICustomTypeDescriptor.GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd) 
        { 
            return this; 
        } 
 
        #endregion

	}
}
