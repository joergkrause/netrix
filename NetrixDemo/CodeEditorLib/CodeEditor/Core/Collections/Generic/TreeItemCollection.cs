using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.CodeEditor.Library.Collections.Generic
{
	public class TreeItemCollection<T>
	{
		private string _parent;
		private LightCollection<string> _items;
		private TreeCollection<T> _ownerCollection;

		internal TreeItemCollection(ref TreeCollection<T> ownerCollection, string parent, LightCollection<string> items)
		{
			_parent = parent;
			_items = new LightCollection<string>(items);
			_ownerCollection = ownerCollection;
		}
		internal TreeItemCollection(ref TreeCollection<T> ownerCollection , string parent)
			: this(ref ownerCollection, parent, new LightCollection<string>())
		{
		}

		public TreeItem<T> Add(T value)
		{
			string key = _ownerCollection.GlobalCollection.CreateFreeKey();
			return Add(key, value);
		}
		public TreeItem<T> Add(string id)
		{
			return Add(id, default(T));
		}
		public TreeItem<T> Add(string id, T value)
		{
			if(_ownerCollection.GlobalCollection.Contains(id))
				throw new Exception("key \""+id+"\" already exist");

			TreeItem<T> treeItem = new TreeItem<T>(id, _parent, ref _ownerCollection);
			_ownerCollection.Global_AddItem(id, treeItem);

			_items.Add(id);
			return treeItem;

		}
		public TreeItem<T> TryAdd(string id, T value)
		{
			if (_ownerCollection.GlobalCollection.Contains(id))
				return null;
			return Add(id, value);
		}

		public void Remove(int index)
		{
			
		}

		public TreeItem<T> this[int index]
		{
			get
			{
				return _ownerCollection.GlobalCollection[_items[index]];
			}
		}
		public TreeItem<T> this[string id]
		{
			get
			{
				return _ownerCollection.GlobalCollection[id];
			}
		}

		public int Count
		{
			get
			{
				return _items.Count;
			}
		}

		internal LightCollection<string> ItemsId
		{
			get
			{
				return _items;
			}
		}

	}
}
