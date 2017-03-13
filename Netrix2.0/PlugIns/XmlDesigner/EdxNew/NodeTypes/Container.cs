using System;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner.Edx
{

	/// <summary>
	/// Implements container node which wraps a sequence of other nodes.
	/// </summary>
	public class Container : EdxNode
	{

		private EdxNode edxnode;
		private bool bEmpty;
		
		/// <summary>
		/// Constructor for container class.
		/// </summary>
        /// <param name="editor"></param>
		/// <param name="p"></param>
		/// <param name="sTemp"></param>
		/// <param name="sPath"></param>
        /// <param name="sOptions"></param>
		/// <param name="index"></param>
		public Container(IHtmlEditor editor, EdxNode p, string sTemp, string sPath, string sOptions, int index ) : base(editor, p, sTemp, sPath, sOptions, index)
		{
			// init class vars
			this.bEmpty = true;
            this.edxnode = p;
		}

		/// <summary>
		/// Associate container with control.
		/// </summary>
		/// <param name="h"></param>
        /// <param name="backLinkNode"></param>
        public new void Associate(XmlNode h, XmlNode backLinkNode )
		{
            base.Associate(h, backLinkNode);
			this.root.WatchChanges( h, this );
        }

        # region Service Methods

        public override XmlNode XHTML(XmlNode oFrag)
        {
			// find our edit node
			XmlNode editnode = GetXmlNode();
			if( editnode == null )
			{
				Util.Err("containerXHTML: no XML node associated" );
				return null;
			}	
			// if we don't have a parent XML, create one
			if( oFrag == null )
				oFrag = Util.StringToXmlNode( "<tmp></tmp>" );
			
			// display ourselves
			XmlNodeList children = editnode.ChildNodes;
			if( children.Count == 0 )
			{
                throw new NotImplementedException("");
				// empty container-- see if an placeholder template is provided
				//XmlNode node = CreateNode( "#empty", 0 );
				// if so, add it
                //if( node != null )
                //{
                //    oFrag.AppendChild( node );
                //}
				//bEmpty = true;
			}
			else
			{
				for(int i = 0; i < children.Count; i++ )
				{
                    XmlNode newNode;
                    XmlNode child = children[i];
                    switch (child.NodeType)
                    {
                        case XmlNodeType.Element:
                            newNode = CreateNode(child, i);
                            break;
                        case XmlNodeType.Text:
                            newNode = CreateNode(editnode, i);
                            newNode.InnerXml = children[i].Value;
                            break;
                        default:
                            throw new Exception("Unknown Type in Container");
                    }
					// attach this node to the parent fragment node
                    oFrag.AppendChild(newNode);
				}
				bEmpty = false;
			}
			return oFrag;			
        }

		/// <summary>
		/// Create new node.
		/// </summary>
		/// <param name="parentNode"></param>
		/// <param name="index"></param>
		public XmlNode CreateNode(XmlNode parentNode, int index )
		{
            string tag = parentNode.Name;
			XmlNode node = GetTemplate().SelectSingleNode( "edx:match[@element = '" + tag + "']", EdxDocument.GetEdxDocument.XmlnsEdx );
			if( node == null )
			{
				if( tag != "#empty" )
				{
					Util.Err("Error: no match found for element " + tag + " in container template " + edxtemplate );
				}
				return null;
			}

			node = node.ChildNodes[0].CloneNode( true );
			
			// and fetch xhtml for this new child
			string sTemplate = Util.GetXmlAttribute( node, "edxtemplate" );
			string sOptions = Util.GetXmlAttribute( node, "edxoptions" );
			string sPath = Util.GetXmlAttribute( node, "edxpath" );
			if( sPath == null || sPath == "." )
				sPath = "";
			else
				sPath = "/" + sPath;
			EdxNode obj = Factory( editor, sTemplate, "node()[" + index + "]" + sPath, sOptions, index );
            obj.Associate(node, parentNode);

			// and in turn get its xhtml
			return obj.XHTML( node );
			//return node;
		}

		/// <summary>
		/// Can move up within the container.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public bool CanMoveUp(EdxNode  e )
		{
            // TODO
			return false;
		}

		/// <summary>
		/// Determines whether we can move down an element.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public bool CanMoveDown(EdxNode  e )
		{
            // TODO
			return false;
		}

        /// <summary>
        /// Move up an element within the container.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
		public bool MoveUp(EdxNode  e )
		{
            //string uid = ((Interop.IHTMLUniqueId)e.hobj).UniqueID;

            //Interop.IHTMLElementCollection children = hobj.GetChildren() as Interop.IHTMLElementCollection;
            //for (int i = 0; i < children.GetLength(); i++)
            //{
            //    Interop.IHTMLElement child = (Interop.IHTMLElement)children[i];
            //    if (((Interop.IHTMLUnique)child).UniqueID == uid)
            //    {
            //        if( i == 0 )
            //        {
            //            Util.Err("Error: moveUp called on first node" );
            //            return false;
            //        }
				
            //        // swap HTML first
            //        Interop.IHTMLDOMNode tmp = ((Interop.IHTMLDOMNode)children[i - 1]).swapNode(child);
				
            //        // do XML nodes
            //        XmlNode oXml = GetXmlNode();
            //        XmlManager xmlmgr = root.XmlManager;
            //        xmlmgr.OpenTransaction( oXml );
            //        xmlmgr.Process( "moveChildUp", oXml, i );
            //        xmlmgr.CloseTransaction();
				
            //        // finally swap edxnodes
            //        RemoveChild( e );
            //        InsertChild( e, i - 1 );
				
            //        // broadcast to anyone else interested
            //        root.AlertChange( oXml, this );
            //        return true;
            //    }
			//}

			Util.Err("Error: moveUp couldn't find " + e.GetXmlNode().Name + " in container collection." );// h
			return false;
		}

		/// <summary>
		/// Move down an element within the container.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
        public bool MoveDown(EdxNode  e )
		{
            //string uid = ((Interop.IHTMLUnique)e.hobj).UniqueID;

            //Interop.IHTMLElementCollection children = hobj.GetChildren() as Interop.IHTMLElementCollection;
            //for (int i = 0; i < children.GetLength(); i++)
            //{

            //    if (((Interop.IHTMLUnique)children[i]).UniqueID == uid)
            //    {
            //        if( i == children.Count - 1 )
            //        {
            //            Util.Err("Error: moveDown called on last node" );
            //        }
				
            //        // do HTML nodes first                              
            //        XmlControl tmp = ((XmlControl) children[i+1]).ElementDom.SwapNode( children[i] ) as XmlControl;
				
            //        // next swap XML
            //        XmlNode oXml = GetXmlNode();
            //        XmlManager xmlmgr = root.XmlManager;
            //        xmlmgr.OpenTransaction( oXml );
            //        xmlmgr.Process( "moveChildDown", oXml, i );
            //        xmlmgr.CloseTransaction();
				
            //        // finally do edxnodes
            //        RemoveChild( e );
            //        InsertChild( e, i + 1 );
				
            //        // let other interested parties know
            //        root.AlertChange( oXml, this );
            //        return true;
            //    }
            //}

            //Util.Err("Error: moveDown couldn't find " + h.tagName + " in container collection." );
			return false;
		}

        /// <summary>
        /// May do future schema checking or cardinality testing here but for now it's a green light.
        /// </summary>
        /// <param name="child"></param>
        public bool PermitChildSplit(EdxNode child)
        {
            return true;
        }

		/// <summary>
		/// Insert an new element in the container.
		/// </summary>
		/// <param name="sTemplate"></param>
		/// <param name="e"></param>
		public void Insert(string sTemplate, EdxNode e )
		{
            //View v = root.GetView();
            //XmlNode t = v.GetTemplate( sTemplate );
		
            //// get XML fragment to insert
            //XmlNode x = t.SelectSingleNode( "edx:insert" );
            //if( x == null )
            //{
            //    Util.Err("Error: couldn't load XML insertion fragment for template " + sTemplate );
            //    return;
            //}
		
            //// get position of element to insert after
            //string uid = e.hobj.UniqueID;
            //int i;
            //ElementCollection children = hobj.ElementDom.GetChildNodes();
            //if( children.Count > 0 )
            //{
            //    for(i = 0; i < children.Count; i++ )
            //    {
            //        if( ((XmlControl)children[i]).UniqueID == uid )
            //            break;
            //    }
            //    if( i == children.Count )
            //    {
            //        Util.Err("Error: couldn't find child to insert after in parent container" );
            //        return;
            //    }
            //}
            //else
            //{
            //    i = 0;
            //}
		
            //// insert the XML node
            //x = x.ChildNodes[0].CloneNode( true );
		
            //XmlNode editnode = GetXmlNode();
            //XmlManager xmlmgr = root.XmlManager;
            //xmlmgr.OpenTransaction( editnode );
            //xmlmgr.Process( "insertNode", editnode, x, i );
            //xmlmgr.CloseTransaction();
		
            //// insert seed node into HTML tree
            //string tag = x.Name;
            //XmlNode node = CreateNode( tag, i );

            //// insert the fragment
            //tag = hobj.TagName;
            //if( tag == "TBODY" )
            //{
            //    Util.InsertRowAt( (Interop.IHTMLTableBody) hobj.GetBaseElement(), node, i );
            //}
            //else if( tag == "TR" )
            //{
            //    Util.InsertCellAt( (Interop.IHTMLTableRow) hobj.GetBaseElement(), node, i );
            //}
            //else
            //{
            //    if( hobj.ElementDom.GetChildNodes().Count > 0 )
            //    {
            //        (((XmlControl)hobj).ElementDom.GetChildNodes()[i]).InsertAdjacentHTML( "beforeBegin", node.InnerXml );
            //    }
            //    else
            //    {
            //        hobj.InnerHtml = node.xml;
            //    }
            //}
		
            //// and do the association
            //((Container) childNodes[i]).Associate( hobj.childNodes[i] );
            //performAssociation( hobj.childNodes[i] );
		
            //// see if we need to clean up empty container placeholder
            //if( bEmpty && ChildNodes.Count == 2 )
            //{
            //    DeleteChild( ChildNodes[1] );
            //}
            //bEmpty = false;
		
            //// alert about changes
            //root.AlertChange( editnode, this );
		}

		public override bool CanDelete
		{
            get
            {
                // if we have children we're not going anywhere
                if (ChildNodes.Count != 0)
                    return false;

                // see what parent thinks
                if (!((Container)parent).PermitChildDelete()) // cast to IContainer/IRegion ?
                    return false;

                // guess we're good to go
                return true;
            }
		}

		/// <summary>
		/// May do future schema checking or cardinality testing here but for now it's a green light.
		/// </summary>
		public bool PermitChildDelete()
		{
			return true;
		}

		/// <summary>
		/// Deletes a child of this container.
		/// </summary>
		/// <param name="child"></param>
		public void DeleteChild(XmlNode child )
		{
			// find the child index

            int i = -1; // Util.ArrayIndex(ChildNodes, child);
			if( i == -1 )
			{
				Util.Err("containerDelete: child not found" );
				return;
			}

			// remove the HTML node
            //hobj.RemoveChild(ChildNodes[i]);
		
			// save child XML node
			XmlNode editnode = GetXmlNode();
			XmlNode nChild = editnode.ChildNodes[i];
		
			// and now have the edxnode take itself out
			((EdxNode)ChildNodes[i]).Cleanup();
		
			// remove the XML node (if it's not a placeholder which will have same node as parent)
			if( nChild != null && editnode != nChild )
			{
				XmlManager xmlmgr = root.XmlManager;
				xmlmgr.OpenTransaction( editnode );
				xmlmgr.Process( "deleteNode", editnode, i );
				xmlmgr.CloseTransaction();
			}

			// see if this leaves us empty (and we don't know that already)
			if( ChildNodes.Count == 0 )
			{
				// force a reload to pick up empty container placeholder if provided
				root.AlertChange( editnode, null );
			}
			else
			{
				// alert about changes
				root.AlertChange( editnode, this );
			}
		}


		/// <summary>Node has split.</summary>
		/// <remarks>
		///	Called when an immediate child of the container has split.  This will cause
		///	a reload of the spec'd node and a new node to be created immediately following
		///	for the new XML node.  Returns new child edxnode so caller can establish focus.
		/// </remarks>
		public EdxNode NodeHasSplit(EdxNode  child )
		{
			
            //int i;
            //for(i = 0; i < ChildNodes.Count; i++ )
            //{
            //    if( child == ChildNodes[i] )
            //        break;
            //}
            //if( i == ChildNodes.Count )
            //{
            //    Util.Err("nodeHasSplit: child not found" );
            //    return null;
            //}
		
            //// first reload the split child
            //child.Load();
		
            //// now pick up the new node immediately after
            //i++;
            //XmlNode editnode = GetXmlNode();
            //XmlNode newNode = editnode.ChildNodes[i];
            //string tag = newNode.Name;
            //XmlNode node = CreateNode( tag, i );
		
            //// insert the fragment
            //tag = hobj.TagName;
            //if( tag == "TBODY" || tag == "TR" )
            //{
            //    Util.InsertRowAt( hobj, node, i );
            //}
            //else
            //{
            //    string s = node.InnerXml;
            //    Regex tagSplit = new Regex(@">\s+<", RegexOptions.Compiled);
            //    s = tagSplit.Replace(s, "><" );
            //    ((XmlControl)hobj.ElementDom.GetChildNodes()[i-1]).InsertAdjacentHTML( "afterEnd", s );
            //}
		
            //// and do the association
            //((EdxNode)childNodes[i]).Associate( hobj.ElementDom.GetChildNodes()[i] as XmlControl);
            //PerformAssociation( hobj.ElementDom.GetChildNodes()[i] as XmlControl );
		
            //// we're good, return the new guy
            //return ((EdxNode)ChildNodes[i]);
            return null;
		}



		//
		//						_containerCleanup
		//
		public override void Cleanup()
		{
			XmlNode editnode = GetXmlNode();
			if( editnode != null )
			{
				root.UnwatchChanges( editnode, this );
			}
			edxnode.Cleanup();
		}

        # endregion

    }
}
