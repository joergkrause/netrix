using System;  
using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This element is used to draw a simple rectangle.  
	/// </summary>
	/// <remarks>
	/// The rectangle is defined by the content width specified in the CSS2 properties.
	/// </remarks>
	[ToolboxItem(false)]
    public class RectElement : PredefinedElement
	{


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
                    return Convert.ToDecimal(VmlDesignerBehavior.RotationParse(base.GetStyleAttribute("rotation")));
                }
            }
            set { base.SetStyleAttribute("rotation", value.ToString());
                if (value == 0){
                    base.RemoveStyleAttribute("rotation");
                }
                
            }
        }

		public RectElement(IHtmlEditor editor) : base("v:rect", editor)
		{
		}        
        
        protected RectElement(string parentTag, IHtmlEditor editor) : base(parentTag, editor)
        {
        }

        internal RectElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
