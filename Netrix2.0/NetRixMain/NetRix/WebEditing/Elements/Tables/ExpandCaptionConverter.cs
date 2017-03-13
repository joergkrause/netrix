using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using System;
using System.Globalization;
using System.ComponentModel;
using System.Drawing;

namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// Supports the PropertyGrid.
    /// </summary>
    /// <remarks>
    /// This Converter class is used to show the caption properties within the table element in the propertygrid
    /// as expandable section (+ sign to open) instead of presenting them as stand alone property only.
    /// The convert from and to methods are able to create or remove the caption element directly.
    /// </remarks>
    internal class ExpandCaptionConverter : System.ComponentModel.ExpandableObjectConverter
    {

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] filter)
        {
            return TypeDescriptor.GetProperties(value, filter);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context) 
        {
            return base.GetPropertiesSupported(context);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(CaptionElement))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type t) 
        {
            if (t == typeof(string)) 
            {
                return true;
            }
            return base.CanConvertFrom(context, t);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value) 
        {
//            if (value is string) 
//            {
//                try 
//                {
//                    string s = (string) value;
//                    TableElement tE = (TableElement) context.Instance;
//                    CaptionElement cE;
//                    if (tE.Caption == null)
//                    {
//                        cE = (CaptionElement) GenericElementFactory.CreateElement((Interop.IHTMLElement) tE.BaseTable.createCaption(), base.htmlEditor);
//                    } 
//                    else 
//                    {
//                        cE = tE.Caption;
//                    }
//                    cE.InnerText = s;
//                    return cE;
//                }
//                catch 
//                {
//                }
//            }
            return base.ConvertFrom(context, info, value);
        }
                                 
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType) 
        {
//            if (destType == typeof(string) && value is CaptionElement) 
//            {
//                CaptionElement cE = (CaptionElement) value;
//                if (cE.Content == null)
//                {
//                    return String.Empty;
//                } 
//                else 
//                {
//                    return cE.Content;
//                }
//            }
            return base.ConvertTo(context, culture, value, destType);
        }

    }

}