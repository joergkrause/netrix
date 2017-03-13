using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel.Design;
using System.IO;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Collections;
using System.Globalization;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Reflection;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.XmlDesigner
{
    /// <summary>
    /// Serialize XML object back into string format.
    /// </summary>
    public class EmbeddedSerializer
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
                EmbeddedSerializer.ReferenceKeyComparer.Default = new EmbeddedSerializer.ReferenceKeyComparer();
            }
            public ReferenceKeyComparer()
            {
            }
            bool IEqualityComparer.Equals(object x, object y)
            {
                return object.ReferenceEquals(x, y);
            }
            int IEqualityComparer.GetHashCode(object obj)
            {
                return obj.GetHashCode();
            }

            // Fields
            internal static readonly EmbeddedSerializer.ReferenceKeyComparer Default;
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
            return objectModelName.Replace('.', '-');
        }

        private static string ConvertPersistToObjectModelName(string persistName)
        {
            return persistName.Replace('-', '.');
        }

        /// <summary>
        /// Serialize complete control.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="element"></param>
        public static void SerializeControl(Control control, Interop.IHTMLElement element)
        {
            ISite site1 = control.Site;
            if (site1 == null)
            {
                IComponent component1 = control.Page;
                if (component1 != null)
                {
                    site1 = component1.Site;
                }
            }
            IDesignerHost host1 = null;
            if (site1 != null)
            {
                host1 = (IDesignerHost)site1.GetService(typeof(IDesignerHost));
            }
            EmbeddedSerializer.SerializeControl(control, host1, element);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="host"></param>
        /// <param name="element"></param>
        public static void SerializeControl(Control control, IDesignerHost host, Interop.IHTMLElement element)
        {
            EmbeddedSerializer.SerializeControl(control, host, element, EmbeddedSerializer.GetCurrentFilter(host));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="host"></param>
        /// <param name="element"></param>
        public static void SerializeInnerContents(Control control, IDesignerHost host, Interop.IHTMLElement element)
        {
            ObjectPersistData data1 = null;
            IControlBuilderAccessor accessor1 = control;
            if (accessor1.ControlBuilder != null)
            {
                data1 = accessor1.ControlBuilder.GetObjectPersistData();
            }
            EmbeddedSerializer.SerializeInnerContents(control, host, data1, element, EmbeddedSerializer.GetCurrentFilter(host));            
        }

        internal static void SerializeInnerContents(Control control, IDesignerHost host, ObjectPersistData persistData, Interop.IHTMLElement element, string filter)
        {
            PersistChildrenAttribute attribute1 = (PersistChildrenAttribute)TypeDescriptor.GetAttributes(control)[typeof(PersistChildrenAttribute)];
            ParseChildrenAttribute attribute2 = (ParseChildrenAttribute)TypeDescriptor.GetAttributes(control)[typeof(ParseChildrenAttribute)];
            if (attribute1.Persist || (!attribute2.ChildrenAsProperties && control.HasControls()))
            {
                for (int num1 = 0; num1 < control.Controls.Count; num1++)
                {
                    EmbeddedSerializer.SerializeControl(control.Controls[num1], host, element, string.Empty);
                }
            }
            else
            {
                EmbeddedSerializer.SerializeInnerProperties(control, host, persistData, element, filter);
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
            EmbeddedSerializer.SerializeInnerProperties(obj, host, data1, element, EmbeddedSerializer.GetCurrentFilter(host));
        }

        private static void SerializeInnerProperties(object obj, IDesignerHost host, ObjectPersistData persistData, Interop.IHTMLElement element, string filter)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
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
                                string text2 = properties[num1].Name;
                                if (properties[num1].PropertyType == typeof(string))
                                {
                                    EmbeddedSerializer.SerializeStringProperty(obj, host, properties[num1], persistData, attribute1.Mode, element, filter);
                                }
                                else if (typeof(ICollection).IsAssignableFrom(properties[num1].PropertyType))
                                {
                                    EmbeddedSerializer.SerializeCollectionProperty(obj, host, properties[num1], persistData, attribute1.Mode, element, filter);
                                }
                                else if (typeof(ITemplate).IsAssignableFrom(properties[num1].PropertyType))
                                {
                                    EmbeddedSerializer.SerializeTemplateProperty(obj, host, properties[num1], persistData, element, filter);
                                }
                                else
                                {
                                    EmbeddedSerializer.SerializeComplexProperty(obj, host, properties[num1], persistData, element, filter);
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

        private static void SerializeStringProperty(object elementObj, IDesignerHost host, PropertyDescriptor propDesc, ObjectPersistData persistData, PersistenceMode persistenceMode, Interop.IHTMLElement element, string filter)
        {
            string propName = propDesc.Name;
            DataBindingCollection bindingCollection = null;
            if (elementObj is IDataBindingsAccessor)
            {
                bindingCollection = ((IDataBindingsAccessor)elementObj).DataBindings;
            }
            ExpressionBindingCollection expressionCollection = null;
            if (elementObj is IExpressionsAccessor)
            {
                expressionCollection = ((IExpressionsAccessor)elementObj).Expressions;
            }
            if ((persistenceMode == PersistenceMode.InnerProperty) || EmbeddedSerializer.CanSerializeAsInnerDefaultString(filter, propName, propDesc.PropertyType, persistData, persistenceMode, bindingCollection, expressionCollection))
            {
                ArrayList list1 = new ArrayList();
                if (((bindingCollection == null) || (bindingCollection[propName] == null)) || ((expressionCollection == null) || (expressionCollection[propName] == null)))
                {
                    string propValue = string.Empty;
                    object propValueObj = propDesc.GetValue(elementObj);
                    if (propValueObj != null)
                    {
                        propValue = propValueObj.ToString();
                    }
                    bool flag1 = true;
                    if (filter.Length == 0)
                    {
                        bool useResult;
                        bool shouldSerialize = EmbeddedSerializer.GetShouldSerializeValue(elementObj, propName, out useResult);
                        if (useResult)
                        {
                            flag1 = shouldSerialize;
                        }
                        else
                        {
                            object obj2 = EmbeddedSerializer.GetPropertyDefaultValue(propDesc, propName, persistData, filter, host);
                            flag1 = !object.Equals(propValueObj, obj2);
                        }
                    }
                    else
                    {
                        object obj3 = EmbeddedSerializer.GetPropertyDefaultValue(propDesc, propName, persistData, filter, host);
                        flag1 = !object.Equals(propValueObj, obj3);
                    }
                    if (flag1)
                    {
                        IDictionary dictionary1 = EmbeddedSerializer.GetExpandos(filter, propName, persistData);
                        list1.Add(new Triplet(filter, propValue, dictionary1));
                    }
                }
                if (persistData != null)
                {
                    ICollection collection3 = persistData.GetPropertyAllFilters(propName);
                    foreach (PropertyEntry entry1 in collection3)
                    {
                        if ((string.Compare(entry1.Filter, filter, StringComparison.OrdinalIgnoreCase) != 0) && (entry1 is ComplexPropertyEntry))
                        {
                            ComplexPropertyEntry entry2 = (ComplexPropertyEntry)entry1;
                            object obj4 = entry2.Builder.BuildObject();
                            string text3 = obj4.ToString();
                            IDictionary dictionary2 = EmbeddedSerializer.GetExpandos(entry1.Filter, propName, persistData);
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
                        EmbeddedSerializer.WriteInnerPropertyBeginTag(element, filter1, propName, dictionary3, true);
                        element.InsertAdjacentHTML("afterBegin", triplet1.Second.ToString());
                        EmbeddedSerializer.WriteInnerPropertyEndTag(element, filter1, propName);
                    }
                }
            }
        }

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
                            EmbeddedSerializer.SerializeControl(control2, host, element);
                        }
                        //writer.Write(builder1.ToString());
                    }
                    catch (Exception)
                    {
                    }
                }
                //writer.Flush();
            }
        }

        private static void SerializeTemplateProperty(object obj, IDesignerHost host, PropertyDescriptor propDesc, ObjectPersistData persistData, Interop.IHTMLElement element, string filter)
        {
            string propName = propDesc.Name;
            string serTemplate = string.Empty;
            ITemplate template1 = (ITemplate)propDesc.GetValue(obj);
            if (template1 != null)
            {
                string text3 = String.Empty;
                //serTemplate = EmbeddedSerializer.SerializeTemplate(template1, element, host);
                EmbeddedSerializer.SerializeTemplate(template1, element, host);
                serTemplate = element.GetInnerHTML();
                if ((filter.Length > 0) && (persistData != null))
                {
                    TemplatePropertyEntry entry1 = persistData.GetFilteredProperty(string.Empty, propName) as TemplatePropertyEntry;
                    if (entry1 != null)
                    {
                        EmbeddedSerializer.SerializeTemplate(entry1.Builder as ITemplate, element, host);  
                        text3 = element.GetInnerHTML();
                    }
                }
                IDictionary dictionary1 = EmbeddedSerializer.GetExpandos(filter, propName, persistData);
                if ((((template1 != null) && (dictionary1 != null)) && (dictionary1.Count > 0)) || !string.Equals(text3, serTemplate))
                {
                    EmbeddedSerializer.WriteInnerPropertyBeginTag(element, filter, propName, dictionary1, false);
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
                    EmbeddedSerializer.WriteInnerPropertyEndTag(element, filter, propName);
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
                    IDictionary dictionary2 = EmbeddedSerializer.GetExpandos(entry2.Filter, propName, persistData);
                    EmbeddedSerializer.WriteInnerPropertyBeginTag(element, entry2.Filter, propName, dictionary2, false);
                    EmbeddedSerializer.SerializeTemplate((ITemplate)entry2.Builder, element, host);
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
                        EmbeddedSerializer.WriteInnerPropertyEndTag(element, entry2.Filter, propName);
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
            if (name.ToLower() == "name") return;
            if ((filter != null) && (filter.Length > 0))
            {
                element.SetAttribute(String.Concat(filter, ":", name), value, 0);
            } else {
                if (String.IsNullOrEmpty(value))
                {
                    try { element.RemoveAttribute(name, 0); } catch{}
                }
                else
                {
                    try { element.SetAttribute(name, value, 0); } catch{}
                }
            }
        }

        private static void WriteInnerPropertyBeginTag(Interop.IHTMLElement element, string filter, string name, IDictionary expandos, bool requiresNewLine)
        {
            if ((filter != null) && (filter.Length > 0))
            {
                name = filter + ":" + name;
            }
            Interop.IHTMLElement newElement = ((Interop.IHTMLDOMChildrenCollection)((Interop.IHTMLDOMNode)element).childNodes).item(0) as Interop.IHTMLElement;
            if (expandos != null)
            {
                foreach (DictionaryEntry entry1 in expandos)
                {
                    SimplePropertyEntry entry2 = entry1.Value as SimplePropertyEntry;
                    if (entry2 != null)
                    {
                        EmbeddedSerializer.WriteAttribute(newElement, ControlBuilder.DesignerFilter, entry1.Key.ToString(), entry2.Value.ToString());
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
                element.InsertAdjacentHTML("afterBegin", ((LiteralControl)control).Text);
            }
            else
            {
                string controlName;
                XmlControl control1 = control as XmlControl;
                if (control1 != null)
                {
                    controlName = control1.TagName;
                }
                else
                {
                    controlName = EmbeddedSerializer.GetTagName(control.GetType(), host);
                }
                ObjectPersistData data2 = null;
                IControlBuilderAccessor accessor3 = control;
                if (accessor3.ControlBuilder != null)
                {
                    data2 = accessor3.ControlBuilder.GetObjectPersistData();
                }
                EmbeddedSerializer.SerializeAttributes(control, host, string.Empty, data2, element, filter);
                EmbeddedSerializer.SerializeInnerContents(control, host, data2, element, filter);
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


        private static string GetPersistValue(PropertyDescriptor propDesc, Type propType, object propValue, EmbeddedSerializer.BindingType bindingType, bool topLevelInDesigner)
        {
            string text1 = string.Empty;
            if (bindingType == EmbeddedSerializer.BindingType.Data)
            {
                return ("<%# " + propValue.ToString() + " %>");
            }
            if (bindingType == EmbeddedSerializer.BindingType.Expression)
            {
                return ("<%$ " + propValue.ToString() + " %>");
            }
            if (propType.IsEnum)
            {
                return Enum.Format(propType, propValue, "G");
            }
            if (propType == typeof(string))
            {
                if (propValue != null)
                {
                    text1 = propValue.ToString();
                    if (!topLevelInDesigner)
                    {
                        text1 = HttpUtility.HtmlEncode(text1);
                    }
                }
                return text1;
            }
            TypeConverter converter1 = null;
            if (propDesc != null)
            {
                converter1 = propDesc.Converter;
            }
            else
            {
                converter1 = TypeDescriptor.GetConverter(propValue);
            }
            if (converter1 != null)
            {
                text1 = converter1.ConvertToInvariantString(null, propValue);
            }
            else
            {
                text1 = propValue.ToString();
            }
            if (!topLevelInDesigner)
            {
                text1 = HttpUtility.HtmlEncode(text1);
            }
            return text1;
        }

        private static void SerializeCollectionProperty(object obj, IDesignerHost host, PropertyDescriptor propDesc, ObjectPersistData persistData, PersistenceMode persistenceMode, Interop.IHTMLElement element, string filter)
        {
            string text1 = propDesc.Name;
            bool flag1 = false;
            ICollection collection1 = propDesc.GetValue(obj) as ICollection;
            int num1 = 0;
            if (collection1 != null)
            {
                num1 = collection1.Count;
            }
            int num2 = 0;
            ObjectPersistData data1 = null;
            if (persistData != null)
            {
                ComplexPropertyEntry entry1 = persistData.GetFilteredProperty(string.Empty, text1) as ComplexPropertyEntry;
                if (entry1 != null)
                {
                    data1 = entry1.Builder.GetObjectPersistData();
                    num2 = data1.CollectionItems.Count;
                }
            }
            if (filter.Length == 0)
            {
                flag1 = true;
            }
            else if (persistData != null)
            {
                if (persistData.GetFilteredProperty(filter, text1) is ComplexPropertyEntry)
                {
                    flag1 = true;
                }
                else if (num2 != num1)
                {
                    flag1 = true;
                }
                else if (data1 != null)
                {
                    IEnumerator enumerator1 = collection1.GetEnumerator();
                    IEnumerator enumerator2 = data1.CollectionItems.GetEnumerator();
                    while (enumerator1.MoveNext())
                    {
                        enumerator2.MoveNext();
                        ComplexPropertyEntry entry2 = (ComplexPropertyEntry)enumerator2.Current;
                        if (enumerator1.Current.GetType() != entry2.Builder.ControlType)
                        {
                            flag1 = true;
                            break;
                        }
                    }
                }
            }
            bool flag2 = false;
            ArrayList list1 = new ArrayList();
            if (num1 > 0)
            {
                StringWriter writer1 = new StringWriter(CultureInfo.InvariantCulture);
                IDictionary dictionary1 = new Hashtable(EmbeddedSerializer.ReferenceKeyComparer.Default);
                if (data1 != null)
                {
                    foreach (ComplexPropertyEntry entry3 in data1.CollectionItems)
                    {
                        ObjectPersistData data2 = entry3.Builder.GetObjectPersistData();
                        if (data2 != null)
                        {
                            data2.AddToObjectControlBuilderTable(dictionary1);
                        }
                    }
                }
                if (!flag1)
                {
                    flag2 = true;
                    foreach (object obj1 in collection1)
                    {
                        string text2 = EmbeddedSerializer.GetTagName(obj1.GetType(), host);
                        ObjectPersistData data3 = null;
                        ControlBuilder builder1 = (ControlBuilder)dictionary1[obj1];
                        if (builder1 != null)
                        {
                            data3 = builder1.GetObjectPersistData();
                        }
                        EmbeddedSerializer.SerializeAttributes(obj1, host, string.Empty, data3, element, filter);
                        EmbeddedSerializer.SerializeInnerProperties(obj1, host, data3, element, filter);
                    }
                    IDictionary dictionary2 = EmbeddedSerializer.GetExpandos(filter, text1, data1);
                    list1.Add(new Triplet(string.Empty, writer1, dictionary2));
                }
                else
                {
                    // remove old children, then add all again
                    Interop.IHTMLDOMChildrenCollection children = ((Interop.IHTMLDOMNode) element).childNodes as Interop.IHTMLDOMChildrenCollection;
                    //if (children != null)
                    //{
                    //    for (int child = 0; child < children.length; child++)
                    //    {
                    //        Interop.IHTMLDOMNode childnode = children.item(child) as Interop.IHTMLDOMNode;
                    //        if (childnode != null)
                    //        {
                    //            ((Interop.IHTMLDOMNode)element).removeChild(childnode);
                    //        }
                    //    }
                    //}                    
                    int child = 0;
                    foreach (object obj2 in collection1)
                    {
                        string text3 = EmbeddedSerializer.GetTagName(obj2.GetType(), host);                        
                        if (obj2 is Control)
                        {
                            EmbeddedSerializer.SerializeControl((Control)obj2, host, element, string.Empty);
                            continue;
                        }                        
                        Interop.IHTMLElement newChild = children.item(child++) as Interop.IHTMLElement; // ((Interop.IHTMLDocument2)element.GetDocument()).CreateElement(text3);
                        ObjectPersistData data4 = null;
                        ControlBuilder builder2 = (ControlBuilder)dictionary1[obj2];
                        if (builder2 != null)
                        {
                            data4 = builder2.GetObjectPersistData();
                        }
                        if ((filter.Length == 0) && (data4 != null))
                        {
                            EmbeddedSerializer.SerializeAttributes(obj2, host, string.Empty, data4, newChild, string.Empty);
                            EmbeddedSerializer.SerializeInnerProperties(obj2, host, data4, newChild, string.Empty);
                        }
                        else
                        {
                            EmbeddedSerializer.SerializeAttributes(obj2, host, string.Empty, (ObjectPersistData)null, newChild, string.Empty);
                            EmbeddedSerializer.SerializeInnerProperties(obj2, host, null, newChild, string.Empty);
                        }
                    }
                    IDictionary dictionary3 = EmbeddedSerializer.GetExpandos(filter, text1, persistData);
                    list1.Add(new Triplet(filter, writer1, dictionary3));
                }
            }
            else if (num2 > 0)
            {
                IDictionary dictionary4 = EmbeddedSerializer.GetExpandos(filter, text1, persistData);
                list1.Add(new Triplet(filter, new StringWriter(CultureInfo.InvariantCulture), dictionary4));
            }
            if (persistData != null)
            {
                ICollection collection2 = persistData.GetPropertyAllFilters(text1);
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
                                EmbeddedSerializer.SerializeControl((Control)obj3, host, element, string.Empty);
                                continue;
                            }
                            string text4 = EmbeddedSerializer.GetTagName(obj3.GetType(), host);
                            ObjectPersistData data6 = entry5.Builder.GetObjectPersistData();
                            EmbeddedSerializer.SerializeAttributes(obj3, host, string.Empty, data6, element, string.Empty);
                            EmbeddedSerializer.SerializeInnerProperties(obj3, host, data6, element, string.Empty);
                        }
                        IDictionary dictionary5 = EmbeddedSerializer.GetExpandos(entry4.Filter, text1, persistData);
                        list1.Add(new Triplet(entry4.Filter, writer2, dictionary5));
                    }
                }
            }
            foreach (Triplet triplet1 in list1)
            {
                string txtFirst = triplet1.First.ToString();
                IDictionary dictionary6 = (IDictionary)triplet1.Third;
                if ((((list1.Count == 1) && (txtFirst.Length == 0)) && (persistenceMode != PersistenceMode.InnerProperty)) && ((dictionary6 == null) || (dictionary6.Count == 0)))
                {
                    element.InsertAdjacentHTML("beforeEnd", triplet1.Second.ToString());
                    continue;
                }
                string txtScnd = triplet1.Second.ToString().Trim();
                if (txtScnd.Length > 0)
                {
                    EmbeddedSerializer.WriteInnerPropertyBeginTag(element, txtFirst, text1, dictionary6, true);
                    EmbeddedSerializer.WriteInnerPropertyEndTag(element, txtFirst, text1);
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
            EmbeddedSerializer.SerializeInnerProperties(obj1, host, data1, element, filter);
            string text2 = element.GetInnerHTML();
            ArrayList list1 = EmbeddedSerializer.SerializeAttributes(obj1, host, string.Empty, data1, filter, false);
            bool flag1 = true;
            foreach (Triplet triplet1 in list1)
            {
                string filterTest = triplet1.First.ToString();
                if (filterTest != ControlBuilder.DesignerFilter)
                {
                    flag1 = false;
                }
                EmbeddedSerializer.WriteAttribute(element, filterTest, triplet1.Second.ToString(), triplet1.Third.ToString());
            }
            string innerHtml = string.Empty;
            if (!flag1 || (text2.Length > 0))
            {
                innerHtml = element.GetInnerHTML();
            }
            if ((innerHtml.Length + text2.Length) > 0)
            {
                EmbeddedSerializer.WriteInnerPropertyEndTag(element, string.Empty, text1);
            }
            if (persistData != null)
            {
                ICollection collection1 = persistData.GetPropertyAllFilters(text1);
                foreach (ComplexPropertyEntry entry2 in collection1)
                {
                    if (entry2.Filter.Length > 0)
                    {
                        object obj2 = entry2.Builder.BuildObject();
                        EmbeddedSerializer.SerializeAttributes(obj2, host, string.Empty, (ObjectPersistData)null, element, string.Empty);
                        EmbeddedSerializer.SerializeInnerProperties(obj2, host, null, element, string.Empty);
                        EmbeddedSerializer.WriteInnerPropertyEndTag(element, entry2.Filter, text1);
                    }
                }
            }
        }

        private static bool GetShouldSerializeValue(object obj, string name, out bool useResult)
        {
            useResult = false;
            Type type1 = obj.GetType();
            BindingFlags flags1 = BindingFlags.Public | BindingFlags.Instance; // | BindingFlags.IgnoreCase);
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
            string text1 = string.Empty;
            string text2 = string.Empty;
            WebFormsReferenceManager manager1 = null;
            if (host.RootComponent != null)
            {
                WebFormsRootDesigner designer1 = host.GetDesigner(host.RootComponent) as WebFormsRootDesigner;
                if (designer1 != null)
                {
                    manager1 = designer1.ReferenceManager;
                }
            }
            if (manager1 == null)
            {
                IWebFormReferenceManager manager2 = (IWebFormReferenceManager)host.GetService(typeof(IWebFormReferenceManager));
                if (manager2 != null)
                {
                    text2 = manager2.GetTagPrefix(type);
                }
                else
                {                    
                    object o = Convert.ChangeType(type, type);
                    return "";
                }
            }
            else
            {
                text2 = manager1.GetTagPrefix(type);
            }
            if (string.IsNullOrEmpty(text2))
            {
                text2 = manager1.RegisterTagPrefix(type);
            }
            if ((text2 != null) && (text2.Length != 0))
            {
                text1 = text2 + ":" + type.Name;
            }
            if (text1.Length == 0)
            {
                text1 = type.FullName;
            }
            return text1;
        }
        private static object GetPropertyDefaultValue(PropertyDescriptor propDesc, string name, ObjectPersistData defaultPropertyEntries, string filter, IDesignerHost host)
        {
            if ((filter.Length > 0) && (defaultPropertyEntries != null))
            {
                string text1 = EmbeddedSerializer.ConvertPersistToObjectModelName(name);
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
                        text2 = text3 + ":" + text2;
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
            ArrayList list1 = EmbeddedSerializer.SerializeAttributes(obj, host, prefix, persistData, filter, false);
            foreach (Triplet triplet1 in list1)
            {
                EmbeddedSerializer.WriteAttribute(element, triplet1.First.ToString(), triplet1.Second.ToString(), triplet1.Third.ToString());
            }
        }

        private static void SerializeAttributesRecursive(object obj, IDesignerHost host, string prefix, ObjectPersistData persistData, string filter, ArrayList attributes, DataBindingCollection dataBindings, ExpressionBindingCollection expressions, bool topLevelInDesigner)
        {
            PropertyDescriptorCollection collection1 = TypeDescriptor.GetProperties(obj);
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
            for (int num1 = 0; num1 < collection1.Count; num1++)
            {
                try
                {
                    EmbeddedSerializer.SerializeAttribute(obj, collection1[num1], dataBindings, expressions, host, prefix, persistData, filter, attributes, topLevelInDesigner);
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
            DesignOnlyAttribute designOnlyAttr = (DesignOnlyAttribute)propDesc.Attributes[typeof(DesignOnlyAttribute)];
            if ((designOnlyAttr == null) || !designOnlyAttr.IsDesignOnly)
            {
                # region !IsDesignOnly
                string propertyName = propDesc.Name;
                Type propertyType = propDesc.PropertyType;
                PersistenceMode persistMode = ((PersistenceModeAttribute)propDesc.Attributes[typeof(PersistenceModeAttribute)]).Mode;
                bool hasDataBindings = (dataBindings != null) && (dataBindings[propertyName] != null);
                bool hasExpressions = (expressions != null) && (expressions[propertyName] != null);
                if (((hasDataBindings || hasExpressions) || (propDesc.SerializationVisibility != DesignerSerializationVisibility.Hidden)) && (((persistMode == PersistenceMode.Attribute) || ((hasDataBindings && hasExpressions) && (propertyType == typeof(string)))) || ((persistMode != PersistenceMode.InnerProperty) && (propertyType == typeof(string)))))
                {
                    string text2 = string.Empty;
                    if (prefix.Length > 0)
                    {
                        text2 = prefix + "-" + propertyName;
                    }
                    else
                    {
                        text2 = propertyName;
                    }
                    if (propDesc.SerializationVisibility == DesignerSerializationVisibility.Content)
                    {
                        object obj1 = propDesc.GetValue(obj);
                        EmbeddedSerializer.SerializeAttributesRecursive(obj1, host, text2, persistData, filter, attributes, dataBindings, expressions, topLevelInDesigner);
                    }
                    else
                    {
                        IAttributeAccessor accessor1 = obj as IAttributeAccessor;
                        if (!propDesc.IsReadOnly || ((accessor1 != null) && (accessor1.GetAttribute(text2) != null)))
                        {
                            string text3 = EmbeddedSerializer.ConvertPersistToObjectModelName(text2);
                            if (!FilterableAttribute.IsPropertyFilterable(propDesc))
                            {
                                filter = string.Empty;
                            }
                            if (EmbeddedSerializer.CanSerializeAsInnerDefaultString(filter, text3, propertyType, persistData, persistMode, dataBindings, expressions))
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
                                object obj3 = propDesc.GetValue(obj);
                                EmbeddedSerializer.BindingType type2 = EmbeddedSerializer.BindingType.None;
                                if (dataBindings != null)
                                {
                                    DataBinding binding1 = dataBindings[text3];
                                    if (binding1 != null)
                                    {
                                        obj3 = binding1.Expression;
                                        type2 = EmbeddedSerializer.BindingType.Data;
                                    }
                                }
                                if (type2 == EmbeddedSerializer.BindingType.None)
                                {
                                    if (expressions != null)
                                    {
                                        ExpressionBinding binding2 = expressions[text3];
                                        if ((binding2 != null) && !binding2.Generated)
                                        {
                                            obj3 = binding2.ExpressionPrefix + ":" + binding2.Expression;
                                            type2 = EmbeddedSerializer.BindingType.Expression;
                                        }
                                    }
                                    else if (persistData != null)
                                    {
                                        BoundPropertyEntry entry1 = persistData.GetFilteredProperty(filter, propertyName) as BoundPropertyEntry;
                                        if ((entry1 != null) && !entry1.Generated)
                                        {
                                            obj2 = EmbeddedSerializer.GetPropertyDefaultValue(propDesc, text2, persistData, filter, host);
                                            if (object.Equals(obj3, obj2))
                                            {
                                                obj3 = entry1.ExpressionPrefix + ":" + entry1.Expression;
                                                type2 = EmbeddedSerializer.BindingType.Expression;
                                            }
                                        }
                                    }
                                }
                                if (filter.Length == 0)
                                {
                                    bool flag4 = false;
                                    bool flag5 = false;
                                    if (type2 == EmbeddedSerializer.BindingType.None)
                                    {
                                        flag5 = EmbeddedSerializer.GetShouldSerializeValue(obj, propertyName, out flag4);
                                    }
                                    if (flag4)
                                    {
                                        flag3 = flag5;
                                    }
                                    else
                                    {
                                        obj2 = EmbeddedSerializer.GetPropertyDefaultValue(propDesc, text2, persistData, filter, host);
                                        flag3 = !object.Equals(obj3, obj2);
                                    }
                                }
                                else
                                {
                                    obj2 = EmbeddedSerializer.GetPropertyDefaultValue(propDesc, text2, persistData, filter, host);
                                    flag3 = !object.Equals(obj3, obj2);
                                }
                                if (flag3)
                                {
                                    string text4 = EmbeddedSerializer.GetPersistValue(propDesc, propertyType, obj3, type2, topLevelInDesigner);
                                    if (((topLevelInDesigner && (obj2 != null)) && ((text4 == null) || (text4.Length == 0))) && EmbeddedSerializer.ShouldPersistBlankValue(obj2, propertyType))
                                    {
                                        text4 = string.Empty;
                                    }
                                    if ((text4 != null) && (!propertyType.IsArray || (text4.Length > 0)))
                                    {
                                        attributes.Add(new Triplet(filter, text2, text4));
                                    }
                                    else if (topLevelInDesigner)
                                    {
                                        attributes.Add(new Triplet(filter, text2, null));
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
                                                text5 = EmbeddedSerializer.GetPersistValue(propDesc, entry2.Type, entry3.Value, EmbeddedSerializer.BindingType.None, topLevelInDesigner);
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
                                            type2 = EmbeddedSerializer.BindingType.Data;
                                            string text7 = entry4.ExpressionPrefix;
                                            if (text7.Length > 0)
                                            {
                                                text6 = text7 + ":" + text6;
                                                type2 = EmbeddedSerializer.BindingType.Expression;
                                            }
                                            text5 = EmbeddedSerializer.GetPersistValue(propDesc, entry2.Type, text6, type2, topLevelInDesigner);
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
                # endregion !IsDesignOnly
            }
            else
            {
                //((XmlControl)obj).GetBaseElement().RemoveAttribute(propDesc.Name, 0);
            }
        }

        private static ArrayList SerializeAttributes(object obj, IDesignerHost host, string prefix, ObjectPersistData persistData, string filter, bool topLevelInDesigner)
        {
            ArrayList listAttributes = new ArrayList();
            EmbeddedSerializer.SerializeAttributesRecursive(obj, host, prefix, persistData, filter, listAttributes, null, null, topLevelInDesigner);
            if (persistData != null)
            {
                foreach (PropertyEntry persistEntry in persistData.AllPropertyEntries)
                {
                    BoundPropertyEntry boundEntry = persistEntry as BoundPropertyEntry;
                    if ((boundEntry != null) && !boundEntry.Generated)
                    {
                        string[] boundPropNameArray = boundEntry.Name.Split('.');
                        if (boundPropNameArray.Length > 1)
                        {
                            object obj1 = obj;
                            string[] boundPropArray = boundPropNameArray;
                            for (int num1 = 0; num1 < boundPropArray.Length; num1++)
                            {
                                string text1 = boundPropArray[num1];
                                PropertyDescriptor propDescriptor = TypeDescriptor.GetProperties(obj1)[text1];
                                if (propDescriptor == null)
                                {
                                    continue;
                                }
                                PersistenceModeAttribute attribute1 = propDescriptor.Attributes[typeof(PersistenceModeAttribute)] as PersistenceModeAttribute;
                                if (attribute1 != PersistenceModeAttribute.Attribute)
                                {
                                    string text2 = string.IsNullOrEmpty(boundEntry.ExpressionPrefix) ? boundEntry.Expression : (boundEntry.ExpressionPrefix + ":" + boundEntry.Expression);
                                    string text3 = EmbeddedSerializer.GetPersistValue(TypeDescriptor.GetProperties(boundEntry.PropertyInfo.DeclaringType)[boundEntry.PropertyInfo.Name], boundEntry.Type, text2, string.IsNullOrEmpty(boundEntry.ExpressionPrefix) ? EmbeddedSerializer.BindingType.Data : EmbeddedSerializer.BindingType.Expression, topLevelInDesigner);
                                    listAttributes.Add(new Triplet(boundEntry.Filter, EmbeddedSerializer.ConvertObjectModelToPersistName(boundEntry.Name), text3));
                                    continue;
                                }
                                obj1 = propDescriptor.GetValue(obj1);
                            }
                        }
                    }
                }
            }
            if (obj is Control)
            {
                System.Web.UI.AttributeCollection collection1 = null;
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
                            string text6 = EmbeddedSerializer.ConvertPersistToObjectModelName(text4);
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
                                            text7 = text8 + ":" + text7;
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
                                listAttributes.Add(new Triplet(filter, text4, text5));
                            }
                        }
                    }
                }
            }
            if (persistData != null)
            {
                if (!string.IsNullOrEmpty(persistData.ResourceKey))
                {
                    listAttributes.Add(new Triplet("meta", "resourceKey", persistData.ResourceKey));
                }
                if (!persistData.Localize)
                {
                    listAttributes.Add(new Triplet("meta", "localize", "false"));
                }
                foreach (PropertyEntry entry4 in persistData.AllPropertyEntries)
                {
                    if (string.Compare(entry4.Filter, filter, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        continue;
                    }
                    string text11 = string.Empty;
                    if (entry4 is SimplePropertyEntry)
                    {
                        SimplePropertyEntry entry5 = (SimplePropertyEntry)entry4;
                        if (entry5.UseSetAttribute)
                        {
                            listAttributes.Add(new Triplet(entry4.Filter, EmbeddedSerializer.ConvertObjectModelToPersistName(entry4.Name), entry5.Value.ToString()));
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
                                text9 = text10 + ":" + text9;
                            }
                            listAttributes.Add(new Triplet(entry4.Filter, EmbeddedSerializer.ConvertObjectModelToPersistName(entry4.Name), text9));
                        }
                    }
                }
            }
            if (((obj is Control) && (persistData != null)) && (host.GetDesigner((Control)obj) == null))
            {
                foreach (EventEntry entry7 in persistData.EventEntries)
                {
                    listAttributes.Add(new Triplet(string.Empty, "On" + entry7.Name, entry7.HandlerMethodName));
                }
            }
            return listAttributes;
        }

        private static PropertyDescriptor GetComplexProperty(object target, string propName, out object realTarget)
        {
            realTarget = null;
            char[] chArray1 = new char[] { '.' };
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