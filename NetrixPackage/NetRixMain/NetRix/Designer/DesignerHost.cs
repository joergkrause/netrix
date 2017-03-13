using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Web.UI;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Collections.Generic;

namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// DesignerHost creates the designer to add WebForms controls. 
    /// </summary>
    /// <remarks>
    /// The designer host is responsible for all designer related tasks and design time features. Design time in the sense
    /// of this control means running the NetRix control in its "Design Mode" (compared with "Browse Mode"). NetRix provides
    /// a fully implemented design time environment at host applications runtime. The interfaces implemented by this class
    /// as well as several others form this environment. Components such as PropertyGrid are being able to use this by calling
    /// services (classes exposed as services are available through the ServiceProvider) or using reflection to indicate
    /// features added as attributes.
    /// </remarks>
    public class DesignerHost :
        IDesignerHost,
        IContainer,
        IComponentChangeService,
        ITypeDescriptorFilterService,
        ITypeResolutionService
    {

        private Dictionary<string, IComponent> components;
        private Dictionary<IComponent, IDesigner> designers;

        private IComponent rootComponent;
        private IDesigner rootDesigner;     // TODO: Let Extender setup it's own rootdesigner
        private IComponent documentComponent;
        private IUIService uiservice;
        private IHtmlEditor editor;

        private bool loading = false;

        /// <summary>
        /// Creates a new instance of the designer that hosts all components.
        /// </summary>
        /// <param name="editor">Related NetRix editor control.</param>
        public DesignerHost(IHtmlEditor editor)
        {
            this.editor = editor;
            this.components = new Dictionary<string, IComponent>();
            this.designers = new Dictionary<IComponent, IDesigner>();
            // Add internally provides services
            editor.ServiceProvider.AddService(typeof(IDesignerHost), this);
            editor.ServiceProvider.AddService(typeof(IContainer), this);
            editor.ServiceProvider.AddService(typeof(IComponentChangeService), this);
            editor.ServiceProvider.AddService(typeof(ITypeDescriptorFilterService), this);
            editor.ServiceProvider.AddService(typeof(ITypeResolutionService), this);

            this.uiservice = new UIService(editor);
            editor.ServiceProvider.AddService(typeof(System.Windows.Forms.Design.IUIService), uiservice);
        }

        #region IDesignerHost Member

        /// <summary>
        /// Exposes this as a container for components.
        /// </summary>
        public IContainer Container
        {
            get
            {
                return this;
            }
        }


        # region Events

        event EventHandler IDesignerHost.TransactionOpening { add { } remove { } }

        event EventHandler IDesignerHost.TransactionOpened { add { } remove { } }

        public event EventHandler LoadComplete;

        event EventHandler IDesignerHost.Activated { add { } remove { } }

        event EventHandler IDesignerHost.Deactivated { add { } remove { } }

        event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosed { add { } remove { } }

        # endregion

        public IDesigner GetDesigner(IComponent component)
        {
            if (component == null) return null;
            if (designers.ContainsKey(component))
                return (IDesigner)designers[component];
            else
                return null;
        }

        public bool Loading
        {
            get
            {
                return loading;
            }
        }

        public void OnLoadComplete()
        {
            if (LoadComplete != null)
            {
                LoadComplete(this, EventArgs.Empty);
            }
        }

        public IComponent CreateComponent(Type componentClass, string name)
        {
            if (componentClass == null)
            {
                throw new ArgumentNullException("Cannot Create Component of Type null");
            }
            IComponent component;
            IServiceProvider sp = this;
            object local = null;
            try
            {
                object[] args = new object[] { editor };
                Type[] types = new Type[] { typeof(IHtmlEditor) };
                local = this.CreateObject(componentClass, args, types);
            }
            catch
            {
            }
            if (local == null)
            {
                local = this.CreateObject(componentClass, null, null);
            }
            component = local as IComponent;
            if (component == null)
            {
                throw new ArgumentException(componentClass.FullName + " is not IComponent");
            }
            if (!(component.Site is DesignSite))
            {
                Add(component, name);
            }
            return component;
        }

        IComponent IDesignerHost.CreateComponent(Type componentClass)
        {
            string str = GetNewComponentName(componentClass);
            return CreateComponent(componentClass, str);
        }

        public bool InTransaction
        {
            get
            {
                return false;
            }
        }

        public string TransactionDescription
        {
            get
            {
                return null;
            }
        }

        public void DestroyComponent(IComponent component)
        {
            // TODO:  Implementierung von DesignerHost.DestroyComponent hinzufügen
        }

        public void Activate()
        {
            // TODO:  Implementierung von DesignerHost.Activate hinzufügen
        }

        public string RootComponentClassName
        {
            get
            {
                return rootComponent.GetType().FullName;
            }
        }

        public Type GetType(string typeName)
        {
            return Type.GetType(typeName);
        }

        event DesignerTransactionCloseEventHandler IDesignerHost.TransactionClosing { add { } remove { } }

        public DesignerTransaction CreateTransaction(string description)
        {
            // TODO:  Implementierung von DesignerHost.CreateTransaction hinzufügen
            return null;
        }

        DesignerTransaction IDesignerHost.CreateTransaction()
        {
            // TODO:  Implementierung von DesignerHost.System.ComponentModel.Design.IDesignerHost.CreateTransaction hinzufügen
            return null;
        }

        public IComponent RootComponent
        {
            get
            {
                if (rootComponent == null)
                {
                    rootComponent = new Page();
                    string name = ((Page)rootComponent).UniqueID;
                    rootComponent.Site = new DesignSite(this, name);
                    ((DesignSite)rootComponent.Site).SetComponent(rootComponent);
                    rootDesigner = CreateComponentDesigner(rootComponent, typeof(Page));
                    if (rootDesigner != null)
                    {
                        rootDesigner.Initialize(rootComponent);
                        designers[rootComponent] = rootDesigner;
                    }
                    else
                    {
                        rootDesigner = new AspWebFormsRootDesigner(editor.ServiceProvider);
                        designers[rootComponent] = rootDesigner;
                    }
                    components[name] = rootComponent;
                }
                return rootComponent;
            }
        }

        #endregion

        #region IServiceContainer Member

        public void RemoveService(Type serviceType, bool promote)
        {
            editor.ServiceProvider.RemoveService(serviceType, promote);
        }

        void IServiceContainer.RemoveService(Type serviceType)
        {
            editor.ServiceProvider.RemoveService(serviceType);
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            editor.ServiceProvider.AddService(serviceType, callback, promote);
        }

        void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            editor.ServiceProvider.AddService(serviceType, callback);
        }

        void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote)
        {
            editor.ServiceProvider.AddService(serviceType, serviceInstance, promote);
        }

        void IServiceContainer.AddService(Type serviceType, object serviceInstance)
        {
            editor.ServiceProvider.AddService(serviceType, serviceInstance);
        }

        #endregion

        #region IServiceProvider Member

        public object GetService(Type serviceType)
        {
            if (editor.ServiceProvider != null)
            {
                object service = editor.ServiceProvider.GetService(serviceType);
                //if (service == null)
                //{
                //    System.Diagnostics.Debug.WriteLine(serviceType.ToString());
                //}
                return service;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region IContainer Member

        public void Clear()
        {
            components.Clear();
            designers.Clear();
        }

        ComponentCollection cc = null;

        ComponentCollection IContainer.Components
        {
            get
            {
                if (cc == null)
                {
                    IComponent[] c = new IComponent[components.Count];
                    components.Values.CopyTo(c, 0);
                    cc = new ComponentCollection(c);
                }
                return cc;
            }
        }

        /// <summary>
        /// Return the component with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IComponent this[string name]
        {
            get
            {
                if (components.ContainsKey(name))
                {
                    return (IComponent)components[name];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Remove a component. Does not remove connected element from DOM.
        /// </summary>
        /// <remarks>To remove from DOM, use the <see cref="ComponentRemoved"/> event and ElementNode operations in the event handler.</remarks>
        /// <param name="component"></param>
        public void Remove(IComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("Cannot Remove Component of Value Null.");
            }
            if (component.Site == null)
            {
                return;
            }
            string str = component.Site.Name;
            if (str != null && components.ContainsKey(str) && components[str] == component)
            {
                if (ComponentRemoving != null)
                {
                    ComponentRemoving(this, new ComponentEventArgs(component));
                }
                if (designers != null)
                {
                    IDesigner iDesigner = (IDesigner)designers[component];
                    if (iDesigner != null)
                    {
                        designers.Remove(component);
                        iDesigner.Dispose();
                    }
                }
                components.Remove(str);
                component.Dispose();
                try
                {
                    if (ComponentRemoved != null)
                    {
                        ComponentRemoved(this, new ComponentEventArgs(component));
                    }
                }
                catch
                {
                }
                component.Site = null;
            }
        }

        /// <summary>
        /// Add a component. Used internally. Does not add anything to the document.
        /// </summary>
        /// <param name="component">The component</param>
        /// <param name="uniquename">The internal (HTML) name, MSHTML internally generates for the generic element. Such as 'ms_id3'.</param>
        public void Add(IComponent component, string uniquename)
        {
            //System.Diagnostics.Debug.WriteLine(component.GetHashCode(), "HASH ON DESIGNER");
            if (component == null)
            {
                throw new ArgumentNullException("Cannot Add Component with Value of Null.");
            }
            if (uniquename == null)
            {
                throw new ArgumentNullException("Component must have a unique name");
            }
            string name = null;
            ISite iSite = component.Site;
            if (iSite != null && iSite.Name != null)
            {
                name = iSite.Name;
            }
            if (component is Control)
            {
                name = ((Control)component).ID;
            }
            if (name == null)
            {
                name = GetNewComponentName(component.GetType());
            }
            //string uniqueName = GetNewComponentName(component.GetType());
            if (iSite != null && iSite.Container == this && name != null)
            {
                iSite.Name = name;
            }
            if (!IsValidComponentName(name) && !(component is IElement))
            {
                throw new ArgumentException(String.Concat("\'", name, "\' is not a valid name"));
            }
            if (components.ContainsKey(uniquename))
            {
                components.Remove(uniquename);
            }
            else
            {
                // if an element is dropped after drag drop it's new as well, so we have to remove the former instance by ID
                foreach (KeyValuePair<string, IComponent> cmpnt in components)
                {
                    if (cmpnt.Value is Control)
                    {
                        if (name == ((Control)cmpnt.Value).ID)
                        {
                            if (cmpnt.Value.Site != null)
                            {
                                components.Remove(cmpnt.Value.Site.Name);
                                break;
                            }
                        }
                    }
                }
            } 
            if (iSite != null)
            {
                iSite.Container.Remove(component);
            }
            if (ComponentAdding != null)
            {
                ComponentAdding(this, new ComponentEventArgs(component));
            }
            //
            IServiceProvider iServiceProvider = this;
            DesignSite designSite = new DesignSite(iServiceProvider, uniquename);
            // Now we handle the root component
            Type type = typeof(IDesigner);
            if (documentComponent != null && rootComponent == null)
            {
                type = typeof(IRootDesigner);
            }
            IDesigner iDesigner = CreateComponentDesigner(component, type);
            if (iDesigner == null)
            {
                throw new Exception(String.Concat("Unable to create a designer for component of type ", component.GetType().FullName));
            }
            if (documentComponent == null &&
                (component is IElement && ((IElement)component).TagName.Equals(DocumentComponentName))
                )
            {
                documentComponent = component;
            }
            else if (rootComponent == null)
            {
                if (component is Page)
                {
                    rootComponent = component;
                }
                else
                {
                    rootComponent = new Page();
                }
                rootComponent.Site = new DesignSite(this, uniquename);
                ((DesignSite)rootComponent.Site).SetComponent(rootComponent);
                rootDesigner = CreateComponentDesigner(rootComponent, typeof(Page));
                if (rootDesigner != null)
                {
                    rootDesigner.Initialize(rootComponent);
                    designers[rootComponent] = rootDesigner;
                }
                else
                {
                    rootDesigner = new AspWebFormsRootDesigner(editor.ServiceProvider);
                    designers[rootComponent] = rootDesigner;
                }
                components[uniquename] = rootComponent;
            }
            designSite.SetComponent(component);
            component.Site = designSite;
            components[uniquename] = component;
            iDesigner.Initialize(component);
            designers[component] = iDesigner;
            cc = null;
            if (ComponentAdded != null)
            {
                ComponentAdded(this, new ComponentEventArgs(component));
            }
        }

        void IContainer.Add(IComponent component)
        {
            this.Add(component, null);
        }

        #endregion

        #region IDisposable Member

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion

        #region IComponentChangeService Member

        void IComponentChangeService.OnComponentChanged(object component, MemberDescriptor member, object oldValue, object newValue)
        {
            if (loading)
            { return; }
            if (ComponentChanged != null)
            {
                ComponentChanged(this, new ComponentChangedEventArgs(component, member, oldValue, newValue));
            }
        }

        void IComponentChangeService.OnComponentChanging(object component, MemberDescriptor member)
        {
            if (loading)
            { return; }
            if (ComponentChanging != null)
            {
                ComponentChanging(this, new ComponentChangingEventArgs(component, member));
            }
        }

        # region Events

        public event ComponentEventHandler ComponentRemoving;

        public event ComponentEventHandler ComponentAdded;

        public event ComponentRenameEventHandler ComponentRename;

        public event ComponentEventHandler ComponentAdding;

        public event ComponentEventHandler ComponentRemoved;

        public event ComponentChangingEventHandler ComponentChanging;

        public event ComponentChangedEventHandler ComponentChanged;

        # endregion

        #endregion

        #region ITypeDescriptorFilterService Member

        bool ITypeDescriptorFilterService.FilterProperties(IComponent component, IDictionary properties)
        {
            return false;
            //IDesigner iDesigner = GetDesigner(component);
            //if (iDesigner is IDesignerFilter)
            //{
            //    ((IDesignerFilter)iDesigner).PreFilterProperties(properties);
            //    ((IDesignerFilter)iDesigner).PostFilterProperties(properties);
            //}
            //return (iDesigner != null);
        }

        bool ITypeDescriptorFilterService.FilterAttributes(IComponent component, IDictionary attributes)
        {
            return false;
            //IDesigner iDesigner = GetDesigner(component);
            //if (iDesigner is IDesignerFilter)
            //{
            //    ((IDesignerFilter)iDesigner).PreFilterAttributes(attributes);
            //    ((IDesignerFilter)iDesigner).PostFilterAttributes(attributes);
            //}
            //return (iDesigner != null);
        }

        bool ITypeDescriptorFilterService.FilterEvents(IComponent component, IDictionary events)
        {
            return false;
            //IDesigner iDesigner = GetDesigner(component);
            //if (iDesigner is IDesignerFilter)
            //{
            //    ((IDesignerFilter)iDesigner).PreFilterEvents(events);
            //    ((IDesignerFilter)iDesigner).PostFilterEvents(events);
            //}
            //return (iDesigner != null);
        }

        #endregion

        # region ITypeResolutionService Member

        string ITypeResolutionService.GetPathOfAssembly(AssemblyName assemblyName)
        {
            return assemblyName.CodeBase;
        }

        Type ITypeResolutionService.GetType(string name)
        {
            return GetType(name, false, false);
        }

        Type ITypeResolutionService.GetType(string name, bool throwOnError)
        {
            return GetType(name, throwOnError, false);
        }

        public Type GetType(string name, bool throwOnError, bool ignoreCase)
        {
            return Type.GetType(name, throwOnError, ignoreCase);
        }

        Assembly ITypeResolutionService.GetAssembly(AssemblyName name)
        {
            return GetAssembly(name, false);
        }

        void ITypeResolutionService.ReferenceAssembly(AssemblyName name)
        {
            Assembly a = Assembly.Load(name.FullName);
            if (!this.cachedAssemblies.ContainsKey(name))
            {
                this.cachedAssemblies[name] = a;
            }
        }

        private Dictionary<AssemblyName, Assembly> cachedAssemblies;
        private AssemblyName[] names;

        private string GetPathOfAssembly(AssemblyName name)
        {
            return name.CodeBase;
        }

        public Assembly GetAssembly(AssemblyName name, bool throwOnError)
        {
            Assembly assembly = null;
            if (this.cachedAssemblies == null)
            {
                this.cachedAssemblies = new Dictionary<AssemblyName, Assembly>();
            }
            if (this.cachedAssemblies.ContainsKey(name))
            {
                assembly = this.cachedAssemblies[name] as Assembly;
            }
            if (assembly == null)
            {
                assembly = Assembly.Load(name.FullName);
                if (assembly != null)
                {
                    this.cachedAssemblies[name] = assembly;
                    return assembly;
                }
                if (this.names == null)
                {
                    return assembly;
                }
                for (int i = 0; i < this.names.Length; i++)
                {
                    if (name.Equals(this.names[i]))
                    {
                        try
                        {
                            assembly = Assembly.LoadFrom(this.GetPathOfAssembly(name));
                            if (assembly != null)
                            {
                                this.cachedAssemblies[name] = assembly;
                            }
                        }
                        catch
                        {
                            if (throwOnError)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            return assembly;

        }

        # endregion

        # region Helper methods

        private object CreateObject(Type objectType, object[] args, Type[] argTypes)
        {
            ConstructorInfo constructorInfo = null;
            if (args != null && argTypes.Length != 0)
            {
                constructorInfo = objectType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, argTypes, null);
            }
            try
            {
                object local;

                if (constructorInfo != null)
                {
                    local = constructorInfo.Invoke(args);
                }
                else
                {
                    IHtmlEditor baseEditor = GetService(typeof(IEditorInstanceService)) as IHtmlEditor;
                    local = Activator.CreateInstance(objectType, baseEditor);
                }
                return local;
            }
            catch (Exception e)
            {
                if (e is TargetInvocationException)
                {
                    e = e.InnerException;
                }
                throw new Exception(String.Concat("Cannot create an instance of type ", objectType.FullName), e);
            }
        }

        protected virtual string GetNewComponentName(Type componentType)
        {
            if (componentType == null)
            {
                throw new ArgumentNullException("componentType");
            }
            else
            {
                // check whether a plug-in has an INameCreationService implemented
                INameCreationService ns = ((INameCreationService)GetService(typeof(INameCreationService)));
                if (ns == null)
                {
                    // if not, use our own one
                    ns = new NameCreationService(editor);
                    editor.ServiceProvider.AddService(typeof(INameCreationService), ns);
                }
                return ns.CreateName(this, componentType);
            }
        }

        protected virtual bool IsValidComponentName(string name)
        {
            if (name == null || name.Length == 0)
            {
                return false;
            }
            else
            {
                INameCreationService ns = ((INameCreationService)GetService(typeof(INameCreationService)));
                if (ns == null)
                {
                    // if not, use our own one
                    ns = new NameCreationService(editor);
                    editor.ServiceProvider.AddService(typeof(INameCreationService), ns);
                }
                return ns.IsValidName(name);
            }
        }


        protected virtual IDesigner CreateComponentDesigner(IComponent component, Type designerType)
        {
            IDesigner t = TypeDescriptor.CreateDesigner(component, designerType);
            return t;
        }

        protected virtual string DocumentComponentName
        {
            get
            {
                return "Document";
            }
        }

        internal void RenameComponent(DesignSite site, string newName)
        {
            if (!IsValidComponentName(newName))
            {
                throw new ArgumentException(String.Concat("\'", newName, "\' is not a valid name for the component."));
            }
            if (components.ContainsKey(newName)) // new object does not has a name
            {
                throw new ArgumentException(String.Concat("A component with the name \'", newName, "\' already exists."));
            }
            string str = site.Name;
            components.Remove(str);
            components.Add(newName, site.Component);
            site.SetName(newName);
            if (ComponentRename != null)
            {
                ComponentRename(this, new ComponentRenameEventArgs(site.Component, str, newName));
            }
        }

        # endregion

    }
}
