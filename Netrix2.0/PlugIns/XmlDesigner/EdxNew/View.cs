using System;
using System.Collections.Generic;
using System.Xml;
using System.Collections;

namespace GuruComponents.Netrix.XmlDesigner.Edx
{

	/// <summary>
	/// Provides handy access functions to the edit view descriptor document.
	/// </summary>
    /// <remarks>
    /// The transform document consists of multiple views within one views root.
    /// Each view is represented by one instance of this class.
    /// </remarks>
	public class View
	{

        private Dictionary<string, List<Match>> aContainers; // <templatename, Container>
        private Dictionary<string, Dictionary<string, string>> aContainerMaps; // <templatename, map path>
        private EdxDocument edxDoc;
		private XmlNode docroot;
	
		// default to first view in the document
		private XmlNode currentView;
		
		private string currentViewName;		
	
		/// <summary>
		/// Constructor for the view class.
		/// </summary>
		/// <param name="viewroot">Root node from native XML transform document.</param>
        /// <param name="edxDoc"></param>
		public View(XmlNode viewroot, EdxDocument edxDoc)
		{
			this.docroot = viewroot;
            this.edxDoc = edxDoc;
	
			// default to first view in the document
            this.currentView = viewroot.SelectSingleNode("//edx:view", edxDoc.XmlnsEdx);
			if( this.currentView == null )
			{
				Util.Err("No edit views found!" );
                return;
			}
			this.currentViewName = this.currentView.SelectSingleNode( "@uiname" ).Value;
	
			// maintain cache of container matches and maps
			this.aContainers = new Dictionary<string,List<Match>>();
			this.aContainerMaps = new Dictionary<string,Dictionary<string,string>>();
	
		}

		/// <summary>
		/// Looks up template node for specified template.
		/// </summary>
		/// <param name="sTemplate"></param>
		/// <returns></returns>
		public XmlNode GetTemplate( string sTemplate )
		{
            XmlNode node = this.currentView.SelectSingleNode("//edx:template[@name = '" + sTemplate + "']", edxDoc.XmlnsEdx);
			if( node == null )
			{
				// look for common definition
                node = this.docroot.SelectSingleNode("/edx:editviews/edx:common/edx:template[@name = '" + sTemplate + "']", edxDoc.XmlnsEdx);
			}
			if( node == null )
			{
				Util.Err("No information for " + sTemplate + " template in " + this.currentViewName + " view or common view definitions." );
			}
			return node;
		}

		/// <summary>
		/// Looks up type of this template ("region", "container").
		/// </summary>
		/// <param name="oTemplateNode"></param>
		/// <returns></returns>
		public TemplateType GetTemplateType(XmlNode oTemplateNode )
		{
            return (TemplateType)Enum.Parse(typeof(TemplateType), oTemplateNode.SelectSingleNode("@type", edxDoc.XmlnsEdx).Value, true);
		}

        /// <summary>
        /// Get template default display name.
        /// </summary>
        /// <param name="oTemplateNode"></param>
        /// <returns></returns>
		public string GetTemplateDefaultDisplayName(XmlNode oTemplateNode )
		{
            XmlNode n = oTemplateNode.SelectNodes("edx:xhtml", edxDoc.XmlnsEdx)[0];
			if( n == null )
			{
				Util.Err("Couldn't get default XHTML template." );
				return "";
			}
			XmlAttribute s = n.Attributes[ "display" ];
			if( s == null )
				return "default";
			else
				return s.Value;
		}

		/// <summary>
		/// Looks up template node for specified template name
		/// </summary>
		/// <param name="oTemplateNode"></param>
		/// <param name="sName"></param>
		public XmlNode GetTemplateHtmlByName(XmlNode oTemplateNode, string sName )
		{
			if( sName == null || sName == "default" )
                return oTemplateNode.SelectSingleNode("edx:xhtml", edxDoc.XmlnsEdx); // [0] must be the first
			else
                return oTemplateNode.SelectSingleNode("edx:xhtml[@display = '" + sName + "']", edxDoc.XmlnsEdx);
		}

		/// <summary>
		/// Looks up the display name for the spec'd template.
		/// </summary>
		/// <param name="oTemplateNode"></param>
		public string GetTemplateName(XmlNode oTemplateNode )
		{
            XmlNode node = oTemplateNode.SelectSingleNode("@uiname", edxDoc.XmlnsEdx);
			if( node == null )
			{
				// fall back to just the template name itself
                node = oTemplateNode.SelectSingleNode("@name", edxDoc.XmlnsEdx);
			}
			if( node != null )
				return node.Value;
			else
			{
				Util.Err("Error: template with no name" );
				return "";
			}
		}

