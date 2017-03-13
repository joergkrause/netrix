
using System;

namespace Comzept.Library.Drawing.GDI
{
	/// <summary>
	/// Summary description for GDIObject.
	/// </summary>
	public abstract class GDIObject : IDisposable
	{
		protected bool IsCreated = false;

		protected virtual void Destroy()
		{
			IsCreated = false;
			MemHandler.Remove(this);
		}

		protected virtual void Create()
		{
			IsCreated = true;
			MemHandler.Add(this);
		}

		#region Implementation of IDisposable

		public void Dispose()
		{
			this.Destroy();
		}

		#endregion
	}
}