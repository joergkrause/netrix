using System.ComponentModel;
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
    /// Defines a image element in a form.
    /// </summary>
    /// <remarks>
    /// This is one of the classes related to the INPUT element. The type of element ist fixed
    /// by the type property.
    /// </remarks>
    public sealed class InputImageElement : InputElement
    {

        /// <summary>
        /// Gets the type of input element.
        /// </summary>
        /// <remarks>
        /// This property is for information only and cannot be changed.
        /// </remarks>

		[Browsable(true)]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public override string type
		{
			get
			{
				return "image";
			}
		}

        /// <summary>
        /// Use as alt="text" attribute.
        /// </summary>
        /// <remarks>
        /// For user agents that cannot display images, forms, or applets, this 
        /// attribute specifies alternate text. The language of the alternate text is specified by the lang attribute. 
        /// </remarks>

        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string alt
        {
            get
            {
                return base.GetStringAttribute("alt");
            }

            set
            {
                base.SetStringAttribute("alt", value);
            }
        }

        /// <summary>
        /// HSPACE sets the horizontal space between the image and surrounding text. 
        /// </summary>
        /// <remarks>
        /// HSPACE has the most pronounced effect when it is used in conjunction with 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.InputImageElement.vSpace">vSpace</see> to right or left 
        /// align the image.
        /// <seealso cref="vSpace"/>
        /// </remarks>

        [DefaultValueAttribute(0)]
        [Description("")]
        [DisplayName()]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [CategoryAttribute("Element Layout")]

        public int hSpace
        {
            get
            {
                return base.GetIntegerAttribute("hSpace", 0);
            }

            set
            {
                base.SetIntegerAttribute("hSpace", value, 0);
            }
        }

        /// <summary>
        /// SRC tells where to get the picture that should be put on the page. SRC is the one required attribute.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>

        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Behavior")]
        [Description("")]
        [DisplayName()]
		[EditorAttribute(
			 typeof(UITypeEditorUrl),
			 typeof(UITypeEditor))]

        public string src
        {
            set
            {
                this.SetStringAttribute ("src", this.GetRelativeUrl(value));
                return;
            } 
            get
            {
                return this.GetRelativeUrl (this.GetStringAttribute ("src"));
            }  
        }

        /// <summary>
        /// VSPACE sets the vertical space between the image and surrounding text.
        /// </summary>
        /// <remarks>
        /// <seealso cref="hSpace"/>
        /// </remarks>
        [Description("")]
        [DisplayName()]
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(0)]

        public int vSpace
        {
            get
            {
                return base.GetIntegerAttribute("vSpace", 0);
            }

            set
            {
                base.SetIntegerAttribute("vSpace", value, 0);
            }
        }

        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="HorizontalAlign"]/*'/>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(ImageAlign.NotSet)]
        [DescriptionAttribute("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]
        public ImageAlign align
        {
            set
            {
                this.SetEnumAttribute ("align", value, ImageAlign.NotSet);
                return;
            } 
      
            get
            {
                return (ImageAlign) this.GetEnumAttribute ("align", ImageAlign.NotSet);
            } 
      
        }

        /// <summary>
        /// BORDER is most useful for removing the visible border around images which are inside links.
        /// </summary>
        /// <remarks>
        /// By default images inside lunks have visible borders around them to indicate that they are links. However, user generally recognize these "link moments" and the border merely detracts from the appearance of the page.
        /// </remarks>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(1)]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int border
        {
            get
            {
                return this.GetIntegerAttribute ("border", 1);
            } 
      
            set
            {
                this.SetIntegerAttribute ("border", value, 1);
                return;
            } 
      
        }

        /// <summary>
        /// WIDTH and HEIGHT tell the browser the dimensions of the image.
        /// </summary>
        /// <remarks>
        /// The browser can use this information to reserve space for the image as it contructs the page, even though the image has not downloaded yet.
        /// </remarks>

        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(30)]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int height
        {
            set
            {
                this.SetIntegerAttribute ("height", value, 30);
                return;
            } 
      
            get
            {
                return this.GetIntegerAttribute ("height", 30);
            } 
      
        }

        /// <summary>
        /// WIDTH and HEIGHT tell the browser the dimensions of the image.
        /// </summary>
        /// <remarks>
        /// The browser can use this information to reserve space for the image as it contructs the page, even though the image has not downloaded yet.
        /// <para>
        /// Set the value to -1 to remove the attribute. The default value for the UI defaults to 30. This is simple for user support. Removing the attribute
        /// will not set the value to 30 nor the element will inherit that behavior.
        /// </para>
        /// </remarks>

        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(30)]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int width
        {
            set
            {
                this.SetIntegerAttribute ("width", value, -1);
            } 
      
            get
            {
                return this.GetIntegerAttribute("width", -1);
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
        public InputImageElement(IHtmlEditor editor)
            : base(@"input type=""image""", editor)
        {
        }


        internal InputImageElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
