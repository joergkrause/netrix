using System;
using System.Collections.Generic;
using System.Text;
using sys = System;

namespace GuruComponents.CodeEditor.Library.Collections.Generic
{
	public interface IKeyedCollection<T>
	{

		int Count { get;}

		T this[int index] { get;set;}

		T this[string key]{ get;set;}
		
		string[] Keys{ get;}

		void Add(string key, T item);

		void AddRange(string[] keys, T[] items);

		void Clear();

		void Insert(int index, string key, T item);

		void RemoveAt(int index);

		void Remove(string key);

		void Remove(T item);

		bool Contains(string key);

		bool Contains(T item);

		int IndexOf(string key);

		int IndexOf(T item);

		T[] GetItems();

		T[] GetItems(int startIndex);

		T[] GetItems(int startIndex, int finalIndex);

		void Sort();

		void Reverse();

		void Reverse(int index, int length);

		string CreateFreeKey();

		void Swap(int index1, int index2);
		void Swap(T item1, T item2);

		void Move(int index, int newIndex);
		void Move(T item, int newIndex);

        bool TryGetItem(int index, out T item);
        bool TryGetItem(string key, out T item);
	}
}
