using System;
namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Stores information about one specific register directive.
    /// </summary>
    public interface IRegisterDirective
    {
        /// <summary>
        /// TODO: Add comment
        /// </summary>
        string AssemblyName { get; }
        /// <summary>
        /// TODO: Add comment
        /// </summary>
        string DirectiveName { get; }
        /// <summary>
        /// TODO: Add comment
        /// </summary>
        bool IsUserControl { get; }
        /// <summary>
        /// TODO: Add comment
        /// </summary>
        string NamespaceName { get; }
        /// <summary>
        /// TODO: Add comment
        /// </summary>
        string SourceFile { get; }
        /// <summary>
        /// TODO: Add comment
        /// </summary>
        string TagName { get; }
        /// <summary>
        /// TODO: Add comment
        /// </summary>
        string TagPrefix { get; }
        /// <summary>
        /// TODO: Add comment
        /// </summary>
        Type ObjectType { get; }
        //IRegisterDirectiveCollection Directives { get; }
    }
}
