using System;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using System.Web.UI.WebControls;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Collections.Generic;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner.Edx
{

	/// <summary>
	/// Class for root EDX node.
	/// </summary>
	public class Root : Region
	{

		// public "property"
		public Region selectedRegion ;
		public int? selectedIndex ;
		public Field focusField ;
		public object popupOwner ;

        private XmlDocument oEditXml;

        public XmlDocument EditXml
        {
            get { return oEditXml; }
            set { oEditXml = value; }
        }
        private string sBasePath;	// TODO: Prop

        public string BasePath
        {
            get { return sBasePath; }
            set { sBasePath = value; }
        }

		private int nextID = 0;
		private Dictionary<XmlNode, List<EdxNode>> dChanges;
		private Dictionary<string, EdxNode> dIDs;
		private XmlManager oXmlMgr;

		/// <summary>
		/// Manage field selection (Type Selection)
		/// </summary>
		public List<Selection> aFieldSelection = null;

        public Root(IHtmlEditor editor, string sTemp, string sPath)
            : base(editor, null, sTemp, sPath, null, -1)
		{

			oXmlMgr = new XmlManager(this);

			// public "property"
			this.selectedRegion = null;
			this.selectedIndex = -1;
			this.focusField = null;
			this.popupOwner = null;
		
			// doc mgmt vars
			this.oEditXml = null;
			this.nextID = 0;
            this.dChanges = new Dictionary<XmlNode, List<EdxNode>>();
			this.dIDs = new Dictionary<string,EdxNode>();
			this.oXmlMgr = new XmlManager( this );

			// manage field selection
			this.aFieldSelection = null;

			// catch global events
            //IElement body = editor.GetBodyElement();
            //body.SelectionChange += new GuruComponents.Netrix.Events.DocumentEventHandler(Root_SelectionChange);
	
			// assign ourselves our ID
			this.id = this.AssignID( this );
		}

        void Root_SelectionChange(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
        {
            // TODO: old code
            //rootOnSelectionChange();
        }

		# region Public Properties

		public XmlManager XmlManager
		{
			get
			{
				return oXmlMgr;
			}
		}

		public Root EditRoot
		{
			get
			{
				return this;
			}
		}


		# endregion

		/// <summary>
        /// Assigns a unique edxid and places in a hash for quick lookup.  
		/// </summary>
		/// <param name="e"></param>
        /// <returns>Returns ID value.</returns>
        public string AssignID( EdxNode e )
		{
			string sID = "e" + this.nextID++;
			this.dIDs.Add( sID, e );
			return sID;
		}

		/// <summary>
		/// Unassign the ID.
		/// </summary>
		/// <param name="sID"></param>
        public void DeassignID(string sID )
		{
			if( this.dIDs.ContainsKey( sID ) )
			{
				this.dIDs.Remove( sID );
			}
			else
			{
				Util.Err("deassignID: " + sID + " not found" );
			}
		}

		/// <summary>
		/// Returns the edxnode associated with the specific ID.
		/// </summary>
		/// <param name="sID"></param>
		/// <returns></returns>
		public EdxNode LookupID(string sID )
		{
			if( this.dIDs.ContainsKey( sID ) )
			{
				return this.dIDs[ sID ];
			}
			else
			{
				Util.Err("lookupID: " + sID + " not found" );
                throw new ApplicationException("lookupID: " + sID + " not found");
			}
		}

		/// <summary>
		/// Register a particular edx node to monitor changes on a certain XML DOM node.
		/// </summary>
		/// <param name="oXmlNode"></param>
		/// <param name="oEdx"></param>
		public void WatchChanges(XmlNode oXmlNode, EdxNode oEdx )
		{
			List<EdxNode> a;
	
			if( dChanges.ContainsKey( oXmlNode ) )
			{
				a = dChanges[oXmlNode];
				a.Add(oEdx);
			}
			else
			{
				a = new List<EdxNode>();
				a.Add(oEdx);
				dChanges.Add( oXmlNode, a );
			}
		}

		/// <summary>
		/// Un-register a particular edx node from monitoring changes on a certain XML DOM node.
		/// </summary>
		/// <param name="oXmlNode"></param>
		/// <param name="oEdx"></param>
		public void UnwatchChanges(XmlNode oXmlNode, EdxNode oEdx )
		{
			if( dChanges.ContainsKey( oXmlNode ) )
			{
				List<EdxNode> a = dChanges[oXmlNode];
					
				for(int i = 0; i < a.Count; i++ )
				{
					if( a[i] == oEdx )
					{
						// remove this one
						a.RemoveAt(i);
						return;
					}
				}
			}
			Util.Err("Error: unwatchChanges couldn't find object to de-register" );
		}

		/// <summary>
		/// Alert changes.
		/// </summary>
		/// <remarks>
		///	Notify any listeners of a change on this node.  Interesting little two step here.
		///	Some listeners may vanish from notify list as we issue the alerts.  Others may
		///	arrive.  We do not want to not send to any vanished listeners nor do we want to
		///	send to new arrivals, only the original and still valid.
		/// </remarks>
		public void AlertChange(XmlNode oXmlNode, EdxNode oEdx )
		{
            //if( dChanges.ContainsKey(oXmlNode))
            //{
            //    // get current notify list
            //    List<EdxNode> a = dChanges[oXmlNode];
			
            //    // snapshot it
            //    IList aOrig = Util.ArrayCopy( a );
			
            //    // walk the snapshot notifying					
            //    for(int i = 0; i < aOrig.Count; i++ )
            //    {
            //        Widget e = aOrig[i] as Widget; // right cast?

            //        // see if it's still on the list
            //        if( Util.ArrayIndex( a, e ) != -1 )
            //        {
            //            // looks valid, do it
            //            e.OnXmlNodeChange( oEdx );
            //        }
            //    }
            //}
		}

        ///// <summary>
        ///// Disable Fieldcursor
        ///// </summary>
        //public void FieldCursor()
        //{
        //    FieldCursor(-1, 0, 0);
        //}

        ///// <summary>
        ///// Positive values enable the "fake" cursor.  A negative X value disables it.
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <param name="h"></param>
        //public void FieldCursor(int x, int y, int h )
        //{
        //    if( x < 0 )
        //    {
        //        if( cursorDiv != null )
        //            cursorDiv.GetStyle().SetDisplay("none");
        //        if( cursorTimeout != -1 )
        //        {
        //            //window.clearTimeout( cursorTimeout );
        //            cursorTimeout = -1;
        //        }
        //        return;
        //    }
		
        //    // make sure we've made the cursor
        //    if( cursorDiv == null )
        //    {
        //        cursorDiv = editor.CreateElement( "DIV" ).GetBaseElement();
        //        cursorDiv.GetStyle().SetAttribute("position", "absolute", 0);
        //        cursorDiv.GetStyle().SetBackgroundColor("black");
        //        cursorDiv.GetStyle().SetWidth("1px");
        //        cursorDiv.GetStyle().SetZIndex("10000");
        //        ((Interop.IHTMLDOMNode)hobj).appendChild( cursorDiv as Interop.IHTMLDOMNode );
        //    }
		
        //    // position it and show it
        //    Interop.IHTMLRect r = ((Interop.IHTMLElement2)hobj).GetBoundingClientRect();
        //    cursorDiv.GetStyle().SetLeft(Unit.Pixel(x - r.left).ToString());
        //    cursorDiv.GetStyle().SetTop(Unit.Pixel(y - r.top).ToString());
        //    cursorDiv.GetStyle().SetHeight(Unit.Pixel(h).ToString());
        //    cursorDiv.GetStyle().SetDisplay("block");
        //    if( cursorTimeout == -1 )
        //    {
        //        ToggleCursor();
        //    }
        //}

        ////
        ////						_rootToggleCursor
        ////
        //public void ToggleCursor()
        //{
        //    if( cursorDiv.GetStyle().GetBackgroundColor().ToString() == "black" )
        //        cursorDiv.GetStyle().SetBackgroundColor("white");
        //    else
        //        cursorDiv.GetStyle().SetBackgroundColor("black");
        //    string cmd = hobj.ID + ".eobj.toggleCursor()";
        //    //TODO: Wait 300 ms and issue command
        //    //cursorTimeout = window.setTimeout( cmd, 300 );
        //}

//		/// <summary>
//		/// Catch global selection changes. Has context in the parent behavior.
//		/// </summary>
//		public void OnSelectionChange()
//		{
//			this.eobj.onSelectionChange();
//		}

		/// <summary>
		/// Catch global selection changes. Has context in the parent behavior.
		/// </summary>
		public void OnSelectionChange()
		{
			Interop.IHTMLSelectionObject sel = ((Interop.IHTMLDocument2) editor.GetActiveDocument(false)).GetSelection();
		
			// see if non-text selection
			if( sel.GetSelectionType() != "Text" )
			{
				ClearFieldSelection();
				return;
			}
		
			// get the edx granddaddy of this selection
			Interop.IHTMLTxtRange tr = sel.CreateRange() as Interop.IHTMLTxtRange;
			Interop.IHTMLElement par = tr.ParentElement();
			while( par != null && Util.GetEdxFromElement(par) != null)
				par = par.GetParentElement();
			if( par == null )
			{
				// extends outside ourselves, ignore it
				ClearFieldSelection();
				return;
			}
		
			// attempt to build list of selected or partially selected fields
			List<Selection> a = new List<Selection>();
            EdxNode epar = Util.GetEdxFromElement(par);
			EdxNode er = (epar is Field) ? epar : Util.TraverseRight( epar, false );
			while( er != null && (er == epar || er.IsChildOf( epar )) )
			{
				Selection esel = ((Field)er).GetSelection( tr );
				if( esel != Selection.Empty )
					a.Add(esel);
				er = Util.TraverseRight( er, true );
			}
		
			// see what we got
			if( a.Count == 0 )
			{
				ClearFieldSelection();
				return;
			}

			// set it as the current selection and notify
			aFieldSelection = a;

			// shut off field cursor		
			//FieldCursor();
		
			// notify interested parties
            // TODO: old value
			//hobj.edxselectionchange.fire();
		}

		//
		//						_rootClearFieldSelection
		//
		public void ClearFieldSelection()
		{
			if( aFieldSelection != null )
			{
				aFieldSelection = null;
				//hobj.edxselectionchange.fire();
			}
		}

		/// <summary>
		/// Scans selected field list seeing if we can apply spec'd tag to all items.
		/// </summary>
		/// <param name="sTag"></param>
		public bool CanApplyTag( string sTag )
		{
			if( aFieldSelection == null )
				return false;
	
				
			for(int i = 0; i < aFieldSelection.Count; i++ )
			{
				if( !((Selection)aFieldSelection[i]).oField.CanApplyTag( sTag ) )
					return false;
			}
			// made it, looks like we're good
			return true;
		}

		/// <summary>
		/// First scan and see if any of the selected fields already have the spec'd tag applied. If so, we unapply. Otherwise we proceed and apply.
		/// </summary>
		/// <param name="sTag"></param>
		public void ApplyTag(string sTag )
		{
			if( aFieldSelection == null )
				return;
	
			// if we have lots of stuff selected, try to apply tag up high
			bool bHigh = aFieldSelection.Count > 2;
		
			// open a transaction
			XmlManager xmlmgr = this.XmlManager;
			xmlmgr.OpenTransaction( null );
		
			// snapshot the top nodes and maps for all segments
			List<XmlNode> aTopNodes = new List<XmlNode>();
            Dictionary<string, Dictionary<string, string>> aMaps = new Dictionary<string, Dictionary<string, string>>();
            View v = EdxDocument.GetEdxDocument.GetView();
				
			for(int i = 0; i < aFieldSelection.Count; i++ )
			{
				EdxNode e = ((Selection) aFieldSelection[i]).oField.GetTopContainer();
				aTopNodes.Add(e.GetXmlNode());
				aMaps.Add("", v.GetContainerMap( e.edxtemplate )); // TODO: Add field value here as key
			}

			// see which way we're going, setting or clearing		
			bool bSense = true;
			for(int i = 0; i < aFieldSelection.Count; i++ )
			{
				if( ((Selection) aFieldSelection[i]).oField.IsApplied( sTag, aTopNodes[i] as XmlNode) )
				{
					bSense = false;
					break;
				}
			}

			// perform the apply or remove operations
			List<XmlNode> aChanged = new List<XmlNode>();
            for (int i = 0; i < aFieldSelection.Count; i++)
			{
                Selection fieldSelection = aFieldSelection[i];
				if( bSense )
				{
					if( !(fieldSelection.oField.IsApplied( sTag, aTopNodes[i] as XmlNode) )) // !
                        aChanged.Add(fieldSelection.oField.ApplyTag(sTag, fieldSelection, bHigh, aTopNodes[i], aMaps[sTag])); // TODO: sTag = i
				}
				else
				{
					if( (fieldSelection.oField.IsApplied( sTag, aTopNodes[i] as XmlNode ) ))
                        aChanged.Add(fieldSelection.oField.RemoveTag(sTag, fieldSelection, aTopNodes[i], aMaps[sTag])); // TODO: sTag = i
				}
			}

			// now run thru collapsing as many nodes as we can
			for(int i = 0; i < aChanged.Count; i++ )
			{
                if( aChanged[i] != null )
                    Util.CoalesceXmlNodes( aChanged[i], xmlmgr );
			}
		
			// build minimal notify list
			List<XmlNode> aNotify = new List<XmlNode>();
			for(int i = 0; i < aChanged.Count; i++ )
			{
                //if( aChanged[i] != null && Util.ArrayIndex( aNotify, aChanged[i] as XmlNode) == -1 )
                //    aNotify.Add(aChanged[i]);
			}
		
			// clear the document's selection
			editor.Selection.ClearSelection();
			ClearFieldSelection();
				
			// and do alerts to cause redraws
			for(int i = 0; i < aNotify.Count; i++ )
			{
				AlertChange( aNotify[i] as XmlNode, null );			
			}
		
			// close out transaction
			xmlmgr.SetNotify( aNotify );
			xmlmgr.CloseTransaction();
		}

		/// <summary>
		/// Deletes a text selection.
		/// </summary>
        /// <remarks>
        /// Walks a selection deleting the individual pieces and redrawing the view(s) as needed.
        /// </remarks>
        public void DeleteTextSelection()
		{
			XmlManager xmlmgr = root.XmlManager;
			xmlmgr.OpenTransaction( null );
		
			// snapshot the top nodes and maps for all segments
			List<XmlNode> aTopNodes = new List<XmlNode>();
				
			for(int i = 0; i < aFieldSelection.Count; i++ )
			{
				EdxNode e = ((Selection) aFieldSelection[i]).oField.GetTopContainer();
				if( e == null )
				{
					aTopNodes.Add(((Selection) aFieldSelection[i]).oField.GetXmlNode().ParentNode);
				}
				else
				{
					aTopNodes.Add(e.GetXmlNode());
				}
			}

			List<XmlNode> aNotify = new List<XmlNode>();
		
			// first run thru and process any partial deletions (whole node not going away)
			for(int i = 0; i < root.aFieldSelection.Count; i++ )
			{
				Selection sel = (Selection) root.aFieldSelection[i];
				if( !sel.bAll || !sel.oField.CanDelete )
				{
					// just remove some chars
					Field e = sel.oField;
					string s = e.EditText.Substring( 0, sel.start ) + e.EditText.Substring( sel.end );
					e.editText = s;
					XmlNode n = e.GetXmlNode();
					xmlmgr.Process( "updateNode", n, s );
					if( Util.ArrayIndex( aNotify, n ) == -1 )
					{
						aNotify[aNotify.Count] = n;
					}
				}
			}
		
			// now do the ones where whole nodes will vanish
			for(int i = 0; i < root.aFieldSelection.Count; i++ )
			{
				Selection sel = (Selection) root.aFieldSelection[i];
				if( sel.bAll && sel.oField.CanDelete )
				{
					Field e = sel.oField;
					XmlNode n = e.DeleteNode( aTopNodes[i] );
					if( Util.ArrayIndex( aNotify, n ) == -1 )
					{
						aNotify[aNotify.Count] = n;
					}
				}
			}

			// some of the XML nodes on the notify list may actually have been
			// deleted during course of processing subsequent nodes (coalescing action)
			// so we look for parentless nodes and remove 'em from list
			for(int i = 0; i < aNotify.Count; i++ )
			{
				if( ((XmlNode)aNotify[i]).ParentNode == null )
				{
					aNotify.RemoveAt(i);
					i--;
				}
			}		
		
			// done processing XML
			xmlmgr.SetNotify( aNotify );
			xmlmgr.CloseTransaction();
		
			// clear selection
			root.ClearFieldSelection();

            editor.Selection.ClearSelection();
		
			// redraw the appropriate parent container(s)
			for(int i = 0; i < aNotify.Count; i++ )
			{
				AlertChange( aNotify[i] as XmlNode, null );
			}
		}

		/// <summary>
        /// Copy text from selected fields to clipboard.
		/// </summary>
		public void CopyTextSelection()
		{
			// walk fields grabbing their text
			string sText = "";
				
			for(int i = 0; i < aFieldSelection.Count; i++ )
			{
				Selection sel = (Selection) root.aFieldSelection[i];
				if( !sel.bAll )
				{
					// grab just selected char
					Field e = sel.oField;
					sText += e.EditText.Substring( sel.start, sel.end );
				}
				else
				{
					Field e = sel.oField;
					sText += e.EditText;
				}
			}
		
			// place on clipboard
			Clipboard.SetDataObject(sText); // "Text"
		}

		/// <summary>
		/// Kludge routine called by timeout to try to get focus back after doing a select in a field.
		/// </summary>
		public void RestoreFocus()
		{
			if( aFieldSelection != null && aFieldSelection.Count == 1 )
			{
				Field e = ((Selection) aFieldSelection[0]).oField;
				// TODO: e.hobj.focus();
			}
		}
		
		/// <summary>
		/// Compares to dictionaries for equivalency.
		/// </summary>
		/// <param name="d1"></param>
		/// <param name="d2"></param>
		public bool RootCorrelate(Hashtable d1, Hashtable d2 )
		{
			//  enumerate the dIDs and see what we're missing
			ArrayList da = new ArrayList(d1.Keys);
			string s = "";
		
			for(int i = 0; i < da.Count; i++ )
			{
				if( d2.ContainsKey( da[i] ) )
				{
					if( d1[ da[i] ] != d2[ da[i] ] )
						s += "mismatch: " + da[i] + "\n";
				}
				else
				{
					s += "missing: " + da[i] + "\n";
				}
			}
			if( s.Length != 0 )
			{
				Util.Err("correlation error:\n" + s );
				return false;
			}
			return true;
		}

	}
}