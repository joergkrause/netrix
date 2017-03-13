using System;
using System.Collections;
using System.Xml;
using System.Configuration;

namespace GuruComponents.Netrix.PlugIns
{
	/// <summary>
	/// This class uses the App.Config of host application to detect plugins.
	/// </summary>
	public class NetRixConfiguration : IConfigurationSectionHandler
	{
        /// <summary>
        /// List of loaded Plugins.
        /// </summary>
	    public ArrayList Plugins;

        /// <summary>
        /// Plugin information store.
        /// </summary>
        public struct PlugIn
        {

            private ArrayList t;
            /// <summary>
            /// Plugin types list.
            /// </summary>
            public Type[] Types
            {
                get
                {
                    Type[] tArray = new Type[t.Count];
                    t.CopyTo(tArray);
                    return tArray;
                }
            }
            /// <summary>
            /// Name
            /// </summary>
            public string Name;
            /// <summary>
            /// Add type
            /// </summary>
            /// <param name="typeName"></param>
            /// <param name="assembly"></param>
            public void AddType(string typeName, string assembly)
            {
                if (t == null)
                {
                    t = new ArrayList();
                }
                Type type = Type.GetType(String.Format("{0}.{1},{0}", assembly, typeName), false, true);
                if (type != null)
                {
                    t.Add(type);
                }
            }
        }

        #region IConfigurationSectionHandler Member

        /// <summary>
        /// Create plugin entry.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // assuming the Node has one or more <PluginAssembly> nodes, which each have one or more <Type> nodes
            PlugIn p;
            //= new Plugin();

            XmlNodeList list = section.SelectNodes("//PluginAssembly");

            Plugins = new ArrayList();
            foreach (XmlNode n in list)
            {
                p = new PlugIn();
                p.Name = n.Attributes["name"].Value;
                foreach (XmlNode tn in n.SelectNodes("Type"))
                {
                    p.AddType(tn.Attributes["name"].Value, p.Name);                    
                }
                Plugins.Add(p);
            }

            return RegisteredPlugins;
        }

        #endregion

        /// <summary>
        /// Returns Plugin-Objects vor each assembly, containing an array of registered types.
        /// </summary>
        private PlugIn[] RegisteredPlugins
        {
            get
            {
                PlugIn[] p = new PlugIn[Plugins.Count];
                Plugins.CopyTo(p);
                return p;
            }
        }

    }
}
