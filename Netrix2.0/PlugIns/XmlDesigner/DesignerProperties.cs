using System;
using System.ComponentModel;
using System.Drawing;

namespace GuruComponents.Netrix.XmlDesigner
{
	/// <summary>
	/// Properties for XML Extender Provider
	/// </summary>
    [Serializable()]
    public class DesignerProperties 
    {

        private bool icon;
        private bool active = true;
        private string[] aliases = new string[0];
        [NonSerialized()]
        private IHtmlEditor editor;

        /// <summary>
        /// Ctor for designer support.
        /// </summary>
        public DesignerProperties()
        {
        }

        /// <summary>
        /// Regular ctor with editor reference.
        /// </summary>
        /// <param name="editor">Editor reference</param>
        public DesignerProperties(IHtmlEditor editor)
        {
            this.editor = editor;
        }

        /// <summary>
        /// Activates or deactivates the rendering of XML elements.
        /// </summary>
        [Browsable(true), RefreshProperties(RefreshProperties.All), DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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
        /// Shows a small icon in the upper left corner of an element to indicate as XML.
        /// </summary>
        /// <remarks>
        /// The icon is helpful for elements which by default have no visual appearance. However, 
        /// currently we support only one icon for all elements. To distinguish between elements
        /// it's recommended to override the Draw method of the attached element designer and create
        /// some kind of private drawing.
        /// </remarks>
        [Browsable(true), RefreshProperties(RefreshProperties.All), DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public bool Icon
        {
            get
            { 
                return icon;
            }
            set
            {
                icon = value;
            }
        }

        /// <summary>
        /// Override to support appearance in the PropertyGrid.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int i = 0;
            if (Active != true) i++;
            if (Icon != false) i++;
            if (Namespaces != null) i++;
            return String.Format("{0} propert{1} modified", i, (i == 1) ? "y" : "ies");
        }

    }
}
