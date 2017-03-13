// it is necessary to save this file as unicode to preserve the embedded entities
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using GuruComponents.Netrix.HtmlFormatting.Elements;

namespace GuruComponents.Netrix.HtmlFormatting
{
    /// <summary>
    /// The HtmlFormatter formats a HTML stream into XHTML format and indent the element in a functional
    /// related way, e.g. it recognizes tables and can distinguish between inline and block tags.
    /// </summary>
    public interface IHtmlFormatter
    {

        /// <summary>
        /// This method adds a new TagInfo object to the collection of TagInfos. These collection is used to format
        /// any registered tag correctly. All tag known in HTML 4.0 are predefined. The Plug-In module uses this method to 
        /// enhance the know tags.
        /// </summary>
        /// <param name="ti"></param>
        void AddCustomElement(ITagInfo ti);

        /// <summary>
        /// Starts the beautifier and returns the resulting code in the TextWriter.
        /// </summary>
        /// <param name="input">The string to be beautified.</param>
        /// <param name="output">A TextWriter derived pointer to the result.</param>
        /// <param name="options">The formatting options used to handle the content.</param>
        void Format(string input, TextWriter output, IHtmlFormatterOptions options);

    }
}
