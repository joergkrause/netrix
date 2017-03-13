using System;
using System.Xml;
using System.Collections;

using System.Windows.Forms;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner.Edx
{

	/// <summary>
	/// Misc. utility subs for EDX Document editor.
	/// </summary>
	public class Util
	{

        const string XhtmlStrict = "xhtml1-strict.dtd";
        const string XhtmlLat1 = "xhtml-lat1.ent";
        const string XhtmlSpecial = "xhtml-special.ent";
        const string XhtmlSymbol = "xhtml-symbol.ent";

		public static string InnerXML(XmlNode node )
		{
			XmlNode child = node.FirstChild;
			if( child == null )
				return "";
			StringBuilder s = new StringBuilder();
			while( child != null )
			{
				s.Append( child.InnerXml);
				child = child.NextSibling;
			}
			return s.ToString();
		}

        public static string XHTML(XmlNode node)
        {
            XmlNode child = node.FirstChild;
            if (child == null)
                return "";
            StringBuilder s = new StringBuilder();
            while (child != null)
            {
                s.Append(child.OuterXml);
                child = child.NextSibling;
            }
            return s.ToString();
        }

		/// <summary>
		/// Attempts to manage special casing for loading TBODY and TR elements.
		/// </summary>
		/// <param name="htmlNode"></param>
		/// <param name="oXml"></param>
		public static void SetInnerHTML(XmlNode htmlNode, XmlNode oXml)
		{
			switch( htmlNode.Name.ToUpper() )
			{
				case "TBODY":
					Util.InsertRows( htmlNode, oXml );
					break;
				case "TR":
					Util.InsertCells( htmlNode, oXml );
					break;
				default:
					Util.Err("Error: util.setInnerHTML got invalid parent tag: " + htmlNode.Name );
					break;
			}
		}

        public static XmlParserContext CreateXhtmlContext()
        {
            XmlNameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
            XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
            context.DocTypeName = "html";
            context.PublicId = "-//W3C//DTD XHTML 1.0 Strict//EN";
            context.SystemId = "xhtml1-strict.dtd";
            return context;
        }

        /// <summary>
        /// Helper method that returns a new initialized <see cref="T:XmlResolver"/> implementation.
        /// </summary>
        public static XmlResolver CreateXhtmlResolver()
        {
            XhtmlResolver resolver = new XhtmlResolver();
            resolver.AddResource(new Uri("urn:" + "-//W3C//DTD XHTML 1.0 Strict//EN"), typeof(XmlDesigner), XhtmlStrict);
            resolver.AddResource(new Uri("urn:" + XhtmlStrict), typeof(XmlDesigner), XhtmlStrict);
            resolver.AddResource(new Uri("urn:" + XhtmlLat1), typeof(XmlDesigner), XhtmlLat1);
            resolver.AddResource(new Uri("urn:" + XhtmlSpecial), typeof(XmlDesigner), XhtmlSpecial);
            resolver.AddResource(new Uri("urn:" + XhtmlSymbol), typeof(XmlDesigner), XhtmlSymbol);
            return resolver;
        }

        /// <summary>
        /// Initializes a new <see cref="T:XmlNamespaceManager"/> with the default namespace set to XHTML.
        /// </summary>
        /// <remarks>NOTE: This is only used to simplify the XPath test expression above</remarks>
        public static XmlNamespaceManager CreateEdxNamespaceManager()
        {
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(new NameTable());
            nsMgr.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            nsMgr.AddNamespace("edx", "netrix-edx-edit-view");
            return nsMgr;
        }


		/// <summary>
		/// Insert rows if surrounding code forms a table.
		/// </summary>
		/// <param name="htmlNode"></param>
		/// <param name="oXml"></param>
        private static void InsertRows(XmlNode htmlNode, XmlNode oXml )
		{
			XmlNodeList children = oXml.ChildNodes;
			for(int i = 0; i < children.Count; i++ )
			{
				XmlNode child = children[i];
				string childTag = child.Name;

				// make sure it's a valid relationship
				childTag = childTag.ToUpper();
				if( childTag != "TR" )
				{
					Err( "Error: cannot add " + childTag + " to TBODY element." );
					return;
				}
		
				// let's make a new row
                XmlNode tr = htmlNode.AppendChild(htmlNode.OwnerDocument.CreateElement("TR"));
		
				// clone the TR attribs if any
				Util.CloneAttribsFromXml( tr, child );
		
				// now add the cells
				Util.InsertCells( tr, child );
			}
		}

		/// <summary>
		/// Insert row at specific position.
		/// </summary>
		/// <param name="hTable"></param>
		/// <param name="oXml"></param>
		/// <param name="index"></param>
        public static void InsertRowAt(XmlNode hTable, XmlNode oXml, int index )
		{
			// first child should be row to insert
			XmlNode child = oXml;
			if( child == null )
			{
				Util.Err("Error: utilInsertRowAt got null child" );
				return;
			}
			string childTag = child.Name.ToUpper();
	
			// make sure it's a valid relationship
			childTag = childTag.ToUpper();
			if( childTag != "TR" )
			{
				Util.Err("Error: utilInsertRowAt cannot add " + childTag + " to TBODY element." );
				return;
			}

			// let's make a new row
            XmlNode tr = hTable.OwnerDocument.CreateElement("TR");

			// clone the TR attribs if any
			CloneAttribsFromXml( tr, child );

			// now add the cells
			InsertCells( tr, child );
		}


		/// <summary>
		/// Insert Cells in a row.
		/// </summary>
		/// <param name="tr"></param>
		/// <param name="oXml"></param>
        public static void InsertCells(XmlNode tr, XmlNode oXml )
		{
			
			XmlNodeList children = oXml.ChildNodes;
			for(int i = 0; i < children.Count; i++ )
			{
				XmlNode child = children[i];
				string childTag = child.Name;
		
				// make sure it's a valid relationship
				childTag = childTag.ToUpper();
				if( childTag != "TD" )
				{
					Util.Err("Error: cannot add " + childTag + " to TR element." );
					return;
				}
		
				// let's make a new one
                XmlNode td = tr.OwnerDocument.CreateElement("TD");
		
				// clone the attribs if any
				CloneAttribsFromXml( td, child );
		
				// now we should be back in innerHTML land!
				td.InnerText = InnerXML( child );
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="hRow"></param>
		/// <param name="oXml"></param>
		/// <param name="index"></param>
		public static void InsertCellAt(XmlNode hRow, XmlNode oXml, int index )
		{
			// TODO: h should be of type TableCell, which needs to inherit XmlNode

			XmlNode child = oXml;
			string childTag = child.Name;
		
			// make sure it's a valid relationship
			childTag = childTag.ToUpper();
			if( childTag != "TD" )
			{
				Err( "Error: cannot add " + childTag + " to TR element." );
				return;
			}
		
			// let's make a new one
			XmlNode td = hRow.OwnerDocument.CreateElement("TD");
		
			// clone the attribs if any
			CloneAttribsFromXml( td , child );
		
			// now we should be back in innerHTML land!
			td.InnerText = InnerXML( child );
		}

		/// <summary>
		/// Walks the supplised XML attributes for this node and installs them on the HTML node.
		/// </summary>
		/// <param name="h"></param>
		/// <param name="x"></param>
		public static void CloneAttribsFromXml(XmlNode h, XmlNode x)
		{
			XmlAttributeCollection attribs = x.Attributes;
			for(int i = 0; i < attribs.Count; i++ )
			{
                XmlAttribute a = h.OwnerDocument.CreateAttribute(attribs[i].Name);
                a.Value = attribs[i].Value;
				h.Attributes.Append(a);               
			}
		}

		/// <summary>
		/// Attempts to load the supplied string into an XML doc.
		/// </summary>
		/// <param name="sXml"></param>
		public static XmlNode StringToXmlNode(string sXml )
		{
			XmlDocument oXml = new XmlDocument();
			XmlElement oRoot = null;
			if( oXml == null )
			{
				Err( "Error: stringToXmlNode couldn't instantiate XMLDOM" );
				return null;
			}
			try 
			{
				oXml.LoadXml( sXml );
				oRoot = oXml.DocumentElement;
			}
			catch(Exception e)
			{
				Err( "Error: " + e + "\ncouldn't load into XML: " + sXml );
				return null;
			}
			return oRoot as XmlNode;
		}

		/// <summary>
		/// Looks for spec'd attribute attrib and returns value or null if not found.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="sAttrib"></param>
		/// <returns></returns>
		public static string GetXmlAttribute(XmlNode node, string sAttrib )
		{
			if (node.Attributes[sAttrib] == null)
			{
				return null;
			} 
			else
			{
				return node.Attributes[sAttrib].Value;
			}
		}

		//
		//						utilSetXmlAttribute
		//
		public static void SetXmlAttribute(XmlNode oXml, string sAttrib, string sVal)
		{
			XmlAttribute attrib = oXml.OwnerDocument.CreateAttribute( sAttrib );
			attrib.Value = sVal;
			oXml.Attributes.Append( attrib );
		}

		//
		//						utilUpdateXmlNodeValue
		//
		public static void UpdateXmlNodeValue(XmlNode node, string val )
		{
			if( node == null )
			{
				Err( "utilUpdateXmlNodeValue: null node" );
				return;
			}
			switch( node.NodeType )
			{
				case XmlNodeType.Text:
					((XmlText) node).Value = val;
					break;
				case XmlNodeType.Attribute:
					((XmlAttribute)node).Value = val;
					break;
				default:
					node.Value = val;
					break;
			}
		}

		/// <summary>
		/// utilGetXmlNodeValue
		/// </summary>
		/// <param name="node"></param>
		public static string GetXmlNodeValue(XmlNode node)
		{
			if( node == null )
			{
				Err( "utilGetXmlNodeValue: null node" );
				return "";
			}
			switch( node.NodeType )
			{
				case XmlNodeType.Text:
					return node.Value;
		
				case XmlNodeType.Attribute:
					return node.Value;
	
				case XmlNodeType.Element:
					string s = "";
					for(int i = 0; i < node.ChildNodes.Count; i++ )
					{
						XmlNode child = node.ChildNodes[i];
						if( child.NodeType == XmlNodeType.Text )
							s += child.Value;
					}
					return s;
		
				default:
					Err( "utilGetXmlNodeValue: can't take value of node type '" + node.NodeType.ToString() + "'" );
					return "";
			}
		}

		/// <summary>
		/// Returns true if they're same type node with same attributes, contents may differ.
		/// </summary>
		/// <param name="n1"></param>
		/// <param name="n2"></param>
		/// <returns></returns>
		public static bool XmlNodesCanMerge(XmlNode n1, XmlNode n2)
		{
			// check for same type (JS was: nodeName)
			if( n1.NodeType != n2.NodeType )
				return false;
	
			// if they're text they can always merge
			if( n1.NodeType == XmlNodeType.Text )
				return true;
	
			// check for matching attributes
			for(int i = 0; i < n1.Attributes.Count; i++ )
			{
				XmlAttribute att1 = n1.Attributes[i];
				XmlAttribute att2 = n2.Attributes[att1.Name];
				if( att2 == null )
					return false;
				if( att1.Value != att2.Value )
					return false;
			}
	
			// looks good
			return true;
		}

		/// <summary>
		/// Attempts to find a field to the right of the spec'd one. Depth-first tree traversal.
		/// </summary>
		/// <param name="cur"></param>
		/// <param name="bUp"></param>
		public static EdxNode TraverseRight(EdxNode cur, bool bUp )
		{

			EdxNode en;

			// catch root
			if( cur == null )
				return null;

			if( bUp )
			{
				// look right first
				en = (EdxNode)cur.NextSibling;
				if( en == null )
					return Util.TraverseRight( cur.parent, true );
			}
			else
			{
				// look down first
				if( cur.ChildNodes.Count == 0 )
					return Util.TraverseRight( cur, true );
				en = cur.ChildNodes[0] as EdxNode;
			}	
			if( en is Field )
				return en;
			else
				return TraverseRight( en, false );
		}

		/// <summary>
		/// Attempts to find a field to the left of the spec'd one. Depth-first tree traversal.
		/// </summary>
		/// <param name="cur"></param>
		/// <param name="bUp"></param>
		/// <returns></returns>
		public static EdxNode TraverseLeft(EdxNode cur, bool bUp )
		{
			EdxNode en;

			// catch root
			if( cur == null )
				return null;

			if( bUp )
			{
				// look left first
				en = cur.PreviousSibling;
				if( en == null )
					return Util.TraverseLeft( cur.parent, true );
			}
			else
			{
				// look down first
				if( cur.ChildNodes.Count == 0 )
					return Util.TraverseLeft( cur, true );
				en = cur.ChildNodes[cur.ChildNodes.Count - 1];
			}	
			if( en is Field )
				return en;
			else
				return TraverseLeft( en, false );
		}

        /// <summary>
        /// Parses Options from string attribute.
        /// </summary>
        /// <remarks>
        ///	Expects edxoptions in CSS-like format:  name:value;name:value; etc.
        ///	Peels apart options string and returns an array of name/value pairs (see 
        ///	immediately below). Normalizes all trimming excess white space and 
        ///	converting to lower case.
        /// </remarks>
        /// <param name="sOptions">The option string.</param>
        /// <returns>A collection of option values.</returns>
        public static List<Option> ParseOptions(string sOptions)
		{
			string[] ops = sOptions.Split(';');
            List<Option> a = new List<Option>();
			
			for(int i = 0; i < ops.Length; i++ )
			{
				string op = ops[i].Trim();
				if( op.Length == 0 )
					continue;
			
				string[] nv = ops[i].Split(':');
				if( nv.Length != 2 )
				{
					Err( "Error: options must be of format 'name:value' and semicolon delimited: " + sOptions );
					return a;
				}
				a.Add(new Option( nv[0].Trim().ToLower(), nv[1].Trim().ToLower() ));
			}
			return a;
		}
        
		/// <summary>
		/// Takes an XML node and builds a signature path to that node.
		///	For convenience, also recognizes n as an existing signature and
		///	simply returns it.
		/// </summary>
		/// <param name="n"></param>
        public static List<string> GetSigFromNode(XmlNode n)
		{
			List<string> a = new List<string>();
	
			// first, see if it's an attribute-- they don't have parents like normal nodes
			if( n.NodeType == XmlNodeType.Attribute )
			{
				a.Add ("@" + n.Name);
				n = n.SelectSingleNode( ".." );		// get parent element
			}
	
			XmlNode par = n.ParentNode;
			while( par != null )
			{
				int i;
				for(i = 0; i < par.ChildNodes.Count; i++ )
				{
					if( par.ChildNodes[i] == n )
						break;
				}
				a.Add(i.ToString());
				n = par;
				par = n.ParentNode;
			}
			a.Reverse();
			return a;
		}

		/// <summary>
		/// Takes a signature array and returns the assoc'd node.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="oXmlDoc"></param>
        public static XmlNode GetNodeFromSig(List<string> a, XmlNode oXmlDoc)
		{
			XmlNode n = oXmlDoc;
			for(int i = 0; i < a.Count; i++ )
			{
				if(!String.IsNullOrEmpty(a[i]))
				{
					// hit an attribute
					n = n.SelectSingleNode( a[i] );
				}
				else
				{
					n = n.ChildNodes[i];
				}
			}
			return n;
		}

		/// <summary>
		/// Returns a string representation of a node signature.
		/// </summary>
		/// <param name="a"></param>
		public static string GetSigStringFromSig(string[] a )
		{
			string s = "";
			for(int i = 0; i < a.Length; i++ )
			{
				if( s.Length != 0 )
					s += ":";
				s += a[i];
			}
			return s;
		}

		/// <summary>
		/// Returns a node signature from a string representation.
		/// </summary>
		/// <param name="s"></param>
		public static IList GetSigFromSigString(string s )
		{
			string[] p = s.Split(':');
			ArrayList a = new ArrayList();
	
			for(int i = 0; i < p.Length; i++ )
			{
				if( p[i][0] != '@' )
					a[i] = Convert.ToInt32(p[i]);
				else
					a[i] = p[i];
			}
			return a as IList;
		}

		/// <summary>
		/// Finds index of an item within the passed array.  Returns -1 if not found.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="item"></param>
		public static int ArrayIndex(List<XmlNode> a, XmlNode item )
		{	
			for(int i = 0; i < a.Count; i++ )
			{
				if( ((XmlNode) a[i]).Equals(item))
					return i;
			}
			return -1;
		}

        public static int ArrayIndex(XmlNodeList a, XmlNode item)
        {
            for (int i = 0; i < a.Count; i++)
            {
                if (((XmlNode)a[i]).Equals(item))
                    return i;
            }
            return -1;
        }

		/// <summary>
		/// Descends into spec'd node and does as much work as it can merging like type nodes.
		/// </summary>
		/// <param name="nParent"></param>
		/// <param name="xmlmgr"></param>
        public static void CoalesceXmlNodes(XmlNode nParent, XmlManager xmlmgr )
		{
			bool bDidSomething = true;
	
			while( bDidSomething )
			{
				bDidSomething = false;
				for(int i = 0; i < nParent.ChildNodes.Count - 1; i++ )
				{
					XmlNode n1 = nParent.ChildNodes[i];
					XmlNode n2 = nParent.ChildNodes[i+1];
					if( Util.XmlNodesCanMerge( n1, n2 ) )
					{
						if( n1.NodeType == XmlNodeType.Text )
						{
							xmlmgr.Process( "joinText", n1 );
						}
						else
						{
							xmlmgr.Process( "joinNodes", n1 );
						}
						bDidSomething = true;
						break;
					}
				}
			}
			for(int i = 0; i < nParent.ChildNodes.Count; i++ )
			{
				Util.CoalesceXmlNodes( nParent.ChildNodes[i], xmlmgr );
			}
		}

		/// <summary>
		/// Removes an XML node and looks up tree to see if further cleanup can happen.
		/// </summary>
		/// <param name="nDelete"></param>
		/// <param name="nTopContainer"></param>
		/// <param name="xmlmgr"></param>
		/// <returns>Returns parent of last node deleted.</returns>
		public static XmlNode DeleteXmlNode(XmlNode nDelete, XmlNode nTopContainer, XmlManager xmlmgr)
		{
			// get parent of this node
			XmlNode nParent = nDelete.ParentNode;

			// find index of ourselves in parent
			int iIndex = Util.ArrayIndex( nParent.ChildNodes, nDelete );
		
			// tell XML manager to do the dirty work
			xmlmgr.Process( "deleteNode", nParent, iIndex );
		
			// see if this happens to leave us with an empty parent node
			if( nParent.ChildNodes.Count == 0 && nParent != nTopContainer )
			{
				return Util.DeleteXmlNode( nParent, nTopContainer, xmlmgr );
			}
			else
			{
				return nParent;
			}
		}

		/// <summary>
		/// utilArrayCopy
		/// </summary>
		/// <param name="a"></param>
		/// <returns></returns>
		public static IList ArrayCopy(IList a )
		{
            //IList clone = null;
            //a.CopyTo(clone, 0);
            //return clone;
            return null;
		}

		/// <summary>
		/// If the URL is relative, applies the supplied base path and returns full URL.
		/// </summary>
		/// <param name="sUrl"></param>
		/// <param name="sBase"></param>
		public static string ResolveUrl(string sUrl, string sBase )
		{
			string sFullPath = sUrl;
	
			// look for UNC or URLs like c:
			if( sUrl[0] == '\\' || sUrl[1] == ':' )
				return sUrl;
		
			// look for web URLs
			if( sUrl.IndexOf( "http://" ) == -1 && sUrl[0] != '/'  )
				sFullPath = sBase + sUrl;
			return sFullPath;
		}

		/// <summary>
		/// Error Message
		/// </summary>
		/// <param name="sMsg"></param>
		public static void Err(string sMsg)
		{
			MessageBox.Show( sMsg );
		}

		static string dbglog = String.Empty;

		/// <summary>
		/// Appends a string to a debug buffer.  Print 'dbglog' from immediate window in Visual Studio to see what's going on.
		/// </summary>
		/// <param name="s"></param>
		public static void dbg(string s)
		{
			DateTime d = DateTime.Now;
			dbglog += String.Format("({0}.{1}) {2}\n", d.Second, d.Millisecond, s);
		}


        internal static Color GetColor(object p)
        {
            if (p == null)
                return Color.Empty;
            else
                return ColorTranslator.FromHtml(p.ToString());
        }

        internal static EdxNode GetEdxFromElement(Interop.IHTMLElement element)
        {
            object[] attr = new object[1];
            element.GetAttribute("__eobj", 0, attr);
            EdxNode edx = attr[0] as EdxNode;
            return edx;
        }

    }
}