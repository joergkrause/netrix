using System;
using System.ComponentModel;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Documents
{

    /// <summary>
    /// The purpose of this class is to provide a collection editor for linked style sheets.
    /// </summary>
    /// <remarks>
    /// This is necessary because any document can contain as many linked stylesheets as needed.   
    /// </remarks>
	public class LinkElementCollection : System.Collections.CollectionBase, ICollectionBase
    {
        /// <summary>
        /// Adds a new element to the collection. Always set values for CSS (type="text/css" and rel="stylesheet").
        /// </summary>
        /// <remarks>
        /// This member supports the NetRix infrastructure and supports the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </remarks>
        /// <param name="linkElement"></param>
        public void Add(LinkElement linkElement) 
        {
            if (linkElement == null) return;
            if (linkElement.type == null || linkElement.type.Equals(String.Empty))
            {
                linkElement.type = "text/css";
            }
            if (linkElement.rel == null || linkElement.rel.Equals(String.Empty))
            {
                linkElement.rel = "stylesheet";
            }
            base.List.Add(linkElement);
        }

        /// <summary>
        /// Checks if the link element is part of the collection.
        /// </summary>
        /// <remarks>
        /// This member supports the NetRix infrastructure and supports the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </remarks>
        /// <param name="linkElement"></param>
        /// <returns></returns>
        public bool Contains(LinkElement linkElement) 
        {
            if (linkElement == null) return false;
            return List.Contains(linkElement);
        }

        /// <summary>
        /// Removes the link element from the collection.
        /// </summary>
        /// <remarks>
        /// This member supports the NetRix infrastructure and supports the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </remarks>
        /// <param name="linkElement"></param>
        public void Remove(LinkElement linkElement) 
        {
            if (linkElement == null) return;
            base.List.Remove(linkElement);
        }

        /// <summary>
        /// Gets or sets a link element in the collection using an index.
        /// </summary>
        /// <remarks>
        /// This member supports the NetRix infrastructure and supports the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </remarks>
        public LinkElement this[int index] 
        {
            get 
            {
                return (LinkElement)base.List[index];
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

        /// <summary>
        /// Fired if an element is being removed.
        /// </summary>
        public event CollectionRemoveHandler OnRemoveHandler;

        protected override void OnRemove(int index, object value)
        {
            base.OnRemove(index, value);
            if (OnRemoveHandler != null)
            {
                OnRemoveHandler(index, value);
            }
        }

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
		#region ICollectionBase Members

		void GuruComponents.Netrix.WebEditing.Documents.ICollectionBase.Add(object obj)
		{
			if (obj is LinkElement)
			{
				this.Add(obj as LinkElement);
			}
		}

		#endregion
	}
}                                                                            