using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.CodeEditor.Library.Serialization
{
    [AttributeUsage( AttributeTargets.Class)]
    public class SerializerAttribute :   Attribute
    {
        public SerializerAttribute()
        {
            
        }
    }
}
