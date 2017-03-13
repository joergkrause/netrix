using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.CodeEditor.Library.Collections.Generic
{
	public class TreeItem<T>
	{
		private string _id;

		private string _parent;
		private string _next;
		private string _previus;
		//private string _fullPath;

		private TreeCollection<T> _ownerCollection;

		private LightCollection<string> _subitems;

		internal TreeItem(string id, string parent, ref TreeCollection<T> ownerCollection)
		{
			_id = id;
			_parent = parent;
			_ownerCollection = ownerCollection;
			_subitems = new LightCollection<string>();
		}

		internal void SetId(string id)
		{
			_id=id;
		}
		internal void SetParent(string parent)
		{
			_parent = parent;
		}
		/*internal void SetGlobalCollectionRef(ref KeyedCollection<TreeItemInternal<T>> globalCollection)
		{
			_globalCollection = globalCollection;
		}*/
		internal void SetNext(string next)
		{
			_next = next;
		}
		internal void SetPrevius(string previus)
		{
			_previus = previus;
		}

		public string Id
		{
			get
			{
				return _id;
			}
		}
		public T Value
		{
			get
			{
				return _ownerCollection.GlobalValues[_id];
			}
			set
			{
				_ownerCollection.GlobalValues[_id] = value;
			}
		}

		public int Level
		{
			get
			{
				string[] strs = GetFullPath().Split('/');
				return strs.Length-1;
			}
		}

		public TreeItem<T> Parent
		{
			get
			{
				return _ownerCollection.GlobalCollection[_parent];
			}
		}
		public TreeItem<T> Next
		{
			get
			{
				return _ownerCollection.GlobalCollection[_next];
			}
		}
		public TreeItem<T> Previus
		{
			get
			{
				return _ownerCollection.GlobalCollection[_previus];
			}
		}

		public TreeItemCollection<T> Subitems
		{
			get
			{
				return new TreeItemCollection<T>(ref _ownerCollection,_id,_subitems);
			}
		}

		public string GetFullPath()
		{
			LightCollection<string> paths=new LightCollection<string>();
			paths.Add(_id);
			TreeItem<T> parent = this.Parent;
			while (parent != null)
			{
				paths.Add(parent.Id);
				parent = parent.Parent;
			}
			paths.Reverse();
			string fullPath = string.Join("/", paths.GetItems());
			return fullPath;
		}
	}
}


