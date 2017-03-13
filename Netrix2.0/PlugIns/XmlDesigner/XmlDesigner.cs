using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Web.UI;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.PlugIns;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.HtmlFormatting;
using GuruComponents.Netrix.HtmlFormatting.Elements;

namespace GuruComponents.Netrix.XmlDesigner
{
    /// <summary>
    /// XML Designer Extender Provider.
    /// </summary>
    /// <remarks>
    /// The purpose of the XML Designer if the programmatic replace of any kind of XML with a design time viewable HTML block.
    /// The resulting XML object can fire events, interact with the user, expose properties to the PropertyGrid as well as show
    /// commands in the PropertyGrid's command area.
    /// <para>
    /// To make XML working within the core component you must add the extender provider to your form. Once this is done set
    /// the appropriate namespace alias. This is the first conjunction between XML tags and there design time behavior. It's possible
    /// to add more extender providers to register any number of namespace aliases.
    /// </para> 
    /// <para>
    /// The example below explains how to register a namespace and some elements. It's necessary to register each element separatly, using
    /// all four properties of the method <see cref="HtmlEditor.RegisterElement"/>. The first parameter is the the alias used for
    /// the element. It must match the registered namespace. The second is the name of the tag. The third is the type of
    /// element, used for the formatter plug-in. If you don't need to use the formatter remember that any default safe operation
    /// of the main control primarily uses the formatter to create valid XHTML from internal HTML. Internally the control isn't 
    /// aware of XML and the formatter is used to re-build valid XML on safe. If you're in doubt about the options use the
    /// default <see cref="FormattingFlags.Xml"/>. The fourth parameter is the most important one. This is the type of class you
    /// need to control the element's behavior. The base class is <see cref="XmlControl"/>, which in turn implements 
    /// <see cref="IElement"/>. See the description of <see cref="XmlControl"/> for more information about how to implement
    /// XML Designer specific behavior classes.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example explains how to register a namespace and a number of specific elements.
    /// <code>
	/// DesignerProperties dp = xmlDesigner1.GetXmlDesigner(this.htmlEditor1);
	/// dp.Namespace = "rn";
	/// xmlDesigner1.SetXmlDesigner(htmlEditor1, dp);            
	/// xmlDesigner1.RegisterElement("rn", "tracklink", FormattingFlags.Xml, typeof(TracklinkControl));
	/// xmlDesigner1.RegisterElement("rn", "fattach",	FormattingFlags.Xml, typeof(ButtonControl));
	/// xmlDesigner1.RegisterElement("rn", "unsub", FormattingFlags.Xml, typeof(UnsubControl));
	/// xmlDesigner1.RegisterElement("rn", "section", FormattingFlags.Xml, typeof(SectionControl));
	/// xmlDesigner1.RegisterElement("rn", "friend", FormattingFlags.Xml, typeof(FriendControl));
    /// </code>
    /// Alternatively, if elements have been decorated with proper attributes, the following short sequence is allowed:
    /// <code>
    /// xmlDesigner1.RegisterElement(typeof(FriendControl));
    /// </code>
    /// The appropriate values are read from attributes, then.
    /// </example>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(XmlDesigner), "Resources.ToolBox.ico")]
    [ProvideProperty("XmlDesigner", "GuruComponents.Netrix.IHtmlEditor")]
    [Serializable()]
    public class XmlDesigner : Component, IExtenderProvider, IPlugIn
    {

        [NonSerialized()]
        private static Hashtable properties;

        /// <summary>
        /// Default Constructor supports design time behavior.
        /// </summary>
        public XmlDesigner()
        {
            properties = new Hashtable();
        }
        
        /// <summary>
        /// Constructor for standard usage.
        /// </summary>
        /// <param name="parent"></param>
        public XmlDesigner(IContainer parent) : this()
        {
            properties = new Hashtable();
            parent.Add(this);
        }

        internal static DesignerProperties EnsurePropertiesExists(IHtmlEditor key)
        {
            DesignerProperties p = (DesignerProperties) properties[key];
            if (p == null)
            {
                p = new DesignerProperties(key);
                properties[key] = p;
            }
            return p;
        }   

        # region +++++ Block: XmlDesigner         

