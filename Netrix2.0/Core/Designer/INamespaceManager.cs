using System;
namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Handle namespaces to allow rendering of XML elements.
    /// </summary>
    public interface INamespaceManager
    {
        /// <summary>
        /// Remove registered namespaces.
        /// </summary>
        void Clear();
        /// <summary>
        /// Return registered behavior.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        GuruComponents.Netrix.WebEditing.Behaviors.IBaseBehavior GetBehaviorOfElement(GuruComponents.Netrix.ComInterop.Interop.IHTMLElement element);
        /// <summary>
        /// Register a namespace.
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="namespaceName"></param>
        /// <param name="behavior"></param>
        void RegisterNamespace(string alias, string namespaceName, Type behavior);
        /// <summary>
        /// Register a namespace.
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="namespaceName"></param>
        void RegisterNamespace(string alias, string namespaceName);
    }
}
