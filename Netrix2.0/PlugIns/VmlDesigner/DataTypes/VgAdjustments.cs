using System;
using System.ComponentModel;

using Comzept.Genesis.NetRix.VgxDraw;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
	/// <summary>
	/// A comma-delimited list of numbers that are the parameters for the guide formulas that define the path of the shape.
	/// </summary>
    /// <remarks>
    /// Values may be omitted to allow for using defaults. There can be up to 8 adjustment values.
    /// </remarks>
	public class VgAdjustments
	{

		IVgAdjustments native;

		internal VgAdjustments(IVgAdjustments adj)
		{
			native = adj;
		}

		#region IVgAdjustments Members

		[Browsable(false)]
		public int Creator
		{
			get
			{
				return native.Creator;
			}
		}

		[Browsable(false)]
		public object ParentShape
		{
			get
			{
				return native.parentShape;
			}
		}

		public int this[int Index]
		{
			get
			{
				return native[Index];
			}
			set
			{
				native[Index] = value;
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
				return native.Application;
			}
		}

		public TriState Exists(int Index)
		{
			return (TriState) native.get_exists(Index);
		}

        /// <summary>
        /// A comma-delimited list of numbers that are the parameters for the guide formulas that define the path of the shape.
        /// </summary>
		[Browsable(true), RefreshProperties(RefreshProperties.Repaint)]
		public string Value
		{
			get
			{
				return native.value;
			}
			set
			{
				native.value = value;
			}
		}

        /// <summary>
        /// The number of values in the list, after parsing the string in <see cref="Value"/>.
        /// </summary>
		public int Length
		{
			get
			{
				return native.length;
			}
		}

		#endregion

		public override string ToString()
		{
			return "Adjustment Properties" ;
		}


	}
}
