using System;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner.Edx
{
	/// <summary>
	/// Simple object describing a selection or partial selection in a field.
	/// </summary>
	public struct Selection
	{
		/// <summary>
		/// Empty Selection
		/// </summary>
		public readonly static Selection Empty; 

		public Field oField;
		public bool bAll;
		public int start;
		public int end;

        public Selection(Field e, bool all, int st, int en )
		{
			this.oField = e;
			this.bAll = all;
			this.start = st;
			this.end = en;
		}

		public static bool operator != (Selection s1, Selection s2)
		{ 
			if (s1.bAll		!= s2.bAll
				||
				s1.start	!= s2.start
				||
				s1.end		!= s2.end
				||
				s1.oField	!= s2.oField)
				return true;
			else
				return false;

		}

		public static bool operator == (Selection s1, Selection s2)
		{ 
			if (s1.bAll		== s2.bAll
				&&
				s1.start	== s2.start
				&&
				s1.end		== s2.end
				&&
				s1.oField	== s2.oField)
				return true;
			else
				return false;

		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		public override string ToString()
		{
			return base.ToString ();
		}

		public override bool Equals(object obj)
		{
			if (obj is Selection) 
			{
				return (((Selection) obj) == this);
			} 
			else 
			{
				return false;
			}
		}



	}
}
