using System;
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
    /// This class represents an AREA element.
    /// </summary>
    /// <remarks>
    /// &lt;AREA ...&gt; defines a single area within an image map. &lt;AREA ...&gt; requires the SHAPE and ALT attributes. 
    /// In most cases also requires the HREF attribute to indicate the page the area points to.
    /// <para>
    /// A typical AREA section should look like this:
    /// </para>
    /// <code>
///&lt;DIV ALIGN=CENTER>
///&lt;MAP NAME="map1">
///&lt;AREA 
///HREF="contacts.html" ALT="Contacts" TITLE="Contacts"
///SHAPE=RECT COORDS="6,116,  97,184">
///&lt;AREA 
///HREF="products.html" ALT="Products" TITLE="Products"
///SHAPE=CIRCLE COORDS="251,143,  47">
///&lt;AREA 
///HREF="new.html" ALT="New!" TITLE="New!"
///SHAPE=POLY COORDS="150,217, 190,257,  150,297,  110,257">
///&lt;AREA HREF="default.html" SHAPE=DEFAULT>
///&lt;/MAP>
///&lt;IMG SRC="testmap.gif" 
///ALT="map of GH site" BORDER=0 WIDTH=300 HEIGHT=300
///USEMAP="#map1">
///&lt;/DIV>
    /// </code>
    /// </remarks>
    /// <example>
    /// To create a new AREA element one can use the default constructor of this class 
    /// </example>
    public sealed class AreaElement : Element
    {
        /// <summary>
        /// HREF indicates the URL being linked to.
        /// </summary>
        /// <remarks>HREF makes the anchor into a link.</remarks>

        [Category("Element Behavior")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute()]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string href
        {
            set
            {
                this.SetStringAttribute ("href", this.GetRelativeUrl(value));
                return;
            } 
            get
            {
                return this.GetRelativeUrl (this.GetStringAttribute ("href"));
            }  
        }

        /// <summary>
        /// Indicates that this area is not a link.
        /// </summary>
        /// <remarks>
        /// NOHREF indicates that the AREA is not a link. Of course, NOHREF doesn't make much sense unless the area 
        /// is in a map that uses &lt;AREA SHAPE=DEFAULT&gt; or the area overlies another area that does have an HREF. 
        /// For example, suppose we wanted an image map in which a circle in the map is a link, but a smaller circle inside 
        /// the link circle is not a link. We could accomplish that by first putting the tag for the inner area 
        /// (earlier &lt;AREA ...> tags overlay later &lt;AREA ...> tags), and using the NOHREF atribute, then putting 
        /// the tag for the outer &lt;AREA ...>: 
        /// </remarks>

        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(false)]
        [DescriptionAttribute("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

        public bool nohref
        {
            get
            {
                return base.GetBooleanAttribute("nohref");
            }

            set
            {
                base.SetBooleanAttribute("nohref", value);
            }
        }


        [CategoryAttribute("JavaScript Events")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName("onFocus_Area")]

        [ScriptingVisible()] public string ScriptOnFocus
        {
            get
            {
                return base.GetStringAttribute("onfocus");
            }

            set
            {
                base.SetStringAttribute("onfocus", value);
            }
        }


        [CategoryAttribute("JavaScript Events")]
        [DescriptionAttribute("Event raised when a the area left the focus (becomes inactive)")]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName("onBlur_Area")]

        [ScriptingVisible()] public string ScriptOnBlur
        {
            get
            {
                return base.GetStringAttribute("onblur");
            }

            set
            {
                base.SetStringAttribute("onblur", value);
            }
        }

        /// <summary>
        /// Use as alt="text" attribute.
        /// </summary>
        /// <remarks>
        /// For user agents that cannot display images, forms, or applets, this 
        /// attribute specifies alternate text. The language of the alternate text is specified by the lang attribute. 
        /// </remarks>

        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
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


        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string accesskey
        {
            get
            {
                return base.GetStringAttribute("accesskey");
            }

            set
            {
                base.SetStringAttribute("accesskey", (value.Length > 0) ? value.Substring(1) : String.Empty);
            }
        }

        /// <summary>
        /// Coordinates for the link area shape.
        /// </summary>
        /// <remarks>
        /// COORDS indicates whare the shape is located in the image map. COORDS is necessary if you set SHAPE to RECT, CIRCLE, 
        /// or POLY. COORDS is not required if you set SHAPE to DEFAULT. 
        /// <para>Coordinates are always entered as a comma delimited string of numbers. That is, the stirng consists of 
        /// a number, then a comma, then another number, then a comma, etc. Don’t put a comma after the last number. 
        /// The exact meaning of those list of numbers is different for each shape, so see the descriptions of the 
        /// values for the SHAPE attribute for details.</para>
        /// </remarks>

        [CategoryAttribute("Element Layout")]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string coords
        {
            get
            {
                return base.GetStringAttribute("coords");
            }

            set
            {
                base.SetStringAttribute("coords", value);
            }
        }
      
        /// <summary>
        /// The shap which creates the sensitive area.
        /// </summary>                                
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.WebEditing.Elements.AreaShape"/> for more details.
        /// </remarks>

        [DescriptionAttribute("")]
        [DefaultValueAttribute(AreaShape.Default)]
        [CategoryAttribute("Element Layout")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

        public AreaShape shape
        {
            set
            {
                this.SetEnumAttribute ("shape", (AreaShape) value, (AreaShape) 0);
                return;
            } 
      
            get
            {
                return (AreaShape) this.GetEnumAttribute ("shape", (AreaShape) 0);
            }       
        }

        /// <summary>
        /// Gets or set the TAB index.
        /// </summary>
        /// <remarks>
        /// If the user hits the TAB key the focus moves from block element to block element, beginning with the first element on a page.
        /// To change the default TAB chain the tabIndex can be set to any numeric (integer) value which force the order of the TAB key.
        /// The value must be greater or equal than 0 (zero).
        /// <para>
        /// To remove the attribute set the value to 0 (zero).
        /// </para>
        /// </remarks>

        [DefaultValueAttribute(0)]
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorInt),
             typeof(UITypeEditor))]
        [DisplayName()]

        public short tabIndex
        {
            get
            {
                return (short) base.GetIntegerAttribute("tabIndex", 0);
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Values less than 0 are not allowed");
                }
                base.SetIntegerAttribute("tabIndex", (int) value, 0);
            }
        }        

        public override string ToString()
        {
            return String.Concat(@"<AREA shape=""", shape.ToString(), @""">");
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
        public AreaElement(IHtmlEditor editor)
            : base("area", editor)
        {
        }

        internal AreaElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
