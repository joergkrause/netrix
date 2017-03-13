namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Stores information about one page directive.
    /// </summary>
    public interface IDirective
    {
        /// <summary>
        /// Add an attribute
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        void AddAttribute(string attributeName, string attributeValue);
        /// <summary>
        /// Get the directive name.
        /// </summary>
        string DirectiveName { get; }
        /// <summary>
        /// Design time support
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}
