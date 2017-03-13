using System;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Collections;

using GuruComponents.Netrix;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.ComInterop;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner.Edx
{

	/// <summary>
	/// Implements built-in widget nodes which provide such things as image displays, drop down selects, text input boxes, icons, etc.
	/// </summary>
	public class Widget : EdxNode
	{

        /// <summary>
        /// Type of widget
        /// </summary>
		public enum WidgetType
		{
            /// <summary>
            /// It's an icon.
            /// </summary>
			Icon,
            /// <summary>
            /// It's an image.
            /// </summary>
			Image,
            /// <summary>
            /// Simple text fragment.
            /// </summary>
			Text,
            /// <summary>
            /// A selection
            /// </summary>
			Select,
            /// <summary>
            /// An index entry.
            /// </summary>
			Index
		}

		private WidgetType widgetType;
		private string widgetSubType;
        private string resAssPath;

		/// <summary>
		/// Constructor
		/// </summary>
        /// <param name="editor"></param>
		/// <param name="p"></param>
		/// <param name="sTemp"></param>
		/// <param name="sPath"></param>
		/// <param name="sOptions"></param>
		/// <param name="index"></param>
		public Widget(IHtmlEditor editor, EdxNode p, string sTemp, string sPath, string sOptions, int index ) : base(editor, p, sTemp, sPath, sOptions, index )
		{
            resAssPath = this.GetType().Assembly.GetName().FullName;
			// init class vars
			this.type = sTemp;
			string[] a = this.type.Substring(7).Split('.');	// widget:icon or widget:icon.add
			this.widgetType = (Widget.WidgetType) Enum.Parse(typeof(Widget.WidgetType), a[0], true);
			if( a.Length > 1 )
				this.widgetSubType = a[1];
			else
				this.widgetSubType = null;
        }

        # region Service Methods

        /// <summary>
        /// Transform widget into Xhtml
        /// </summary>
        /// <param name="oFrag"></param>
        /// <returns></returns>
        public override XmlNode XHTML(XmlNode oFrag)
        {
            try
            {
                XmlNode editnode = GetXmlNode();
                switch (widgetType) 
                {
                    case WidgetType.Image:
                        if (editnode != null)
                        {
                            Util.SetXmlAttribute(oFrag, "src", Util.ResolveUrl(editnode.Value, root.BasePath));
                            root.WatchChanges(editnode, this);
                        }
                        break;
                    case WidgetType.Index:
                        if (editnode != null)
                        {
                            root.WatchChanges(editnode.ParentNode, this);
                        }
                        break;
                    case WidgetType.Icon:
                        if (false && parent is Region)
                        {
                            Util.Err("Error: icon does not have template type 'region' as parent");
                            return null;
                        }
                        // get tooltip name to show
                        View v = EdxDocument.GetEdxDocument.GetView();
                        string uiname = v.GetTemplateName(parent.GetTemplate());
                        if (oFrag.Name.ToUpper() == "IMG")
                        {
                            string isrc = Util.GetXmlAttribute(oFrag, "src");
                            if (isrc == null || isrc == "")
                            {
                                // look up icon for built-ins
                                switch (widgetSubType)
                                {
                                    case "details":
                                        isrc = String.Format("res://{0}/iconDetails.gif", resAssPath);
                                        break;
                                    case "hidden":
                                        isrc =  String.Format("res://{0}/iconHidden.gif", resAssPath);
                                        break;
                                    case "para":
                                        isrc =  String.Format("res://{0}/iconPara.gif", resAssPath);
                                        break;
                                    case "add":
                                        isrc =  String.Format("res://{0}/iconAdd.gif", resAssPath);
                                        break;
                                    case null:
                                    case "default":
                                    default:
                                        isrc = String.Format("res://{0}/iconDefault.gif", resAssPath);
                                        break;
                                }
                                Util.SetXmlAttribute(oFrag, "src", isrc);
                            }
                            else
                            {
                                isrc = Util.ResolveUrl(isrc, root.BasePath);
                            }
                            Util.SetXmlAttribute(oFrag, "alt", uiname);
                        }
                        else
                        {
                            Util.SetXmlAttribute(oFrag, "title", uiname);
                        }
                        string style = Util.GetXmlAttribute(oFrag, "style");
                        if (style == null)
                            Util.SetXmlAttribute(oFrag, "style", "cursor:hand;");
                        else
                            Util.SetXmlAttribute(oFrag, "style", "cursor:hand;" + style);
                        break;
                    case WidgetType.Text:
                        if (editnode != null)
                        {
                            Util.SetXmlAttribute(oFrag, "value", editnode.Value);
                            root.WatchChanges(editnode, this);
                        }
                        break;
                    case WidgetType.Select:
                        // debug - maybe someday make these watch their assoc'd node for changes too
                        break;
                }
            }
            catch (Exception e)
            {
                Util.Err("exception in widgetXHTML: " + e + " type: " + this.widgetType.ToString());
            }
            return oFrag;
        }

        /// <summary>
        /// Associate widget node
        /// </summary>
        /// <param name="h"></param>
        public override void Associate(XmlNode h, XmlNode backLinkNode )
		{
			base.Associate( h, backLinkNode );
			
            switch (widgetType)
            {
                case WidgetType.Image:
                    if(h.Name != "IMG" )
			        {
				        Util.Err("Error: image widget must be used only on IMG tags" );
				        return;
                    }
				    //h.OnClick		+= new DocumentEventHandler(widgetOnClick);
				    //h.OnContextMenu += new DocumentEventHandler(widgetOnContextMenu);
                    break;
                case WidgetType.Text:
                    if (h.Name != "INPUT")
			        {
				        Util.Err("Error: widget type 'text' must be used only on INPUT tags." );
				        return;
                    }
                    //h.OnBlur        += new DcoumentEventHandler(Widget_OnBlur);
                    break;
                case WidgetType.Select:
                    if (h.Name != "SELECT")
			        {
				        Util.Err("Error: widget type 'select' must by used only on SELECT tags." );
				        return;
                    }
                    //h.OnChange      += new DocumentEventHandler(Widget_OnBlur);
			
			        // turn on selected item
			        XmlNode editnode = GetXmlNode();
			        string val = Util.GetXmlNodeValue( editnode );
        				
			        for(int i=0; i < h.ChildNodes.Count; i++ )
			        {
				        if( h.ChildNodes[i].Value == val )
				        {
					        //h.ChildNodes[i].selected = true;
					        break;
				        }
			        }
                    break;
                case WidgetType.Index:
				    WriteIndexText();
			
				    // see if we're node-linked to a template
				    if( this.nodeLink != null)
				    {
					    //h.OnClick += new DocumentEventHandler(widgetOnClick);
					    //h.SetStyleAttribute("cursor", "hand");
				    }
                    break;
                case WidgetType.Icon:
				    //h.OnClick		+= new DocumentEventHandler(widgetOnClick);
				    //h.OnContextMenu += new DocumentEventHandler(widgetOnContextMenu);
                    break;
                default:
                    throw new ApplicationException("Widgettype not supported: " + widgetType);
            }
			// make sure we're on right tag type
        }

		/// <summary>
		/// Click on widget.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="evt"></param>
		public void widgetOnClick(object sender, DocumentEventArgs evt)
		{
            //Widget e = ((EdxControl) evt.SrcElement).eobj;
            //if( e.widgetType == WidgetType.Icon || e.widgetType == WidgetType.Image )
            //{
            //    e.parent.select();
            //}
            //else if( e.widgetType == WidgetType.Index )
            //{
            //    e.ProcessNodeLink();
            //}
		}

		/// <summary>
		/// Highlights the index and finds any region(s) and sets their new XML node.
		/// </summary>
		public void ProcessNodeLink()
		{
            //if( root.selectedIndex != null )
            //{
            //    root.selectedIndex.toggleColors();
            //}
            //root.selectedIndex = this;
            //ToggleColors();
		
            //// find node-linked template(s)
            //UpdateLinkedTemplates( root, nodeLink, GetXmlNode() );				
		}

		/// <summary>
        /// _widgetToggleColors
		/// </summary>
		public void ToggleColors()
		{
            //string c = ColorTranslator.ToHtml(((Interop.IHTMLElement2)hobj).GetCurrentStyle().color);
            //if( c == "#fffffe" )
            //    c = "transparent";
            //string bc = ColorTranslator.ToHtml(((Interop.IHTMLElement2)hobj).GetCurrentStyle().backgroundColor);
            //if( bc == "transparent" )
            //    bc = "#fffffe";
				
            //hobj.GetStyle().SetColor(bc);
            //hobj.GetStyle().SetBackgroundColor(c);
		}

		/// <summary>
        /// widgetUpdateLinkedTemplates
		/// </summary>
		/// <param name="e"></param>
		/// <param name="sTemplate"></param>
		/// <param name="oXml"></param>
		public void UpdateLinkedTemplates( IElement e, string sTemplate, XmlNode oXml )
		{
            //if (sTemplate == e.GetAttribute("edxtemplate"))
            //{
            //    e.setXmlNode( oXml );
            //    return;
            //}
            //ElementCollection ec = e.ElementDom.GetChildNodes();

            //for(int i = 0; i < ec.Count; i++ )
            //{
            //    UpdateLinkedTemplates(ec[i], sTemplate, oXml);
            //}
		}

		/// <summary>
		/// Checks for change in the widget and captures the value to XML if necessary.
		/// </summary>
		public void Widget_OnBlur(object sender, DocumentEventArgs evt)
		{
            //XmlControl h = (XmlControl) sender;
            //Widget e = (Widget)h.EdxNodeAssociated;
            //if( e.widgetType == WidgetType.Text || e.widgetType == WidgetType.Select )
            //{
            //    XmlNode editnode = e.GetXmlNode();
            //    if( editnode != null )
            //    {
            //        if( editnode.Value != h.InnerText )
            //        {
            //            XmlManager xmlmgr = e.root.XmlManager;
            //            xmlmgr.OpenTransaction( editnode );
            //            xmlmgr.Process( "updateNode", editnode, h.InnerText );
            //            xmlmgr.CloseTransaction();
            //            e.root.AlertChange( editnode, e );
            //        }
            //    }
            //}
		}

		/// <summary>
		/// Cleanup by removing watches.
		/// </summary>
		public new void Cleanup()
		{
			if( widgetType == WidgetType.Image || widgetType == WidgetType.Text )
			{
				XmlNode editnode = GetXmlNode();
				if( editnode != null )
				{
					root.UnwatchChanges( editnode, this );
				}
			}
			else if( widgetType == WidgetType.Index )
			{
				XmlNode editnode = GetXmlNode();
				if( editnode != null )
				{
					root.UnwatchChanges( editnode.ParentNode, this );
				}
				if( root.selectedIndex == 0 )
					root.selectedIndex = null;
			}
			base.Cleanup();
		}

        /// <summary>
        /// Notified by watch list when XML changes.
        /// </summary>
        /// <param name="sender"></param>
        public void OnXmlNodeChange(object sender)
        {
            XmlNode editnode = GetXmlNode();
            switch (widgetType)
            {
                case WidgetType.Image:
                    if (editnode != null)
                    {
                        hobj.Attributes["src"].Value = Util.ResolveUrl(editnode.Value, root.BasePath);
                    }
                    break;
                case WidgetType.Text:
                    if (editnode != null)
                    {
                        hobj.InnerText = editnode.Value;
                    }
                    break;
                case WidgetType.Index:
                    WriteIndexText();
                    break;
            }
        }

        /// <summary>
        /// Write Index Text.
        /// </summary>
        public void WriteIndexText()
        {
            XmlNode editnode = GetXmlNode();

            XmlNode par = editnode.ParentNode;
            int i = 0;
            for (i = 0; i < par.ChildNodes.Count; i++)
            {
                if (par.ChildNodes[i] == editnode)
                    break;
            }
            hobj.InnerText = "" + (i + 1).ToString();
        }
        # endregion
    }
}