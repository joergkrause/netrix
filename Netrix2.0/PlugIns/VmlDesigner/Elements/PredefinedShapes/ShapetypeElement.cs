using System;  
using System.ComponentModel;  

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This is the element used to describe a shape so that it may be referenced at a later point in the document by a shape element.
	/// </summary>
	/// <remarks>
    /// This is the element used to describe a shape so that it may be referenced at a later point in the document by a 
    /// shape element.  It is identical to the shape element except that it cannot reference another shapetype element 
    /// and that the visibility property is always hidden.  
    /// Hint: Authoring agents may choose to make shapetype elements visible to allow them to be edited - in this case 
    /// the CSS positioning properties become relevant. In NetRix the PropertyGrid will show all present shapetype elements 
    /// in the current document as an alternative choice for the shapetype attribute of any shape element whithin the
    /// same document.
    /// <para>
    /// When a shape element makes reference to a shapetype, the shape may duplicate some of the attributes that have 
    /// already been specified in the shapetype.   In these cases, the attributes in the shape override those of the 
    /// shapetype.
    /// </para>
	/// </remarks>    
    [ToolboxItem(false)]
    public sealed class ShapetypeElement : CommonShapeElement
    {

		public ShapetypeElement(IHtmlEditor editor) : base("v:shapetype", editor)
		{
		}

        internal ShapetypeElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
