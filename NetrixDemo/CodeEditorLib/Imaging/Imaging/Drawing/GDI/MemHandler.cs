
using System.Collections;

namespace Comzept.Library.Drawing.GDI
{
	/// <summary>
	/// Summary description for MemHandler.
	/// </summary>
	public class MemHandler
	{
		private static ArrayList mHeap = new ArrayList();

		public static void Add(GDIObject item)
		{
			mHeap.Add(item);
		}

		public static void Remove(GDIObject item)
		{
			if (mHeap.Contains(item))
			{
				mHeap.Remove(item);
			}
		}

		public static GDIObject[] Items
		{
			get
			{
				ArrayList al = new ArrayList();

				foreach (GDIObject go in mHeap)
				{
					if (go != null)
						al.Add(go);
				}

				GDIObject[] gos = new GDIObject[al.Count];
				al.CopyTo(0, gos, 0, al.Count);
				return gos;
			}
		}

		public static void DestroyAll()
		{
			foreach (GDIObject go in Items)
			{
				go.Dispose();
			}
		}
	}
}