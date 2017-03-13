using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Web;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.Styles;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;

namespace GuruComponents.Netrix.WebEditing.Documents
{
    /// <summary>
    /// This class represents a HTML document and its various settings
    /// </summary>
    /// <remarks>
    /// Especially it contains information from the header block, like title, meta tags and linked stylesheets.
    /// The class allows access to script blocks, and frame definitions, too.
    /// The common properties can be changed through the PropertyGrid, optionally, some
    /// others are informative, like number of images, frames or script blocks.
    /// <seealso cref=" GuruComponents.Netrix.WebEditing.Documents.IDocumentStructure">IDocumentStructure</seealso>
    /// </remarks>
    public class HtmlDocumentStructure : IDocumentStructure, ICustomTypeDescriptor, IDisposable
    {

        private Interop.IHTMLDocument2 msHtmlDocument;

        private ScriptElementCollection _scriptCollection;
        private LinkElementCollection _stylesCollection;
        private StyleElementCollection _styleTagCollection;
        private MetaElementCollection _metasCollection;
        private MetaElementCollection _equivCollection;
        private HtmlEditor htmlEditor;
      
        internal HtmlDocumentStructure(IHtmlEditor editor)
        {
            htmlEditor = (HtmlEditor)editor;
            msHtmlDocument = editor.GetActiveDocument(false);
            _scriptCollection = new ScriptElementCollection();
            _stylesCollection = new LinkElementCollection();
            _styleTagCollection = new StyleElementCollection();
            _metasCollection = new MetaElementCollection(this.htmlEditor);
            _metasCollection.msHtmlDocument = this.msHtmlDocument;
            _equivCollection = new MetaElementCollection(this.htmlEditor);
        }

        # region Public Properties

        /// <overloads/>
        /// <summary>
        /// Inserts a BGSOUND tag in the head section.
        /// </summary>
        /// <remarks>
        /// The default value for LOOPS is set to -1, the default for AUTOSTART is set to TRUE.
        /// </remarks>
        /// <param name="src">The name of the sound file to be played.</param>
        /// <returns>Returns the native element object.</returns>
        public IElement SetBgSound(string src)
        {
            return SetBgSound(src, -1, true);
        }

        /// <summary>
        /// Inserts a BGSOUND tag in the head section.
        /// </summary>
        /// <remarks>
        /// The default for AUTOSTART is set to TRUE.
        /// </remarks>
        /// <param name="src">The name of the sound file to be played.</param>
        /// <param name="loop">How man yloops the sound will run.</param>
        /// <returns>Returns the native element object.</returns>
        public IElement SetBgSound(string src, int loop)
        {
            return SetBgSound(src, loop, true);
        }

        /// <summary>
        /// Inserts a BGSOUND tag in the head section.
        /// </summary>
        /// <remarks>
        /// All parameters can be set directly.
        /// </remarks>
        /// <param name="src">The name of the sound file to be played.</param>
        /// <param name="loop">The number of loops the sound should play. Set -1 for infinite loops.</param>
        /// <returns>Returns the native element object.</returns>
        /// <param name="autostart"></param>
        public IElement SetBgSound(string src, int loop, bool autostart)
        {
            Interop.IHTMLElement bgSound = this.msHtmlDocument.CreateElement("BGSOUND");
            BgSoundElement bgSoundElement = (BgSoundElement) htmlEditor.GenericElementFactory.CreateElement(bgSound);
            bgSoundElement.src = src;
            bgSoundElement.loop = loop.ToString();
            bgSoundElement.autostart = autostart;
            Interop.IHTMLDOMNode head = this.GetHeadElement();
            head.appendChild((Interop.IHTMLDOMNode) bgSoundElement.GetBaseElement());
            return bgSoundElement;
        }

        /// <overloads/>
        /// <summary>
        /// Inserts a BASE element without target information.
        /// </summary>
        /// <param name="href">Set the default address for hypertext links.</param>
        /// <returns>Returns the native element object.</returns>
        public IElement SetBase(string href)
        {
            return SetBase(href, null);
        }

        /// <summary>
        /// Inserts a BASE element.
        /// </summary>
        /// <param name="href">Set the default address for hypertext links.</param>
        /// <param name="target">Sets the default window for linked documents. Can be <c>null</c> (<c>Nothing</c> in Visual Basic) to avoid setting this attribute.</param>
        /// <returns>Returns the native element object.</returns>
        public IElement SetBase(string href, string target)
        {
            this.SetBaseNode(href, target);
            return GetBase();
        }

