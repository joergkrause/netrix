using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.Design;
using System.Web.UI;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.Events;
using System.Collections.Generic;

namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// The purpose of this class is the management of namespaces.
    /// </summary>
    /// <remarks>
    /// Any element containing a namespace alias, like &lt;asp:button&gt; can have its own designer behavior.
    /// The namespace manager is called if this kind of element occurs and is supposed to attach a designer to 
    /// that element. Once the designer is attached, the renderer usese the designer to request the design time
    /// behavior. For applicable designers, various plug-ins are attachable.
    /// <seealso cref="GuruComponents.Netrix.Designer.IDesignTimeBehavior"/>
    /// <seealso cref="GuruComponents.Netrix.Designer.CommandWrapper"/>
    /// </remarks>
    sealed class NamespaceManager :
                IDisposable,
                Interop.IElementBehaviorFactory,
                Interop.IElementNamespaceFactory,
                Interop.IElementNamespaceFactoryCallback,
                INamespaceManager
    {

        private HtmlEditor _editor;
        private Dictionary<string, string> _registeredNamespaces;
        private Dictionary<string, Type> _registeredBehaviors;
        private Dictionary<Interop.IHTMLElement, IBaseBehavior> _behaviorInstances;
        private Interop.IElementNamespaceTable _namespaceTable;
        private bool saving;

        /// <summary>
        /// Returns a list of registered namespace aliases.
        /// </summary>
        internal ICollection AliasList
        {
            get
            {
                return _registeredNamespaces.Keys;
            }
        }

        /// <summary>
        /// Returns a list of registered namespaces.
        /// </summary>
        internal ICollection NamespaceList
        {
            get
            {
                return _registeredNamespaces.Values;
            }
        }

        internal NamespaceManager(IHtmlEditor editor)
        {
            _editor = (HtmlEditor)editor;
            _registeredNamespaces = new Dictionary<string, string>();
            _registeredBehaviors = new Dictionary<string, Type>();
            _behaviorInstances = new Dictionary<Interop.IHTMLElement, IBaseBehavior>();
            _editor.Saving += new SaveEventHandler(_editor_Saving);
            _editor.Saved += new SaveEventHandler(_editor_Saved);
        }
        void _editor_Saved(object sender, SaveEventArgs e)
        {
            saving = false;
        }

        void _editor_Saving(object sender, SaveEventArgs e)
        {
            saving = true;
        }

        public IBaseBehavior GetBehaviorOfElement(Interop.IHTMLElement element)
        {
            if (_behaviorInstances.ContainsKey(element))
            {
                return _behaviorInstances[element];
            }
            else
            {
                return null;
            }
        }

        public void Clear()
        {
            _registeredNamespaces.Clear();
            ClearBehaviors();
        }

        public void ClearBehaviors()
        {
            foreach (KeyValuePair<Interop.IHTMLElement, IBaseBehavior> dtb in _behaviorInstances)
            {
                if (dtb.Value is IDisposable)
                {
                    ((IDisposable)dtb.Value).Dispose();
                }
            }
            _registeredBehaviors.Clear();
        }

        private bool RegisterNamespaceWithEditor(string namespaceName)
        {
            if (_namespaceTable == null)
            {
                Interop.IOleServiceProvider interop_IOleServiceProvider = _editor.GetActiveDocument(true) as Interop.IOleServiceProvider;
                Guid guid1 = typeof(Interop.IElementNamespaceTable).GUID;
                Guid guid2 = guid1;
                IntPtr i;
                if (interop_IOleServiceProvider != null && interop_IOleServiceProvider.QueryService(ref guid1, ref guid2, out i) == 0 && i != Interop.NullIntPtr)
                {
                    _namespaceTable = (Interop.IElementNamespaceTable)Marshal.GetObjectForIUnknown(i);
                    Marshal.Release(i);
                }
                else
                {
                    // cannot register, document not ready
                    return false;
                }
            }
            if (_namespaceTable == null)
            {
                return false;
            }
            object local = this;
            try
            {
                _namespaceTable.AddNamespace(namespaceName, null, 2, ref local);
                return true;
            }
            catch (Exception)
            {
                bool flag = false;
                return flag;
            }
        }

        /// <summary>
        /// Returns the list of registered namespaces as dictionary with alias:ns pairs.
        /// </summary>
        internal IDictionary RegisteredNamespaces
        {
            get
            {
                return this._registeredNamespaces;
            }
        }

        /// <summary>
        /// Registers a namespace 
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="namespaceName"></param>
        public void RegisterNamespace(string alias, string namespaceName)
        {
            if (!_registeredNamespaces.ContainsKey(alias) && RegisterNamespaceWithEditor(alias))
            {
                _registeredNamespaces[alias] = namespaceName;
            }
        }

        /// <summary>
        /// Registers a namespace and the callback class which is responsible for the painting of all
        /// elements that are associated with the namespace.
        /// </summary>
        /// <remarks>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Behaviors.IBaseBehavior"/>
        /// </remarks>
        /// <param name="alias">Alias for namespace, like "asp" for ASP.NET objects.</param>
        /// <param name="namespaceName">URN of namespace, it's allowed to set to empty string if not required.</param>
        /// <param name="behavior">Callback object of type IBaseBehavior and any derived class, which has the right painting methods.</param>
        public void RegisterNamespace(string alias, string namespaceName, Type behavior)
        {
            RegisterNamespace(alias, namespaceName);
            if (!_registeredBehaviors.ContainsKey(alias))
            {
                _registeredBehaviors.Add(alias, behavior);
            }
        }

        void IDisposable.Dispose()
        {
            _namespaceTable = null;
            _registeredNamespaces = null;
            _registeredBehaviors = null;
            _editor = null;
        }

        void Interop.IElementNamespaceFactory.Create(Interop.IElementNamespace pNamespace)
        {

        }

        void Interop.IElementNamespaceFactoryCallback.Resolve(string namespaceName, string tagName, string attributes, Interop.IElementNamespace pNamespace)
        {
            if (_registeredNamespaces.ContainsKey(namespaceName))
            {
                int flag = 0;
                // we read the attributes probably used for XML elements to control the render behavior
                // ElementDescriptor controls "literalcontent" attribute (Default=false, Literal=true, NestedLiteral=nested)
                Type t = _editor.GenericElementFactory.GetElementType(String.Format("{0}:{1}", namespaceName, tagName));
                if (t != null)
                {
                    object[] o = t.GetCustomAttributes(typeof(ElementDescriptorAttribute), true);
                    if (o != null && o.Length == 1)
                    {
                        ElementDescriptorAttribute eda = o[0] as ElementDescriptorAttribute;
                        if (eda != null)
                        {
                            flag = (int)eda.Descriptor;
                        }
                    }
                }
                pNamespace.AddTag(tagName, flag); // LiteralContent = true/false/nested
            }
        }

        /// <summary>
        /// We're looking recursively for the latest instance of the element, to get the one we really need to render.
        /// To achieve this, we're rendering when either the parent is a custom tag too or the body has been reached.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool IsRenderable(Interop.IHTMLElement element)
        {
            Interop.IHTMLElement parent = element.GetParentElement();
            ArrayList alias = new ArrayList(_registeredNamespaces.Keys);
            //if (parent.GetTagName() == "BODY") return true;
            while (parent != null && parent.GetTagName() != "BODY")
            {
                if (alias.Contains(((Interop.IHTMLElement2)parent).GetScopeName()))
                {
                    return false;
                }
                parent = parent.GetParentElement();
                //if (parent.GetTagName() == "BODY") return true;
            }
            return true;
        }

        /// <summary>
        /// this version supports global (per namespace) designers only, for &lt;asp:hyperlink> it is "asp" bstrBehavior contains the 
        /// element name, for &lt;asp:hyperlink> it is "hyperlink".
        /// </summary>
        /// <param name="bstrBehavior"></param>
        /// <param name="bstrBehaviorUrl"></param>
        /// <param name="pSite"></param>
        /// <returns></returns>
        Interop.IElementBehavior Interop.IElementBehaviorFactory.FindBehavior(string bstrBehavior, string bstrBehaviorUrl, Interop.IElementBehaviorSite pSite)
        {
            if (saving) return null;
            Interop.IHTMLElement element = pSite.GetElement();
            if (!IsRenderable(element)) return null;
            string tagPrefix = ((Interop.IHTMLElement2)element).GetScopeName();
            if (!_registeredBehaviors.ContainsKey(tagPrefix)) return null;
            if (_registeredBehaviors[tagPrefix] == null) return null;
            IBaseBehavior o = Activator.CreateInstance(_registeredBehaviors[tagPrefix], _editor) as IBaseBehavior;
            _behaviorInstances.Add(element, o); // just for cleaning up
            string f = String.Concat(tagPrefix, ":", element.GetTagName());
            if (((GuruComponents.Netrix.ElementFactory)((HtmlEditor)this._editor).GenericElementFactory).IsRegistered(f))
                return (IBaseBehavior)o;
            else
                return null;
        }

        internal IDesignTimeBehavior GetBehaviorOfScope(string tagPrefix)
        {
            if (_registeredBehaviors.ContainsKey(tagPrefix) && _registeredBehaviors[tagPrefix] != null)
            {
                return _registeredBehaviors[tagPrefix] as IDesignTimeBehavior;
            }
            else
            {
                return null;
            }
        }

    }
}