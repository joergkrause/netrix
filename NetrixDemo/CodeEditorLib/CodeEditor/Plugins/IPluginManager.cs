using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.CodeEditor.Library.Collections.Generic;

namespace GuruComponents.CodeEditor.Library.Plugins
{
    public interface IPluginManager
    {
        KeyedCollection<PluginLoadInfo> Plugins { get;}

        PluginBase LoadPlugin(PluginLoadInfo plugin);

        void UnloadPlugin(PluginBase plugin);

        KeyedCollection<PluginBase> PluginsLoaded{get;}

        string AssemblySearchPath { get;set;}

        Type PluginBaseType { get;}

        void Save();
    }
}
