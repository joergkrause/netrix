using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// Collection of cell elements.
    /// </summary>
    public class TableCellElements : System.Collections.CollectionBase
    {

        /// <summary>
        /// Adds a cell.
        /// </summary>
        /// <param name="o"></param>
        public void Add(TableCellElement o) 
        {
            base.List.Add(o);
        }

        /// <summary>
        /// Determines whether the cel exists in the collection.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Contains(TableCellElement o) 
        {
            return List.Contains(o);
        }

        /// <summary>
        /// Removes a cell.
        /// </summary>
        /// <remarks>
        /// Improper usage could result in wrong table rendering and unpredictable results.
        /// </remarks>
        /// <param name="o"></param>
        public void Remove(TableCellElement o) 
        {
            base.List.Remove(o);
        }

        /// <summary>
        /// Returns an array representing the content of the list.
        /// </summary>
        /// <returns></returns>
        public IElement[] ToArray()
        {
            IElement[] array = new IElement[base.List.Count];
            for (int i = 0; i < base.List.Count; i++)
            {
                array[i] = this[i];
            }
            return array;
        }

        /// <summary>
        /// Access to a cell within the collection.
        /// </summary>
        /// <param name="index">0 based index.</param>
        /// <returns>Cell element of <c>null</c> (<c>Nothing</c> in VB.NET) if index is outside boundaries.</returns>
        public TableCellElement this[int index] 
        {
            get 
            {
                if (index >= 0 && base.List.Count > index)
                {
                    return (TableCellElement)base.List[index];
                }
                else 
                {
                    return null;
                }
            }
            set
            {
                if (index >= 0 && base.List.Count > 0)
                {
                    base.List[index] = value;
                }
            }
        }


    }
}
