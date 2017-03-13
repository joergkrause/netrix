using System;  
using System.Drawing;
using System.ComponentModel;  

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.VmlDesigner.DataTypes;
using Comzept.Genesis.NetRix.VgxDraw;
using System.Web.UI.WebControls;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// Defines an extrusion for a shape.
	/// </summary>
    [ToolboxItem(false)]
    public class ExtrusionElement : BaseShapeElement
    {

        /// <summary>
        /// Determines whether the center of rotation will be the geometric center of the extrusion.
        /// </summary>
        public TriState AutoRotationCenter  
        {
            get
            {
                return (TriState) base.GetEnumAttribute("AutoRotationCenter", TriState.False);
            }
            set
            {
                SetEnumAttribute("AutoRotationCenter", (VgTriState)(int)value, VgTriState.vgTriStateFalse);
            }
        }

        ///// <summary>
        ///// Defines the amount of backward extrusion. 
        ///// </summary>
        //public int BackDepth 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        /// <summary>
        /// Specifies the amount of brightness of a scene. 
        /// </summary>
        [DefaultValue(20000)]
        public int Brightness 
        {
            get
            {
                return base.GetIntegerAttribute("brightness", 20000);
            }
            set
            {
                base.SetIntegerAttribute("brightness", value, 20000);
            }
        }
        /// <summary>
        /// Defines the color of the extrusion faces. 
        /// </summary>
        [TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
		[EditorAttribute(
			 typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
			 typeof(System.Drawing.Design.UITypeEditor))]
        public System.Drawing.Color Color 
        {
            get
            {
                return base.GetColorAttribute("color");
            }
            set
            {
                base.SetColorAttribute("color", value);
            }
        }

        /// <summary>
        /// Determines the mode of extrusion color. 
        /// </summary>
        public ThreeDColorMode ColorMode 
        {
            get
            {
                return (ThreeDColorMode) base.GetEnumAttribute("colormode", Vg3DColorMode.vg3DColorModeAuto);
            }
            set
            {
                base.SetEnumAttribute("colormode", (Vg3DColorMode) (int) value, Vg3DColorMode.vg3DColorModeAuto);
            }
        }
        /// <summary>
        /// Defines the amount of diffusion of reflected light from an extruded shape. 
        /// </summary>
        public bool Diffusity 
        {
            get
            {
                return base.GetBooleanAttribute("diffusity");
            }
            set
            {
                base.SetBooleanAttribute("diffusity", value);
            }
        }
        /// <summary>
        /// Defines the apparent bevel of the extrusion edges.
        /// </summary>
        /// <remarks>
        /// Sets the size of a simulated rounded beveled edge. Ranges from 0 to 32767 in floating point pts. Default is "1pt".
        /// </remarks>
        [DefaultValue(typeof(Unit), "1pt")]
        public Unit Edge  
        {
            get
            {
                return base.GetUnitAttribute("edge");
            }
            set
            {
                if (value.Value > 32767D || value.Value < 0D) 
                    throw new ArgumentException("Edge requires a value between 0 and 32767 floating point pts.");
                base.SetUnitAttribute("edge", value);
            }
        }
        /// <summary>
        /// Defines the default extrusion behavior for graphical editors. 
        /// </summary>
        public bool Ext 
        {
            get
            {
                return base.GetBooleanAttribute("ext");
            }
            set
            {
                base.SetBooleanAttribute("ext", value);
            }
        }

        ///// <summary>
        ///// Defines the number of facets used to describe curved surfaces of an extrusion. 
        ///// </summary>
        //public bool Facet 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the amount of forward extrusion. 
        ///// </summary>
        //public bool ForeDepth 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Determines whether the front face of the extrusion will respond to changes in the lighting. 
        ///// </summary>
        //public bool LightFace 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Determines whether the primary light source will be harsh. 
        ///// </summary>
        //public bool LightHarsh 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Determines whether the secondary light source will be harsh. 
        ///// </summary>
        //public bool LightHarsh2
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the intensity of the primary light source for the scene. 
        ///// </summary>
        //public bool LightLevel 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the intensity of the secondary light source for the scene. 
        ///// </summary>
        //public bool LightLevel2 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Specifies the position of the primary light in a scene. 
        ///// </summary>
        //public bool LightPosition 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Specifies the position of the secondary light in a scene. 
        ///// </summary>
        //public bool LightPosition2 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Determines whether the rotation of the extruded object is specified by the RotationAngle attribute. 
        ///// </summary>
        //public bool LockRotationCenter 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Determines whether the surface of the extruded shape will resemble metal. 
        ///// </summary>
        //public bool Metal 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Determines whether an extrusion will be displayed. 
        ///// </summary>
        //public bool On 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Specifies the vector around which a shape will be rotated. 
        ///// </summary>
        //public bool Orientation 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the angle that an extrusion rotates around the orientation. 
        ///// </summary>
        //public bool OrientationAngle 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Specifies the plane that is at right angles to the extrusion. 
        ///// </summary>
        //public bool Plane 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the rendering mode of the extrusion. 
        ///// </summary>
        //public bool Render 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Specifies the rotation of the object about the x- and y-axes. 
        ///// </summary>
        //public bool RotationAngle 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Specifies the center of rotation for a shape. 
        ///// </summary>
        //public bool RotationCenter 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the concentration of reflected light of an extrusion surface. 
        ///// </summary>
        //public bool Shininess 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the amount of skew of an extrusion. 
        ///// </summary>
        //public bool SkewAmt 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the angle of skew of an extrusion. 
        ///// </summary>
        //public bool SkewAngle 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the specularity of an extruded shape. 
        ///// </summary>
        //public bool Specularity 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the way that the shape is extruded. 
        ///// </summary>
        //public bool Type 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the viewpoint of the observer. 
        ///// </summary>
        //public bool Viewpoint 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}
        ///// <summary>
        ///// Defines the origin of the viewpoint within the bounding box of the shape. 
        ///// </summary>
        //public bool ViewpointOrigin 
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}

        public ExtrusionElement(VmlBaseElement peer, VmlBaseElement parent, IHtmlEditor editor)
            : this(peer.GetBaseElement(), parent.GetBaseElement(), editor)
        {

        }

        
        internal ExtrusionElement(Interop.IHTMLElement peer, Interop.IHTMLElement parent, IHtmlEditor editor) : this(peer, editor)
		{
            Interop.IHTMLDOMNode parentNode = (Interop.IHTMLDOMNode) parent;
            Interop.IHTMLElement extrusion = base.GetBaseElement();
            if (extrusion != null)
            {
                parentNode.appendChild(extrusion as Interop.IHTMLDOMNode);
            }
		}

        internal ExtrusionElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
