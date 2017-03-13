using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// &lt;HEAD ...&gt; is the head element for any HTML page. 
    /// </summary>
    /// <remarks>
    /// Within HTML, the document has two sections to it: HEAD and BODY. HEAD is like the cover page of the document. 
    /// Just as the cover page of a book contains information about the book (such as the title), the HEAD section contains 
    /// information about the document. This information is communicated through the TITLE tag (which is required) and the 
    /// LINK and META tags.
    /// </remarks>
    public sealed class HeadElement : Element
    {

        internal HeadElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 

    }
}
