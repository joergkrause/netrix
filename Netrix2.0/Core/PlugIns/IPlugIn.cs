using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.PlugIns
{
	/// <summary>
	/// Base interface to identify plug-ins.
	/// </summary>
	public interface IPlugIn : IDisposable
	{

        /// <summary>
        /// The base type.
        /// </summary>
        Type Type { get; }
        
        /// <summary>
        /// Common name.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Is implemented as extender provider.
        /// </summary>
        bool IsExtenderProvider { get; }
        
        /// <summary>
        /// This plugin supports these features.
        /// </summary>
        Feature Features { get; }

        /// <summary>
        /// The default alias and namespaces, which the element creator uses to identify the plugin.
        /// </summary>
        /// <remarks>
        /// If the plug-in returns "v" as an element alias and the element creator tries to create an
        /// element with such an alias, the private element creator method of this particular plug-in gets called.
        /// </remarks>
        IDictionary GetSupportedNamespaces(IHtmlEditor key);

        /// <summary>
        /// This method gets called if the control reaches readystatecomplet before the user accesible event gets called.
        /// </summary>
        /// <remarks>
        /// Use with caution. Used internally to register commands.
        /// </remarks>
        void NotifyReadyStateCompleted(IHtmlEditor editor);

        /// <summary>
        /// List of element types, which the extender plug-in extends.
        /// </summary>
        /// <remarks>
        /// Each element can support design time environments, like the PropertyGrid. The PropertyGrid can show,
        /// for instance, commands and additional property pages, specific to the current object. The basic
        /// features provided by the base component are managed by a simple designer. Each plugin which deals
        /// explicitly with elements, like TableDesigner or XmlDesigner, can extend the elements design time behavior
        /// by providing additional verbs (they appear as commands in the PropertyGrid), property pages (which
        /// appears via the property button in the PropertyGrid) or manipulated list of properties. 
        /// <para>
        /// Internally the plugin changes the <see cref="System.ComponentModel.ICustomTypeDescriptor">ICustomTypeDescriptor</see>
        /// of the underyling element. Environments or hosts other than PropertyGrid may use reflection to get the
        /// right information.
        /// </para>
        /// </remarks>
        List<CommandExtender> GetElementExtenders(IElement component);

        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <param name="tagName">Private element within the registered namespace.</param>
        /// <param name="editor">Reference to editor which delivers the current document.</param>
        /// <returns></returns>
        Control CreateElement(string tagName, IHtmlEditor editor);

	}

}
