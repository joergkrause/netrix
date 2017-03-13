using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.RegularExpressions;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.PlugIns;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.HtmlFormatting;
using GuruComponents.Netrix.HtmlFormatting.Elements;
using WebFormsReferenceManager = GuruComponents.Netrix.Designer.WebFormsReferenceManager;

namespace GuruComponents.Netrix.AspDotNetDesigner
{
    /// <summary>
    /// AspDotNet (ASP.NET) control designer extender provider.
    /// </summary>
    /// <remarks>
    /// This plug-in extends NetRix so it can handle webcontrols, htmlcontrols and usercontrols from the ASP.NET namespaces.
    /// <para>
    /// The intention of this plug-in is providing a low level access to a design time environment to display ASP.NET controls,
    /// user controls and types derived from System.Web.UI.WebControl. It's not intended to build a whole IDE like Visual
    /// Studio.NET. Therefore a couple of rules are applying:
    /// <list type="bullet">
    ///     <item>Documents loaded must appear as simple HTML documents, e.g. neither as ASPX nor ASCX.</item>
    ///     <item>Directives defined in the document header has to be stripped out and the directives has to be defined separatly.</item>
    ///     <item>The host application is responsible for any non-HTML parts of an document and is supposed to define a decument engine to handle embedded directives.</item>
    /// </list>
    /// The usage of user controls requires several steps, e.g. the registration of namespaces and control names, as well as
    /// instructions for the formatter and registration of the type. The type is required to create objects on-the-fly and
    /// use them internally. The assembly where the type is defined must be part of the app domain and loadable at runtime.
    /// </para>
    /// <para>
    /// <b>Basic Steps to Show and Edit ASP.NET based Controls:</b><br/>
    /// Before you can show ASP.NET controls and derived objects, like user controls, you must prepare several steps to get the
    /// designer running:
    /// <list type="bullet">
    ///     <item>Have an element class that you wish to use. For legacy ASP.NET controls this is already done, you can use types from .NET framework.</item>
    ///     <item>For private elements, implement a user control, derive from WebControl, and attach a designer.</item>
    ///     <item>Register the private element in both, the AspDotNetDesigner plugin as assembly and in HtmlEditor for formatting.</item>
    ///     <item></item>
    /// </list>
    /// Registering the element is a two-step procedure. Both steps take place before loading (or first time after constructor).
    /// First, add a handler to get the right call from <see cref="RegisterDirectives"/> event:
    /// <code>
    /// aspDotNetDesigner1.RegisterDirectives += new EventHandler(aspDotNetDesigner1_RegisterDirectives);
    /// </code>
    /// Then, in the handler, add your element:
    /// <code>
    /// RegisterDirective directive = new RegisterDirective("uc", "MyUserControl", typeof(MyUserControl).AssemblyQualifiedName, typeof(MyUserControl), true);
    /// aspDotNetDesigner1.RegisterDirective(directive, htmlEditor1);
    /// AspTagInfo tagInfo = new AspTagInfo("uc:MyUserControl", FormattingFlags.Xml);
    /// htmlEditor1.RegisterElement(tagInfo, typeof(MyUserControl));
    /// </code>
    /// In this example, "uc" is the alias used in the document for the element "MyUserControl". Also the class has the same name
    /// "MyUserControl" (but it could have a different name). <see cref="AddRegisterDirective"/> registers the information usually 
    /// found in the &lt;%@ Register %&gt; statement in the document. If you're working with ASCX or ASPX documents it's necessary
    /// to transform them first, following these steps:
    /// <list type="bullet">
    ///     <item>Strip out any content above the HTML tag.</item>
    ///     <item>Add HTML, HEAD and BODY section if not already there (especially for ASCX files), and load content into BODY.</item>
    ///     <item>Scan any Register directives and call <see cref="AddRegisterDirective"/> as described below.</item>
    ///     <item>Remove any Control or Page directives. This is part of the host application and not supported by AspDotNetDesigner class.</item>
    ///     <item>Load the prepared document and call <see cref="AddRegisterDirective"/> during Loading event (see above).</item>
    /// </list>
    /// After document has been edited by user just get body content by calling InnerHtml and restore the document structure. 
    /// It's also possible to use the formatter separatly to format the content and get XHTML back.
    /// </para>
    /// <para>
    /// Probably showing the webcontrols within the PropertyGrid is a good solution for a VS.NET like IDE. If you want to
    /// do so, just connect the event HtmlElementChanged and handle the current control. However, to avoid conflicts
    /// with <see cref="IElement"/> based objects, two properties allow access:
    /// <code>
    /// if (e.CurrentElement == null)
    /// {
    ///     propertyGrid1.SelectedObject = e.CurrentControl;
    /// }
    /// else
    /// {
    ///     propertyGrid1.SelectedObject = e.CurrentElement;
    /// }
    /// </code>
    /// All elements handled internally by NetRix derive from <see cref="IElement"/>. Foreign sources for components,
    /// derived from <see cref="IComponent"/> and subsequently implementing <see cref="Control"/> are handled properly
    /// by the infrastructure, however, all internal procedures based on <see cref="IElement"/> can not handle this.
    /// For that reason, some event argument classes provide distinguish access to "controls" and "native controls".
    /// </para>
    /// </remarks>
    /// <seealso cref="IHtmlEditor"/>
    /// <seealso cref="IHtmlEditor.RegisterElement">RegisterElement (IHtmlEditor)</seealso>
    /// <seealso cref="AddRegisterDirective"/>
    /// <seealso cref="AspTagInfo"/>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(AspDotNetDesigner), "Resources.ToolBox.ico")]
    [ProvideProperty("AspDotNetDesigner", "GuruComponents.Netrix.IHtmlEditor")]
    public class AspDotNetDesigner : Component, IExtenderProvider, IPlugIn
    {
        private Hashtable properties;
        private static readonly DirectiveRegex dirRegex = new DirectiveRegex();
        private static readonly ServerTagsRegex tagRegex = new ServerTagsRegex();
        private static readonly TagRegex usertagRegex = new TagRegex();
        private static Dictionary<IHtmlEditor, List<IRegisterDirective>> preRegistered;
        private static Dictionary<IHtmlEditor, List<IDirective>> pageDirectives;
        private static Dictionary<IHtmlEditor, List<IDirective>> controlDirectives;
        private static Dictionary<IHtmlEditor, string> basePath;