        /// <summary>
        /// XMLDesigner Properties.
        /// </summary>
        /// <param name="htmlEditor">Editor reference.</param>
        /// <returns></returns>
        [ExtenderProvidedProperty(), Category("NetRix Component"), Description("XmlDesigner Properties")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DesignerProperties GetXmlDesigner(IHtmlEditor htmlEditor)
        {
            return EnsurePropertiesExists(htmlEditor);
        }

        public void SetXmlDesigner(IHtmlEditor htmlEditor, DesignerProperties Properties)
        {
            //editor = htmlEditor;
            EnsurePropertiesExists(htmlEditor).Active = Properties.Active;
            EnsurePropertiesExists(htmlEditor).Namespaces = Properties.Namespaces;
            EnsurePropertiesExists(htmlEditor).Icon = Properties.Icon;
            // 
            htmlEditor.AddEditDesigner(GlobalEvents.GetGlobalEventsFactory(htmlEditor));
            GlobalEvents.GetGlobalEventsFactory(htmlEditor).PreHandleEvent += new ElementEventHandler(XmlDesigner_PreHandleEvent);
            GlobalEvents.GetGlobalEventsFactory(htmlEditor).PostHandleEvent += new ElementEventHandler(XmlDesigner_PostHandleEvent);
            // Commands

            htmlEditor.RegisterPlugIn(this);
        }

        void XmlDesigner_PostHandleEvent(object sender, Interop.IHTMLEventObj e)
        {
            //if (e.srcElement != null)
            //{
            //    object[] pVars = new object[1];
            //    // Looking for internal elements
            //    e.srcElement.GetAttribute("edxtemplate", 0, pVars);
            //    if (pVars[0] != null && pVars[0].ToString().Length > 0)
            //    {
            //        System.Diagnostics.Debug.WriteLine(e.srcElement.GetTagName(), e.type);
            //        // handle widgets
            //        switch (pVars[0].ToString())
            //        {
            //            case "widget:icon":
            //                if (e.type == "mouseup" && e.button == 2) // right click == context menu
            //                {
            //                    MessageBox.Show("oops" + e.button);
            //                }
            //                break;
            //        }
            //        //e.cancelBubble = true;
            //    }
            //}
        }

        void XmlDesigner_PreHandleEvent(object sender, Interop.IHTMLEventObj e)
        {

        }

        /// <summary>
        /// Assembly version string.
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
        /// Supports serialization
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <returns></returns>
        public bool ShouldSerializeXmlDesigner(IHtmlEditor htmlEditor)
        {
            return true;
        }

        # endregion

        #region IExtenderProvider Member

        /// <summary>
        /// Whether the plugin can extend the editor.
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
        /// Registers a specific XML element for usage with XMLDesigner.
        /// </summary>
        /// <param name="alias">Namespace alias, e.g. "ls" for a tag names &lt;ls:element /&gt;.</param>
        /// <param name="elementName">The name of the element, e.g. "element" for a tag names &lt;ls:element /&gt;.</param>
        /// <param name="flags">Formatting flags to control the formatter (code beautifier) about the kind of element.</param>
        /// <param name="elementType">The type of the element, used to instantiate element objects.</param>
        /// <param name="key"></param>
        public void RegisterElement(string alias, string elementName, FormattingFlags flags, Type elementType, IHtmlEditor key)
        {
            XmlTagInfo tagInfo = new XmlTagInfo(String.Concat(alias, ":", elementName), flags);
            key.RegisterElement(tagInfo, elementType);
        }

        /// <summary>
        /// Register element by reading the mandatory attributes provides with the element's definition.
        /// </summary>
        /// <remarks>
        /// Element classes for custom elements are supposed to provide several attributes. The register
        /// method takes care of the following one, in fact both of these attributes are mandatory:
        /// <list type="bullet">
        ///     <item><see cref="FormattingAttribute"/>: Instructs the formatter to format the element as XML one.</item>
        ///     <item><see cref="XmlElementAttribute"/>: Provides namespace alias and element name as it appears in the document.</item>
        /// </list>
        /// </remarks>
        /// <exception cref="ArgumentNullException">In case at least one of the attributes is missing an exception is thrown.</exception>
        /// <param name="elementType">Type of element we want to register with the XmlDesigner.</param>
        /// <param name="key"></param>
        public void RegisterElement(Type elementType, IHtmlEditor key)
        {
            object[] attributes = elementType.GetCustomAttributes(false);
            string alias = null;
            string name = null;
            FormattingFlags flags = FormattingFlags.None;
            if (attributes != null && attributes.Length > 0)
            {
                foreach (object attr in attributes)
                {
                    switch (attr.GetType().Name)
                    {
                        case "FormattingAttribute":
                            flags = ((FormattingAttribute)attr).Formatting;
                            break;
                        case "XmlElementAttribute":
                            alias = ((XmlElementAttribute)attr).Alias;
                            name = ((XmlElementAttribute)attr).Name;
                            break;
                    }
                }
            }
            if (String.IsNullOrEmpty(alias))
                throw new ArgumentNullException("Attribute property Alias did not provide a valid string");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("Attribute property ElementName did not provide a valid string");
            XmlTagInfo tagInfo = new XmlTagInfo(String.Concat(alias, ":", name), flags);
            key.RegisterElement(tagInfo, elementType);
        }

        /// <summary>
        /// Removes an element definition from element register.
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="elementName"></param>
        /// <param name="key"></param>
        public void UnregisterElement(string alias, string elementName, IHtmlEditor key)
        {
            ((ElementFactory) key.GenericElementFactory).UnRegisterElement(String.Concat(alias, ":", elementName));
        }

        /// <summary>
        /// Load XML directly. Not yet implemented.
        /// </summary>
        /// <param name="path"></param>
        public void LoadXml(string path)
        {
            //if (edxXml == null)
            //{
            //    // no transform sheet provided, let's look for processing instruction
            //    // look for edit view processing instruction
            //    // we expect that the instruction is first one after root element
            //    XmlNodeList children = xFrag.FirstChild.ChildNodes;

            //    for (int i = 0; i < children.Count; i++)
            //    {
            //        // look for processing instruction nodes
            //        if (children[i].NodeType == XmlNodeType.ProcessingInstruction)
            //        {
            //            string name = children[i].Name;
            //            string proc = children[i].Value;
            //            if (name == "edxview")
            //            {
            //                xEdxDoc = new XmlDocument();
            //                if (!File.Exists(proc))
            //                {
            //                    throw new ApplicationException("The document contains a process instruction bad the file given has not been found. Tag: " + proc);
            //                }
            //                xEdxDoc.Load(proc);
            //                break;
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Write back formatted XML content to the file with given path.
        /// </summary>
        /// <param name="path">Path to write document to.</param>
        /// <param name="key">The editor which content we refer to.</param>
        public void SaveXml(string path, IHtmlEditor key)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.Write(GetXml(key));
            sw.Close();
        }

        private HtmlFormatter formatter;
        private HtmlFormatterOptions fo;

        /// <summary>
        /// Returns the Inner XML handled by XmlDesigner as formatted string.
        /// </summary>
        /// <param name="key">The editor which content we refer to.</param>
        /// <returns>Formatted string of xml.</returns>
        public string GetXml(IHtmlEditor key)
        {
            fo = new HtmlFormatterOptions(' ', 1, 1024, true);
            formatter = new HtmlFormatter();
            StringWriter sw = new StringWriter();
            formatter.Format(key.InnerHtml, sw, fo);
            return sw.ToString();
        }

        #region IPlugIn Member

        /// <summary>
        /// Native constant name of this plug-in. Read only.
        /// </summary>
		[Browsable(true)]
        public string Name
        {
            get
            {
                return "XmlDesigner";
            }
        }

        /// <summary>
        /// This Plug-in is implemented as extender provider. Read only.
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
        /// Type of plugin for reference in editor control.
        /// </summary>
        /// <remarks>
        /// This property supports the NetRix infrastructure and is not
        /// intended fot public use.
        /// </remarks>
		[Browsable(false)]
        public Type Type
        {
            get
            {
                return this.GetType();
            }
        }

        /// <summary>
        /// Available features of this plug-in.
        /// </summary>
        public Feature Features
        {
            get { 
                return Feature.DesignerHostSupport | Feature.MultipleNamespaces;
            }
        }

        /// <summary>
        /// Get a dictionary of registered namespaces, with alias as key and namespace as value.
        /// </summary>
        /// <param name="key">The editor which content we refer to.</param>
        /// <returns>Returns dictionary of registered namespaces.</returns>
        public IDictionary GetSupportedNamespaces(IHtmlEditor key)
        {
            return key.GetRegisteredNamespaces();
        }

        public Control CreateElement(string tagName, IHtmlEditor editor)
        {
            Interop.IHTMLElement el;
            el = editor.GetActiveDocument(false).CreateElement(tagName);
            return editor.GenericElementFactory.CreateElement(el);
        }

        /// <summary>
        /// List of element types, which the extender plugin extends.
        /// </summary>
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.PlugIns.IPlugIn.GetElementExtenders"/> for background information.
        /// For XMLDesigner, there are no extended elements, because XmlDesigner handles all custom elements internally.
        /// </remarks>
        public List<CommandExtender> GetElementExtenders(IElement component)
        {
            return null;
        }

        #endregion


        #region IPlugIn Members

        private bool isready;

        public void NotifyReadyStateCompleted(IHtmlEditor editor)
        {
            isready = true;
        }

        #endregion
    }
}
