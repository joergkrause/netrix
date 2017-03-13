using System;

using GuruComponents.Netrix.HtmlFormatting.Elements;
using GuruComponents.Netrix.ComInterop;
using System.Web.UI;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// Factory to create native element objects based on legacy ones.
    /// </summary>
    public interface IElementFactory
    {

        /// <summary>
        /// Makes a association between an element and a native element class.
        /// </summary><remarks>
        /// In case of an already created object 
        /// this object is returned instead of creating a new one.
        /// </remarks>
        /// <param name="el">The element for that a native object is needed.</param>
        /// <returns>IElement if creating or retrieving was successful, null else. The caller is responsible to cast to the right class type.</returns>
        Control CreateElement(Interop.IHTMLElement el);

        /// <summary>
        /// Reset the cache and attached designer collections.
        /// </summary>
        /// <remarks>
        /// The LoadXX methods and InnerHtml call this method implicitly, to under normal circumestances it isn't 
        /// required to call this from user code.
        /// </remarks>
        void ResetElementCache();

        /// <summary>
        /// Removes an element from the list of registered types.
        /// </summary>
        /// <remarks>
        /// The element is recognized by alias and name, divided by colon (asp:button, html:img). The basic
        /// HTML elements have the alias html, even if the document loaded is not XHTML conform. For
        /// a HTML comment the name is "!" without an alias. Plug-Ins may define different name schemas.
        /// </remarks>
        /// <seealso cref="CreateElement"/>
        /// <param name="elementName">The name of the element type, including the alias (see remarks).</param>
        void UnRegisterElement(string elementName);

        /// <summary>
        /// Returns the type of an element.
        /// </summary>
        /// <param name="elementName">Name, must be written with alias. Use "html" for standard elements: "html:p".</param>
        /// <returns>Type of element, if registered, or <c>null</c> otherwise.</returns>
        Type GetElementType(string elementName);
    }

}
