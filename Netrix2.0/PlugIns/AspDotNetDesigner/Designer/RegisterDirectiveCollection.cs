using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Web.UI;

namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Supports the register directives for ASP.NET as well as user controls.
    /// </summary>
    public sealed class RegisterDirectiveCollection: ICollection, IRegisterDirectiveCollection
    {
        private const string ASPNAMESPACE = "System.Web.UI.WebControls";

        private const string ASPASSEMBLY = "System.Web";

        /// <summary>
        /// asp
        /// </summary>
        public const string ASPTAGPREFIX = "asp";

        private HybridDictionary _registerDirectiveTable;

        private string _registerDirectivesText;

        /// <summary>
        /// Number of directives.
        /// </summary>
        public int Count
        {
            get
            {
                return _registerDirectiveTable.Count;
            }
        }

        /// <summary>
        /// Readonly, always false.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Sync always false.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Always null.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// Directive
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <returns></returns>
        public IRegisterDirective this[string tagPrefix]
        {
            get
            {
                return GetRegisterDirective(tagPrefix, null);
            }
        }
        /// <summary>
        /// Directive
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public IRegisterDirective this[string tagPrefix, string tagName]
        {
            get
            {
                return GetRegisterDirective(tagPrefix, tagName);
            }
        }
        /// <summary>
        /// Added event
        /// </summary>
        public event DirectiveEventHandler DirectiveAdded;
        /// <summary>
        /// ctor
        /// </summary>
        public RegisterDirectiveCollection()
        {
            _registerDirectiveTable = new HybridDictionary();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="directive"></param>
        internal void AddParsedRegisterDirective(IRegisterDirective directive)
        {
            AddRegisterDirective(directive, false);
        }
        /// <summary>
        /// Add directive.
        /// </summary>
        /// <param name="directive"></param>
        public void AddRegisterDirective(IRegisterDirective directive)
        {
            AddRegisterDirective(directive, true);
        }
        /// <summary>
        /// Add directive.
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="raiseEvent"></param>
        public void AddRegisterDirective(IRegisterDirective directive, bool raiseEvent)
        {
            string str = directive.TagPrefix;
            if (directive.IsUserControl)
            {
                str = String.Concat(str, ":", directive.TagName);
            }
            bool flag = _registerDirectiveTable.Contains(str.ToLower());
            _registerDirectiveTable[str.ToLower()] = directive;
            _registerDirectivesText = null;
            if (!flag && raiseEvent && DirectiveAdded != null)
            {
                DirectiveAdded(this, new DirectiveEventArgs(directive as IDirective));
            }
        }
        /// <summary>
        /// copy to array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            _registerDirectiveTable.Values.CopyTo(array, index);
        }
        /// <summary>
        /// clear table
        /// </summary>
        public void Clear()
        {
            _registerDirectiveTable.Clear();
            _registerDirectivesText = null;
        }
        /// <summary>
        /// enumerate
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _registerDirectiveTable.Values.GetEnumerator();
        }
        /// <summary>
        /// Get directive.
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public IRegisterDirective GetRegisterDirective(string tagPrefix, string tagName)
        {
            tagPrefix = tagPrefix.ToLower();
            if (tagName != null)
            {
                tagName = tagName.ToLower();
            }
            RegisterDirective registerDirective = (RegisterDirective)_registerDirectiveTable[tagPrefix];
            if (registerDirective == null && tagName != null && tagName.Length != 0)
            {
                registerDirective = (RegisterDirective)_registerDirectiveTable[String.Concat(tagPrefix, ":", tagName)];
            }
            return registerDirective as IRegisterDirective;
        }
        /// <summary>
        /// Get object type
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="tagPrefix"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type GetObjectType(IServiceProvider serviceProvider, string tagPrefix, string typeName)
        {
            if (String.Compare(tagPrefix, "asp", true) == 0)
            {
                return Type.GetType(String.Concat(ASPNAMESPACE, ".", typeName, ", ", ASPASSEMBLY), false, true);
            }
            IRegisterDirective registerDirective = GetRegisterDirective(tagPrefix, typeName);
            if (registerDirective == null)
            {
                return null;
            }
            Type type = null;
            if (!registerDirective.IsUserControl)
            {
                string str = String.Concat(new string[]{registerDirective.NamespaceName, ".", typeName, ", ", registerDirective.AssemblyName});

                //TODO: Make type out of file
                //DesignTimeParseData data = new DesignTimeParseData(host, parseText);
                //Control c = DesignTimeTemplateParser.ParseControl(data);
                //directive.ObjectType

                type = ((ITypeResolutionService)serviceProvider.GetService(typeof(ITypeResolutionService))).GetType(str, false, true);
            }
            else
            {
                type = typeof(UserControl);
            }
            return type;
        }
        /// <summary>
        /// get prefix
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public string GetTagPrefix(Type objectType)
        {
            string otNamespace = objectType.Namespace;
            string otAssemname = objectType.Assembly.GetName().Name;
            if (otNamespace != null && otNamespace.Equals(ASPNAMESPACE) && otAssemname.Equals(ASPASSEMBLY))
            {
                return ASPTAGPREFIX;
            }
            IEnumerator iEnumerator = _registerDirectiveTable.Values.GetEnumerator();
            try
            {
                while (iEnumerator.MoveNext())
                {
                    RegisterDirective registerDirective = (RegisterDirective)iEnumerator.Current;
                    if (registerDirective.ObjectType != null)
                    {
                        if (String.Compare(registerDirective.ObjectType.FullName, objectType.FullName, true) == 0)
                        {
                            return registerDirective.TagPrefix;
                        }
                    }
                    if (!registerDirective.IsUserControl && String.Compare(registerDirective.NamespaceName, otNamespace, true) == 0 && String.Compare(registerDirective.AssemblyName, otAssemname, true) == 0)
                    {
                        return registerDirective.TagPrefix;
                    }
                }
            }
            finally
            {
                IDisposable iDisposable = iEnumerator as IDisposable;
                if (iDisposable != null)
                {
                    iDisposable.Dispose();
                }
            }
            return null;
        }
        /// <summary>
        /// get tag name
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public string GetTagName(Type objectType)
        {
            string otNamespace = objectType.Namespace;
            string otAssemname = objectType.Assembly.GetName().Name;
            IEnumerator iEnumerator = _registerDirectiveTable.Values.GetEnumerator();
            try
            {
                while (iEnumerator.MoveNext())
                {
                    RegisterDirective registerDirective = (RegisterDirective)iEnumerator.Current;
                    if (registerDirective.ObjectType != null)
                    {
                        if (String.Compare(registerDirective.ObjectType.FullName, objectType.FullName, true) == 0)
                        {
                            if (!String.IsNullOrEmpty(registerDirective.TagName))
                            {
                                return registerDirective.TagName;
                            }
                            else
                            {
                                return objectType.Name;
                            }
                        }
                    }
                    if (!registerDirective.IsUserControl && String.Compare(registerDirective.NamespaceName, otNamespace, true) == 0 && String.Compare(registerDirective.AssemblyName, otAssemname, true) == 0)
                    {
                        return registerDirective.TagName;
                    }
                }
            }
            finally
            {
                IDisposable iDisposable = iEnumerator as IDisposable;
                if (iDisposable != null)
                {
                    iDisposable.Dispose();
                }
            }
            return null;
        }
        
        /// <summary>
        /// Makes register directives string representation.
        /// </summary>
        /// <remarks>
        /// Returns things like &lt;%@ Register TagPrefix=\"uc1\" TagName=\"AvanteamTextField\" Src=\"Avanteam.Library.WebRenderFields\" %&gt;.
        /// </remarks>
        /// <returns></returns>
        public override string ToString()
        {
            if (_registerDirectivesText == null)
            {
                _registerDirectivesText = String.Empty;
                IEnumerator iEnumerator = _registerDirectiveTable.Values.GetEnumerator();
                try
                {
                    while (iEnumerator.MoveNext())
                    {
                        RegisterDirective registerDirective = (RegisterDirective)iEnumerator.Current;
                        _registerDirectivesText = String.Concat(_registerDirectivesText, registerDirective.ToString());
                    }
                }
                finally
                {
                    IDisposable iDisposable = iEnumerator as IDisposable;
                    if (iDisposable != null)
                    {
                        iDisposable.Dispose();
                    }
                }
            }
            return _registerDirectivesText;
        }

    }

}