		/// <summary>
		/// Gets list of display formats for spec'd template, e.g. default, details, hidden, etc.
		/// </summary>
		/// <param name="oTemplateNode"></param>
		/// <returns></returns>
		public List<string> GetDisplays(XmlNode oTemplateNode )
		{
            XmlNodeList nodes = oTemplateNode.SelectNodes("edx:xhtml", edxDoc.XmlnsEdx);
            List<string> a = new List<string>();
			
			for(int i = 0; i < nodes.Count; i++ )
			{
                XmlNode n = nodes[i].SelectSingleNode("@display", edxDoc.XmlnsEdx);
				if( n != null )
					a.Add(n.Value);
			}
			return a;
		}

		/// <summary>
		/// Returns an array of objects describing each of the matches on the spec'd container.
		/// </summary>
		/// <param name="sTemplate"></param>
        public List<Match> GetContainerMatches(string sTemplate)
		{
			// check for cached copy
			if( aContainers[sTemplate] != null )
                return aContainers[sTemplate];
			
			XmlNode oTemplateNode = GetTemplate( sTemplate );
			if( oTemplateNode == null )
			{
				Util.Err("getContainerMatches: no template found for '" + sTemplate + "'" );
				return null;
			}
		
			XmlNodeList nodes = oTemplateNode.SelectNodes( "edx:match", edxDoc.XmlnsEdx );
			List<Match> a = new List<Match>();				
	
			for(int i = 0; i < nodes.Count; i++ )
			{
				string sTag = (nodes[i].Attributes[ "element" ] == null) ? null : nodes[i].Attributes[ "element" ].Value;
				if( sTag != null )
				{
					// get template name from XHTML node
					if( nodes[i].ChildNodes.Count != 1 )
					{
						Util.Err("getContainerMatches: a match must contain one and only one HTML node inside the match spec, tag = " + sTag );
						return null;
					}
					string sMatchTemplate = (nodes[i].ChildNodes[0].Attributes[ "edxtemplate" ] == null) ? null : nodes[i].ChildNodes[0].Attributes[ "edxtemplate" ].Value;
					if( sMatchTemplate == null )
					{
						Util.Err("getContainerMatches: no edxtemplate spec'd for match on tag " + sTag );
						return null;
					}
				
					// got candidate, check it's template for XML insert fragment
                    XmlNode oins = this.currentView.SelectSingleNode("edx:template[@name='" + sMatchTemplate + "']", edxDoc.XmlnsEdx);
					if( oins != null )
					{
						// got a fragment?
                        if (oins.SelectSingleNode("edx:insert", edxDoc.XmlnsEdx) != null)
						{
							string sUI = (oins.Attributes[ "uiname" ] == null) ? null : oins.Attributes[ "uiname" ].Value;
							if( sUI == null )
								sUI = sTag;
						
							// create a full match spec
							Match oMatch = new Match( sTag, sMatchTemplate, sUI, oins );
							a.Add(oMatch);
						}
					}
				}
			}
		
			// preserve for future
			aContainers[sTemplate] = a;
			return a;
		}

		/// <summary>
		/// Looks for or builds a tag map for spec'd container template.
		/// </summary>
		/// <param name="sTemplate"></param>
        public Dictionary<string, string> GetContainerMap(string sTemplate)
		{
            if (aContainerMaps[sTemplate] == null)
            {
                CompileContainerMap(sTemplate);
            }
			return aContainerMaps[sTemplate];
		}

