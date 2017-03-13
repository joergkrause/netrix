using System;  
using System.ComponentModel;  
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
    /// This element is used to draw a rectangle with rounded corners.
	/// </summary>
	[ToolboxItem(false)]
    public sealed class RoundRectElement : PredefinedElement
	{

        /// <summary>
        /// Defines rounded corners as a percentage of half the smaller dimension of the rectangle.
        /// </summary>
        /// <remarks>
        /// 0.0 (0%) – square corners, 1.0 (100%) - smaller dimension forms a semi-circle.
        /// </remarks>
        [Browsable(true), Category("Element Layout"), DefaultValue(0.2)]
        public float ArcSize
        {
            get
            {
                return Convert.ToSingle(base.GetAttribute("arcsize"));
            }
            set
            {
                if (value > 1) {
                    throw new ArgumentException("Values higher than 1 are not allowed in that context. The provided value was " + value.ToString() + ".");
                }else{
                    base.SetAttribute("arcsize", value);
                }
            }
        }

        /// <summary>
        /// Create new instance of element.
        /// </summary>
        /// <param name="editor">Editor reference</param>
        public RoundRectElement(IHtmlEditor editor)
            : base("v:roundrect", editor)
		{
		}

        internal RoundRectElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }


        [Browsable(true), DefaultValue(0)]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorAngle),
             typeof(System.Drawing.Design.UITypeEditor))]
        public decimal Angle {
            get {
                if (base.GetStyleAttribute("rotation").Equals(String.Empty)){
                    return 0;
                }
                else {
                    return Convert.ToDecimal(base.GetStyleAttribute("rotation"));
                }
            }
            set { base.SetStyleAttribute("rotation", value.ToString());
                if (value == 0){
                    base.RemoveStyleAttribute("rotation");
                }
                
            }
        }

	}
}
