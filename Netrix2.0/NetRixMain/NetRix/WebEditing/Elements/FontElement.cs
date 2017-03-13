using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The FONT element allows authors to suggest rudimentary font changes. Deprecated.
    /// </summary>
    /// <remarks>
    /// The element is deprecated in HTML 4.0 in favor of style sheets.
    /// Use of the FONT element brings numerous usability and accessibility problems.
    /// <para>
    /// The following articels on behalf of Warren Steel explains whats wrong with the FONT element. 
    /// </para>
    /// <para>
    /// When Netscape introduced its FONT element, with its SIZE= and COLOR= attributes, many web authors welcomed the promise of control over the presentation of their documents; the same authors felt a twinge of anticipation when Microsoft introduced an additional FACE= attribute. Many of these authors did not realize that their documents would become invisible, illegible, or inaccessible to many viewers. Yet this is exactly what has happened, due to mistaken expectations and faulty implementation in popular browsers. </para>
    /// <para>Extensions to HTML are said to "degrade gracefully" if they do not interfere with basic legibility in browsers that do not support these extensions. For character-mode browsers such as Lynx, or other browsers that do not support font sizes, colors, and styles, the effects of the FONT element are relatively benign. If the author tries to emphasize specific text by its size or color, the user of the text-mode browser will see the text, but will not see the emphasis. If the author uses font settings instead of HTML headings, the same user will not see headings, and neither will the search engine or indexer looking for keywords in high-level headings to display prominently in the search results. But at least, the Lynx user will be able to see the text.</para>
    /// <para>The truly insidious effects of the FONT element are reserved for users of popular graphic browsers like Netscape and Internet Explorer.</para>
    /// <list type="number">
    /// <item>One Netscape user has a laptop with a relatively small display; she has small fonts configured in order to view a decent amount of material on screen. The author has designed a table or other page with &lt;FONT SIZE=1&gt;; this text is now so tiny that it's illegible.</item>
    /// <item>Another nearsighted user has his laptop configured with extra-large fonts, and the "web page designer" decides to make a statement with &lt;FONT SIZE="+4"&gt;; now a single phrase occupies most of the display window, and the user gives up in frustration.</item>
    /// <item>A third user has a sophisticated workstation with a large monitor, but, like ten percent of male computer users, he is colorblind, and requires a strong contrast between ordinary text and background. He has carefully configured his browser to use black text on a yellow background, and has specified that his color scheme should override document colors. Along comes a "kewl" web page with white text on a black background. Our user is overriding bodycolors, so he still sees black on yellow. But this page also contains a sentence here, a phrase there, emphasized with &lt;FONT COLOR="yellow"&gt; - this worked fine on the author's black background, but is completely 
    /// invisible against the user's yellow background. He may wonder what all those empty spaces mean, 
    /// but unless he views the HTML source, he will never know that he is missing an important part of the author's message. While Netscape allows the user to override text and body colors, it does not allow the user to override font colors.</item>
    /// <item>Howard P. Marvel suggests that an author might set the &lt;FONT COLOR= &gt; attribute to contrast with a table cell whose background color is set with the proprietary &lt;TD BGCOLOR= &gt; attribute recognized by some browsers. Users of a still-popular browser such as Netscape 2.0, which recognizes &lt;FONT COLOR= &gt; but not &lt;TD BGCOLOR= &gt; may not be able to read this "highlighted" text at all. </item>
    /// <item>A large corporation opens a website to serve its customers in Eastern Europe. Knowing that most of their viewers will have at least one font with an Eastern European Roman or Cyrillic character set, they are careful to set the proper "charset" in the server headers. But then, in an attempt to give the page a distinctive "look and feel," they add the tag &lt;FONT FACE="Arial,Helvetica,Geneva"&gt;; the Netscape user on a Windows or Mac system may well have at least one of these fonts installed, but only in the common Western European character set. Here, the use of the FONT element means that the user must delete the specified fonts from his system in order to read the text in his native language and alphabet.</item>
    /// <item>An author who "enhances" her pages for Microsoft Internet Explorer decides to play desktop publisher and inserts the &lt;FONT FACE= &gt; tag in her pages in the hope that others will see what she sees on her Macintosh. The viewer, using Netscape 3.0 on a Unix workstation, sees some approximation of the author's font, but here the letters appear so crowded, the kerning and leading so deranged, that the author's exquisite taste has become an ugly hash. The viewer knows he can modify the X-resources, but thinks instead: why bother, I'll just hit the Back button. </item>
    /// </list>
    /// <para>The font tag is a hindrance to communication over the World Wide Web because it makes too many assumptions about the user's system, browser, and configuration. Cascading Style Sheets, on the other hand, negotiate between author and viewer to create a carefully-designed appearance that is accessible to all. People create web documents for many reasons. If you have something to say, information to provide, a message to preach, feelings to express, a product to sell, then it's in your interest to make your work accessible. Smart web authors, who want to get their message across, stay far away from the FONT element. </para>
    /// <para>Is the FONT element ever appropriate? Not the COLOR= or FACE= attributes, nor absolute values of the SIZE= attribute (e.g. &lt;FONT SIZE=1&gt;). Relative values may be useful in moderation as a gentle (and expendable) form of emphasis, or to mark legalistic disclaimers or other "fine print." In other words, &lt;FONT SIZE="+1"&gt; and &lt;FONT SIZE="-1"&gt; may be acceptable. But their appearance is precisely duplicated by the long-standing HTML tags &lt;BIG&gt; and &lt;SMALL&gt;. The FONT element is broken in current implementations, is prone to cause unpredictable and unavoidable data loss, and is quickly becoming obsolete with the advent of Style Sheets.</para>
    /// <para>
    /// The FONT element is an inline element, meaning that it cannot contain block-level elements such as P and TABLE. Again, style sheets provide much more flexibility in suggesting font styles.
    /// </para>
    /// </remarks>
    [Obsolete("Element is deprecated in HTML 4. Use Span + CSS instead.")]
    public sealed class FontElement : StyledElement
    {

        /// <summary>
        /// The COLOR attribute suggests a text color.
        /// </summary>
        /// <remarks>
        /// While most browsers allow users to override author color changes, the widely used Netscape 
        /// Navigator 2.x, 3.x, and 4.x do not override colors specified with FONT. This makes the COLOR 
        /// attribute very dangerous from an accessibility point of view.
        /// </remarks>

        [Category("Element Layout")]
        [DefaultValueAttribute(typeof(Color), "")]
        [DescriptionAttribute("")]
		[TypeConverterAttribute(typeof(UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UITypeEditorColor),
			 typeof(UITypeEditor))]
        [DisplayNameAttribute()]

		public Color color
        {
            get
            {
                return base.GetColorAttribute("color");
            }

            set
            {
                base.SetColorAttribute("color", value);
            }
        }

        /// <summary>
        /// The FACE attribute gives a comma-separated list of font faces in which to display text.
        /// </summary>
        /// <remarks>
        /// The fonts are listed in order of preference, so that if the browser does not have the first font listed, it will try the second, then the third, and so on.
        /// </remarks>

        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Layout")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorFont),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string face
        {
            get
            {
                string font = base.GetStringAttribute("face", "undefined");
                return font.Equals("undefined") ? String.Empty : font;
            }

            set
            {
                base.SetStringAttribute("face", value, "undefined");
            }
        }

        /// <summary>
        /// The SIZE of the content.
        /// </summary>

        [DescriptionAttribute("")]
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(typeof(FontUnit), "")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

        public FontUnit size

        {
            get
            {
                object local = base.GetStringAttribute("size", "undefined");
                if (local == null)
                {
                    return FontUnit.Empty;
                }
                string str = (String)local;
                switch (str)
                {
                    case "undefined":
                    case "0":
                        return FontUnit.Empty;
                    case "+1":
                        return FontUnit.Larger;
                    case "-1":
                        return FontUnit.Smaller;
                    default:
                        int i;
                        try
                        {
                            i = Int32.Parse(str, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            FontUnit fontUnit = FontUnit.Empty;
                            return fontUnit;
                        }
                        if (i > 7 || i <= 0)
                        {
                            return FontUnit.Empty;
                        }
                        else
                        {
                            return (FontUnit)(i);
                        }
                }
            }
            set
            {
                string str = null;
                switch (value)
                {
                    case FontUnit.Empty:
                        base.RemoveAttribute("size");
                        return;
                    case FontUnit.Smaller:
                        str = "-1";
                        break;
                    case FontUnit.Larger:
                        str = "+1";
                        break;
                    case FontUnit.XSmall:
                    case FontUnit.XXSmall:
                    case FontUnit.Small:
                    case FontUnit.Medium:
                    case FontUnit.Large:
                    case FontUnit.XLarge:
                    case FontUnit.XXLarge:
                        int i = (int) value;
                        str = i.ToString();
                        break;
                }
                base.SetStringAttribute("size", str, "undefined");
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
        public FontElement(IHtmlEditor editor)
            : base("font", editor)
        {
        }

        internal FontElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
