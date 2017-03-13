using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The MARQUEE element creates a scrolling display. Not recommended.
    /// </summary>
    /// <remarks>
    /// &lt;MARQUEE ...> creates a scrolling display. &lt;MARQUEE ...> is an MSIE extension, but is now supported by NS 7. 
    /// &lt;MARQUEE ...> is often regarded as one of the "evil" tags, and that perception alone might be enough reason 
    /// to not use it. However, used lightly and with taste (and understanding that it will never render everywhere), 
    /// &lt;MARQUEE ...> isn't such a bad tag. It can work well for announcements. 
    /// </remarks>
    public sealed class MarqueeElement : StyledElement
    {

        /// <summary>
        /// The WIDTH attribute specifies the width of the line as a percentage or a number of pixels.
        /// </summary>
        /// <remarks>
        /// These attribute is deprecated in favor of style sheets. If a width is specified, percentages are 
        /// generally preferred since they adjust to varying window sizes. The width property of Cascading 
        /// Style Sheets provides greater flexibility in suggesting the width of horizontal rules.
        /// </remarks>

        [Description("l")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorUnit),
             typeof(UITypeEditor))]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DisplayNameAttribute()]
        public new Unit Width
        {
            set
            {
                this.SetUnitAttribute ("width", value);
                this.RemoveStyleAttribute("width");
                return;
            } 
            get
            {
                return this.GetUnitAttribute ("width");
            }   
        }

        /// <summary>
        /// The HEIGHT of the element.
        /// </summary>
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorUnit),
             typeof(UITypeEditor))]
        [DisplayName()]
        public new Unit Height
        {
            set
            { 
                this.SetUnitAttribute ("height", value);
            } 
            get
            {
                return this.GetUnitAttribute ("height");
            } 
      
        }

        /// <summary>
        /// DIRECTION indicates which direction the marquee scrolls.
        /// </summary>
        /// <remarks>
        /// DIRECTION=LEFT, which is the default, indicates that the marquee starts at the right and moves leftwards across the page. DIRECTION=RIGHT indicates that the marquee starts at the left and moves rightwards across the page.
        /// </remarks>
        /// <exception cref="System.ArgumentException">
        /// The exception will thrown if any other value than 'left' or 'right' is set.
        /// </exception>

        [CategoryAttribute("Element Behavior")]
        [DefaultValue("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [Description("")]
        [DisplayName()]

        public string Direction
        {
            set
            {
                if (value.ToLower().Equals("left") || value.ToLower().Equals("right"))
                {
                    this.SetStringAttribute ("direction", value);
                } 
                else 
                {
                    throw new ArgumentException("This property does allow the values 'left' and 'right' only.");
                }
            } 
      
            get
            {
                return this.GetStringAttribute("direction");
            }       
        }

        /// <summary>
        /// BEHAVIOR indicates how the contents scroll.
        /// </summary>
        /// <remarks>
        /// BEHAVIOR=SCROLL, which is the default, indicates that the content should scroll off the edge of the marquee area, then reappear on the other side.
        /// BEHAVIOR=SLIDE is almost the same, except that it indicates that when the leading part content reaches the edge it should start over without scrolling off. Notice in this example that the contents start scrolling again as soon as the "H" reaches the left side.
        /// BEHAVIOR=ALTERNATE makes the content bounce back and forth, all of it remaining visible all the time (assuming of course that it all fits). 
        /// </remarks>
        /// <exception cref="System.ArgumentException">
        /// The exception will thrown if any other value than 'scroll', 'slide', or 'alternate' is set.
        /// </exception>

        [CategoryAttribute("Element Behavior")]
        [DefaultValue("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [Description("")]
        [DisplayName()]

        public string Behavior
        {
            set
            {
                if (value.ToLower().Equals("scroll") || value.ToLower().Equals("slide") || value.ToLower().Equals("alternate"))
                {
                    this.SetStringAttribute ("behavior", value);
                } 
                else 
                {
                    throw new ArgumentException("This property does allow the values 'scroll', 'slide', or 'alternate' only.");
                }
            } 
      
            get
            {
                return this.GetStringAttribute("behavior");
            }       
        }

        /// <summary>
        /// The background color of the scroll area.
        /// </summary>

        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Layout")]
        [TypeConverterAttribute(typeof(UITypeConverterColor))]
        [EditorAttribute(
             typeof(UITypeEditorColor),
             typeof(UITypeEditor))]
        [DisplayName()]

        public Color BgColor
        {
            set
            {
                this.SetColorAttribute ("bgColor", value);
            } 
            get
            {
                return this.GetColorAttribute ("bgColor");
            } 
      
        }

        /// <summary>
        /// HSPACE sets the horizontal space between the content and surrounding text. 
        /// </summary>
        /// <remarks>
        /// </remarks>

        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(0)]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int hSpace
        {
            get
            {
                return this.GetIntegerAttribute ("hSpace", 0);
            } 
      
            set
            {
                this.SetIntegerAttribute ("hSpace", value, 0);
            } 
      
        }

        /// <summary>
        /// VSPACE sets the vertical space between the content and surrounding text.
        /// </summary>
        /// <remarks>
        /// The space assigned by this attribute is added on both sides of the element. If a value of 5 is set the 
        /// element grows vertically 10 pixels.
        /// <para>
        /// Set the value to 0 (zero) to remove the attribute. 0 (zero) is the default value.
        /// </para>
        /// </remarks>

        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(0)]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int vSpace
        {
            set
            {
                this.SetIntegerAttribute ("vSpace", value, 0);
            } 
      
            get
            {
                return this.GetIntegerAttribute ("vSpace", 0);
            } 
      
        }

        /// <summary>
        /// SCROLLDELAY, together with SCROLLAMOUNT, sets the speed of the scrolling. 
        /// </summary>
        /// <remarks>
        /// Marquee moves the content by displaying the content, then delaying for some short period of time, then displaying the content again in a new position. SCROLLDELAY sets the amount of delay in milliseconds (a millisecond is 1/1000th of a second). The default delay is 85. 
        /// </remarks>

        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(85)]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int ScrollDelay
        {
            set
            {
                this.SetIntegerAttribute ("scrolldelay", value, 0);
            } 
      
            get
            {
                return this.GetIntegerAttribute ("scrolldelay", 0);
            } 
      
        }

        /// <summary>
        /// SCROLLAMOUNT, together with SCROLLDELAY, sets the speed of the scrolling.
        /// </summary>
        /// <remarks>
        /// Marquee moves the content by displaying the content, then delaying for some short period of time, then displaying the content again in a new position. SCROLLAMOUNT sets the size in pixels of each jump. A higher value for SCROLLAMOUNT makes the marquee scroll faster. The default value is 6.
        /// </remarks>

        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(6)]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int ScrollAmount
        {
            set
            {
                this.SetIntegerAttribute ("scrollamount", value, 0);
            } 
      
            get
            {
                return this.GetIntegerAttribute ("scrollamount", 0);
            } 
      
        }

        /// <summary>
        /// LOOP sets how many times the marquee should loop.
        /// </summary>
        /// <remarks>
        /// The default value (i.e. if you don't put a LOOP attribute at all) is INFINITE, which means that the marquee loops endlessly. 
        /// </remarks>

        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute("INFINITE")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string Loop
        {
            set
            {
                if (!value.ToLower().Equals("infinite"))
                {
                    for(int i = 0; i < value.Length; i++)
                    {
                        if (!Char.IsNumber(value[i]))
                        {
                            throw new ArgumentException("The value must be a number or the string INFINITE");
                        }                        
                    }
                }
                this.SetStringAttribute ("loop", value, "");
            }
      
            get
            {
                return this.GetStringAttribute ("loop", "");
            }       
        }

        /// <summary>
        /// Gets or sets the inner HTML of the Marquee element.
        /// </summary>
        [Browsable(false)]
        public string Content
        {
            set
            {
                base.InnerHtml = value;
            }       
            get
            {
                return base.InnerHtml;
            }        
        }

         /// <summary>
        /// Creates the specified element.
        /// </summary>
        /// <remarks>
        /// The element is being created and attached to the current document, but nevertheless not visible,
        /// until it's being placed anywhere within the DOM. To attach an element it's possible to either
        /// use the <see cref="ElementDom"/> property of any other already placed element and refer to this
        /// DOM or use the body element (<see cref="HtmlEditor.GetBodyElement"/>) and add the element there. Also, in 
        /// case of user interactive solutions, it's possible to add an element near the current caret 
        /// position, using <see cref="HtmlEditor.CreateElementAtCaret(string)"/> method.
        /// <para>
        /// Note: Invisible elements do neither appear in the DOM nor do they get saved.
        /// </para>
        /// </remarks>
        /// <param name="editor">The editor this element belongs to.</param>
        public MarqueeElement(IHtmlEditor editor)
            : base("marquee", editor)
        {
        }

        internal MarqueeElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
