using System;  
using System.Drawing;  
using System.ComponentModel;  

using GuruComponents.Netrix.ComInterop;
using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.VmlDesigner.DataTypes;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
    /// <summary>
    /// The base class for all predefined shapes.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [ToolboxItem(false)]
    public abstract class PredefinedElement : BaseShapeElement
    {

        # region CSS Properties


        # endregion 

        # region VML Properties

        /// <summary>
        /// URL link if the element is clicked on.
        /// </summary>
        [Browsable(true), Category("Element Behavior"), DefaultValue("")]
        public virtual string Href
        {
            get
            {
                return base.GetStringAttribute("href");
            }
            set
            {
                base.SetStringAttribute("href", value);
            }
        }
        /// <summary>
        /// The target frame for href.
        /// </summary>
        [Browsable(true), Category("Element Behavior"), DefaultValue("")]
        public virtual string Target
        {
            get
            {
                return base.GetStringAttribute("href");
            }
            set
            {
                base.SetStringAttribute("href", value);
            }
        }
        
        /// <summary>
        /// alternate text if element cannot be displayed.
        /// </summary>
        [Browsable(true), Category("Element Behavior"), DefaultValue("")]
        public virtual string Alt
        {
            get
            {
                return base.GetStringAttribute("alt");
            }
            set
            {
                base.SetStringAttribute("alt", value);
            }
        }
        
        /// <summary>
        /// size of coordinate space inside the element
        /// </summary>
        [Browsable(true), Category("Element Layout")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual VgVector2D CoordSize
        {
            get
            {
                object coordObj = base.GetAttribute("coordsize");
				if (coordObj is string)
				{
					return new VgVector2D(coordObj.ToString());
				}
				if (coordObj is IVgVector2D)
				{
					return new VgVector2D((IVgVector2D) base.GetAttribute("coordsize"));
				}
				return null;
            }
            set
            {
                if (value.NativeVector == null)
                {
                    base.SetAttribute("coordsize", "100,100");
                    VgVector2D v2 = CoordSize;
                    v2.x = value.x;
                    v2.y = value.y;
                }
                else
                {
                    base.SetAttribute("coordsize", value.NativeVector);
                }
            }
        }
        /// <summary>
        /// coordinate at top-left corner of element
        /// </summary>
        [Browsable(true), Category("Element Layout")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual VgVector2D CoordOrigin
        {
            get
            {
                return new VgVector2D((IVgVector2D) base.GetAttribute("coordorigin"));
            }
            set
            {
                base.SetAttribute("coordorigin", value.NativeVector);
            }
        }
        /// <summary>
        /// Outline to use for tight text wrapping.
        /// </summary>
        /// <remarks>
        /// In the form "x1,y1,x2,y2,x3,y3…" (same as coords in an AREA). Describes in drawing units around a shape. Used for the tight wrapping of text around an object.
        /// </remarks>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public virtual string WrapCoords
        {
            get
            {
                return base.GetStringAttribute("wrapcoords");
            }
            set
            {
                base.SetStringAttribute("wrapcoords", value);
            }
        }
        /// <summary>
        /// opacity of the shape
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue(1.0F)]
        public virtual float Opacity
        {
            get
            {
                return Convert.ToSingle(base.GetAttribute("opacity"));
            }
            set
            {
                if (value > 1)
                {
                    throw new ArgumentException("Value for opacitiy must between 0 (transparent) and 1 (opaque)");
                }
                base.SetAttribute("opacity", value.ToString());
            }
        }
        /// <summary>
        /// A color value that will be transparent and show anything behind the shape.
        /// </summary>
        [Browsable(true), Category("Element Layout")]
        [TypeConverterAttribute(typeof(UserInterface.TypeConverters.UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UserInterface.TypeEditors.UITypeEditorColor),
			 typeof(System.Drawing.Design.UITypeEditor))]
        public virtual Color ChromaKey
        {
            get
            {
                return base.GetColorAttribute("chromakey");
            }
            set
            {
                base.SetColorAttribute("chromakey", value);
            }
        }
        /// <summary>
        /// Boolean whether to stroke the outline or not
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual VgStroke Stroke
        {
            get
            {
                VgStroke stroke = new VgStroke(((IVgStroke) GetAttribute("stroke")), base.HtmlEditor);
                return stroke;       
            }
        }
        /// <summary>
        /// RGB color to use for the stroke        
        /// </summary>
        [Browsable(true), Category("Element Layout")]
                [TypeConverterAttribute(typeof(UserInterface.TypeConverters.UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UserInterface.TypeEditors.UITypeEditorColor),
			 typeof(System.Drawing.Design.UITypeEditor))]
        public virtual Color StrokeColor
        {
            get
            {
                return base.GetColorAttribute("strokecolor");
            }
            set
            {
                base.SetColorAttribute("strokecolor", value);
            }
        }
        /// <summary>
        /// Weight of the line to use for stroking.
        /// </summary>
        [Browsable(true), Category("Element Layout")]
                [EditorAttribute(
             typeof(UserInterface.TypeEditors.UITypeEditorUnit),
             typeof(System.Drawing.Design.UITypeEditor))]
        public virtual System.Web.UI.WebControls.Unit StrokeWeight
        {
            get
            {
                return base.GetUnitAttribute("strokeweight");
            }
            set
            {
                base.SetUnitAttribute("strokeweight", value);
            }
        }
        /// <summary>
        /// Boolean whether to fill the shape or not
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue(true)]
		[TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual VgFillFormat Fill
        {
            get
            {
                return new VgFillFormat(GetAttribute("fill") as IVgFill);
            }
        }
        /// <summary>
        /// RGB color to use for the fill.
        /// </summary>
        [Browsable(true), Category("Element Layout")]
                [TypeConverterAttribute(typeof(UserInterface.TypeConverters.UITypeConverterColor))]
		[EditorAttribute(
			 typeof(UserInterface.TypeEditors.UITypeEditorColor),
			 typeof(System.Drawing.Design.UITypeEditor))]
        public virtual Color FillColor
        {
            get
            {
                return base.GetColorAttribute("fillcolor");
            }
            set
            {
                base.SetColorAttribute("fillcolor", value);
            }
        }
        /// <summary>
        /// Boolean whether the element is to be printed.
        /// </summary>
        [Browsable(true), Category("Element Behavior"), DefaultValue(true)]
        public virtual bool Print
        {
            get
            {
                return base.GetBooleanAttribute("print");
            }
            set
            {
                base.SetBooleanAttribute("print", value);
            }
        }

        # endregion VML Properties

        /// <summary>
        /// Create new instance of element.
        /// </summary>
        /// <param name="newTag">Tag name</param>
        /// <param name="editor">Editor reference</param>
        protected PredefinedElement(string inheritedShape, IHtmlEditor editor)
            : base(inheritedShape, editor)
        {
        }

        /// <summary>
        /// Create new instance of element.
        /// </summary>
        /// <param name="peer">Peer element name</param>
        /// <param name="editor">Editor reference</param>
        protected PredefinedElement(Interop.IHTMLElement peer, IHtmlEditor editor)
            : base(peer, editor)
        {
        }

    }
}