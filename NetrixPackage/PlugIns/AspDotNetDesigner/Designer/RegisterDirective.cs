using System;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.Designer
{

    /// <summary>
    /// A class that handles the @Register directive.
    /// </summary>
    public sealed class RegisterDirective : Directive, IRegisterDirective
    {

        private Type referenceType;        

        /// <summary>
        /// Type
        /// </summary>
        public Type ObjectType 
        { 
            get 
            {
                if (IsUserControl)
                {
                    referenceType = typeof(AscxElement);
                }
                return referenceType; 
            }
            internal set 
            { 
                referenceType = value; 
            }
        }

        /// <summary>
        /// Assembly name
        /// </summary>
        public string AssemblyName
        {
            get
            {
                object local = base.Dictionary["assembly"];
                if (local == null)
                {
                    return "";
                }
                else
                {
                    return (String)local;
                }
            }
        }

        /// <summary>
        /// Returns "Register"
        /// </summary>
        public override string DirectiveName
        {
            get
            {
                return "Register";
            }
        }

        /// <summary>
        /// True if user control made from ascx file.
        /// </summary>
        public bool IsUserControl
        {
            get
            {
                return base.Dictionary["tagname"] == null == false;
            }
        }

        /// <summary>
        /// Namespace
        /// </summary>
        public string NamespaceName
        {
            get
            {
                object local = base.Dictionary["namespace"];
                if (local == null)
                {
                    return "";
                }
                else
                {
                    return (String)local;
                }
            }
        }

        /// <summary>
        /// Source
        /// </summary>
        public string SourceFile
        {
            get
            {
                object local = base.Dictionary["src"];
                if (local == null)
                {
                    return "";
                }
                else
                {
                    return (String)local;
                }
            }
        }

        /// <summary>
        /// Name
        /// </summary>
        public string TagName
        {
            get
            {
                object local = base.Dictionary["tagname"];
                if (local == null)
                {
                    return "";
                }
                else
                {
                    return (String)local;
                }
            }
        }

        /// <summary>
        /// Prefix
        /// </summary>
        public string TagPrefix
        {
            get
            {
                object local = base.Dictionary["tagprefix"];
                if (local == null)
                {
                    return "";
                }
                else
                {
                    return (String)local;
                }
            }
        }

        /// <summary>
        /// Create a directive object from string.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <param name="cc">String, like &lt;%@ Register Assembly="Name" Namespace="Field" TagPrefix="Pref" %&gt;.</param>
        /// <param name="host">Reference to host.</param>
        /// <returns>Directive object, or throws an exception if format is invalid.</returns>
        internal static Directive GetDirectiveFromString(CaptureCollection cc, IDesignerHost host)
        {
            if (cc.Count > 0)
            {
                string assembly = "";
                string namspace = "";
                string tgprefix = "";
                string tagname = "";
                string src = "";
                foreach (Capture c in cc)
                {
                    string[] pairs = c.Value.Split(new String[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    if (pairs.Length != 2) continue;
                    string val = pairs[1];
                    if (val.StartsWith(@"""")) val = val.Substring(1);
                    if (val.EndsWith(@"""")) val = val.Substring(0, val.Length - 1);
                    val = val.Trim();
                    switch (pairs[0].Trim().ToLower())
                    {
                        case "assembly":
                            assembly = val;
                            break;
                        case "namespace":
                            namspace = val;
                            break;
                        case "src":
                            src = val;
                            break;
                        case "tagprefix":
                            tgprefix = val;
                            break;
                        case "tagname":
                            tagname = val;
                            break;
                    }
                }
                if (!String.IsNullOrEmpty(src))
                {
                    return new RegisterDirective(tgprefix, tagname, src, true);
                }
                else
                {
                    return new RegisterDirective(tgprefix, namspace, assembly);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("text", "Register Directive String has not the expected format.");
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public RegisterDirective()
        {        
        }

        /// <summary>
        /// Ctor with params.
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="tagName"></param>
        /// <param name="sourceFile"></param>
        /// <param name="type"></param>
        /// <param name="userControl"></param>
        public RegisterDirective(string tagPrefix, string tagName, string sourceFile, Type type, bool userControl)
        {
            base.Dictionary.Add("tagprefix", tagPrefix);
            if (userControl)
            {
                if (!String.IsNullOrEmpty(tagName))
                {
                    base.Dictionary.Add("tagname", tagName);
                    base.Dictionary.Add("src", sourceFile);
                }
                else
                {
                    base.Dictionary.Add("assembly", type.Assembly.FullName);
                }
            }
            referenceType = type;
        }

        private bool _expandusercontrol;
        /// <summary>
        /// Expand the user control in design view.
        /// </summary>
        public bool ExpandUserControl
        {
            get{return _expandusercontrol;}
            set{_expandusercontrol=value;}
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="namespaceName"></param>
        /// <param name="assemblyOfType"></param>
        public RegisterDirective(string tagPrefix, string namespaceName, Type assemblyOfType)
            :this(tagPrefix, namespaceName, assemblyOfType.Assembly.FullName)
        {
            referenceType = assemblyOfType;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="namespaceName"></param>
        /// <param name="assemblyName"></param>
        public RegisterDirective(string tagPrefix, string namespaceName, string assemblyName)
        {
            base.Dictionary.Add("tagprefix", tagPrefix);
            base.Dictionary.Add("namespace", namespaceName);
            base.Dictionary.Add("assembly", assemblyName);
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tagPrefix"></param>
        /// <param name="tagName"></param>
        /// <param name="sourceFile"></param>
        /// <param name="userControl"></param>
        public RegisterDirective(string tagPrefix, string tagName, string sourceFile, bool userControl)
        {
            base.Dictionary.Add("tagprefix", tagPrefix);
            if (userControl)
            {
                base.Dictionary.Add("tagname", tagName);
                base.Dictionary.Add("src", sourceFile);
            }
        }
    }

}
