using System;
using System.Collections;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.Design;

namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Aclass for accessing the types, directives, and controls in the current Web project document.
    /// </summary>
    public class WebFormsReferenceManager: System.Web.UI.Design.WebFormsReferenceManager, IReferenceManager, IDisposable
    {
        private IServiceProvider _serviceProvider;
        private RegisterDirectiveCollection _registerDirectives;

        private int _tagPrefixCounter;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <param name="registerDirectives"></param>
        public WebFormsReferenceManager(IHtmlEditor htmlEditor, RegisterDirectiveCollection registerDirectives) //, IDesignTimeBehavior behavior)
        {
            if (htmlEditor.ServiceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            if (registerDirectives == null)
            {
                throw new ArgumentNullException("registerDirectives");
            }
            _serviceProvider = htmlEditor.ServiceProvider;
            _registerDirectives = registerDirectives;
        }

        void IDisposable.Dispose()
        {
            _serviceProvider = null;
            _registerDirectives = null;
        }

        /// <summary>
        /// type
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type GetObjectType(string tagPrefix, string typeName)
        {
            return _registerDirectives.GetObjectType(_serviceProvider, tagPrefix, typeName);
        }

        string IWebFormReferenceManager.GetRegisterDirectives()
        {
            return _registerDirectives.ToString();
        }

        string IWebFormReferenceManager.GetTagPrefix(Type objectType)
        {
            string prefix = _registerDirectives.GetTagPrefix(objectType);
            if (prefix == null)
            {
                Assembly assembly = objectType.Assembly;
                object[] locals1 = assembly.GetCustomAttributes(typeof(TagPrefixAttribute), true);
                string str2 = objectType.Namespace;
                bool flag = false;
                object[] locals2 = locals1;
                for (int i = 0; i < locals2.Length; i++)
                {
                    TagPrefixAttribute tagPrefixAttribute = (TagPrefixAttribute)locals2[i];
                    if (str2.Equals(tagPrefixAttribute.NamespaceName))
                    {
                        prefix = tagPrefixAttribute.TagPrefix;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    int i = _tagPrefixCounter++;
                    prefix = String.Concat("n", i);
                }
                string str3;
                if (assembly.GlobalAssemblyCache)
                {
                    str3 = assembly.GetName().FullName;
                }
                else
                {
                    str3 = assembly.GetName().Name;
                }
                _registerDirectives.AddRegisterDirective(new RegisterDirective(prefix, str2, str3));
            }
            return prefix;
        }
        /// <summary>
        /// The type name.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public string GetTagName(Type objectType)
        {
            return _registerDirectives.GetTagName(objectType);
        }

        #region IRegisterDirectiveCollection Members

        /// <summary>
        /// Add a directive.
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="raiseEvent"></param>
        public void AddRegisterDirective(IRegisterDirective directive, bool raiseEvent)
        {
            _registerDirectives.AddRegisterDirective(directive, raiseEvent);
            InvokeDirectiveAdded(directive as IDirective);
        }

        /// <summary>
        /// Add a directive
        /// </summary>
        /// <param name="directive"></param>
        public void AddRegisterDirective(IRegisterDirective directive)
        {
            _registerDirectives.AddRegisterDirective(directive);
            InvokeDirectiveAdded(directive as IDirective);
        }

        private void InvokeDirectiveAdded(IDirective directive)
        {
            if (DirectiveAdded != null)
            {
                DirectiveAdded(this, new DirectiveEventArgs(directive));
            }
        }
        /// <summary>
        /// Fired if a new directive has been added.
        /// </summary>
        public event DirectiveEventHandler DirectiveAdded;
        /// <summary>
        /// The Type.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="tagPrefix"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type GetObjectType(IServiceProvider serviceProvider, string tagPrefix, string typeName)
        {
            return _registerDirectives.GetObjectType(serviceProvider, tagPrefix, typeName);
        }
        /// <summary>
        /// The directive.
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public IRegisterDirective GetRegisterDirective(string tagPrefix, string tagName)
        {
            return _registerDirectives.GetRegisterDirective(tagPrefix, tagName);
        }
        /// <summary>
        /// The directive.
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <returns></returns>
        public IRegisterDirective this[string tagPrefix]
        {
            get {
                return _registerDirectives[tagPrefix]; }
        }

        /// <summary>
        /// The directive.
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public IRegisterDirective this[string tagPrefix, string tagName]
        {
            get {
                return _registerDirectives[tagPrefix, tagName]; }
        }

        #endregion

        /// <summary>
        /// directives
        /// </summary>
        /// <returns></returns>
        public override ICollection GetRegisterDirectives()
        {
            string[] hd = new string[_registerDirectives.Count];
            int i = 0;
            foreach (RegisterDirective de in _registerDirectives)
            {
                hd[i++] = de.ToString();
            }
            return hd;
        }
        /// <summary>
        /// prefix
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override string GetTagPrefix(Type objectType)
        {
            return _registerDirectives.GetTagPrefix(objectType);
        } 

        /// <summary>
        /// type
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public override Type GetType(string tagPrefix, string tagName)
        {
            return GetObjectType(_serviceProvider, tagPrefix, tagName);   
        }
        /// <summary>
        /// control path
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public override string GetUserControlPath(string tagPrefix, string tagName)
        {
            foreach (Directive d in _registerDirectives)
            {
                if (d is RegisterDirective)
                {
                    if (((RegisterDirective)d).TagPrefix == tagPrefix && ((RegisterDirective)d).TagName == tagName)
                    {
                        return ((RegisterDirective)d).SourceFile;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// "asp"
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override string RegisterTagPrefix(Type objectType)
        {            
            return "asp";
        }
    }
}
