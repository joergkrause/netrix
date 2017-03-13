using System;
using System.Collections;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{

	#region EventArgs and Delegate

	public class CollectionEventArgs : EventArgs
	{
		public CollectionEventArgs()
		{
		}

		public CollectionEventArgs(object item, int index)
		{
			this.Index = index;
			this.Item = item;
		}

		public object Item = null;
		public int Index = 0;
	}

	public delegate void CollectionEventHandler(object sender, CollectionEventArgs e);

	#endregion

	public abstract class BaseCollection : System.Collections.CollectionBase, IList
	{
        public BaseCollection()
		{
		}

		#region Events

		public event CollectionEventHandler ItemAdded = null;

		protected virtual void OnItemAdded(int index, object item)
		{
			if (this.ItemAdded != null)
			{
				CollectionEventArgs e = new CollectionEventArgs(item, index);

				this.ItemAdded(this, e);
			}
		}

		public event CollectionEventHandler ItemRemoved = null;

		protected virtual void OnItemRemoved(int index, object item)
		{
			if (this.ItemRemoved != null)
			{
				CollectionEventArgs e = new CollectionEventArgs(item, index);

				this.ItemRemoved(this, e);
			}
		}

		public event EventHandler ItemsCleared = null;

		protected virtual void OnItemsCleared()
		{
			if (this.ItemsCleared != null)
				this.ItemsCleared(this, EventArgs.Empty);
		}

		#endregion

		#region Overrides

		protected override void OnClearComplete()
		{
			base.OnClearComplete();
			this.OnItemsCleared();
		}

		protected override void OnRemoveComplete(int index, object value)
		{
			base.OnRemoveComplete(index, value);
			this.OnItemRemoved(index, value);
		}

		protected override void OnInsertComplete(int index, object value)
		{
			base.OnInsertComplete(index, value);
			this.OnItemAdded(index, value);
		}

		#endregion
	}
}