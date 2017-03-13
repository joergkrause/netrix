using System;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.Events;
using System.Collections.Generic;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner.Edx
{

    /// <summary>
    /// Inline field definition.
    /// </summary>
	public class Field : Region
	{

        /// <summary>
        /// Type of field
        /// </summary>
		public enum FieldType
		{
            /// <summary>
            /// Complex element
            /// </summary>
			Rich,
            /// <summary>
            /// Simple flow element
            /// </summary>
			Flow
		}

        /// <summary>
        /// Field type.
        /// </summary>
		public FieldType fieldType;	// TODO: Enum
		private string sHTMLSnapshot;
		public bool bHonorIndexOnFocus;
		public string editText;
		public int cursorIndex;

		/// <summary>
		/// Constructor for field class.
		/// </summary>
        /// <param name="editor"></param>
		/// <param name="p"></param>
		/// <param name="sTemp"></param>
		/// <param name="sPath"></param>
		/// <param name="sOptions"></param>
		/// <param name="index"></param>
        public Field(IHtmlEditor editor, EdxNode p, string sTemp, string sPath, string sOptions, int index)
            : base(editor, p, sTemp, sPath, sOptions, index)
		{
            if (sTemp.IndexOf(":") == 0)
            {
                throw new ArgumentException("Field template must be in format field:type. Provided name :" + sTemp);
            }
			// init our vars	
			this.fieldType = (Field.FieldType) Enum.Parse(typeof(Field.FieldType), sTemp.Substring(6), true);
			if( this.fieldType != FieldType.Flow )
				Util.Err("field must be type flow" );
			this.sHTMLSnapshot = null;
			this.bHonorIndexOnFocus = false;
	
			// for doing our own editing
			this.editText = null;
			this.cursorIndex = -1;

        }

        # region Service Method

        /// <summary>
        /// Current text of the field.
        /// </summary>
        public string EditText
        {
            get
            {
                return editText;
            }
        }

        /// <summary>
        /// Creates XHTML for field.
        /// </summary>
        /// <param name="oFrag"></param>
        /// <returns></returns>
		public override XmlNode XHTML(XmlNode oFrag )
		{
			//if we already have HTML assoc'd, load it directly
			if( this.hobj != null )
			{
                oFrag.InnerText = this.hobj.InnerText;
			}
			return oFrag;
		}

        /*

		//
		//						_fieldAssociate
		//
		public void Associate(XmlControl h )
		{
			base.Associate( h );
	
			// sanity check the tag we're being attached to to make sure it support content editable
			// couldn't find a good way to do this so I'll just check a few that bit ME
			switch( h.TagName.ToUpper() )
			{
				case "TR":
				case "TD":
					Util.Err("Error: you cannot attach a field:* template to a " + h.TagName + " tag at this time." );
					return;
				default:
					break;
			}
	
			// set up for editing
			h.tabIndex = 0;
			XmlNode editnode = this.GetXmlNode();
			string s = Util.GetXmlNodeValue( editnode );
			h.InnerText = s;
			this.editText = s;
			this.root.WatchChanges( editnode, this );

			// attach event handlers
			h.OnFocus		+= new DocumentEventHandler(field_OnFocus);
			h.OnBlur		+= new DocumentEventHandler(field_OnBlur );
			h.OnClick		+= new DocumentEventHandler(field_OnClick );
			h.OnKeyDown		+= new DocumentEventHandler(field_OnKeyDown );
			h.OnKeyUp		+= new DocumentEventHandler(field_OnKeyUp );
			h.OnKeyPress	+= new DocumentEventHandler(field_OnKeyPress );
		}

		//
		//						fieldOnFocus
		//
		//	Note: has event context.
		//
		public void field_OnFocus(object sender, DocumentEventArgs evt)
		{
			EdxControl h = evt.SrcElement as EdxControl;
			Field e = (Field) h.eobj;
			e.root.focusField = e;
			Interop.IHTMLElement2 body = (Interop.IHTMLElement2) Edx.Instance.Editor.GetBodyElement().GetBaseElement();
	
			// see if we're supposed to go to spec'd position
			if( e.bHonorIndexOnFocus )
				e.bHonorIndexOnFocus = false;
			else
				e.cursorIndex = 0;

			// make a text range pointing to current element's text		
			Interop.IHTMLTxtRange tr = ((Interop.IHtmlBodyElement)body).createTextRange();
			tr.MoveToElementText( e.hobj.GetBaseElement() );
	
			// see if we need to auto-select some placeholder text (kinda kludgey, I know)
			if( e.editText.Length > 0 
				&& e.editText[0] == '[' 
				&& e.editText[e.editText.Length-1] == ']' 
				)
			{
				if( e.root.aFieldSelection != null
					&& e.root.aFieldSelection.Count == 1
					&& ((Selection)e.root.aFieldSelection[0]).oField.Equals(e)
					&& ((Selection)e.root.aFieldSelection[0]).bAll == true
					)
				{
					// already selected
					return;
				}
		
				e.cursorIndex = 0;
				tr.Select();
				System.Threading.Thread.Sleep(100);
				Edx.Instance.eobj.RestoreFocus();
				return;
			}

			// find cursor coors
			tr.Move( "character", e.cursorIndex );
			Interop.IHTMLRect rect = h.getClientRects()[0] as Interop.IHTMLRect;
			e.root.FieldCursor( rect.right, rect.top, rect.bottom - rect.top );
		}

		/// <summary>
		/// Looks at position of input index and adjusts cursor position accordingly.
		/// </summary>
		/// <param name="h"></param>
		public void SetCursorPosition(XmlControl h )
		{
			string s = editText.Substring( 0, cursorIndex );
			Interop.IHTMLElement2 body = (Interop.IHTMLElement2) Edx.Instance.Editor.GetBodyElement().GetBaseElement();
			Interop.IHTMLTxtRange tr = ((Interop.IHtmlBodyElement) body).createTextRange();
			tr.MoveToElementText( h );
			tr.Collapse( true );
			tr.MoveEnd( "character", cursorIndex );
			Interop.IHTMLRectCollection r = body.GetClientRects() as Interop.IHTMLRectCollection; // tr
			int rcnt = r.length;
			Interop.IHTMLRect rect = r.item(rcnt - 1) as Interop.IHTMLRect;
		
			// expand by one char to see if we're at end of a line
			if( cursorIndex != editText.Length )
			{
				tr.Expand( "character" );
				r = body.GetClientRects(); //tr
				if( r.length != rcnt )
				{
					// grew a new rectangle, go to start of it
					rect = r[r.length - 1];
					root.FieldCursor( rect.left, rect.top, rect.bottom - rect.top );
					return;
				}
			}
		
			// stay with end of previous
			root.FieldCursor( rect.right, rect.top, rect.bottom - rect.top );
		}

		//
		//						_fieldEnterRight
		//
		//	Enters field at far right edge.
		//
		public void EnterRight()
		{
			cursorIndex = editText.Length;
			bHonorIndexOnFocus = true;
			hobj.focus();
		}

		//
		//						_fieldEnterLeft
		//
		//	Enters field at far left edge.
		//
		public void EnterLeft()
		{
			cursorIndex = 0;
			bHonorIndexOnFocus = true;
			hobj.focus();
		}

		//
		//						_fieldProcessBackspaceKey
		//
		public void ProcessBackspaceKey()
		{
			if( cursorIndex > 0 )
			{
				string s = editText;
				cursorIndex--;
				s = s.Substring( 0, cursorIndex ) + s.Substring( cursorIndex + 1 );
				//alert( "s: " + s );
				hobj.InnerText = s;
				editText = s;
				SetCursorPosition( hobj );
				return;
			}
		
			// find where we're going
			EdxNode el = Util.TraverseLeft( this, true );
			CursorSave oCursor;
			if( el != null )
			{
				// save position as an XML node signature and an offset into same
				int leftPosition = ((Field)el).editText.Length;
				ArrayList leftSig = Util.GetSigFromNode( el.GetXmlNode());
				int instance = ((Field)el).GetObservingInstance();
				oCursor = new CursorSave( leftSig, leftPosition, instance );
			}
		
			// see if we scrub where we've been
			if( editText.Length == 0 )
			{
				XmlManager xmlmgr = root.XmlManager;
				xmlmgr.OpenTransaction( null );
				XmlNode n = DeleteNode( GetTopContainer() );
				xmlmgr.SetNotify( n );
				xmlmgr.CloseTransaction();
				root.AlertChange( n, null );
			}
		
			// go to new home
			RestoreCursor( oCursor );
		}

		/// <summary>
		/// Used to help freeze dry a cursor position.  
		/// </summary>
		/// <remarks>
		/// When multiple fields are observing the same XML node, this finds the instance # of the current field so that we
		/// can return to the proper observing instance later.  See restoreCursor() below for more.
		/// </remarks>
		/// <returns></returns>
		public int GetObservingInstance()
		{
			int i = 0;
			Root e = root;
			bool bDir = false;
			XmlNode editnode = GetXmlNode();
			while( e != null )
			{
				EdxNode e = Util.TraverseRight( e, bDir );
				if( e.Equals(this) )
					return i;
				if( e.GetXmlNode() == editnode )
					i++;
				bDir = true;
			}
			Util.Err("getObservingInstance: couldn't find self" );
			return 0;
		}

		/// <summary>
		/// Restores cursor position from freeze-dried snapshot.
		/// </summary>
		/// <param name="oCursor"></param>
		public void RestoreCursor(CursorSave oCursor )
		{
			if( oCursor != CursorSave.Empty )
			{
				// re-hydrate sig to node
				XmlNode nLeft = Util.GetNodeFromSig( oCursor.signature, root.oEditXml );
				int inst = oCursor.instance;
			
				// traverse all fields until we find one that owns this node
				Root eCur = root;
				bool bDir = false;
				while( true )
				{
					eCur = Util.TraverseRight( eCur as EdxNode, bDir );
					if( eCur == null )
						break;
					bDir = true;
					if( eCur.GetXmlNode() == nLeft )
					{
						if( inst-- == 0 )
						{
							eCur.cursorIndex = oCursor.offset;
							eCur.bHonorIndexOnFocus = true;
							eCur.hobj.blur();	// force it to process as new focus
							eCur.hobj.focus();
							return;
						}
					}
				}
			}
		
			// fell thru, not an error, just nowhere to go, clear cursor
			root.FieldCursor();
		}

		//
		//						_fieldProcessDeleteKey
		//
		public void ProcessDeleteKey()
		{
			// check for selection
			if( root.aFieldSelection != null )
			{
				root.FieldCursor();
			
				// see if we can find a cursor spot to return to
				CursorSave oCursor;
				if( ((Selection) root.aFieldSelection[0]).bAll )
				{
					Field eLeft = Util.TraverseLeft( ((Selection)root.aFieldSelection[0]).oField ) as Field;
					if( eLeft != null )
					{
						ArrayList leftSig = Util.GetSigFromNode( eLeft.GetXmlNode() );
						int instance = ((Field)eLeft).GetObservingInstance();
						oCursor = new CursorSave( leftSig, eLeft.editText.Length, instance );
					}
				}
				else
				{
					Field e = ((Selection)root.aFieldSelection[0]).oField;
					int instance = e.GetObservingInstance();
					ArrayList leftSig = Util.GetSigFromNode( e.GetXmlNode() );
					oCursor = new CursorSave( leftSig, e.start, instance );
				}
				root.DeleteTextSelection();
			
				RestoreCursor( oCursor );
				return;
			}
		
			if( editText.Length > cursorIndex )
			{
				string s = editText;
				s = s.Substring( 0, cursorIndex ) + s.Substring( cursorIndex + 1 );
				hobj.InnerText = s;
				editText = s;
			}
		}

		//
		//						_fieldReplaceSelection
		//
		public void ReplaceSelection(string sText )
		{
			root.FieldCursor();
			
			// pop the new char into the first selected field
			CursorSave oCursor;
			Selection oSel = (Selection) root.aFieldSelection[0];
			Field e = oSel.oField;
			XmlControl h = e.hobj;
			string s = e.editText;
			e.editText = s.Substring( 0, oSel.start ) + sText + s.Substring( oSel.start );
			h.InnerText = e.editText;
		
			// modify the first selection to start after newly added char
			oSel.start += sText.Length;
			oSel.end += sText.Length;
			oSel.bAll = false;
		
			// grab this position
			ArrayList leftSig = Util.GetSigFromNode( oSel.oField.GetXmlNode() );
			int instance = oSel.oField.GetObservingInstance();
			oCursor = new CursorSave( leftSig, oSel.start, instance );

			root.DeleteTextSelection();
			
			RestoreCursor( oCursor );
		}

		//
		//						fieldOnKeyDown
		//
		//	Note: has event context.
		//
		public void field_OnKeyDown(object sender, DocumentEventArgs evt)
		{
			Keys keyCode = evt.KeyCode;
			EdxControl h = evt.SrcElement as EdxControl;
			Field e = h.eobj as Field;
	
			if( keyCode == Keys.ShiftKey || keyCode == Keys.ControlKey || keyCode == Keys.Alt )
				return;
		
			switch( keyCode )
			{
//				case 16:		// shift
//				case 17:		// ctrl
//				case 18:		// alt
//					return;
	
				case Keys.Right:		// right arrow
					if( e.root.aFieldSelection != null )
					{
						e.root.ClearFieldSelection();
						window.document.selection.empty();
					}		
					if( e.cursorIndex < e.editText.Length )
					{
						if( evt.ControlKey )
						{
							int w = e.editText.Substring( e.cursorIndex ).IndexOf(" "); //.search( /\s+/ );
							if( w != -1 )
							{
								e.cursorIndex += w;
								w = e.editText.Substring( e.cursorIndex ).IndexOf(" "); //.search( /\w/ );
								if( w != -1 )
								{
									e.cursorIndex += w;
									e.SetCursorPosition(h as EdxControl);
									break;
								}
							}
							e.cursorIndex = e.editText.Length;
						}
						else
							e.cursorIndex++;
			
						e.SetCursorPosition(h);
					}
					else
					{
						// look for adjacent field to hop to
						Field er = Util.TraverseRight( e, true ) as Field;
						if( er != null )
						{
							er.EnterLeft();
						}
					}
					break;
		
				case Keys.Left:		// left arrow (37)
					if( e.root.aFieldSelection != null )
					{
						e.root.ClearFieldSelection();
						window.document.selection.empty();
					}		
					if( e.cursorIndex > 0 )
					{
						if( evt.ControlKey )
						{
							int w = e.cursorIndex;
							while( w > 0 && e.editText[ w-1 ] != ' '  )
								w--;
							if( w == e.cursorIndex )
							{
								while( w > 0 && e.editText[ w-1 ] == ' ' )
									w--;
								while( w > 0 && e.editText[ w-1 ] != ' ' )
									w--;
							}
							e.cursorIndex = w;		
						}
						else
							e.cursorIndex--;
						e.SetCursorPosition(h);
					}
					else
					{
						Field el = Util.TraverseLeft( e, true ) as Field;
						if( el != null )
						{
							el.EnterRight();
						}
					}
					break;
	
				case Keys.Back:			// backspace
					break;
	
				case Keys.Delete:		// delete
					e.ProcessDeleteKey();
					break;
				
//				case 67:		// ctrl-C (copy)
//				case 76:		// ctrl-V (paste)
//				case 78:		// ctrl-X (cut)
//					break;
		
				default:
					return;
			}
	
			// consume the event
			evt.SetCancelBubble(true);
		}


		/// <summary>
		/// fieldOnKeyPress
		/// </summary>
		/// <remarks>
		/// Oddly even tho we are cancelling bubble, we still end up having this prop up to top level frame.
		/// </remarks>
		/// <param name="sender"></param>
		/// <param name="evt"></param>
		public void field_OnKeyPress(object sender, DocumentEventArgs evt)
		{
			Keys keyCode = evt.KeyCode;
			EdxControl h = evt.SrcElement as EdxControl;
			Field e = h.eobj as Field;
			XmlManager xmlmgr = e.root.XmlManager;
			switch( keyCode )
			{
				case Keys.C:			// ctrl-C (copy)
				case Keys.X:		// ctrl-X (cut)
					// look for a selection to move to clipboard
					if( e.root.aFieldSelection != null )
					{
						e.root.CopyTextSelection();
						if( keyCode == Keys.X )
						{
							e.ProcessDeleteKey();
						}
					}
					break;
	
				case Keys.Back:			// ctrl-H (backspace)
					// treat same as delete for simplicity's sake
					if( e.root.aFieldSelection != null )
					{
						e.ProcessDeleteKey();
						break;
					}
		
					// normal processing	
					e.ProcessBackspaceKey();
					break;

				//case 10:
				case Keys.Enter:
					// ignore this when there's a selection pending (no reason, just avoiding writing
					// complicated code)
					if( e.root.aFieldSelection != null )
						break;
			
					// do node splitting, auto-insertion handling here
				switch( e.GetEnterAction() )
				{
					case "split":
						e.SplitNode();
						break;
					case "new":
						e.NewNode();
						break;
					default:
						break;
				}
					break;
	
				case Keys.V:		// ctrl-V (paste)
					// get data to paste
					string sText = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();
					if( sText == "" || sText == null )
						break;
			
					// see if we need to process a pending selection
					if( e.root.aFieldSelection != null )
					{
						e.ReplaceSelection( sText );
						return;
					}
					string s = e.editText;
					e.editText = s.Substring( 0, e.cursorIndex ) + sText + s.Substring( e.cursorIndex );
					h.InnerText = e.editText;
					e.cursorIndex += sText.Length;
					e.SetCursorPosition( evt.SrcElement as EdxControl );
					break;
		
				case Keys.Y:		// ctrl-Y (redo)
					if( xmlmgr.CanRedo() )
					{
						e.root.FieldCursor();
						xmlmgr.Redo();
					}
					break;
		
				case Keys.Z:		// ctrl-Z (undo)
					e.SaveNode();
					
					if( xmlmgr.CanUndo() )
					{
						e.root.FieldCursor();
						xmlmgr.Undo();
					}
					break;

				default:
					// see if we need to process a pending selection
					if( e.root.aFieldSelection != null )
					{
						e.ReplaceSelection( keyCode.ToString()  );
						return;
					}
					string s = e.editText;
					e.editText = String.Concat(s.Substring( 0, e.cursorIndex ), (char) keyCode, s.Substring( e.cursorIndex ));
					h.InnerText = e.editText;
					e.cursorIndex++;
					e.SetCursorPosition( evt.SrcElement as EdxControl);
					break;
			}
			// consume the event
			evt.SetCancelBubble( true );
		}

		//
		//						fieldOnKeyUp
		//
		//	Note: has HTML node 'this' context
		//
		public void field_OnKeyUp(object sender, DocumentEventArgs evt)
		{
			Keys keyCode = evt.KeyCode;
			Field e = ((EdxControl) evt.SrcElement).eobj as Field;

			switch( keyCode )
			{
				case Keys.Shift:		//shift
				case Keys.Control:		// ctrl
				case Keys.Alt:			// alt
					break;
	
				default:
					break;
			}
			// consume the event
			evt.SetCancelBubble( true );
		}

		/// <summary>
		/// Checks for change in the field and captures the value to XML if necessary.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="evt"></param>
		public void field_OnBlur(object sender, DocumentEventArgs evt)
		{
			Keys keyCode = evt.KeyCode;
			Field e = ((EdxControl)evt.SrcElement).eobj as Field;

			// save off value if changed
			e.SaveNode();
			
			// turn off cursor for this field
			e.root.FieldCursor();
		}

		//
		//						fieldOnClick
		//
		public void field_OnClick(object sender, DocumentEventArgs evt)
		{
			Keys keyCode = evt.KeyCode;
			Field e = ((EdxControl) evt.SrcElement).eobj as Field;
	
			Interop.IHTMLTxtRange tr = window.document.selection.createRange();
			Interop.IHTMLRect rect = tr.getClientRects()[0];
			e.root.FieldCursor( rect.left, rect.top, rect.bottom - rect.top );
	
			Interop.IHTMLTxtRange tr2 = window.document.body.createTextRange();
			tr2.MoveToElementText( h );
			tr2.SetEndPoint( "EndToStart", tr );
			e.cursorIndex = tr2.GetText().Length;
		}

		/// <summary>
		/// Saves off to XML if changed.
		/// </summary>
		public void SaveNode()
		{
			XmlNode editnode = GetXmlNode();
			if( editnode != null )
			{
				if( Util.GetXmlNodeValue( editnode ) != hobj.InnerText )
				{
					XmlManager xmlmgr = root.XmlManager;
					xmlmgr.OpenTransaction( editnode );
					xmlmgr.Process( "updateNode", editnode, hobj.InnerText );
					xmlmgr.CloseTransaction();

					root.AlertChange( editnode, this );
				}
			}
		}

		//
		//						_fieldOnXmlNodeChange
		//
		public void OnXmlNodeChange( object sender )
		{
			// ignore updates from ourself
			if( sender == this )
				return;

			XmlNode editnode = this.GetXmlNode();
			if( editnode != null )
			{
				if( Util.GetXmlNodeValue( editnode ) != this.hobj.InnerText )
				{
					this.editText = Util.GetXmlNodeValue( editnode );
					this.hobj.InnerText = this.editText;
					this.cursorIndex = 0;
				}
			}
		}

		/// <summary>
		/// Walks up tree looking for a defined enter action: split, new, or none Defaults to 'none' if no definition is found.
		/// </summary>
		/// <returns></returns>
		public string GetEnterAction()
		{
			string action = "none";
			Field cur = this;
			while( cur != null )
			{
				if( cur.enterAction != undefined )
				{
					action = cur.enterAction;
					break;
				}
				cur = cur.parent;
			}
			return action;
		}

		/// <summary>
		/// Walks up tree looking for appropriate place to create a new template node.
		/// </summary>
		public void NewNode()
		{
			Util.Err("newNode not implemented yet" );
		}

		/// <summary>
		/// Splits the current node and walks up tree splitting until it hits a barrier. Used to generate new paragraphs on enter and that sort of thing.
		/// </summary>
		public void SplitNode()
		{
			root.FieldCursor( -1 );				// turn off cursor
			SaveNode();							// save off in case changed

			// perform the split		
			XmlNode editnode = GetXmlNode();
			XmlManager xmlmgr = root.XmlManager;
			xmlmgr.OpenTransaction( null );		// don't know alert node yet
			xmlmgr.Process( "splitText", editnode, cursorIndex );
			XmlNode newNode = editnode.NextSibling;
		
			// find the index of where we split
			int i;	
			XmlNode par = editnode.ParentNode;
			for(i = 0; i < par.ChildNodes.Count; i++ )
			{
				if( par.ChildNodes[i] == newNode )
					break;
			}
			if( i == par.ChildNodes.Count )
			{
				Util.Err("splitNode couldn't find next sibling" );
				return;
			}
				
			// walk up the tree splitting until we hit the container that says no
			XmlNode topChild;
			if( !parent.CanSplit() )
				topChild = this;
			else
				topChild = parent.SplitNode( i, editnode );  
		
			// set undo/redo notify and close out
			xmlmgr.SetNotify( topChild.parent.getXmlNode() );
			xmlmgr.CloseTransaction();
		
			// update parent container
			var enew = topChild.parent.nodeHasSplit( topChild );
		
			// establish focus in new node
			EdxNode enew = Util.TraverseRight( enew, false );
			enew.hobj.focus();
		}

		//
		//						_fieldCleanup
		//
		public new void Cleanup()
		{
			if( root.focusField == this )
				root.focusField = null;
			XmlNode editnode = GetXmlNode();
			if( editnode != null )
			{
				root.UnwatchChanges( editnode, this );
			}
			root.DeassignID( id );
			parent.RemoveChild( this );
		}
         
         */

        /// <summary>
        /// Checks whether we can delete this field.
        /// </summary>
        /// <returns></returns>
        public override bool CanDelete
        {
            get
            {
                // TODO: No such method in EdxNode
                //if (!this.parent.permitChildDelete())
                //    return false;
                return true;
            }
        }

        /// <summary>
        /// Deletes a leaf edit node.
        /// </summary>
        /// <remarks>Expects an already-eop XML manager transaction.</remarks>
        public XmlNode DeleteNode(XmlNode nTopContainer)
        {
            XmlNode editnode = GetXmlNode();
            XmlManager xmlmgr = root.XmlManager;

            // delete the nodes and any newly empty parent(s)
            XmlNode nTop = Util.DeleteXmlNode(editnode, nTopContainer, xmlmgr);

            // coalesce if possible
            if (nTop != nTopContainer)
            {
                Util.CoalesceXmlNodes(nTop, xmlmgr);
            }

            return nTop;
        }


        /// <summary>
        /// Returns true if we can wrap ourselves (or selected portion thereof) in spec'd tag. 
        /// </summary>
        /// <remarks>
        ///	Note: these next few routines are tricky cuz we're destroy the synchronization
        ///	between the edxnode tree and the XML node tree as we work.  We need to only
        ///	assume we still have our original XML node but not much else.
        /// </remarks>
        /// <param name="sTag"></param>
        /// <returns></returns>
        public bool CanApplyTag(string sTag)
        {
            // get master container
            EdxNode eTop = GetTopContainer();
            if (eTop == null)
                return false;

            XmlNode oTopNode = eTop.GetXmlNode();
            Dictionary<string, string> aMap = EdxDocument.GetEdxDocument.GetView().GetContainerMap(eTop.edxtemplate);

            // trace out ancestry up to master container node
            List<XmlNode> aAncestry = new List<XmlNode>();
            XmlNode n = GetXmlNode();
            while (n != oTopNode)
            {
                // if we hit the desired tag, we of course then can apply it (already applied)
                if (n.Name.Equals(sTag))
                    return true;

                aAncestry.Add(n);
                n = n.ParentNode;
            }

            // now walk down looking for place to apply spec'd tag
            for (int i = aAncestry.Count - 1; i >= 0; i--)
            {
                string sCurTag = ((XmlNode)aAncestry[i]).Name;
                //Hashtable aChildMap = (Hashtable)aSubMap[sTag];
                //if (aChildMap is Hashtable && aChildMap[sCurTag] != null)
                //    return true;
                //aSubMap = aSubMap[sCurTag];
                //if (!(aMap is Array))
                //    break;
            }

            // never hit match		
            return false;
        }

        /// <summary>
        /// Inserts spec'd tag somewhere up above us.
        /// </summary>
        /// <remarks>
        ///	If bHigh is set, try to set it as high
        ///	as possible in the hierarchy if there are multiple choices.  Else, try to set low.
        ///	As with comment further above, we are working only in the XML tree here-- the edxnode
        ///	is in an unknown state from possible previous processing.  Caution!
        /// </remarks>
        /// <param name="sTag"></param>
        /// <param name="oSel"></param>
        /// <param name="bHigh"></param>
        /// <param name="oTopNode"></param>
        /// <param name="aMap"></param>
        /// <returns>Returns highest affected node.</returns>
        public XmlNode ApplyTag(string sTag, Selection oSel, bool bHigh, XmlNode oTopNode, Dictionary<string, string> aMap)
        {
            // trace out ancestry up to master container node
            ArrayList aAncestry = new ArrayList();
            XmlNode editnode = GetXmlNode();
            XmlNode n = editnode;
            while (n != oTopNode)
            {
                aAncestry.Add(n);
                n = n.ParentNode;
            }
            Dictionary<string, string> aSubMap;
            // now walk down looking for place to apply spec'd tag				
            int iApply = -1;
            for (int i = aAncestry.Count - 1; i >= 0; i--)
            {
                string sCurTag = ((XmlNode)aAncestry[i]).Name;
                //Dictionary<string, string> aChildMap = aMap[sTag];
                //if (aChildMap != null && aChildMap[sCurTag] != null)
                //{
                //    iApply = i;
                //    if (bHigh)	// looking for highest possible?
                //        break;	// done
                //}
                //aSubMap = aMap[sCurTag];
                //if (aSubMap == null)
                //    break;
            }
            if (iApply == -1)
            {
                // must be some mistake, never hit a match
                Util.Err("applyTag: never found insertion spot.");
                return null;
            }

            // should already be an open XML transaction
            XmlManager xmlmgr = root.XmlManager;

            // see if we need to do a self-split first
            if (!oSel.bAll)
            {
                if (oSel.end != editText.Length)
                {
                    xmlmgr.Process("splitText", editnode, oSel.end);
                }
                if (oSel.start != 0)
                {
                    xmlmgr.Process("splitText", editnode, oSel.start);
                    editnode = editnode.NextSibling;
                }
            }

            // split up to selected ancestor as needed
            XmlNode nHighest = ((XmlNode)aAncestry[iApply]).ParentNode;
            XmlNode nPrev = editnode;
            XmlNode nCur = editnode.ParentNode;
            while (nCur != nHighest)
            {
                if (nCur.ChildNodes.Count != 1)
                {
                    int iIndex = Util.ArrayIndex(nCur.ChildNodes, nPrev);
                    if (iIndex == -1)
                    {
                        Util.Err("applyTag: XML node found in parent");
                        return null;
                    }
                    if (iIndex != nCur.ChildNodes.Count - 1)
                    {
                        xmlmgr.Process("splitNode", nCur, iIndex + 1);
                    }
                    if (iIndex != 0)
                    {
                        xmlmgr.Process("splitNode", nCur, iIndex);
                        nCur = nCur.NextSibling;
                    }
                }
                nPrev = nCur;
                nCur = nCur.ParentNode;
            }

            // insert the desired node immediately above the selected ancestor node
            XmlDocument oDoc = nHighest.OwnerDocument;
            string nsuri = nHighest.NamespaceURI;
            XmlNode nNew = oDoc.CreateNode(XmlNodeType.Element, sTag, nsuri); // first para was 1 (Element ??)
            int idx = Util.ArrayIndex(nHighest.ChildNodes, nPrev);
            if (idx == -1)
            {
                Util.Err("applyTag: couldn't find XML node index to insert");
                return null;
            }
            xmlmgr.Process("deleteNode", nHighest, idx);
            xmlmgr.Process("insertNode", nHighest, nNew, idx);
            xmlmgr.Process("insertNode", nNew, nPrev, 0);

            // whew
            return nHighest;
        }


        /// <summary>
        /// Removes tag from up above.  Same basic comments as applyTag above.
        /// </summary>
        /// <param name="sTag"></param>
        /// <param name="oSel"></param>
        /// <param name="oTopNode"></param>
        /// <param name="aMap"></param>
        /// <returns>Returns highest affected node.</returns>
        public XmlNode RemoveTag(string sTag, Selection oSel, XmlNode oTopNode, Dictionary<string, string> aMap)
        {
            // trace out ancestry up to master container node
            ArrayList aAncestry = new ArrayList();
            XmlNode editnode = GetXmlNode();
            XmlNode n = editnode;
            while (n != oTopNode)
            {
                aAncestry[aAncestry.Count] = n;
                n = n.ParentNode;
            }

            // now walk down looking for place to apply spec'd tag

            int iApply = -1;
            for (int i = aAncestry.Count - 1; i >= 0; i--)
            {
                if (((XmlNode)aAncestry[i]).Name == sTag)
                {
                    iApply = i;
                    break;
                }
            }
            if (iApply == -1)
            {
                // must be some mistake, never hit a match
                Util.Err("removeTag: never found tag to remove: '" + sTag + "'");
                return null;
            }

            // should already be an open XML transaction
            XmlManager xmlmgr = root.XmlManager;

            // see if we need to do a self-split first
            if (!oSel.bAll)
            {
                if (oSel.end != editText.Length)
                {
                    xmlmgr.Process("splitText", editnode, oSel.end);
                }
                if (oSel.start != 0)
                {
                    xmlmgr.Process("splitText", editnode, oSel.start);
                    editnode = editnode.NextSibling;
                }
            }

            // split up to selected ancestor as needed
            XmlNode nHighest = ((XmlNode)aAncestry[iApply]).ParentNode;
            XmlNode nPrev = editnode;
            XmlNode nCur = editnode.ParentNode;
            while (nCur != nHighest)
            {
                if (nCur.ChildNodes.Count != 1)
                {
                    int iIndex = Util.ArrayIndex(nCur.ChildNodes, nPrev);
                    if (iIndex == -1)
                    {
                        Util.Err("removeTag: couldn't find XML node in parent");
                        return null;
                    }
                    if (iIndex != nCur.ChildNodes.Count - 1)
                    {
                        xmlmgr.Process("splitNode", nCur, iIndex + 1);
                    }
                    if (iIndex != 0)
                    {
                        xmlmgr.Process("splitNode", nCur, iIndex);
                        nCur = nCur.NextSibling;
                    }
                }
                nPrev = nCur;
                nCur = nCur.ParentNode;
            }

            // remove the selected ancestor node
            XmlNode nRemove = nPrev;
            XmlNode nSave = nRemove.ChildNodes[0];
            int idx = Util.ArrayIndex(nHighest.ChildNodes, nRemove);
            if (idx == -1)
            {
                Util.Err("removeTag: node to remove not found in parent");
                return null;
            }
            xmlmgr.Process("deleteNode", nHighest, idx);
            xmlmgr.Process("insertNode", nHighest, nSave, idx);

            // whew
            return nHighest;
        }


        /// <summary>
        /// Returns true if the spec'd tag is found anywhere up above the current field.
        /// </summary>
        /// <param name="sTag"></param>
        /// <param name="oTopNode"></param>
        public bool IsApplied(string sTag, XmlNode oTopNode)
        {
            // trace out ancestry up to master container node
            XmlNode n = GetXmlNode();
            while (!n.Equals(oTopNode))
            {
                // if we hit the desired tag, we of course then can apply it (already applied)
                if (n.Name == sTag)
                    return true;
                n = n.ParentNode;
            }

            // fell thru, not applied
            return false;
        }

        /// <summary>
        /// Looks to see if all or part of current field lies inside the supplied text range.  
        /// </summary>
        /// <remarks>
        /// If an intersection is found, returns a fieldSelection object defining the selection, else null.
        /// </remarks>		
        public Selection GetSelection(Interop.IHTMLTxtRange tr)
        {
            int start, end;
            Interop.IHTMLTxtRange dup;

            Interop.IHTMLTxtRange ourtr = ((Interop.IHtmlBodyElement)editor.GetBodyElement().GetBaseElement()).createTextRange();
            // TODO: Connect to native element
            //ourtr.MoveToElementText(hobj);

            if (tr.InRange(ourtr))
            {
                // we are equal to or completely contained in the main range
                return new Selection(this, true, 0, editText.Length);
            }

            // see if main selection is completely inside us
            if (ourtr.InRange(tr))
            {
                dup = ourtr.Duplicate();
                dup.SetEndPoint("EndToStart", tr);
                start = dup.GetText().Length;

                dup = ourtr.Duplicate();
                dup.SetEndPoint("StartToEnd", tr);
                end = editText.Length - dup.GetText().Length;
                if (start == end)	// don't return empty
                    return Selection.Empty;
                return new Selection(this, false, start, end);

            }

            // check for left endpoint within main range
            dup = ourtr.Duplicate();
            dup.Collapse(true);
            if (tr.InRange(dup))
            {
                // left side is "in"
                dup.SetEndPoint("EndToEnd", tr);
                end = dup.GetText().Length;
                if (end == 0)	// don't return empty
                    return Selection.Empty;
                return new Selection(this, false, 0, end);
            }

            // check for right endpoint within main range
            dup = ourtr.Duplicate();
            dup.Collapse(false);
            if (tr.InRange(dup))
            {
                // right side is "in"
                dup.SetEndPoint("StartToStart", tr);
                start = editText.Length - dup.GetText().Length;
                if (start == editText.Length)	// don't return empty
                    return Selection.Empty;
                return new Selection(this, false, start, editText.Length);
            }

            // no intersection
            return Selection.Empty;
        }

        /// <summary>
        /// Looks up node hierarchy for a non-splitting container.
        /// </summary>
        /// <returns></returns>
        public EdxNode GetTopContainer()
        {
            // look upwards for non-splitting container
            EdxNode par = this.parent;
            while (par != null && par.CanSplit)
                par = par.parent;

            // if we didn't hit a container, we can't apply, done
            if (par == null || !(par is Container))
                return null;

            return par;
        }

# endregion
	}
}