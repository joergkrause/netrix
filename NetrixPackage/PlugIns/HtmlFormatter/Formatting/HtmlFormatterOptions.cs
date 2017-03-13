using System.ComponentModel;
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
    [Serializable()]
    public sealed class HtmlFormatterOptions : IHtmlFormatterOptions
    {
        private HtmlFormatterCase _elementCasing;
        private HtmlFormatterCase _attributeCasing;
        private bool _makeXhtml;
        private char _indentChar;
        private int _indentSize;
        private int _maxLineLength;
        private EntityFormat _Entities;
        private bool _wordClean = false;
		private bool _commentCDATA = true;
		private string _commentCDATAString = "//";
        private bool _commentCDATAAddsNewline = false;
        private bool _preserveWhitespace;
        private bool _fakeEmptyParaTag = true;
        private bool _reformatStyles = false;

        /// <summary>
        /// After CDATA and before ending a newline is added autoamtically.
        /// </summary>
        /// <seealso cref="CommentCDATA"/>
        /// <remarks>By default there is no newline added.</remarks>
        [Browsable(true), Description("Empty P Tags do not render properly in MSHTML. To fix this option adds &nbsp; between."),
         Category("Formatting"), DefaultValue(false)]
        public bool CommentCDATAAddsNewline
        {
            get { return _commentCDATAAddsNewline; }
            set { _commentCDATAAddsNewline = value; }
        }

        /// <summary>
        /// Empty P Tags do not render properly in MSHTML. To fix this option adds &amp;nbsp; between.
        /// </summary>
        [Browsable(true), Description("Empty P Tags do not render properly in MSHTML. To fix this option adds &nbsp; between."),
         Category("Formatting"), DefaultValue(false)]
        public bool FakeEmptyParaTag
        {
            get { return _fakeEmptyParaTag; }
            set { _fakeEmptyParaTag = value; }
        }

        /// <summary>
        /// Indicates whether whitespaces (space, newline, tab) should be preserved, if possible. Default is false.
        /// </summary>
        [Browsable(true), Description("Indicates whether whitespaces (space, newline, tab) should be preserved, if possible."),
        Category("Formatting"), DefaultValue(false)]
        public bool PreserveWhitespace
        {
            get { return _preserveWhitespace; }
            set { _preserveWhitespace = value; }
        }

        /// <summary>
        /// Reformats the CSS in attributes and style tags to remove unneccessary spaces. Default is false.
        /// </summary>
        [Browsable(true), Description("Reformats the CSS in attributes and style tags to remove unneccessary spaces."),
        Category("CSS"), DefaultValue(false)]
        public bool ReformatStyles
        {
            get { return _reformatStyles; }
            set { _reformatStyles = value; }
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
		/// </remarks>		
        [Browsable(true), Description(" Gets or sets the ability to add JavaScript comments before CDATA token."),
         Category("CDATA"), DefaultValue(false)]
        public bool CommentCDATA
		{
			get
			{                  
				return this._commentCDATA;
			}
			set
			{
				this._commentCDATA = value;
			}
		}

		/// <summary>
		/// The string value used if the formatter needs to add comments before CDATA tokens.
		/// </summary>
		/// <remarks>
		/// <seealso cref="CommentCDATA"/>
		/// </remarks>
        [Browsable(true), Description("The string value used if the formatter needs to add comments before CDATA tokens."),
           Category("CDATA"), DefaultValue("//")]
        public string CommentCDATAString
		{
			get
			{
				return this._commentCDATAString;
			}
			set
			{
				this._commentCDATAString = value;
			}
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
        [Browsable(true), Description("Gets or sets the ability to clean MS Word styles."),
       Category("Formatting"), DefaultValue(false)]
        public bool WordClean
        {
            get
            {
                return this._wordClean;
            }
            set
            {
                this._wordClean = value;
            }
        }
        /// <summary>
        /// Set the case of attributes. If <code>true</code> all attribute names are upper case.
        /// </summary>
        [Browsable(true), Description("Set the case of attributes."),
       Category("Attributes"), DefaultValue(false)]
        public HtmlFormatterCase AttributeCasing
        {
            get
            {
                return _attributeCasing;
            }
            set
            {
                _attributeCasing = value;
            }
        }

        /// <summary>
        /// Determines how entities are treaten.
        /// </summary>
        /// <remarks>
        /// The entities are built as &amp;name; variant by default, if any known
        /// entity exists. Otherwise any enhanced ASCII or unicode character will be
        /// replaced by the numeric expression &amp;#NNN;.
        /// </remarks>
        [Browsable(true), Description("Determines how entities are treaten."),
      Category("Entities"), DefaultValue(typeof(EntityFormat), "Named")]
        public EntityFormat Entities
        {
            get
            {
                return _Entities;
            }
            set
            {
                _Entities = value;
            }
        }

        /// <summary>
        /// Set the case of element names. If <code>true</code> all element names are upper case.
        /// </summary>
        [Browsable(true), Description("Set the case of element names. If <code>true</code> all element names are upper case."),
           Category("Formatting"), DefaultValue(false)]
        public HtmlFormatterCase ElementCasing
        {
            get
            {
                return _elementCasing;
            }
            set
            {
                _elementCasing = value;
            }
        }

        /// <summary>
        /// Determines the char used to build indentation.
        /// </summary>
        [Browsable(true), Description("Determines the char used to build indentation."),
           Category("Formatting"), DefaultValue(' ')]
        public char IndentChar
        {
            get
            {
                return _indentChar;
            }
            set
            {
                _indentChar = value;
            }
        }

        /// <summary>
        /// Determines the number of chars used for each indentation level. Use 1 for tabs and 4 to 8 for whitespace.
        /// </summary>
        [Browsable(true), Description("Determines the number of chars used for each indentation level. Use 1 for tabs and 4 to 8 for whitespace."),
           Category("Formatting"), DefaultValue(4)]
        public int IndentSize
        {
            get
            {
                return _indentSize;
            }
            set
            {
                _indentSize = value;
            }
        }

        /// <summary>
        /// If <code>true</code> the output conforms to XHTML 1.0, otherwise to HTML 4.0
        /// If input is XHTML compliant the output will never changed back to HTML 4.0 regardless this option.
        /// </summary>
        [Browsable(true), Description("Make XHTML 1.0 or HTML 4.0 compatible output."),
           Category("Formatting"), DefaultValue(true)]
        public bool MakeXhtml
        {
            get
            {
                return _makeXhtml;
            }
            set
            {
                _makeXhtml = value;
            }
        }

        /// <summary>
        /// Forces a line break after leaving the given number of columns per line.
        /// </summary>
        [Browsable(true), Description("Forces a line break after leaving the given number of columns per line."),
           Category("Formatting"), DefaultValue(512)]
        public int MaxLineLength
        {
            get
            {
                return _maxLineLength;
            }
            set
            {
                _maxLineLength = value;
            }
        }

        /// <overloads/>
        /// <summary>
        /// Standard constructor to build default formatter options.
        /// </summary>
        /// <remarks>
        /// This constructor sets the folowing default options:
        /// <list type="bullet">
        ///     <item><term>IndentChar = ' ' (space)</term></item>
        ///     
        /// </list>
        /// </remarks>
        public HtmlFormatterOptions() : this(' ', 4, 512, true)
        {
        }

        /// <summary>
        /// Standard constructor to build formatter options with the minimum number of parameters needed.
        /// </summary>
        /// <param name="indentChar">Char used to indent</param>
        /// <param name="indentSize">Number of chars the indent char is repeated each level</param>
        /// <param name="maxLineLength">Dermines the forced line break</param>
        /// <param name="makeXhtml"><code>true</code> if XHTML should produced</param>
        public HtmlFormatterOptions(char indentChar, int indentSize, int maxLineLength, bool makeXhtml) : this(indentChar, indentSize, maxLineLength, HtmlFormatterCase.LowerCase, HtmlFormatterCase.LowerCase, makeXhtml)
        {
        }

        /// <summary>
        /// Advanced constructor to build formatter options with the minimum number of parameters needed.
        /// </summary>
        /// <param name="indentChar">Char used to indent</param>
        /// <param name="indentSize">Number of chars the indent char is repeated each level</param>
        /// <param name="maxLineLength">Dermines the forced line break</param>
        /// <param name="elementCasing"><code>true</code> makes all attributes upper case</param>
        /// <param name="attributeCasing"><code>true</code> makes all elements upper case</param>
        /// <param name="makeXhtml"><code>true</code> if XHTML should produced</param>
        public HtmlFormatterOptions(char indentChar, int indentSize, int maxLineLength, HtmlFormatterCase elementCasing, HtmlFormatterCase attributeCasing, bool makeXhtml)
        {
            _indentChar = indentChar;
            _indentSize = indentSize;
            _maxLineLength = maxLineLength;
            _elementCasing = elementCasing;
            _attributeCasing = attributeCasing;
            _makeXhtml = makeXhtml;
            _Entities = EntityFormat.Named;
            _preserveWhitespace = false;
        }

        /// <summary>
        /// Advanced constructor to build formatter options with the minimum number of parameters needed.
        /// </summary>
        /// <param name="indentChar">Char used to indent</param>
        /// <param name="indentSize">Number of chars the indent char is repeated each level</param>
        /// <param name="maxLineLength">Dermines the forced line break</param>
        /// <param name="elementCasing"><c>true</c> makes all attributes upper case</param>
        /// <param name="attributeCasing"><c>true</c> makes all elements upper case</param>
        /// <param name="makeXhtml"><c>true</c> if XHTML should produced</param>
        /// <param name="Entities">Determines the formatting of entities.</param>
        public HtmlFormatterOptions(char indentChar, int indentSize, int maxLineLength, HtmlFormatterCase elementCasing, HtmlFormatterCase attributeCasing, bool makeXhtml, EntityFormat Entities)
        {
            _indentChar = indentChar;
            _indentSize = indentSize;
            _maxLineLength = maxLineLength;
            _elementCasing = elementCasing;
            _attributeCasing = attributeCasing;
            _makeXhtml = makeXhtml;
            _Entities = Entities;
            _preserveWhitespace = false;
            _reformatStyles = false;
        }

        /// <summary>
        /// Support a text for the propertygrid.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int changed = 0;
            changed += _indentChar == ' ' ? 0 : 1;
            changed += _indentSize == 4 ? 0 : 1;
            changed += _maxLineLength == 512 ? 0 : 1;
            changed += _elementCasing != HtmlFormatterCase.PreserveCase ? 1 : 0;
            changed += _attributeCasing != HtmlFormatterCase.PreserveCase ? 1 : 0;
            changed += _makeXhtml ? 0 : 1;
            changed += _Entities == EntityFormat.Named ? 0 : 1;
            changed += _preserveWhitespace ? 1 : 0;
            changed += _fakeEmptyParaTag ? 1 : 0;
            changed += _commentCDATAAddsNewline ? 1 : 0;
            changed += _commentCDATA ? 1 : 0;
            changed += _reformatStyles ? 1 : 0;
            return String.Format("{0} propert{1} changed", changed, (changed == 1) ? "y" : "ies");
        }
    }
}
