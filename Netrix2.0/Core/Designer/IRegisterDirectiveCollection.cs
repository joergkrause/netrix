using System;
namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Supports the Register directives of ASP.NET pages. Used by AspDotNetDesigner and similar plug-ins.
    /// </summary>
    public interface IRegisterDirectiveCollection
    {
        /// <summary>
        /// Add a directive
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="raiseEvent"></param>
        void AddRegisterDirective(IRegisterDirective directive, bool raiseEvent);
        /// <summary>
        /// Add a directive
        /// </summary>
        /// <param name="directive"></param>
        void AddRegisterDirective(IRegisterDirective directive);
        /// <summary>
        /// Fired if a directive is being added.
        /// </summary>
        event DirectiveEventHandler DirectiveAdded;
        /// <summary>
        /// Get a type from registered tag information.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="tagPrefix"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        Type GetObjectType(IServiceProvider serviceProvider, string tagPrefix, string typeName);
        /// <summary>
        /// Return the original directive.
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        IRegisterDirective GetRegisterDirective(string tagPrefix, string tagName);
        /// <summary>
        /// Get the tag name from type.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        string GetTagName(Type objectType);
        /// <summary>
        /// Get directive from prefix
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <returns></returns>
        IRegisterDirective this[string tagPrefix] { get; }
        /// <summary>
        /// Get directive from prefix and name.
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        IRegisterDirective this[string tagPrefix, string tagName] { get; }
    }
}
