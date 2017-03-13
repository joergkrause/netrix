using System;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix
{

    /// <summary>
    /// The internal representation of block formatting options.
    /// </summary>
    /// <remarks>
    /// The order
    /// is strongly determined from the IDM_GETBLOCKFMTS command and make
    /// the usage language independent.
    /// </remarks>
    public enum HtmlFormat 
    {
        /// <summary>
        /// Remove the block formatting.
        /// </summary>
        Normal          = 0,
        /// <summary>
        /// Assign the &lt;pre&gt; tag formatting to the whole block.
        /// </summary>
        Formatted       = 1,
        /// <summary>
        /// Assign the &lt;address&gt; tag formatting to the whole block.
        /// </summary>
        Address         = 2,
        /// <summary>
        /// Assign the &lt;h1&gt; tag formatting to the whole block.
        /// </summary>
        Heading1        = 3,
        /// <summary>
        /// Assign the &lt;h2&gt; tag formatting to the whole block.
        /// </summary>
        Heading2        = 4,
        /// <summary>
        /// Assign the &lt;h3&gt; tag formatting to the whole block.
        /// </summary>
        Heading3        = 5,
        /// <summary>
        /// Assign the &lt;h4&gt; tag formatting to the whole block.
        /// </summary>
        Heading4        = 6,
        /// <summary>
        /// Assign the &lt;h5&gt; tag formatting to the whole block.
        /// </summary>
        Heading5        = 7,
        /// <summary>
        /// Assign the &lt;h6&gt; tag formatting to the whole block.
        /// </summary>
        Heading6        = 8,
        /// <summary>
        /// Assign the &lt;ol&gt; tag formatting to the whole block.
        /// </summary>
        OrderedList     = 9,
        /// <summary>
        /// Assign the &lt;ul&gt; tag formatting to the whole block.
        /// </summary>
        UnorderedList   = 10,
        /// <summary>
        /// Assign the &lt;index&gt; tag formatting to the whole block.
        /// </summary>
        IndexList       = 11,
        /// <summary>
        /// Assign the &lt;menu&gt; tag formatting to the whole block.
        /// </summary>
        MenuList        = 12,
        /// <summary>
        /// Assign the &lt;dt&gt; tag formatting to the whole block.
        /// </summary>
        DefinitionTerm  = 13,
        /// <summary>
        /// Assign the &lt;dd&gt; tag formatting to the whole block.
        /// </summary>
        Definition      = 14,
        /// <summary>
        /// Assign the &lt;p&gt; or &lt;div&gt; tag formatting to the whole block.
        /// </summary>
        Paragraph       = 15
    }
}