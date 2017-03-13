using System;
using GuruComponents.CodeEditor.Library.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.IO;

namespace GuruComponents.CodeEditor.Library.Plugins
{
    public abstract class PluginManager : IPluginManager
    {
        private KeyedCollection<PluginLoadInfo> _Plugins = null;

        private KeyedCollection<PluginBase> _PluginsLoaded = null;

        private string _AssemblySearchPath = null;

        private KeyedCollection<Assembly> _Assemblies = null;


        public PluginManager()
        {
            _Assemblies = new KeyedCollection<Assembly>();
            _Plugins = new KeyedCollection<PluginLoadInfo>();
            _PluginsLoaded = new KeyedCollection<PluginBase>();
        }

        public string AssemblySearchPath
        {
            get
            {
                return _AssemblySearchPath;
            }
            set
            {
                _AssemblySearchPath = value;
            }
        }


        public virtual Type PluginBaseType
        {
            get
            {
                return typeof(PluginBase);
            }
        }

        public KeyedCollection<PluginBase> PluginsLoaded
        {
            get
            {
                return _PluginsLoaded;
            }
        }

        public KeyedCollection<PluginLoadInfo> Plugins
        {
            get
            {
                return _Plugins;
            }
        }

        public void AddPlugin(string key, ref PluginBase plugin, bool loaded)
        {
            Assembly plugAss = plugin.GetType().Assembly;

            string fileName= Path.GetFileName(plugAss.Location);

            _Plugins.Add(key, new PluginLoadInfo(loaded, fileName, plugin.GetType().FullName));

            if (loaded)
                _PluginsLoaded.Add(key, plugin);
        }

        public void AddPlugin(string key, ref PluginBase plugin)
        {
            this.AddPlugin(key, ref plugin, false);
        }

        public void RemovePlugin(string key)
        {
            _Plugins.Remove(key);
            _PluginsLoaded.Remove(key);
        }

        public void RemovePlugin(PluginBase plugin)
        {
            _Plugins.Remove(plugin.GetType().FullName);
            _PluginsLoaded.Remove(plugin);
        }

        public PluginApplication PluginApplication
        {
            get
            {
                return GuruComponents.CodeEditor.Library.Plugins.PluginApplication.Istance;
            }
        }

        private string _ConfigFile = null;

        public virtual void LoadFromXml(string filename)
        {
            XmlDocument xdoc = new XmlDocument();

            xdoc.Load(filename);

            _ConfigFile = filename;

            XmlNodeList plugins = xdoc.SelectNodes("plugins/plugin");

            if (plugins == null) return;

            foreach (XmlNode current in plugins)
            {
                bool loaded = bool.Parse(current.Attributes["loaded"].InnerText);
                string plugFileName = current.Attributes["filename"].InnerText;
                string className = current.Attributes["className"].InnerText;

                PluginLoadInfo plugInfo = new PluginLoadInfo(loaded, plugFileName, className);

                this.Plugins.Add(className, plugInfo);

                if (plugInfo.Loaded)
                {
                    PluginBase plugLoaded = LoadPlugin(plugInfo);
                }
            }
        }

        protected abstract void AddToPluginsMenu(PluginBase plugin);

        protected abstract void RemoveFromPluginsMenu(PluginBase plugin);


        public virtual void UnloadPlugin(PluginBase plugin)
        {
            this.PluginsLoaded.Remove(plugin);
            RemoveFromPluginsMenu(plugin);
        }

        public virtual PluginBase LoadPlugin(PluginLoadInfo plugin)
        {
            if (this.PluginsLoaded.Contains(plugin.ClassName))
            {
                return this.PluginsLoaded[plugin.ClassName];
            }

            Assembly ass = null;

            if (_Assemblies.Contains(plugin.Filename))
            {
                ass = _Assemblies[plugin.Filename];
            }
            else
            {
                string path = Path.Combine(this.AssemblySearchPath, plugin.Filename);

                ass = Assembly.LoadFile(path);

                _Assemblies.Add(plugin.Filename, ass);
            }

            PluginBase plug = (PluginBase)ass.CreateInstance(plugin.ClassName);

            this.PluginsLoaded.Add(plugin.ClassName, plug);

            if (plug.ShowMenuItem)
            {
                AddToPluginsMenu(plug);
            }

            return plug;
        }

        public void Save()
        {
            this.Save(_ConfigFile);
        }

        public virtual void Save(string filename)
        {
            XmlTextWriter xwr = new XmlTextWriter(filename, Encoding.UTF8);

            xwr.Formatting = Formatting.Indented;

            xwr.WriteStartElement("plugins");

            foreach (PluginLoadInfo current in this.Plugins)
            {
                xwr.WriteStartElement("plugin");
               
                xwr.WriteAttributeString("loaded", current.Loaded.ToString());

                xwr.WriteAttributeString("filename", current.Filename);

                xwr.WriteAttributeString("className", current.ClassName);

                xwr.WriteEndElement();
            }

            xwr.WriteEndElement();

            xwr.Flush();

            xwr.Close();
        }
    }
}