        /// <summary>
        /// Default Constructor supports design time behavior.
        /// </summary>
        public AspDotNetDesigner()
        {
            properties = new Hashtable();
            preRegistered = new Dictionary<IHtmlEditor, List<IRegisterDirective>>();
            pageDirectives = new Dictionary<IHtmlEditor, List<IDirective>>();
            controlDirectives = new Dictionary<IHtmlEditor, List<IDirective>>();
            basePath = new Dictionary<IHtmlEditor, string>();
        }


        /// <summary>
        /// Default Constructor supports design time behavior
        /// </summary>
        /// <param name="parent"></param>
        public AspDotNetDesigner(IContainer parent)
            : this()
        {
            properties = new Hashtable();
            if (parent != null)
            {
                parent.Add(this);
            }
        }

        private DesignerProperties EnsurePropertiesExists(IHtmlEditor key)
        {
            DesignerProperties p = (DesignerProperties)properties[key];
            if (p == null)
            {
                p = new DesignerProperties();
                properties[key] = p;
            }
            return p;
        }

        # region +++++ Block: AspDotNetDesigner

        /// <summary>
        /// Returns the designer's properties at design time.
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <returns></returns>
        [ExtenderProvidedProperty(), Category("NetRix Component"), Description("AspDotNetDesigner Properties")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DesignerProperties GetAspDotNetDesigner(IHtmlEditor htmlEditor)
        {
            return this.EnsurePropertiesExists(htmlEditor);
        }
        /// <summary>
        /// Sets the designer's properties at design time.
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <param name="Properties"></param>
        public void SetAspDotNetDesigner(IHtmlEditor htmlEditor, DesignerProperties Properties)
        {
            EnsurePropertiesExists(htmlEditor).LoadAscx = true;
            EnsurePropertiesExists(htmlEditor).Active = Properties.Active;
            EnsurePropertiesExists(htmlEditor).ExpandUserControls = Properties.ExpandUserControls;
            EnsurePropertiesExists(htmlEditor).RequireServerAttribute = Properties.ExpandUserControls;

            RegisterAll(htmlEditor);

            // Commands
            // activate behaviors when document is ready, otherwise it will fail
            htmlEditor.Loading += new LoadEventHandler(htmlEditor_Loading);
            // Register
            htmlEditor.RegisterPlugIn(this);
        }

        private bool firstTimeRegistered = false;

        /// <summary>
        /// Called by Host to notify the document state. You should not call this from user code.
        /// </summary>
        /// <param name="htmlEditor"></param>
        public void NotifyReadyStateCompleted(IHtmlEditor htmlEditor)
        {
            if (!firstTimeRegistered)
            {
                RegisterAll(htmlEditor);
                firstTimeRegistered = true;
                htmlEditor.AddEditDesigner(GlobalEvents.GetGlobalEventsFactory(htmlEditor));
            }
        }

        // After ResetDesiredProperties/Initialize and before Loading!
        void htmlEditor_Loading(object sender, LoadEventArgs e)
        {
            IHtmlEditor htmlEditor = (IHtmlEditor)sender;
            RegisterAll(htmlEditor);
            if (preRegistered.Count > 0)
            {
                // Register preregistered directives for the editor which invokes the loader 
                foreach (KeyValuePair<IHtmlEditor, List<IRegisterDirective>> kv in preRegistered)
                {
                    if (htmlEditor.Equals(kv.Key))
                    {
                        foreach (IRegisterDirective directive in kv.Value)
                        {
                            AddRegisterDirective(directive, htmlEditor);
                            // Register ASCX
                            string f = String.Format("{0}:{1}", directive.TagPrefix, directive.TagName);
                            AscxTagInfo a = new AscxTagInfo(f, FormattingFlags.Xml, ElementType.Block);
                            htmlEditor.RegisterElement(a, typeof(UserControl));
                        }
                        break;
                    }
                }
            }
        }

        private void RegisterAll(IHtmlEditor htmlEditor)
        {
            RegisterDirectiveCollection directives = new RegisterDirectiveCollection();
            // Register Services
            if (htmlEditor.ServiceProvider.GetService(typeof(IWebFormReferenceManager)) == null)
            {
                // Directives gives as a hint how to persist the controls
                IReferenceManager wrm = new WebFormsReferenceManager(htmlEditor, directives); //, EnsureBehavior(htmlEditor));
                wrm.DirectiveAdded += new DirectiveEventHandler(wrm_DirectiveAdded);
                htmlEditor.ServiceProvider.AddService(typeof(IWebFormReferenceManager), wrm);
            }
            if (htmlEditor.ServiceProvider.GetService(typeof(IUserControlTypeResolutionService)) == null)
                htmlEditor.ServiceProvider.AddService(typeof(IUserControlTypeResolutionService), new UserControlTypeResolution(htmlEditor));
            if (htmlEditor.ServiceProvider.GetService(typeof(INameCreationService)) == null)
                htmlEditor.ServiceProvider.AddService(typeof(INameCreationService), new NameCreationService(htmlEditor));

            DictionaryService ds = new DictionaryService();
            if (htmlEditor.ServiceProvider.GetService(typeof(IDictionaryService)) != null)
            {
                htmlEditor.ServiceProvider.RemoveService(typeof(IDictionaryService));
            }
            htmlEditor.ServiceProvider.AddService(typeof(IDictionaryService), ds);
            // Register Namespaces
            if (EnsurePropertiesExists(htmlEditor).Active)
            {
                htmlEditor.RegisterNamespace("asp", typeof(DesignTimeBehavior));
                // Register private namespaces for custom elements
                if (EnsurePropertiesExists(htmlEditor).Namespaces != null)
                {
                    foreach (string ns in EnsurePropertiesExists(htmlEditor).Namespaces)
                    {
                        string[] nsParts = ns.Split(',');
                        if (nsParts.Length == 2)
                        {
                            htmlEditor.RegisterNamespace(nsParts[0], nsParts[1], typeof(DesignTimeBehavior));
                        }
                        else
                        {
                            htmlEditor.RegisterNamespace(ns, typeof(DesignTimeBehavior));
                        }
                    }
                }
            }
            // Register all standard asp:controls for formatter
            Assembly webUI = typeof(Label).Assembly;
            foreach (Type t in webUI.GetTypes())
            {
                if (t.IsClass && (t.IsSubclassOf(typeof(WebControl)) || t.IsSubclassOf(typeof(Control))) && !t.IsAbstract)
                {
                    object[] at = t.GetCustomAttributes(true);
                    bool checkNonVisual = false; // at the moment we don't support non visuals
                    foreach (object a in at)
                    {
                        if (a is NonVisualControlAttribute) 
                        {
                            checkNonVisual = true;
                            break;
                        }
                    }
                    if (checkNonVisual) continue;
                    AspTagInfo tagInfo;
                    tagInfo = new AspTagInfo(t.Name, FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any);
                    htmlEditor.RegisterElement(tagInfo, t);
                }
            }
            // Ask the host to add more
            InvokeRegisterDirectives();
        }

        void GlobalEvents_PreHandleEvent(object sender, ElementEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(e.EventObj.type, ((System.Windows.Forms.UserControl) sender).Name);
        }

        void GlobalEvents_PostHandleEvent(object sender, ElementEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(e.EventObj.type, ((System.Windows.Forms.UserControl) sender).Name);
        }

        void wrm_DirectiveAdded(object sender, DirectiveEventArgs e)
        {
            if (DirectiveAdded != null)
            {
                DirectiveAdded(sender, e);
            }
        }

        /// <summary>
        /// Assembly version
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
        /// Supports property grid and VS designer
        /// </summary>
        /// <param name="htmlEditor"></param>
        /// <returns></returns>
        public bool ShouldSerializeAspDotNetDesigner(IHtmlEditor htmlEditor)
        {
            return true;
        }

        # endregion

        #region IExtenderProvider Member

        /// <summary>
        /// Allows the extension of HTML Editor.
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
        /// Overriden
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Click plus sign for details";
        }

