using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// &lt;HTML ...&gt; is the root element for any HTML page. 
    /// </summary>
    public sealed class HtmlElement : Element
    {

        internal HtmlElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 

    }
}
