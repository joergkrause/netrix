using System;
using System.Collections;

namespace GuruComponents.Netrix.WebEditing.Documents
{
	/// <summary>
	/// Summary description for ICollectionBase.
	/// </summary>
	public interface ICollectionBase
	{
        /// <summary>
        /// Add an element
        /// </summary>
        /// <param name="obj"></param>
		void Add(object obj);
        /// <summary>
        /// Remove all elements.
        /// </summary>
		void Clear();
        /// <summary>
        /// Remove element at index.
        /// </summary>
        /// <param name="index"></param>
		void RemoveAt(int index);
        /// <summary>
        /// Enumerate
        /// </summary>
        /// <returns></returns>
		IEnumerator GetEnumerator();
        /// <summary>
        /// Number of elements
        /// </summary>
		int Count { get; }

	}
}
