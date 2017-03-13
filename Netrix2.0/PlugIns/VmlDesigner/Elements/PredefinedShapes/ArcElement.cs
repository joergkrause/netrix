using System;  
using System.ComponentModel;  
using GuruComponents.Netrix.ComInterop;

using TE=GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This element is used to draw an arc defined as a segment of an oval.
	/// </summary>
	/// <remarks>
    /// The content width and height define the width and height of that oval.  The arc is defined by the intersection of 
    /// the oval with the start and end radius vectors given by the angles.   The angles are calculated on the basis of 
    /// a circle (width equal to height) which is then scaled anisotropically to the desired width and height.
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class ArcElement : OvalElement
	{

        /// <summary>
        /// Defines the starting point of the arc.
        /// </summary>
        /// <remarks>
        /// The arc is defined as a stroke drawn along an oval bounded by the Style attributes of a shape. 
        /// The start of the stroke is defined by an angle measured from straight up (12 o'clock) clockwise. 
        /// The default value is 0 degrees.
        /// </remarks>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(0)]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorAngle),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]
        public decimal StartAngle
        {
            get
            {
                return Convert.ToDecimal( base.GetIntegerAttribute("startAngle", -1) );
            }

            set
            {
                base.SetIntegerAttribute("startAngle", (int)value, -1);
            }
        }

        /// <summary>
        /// Defines the end point of the arc.
        /// </summary>
        /// <remarks>
        /// The arc is defined as a stroke drawn along an oval bounded by the Style attributes of a shape. 
        /// The start of the stroke is defined by an angle measured from straight up (12 o'clock) clockwise. 
        /// The default value is 0 degrees.
        /// </remarks>
        [CategoryAttribute("Element Layout")]
        [DefaultValueAttribute(0)]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorAngle),
             typeof(System.Drawing.Design.UITypeEditor))]
        [TE.DisplayName()]
        public decimal EndAngle
        {
            get
            {
                return Convert.ToDecimal(base.GetIntegerAttribute("endAngle", -1));
            }

            set
            {
                base.SetIntegerAttribute("endAngle", (int)value, -1);
            }
        }


		public ArcElement(IHtmlEditor editor) : base("v:arc", editor)
		{
		}

        internal ArcElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
