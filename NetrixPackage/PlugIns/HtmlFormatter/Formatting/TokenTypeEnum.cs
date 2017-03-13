namespace GuruComponents.Netrix.HtmlFormatting
{
    /// <summary>
    /// The Type of the currently processed or stored token.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Whitspace
        /// </summary>
        Whitespace = 0,
        /// <summary>
        /// Name of a tag
        /// </summary>
        TagName = 1,
        /// <summary>
        /// Name of an attribute
        /// </summary>
        AttrName = 2,
        /// <summary>
        /// Value of an attribute
        /// </summary>
        AttrVal = 3,
        /// <summary>
        /// Text (any other part not specific tokenized)
        /// </summary>
        TextToken = 4,
        /// <summary>
        /// A tag which is self terminated, like &lt;br/&gt;
        /// </summary>
        SelfTerminating = 5,
        /// <summary>
        /// Empty token
        /// </summary>
        Empty = 6,
        /// <summary>
        /// Comment token
        /// </summary>
        Comment = 7,
        /// <summary>
        /// During retrieving of this token an error occured
        /// </summary>
        Error = 8,
        /// <summary>
        /// Open bracket (&lt;)
        /// </summary>
        OpenBracket = 10,
        /// <summary>
        /// Close bracket (&gt;)
        /// </summary>
        CloseBracket = 11,
        /// <summary>
        /// A forward slash appears, controls closing and self terminated tags
        /// </summary>
        ForwardSlash = 12,
        /// <summary>
        /// Double quote (")
        /// </summary>
        DoubleQuote = 13,
        /// <summary>
        /// Single quote (')
        /// </summary>
        SingleQuote = 14,
        /// <summary>
        /// The equals sign (=)
        /// </summary>
        EqualsChar = 15,
        /// <summary>
        /// A script block which does not have a runat="server" attribute
        /// </summary>
        ClientScriptBlock = 20,
        /// <summary>
        /// A style block
        /// </summary>
        Style = 21,
        /// <summary>
        /// A server side script block 
        /// </summary>
        InlineServerScript = 22,
        /// <summary>
        /// ASP / ASP.NET / PHP and other script blocks
        /// </summary>
        ServerScriptBlock = 23,
        /// <summary>
        /// A XML directive
        /// </summary>
        XmlDirective = 24,
        /// <summary>
        /// A PHP script tag
        /// </summary>
        PhpScriptTag = 25
    }
}
