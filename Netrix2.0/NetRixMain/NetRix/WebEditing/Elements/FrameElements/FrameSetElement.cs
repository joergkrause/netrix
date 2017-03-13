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
    /// The FRAMESET element is a frame container for dividing a window into rectangular subspaces called frames.
    /// </summary>
    /// <remarks>
    /// In a Frameset document, the outermost FRAMESET element takes the place of BODY and immediately follows the HEAD.
    /// <para>The FRAMESET element contains one or more FRAMESET or FRAME elements, along with an optional NOFRAMES element to provide alternate content for browsers that do not support frames or have frames disabled. A meaningful NOFRAMES element should always be provided and should at the very least contain links to the main frame or frames.</para>
    /// </remarks>
	public sealed class FrameSetElement : StyledElement
	{


		[Category("Element Layout")]
		[DefaultValueAttribute(typeof(Color), "")]
		[DescriptionAttribute("")]
		[TypeConverterAttribute(typeof(UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UITypeEditorColor),
			 typeof(UITypeEditor))]
		[DisplayNameAttribute()]
        public new Color BorderColor
		{
			get
			{
				return base.GetColorAttribute("bordercolor");
			}

			set
			{
				base.SetColorAttribute("bordercolor", value);
			}
		}


		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute(true)]
        [TypeConverter(typeof(UITypeConverterDropList))]
		[DescriptionAttribute("")]
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
        /// The ROWS attribute defines the dimensions of each frame in the set.
        /// </summary>
        /// <remarks>
        /// The attribute takes a comma-separated list of lengths, specified in pixels, as a percentage, 
        /// or as a relative length. A relative length is expressed as i* where i is an integer. 
        /// For example, a frameset defined with ROWS="3*,*" (* is equivalent to 1*) will have its 
        /// first row allotted three times the height of the second row.
        /// </remarks>

		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

		public string Rows
		{
			set
			{
				this.SetStringAttribute ("rows", value, "*,*");
				return;
			} 
      
			get
			{
				return this.GetStringAttribute ("rows", "*,*");
			} 
      
		}

        /// <summary>
        /// The COLS attribute defines the dimensions of each frame in the set.
        /// </summary>
        /// <remarks>
        /// The attribute takes a comma-separated list of lengths, specified in pixels, as a percentage, 
        /// or as a relative length. A relative length is expressed as i* where i is an integer. 
        /// For example, a frameset defined with COLS="3*,*" (* is equivalent to 1*) will have its 
        /// first column allotted three times the height of the second column.
        /// </remarks>

		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

		public string Cols
		{
			set
			{
				this.SetStringAttribute ("cols", value, "*,*");
				return;
			} 
      
			get
			{
				return this.GetStringAttribute ("cols", "*.*");
			} 
      
		}


		[CategoryAttribute("Element Layout")]
		[DefaultValueAttribute(0)]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

		public int Border
		{
			get
			{
				return this.GetIntegerAttribute ("border", 0);
			} 
      
			set
			{
				this.SetIntegerAttribute ("border", value, 0);
				return;
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

		[ScriptingVisible()] public string ScriptOnUnLoad
		{
			set
			{
				this.SetStringAttribute ("unLoad", value);
				return;
			} 
  
			get
			{
				return this.GetStringAttribute ("unLoad");
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
        public FrameSetElement(IHtmlEditor editor)
            : base("frameset", editor)
        {
        }

		internal FrameSetElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
		}

	}
}