using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using System;
using System.Web.UI.WebControls;
using System.ComponentModel;

using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// The HX element defines a heading.
    /// </summary>
    /// <remarks>
    /// Any heading (H1 through H6) uses the same class to instantiate elements.
    /// </remarks>
	public sealed class HeaderElement : StyledElement
	{
		/// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="HorizontalAlign"]/*'/>
		[DescriptionAttribute("")]
		[DefaultValueAttribute(System.Web.UI.WebControls.HorizontalAlign.Left)]
		[CategoryAttribute("Element Layout")]
        [TypeConverter(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterDropList))]
        [TE.DisplayName()]
		public System.Web.UI.WebControls.HorizontalAlign align
		{
			set
			{
				this.SetEnumAttribute ("align", (HorizontalAlign) value, (HorizontalAlign) 0);
				return;
			} 
      
			get
			{
				return (HorizontalAlign) this.GetEnumAttribute ("align", (HorizontalAlign) 0);
			} 
      
		}

        /// <summary>
        /// Creates the specified header element.
        /// </summary>
        /// <param name="editor">The editor this element belongs to.</param>
        /// <param name="h">The type of header being created.</param>
        public HeaderElement(IHtmlEditor editor, Header h)
            : base(h.ToString(), editor)
        {
        }

		internal HeaderElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
		}
	}
}