using System;
using System.ComponentModel;
using System.Drawing;

namespace GuruComponents.Netrix.WebEditing.HighLighting
{
	/// <summary>
	/// This class supports the propertygrid at design time is not exposed to being used from user code.
	/// </summary>
	internal class MyExpandableObjectConverter : ExpandableObjectConverter
	{
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            Color color = Color.Empty;
            ColorType type = ColorType.Auto;
            if (propertyValues["Type"] != null)
            {
                type = (ColorType) Enum.Parse(typeof(ColorType), propertyValues["Type"].ToString());
            }
            if (propertyValues["ColorValue"] != null && !propertyValues["Type"].Equals("Empty") && !propertyValues["Type"].Equals("Transparent") && !propertyValues["Type"].Equals("Auto") && !propertyValues["Type"].Equals("Inherit"))
            {
                color = (Color) propertyValues["ColorValue"];
            }
            HighlightColor hlc = new HighlightColor(color, type);
            return hlc;
        }

	}
}
