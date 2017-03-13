using System;

namespace GuruComponents.Netrix.HtmlFormatting
{
    /// <summary>
    /// Formatter options used to determine the overall behavior of the formatter class.
    /// </summary>
    /// <remarks>
    /// The formatter will analyse and refresh the content to beeing HTML 4/XHTML compliant.
    /// This changes the indentation, quotes and newlines. The formatter does not allow
    /// any code preservation.
    /// </remarks>
    public interface IHtmlFormatterOptions
    {
        /// <summary>
        /// Empty P Tags do not render properly in MSHTML. To fix this option adds &amp;nbsp; between.
        /// </summary>
        bool FakeEmptyParaTag
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether whitespaces (space, newline, tab) should be preserved, if possible. Default is false.
        /// </summary>
        bool PreserveWhitespace
        {
            get;
            set;
        }

        /// <summary>
		/// Gets or sets the ability to add JavaScript comments before CDATA token.
		/// </summary>
		/// <remarks>
		/// The XHTML module wraps JavaScript code into &lt;![CDATA[ ]]&gt; sections. Some
		/// script processors have problems reading this. To avoid any scripting issues this
		/// option can set to <c>true</c> and the formatter will add // sign in front of &lt;![CDATA[ ]]&gt;:
		/// <para>
		/// &lt;script>
		/// //&lt;![CDATA[ 
		///		Script goes here
		/// //]]&gt;
		/// &lt;/script>
		/// </para>
		/// <para>
		/// Default setting is <c>false</c> (option not being used).
		/// </para>						
		/// The used string for comment signs defaults to "//". The value can be changed using <see cref="CommentCDATAString"/>.
		/// <seealso cref="CommentCDATAString"/>		
        /// <seealso cref="CommentCDATAAddsNewline"/>
		/// </remarks>		
		bool CommentCDATA
		{
			get;
			set;
		}

		/// <summary>
		/// The string value used if the formatter needs to add comments before CDATA tokens.
		/// </summary>
		/// <remarks>
		/// <seealso cref="CommentCDATA"/>
		/// </remarks>
		string CommentCDATAString
		{
			get;
			set;
		}

        /// <summary>
        /// After CDATA and before ending a newline is added autoamtically.
        /// </summary>
        /// <seealso cref="CommentCDATA"/>
        /// <remarks>By default there is no newline added.</remarks>
        bool CommentCDATAAddsNewline
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ability to clean MS Word styles.
        /// </summary>
        /// <remarks>
        /// MS Word produces some weird output when content was saved as HTML. If such
        /// content was loaded or dropped this formatting feature can strip out the
        /// most of the non HTML 4/XHTML 1 compliant code.
        /// <para>
        /// The value defaults to <c>false</c> and must set explicitly.
        /// </para>
        /// </remarks>
        bool WordClean { get; set; }

        /// <summary>
        /// Set the case of attributes. If <code>true</code> all attribute names are upper case.
        /// </summary>
        HtmlFormatterCase AttributeCasing { get; set; }

        /// <summary>
        /// If true (default) the entities are built as &amp;name; variant, if any known
        /// entity exists. Otherwise any enhanced ASCII or unicode character will be
        /// replaced by the numeric expression &amp;#NNN;.
        /// </summary>
        EntityFormat Entities { get; set; }

        /// <summary>
        /// Set the case of element names. If <code>true</code> all element names are upper case.
        /// </summary>
        HtmlFormatterCase ElementCasing { get; set; }

        /// <summary>
        /// Determines the char used to build indentation.
        /// </summary>
        char IndentChar { get; set; }

        /// <summary>
        /// Determines the number of chars used for each indentation level. Use 1 for tabs and 4 to 8 for whitespace.
        /// </summary>
        int IndentSize { get; set; }

        /// <summary>
        /// If <code>true</code> the output conforms to XHTML 1.0, otherwise to HTML 4.0
        /// If input is XHTML compliant the output will never changed back to HTML 4.0 regardless this option.
        /// </summary>
        bool MakeXhtml { get; set; }

        /// <summary>
        /// Forces a line break after leaving the given number of columns per line.
        /// </summary>
        int MaxLineLength { get; set; }

        /// <summary>
        /// Reformats the CSS in attributes and style tags to remove unneccessary spaces. Default is false.
        /// </summary>
        bool ReformatStyles
        {
            get;
            set;
        }

    }
}
