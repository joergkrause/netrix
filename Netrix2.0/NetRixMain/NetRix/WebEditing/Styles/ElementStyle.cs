using System;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using TE = GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.UserInterface.TypeEditors;

namespace GuruComponents.Netrix.WebEditing.Styles
{
    /// <summary>
    /// This class represents one style definition.
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
    public class ElementStyle : IElementStyle, ICustomTypeDescriptor
    {

        private Interop.IHTMLStyle htmlStyle;
        private Background backgroundStyle;
        private Border borderStyle;
        private Font fontStyle;
        private Margin marginStyle;
        private Padding paddingStyle;
        private TextDecoration textDecorationStyle;

        /// <summary>
        /// Creates a new instance based on the native element's style object.
        /// </summary>
        /// <param name="hs"></param>
        public ElementStyle(Interop.IHTMLStyle hs)
        {
            this.htmlStyle = hs;
            backgroundStyle = new Background(hs);
            borderStyle = new Border(hs);
            fontStyle = new Font(hs);
            paddingStyle = new Padding(hs);
            marginStyle = new Margin(hs);
            textDecorationStyle = new TextDecoration(hs);
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IBackground BackgroundStyle
        {
            get { return backgroundStyle; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IBorder BorderStyle
        {
            get { return borderStyle; }
        }

        public class Background : GuruComponents.Netrix.WebEditing.Styles.IBackground
        {

            private Interop.IHTMLStyle htmlStyle;

            internal Background(Interop.IHTMLStyle htmlStyle)
            {
                this.htmlStyle = htmlStyle;
            }

            public override string ToString()
            {
                return "";
            }

            /// <summary>
            /// Sets or retrieves up to five separate background properties of the object. 
            /// </summary>
            [DefaultValue(typeof(BackgroundStyles), "None")]
            public BackgroundStyles background
            {
                get
                {
                    string bgs = htmlStyle.GetBackground();
                    if (bgs != null)
                    {
                        return (BackgroundStyles)Enum.Parse(typeof(BackgroundStyles), bgs, true);
                    }
                    else
                    {
                        return BackgroundStyles.None;
                    }
                }
                set
                {
                    htmlStyle.SetBackground(Enum.GetName(typeof(BackgroundStyles), value));
                }
            }
            /// <summary>
            /// Sets or retrieves how the background image is attached to the object within the document. 
            /// </summary>
            [DefaultValue("")]
            public string backgroundAttachment
            {
                get
                {
                    return (this.htmlStyle.GetBackgroundAttachment() == null) ? "" : this.htmlStyle.GetBackgroundAttachment();
                }
                set
                {
                    this.htmlStyle.SetBackgroundAttachment(value);
                }
            }
            /// <summary>
            /// Sets or retrieves the color behind the content of the object.  
            /// </summary>
            [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
            [EditorAttribute(
                 typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
                 typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Color), "Empty")]
            public Color backgroundColor
            {
                get
                {
                    object o = this.htmlStyle.GetBackgroundColor();
                    return (o == null) ? Color.Empty : RgbColorConverter(o.ToString());
                }
                set
                {
                    this.htmlStyle.SetBackgroundColor(ColorTranslator.ToHtml(value));
                }
            }
            /// <summary>
            /// Sets or retrieves the background image of the object.  
            /// </summary>
            [DefaultValue("")]
            public string backgroundImage
            {
                get
                {
                    return (this.htmlStyle.GetBackgroundImage() == null) ? "" : this.htmlStyle.GetBackgroundImage();
                }
                set
                {
                    this.htmlStyle.SetBackgroundImage(value);
                }
            }

            /// <summary>
            /// Sets or retrieves the position of the background of the object.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit backgroundPositionUnit
            {
                get
                {
                    string o = this.htmlStyle.GetBackgroundPosition();
                    if (o == null)
                    {
                        return Unit.Empty;
                    }
                    else
                    {
                        return Unit.Parse(o, System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                set
                {
                    this.htmlStyle.SetBackgroundPosition(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the position of the background of the object.  
            /// </summary>
            [DefaultValue(typeof(VerticalAlign), "NotSet")]
            public System.Web.UI.WebControls.VerticalAlign backgroundPositionVerticalAlign
            {
                get
                {
                    if (this.htmlStyle.GetBackgroundPosition() != null)
                    {
                        VerticalAlign va = (VerticalAlign)Enum.Parse(typeof(VerticalAlign), this.htmlStyle.GetBackgroundPosition(), true);
                        return va;
                    }
                    else
                    {
                        return VerticalAlign.NotSet;
                    }
                }
                set
                {
                    this.htmlStyle.SetBackgroundPosition(Enum.GetName(typeof(VerticalAlign), value));
                }
            }

            /// <summary>
            /// Sets or retrieves the position of the background of the object.  
            /// </summary>
            [DefaultValue(typeof(HorizontalAlign), "NotSet")]
            public System.Web.UI.WebControls.HorizontalAlign backgroundPositionHorizontalAlign
            {
                get
                {
                    if (this.htmlStyle.GetBackgroundPosition() != null)
                    {
                        HorizontalAlign ha = (HorizontalAlign)Enum.Parse(typeof(HorizontalAlign), this.htmlStyle.GetBackgroundPosition(), true);
                        return ha;
                    }
                    else
                    {
                        return HorizontalAlign.NotSet;
                    }
                }
                set
                {
                    this.htmlStyle.SetBackgroundPosition(Enum.GetName(typeof(HorizontalAlign), value));
                }
            }


            /// <summary>
            /// Sets or retrieves the x-coordinate of the backgroundPosition property.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit backgroundPositionX
            {
                get
                {
                    if (this.htmlStyle.GetBackgroundPositionX() != null)
                    {
                        return Unit.Parse(this.htmlStyle.GetBackgroundPositionX().ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        return Unit.Empty;
                    }
                }
                set
                {
                    this.htmlStyle.SetBackgroundPositionX(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the y-coordinate of the backgroundPosition property.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit backgroundPositionY
            {
                get
                {
                    if (this.htmlStyle.GetBackgroundPositionY() != null)
                    {
                        return Unit.Parse(this.htmlStyle.GetBackgroundPositionY().ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        return Unit.Empty;
                    }
                }
                set
                {
                    this.htmlStyle.SetBackgroundPositionY(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }


            /// <summary>
            /// Sets or retrieves how the backgroundImage property of the object is tiled. 
            /// </summary>
            [DefaultValue(typeof(BackgroundRepeat), "NotRepeat")]
            public BackgroundRepeat backgroundRepeat
            {
                get
                {
                    if (this.htmlStyle.GetBackgroundRepeat() != null)
                    {
                        return (BackgroundRepeat)ElementStyle.ConvertEnumeration(typeof(BackgroundRepeat), this.htmlStyle.GetBackgroundRepeat());
                    }
                    else
                    {
                        return BackgroundRepeat.NoRepeat;
                    }
                }
                set
                {
                    this.htmlStyle.SetBackgroundRepeat(value.ToString());
                }
            }

        }

        public class Border : GuruComponents.Netrix.WebEditing.Styles.IBorder
        {

            private Interop.IHTMLStyle htmlStyle;

            internal Border(Interop.IHTMLStyle htmlStyle)
            {
                this.htmlStyle = htmlStyle;
            }

            public override string ToString()
            {
                return "";
            }
            /// <summary>
            /// Sets or retrieves the properties to draw around the object. 
            /// </summary>
            [DefaultValue("")]
            public string border
            {
                get
                {
                    return (this.htmlStyle.GetBorder() == null) ? "" : this.htmlStyle.GetBorder();
                }
                set
                {
                    this.htmlStyle.SetBorder(value);
                }
            }

            /// <summary>
            /// Sets or retrieves the properties of the bottom border of the object. 
            /// </summary>
            [DefaultValue("")]
            public string borderBottom
            {
                get
                {
                    return (this.htmlStyle.GetBorderBottom() == null) ? "" : this.htmlStyle.GetBorderBottom();
                }
                set
                {
                    this.htmlStyle.SetBorderBottom(value);
                }
            }

            /// <summary>
            /// Sets or retrieves the color of the bottom border of the object.  
            /// </summary>
            [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
            [EditorAttribute(
                 typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
                 typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Color), "Empty")]
            public Color borderBottomColor
            {
                get
                {
                    if (this.htmlStyle.GetBorderBottomColor() != null)
                    {
                        return RgbColorConverter(this.htmlStyle.GetBorderBottomColor().ToString());
                    }
                    else
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    this.htmlStyle.SetBorderBottomColor(ColorTranslator.ToHtml(value));
                }
            }

            /// <summary>
            /// Sets or retrieves the style of the bottom border of the object.  
            /// </summary>
            [DefaultValue(typeof(BorderStyle), "NotSet")]
            public System.Web.UI.WebControls.BorderStyle borderBottomStyle
            {
                get
                {
                    if (this.htmlStyle.GetBorderStyle() != null)
                        return (System.Web.UI.WebControls.BorderStyle)ElementStyle.ConvertEnumeration(typeof(System.Web.UI.WebControls.BorderStyle), this.htmlStyle.GetBorderStyle());
                    else
                        return System.Web.UI.WebControls.BorderStyle.NotSet;
                }
                set
                {
                    this.htmlStyle.SetBorderStyle(value.ToString());
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the bottom border of the object.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit borderBottomWidth
            {
                get
                {
                    object val = this.htmlStyle.GetBorderBottomWidth();
                    if (val == null || val.ToString().Equals("auto"))
                    {
                        return Unit.Empty;
                    }
                    else
                    {
                        return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                set
                {
                    this.htmlStyle.SetBorderBottomWidth(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the object.  
            /// </summary>
            [DefaultValue(true)]
            public bool borderBottomWidthAuto
            {
                get
                {
                    object val = this.htmlStyle.GetBorderBottomWidth();
                    if (val == null || val.ToString().Equals("auto"))
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
                    this.htmlStyle.SetBorderBottomWidth(value ? "auto" : "");
                }
            }

            /// <summary>
            /// Sets or retrieves the border color of the object. 
            /// </summary>
            [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
            [EditorAttribute(
                 typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
                 typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Color), "Empty")]
            public Color borderColor
            {
                get
                {
                    if (this.htmlStyle.GetBorderColor() != null)
                        return RgbColorConverter(this.htmlStyle.GetBorderColor().ToLower());
                    else
                        return Color.Empty;
                }
                set
                {
                    this.htmlStyle.SetBorderColor(ColorTranslator.ToHtml(value));
                }
            }

            /// <summary>
            /// Sets or retrieves the properties of the left border of the object. 
            /// </summary>
            [DefaultValue("")]
            public string borderLeft
            {
                get
                {
                    return (this.htmlStyle.GetBorderLeft() == null) ? "" : this.htmlStyle.GetBorderLeft();
                }
                set
                {
                    this.htmlStyle.SetBorderLeft(value);
                }
            }

            /// <summary>
            /// Sets or retrieves the color of the left border of the object.  
            /// </summary>
            [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
            [EditorAttribute(
                 typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
                 typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Color), "Empty")]
            public Color borderLeftColor
            {
                get
                {
                    if (this.htmlStyle.GetBorderLeftColor() != null)
                        return RgbColorConverter(this.htmlStyle.GetBorderLeftColor().ToString());
                    else
                        return Color.Empty;
                }
                set
                {
                    this.htmlStyle.SetBorderLeftColor(ColorTranslator.ToHtml(value));
                }
            }

            /// <summary>
            /// Sets or retrieves the style of the left border of the object.  
            /// </summary>        
            [DefaultValue(typeof(BorderStyle), "NotSet")]
            public System.Web.UI.WebControls.BorderStyle borderLeftStyle
            {
                get
                {
                    if (this.htmlStyle.GetBorderLeftStyle() != null)
                        return (System.Web.UI.WebControls.BorderStyle)ElementStyle.ConvertEnumeration(typeof(BorderStyle), this.htmlStyle.GetBorderLeftStyle());
                    else
                        return System.Web.UI.WebControls.BorderStyle.NotSet;
                }
                set
                {
                    this.htmlStyle.SetBorderLeftStyle(value.ToString());
                }

            }

            /// <summary>
            /// Sets or retrieves the width of the left border of the object.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit borderLeftWidth
            {
                get
                {
                    object val = this.htmlStyle.GetBorderLeftWidth();
                    if (val == null || val.Equals("auto"))
                    {
                        return Unit.Empty;
                    }
                    else
                    {
                        return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                set
                {
                    this.htmlStyle.SetBorderLeftWidth(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the object.  
            /// </summary>
            [DefaultValue(true)]
            public bool borderLeftWidthAuto
            {
                get
                {
                    object val = this.htmlStyle.GetBorderLeftWidth();
                    if (val == null || val.ToString().Equals("auto"))
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
                    this.htmlStyle.SetBorderLeftWidth(value ? "auto" : "");
                }
            }

            /// <summary>
            /// Sets or retrieves the properties of the right border of the object.  
            /// </summary>
            [DefaultValue("")]
            public string borderRight
            {
                get
                {
                    return (this.htmlStyle.GetBorderRight() == null) ? "" : this.htmlStyle.GetBorderRight().ToString();
                }
                set
                {
                    this.htmlStyle.SetBorderRight(value);
                }
            }

            /// <summary>
            /// Sets or retrieves the color of the right border of the object.  
            /// </summary>
            [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
            [EditorAttribute(
                 typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
                 typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Color), "Empty")]
            public Color borderRightColor
            {
                get
                {
                    if (this.htmlStyle.GetBorderRightColor() != null)
                    {
                        return RgbColorConverter(this.htmlStyle.GetBorderRightColor().ToString());
                    }
                    else
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    this.htmlStyle.SetBorderRightColor(ColorTranslator.ToHtml(value));
                }
            }

            /// <summary>
            /// Sets or retrieves the style of the right border of the object.  
            /// </summary>
            [DefaultValue(typeof(BorderStyle), "NotSet")]
            public System.Web.UI.WebControls.BorderStyle borderRightStyle
            {
                get
                {
                    if (this.htmlStyle.GetBorderRightStyle() != null)
                        return (System.Web.UI.WebControls.BorderStyle)ElementStyle.ConvertEnumeration(typeof(BorderStyle), this.htmlStyle.GetBorderRightStyle());
                    else
                        return System.Web.UI.WebControls.BorderStyle.NotSet;
                }
                set
                {
                    this.htmlStyle.SetBorderRightStyle(value.ToString());
                }

            }

            /// <summary>
            /// Sets or retrieves the width of the right border of the object.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit borderRightWidth
            {
                get
                {
                    if (this.htmlStyle.GetBorderRightWidth() != null)
                        return Unit.Parse(this.htmlStyle.GetBorderRightWidth().ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    else
                        return Unit.Empty;
                }
                set
                {
                    this.htmlStyle.SetBorderRightWidth(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the right border of the object.  
            /// </summary>
            [DefaultValue("")]
            public string borderRightWidthValue
            {
                get
                {
                    if (this.htmlStyle.GetBorderRightWidth() == null) return "";
                    string s = this.htmlStyle.GetBorderRightWidth().ToString().ToLower();
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
                    this.htmlStyle.SetBorderRightWidth(value);
                }
            }

            /// <summary>
            /// Sets or retrieves the style of the left, right, top, and bottom borders of the object.  
            /// </summary>
            [DefaultValue(typeof(BorderStyle), "NotSet")]
            public System.Web.UI.WebControls.BorderStyle borderStyle
            {
                get
                {
                    if (this.htmlStyle.GetBorderStyle() != null)
                        return (System.Web.UI.WebControls.BorderStyle)ElementStyle.ConvertEnumeration(typeof(System.Web.UI.WebControls.BorderStyle), this.htmlStyle.GetBorderStyle());
                    else
                        return System.Web.UI.WebControls.BorderStyle.NotSet;
                }
                set
                {
                    this.htmlStyle.SetBorderStyle(value.ToString());
                }
            }

            /// <summary>
            /// Sets or retrieves the properties of the top border of the object.  
            /// </summary>
            [DefaultValue("")]
            public string borderTop
            {
                get
                {
                    return (this.htmlStyle.GetBorderTop() == null) ? "" : this.htmlStyle.GetBorderTop();
                }
                set
                {
                    this.htmlStyle.SetBorderTop(value);
                }
            }

            /// <summary>
            /// Sets or retrieves the color of the top border of the object.  
            /// </summary>
            [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
            [EditorAttribute(
                 typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
                 typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Color), "Empty")]
            public Color borderTopColor
            {
                get
                {
                    if (this.htmlStyle.GetBorderTopColor() != null)
                    {
                        return RgbColorConverter(this.htmlStyle.GetBorderTopColor().ToString());
                    }
                    else
                    {
                        return Color.Empty;
                    }
                }
                set
                {
                    this.htmlStyle.SetBorderTopColor(ColorTranslator.ToHtml(value));
                }
            }

            /// <summary>
            /// Sets or retrieves the style of the top border of the object.  
            /// </summary>
            [DefaultValue(typeof(BorderStyle), "NotSet")]
            public System.Web.UI.WebControls.BorderStyle borderTopStyle
            {
                get
                {
                    if (this.htmlStyle.GetBorderTopStyle() != null)
                        return (System.Web.UI.WebControls.BorderStyle)ElementStyle.ConvertEnumeration(typeof(BorderStyle), this.htmlStyle.GetBorderTopStyle());
                    else
                        return System.Web.UI.WebControls.BorderStyle.NotSet;
                }
                set
                {
                    this.htmlStyle.SetBorderTopStyle(value.ToString());
                }

            }

            /// <summary>
            /// Sets or retrieves the width of the top border of the object.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit borderTopWidth
            {
                get
                {
                    if (this.htmlStyle.GetBorderTopWidth() != null)
                        return Unit.Parse(this.htmlStyle.GetBorderTopWidth().ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    else
                        return Unit.Empty;
                }
                set
                {
                    this.htmlStyle.SetBorderTopWidth(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the right border of the object.  
            /// </summary>
            [DefaultValue(true)]
            public string borderTopWidthValue
            {
                get
                {
                    if (this.htmlStyle.GetBorderTopWidth() == null) return "";
                    string s = this.htmlStyle.GetBorderTopWidth().ToString().ToLower();
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
                    this.htmlStyle.SetBorderTopWidth(value);
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the left, right, top, and bottom borders of the object.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit borderWidth
            {
                get
                {
                    if (this.htmlStyle.GetBorderWidth() != null)
                        return Unit.Parse(this.htmlStyle.GetBorderWidth(), System.Globalization.CultureInfo.InvariantCulture);
                    else
                        return Unit.Empty;
                }
                set
                {
                    this.htmlStyle.SetBorderWidth(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }
        }

        /// <summary>
        /// Sets or retrieves whether the object allows floating objects on its left side, right side, or both, so that the next text displays past the floating objects. 
        /// </summary>
        [DefaultValue(typeof(ClearStyles), "None")]
        public ClearStyles clear
        {
            get
            {
                if (this.htmlStyle.GetClear() != null)
                {
                    return (ClearStyles)ConvertEnumeration(typeof(ClearStyles), this.htmlStyle.GetClear());
                }
                else
                {
                    return ClearStyles.None;
                }
            }
            set
            {
                this.htmlStyle.SetClear(ConvertEnumeration(value));
            }
        }


        /// <summary>
        /// Sets or retrieves which part of a positioned object is visible.  
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property defines the shape and size of the positioned object that is visible. The Horizontalposition must be set to absolute. Any part of the object that is outside the clipping region is transparent. Any coordinate can be replaced by the value auto, which exposes the respective side (that is, the side is not clipped).
        /// </para>
        /// <para>
        /// The order of the values rect(0 0 50 50) renders the object invisible because it sets the top and right positions of the clipping region to 0. To achieve a 50-by-50 view port, use rect(0 50 50 0).
        /// </para>
        /// </remarks>
        public IClipStyle clip
        {
            get
            {
                ClipStyle cs = new ClipStyle();
                if (this.htmlStyle.GetClip() != null)
                    cs.ClipString = this.htmlStyle.GetClip();
                return cs;
            }
            set
            {
                this.htmlStyle.SetClip(value.ClipString);
            }
        }


        /// <summary>
        /// Sets or retrieves the color of the text of the object.  
        /// </summary>
        /// <remarks>
        /// Some browsers do not recognize color names, but all browsers should recognize RGB color values and display them correctly.
        /// Therefore the property returns always RGB values, even if the value was previously set using names. This is a different behavior compared to the original MSHTML.
        /// </remarks>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(typeof(Color), "Empty")]
        public Color color
        {
            get
            {
                if (this.htmlStyle.GetColor() != null)
                    return RgbColorConverter(this.htmlStyle.GetColor().ToString());
                else
                    return Color.Empty;
            }
            set
            {
                this.htmlStyle.SetColor(ColorTranslator.ToHtml(value));
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
        [DefaultValue("")]
        public string cssText
        {
            get
            {
                return (this.htmlStyle.GetCssText() == null) ? "" : this.htmlStyle.GetCssText();
            }
            set
            {
                this.htmlStyle.SetCssText(value);
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
        [DefaultValue("")]
        public string cursor
        {
            get
            {
                return (this.htmlStyle.GetCursor() == null) ? "" : this.htmlStyle.GetCursor();
            }
            set
            {
                this.htmlStyle.SetCursor(value);
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
        [DefaultValue(typeof(DisplayStyles), "None")]
        public DisplayStyles display
        {
            get
            {
                if (this.htmlStyle.GetDisplay() != null)
                    return (DisplayStyles)ConvertEnumeration(typeof(DisplayStyles), this.htmlStyle.GetDisplay());
                else
                    return DisplayStyles.None;
            }
            set
            {
                this.htmlStyle.SetDisplay(ConvertEnumeration(value));
            }
        }

        /// <summary>
        /// Sets or retrieves the filter or collection of filters applied to the object.  
        /// </summary>
        /// <remarks>
        /// <para>An object must have layout for the filter to render. A simple way to accomplish this is to give the element a specified height and/or width. However, there are several other properties that can give an element layout. For more information on these other properties, see the hasLayout property.</para>
        /// <para>The shadow filter can be applied to the img object by setting the filter on the image's parent container.</para>
        /// <para>The filter mechanism is extensible and enables you to develop and add additional filters later. For more information about filters, see 
        /// <see href="http://msdn.microsoft.com/workshop/author/filter/filters.asp">Introduction to Filters and Transitions</see> on MSDN (online connection required).</para>
        /// </remarks>
        [DefaultValue("")]
        public string filter
        {
            get
            {
                return (this.htmlStyle.GetFilter() == null) ? "" : this.htmlStyle.GetFilter();
            }
            set
            {
                this.htmlStyle.SetFilter(value);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IFont FontStyle
        {
            get
            {
                return fontStyle;
            }
        }

        public class Font : GuruComponents.Netrix.WebEditing.Styles.IFont
        {

            private Interop.IHTMLStyle htmlStyle;

            internal Font(Interop.IHTMLStyle htmlStyle)
            {
                this.htmlStyle = htmlStyle;
            }

            public override string ToString()
            {
                return "";
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
            [DefaultValue("")]
            public string font
            {
                get
                {
                    return (this.htmlStyle.GetFont() == null) ? "" : this.htmlStyle.GetFont();
                }
                set
                {
                    this.htmlStyle.SetFont(value);
                }
            }

            /// <summary>
            /// Sets or retrieves the name of the font used for text in the object.  
            /// </summary>
            /// <remarks>
            /// <para>The value is a prioritized list of font family names and generic family names. List items are separated by commas to minimize confusion between multiple-word font family names. If the font family name contains white space, it should appear in single or double quotation marks; generic font family names are values and cannot appear in quotation marks. </para>
            /// <para>Because you do not know which fonts users have installed, you should provide a list of alternatives with a generic font family at the end of the list. This list can include embedded fonts. For more information about embedding fonts, see the @font-face rule.</para>
            /// </remarks>
            [DefaultValue("")]
            public string fontFamily
            {
                get
                {
                    return (this.htmlStyle.GetFontFamily() == null) ? "" : this.htmlStyle.GetFontFamily();
                }
                set
                {
                    this.htmlStyle.SetFontFamily(value);
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
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit fontSize
            {
                get
                {
                    if (this.htmlStyle.GetFontSize() != null)
                    {
                        switch (this.htmlStyle.GetFontSize().ToString())
                        {
                            case "xxsmall":
                            case "xx-small":
                                return Unit.Point(6);
                            case "xsmall":
                            case "x-small":
                                return Unit.Point(8);
                            case "xlarge":
                            case "x-large":
                                return Unit.Point(12);
                            case "xxlarge":
                            case "xx-large":
                                return Unit.Point(14);
                            case "normal":
                                return Unit.Point(10);
                            default:
                                return Unit.Parse(this.htmlStyle.GetFontSize().ToString(), System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    else
                        return Unit.Empty;
                }
                set
                {
                    this.htmlStyle.SetFontSize(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the font style of the object as italic, normal, or oblique.  
            /// </summary>
            public string fontStyle
            {
                get
                {
                    return (this.htmlStyle.GetFontStyle() == null) ? "" : this.htmlStyle.GetFontStyle();
                }
                set
                {
                    this.htmlStyle.SetFontStyle(value);
                }
            }

            /// <summary>
            /// Sets or retrieves whether the text of the object is in small capital letters. 
            /// </summary>
            public string fontVariant
            {
                get
                {
                    return (this.htmlStyle.GetFontStyle() == null) ? "" : this.htmlStyle.GetFontStyle();
                }
                set
                {
                    this.htmlStyle.SetFontStyle(value);
                }
            }

            /// <summary>
            /// Sets or retrieves the weight of the font of the object.  
            /// </summary>
            public string fontWeight
            {
                get
                {
                    return (this.htmlStyle.GetFontWeight() == null) ? "" : this.htmlStyle.GetFontWeight();
                }
                set
                {
                    this.htmlStyle.SetFontWeight(value);
                }
            }

        }

        /// <summary>
        /// Sets or retrieves the height of the object.  
        /// </summary>
        [EditorAttribute(
     typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
     typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit height
        {
            get
            {
                object val = this.htmlStyle.GetHeight();
                if (val == null || val.ToString().Equals("auto"))
                {
                    return Unit.Empty;
                }
                else
                {
                    return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.SetHeight(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool heightAuto
        {
            get
            {
                object val = this.htmlStyle.GetHeight();
                if (val == null || val.ToString().Equals("auto"))
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
                this.htmlStyle.SetHeight(value ? "auto" : "");
            }
        }

        /// <summary>
        /// Sets or retrieves the position of the object relative to the left edge of the next positioned object in the document hierarchy.  
        /// </summary>
        [EditorAttribute(
     typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
     typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit left
        {
            get
            {
                object val = this.htmlStyle.GetLeft();
                if (val == null || val.ToString().Equals("auto"))
                {
                    return Unit.Empty;
                }
                else
                {
                    return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.SetLeft(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        [DefaultValue(true)]
        public bool leftAuto
        {
            get
            {
                object val = this.htmlStyle.GetLeft();
                if (val == null || val.ToString().Equals("auto"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Sets or retrieves the amount of additional space between letters in the object.  
        /// </summary>
        [EditorAttribute(
     typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
     typeof(System.Drawing.Design.UITypeEditor))]
        public System.Web.UI.WebControls.Unit letterSpacing
        {
            get
            {
                object val = this.htmlStyle.GetLetterSpacing();
                if (val == null || val.ToString().Equals("auto"))
                {
                    return Unit.Empty;
                }
                else
                {
                    return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.SetLetterSpacing(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        public bool letterSpacingNormal
        {
            get
            {
                object val = this.htmlStyle.GetLetterSpacing();
                if (val == null || val.ToString().Equals("normal"))
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
                this.htmlStyle.SetLetterSpacing(value ? "normal" : "");
            }
        }

        /// <summary>
        /// Sets or retrieves the distance between lines in the object.  
        /// </summary>
        [EditorAttribute(
     typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
     typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit lineHeight
        {
            get
            {
                object val = this.htmlStyle.GetLineHeight();
                if (val == null || val.ToString().Equals("auto"))
                {
                    return Unit.Empty;
                }
                else
                {
                    return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.SetLineHeight(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Sets or retrieves the distance between lines in the object if set to "normal".
        /// </summary>
        [DefaultValue(true)]
        public bool lineHeightNormal
        {
            get
            {
                object val = this.htmlStyle.GetLineHeight();
                if (val == null || val.ToString().Equals("normal"))
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
                this.htmlStyle.SetLineHeight(value ? "normal" : "");
            }
        }


        /// <summary>
        /// Sets or retrieves up to three separate HorizontallistStyle properties of the object. 
        /// </summary>
        public string listStyle
        {
            get
            {
                return (this.htmlStyle.GetListStyle() == null) ? "" : this.htmlStyle.GetListStyle();
            }
            set
            {
                this.htmlStyle.SetListStyle(value);
            }
        }

        /// <summary>
        /// Sets or retrieves a value that indicates which image to use as a list-item marker for the object.  
        /// </summary>
        public string listStyleImage
        {
            get
            {
                return (this.htmlStyle.GetListStyleImage() == null) ? "" : this.htmlStyle.GetListStyleImage();
            }
            set
            {
                this.htmlStyle.SetListStyleImage(value);
            }
        }

        /// <summary>
        /// Sets or retrieves a variable that indicates how the list-item marker is drawn relative to the content of the object.  
        /// </summary>
        [DefaultValue(typeof(ListStylePosition), "NotSet")]
        public ListStylePosition listStylePosition
        {
            get
            {
                if (this.htmlStyle.GetListStylePosition() != null)
                    return (ListStylePosition)ConvertEnumeration(typeof(ListStylePosition), this.htmlStyle.GetListStylePosition());
                else
                    return ListStylePosition.NotSet;
            }
            set
            {
                this.htmlStyle.SetListStylePosition(ConvertEnumeration(value));
            }
        }

        /// <summary>
        /// Sets or retrieves the predefined type of the line-item marker for the object.  
        /// </summary>
        [DefaultValue(typeof(ListStyleType), "None")]
        public ListStyleType listStyleType
        {
            get
            {
                if (this.htmlStyle.GetListStyleType() != null)
                    return (ListStyleType)ConvertEnumeration(typeof(ListStyleType), this.htmlStyle.GetListStyleType());
                else
                    return ListStyleType.None;
            }
            set
            {
                this.htmlStyle.SetListStyleType(ConvertEnumeration(value));
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IMargin MarginStyle
        {
            get { return marginStyle; }
        }

        public class Margin : GuruComponents.Netrix.WebEditing.Styles.IMargin
        {
            private Interop.IHTMLStyle htmlStyle;

            internal Margin(Interop.IHTMLStyle htmlStyle)
            {
                this.htmlStyle = htmlStyle;
            }

            public override string ToString()
            {
                return "";
            }

            /// <summary>
            /// Sets or retrieves the width of the top, right, bottom, and left margins of the object.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit margin
            {
                get
                {
                    if (this.htmlStyle.GetMargin() != null)
                        return Unit.Parse(this.htmlStyle.GetMargin(), System.Globalization.CultureInfo.InvariantCulture);
                    else
                        return Unit.Empty;
                }
                set
                {
                    this.htmlStyle.SetMargin(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the height of the bottom margin of the object.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit marginBottom
            {
                get
                {
                    object val = this.htmlStyle.GetMarginBottom();
                    if (val == null || val.Equals("auto"))
                    {
                        return Unit.Empty;
                    }
                    else
                    {
                        return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                set
                {
                    this.htmlStyle.SetMarginBottom(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the object.  
            /// </summary>
            [RefreshProperties(RefreshProperties.All)]
            public bool marginBottomAuto
            {
                get
                {
                    object val = this.htmlStyle.GetMarginBottom();
                    if (val == null || val.ToString().Equals("auto"))
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
                    this.htmlStyle.SetMarginBottom(value ? "auto" : "");
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the left margin of the object.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit marginLeft
            {
                get
                {
                    object val = this.htmlStyle.GetMarginLeft();
                    if (val == null || val.ToString().Equals("auto"))
                    {
                        return Unit.Empty;
                    }
                    else
                    {
                        return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                set
                {
                    this.htmlStyle.SetMarginLeft(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the object.  
            /// </summary>
            [RefreshProperties(RefreshProperties.All)]
            public bool marginLeftAuto
            {
                get
                {
                    object val = this.htmlStyle.GetMarginLeft();
                    if (val == null || val.ToString().Equals("auto"))
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
                    this.htmlStyle.SetMarginLeft(value ? "auto" : "");
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the right margin of the object.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit marginRight
            {
                get
                {
                    object val = this.htmlStyle.GetMarginRight();
                    if (val == null || val.ToString().Equals("auto"))
                    {
                        return Unit.Empty;
                    }
                    else
                    {
                        return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                set
                {
                    this.htmlStyle.SetMarginRight(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the object.  
            /// </summary>
            [RefreshProperties(RefreshProperties.All)]
            public bool marginRightAuto
            {
                get
                {
                    object val = this.htmlStyle.GetMarginRight();
                    if (val == null || val.ToString().Equals("auto"))
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
                    this.htmlStyle.SetMarginRight(value ? "auto" : "");
                }
            }

            /// <summary>
            /// Sets or retrieves the height of the top margin of the object.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit marginTop
            {
                get
                {
                    object val = this.htmlStyle.GetMarginTop();
                    if (val == null || val.Equals("auto"))
                    {
                        return Unit.Empty;
                    }
                    else
                    {
                        return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                set
                {
                    this.htmlStyle.SetMarginTop(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the object.  
            /// </summary>
            [RefreshProperties(RefreshProperties.All)]
            public bool marginTopAuto
            {
                get
                {
                    object val = this.htmlStyle.GetMarginTop();
                    if (val == null || val.ToString().Equals("auto"))
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
                    this.htmlStyle.SetMarginTop(value ? "auto" : "");
                }
            }
        }

        /// <summary>
        /// Sets or retrieves a value indicating how to manage the content of the object when the content exceeds the height or width of the object. 
        /// </summary>
        public string overflow
        {
            get
            {
                return (this.htmlStyle.GetOverflow() == null) ? "" : this.htmlStyle.GetOverflow();
            }
            set
            {
                this.htmlStyle.SetOverflow(value);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IPadding PaddingStyle
        {
            get { return paddingStyle; }
        }

        public class Padding : GuruComponents.Netrix.WebEditing.Styles.IPadding
        {

            private Interop.IHTMLStyle htmlStyle;

            internal Padding(Interop.IHTMLStyle htmlStyle)
            {
                this.htmlStyle = htmlStyle;
            }

            public override string ToString()
            {
                return "";
            }

            /// <summary>
            /// Sets or retrieves the amount of space to insert between the object and its margin or, if there is a border, between the object and its border.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit padding
            {
                get
                {
                    if (this.htmlStyle.GetPadding() != null)
                    {
                        return Unit.Parse(this.htmlStyle.GetPadding(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        return Unit.Empty;
                    }
                }
                set
                {
                    this.htmlStyle.SetPadding(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the amount of space to insert between the bottom border of the object and the content.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            public System.Web.UI.WebControls.Unit paddingBottom
            {
                get
                {
                    object val = this.htmlStyle.GetPaddingBottom();
                    if (val == null || val.ToString().Equals("auto"))
                    {
                        return Unit.Empty;
                    }
                    else
                    {
                        return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                set
                {
                    this.htmlStyle.SetPaddingBottom(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the object.  
            /// </summary>
            [RefreshProperties(RefreshProperties.All)]
            public bool paddingBottomAuto
            {
                get
                {
                    object val = this.htmlStyle.GetPaddingBottom();
                    if (val == null || val.Equals("auto"))
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
                    this.htmlStyle.SetPaddingBottom(value ? "auto" : "");
                }
            }

            /// <summary>
            /// Sets or retrieves the amount of space to insert between the left border of the object and the content.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit paddingLeft
            {
                get
                {
                    object val = this.htmlStyle.GetPaddingLeft();
                    if (val == null || val.ToString().Equals("auto"))
                    {
                        return Unit.Empty;
                    }
                    else
                    {
                        return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                set
                {
                    this.htmlStyle.SetPaddingBottom(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the object.  
            /// </summary>
            [RefreshProperties(RefreshProperties.All)]
            public bool paddingLeftAuto
            {
                get
                {
                    object val = this.htmlStyle.GetPaddingLeft();
                    if (val == null || val.ToString().Equals("auto"))
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
                    this.htmlStyle.SetPaddingLeft(value ? "auto" : "");
                }
            }

            /// <summary>
            /// Sets or retrieves the amount of space to insert between the right border of the object and the content.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit paddingRight
            {
                get
                {
                    object val = this.htmlStyle.GetPaddingRight();
                    if (val == null || val.ToString().Equals("auto"))
                    {
                        return Unit.Empty;
                    }
                    else
                    {
                        return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                set
                {
                    this.htmlStyle.SetPaddingRight(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the object.  
            /// </summary>
            [RefreshProperties(RefreshProperties.All)]
            public bool paddingRightAuto
            {
                get
                {
                    object val = this.htmlStyle.GetPaddingRight();
                    if (val == null || val.ToString().Equals("auto"))
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
                    this.htmlStyle.SetPaddingRight(value ? "auto" : "");
                }
            }

            /// <summary>
            /// Sets or retrieves the amount of space to insert between the top border of the object and the content.  
            /// </summary>
            [EditorAttribute(
         typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
         typeof(System.Drawing.Design.UITypeEditor))]
            [DefaultValue(typeof(Unit), "Empty")]
            public System.Web.UI.WebControls.Unit paddingTop
            {
                get
                {
                    object val = this.htmlStyle.GetPaddingTop();
                    if (val == null || val.ToString().Equals("auto"))
                    {
                        return Unit.Empty;
                    }
                    else
                    {
                        return Unit.Parse(val.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                set
                {
                    this.htmlStyle.SetPaddingTop(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }

            /// <summary>
            /// Sets or retrieves the width of the object.  
            /// </summary>
            public bool paddingTopAuto
            {
                get
                {
                    object val = this.htmlStyle.GetPaddingTop();
                    if (val == null || val.ToString().Equals("auto"))
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
                    this.htmlStyle.SetPaddingTop(value ? "auto" : "");
                }
            }
        }

        /// <summary>
        /// Sets or retrieves a value indicating whether a page break occurs after the object.  
        /// </summary>
        [DefaultValue(typeof(PageBreakStyles), "Empty")]
        public PageBreakStyles pageBreakAfter
        {
            get
            {
                if (this.htmlStyle.GetPageBreakAfter() != null)
                {
                    return (PageBreakStyles)ConvertEnumeration(typeof(PageBreakStyles), this.htmlStyle.GetPageBreakAfter());
                }
                else
                {
                    return PageBreakStyles.Empty;
                }
            }
            set
            {
                this.htmlStyle.SetPageBreakAfter(ConvertEnumeration(value));
            }
        }

        /// <summary>
        /// Sets or retrieves a string indicating whether a page break occurs before the object. 
        /// </summary>
        [DefaultValue(typeof(PageBreakStyles), "Empty")]
        public PageBreakStyles pageBreakBefore
        {
            get
            {
                if (this.htmlStyle.GetPageBreakBefore() != null)
                {
                    return (PageBreakStyles)ConvertEnumeration(typeof(PageBreakStyles), this.htmlStyle.GetPageBreakBefore());
                }
                else
                {
                    return PageBreakStyles.Empty;
                }
            }
            set
            {
                this.htmlStyle.SetPageBreakBefore(ConvertEnumeration(value));
            }
        }

        /// <summary>
        /// Sets or retrieves the height of the object.  
        /// </summary>
        [DefaultValue(0)]
        public int pixelHeight
        {
            get
            {
                return this.htmlStyle.GetPixelHeight();
            }
            set
            {
                this.htmlStyle.SetPixelHeight(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the left position of the object.  
        /// </summary>
        [DefaultValue(0)]
        public int pixelLeft
        {
            get
            {
                return this.htmlStyle.GetPixelLeft();
            }
            set
            {
                this.htmlStyle.SetPixelLeft(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the top position of the object.  
        /// </summary>
        [DefaultValue(0)]
        public int pixelTop
        {
            get
            {
                return this.htmlStyle.GetPixelTop();
            }
            set
            {
                this.htmlStyle.SetPixelTop(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        [DefaultValue(0)]
        public int pixelWidth
        {
            get
            {
                return this.htmlStyle.GetPixelWidth();
            }
            set
            {
                this.htmlStyle.SetPixelWidth(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the height of the object in the units specified by the height attribute.  
        /// </summary>
        [EditorAttribute(
     typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
     typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit posHeight
        {
            get
            {
                return Unit.Pixel((int)this.htmlStyle.GetPosHeight());
            }
            set
            {
                this.htmlStyle.SetPosHeight((float)value.Value);
            }
        }

        /// <summary>
        /// Retrieves the type of positioning used for the object.  
        /// </summary>
        public string position
        {
            get
            {
                return (this.htmlStyle.GetPosition() == null) ? "" : this.htmlStyle.GetPosition();
            }
            set
            {
                SetAttribute("position", value);
            }
        }

        /// <summary>
        /// Sets or retrieves the left position of the object in the units specified by the left attribute.  
        /// </summary>
        [EditorAttribute(
     typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
     typeof(System.Drawing.Design.UITypeEditor))]
        public System.Web.UI.WebControls.Unit posLeft
        {
            get
            {
                return Unit.Pixel((int)this.htmlStyle.GetPosLeft());
            }
            set
            {
                this.htmlStyle.SetPosLeft((float)value.Value);
            }
        }

        /// <summary>
        /// Sets or retrieves the top position of the object in the units specified by the top attribute.  
        /// </summary>
        [EditorAttribute(
     typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
     typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit posTop
        {
            get
            {
                return Unit.Pixel((int)this.htmlStyle.GetPosTop());
            }
            set
            {
                this.htmlStyle.SetPosTop((float)value.Value);
            }

        }

        /// <summary>
        /// Sets or retrieves the width of the object in the units specified by the width attribute.  
        /// </summary>
        [EditorAttribute(
     typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
     typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit posWidth
        {
            get
            {
                if (this.htmlStyle.GetPosWidth() != 0F)
                {
                    return Unit.Pixel((int)this.htmlStyle.GetPosWidth());
                }
                else
                {
                    return Unit.Empty;
                }
            }
            set
            {
                this.htmlStyle.SetPosWidth((float)value.Value);
            }

        }

        /// <summary>
        /// Get the given attribute from the object. 
        /// </summary>
        /// <remarks>
        /// The caller is supposed to convert to appropriate type.
        /// </remarks>
        /// <param name="attribute">Specifies the attribute name.</param>
        public object GetAttribute(string attribute)
        {
            return this.htmlStyle.GetAttribute(attribute, 0);
        }

        /// <summary>
        /// Removes the given attribute from the object. 
        /// </summary>
        /// <param name="attribute"></param>
        public void RemoveAttribute(string attribute)
        {
            this.htmlStyle.RemoveAttribute(attribute, 0);
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
            this.htmlStyle.SetAttribute(attribute, @value, 0);
        }

        /// <summary>
        /// Sets or retrieves on which side of the object the text will flow. 
        /// </summary>
        public string styleFloat
        {
            get
            {
                return (this.htmlStyle.GetStyleFloat() == null) ? "" : this.htmlStyle.GetStyleFloat();
            }
            set
            {
                this.htmlStyle.SetStyleFloat(value);
            }
        }

        /// <summary>
        /// Sets or retrieves whether the text in the object is left-aligned, 
        /// right-aligned, centered, or justified.  
        /// </summary>
        [DefaultValue(typeof(HorizontalAlign), "NotSet")]
        public System.Web.UI.WebControls.HorizontalAlign textAlign
        {
            get
            {
                return (HorizontalAlign)ConvertEnumeration(typeof(HorizontalAlign), this.htmlStyle.GetTextAlign());
            }
            set
            {
                this.htmlStyle.SetTextAlign(ConvertEnumeration(value));
            }
        }

        /// <summary>
        /// Summarizes a container for text decoration styles.
        /// </summary>
        /// <seealso cref="TextDecoration"/>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ITextDecoration TextDecorationStyle
        {
            get { return textDecorationStyle; }
        }

    /// <summary>
    /// Implements a container for text decoration styles.
    /// </summary>
    /// <remarks>
    /// Based on <see cref="GuruComponents.Netrix.ComInterop.Interop.IHTMLStyle"/> interface.
    /// </remarks>
    public class TextDecoration : GuruComponents.Netrix.WebEditing.Styles.ITextDecoration
        {

            Interop.IHTMLStyle htmlStyle;

            internal TextDecoration(Interop.IHTMLStyle htmlStyle)
            {
                this.htmlStyle = htmlStyle;
            }


            /// <summary>
            /// Sets or retrieves a value that indicates whether the text in the object has blink, line-through, overline, or underline decorations.
            /// </summary>
            public string textDecoration
            {
                get
                {
                    return (this.htmlStyle.GetTextDecoration() == null) ? "" : this.htmlStyle.GetTextDecoration();
                }
                set
                {
                    this.htmlStyle.SetTextDecoration(value);
                }
            }

            /// <summary>
            /// Sets or retrieves a Boolean value that indicates whether the object's textDecoration property has a value of "blink." 
            /// </summary>
            [DefaultValue(false)]
            public bool textDecorationBlink
            {

                get
                {
                    return this.htmlStyle.GetTextDecorationBlink();
                }
                set
                {
                    this.htmlStyle.SetTextDecorationBlink(value);
                }
            }

            /// <summary>
            /// Sets or retrieves a Boolean value indicating whether the text in the object has a line drawn through it.  
            /// </summary>
            [DefaultValue(false)]
            public bool textDecorationLineThrough
            {
                get
                {
                    return this.htmlStyle.GetTextDecorationLineThrough();
                }
                set
                {
                    this.htmlStyle.SetTextDecorationLineThrough(value);
                }
            }

            /// <summary>
            /// Sets or retrieves the Boolean value indicating whether the HorizontaltextDecoration property for the object has been set to none.  
            /// </summary>
            [DefaultValue(false)]
            public bool textDecorationNone
            {

                get
                {
                    return this.htmlStyle.GetTextDecorationNone();
                }
                set
                {
                    this.htmlStyle.SetTextDecorationNone(value);
                }
            }

            /// <summary>
            /// Sets or retrieves a Boolean value indicating whether the text in the object has a line drawn over it.  
            /// </summary>
            [DefaultValue(false)]
            public bool textDecorationOverline
            {
                get
                {
                    return this.htmlStyle.GetTextDecorationOverline();
                }
                set
                {
                    this.htmlStyle.SetTextDecorationOverline(value);
                }
            }

            /// <summary>
            /// Sets or retrieves whether the text in the object is underlined.  
            /// </summary>
            [DefaultValue(false)]
            public bool textDecorationUnderline
            {
                get
                {
                    return this.htmlStyle.GetTextDecorationUnderline();
                }
                set
                {
                    this.htmlStyle.SetTextDecorationUnderline(value);
                }
            }

            public override string ToString()
            {
                return "Text Decoration Styles";
            }

        }

        /// <summary>
        /// Sets or retrieves the indentation of the first line of text in the object.  
        /// </summary>
        [EditorAttribute(
     typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
     typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit textIndent
        {
            get
            {
                if (this.htmlStyle.GetTextIndent() != null)
                {
                    return Unit.Parse(this.htmlStyle.GetTextIndent().ToString(), System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    return Unit.Empty;
                }
            }
            set
            {
                this.htmlStyle.SetTextIndent(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Sets or retrieves the rendering of the text in the object.  
        /// </summary>
        [DefaultValue("")]
        public string textTransform
        {
            get
            {
                return (this.htmlStyle.GetTextTransform() == null) ? "" : this.htmlStyle.GetTextTransform();
            }
            set
            {
                this.htmlStyle.SetTextTransform(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the position of the object relative to the top of the next positioned object in the document hierarchy.  
        /// </summary>
        [EditorAttribute(
            typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
            typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit top
        {
            get
            {
                if (this.htmlStyle.GetTop() == null) return Unit.Empty;
                string val = this.htmlStyle.GetTop().ToString();
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
                this.htmlStyle.SetTop(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Sets or retrieves the top of the object.  
        /// </summary>
        [DefaultValue(true)]
        public bool topAuto
        {
            get
            {
                if (this.htmlStyle.GetTop() == null) return false;
                string val = this.htmlStyle.GetTop().ToString();
                if (val.Equals("auto"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Sets or retrieves the vertical alignment of the object.  
        /// </summary>
        [DefaultValue(typeof(VerticalAlign), "NotSet")]
        public System.Web.UI.WebControls.VerticalAlign verticalAlign
        {
            get
            {
                if (this.htmlStyle.GetVerticalAlign() != null)
                {
                    return (VerticalAlign)ConvertEnumeration(typeof(VerticalAlign), this.htmlStyle.GetVerticalAlign().ToString());
                }
                else
                {
                    return VerticalAlign.NotSet;
                }
            }
            set
            {
                this.htmlStyle.SetVerticalAlign(ConvertEnumeration(value));
            }
        }

        /// <summary>
        /// Sets or retrieves whether the content of the object is displayed.  
        /// </summary>
        [DefaultValue("")]
        public string visibility
        {
            get
            {
                if (this.htmlStyle.GetVisibility() == null) return "";
                return this.htmlStyle.GetVisibility();
            }
            set
            {
                this.htmlStyle.SetVisibility(value);
            }
        }

        /// <summary>
        /// Sets or retrieves a value that indicates whether lines are automatically broken inside the object. 
        /// </summary>
        [DefaultValue("")]
        public string whiteSpace
        {
            get
            {
                if (this.htmlStyle.GetWhiteSpace() == null) return "";
                return this.htmlStyle.GetWhiteSpace();
            }
            set
            {
                this.htmlStyle.SetWhiteSpace(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        [EditorAttribute(
     typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
     typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit width
        {
            get
            {
                if (this.htmlStyle.GetWidth() == null) return Unit.Empty;
                string val = this.htmlStyle.GetWidth().ToString();
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
                this.htmlStyle.SetWidth(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Sets or retrieves the width of the object.  
        /// </summary>
        [DefaultValue(true)]
        public bool widthAuto
        {
            get
            {
                object val = this.htmlStyle.GetWidth();
                if (val == null || val.ToString().Equals("auto"))
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
                this.htmlStyle.SetWidth(value ? "auto" : "");
            }
        }

        /// <summary>
        /// Sets or retrieves the amount of additional space between words in the object.  
        /// </summary>
        [EditorAttribute(
     typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit),
     typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit wordSpacing
        {
            get
            {
                if (this.htmlStyle.GetWordSpacing() == null || this.htmlStyle.GetWordSpacing().ToString().ToLower().Equals("normal"))
                {
                    return Unit.Empty;
                }
                else
                {
                    return Unit.Parse(this.htmlStyle.GetWordSpacing().ToString(), System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            set
            {
                this.htmlStyle.SetWordSpacing(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Sets or retrieves the amount of additional space between words in the object to the default value.
        /// </summary>
        [DefaultValue(true)]
        public bool wordSpacingNormal
        {
            get
            {
                if (this.htmlStyle.GetWordSpacing() == null || this.htmlStyle.GetWordSpacing().ToString().ToLower().Equals("normal"))
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
                this.htmlStyle.SetWordSpacing(value ? "normal" : "");
            }
        }

        /// <summary>
        /// Sets or retrieves the stacking order of positioned objects.  
        /// </summary>
        [DefaultValue(0)]
        public int zIndex
        {
            get
            {
                if (this.htmlStyle.GetZIndex() == null || this.htmlStyle.GetZIndex().ToString().ToLower().Equals("auto"))
                {
                    return 0;
                }
                else
                {
                    return Int32.Parse(this.htmlStyle.GetZIndex().ToString());
                }
            }
            set
            {
                this.htmlStyle.SetZIndex(value);
            }
        }

        /// <summary>
        /// Sets or retrieves the stacking order of positioned objects.  
        /// </summary>
        [DefaultValue(false)]
        public bool zIndexAuto
        {
            get
            {
                if (this.htmlStyle.GetZIndex() == null)
                    return true;
                else
                    return this.htmlStyle.GetZIndex().ToString().ToLower().Equals("auto");
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
        public static System.Enum ConvertEnumeration(Type enumType, string enumString)
        {
            if (String.IsNullOrEmpty(enumString))
            {
                if (Enum.IsDefined(enumType, "NotSet"))
                {
                    enumString = "NotSet";
                }
                if (Enum.IsDefined(enumType, "None"))
                {
                    enumString = "None";
                }
                if (Enum.IsDefined(enumType, "Empty"))
                {
                    enumString = "Empty";
                }
            }
            else
            {

            }
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
            return (Enum)Enum.Parse(enumType, s, true);
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


        public override string ToString()
        {
            return "Click + to change styles";
        }

        #region IStyle Members

        /// <summary>
        /// Sets or retrieves the bottom position of the object.  
        /// </summary>
        [ReadOnly(true)]
        public int pixelBottom
        {
            get
            {
                return this.htmlStyle.GetPixelTop() + htmlStyle.GetPixelHeight();
            }
            set
            {
                throw new NotSupportedException("pixelBottom is readonly");
            }
        }

        /// <summary>
        /// Sets or retrieves the right position of the object.  
        /// </summary>
        [ReadOnly(true)]
        public int pixelRight
        {
            get
            {
                return this.htmlStyle.GetPixelLeft() + htmlStyle.GetPixelWidth();
            }
            set
            {
                throw new NotSupportedException("pixelRight is readonly");
            }
        }

        /// <summary>
        /// Sets or retrieves the bottom position of the object in the units specified by the IHTMLStyle::left attribute.  
        /// </summary>
        [ReadOnly(true)]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit posBottom
        {
            get
            {
                return Unit.Pixel(Convert.ToInt32(this.htmlStyle.GetPosTop() + htmlStyle.GetPosHeight()));
            }
            set
            {
                throw new NotSupportedException("posBottom is readonly");
            }
        }

        /// <summary>
        /// Sets or retrieves the right position of the object in the units specified by the IHTMLStyle::top attribute.  
        /// </summary>
        [ReadOnly(true)]
        [DefaultValue(typeof(Unit), "Empty")]
        public System.Web.UI.WebControls.Unit posRight
        {
            get
            {
                return new Unit(this.htmlStyle.GetPosLeft() + htmlStyle.GetPosWidth(), this.top.Type);
            }
            set
            {
                throw new NotSupportedException("posRight is readonly");
            }

        }


        #endregion

        /// <summary>
        /// Sets or retrieves the documents zoom level.  
        /// </summary>
        [DefaultValue("")]
        public string Zoom
        {
            get
            {
                object zoom = ((Interop.IHTMLStyle3)this.htmlStyle).zoom();
                if (zoom != null)
                {
                    return zoom.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                ((Interop.IHTMLStyle3)this.htmlStyle).zoom(value);
            }
        }

    }
}