        /// <summary>
        /// Create a new element in the given editor control.
        /// </summary>
        /// <remarks>
        /// Element types must be registered and well known before they could created. After creation the
        /// element exists within the local element cache and the current document, but is not yet appended
        /// to the DOM. It's recommended to add the element to the DOM using ElementDom functions like
        /// AppendChild.
        /// <para>
        /// 
        /// </para>
        /// </remarks>
        /// <param name="tagName">The tagname, including the alias (like 'asp:button').</param>
        /// <param name="htmlEditor">The editor this element refers to.</param>
        /// <returns>Returns the element or <c>null</c>, if creation fails. See remarks for reasons.</returns>
        public Control CreateElement(string tagName, IHtmlEditor htmlEditor)
        {
            return CreateElement(tagName, htmlEditor, EnsurePropertiesExists(htmlEditor).RequireServerAttribute);
        }

        /// <summary>
        /// Create a new element in the given editor control.
        /// </summary>
        /// <remarks>
        /// Element types must be registered and well known before they could created. After creation the
        /// element exists within the local element cache and the current document, but is not yet appended
        /// to the DOM. It's recommended to add the element to the DOM using ElementDom functions like
        /// AppendChild.
        /// </remarks>
        /// <param name="tagName">The tagname, including the alias (like 'asp:button').</param>
        /// <param name="htmlEditor">The editor this element refers to.</param>
        /// <param name="createRunatServerAttr">Creates the runat="server" attribute.</param>
        /// <returns>Returns the element or <c>null</c>, if creation fails. See remarks for reasons.</returns>
        public Control CreateElement(string tagName, IHtmlEditor htmlEditor, bool createRunatServerAttr)
        {
            Interop.IHTMLDocument2 doc = htmlEditor.GetActiveDocument(false);
            Interop.IHTMLElement el = doc.CreateElement(tagName);
            if (el != null)
            {
                INamespaceManager ns = (INamespaceManager)htmlEditor.ServiceProvider.GetService(typeof(INamespaceManager));
                INameCreationService nc = (INameCreationService)htmlEditor.ServiceProvider.GetService(typeof(INameCreationService));
                IDesignerHost dh = (IDesignerHost)htmlEditor.ServiceProvider.GetService(typeof(IDesignerHost));
                DesignTimeBehavior behavior = (DesignTimeBehavior)ns.GetBehaviorOfElement(el);
                //behavior.Element = el;
                if (createRunatServerAttr)
                {
                    el.SetAttribute("runat", "server", 0);
                }
                Control webcontrol = htmlEditor.GenericElementFactory.CreateElement(el);
                // add a regular name
                webcontrol.ID = nc.CreateName(dh.Container, webcontrol.GetType());
                return webcontrol;
            }
            else
            {
                return null;
            }
        }

