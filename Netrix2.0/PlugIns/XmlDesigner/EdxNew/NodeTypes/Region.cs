using System;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Drawing;

namespace GuruComponents.Netrix.XmlDesigner.Edx
{

	/// <summary>
	/// Implements a region node which maps to a block of displayed HTML.
	/// </summary>
	public class Region : EdxNode
	{

		Color saveBackground = Color.Empty;
		string viewState = null;
		XmlNode displayLinkNode = null;

		/// <summary>
		/// Constructor for region class.
		/// </summary>
        /// <param name="editor"></param>
		/// <param name="p"></param>
		/// <param name="sTemp"></param>
		/// <param name="sPath"></param>
		/// <param name="sOptions"></param>
		/// <param name="index"></param>
		public Region(IHtmlEditor editor, EdxNode p, string sTemp, string sPath, string sOptions, int index ) : base(editor, p, sTemp, sPath, sOptions, index )
		{
            base.parent = p;
			// init class vars
			this.saveBackground = Color.Empty;
			this.viewState = null;
			this.displayLinkNode = null;
		}

        /// <summary>
        /// Creates region's XHTML.
        /// </summary>
        /// <param name="oFrag"></param>
        /// <returns></returns>
		public override XmlNode XHTML(XmlNode oFrag )
		{
			XmlNode oHtml;
	
			View v = EdxDocument.GetEdxDocument.GetView();

			// find our edit node
			XmlNode editnode = GetXmlNode();

			// see if we're display-linked to an attribute
			if( this.displayLink != null )
			{
				XmlNode n = editnode.SelectSingleNode( "@" + displayLink );
				if( n != null )
				{
					viewState = n.Value;
					displayLinkNode = n;
					root.WatchChanges( n, this );
				}
			}
            XmlNode templateNode = GetTemplate();
            // reached root
            if (templateNode == null) return null;
            // 
            if (viewState == null)
            {                
                if (templateNode != null)
                {
                    viewState = v.GetTemplateDefaultDisplayName(templateNode);
                }
            }
            oHtml = v.GetTemplateHtmlByName(templateNode, viewState);
			if( oHtml == null )
			{
				Util.Err("Error: no HTML template for " + viewState );
				return null;
			}

			// clone the template node
			XmlNode oXml = oHtml.CloneNode( true );

			// find the children
			XmlNodeList children = oXml.SelectNodes( "//*[@edxtemplate != '']" );
			// loop through all children which represent EdxNodes	
			foreach (XmlNode child in children)
			{
				string sTemplate = Util.GetXmlAttribute( child, "edxtemplate" );
				string sPath = Util.GetXmlAttribute( child, "edxpath" );
				string sOptions = Util.GetXmlAttribute( child, "edxoptions" );
                EdxNode obj = null;
                // look for multiple elements
                if (sPath == null)
                {
                    // probably a widget
                    obj = Factory(editor, sTemplate, sPath, sOptions, -1);
                    // associate the edx with the working xml data object                
                    obj.Associate(hobj, child);
                    obj.XHTML(child);
                }
                else
                {
                    XmlNodeList multiNodes = hobj.SelectNodes(sPath, EdxDocument.GetEdxDocument.XmlnsFrag);
                    foreach (XmlNode hobjSub in multiNodes)
                    {
                        obj = Factory(editor, sTemplate, sPath, sOptions, -1);
                        // associate the edx with the working xml data object                
                        obj.Associate(hobjSub, child);
                        obj.XHTML(child);
                    }
                }
			}
		
			// if parent provided, append children
            if (oFrag != null && oXml != null && oFrag.OwnerDocument != null)
            {
                for (int i = 0; i < oXml.ChildNodes.Count; i++)
                {
                    XmlNode oTmp = oXml.ChildNodes[i].CloneNode(true);
                    oFrag.AppendChild(oTmp);
                }
            }
			
			// done, return the XML fragment
			return oXml;
		}

		/// <summary>
        /// Selects the current region, deselecting any previously selected region.
		/// </summary>
        public void Select()
		{
			// do toggle if previously selected was us
			if( root.selectedRegion == this )
			{
				Unselect();
				return;
			}
		    // TODO: Add behavior to mark selection
		}

		/// <summary>
        /// Deselects the current region, restoring previous display attribs.
		/// </summary>
        public void Unselect()
		{
			// TODO: Remove behavior from all selections
			root.selectedRegion = null;
		}

		/// <summary>
		/// Determines whether the region can be deleted.
		/// </summary>
		/// <returns></returns>
		public override bool CanDelete
		{
            get
            {
                // non-splittable regions are permanent
                if (!allowSplit)
                    return false;

                // see what parent thinks
                if (!((Region)parent).PermitChildDelete())
                    return false;

                // guess we're good to go
                return true;
            }
		}

		/// <summary>
		/// Get perimssion to delete children.
		/// </summary>
		/// <returns></returns>
        public bool PermitChildDelete()
		{
			if( !this.allowSplit )
				return false;
			return true;
		}

		/// <summary>
        /// Called when the node is destoyed.
		/// </summary>
        public override void Cleanup()
		{
			if( this == this.root.selectedRegion )
			{
				this.root.selectedRegion = null;
			}
			if( this.displayLinkNode != null )
			{
				this.root.UnwatchChanges( this.displayLinkNode, this );
			}
			base.parent.Cleanup();
		}
	}
}
