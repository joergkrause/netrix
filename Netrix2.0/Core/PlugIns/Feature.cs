using System;

namespace GuruComponents.Netrix.PlugIns
{
    /// <summary>
    /// This list provides several features PlugIns could implement.
    /// </summary>
    [Flags]
    public enum Feature
    {
        /// <summary>
        /// This plugin can register its own elements.
        /// </summary>
        RegisterElements,
        /// <summary>
        /// This plugin can create new elements within its own namespace.
        /// </summary>
        CreateElements,
        /// <summary>
        /// This plugin has at least one own namespace.
        /// </summary>
        OwnNamespace,
        /// <summary>
        /// This plugin supports multiple namespaces.
        /// </summary>
        MultipleNamespaces,
        /// <summary>
        /// This plugin supports edit designer.
        /// </summary>
        EditDesigner,
        /// <summary>
        /// This plugin makes usage of the designer host.
        /// </summary>
        DesignerHostSupport,
        /// <summary>
        /// Plugin provides tools (toolstrip buttons, menu items) to extend the integrated UI.
        /// </summary>
        ProvideTools,
        /// <summary>
        /// This plugin does not support any of the features above.
        /// </summary>
        None

    }
}
