using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.CodeEditor.Library.Collections.Generic
{
	public class TreeItemEnumerator<T>:IEnumerator<TreeItem<T>>
	{
		private TreeItem<T> _current;
		private KeyedCollection<TreeItem<T>> _globalCollection;
		private LightCollection<string> _rootItems;

		internal TreeItemEnumerator(ref KeyedCollection<TreeItem<T>> globalCollection,LightCollection<string> rootItems)
		{
			_globalCollection = globalCollection;
			_rootItems = rootItems;
		}

		public void Dispose()
		{
			_current = null;
			_globalCollection.Clear();
			_rootItems.Clear();
		}

		public TreeItem<T> Current
		{
			get { return _current; }
		}
		object IEnumerator.Current
		{
			get { return _current; }
		}

		public void Reset()
		{
			_current = null;
		}

		public bool MoveNext()
		{
			if (_current == null)
			{
				_current = _globalCollection[_rootItems[0]];
				return true;
			}

			TreeItem<T> current = _current.Next;

			if (current != null)
			{
				_current = current;
				return true;
			}

			while (current == null)
			{
				current = _current.Parent;

				if (current == null) return false;

				current = current.Next;
			}

			_current = current;
			return true;

		}
		public bool MovePrevius()
		{
			if (_current == null)
			{
				return false;
			}

			TreeItem<T> current = _current.Previus;

			if (current != null)
			{
				_current = current;
				return true;
			}

			while (current == null)
			{
				current = _current.Parent;

				if (current == null) return false;

				current = current.Next;
			}

			_current = current;
			return true;

		}
		public bool MoveNextSibiling()
		{
			if (_current == null)
			{
				_current = _globalCollection[_rootItems[0]];
				return true;
			}

			TreeItem<T> current = _current.Next;

			if (current != null)
			{
				_current = current;
				return true;
			}
			return false;
		}
		public bool MovePreviusSibiling()
		{
			if (_current == null) return false;

			TreeItem<T> current = _current.Previus;

			if (current != null)
			{
				_current = current;
				return true;
			}
			return false;
		}
		public bool MoveParent()
		{
			if (_current == null) return false;

			TreeItem<T> current = _current.Parent;

			if (current != null)
			{
				_current = current;
				return true;
			}
			return false;
		}
		public bool MoveFirstSibiling()
		{
			if (_current == null)
			{
				_current = _globalCollection[_rootItems[0]];
				return true;
			}

			if (_current.Previus == null) return false;

			TreeItem<T> newCurrent = _current.Previus;
			while (newCurrent != null)
			{
				_current = newCurrent;
				newCurrent = _current.Previus;
			}
			return true;
		}
		public bool MoveLastSibiling()
		{
			if (_current == null)
			{
				_current = _globalCollection[_rootItems[_rootItems.Count - 1]];
				return true;
			}

			if (_current.Next == null) return false;

			TreeItem<T> newCurrent = _current.Next;
			while (newCurrent != null)
			{
				_current = newCurrent;
				newCurrent = _current.Next;
			}
			return true;
		}
		public bool MoveFirstChild()
		{
			if (_current == null)
			{
				_current = _globalCollection[_rootItems[0]];
				return true;
			}

			if (_current.Subitems.Count == 0) return false;

			_current = _current.Subitems[0];
			return true;
		}
		public bool MoveLastChild()
		{
			if (_current == null)
			{
				_current = _globalCollection[_rootItems[_rootItems.Count-1]];
				return true;
			}

			if (_current.Subitems.Count == 0) return false;

			_current = _current.Subitems[_current.Subitems.Count-1];
			return true;
		}
	}
}
