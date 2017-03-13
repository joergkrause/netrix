using System;  
using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This element is used to draw an oval defined by the CSS2 content width and height.
	/// </summary>
	[ToolboxItem(false)]
    public class OvalElement : PredefinedElement
	{
		public OvalElement(IHtmlEditor editor) : base("v:oval", editor)
		{
		}

        protected OvalElement(string parent, IHtmlEditor editor) : base(parent, editor)
        {
        }

        internal OvalElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
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
