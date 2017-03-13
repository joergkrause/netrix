using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// Native object class for v:textpath element.
	/// </summary>
	/// <remarks>
    /// This sub-element may appear inside a shape or a shapetype to define a vector path based on the text data, font and 
    /// font styles supplied.  The path which results is then mapped into the region defined by the v attribute of the 
    /// shape.
	/// </remarks>
    [ToolboxItem(false)]
    public sealed class TextpathElement : VmlBaseElement
    {

        /// <summary>
        /// Determines whether the character paths are displayed or not.
        /// </summary>
        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(false)]
        [DescriptionAttribute("")]
        [DisplayNameAttribute()]
        public bool On
        {
            get
            {
                return base.GetBooleanAttribute("on");
            }
            set
            {
                base.SetBooleanAttribute("on", value);
            }
        }

        /// <summary>
        /// Stretches the text path out to the edges of the shapebox.
        /// </summary>
        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(false)]
        [DescriptionAttribute("")]
        [DisplayName()]
        public bool FitShape
        {
            get
            {
                return base.GetBooleanAttribute("fitShape");
            }
            set
            {
                base.SetBooleanAttribute("fitShape", value);
            }
        }

        /// <summary>
        /// Sizes the text to fill the path it lays out on.
        /// </summary>
        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(false)]
        [DescriptionAttribute("")]
        [DisplayName()]
        public bool FitPath
        {
            get
            {
                return base.GetBooleanAttribute("fitPath");
            }
            set
            {
                base.SetBooleanAttribute("fitPath", value);
            }
        }

        /// <summary>
        /// Removes any additional space reserved for ascenders and descenders if not used.
        /// </summary>
        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(false)]
        [DescriptionAttribute("")]
        [DisplayName()]
        public bool Trim
        {
            get
            {
                return base.GetBooleanAttribute("trim");
            }
            set
            {
                base.SetBooleanAttribute("trim", value);
            }
        }

        /// <summary>
        /// Use straight x measurement instead of measuring along the path.
        /// </summary>
        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(false)]
        [DescriptionAttribute("")]
        [DisplayName()]
        public bool XScale
        {
            get
            {
                return base.GetBooleanAttribute("xscale");
            }
            set
            {
                base.SetBooleanAttribute("xscale", value);
            }
        }

        /// <summary>
        /// The string to render as a text path.
        /// </summary>
        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(null)]
        [DescriptionAttribute("")]
        [DisplayName()]
        public string @String
        {
            get
            {
                return base.GetStringAttribute("string");
            }
            set
            {
                base.SetStringAttribute("string", value);
            }
        }

		public TextpathElement(IHtmlEditor editor) : base("v:textpath", editor)
		{
		}

        internal TextpathElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
