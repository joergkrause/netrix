using System;
using GuruComponents.CodeEditor.Library.Collections.Generic;

namespace GuruComponents.CodeEditor.Library.Plugins
{
    public abstract class PluginApplication : IPluginApplication
    {
        private KeyedCollection<IPluginObject> _Objects = null;

        protected static PluginApplication istance = null;


        public PluginApplication()
        {
            _Objects = new KeyedCollection<IPluginObject>();
        }


        public KeyedCollection<IPluginObject> PublicObjects
        {
            get { return _Objects; }
        }

        public abstract IPluginManager Manager { get;}



        public abstract void Run();


        public static PluginApplication Istance
        {
            get
            {
                return istance;
            }
        }
    }
}
