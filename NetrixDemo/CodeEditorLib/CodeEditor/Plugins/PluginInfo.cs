using System;

namespace GuruComponents.CodeEditor.Library.Plugins
{
    public abstract class PluginInfo
    {
        private string _Author = null;
        private string _Company = null;
        private string _Email = null;
        private string _WebSite = null;
        private Version _Version = null;

        public Version Version
        {
            get { return _Version; }
        }

        public string WebSite
        {
            get { return _WebSite; }
        }

        public string Email
        {
            get { return _Email; }
        }

        public string Company
        {
            get { return _Company; }
        }

        public string Author
        {
            get { return _Author; }
        }

        protected void SetAuthor(string author)
        {
            _Author = author;
        }

        protected void SetCompany(string company)
        {
            _Company = company;
        }

        protected void SetEmail(string email)
        {
            _Email = email;
        }

        protected void SetWebSite(string webSite)
        {
            _WebSite = webSite;
        }

        protected void SetVersion(Version version)
        {
            _Version = version;
        }

        protected virtual void ShowAboutBox()
        {

        }
    }
}
