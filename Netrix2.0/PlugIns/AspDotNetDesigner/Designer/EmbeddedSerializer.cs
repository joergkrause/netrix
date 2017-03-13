using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using AttributeCollection=System.Web.UI.AttributeCollection;
using System.Collections.Generic;

namespace GuruComponents.Netrix.AspDotNetDesigner
{
    /// <summary>
    /// Serializes an control back into native element within document.
    /// </summary>
    internal class EmbeddedSerializer
    {

        private const char FILTER_SEPARATOR_CHAR = ':';
        private const char PERSIST_CHAR = '-';
        private const char OM_CHAR = '.';

        private enum BindingType
        {
            None,
            Data,
            Expression
        }

        private class ReferenceKeyComparer : IEqualityComparer
        {
            // Methods
            static ReferenceKeyComparer()
            {
                Default = new ReferenceKeyComparer();
            }

            bool IEqualityComparer.Equals(object x, object y)
            {
                return ReferenceEquals(x, y);
            }
            int IEqualityComparer.GetHashCode(object obj)
            {
                return obj.GetHashCode();
            }

            // Fields
            internal static readonly ReferenceKeyComparer Default;
        }



        private static bool CanSerializeAsInnerDefaultString(string filter, string name, Type type, ObjectPersistData persistData, PersistenceMode mode, DataBindingCollection dataBindings, ExpressionBindingCollection expressions)
        {
            if (((((type == typeof(string)) && (filter.Length == 0)) && ((mode == PersistenceMode.InnerDefaultProperty) || (mode == PersistenceMode.EncodedInnerDefaultProperty))) && ((dataBindings == null) || (dataBindings[name] == null))) && ((expressions == null) || (expressions[name] == null)))
            {
                if (persistData == null)
                {
                    return true;
                }
                ICollection collection1 = persistData.GetPropertyAllFilters(name);
                if (collection1.Count == 0)
                {
                    return true;
                }
                if (collection1.Count == 1)
                {
                    foreach (PropertyEntry entry1 in collection1)
                    {
                        if ((entry1.Filter.Length == 0) && (entry1 is ComplexPropertyEntry))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static string ConvertObjectModelToPersistName(string objectModelName)
        {
            return objectModelName.Replace(OM_CHAR, PERSIST_CHAR);
        }

        private static string ConvertPersistToObjectModelName(string persistName)
        {
            return persistName.Replace(PERSIST_CHAR, OM_CHAR);
        }

        public static void SerializeControl(Control control, Interop.IHTMLElement element)
        {
            ISite controlSite = control.Site;
            if (controlSite == null)
            {
                IComponent page = control.Page;
                if (page != null)
                {
                    controlSite = page.Site;
                }
            }
            IDesignerHost designerHost = null;
            if (controlSite != null)
            {
                designerHost = (IDesignerHost)controlSite.GetService(typeof(IDesignerHost));
            }
            SerializeControl(control, designerHost, element);
        }

        public static void SerializeControl(Control control, IDesignerHost host, Interop.IHTMLElement element)
        {
            SerializeControl(control, host, element, GetCurrentFilter(host));
        }

        public static void SerializeInnerContents(Control control, IDesignerHost host, Interop.IHTMLElement element)
        {
            ObjectPersistData persistData = null;
            IControlBuilderAccessor builderAccessor = control;
            if (builderAccessor.ControlBuilder != null)
            {
                persistData = builderAccessor.ControlBuilder.GetObjectPersistData();
            }
            SerializeInnerContents(control, host, persistData, element, GetCurrentFilter(host));            
        }

        internal static void SerializeInnerContents(Control control, IDesignerHost host, ObjectPersistData persistData, Interop.IHTMLElement element, string filter)
        {
            PersistChildrenAttribute persistChildrenAttribute = (PersistChildrenAttribute)TypeDescriptor.GetAttributes(control)[typeof(PersistChildrenAttribute)];
            ParseChildrenAttribute parseChildrenAttribute = (ParseChildrenAttribute)TypeDescriptor.GetAttributes(control)[typeof(ParseChildrenAttribute)];
            if (persistChildrenAttribute.Persist || (!parseChildrenAttribute.ChildrenAsProperties && control.HasControls()))
            {
                for (int i = 0; i < control.Controls.Count; i++)
                {
                    SerializeControl(control.Controls[i], host, element, string.Empty);
                }
            }
            else
            {
                SerializeInnerProperties(control, host, persistData, element, filter);
            }
        }

        public static void SerializeInnerProperties(object obj, IDesignerHost host, Interop.IHTMLElement element)
        {
            ObjectPersistData data1 = null;
            IControlBuilderAccessor accessor1 = (IControlBuilderAccessor)obj;
            if (accessor1.ControlBuilder != null)
            {
                data1 = accessor1.ControlBuilder.GetObjectPersistData();
            }
            SerializeInnerProperties(obj, host, data1, element, GetCurrentFilter(host));
        }

        private static void SerializeInnerProperties(object obj, IDesignerHost host, ObjectPersistData persistData, Interop.IHTMLElement element, string filter)
        {
            List<PropertyDescriptor> l = new List<PropertyDescriptor>();
            List<string> typeHierarchy = new List<string>();
            Type objType = obj.GetType();
            string typeName = objType.Name;
            typeHierarchy.Add(typeName);
            while (objType.BaseType != typeof(object))
            {
                objType = objType.BaseType;
                typeHierarchy.Add(objType.Name);
            }
            foreach (PropertyDescriptor pc in TypeDescriptor.GetProperties(obj))
            {
                if (typeHierarchy.Contains(pc.ComponentType.Name))
                {
                    l.Add(pc);
                }
            }
            PropertyDescriptorCollection properties = new PropertyDescriptorCollection(l.ToArray());
            if (obj is Control)
            {
                try
                {
                    ControlCollection collection2 = ((Control)obj).Controls;
                }
                catch (Exception exception1)
                {
                    IComponentDesignerDebugService debugService = host.GetService(typeof(IComponentDesignerDebugService)) as IComponentDesignerDebugService;
                    if (debugService != null)
                    {
                        debugService.Fail(exception1.Message);
                    }
                }
            }
            for (int num1 = 0; num1 < properties.Count; num1++)
            {
                try
                {
                    if (!FilterableAttribute.IsPropertyFilterable(properties[num1]))
                    {
                        string text1 = string.Empty;
                    }
                    if (properties[num1].SerializationVisibility != DesignerSerializationVisibility.Hidden)
                    {
                        PersistenceModeAttribute attribute1 = (PersistenceModeAttribute)properties[num1].Attributes[typeof(PersistenceModeAttribute)];
                        if (attribute1.Mode != PersistenceMode.Attribute)
                        {
                            DesignOnlyAttribute attribute2 = (DesignOnlyAttribute)properties[num1].Attributes[typeof(DesignOnlyAttribute)];
                            if ((attribute2 == null) || !attribute2.IsDesignOnly)
                            {
                                if (properties[num1].PropertyType == typeof(string))
                                {
                                    SerializeStringProperty(obj, host, properties[num1], persistData, attribute1.Mode, element, filter);
                                }
                                else if (typeof(ICollection).IsAssignableFrom(properties[num1].PropertyType))
                                {
                                    SerializeCollectionProperty(obj, host, properties[num1], persistData, attribute1.Mode, element, filter);
                                }
                                else if (typeof(ITemplate).IsAssignableFrom(properties[num1].PropertyType))
                                {
                                    SerializeTemplateProperty(obj, host, properties[num1], persistData, element, filter);
                                }
                                else
                                {
                                    SerializeComplexProperty(obj, host, properties[num1], persistData, element, filter);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (host != null)
                    {
                        IComponentDesignerDebugService debugService = host.GetService(typeof(IComponentDesignerDebugService)) as IComponentDesignerDebugService;
                        if (debugService != null)
                        {
                            debugService.Fail(ex.Message);
                        }
                    }
                }
            }
        }

        private static void SerializeStringProperty(object obj, IDesignerHost host, PropertyDescriptor propDesc, ObjectPersistData persistData, PersistenceMode persistenceMode, Interop.IHTMLElement element, string filter)
        {
            string propertyName = propDesc.Name;
            DataBindingCollection bindingCollection = null;
            if (obj is IDataBindingsAccessor)
            {
                bindingCollection = ((IDataBindingsAccessor)obj).DataBindings;
            }
            ExpressionBindingCollection expressions = null;
            if (obj is IExpressionsAccessor)
            {
                expressions = ((IExpressionsAccessor)obj).Expressions;
            }
            if ((persistenceMode == PersistenceMode.InnerProperty) || CanSerializeAsInnerDefaultString(filter, propertyName, propDesc.PropertyType, persistData, persistenceMode, bindingCollection, expressions))
            {
                ArrayList list1 = new ArrayList();
                if (((bindingCollection == null) || (bindingCollection[propertyName] == null)) || ((expressions == null) || (expressions[propertyName] == null)))
                {
                    string propValueName = string.Empty;
                    object propValue = propDesc.GetValue(obj);
                    if (propValue != null)
                    {
                        propValueName = propValue.ToString();
                    }
                    bool notDefault;
                    if (filter.Length == 0)
                    {
                        bool flag2;
                        bool flag3 = GetShouldSerializeValue(obj, propertyName, out flag2);
                        if (flag2)
                        {
                            notDefault = flag3;
                        }
                        else
                        {
                            object obj2 = GetPropertyDefaultValue(propDesc, propertyName, persistData, filter, host);
                            notDefault = !Equals(propValue, obj2);
                        }
                    }
                    else
                    {
                        object defaultValue = GetPropertyDefaultValue(propDesc, propertyName, persistData, filter, host);
                        notDefault = !Equals(propValue, defaultValue);
                    }
                    if (notDefault)
                    {
                        IDictionary expandos = GetExpandos(filter, propertyName, persistData);
                        list1.Add(new Triplet(filter, propValueName, expandos));
                    }
                }
                if (persistData != null)
                {
                    ICollection collection3 = persistData.GetPropertyAllFilters(propertyName);
                    foreach (PropertyEntry entry1 in collection3)
                    {
                        if ((string.Compare(entry1.Filter, filter, StringComparison.OrdinalIgnoreCase) != 0) && (entry1 is ComplexPropertyEntry))
                        {
                            ComplexPropertyEntry entry2 = (ComplexPropertyEntry)entry1;
                            object designTimeObj = entry2.Builder.BuildObject();
                            string text3 = designTimeObj.ToString();
                            IDictionary dictionary2 = GetExpandos(entry1.Filter, propertyName, persistData);
                            list1.Add(new Triplet(entry1.Filter, text3, dictionary2));
                        }
                    }
                }
                foreach (Triplet triplet1 in list1)
                {
                    bool flag4 = false;
                    IDictionary dictionary3 = triplet1.Third as IDictionary;
                    if (((list1.Count == 1) && (triplet1.First.ToString().Length == 0)) && ((dictionary3 == null) || (dictionary3.Count == 0)))
                    {
                        if (persistenceMode == PersistenceMode.InnerDefaultProperty)
                        {
                            //writer.Write(triplet1.Second.ToString());
                            element.InsertAdjacentHTML("afterBegin", triplet1.Second.ToString());
                            flag4 = true;
                        }
                        else if (persistenceMode == PersistenceMode.EncodedInnerDefaultProperty)
                        {
                            string enc = HttpUtility.HtmlEncode(triplet1.Second.ToString());
                            element.InsertAdjacentHTML("afterBegin", enc);
                            flag4 = true;
                        }
                    }
                    if (!flag4)
                    {
                        string filter1 = triplet1.First.ToString();
                        WriteInnerPropertyBeginTag(element, filter1, propertyName, dictionary3, true);
                        //writer.Write(triplet1.Second.ToString());
                        element.InsertAdjacentHTML("afterBegin", triplet1.Second.ToString());
                        WriteInnerPropertyEndTag(element, filter1, propertyName);
                    }
                }
            }
        }

        //public static string SerializeTemplate(ITemplate template, IDesignerHost host)
        //{
        //    StringWriter writer1 = new StringWriter(CultureInfo.InvariantCulture);
        //    EmbeddedSerializer.SerializeTemplate(template, writer1, host);
        //    return writer1.ToString();
        //}

        public static void SerializeTemplate(ITemplate template, Interop.IHTMLElement element, IDesignerHost host)
        {
            if (template != null)
            {
                if (element == null)
                {
                    throw new ArgumentNullException("element");
                }
                if (template is TemplateBuilder)
                {
                    //writer.Write(((TemplateBuilder)template).Text);
                    element.InsertAdjacentHTML("afterBegin", ((TemplateBuilder)template).Text);
                }
                else
                {
                    Control control1 = new Control();
                    //StringBuilder builder1 = new StringBuilder();
                    try
                    {
                        template.InstantiateIn(control1);
                        foreach (Control control2 in control1.Controls)
                        {
                            //builder1.Append(EmbeddedSerializer.SerializeControl(control2, host, element));
                            SerializeControl(control2, host, element);
                        }
                        //writer.Write(builder1.ToString());
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private static void SerializeTemplateProperty(object obj, IDesignerHost host, PropertyDescriptor propDesc, ObjectPersistData persistData, Interop.IHTMLElement element, string filter)
        {
            string propName = propDesc.Name;
            ITemplate template1 = (ITemplate)propDesc.GetValue(obj);
            if (template1 != null)
            {
                string text3 = String.Empty;
                //serTemplate = EmbeddedSerializer.SerializeTemplate(template1, element, host);
                SerializeTemplate(template1, element, host);
                string serTemplate;
                serTemplate = element.GetInnerHTML();
                if ((filter.Length > 0) && (persistData != null))
                {
                    TemplatePropertyEntry entry1 = persistData.GetFilteredProperty(string.Empty, propName) as TemplatePropertyEntry;
                    if (entry1 != null)
                    {
                        SerializeTemplate(entry1.Builder as ITemplate, element, host);  
                        text3 = element.GetInnerHTML();
                    }
                }
                IDictionary dictionary1 = GetExpandos(filter, propName, persistData);
                if ((((template1 != null) && (dictionary1 != null)) && (dictionary1.Count > 0)) || !string.Equals(text3, serTemplate))
                {
                    WriteInnerPropertyBeginTag(element, filter, propName, dictionary1, false);
                    if ((serTemplate.Length > 0) && !serTemplate.StartsWith("\r\n", StringComparison.Ordinal))
                    {
                        //writer.WriteLine();
                        element.InsertAdjacentText("afterEnd", Environment.NewLine);
                    }
                    //writer.Write(serTemplate);
                    element.InsertAdjacentHTML("BeforeEnd", serTemplate);
                    if ((serTemplate.Length > 0) && !serTemplate.EndsWith("\r\n", StringComparison.Ordinal))
                    {
                        //writer.WriteLine();
                        element.InsertAdjacentText("afterEnd", Environment.NewLine);
                    }
                    WriteInnerPropertyEndTag(element, filter, propName);
                }
            }
            if (persistData != null)
            {
                ICollection collection1 = persistData.GetPropertyAllFilters(propName);
                foreach (TemplatePropertyEntry entry2 in collection1)
                {
                    if (string.Compare(entry2.Filter, filter, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        continue;
                    }
                    IDictionary dictionary2 = GetExpandos(entry2.Filter, propName, persistData);
                    WriteInnerPropertyBeginTag(element, entry2.Filter, propName, dictionary2, false);
                    SerializeTemplate((ITemplate)entry2.Builder, element, host);
                    string text4 = element.GetInnerHTML();
                    if (text4 != null)
                    {
                        if (!text4.StartsWith("\r\n", StringComparison.Ordinal))
                        {
                            //writer.WriteLine();
                            element.InsertAdjacentText("BeforeEnd", Environment.NewLine);
                        }
                        //writer.Write(text4);
                        if (!text4.EndsWith("\r\n", StringComparison.Ordinal))
                        {
                            //writer.WriteLine();
                            element.InsertAdjacentText("afterEnd", Environment.NewLine);
                        }
                        WriteInnerPropertyEndTag(element, entry2.Filter, propName);
                    }
                }
            }
        }

        private static bool ShouldPersistBlankValue(object defValue, Type type)
        {
            if (type == typeof(string))
            {
                return !defValue.Equals("");
            }
            if (type == typeof(Color))
            {
                Color color1 = (Color)defValue;
                return !color1.IsEmpty;
            }
            if (type == typeof(FontUnit))
            {
                FontUnit unit1 = (FontUnit)defValue;
                return !unit1.IsEmpty;
            }
            if (type == typeof(Unit))
            {
                return !defValue.Equals(Unit.Empty);
            }
            return false;
        }

        private static void WriteAttribute(Interop.IHTMLElement element, string filter, string name, string value)
        {
            if ((filter != null) && (filter.Length > 0))
            {
                element.SetAttribute(String.Concat(filter, FILTER_SEPARATOR_CHAR, name), value, 0);
            } else {
                element.SetAttribute(name, value, 0);
            }
            element.SetAttribute(name, value, 0);
        }

        private static void WriteInnerPropertyBeginTag(Interop.IHTMLElement element, string filter, string name, IDictionary expandos, bool requiresNewLine)
        {
                        
            if ((filter != null) && (filter.Length > 0))
            {
                name = filter + FILTER_SEPARATOR_CHAR + name;
            }
            //Interop.IHTMLElement newElement = ((Interop.IHTMLDocument2) element.GetDocument()).CreateElement(name);
            Interop.IHTMLElement newElement = ((Interop.IHTMLDOMChildrenCollection)((Interop.IHTMLDOMNode)element).childNodes).item(0) as Interop.IHTMLElement;
            if (expandos != null)
            {
                foreach (DictionaryEntry entry1 in expandos)
                {
                    SimplePropertyEntry entry2 = entry1.Value as SimplePropertyEntry;
                    if (entry2 != null)
                    {
                        WriteAttribute(newElement, ControlBuilder.DesignerFilter, entry1.Key.ToString(), entry2.Value.ToString());
                    }
                }
            }
            //((Interop.IHTMLElement2) element).InsertAdjacentElement("afterBegin", newElement);
            if (requiresNewLine)
            {
                newElement.InsertAdjacentText("afterEnd", Environment.NewLine);
            }
        }

        private static void WriteInnerPropertyEndTag(Interop.IHTMLElement element, string filter, string name)
        {
            //writer.Write("</");
            //if ((filter != null) && (filter.Length > 0))
            //{
            //    writer.Write(filter);
            //    writer.Write(FILTER_SEPARATOR_CHAR);
            //}
            //writer.Write(name);
            //writer.WriteLine('>');
        }

        private static void SerializeControl(Control control, IDesignerHost host, Interop.IHTMLElement element, string filter)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (control is LiteralControl)
            {                
                //writer.Write(((LiteralControl)control).Text);
                element.InsertAdjacentHTML("afterBegin", ((LiteralControl)control).Text);
            }
            //else if (control is DesignerDataBoundLiteralControl)
            //{
            //    DataBinding binding1 = ((IDataBindingsAccessor)control).DataBindings["Text"];
            //    if (binding1 != null)
            //    {
            //        writer.Write("<%# ");
            //        writer.Write(binding1.Expression);
            //        writer.Write(" %>");
            //    }
            //}
            else if (control is UserControl)
            {
                IUserControlDesignerAccessor accessor1 = (IUserControlDesignerAccessor)control;
                string tagName = accessor1.TagName;
                if (tagName.Length > 0)
                {
                    element.SetAttribute("runat", "server", 0);
                    ObjectPersistData data1 = null;
                    IControlBuilderAccessor accessor2 = control;
                    if (accessor2.ControlBuilder != null)
                    {
                        data1 = accessor2.ControlBuilder.GetObjectPersistData();
                    }
                    SerializeAttributes(control, host, string.Empty, data1, element, filter);
                    //writer.Write('>');
                    string innerText = accessor1.InnerText;
                    if ((innerText != null) && (innerText.Length > 0))
                    {
                        element.SetInnerText(innerText);
                    }
                }
            }
            else
            {
                string controlName;
                HtmlControl control1 = control as HtmlControl;
                if (control1 != null)
                {
                    controlName = control1.TagName;
                }
                else
                {
                    controlName = GetTagName(control.GetType(), host);
                }
                element.SetAttribute("runat", "server", 0);
                ObjectPersistData data2 = null;
                IControlBuilderAccessor accessor3 = control;
                if (accessor3.ControlBuilder != null)
                {
                    data2 = accessor3.ControlBuilder.GetObjectPersistData();
                }
                SerializeAttributes(control, host, string.Empty, data2, element, filter);
                SerializeInnerContents(control, host, data2, element, filter);
            }
        }

        private static string GetCurrentFilter(IDesignerHost host)
        {
            return string.Empty;
        }
        private static string GetDirectives(IDesignerHost designerHost)
        {
            string text1 = string.Empty;
            WebFormsReferenceManager manager1 = null;
            if (designerHost.RootComponent != null)
            {
                WebFormsRootDesigner designer1 = designerHost.GetDesigner(designerHost.RootComponent) as WebFormsRootDesigner;
                if (designer1 != null)
                {
                    manager1 = designer1.ReferenceManager;
                }
            }
            if (manager1 == null)
            {
                IWebFormReferenceManager manager2 = (IWebFormReferenceManager)designerHost.GetService(typeof(IWebFormReferenceManager));
                if (manager2 != null)
                {
                    text1 = manager2.GetRegisterDirectives();
                }
                return text1;
            }
            StringBuilder builder1 = new StringBuilder();
            foreach (string text2 in manager1.GetRegisterDirectives())
            {
                builder1.Append(text2);
            }
            return builder1.ToString();
        }
        private static IDictionary GetExpandos(string filter, string name, ObjectPersistData persistData)
        {
            IDictionary dictionary1 = null;
            if (persistData != null)
            {
                BuilderPropertyEntry entry1 = persistData.GetFilteredProperty(filter, name) as BuilderPropertyEntry;
                if (entry1 != null)
                {
                    ObjectPersistData data1 = entry1.Builder.GetObjectPersistData();
                    dictionary1 = data1.GetFilteredProperties(ControlBuilder.DesignerFilter);
                }
            }
            return dictionary1;
        }


        private static string GetPersistValue(PropertyDescriptor propDesc, Type propType, object propValue, BindingType bindingType, bool topLevelInDesigner)
        {
            string propConverted = string.Empty;
            if (bindingType == BindingType.Data)
            {
                return ("<%# " + propValue + " %>");
            }
            if (bindingType == BindingType.Expression)
            {
                return ("<%$ " + propValue + " %>");
            }
            if (propType.IsEnum)
            {
                return Enum.Format(propType, propValue, "G");
            }
            if (propType == typeof(string))
            {
                if (propValue != null)
                {
                    propConverted = propValue.ToString();
                    if (!topLevelInDesigner)
                    {
                        // TODO: add control from prop
                        //text1 = HttpUtility.HtmlEncode(text1);
                        //text1 = GuruComponents.Netrix.HtmlFormatting.HtmlFormatter.GetEntities(text1, GuruComponents.Netrix.HtmlFormatting.EntityFormat.Named);
                    }
                }
                return propConverted;
            }
            TypeConverter propConverter;
            if (propDesc != null)
            {
                propConverter = propDesc.Converter;
            }
            else
            {
                propConverter = TypeDescriptor.GetConverter(propValue);
            }
            if (propConverter != null)
            {
                propConverted = propConverter.ConvertToInvariantString(null, propValue);
            }
            else
            {
                propConverted = propValue.ToString();
            }
            if (!topLevelInDesigner)
            {
                // TODO: add control from prop
                //text1 = GuruComponents.Netrix.HtmlFormatting.HtmlFormatter.GetEntities(text1, GuruComponents.Netrix.HtmlFormatting.EntityFormat.Named);
                //text1 = HttpUtility.HtmlEncode(text1);
            }
            return propConverted;
        }

        private static void SerializeCollectionProperty(object obj, IDesignerHost host, PropertyDescriptor propDesc, ObjectPersistData persistData, PersistenceMode persistenceMode, Interop.IHTMLElement element, string filter)
        {
            string descName = propDesc.Name;
            bool noFilter = false;
            ICollection propValues = propDesc.GetValue(obj) as ICollection;
            int propValuesCount = 0;
            if (propValues != null)
            {
                propValuesCount = propValues.Count;
            }
            int persistDataCount = 0;
            ObjectPersistData objectPersistData = null;
            if (persistData != null)
            {
                ComplexPropertyEntry complexPropertyEntry = persistData.GetFilteredProperty(string.Empty, descName) as ComplexPropertyEntry;
                if (complexPropertyEntry != null)
                {
                    objectPersistData = complexPropertyEntry.Builder.GetObjectPersistData();
                    persistDataCount = objectPersistData.CollectionItems.Count;
                }
            }
            if (filter.Length == 0)
            {
                noFilter = true;
            }
            else if (persistData != null)
            {
                if (persistData.GetFilteredProperty(filter, descName) is ComplexPropertyEntry)
                {
                    noFilter = true;
                }
                else if (persistDataCount != propValuesCount)
                {
                    noFilter = true;
                }
                else if (objectPersistData != null)
                {
                    IEnumerator enumerator1 = propValues.GetEnumerator();
                    IEnumerator enumerator2 = objectPersistData.CollectionItems.GetEnumerator();
                    while (enumerator1.MoveNext())
                    {
                        enumerator2.MoveNext();
                        ComplexPropertyEntry entry2 = (ComplexPropertyEntry)enumerator2.Current;
                        if (enumerator1.Current.GetType() != entry2.Builder.ControlType)
                        {
                            noFilter = true;
                            break;
                        }
                    }
                }
            }
            bool flag2 = false;
            ArrayList list1 = new ArrayList();
            if (propValuesCount > 0)
            {
                StringWriter writer1 = new StringWriter(CultureInfo.InvariantCulture);
                IDictionary dictionary1 = new Hashtable(ReferenceKeyComparer.Default);
                if (objectPersistData != null)
                {
                    foreach (ComplexPropertyEntry entry3 in objectPersistData.CollectionItems)
                    {
                        ObjectPersistData data2 = entry3.Builder.GetObjectPersistData();
                        if (data2 != null)
                        {
                            data2.AddToObjectControlBuilderTable(dictionary1);
                        }
                    }
                }
                if (!noFilter)
                {
                    flag2 = true;
                    foreach (object obj1 in propValues)
                    {
                        string text2 = GetTagName(obj1.GetType(), host);
                        ObjectPersistData data3 = null;
                        ControlBuilder builder1 = (ControlBuilder)dictionary1[obj1];
                        if (builder1 != null)
                        {
                            data3 = builder1.GetObjectPersistData();
                        }
                        //writer1.Write('<');
                        //writer1.Write(text2);
                        SerializeAttributes(obj1, host, string.Empty, data3, element, filter);
                        //writer1.Write('>');
                        SerializeInnerProperties(obj1, host, data3, element, filter);
                        //writer1.Write("</");
                        //writer1.Write(text2);
                        //writer1.WriteLine('>');
                    }
                    IDictionary dictionary2 = GetExpandos(filter, descName, objectPersistData);
                    list1.Add(new Triplet(string.Empty, writer1, dictionary2));
                }
                else
                {
                    // remove old children, then add all again
                    Interop.IHTMLDOMChildrenCollection children = ((Interop.IHTMLDOMNode) element).childNodes as Interop.IHTMLDOMChildrenCollection;
                    if (children != null)
                    {
                        while (children.length > 0)
                        {
                            Interop.IHTMLDOMNode childnode = children.item(0) as Interop.IHTMLDOMNode;
                            if (childnode != null)
                            {
                                ((Interop.IHTMLDOMNode)element).removeChild(childnode);
                            }
                        }
                    }                    
                    int child = 0;
                    foreach (object obj2 in propValues)
                    {
                        string text3 = GetTagName(obj2.GetType(), host);                        
                        if (obj2 is Control)
                        {
                            SerializeControl((Control)obj2, host, element, string.Empty);
                            continue;
                        }                        
                        Interop.IHTMLElement newChild = null;
                        if (children.length > child)
                        {
                            //newChild = children.item(child++) as Interop.IHTMLElement;
                        }
                        if (newChild == null)
                        {
                            newChild = ((Interop.IHTMLDocument2)element.GetDocument()).CreateElement(text3);
                        }
                        //writer1.Write('<');
                        //writer1.Write(text3);
                        ObjectPersistData data4 = null;
                        ControlBuilder builder2 = (ControlBuilder)dictionary1[obj2];
                        if (builder2 != null)
                        {
                            data4 = builder2.GetObjectPersistData();
                        }
                        if ((filter.Length == 0) && (data4 != null))
                        {
                            SerializeAttributes(obj2, host, string.Empty, data4, newChild, string.Empty);
                            //writer1.Write('>');
                            SerializeInnerProperties(obj2, host, data4, newChild, string.Empty);
                        }
                        else
                        {
                            SerializeAttributes(obj2, host, string.Empty, (ObjectPersistData)null, newChild, string.Empty);
                            //writer1.Write('>');
                            SerializeInnerProperties(obj2, host, null, newChild, string.Empty);
                        }
                        ((Interop.IHTMLDOMNode) element).appendChild(newChild as Interop.IHTMLDOMNode);
                        //writer1.Write("</");
                        //writer1.Write(text3);
                        //writer1.WriteLine('>');
                    }
                    IDictionary dictionary3 = GetExpandos(filter, descName, persistData);
                    list1.Add(new Triplet(filter, writer1, dictionary3));
                }
            }
            else if (persistDataCount > 0)
            {
                IDictionary dictionary4 = GetExpandos(filter, descName, persistData);
                list1.Add(new Triplet(filter, new StringWriter(CultureInfo.InvariantCulture), dictionary4));
            }
            if (persistData != null)
            {
                ICollection collection2 = persistData.GetPropertyAllFilters(descName);
                foreach (ComplexPropertyEntry entry4 in collection2)
                {
                    StringWriter writer2 = new StringWriter(CultureInfo.InvariantCulture);
                    if ((string.Compare(entry4.Filter, filter, StringComparison.OrdinalIgnoreCase) != 0) && (!flag2 || (entry4.Filter.Length > 0)))
                    {
                        ObjectPersistData data5 = entry4.Builder.GetObjectPersistData();
                        data5.CollectionItems.GetEnumerator();
                        foreach (ComplexPropertyEntry entry5 in data5.CollectionItems)
                        {
                            object obj3 = entry5.Builder.BuildObject();
                            if (obj3 is Control)
                            {
                                SerializeControl((Control)obj3, host, element, string.Empty);
                                continue;
                            }
                            string text4 = GetTagName(obj3.GetType(), host);
                            ObjectPersistData data6 = entry5.Builder.GetObjectPersistData();
                            //writer2.Write('<');
                            //writer2.Write(text4);
                            SerializeAttributes(obj3, host, string.Empty, data6, element, string.Empty);
                            //writer2.Write('>');
                            SerializeInnerProperties(obj3, host, data6, element, string.Empty);
                            //writer2.Write("</");
                            //writer2.Write(text4);
                            //writer2.WriteLine('>');
                        }
                        IDictionary dictionary5 = GetExpandos(entry4.Filter, descName, persistData);
                        list1.Add(new Triplet(entry4.Filter, writer2, dictionary5));
                    }
                }
            }
            foreach (Triplet triplet1 in list1)
            {
                string text5 = triplet1.First.ToString();
                IDictionary dictionary6 = (IDictionary)triplet1.Third;
                if ((((list1.Count == 1) && (text5.Length == 0)) && (persistenceMode != PersistenceMode.InnerProperty)) && ((dictionary6 == null) || (dictionary6.Count == 0)))
                {
                    //writer.Write(triplet1.Second.ToString());
                    element.InsertAdjacentHTML("beforeEnd", triplet1.Second.ToString());
                    continue;
                }
                string text6 = triplet1.Second.ToString().Trim();
                if (text6.Length > 0)
                {
                    WriteInnerPropertyBeginTag(element, text5, descName, dictionary6, true);
                    //writer.WriteLine(text6);
                    WriteInnerPropertyEndTag(element, text5, descName);
                }
            }
        }

        private static void SerializeComplexProperty(object obj, IDesignerHost host, PropertyDescriptor propDesc, ObjectPersistData persistData, Interop.IHTMLElement element, string filter)
        {
            string text1 = propDesc.Name;
            object obj1 = propDesc.GetValue(obj);
            ObjectPersistData data1 = null;
            if (persistData != null)
            {
                ComplexPropertyEntry entry1 = persistData.GetFilteredProperty(string.Empty, text1) as ComplexPropertyEntry;
                if (entry1 != null)
                {
                    data1 = entry1.Builder.GetObjectPersistData();
                }
            }
            //StringWriter writer1 = new StringWriter(CultureInfo.InvariantCulture);
            SerializeInnerProperties(obj1, host, data1, element, filter);
            //string text2 = writer1.ToString();
            string text2 = element.GetInnerHTML();
            if (text2 == null) text2 = String.Empty;
            ArrayList list1 = SerializeAttributes(obj1, host, string.Empty, data1, filter, false);
            //StringWriter writer2 = new StringWriter(CultureInfo.InvariantCulture);
            bool flag1 = true;
            foreach (Triplet triplet1 in list1)
            {
                string filterTest = triplet1.First.ToString();
                if (filterTest != ControlBuilder.DesignerFilter)
                {
                    flag1 = false;
                }
                WriteAttribute(element, filterTest, triplet1.Second.ToString(), triplet1.Third.ToString());
            }
            string text4 = string.Empty;
            if (!flag1 || (text2.Length > 0)) 
            {
                //text4 = writer2.ToString();
                text4 = element.GetInnerHTML();
            }
            if ((text4.Length + text2.Length) > 0)
            {
                //writer.WriteLine();
                //writer.Write('<');
                //writer.Write(text1);
                //writer.Write(text4);
                //writer.Write('>');
                //writer.Write(text2);
                WriteInnerPropertyEndTag(element, string.Empty, text1);
            }
            if (persistData != null)
            {
                ICollection collection1 = persistData.GetPropertyAllFilters(text1);
                foreach (ComplexPropertyEntry entry2 in collection1)
                {
                    if (entry2.Filter.Length > 0)
                    {
                        object obj2 = entry2.Builder.BuildObject();
                        //writer.WriteLine();
                        //writer.Write('<');
                        //writer.Write(entry2.Filter);
                        //writer.Write(FILTER_SEPARATOR_CHAR);
                        //writer.Write(text1);
                        SerializeAttributes(obj2, host, string.Empty, (ObjectPersistData)null, element, string.Empty);
                        //writer.Write('>');
                        SerializeInnerProperties(obj2, host, null, element, string.Empty);
                        WriteInnerPropertyEndTag(element, entry2.Filter, text1);
                    }
                }
            }
        }

        private static bool GetShouldSerializeValue(object obj, string name, out bool useResult)
        {
            useResult = false;
            Type type1 = obj.GetType();
            BindingFlags flags1 = BindingFlags.Public | (BindingFlags.Instance | BindingFlags.IgnoreCase);
            PropertyInfo info1 = type1.GetProperty(name, flags1);
            flags1 |= BindingFlags.NonPublic;
            MethodInfo info2 = info1.DeclaringType.GetMethod("ShouldSerialize" + name, flags1);
            if (info2 != null)
            {
                useResult = true;
                object obj1 = info2.Invoke(obj, new object[0]);
                return (bool)obj1;
            }
            return true;
        }

        private static string GetTagName(Type type, IDesignerHost host)
        {
            string fullName = string.Empty;
            string tagPrefix = string.Empty;
            WebFormsReferenceManager refManager = null;
            if (host != null && host.RootComponent != null)
            {
                WebFormsRootDesigner rootDesigner = host.GetDesigner(host.RootComponent) as WebFormsRootDesigner;
                if (rootDesigner != null)
                {
                    refManager = rootDesigner.ReferenceManager;
                }
            }
            if (refManager == null)
            {
                IWebFormReferenceManager manager2 = (IWebFormReferenceManager)host.GetService(typeof(IWebFormReferenceManager));
                if (manager2 != null)
                {
                    tagPrefix = manager2.GetTagPrefix(type);
                }
            }
            else
            {
                tagPrefix = refManager.GetTagPrefix(type);
            }
            if (String.IsNullOrEmpty(tagPrefix))
            {
                tagPrefix = refManager.RegisterTagPrefix(type);
            }
            if ((tagPrefix != null) && (tagPrefix.Length != 0))
            {
                fullName = tagPrefix + FILTER_SEPARATOR_CHAR + type.Name;
            }
            if (fullName.Length == 0)
            {
                fullName = type.FullName;
            }
            return fullName;
        }
        private static object GetPropertyDefaultValue(PropertyDescriptor propDesc, string name, ObjectPersistData defaultPropertyEntries, string filter, IDesignerHost host)
        {
            if ((filter.Length > 0) && (defaultPropertyEntries != null))
            {
                string text1 = ConvertPersistToObjectModelName(name);
                IFilterResolutionService service1 = null;
                ServiceContainer container1 = new ServiceContainer();
                if (host != null)
                {
                    service1 = (IFilterResolutionService)host.GetService(typeof(IFilterResolutionService));
                    if (service1 != null)
                    {
                        container1.AddService(typeof(IFilterResolutionService), service1);
                    }
                    IThemeResolutionService service2 = (IThemeResolutionService)host.GetService(typeof(IThemeResolutionService));
                    if (service2 != null)
                    {
                        container1.AddService(typeof(IThemeResolutionService), service2);
                    }
                }
                PropertyEntry entry1 = null;
                entry1 = defaultPropertyEntries.GetFilteredProperty(string.Empty, text1);
                if (entry1 is SimplePropertyEntry)
                {
                    return ((SimplePropertyEntry)entry1).Value;
                }
                if (entry1 is BoundPropertyEntry)
                {
                    string text2 = ((BoundPropertyEntry)entry1).Expression.Trim();
                    string text3 = ((BoundPropertyEntry)entry1).ExpressionPrefix.Trim();
                    if (text3.Length > 0)
                    {
                        text2 = text3 + FILTER_SEPARATOR_CHAR + text2;
                    }
                    return text2;
                }
                if (entry1 is ComplexPropertyEntry)
                {
                    ControlBuilder builder1 = ((ComplexPropertyEntry)entry1).Builder;
                    builder1.SetServiceProvider(container1);
                    object obj1 = null;
                    try
                    {
                        obj1 = builder1.BuildObject();
                    }
                    finally
                    {
                        builder1.SetServiceProvider(null);
                    }
                    return obj1;
                }
            }
            DefaultValueAttribute attribute1 = (DefaultValueAttribute)propDesc.Attributes[typeof(DefaultValueAttribute)];
            if (attribute1 != null)
            {
                return attribute1.Value;
            }
            return null;
        }

        private static void SerializeAttributes(object obj, IDesignerHost host, string prefix, ObjectPersistData persistData, Interop.IHTMLElement element, string filter)
        {
            ArrayList list1 = SerializeAttributes(obj, host, prefix, persistData, filter, false);
            foreach (Triplet triplet1 in list1)
            {
                WriteAttribute(element, triplet1.First.ToString(), triplet1.Second.ToString(), (triplet1.Third == null) ? "" :triplet1.Third.ToString());
            }
        }

        private static void SerializeAttributesRecursive(object obj, IDesignerHost host, string prefix, ObjectPersistData persistData, string filter, ArrayList attributes, DataBindingCollection dataBindings, ExpressionBindingCollection expressions, bool topLevelInDesigner)
        {
            List<PropertyDescriptor> l = new List<PropertyDescriptor>();
            List<string> typeHierarchy = new List<string>();
            Type objType = obj.GetType();
            string typeName = objType.Name;
            typeHierarchy.Add(typeName);
            while (objType.BaseType != typeof(object))
            {
                objType = objType.BaseType;
                typeHierarchy.Add(objType.Name);
            }
            foreach (PropertyDescriptor pc in TypeDescriptor.GetProperties(obj))
            {
                if (typeHierarchy.Contains(pc.ComponentType.Name))
                {
                    l.Add(pc);
                }
            }
            PropertyDescriptorCollection properties = new PropertyDescriptorCollection(l.ToArray());
            if (obj is IDataBindingsAccessor)
            {
                dataBindings = ((IDataBindingsAccessor)obj).DataBindings;
            }
            if (obj is Control)
            {
                try
                {
                    ControlCollection collection2 = ((Control)obj).Controls;
                }
                catch (Exception exception1)
                {
                    IComponentDesignerDebugService service1 = host.GetService(typeof(IComponentDesignerDebugService)) as IComponentDesignerDebugService;
                    if (service1 != null)
                    {
                        service1.Fail(exception1.Message);
                    }
                }
            }
            if (obj is IExpressionsAccessor)
            {
                expressions = ((IExpressionsAccessor)obj).Expressions;
            }
            for (int num1 = 0; num1 < properties.Count; num1++)
            {
                try
                {
                    SerializeAttribute(obj, properties[num1], dataBindings, expressions, host, prefix, persistData, filter, attributes, topLevelInDesigner);
                }
                catch (Exception exception2)
                {
                    if (host != null)
                    {
                        IComponentDesignerDebugService service2 = host.GetService(typeof(IComponentDesignerDebugService)) as IComponentDesignerDebugService;
                        if (service2 != null)
                        {
                            service2.Fail(exception2.Message);
                        }
                    }
                }
            }
        }

        private static void SerializeAttribute(object obj, PropertyDescriptor propDesc, DataBindingCollection dataBindings, ExpressionBindingCollection expressions, IDesignerHost host, string prefix, ObjectPersistData persistData, string filter, ArrayList attributes, bool topLevelInDesigner)
        {
            DesignOnlyAttribute attribute1 = (DesignOnlyAttribute)propDesc.Attributes[typeof(DesignOnlyAttribute)];
            if ((attribute1 == null) || !attribute1.IsDesignOnly)
            {
                string text1 = propDesc.Name;
                Type type1 = propDesc.PropertyType;
                PersistenceMode mode1 = ((PersistenceModeAttribute)propDesc.Attributes[typeof(PersistenceModeAttribute)]).Mode;
                bool flag1 = (dataBindings != null) && (dataBindings[text1] != null);
                bool flag2 = (expressions != null) && (expressions[text1] != null);
                if (((flag1 || flag2) || (propDesc.SerializationVisibility != DesignerSerializationVisibility.Hidden)) && (((mode1 == PersistenceMode.Attribute) || ((flag1 && flag2) && (type1 == typeof(string)))) || ((mode1 != PersistenceMode.InnerProperty) && (type1 == typeof(string)))))
                {
                    string text2 = string.Empty;
                    if (prefix.Length > 0)
                    {
                        text2 = prefix + "-" + text1;
                    }
                    else
                    {
                        text2 = text1;
                    }
                    if (propDesc.SerializationVisibility == DesignerSerializationVisibility.Content)
                    {
                        object obj1 = propDesc.GetValue(obj);
                        SerializeAttributesRecursive(obj1, host, text2, persistData, filter, attributes, dataBindings, expressions, topLevelInDesigner);
                    }
                    else
                    {
                        IAttributeAccessor accessor1 = obj as IAttributeAccessor;
                        if (!propDesc.IsReadOnly || ((accessor1 != null) && (accessor1.GetAttribute(text2) != null)))
                        {
                            string text3 = ConvertPersistToObjectModelName(text2);
                            if (!FilterableAttribute.IsPropertyFilterable(propDesc))
                            {
                                filter = string.Empty;
                            }
                            if (CanSerializeAsInnerDefaultString(filter, text3, type1, persistData, mode1, dataBindings, expressions))
                            {
                                if (topLevelInDesigner)
                                {
                                    attributes.Add(new Triplet(filter, text2, null));
                                }
                            }
                            else
                            {
                                bool flag3 = true;
                                object obj2 = null;
                                object obj3 = null;
                                try
                                {
                                    obj3 = propDesc.GetValue(obj);
                                }
                                catch
                                {
                                }
                                BindingType type2 = BindingType.None;
                                if (dataBindings != null)
                                {
                                    DataBinding binding1 = dataBindings[text3];
                                    if (binding1 != null)
                                    {
                                        obj3 = binding1.Expression;
                                        type2 = BindingType.Data;
                                    }
                                }
                                if (type2 == BindingType.None)
                                {
                                    if (expressions != null)
                                    {
                                        ExpressionBinding binding2 = expressions[text3];
                                        if ((binding2 != null) && !binding2.Generated)
                                        {
                                            obj3 = binding2.ExpressionPrefix + FILTER_SEPARATOR_CHAR + binding2.Expression;
                                            type2 = BindingType.Expression;
                                        }
                                    }
                                    else if (persistData != null)
                                    {
                                        BoundPropertyEntry entry1 = persistData.GetFilteredProperty(filter, text1) as BoundPropertyEntry;
                                        if ((entry1 != null) && !entry1.Generated)
                                        {
                                            obj2 = GetPropertyDefaultValue(propDesc, text2, persistData, filter, host);
                                            if (Equals(obj3, obj2))
                                            {
                                                obj3 = entry1.ExpressionPrefix + FILTER_SEPARATOR_CHAR + entry1.Expression;
                                                type2 = BindingType.Expression;
                                            }
                                        }
                                    }
                                }
                                if (filter.Length == 0)
                                {
                                    bool flag4 = false;
                                    bool flag5 = false;
                                    if (type2 == BindingType.None)
                                    {
                                        flag5 = GetShouldSerializeValue(obj, text1, out flag4);
                                    }
                                    if (flag4)
                                    {
                                        flag3 = flag5;
                                    }
                                    else
                                    {
                                        obj2 = GetPropertyDefaultValue(propDesc, text2, persistData, filter, host);
                                        flag3 = !Equals(obj3, obj2);
                                    }
                                }
                                else
                                {
                                    obj2 = GetPropertyDefaultValue(propDesc, text2, persistData, filter, host);
                                    flag3 = !Equals(obj3, obj2);
                                }
                                if (flag3)
                                {
                                    if (obj3 != null)
                                    {
                                        string text4 = GetPersistValue(propDesc, type1, obj3, type2, topLevelInDesigner);
                                        if (((topLevelInDesigner && (obj2 != null)) && ((text4 == null) || (text4.Length == 0))) && ShouldPersistBlankValue(obj2, type1))
                                        {
                                            text4 = string.Empty;
                                        }
                                        if ((text4 != null) && (!type1.IsArray || (text4.Length > 0)))
                                        {
                                            attributes.Add(new Triplet(filter, text2, text4));
                                        }
                                        else if (topLevelInDesigner)
                                        {
                                            attributes.Add(new Triplet(filter, text2, null));
                                        }
                                    }
                                }
                                else if (topLevelInDesigner)
                                {
                                    attributes.Add(new Triplet(filter, text2, null));
                                }
                                if (persistData != null)
                                {
                                    ICollection collection1 = persistData.GetPropertyAllFilters(text3);
                                    foreach (PropertyEntry entry2 in collection1)
                                    {
                                        if (string.Compare(entry2.Filter, filter, StringComparison.OrdinalIgnoreCase) == 0)
                                        {
                                            continue;
                                        }
                                        string text5 = string.Empty;
                                        if (entry2 is SimplePropertyEntry)
                                        {
                                            SimplePropertyEntry entry3 = (SimplePropertyEntry)entry2;
                                            if (entry3.UseSetAttribute)
                                            {
                                                text5 = entry3.Value.ToString();
                                            }
                                            else
                                            {
                                                text5 = GetPersistValue(propDesc, entry2.Type, entry3.Value, BindingType.None, topLevelInDesigner);
                                            }
                                        }
                                        else if (entry2 is BoundPropertyEntry)
                                        {
                                            BoundPropertyEntry entry4 = (BoundPropertyEntry)entry2;
                                            if (entry4.Generated)
                                            {
                                                continue;
                                            }
                                            string text6 = entry4.Expression.Trim();
                                            type2 = BindingType.Data;
                                            string text7 = entry4.ExpressionPrefix;
                                            if (text7.Length > 0)
                                            {
                                                text6 = text7 + FILTER_SEPARATOR_CHAR + text6;
                                                type2 = BindingType.Expression;
                                            }
                                            text5 = GetPersistValue(propDesc, entry2.Type, text6, type2, topLevelInDesigner);
                                        }
                                        else if (entry2 is ComplexPropertyEntry)
                                        {
                                            ComplexPropertyEntry entry5 = (ComplexPropertyEntry)entry2;
                                            object obj4 = entry5.Builder.BuildObject();
                                            text5 = (string)obj4;
                                        }
                                        attributes.Add(new Triplet(entry2.Filter, text2, text5));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static ArrayList SerializeAttributes(object obj, IDesignerHost host, string prefix, ObjectPersistData persistData, string filter, bool topLevelInDesigner)
        {
            ArrayList list1 = new ArrayList();
            SerializeAttributesRecursive(obj, host, prefix, persistData, filter, list1, null, null, topLevelInDesigner);
            if (persistData != null)
            {
                IEnumerator enumerator1 = persistData.AllPropertyEntries.GetEnumerator();
                try
                {
                Label_0179:
                    while (enumerator1.MoveNext())
                    {
                        PropertyEntry entry1 = (PropertyEntry)enumerator1.Current;
                        BoundPropertyEntry entry2 = entry1 as BoundPropertyEntry;
                        if ((entry2 != null) && !entry2.Generated)
                        {
                            char[] chArray1 = new char[] { OM_CHAR };
                            string[] textArray1 = entry2.Name.Split(chArray1);
                            if (textArray1.Length > 1)
                            {
                                object obj1 = obj;
                                string[] textArray2 = textArray1;
                                for (int num1 = 0; num1 < textArray2.Length; num1++)
                                {
                                    string text1 = textArray2[num1];
                                    PropertyDescriptor descriptor1 = TypeDescriptor.GetProperties(obj1)[text1];
                                    if (descriptor1 == null)
                                    {
                                        goto Label_0179;
                                    }
                                    PersistenceModeAttribute attribute1 = descriptor1.Attributes[typeof(PersistenceModeAttribute)] as PersistenceModeAttribute;
                                    if (attribute1 != PersistenceModeAttribute.Attribute)
                                    {
                                        string text2 = string.IsNullOrEmpty(entry2.ExpressionPrefix) ? entry2.Expression : (entry2.ExpressionPrefix + FILTER_SEPARATOR_CHAR + entry2.Expression);
                                        string text3 = GetPersistValue(TypeDescriptor.GetProperties(entry2.PropertyInfo.DeclaringType)[entry2.PropertyInfo.Name], entry2.Type, text2, string.IsNullOrEmpty(entry2.ExpressionPrefix) ? BindingType.Data : BindingType.Expression, topLevelInDesigner);
                                        list1.Add(new Triplet(entry2.Filter, ConvertObjectModelToPersistName(entry2.Name), text3));
                                        goto Label_0179;
                                    }
                                    obj1 = descriptor1.GetValue(obj1);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    IDisposable disposable1 = enumerator1 as IDisposable;
                    if (disposable1 != null)
                    {
                        disposable1.Dispose();
                    }
                }
            }
            if (obj is Control)
            {
                AttributeCollection collection1 = null;
                if (obj is WebControl)
                {
                    collection1 = ((WebControl)obj).Attributes;
                }
                else if (obj is HtmlControl)
                {
                    collection1 = ((HtmlControl)obj).Attributes;
                }
                else if (obj is UserControl)
                {
                    collection1 = ((UserControl)obj).Attributes;
                }
                if (collection1 != null)
                {
                    foreach (string text4 in collection1.Keys)
                    {
                        string text5 = collection1[text4];
                        bool flag1 = false;
                        if (text5 != null)
                        {
                            object obj2;
                            bool flag2 = false;
                            string text6 = ConvertPersistToObjectModelName(text4);
                            PropertyDescriptor descriptor2 = GetComplexProperty(obj, text6, out obj2);
                            if ((descriptor2 != null) && !descriptor2.IsReadOnly)
                            {
                                flag2 = true;
                            }
                            if (!flag2)
                            {
                                if (filter.Length == 0)
                                {
                                    flag1 = true;
                                }
                                else
                                {
                                    PropertyEntry entry3 = null;
                                    if (persistData != null)
                                    {
                                        entry3 = persistData.GetFilteredProperty(string.Empty, text4);
                                    }
                                    if (entry3 is SimplePropertyEntry)
                                    {
                                        flag1 = !text5.Equals(((SimplePropertyEntry)entry3).PersistedValue);
                                    }
                                    else if (entry3 is BoundPropertyEntry)
                                    {
                                        string text7 = ((BoundPropertyEntry)entry3).Expression;
                                        string text8 = ((BoundPropertyEntry)entry3).ExpressionPrefix;
                                        if (text8.Length > 0)
                                        {
                                            text7 = text8 + FILTER_SEPARATOR_CHAR + text7;
                                        }
                                        flag1 = !text5.Equals(text7);
                                    }
                                    else if (entry3 == null)
                                    {
                                        flag1 = true;
                                    }
                                }
                            }
                            if (flag1)
                            {
                                list1.Add(new Triplet(filter, text4, text5));
                            }
                        }
                    }
                }
            }
            if (persistData != null)
            {
                if (!string.IsNullOrEmpty(persistData.ResourceKey))
                {
                    list1.Add(new Triplet("meta", "resourceKey", persistData.ResourceKey));
                }
                if (!persistData.Localize)
                {
                    list1.Add(new Triplet("meta", "localize", "false"));
                }
                foreach (PropertyEntry entry4 in persistData.AllPropertyEntries)
                {
                    if (!String.IsNullOrEmpty(filter) && string.Compare(entry4.Filter, filter, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        continue;
                    }
                    string text11 = string.Empty;
                    if (entry4 is SimplePropertyEntry)
                    {
                        SimplePropertyEntry entry5 = (SimplePropertyEntry)entry4;
                        if (entry5.UseSetAttribute) 
                        {
                            list1.Add(new Triplet(entry4.Filter, ConvertObjectModelToPersistName(entry4.Name), entry5.Value.ToString()));
                        }
                        continue;
                    }
                    if (entry4 is BoundPropertyEntry)
                    {
                        BoundPropertyEntry entry6 = (BoundPropertyEntry)entry4;
                        if (entry6.UseSetAttribute)
                        {
                            string text9 = ((BoundPropertyEntry)entry4).Expression;
                            string text10 = ((BoundPropertyEntry)entry4).ExpressionPrefix;
                            if (text10.Length > 0)
                            {
                                text9 = text10 + FILTER_SEPARATOR_CHAR + text9;
                            }
                            list1.Add(new Triplet(entry4.Filter, ConvertObjectModelToPersistName(entry4.Name), text9));
                        }
                    }
                }
            }
            if (((obj is Control) && (persistData != null)) && (host.GetDesigner((Control)obj) == null))
            {
                foreach (EventEntry entry7 in persistData.EventEntries)
                {
                    list1.Add(new Triplet(string.Empty, "On" + entry7.Name, entry7.HandlerMethodName));
                }
            }
            return list1;
        }

        private static PropertyDescriptor GetComplexProperty(object target, string propName, out object realTarget)
        {
            realTarget = null;
            char[] chArray1 = new char[] { OM_CHAR };
            string[] textArray1 = propName.Split(chArray1);
            PropertyDescriptor descriptor1 = null;
            string[] textArray2 = textArray1;
            for (int num1 = 0; num1 < textArray2.Length; num1++)
            {
                string text1 = textArray2[num1];
                if (string.IsNullOrEmpty(text1))
                {
                    return null;
                }
                descriptor1 = TypeDescriptor.GetProperties(target)[text1];
                if (descriptor1 == null)
                {
                    return null;
                }
                realTarget = target;
                target = descriptor1.GetValue(target);
            }
            return descriptor1;
        }
 



    }
}