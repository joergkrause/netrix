using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Web;
using System.Xml;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.Exceptions;
using GuruComponents.Netrix.HtmlFormatting;
using STATSTG=System.Runtime.InteropServices.ComTypes.STATSTG;

namespace GuruComponents.Netrix.WebEditing.Documents
{
	/// <summary>
	/// HtmlFrameSet represents a complete framed master document.
	/// </summary>
	/// <remarks>
	/// This is the document containing
	/// one or more FrameSet tags. It returns a collection of FrameWindow objects which contain the
	/// complete frames, including all content and site management. It also contains a collection
	/// of HtmlFrameSet objects which represent frame documents which contain frames themself. This
	/// is a recursive strategy so there is no limit in handling nested frames.
	/// <seealso cref="GuruComponents.Netrix.WebEditing.Documents.IFrameSet">IFrameSet</seealso>
	/// </remarks>
    public class HtmlFrameSet : IFrameSet
	{
        /// <summary>
        /// This class represents a single frame in the frameset.
        /// </summary>
        /// <remarks>
        /// Its objects are stored together with
        /// the related Site to handle the events.  
        /// </remarks>
        public class FrameWindow : ICloneable, IFrameWindow 
        {
            private Interop.IHTMLWindow2 window;
            private Interop.IHTMLDocument2 doc;
            private Interop.IHTMLFrameBase framebase;
            private Interop.IHTMLElement3 body;
            private HtmlFormatter htmlFormatter;
            private Interop.IPersistStreamInit persistStream;
            private Interop.IOleCommandTarget CommandTarget;
            private BodyElement bodyElement;
            private Encoding encoding = Encoding.GetEncoding("ISO-8859-1");
            private FrameElement nativeElement;
            private MSHTMLSite _relatedSite;
            private string url;
            private FrameEvents winEvents;

            internal FrameWindow(Interop.IHTMLWindow2 window, MSHTMLSite relatedSite, HtmlEditor htmlEditor)
            {
                this._relatedSite = relatedSite;
                this.window = window;
                this.doc = (Interop.IHTMLDocument2) window.document;
                this.url = ((Interop.IHTMLLocation) ((Interop.IHTMLWindow2) doc.GetParentWindow()).location).href;
                this.body = (Interop.IHTMLElement3) doc.GetBody();
                this.framebase = ((Interop.IHTMLWindow4) window).frameElement;
                this.htmlFormatter = new HtmlFormatter();
                this.CommandTarget = (Interop.IOleCommandTarget) doc;
                this.winEvents = new FrameEvents(window, htmlEditor);
                this.winEvents.Activate += new EventHandler(winEvents_Activate);
                this.winEvents.DeActivate += new EventHandler(winEvents_DeActivate);
                this.nativeElement = (FrameElement)htmlEditor.GenericElementFactory.CreateElement(framebase as Interop.IHTMLElement);
            }

            void winEvents_DeActivate(object sender, EventArgs e)
            {
                FrameDeActivated(this, new FrameEventArgs(this));
            }

            void winEvents_Activate(object sender, EventArgs e)
            {
                FrameActivated(this, new FrameEventArgs(this));
            }

            internal event FrameActivatedEventHandler FrameActivated;

            internal event FrameActivatedEventHandler FrameDeActivated;

            ~FrameWindow()
            {
            }

            # region Internal Methods

            /// <summary>
            /// Returns the document object of this frame. Used to set as active document
            /// after changing the active frame.
            /// </summary>
            /// <returns></returns>
            internal Interop.IHTMLDocument2 GetInteropDocument()
            {
                return this.doc;
            }

            internal Interop.IHTMLDocument2 GetInteropBody()
            {
                return (Interop.IHTMLDocument2) this.bodyElement.GetBaseElement().GetDocument();
            }

            /// <summary>
            /// Executes the specified command in this Frame.
            /// </summary>
            /// <param name="command"></param>
            protected internal void Exec(int command) 
            {
                Exec(command, null);
            }
      
