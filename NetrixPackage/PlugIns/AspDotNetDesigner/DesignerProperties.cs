using System;
using System.ComponentModel;

namespace GuruComponents.Netrix.AspDotNetDesigner
{
	/// <summary>
	/// Properties for AspDotNet Extender Provider.
	/// </summary>
	/// <remarks>
	/// See properties for further information.
	/// By default the properties are set to the following values:
	/// <list type="bullet">
	///     <item>Active == true, e.g. is activated by default.</item>
	///     <item>ExpandUserControls == true, e.g. expands user controls by default.</item>
	///     <item>RequireServerAttribute == true, e.g. controls must have this attribute.</item>
	///     <item>Namespaces = null, e.g. no custom aliases registered.</item>
	/// </list>
	/// </remarks>
    [Serializable()]
	public class DesignerProperties 
	{

        private bool active;
        private bool expandUserControls;
        private bool requireServerAttribute;
        private string[] aliases;
        private bool loadAscx;
        [NonSerialized()]
        private IHtmlEditor editor;

        /// <summary>
        /// ctor.
        /// </summary>
		public DesignerProperties()
		{
            this.active = true;
            this.expandUserControls = true;
            this.requireServerAttribute = true;
            this.loadAscx = true;
        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="editor"></param>
        public DesignerProperties(IHtmlEditor editor)
        {
            this.active = true;
            this.expandUserControls = true;
            this.requireServerAttribute = true;
            this.editor = editor;
            this.loadAscx = true;
        }

        /// <summary>
        /// Forces the component to check for existing runat="server" attribute and add on insertion if required.
        /// </summary>
        /// <remarks>
        /// When used with ASP.NET controls it's required. For custom elements it's
        /// recommended. However, custom controls may or may not rely on this attribute.
        /// <para>
        /// Default is <c>true</c>.
        /// </para>
        /// In case of inserting controls by using the methods provided by the plug-in the required
        /// attribute runat="server" will be added automatically, if the property is <c>true</c>.
        /// </remarks>
        [Category("Behavior"), DefaultValue(true)]
        [Description(@"Forces the component to check for existing runat=""server"" attribute and add on insertion if required.")]
        public bool RequireServerAttribute
        {
            get { return requireServerAttribute; }
            set { requireServerAttribute = value; }
        }
            
        /// <summary>
        /// Activates the designer at runtime.
        /// </summary>
        [Category("Behavior"), DefaultValue(true)]
        [Description("Activates the designer at runtime.")]
        public bool Active
        {
            get
            { 
                return active;
            }
            set
            {
                active = value;
            }
        }

        /// <summary>
        /// Allows loading of ASCX files directly.
        /// </summary>
        [Category("Behavior"), DefaultValue(true)]
        [Description("Allows loading of ASCX files directly.")]
        public bool LoadAscx
        {
            get
            {
                return loadAscx;
            }
            set
            {
                loadAscx = value;
            }
        }

        /// <summary>
        /// Shows content of user controls in design view.
        /// </summary>
        [Category("Behavior"), DefaultValue(true)]
        [Description("Shows content of user controls in design view.")]
        public bool ExpandUserControls
        {
            get
            {
                return expandUserControls;
            }
            set
            {
                expandUserControls = value;
            }
        }
        /// <summary>
        /// The default namespaces alias and urls used to identify elements.
        /// </summary>
        /// <remarks>
        /// This property contains an array of alias/namespace pairs, such like
        /// <c>rss,http://www.rss.org/</c>, separated with commas. The leading (first) part contains the alias.
        /// For XML snippets it's not required to declare the namespaces elsewhere in the document, though,
        /// for EDX documents the namespace must be declared following the XML rules.
        /// </remarks>
        [Browsable(true), RefreshProperties(RefreshProperties.All), DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public string[] Namespaces
        {
            get
            {
                return aliases;
            }
            set
            {
                aliases = value;
                if (aliases != null && aliases.Length > 0 && editor != null)
                {
                    foreach (string alias in aliases)
                    {
                        if (alias.IndexOf(",") != -1)
                        {
                            string[] an = alias.Split(',');
                            editor.RegisterNamespace(an[0], an[1], typeof(DesignTimeBehavior));
                        }
                        else
                        {
                            editor.RegisterNamespace(alias, typeof(DesignTimeBehavior));
                        }
                    }
                }
                else
                {
                    // unregister
                }
            }
        }

        /// <summary>
        /// Property description to support property grid.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int i = 0;
            if (Active != true) i++;
            if (ExpandUserControls != true) i++;
            if (RequireServerAttribute != true) i++;
            if (aliases != null && aliases.Length > 1) i++;
            return String.Format("{0} {1} modified", i, (i == 1) ? "Property" : "Properties");
        }


    }
}
