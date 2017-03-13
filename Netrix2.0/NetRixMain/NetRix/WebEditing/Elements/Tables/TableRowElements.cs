using System;
using System.Drawing;

using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// Collection of Rows a table contains. 
    /// </summary>
    public class TableRowElements : System.Collections.CollectionBase
    {

        /// <summary>
        /// Adds a row to the underlying table.
        /// </summary>
        /// <param name="o"></param>
        public void Add(TableRowElement o) 
        {
            base.List.Add(o);
        }

        /// <summary>
        /// Determines whether the underlying table contains the given row.  
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Contains(TableRowElement o) 
        {
            return List.Contains(o);
        }

        /// <summary>
        /// Removes a specific row from table.
        /// </summary>
        /// <param name="o"></param>
        public void Remove(TableRowElement o) 
        {
            base.List.Remove(o);
        }

        /// <summary>
        /// Gets or sets a row by its index using an indexer.
        /// </summary>
        public TableRowElement this[int index] 
        {
            get 
            {
                return (TableRowElement)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        /// <summary>
        /// Fired if the collection editor inserts a new element.</summary>
        /// <remarks>This member supports the NetRix infrastructure and supports the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </remarks>
        public event CollectionInsertHandler OnInsertHandler;
        /// <summary>
        /// Fired if the colection editor starts a new sequence. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        public event CollectionClearHandler OnClearHandler;

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsert (index, value);
            // prevent from fail during first collection fill process, because the handler is temporary removed
            if (OnInsertHandler != null)
            {
                OnInsertHandler(index, value);
            }
        }

        protected override void OnClear()
        {
            base.OnClear ();
            // prevent from fail during first collection fill process, because the handler is temporary removed
            if (OnClearHandler != null)
            {
                OnClearHandler();
            }
        }


    }
}