            /// <summary>
            /// Executes the specified command in this Frame with the specified arguments.
            /// </summary>
            /// <param name="command"></param>
            /// <param name="argument"></param>
            protected internal void Exec(int command, object argument) 
            {
                Interop.OLEVARIANT nil = new Interop.OLEVARIANT();
                Interop.OLEVARIANT arg = new Interop.OLEVARIANT();
                if (argument != null)
                {
                    switch (argument.GetType().ToString())
                    {
                        case "System.String":
                            arg.LoadString((string) argument);
                            break;
                        case "System.Boolean":
                            arg.LoadBoolean((bool) argument);
                            break;
                        case "System.Int32":              
                        case "System.Int16":
                            arg.LoadInteger((int) argument);
                            break;
                        default:
                            throw new ArgumentException(String.Concat("Unknown Type: ",argument.GetType()," Argument: ",argument));
                    }
                }          
                int hr;
                try
                {
                    hr = CommandTarget.Exec(ref Interop.Guid_MSHTML, command, (int) Interop.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, arg, nil);
                    if (hr != Interop.S_OK)
                    {
                        throw new CommandUnavailableException(command, "HtmlEditor.Exec: Command fails. Returncode was: " + hr);
                    }
                }
                catch(Exception ex)
                {
                    throw new CommandUnavailableException(command, "HtmlEditor.Exec: Command fails. See for more details InnerException", ex);
                }
            }


            # endregion

            /// <summary>
            /// Deactivate the frame designer and remove all attached behaviors.
            /// </summary>
            public void DeactivateDesigner()
            {
                this.ContentEditable = false;
                this.RemoveAllBehaviors();
            }

            /// <summary>
            /// Removes all attached behaviors assigned to that frame.
            /// </summary>
            public void RemoveAllBehaviors()
            {
                if (nativeElement != null)
                {
                    nativeElement.ElementBehaviors.RemoveBehavior();
                }
            }

            /// <summary>
            /// Make the content editable in the editor. Gets the current editable state.
            /// </summary>
            public bool ContentEditable
            {
                get
                {
                    return (this.doc.GetDesignMode() == "on") ? true : false;
                }
                set
                {
                    this.doc.SetDesignMode((value) ? "on" : "off");
                }
            }

            /// <summary>
            /// Gets true if content was changed snce last save operation.
            /// </summary>
            public bool IsDirty
            {
                get
                {
                    return (persistStream.IsDirty() == Interop.S_OK) ? true : false;
                }
            }

            /// <summary>
            /// Sets or gets the current encoding for that frame.
            /// </summary>
            /// <remarks>
            /// It is not necessary to set
            /// this property as it defaults to the global Encoding of the main document.
            /// </remarks>
            public Encoding Encoding
            {
                get
                {
                    return this.encoding;
                }
                set
                {
                    this.encoding = value;
                }
            }

            /// <summary>
            /// Save the raw content into the file the frame was loaded from. Overwrites.
            /// </summary>
            public void SaveRawContent()
            {
                string content = this.GetRawContent();
                string url = this.GetFullPathUrl();
                StreamWriter sw = new StreamWriter(url, false, this.encoding);
                sw.Write(content);
                sw.Flush();
                sw.Close();
            }

            /// <summary>
            /// Returns the full path to the document based on the relative path and the current position
            /// in file system.
            /// </summary>
            /// <remarks>
            /// If the Source is a HTTP URL this method returns the Url as is.
            /// The internally used URI format with file:// moniker is removed before returning.
            /// </remarks>
            /// <returns>Full path in file format, leading monikers are removed, URL coding is decoded.</returns>
            public string GetFullPathUrl()
            {
                return HttpUtility.UrlDecode(this.url.Replace("file:///", ""));
            }