        /// <summary>
        /// Returns the current BASE element. 
        /// </summary>
        /// <returns>Return the BaseElement object, if any, or <c>null</c> (<c>Nothing</c> in Visual Basic) if there is no such element.</returns>
        public IElement GetBase()
        {
            Interop.IHTMLElementCollection baseElements = GetHeadElements("BASE");
            if (baseElements != null && baseElements.GetLength() > 0)
            {
                return htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) baseElements.Item(0, 0)) as BaseElement;
            }   
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the current BGSOUND element. 
        /// </summary>
        /// <returns>Return the BgSoundElement object, if any, or <c>null</c> (<c>Nothing</c> in Visual Basic) if there is no such element.</returns>
        public IElement GetBgSound()
        {
            Interop.IHTMLElementCollection baseElements = GetHeadElements("BGSOUND");
            if (baseElements != null && baseElements.GetLength() > 0)
            {
                return htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) baseElements.Item(0, 0)) as BgSoundElement;
            }   
            else 
            {
                return null;
            }
        }

        /// <summary>
        /// Get a collection of elements from the given tag name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For element type related operations this method retrieves a collection (ArrayList) of 
        /// elements of a given tag name. This method is commonly used to add DHTML events to specific
        /// elements, and add wizards or dialogs to - for example - a double click.
        /// </para>
        /// <para>
        /// The objects in the ArrayList are derived from IElement interface. They normally have their
        /// native base type, e.g. if one retrieves the collection of "A" tags the objects are from type
        /// <see cref="AnchorElement"/>. It is not recommended to change the expected base type. Even if
        /// it is working, it fails under some circumstances due to the different types of properties the
        /// elements provide. Returning the interface instead of a native class is a simplification to
        /// avoid the need of many getter methods for each type of element.
        /// </para>
        /// </remarks>
        /// <param name="tagName">The tag name to be retrieved.</param>
        /// <returns>The method returns <c>null</c> (<c>Nothing</c> in VB) if no such elements in the document.</returns>
        public ICollection GetElementCollection(string tagName)
        {
            return this.htmlEditor.GetElementsByTagName(tagName);
        }

        /// <summary>
        /// Get the collection of script blocks.
        /// </summary>
        /// <remarks>Supports PropertyGrid by applying a simple script editor (not supported
        /// in NetRix Light version).</remarks>
        [Category("Content"), Browsable(true),
        Description(""),
        EditorAttribute(
            typeof(ScriptCollectionEditor),
            typeof(UITypeEditor))]
		[DisplayNameAttribute()]
        public ICollectionBase ScriptBlocks
        {
            get
            {
                _scriptCollection.OnInsertHandler -= new CollectionInsertHandler(this._scriptCollection_OnInsertHandler);
                _scriptCollection.OnClearHandler -= new CollectionClearHandler(this._scriptCollection_OnClearHandler);
                _scriptCollection.OnRemoveHandler -= new CollectionRemoveHandler(_scriptCollection_OnRemoveHandler);
                Interop.IHTMLElementCollection scripts = this.msHtmlDocument.GetScripts();
                _scriptCollection.Clear();
                if (scripts.GetLength() > 0)
                {                    
                    for (int i = 0; i < scripts.GetLength(); i++)
                    {
                        ScriptElement o = htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) scripts.Item(i, i)) as ScriptElement;
                        _scriptCollection.Add(o);
                    }
                }
                _scriptCollection.OnInsertHandler += new CollectionInsertHandler(this._scriptCollection_OnInsertHandler);
                _scriptCollection.OnClearHandler += new CollectionClearHandler(this._scriptCollection_OnClearHandler);
                _scriptCollection.OnRemoveHandler += new CollectionRemoveHandler(_scriptCollection_OnRemoveHandler);
                return _scriptCollection;
            }
            set
            {
                // Set is not used by collection editor. It is for use by host application.
                RemoveAllScriptElements();
                if (value == null)
                {
                    _scriptCollection.Clear();                    
                } 
                else 
                {
                    _scriptCollection = (ScriptElementCollection) value;
                    Interop.IHTMLDOMNode parent = GetHeadElement();
                    foreach (ScriptElement script in _scriptCollection)
                    {                        
                        parent.appendChild((Interop.IHTMLDOMNode) script.GetBaseElement());
                    }
                }
            }
        }

        void _scriptCollection_OnRemoveHandler(int index, object value)
        {
            ScriptElement scr = (ScriptElement)value;
            Interop.IHTMLDOMNode node = (Interop.IHTMLDOMNode)scr.GetBaseElement();
            node.removeNode(true);
        }

        /// <summary>
        /// Get the current collection of META tags.</summary>
        /// <remarks>
        /// <para> The getter is called by the propertygrid via reflection
        /// to determine the META tags and use events to propagate new values.
        /// The setter is only to use from the host application and set the complete collection at once. To 
        /// add one value only just call the getter and then the Add() method.</para>
        /// <para>To delete the whole list simply assign null: <c>MetaCollection = null;</c>. This will <i>not</i> 
        /// destroy the object internally nor clear any resources, it will just call the <c>Clear</c> method.</para>
        /// </remarks>
        /// <example>
        /// The following example retrieves two TextBox controls to add a new META tag. It assumes that the
        /// current collection was previously retrieved by calling <c>this.MetaElements = this.HtmlEditor.DocumentStructure.MetaTags;</c>.
        /// <code>
        /// private void buttonAdd_Click(object sender, System.EventArgs e)
        /// {
        ///   if (this.textBoxName.Text.Length == 0 || this.textBoxContent.Text.Length == 0)
        ///   {
        ///       MessageBox.Show("Cannot add empty fields");
        ///   } 
        ///   else 
        ///   {
        ///       MetaElement mE = new MetaElement();
        ///       mE.name = this.textBoxName.Text;
        ///       mE.content = this.textBoxContent.Text;
        ///       this.MetaElements.Add(mE);
        ///   }
        /// }
        /// </code>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.MetaElement"/>
        /// </example>
        [Category("Content"), Browsable(true),
        Description(""),
        EditorAttribute(
            typeof(MetaCollectionEditor),
            typeof(UITypeEditor))]
	    [DisplayName()]
        public ICollectionBase NamedMetaTags
        {
            get
            {
                // prevent firing events during filling the target collection 
                _metasCollection.OnInsertHandler -= new CollectionInsertHandler(_metasCollection_OnInsertHandler);
                _metasCollection.OnClearHandler -= new CollectionClearHandler(_metasCollection_OnClearHandler);
                _metasCollection.OnRemoveHandler -= new CollectionRemoveHandler(_metasCollection_OnRemoveHandler);
                Interop.IHTMLElementCollection metas = ((Interop.IHTMLDocument3) this.msHtmlDocument).GetElementsByTagName("META");
                _metasCollection.Clear();
                if (metas.GetLength() > 0)
                {                    
                    for (int i = 0; i < metas.GetLength(); i++)
                    {
                        Interop.IHTMLElement meta = (Interop.IHTMLElement) metas.Item(i, i);
                        MetaElement test = htmlEditor.GenericElementFactory.CreateElement(meta) as MetaElement;
                        if (test != null && test.name != null && !test.name.Equals(String.Empty))
                        {
                            this._metasCollection.Add(test);
                        }
                    }
                }
                _metasCollection.OnInsertHandler += new CollectionInsertHandler(_metasCollection_OnInsertHandler);
                _metasCollection.OnClearHandler += new CollectionClearHandler(_metasCollection_OnClearHandler);
                _metasCollection.OnRemoveHandler += new CollectionRemoveHandler(_metasCollection_OnRemoveHandler);
                return _metasCollection;
            }
            set
            {
                // Set is not used by collection editor. It is for use by host application.
                if (value == null)
                {
                    _metasCollection.Clear();
                    RemoveAllMetaElements("name");
                } 
                else 
                {
                    _metasCollection = (MetaElementCollection) value;
                    Interop.IHTMLDOMNode parent = GetHeadElement();
                    foreach (MetaElement me in _metasCollection)
                    {
                        parent.appendChild((Interop.IHTMLDOMNode) me.GetBaseElement());
                    }
                }
            }
        }

        private void _metasCollection_OnRemoveHandler(int index, object value)
        {
            MetaElement meta = (MetaElement)value;
            Interop.IHTMLDOMNode node = (Interop.IHTMLDOMNode)meta.GetBaseElement();
            node.removeNode(true);
        }

        /// <summary>
        /// Get the current collection of HTTP-EQUIV META tags.
        /// </summary>
        /// <remarks>
        /// <para> The getter is called by the propertygrid via reflection
        /// to determine the META tags and use events to propagate new values. HTTP-EQUIV meta tags are modifiers for
        /// HTTP headers. The are often used to implement auto refresh or overwrite the default settings for charset or MIME type.
        /// The setter is only to use from the host application and set the complete collection at once. To 
        /// add one value only just call the getter and then the Add() method.</para>
        /// <para>To delete the whole list simply assign null: <c>MetaCollection = null;</c>. This will <i>not</i> 
        /// destroy the object internally nor clear any resources, it will just call the <c>Clear</c> method.</para>
        /// <para>
        /// The <see cref="MetaElement"/> object is the same as used for named META tags. The both attributes used here
        /// (name="" and http-equiv="") are programmed in a toggle manner. This means that if the attribute name is set
        /// from hsot app the http-equiv parameter will set to String.Empty and vice versa.
        /// </para>
        /// </remarks>
        /// <example>
        /// The following example retrieves two TextBox controls to add a new META tag. It assumes that the
        /// current collection was previously retrieved by calling <c>this.MetaElements = this.HtmlEditor.DocumentStructure.MetaTags;</c>.
        /// <code>
        /// private void buttonAdd_Click(object sender, System.EventArgs e)
        /// {
        ///   if (this.textBoxName.Text.Length == 0 || this.textBoxContent.Text.Length == 0)
        ///   {
        ///       MessageBox.Show("Cannot add empty fields");
        ///   } 
        ///   else 
        ///   {
        ///       MetaElement mE = new MetaElement();
        ///       mE.httpEquiv = this.textBoxName.Text;
        ///       mE.content = this.textBoxContent.Text;
        ///       this.MetaElements.Add(mE);
        ///   }
        /// }
        /// </code>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.MetaElement"/>
        /// </example>
        [Category("Content"), Browsable(true),
        Description(""),
        EditorAttribute(
            typeof(MetaCollectionEditor),
            typeof(UITypeEditor))]
		[DisplayName()]
        public ICollectionBase HttpEquivMetaTags
        {
            get
            {
                // stop handler to prevent firing events during filling the target collection 
                _equivCollection.OnInsertHandler -= new CollectionInsertHandler(_equivCollection_OnInsertHandler);
                _equivCollection.OnClearHandler -= new CollectionClearHandler(_equivCollection_OnClearHandler);
                _equivCollection.OnRemoveHandler -= new CollectionRemoveHandler(_equivCollection_OnRemoveHandler);
                Interop.IHTMLElementCollection metas = ((Interop.IHTMLDocument3) this.msHtmlDocument).GetElementsByTagName("META");
                _equivCollection.Clear();
                if (metas.GetLength() > 0)
                {                    
                    for (int i = 0; i < metas.GetLength(); i++)
                    {
                        Interop.IHTMLElement meta = (Interop.IHTMLElement) metas.Item(i, i);
                        MetaElement test = htmlEditor.GenericElementFactory.CreateElement(meta) as MetaElement;
                        if (test != null && test.HttpEquiv != null && !test.HttpEquiv.Equals(String.Empty))
                        {
                            _equivCollection.Add(test);
                        }
                    }
                }
                _equivCollection.OnInsertHandler += new CollectionInsertHandler(_equivCollection_OnInsertHandler);
                _equivCollection.OnClearHandler += new CollectionClearHandler(_equivCollection_OnClearHandler);
                _equivCollection.OnRemoveHandler += new CollectionRemoveHandler(_equivCollection_OnRemoveHandler);
                return _equivCollection;
            }
            set
            {
                if (value == null)
                {
                    _equivCollection.Clear();
                    RemoveAllMetaElements("http-equiv");
                } 
                else 
                {
                    _equivCollection = (MetaElementCollection) value;
                    Interop.IHTMLDOMNode parent = GetHeadElement();
                    foreach (MetaElement me in _metasCollection)
                    {
                        parent.appendChild((Interop.IHTMLDOMNode) me.GetBaseElement());
                    }
                }
            }
        }

        void _equivCollection_OnRemoveHandler(int index, object value)
        {
            MetaElement meta = (MetaElement)value;
            Interop.IHTMLDOMNode node = (Interop.IHTMLDOMNode)meta.GetBaseElement();
            node.removeNode(true);
        }

        /// <summary>
        /// This getter retrieves the collection of embedded stylesheets.
        /// </summary>
        /// <remarks>
        /// The property detects the style tags in the head section only.
        /// </remarks>
        [Category("Content"), Browsable(true), Description("")]
        [EditorAttribute(
             typeof(StyleCollectionEditor),
             typeof(UITypeEditor))]
        [DisplayName()]
        public ICollectionBase EmbeddedStylesheets
        {
            get
            {                
                _styleTagCollection.OnInsertHandler -= new CollectionInsertHandler(_styleTagCollection_OnInsertHandler);
                _styleTagCollection.OnClearHandler -= new CollectionClearHandler(_styleTagCollection_OnClearHandler);
                Interop.IHTMLElementCollection styles = ((Interop.IHTMLDocument3) this.msHtmlDocument).GetElementsByTagName("STYLE");
                _styleTagCollection.Clear();
                if (styles.GetLength() > 0)
                {                    
                    for (int i = 0; i < styles.GetLength(); i++)
                    {
                        Interop.IHTMLElement style = (Interop.IHTMLElement) styles.Item(i, i);
                        StyleElement o = htmlEditor.GenericElementFactory.CreateElement(style) as StyleElement;
                        _styleTagCollection.Add(o);
                    }
                }
                _styleTagCollection.OnInsertHandler += new CollectionInsertHandler(_styleTagCollection_OnInsertHandler);
                _styleTagCollection.OnClearHandler += new CollectionClearHandler(_styleTagCollection_OnClearHandler);
                return _styleTagCollection;
            }
            set
            {
                if (value == null)
                {
                    _styleTagCollection.Clear();
                    RemoveAllStyleElements();
                } 
                else 
                {
                    _styleTagCollection = (StyleElementCollection) value;
                    Interop.IHTMLDOMNode parent = GetHeadElement();
                    foreach (StyleElement se in _styleTagCollection)
                    {
                        parent.appendChild((Interop.IHTMLDOMNode) se.GetBaseElement());
                    }
                }
            }
        }

        /// <summary>
        /// This getter retrieves the collection of linked stylesheets.
        /// </summary>
        /// <remarks>It detects the stylesheets in link tags in a tolerant
        /// way, ether by checking the type attribute for "text/css" or the url in href attribute 
        /// ends with ".css".
        /// </remarks>
        [Category("Content"), Browsable(true), Description("")]
        [EditorAttribute(
           typeof(LinkCollectionEditor),
           typeof(UITypeEditor))]
		[DisplayName()]
        public ICollectionBase LinkedStylesheets
        {
            get
            {                
                _stylesCollection.OnInsertHandler -= new CollectionInsertHandler(_stylesCollection_OnInsertHandler);
                _stylesCollection.OnClearHandler -= new CollectionClearHandler(_stylesCollection_OnClearHandler);
                _stylesCollection.OnRemoveHandler -= new CollectionRemoveHandler(_stylesCollection_OnRemoveHandler);
                Interop.IHTMLElementCollection styles = ((Interop.IHTMLDocument3) this.msHtmlDocument).GetElementsByTagName("LINK");
                _stylesCollection.Clear();
                if (styles.GetLength() > 0)
                {                    
                    object[] pType = new object[1] { null };
                    object[] pHref = new object[1] { null };
                    for (int i = 0; i < styles.GetLength(); i++)
                    {
                        Interop.IHTMLElement link = (Interop.IHTMLElement) styles.Item(i, i);
                        link.GetAttribute("type", 0, pType);
                        link.GetAttribute("href", 0, pHref);
                        if ((pType[0] != null && pType[0].ToString().ToLower().Equals("text/css"))
                            ||
                            pHref[0] != null && pHref[0].ToString().ToLower().EndsWith(".css"))
                        {
                            LinkElement o = htmlEditor.GenericElementFactory.CreateElement(link) as LinkElement;
                            _stylesCollection.Add(o);
                        }
                    }
                }
                _stylesCollection.OnInsertHandler += new CollectionInsertHandler(_stylesCollection_OnInsertHandler);
                _stylesCollection.OnClearHandler += new CollectionClearHandler(_stylesCollection_OnClearHandler);
                _stylesCollection.OnRemoveHandler += new CollectionRemoveHandler(_stylesCollection_OnRemoveHandler);
                return _stylesCollection;
            }
            set
            {
                if (value == null)
                {
                    _stylesCollection.Clear();
                    RemoveAllLinkElements("text/css");
                } 
                else 
                {
                    _stylesCollection = (LinkElementCollection) value;
                    Interop.IHTMLDOMNode parent = GetHeadElement();
                    foreach (StyleElement se in _stylesCollection)
                    {
                        parent.appendChild((Interop.IHTMLDOMNode) se.GetBaseElement());
                    }                    
                }
            }
        }

        /// <summary>
        /// Sets or gets the document title.
        /// </summary>
        /// <remarks>
        /// If rendered in a browser the title appears in the window's title bar.
        /// In HTML it's rendered into the &lt;title&gt; tag on the head section.
        /// </remarks>
        [Category("Content"), Browsable(true), Description("Title_Body")]
		[DisplayName("Title_Body")]
        public string Title
        {
            get
            {
                return msHtmlDocument.GetTitle();
            }
            set
            {
                msHtmlDocument.SetTitle(value);
            }
        }

        /// <summary>
        /// Gets the current documents encoding.
        /// </summary>
        [Browsable(false)]
        public Encoding Encoding
        {
            get
            {
                try
                {
                    return Encoding.GetEncoding(msHtmlDocument.GetCharset());
                }
                catch
                {
                    return Encoding.ASCII;
                }
            }
        }

        /// <summary>
        /// Gets the character set used by the document.
        /// </summary>
        [Category("Content"), Browsable(true), Description("")]

		[DisplayName()]

        public string charset
        {
            get
            {
                return this.msHtmlDocument.GetCharset();
            }
        }

        /// <summary>
        /// Gets the URL or local path where the document comes from. Set to "about:blank" if newly created.
        /// </summary>
        [Category("Content"), Browsable(true), Description("")]

		[DisplayName()]

        public string URL
        {
            get
            {
                string url = this.msHtmlDocument.GetURL();
                // remove custom moniker, if any
                if (url.StartsWith("genesis:"))
                    url = url.Substring("genesis:".Length);
                // remove file moniker
                return HttpUtility.UrlDecode(url).Replace("file://", "");
            }
        }

        /// <summary>
        /// Returns the base path of the document without filename.
        /// </summary>
        /// <remarks>Used to build relative paths
        /// (links) from this document to subdocs, like script blocks, stylesheets etc.
        /// If the document is in browse mode and URL returns a URI beginning with "http:", we
        /// return nothing here (String.Empty).
        /// </remarks>
        [Category("Content"), Browsable(false), Description("")]
        public string BasePath
        {
            get
            {
                string url = this.URL;
                if (url.StartsWith("http"))
                {
                    return String.Empty;
                } 
                else 
                {
                    return Path.GetDirectoryName(url);
                }
            }
        }

        /// <summary>
        /// The time it takes to load the document with a 56K modem connection. This value does
        /// not calculate the embedded elements, images and so on.
        /// </summary>

        [Category("Information"), Browsable(true), Description("")]
		[DisplayName()]

        public string LoadTime56K
        {
            get
            {
                try
                {
                    return String.Format("{0:N2} sec.", (Convert.ToDouble(FileSize) * 9.0 / 56000.0) + 1.0);
                }
                catch
                {
                    return "n/a";
                }

            }
        }

        /// <summary>
        /// The file size in bytes.
        /// </summary>
        [Category("Information"), Browsable(true), Description("")]
		[DisplayName()]
        public long FileSize
        {
            get
            {
                try
                {
                    Interop.IPersistStreamInit streamInit = (Interop.IPersistStreamInit) this.msHtmlDocument;
                    IStream iStream;
                    Win32.CreateStreamOnHGlobal(Interop.NullIntPtr, true, out iStream);
                    streamInit.Save(iStream, 1);
                    STATSTG statstg;
                    iStream.Stat(out statstg, 1);
                    return statstg.cbSize;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Time and date when the file was last modified.
        /// </summary>

        [Category("Information"), Browsable(true), Description("")]
		[DisplayName()]

        public string FileLastModified
        {
            get
            {
                try
                {
                    return this.msHtmlDocument.GetLastModified();
                }
                catch
                {
                    return "n/a";
                }
            }
        }

        /// <summary>
        /// The number of frames defined in this document. Does not recognize IFrame tags.
        /// </summary>

        [Category("Information"), Browsable(true), Description("")]
		[DisplayName()]

        public int NumberOfFrames
        {
            get
            {
                Interop.IHTMLElementCollection eColl = ((Interop.IHTMLDocument3) this.msHtmlDocument).GetElementsByTagName("FRAME");
                if (eColl == null)
                    return 0;
                else
                    return eColl.GetLength();
            }
        }

        /// <summary>
        /// The number of styles defined in this document. Does not recognize inline style tags.
        /// </summary>

        [Category("Information"), Browsable(true), Description("")]
		[DisplayName()]

        public int NumberOfStylesheets
        {
            get
            {
                Interop.IHTMLStyleSheetsCollection eColl = this.msHtmlDocument.GetStyleSheets();
                if (eColl == null)
                    return 0;
                else
                    return eColl.Length;
            }
        }

        /// <summary>
        /// The number of embedded images.
        /// </summary>

        [Category("Information"), Browsable(true), Description("")]
        [DisplayName()]

		public int NumberOfImages
        {
            get
            {
                Interop.IHTMLElementCollection eColl = this.msHtmlDocument.GetImages();
                if (eColl == null)
                    return 0;
                else
                    return eColl.GetLength();
            }
        }

        /// <summary>
        /// The number of script blocks.
        /// </summary>

        [Category("Information"), Browsable(true), Description("")]
		[DisplayName()]

        public int NumberOfScriptBlocks
        {
            get
            {
                Interop.IHTMLElementCollection eColl = this.msHtmlDocument.GetScripts();
                if (eColl == null)
                    return 0;
                else
                    return eColl.GetLength();
            }
        }

        /// <summary>
        /// The total number of meta tags.
        /// </summary>

        [Category("Information"), Browsable(true), Description("")]
        [DisplayName()]

		public int NumberOfMetaTags
        {
            get
            {
                Interop.IHTMLElementCollection eColl = ((Interop.IHTMLDocument3) this.msHtmlDocument).GetElementsByTagName("META");
                if (eColl == null)
                    return 0;
                else
                    return eColl.GetLength();
            }
        }

        /// <summary>
        /// Overridden to help designers displaying the object to the user.
        /// </summary>
        /// <returns>The string [DOCUMENT]</returns>
        public override string ToString()
        {
			// TODO: Localize by hand using a property
            return "[DOCUMENT]";
        }

        # endregion

        # region Event handler and helper methods for editable collections

        private void SetBaseNode(string Href, string Target)
        {
            Interop.IHTMLDOMNode head = GetHeadElement();
            Interop.IHTMLElementCollection baseElements = GetHeadElements("BASE");
            if (baseElements != null && baseElements.GetLength() > 0)
            {
                RemoveBaseNode();
            }
            if (head != null)
            {
                Interop.IHTMLElement @base = this.msHtmlDocument.CreateElement("BASE");
                if (Href != null)
                {
                    @base.SetAttribute("href", Href, 0);
                }
                if (Target != null)
                {
                    @base.SetAttribute("target", Target, 0);
                }
                head.appendChild((Interop.IHTMLDOMNode) @base);
            }
        }

        /// <summary>
        /// Removes the BASE element and returns the href attribute for further reference. 
        /// </summary>
        /// <returns>href</returns>
        internal string RemoveBaseNode()
        {
            Interop.IHTMLElementCollection eColl = GetHeadElements("BASE");            
            if (eColl != null)
            {
                Interop.IHTMLDOMNode parent = GetHeadElement();
                string[] hrefs = new string[eColl.GetLength()];
                for (int i = 0; i < eColl.GetLength(); i++)
                {
                    Interop.IHTMLElement element = eColl.Item(i, i) as Interop.IHTMLElement;
                    try
                    {
                        object[] locals = new object[1];
                        locals[0] = null;
                        if (element != null) element.GetAttribute("href", 0, locals);
                        object local1 = locals[0];
                        if (local1 is DBNull)
                        {
                            local1 = null;
                        }
                        hrefs[i] = (local1 == null) ? String.Empty : local1.ToString();
                    }
                    catch
                    {
                        hrefs[i] = String.Empty;
                    }
                    parent.removeChild((Interop.IHTMLDOMNode) element);
                }
                return String.Join(",", hrefs);
            } 
            else 
            {
                return String.Empty;
            }
        }

        private Interop.IHTMLDOMNode GetHeadElement()
        {
            Interop.IHTMLDOMNode parent = null;
            try
            {
                Interop.IHTMLElementCollection eColl = ((Interop.IHTMLDocument3) this.msHtmlDocument).GetElementsByTagName("HEAD");
                parent = (Interop.IHTMLDOMNode)((Interop.IHTMLElement) eColl.Item(0, 0));
                if (parent == null)
                {
                    return null;
                }
            }
            catch
            {
                // ignore errors to support empty documents
            }
            return parent;
        }

        /// <summary>
        /// Get the collection of head elements filtered by the given tagname. Throws an exception if the head is not 
        /// structured as expected. 
        /// </summary>
        /// <param name="tagName">Tagname to filter</param>
        /// <returns>Interop.IHTMLElementCollection or null, if there are no such elements in the head section.</returns>
        private Interop.IHTMLElementCollection GetHeadElements(string tagName)
        {
            Interop.IHTMLElementCollection eColl;
            eColl = ((Interop.IHTMLDocument3) this.msHtmlDocument).GetElementsByTagName(tagName.ToUpper());
            if (eColl.GetLength() > 0)
            {
                Interop.IHTMLDOMNode parent = (Interop.IHTMLDOMNode)((Interop.IHTMLElement) eColl.Item(0, 0)).GetParentElement();
                if (parent.nodeName.ToUpper() != "HEAD")
                {
                    throw new ArgumentException("The document is not well structured. Please close and reload immediataly.");
                }
                return eColl;
            }
            return null;
        }

        /// <summary>
        /// Removes all META tags in header to prepare the document to load the changed list after insert or remove actions.
        /// </summary>
        /// <param name="attribute">Remove only tags with this existing attribute, ether "name" or "http-equiv".</param>
        private void RemoveAllMetaElements(string attribute)
        {
            Interop.IHTMLElementCollection eColl = GetHeadElements("META");
            if (eColl == null) return;
            Interop.IHTMLDOMNode parent = GetHeadElement();
            int oldLength = eColl.GetLength();
            ArrayList remainingTags = new ArrayList();
            if (oldLength > 0)
            {                
                // Remove all Elements, preserve the other type in remainingTags
                for (int i = 0; i < oldLength; i++)
                {
                    Interop.IHTMLMetaElement element = eColl.Item(0, 0) as Interop.IHTMLMetaElement;
                    switch (attribute)
                    {
                        case "name":
                            if (element.name == null || element.name.Equals(String.Empty))
                            {
                                remainingTags.Add(element);
                            }
                            break;
                        case "http-equiv":
                            if (element.httpEquiv == null || element.httpEquiv.Equals(String.Empty))
                            {
                                remainingTags.Add(element);
                            }
                            break;
                    }
                    parent.removeChild((Interop.IHTMLDOMNode) element);
                }
            } 
            // restore the remainingTags list
            foreach (Interop.IHTMLMetaElement rTag in remainingTags)
            {
                parent.appendChild((Interop.IHTMLDOMNode) rTag);
            }
        }

        /// <summary>
        /// Removes all LINK tags in header to prepare the document to load the changed list after insert or remove actions.
        /// </summary>
        /// <param name="typeParameter">Remove only tags with this existing attribute (text/css commonly).</param>
        private void RemoveAllLinkElements(string typeParameter)
        {
            Interop.IHTMLElementCollection eColl = GetHeadElements("LINK");
            while (eColl != null && eColl.GetLength() > 0)
            {
                Interop.IHTMLDOMNode parent = GetHeadElement();
                int oldLength = eColl.GetLength();
                for (int i = 0; i < oldLength; i++)
                {
                    Interop.IHTMLLinkElement element = eColl.Item(0, 0) as Interop.IHTMLLinkElement;
                    if (element.type == typeParameter)
                    {
                        parent.removeChild((Interop.IHTMLDOMNode) element);
                    } 
                    else 
                    {
                        continue;
                    }                    
                }
            } 
        }

        /// <summary>
        /// Removes all STYLE tags in header to prepare the document to load the changed list after insert or remove actions.
        /// </summary>
        private void RemoveAllStyleElements()
        {
            Interop.IHTMLElementCollection eColl = GetHeadElements("STYLE");
            while (eColl != null && eColl.GetLength() > 0)
            {
                Interop.IHTMLDOMNode parent = GetHeadElement();
                int oldLength = eColl.GetLength();
                for (int i = 0; i < oldLength; i++)
                {
                    Interop.IHTMLStyleElement element = eColl.Item(0, 0) as Interop.IHTMLStyleElement;
                    parent.removeChild((Interop.IHTMLDOMNode) element);
                }
            } 
        }

        /// <summary>
        /// Removes all script elements defined in the head area. TODO: additional param to control scripts in body?
        /// </summary>
        private void RemoveAllScriptElements()
        {
            Interop.IHTMLElementCollection eColl = GetHeadElements("SCRIPT");
            while (eColl != null && eColl.GetLength() > 0)
            {
                Interop.IHTMLDOMNode parent = GetHeadElement();
                int oldLength = eColl.GetLength();
                for (int i = 0; i < oldLength; i++)
                {
                    Interop.IHTMLScriptElement element = eColl.Item(0, 0) as Interop.IHTMLScriptElement;
                    parent.removeChild((Interop.IHTMLDOMNode) element);
                }
            } 
        }

        private void _metasCollection_OnInsertHandler(int index, object value)
        {
            Interop.IHTMLDOMNode parent = GetHeadElement();
            if (parent != null)
            {
                parent.appendChild((Interop.IHTMLDOMNode) ((MetaElement)value).GetBaseElement());
            }
        }

        private void _equivCollection_OnInsertHandler(int index, object value)
        {
            Interop.IHTMLDOMNode parent = GetHeadElement();
            if (parent != null)
            {
                parent.appendChild((Interop.IHTMLDOMNode) ((MetaElement)value).GetBaseElement());
            }
        }

        private void _stylesCollection_OnRemoveHandler(int index, object value)
        {
            LinkElement lnk = (LinkElement)value;
            Interop.IHTMLDOMNode node = (Interop.IHTMLDOMNode)lnk.GetBaseElement();
            node.removeNode(true);
        }

        private void _stylesCollection_OnInsertHandler(int index, object value)
        {
            Interop.IHTMLDOMNode parent = GetHeadElement();
            if (parent != null)
            {
                Interop.IHTMLElement element = ((LinkElement)value).GetBaseElement();
                parent.appendChild((Interop.IHTMLDOMNode) element);
            }
        }

        private void _styleTagCollection_OnInsertHandler(int index, object value)
        {
            Interop.IHTMLDOMNode parent = GetHeadElement();
            if (parent != null)
            {
                Interop.IHTMLElement element = ((StyleElement)value).GetBaseElement();
                parent.appendChild((Interop.IHTMLDOMNode) element);
            }
        }

        private void _scriptCollection_OnInsertHandler(int index, object value)
        {
            Interop.IHTMLDOMNode parent = GetHeadElement();
            if (parent != null)
            {
                Interop.IHTMLElement element = ((ScriptElement)value).GetBaseElement();
                parent.appendChild((Interop.IHTMLDOMNode) element);
            }
        }
        
        private void _metasCollection_OnClearHandler()
        {
            RemoveAllMetaElements("name");
        }

        private void _equivCollection_OnClearHandler()
        {
            RemoveAllMetaElements("http-equiv");
        }

        private void _stylesCollection_OnClearHandler()
        {
            RemoveAllLinkElements("text/css");
        }

        private void _styleTagCollection_OnClearHandler()
        {
            RemoveAllStyleElements();
        }

        private void _scriptCollection_OnClearHandler()
        {
            RemoveAllScriptElements();
        }

        # endregion
    
        # region Events

            protected void OnHelp(DocumentEventArgs e)
            {
                if (Help != null) { Help(this, e); }
            }

            protected void OnClick(DocumentEventArgs e)
            {
               if (Click != null) { Click(this, e); }
            }

            protected void OnDblClick(DocumentEventArgs e)
            {
                if (DblClick != null) { DblClick(this, e); }
            }

            protected void OnKeyDown(DocumentEventArgs e)
            {
                if (KeyDown != null) { KeyDown(this, e); }
            }

            protected void OnKeyUp(DocumentEventArgs e)
            {
                if (KeyUp != null) { KeyUp(this, e); }
            }

            protected void OnKeyPress(DocumentEventArgs e)
            {
                if (KeyPress != null) { KeyPress(this, e); }
            }

            protected void OnMouseDown(DocumentEventArgs e)
            {
                if (MouseDown != null) { MouseDown(this, e); }
            }

            protected void OnMouseMove(DocumentEventArgs e)
            {
                if (MouseMove != null) { MouseMove(this, e); }
            }

            protected void OnMouseUp(DocumentEventArgs e)
            {
                if (MouseUp != null) { MouseUp(this, e); }
            }

            protected void OnMouseOut(DocumentEventArgs e)
            {
                if (MouseOut != null) { MouseOut(this, e); }
            }

            protected void OnMouseOver(DocumentEventArgs e)
            {
                if (MouseOver != null) { MouseOver(this, e); }
            }

            protected void OnReadystateChange(DocumentEventArgs e)
            {
                if (ReadystateChange != null) { ReadystateChange(this, e); }
            }

            protected void OnBeforeUpdate(DocumentEventArgs e)
            {
                if (BeforeUpdate != null) { BeforeUpdate(this, e); }
            }

            protected void OnAfterUpdate(DocumentEventArgs e)
            {
                if (AfterUpdate != null) { AfterUpdate(this, e); }
            }

            protected void OnRowExit(DocumentEventArgs e)
            {
                if (RowExit != null) { RowExit(this, e); }
            }

            protected void OnRowEnter(DocumentEventArgs e)
            {
                if (RowEnter != null) { RowEnter(this, e); }
            }

            protected void OnDragStart(DocumentEventArgs e)
            {
                if (DragStart != null) { DragStart(this, e); }
            }

            protected void OnSelectStart(DocumentEventArgs e)
            {
                if (SelectStart != null) { SelectStart(this, e); }
            }

            protected void OnErrorUpdate(DocumentEventArgs e)
            {
                if (ErrorUpdate != null) { ErrorUpdate(this, e); }
            }

            protected void OnContextMenu(DocumentEventArgs e)
            {
                if (ContextMenu != null) { ContextMenu(this, e); }
            }

            protected void OnStop(DocumentEventArgs e)
            {
                if (Stop != null) { Stop(this, e); }
            }

            protected void OnRowsDelete(DocumentEventArgs e)
            {
                if (RowsDelete != null) { RowsDelete(this, e); }
            }

            protected void OnRowsInserted(DocumentEventArgs e)
            {
                if (RowsInserted != null) { RowsInserted(this, e); }
            }

            protected void OnCellChange(DocumentEventArgs e)
            {
                if (CellChange != null) { CellChange(this, e); }
            }

            protected void OnPropertyChange(DocumentEventArgs e)
            {
                if (PropertyChange != null) { PropertyChange(this, e); }
            }

            protected void OnDatasetChanged(DocumentEventArgs e)
            {
                if (DatasetChanged != null) { DatasetChanged(this, e); }
            }

            protected void OnDataAvailable(DocumentEventArgs e)
            {
                if (DataAvailable != null) { DataAvailable(this, e); }
            }

            protected void OnDatasetComplete(DocumentEventArgs e)
            {
                if (DatasetComplete != null) { DatasetComplete(this, e); }
            }

            protected void OnBeforeEditfocus(DocumentEventArgs e)
            {
                if (BeforeEditFocus != null) { BeforeEditFocus(this, e); }
            }

            protected void OnSelectionChange(DocumentEventArgs e)
            {
                if (SelectionChange != null && e.SrcElement != null) { SelectionChange(this, e); }
            }

            protected void OnControlSelect(DocumentEventArgs e)
            {
                if (ControlSelect != null) { ControlSelect(this, e); }
            }

            protected void OnMouseWheel(DocumentEventArgs e)
            {
                if (MouseWheel != null) { MouseWheel(this, e); }
            }

            protected void OnFocusIn(DocumentEventArgs e)
            {
                if (FocusIn != null) { FocusIn(this, e); }
            }

            protected void OnFocusOut(DocumentEventArgs e)
            {
                if (FocusOut != null) { FocusOut(this, e); }
            }

            protected void OnActivate(DocumentEventArgs e)
            {
                if (Activate != null) { Activate(this, e); }
            }

            protected void OnDeactivate(DocumentEventArgs e)
            {
                if (Deactivate != null) { Deactivate(this, e); }
            }

            protected void OnBeforeActivate(DocumentEventArgs e)
            {
                if (BeforeActivate != null) { BeforeActivate(this, e); }
            }

            protected void OnBeforeDeactivate(DocumentEventArgs e)
            {
                if (BeforeDeactivate != null) { BeforeDeactivate(this, e); }                
            }

            /// <summary>
            /// Fires when the user presses the F1 key while the browser is the active window.
            /// </summary>
            public event DocumentEventHandler Help;
            /// <summary>
            /// Fires when the user clicks the left mouse button on the object. 
            /// </summary>
            public event DocumentEventHandler Click;
            /// <summary>
            /// Fires when the user double-clicks the object.
            /// </summary>
            public event DocumentEventHandler DblClick;
            /// <summary>
            /// Fires when the user presses a key.
            /// </summary>
            public event DocumentEventHandler KeyDown;
            /// <summary>
            /// Fires when the user releases a key.
            /// </summary>
            public event DocumentEventHandler KeyUp;
            /// <summary>
            /// Fires when the user presses an alphanumeric key.
            /// </summary>
            public event DocumentEventHandler KeyPress;
            /// <summary>
            /// Fires when the user clicks the object with either mouse button.
            /// </summary>
            public event DocumentEventHandler MouseDown;
            /// <summary>
            /// Fires when the user moves the mouse over the object.
            /// </summary>
            public event DocumentEventHandler MouseMove;
            /// <summary>
            /// Fires when the user releases a mouse button while the mouse is over the object.
            /// </summary>
            public event DocumentEventHandler MouseUp;
            /// <summary>
            /// Fires when the user moves the mouse pointer outside the boundaries of the object.
            /// </summary>
            public event DocumentEventHandler MouseOut;
            /// <summary>
            /// Fires when the user moves the mouse pointer into the object.
            /// </summary>
            public event DocumentEventHandler MouseOver;
            /// <summary>
            /// Fires when the state of the object has changed.
            /// </summary>
            public event DocumentEventHandler ReadystateChange;
            /// <summary>
            /// Fires on a databound object before updating the associated data in the data source object.
            /// </summary>
            public event DocumentEventHandler BeforeUpdate;
            /// <summary>
            /// Fires on a databound object after successfully updating the associated data in the data source object.
            /// </summary>
            public event DocumentEventHandler AfterUpdate;
            /// <summary>
            /// Fires just before the data source control changes the current row in the object.
            /// </summary>
            public event DocumentEventHandler RowExit;
            /// <summary>
            /// Fires to indicate that the current row has changed in the data source and new data values are available on the object. 
            /// </summary>
            public event DocumentEventHandler RowEnter;
            /// <summary>
            /// Fires on the source object when the user starts to drag a text selection or selected object.
            /// </summary>
            public event DocumentEventHandler DragStart;
            /// <summary>
            /// Fires when the object is being selected.
            /// </summary>
            public event DocumentEventHandler SelectStart;
            /// <summary>
            /// Fires on a databound object when an error occurs while updating the associated data in the data source object.
            /// </summary>
            public event DocumentEventHandler ErrorUpdate;
            /// <summary>
            /// Fires when the user clicks the right mouse button in the client area, opening the context menu.
            /// </summary>
            public event DocumentEventHandler ContextMenu;
            /// <summary>
            /// Fires when the user leaves the Web page.
            /// </summary>
            public event DocumentEventHandler Stop;
            /// <summary>
            /// Fires when rows are about to be deleted from the recordset.
            /// </summary>
            public event DocumentEventHandler RowsDelete;
            /// <summary>
            /// Fires just after new rows are inserted in the current recordset.
            /// </summary>
            public event DocumentEventHandler RowsInserted;
            /// <summary>
            /// Fires when data changes in the data provider.
            /// </summary>
            public event DocumentEventHandler CellChange;
            /// <summary>
            /// Fires when a property changes on the object.
            /// </summary>
            public event DocumentEventHandler PropertyChange;
            /// <summary>
            /// Fires when the data set exposed by a data source object changes.
            /// </summary>
            public event DocumentEventHandler DatasetChanged;
            /// <summary>
            /// Fires periodically as data arrives from data source objects that asynchronously transmit their data.
            /// </summary>
            public event DocumentEventHandler DataAvailable;
            /// <summary>
            /// Fires to indicate that all data is available from the data source object.
            /// </summary>
            public event DocumentEventHandler DatasetComplete;
            /// <summary>
            /// Fires before an object contained in an editable element enters a UI-activated state or when an editable container object is control selected.
            /// </summary>
            public event DocumentEventHandler BeforeEditFocus;
            /// <summary>
            /// Fires when the selection state of a document changes.
            /// </summary>
            public event DocumentEventHandler SelectionChange;
            /// <summary>
            /// Fires when the user is about to make a control selection of the object.
            /// </summary>
            public event DocumentEventHandler ControlSelect;
            /// <summary>
            /// Fires when the wheel button is rotated.
            /// </summary>
            public event DocumentEventHandler MouseWheel;
            /// <summary>
            /// Fires for an element just prior to setting focus on that element.
            /// </summary>
            public event DocumentEventHandler FocusIn;
            /// <summary>
            /// Fires for the current element with focus immediately after moving focus to another element.
            /// </summary>
            public event DocumentEventHandler FocusOut;
            /// <summary>
            /// Fires when the object is set as the active element.
            /// </summary>
            public event DocumentEventHandler Activate;
            /// <summary>
            /// Fires when the activeElement is changed from the current object to another object in the parent document.
            /// </summary>
            public event DocumentEventHandler Deactivate;
            /// <summary>
            /// Fires immediately before the object is set as the active element.
            /// </summary>
            public event DocumentEventHandler BeforeActivate;
            /// <summary>
            /// Fires immediately before the activeElement is changed from the current object to another object in the parent document.
            /// </summary>
            public event DocumentEventHandler BeforeDeactivate;

            internal void InvokeHelp(Interop.IHTMLEventObj e)
            {
                if (Help != null) { OnHelp(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeClick(Interop.IHTMLEventObj e)
            {
               if (Click != null) { OnClick(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeDblClick(Interop.IHTMLEventObj e)
            {
                if (DblClick != null) { OnDblClick(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeKeyDown(Interop.IHTMLEventObj e)
            {
                if (KeyDown != null) { OnKeyDown(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeKeyUp(Interop.IHTMLEventObj e)
            {
                if (KeyUp != null) { OnKeyUp(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeKeyPress(Interop.IHTMLEventObj e)
            {
                if (KeyPress != null) { OnKeyPress(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeMouseDown(Interop.IHTMLEventObj e)
            {
                if (MouseDown != null) { OnMouseDown(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeMouseMove(Interop.IHTMLEventObj e)
            {
                if (MouseMove != null) { OnMouseMove(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeMouseUp(Interop.IHTMLEventObj e)
            {
                if (MouseUp != null) { OnMouseUp(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeMouseOut(Interop.IHTMLEventObj e)
            {
                if (MouseOut != null) { OnMouseOut(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeMouseOver(Interop.IHTMLEventObj e)
            {
                if (MouseOver != null) { OnMouseOver(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeReadystateChange(Interop.IHTMLEventObj e)
            {
                if (ReadystateChange != null) { OnReadystateChange(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeBeforeUpdate(Interop.IHTMLEventObj e)
            {
                if (BeforeUpdate != null) { OnBeforeUpdate(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeAfterupdate(Interop.IHTMLEventObj e)
            {
                if (AfterUpdate != null) { OnAfterUpdate(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeRowExit(Interop.IHTMLEventObj e)
            {
                if (RowExit != null) { OnRowExit(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeRowEnter(Interop.IHTMLEventObj e)
            {
                if (RowEnter != null) { OnRowEnter(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeDragstart(Interop.IHTMLEventObj e)
            {
                if (DragStart != null) { OnDragStart(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeSelectstart(Interop.IHTMLEventObj e)
            {
                if (SelectStart != null) { OnSelectStart(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeErrorUpdate(Interop.IHTMLEventObj e)
            {
                if (ErrorUpdate != null) { OnErrorUpdate(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeContextMenu(Interop.IHTMLEventObj e)
            {
                if (ContextMenu != null) { OnContextMenu(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeStop(Interop.IHTMLEventObj e)
            {
                if (Stop != null) { OnStop(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeRowsDelete(Interop.IHTMLEventObj e)
            {
                if (RowsDelete != null) { OnRowsDelete(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeRowsInserted(Interop.IHTMLEventObj e)
            {
                if (RowsInserted != null) { OnRowsInserted(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeCellchange(Interop.IHTMLEventObj e)
            {
                if (CellChange != null) { OnCellChange(new DocumentEventArgs(e, null)); }
            }

            internal void InvokePropertyChange(Interop.IHTMLEventObj e)
            {
                if (PropertyChange != null) { OnPropertyChange(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeDatasetChanged(Interop.IHTMLEventObj e)
            {
                if (DatasetChanged != null) { OnDatasetChanged(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeDataavailable(Interop.IHTMLEventObj e)
            {
                if (DataAvailable != null) { OnDataAvailable(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeDatasetComplete(Interop.IHTMLEventObj e)
            {
                if (DatasetComplete != null) { OnDatasetComplete(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeBeforeeditfocus(Interop.IHTMLEventObj e)
            {
                if (BeforeEditFocus != null) { OnBeforeEditfocus(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeSelectionchange(Interop.IHTMLEventObj e)
            {
                if (SelectionChange != null) { OnSelectionChange(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeControlselect(Interop.IHTMLEventObj e)
            {
                if (ControlSelect != null) { OnControlSelect(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeMousewheel(Interop.IHTMLEventObj e)
            {
                if (MouseWheel != null) { OnMouseWheel(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeFocusin(Interop.IHTMLEventObj e)
            {
                if (FocusIn != null) { OnFocusIn(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeFocusout(Interop.IHTMLEventObj e)
            {
                if (FocusOut != null) { OnFocusOut(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeActivate(Interop.IHTMLEventObj e)
            {
                if (Activate != null) { OnActivate(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeDeactivate(Interop.IHTMLEventObj e)
            {
                if (Deactivate != null) { OnDeactivate(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeBeforeActivate(Interop.IHTMLEventObj e)
            {
                if (BeforeActivate != null) { OnBeforeActivate(new DocumentEventArgs(e, null)); }
            }

            internal void InvokeBeforeDeactivate(Interop.IHTMLEventObj e)
            {
                if (BeforeDeactivate != null) { OnBeforeDeactivate(new DocumentEventArgs(e, null)); }                
            }



        # endregion

        #region ICustomTypeDescriptor Member

        PropertyDescriptorCollection pdc = null;

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] filter) 
        { 
            if (pdc == null)
            {
                PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(GetType(), filter); 
                // notice we use the type here so we don't recurse 
                PropertyDescriptor[] newProps = new PropertyDescriptor[baseProps.Count]; 
                for (int i = 0; i < baseProps.Count; i++) 
                { 
                    newProps[i] = new CustomPropertyDescriptor(baseProps[i], filter); 
                } 
                // probably wanna cache this... 
                pdc = new PropertyDescriptorCollection(newProps); 
            }
            return pdc;
        } 
 
        AttributeCollection ICustomTypeDescriptor.GetAttributes() 
        { 
            return TypeDescriptor.GetAttributes(this, true); 
        } 
 
        string ICustomTypeDescriptor.GetClassName() 
        { 
            return TypeDescriptor.GetClassName(this, true); 
        } 
 
        string ICustomTypeDescriptor.GetComponentName() 
        { 
            return TypeDescriptor.GetComponentName(this, true); 
        } 
 
        TypeConverter ICustomTypeDescriptor.GetConverter() 
        { 
            return TypeDescriptor.GetConverter(this, true); 
        } 
 
        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() 
        { 
            return TypeDescriptor.GetDefaultEvent(this, true); 
        } 
 
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes) 
        { 
            return TypeDescriptor.GetEvents(this, attributes, true); 
        } 
 
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents() 
        { 
            return TypeDescriptor.GetEvents(this, true); 
        } 
 
        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() 
        { 
            return TypeDescriptor.GetDefaultProperty(this, true); 
        } 
 
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() 
        { 
            return TypeDescriptor.GetProperties(this, true); 
        } 
 
        object ICustomTypeDescriptor.GetEditor(Type editorBaseType) 
        { 
            return TypeDescriptor.GetEditor(this, editorBaseType, true); 
        } 
 
        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) 
        { 
            return this; 
        } 
 
        #endregion

        		///<overloads/>
        /// <summary>
        /// Creates a style sheet for the document.
        /// </summary>
        /// <param name="linkedStyleSheet">Specifies how to add the style sheet to the document. 
        /// If a file name is specified for the URL, the style information will be added as a link object. 
        /// If the URL contains style information, this information will be added to the style object.</param>
        /// <returns>Returns StyleSheet information.</returns>
        public IStyleSheet CreateStyleSheet(string linkedStyleSheet)
        {
            return CreateStyleSheet(linkedStyleSheet, 0);
        }
        
        /// <summary>
        /// Creates a style sheet for the document.
        /// </summary>
        /// <param name="linkedStyleSheet">Specifies how to add the style sheet to the document. If a file name is specified for the URL, the style information will be added as a link object. If the URL contains style information, this information will be added to the style object.</param>
        /// <param name="index">Specifies the index that indicates where the new style sheet is inserted in the styleSheets collection.</param>
        /// <returns>Returns StyleSheet information.</returns>
        public IStyleSheet CreateStyleSheet(string linkedStyleSheet, int index)
        {
            Interop.IHTMLStyleSheet styleSheet = this.msHtmlDocument.CreateStyleSheet(linkedStyleSheet, index);
            if (styleSheet != null)
            {
                StyleSheet ss = new StyleSheet(styleSheet, htmlEditor);
                return ss;
            }
            return null;
        }

        /// <summary>
        /// This method returns all style selectors linked with the current document this element is in.
        /// </summary>
        /// <remarks>
        /// The type will be recognized by the sign typically used for selectors, like ".class", "#id", and "@rule".
        /// </remarks>
        /// <param name="selectorType">String with the type</param>
        /// <returns>Array of objects</returns>
        public IStyleRule[] GetDocumentStyleSelectors(string selectorType)
        {
            Interop.IHTMLStyleSheetRulesCollection rules;
            Interop.IHTMLStyleSheet ssh;
            // search linked styles
            IStyleRule[] s1 = new IStyleRule[0];
            IStyleRule[] s2 = new IStyleRule[0];
            LinkElementCollection stylesL = (LinkElementCollection) this.LinkedStylesheets;
            if (stylesL.Count > 0)
            {
                for (int s = 0; s < stylesL.Count; s++)
                {
                    LinkElement link = stylesL[s];
                    if (link != null)
                    {
                        Interop.IHTMLLinkElement le;
                        le = (Interop.IHTMLLinkElement)link.GetBaseElement();
                        ssh = le.styleSheet;
                        rules = ssh.GetRules();
                        if (rules.GetLength() > 0)
                        {
                            s1 = Helper.AddRuleFromCollection(selectorType, rules);
                        }
                    }
                }
            }
            // search embedded styles
            StyleElementCollection stylesS = (StyleElementCollection) this.EmbeddedStylesheets;
            if (stylesS.Count > 0)
            {
                for (int s = 0; s < stylesS.Count; s++)
                {
                    StyleElement style = stylesS[s];
                    if (style != null)
                    {
                        Interop.IHTMLStyleElement se;
                        se = (Interop.IHTMLStyleElement)style.GetBaseElement();
                        ssh = se.styleSheet;
                        rules = ssh.GetRules();
                        if (rules.GetLength() > 0)
                        {
                            s2 = Helper.AddRuleFromCollection(selectorType, rules);
                        }
                    }
                }
            }
            IStyleRule[] selObj = new IStyleRule[s1.Length + s2.Length];
            Array.Copy(s1, 0, selObj, 0, s1.Length);
            Array.Copy(s2, 0, selObj, s1.Length, s2.Length);
            return selObj;
        }


        public void Dispose()
        {
            _scriptCollection.Clear();
            _stylesCollection.Clear();
            _styleTagCollection.Clear();
            _metasCollection.Clear();
            _metasCollection.msHtmlDocument = null;
            _equivCollection.Clear();
            _scriptCollection = null;
            _stylesCollection = null;
            _styleTagCollection = null;
            _metasCollection = null;
            _equivCollection = null;
        }
    }
}