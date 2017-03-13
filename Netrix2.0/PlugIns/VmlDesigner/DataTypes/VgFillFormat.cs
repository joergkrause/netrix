using System;
using System.ComponentModel;

using Comzept.Genesis.NetRix.VgxDraw;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
    /// <summary>
    /// Specifies the fill value for this shape.
    /// </summary>
    public class VgFillFormat
    {

        private IVgFill native;

        public VgFillFormat(IVgFill fill)
        {
            native = fill;
        }

        #region IVgFill Members

        public Comzept.Genesis.NetRix.VgxDraw.VgSigmaType Method
        {
            get
            {
                return native.method;
            }
            set
            {
                native.method = value;
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

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public VgVector2D Size
        {
            get
            {
                return new VgVector2D(native.size);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public VgVector2D FocusPosition
        {
            get
            {
                return new VgVector2D(native.focusposition);
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

        public void Picture(string PictureFile)
        {
            native.Picture(PictureFile);
        }

        public void Background()
        {
            native.Background();
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public VgGradientColorArray Colors
        {
            get
            {
                return new VgGradientColorArray(native.colors);
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

        public void OneColorGradient(Comzept.Genesis.NetRix.VgxDraw.VgGradientStyle Style, int Variant, double Degree)
        {
            native.OneColorGradient(Style, Variant, Degree);
        }

        public double Opacity2
        {
            get
            {
                return native.opacity2;
            }
            set
            {
                native.opacity2 = value;
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public VgVector2D Position
        {
            get
            {
                return new VgVector2D(native.position);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public VgVector2D Origin
        {
            get
            {
                return new VgVector2D(native.origin);
            }
        }

        public Comzept.Genesis.NetRix.VgxDraw.VgAspectType Aspect
        {
            get
            {
                return native.aspect;
            }
            set
            {
                native.aspect = value;
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public VgVector2D FocusSize
        {
            get
            {
                return new VgVector2D(native.focussize);
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
                native.on = (VgTriState)(((int)value == 1) ? -1 : (int)value);
            }
        }

        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        public System.Drawing.Color Color
        {
            get
            {
                return new VgColor(native.color).Value;
            }
        }

        public FillType Type
        {
            get
            {
                return (FillType)(int)native.Type;
            }
            set
            {
                native.Type = (VgFillType)(int)value;
            }
        }

        public TriState AlignShape
        {
            get
            {
                return (TriState)(int)native.alignshape;
            }
            set
            {
                native.alignshape = (VgTriState)(((int)value == 1) ? -1 : (int)value);
            }
        }

        public double Focus
        {
            get
            {
                return native.focus;
            }
            set
            {
                native.focus = value;
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

        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
             typeof(System.Drawing.Design.UITypeEditor))]
        public System.Drawing.Color Color2
        {
            get
            {
                return new VgColor(native.color2).Value;
            }
        }

        public void Textured(string TextureFile)
        {
            native.Textured(TextureFile);
        }

        public void TwoColorGradient(Comzept.Genesis.NetRix.VgxDraw.VgGradientStyle Style, int Variant)
        {
            native.TwoColorGradient(Style, Variant);
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

        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorAngle),
             typeof(System.Drawing.Design.UITypeEditor))]
        public decimal Angle
        {
            get
            {
                return Convert.ToDecimal(native.angle);
            }
            set
            {
                native.angle = (double)value;
            }
        }

        #endregion

        public override string ToString()
        {
            return "Fill Properties";
        }

    }
}