            /// <summary>
            /// Returns the raw content.
            /// </summary>
            /// <remarks>
            /// Usable if the host application has its own save method.
            /// </remarks>
            /// <returns>String with raw content</returns>
            public string GetRawContent()
            {
                string content = String.Empty;
                persistStream = (Interop.IPersistStreamInit) this.doc;
                IStream iStream;
                Win32.CreateStreamOnHGlobal(Interop.NullIntPtr, true, out iStream);
                persistStream.Save(iStream, 1);
                STATSTG statStg;
                iStream.Stat(out statStg, 1);
                int i = (int)statStg.cbSize;
                byte[] bs = new byte[(uint)i];
                IntPtr j;
                Win32.GetHGlobalFromStream(iStream, out j);
                IntPtr k = Win32.GlobalLock(j);
                if (k != Interop.NullIntPtr)
                {
                    Marshal.Copy(k, bs, 0, i);
                    Win32.GlobalUnlock(j);
                    StreamReader streamReader = null;
                    try
                    {
                        streamReader = new StreamReader(new MemoryStream(bs), Encoding.Default);
                        content = streamReader.ReadToEnd();
                        // HACK: A very dirty hack to remove the double http-equiv we get here; I've no idea what the reason is..
                        int i1 = 0;
                        while ((i1 = content.IndexOf(String.Concat(Environment.NewLine, "http-equiv"), i1)) > 0)
                        {
                            int i2 = content.IndexOf("\"", i1 + 14);
                            if (i1 > 0 && i2 > 0)
                            {
                                content = String.Concat(content.Substring(0, i1), content.Substring(++i2));
                            }
                            i1 = i2;
                        }
                    }
                    finally
                    {
                        if (streamReader != null)
                        {
                            streamReader.Close();
                        }
                    }
                }
//                this.ContentEditable = savedEditableState;
                return content;
            }

   
            /// <summary>
            /// Returns a string with the outer html of the body.
            /// </summary>
            /// <remarks>
            /// This is for further investigation only,
            /// not for saving, as it does not contain the full content.
            /// </remarks>
            /// <returns>String which contains the content of the frame.</returns>
            public string GetBodyContent()
            {
                return ((Interop.IHTMLElement) body).GetOuterHTML();
            }

            /// <summary>
            /// The name of the frame as it is in the name attribute of the frame definition.
            /// </summary>
            public string FrameName
            {
                get
                {
                    return framebase.name;
                }
                set
                {
                    framebase.name = value;
                }
            }

            /// <summary>
            /// The frame src attribute, mostly the filename and a relative path. URL format.
            /// </summary>
            public string FrameSrc
            {
                get
                {
                    return framebase.src;
                }
                set
                {
                    framebase.src = value;
                }
            }

            #region ICloneable Member

            /// <summary>
            /// Clones the object to give losed access to the properties. Cloning is recommend if the application
            /// changes the properties but disposes the objects later and let the garbage collector remove them.
            /// Without cloning it is possible that internal processes fail later due to missing references.
            /// </summary>
            /// <returns></returns>
            public object Clone()
            {
                return this.MemberwiseClone();
            }

            #endregion

            #region IFrameWindow Member

            /// <summary>
            /// Add a behavior to this frame.
            /// </summary>
            /// <remarks>
            /// Binary behaviors are permanent drawings on the surface. Multiple
            /// behaviors are drawn in the order they are added.
            /// </remarks>
            /// <param name="behavior">The behavior that is used to change the frame area.</param>
            public void RemoveBehavior(IBaseBehavior behavior)
            {
                nativeElement.ElementBehaviors.RemoveBehavior(behavior);
            }

            /// <summary>
            /// Returns a well formatted representation of the frame content.
            /// </summary>
            /// <param name="fo">The Formatter Options used to format the code.</param>
            /// <returns>Returns a string of well formatted and XHTML compatible content.</returns>
            public string GetFormattedContent(IHtmlFormatterOptions fo)
            {
                string content = GetRawContent();
                if (fo.IndentSize < 0)
                {
                    throw new ArgumentException("Indentsize must be greater or equal than 0", "indentSize");
                }
                if (fo.MaxLineLength < 1)
                {
                    throw new ArgumentException("MaxLineLenght must be greater than 0", "maxLineLenght");
                }
                StringBuilder sb = new StringBuilder();
                StringWriter target = new StringWriter(sb);
                htmlFormatter.Format(content, target, fo);
                return sb.ToString();
            }

            /// <summary>
            /// Save the formatted content into the file the frame was loaded from.
            /// </summary>
            /// <remarks>
            /// Overwrites the content to the existing (underlying) file.
            /// </remarks>
            /// <param name="fo">The Formatter Options used to format the content.</param>
            public void SaveFormattedContent(IHtmlFormatterOptions fo)
            {
                string content = this.GetFormattedContent(fo);
                string url = this.GetFullPathUrl();
                StreamWriter sw = new StreamWriter(url, false, this.encoding);
                sw.Write(content);
                sw.Flush();
                sw.Close();                
            }

            /// <summary>
            /// Remove a previously set binary behavior.
            /// </summary>
            /// <remarks> See also <see cref="AddBehavior"/>.</remarks>
            /// <param name="behavior">The behavior object that has to be removed.</param>
            public void AddBehavior(IBaseBehavior behavior)
            {
                nativeElement.ElementBehaviors.AddBehavior(behavior);
            }

