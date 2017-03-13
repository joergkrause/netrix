using System;  
using System.Drawing;
using System.ComponentModel;  

using Comzept.Genesis.NetRix.ComInterop;
using Comzept.Genesis.NetRix.VmlDesigner.Elements;
using Comzept.Genesis.NetRix;

namespace Comzept.Genesis.VmlDesigner.Elements
{
	/// <summary>
	/// Defines an extrusion for a shape.
	/// </summary>
    public class ExtrusionElement : BaseShapeElement
    {

//        /// <summary>
//        /// Determines whether the center of rotation will be the geometric center of the extrusion.
//        /// </summary>
//        public bool AutoRotationCenter  
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//
//        /// <summary>
//        /// Defines the amount of backward extrusion. 
//        /// </summary>
//        public int BackDepth 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Specifies the amount of brightness of a scene. 
//        /// </summary>
//        [DefaultValue(20000)]
//        public int Brightness 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the color of the extrusion faces. 
//        /// </summary>
//        public System.Drawing.Color Color 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//
//        /// <summary>
//        /// Determines the mode of extrusion color. 
//        /// </summary>
//        public VgxDraw.Vg3DColorMode ColorMode 
//        {
//            get
//            {
//                
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the amount of diffusion of reflected light from an extruded shape. 
//        /// </summary>
//        public bool Diffusity 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the apparent bevel of the extrusion edges.
//        /// </summary>
//        public bool Edge  
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the default extrusion behavior for graphical editors. 
//        /// </summary>
//        public bool Ext 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the number of facets used to describe curved surfaces of an extrusion. 
//        /// </summary>
//        public bool Facet 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the amount of forward extrusion. 
//        /// </summary>
//        public bool ForeDepth 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Determines whether the front face of the extrusion will respond to changes in the lighting. 
//        /// </summary>
//        public bool LightFace 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Determines whether the primary light source will be harsh. 
//        /// </summary>
//        public bool LightHarsh 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Determines whether the secondary light source will be harsh. 
//        /// </summary>
//        public bool LightHarsh2
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the intensity of the primary light source for the scene. 
//        /// </summary>
//        public bool LightLevel 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the intensity of the secondary light source for the scene. 
//        /// </summary>
//        public bool LightLevel2 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Specifies the position of the primary light in a scene. 
//        /// </summary>
//        public bool LightPosition 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Specifies the position of the secondary light in a scene. 
//        /// </summary>
//        public bool LightPosition2 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Determines whether the rotation of the extruded object is specified by the RotationAngle attribute. 
//        /// </summary>
//        public bool LockRotationCenter 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Determines whether the surface of the extruded shape will resemble metal. 
//        /// </summary>
//        public bool Metal 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Determines whether an extrusion will be displayed. 
//        /// </summary>
//        public bool On 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Specifies the vector around which a shape will be rotated. 
//        /// </summary>
//        public bool Orientation 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the angle that an extrusion rotates around the orientation. 
//        /// </summary>
//        public bool OrientationAngle 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Specifies the plane that is at right angles to the extrusion. 
//        /// </summary>
//        public bool Plane 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the rendering mode of the extrusion. 
//        /// </summary>
//        public bool Render 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Specifies the rotation of the object about the x- and y-axes. 
//        /// </summary>
//        public bool RotationAngle 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Specifies the center of rotation for a shape. 
//        /// </summary>
//        public bool RotationCenter 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the concentration of reflected light of an extrusion surface. 
//        /// </summary>
//        public bool Shininess 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the amount of skew of an extrusion. 
//        /// </summary>
//        public bool SkewAmt 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the angle of skew of an extrusion. 
//        /// </summary>
//        public bool SkewAngle 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the specularity of an extruded shape. 
//        /// </summary>
//        public bool Specularity 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the way that the shape is extruded. 
//        /// </summary>
//        public bool Type 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the viewpoint of the observer. 
//        /// </summary>
//        public bool Viewpoint 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }
//        /// <summary>
//        /// Defines the origin of the viewpoint within the bounding box of the shape. 
//        /// </summary>
//        public bool ViewpointOrigin 
//        {
//            get
//            {
//            }
//            set
//            {
//            }
//        }


        internal ExtrusionElement(Interop.IHTMLElement peer, Interop.IHTMLElement parent, IHtmlEditor editor)
            : this(peer, editor)
		{
            Interop.IHTMLDOMNode parentNode = (Interop.IHTMLDOMNode) parent;
            Interop.IHTMLElement extrusion = base.GetBaseElement();
            if (extrusion != null)
            {
                parentNode.appendChild(extrusion as Interop.IHTMLDOMNode);
            }
		}

        internal ExtrusionElement(Interop.IHTMLElement peer, IHtmlEditor editor)
            : base(peer, editor)
        {
        }

	}
}
