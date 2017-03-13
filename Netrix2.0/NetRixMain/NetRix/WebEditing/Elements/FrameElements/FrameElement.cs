using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The FRAME element defines a frame--a rectangular subspace within a Frameset document.
    /// </summary>
    /// <remarks>
    /// Each FRAME must be contained within a FRAMESET that defines the dimensions of the frame.
    /// </remarks>
	public sealed class FrameElement : StyledElement
	{

        /// <summary>
        /// The SRC attribute provides the URI of the frame's content.
        /// </summary>
        /// <remarks>
        /// typically an HTML document. If the frame's content is an image, video, or similar object, and 
        /// if the object cannot be described adequately using the TITLE attribute of FRAME, then authors should 
        /// use the LONGDESC attribute to provide the URI of a full HTML description of the object.
        /// </remarks>

        [Category("Standard Values")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
		[EditorAttribute(
			 typeof(UITypeEditorUrl),
			 typeof(UITypeEditor))]
        [DisplayNameAttribute()]

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
        /// The FRAMEBORDER attribute specifies whether or not the frame has a visible border.
        /// </summary>
        /// <remarks>
        /// The default value, 1, tells the browser to draw a border between the frame and all adjoining frames. The value 0 indicates 
        /// that no border should be drawn, though borders from other frames will override this.
        /// The NetRix property will transform the boolean value into 1 or 0 accordingly to this definition.
        /// </remarks>

		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute(true)]
		[DescriptionAttribute("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

		public bool frameBorder
		{
			set
			{
				this.SetAttribute ("frameBorder", (value) ? 1 : 0);
				return;
			} 
      
			get
			{
				return this.GetIntegerAttribute ("frameBorder", 1)  ==  1;
			} 
      
		}

        /// <summary>
        /// The boolean NORESIZE attribute prevents the user from resizing the frame.
        /// </summary>
        /// <remarks>
        /// This attribute should never be used in a user-friendly Web site.
        /// </remarks>

		[DefaultValueAttribute(false)]
		[DescriptionAttribute("")]
		[CategoryAttribute("Element Behavior")]
		[TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

        public bool NoResize
		{
			get
			{
				return base.GetBooleanAttribute("noresize");
			}

			set
			{
				base.SetBooleanAttribute("noresize", value);
			}
		}

        /// <summary>
        /// The SCROLLING attribute specifies whether scrollbars are provided for the frame.
        /// </summary>
        /// <remarks>
        /// The default value, auto, generates scrollbars only when necessary. The value yes gives scrollbars 
        /// at all times, and the value no suppresses scrollbars--even when they are needed to see all the 
        /// content. The value no should never be used.
        /// </remarks>

		[DefaultValueAttribute(false)]
		[DescriptionAttribute("")]
		[CategoryAttribute("Element Behavior")]
        [TypeConverter(typeof(UITypeConverterDropList))]
		[DisplayName()]

        public ScrollType Scrolling
		{
			get
			{
				return (ScrollType) base.GetEnumAttribute("scrolling", ScrollType.Auto);
			}

			set
			{
				base.SetEnumAttribute("scrolling", value, ScrollType.Auto);
			}
		}

        /// <summary>
        /// The MARGINHEIGHT attribute defines the number of pixels to use as the top/bottom margins within the frame.
        /// </summary>
        /// <remarks> The value must be non-negative.</remarks>

		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute(-1)]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

		public int marginHeight
		{
			get
			{
				return this.GetIntegerAttribute ("marginHeight", -1);
			} 
      
			set
			{
				this.SetIntegerAttribute ("marginHeight", value, -1);
				return;
			} 
      
		}

        /// <summary>
        /// The MARGINWIDTH attribute defines the number of pixels to use as the left/right margins within the frame. 
        /// </summary>
        /// <remarks>The value must be non-negative.</remarks>

		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute(-1)]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public int marginWidth
		{
			set
			{
				this.SetIntegerAttribute ("marginWidth", value, -1);
				return;
			} 
      
			get
			{
				return this.GetIntegerAttribute ("marginWidth", -1);
			} 
      
		}

        /// <summary>
        /// The NAME attribute gives a name to the frame for use with the TARGET attribute.
        /// </summary>
        /// <remarks>
        ///  The TARGET attribute is used in the A, AREA, BASE, FORM, and LINK elements. The NAME 
        ///  attribute value must begin with a character in the range A-Z or a-z.
        ///  <para>
        ///  Cannot be set from within the propertygrid due to synchronization problems with the
        ///  underlying frame management. To rename the host application must refresh the collection
        ///  of frames within the HtmlFrameSet class.
        ///  </para>
        /// </remarks>

		[CategoryAttribute("Standard Values")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("name_Frame")]
        [ReadOnly(true)]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
		[DisplayName("name_Frame")]

        public string Name
		{
			set
			{
				this.SetStringAttribute ("name", value);
				return;
			} 
      
			get
			{
				return this.GetStringAttribute ("name");
			} 
      
		}


		[CategoryAttribute("JavaScript Events")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnActivate
		{
			set
			{
				this.SetStringAttribute ("onActivate", value);
				return;
			} 
      
			get
			{
				return this.GetStringAttribute ("onActivate");
			} 
      
		}


		[CategoryAttribute("JavaScript Events")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnDeactivate
		{
			set
			{
				this.SetStringAttribute ("onDeactivate", value);
				return;
			} 
      
			get
			{
				return this.GetStringAttribute ("onDeactivate");
			} 
      
		}


		[CategoryAttribute("JavaScript Events")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnLoad
		{
			set
			{
				this.SetStringAttribute ("onLoad", value);
				return;
			} 
      
			get
			{
				return this.GetStringAttribute ("onLoad");
			} 
      
		}


		[CategoryAttribute("JavaScript Events")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnScroll
		{
			set
			{
				this.SetStringAttribute ("onScroll", value);
				return;
			} 
  
			get
			{
				return this.GetStringAttribute ("onScroll");
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
        public FrameElement(IHtmlEditor editor)
            : base("frame", editor)
        {
        }

		internal FrameElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
		}

	}
}