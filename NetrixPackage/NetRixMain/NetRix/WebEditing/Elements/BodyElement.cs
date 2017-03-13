using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The BODY element contains the document body.
    /// </summary>
    /// <remarks>
    /// BODY is required in non-frames documents, but its start and end tags are always optional. In frames documents, BODY must be contained within the NOFRAMES element, if NOFRAMES is used.
    /// </remarks>
    //[Editor(typeof(MyComponentEditor), typeof(ComponentEditor))] 
    public class BodyElement : StyledElement
	{

        /// <summary>
        /// Hide the inherited member. Not used.
        /// </summary>
		[Browsable(false)]
        public override string title
		{
			get
			{
				return String.Empty;
			}
		}

        /// <summary>
        /// ALINK suggests an active link color. 
        /// </summary>
        /// <remarks>
        /// Active means the link is selected by user. It is recommended to use style sheets (CSS) instead.
        /// </remarks>

        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(typeof(Color), "")]
        [DescriptionAttribute("")]
        [TypeConverterAttribute(typeof(UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UITypeEditorColor),
			 typeof(UITypeEditor))]
        [DisplayNameAttribute()]
		public Color aLink
        {
            get
            {
                return base.GetColorAttribute("aLink");
            }

            set
            {
                base.SetColorAttribute("aLink", value);
            }
        }

        /// <summary>
        /// BGPROPERTIES defines the behavior of a background image.
        /// </summary>
        /// <remarks>
        /// This attribute is not HTML 4.0 conform and is recognized by MSIE only. It is not recommended to use
        /// this property.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.BackgroundPropertyType">BackgroundPropertyType</seealso>
        /// </remarks>

        [DefaultValueAttribute(BackgroundPropertyType.Scroll)]
        [DescriptionAttribute("")]
        [CategoryAttribute("Element Behavior")]
		[DisplayName()]
        [TypeConverter(typeof(UITypeConverterDropList))]

		public BackgroundPropertyType bgProperties
        {
            get
            {
                return (BackgroundPropertyType)base.GetEnumAttribute("bgProperties", BackgroundPropertyType.None);
            }

            set
            {
                base.SetEnumAttribute("bgProperties", value, BackgroundPropertyType.None);
            }
        }

        /// <summary>
        /// The BACKGROUND attribute suggests a background image for tiling on the document canvas.
        /// </summary>
        /// <remarks>
        /// To help ensure a readable document, the <see cref="GuruComponents.Netrix.WebEditing.Elements.BodyElement.bgColor">bgColor</see>, 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.BodyElement.text">text</see>, 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.BodyElement.link">link</see>, 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.BodyElement.vLink">vLink</see>, and 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.BodyElement.aLink">aLink</see> attributes should always be included when BACKGROUND is given. The BGCOLOR will be used for those not loading images.
        /// 
        /// </remarks>

        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Behavior")]
		[EditorAttribute(
			 typeof(UITypeEditorUrl),
			 typeof(UITypeEditor))]
        [DisplayName()]

		public string background
        {
            set
            {
                this.SetStringAttribute ("background", this.GetRelativeUrl(value));
                return;
            } 
            get
            {
                return this.GetRelativeUrl (this.GetStringAttribute ("background"));
            }  
        }

        /// <summary>
        /// BGCOLOR suggests a background color.
        /// </summary>
        /// <remarks>
        /// If the BACKGROUND attribute is set and the images is available this attribute is not recognized.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.BodyElement.background">background</seealso>
        /// </remarks>
        [DefaultValueAttribute(typeof(Color), "")]
        [CategoryAttribute("Element Layout")]
        [DescriptionAttribute("")]
		[TypeConverterAttribute(typeof(UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UITypeEditorColor),
			 typeof(UITypeEditor))]
		[DisplayName()]
		public Color bgColor
        {
            get
            {
                return base.GetColorAttribute("bgColor");
            }

            set
            {
                base.SetColorAttribute("bgColor", value);
            }
        }

        /// <summary>
        /// Set the BOTTOMMARGIN attribute.
        /// </summary>
        /// <remarks>
        /// The attribute sets the margin at the bottom of the page.
        /// This attribute defaults to 15 pixels. To remove the attribute completely set the value to -1.
        /// </remarks>

        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(15)]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

		public int bottomMargin
        {
            get
            {
                return base.GetIntegerAttribute("bottomMargin", -1);
            }

            set
            {
                base.SetIntegerAttribute("bottomMargin", value, -1);
            }
        }

        /// <summary>
        /// Set the LEFTMARGIN attribute.
        /// </summary>
        /// <remarks>
        /// The attribute sets the margin at the left side of the page.
        /// This attribute defaults to 10 pixels. To remove the attribute completely set the value to -1.
        /// </remarks>

        [DescriptionAttribute("")]
        [DefaultValueAttribute(10)]
        [CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
		[DisplayName()]

		public int leftMargin
        {
            get
            {
                return base.GetIntegerAttribute("leftmargin", -1);
            }

            set
            {
                base.SetIntegerAttribute("leftMargin", value, -1);
            }
        }

        /// <summary>
        /// LINK suggests a link color. 
        /// </summary>
        /// <remarks>
        /// This value can be overwritten by style sheets. It applies to ANCHOR tags only.
        /// </remarks>

        [DefaultValueAttribute(typeof(Color), "")]
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
		[TypeConverterAttribute(typeof(UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UITypeEditorColor),
			 typeof(UITypeEditor))]
		[DisplayName()]

		public Color link
        {
            get
            {
                return base.GetColorAttribute("link");
            }

            set
            {
                base.SetColorAttribute("link", value);
            }
        }


        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(false)]
        [DescriptionAttribute("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

		public bool noWrap
        {
            get
            {
                return base.GetBooleanAttribute("noWrap");
            }

            set
            {
                base.SetBooleanAttribute("noWrap", value);
            }
        }

        /// <summary>
        /// Set the RIGHTMARGIN attribute.
        /// </summary>
        /// <remarks>
        /// The attribute sets the margin at the right side of the page.
        /// This attribute defaults to 10 pixels. To remove the attribute completely set the value to -1.
        /// </remarks>

        [DescriptionAttribute("")]
        [DefaultValueAttribute(10)]
        [CategoryAttribute("Element Layout")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

		public int rightMargin
        {
            get
            {
                return base.GetIntegerAttribute("rightMargin", -1);
            }

            set
            {
                base.SetIntegerAttribute("rightMargin", value, -1);
            }
        }


        [DefaultValueAttribute(ScrollType.Auto)]
        [DescriptionAttribute("")]
        [CategoryAttribute("Element Behavior")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

		public ScrollType scroll
        {
            get
            {
                return (ScrollType)base.GetEnumAttribute("scroll", ScrollType.Auto);
            }

            set
            {
                base.SetEnumAttribute("scroll", value, ScrollType.Auto);
            }
        }

        /// <summary>
        /// TEXT suggests a text color.
        /// </summary>
        /// <remarks>
        /// It is recommended to use style sheets (CSS) instead of setting this attribute to a static color.
        /// </remarks>

        [DescriptionAttribute("")]
		[CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(typeof(Color), "")]
		[TypeConverterAttribute(typeof(UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UITypeEditorColor),
			 typeof(UITypeEditor))]
		[DisplayName()]

		public Color text
        {
            get
            {
                return base.GetColorAttribute("text");
            }

            set
            {
                base.SetColorAttribute("text", value);
            }
        }

        /// <summary>
        /// Set the TOPMARGIN attribute.
        /// </summary>
        /// <remarks>
        /// The attribute sets the margin at the top of the page.
        /// This attribute defaults to 15 pixels. To remove the attribute completely set the value to -1.
        /// </remarks>

        [DescriptionAttribute("")]
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(15)]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

		public int topMargin
        {
            get
            {
                return base.GetIntegerAttribute("topMargin", -1);
            }

            set
            {
                base.SetIntegerAttribute("topMargin", value, -1);
            }
        }

        /// <summary>
        /// VLINK suggests a visited link color.
        /// </summary>
        /// <remarks>
        /// The visited link table is stored in the browser. The time the links remains as visited cannot be
        /// controlled by server application. Therefore the behavior is not predictable nor reliable.
        /// </remarks>

        [DefaultValueAttribute(typeof(Color), "")]
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
		[TypeConverterAttribute(typeof(UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UITypeEditorColor),
			 typeof(UITypeEditor))]
		[DisplayName()]

		public Color vLink
        {
            get
            {
                return base.GetColorAttribute("vLink");
            }

            set
            {
                base.SetColorAttribute("vLink", value);
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
        public BodyElement(IHtmlEditor editor)
            : base("body", editor)
        {
        }

        internal BodyElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
		}

	}
}