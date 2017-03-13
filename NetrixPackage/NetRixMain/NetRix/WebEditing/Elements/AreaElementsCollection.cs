using System.Collections;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// Stores a collection of area definitions as subsequent part of the map element.
    /// </summary>
    /// <remarks>
    /// Areas define hotspots in image maps. See <see cref="GuruComponents.Netrix.WebEditing.Elements.AreaElement">AreaElement</see>
    /// for more information about the elements this collection is build from.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.AreaElement">AreaElement</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.MapElement">MapElement</seealso>
    /// </remarks>
    public class AreaElementsCollection : CollectionBase
    {

        /// <summary>
        /// Adds the given element to the collection.
        /// </summary>
        /// <remarks>
        /// To create an area element use the default constructor of the <see cref="GuruComponents.Netrix.WebEditing.Elements.AreaElement">AreaElement</see> class.
        /// This constructor creates the element and adds it to the document, but the element is still not part
        /// of the DOM. To make it usable it must be inserted into the AREA collection of an existing MAP element.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.MapElement">MapElement</seealso>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.AreaElement">AreaElement</seealso>
        /// </remarks>
        /// <param name="o">The element which should be added to the collection.</param>
        public void Add(AreaElement o) 
        {
            base.List.Add(o);
        }

        /// <summary>
        /// Checks if the MAP contains the AREA tag.
        /// </summary>
        /// <remarks>
        /// This method is used from withing the <see cref="GuruComponents.Netrix.WebEditing.Elements.AreaElement">AreaElement</see> class.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.MapElement">MapElement</seealso>
        /// </remarks>
        /// <param name="o">The AREA object which should be checked.</param>
        /// <returns>Returns <c>true</c> if the element exists.</returns>
        public bool Contains(AreaElement o) 
        {
            return List.Contains(o);
        }

        /// <summary>
        /// Removes the AREA tag from the MAP.
        /// </summary>
        /// <remarks>
        /// This method is used from withing the <see cref="GuruComponents.Netrix.WebEditing.Elements.AreaElement">AreaElement</see> class.
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.MapElement">MapElement</seealso>
        /// </remarks>
        /// <param name="o">The AREA object which should be removed.</param>
        public void Remove(AreaElement o) 
        {
            base.List.Remove(o);
        }

        /// <summary>
        /// Gets or sets an AREA element at the specified index in the MAP.
        /// </summary>
        public AreaElement this[int index] 
        {
            get 
            {
                return (AreaElement)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        /// <summary>
        /// Fired if the collection is cleared.
        /// </summary>
        /// <remarks>
        /// THIS EVENT SUPPORTS THE NETRIX UI INFRASTRUCTURE AND SHOULD NOT BE USED FROM USER CODE.
        /// </remarks>
        public event CollectionClearHandler OnClearHandler;
        /// <summary>
        /// Fired if a new element is added to the collection.
        /// </summary>
        /// <remarks>
        /// THIS EVENT SUPPORTS THE NETRIX UI INFRASTRUCTURE AND SHOULD NOT BE USED FROM USER CODE.
        /// </remarks>
        public event CollectionInsertHandler OnInsertHandler;

        protected override void OnClear()
        {
            base.OnClear ();
            if (OnClearHandler != null)
            {
                OnClearHandler();
            }
        }

        protected override void OnInsert(int index, object value)
        {
            base.OnInsert (index, value);
            if (OnInsertHandler != null)
            {
                OnInsertHandler(index, value);
            }
        }
    }
}