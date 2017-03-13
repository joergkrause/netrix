using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.VmlDesigner.Elements;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
	/// <summary>
	/// Definition of a vector of Point.
	/// </summary>
	public class VgDashStyleArray : GuruComponents.Netrix.WebEditing.Documents.ICollectionBase
	{
        private IVgDashStyleArray nativeVector;        
		private ArrayList wrappedVector;

		internal VgDashStyleArray(IVgDashStyleArray nativeVector)
		{
			this.nativeVector = nativeVector;
			wrappedVector = new ArrayList(nativeVector.length);
			for(int i = 0; i < nativeVector.length; i++)
			{
				wrappedVector.Add(nativeVector[i]);
			}
		}

		#region IVgDashStyleArray Members

		[Browsable(false)]
		public int Creator
		{
			get
			{
				return nativeVector.Creator;
			}
		}

		public int this[int Index]
		{
			get
			{
				return nativeVector[Index];
			}
			set
			{
				nativeVector[Index] = value;
			}
		}

		[Browsable(false)]
		public VgDashStyle ParentDashStyle
		{
			get
			{
				return new VgDashStyle(nativeVector.parentDashStyle as IVgLineDashStyle);
			}
		}

        /// <summary>
        /// Application
        /// </summary>				
		[Browsable(false)]
		public object Application
		{
			get
			{
				return null;
			}
		}

		public int Length
		{
			get
			{
				return nativeVector.length;
			}
		}

		#endregion

		#region ICollectionBase Members

		public IEnumerator GetEnumerator()
		{
			return wrappedVector.GetEnumerator();
		}

		public void RemoveAt(int index)
		{
			wrappedVector.RemoveAt(index);
			SetValue();
		}

		[Browsable(false)]
		public int Count
		{
			get
			{
				return wrappedVector.Count;
			}
		}

		public void Clear()
		{
			wrappedVector.Clear();
			SetValue();
		}

		public void Add(object obj)
		{
			wrappedVector.Add(obj);
		}

		#endregion

		private void SetValue()
		{
			string[] w = new string[wrappedVector.Count];
			wrappedVector.CopyTo(w);
			ParentDashStyle.Value = String.Join(" ", w);
		}

		public override string ToString()
		{
			return "Dashstyle Elements";
		}


	}
}
