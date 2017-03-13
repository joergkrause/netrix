namespace GuruComponents.Netrix.VmlDesigner.Elements
{
    /// <summary>
    /// This is the handler definition for the collection insertion event. 
    /// </summary>
    public delegate void CollectionInsertHandler(int index, object @value);
    /// <summary>
    /// This is the handler definition for the collection remove event. 
    /// </summary>
    public delegate void CollectionRemoveHandler(int index, object @value);
    /// <summary>
    /// This is the handler definition for the collection clear event. 
    /// </summary>
    public delegate void CollectionClearHandler();
}
