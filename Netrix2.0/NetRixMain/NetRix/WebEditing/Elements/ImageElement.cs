using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute = GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE = GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements {

  /// <summary>
  /// This class represents the &lt;img&gt; element.
  /// </summary>
  /// <remarks>
  /// <para>
  /// &lt;IMG ...&gt; puts an image on the web page. IMG requires two attributes: <see cref="src"/> and <see cref="alt"/>. <see cref="alt"/> is always required in &lt;IMG ...&gt; - 
  /// don't make the common mistake of leaving it out. See the Rules of <see cref="alt"/> for more details. 
  /// </para>
  /// Classes directly or indirectly inherited from 
  /// <see cref="GuruComponents.Netrix.WebEditing.Elements.Element"/> are not intended to be instantiated
  /// directly by the host application. Use the various insertion or creation methods in the base classes
  /// instead. The return values are of type <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement"/>
  /// and can be casted into the element just created.
  /// <para>
  /// Examples of how to create and modify elements with the native element classes can be found in the 
  /// description of other element classes.
  /// </para>
  /// </remarks>
  public sealed class ImageElement : SelectableElement {
    /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="HorizontalAlign"]/*'/>
    [Category("Element Layout")]
    [DefaultValueAttribute(ImageAlign.NotSet)]
    [DescriptionAttribute("")]
    [TypeConverter(typeof(UITypeConverterDropList))]
    [DisplayNameAttribute()]
    public ImageAlign align {
      set {
        this.SetEnumAttribute("align", value, ImageAlign.NotSet);
        return;
      }

      get {
        return (ImageAlign)this.GetEnumAttribute("align", ImageAlign.NotSet);
      }

    }

    /// <summary>
    /// USEMAP indicates that the image is an image map and uses the map definition named by this attribute. 
    /// </summary>
    /// <remarks>
    /// The name of the map is set by the NAME attribute of the MAP tag. Note that USEMAP requires that you 
    /// precede the name of the map with a hash symbol (#). For example, the following code creates an 
    /// image map named map1: 
    /// <code>
    /// &lt;DIV ALIGN=CENTER&gt;
    ///      &lt;MAP NAME="map1"&gt;
    ///           &lt;AREA 
    ///                HREF="contacts.html" ALT="Contacts" TITLE="Contacts" 
    ///                SHAPE=RECT COORDS="6,116,97,184"&gt;
    ///           &lt;AREA 
    ///                HREF="products.html" ALT="Products" TITLE="Products" 
    ///                SHAPE=CIRCLE COORDS="251,143,47"&gt;
    ///           &lt;AREA 
    ///                HREF="new.html" ALT="New!" TITLE="New!"     
    ///                SHAPE=POLY COORDS="150,217, 190,257, 150,297,110,257"&gt;
    ///           &lt;/MAP&gt;
    ///           &lt;IMG SRC="testmap.gif" ALT="map of GH site" BORDER=0 WIDTH=300 HEIGHT=300
    /// USEMAP="#map1"&gt;&lt;BR&gt;
    /// [ &lt;A HREF="contacts.html" ALT="Contacts"&gt;Contacts&lt;/A&gt; ]
    /// [ &lt;A HREF="products.html" ALT="Products"&gt;Products&lt;/A&gt; ]
    /// [ &lt;A HREF="new.html"      ALT="New!"&gt;New!&lt;/A&gt; ]
    /// &lt;/DIV&gt;
    /// </code>
    /// </remarks>
    [CategoryAttribute("Element Behavior")]
    [DefaultValueAttribute(false)]
    [DescriptionAttribute("")]
    [EditorAttribute(
         typeof(UITypeEditorString),
         typeof(UITypeEditor))]
    [DisplayName()]
    public string usemap {
      get {
        return this.GetStringAttribute("usemap");
      }

      set {
        this.SetStringAttribute("usemap", value);
        return;
      }

    }

    /// <summary>
    /// Use as alt="text" attribute.
    /// </summary>
    /// <remarks>
    /// For user agents that cannot display images, forms, or applets, this 
    /// attribute specifies alternate text. The language of the alternate text is specified by the lang attribute. 
    /// </remarks>
    [CategoryAttribute("Element Layout")]
    [DefaultValueAttribute(false)]
    [DescriptionAttribute("")]
    [EditorAttribute(
         typeof(UITypeEditorString),
         typeof(UITypeEditor))]
    [DisplayName()]
    public string alt {
      get {
        return this.GetStringAttribute("alt");
      }

      set {
        this.SetStringAttribute("alt", value);
        return;
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
    public int border {
      get {
        return this.GetIntegerAttribute("border", 1);
      }

      set {
        this.SetIntegerAttribute("border", value, 1);
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
    [DefaultValueAttribute(0)]
    [DescriptionAttribute("")]
    [EditorAttribute(
         typeof(UITypeEditorInt),
         typeof(UITypeEditor))]
    [DisplayName()]
    public int height {
      set {
        this.SetIntegerAttribute("height", value, -1);
        return;
      }

      get {
        return this.GetIntegerAttribute("height", -1);
      }

    }

    /// <summary>
    /// HSPACE sets the horizontal space between the image and surrounding text. 
    /// </summary>
    /// <remarks>
    /// HSPACE has the most pronounced effect when it is used in conjunction with <see cref="align"/> to right or left align the image.
    /// The space assigned by this attribute is added on both sides of the element. If a value of 5 is set the element grows vertically 10 pixels.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.ImageElement.vSpace">vSpace</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.ImageElement.align">align</seealso>
    /// </remarks>
    [CategoryAttribute("Element Layout")]
    [DefaultValueAttribute(0)]
    [DescriptionAttribute("")]
    [EditorAttribute(
         typeof(UITypeEditorInt),
         typeof(UITypeEditor))]
    [DisplayName()]

    public int hSpace {
      get {
        return this.GetIntegerAttribute("hSpace", 0);
      }

      set {
        this.SetIntegerAttribute("hSpace", value, 0);
        return;
      }

    }

    /// <summary>
    /// Supports the JavaScript event 'doubleclick'.
    /// </summary>
    /// <remarks>
    /// The property should contain the name of a JavaScript mehtod or JavaScript executable code.
    /// </remarks>
    [CategoryAttribute("JavaScript Events")]
    [DefaultValueAttribute("")]
    [DescriptionAttribute("")]
    [EditorAttribute(
         typeof(UITypeEditorString),
         typeof(UITypeEditor))]
    [DisplayName()]
    [ScriptingVisible()]
    public string ScriptOnDblClick {
      get {
        return this.GetStringAttribute("onDblClick");
      }

      set {
        this.SetStringAttribute("onDblClick", value);
        return;
      }

    }

    /// <summary>
    /// Supports the JavaScript event 'mousedown'.
    /// </summary>
    /// <remarks>
    /// The property should contain the name of a JavaScript mehtod or JavaScript executable code.
    /// </remarks>
    [CategoryAttribute("JavaScript Events")]
    [DefaultValueAttribute("")]
    [DescriptionAttribute("")]
    [EditorAttribute(
         typeof(UITypeEditorString),
         typeof(UITypeEditor))]
    [DisplayName()]
    [ScriptingVisible()]
    public string ScriptOnMouseDown {
      set {
        this.SetStringAttribute("onMouseDown", value);
        return;
      }

      get {
        return this.GetStringAttribute("onMouseDown");
      }

    }

    /// <summary>
    /// Supports the JavaScript event 'mouseup'.
    /// </summary>
    /// <remarks>
    /// The property should contain the name of a JavaScript mehtod or JavaScript executable code.
    /// </remarks>
    [CategoryAttribute("JavaScript Events")]
    [DefaultValueAttribute("")]
    [DescriptionAttribute("")]
    [EditorAttribute(
         typeof(UITypeEditorString),
         typeof(UITypeEditor))]
    [DisplayName()]
    [ScriptingVisible()]
    public string ScriptOnMouseUp {
      get {
        return this.GetStringAttribute("onMouseUp");
      }

      set {
        this.SetStringAttribute("onMouseUp", value);
        return;
      }

    }

    /// <summary>
    /// SRC tells where to get the picture that should be put on the page.
    /// </summary>
    /// <remarks>
    /// SRC is the one required attribute. It is recommended to use relative paths. If a filename is given the property will recognize and set
    /// the relative path automatically.
    /// <para>
    /// Note: To bypass the automatic truncation of paths you can use the base class' SetAttribute method, which provides a raw access to 
    /// all supported attributes like <c>img.SetAttribute("src", "file:///c:\mypath\myimage.png");</c>. Further access to <c>src></c> property will
    /// still truncate the value, but only for the caller. It will not change the content until you set the value explicitly through this property.
    /// </para>
    /// </remarks>
    [CategoryAttribute("Element Layout")]
    [DefaultValueAttribute("")]
    [DescriptionAttribute("")]
    [EditorAttribute(
       typeof(UITypeEditorUrl),
       typeof(UITypeEditor))]
    [DisplayName()]
    public string src {
      set {
        this.SetStringAttribute("src", this.GetRelativeUrl(value));
        return;
      }
      get {
        return this.GetRelativeUrl(this.GetStringAttribute("src"));
      }
    }

    /// <summary>
    /// VSPACE sets the vertical space between the image and surrounding text.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.ImageElement.hSpace">hSpace</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.ImageElement.align">align</seealso>
    /// </summary>
    /// <remarks>
    /// The space assigned by this attribute is added on both sides of the element. If a value of 5 is set the element grows vertically 10 pixels.
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
    public int vSpace {
      set {
        this.SetIntegerAttribute("vSpace", value, 0);
        return;
      }

      get {
        return this.GetIntegerAttribute("vSpace", 0);
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
    [DefaultValueAttribute(0)]
    [DescriptionAttribute("")]
    [EditorAttribute(
         typeof(UITypeEditorInt),
         typeof(UITypeEditor))]
    [DisplayName()]
    public int width {
      set {
        this.SetIntegerAttribute("width", value, -1);
      }

      get {
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
    public ImageElement(IHtmlEditor editor)
      : base("img", editor) {
    }

    internal ImageElement(Interop.IHTMLElement peer, IHtmlEditor editor)
      : base(peer, editor) {
    }
  }
}