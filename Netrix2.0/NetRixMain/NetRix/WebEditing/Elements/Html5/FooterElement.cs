using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using System;
using System.Web.UI.WebControls;
using System.ComponentModel;

using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements.Html5
{
    
    /// <summary>
    /// The footer element defines a footer area.
    /// </summary>
    /// <remarks>
    /// A footer is a semantic HTML 5 element.
    /// </remarks>
	public sealed class FooterElement : StyledElement
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
        public FooterElement(IHtmlEditor editor)
            : base("footer", editor)
        {
        }

		internal FooterElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
		{
		}
	}
}