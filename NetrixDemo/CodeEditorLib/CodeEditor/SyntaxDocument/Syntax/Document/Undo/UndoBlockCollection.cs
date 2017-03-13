using System;
using System.Collections;
using T = GuruComponents.CodeEditor.CodeEditor.Syntax.UndoBlock;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class UndoBlockCollection : ICollection, IList, IEnumerable, ICloneable
	{
		private const int DefaultMinimumCapacity = 16;

		private T[] m_array = new T[DefaultMinimumCapacity];
		private int m_count = 0;
		private int m_version = 0;

		/// <summary>
		/// 
		/// </summary>
		public string Name = "UndoAction";

		// Construction

		/// <summary>
		/// 
		/// </summary>
		public UndoBlockCollection()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="collection"></param>
		public UndoBlockCollection(UndoBlockCollection collection)
		{
			AddRange(collection);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		public UndoBlockCollection(T[] array)
		{
			AddRange(array);
		}

		// Operations (type-safe ICollection)

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get { return m_count; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		public void CopyTo(T[] array)
		{
			this.CopyTo(array, 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="start"></param>
		public void CopyTo(T[] array, int start)
		{
			if (m_count > array.GetUpperBound(0) + 1 - start)
				throw new ArgumentException("Destination array was not long enough.");

			// for (int i=0; i < m_count; ++i) array[start+i] = m_array[i];
			Array.Copy(m_array, 0, array, start, m_count);
		}

		// Operations (type-safe IList)

		/// <summary>
		/// 
		/// </summary>
		public T this[int index]
		{
			get
			{
				ValidateIndex(index); // throws
				return m_array[index];
			}
			set
			{
				ValidateIndex(index); // throws

				++m_version;
				m_array[index] = value;
			}
		}

		/// <summary>
/// 
/// </summary>
/// <param name="item"></param>
/// <returns></returns>
		public int Add(T item)
		{
			if (NeedsGrowth())
				Grow();

			++m_version;
			m_array[m_count] = item;

			return m_count++;
		}

		/// <summary>
/// 
/// </summary>
		public void Clear()
		{
			++m_version;
			m_array = new T[DefaultMinimumCapacity];
			m_count = 0;
		}

		/// <summary>
/// 
/// </summary>
/// <param name="item"></param>
/// <returns></returns>
		public bool Contains(T item)
		{
			return ((IndexOf(item) == -1) ? false : true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(T item)
		{
			for (int i = 0; i < m_count; ++i)
				if (m_array[i] == (item))
					return i;
			return -1;
		}

		/// <summary>
/// 
/// </summary>
/// <param name="position"></param>
/// <param name="item"></param>
		public void Insert(int position, T item)
		{
			ValidateIndex(position, true); // throws

			if (NeedsGrowth())
				Grow();

			++m_version;
			// for (int i=m_count; i > position; --i) m_array[i] = m_array[i-1];
			Array.Copy(m_array, position, m_array, position + 1, m_count - position);

			m_array[position] = item;
			m_count++;
		}

		/// <summary>
/// 
/// </summary>
/// <param name="item"></param>
		public void Remove(T item)
		{
			int index = IndexOf(item);
			if (index < 0)
				throw new ArgumentException("Cannot remove the specified item because it was not found in the specified Collection.");

			RemoveAt(index);
		}

		/// <summary>
/// 
/// </summary>
/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			ValidateIndex(index); // throws

			++m_version;
			m_count--;
			// for (int i=index; i < m_count; ++i) m_array[i] = m_array[i+1];
			Array.Copy(m_array, index + 1, m_array, index, m_count - index);

			if (NeedsTrimming())
				Trim();
		}

		// Operations (type-safe IEnumerable)
		/// <summary>
/// 
/// </summary>
/// <returns></returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		// Operations (type-safe ICloneable)
		/// <summary>
/// 
/// </summary>
/// <returns></returns>
		public UndoBlockCollection Clone()
		{
			UndoBlockCollection tc = new UndoBlockCollection();
			tc.AddRange(this);
			tc.Capacity = this.m_array.Length;
			tc.m_version = this.m_version;
			return tc;
		}

		// Public helpers (just to mimic some nice features of ArrayList)
		/// <summary>
/// 
/// </summary>
		public int Capacity
		{
			get { return m_array.Length; }
			set
			{
				if (value < m_count) value = m_count;
				if (value < DefaultMinimumCapacity) value = DefaultMinimumCapacity;

				if (m_array.Length == value) return;

				++m_version;

				T[] temp = new T[value];
				// for (int i=0; i < m_count; ++i) temp[i] = m_array[i];
				Array.Copy(m_array, 0, temp, 0, m_count);
				m_array = temp;
			}
		}

		/// <summary>
/// 
/// </summary>
/// <param name="collection"></param>
		public void AddRange(UndoBlockCollection collection)
		{
			// for (int i=0; i < collection.Count; ++i) Add(collection[i]);

			++m_version;

			Capacity += collection.Count;
			Array.Copy(collection.m_array, 0, this.m_array, m_count, collection.m_count);
			m_count += collection.Count;
		}

		/// <summary>
/// 
/// </summary>
/// <param name="array"></param>
		public void AddRange(T[] array)
		{
			// for (int i=0; i < array.Length; ++i) Add(array[i]);

			++m_version;

			Capacity += array.Length;
			Array.Copy(array, 0, this.m_array, m_count, array.Length);
			m_count += array.Length;
		}

		// Implementation (helpers)

		private void ValidateIndex(int index)
		{
			ValidateIndex(index, false);
		}

		private void ValidateIndex(int index, bool allowEqualEnd)
		{
			int max = (allowEqualEnd) ? (m_count) : (m_count - 1);
			if (index < 0 || index > max)
				throw new ArgumentOutOfRangeException("Index was out of range.  Must be non-negative and less than the size of the collection.", (object) index, "Specified argument was out of the range of valid values.");
		}

		private bool NeedsGrowth()
		{
			return (m_count >= Capacity);
		}

		private void Grow()
		{
			if (NeedsGrowth())
				Capacity = m_count*2;
		}

		private bool NeedsTrimming()
		{
			return (m_count <= Capacity/2);
		}

		private void Trim()
		{
			if (NeedsTrimming())
				Capacity = m_count;
		}

		// Implementation (ICollection)

		/* redundant w/ type-safe method
			int ICollection.Count
			{
				get
				{ return m_count; }
			}
			*/

		bool ICollection.IsSynchronized
		{
			get { return m_array.IsSynchronized; }
		}

		object ICollection.SyncRoot
		{
			get { return m_array.SyncRoot; }
		}

		void ICollection.CopyTo(Array array, int start)
		{
			this.CopyTo((T[]) array, start);
		}

		// Implementation (IList)

		bool IList.IsFixedSize
		{
			get { return false; }
		}

		bool IList.IsReadOnly
		{
			get { return false; }
		}

		object IList.this[int index]
		{
			get { return (object) this[index]; }
			set { this[index] = (T) value; }
		}

		int IList.Add(object item)
		{
			return this.Add((T) item);
		}

		/* redundant w/ type-safe method
			void IList.Clear()
			{
				this.Clear();
			}
			*/

		bool IList.Contains(object item)
		{
			return this.Contains((T) item);
		}

		int IList.IndexOf(object item)
		{
			return this.IndexOf((T) item);
		}

		void IList.Insert(int position, object item)
		{
			this.Insert(position, (T) item);
		}

		void IList.Remove(object item)
		{
			this.Remove((T) item);
		}

		/* redundant w/ type-safe method
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}
			*/

		// Implementation (IEnumerable)

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator) (this.GetEnumerator());
		}

		// Implementation (ICloneable)

		object ICloneable.Clone()
		{
			return (object) (this.Clone());
		}

		// Nested enumerator class

		/// <summary>
		/// 
		/// </summary>
		public class Enumerator : IEnumerator
		{
			private UndoBlockCollection m_collection;
			private int m_index;
			private int m_version;

			// Construction

			public Enumerator(UndoBlockCollection tc)
			{
				m_collection = tc;
				m_index = -1;
				m_version = tc.m_version;
			}

			// Operations (type-safe IEnumerator)

			/// <summary>
			/// 
			/// </summary>
			public T Current
			{
				get { return m_collection[m_index]; }
			}

			/// <summary>
/// 
/// </summary>
/// <returns></returns>
			public bool MoveNext()
			{
				if (m_version != m_collection.m_version)
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");

				++m_index;
				return (m_index < m_collection.Count) ? true : false;
			}

			/// <summary>
/// 
/// </summary>
			public void Reset()
			{
				if (m_version != m_collection.m_version)
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");

				m_index = -1;
			}

			// Implementation (IEnumerator)

			object IEnumerator.Current
			{
				get { return (object) (this.Current); }
			}

			/* redundant w/ type-safe method
				bool IEnumerator.MoveNext()
				{
					return this.MoveNext();
				}
				*/

			/* redundant w/ type-safe method
				void IEnumerator.Reset()
				{
					this.Reset();
				}
				*/
		}
	}
}