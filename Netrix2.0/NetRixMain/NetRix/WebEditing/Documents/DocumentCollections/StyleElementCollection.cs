using System;
using System.ComponentModel;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Documents
{

    /// <summary>
    /// The purpose of this class is to provide a collection editor for embedded style elements. This is 
    /// necessary because any document can contain as many style tags as needed.
    /// </summary>
//    [PropertyTab(typeof(GuruComponents.Netrix.UserInterface.TypeEditors.CustomPropertyTab), PropertyTabScope.Component)]
    public class StyleElementCollection : System.Collections.CollectionBase, ICollectionBase
    {
        /// <summary>
        /// Adds a style element to the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o"></param>
        public void Add(StyleElement o) 
        {
            if (o == null) return;
            o.type = "text/css";
            base.List.Add(o);
        }

        /// <summary>
        /// Checks if the element is part of the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Contains(StyleElement o) 
        {
            if (o == null) return false;
            return List.Contains(o);
        }

        /// <summary>
        /// Removes the element from the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o"></param>
        public void Remove(StyleElement o) 
        {
            if (o == null) return;
            base.List.Remove(o);
        }

        /// <summary>
        /// Gets or sets a style element in the collection using an index. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        public StyleElement this[int index] 
        {
            get 
            {
                return (StyleElement)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        /// <summary>
        /// Fired if the collection editor inserts a new element. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
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
		#region ICollectionBase Members

		void GuruComponents.Netrix.WebEditing.Documents.ICollectionBase.Add(object obj)
		{
			if (obj is StyleElement)
			{
				this.Add(obj as StyleElement);
			}
		}

		#endregion
	}
}