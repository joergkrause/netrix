using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// &lt;TITLE ...&gt; gives the page a unique name. 
    /// </summary>
    /// <remarks>
    /// Use <see cref="Element.InnerText"/> to check the inner part of title.
    /// </remarks>
    public sealed class TitleElement : Element
    {

        internal TitleElement(Interop.IHTMLElement peer, IHtmlEditor editor)
            : base(peer, editor)
        {
        } 

    }
}
