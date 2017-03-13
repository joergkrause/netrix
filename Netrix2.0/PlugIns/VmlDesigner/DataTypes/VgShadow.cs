using System;  
using System.ComponentModel;  

using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.VmlDesigner.DataTypes;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This sub-element may appear inside a shape or a shapetype to define a shadow effect on a shape. 
	/// </summary>
	/// <remarks>
	/// </remarks>
	public sealed class VgShadow
	{

		private IVgShadow native;

        internal VgShadow(IVgShadow native)
        {
			this.native  = native;
		}

		private IVgShadow GetBaseElement()
		{
			return native;
		}

		#region IVgShadow Members

		[Browsable(false)]
		public int Creator
		{
			get
			{
				return ((IVgShadow) GetBaseElement()).Creator;
			}
		}

		[Browsable(false)]
		public object parentShape
		{
			get
			{
				return ((IVgShadow) GetBaseElement()).parentShape;
			}
		}

		public double Opacity
		{
			get
			{
				return Convert.ToDouble(native.opacity);
			}
			set
			{
				native.opacity = value;
			}
		}

		public TriState Obscured
		{
			get
			{
				return (TriState) native.obscured;
			}
			set
			{
				native.obscured = (VgTriState)(((int)value == 1) ? -1 : (int)value);;
			}
		}

		[Browsable(false)]
		public object Application
		{
			get
			{
				return native.Application;
			}
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public VgVector2D Origin
		{
			get
			{
				return new VgVector2D(native.origin as IVgVector2D);
			}
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public VgSkewMatrix Matrix
		{
			get
			{
				return new VgSkewMatrix(native.matrix as IVgSkewMatrix);
			}
		}

		public TriState On
		{
			get
			{
				return (TriState) native.on;
			}
			set
			{
				native.on = (VgTriState)(((int)value == 1) ? -1 : (int)value);;
			}
		}

		public System.Drawing.Color Color
		{
			get
			{
				return new VgColor(native.color).Value;
			}
			set
			{
				new VgColor(native.color).Value = value;
			}
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public VgVector2D Offset2
		{
			get
			{
				return new VgVector2D(native.offset2 as IVgVector2D);
			}
		}

		public Comzept.Genesis.NetRix.VgxDraw.VgShadowType Type
		{
			get
			{
				return (VgShadowType) native.Type;
			}
			set
			{
				native.Type = value;
			}
		}

		public System.Drawing.Color Color2
		{
			get
			{
				return new VgColor(native.color2).Value;
			}
			set
			{
				new VgColor(native.color2).Value = value;
			}
		}

		public string Template
		{
			get
			{
				return native.template;
			}
			set
			{
				native.template = value;
			}
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public VgSkewOffset Offset
		{
			get
			{
				return new VgSkewOffset(native.offset as IVgSkewOffset);
			}
		}

		#endregion

		public override string ToString()
		{
			return "Shadow Properties";
		}

	}
}
