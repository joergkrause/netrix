using System;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Documents
{

    /// <summary>
    /// This class stores a collection of frames. It supports the NetRix infrastructure and is used to 
    /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
    /// </summary>
    public class FrameElementCollection : System.Collections.CollectionBase,ICollectionBase
    {
        /// <summary>
        /// Add a frame element to the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o"></param>
        public void Add(FrameElement o) 
        {
            base.List.Add(o);
        }

        /// <summary>
        /// Checks if the given frame element is already part of the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o">The frame element</param>
        /// <returns>True if the element is already part of the collection.</returns>
        public bool Contains(FrameElement o) 
        {
            return List.Contains(o);
        }

        /// <summary>
        /// Remove the given frame element from the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o"></param>
        public void Remove(FrameElement o) 
        {
            base.List.Remove(o);
        }

        /// <summary>
        /// Gets or sets a frame element in the collection using an index. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        public FrameElement this[int index] 
        {
            get 
            {
                return (FrameElement)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
		}
		#region ICollectionBase Members

		void GuruComponents.Netrix.WebEditing.Documents.ICollectionBase.Add(object obj)
		{
			if (obj is FrameElement)
			{
				this.Add(obj as FrameElement);
			}
		}

		#endregion
	}
}