		/// <summary>
        /// Compiles a tag map for spec'd container template.
		/// </summary>
		/// <param name="sTemplate"></param>
        public void CompileContainerMap(string sTemplate )
		{
			// look up template
			XmlNode oTemp = GetTemplate( sTemplate );
			if( oTemp == null )
				return;
		
			// see what type we have
			TemplateType sType = GetTemplateType( oTemp );
			if( sType == TemplateType.Container )
			{
				// parse container match contents
                XmlNodeList nodes = oTemp.SelectNodes("edx:match", edxDoc.XmlnsEdx);
			
				// make sure we have some matches
				if( nodes.Count == 0 )
				{
					// pretty dumb container, but maybe it's a development stub, no error
					//aContainerMaps[sTemplate] = "*";	// show no sub-containment
                    aContainerMaps[sTemplate] = new Dictionary<string, string>();
                    aContainerMaps[sTemplate]["*"] = null;
					return;
				}

				// put an empty map in place to keep us from trying to compile ourselves
				Dictionary<string, string> a = new Dictionary<string,string>();
				aContainerMaps[sTemplate] = a;
			
				// parse matched element list
					
				for(int i = 0; i < nodes.Count; i++ )
				{
					XmlNode n = nodes[i];
					string sTag = n.Attributes[ "element" ].Value;
					if( sTag == null || sTag == "" )
					{
						Util.Err("Bad match spec in container '" + sTemplate + "'.  Must specify element to match." );
						continue;
					}
				
					// verify we see one and only one node of XHTML inside the match
					if( n.ChildNodes.Count != 1 )
					{
						Util.Err("Bad XHTML spec for match on tag '" + sTag + "'.  Must contain one and only one node of XHTML." );
						continue;
					}
				
					// get the XHTML node for this match
					XmlNode oXhtml = n.ChildNodes[0];
				
					// get the template and options (if spec'd)
					string sTmp = (oXhtml.Attributes[ "edxtemplate" ] == null) ? null : oXhtml.Attributes[ "edxtemplate" ].Value;
					if( sTmp == null || sTmp == "" )
					{
						// must not be a "live" node, just show as end of line
						a[sTag] = "*";
						continue;
					}
				
					// figure out if splittable
					bool bSplit;
					if( sTmp.Substring( 0, 6 ) == "field:" )
					{
						a[sTag] = "*";
						continue;
					}
					else if( sTmp.Substring( 0, 7 ) == "widget:" )
					{
						a[sTag] = "*";
						continue;
					}
					else if( GetTemplateType( GetTemplate( sTmp ) ) == TemplateType.Container )
						bSplit = true;
					else
						bSplit = false;
				
					// see if explicit options say otherwise
					string sOptions = (oXhtml.Attributes[ "edxoptions" ] == null) ? null : oXhtml.Attributes[ "edxoptions" ].Value;
					if( sOptions != null && sOptions != "" )
					{
						IList aOps = Util.ParseOptions( sOptions );
						for(int j = 0; j < aOps.Count; j++ )
						{
							if( ((Option) aOps[j]).Name == "allow-split" && ((Option) aOps[j]).Value == "true" )
							{
								bSplit = true;
								break;
							}
						}
					}
				
					// if it's not splittable, end of the line
					if( !bSplit )
					{
						a[sTag] = "*";
						continue;
					}
				
					// descend into it
					GetContainerMap( sTmp );
					// a[sTag] = aContainerMaps[sTmp];
				}
			
				// note: our map is already in place, we're done
			}
			else
			{
				// must be wrapper region, look inside default XHTML for a container
				XmlNode oXhtml = GetTemplateHtmlByName( oTemp, null );
				if( oXhtml == null )
					return;
			
				// get all nodes with edxtemplates
                XmlNodeList nodes = oXhtml.SelectNodes(".//*[@edxtemplate != '']", edxDoc.XmlnsEdx);					
			
				// look for a nested container
				string sCont = null;
				for(int i = 0; i < nodes.Count; i++ )
				{
					XmlNode n = nodes[i];
					string sTemp = (n.Attributes[ "edxtemplate" ] == null) ? null : n.Attributes[ "edxtemplate" ].Value;
					if( sTemp.StartsWith("field:") || sTemp.StartsWith("widget:"))
						continue;
					
					XmlNode oTmp = GetTemplate( sTemp );
					if( oTmp == null )
					{
						Util.Err("No template found for edxtemplate '" + sTemp + "'" );
						return;
					}
					if( GetTemplateType( oTmp ) == TemplateType.Container )
					{
						if( sCont != null )
						{
							Util.Err("Can't have two containers in a splittable region: " + sTemplate );
							return;
						}
						sCont = sTemp;
					}
				}
				if( sCont == null )
				{
					// not an error, just the end of the line.  we mark this tag
					// to show we've visited it
                    aContainerMaps[sTemplate] = new Dictionary<string, string>(); // "*";
				}
				else
				{
					// descend into this container (if necessary)
					GetContainerMap( sCont );
			
					// and we ourselves share that map then since we're merely a wrapper for it
					aContainerMaps[sTemplate] = aContainerMaps[sCont];
				}
			}
		}
	}
}