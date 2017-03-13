using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.CodeEditor.Library.Plugins
{
    public class PluginLoadInfo
    {
        private bool _Loaded = false;

        private string _Filename = null;

        private string _ClassName = null;


        public PluginLoadInfo(bool loaded,
            string filename, string className)
        {
            _Loaded = loaded;
            _Filename = filename;
            _ClassName = className;
        }


        public string ClassName
        {
            get
            {
                return _ClassName;
            }
            set
            {
                _ClassName = value;
            }
        }

        public bool Loaded
        {
            get { return _Loaded; }
            set { _Loaded = value; }
        }

        public string Filename
        {
            get
            {
                return _Filename;
            }
            set
            {
                _Filename = value;
            }
        }
    }
}
