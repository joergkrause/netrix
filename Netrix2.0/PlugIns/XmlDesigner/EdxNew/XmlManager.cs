using System;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace GuruComponents.Netrix.XmlDesigner.Edx
{

	/// <summary>
	/// Manages changes to the underlying XML DOM and provides history functions for undo/redo.
	/// </summary>
	public class XmlManager
	{

		private Root root;
		private List<Transaction> aTransactions;
		private int transactionIndex = 0;
		private Transaction currentTransaction = null;
	
		/// <summary>
		/// Ctor for manager.
		/// </summary>
		/// <param name="r"></param>
        public XmlManager(Root r ) // TODO: or base interface
        {
            // init vars
            this.root = r;
            this.aTransactions = new List<Transaction>();
            this.transactionIndex = 0;
            this.currentTransaction = null;	
        }

		/// <summary>
        /// Starts a transaction, providing a node to notify on upon undo/redo.
		/// </summary>
		/// <param name="n"></param>
		public void OpenTransaction(XmlNode n ) // or EdxNode ??
		{
			if( currentTransaction != null )
			{
				Util.Err("openTransaction: transaction already open" );
				return;
			}
			currentTransaction = new Transaction( n );
		}

        /// <summary>
        /// Closes out current transaction.
        /// </summary>
		public void CloseTransaction()
		{
			if( currentTransaction == null )
			{
				Util.Err("closeTransaction: no transaction open" );
				return;
			}
			aTransactions[transactionIndex++] = currentTransaction;
	
			// erase any transactions further ahead as redo is no longer an option
			for (int i = aTransactions.Count; i > transactionIndex; i--)
			{
				aTransactions.RemoveAt(i);
			}
			currentTransaction = null;
		
			// send document change event
			//root.hobj.edxdocumentchange.fire();
		}

		/// <summary>
        /// Cancels the current transaction.
		/// </summary>
		public void CancelTransaction()
		{
			if( currentTransaction == null )
			{
				Util.Err("cancelTransaction: no transaction open" );
				return;
			}
		
			// simply remove pending transaction, error recovery must be done by caller
			currentTransaction = null;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
		public void SetNotify(XmlNode n )
		{
			List<XmlNode> a = new List<XmlNode>(1);
			a.Add(n);
			SetNotify(a);
		}

        /// <summary>
        /// Sets the notify node on an open transaction
        /// </summary>
        /// <remarks>Useful when we don't know the
		//	highest node until late in the process.  Param 'n' can be either a single
		//	node or an array of nodes.</remarks>
        /// <param name="n"></param>
		public void SetNotify(List<XmlNode> n )
		{
			if( currentTransaction == null )
			{
				Util.Err("xmlmgr.setNotify: no transaction open" );
				return;
			}
		
			// convert to signatures
			ArrayList aSig = new ArrayList();
			for(int i = 0; i < n.Count; i++ )
			{
				aSig.Add(Util.GetSigFromNode( n[i] as XmlNode ));
			}
	
			// store it in current transaction
			currentTransaction.aNotify = aSig;
		}

		/// <summary>
		/// Public routine which processes a verb to make a change to the XML DOM.
		/// </summary>
		/// <remarks>
		///	Verbs and uses:
		///		"updateNode" oNode, sNewVal        - updates an XML element, attribute, or text node value
		///		"moveChildUp" oNode, index         - moves child at spec'd position up one slot
		///		"moveChildDown" oNode, index       - moves child at spec'd position down one slot
		///		"insertNode" oParent, oNode, index - inserts new node at  spec'd position
		///		"deleteNode" oNode, index          - deletes node at  spec'd position
		///		"splitText" oNode, index           - splits text at spec'd character
		///		"joinText" oNode				   - joins text from oNode and next sibling
		///		"splitNode" oNode, index		   - splits node at spec'd child index
		///		"joinNodes"	oNode				   - joins node with next sibling
		/// </remarks>
		public void Process(string command, params object[] arguments)
		{
			if( currentTransaction == null )
			{
				Util.Err("process: no open transaction" );
				return;
			}
		    
            // now actually do it
			Exec(command, true, arguments );
		}

        /// <summary>
        /// Executes an internal command.
        /// </summary>
        /// <remarks>
        ///	Private routine which runs a command.  Called either by process() or by undo()/redo()
        ///	to perform a basic XML DOM manipulation.  If bForw is true a complement undo command is 
        ///	first entered into the current transaction.  Else, a complement redo command is gen'd.
        /// </remarks>
        /// <param name="command"></param>
        /// <param name="bForw"></param>
        /// <param name="a"></param>
		private void Exec(string command, bool bForw, params object[] a)
		{
			if( currentTransaction == null )
			{
				Util.Err("exec: no current transaction" );
				return;
			}
		
			XmlNode tmp;
            List<string> sig;
			XmlNode node;
			int index;
			object[] comp;
			// decode command
			switch( command )
			{
				case "updateNode":
					if( a.Length != 2 )
					{
						Util.Err("updateNode requires two params" );
						return;
					}
			
					sig = Util.GetSigFromNode( a[1] as XmlNode );
					node = Util.GetNodeFromSig( sig, root.EditXml );
					string val = a[2].ToString();
					string oldVal = Util.GetXmlNodeValue( node );
			
					Transaction ct = currentTransaction;
					Complement( bForw, "updateNode", sig, oldVal);
			
					// perform the update
					Util.UpdateXmlNodeValue( node, val );
					break;
		
				case "moveChildUp":
                    if (a.Length != 2)
					{
						Util.Err("moveChildUp requires two params" );
						return;
					}
					sig = Util.GetSigFromNode( a[1] as XmlNode );
					node = Util.GetNodeFromSig( sig, root.EditXml );
					index = (int)a[2];

					Complement( bForw, "moveChildDown", sig, index - 1);
			
					tmp = node.RemoveChild( node.ChildNodes[index] );
					node.InsertBefore( tmp, node.ChildNodes[index-1] );
					break;

				case "moveChildDown":
                    if (a.Length != 2)
					{
						Util.Err("moveChildDown requires two params" );
						return;
					}
					sig = Util.GetSigFromNode( a[1] as XmlNode );
					node = Util.GetNodeFromSig( sig, root.EditXml );
					index = (int)a[2];
			
					Complement( bForw, "moveChildUp", sig, index + 1);
			
					tmp = node.RemoveChild( node.ChildNodes[index] );
					if( index == node.ChildNodes.Count - 1 )
						node.AppendChild( tmp );
					else
						node.InsertBefore( tmp, node.ChildNodes[index+1] );
					break;
		
				case "insertNode":
                    if (a.Length != 3)
					{
						Util.Err("insertNode requires three params" );
						return;
					}
					List<string> parentSig = Util.GetSigFromNode( a[1] as XmlNode);
					XmlNode parentNode = Util.GetNodeFromSig( parentSig, root.EditXml );
					node  = (XmlNode) a[2];
					index = (int) a[3];
			
					Complement( bForw, "deleteNode", parentSig, index);
						
					if( index == parentNode.ChildNodes.Count )
						parentNode.AppendChild( node );
					else
						parentNode.InsertBefore( node, parentNode.ChildNodes[index] );
					break;

				case "deleteNode":
                    if (a.Length != 2)
					{
						Util.Err("deleteNode requires two params" );
						return;
					}
					sig = Util.GetSigFromNode( a[1] as XmlNode );
					node = Util.GetNodeFromSig( sig, root.EditXml );
					index = (int)a[2];
			
					XmlNode oldnode = node.RemoveChild( node.ChildNodes[index] );
					Complement( bForw, "insertNode", sig, oldnode.CloneNode(true), index);

					break;
		
				case "splitText":
					SplitText( bForw, a );
					break;
		
				case "joinText":
					JoinText( bForw, a );
					break;
		
				case "splitNode":
					SplitNode( bForw, a );
					break;
		
				case "joinNodes":
					JoinNodes( bForw, a );
					break;
			
				default:
					Util.Err("xmlmgr: unknown verb: " + command );
					return;
			}
		}

		/// <summary>
		/// Complement adds an action to the undo/redo stack.
		/// </summary>
		/// <param name="comp"></param>
		/// <param name="bForw"></param>
        public void Complement(bool bForw, params object[] comp)
		{
			Transaction ct = currentTransaction;
			if( bForw )
				ct.aUndo.Push(comp);
			else
				ct.aRedo.Push(comp);
		}

		/// <summary>
		/// CanUndo
		/// </summary>
		/// <returns></returns>
		public bool CanUndo()
		{
			return this.transactionIndex > 0;
		}

		/// <summary>
		/// CanRedo
		/// </summary>
		/// <returns></returns>
		public bool CanRedo()
		{
			return this.transactionIndex < this.aTransactions.Count;
		}

		/// <summary>
		/// Undo last change.
		/// </summary>
		public void Undo()
		{
            // TODO:
            //if( transactionIndex == 0 )
            //    return;

            //// go back a step		
            //transactionIndex--;
            //Transaction ct = (Transaction) aTransactions[transactionIndex];
            //currentTransaction = ct;
            //ArrayList steps = ct.aUndo;

            //// walk steps in reverse order
				
            //ct.aRedo = new Array();
            //for(int i = steps.Count - 1; i >= 0; i-- )
            //{
            //    Exec( steps[i], false );
            //}
            //currentTransaction = null;
		
            //// DOM node(s) to alert on
            ////DoAlerts( ct.aNotify );

            //// send document change event
            ////root.hobj.edxdocumentchange.fire();
		}

		/// <summary>
		/// Redo last operation from transaction stack.
		/// </summary>
        public void Redo()
		{
            //if( transactionIndex == aTransactions.Count )
            //    return;

            //// go forward a step		
            //Transaction ct = (Transaction) aTransactions[transactionIndex];
            //currentTransaction = ct;
            //transactionIndex++;
            //Stack steps = ct.aRedo;

            //// walk steps in forward order
            //ct.aUndo = new Stack();
				
            //for(int i = steps.Count - 1; i >= 0; i-- )
            //{
            //    Exec( steps.Pop().ToString(), true, null );
            //}
            //currentTransaction = null;

			// DOM node to alert on
			//DoAlerts( ct.aNotify );

			// send document change event
			//root.hobj.edxdocumentchange.fire();
		}

		//
		//						DoAlerts
		//
        //public void DoAlerts(ArrayList a )
        //{
        //    for(int i = 0; i < a.Count; i++ )
        //    {
        //        XmlNode n = Util.GetNodeFromSig( a[i] as ArrayList, root.EditXml );
        //        root.AlertChange( n, null );
        //    }
        //}

		/// <summary>
		/// Splits text sections.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="bForw"></param>
		public void SplitText( bool bForw, params object[] a )
		{
			if( a.Length != 3 )
			{
				Util.Err("splitText requires two params" );
				return;
			}
			List<string> sig  = Util.GetSigFromNode( a[1] as XmlNode );
			XmlNode node = Util.GetNodeFromSig( sig, root.EditXml );
			int index = (int)a[2];
		
			XmlNode newNode;
			switch( node.NodeType )
			{
				case XmlNodeType.Text:
					newNode = node.CloneNode( false );
					break;
				case XmlNodeType.Element:
					newNode = node.CloneNode( true );
					break;
				default:
					Util.Err("Can't do text split on nodes of type '" + node.NodeType.ToString() + "'" );
					return;
			}

			Complement( bForw, "joinText", sig);
		
			// now assign the appropriate text to each side depending on cursor position
			string sLeft = Util.GetXmlNodeValue( node );
			string sRight = sLeft;
			sLeft = sLeft.Substring( 0, index );
			sRight = sRight.Substring( index );
		
			Util.UpdateXmlNodeValue( node, sLeft );
			Util.UpdateXmlNodeValue( newNode, sRight );
		
			// and insert the new node to the right of the original
			XmlNode par = node.ParentNode;
			if( node.NextSibling != null )
				par.InsertBefore( newNode, node.NextSibling );
			else
				par.AppendChild( newNode );
		}

		/// <summary>
		/// JoinText
		/// </summary>
		/// <param name="a">XmlNode[]</param>
		/// <param name="bForw"></param>
        public void JoinText(bool bForw, params object[] a)
		{
			if( a.Length != 2 )
			{
				Util.Err("joinText requires one param" );
				return;
			}
			List<string> sig  = Util.GetSigFromNode( a[1] as XmlNode );
			XmlNode node = Util.GetNodeFromSig( sig, root.EditXml );
			XmlNode nextNode = node.NextSibling;
		
			switch( node.NodeType )
			{
				case XmlNodeType.Text:
				case XmlNodeType.Element:
					break;
				default:
					Util.Err("Can't do text join on nodes of type '" + node.NodeType.ToString() + "'" );
					return;
			}
			string s = Util.GetXmlNodeValue( node );
		
			Complement( bForw, "splitText", sig, s.Length);
				
			s += Util.GetXmlNodeValue( nextNode );
			Util.UpdateXmlNodeValue( node, s );
		
			// and remove the right node
			node.ParentNode.RemoveChild( node.NextSibling );
		}

		/// <summary>
		/// Splits a node at the spec'd child index.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="bForw"></param>
        public void SplitNode(bool bForw, params object[] a)
		{
			if( a.Length != 3 )
			{
				Util.Err("splitNode requires two params" );
				return;
			}
			List<string> sig = Util.GetSigFromNode( a[1]  as XmlNode);
			XmlNode node = Util.GetNodeFromSig( sig, root.EditXml );
			int index = (int) a[2];
		
			Complement( bForw, "joinNodes", sig);
				
			// do a split
			XmlNode newNode = node.CloneNode( false );
		
			// remove from left, append to right
			while( node.ChildNodes.Count != index )
			{
				XmlNode n = node.RemoveChild( node.ChildNodes[index] );
				newNode.AppendChild( n );
			}
		
			// attach to parent
			XmlNode par = node.ParentNode;
			if( node.NextSibling != null )
				par.InsertBefore( newNode, node.NextSibling );
			else
				par.AppendChild( newNode );
		}

		/// <summary>
		/// Joins a node with its next sibling.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="bForw"></param>
		public void JoinNodes(bool bForw, params object[] a )
		{
			if( a.Length != 2 )
			{
				Util.Err("joinNodes requires one param" );
				return;
			}
			List<string> sig = Util.GetSigFromNode( a[1] as XmlNode);
			XmlNode node = Util.GetNodeFromSig( sig, root.EditXml );
		
			Complement(bForw, "splitNode", sig, node.ChildNodes.Count);
				
			// move children from next sibling onto left node
			XmlNode nextNode = node.NextSibling;
				
			while( nextNode.ChildNodes.Count != 0 )
			{
				XmlNode n = nextNode.RemoveChild( nextNode.ChildNodes[0] );
				node.AppendChild( n );
			}
		
			// remove the next sibling from the parent
			XmlNode par = node.ParentNode;
			par.RemoveChild( nextNode );
		}

		/// <summary>
		/// Flushes the undo/redo buffer.
		/// </summary>
		public void ClearHistory()
		{
			this.aTransactions = new List<Transaction>();
			this.transactionIndex = 0;
		}
	}
}