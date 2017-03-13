using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.CodeEditor.Library.Serialization
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SerializerPropertyAttribute :  Attribute
    {
        public SerializerPropertyAttribute()
        {
            
        }
    }
}
