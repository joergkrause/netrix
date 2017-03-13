using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// Base implementation for register directives storage.
    /// </summary>
    public abstract class Directive : IDirective
    {
        private static IDictionary _casedNameTable = new HybridDictionary(true);

        private HybridDictionary _dictionary;


        /// <summary>
        /// All attributes
        /// </summary>
        protected IDictionary Dictionary
        {
            get
            {
                if (_dictionary == null)
                {
                    _dictionary = new HybridDictionary();
                }
                return _dictionary;
            }
        }

        /// <summary>
        /// The name
        /// </summary>
        public abstract string DirectiveName
        {
            get;
        }

        static Directive()
        {
            _casedNameTable["aspcompat"] = "AspCompat";
            _casedNameTable["assembly"] = "Assembly";
            _casedNameTable["buffer"] = "Buffer";
            _casedNameTable["class"] = "Class";
            _casedNameTable["classname"] = "ClassName";
            _casedNameTable["clienttarget"] = "ClientTarget";
            _casedNameTable["codepage"] = "CodePage";
            _casedNameTable["compileroptions"] = "CompilerOptions";
            _casedNameTable["contenttype"] = "ContentType";
            _casedNameTable["culture"] = "Culture";
            _casedNameTable["debug"] = "Debug";
            _casedNameTable["enablesessionstate"] = "EnableSessionState";
            _casedNameTable["enableviewstate"] = "EnableViewState";
            _casedNameTable["enableviewstatemac"] = "EnableViewStateMac";
            _casedNameTable["masterpagefile"] = "MasterPageFile";
            _casedNameTable["errorpage"] = "ErrorPage";
            _casedNameTable["explicit"] = "Explicit";
            _casedNameTable["inherits"] = "Inherits";
            _casedNameTable["language"] = "Language";
            _casedNameTable["lcid"] = "LCID";
            _casedNameTable["namespace"] = "Namespace";
            _casedNameTable["responseencoding"] = "ResponseEncoding";
            _casedNameTable["src"] = "Src";
            _casedNameTable["strict"] = "Strict";
            _casedNameTable["tagprefix"] = "TagPrefix";
            _casedNameTable["tagname"] = "TagName";
            _casedNameTable["trace"] = "Trace";
            _casedNameTable["tracemode"] = "TraceMode";
            _casedNameTable["transaction"] = "Transaction";
            _casedNameTable["warninglevel"] = "WarningLevel";
        }
        /// <summary>
        /// Adds an attribute
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public void AddAttribute(string attributeName, string attributeValue)
        {
            if (attributeValue == null || attributeValue.Length == 0)
            {
                Dictionary.Remove(attributeName.ToLower());
                return;
            }
            Dictionary[attributeName.ToLower()] = attributeValue;
        }

        /// <summary>
        /// gets cased name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected static string GetCasedName(string name)
        {
            string str = (String)_casedNameTable[name.ToLower()];
            if (str != null)
            {
                return str;
            }
            else
            {
                return name;
            }
        }

        /// <summary>
        /// As it appears in code
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<%@ ");
            stringBuilder.Append(DirectiveName);
            IDictionaryEnumerator iDictionaryEnumerator = Dictionary.GetEnumerator();
            while (iDictionaryEnumerator.MoveNext())
            {
                DictionaryEntry dictionaryEntry = (DictionaryEntry)iDictionaryEnumerator.Current;
                string str1 = GetCasedName((String)dictionaryEntry.Key);
                string str2 = String.Empty;
                object local = dictionaryEntry.Value;
                if (local != null)
                {
                    str2 = local.ToString();
                }
                if (str2 != null && str2.Length > 0)
                {
                    stringBuilder.Append(" ");
                    stringBuilder.Append(str1);
                    if (str2.IndexOf("\"") != -1)
                    {
                        stringBuilder.Append("=\'");
                        stringBuilder.Append(str2);
                        stringBuilder.Append('\'');
                    }
                    else
                    {
                        stringBuilder.Append("=\"");
                        stringBuilder.Append(str2);
                        stringBuilder.Append('\"');
                    }
                }
            }
            stringBuilder.Append(" %>");
            return stringBuilder.ToString();
        }
    }

}
