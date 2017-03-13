using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace GuruComponents.Netrix.SpellChecker
{

    /// <summary>
    /// Adds the license manager verb (action) to context menu at VS.NET design time.
    /// </summary>
    /// <remarks>
    /// THIS CLASS CANNOT BE USED FROM USER CODE. IT SUPPORTS THE NETRIX DESIGN TIME INFRASTRUCTURE ONLY.
    /// </remarks>
    public class SpellControlDesigner : ComponentDesigner
    {

        class UIDesignerActionList : DesignerActionList
        {
            private IComponent spellerExtender;

            public UIDesignerActionList(IComponent component)
                : base(component)
            {
                spellerExtender = component;
            }

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                DesignerActionItemCollection items = new DesignerActionItemCollection();
                items.Add(new DesignerActionHeaderItem("Speller's UI Options", "UIOptions"));
                DesignerActionItem propItem;
                propItem = new DesignerActionTextItem("Set the UI extension (toolstrip, menu) for the main UI.", "UIOptions");
                propItem.AllowAssociate = true;
                items.Add(propItem);
                return items;
            }

            private Speller SpellerInstance
            {
                get { return ((Speller)spellerExtender); }
            }


        }

        private IComponent spellerExtender;
        DesignerActionListCollection dac;

        /// <summary>
        /// Intitializes the commponent.
        /// </summary>
        /// <param name="component"></param>
        public override void Initialize(IComponent component)
        {
            spellerExtender = component;
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
                DesignerVerb[] verbs = new DesignerVerb[1];
                verbs[0] = new DesignerVerb("&Register (Online)", new EventHandler(OnRegister));
                return new DesignerVerbCollection(verbs);
            }
        }

        /// <summary>
        /// Supports the popertygrid.
        /// </summary>
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                List<DesignerActionList> da = new List<DesignerActionList>();
                UIDesignerActionList udacl = new UIDesignerActionList(spellerExtender);
                da.Add(udacl);
                dac = new DesignerActionListCollection(da.ToArray());
                return dac;
            }
        }

        private static void OnRegister(object sender, EventArgs ea)
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("http://www.netrixcomponent.net");
            p.Start();
        }

    }
}
