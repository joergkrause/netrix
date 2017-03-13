using System;  
using System.ComponentModel;  

using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.VmlDesigner.DataTypes;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This sub-element may appear inside a shape or a shapetype to define a shadow effect on a shape. 
	/// </summary>
	/// <remarks>
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class ShadowElement : VmlBaseElement
	{

		public ShadowElement(IHtmlEditor editor) : base("v:shadow", editor)
		{
		}

        internal ShadowElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
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
				return Convert.ToDouble(GetAttribute("opacity"));
			}
			set
			{
				SetAttribute("opacitiy", value);
			}
		}

		public bool Obscured
		{
			get
			{
				return GetBooleanAttribute("obscured");
			}
			set
			{
				SetBooleanAttribute("obscured", value);
			}
		}

		[Browsable(false)]
		public object Application
		{
			get
			{
				// TODO:  Add ShadowElement.Application getter implementation
				return null;
			}
		}

		public VgVector2D Origin
		{
			get
			{
				return new VgVector2D(GetAttribute("origin") as IVgVector2D);
			}
		}

		public VgSkewMatrix Matrix
		{
			get
			{
				return new VgSkewMatrix(GetAttribute("matrix") as IVgSkewMatrix);
			}
		}

		public bool On
		{
			get
			{
				return GetBooleanAttribute("on");
			}
			set
			{
				SetBooleanAttribute("on", value);
			}
		}

		public System.Drawing.Color Color
		{
			get
			{
				return GetColorAttribute("color");
			}
		}

		public VgVector2D Offset2
		{
			get
			{
				return new VgVector2D(GetAttribute("offset2") as IVgVector2D);
			}
		}

		public VgShadowType Type
		{
			get
			{
				return (VgShadowType) GetEnumAttribute("type", VgShadowType.vgShadowSingle);
			}
			set
			{
				SetEnumAttribute("type", value, VgShadowType.vgShadowSingle);
			}
		}

		public System.Drawing.Color Color2
		{
			get
			{
				return GetColorAttribute("color2");
			}
		}

		public string Template
		{
			get
			{
				return GetStringAttribute("template");
			}
			set
			{
				SetStringAttribute("template", value);
			}
		}

		public VgSkewOffset Offset
		{
			get
			{
				return new VgSkewOffset(GetAttribute("offset") as IVgSkewOffset);
			}
		}

		#endregion
	}
}