        ///// <summary>
        ///// Insert Element related to another element.
        ///// </summary>
        ///// <param name="method"></param>
        ///// <param name="element"></param>
        //public void InsertAdjacentElement(InsertWhere method, IElement relative, Control ctrlToInsert, IHtmlEditor htmlEditor)
        //{
        //    IElement element = GetNativeElement(ctrlToInsert, htmlEditor);
        //    if (element == null)
        //    {
        //        // not yet placed anywhere 
        //        relative.InsertAdjacentHtml(method, GetNativeHtml(ctrlToInsert, htmlEditor));
        //    }
        //    else
        //    {
        //        // exists already, insert
        //        relative.InsertAdjacentElement(method, element);
        //    }
        //}

        //private IElement GetNativeElement(System.Web.UI.Control aspControl, IHtmlEditor htmlEditor)
        //{
        //    IDesignerHost dh = (IDesignerHost)htmlEditor.ServiceProvider.GetService(typeof(IDesignerHost));
        //    IDesigner designer = dh.GetDesigner(aspControl);
        //    if (designer is System.Web.UI.Design.HtmlControlDesigner)
        //    {
        //        System.Web.UI.Design.IHtmlControlDesignerBehavior behavior = ((System.Web.UI.Design.HtmlControlDesigner)designer).Behavior;
        //        if (behavior != null && behavior is DesignTimeBehavior)
        //        {
        //            IElement nativeElement = ((DesignTimeBehavior)behavior).NativeElement;
        //            return nativeElement;
        //        }
        //    }
        //    return null;
        //}

        //private string GetNativeHtml(System.Web.UI.Control aspControl, IHtmlEditor htmlEditor)
        //{
        //    IDesignerHost dh = (IDesignerHost)htmlEditor.ServiceProvider.GetService(typeof(IDesignerHost));
        //    IDesigner designer = dh.GetDesigner(aspControl);
        //    if (designer == null)
        //    {
        //        ((DesignerHost)dh).Add(aspControl, aspControl.ID);
        //        designer = dh.GetDesigner(aspControl);
        //    }
        //    if (designer is System.Web.UI.Design.HtmlControlDesigner)
        //    {
        //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //        StringWriter sw = new StringWriter(sb);
        //        HtmlTextWriter tw = new HtmlTextWriter(sw);
        //        aspControl.RenderControl(tw);
        //        string s = sb.ToString();
        //    }
        //    return null;
        //}