            /// <summary>
            /// Returns the <see cref="FrameElement"/> which represents the object of the frame.
            /// </summary>
            public IElement NativeFrameElement
            {
                get
                {
                    return this.nativeElement;
                }
            }

            #endregion
        }


        private HtmlEditor htmlEditor;
        private Interop.IHTMLDocument2 msHtmlDocument = null;
        private ArrayList FrameSets = null;
        private ArrayList FrameWindows = null;
        private IFrameWindow activeFrame;

        private bool _fchangepriority;
        private bool _fIsDesignerActive;

        internal HtmlFrameSet(IHtmlEditor editor, Interop.IHTMLDocument2 baseDocument)
        {
            htmlEditor = (HtmlEditor) editor;
            msHtmlDocument = baseDocument;
            _fchangepriority = false;
            _fIsDesignerActive = false;
        }

        # region Common (Public) Methods and Properties

        /// <summary>
        /// Returns <c>true</c>, if the frame was first time activated by user action.
        /// </summary>
        public bool IsFirstActivated
        {
            get
            {
                return this._fchangepriority;
            }
        }

        /// <summary>
        /// This public method creates a collection of all frames in the document, regardless in which frameset
        /// they are placed.
        /// </summary>
        /// <remarks>
        /// The collection contains <see cref="FrameWindow"/> objects. 
        /// </remarks>
        public void ActivateFrames()
        {
            if (_fIsDesignerActive) return;
            Interop.IOleContainer container = (Interop.IOleContainer) this.msHtmlDocument;            
            Interop.IEnumUnknown fEnum;
            container.EnumObjects ((int) Interop.OLECONTF.OLECONTF_EMBEDDINGS, out fEnum);
            if (FrameSets == null)
                FrameSets = new ArrayList();
            if (FrameWindows == null)
                FrameWindows = new ArrayList();
            FrameWindows.Clear();
            FrameSets.Clear();            
            if (fEnum != null)
            {
                MSHTMLSite ms = this.htmlEditor.MshtmlSite;
                Interop.IUnknown Unknown;
                uint Dummy;
                do
                {
                    fEnum.Next(1, out Unknown, out Dummy);
                    if (Unknown != null)
                    {
                        Interop.IHTMLElement frame = (Interop.IHTMLElement)Unknown;
                        Interop.IHTMLWindow2 window = ((Interop.IHTMLFrameBase2)frame).contentWindow;
                        // The document can be ether a real body or an additinal frameset document
                        if (window.document.GetBody() is Interop.IHtmlBodyElement)
                        {
                            FrameWindow fw = new FrameWindow(window, ms, this.htmlEditor);
                            fw.FrameActivated += new FrameActivatedEventHandler(fw_FrameActivated);
                            fw.FrameDeActivated += new FrameActivatedEventHandler(fw_FrameDeActivated);
                            // set the global encoding as default
                            fw.Encoding = this.htmlEditor.Encoding;
                            // add frame to collection
                            FrameWindows.Add(fw);
                            //break;
                        }
                        else
                        {
                            // assuming that a document which is has no body is a frameset
                            // recursively build all nested framesets in sub documents
                            FrameSets.Add(new HtmlFrameSet(this.htmlEditor, (Interop.IHTMLDocument2)window.document));
                            //break;
                        } // end switch
                    }
                } while (Unknown != null);
            } // end if
            _fIsDesignerActive = true;
        }

        void fw_FrameDeActivated(object sender, FrameEventArgs e)
        {
            SetFrameDeActivated(e);
        }

        void fw_FrameActivated(object sender, FrameEventArgs e)
        {
            SetFrameActivated(e);
        }

        private Guid IHtmlEditServicesGuid = new Guid("3050f663-98b5-11cf-bb82-00aa00bdce0b");
        private Guid SHtmlEditServicesGuid = new Guid("3050f7f9-98b5-11cf-bb82-00aa00bdce0b");

