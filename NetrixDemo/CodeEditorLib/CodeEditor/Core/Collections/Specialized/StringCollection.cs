using System;
using System.Collections.Generic;
using System.Text;

using GuruComponents.CodeEditor.Library.Collections.Generic;

namespace GuruComponents.CodeEditor.Library.Collections.Specialized
{
	public class StringCollection:ILightCollection<string>
	{
		private LightCollection<string> _coll;

		public StringCollection()
		{
			_coll = new LightCollection<string>();
		}
		public StringCollection(string[] items)
		{
			_coll = new LightCollection<string>(items);
		}
		public StringCollection(ILightCollection<string> collection)
		{
			_coll = new LightCollection<string>(collection);
		}
		public StringCollection(int startCapacity)
		{
			_coll = new LightCollection<string>(startCapacity);
		}

		#region ILightCollection<string> Members

		public int Count
		{
			get { return _coll.Count; }
		}
		public string this[int index]
		{
			get
			{
				return _coll[index];
			}
			set
			{
				_coll[index] = value;
			}
		}
		public int Add(string item)
		{
			return _coll.Add(item);
		}
		public void AddRange(string[] items)
		{
			_coll.AddRange(items);
		}
		public void Clear()
		{
			_coll.Clear();
		}
		public bool Contains(string item)
		{
			return _coll.Contains(item);
		}
		public void Insert(int index, string item)
		{
			_coll.Insert(index, item);
		}
		public bool Remove(string item)
		{
			return _coll.Remove(item);
		}
		public void RemoveAt(int index)
		{
			_coll.RemoveAt(index);
		}
		public string Find(Predicate<string> match)
		{
			return _coll.Find(match);
		}
		public int IndexOf(string item)
		{
			return _coll.IndexOf(item);
		}
		public int IndexOf(string item, int index)
		{
			return _coll.IndexOf(item, index);
		}
		public int IndexOf(string item, int index, int count)
		{
			return _coll.IndexOf(item, index, count);
		}
		public string[] GetItems()
		{
			return _coll.GetItems();
		}
		public string[] GetItems(int startIndex)
		{
			return _coll.GetItems(startIndex);
		}
		public string[] GetItems(int startIndex, int finalIndex)
		{
			return _coll.GetItems(startIndex, finalIndex);
		}
		public void CopyTo(Array array, int index)
		{
			_coll.CopyTo(array, index);
		}
		public IEnumerator<string> GetEnumerator()
		{
			return _coll.GetEnumerator();
		}
		public void Reverse()
		{
			_coll.Reverse();
		}
		public void Reverse(int index, int length)
		{
			_coll.Reverse(index, length);
		}
		public void Move(int index, int newIndex)
		{
			_coll.Move(index, newIndex);
		}
		public void Move(string item, int newIndex)
		{
			_coll.Move(item, newIndex);
		}
		public void Swap(int index1, int index2)
		{
			_coll.Swap(index1, index2);
		}
		public void Swap(string item1, string item2)
		{
			_coll.Swap(item1, item2);
		}

		#endregion

		public string Join(string separator)
		{
			string[] items = _coll.GetItems();
			return string.Join(separator, items);
		}
		public string Join(string separator,int startIndex,int count)
		{
			string[] items = _coll.GetItems();
			return string.Join(separator, items, startIndex,count);
		}
		public void ToLower()
		{
			for (int i = 0; i < _coll.Count; i++)
			{
				if (_coll[i] == null) continue;

				_coll[i] = _coll[i].ToLower();
			}
		}
		public void ToUpper()
		{
			for (int i = 0; i < _coll.Count; i++)
			{
				if (_coll[i] == null) continue;

				_coll[i] = _coll[i].ToUpper();
			}
		}
		public void ReverseStrings()
		{
			for (int i = 0; i < _coll.Count; i++)
			{
				if (_coll[i] == null) continue;

				char[] chars = _coll[i].ToCharArray();
				Array.Reverse(chars);
				_coll[i] = new string(chars);
			}
		}

        #region ILightCollection<string> Members

        public bool TryGetItem(int index, out string item)
        {
            return _coll.TryGetItem(index,out item);
        }

        #endregion
}
}
