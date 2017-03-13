using System;
using System.Windows.Forms;

namespace GuruComponents.CodeEditor.Library.Plugins
{
    /// <summary>
    /// Abstract class for starting implementing your own plugin class
    /// </summary>
    public abstract class PluginBase : IPluginApplication
    {

        /// <summary>
        /// Use this method for the initialization of plugins on your software
        /// </summary>
        /// <param name="application">The Main application class</param>
        /// <returns></returns>
        public abstract bool Execute(PluginApplication application);

        /// <summary>
        /// Use this method for the initialization of plugins on your software
        /// </summary>
        /// <param name="application">The Main application class</param>
        /// <param name="parameters">if your application</param>
        /// <returns></returns>
        public abstract bool Execute(PluginApplication application, params object[] parameters);

        /// <summary>
        /// 
        /// </summary>
        public abstract PluginInfo About { get;}

        /// <summary>
        /// 
        /// </summary>
        public abstract bool ShowMenuItem { get;}

        /// <summary>
        /// 
        /// </summary>
        public abstract string Name { get;}

        /// <summary>
        /// 
        /// </summary>
        protected ToolStripMenuItem menuItem;

        /// <summary>
        /// 
        /// </summary>
        public virtual ToolStripMenuItem MenuItem
        {
            get
            {
                if (menuItem == null)
                {
                    menuItem = new ToolStripMenuItem(this.Name);

                    menuItem.Click += new EventHandler(menuItem_Click);
                }

                return menuItem;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void menuItem_Click(object sender, EventArgs e)
        {
            this.OnMenuClick();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnMenuClick()
        {

        }

    }
}
