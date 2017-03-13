using System;
using System.Drawing;
using System.Web;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Collections;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
	/// This class provides access to an object that represents 
	/// a single style sheet in the document.
	/// StyleSheet class implements IStyleSheet interface.
	/// </summary>
    /// <remarks>
    /// The style sheet object is used to retrieve information about a style sheet. For example, you can use it 
    /// to retrieve the URL of a style sheet's source file, and to retrieve the element in the document that defines 
    /// the style sheet. You can then use the style sheet object to modify the document's style sheet.
    /// </remarks>
	public class StyleSheet : IStyleSheet
	{

        private Interop.IHTMLStyleSheet _native;
        private IHtmlEditor _editor;

        internal StyleSheet(Interop.IHTMLStyleSheet native, IHtmlEditor editor)
        {
            _native = native;
            _editor = editor;
        }

        #region IHTMLStyleSheet Members

        /// <summary>
        /// Sets or retrieves the title of the style sheet.
        /// </summary>
        public string Title
        {
            set { _native.SetTitle(value); }
            get { return _native.GetTitle(); }
        }

        /// <summary>
        /// Retrieves the style sheet that imported the current style sheets.
        /// </summary>
        /// <returns>Returns the parent style sheet or <c>null</c> if there is no parent.</returns>
        public IStyleSheet GetParentStyleSheet()
        {
            Interop.IHTMLStyleSheet parent = _native.GetParentStyleSheet();
            if (parent != null)
            {
                return new StyleSheet(_native.GetParentStyleSheet(), _editor);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves the next object in the HTML hierarchy. 
        /// </summary>
        /// <returns>Returns usually the STYLE or LINK element containing the stylesheet.</returns>
        public IElement GetOwningElement()
        {
            return this._editor.GenericElementFactory.CreateElement(_native.GetOwningElement()) as IElement;
        }

        /// <summary>
        /// Sets or retrieves whether a style sheet is applied to the object.
        /// </summary>
        public bool Disabled
        {
            set { _native.SetDisabled(value); }
            get { return _native.GetDisabled(); }
        }

        /// <summary>
        /// Retrieves whether the rule or style sheet is defined on the page or is imported.
        /// </summary>
        public bool ReadOnly
        {
            get { return _native.GetReadOnly(); }
        }

        /// <summary>
        /// Retrieves a zero-based collection of all the imported style sheets 
        /// defined for the given StyleSheet object. 
        /// </summary>
        /// <remarks>
        /// An imported style sheet is one that is brought into the document 
        /// using the Cascading Style Sheets (CSS) at import rule.
        /// </remarks>
        /// <returns></returns>
        public Interop.IHTMLStyleSheetsCollection GetImports()
        {
            return null;
        }

        /// <summary>
        /// Sets or retrieves the URL of the linked style sheet.
        /// </summary>
        public string Href
        {
            set { _native.SetHref(value); }
            get { return _native.GetHref(); }
        }

        /// <summary>
        /// Retrieves the Cascading Style Sheets (CSS) language in which the style sheet is written.
        /// </summary>
        /// <remarks>
        /// This property can be any string, including an empty string. Style sheets 
        /// having any type other than "text/css" are not supported.
        /// </remarks>
        public string StyleSheetType
        {
            get { return _native.GetStyleSheetType(); }
        }

        /// <summary>
        /// Retrieves the string identifying the object.
        /// </summary>
        /// <remarks>
        /// This is an SGML identifier used as the target for hypertext links or 
        /// for naming particular objects associated with style sheets.
        /// </remarks>
        public string Id
        {
            get { return _native.GetId(); }
        }

        /// <summary>
        /// Adds a style sheet to the imports collection for the specified style sheet.
        /// </summary>
        /// <param name="bstrURL">Specifies the location of the source file for the style sheet.</param>
        /// <param name="lIndex">Specifies the requested position for the style sheet in the collection. 
        /// If this value is not given, the style sheet is added to the end of the collection.</param>
        /// <returns>Returns a zero-based index value indicating the position of the imported 
        /// style sheet in the imports collection.</returns>
        public int AddImport(string bstrURL, int lIndex)
        {
            return _native.AddImport(bstrURL, lIndex);
        }

        /// <summary>
        /// Creates a new rule for a style sheet.
        /// </summary>
        /// <param name="bstrSelector">Specifies the Rule Name</param>
        /// <param name="bstrStyle">Specifies the Rule style</param>
        /// <param name="lIndex">Specifies Rule Index</param>
        /// <returns>Returns a index value of the rule in the collection</returns>
        public int AddRule(string bstrSelector, string bstrStyle, int lIndex)
        {
            return _native.AddRule(bstrSelector, bstrStyle, lIndex);
        }

        /// <summary>
        /// Removes the imported style sheet by ordinal position in the 
        /// imports collection based on the value of the index passed in.
        /// </summary>
        /// <param name="lIndex">Specifies the index value of the imported style sheet to be removed.</param>
        public void RemoveImport(int lIndex)
        {
            _native.RemoveImport(lIndex);
        }

        /// <summary>
        /// Deletes an existing style rule for the styleSheet object, 
        /// and adjusts the index of the rules collection accordingly.
        /// </summary>
        /// <param name="lIndex">Specifies the index value of the rule to be deleted 
        /// from the style sheet. If an index is not provided, the first rule in the 
        /// rules collection is removed.</param>
        public void RemoveRule(int lIndex)
        {
            _native.RemoveRule(lIndex);
        }

        /// <summary>
        /// Sets or retrieves the media type. 
        /// </summary>
        public string Media
        {
            set { _native.SetMedia(value); }
            get { return _native.GetMedia(); }
        }

        /// <summary>
        /// Sets or retrieves the persisted representation of the style rule.
        /// </summary>
        public string CssText
        {
            set { _native.SetCssText(value); }
            get { return _native.GetCssText(); }
        }

        /// <summary>
        /// Retrieves a collection of rules defined in the styleSheet. 
        /// </summary>
        /// <remarks>If there are no rules, the length of the collection returned is zero.</remarks>
        public StyleRuleCollection Rules
        {
            get
            {
                Interop.IHTMLStyleSheetRulesCollection sr = _native.GetRules();
                StyleRuleCollection src = new StyleRuleCollection(Helper.AddRuleFromCollection(null, sr));
                return src;
                //return rules;
            }
        }

        #endregion
    }
}
