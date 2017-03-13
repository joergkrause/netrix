using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using System.Reflection;
using System.Xml;

namespace GuruComponents.CodeEditor.Library.Serialization
{
    public class Serializer
    {

        public static string Serialize(object obj)
        {
           object[] attrs = obj.GetType().GetCustomAttributes(typeof(SerializerAttribute), false);

           if (attrs.Length == 0)
           {
               throw new InvalidOperationException("The obj is not marked with SerializerAttribute");
           }


           PropertyInfo[] infos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic);

           StringBuilder sbXml = new StringBuilder();
           StringWriter sw = new StringWriter(sbXml);
           //sw.Encoding = Encoding.UTF8;
           XmlTextWriter xwr = new XmlTextWriter(sw);
            
           xwr.Formatting = Formatting.Indented;

           xwr.WriteStartElement("Object");
           xwr.WriteAttributeString("Assembly", obj.GetType().Assembly.FullName);
           xwr.WriteAttributeString("Type", obj.GetType().FullName);

           foreach (PropertyInfo current in infos)
           {
               if (!current.CanWrite)
                   throw new Exception("The property " + current.Name + " don't have a set method!");
               xwr.WriteStartElement("Property");
               xwr.WriteAttributeString("Name", current.Name);
               xwr.WriteAttributeString("Type", current.PropertyType.Name);

               xwr.WriteEndElement();
           }

           xwr.WriteEndElement();

           xwr.Flush();

           xwr.Close();

           return sbXml.ToString();
        }
    }
}
