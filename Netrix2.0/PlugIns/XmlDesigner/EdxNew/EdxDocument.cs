using System;
using System.Xml;
using System.Collections;

using GuruComponents.Netrix;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Collections.Generic;
using GuruComponents.Netrix.HtmlFormatting.Elements;
using System.Web.UI;
using System.IO;
using System.ComponentModel;
using GuruComponents.Netrix.HtmlFormatting;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner.Edx
{

	/// <summary>
	/// Represents the template document and xml fragment we're working on.
	/// </summary>
	public class EdxDocument
	{

        private Root eobj;		// ref to edxnode class object

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Root Root
        {
            get { return eobj; }
        }
		private string rootid;		// root ID
		private string edxid;		// body ID ?
		private IHtmlEditor editor;
        private XmlDocument xEdxDoc = new XmlDocument();  // Transform docment 
        private XmlElementDesigner assiocatedDesigner;
        
        [Browsable(false)]
        public ViewLink ViewLink
        {
            get { return assiocatedDesigner.AssociatedViewLink; }
        }

        public XmlDocument XmlTemplateDoc
        {
            get { return xEdxDoc; }
        }
        private XmlDocument xFrag = new XmlDocument(); // Fragment loaded in editor 

        public XmlDocument XmlDocument
        {
            get { return xFrag; }
        }
        private XmlNamespaceManager xmlnsFrag;

        public XmlNamespaceManager XmlnsFrag
        {
            get { return xmlnsFrag; }
        }
        private XmlNamespaceManager xmlnsEdx;

        public XmlNamespaceManager XmlnsEdx
        {
            get { return xmlnsEdx; }
        }

        private XmlControl nativeRootControl;

        /// <summary>
        /// Root element in xFrag
        /// </summary>
        private XmlNode rootElement;

        private View edxView;
        private XmlParserContext xhtmlContext;
        private XmlReaderSettings xs;
        private HtmlFormatter formatter;
        private HtmlFormatterOptions fo;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="component">Associated editor</param>
		private EdxDocument(XmlControl component)
		{
			this.editor = component.HtmlEditor;
            this.nativeRootControl = component;
            //prepare the validating reader for accepting HTML characters 
            xhtmlContext = Util.CreateXhtmlContext();
            xs = new XmlReaderSettings();
            xs.ProhibitDtd = false;
            xs.ValidationType = ValidationType.None;
            xs.XmlResolver = Util.CreateXhtmlResolver();
            // Namespace resolver
            xmlnsEdx = Util.CreateEdxNamespaceManager();
            fo = new HtmlFormatterOptions(' ', 1, 1024, true);
            formatter = new HtmlFormatter();
        }

        internal static void Initialize(XmlControl component)
        {
            edxDocInstance = new EdxDocument(component);
        }

        private static EdxDocument edxDocInstance;
        public static EdxDocument GetEdxDocument
        {
            get 
            {
                if (edxDocInstance == null)
                {
                    throw new ApplicationException("EdxDocument not yet instantiated");
                }
                return edxDocInstance;
            }
        }


		public IHtmlEditor Editor
		{
			get
			{
				return editor;
			}
		}

		/// <summary>
		/// Initialize edx document. Assuming the editor has already loaded the document we edit.
		/// </summary>
		/// <param name="documentFragmentXml">XML document</param>
        /// <param name="edxXml"></param>
        public void DoInit(string documentFragmentXml, XmlDocument edxXml)
		{
            // assure xhtml compliance
            StringWriter sw = new StringWriter();
            formatter.Format(documentFragmentXml, sw, fo);

            XmlReader xr = XmlReader.Create(new StringReader(sw.ToString()), xs, xhtmlContext);
            // load document fragment
            xFrag.Load(xr);

            xmlnsFrag = new XmlNamespaceManager(xFrag.NameTable);
            foreach (DictionaryEntry de in editor.GetRegisteredNamespaces())
            {
                xmlnsFrag.AddNamespace(de.Key.ToString(), de.Value.ToString());
            }
            xEdxDoc = edxXml;
            // Avoid getting the Doctype node as first one
            rootElement = xFrag.FirstChild;
            // first element from fragment    
            
            while (!(rootElement is XmlElement))
            {
                rootElement = rootElement.NextSibling;
            }
            // create view (only one supported at the moment)
            XmlNode viewRoot = xEdxDoc.SelectSingleNode("//editviews");
            if (viewRoot == null)
            {
                throw new ApplicationException("EDX Template does not contain a views collection");
            }
            edxView = new View(viewRoot, this);
		
		}

        private Interop.IHTMLDocument3 doc3;

        public string Update()
        {

            doc3 = (Interop.IHTMLDocument3)((Interop.IHTMLElement)ViewLink.DesignTimeElementView).GetDocument();

            XmlNodeList edxNodes = xFrag.SelectNodes("//*[@id != '']", XmlnsFrag);
            object[] pVars = new object[1] { null };
            foreach (XmlNode edxNode in edxNodes)
            {
                XmlAttribute idAttribute = edxNode.Attributes["id"];
                string id = idAttribute.Value;
                edxNode.Attributes.Remove(idAttribute);
                Interop.IHTMLElement viewElement = doc3.GetElementById(id);
                if (viewElement != null)
                {
                    string s = viewElement.GetInnerHTML();

                    string Debug = viewElement.GetOuterHTML();

                    viewElement.GetAttribute("edxpath", 0, pVars);
                    string sPath = pVars[0].ToString();
                    if (String.IsNullOrEmpty(sPath)) continue;
                    // now rertieve the path to set to proper position
                    XmlNode xNode = edxNode.SelectSingleNode(sPath, XmlnsFrag);
                    if (xNode == null)
                    {
                        continue;
                    }
                    switch (xNode.NodeType)
                    {
                        case XmlNodeType.Attribute:
                            xNode.Value = s;
                            break;
                        case XmlNodeType.Element:
                            if (String.IsNullOrEmpty(s))
                            {
                                edxNode.ParentNode.RemoveChild(edxNode);
                            }
                            else
                            {
                                try
                                {
                                    xNode.InnerXml = s; // String.IsNullOrEmpty(s) ? "" : s; 
                                }
                                catch (Exception ex)
                                {
                                    xNode.InnerXml = ex.Message;
                                }
                            }
                            break;
                    }
                }
            }
            return XmlDocument.OuterXml;
        }

        /// <summary>
        /// Processes the node and creates design time html.
        /// </summary>
        public string Process(XmlElementDesigner assiocatedDesigner)
        {
            // establish the backlinks
            this.assiocatedDesigner = assiocatedDesigner;

            // Begin Transforming the root node
            XmlNode rootNode = edxView.GetTemplate("root");
            XmlNode rootXhtml = edxView.GetTemplateHtmlByName(rootNode, null);

            //edxView.CompileContainerMap("root");
            // Get first region 
            XmlNode child = rootXhtml.FirstChild;
            if (child == null)
                return "";
            this.eobj = new Root(editor, "root", "root");
            this.eobj.EditXml = xEdxDoc;
            // associate with HTML since we're already here
            this.eobj.Associate(rootElement, child); //this.Editor.GetBodyElement());
            this.edxid = eobj.id;
            //rootid = this.id;
            rootid = this.edxid;

            string html = eobj.XHTML(xFrag).InnerXml;
            return html;
        }

		/// <summary>
		/// Monitor property changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void  Edx_OnPropertyChange(object sender, DocumentEventArgs e)
		{
			string propertyName = e.PropertyName;

			// Detach the onpropertychange event to prevent it from firing while the changes are handled.
			this.Editor.GetBodyElement().PropertyChange -= new GuruComponents.Netrix.Events.DocumentEventHandler(Edx_OnPropertyChange);

			//  Re-attach the onpropertychange event
			this.Editor.GetBodyElement().PropertyChange += new GuruComponents.Netrix.Events.DocumentEventHandler(Edx_OnPropertyChange);
		}

		/// <summary>
		/// Gets the node in the XML tree assoc'd with this node.
		/// </summary>
		public XmlNode GetXmlNode()
		{
			return eobj.GetXmlNode();
		}

        /// <summary>
        /// Returns view manager object.
        /// </summary>
        /// <returns></returns>
        public View GetView()
        {
            return this.edxView;
        }


		//
		//						enableIcons
		//
		public void EnableIcons( bool flg )
		{
			// TODO: Set Binary Behavior for elements
//			for(int i = 0; i < this.all.length; i++ )
//			{
//				var h = this.all(i);
//				try
//				{
//					if( h.edxtemplate != null && h.edxtemplate.substr(0,11) == "widget:icon" )
//					{
//						h.style.display = flg ? "block" : "none";
//					}
//				}
//				catch(Exception e) {}
//			}
		}

		/// <summary>
		/// Looks for selected item and checks to see if it can move up.
		/// </summary>
		/// <returns></returns>
		public bool CanMoveUp()
		{
			if( eobj.selectedRegion != null )
			{
				Region r = eobj.selectedRegion;
				if( r.parent == null )	// trap for root node
					return false;
				if( r.parent is Container )
				{
					bool b = ((Container) r.parent).CanMoveUp( r );
					return b;
				}
			}
			return false;
		}

		/// <summary>
		/// Looks for selected item and checks to see if it can move down.
		/// </summary>
		/// <returns></returns>
		public bool CanMoveDown()
		{
			if( eobj.selectedRegion != null )
			{
				Region r = eobj.selectedRegion;
				if( r.parent == null )	// trap for root node
					return false;
				if( r.parent is Container )
				{
					return ((Container) r.parent).CanMoveDown( r );
				}
			}
			return false;
		}

		/// <summary>
		/// Looks for selected item and checks to see if it can move up.
		/// </summary>
		public void  MoveUp()
		{
			if( eobj.selectedRegion != null )
			{
				Region r = eobj.selectedRegion;
				if( r.parent is Container )
				{
					((Container) r.parent).MoveUp( r );
				}
			}
		}

		/// <summary>
		/// Looks for selected item and checks to see if it can move down.
		/// </summary>
		public void  MoveDown()
		{
			if( eobj.selectedRegion != null )
			{
				Region r = eobj.selectedRegion;
				if( r.parent is Container )
				{
					((Container) r.parent).MoveDown( r );
				}
			}
		}

		/// <summary>
		/// Sees if we can apply spec'd tag to all regions of current field selection.
		/// </summary>
		/// <param name="sTag"></param>
		/// <returns></returns>
		public bool CanApplyTag(string sTag)
		{
			return eobj.CanApplyTag( sTag );
		}

		/// <summary>
		/// Wraps current field selection in spec'd tag.
		/// </summary>
		/// <param name="sTag"></param>
		public void  ApplyTag( string sTag )
		{
			eobj.ApplyTag( sTag );
		}

		public void  EdxCommand(string sCmd )
		{
			switch( sCmd )
			{
				case "Bold":
					Editor.TextFormatting.ToggleBold();
					goto case "Focus";
				case "Underline":
					Editor.TextFormatting.ToggleUnderline();
					goto case "Focus";
				case "Italic":
					Editor.TextFormatting.ToggleItalics();
					goto case "Focus";
				case "CreateLink":
					Editor.Document.InsertHyperlink();
					goto case "Focus";
				case "Focus":
					Field ff = eobj.focusField;
					if( ff != null && ff.fieldType == Field.FieldType.Rich )
					{
						//ff.hobj.focus();						
					}
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Whether undo can be applied.
		/// </summary>
		/// <returns></returns>
        public bool CanUndo()
		{
			return eobj.XmlManager.CanUndo();
		}

        /// <summary>
        /// Whether redo can be applied.
        /// </summary>
        /// <returns></returns>
        public bool CanRedo()
		{
			return eobj.XmlManager.CanRedo();
		}

        /// <summary>
        /// Perform undo operation.
        /// </summary>
	
        public void  Undo()
		{
			eobj.XmlManager.Undo();
		}

		/// <summary>
		/// Perform redo operation.
		/// </summary>
        public void  Redo()
		{
			eobj.XmlManager.Redo();
		}

        /// <summary>
        /// Clear undo/redo history list.
        /// </summary>
		public void  ClearHistory()
		{
			eobj.XmlManager.ClearHistory();
		}

	}
}