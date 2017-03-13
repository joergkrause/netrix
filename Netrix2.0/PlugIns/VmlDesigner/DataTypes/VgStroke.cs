using System;
using System.ComponentModel;
using GuruComponents.Netrix.VmlDesigner.Elements;
using GuruComponents.Netrix.ComInterop;
using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
	/// <summary>
	/// Zusammenfassung für VgStroke.
	/// </summary>
    public class VgStroke
    {

        IVgStroke native;
        IHtmlEditor htmlEditor;

        public VgStroke(IVgStroke native, IHtmlEditor editor)
        {
            this.htmlEditor = editor;
            this.native = native;
        }

        #region IVgStroke Member

        public Comzept.Genesis.NetRix.VgxDraw.VgLineStyle LineStyle
        {
            get
            {
                return native.linestyle;
            }
            set
            {
                native.linestyle = value;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.VgArrowheadStyle EndArrow
        {
            get
            {
                return native.endarrow;
            }
            set
            {
                native.endarrow = value;
            }
        }

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
				if (native.parentShape == null)
				{
					return null;
				} 
				else 
				{
					return new ShapeElement(((IVgShape) native.parentShape) as Interop.IHTMLElement, htmlEditor);
				}
            }
        }

        public double Opacity
        {
            get
            {
                return native.opacity;
            }
            set
            {
                native.opacity = value;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.VgArrowheadWidth StartArrowWidth
        {
            get
            {
                return native.startarrowwidth;
            }
            set
            {
                native.startarrowwidth = value;
            }
        }

        public double MiterLimit
        {
            get
            {
                return native.miterlimit;
            }
            set
            {
                native.miterlimit = value;
            }
        }

        [Browsable(true)]
		[TypeConverter(typeof(ExpandableObjectConverter))]
        public VgVector2D ImageSize
        {
            get
            {
                return new VgVector2D(native.imagesize);
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

        public Comzept.Genesis.NetRix.VgxDraw.VgArrowheadWidth EndArrowWidth
        {
            get
            {
                return native.endarrowwidth;
            }
            set
            {
                native.endarrowwidth = value;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.VgArrowheadLength EndArrowLength
        {
            get
            {
                return native.endarrowlength;
            }
            set
            {
                native.endarrowlength = value;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.VgAspectType ImageAspect
        {
            get
            {
                return native.imageaspect;
            }
            set
            {
                native.imageaspect = value;
            }
        }

        public double Weight
        {
            get
            {
                return native.weight;
            }
            set
            {
                native.weight = value;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.VgLineEndCapStyle EndCap
        {
            get
            {
                return native.endcap;
            }
            set
            {
                native.endcap = value;
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

        [TypeConverter(typeof(ExpandableObjectConverter))]
		public VgColor Color
        {
            get
            {
                return new VgColor(native.color);
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.VgArrowheadStyle StartArrow
        {
            get
            {
                return native.startarrow;
            }
            set
            {
                native.startarrow = value;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.VgLineFillType FillType
        {
            get
            {
                return native.filltype;
            }
            set
            {
                native.filltype = value;
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
		public VgDashStyle DashStyle
        {
            get
            {
                return new VgDashStyle(native.dashstyle);
            }
        }

        public TriState ImageAlignShape
        {
            get
            {
                return (TriState) native.imagealignshape;
            }
            set
            {
                native.imagealignshape = (VgTriState)(((int)value == 1) ? -1 : (int)value);;
            }
        }

        public string Src
        {
            get
            {
                return native.src;
            }
            set
            {
                native.src = value;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.VgLineJoinStyle JoinStyle
        {
            get
            {
                return native.joinstyle;
            }
            set
            {
                native.joinstyle = value;
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.VgArrowheadLength StartArrowLength
        {
            get
            {
                return native.startarrowlength;
            }
            set
            {
                native.startarrowlength = value;
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
		public VgColor Color2
        {
            get
            {
                return new VgColor(native.color2);
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

        #endregion

		public override string ToString()
		{
			return "Stroke Properties";
		}


    }
}
