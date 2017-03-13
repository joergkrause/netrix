using System;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Collections.Generic;
using System.ComponentModel;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner.Edx
{

	/// <summary>
	/// Implements base class for EDX nodes.
	/// </summary>
    /// <remarks>
    /// Each EDX node class represents a section within the transform document. The nodes instruct the viewlink
    /// how to transform the XML into HTML and back. It also provides many options to manipulate the underlying document,
    /// for instance, adding elements or moving them around.
    /// </remarks>
	public abstract class EdxNode
	{
        /// <summary>
        /// Referenced editor (the one the element's native peer belongs to).
        /// </summary>
        protected IHtmlEditor editor;
        /// <summary>
        /// Root element
        /// </summary>
	    public Root root;
        /// <summary>
        /// Parent node or null if this is the parent.
        /// </summary>
		public EdxNode parent;		
        //private EdxNode next;
        //private EdxNode prev;
        ViewElement ve;
        /// <summary>
        /// Related native element
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ViewElement ViewElement
        {
            get { return ve; }
        }
        /// <summary>
        /// Associated element in the HTML/XML document the data comes from.
        /// </summary>
        protected XmlNode hobj;
		
        public string edxtemplate;
		private string edxpath;
        /// <summary>
        /// Original node in template
        /// </summary>
		private XmlNode oTemplate;
        /// <summary>
        /// Editable node in document fragement
        /// </summary>
		private XmlNode oEditNode;
        /// <summary>
        /// id
        /// </summary>
		public string id;
        private int index;
        /// <summary>
        /// Allow split
        /// </summary>
		public bool allowSplit;
        /// <summary>
        /// Link name
        /// </summary>
		public string displayLink;
        /// <summary>
        /// Link node
        /// </summary>
		public string nodeLink;
        /// <summary>
        /// Action required on enter.
        /// </summary>
		public string enterAction;
        /// <summary>
        /// type
        /// </summary>
        protected string type;
        private List<EdxNode> childNodes;
        /// <summary>
        /// EdxNode's children.
        /// </summary>
        public List<EdxNode> ChildNodes
        {
            get
            {
                return childNodes; // throw new NotImplementedException();
            }
        }

		/// <summary>
		/// Constructor for base edxnode. 
		/// Passed parent edxnode, template name, edxpath, and an optional index within the parent node list to insert at.
		/// </summary>
        /// <param name="editor"></param>
		/// <param name="p"></param>
		/// <param name="sTemplate"></param>
		/// <param name="sPath"></param>
		/// <param name="sOptions"></param>
		/// <param name="index"></param>
		public EdxNode(IHtmlEditor editor, EdxNode p, string sTemplate, string sPath, string sOptions, int index)
		{

            this.editor = editor;
            this.editor.ReadyStateComplete += new EventHandler(editor_ReadyStateComplete);
            if (sTemplate == null) throw new Exception("");	
			// capture tree info
			if( p == null )
			{
				// we must be root
				this.parent = null;
				this.root = this as Root;
			}
			else
			{
				this.parent = p;
				this.root = p.root;
                this.index = index;
				this.id = this.root.AssignID( this );
				if( index == -1 /* undefined */) 
					p.AppendChild( this );
				else
					p.InsertChild( this, index );
			}
			this.edxtemplate = sTemplate;
			this.edxpath = sPath;
			this.oTemplate = null;
			this.oEditNode = null;
            this.childNodes = new List<EdxNode>(); // TODO: EdxNode[] ?
			this.ProcessOptions( sOptions );
		}

        void editor_ReadyStateComplete(object sender, EventArgs e)
        {
            if (id != null && EdxDocument.GetEdxDocument.ViewLink != null) // probably nothing loaded
            {
                Interop.IHTMLDocument3 doc3 = (Interop.IHTMLDocument3)((Interop.IHTMLElement)EdxDocument.GetEdxDocument.ViewLink.DesignTimeElementView).GetDocument();
                backLinkNode = doc3.GetElementById(id);
                if (backLinkNode != null)
                {
                    ve = new ViewElement(backLinkNode, editor);
                    ve.Click += new GuruComponents.Netrix.Events.DocumentEventHandler(ve_Click);
                    ve.ControlSelect += new GuruComponents.Netrix.Events.DocumentEventHandler(ve_ControlSelect);
                    ve.PropertyChange += new GuruComponents.Netrix.Events.DocumentEventHandler(ve_PropertyChange);
                }
            }
        }

        void ve_PropertyChange(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            
        }

        void ve_ControlSelect(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            //e.SetReturnValue(false);
            //((Interop.IHTMLElement3)backLinkNode).setActive();
            //((Interop.IHTMLElement3)backLinkNode).isContentEditable;
        }

        void ve_Click(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            
        }

        private Interop.IHTMLElement backLinkNode;

        /// <summary>
        /// Native element node.
        /// </summary>
        [Browsable(false)]
        public Interop.IHTMLElement BackLinkNode
        {
            get 
            {
                return backLinkNode; 
            }
        }

        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public IElement BackLinkNode2
        //{
        //    get
        //    {
        //        IElement test = null;
        //        if (id != null)
        //        {
        //            test = editor.GetBodyElement().ElementDom.FirstChild;
        //        }
        //        return test;
        //    }
        //}

		/// <summary>
		/// Gets the node in the XML tree assoc'd with this node.
		/// </summary>
		/// <returns></returns>
		public XmlNode GetXmlNode()
		{
			if( oEditNode != null )
				return oEditNode;

            return hobj; //.SelectSingleNode(edxpath);

            //if (parent == null) 	// root node
            //{
            //    if (root != this)
            //    {
            //        // get root's node
            //        oEditNode = root.GetXmlNode().SelectSingleNode(edxpath); //EditRoot
            //    }
            //    else
            //    {
            //        // get root node finally
            //        oEditNode = ((Root)this).EditXml.SelectSingleNode("//edx:template[@name='"+ edxpath+ "']", EdxDocument.GetEdxDocument.XmlnsEdx);
            //    }
            //}
            //else
            //{
            //    XmlNode parentNode = parent.GetXmlNode();
            //    if (parentNode != null)
            //    {
            //        oEditNode = parent.GetXmlNode().SelectSingleNode(edxpath);
            //    }
            //    else
            //    {
            //        // root node reached
            //    }
            //}
		
			// if it's null, and it's spec'd as an attribute directly below current context
			// we'll create it on the fly.  this make life easier in field.applyTag() as we
			// don't have to use templates for stuff like link tags
            //if( oEditNode == null && edxpath[0] == '@' )
            //{
            //    // set to empty string
            //    Util.SetXmlAttribute( parent.GetXmlNode(), edxpath.Substring(1), "" );
            //}
		
            //return oEditNode;
		}

		/// <summary>
		/// Get the template associated with this node.
		/// </summary>
		/// <returns></returns>
		public XmlNode GetTemplate()
		{
			if( oTemplate != null )
				return oTemplate;
			View v = EdxDocument.GetEdxDocument.GetView();
			oTemplate = v.GetTemplate( edxtemplate );
			if( oTemplate == null )
				Util.Err("null template on " + GetType().Name);
			return oTemplate;
		}

		/// <summary>
		/// Called when we can attach ourselves to an HTML node in the HTML DOM.
		/// </summary>
		/// <param name="srcNode"></param>
        /// <param name="backLinkNode"></param>
		public virtual void Associate(XmlNode srcNode, XmlNode backLinkNode )
		{
            this.hobj = srcNode;
            System.Diagnostics.Debug.WriteLine(id, "Associate ID for Element" + backLinkNode.Name + " as " + this.GetType().Name);
            if (hobj is XmlAttribute)
            {
                Util.SetXmlAttribute(((XmlAttribute)hobj).OwnerElement, "id", id);
            }
            else
            {
                Util.SetXmlAttribute(hobj, "id", id);              
            }
			// set a couple things in the html
            Util.SetXmlAttribute(backLinkNode, "id", id);
            //Util.SetXmlAttribute(backLinkNode, "eobj", "-");
		}

		/// <summary>
		/// Process node's options.
		/// </summary>
		/// <param name="sOptions"></param>
        public void ProcessOptions(string sOptions )
		{
			// process edxoptions
			if( sOptions != null && sOptions.Length != 0 )
			{
				List<Option> a = Util.ParseOptions( sOptions );
						
				for(int i = 0; i < a.Count; i++ )
				{
					switch((a[i]).Name )
					{
						case "enter-action":
						switch( (a[i]).Value )
						{
							case "none":
							case "split":
							case "new":
								this.enterAction = a[i].Value;
								break;
							default:
								Util.Err("Error: invalid value for 'enter-action' option: " + this.enterAction + "\nMust be none, split, or new." );
								break;
						}
							break;
	
						case "allow-split":
						switch( ( a[i]).Value )
						{
							case "true":
							case "yes":
								this.allowSplit = true;
								break;
							case "false":
							case "no":
								this.allowSplit = false;
								break;
							default:
								Util.Err("Error: invalid option for 'allow-split' option: " + (a[i]).Value + "\nMust be true, yes, false, or no." );
								break;
						}
							break;
			
						case "node-link":
							// links a particular template to an index widget
							this.nodeLink = a[i].Value;
							break;
			
						case "display-link":
							// links the display selection for a region to a particular attribute node
							this.displayLink = a[i].Value;
							break;
				
						case "debug":
							if( a[i].Value == "true" )
							{
								// TODO: Dump
							}
							break;
				
						default:
							Util.Err("Error: unrecognized edxoption: " + a[i].Name);			
							break;
					}
				}
			}
		}

		/// <summary>
		/// Maintain a list of all edxnode children in order.
		/// </summary>
		/// <param name="node"></param>
		public void AppendChild(EdxNode node )
		{
			this.InsertChild( node, this.ChildNodes.Count );
		}

		/// <summary>
		/// Removes the specific child from the child list.
		/// </summary>
		/// <param name="node"></param>
		public EdxNode RemoveChild(EdxNode node )
		{
				
			for(int i = 0; i < ChildNodes.Count; i++ )
			{
				if( ChildNodes[i] == node )
				{
					if( node.PreviousSibling != null )
						node.PreviousSibling.NextSibling = node.NextSibling;
					if( node.NextSibling != null )
						node.NextSibling.PreviousSibling = node.PreviousSibling;
					//ChildNodes.splice( i, 1 );
					return node;
				}
			}
			Util.Err("Child node not found in childNode list" );
			return null;
		}

		/// <summary>
		/// Inserts new child at spec'd index.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="index"></param>
		public void InsertChild(EdxNode node, int index )
		{
			// sanity check
			if( index > ChildNodes.Count )
			{
				Util.Err("insertChild: can't insert past end of array." );
				return;
			}
		
			// resolve forw/back pointers
			if( index != 0 )
			{
				((EdxNode) ChildNodes[index-1]).NextSibling = node;
				node.PreviousSibling = ChildNodes[index-1] as EdxNode;
			}
			else
				node.PreviousSibling = null;
		
			if( index < ChildNodes.Count )
			{
				((EdxNode) ChildNodes[index]).PreviousSibling = node;
				node.NextSibling = ChildNodes[index] as EdxNode;
			}
			else
				node.NextSibling = null;
		
			// insert into array
			ChildNodes.Insert( index, node );
		}

        /// <summary>
        /// Next sibling
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public EdxNode NextSibling
		{
			get
			{
                if (parent != null && parent.ChildNodes.Count < index)
                {
                    return parent.ChildNodes[index+1];
                }
                return null;
			}
            set
            {
            }
		}

        /// <summary>
        /// Previous sibling.
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
		public EdxNode PreviousSibling
		{
			get
			{
                if (parent != null && index > 0 && parent.ChildNodes.Count <= index)
                {
                    return parent.ChildNodes[index - 1];
                }
                return null;
			}
            set
            {
            }
		}

		/// <summary>
		/// Returns true if the node is a child of the spec'd parent.
		/// </summary>
		/// <param name="par"></param>
		public bool IsChildOf(EdxNode par )
		{
			EdxNode e = this;
			while( e != null && e != par )
				e = e.parent;
			return e == par;
		}

		/// <summary>
		/// Determines whether or not we can split the node.
		/// </summary>
        public bool CanSplit
		{
            get
            {
                // containers and field default to splittable, all else not
                bool bRet = (this is Container) || (this is Field);

                // look for explicit option for this node
                //			if( this.allowSplit != null )
                bRet = allowSplit;

                // if we already know we can't split, look no further
                if (!bRet)
                    return false;

                // check against parent to see if it works
                switch (parent.GetType().Name)
                {
                    case "Region":
                    case "Root":
                        bRet &= parent.CanSplit;
                        break;
                    case "Container":
                        bRet &= ((Container)parent).PermitChildSplit(this);
                        break;
                    default:
                        Util.Err("unexpected parent container for " + GetType().Name + ": " + parent.GetType().Name);
                        bRet = false;
                        break;
                }
                return bRet;
            }
		}

		/// <summary>
        /// Attempts to split the node at the spec'd index.
		/// </summary>
        /// <remarks>However if the last Node Split our node, we move up the tree.</remarks>
		/// <param name="index"></param>
		/// <param name="lastNodeSplit"></param>
		/// <returns></returns>
        public EdxNode SplitNode(int index, XmlNode lastNodeSplit )
		{
			// we should have been asked before being called
			if( !CanSplit )
			{
				Util.Err("splitNode called on non-splittable node" );
				return null;
			}
			
			// see if we're attached to same XML node as caller
			XmlNode editnode = GetXmlNode();
			if( editnode == lastNodeSplit )
			{
				// we're a wrapper of some sort, see if we can just move on up
				if( parent.CanSplit )
					return parent.SplitNode( index, lastNodeSplit );
				else
					return this;
			}
		
			// do a split (there should already be an open transaction
			XmlManager xmlmgr = root.XmlManager;
			xmlmgr.Process( "splitNode", editnode, index );
			XmlNode newNode = editnode.NextSibling;
		
			// find the index of where we split
				
			XmlNode par = editnode.ParentNode;
			int i;
			for(i = 0; i < par.ChildNodes.Count; i++ )
			{
				if( par.ChildNodes[i] == newNode )
					break;
			}
			if( i == par.ChildNodes.Count )
			{
				Util.Err("splitNode couldn't find next sibling" );
				return null;
			}
		
			// ascend the tree looking for parent that finally blocks the split
			if( parent.CanSplit )
				return parent.SplitNode( i, editnode );
			else
				return this;
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool CanDelete
		{
			// if they haven't implemented this, they can't be deleted
            get { return false; }
		}

		/// <summary>
		/// Called when a node is being destroyed.
		/// </summary>
		public virtual void Cleanup()
		{
			while( ChildNodes.Count != 0 )
				((EdxNode)ChildNodes[0]).Cleanup();
		
			root.DeassignID( id );
			if( parent != null )
				parent.RemoveChild( this );
		}

        /// <summary>
        /// Returns the XHTML this node represents.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public abstract XmlNode XHTML(XmlNode child);

		/// <summary>
		/// Manufactures edxnodes from a template name.
		/// </summary>
        /// <param name="editor"></param>
		/// <param name="sTemplate"></param>
		/// <param name="sPath"></param>
		/// <param name="sOptions"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public EdxNode Factory(IHtmlEditor editor, string sTemplate, string sPath, string sOptions, int index )
		{
			if( sTemplate == null )
			{
				Util.Err("Missing edxtemplate: all EDX elements must specify a edxtemplate value." );
				return null;
			}

			// check for built-ins
			if( sTemplate.IndexOf(":") > 0 )
			{
				// built-ins
				if( sTemplate.Substring(0,6) == "field:" )
				{
					return new Field(editor, this, sTemplate, sPath, sOptions, index );
				}
				else if( sTemplate.Substring(0,7) == "widget:" )
				{
                    return new Widget(editor, this, sTemplate, sPath, sOptions, index);
				}
				else
				{
					Util.Err("Only built-in template names may contain ':': " + sTemplate );
					return null;
				}
			}
			else
			{
				// standard template spec'd in view doc
                View v = EdxDocument.GetEdxDocument.GetView();
				XmlNode oTemp = v.GetTemplate( sTemplate );

				// discover our type from the template
				TemplateType type = v.GetTemplateType( oTemp );
				if( type == TemplateType.Region )
				{
                    return new Region(editor, this, sTemplate, sPath, sOptions, index);
				}
				else if( type == TemplateType.Container )
				{
                    return new Container(editor, this, sTemplate, sPath, sOptions, index);
				}
				else
				{
					Util.Err("Unrecognized template type: " + type );
					return null;
				}
			}
		}

        /// <summary>
        /// Readable name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} [{1}]", hobj.Name, GetType().Name);
        }

	}
}
