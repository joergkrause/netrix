using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Comzept.Genesis.NetRix.PlugIns;
using Comzept.Genesis.NetRix.WebEditing.Elements;
using Comzept.Genesis.NetRix;
using Comzept.Genesis.NetRix.Designer;
using Comzept.Genesis.NetRix.ComInterop;
using System.Collections.Generic;
using System.ComponentModel.Design;
          
namespace Comzept.Genesis.NetRix.Tidy
{
    /// <summary>
	/// Tidy extender; provide tidy feature to NetRix editor instances.
	/// </summary>
	/// <remarks>
	/// </remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Comzept.Genesis.NetRix.Tidy.TidySharpExtender), "Resources.TidySharp.ico")]
    [ProvideProperty("HelpLine", "Comzept.Genesis.NetRix.IHtmlEditor")]
    public class TidySharpExtender : Component, System.ComponentModel.IExtenderProvider, Comzept.Genesis.NetRix.PlugIns.IPlugIn
    {

        private Hashtable properties;

        /// <summary>
        /// Default Constructor supports design time behavior
        /// </summary>
        public TidySharpExtender()
        {
            properties = new Hashtable();
        }

        public TidySharpExtender(IContainer parent)
            : this()
        {
            properties = new Hashtable();
            parent.Add(this);
        }

        private TidySharpProperties EnsurePropertiesExists(IHtmlEditor key)
        {
            TidySharpProperties p = (TidySharpProperties)properties[key];
            if (p == null)
            {
                p = new TidySharpProperties();
                properties[key] = p;
            }
            return p;
        }

        # region +++++ Block: Tidy 

        /// <summary>
        /// Get properties.
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <returns></returns>
        [ExtenderProvidedProperty(), Category("NetRix Component"), Description("Tidy Properties")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TidySharpProperties GetTidySharpExtender(IHtmlEditor htmlEditor)
        {
            return this.EnsurePropertiesExists(htmlEditor);
        }
        /// <summary>
        /// Set properties.
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <param name="Properties"></param>
        public void SetTidySharpExtender(IHtmlEditor htmlEditor, TidySharpProperties Properties)
        {   
        }

        //private HelplineCommands commands;

        //[Browsable(false)]
        //public HelplineCommands Commands
        //{
        //    get
        //    {
        //        if (commands == null)
        //        {
        //            commands = new HelplineCommands();
        //        }
        //        return commands;
        //    }
        //}

        //private void TidySharpOperation(object sender, EventArgs e)
        //{
        //    CommandWrapper cw = (CommandWrapper) sender;
        //    if (cw.CommandID.Guid.Equals(Commands.CommandGroup))
        //    {
        //        switch ((HelplineCommand)cw.ID)
        //        {
        //            case HelplineCommand.Activate:
        //                EnsureBehaviorExists(cw.TargetEditor).LineVisible = true;                        
        //                break;
        //            case HelplineCommand.Deactivate:
        //                EnsureBehaviorExists(cw.TargetEditor).LineVisible = false;
        //                break;
        //        }                
        //    }
        //}

        /// <summary>
        /// Version of assembly.
        /// </summary>
        [Browsable(true), ReadOnly(true)]
        public string Version
        {
            get
            {
                return this.GetType().Assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Returns true, extender is serializable.
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <returns></returns>
        public bool ShouldSerializeTidySharpExtender(IHtmlEditor htmlEditor)
        {
            TidySharpProperties p = EnsurePropertiesExists(htmlEditor);
            return true;
        }

        # endregion

        #region IExtenderProvider Member

        /// <summary>
        /// Which control we can extend.
        /// </summary>
        /// <param name="extendee"></param>
        /// <returns></returns>
        public bool CanExtend(object extendee)
        {
            if (extendee is IHtmlEditor)
            {
                return true;
            } 
            else 
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Designer support.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Click + for details";
        }

        #region IPlugIn Member

        /// <summary>
        /// Name of Plug-in.
        /// </summary>
        public string Name
        {
            get
            {
                return "HelpLine";
            }
        }

        /// <summary>
        /// Indicates whether this is an extender.
        /// </summary>
        [Browsable(false)]
        public bool IsExtenderProvider
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Type Type
        {
            get
            {
                return this.GetType();
            }
        }

        /// <summary>
        /// Editor features.
        /// </summary>
        [Browsable(false)]
        Feature IPlugIn.Features
        {
            get { return Feature.None; }
        }

        /// <summary>
        /// No Namespace support.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Browsable(false)]
        public IDictionary GetSupportedNamespaces(IHtmlEditor key)
        {
            return null;
        }

        System.Web.UI.Control IPlugIn.CreateElement(string tagName, IHtmlEditor editor)
        {
            throw new Exception("The method or operation is not available.");
        }

        /// <summary>
        /// List of element types, which the extender plugin extends.
        /// </summary>
        /// <remarks>
        /// See <see cref="Comzept.Genesis.NetRix.PlugIns.IPlugIn.ElementExtenders"/> for background information.
        /// </remarks>
        public List<CommandExtender> GetElementExtenders(IElement component)
        {
            return null;
        }

        #endregion

    }
}
