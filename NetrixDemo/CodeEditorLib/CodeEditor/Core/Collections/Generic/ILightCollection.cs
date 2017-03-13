using System;
using System.Collections.Generic;
using System.Text;
using sys = System;

namespace GuruComponents.CodeEditor.Library.Collections.Generic
{
	public interface ILightCollection<T>
	{
		int Count { get;}

		T this[int index]
		{
			get;
			set;
		}

		int Add(T item);

		void AddRange(T[] items);

		void Clear();

		bool Contains(T item);

		void Insert(int index, T item);

		bool Remove(T item);

		void RemoveAt(int index);

		T Find(Predicate<T> match);

		int IndexOf(T item);

		int IndexOf(T item, int index);

		int IndexOf(T item, int index, int count);

		T[] GetItems();

		T[] GetItems(int startIndex);

		T[] GetItems(int startIndex, int finalIndex);

		void CopyTo(Array array, int index);

		IEnumerator<T> GetEnumerator();

		void Reverse();

		void Reverse(int index, int length);

		void Move(int index, int newIndex);
		void Move(T item, int newIndex);

		void Swap(int index1, int index2);
		void Swap(T item1, T item2);

        bool TryGetItem(int index, out T item);
	}
}