        private void SetEditDesigner(MSHTMLSite ms, Interop.IHTMLDocument2 ActiveDocument)
        {
            // prepare add designer methods
            Interop.IOleServiceProvider isp = (Interop.IOleServiceProvider) ActiveDocument;
            if (isp != null)
            {
                IntPtr ppv;                
                isp.QueryService(ref SHtmlEditServicesGuid, ref IHtmlEditServicesGuid, out ppv);
                Interop.IHTMLEditDesigner ds = (Interop.IHTMLEditDesigner) ms;      // Standard Designer
                Interop.IHTMLEditServices es = (Interop.IHTMLEditServices) Marshal.GetObjectForIUnknown(ppv);                    
                // TODO: Add Extender Provider
                Marshal.Release(ppv);
            }
        }

        /// <summary>
        /// Removes the attached frame behaviors and deactivate the frame editor.
        /// </summary>
        /// <remarks>
        /// Clears the frame collection. The document
        /// will still remains in memory and can be edited using standard editing features.
        /// </remarks>
        public void DeactivateFrames()
        {
            if (this.FrameWindows ==  null) return;
            foreach(FrameWindow fw in this.FrameWindows)
            {
                fw.DeactivateDesigner();
            }
            this.FrameWindows.Clear(); // let the finalizer do the dirty stuff
            this.FrameSets.Clear();
            this.FrameWindows = null;
            this.FrameSets = null;
            _fIsDesignerActive = false;
        }

        /// <summary>
        /// Gets the collection of FrameWindow objects.
        /// </summary>
        /// <remarks>
        /// Each entry represents one 
        /// frame in the current frameset. Return null if there are no frames.
        /// </remarks>
        public ICollection FrameCollection
        {
            get
            {
                return this.FrameWindows;
            }
        }

        /// <summary>
        /// Gets the collection of Framesets.
        /// </summary>
        /// <remarks>
        /// This list contains documents which are part
        /// of a framed document and contain a additional frame definition. Each document
        /// is represented by a HtmlFrameSet object which contains FrameWindow object and so on,
        /// in recursively manner.
        /// </remarks>
        public ICollection FrameSetCollection
        {
            get
            {
                return this.FrameSets;
            }
        }

        /// <summary>
        /// Returns the active frame windows object.
        /// </summary>
        /// <remarks>Set by the event fired from MSHTML Site.</remarks>
        /// <returns></returns>
        public IFrameWindow GetActiveFrame()
        {
            return activeFrame as IFrameWindow;
        }

        /// <summary>
        /// The number of frames of the current set. Frames in subsets are not counted.
        /// </summary>
        public int Count
        {
            get
            {
                return this.FrameCollection.Count;
            }
        }

        /// <summary>
        /// Returns the structure of the frame definition document as <see cref="XmlDocument"/> object.
        /// </summary>
        /// <remarks>
        /// This is not an interactive
        /// access to the underlying structure and therefore changes in the DOM of the returned object are not reflected in the
        /// document. To change the structure using XML it is recommended to read <see cref="XmlDocument"/> object, change the content
        /// and reload the document using the <see cref="HtmlEditor.LoadUrl"/> method.
        /// </remarks>
        /// <exception cref="System.Xml.XmlException">Access to formatter fails due to unexpected content.</exception>
        /// <returns>In XmlDocument object which contains the complete base document with the frame structure.</returns>
        public XmlDocument GetFrameStructure()
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                HtmlFormatterOptions fo = new HtmlFormatterOptions(' ', 1, 2048, true);
                string content = this.htmlEditor.GetFormattedHtml(fo);
                xDoc.LoadXml(content);
            }
            catch (Exception ex)
            {
                throw new XmlException("Access to formatter fails due to unexpected content:\n\n" + ex.Message);
            }
            return xDoc;
        }

        # endregion

        /// <summary>
        /// Fired if any of the frames in that set is beeing activated.
        /// </summary>
        public event FrameActivatedEventHandler FrameActivated;

        /// <summary>
        /// Fired if related from losses the focus (by getting a blur on window level).
        /// </summary>
        public event FrameActivatedEventHandler FrameDeActivated;

        private void SetFrameActivated(FrameEventArgs e)
        {
            this.activeFrame = e.FrameWindow;
            if (FrameActivated != null)
            {
                _fchangepriority = true;
                FrameActivated(this, e);
            }
            // Set active document so other classes like Selection can work with it
            htmlEditor.SetActiveFrameDocument(((FrameWindow) e.FrameWindow).GetInteropDocument());
        }

        private void SetFrameDeActivated(FrameEventArgs e)
        {
            if (FrameDeActivated != null)
            {
                _fchangepriority = false;
                FrameDeActivated(this, e);
            }
        }
    }
}
