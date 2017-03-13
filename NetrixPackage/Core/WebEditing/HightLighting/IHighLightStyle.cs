using System;
using System.Drawing;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.HighLighting
{
	/// <summary>
	/// Defines a style for customized HighLighting.
	/// </summary>
	/// <remarks>
	/// This used as a parameter for the HighLight services, 
	/// which add specific styles to elements or text selections without changing the document or DOM
	/// during design time. The style type depends on the constructor used to instantiate the object.
	/// </remarks>
	public interface IHighLightStyle
	{

		/// <summary>
		/// Defines the property of the style.
		/// </summary>
		/// <remarks>
		/// Properties with higher priority override properties with lower priority. The default value is 0, 
		/// and the higher the number, the higher the priority. The rendering priority has a limit of 9.
		/// <para>
		/// If two properties of the IHTMLRenderStyle interface are applied to the same text selection, 
		/// the property with higher rendering priority overrides the property with lower rendering priority. 
		/// If you allow two properties of the IHTMLRenderStyle interface to have the same rendering priority, 
		/// the control will not be able to determine which property will override the other.
		/// </para>
		/// </remarks>
        uint Priority { get; set; }

        /// <summary>
		/// Sets or retrieves the color of the text in the object.
		/// </summary>
		IHighlightColor TextColor { get; set; }

        /// <summary>
		/// Sets or retrieves the color behind the text in the object.
		/// </summary>
		IHighlightColor TextBackgroundColor { get; set; }

        /// <summary>
		/// Sets or retrieves the appearance of the underline decoration of the object.
		/// </summary>
		/// <remarks>
		/// This property supports the thick-dash value as of Internet Explorer 6.
		/// <seealso cref="GuruComponents.Netrix.WebEditing.HighLighting.UnderlineStyle">UnderlineStyle</seealso>
		/// </remarks>
		GuruComponents.Netrix.WebEditing.HighLighting.UnderlineStyle UnderlineStyle { get; set; }

        /// <summary>
		/// Sets or retrieves the appearance of the line-through decoration of the object.
		/// </summary>
		/// <remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.HighLighting.LineThroughStyle">LineThroughStyle</seealso>
		/// </remarks>
		GuruComponents.Netrix.WebEditing.HighLighting.LineThroughStyle LineThroughStyle { get; set; }

        /// <summary>
		/// Sets or retrieves the color of the line-through, overline, or underline decorations of the object.
		/// </summary>
		IHighlightColor LineColor { get; set; }

        /// <summary>
		/// Sets or retrieves whether the text in the object renders with a line-through, overline, or underline decoration.
		/// </summary>
		/// <remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.HighLighting.LineType">LineType</seealso>
		/// </remarks>
		GuruComponents.Netrix.WebEditing.HighLighting.LineType LineType { get; set; }
	}
}