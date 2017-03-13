#region Using directives

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace GuruComponents.CodeEditor.Library.Collections.Generic
{

	public class LightCollection<T> : ILightCollection<T>,ICloneable,IList
	{
        public struct Enumerator : IEnumerator<T>,IEnumerator,IEnumerable
        {
            private LightCollection<T> _list;
            private int _index;
            //private int _version;
            private T _current;


            internal Enumerator(LightCollection<T> list)
            {
                _list = list;
                _index = 0;
                _current = default(T);
            }
 
            public T Current
            {
                get { return _current; }
            }
            public void Dispose()
            {
            }
            object IEnumerator.Current
            {
                get
                {
                    if ((_index == 0) || (_index == (_list.Count + 1)))
                    {
                        throw new InvalidOperationException();
                    }
                    return this.Current;
                }
            }
            public bool MoveNext()
            {
                if (_index < _list.Count)
                {
                    _current = _list[_index];
                    _index++;
                    return true;
                }
                _index = _list.Count + 1;
                _current = default(T);
                return false;
            }
            public void Reset()
            {
                _index = 0;
                _current = default(T);
            }

			#region IEnumerable Members

			public IEnumerator GetEnumerator()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion

			#region IEnumerable Members

			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion
		}

		public class LightCollectionAddEventArgs:EventArgs
		{
			T Item;
			int Index;

			public LightCollectionAddEventArgs(T item, int index)
			{
				Item = item;
				Index = index;
			}
		}
		public class LightCollectionRemoveEventArgs : EventArgs
		{
			T RemovedItem;
			int Index;

			public LightCollectionRemoveEventArgs(T removedItem, int index)
			{
				RemovedItem = removedItem;
				Index = index;
			}
		}
		public class LightCollectionMoveEventArgs : EventArgs
		{
			T Item;
			int NewIndex;
			int OldIndex;

			public LightCollectionMoveEventArgs(T item, int newIndex,int oldIndex)
			{
				Item = item;
				NewIndex = newIndex;
				OldIndex = oldIndex;
			}
		}
		public class LightCollectionAddRangeEventArgs : EventArgs
		{
			T[] Items;
			int StartIndex;

			public LightCollectionAddRangeEventArgs(T[] items, int startIndex)
			{
				Items = items;
				StartIndex = startIndex;
			}
		}

		public delegate void LightCollectionAddHandler(object sender, LightCollectionAddEventArgs e);
		public delegate void LightCollectionRemoveHandler(object sender, LightCollectionRemoveEventArgs e);
		public delegate void LightCollectionMoveHandler(object sender, LightCollectionMoveEventArgs e);
		public delegate void LightCollectionAddRangeHandler(object sender, LightCollectionAddRangeEventArgs e);
		
		public event LightCollectionAddHandler ItemAdd;
		public event LightCollectionRemoveHandler ItemRemove;
		public event LightCollectionMoveHandler ItemMove;
		public event LightCollectionAddRangeHandler ItemAddRange;
		public event EventHandler CollectionClear;
 
		protected T[] _items;
		private int _count;

		internal static string InternalGetResourceError(string key)
		{
			return string.Empty;
		}

        /// <summary>
        /// Constructor of LightCollection
        /// </summary>
		public LightCollection()
		{
			Init(4);
		}

        /// <summary>
        /// Constructor of LightCollection with the initial capacity specified of the collection
        /// </summary>
        /// <param name="StartCapacity">The initial capacity of the collection</param>
		public LightCollection(int StartCapacity)
		{
			Init(StartCapacity);
		}

        /// <summary>
        /// Constructor of LightCollection
        /// </summary>
        /// <param name="coll">The collection from where copy items</param>
		public LightCollection(ILightCollection<T> coll)
		{
			Init(coll.Count);

			this.AddRange(coll.GetItems());
		}

		void Init(int capacity)
		{
			_items = new T[capacity];
			_count = 0;
		}

        /// <summary>
        /// Constructor of LightCollection with array of T[] as argument
        /// </summary>
        /// <param name="array">Array of T[] as initial values</param>
		public LightCollection(T[] array)
		{
			
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			_items = array;
			_count = array.Length;
		}

        /// <summary>
        /// Return the Count  of this collection
        /// </summary>
		public int Count
		{
			get
			{
				return _count;
			}
		}

        /// <summary>
        /// Return a T element from the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual T this[int index]
        {
            get
            {
                try
                {
                    return _items[index];
                }
                catch
                {
                    throw new IndexOutOfRangeException("Here is no item at index(" + index.ToString() + ")");
                }
            }
            set
            {
                try
                {
                    _items[index] = value;
                }
                catch 
                {
                    throw new IndexOutOfRangeException("Here is no item at index(" + index.ToString() + ")");
                }
            }
        }

        /// <summary>
        /// Add a T item to this collection
        /// </summary>
        /// <param name="item">Item to add to this collection</param>
        /// <returns>Return the Item index on this collection</returns>
        public virtual int Add(T item)
		{
			if (this._count == this._items.Length)
			{
				this.EnsureCapacity(this._count + 1);
			}

			this._items[this._count] = item;

			if (ItemAdd != null) ItemAdd(this, new LightCollectionAddEventArgs(item, this._count));

			return _count++;
		}

        /// <summary>
        /// Add a T item to this collection
        /// </summary>
        /// <param name="item">Item to add to this collection</param>
        /// <param name="sort">set to true for sort now the collection</param>
        /// <returns>Return the Item index on this collection</returns>
        public virtual int Add(T item, bool sort)
		{
			int i = Add(item);

			if(sort)
				this.Sort();

			return i;
		}

        private void EnsureCapacity(int min)
		{
			if (this._items.Length < min)
			{
				int newCapacity = (this._items.Length == 0) ? 4 : (this._items.Length * 2);

				if (newCapacity < min)
				{
					newCapacity = min;
				}
				this.SetCollCapacity(newCapacity);
			}
		}

        private void SetCollCapacity(int value)
		{
			if (value != this._items.Length)
			{
				if (value < this._count)
				{
					throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange SmallCapacity");
				}
				if (value > 0)
				{
                    T[] newArray = new T[value];

					if (this._count > 0)
					{
                        Array.Copy(this._items, 0, newArray, 0, this._count);
					}

                    this._items = newArray;
				}
				else
				{
					this._items = new T[4];
				}
			}
		}
 
        /// <summary>
        /// Add a range of Items to this collection
        /// </summary>
        /// <param name="items">Array of T[] Items to add to this collection</param>
        public virtual void AddRange(T[] items)
		{
			InsertRange(this._count, items);
		}

        /// <summary>
        /// Insert a range of T[] elements to this collection starting from index
        /// </summary>
        /// <param name="index">The from where starting insert</param>
        /// <param name="items">T[] array of elements</param>
		public virtual void InsertRange(int index, T[] items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items", "Null Array");
			}
			if ((index < 0) || (index > this._count))
			{
				throw new ArgumentOutOfRangeException("index", "Index out of Range");
			}
			int num1 = items.Length;
			if (num1 > 0)
			{
				this.EnsureCapacity(this._count + num1);

				if (index < this._count)
				{
					Array.Copy(this._items, index, this._items, (int)(index + num1), (int)(this._count - index));
				}
				items.CopyTo(this._items, index);
				this._count += num1;
			}

			if (ItemAddRange != null) ItemAddRange(this, new LightCollectionAddRangeEventArgs(items, index));
		}

        /// <summary>
        /// Empty the collection
        /// </summary>
		public virtual void Clear()
		{
			_items = new T[]{};
			_count = 0;
			if (CollectionClear != null) CollectionClear(this, new EventArgs());
		}

        /// <summary>
        /// Check if a T item is contained on this collection
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>True if is contained otherwise False</returns>
		public virtual bool Contains(T item)
		{
			return (IndexOf(item, 0, _count) >= 0);
		}

        /// <summary>
        /// Insert a T Item at index
        /// </summary>
        /// <param name="index">The index where insert the Item</param>
        /// <param name="item">The item to insert</param>
		public virtual void Insert(int index, T item)
		{
			if ((index < 0) || (index > this._count))
			{
				throw new ArgumentOutOfRangeException("index", "Index must be within the bounds of the List.");
			}
			if (this._count == this._items.Length)
			{
				this.EnsureCapacity(this._count + 1);
			}
			if (index < this._count)
			{
				Array.Copy(this._items, index, this._items, (int)(index + 1), (int)(this._count - index));
			}
			this._items[index] = item;
			this._count++;

			if (ItemAdd != null) ItemAdd(this, new LightCollectionAddEventArgs(item, index));
		}

        /// <summary>
        /// Remove a T item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public virtual bool Remove(T item)
		{
			int i = this.IndexOf(item);
			if (i >= 0)
			{
				this.RemoveAt(i);
				return true;
			}
			return false;
		}

        /// <summary>
        /// Remove a T item at index
        /// </summary>
        /// <param name="index"></param>
		public virtual void RemoveAt(int index)
		{
			if ((index < 0) || (index >= _count))
			{
				throw new ArgumentOutOfRangeException("index", index, "Index was out of range. Must be non-negative and less than the size of the collection."); // InternalGetResourceFromDefault("ArgumentOutOfRange_Index"));
			}

			T removedItem = _items[index];

			Array.Copy(_items, index+1, _items, index , _items.Length - index - 1);


			Array.Resize<T>(ref _items, _count -1);

			_count--;

			if (ItemRemove != null) ItemRemove(this, new LightCollectionRemoveEventArgs(removedItem,index));
		}

        /// <summary>
        /// Get and Set the Collection Capacity
        /// </summary>
		public virtual int Capacity
		{
			get
			{
				return this._items.Length;
			}
			set
			{
				this.SetCollCapacity(value);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
		public virtual T Find(Predicate<T> match)
		{
			
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			for (int i = 0; i < _count; i++)
			{
				if (match(_items[i]))
				{
					return _items[i];
				}
			}
			T type;
			type = default(T);
			return type;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public virtual int IndexOf(T item)
		{
			return IndexOf(item, 0, _count);
		}

		public virtual int IndexOf(T item, int index)
		{
			return IndexOf(item, index, _count-index);
		}

		public virtual int IndexOf(T item, int index, int count)
		{
            //if ((index < 0) || (index > _count))
            //{
            //    throw new ArgumentOutOfRangeException("startIndex", index,"Index was out of range. Must be non-negative and less than the size of the collection."); // InternalGetResourceFromDefault("ArgumentOutOfRange_Index"));
            //}
            //if ((count < 0) || (count > (_count - index)))
            //{
            //    throw new ArgumentOutOfRangeException("count", count, "Count must be positive and count must refer to a location within the string/array/collection."); // InternalGetResourceFromDefault("ArgumentOutOfRange_Count"));
            //}
            //int end = index + count;
            //if ((object)item == null)
            //{
            //    for (int i = index; i < end; i++)
            //    {
            //        if ((object)_items[i] == null)
            //        {
            //            return i;
            //        }
            //    }
            //}
            //else
            //{
            //    for (int j = index; j < end; j++)
            //    {
            //        if(item.Equals(_items[j]))
            //        {
            //            return j;
            //        }
            //    }
            //}
            return Array.IndexOf<T>(this._items, item, index, count);
			//return -1;
		}

		public virtual void TrimToSize()
		{
			this.Capacity = this._count;
		}

		public virtual T[] GetItems()
		{
			if (_count <= 0) return new T[0];
            return GetItems(0, _count-1);
		}

		public virtual T[] GetItems(int startIndex)
		{
			if (_count <= 0) return new T[0];
			return GetItems(startIndex, _count - 1);
		}

		public virtual T[] GetItems(int startIndex, int finalIndex)
		{
			if (_count <= 0) return new T[0];
			if (finalIndex < startIndex)
			{
				throw new ArgumentOutOfRangeException("finalIndex", finalIndex, "finalIndex was out of range. Must be non-negative and less than the size of the collection.");
			}
			if ((startIndex < 0) || (startIndex > _count))
			{
				throw new ArgumentOutOfRangeException("startIndex", startIndex, "startIndex was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (finalIndex > _count - 1)
			{
				throw new ArgumentOutOfRangeException("finalIndex", finalIndex, "finalIndex was out of range. Must be minor of the Count");
			}
			T[] newItems = new T[finalIndex - startIndex + 1];

			Array.Copy(_items, startIndex, newItems, 0, finalIndex+1);

			return newItems;
		}

        public virtual IEnumerator<T> GetEnumerator()
        {
            return new LightCollection<T>.Enumerator(this);
        }

		public virtual void Sort(int index, int length)
		{
			Array.Sort<T>(_items, index, length);
		}

		public virtual void Sort()
		{
			Sort(0, _count);
		}

        public virtual void Sort(Array keys)
        {
            if (keys.Length != _items.Length)
            {
				throw new Exception("The Length of \"keys\" does not match with \"items\" lenght");
            }
            Array.Sort(keys, _items);
        }

		#region ICloneable Members


		#endregion

		#region ICloneable Members

		public LightCollection<T> Clone()
		{
			return new LightCollection<T>(this);
		}

		object ICloneable.Clone()
		{
			return this.Clone() ;
		}

		#endregion

		#region IList Members

		int IList.Add(object value)
		{
            if (!value.GetType().Equals(typeof(T))) throw new ArgumentException("Invalid type " + value.GetType().ToString());
            return this.Add((T)value);
		}

		void IList.Clear()
		{
            this.Clear();
		}

		bool IList.Contains(object value)
		{
            if (!value.GetType().Equals(typeof(T))) throw new ArgumentException("Invalid type " + value.GetType().ToString());
            return this.Contains((T)value);
		}

		int IList.IndexOf(object value)
		{
            if (!value.GetType().Equals(typeof(T))) throw new ArgumentException("Invalid type " + value.GetType().ToString());
            return this.IndexOf((T)value);
		}

		void IList.Insert(int index, object value)
		{
            if (!value.GetType().Equals(typeof(T))) throw new ArgumentException("Invalid type " + value.GetType().ToString());
            this.Insert(index,(T)value);
		}

		bool IList.IsFixedSize
		{
            get { return false; }
		}

		bool IList.IsReadOnly
		{
			get { return false; }
		}

		void IList.Remove(object value)
		{
            if (!value.GetType().Equals(typeof(T))) throw new ArgumentException("Invalid type " + value.GetType().ToString());
            this.Remove((T)value);
		}

		void IList.RemoveAt(int index)
		{
            this.RemoveAt(index);
		}

		object IList.this[int index]
		{
			get
			{
                return this[index];
			}
			set
			{
                if (!value.GetType().Equals(typeof(T))) throw new ArgumentException("Invalid type " + value.GetType().ToString());
                this[index] = (T)value;
			}
		}

		#endregion

		#region ICollection Members

		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyTo(array, index);
		}

		int ICollection.Count
		{
			get 
            {
				return this.Count;
			}
		}

		bool ICollection.IsSynchronized
		{
			get 
            {
				return false;
			}
		}

		object ICollection.SyncRoot
		{
			get 
            {
				return null;
			}
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region ILightCollection<T> Members

		public void CopyTo(Array array, int index)
		{
			Array.Copy(_items, index, array, 0, _count);
		}

		#endregion

		#region ILightCollection<T> Members

		public void Reverse()
		{
			Array.Reverse(_items,0,_count);
		}

		public void Reverse(int index, int length)
		{
			if ((index + length) >= _count) throw new ArgumentOutOfRangeException((index >= _count) ? "index" : "length");
			if (length <= 1) return;
			Array.Reverse(_items,index, length);
		}

		public void Move(int index, int newIndex)
		{
			if (index == newIndex) return;
			if (index < 0 || index >= _items.Length) throw new ArgumentOutOfRangeException("index");
			if (newIndex < 0 || newIndex >= _items.Length) throw new ArgumentOutOfRangeException("newIndex");

			int min=Math.Min(index,newIndex);
			T[] newArray = new T[_items.Length - min];

			int cp_index = index-min;
			int cp_newIndex = newIndex - min;

			Array.Copy(_items, min, newArray, 0, newArray.Length);

			if (index > newIndex)
			{
				int pos = newIndex + 1;

				_items[newIndex] = newArray[cp_index];

				for (int i = 0; i < newArray.Length; i++)
				{
					if (i != (cp_index))
					{
						_items[pos] = newArray[cp_newIndex + i];
						pos++;
					}
				}
			}
			else // index < newIndex
			{
				int pos = index;

				_items[newIndex] = newArray[cp_index];

				for (int i = cp_index+1; i < newArray.Length; i++)
				{
					if (pos == (newIndex)) pos++;

					_items[pos] = newArray[i];
					pos++;
				}
			}

			ItemMove(this, new LightCollectionMoveEventArgs(_items[newIndex], newIndex, index));

		}
		public void Move(T item, int newIndex)
		{
			int index = this.IndexOf(item);
			Move(index, newIndex);
		}

		public void Swap(int index1, int index2)
		{
			if (index1 == index2) return;
			if (index1 < 0 || index1 >= _items.Length) throw new ArgumentOutOfRangeException("index1");
			if (index2 < 0 || index2 >= _items.Length) throw new ArgumentOutOfRangeException("index2");


			T temp = _items[index2];
			_items[index2] = _items[index1];
			_items[index1] = temp;
		}
		public void Swap(T item1, T item2)
		{
			if (item1.Equals(item2)) return;

			int index1 = this.IndexOf(item1);
			int index2 = this.IndexOf(item2);

			Swap(index1, index2);
		}

		#endregion

        #region ILightCollection<T> Members

        public bool TryGetItem(int index, out T item)
        {
            item = default(T);
            if (index < 0 || index >= _items.Length) return false;
            item = _items[index];
            return true;
        }

        #endregion
}
}
