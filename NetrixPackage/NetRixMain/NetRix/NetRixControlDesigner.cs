using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;
# if LICENSED
//using GuruComponents.Netrix.Licensing;
# endif

namespace GuruComponents.Netrix
{

	/// <summary>
	/// Adds the license manager verb (action) to context menu at VS.NET design time.
	/// </summary>
	/// <remarks>
	/// THIS CLASS CANNOT BE USED FROM USER CODE. IT SUPPORTS THE NETRIX DESIGN TIME INFRASTRUCTURE ONLY.
	/// </remarks>
# if LICENSED
    //[LicenseProvider(typeof(NetRixLicenseProvider))]
# endif
    public class NetRixControlDesigner : ControlDesigner
	{

        class UIDesignerActionList : DesignerActionList
        {
            private IComponent editor;

            public UIDesignerActionList(IComponent component)
                : base(component)
            {
                editor = component;
            }

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                DesignerActionItemCollection items = new DesignerActionItemCollection();
                items.Add(new DesignerActionHeaderItem("Ruler Options", "Ruler"));
                DesignerActionItem propItem;
                propItem = new DesignerActionTextItem("Set the appearance of the ruler inside the editor's container.", "Ruler");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionPropertyItem("VRuler", "&Vertical Ruler", "Ruler");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionPropertyItem("HRuler", "&Horizontal Ruler", "Ruler");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionHeaderItem("UI Options", "UI");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionTextItem("Set the appearance of the toolbar and strip elements.", "UI");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionPropertyItem("ShowToolbar", "&Show Toolbar", "UI");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionPropertyItem("DockToolbar", "&Dock Toolbar", "UI");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionPropertyItem("ShowMenuStrip", "&Show MenuStrip", "UI");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionPropertyItem("ShowStatusStrip", "&Show StatusStrip", "UI");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionHeaderItem("Information and Layout", "Layout");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionTextItem("Informations about current layout and dock option.", "Layout");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionTextItem(String.Format(" Size: {0} x {1}", ((Control)editor).Size.Width, ((Control)editor).Size.Height), "Layout");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionTextItem(String.Format(" Location: x={0} y={1}", ((Control)editor).Location.X, ((Control)editor).Location.Y), "Layout");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                propItem = new DesignerActionPropertyItem("Dock", "&Dock Editor", "Layout");
                propItem.AllowAssociate = true;
                items.Add(propItem);

                return items;
            }

            private HtmlEditor Editor
            {
                get { return ((HtmlEditor)editor); }
            }

            # region DesignerAction Properties

            public DockStyle Dock
            {
                get { return ((Control)editor).Dock; }
                set
                {
                    ((Control)editor).Dock = value;
                    Editor.Invalidate();
                }
            }

# if !LIGHT
            [RefreshProperties(RefreshProperties.All)]
            public DockStyle DockToolbar
            {
                get { return Editor.DockToolbar; }
                set
                {
                    if (value == DockStyle.Fill || value == DockStyle.None)
                    {
                        ShowToolbar = false;
                    }
                    else
                    {
                        ShowToolbar = true;
                        Editor.DockToolbar = value;
                    }
                    Editor.Invalidate();
                }
            }

            [RefreshProperties(RefreshProperties.All)]
            public bool ShowToolbar
            {
                get
                {
                    return Editor.ToolbarVisible;
                }
                set
                {
                    Editor.ToolbarVisible = value;
                }
            }

            public bool ShowMenuStrip
            {
                get
                {
                    return Editor.MenuStripVisible;
                }
                set
                {
                    Editor.MenuStripVisible = value;
                }
            }

            public bool ShowStatusStrip
            {
                get
                {
                    return Editor.StatusStripVisible;
                }
                set
                {
                    Editor.StatusStripVisible = value;
                }
            }


            [Description("Show the vertical ruler")]
            public bool VRuler
            {
                get
                {
                    return Editor.ShowVerticalRuler;
                }
                set
                {
                    Editor.ShowVerticalRuler = value;
                    Editor.Invalidate();
                }
            }

            [Description("Show the horizontal ruler")]
            public bool HRuler
            {
                get
                {
                    return Editor.ShowHorizontalRuler;
                }
                set
                {
                    Editor.ShowHorizontalRuler = value;
                    Editor.Invalidate();
                }
            }
# endif

            # endregion DesignerAction Properties

        }
        
        private IComponent htmlEditor;
        DesignerActionListCollection dac;

        public override void Initialize(IComponent component)
        {
            htmlEditor = component;
            base.Initialize(component);
        }


        /// <summary>
        /// Filters the list of properties in the property grid.
        /// </summary>
        /// <remarks>
        /// THIS METHOD CANNOT BE USED FROM USER CODE. IT SUPPORTS THE NETRIX DESIGN TIME INFRASTRUCTURE ONLY.
        /// </remarks>
        protected override void PostFilterProperties(IDictionary properties)
        {
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
        }

        /// <summary>
        /// Creates a new collection of designer verbs.
        /// </summary>
        /// <remarks>
        /// THIS PROPERTY CANNOT BE USED FROM USER CODE. IT SUPPORTS THE NETRIX DESIGN TIME INFRASTRUCTURE ONLY.
        /// </remarks>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerb[] verbs = new DesignerVerb[2];
                verbs[0] = new DesignerVerb("&License Information", new EventHandler(OnLicenseManager));
                verbs[1] = new DesignerVerb("&Register (Online)", new EventHandler(OnRegister));
                return new DesignerVerbCollection(verbs);
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                List<DesignerActionList> da = new List<DesignerActionList>();
                UIDesignerActionList udacl = new UIDesignerActionList(htmlEditor);
                da.Add(udacl);
                dac = new DesignerActionListCollection(da.ToArray());
                return dac;
            }
        }

        private void OnLicenseManager(object sender, EventArgs ea)
        {
            //LicenseManager.Validate(typeof(HtmlEditor), this); // 'this' forces design time behavior in LicenseProvider
           // MessageBox.Show("Check licensing file in installation folder for licensing details");
        }

        private void OnRegister(object sender, EventArgs ea)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("http://www.netrixcomponent.net");
            p.Start();
        }       

	}
}