        /// <summary>
        /// Insert an element and returns the inserted object instance.
        /// </summary>
        /// <remarks>
        /// Caller's are supposed to get the returned instance, because the parameter is orphaned (in fact, it's
        /// disposed right now) and removed from DOM.
        /// <para>
        /// Inserting a huge number of elements is not recommended, because the function runs several internal steps
        /// and therefore could result in performance flaw.
        /// </para>
        /// </remarks>
        /// <exception cref="NotImplementedException">The element you're trying to create is not yet registered.</exception>
        /// <param name="newElem">The element object being inserted.</param>
        /// <param name="htmlEditor">The editor we reference to.</param>
        /// <returns>Returns the new element instances after creation.</returns>
        public Control InsertElementAtCaret(Control newElem, HtmlEditor htmlEditor)
        {
            if (newElem is IElement)
            {
                Interop.IHTMLElement element = ((IElement)newElem).GetBaseElement();
                InsertElementAtCaret(element, htmlEditor.GetActiveDocument(false));
            }
            else
            {
                try
                {
                    IReferenceManager wrm = GetReferenceManager(htmlEditor);
                    if (wrm != null)
                    {
                        // get full name (alias and name)
                        string tagPrefix = wrm.GetTagPrefix(newElem.GetType());
                        string tagName = newElem.GetType().Name;
                        string tag, id;

                        INameCreationService ns = (INameCreationService)htmlEditor.ServiceProvider.GetService(typeof(INameCreationService));
                        IContainer ec = (IContainer)htmlEditor.ServiceProvider.GetService(typeof(IContainer));
                        string name = (newElem.Site != null) ? newElem.Site.Name : ns.CreateName(ec, newElem.GetType());
                        if (String.IsNullOrEmpty(name))
                        {
                            id = String.Format("C{0}", Guid.NewGuid()).Replace("-", "");
                        }
                        else
                        {
                            id = name;
                        }
                        string saveId = newElem.ID;
                        tag = String.Format(@"<{0}:{1} id=""{2}"" {3}></{0}:{1}>",
                            tagPrefix,
                            tagName,
                            id,
                            EnsurePropertiesExists(htmlEditor).RequireServerAttribute ? @"runat=""server""" : "");
                        // insert at caret
                        htmlEditor.Document.InsertHtml(tag);
                        Interop.IHTMLElement el = null;
                        if (htmlEditor.GetActiveDocument(false) != null)
                        {
                            // get back the native instance
                            el = ((Interop.IHTMLDocument3)htmlEditor.GetActiveDocument(false)).GetElementById(id);
                            if (el != null)
                            {
                                if (String.IsNullOrEmpty(saveId))
                                {
                                    //el.RemoveAttribute("id", 0); 
                                }
                                else
                                {
                                    el.SetAttribute("id", saveId, 0);
                                }
                                // take over all attributes of source element
                                IDesignerHost host = htmlEditor.ServiceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
                                EmbeddedSerializer.SerializeControl(newElem, host, el);
                            }
                        }
                        // in case of success return the new instance and dispose the old one
                        if (el != null)
                        {
                            newElem.Dispose();
                            return htmlEditor.GenericElementFactory.CreateElement(el);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException("Element not registered. Consider moving registration code to HtmlEditor's Loading event.");
                    }
                }
                catch (NotImplementedException)
                {
                    throw;
                }
                catch
                {
                    // failed for any reason
                    return null;
                }
            }
            return null;
        }

        /// <summary> 
        /// Insert Element related to another element. 
        /// </summary> 
        /// <param name="method">Inserting method</param> 
        /// <param name="relative">Element relative to which the control is inserted.</param> 
        /// <param name="ctrlToInsert">Control to insert</param>
        /// <param name="htmlEditor">Editor reference</param>
        public Control InsertAdjacentElement(InsertWhere method, IElement relative, Control ctrlToInsert, IHtmlEditor htmlEditor)
        {
            IElement element = GetNativeElement(ctrlToInsert, htmlEditor);
            if (element == null)
            {
                // not yet placed anywhere 
                //relative.InsertAdjacentHtml(method, GetNativeHtml(ctrlToInsert, htmlEditor)); 
                Control newElem = ctrlToInsert;
                try
                {
                    IReferenceManager wrm = GetReferenceManager(htmlEditor);
                    if (wrm != null)
                    {
                        // get full name (alias and name) 
                        string tagPrefix = wrm.GetTagPrefix(newElem.GetType());
                        string tagName = newElem.GetType().Name;
                        string tag, id;
                        if (String.IsNullOrEmpty(newElem.ID))
                        {
                            INameCreationService ns = (INameCreationService)htmlEditor.ServiceProvider.GetService(typeof(INameCreationService));
                            IContainer ec = (IContainer)htmlEditor.ServiceProvider.GetService(typeof(IContainer));
                            string name = ns.CreateName(ec, newElem.GetType());
                            if (String.IsNullOrEmpty(name))
                            {
                                id = String.Format("C{0}", Guid.NewGuid()).Replace("-", "");
                            }
                            else
                            {
                                id = name;
                            }
                        }
                        else
                        {
                            id = newElem.ID;
                        }
                        string saveId = id;
                        tag = String.Format(@"<{0}:{1} id=""{2}"" {3}></{0}:{1}>",
                            tagPrefix,
                            tagName,
                            id,
                            EnsurePropertiesExists(htmlEditor).RequireServerAttribute ? @"runat=""server""" : "");
                        // insert at caret 
                        relative.InsertAdjacentHtml(method, tag);
                        Interop.IHTMLElement el = null;
                        if (htmlEditor.GetActiveDocument(false) != null)
                        {
                            // get back the native instance 
                            el = ((Interop.IHTMLDocument3)htmlEditor.GetActiveDocument(false)).GetElementById(id);
                            if (el != null)
                            {
                                if (String.IsNullOrEmpty(saveId))
                                {
                                    //el.RemoveAttribute("id", 0); 
                                }
                                else
                                {
                                    el.SetAttribute("id", saveId, 0);
                                }
                                // take over all attributes of source element 
                                IDesignerHost host = htmlEditor.ServiceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
                                EmbeddedSerializer.SerializeControl(newElem, host, el);
                            }
                        }
                        // in case of success return the new instance and dispose the old one 
                        if (el != null)
                        {
                            newElem.Dispose();
                            return htmlEditor.GenericElementFactory.CreateElement(el);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException("Element not registered. Consider moving registration code to HtmlEditor's Loading event.");
                    }
                }
                catch (NotImplementedException)
                {
                    throw;
                }
                catch
                {
                    // failed for any reason 
                    return null;
                }
            }
            else
            {
                // exists already, insert 
                relative.InsertAdjacentElement(method, element);
                return element as Control;
            }
            return null;
        }

        private static Interop.IHTMLElement InsertElementAtCaret(Interop.IHTMLElement el, Interop.IHTMLDocument2 doc)
        {
            if (el == null) return null;
            try
            {
                Interop.IMarkupServices ms;
                Interop.IDisplayServices ds;
                ds = (Interop.IDisplayServices)doc;
                Interop.IDisplayPointer dp;
                ds.CreateDisplayPointer(out dp);
                Interop.IHTMLCaret cr;
                ds.GetCaret(out cr);
                cr.MoveDisplayPointerToCaret(dp);
                ms = (Interop.IMarkupServices)doc;
                Interop.IMarkupPointer sp; //, ep;
                ms.CreateMarkupPointer(out sp);		//Create a start markup pointer
                dp.PositionMarkupPointer(sp);
                ms.InsertElement(el, sp, null);
                return el;
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// Register a user control assembly using a Directive object.
        /// </summary>
        /// <remarks>
        /// In case of multiple editors on a form you must register your directive for any editor which should
        /// display the content.
        /// <seealso cref="IReferenceManager"/>
        /// <seealso cref="IRegisterDirective"/>
        /// <seealso cref="DirectiveAdded"/>
        /// </remarks>
        /// <param name="directive">The item contains information about the registered object.</param>
        /// <param name="htmlEditor">Reference to HtmlEditor, this directive is related to.</param>
        public void AddRegisterDirective(IRegisterDirective directive, IHtmlEditor htmlEditor)
        {
            IReferenceManager wrm = GetReferenceManager(htmlEditor);
            if (wrm != null)
            {
                wrm.AddRegisterDirective(directive);
                // This behavior provides the factory for element specific behaviors
                htmlEditor.RegisterNamespace(directive.TagPrefix, typeof(DesignTimeBehavior));
            }
            // Directives per editor
            if (!preRegistered.ContainsKey(htmlEditor))
            {
                List<IRegisterDirective> directives = new List<IRegisterDirective>();
                directives.Add(directive);
                preRegistered.Add(htmlEditor, directives);
            }
            else
            {
                if (!preRegistered[htmlEditor].Contains(directive))
                {
                    preRegistered[htmlEditor].Add(directive);
                }
            }
        }

        /// <summary>
        /// Returns the reference manager, which manages user controls. 
        /// </summary>
        /// <remarks>
        /// The reference manager is responsible for the relation between user controls in the code, recognized
        /// by there prefix (namespace alias) and name, and the assembly which contains the type, which in turn
        /// resolves the control and its designer.
        /// <para>
        /// The manager can be used simple by call <see cref="AddRegisterDirective"/> directly. The directive of type
        /// <see cref="IRegisterDirective"/> will contain the necessary information. The reference manager is simple
        /// some sort of container to provide the directive information to the renderer.
        /// </para>
        /// <seealso cref="AddRegisterDirective"/>
        /// </remarks>
        public IReferenceManager GetReferenceManager(IHtmlEditor htmlEditor)
        {
            return htmlEditor.ServiceProvider.GetService(typeof(IWebFormReferenceManager)) as IReferenceManager;
        }

        /// <summary>
        /// The page which acts as the root component and forms the host for all components.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if host service does not exists (Wrong usage).</exception>
        /// <param name="rootComponent">An object derived from Page.</param>
        /// <param name="htmlEditor"></param>
        public void RegisterRootComponent(Page rootComponent, IHtmlEditor htmlEditor)
        {
            IDesignerHost host = htmlEditor.ServiceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (host == null)
            {
                throw new NullReferenceException("The host does not exists or is not being loaded");
            }
            host.Container.Add(rootComponent);
        }

        /// <summary>
        /// Fired if the host application adds a new directive.
        /// </summary>f
        public event DirectiveEventHandler DirectiveAdded;

        /// <summary>
        /// Fired during document load process to let host application register the directives.
        /// </summary>
        /// <remarks>
        /// Host application should register usercontrol directives in this event to create
        /// all internal references needed to resolve user types.
        /// </remarks>
        public event EventHandler RegisterDirectives;

        private void InvokeRegisterDirectives()
        {
            if (RegisterDirectives != null)
            {
                RegisterDirectives(this, EventArgs.Empty);
            }
        }

        #region IPlugIn Member

        /// <summary>
        /// Returns "AspDotNetDesigner".
        /// </summary>
        [Browsable(true), Category("NetRix")]
        public string Name
        {
            get
            {
                return "AspDotNetDesigner";
            }
        }

        /// <summary>
        /// Declares that this is an extender provider. Returns <c>true</c>.
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
        /// The type.
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
        /// Supported Features. This supports the plug-in infrastructure.
        /// </summary>
        [Browsable(true), Category("NetRix")]
        public Feature Features
        {
            get
            {
                return Feature.CreateElements | Feature.EditDesigner | Feature.MultipleNamespaces | Feature.OwnNamespace | Feature.RegisterElements | Feature.DesignerHostSupport;
            }
        }

        /// <summary>
        /// returns http://asp.schema for the asp alias.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Browsable(false)]
        public IDictionary GetSupportedNamespaces(IHtmlEditor key)
        {
            Hashtable ns = new Hashtable();
            ns.Add("asp", "http://asp.schema");
            return ns;
        }

        /// <summary>
        /// List of element types, which the extender plugin extends.
        /// </summary>
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.PlugIns.IPlugIn.GetElementExtenders"/> for background information.
        /// </remarks>
        public List<CommandExtender> GetElementExtenders(IElement component)
        {
            return null;
        }

        #endregion


        /// <summary>
        /// Load an aspx file from path.
        /// </summary>
        /// <param name="filepath">File path. Ignores if file not found.</param>
        /// <param name="refEditor">The editor instance the contents is loaded into.</param>
        public void LoadFile(string filepath, IHtmlEditor refEditor)
        {
            if (File.Exists(filepath))
            {
                basePath[refEditor] = Path.GetDirectoryName(filepath);
                using (StreamReader fs = new StreamReader(filepath, true))
                {
                    string s = fs.ReadToEnd();
                    LoadAspx(s, refEditor);
                    refEditor.Url = filepath;
                }
            }
        }

        /// <summary>
        /// Load an aspx file from string.
        /// </summary>
        /// <remarks>
        /// &lt;%@ Control Language="C#" ClassName="SampleA1" %>
        /// &lt;%@ Register Assembly="Custom.Kernel.Web" Namespace="v.Kernel.Web.UI.WebControls.AspFields" TagPrefix="aspfield" %>
        /// &lt;%@ Register Assembly="Custom.Library.WebRenderFields" Namespace="Custom.Library.WebRenderFields" TagPrefix="aspField" %>
        /// </remarks>
        /// <param name="content">Content to be loaded.</param>
        /// <param name="refEditor">The editor instance the contents is loaded into.</param>
        public void LoadAspx(string content, IHtmlEditor refEditor)
        {
            preRegistered = new Dictionary<IHtmlEditor, List<IRegisterDirective>>();
            pageDirectives = new Dictionary<IHtmlEditor, List<IDirective>>();
            controlDirectives = new Dictionary<IHtmlEditor, List<IDirective>>();
            // load
            string wrapped = LoadControl(content, refEditor, EnsurePropertiesExists(refEditor));
            refEditor.LoadHtml(wrapped);
        }

        internal static string LoadControl(string content, IHtmlEditor refEditor, DesignerProperties props)
        {
            IDesignerHost dh = refEditor.ServiceProvider.GetService(typeof(IDesignerHost)) as IDesignerHost;
            IReferenceManager wrm = refEditor.ServiceProvider.GetService(typeof(IWebFormReferenceManager)) as IReferenceManager;
            // Read directives
            MatchCollection matches = tagRegex.Matches(content.Trim());
            foreach (Match match in matches)
            {
                string text = match.Value.Replace(Environment.NewLine, "");
                // remove this from content
                content = content.Replace(text, "");
                // Register directives
                Match m = dirRegex.Match(match.Value);
                if (m.Success)
                {
                    string dirName = m.Groups[1].Captures[0].Value.ToLower().Trim();
                    IRegisterDirective rd;
                    switch (dirName)
                    {
                        case "page":
                            CaptureCollection cp = m.Groups[1].Captures;
                            Directive pd = PageDirective.GetDirectiveFromString(cp);
                            if (!pageDirectives.ContainsKey(refEditor))
                            {
                                List<IDirective> directives = new List<IDirective>();
                                directives.Add(pd);
                                pageDirectives.Add(refEditor, directives);
                            }
                            else
                            {
                                pageDirectives[refEditor].Add(pd);
                            }
                            break;
                        case "control":
                            CaptureCollection cc = m.Groups[1].Captures;
                            Directive cd = ControlDirective.GetDirectiveFromString(cc);
                            if (!controlDirectives.ContainsKey(refEditor))
                            {
                                List<IDirective> directives = new List<IDirective>();
                                directives.Add(cd);
                                controlDirectives.Add(refEditor, directives);
                            }
                            else
                            {
                                controlDirectives[refEditor].Add(cd);
                            }
                            break;
                        case "register":
                            CaptureCollection cr = m.Groups[1].Captures;
                            rd = (IRegisterDirective)GuruComponents.Netrix.Designer.RegisterDirective.GetDirectiveFromString(cr, dh);
                            ((RegisterDirective)rd).ExpandUserControl = props.ExpandUserControls;
                            if (!preRegistered.ContainsKey(refEditor))
                            {
                                preRegistered.Add(refEditor, new List<IRegisterDirective>(new IRegisterDirective[] { rd }));
                            }
                            else
                            {
                                List<IRegisterDirective> existingRDs = preRegistered[refEditor];
                                if (existingRDs == null)
                                    existingRDs = new List<IRegisterDirective>();
                                if (!existingRDs.Contains(rd))
                                {
                                    existingRDs.Add(rd);
                                }
                                preRegistered[refEditor] = existingRDs;
                            }
                            // This behavior provides the factory for element specific behaviors
                            refEditor.RegisterNamespace(rd.TagPrefix, typeof(DesignTimeBehavior));


                            /* This block is to support automatically expanded user controls. It needs implementation
                             * of DesignTimeControlParser function in DesignTimeBehavior, which is not yet implemented.
                             * */

                            if (rd.IsUserControl)
                            {
                                // go recursively through nested user controls and look for register directives there as well
                                if (!String.IsNullOrEmpty(rd.AssemblyName))
                                {
                                    AssemblyName name = new AssemblyName(rd.AssemblyName);
                                    if (File.Exists(name.CodeBase))
                                    {
                                        Assembly a = Assembly.Load(new AssemblyName(rd.AssemblyName));
                                        List<Type> types = new List<Type>(a.GetTypes());
                                        Regex rr = new Regex(@"<(?<tagprefix>[\w:\:]+):(?<tagname>[\w:\.]+)(\s+(?<attrname>\w[-\w:]*)(\s*=\s*""(?<attrval>[^""]*)""|\s*=\s*'(?<attrval>[^']*)'|\s*=\s*(?<attrval><%#.*?%>)|\s*=\s*(?<attrval>[^\s=/>]*)|(?<attrval>\s*?)))*\s*(?<empty>/)?>");
                                        MatchCollection usertagMatches = rr.Matches(content.Trim());
                                        if (usertagMatches.Count > 0)
                                        {
                                            List<string> matchField = new List<string>();
                                            foreach (Match user in usertagMatches)
                                            {
                                                if (user.Success && user.Groups["tagprefix"].Value == rd.TagPrefix && !matchField.Contains(user.Groups["tagname"].Value.ToLower()))
                                                {
                                                    matchField.Add(user.Groups["tagname"].Value.ToLower());
                                                }
                                            }
                                            // look for controls within the content
                                            foreach (Type t in types)
                                            {
                                                if (matchField.Contains(t.Name.ToLower()))
                                                {
                                                    ((RegisterDirective)rd).ObjectType = t;
                                                    ((RegisterDirective)rd).ExpandUserControl = props.ExpandUserControls;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            // wrap content 
            string wrapped = String.Format("{0}", content.Trim());
            return wrapped;
        }

        /// <summary>
        /// Get the content as formatted aspx page. All directives are parsed and put to document's beginning.
        /// </summary>
        /// <param name="refEditor">The editor instance the contents is loaded from.</param>
        /// <returns>String with content</returns>
        public string SaveAspx(IHtmlEditor refEditor)
        {
            string content = "";
            // get page directives first
            if (pageDirectives.ContainsKey(refEditor))
            {
                foreach (Directive dir in pageDirectives[refEditor])
                {
                    content += dir.ToString() + Environment.NewLine;
                }
            }
            if (controlDirectives.ContainsKey(refEditor))
            {
                foreach (Directive dir in controlDirectives[refEditor])
                {
                    content += dir.ToString() + Environment.NewLine;
                }
            }
            // get referenced controls
            IReferenceManager wrm = GetReferenceManager(refEditor);
            if (wrm != null)
            {
                content += wrm.GetRegisterDirectives();
            }
            content += refEditor.GetRawHtml();
            HtmlFormatter hf = new HtmlFormatter();
            StringWriter sw = new StringWriter();
            hf.Format(content, sw, refEditor.HtmlFormatterOptions);
            return sw.ToString();
        }

        /// <summary>
        /// Try to get the native (internal) generic element that represents the ASCX controls container.
        /// </summary>
        /// <param name="aspControl">Control what we're looking for. Must be part of document.</param>
        /// <param name="htmlEditor1">Reference to editor</param>
        /// <returns></returns>
        public IElement GetNativeElement(System.Web.UI.Control aspControl, IHtmlEditor htmlEditor1)
        {
            IDesignerHost dh = (IDesignerHost)htmlEditor1.ServiceProvider.GetService(typeof(IDesignerHost));
            IDesigner designer = dh.GetDesigner(aspControl);
            if (designer is System.Web.UI.Design.HtmlControlDesigner)
            {
                System.Web.UI.Design.IHtmlControlDesignerBehavior behavior = ((System.Web.UI.Design.HtmlControlDesigner)designer).Behavior;
                if (behavior != null && behavior is DesignTimeBehavior)
                {
                    IElement nativeElement = ((DesignTimeBehavior)behavior).NativeElement;
                    return nativeElement;
                }
            }
            return null;
        }

    }
}