using System;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
	/// This class represents the effective style definition.
	/// </summary>
	/// <remarks>
	/// A style definition is either an inline style (attribute style of any stylable element) or the
	/// definition part of a style rule. A rule is a combination of a descriptor and a style.
	/// <para>
	/// The purpose of this class is to have an isolated store for the effective CSS definitions. 
	/// This behaves slightly different from the styles defined as part of the elements. The native class CssStyle allows access
	/// to style definitions using the <see cref="System.Windows.Forms.PropertyGrid">PropertyGrid</see> and
	/// not the NetRix StyleEditor. 
	/// </para>
	/// <para>
	/// The effective Style is readonly and presents the cascaded styles including all global or external
	/// definition as well as the inline styles at the selected node.
	/// </para>
	/// </remarks>
    public class CssEffectiveStyle : IEffectiveStyle, ICustomTypeDescriptor
    {

        private class StyleDescriptor : System.ComponentModel.PropertyDescriptor
        {

            private PropertyDescriptor baseProp;
            Attribute[] _filter;

            public StyleDescriptor(PropertyDescriptor pd, Attribute[] filter) : base(pd)
            {
                baseProp = pd;             
                _filter = filter;
            }

            public override string Category
            {
                get
                {
                    return "Style";
                }
            }

            public override bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            public override string DisplayName
            {
                get
                {
                    return String.Concat(baseProp.Name[0].ToString().ToUpper(), baseProp.Name.Substring(1));
                }
            }

            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get
                {
                    return baseProp.ComponentType;
                }
            }

            public override object GetValue(object component)
            {
                return this.baseProp.GetValue(component); 
            }

            public override Type PropertyType
            {
                get
                {
                    return baseProp.PropertyType;
                }
            }

            public override void ResetValue(object component)
            {
                baseProp.ResetValue(component);
            }

            public override void SetValue(object component, object value)
            {
                baseProp.SetValue(component, value);
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }

        }


        private Interop.IHTMLCurrentStyle htmlStyle;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="hs"></param>
        public CssEffectiveStyle(Interop.IHTMLCurrentStyle hs)
        {
            this.htmlStyle = hs;
        }

        # region Subclasses

        /// <summary>
        /// Holds one background style definition.
        /// </summary>
        public class BackgroundStyles : IBackgroundStyles
        {
            private string attachment;
            private System.Drawing.Color color;
            private string image;
            private BackgroundRepeat repeat;
            private BackgroundPosition position;
            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="a"></param>
            /// <param name="c"></param>
            /// <param name="i"></param>
            /// <param name="r"></param>
            /// <param name="p"></param>
            public BackgroundStyles(string a, Color c, string i, BackgroundRepeat r, BackgroundPosition p)
            {
                attachment = a;
                color = c;
                image = i;
                repeat = r;
                position = p;
            }
            /// <summary>
            /// Attachment
            /// </summary>
            public string Attachment
            {
                get
                {
                    return attachment;
                }
            }
            /// <summary>
            /// Color
            /// </summary>
            public System.Drawing.Color Color
            {
                get
                {
                    return color;
                }
            }
            /// <summary>
            /// Image
            /// </summary>
            public string Image
            {
                get
                {
                    return image;
                }
            }
            /// <summary>
            /// Repeat
            /// </summary>
            public BackgroundRepeat Repeat
            {
                get
                {
                    return repeat;
                }
            }


            /// <summary>
            /// Retrieves the x-coordinate and y-coordinate of the backgroundPosition property.  
            /// </summary>
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public IBackgroundPosition Position
            { 
                get
                {
                    return position;
                }
            }  
            /// <summary>
            /// Not implemented.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "";
            }

        }

        /// <summary>
        /// Holds one background position definition.
        /// </summary>
        public class BackgroundPosition : IBackgroundPosition
        {
            private System.Web.UI.WebControls.Unit _X;
            private System.Web.UI.WebControls.Unit _Y;

            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public BackgroundPosition(System.Web.UI.WebControls.Unit x, System.Web.UI.WebControls.Unit y)
            {
                _X = x;
                _Y = y;
            }
            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public System.Web.UI.WebControls.Unit X
            {
                get
                {
                    return _X;
                }
            }

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public System.Web.UI.WebControls.Unit Y
            {
                get
                {
                    return _Y;
                }
            }

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public override string ToString()
            {
                return "";
            }

        }

        /// <summary>
        /// Holds one border property definition.
        /// </summary>
        public class Border : IBorderEffective
        {
            /// <summary>
            /// Holds the properties of a border definition.
            /// </summary>
            /// <remarks>
            /// The properties are built using Color, Style and Width values.
            /// </remarks>
            public struct BorderProperties : IBorderProperties
            {

                private System.Drawing.Color color;
                private System.Web.UI.WebControls.BorderStyle style;
                private string width;

                /// <summary>
                /// TODO: Add comment.
                /// </summary>
                public BorderProperties(Color c, System.Web.UI.WebControls.BorderStyle s, string w)
                {
                    color = c;
                    style = s;
                    width = w;
                }

                /// <summary>
                /// TODO: Add comment.
                /// </summary>
                public System.Drawing.Color BorderColor
                {
                    get
                    {
                        return color;
                    }
                }
                /// <summary>
                /// TODO: Add comment.
                /// </summary>
                public System.Web.UI.WebControls.BorderStyle BorderStyle
                {
                    get
                    {
                        return style;
                    }
                }
                /// <summary>
                /// TODO: Add comment.
                /// </summary>
                public string BorderWidth
                {
                    get
                    {
                        return width;
                    }
                }

                /// <summary>
                /// TODO: Add comment.
                /// </summary>
                public override string ToString()
                {
                    return "";
                }
            }

            private BorderProperties left;
            private BorderProperties right;
            private BorderProperties top;
            private BorderProperties bottom;

            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="lc"></param>
            /// <param name="rc"></param>
            /// <param name="tc"></param>
            /// <param name="bc"></param>
            /// <param name="ls"></param>
            /// <param name="rs"></param>
            /// <param name="ts"></param>
            /// <param name="bs"></param>
            /// <param name="lw"></param>
            /// <param name="rw"></param>
            /// <param name="tw"></param>
            /// <param name="bw"></param>
            public Border(Color lc, Color rc, Color tc, Color bc, BorderStyle ls, BorderStyle rs, BorderStyle ts, BorderStyle bs, string lw, string rw, string tw, string bw)
            {
                left = new BorderProperties(lc, ls, lw);
                right = new BorderProperties(rc, rs, rw);
                top = new BorderProperties(tc, ts, tw);
                bottom = new BorderProperties(bc, bs, bw);
            }
            /// <summary>
            /// Left border style property.
            /// </summary>
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public IBorderProperties Left
            {
                get
                {
                    return left;
                }
            }
            /// <summary>
            /// Right border style property.
            /// </summary>
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public IBorderProperties Right
            {
                get
                {
                    return right;
                }
            }
            /// <summary>
            /// Top border style property.
            /// </summary>
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public IBorderProperties Top
            {
                get
                {
                    return top;
                }
            }
            /// <summary>
            /// Bottom border style property.
            /// </summary>
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public IBorderProperties Bottom
            {
                get
                {
                    return bottom;
                }
            }

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public override string ToString()
            {
                return "";
            }

        }

        /// <summary>
        /// Holds the style clip definition. 
        /// </summary>
        /// <remarks>
        /// The clip is a rectangle, which is defined using a left, right, top and bottom value. 
        /// </remarks>
        public class Clip : IClipEffective
        {
            private string left;
            private string top;
            private string right;
            private string bottom;

            internal Clip(string l, string r, string t, string b)
            {
                this.left = l;
                this.top = t;
                this.right = r;
                this.bottom = b;
            }
            /// <summary>
            /// Left clip style property.
            /// </summary>
            public string Left
            {
                get
                {
                    return left;
                }
            }
            /// <summary>
            /// Top clip style property.
            /// </summary>
            public string Top
            {
                get
                {
                    return top;
                }
            }

            /// <summary>
            /// Right clip style property.
            /// </summary>
            public string Right
            {
                get
                {
                    return right;
                }
            }

            /// <summary>
            /// Bottom clip style property.
            /// </summary>
            public string Bottom
            {
                get
                {
                    return bottom;
                }
            }

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public override string ToString()
            {
                return "Clip";
            }

        }

        /// <summary>
        /// Holds a font definition.
        /// </summary>
        /// <remarks>
        /// The font is defined using Font Family, Size, Style, Variant and Weight.
        /// </remarks>
        public class Font : IFontEffective
        {
            private string fontFamily;
            private Unit fontSize;
            private string fontStyle;
            private string fontVariant;
            private string fontWeight;
            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="ff"></param>
            /// <param name="fs"></param>
            /// <param name="fst"></param>
            /// <param name="fv"></param>
            /// <param name="fw"></param>
            public Font(string ff, Unit fs, string fst, string fv, string fw)
            {
                fontFamily = ff;
                fontSize = fs;
                fontStyle = fst;
                fontVariant = fv;
                fontWeight = fw;
            }

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public string FontFamily
            {
                get
                {
                    return fontFamily;
                }
            }
            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public Unit FontSize
            {
                get
                {
                    return fontSize;
                }
            }

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public string FontStyle
            {
                get
                {
                    return fontStyle;
                }
            }

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public string FontVariant
            {
                get
                {
                    return fontVariant;
                }
            }

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public string FontWeight
            {
                get
                {
                    return fontWeight;
                }
            }

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public override string ToString()
            {
                return "";
            }


        }

        /// <summary>
        /// Used to handle properties build with four values.
        /// </summary>
        /// <remarks>
        /// This class is used to hold the values for left, right, top, and bottom used by the
        /// effective style definition.
        /// </remarks>
        public class FourProperties : IFourProperties
        {

            private string lm, rm, tm, bm;

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public FourProperties(string l, string r, string t, string b)
            {
                lm = l;
                rm = r;
                tm = t;
                bm = b;
            }

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public string Left
            {
                get
                {
                    return lm;
                }
            }
            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public string Right
            {
                get
                {
                    return rm;
                }
            }
            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public string Top
            {
                get
                {
                    return tm;
                }
            }
            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public string Bottom
            {
                get
                {
                    return bm;
                }
            }

            /// <summary>
            /// TODO: Add comment.
            /// </summary>
            public override string ToString()
            {
                return "";
            }


        }


        # endregion

        /// <summary>
        /// Retrieves how the background is built. 
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IBackgroundStyles background
        { 
            get
            {
                return new BackgroundStyles(
                    this.htmlStyle.backgroundAttachment,
                    RgbColorConverter(this.htmlStyle.backgroundColor.ToString()),
                    this.htmlStyle.backgroundImage,
                    (BackgroundRepeat) ConvertEnumeration(typeof(BackgroundRepeat), this.htmlStyle.backgroundRepeat),
                    new BackgroundPosition(Unit.Parse(this.htmlStyle.backgroundPositionX.ToString(), System.Globalization.CultureInfo.InvariantCulture), Unit.Parse(this.htmlStyle.backgroundPositionY.ToString(), System.Globalization.CultureInfo.InvariantCulture))
                    );
            }
        } 
  
        /// <summary>
        /// Retrieves the border properties of the object.  
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IBorderEffective border
        { 
            get
            {
                return new Border(
                    RgbColorConverter(this.htmlStyle.borderLeftColor.ToString()),
                    RgbColorConverter(this.htmlStyle.borderRightColor.ToString()),
                    RgbColorConverter(this.htmlStyle.borderTopColor.ToString()),
                    RgbColorConverter(this.htmlStyle.borderBottomColor.ToString()),

                    (System.Web.UI.WebControls.BorderStyle) ConvertEnumeration(typeof(System.Web.UI.WebControls.BorderStyle), this.htmlStyle.borderLeftStyle),
                    (System.Web.UI.WebControls.BorderStyle) ConvertEnumeration(typeof(System.Web.UI.WebControls.BorderStyle), this.htmlStyle.borderRightStyle),
                    (System.Web.UI.WebControls.BorderStyle) ConvertEnumeration(typeof(System.Web.UI.WebControls.BorderStyle), this.htmlStyle.borderTopStyle),
                    (System.Web.UI.WebControls.BorderStyle) ConvertEnumeration(typeof(System.Web.UI.WebControls.BorderStyle), this.htmlStyle.borderBottomStyle),

                    this.htmlStyle.borderLeftWidth.ToString(),
                    this.htmlStyle.borderRightWidth.ToString(),
                    this.htmlStyle.borderTopWidth.ToString(),
                    this.htmlStyle.borderBottomWidth.ToString()
                    );
            }
        }  

        /// <summary>
        /// Retrieves whether the object allows floating objects on its left side, right side, or both, so that the next text displays past the floating objects. 
        /// </summary>
        public ClearStyles clear        
        { 
            get
            {
                return (ClearStyles) ConvertEnumeration(typeof(ClearStyles), this.htmlStyle.clear);
            }
        }  

      
        /// <summary>
        /// Retrieves which part of a positioned object is visible.  
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property defines the shape and size of the positioned object that is visible. The Horizontalposition must be set to absolute. Any part of the object that is outside the clipping region is transparent. Any coordinate can be replaced by the value auto, which exposes the respective side (that is, the side is not clipped).
        /// </para>
        /// <para>
        /// The order of the values rect(0 0 50 50) renders the object invisible because it sets the top and right positions of the clipping region to 0. To achieve a 50-by-50 view port, use rect(0 50 50 0).
        /// </para>
        /// </remarks>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [ReadOnly(true)]
        public IClipEffective clip        
        { 
            get
            {
                Clip cs = new Clip(this.htmlStyle.clipLeft.ToString(), this.htmlStyle.clipRight.ToString(), this.htmlStyle.clipTop.ToString(), this.htmlStyle.clipBottom.ToString());
                return cs;
            }
        }  

        /// <summary>
        /// Retrieves the color of the text of the object.  
        /// </summary>
        /// <remarks>
        /// Some browsers do not recognize color names, but all browsers should recognize RGB color values and display them correctly.
        /// Therefore the property returns always RGB values, even if the value was previously set using names. This is a different behavior compared to the original MSHTML.
        /// </remarks>
        public Color color        
        { 
            get
            {
                return RgbColorConverter(this.htmlStyle.color.ToString());
            }
        }

        private static Color RgbColorConverter(string cssColor)
        {
            if (cssColor.StartsWith("rgb("))
            {
                string[] rgb = cssColor.Substring(4, cssColor.Length - 5).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                cssColor = String.Format("#{0:x2}{1:x2}{2:x2}",
                    Int32.Parse(rgb[0], System.Globalization.NumberStyles.Number),
                    Int32.Parse(rgb[1], System.Globalization.NumberStyles.Number),
                    Int32.Parse(rgb[2], System.Globalization.NumberStyles.Number)
                    );
            }
            return ColorTranslator.FromHtml(cssColor);
        }

        /// <summary>
        /// Retrieves the type of cursor to display as the mouse pointer moves over the object.  
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
        }  

        /// <summary>
        /// Retrieves whether the object is rendered. 
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
        }  

		private static int ConvertUnitToPoint(GuruComponents.Netrix.WebEditing.FontUnit unit)
		{
			switch (unit)
			{
				case GuruComponents.Netrix.WebEditing.FontUnit.XXSmall:
					return 8;
				case GuruComponents.Netrix.WebEditing.FontUnit.XSmall:
					return 10;
				case GuruComponents.Netrix.WebEditing.FontUnit.Small:
					return 12;
				case GuruComponents.Netrix.WebEditing.FontUnit.Medium:
					return 14;
				case GuruComponents.Netrix.WebEditing.FontUnit.Large:
					return 18;
				case GuruComponents.Netrix.WebEditing.FontUnit.XLarge:
					return 20;
				case GuruComponents.Netrix.WebEditing.FontUnit.XXLarge:
					return 24;
			}
			return 0;
		}

        /// <summary>
        /// Retrieves the font and various font options used for text in the object.  
        /// </summary>
        /// <remarks>
        /// <para>The value is a prioritized list of font family names and generic family names. List items are separated by commas to minimize confusion between multiple-word font family names. If the font family name contains white space, it should appear in single or double quotation marks; generic font family names are values and cannot appear in quotation marks. </para>
        /// <para>Because you do not know which fonts users have installed, you should provide a list of alternatives with a generic font family at the end of the list. This list can include embedded fonts. For more information about embedding fonts, see the @font-face rule.</para>
        /// </remarks>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IFontEffective font
        { 
            get
            {
                // assume, that a fontsize of value "2" means HTML <font> tag size values
				Unit fsz = Unit.Empty;
				try
				{
                    if (this.htmlStyle.fontSize != null)
                    {
                        switch (this.htmlStyle.fontSize.ToString().ToLower())
                        {
                            case "xx-small":
                            case "xxsmall":
                                fsz = new Unit((double)ConvertUnitToPoint(FontUnit.XXSmall), UnitType.Point);
                                break;
                            case "x-small":
                            case "xsmall":
                                fsz = new Unit((double)ConvertUnitToPoint(FontUnit.XSmall), UnitType.Point);
                                break;
                            case "medium":
                                fsz = new Unit((double)ConvertUnitToPoint(FontUnit.Medium), UnitType.Point);
                                break;
                            case "large":
                                fsz = new Unit((double)ConvertUnitToPoint(FontUnit.Large), UnitType.Point);
                                break;
                            case "x-large":
                            case "xlarge":
                                fsz = new Unit((double)ConvertUnitToPoint(FontUnit.XLarge), UnitType.Point);
                                break;
                            case "xx-large":
                            case "xxlarge":
                                fsz = new Unit((double)ConvertUnitToPoint(FontUnit.XXLarge), UnitType.Point);
                                break;
                            case "smaller":
                                fsz = new Unit((double)ConvertUnitToPoint(FontUnit.Smaller), UnitType.Point);
                                break;
                            case "larger":
                                fsz = new Unit((double)ConvertUnitToPoint(FontUnit.Larger), UnitType.Point);
                                break;
                            case "small":
                                fsz = new Unit((double)ConvertUnitToPoint(FontUnit.Small), UnitType.Point);
                                break;
                            default:                                
                                fsz = Unit.Parse(this.htmlStyle.fontSize.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                if (fsz.Type == UnitType.Pixel && !this.htmlStyle.fontSize.ToString().EndsWith("px"))
                                {
                                    fsz = new Unit((double)ConvertUnitToPoint((FontUnit)(int)fsz.Value), UnitType.Point);
                                }
                                break;
                        }
                        return new CssEffectiveStyle.Font(
                            this.htmlStyle.fontFamily,
                            fsz,
                            this.htmlStyle.fontStyle,
                            this.htmlStyle.getAttribute("font-variant", 0).ToString(),
                            this.htmlStyle.fontWeight.ToString()
                            );
                    }
				}
				catch
				{
					fsz = new Unit((double)ConvertUnitToPoint(FontUnit.Medium), UnitType.Point);
				}
                return new CssEffectiveStyle.Font(
                    "Times New Roman",
                    fsz,
                    "Normal",
                    "Normal",
                    "Normal"
                    );
            }
        }   

        /// <summary>
        /// Retrieves the height of the object.  
        /// </summary>
        public string height
        { 
            get
            {
                return this.htmlStyle.height.ToString();
            }
        }  

        /// <summary>
        /// Retrieves the position of the object relative to the left edge of the next positioned object in the document hierarchy.  
        /// </summary>
        public string left        
        { 
            get
            {
                return this.htmlStyle.left.ToString();
            }
        }  

        /// <summary>
        /// Retrieves the amount of additional space between letters in the object.  
        /// </summary>
        public string letterSpacing        
        { 
            get
            {
                return this.htmlStyle.letterSpacing.ToString();
            }
        }  

        /// <summary>
        /// Retrieves the distance between lines in the object.  
        /// </summary>
        public string lineHeight        
        { 
            get
            {
                return this.htmlStyle.lineHeight.ToString();
            }
        }  

        /// <summary>
        /// Retrieves a value that indicates which image to use as a list-item marker for the object.  
        /// </summary>
        public string listStyleImage        
        { 
            get
            {
                return this.htmlStyle.listStyleImage;
            }
        }  

        /// <summary>
        /// Retrieves a variable that indicates how the list-item marker is drawn relative to the content of the object.  
        /// </summary>
        public ListStylePosition listStylePosition        
        { 
            get
            {
                return (ListStylePosition) ConvertEnumeration(typeof(ListStylePosition), this.htmlStyle.listStylePosition);
            }
        }  

        /// <summary>
        /// Retrieves the predefined type of the line-item marker for the object.  
        /// </summary>
        public ListStyleType listStyleType        
        { 
            get
            {
                return (ListStyleType) ConvertEnumeration(typeof(ListStyleType), this.htmlStyle.listStyleType);
            }
        }
  
        /// <summary>
        /// Retrieves the margins of the object.  
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IFourProperties margin
        { 
            get
            {
                return new FourProperties(this.htmlStyle.marginLeft.ToString(), this.htmlStyle.marginRight.ToString(), this.htmlStyle.marginTop.ToString(), this.htmlStyle.marginBottom.ToString());
            }
        }  

        /// <summary>
        /// Retrieves a value indicating how to manage the content of the object when the content exceeds the height or width of the object. 
        /// </summary>
        public string overflow        
        { 
            get
            {
                return this.htmlStyle.overflow;
            }
        }  

        /// <summary>
        /// Retrieves the amount of space to insert between the border of the object and the content.  
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IFourProperties padding 
        { 
            get
            {
                return new FourProperties(
                    this.htmlStyle.paddingLeft.ToString(),
                    this.htmlStyle.paddingRight.ToString(),
                    this.htmlStyle.paddingTop.ToString(),
                    this.htmlStyle.paddingBottom.ToString());
            }
        }  

        /// <summary>
        /// Retrieves a value indicating whether a page break occurs after the object.  
        /// </summary>
        public PageBreakStyles pageBreakAfter        
        { 
            get
            {
                return (PageBreakStyles) ConvertEnumeration(typeof(PageBreakStyles), this.htmlStyle.pageBreakAfter);
            }
        }  

        /// <summary>
        /// Retrieves a string indicating whether a page break occurs before the object. 
        /// </summary>
        public PageBreakStyles pageBreakBefore        
        { 
            get
            {
                return (PageBreakStyles) ConvertEnumeration(typeof(PageBreakStyles), this.htmlStyle.pageBreakBefore);
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
        /// Retrieves on which side of the object the text will flow. 
        /// </summary>
        public string styleFloat        
        { 
            get
            {
                return this.htmlStyle.styleFloat;
            }
        }  
 
        /// <summary>
        /// Retrieves whether the text in the object is left-aligned, right-aligned, centered, or justified.  
        /// </summary>
        public System.Web.UI.WebControls.HorizontalAlign textAlign        
        { 
            get
            {
                return (System.Web.UI.WebControls.HorizontalAlign)ConvertEnumeration(typeof(System.Web.UI.WebControls.HorizontalAlign), this.htmlStyle.textAlign);
            }
        }  

        /// <summary>
        /// Retrieves a value that indicates whether the text in the object has blink, line-through, overline, or underline decorations.
        /// </summary>
        public string textDecoration        
        { 
            get
            {
                return this.htmlStyle.textDecoration;
            }
        }  
  
        /// <summary>
        /// Retrieves the indentation of the first line of text in the object.  
        /// </summary>
        public System.Web.UI.WebControls.Unit textIndent        
        { 
            get
            {
                return Unit.Parse(this.htmlStyle.textIndent.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
        }  

        /// <summary>
        /// Retrieves the position of the object relative to the top of the next positioned object in the document hierarchy.  
        /// </summary>
        public string top        
        { 
            get
            {
                return this.htmlStyle.top.ToString();
            }
        }  

        /// <summary>
        /// Retrieves the vertical alignment of the object.  
        /// </summary>
        public System.Web.UI.WebControls.VerticalAlign verticalAlign        
        { 
            get
            {
                return (VerticalAlign) ConvertEnumeration(typeof(VerticalAlign), this.htmlStyle.verticalAlign.ToString());
            }
        }  

        /// <summary>
        /// Retrieves whether the content of the object is displayed.  
        /// </summary>
        public string visibility        
        { 
            get
            {
                return this.htmlStyle.visibility;
            }
        }  

        /// <summary>
        /// Retrieves the width of the object.  
        /// </summary>
        public string width        
        { 
            get
            {
                return this.htmlStyle.width.ToString();
            }
        }  

        /// <summary>
        /// Retrieves the stacking order of positioned objects.  
        /// </summary>
        public string zIndex        
        { 
            get
            {
                return this.htmlStyle.zIndex.ToString();
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
            if (enumString.ToLower().Equals("auto")) return (Enum)Enum.Parse(enumType, "NotSet", true);
            string s = Char.ToUpper(enumString[0]).ToString();
            for (int i = 1; i < enumString.Length; i++)
            {
                Char nextChar = enumString[i];
                if (Char.Equals(nextChar, '-'))
                {
                    nextChar = enumString[++i];
                    s += Char.ToUpper(nextChar).ToString();
                } 
                else 
                {
                    s += nextChar.ToString();
                }
            }
            try
            {
                return (Enum) Enum.Parse(enumType, s, true);
            }
            catch
            {
                return (Enum) Enum.Parse(enumType, "NotSet", true);
            }
        }

        /// <summary>
        /// Design time support
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Click + to see styles";
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
                    newProps[i] = new CssEffectiveStyle.StyleDescriptor(baseProps[i], filter);
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
