using System;  
using System.ComponentModel;  

using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements {
    /// <summary>
    /// This is the top-level element used to describe a shape.
    /// </summary>
    /// <remarks>
    /// This element may appear by itself or within a &lt;group&gt; element. If a &lt;shapetype&gt; is referenced using 
    /// the type= attribute, any attributes specified in the shape will override those found in the shapetype.
    /// <para>
    /// The path definition is described in more detail below.  Path parameterization allows one canonical 
    /// path to describe a range of shapes which differ only in geometric proportions (for example, ring shapes 
    /// where the ratio of the inner to the outer circle diameter varies).
    /// </para>
    /// </remarks>
    [ToolboxItem(false)]
    public class ShapeElement : CommonShapeElement {

        public ShapeElement(IHtmlEditor editor) : base("v:shape", editor) {
        }

        protected ShapeElement(string inheritedShape, IHtmlEditor editor) : base(inheritedShape, editor) {
        }

        internal ShapeElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor) {
        }

		
		// TODO: Add editor which shows the scanned shapetype elements from page as drop list
		/// <summary>
		/// A reference to a shapetype id that describes the standard path, fill and stroke properties of a shape.
		/// </summary>
		/// <remarks>
		/// Properties specified in the shape will override the shapetype properties.
		/// </remarks>
		public string TypeReference 
		{
			get 
			{
				object objType = GetAttribute("type");
				return ( objType == null ) ? "" : objType.ToString();
			}
			set
			{
				SetStringAttribute("type", value);
			}
		}

		
		/// <summary>
		/// A reference to a shapetype id that describes the standard path, fill and stroke properties of a shape.
		/// </summary>
		/// <remarks>
		/// Properties specified in the shape will override the shapetype properties.
		/// </remarks>
		[Browsable(true), TypeConverter(typeof(ExpandableObjectConverter))]
		public ShapetypeElement ShapeType 
		{
			get 
			{
				if (TypeReference.Length == 0)
				{
					return null;
				}
				else
				{
					string type =  TypeReference.Substring(1);
					IElement e = base.HtmlEditor.GetElementById(type);
					if (e is ShapetypeElement)
					{
						return (ShapetypeElement) e;
					} 
					else 
					{
						return null;
					}
				}
			}
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
