using System;
using System.Collections;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner.Edx
{
	/// <summary>
	/// Teeny object for saving location of cursor in signature/offset format.
	/// </summary>
	public struct CursorSave
	{

		/// <summary>
		/// Empty CursorSave
		/// </summary>
		public readonly static CursorSave Empty; 

		public ArrayList signature;
		public int offset;
		public int instance;

		public CursorSave(ArrayList sig, int off, int inst )
		{
			this.signature = sig;
			this.offset = off;
			this.instance = inst;
		}

		public static bool operator !=(CursorSave c1, CursorSave c2)
		{
			if (c1.signature != c2.signature
				||
				c1.offset != c2.offset
				||
				c1.instance != c2.instance)
				return true;
			else
				return false;
		}

		public static bool operator ==(CursorSave c1, CursorSave c2)
		{
			if (c1.signature == c2.signature
				&&
				c1.offset == c2.offset
				&&
				c1.instance == c2.instance)
				return true;
			else
				return false;
		}

		public override int GetHashCode()
		{
			return signature.GetHashCode () + offset + instance;
		}

		public override bool Equals(object obj)
		{
			if (obj is CursorSave)
				return this == (CursorSave)obj;
			else
				return false;
		}


	}
}
