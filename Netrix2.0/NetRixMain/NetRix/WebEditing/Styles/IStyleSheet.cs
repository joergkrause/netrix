using System;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Styles
{

    /// <summary>
    /// This interface provides access to an object that represents 
    /// a single style sheet in the document.
    /// </summary>
    /// <remarks>
    /// StyleSheet class implements IStyleSheet interface.
    /// <para>
    /// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
    /// class for more details.	
    /// </para>
    /// </remarks>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
    public interface IStyleSheet
    {
		/// <summary>
		/// Adds a style sheet to the imports collection for the specified style sheet.
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		/// <param name="bstrURL">Specifies the location of the source file for the style sheet.</param>
		/// <param name="lIndex">Specifies the requested position for the style sheet in the collection. 
		/// If this value is not given, the style sheet is added to the end of the collection.</param>
		/// <returns>Returns a zero-based index value indicating the position of the imported 
		/// style sheet in the imports collection.</returns>
		int AddImport(string bstrURL, int lIndex);
        
		/// <summary>
		/// Creates a new rule for a style sheet.
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		/// <param name="bstrSelector">Specifies the Rule Name</param>
		/// <param name="bstrStyle">Specifies the Rule style</param>
		/// <param name="lIndex">Specifies Rule Index</param>
		/// <returns>Returns a index value of the rule in the collection</returns>
		int AddRule(string bstrSelector, string bstrStyle, int lIndex);
        
		/// <summary>
		/// Sets or retrieves the persisted representation of the style rule.
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		string CssText { get; set; }
        
		/// <summary>
		/// Sets or retrieves whether a style sheet is applied to the object.
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		bool Disabled { get; set; }
        
		/// <summary>
		/// Retrieves the string identifying the object.
		/// </summary>
		/// <remarks>
		/// This is an SGML identifier used as the target for hypertext links or 
		/// for naming particular objects associated with style sheets.
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </para>
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		string Id { get; }
        
		/// <summary>
		/// Retrieves a zero-based collection of all the imported style sheets 
		/// defined for the given StyleSheet object. 
		/// </summary>
		/// <remarks>
		/// An imported style sheet is one that is brought into the document 
		/// using the Cascading Style Sheets (CSS) at import rule.
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </para>
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		/// <returns></returns>
		GuruComponents.Netrix.ComInterop.Interop.IHTMLStyleSheetsCollection GetImports();
        
		/// <summary>
		/// Retrieves the next object in the HTML hierarchy. 
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		/// <returns>Returns usually the STYLE or LINK element containing the stylesheet.</returns>
		GuruComponents.Netrix.WebEditing.Elements.IElement GetOwningElement();
        
		/// <summary>
		/// Retrieves the style sheet that imported the current style sheets.
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		/// <returns>Returns the parent style sheet or <c>null</c> if there is no parent.</returns>
		IStyleSheet GetParentStyleSheet();
        
		/// <summary>
		/// Retrieves the Cascading Style Sheets (CSS) language in which the style sheet is written.
		/// </summary>
		/// <remarks>
		/// This property can be any string, including an empty string. Style sheets 
		/// having any type other than "text/css" are not supported.
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </para>
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		string StyleSheetType { get; }
        
		/// <summary>
		/// Sets or retrieves the URL of the linked style sheet.
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		string Href { get; set; }
        
		/// <summary>
		/// Sets or retrieves the media type. 
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		string Media { get; set; }
        
		/// <summary>
		/// Retrieves whether the rule or style sheet is defined on the page or is imported.
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		bool ReadOnly { get; }
        
		/// <summary>
		/// Removes the imported style sheet by ordinal position in the 
		/// imports collection based on the value of the index passed in.
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		/// <param name="lIndex">Specifies the index value of the imported style sheet to be removed.</param>
		void RemoveImport(int lIndex);
        
		/// <summary>
		/// Deletes an existing style rule for the styleSheet object, 
		/// and adjusts the index of the rules collection accordingly.
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		/// <param name="lIndex">Specifies the index value of the rule to be deleted 
		/// from the style sheet. If an index is not provided, the first rule in the 
		/// rules collection is removed.</param>
		void RemoveRule(int lIndex);
        
		/// <summary>
		/// Retrieves a collection of rules defined in the styleSheet. 
		/// </summary>
		/// <remarks>
		/// If there are no rules, the length of the collection returned is zero.
		/// <para>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </para>
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		StyleRuleCollection Rules { get; }
        
		/// <summary>
		/// Sets or retrieves the title of the style sheet.
		/// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet">StyleSheet</see> 
		/// class for more details.	
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.StyleSheet"/>
		string Title { get; set; }
    }
}
