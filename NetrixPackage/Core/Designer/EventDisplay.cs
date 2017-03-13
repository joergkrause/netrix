namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Controls the behavior of event tab in PropertyGrid.
    /// </summary>
    public enum EventDisplay
    {
        /// <summary>
        /// Show only events marked with EventVisible attribute.
        /// </summary>
        EventVisible,
        /// <summary>
        /// Show only Scripting events
        /// </summary>
        Scripting,
        /// <summary>
        /// Show both, using different categories.
        /// </summary>
        Both,
        /// <summary>
        /// Show all, wether or not they have an attribute. This includes <see cref="System.Web.UI.Control"/> events, too.
        /// </summary>
        All
    }
}
