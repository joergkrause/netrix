using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using GuruComponents.Netrix.UserInterface.ColorPicker;
using GuruComponents.Netrix.UserInterface.FontPicker;
using GuruComponents.Netrix.UserInterface.StyleEditor;
using System.Diagnostics;

namespace GuruComponents.Netrix.UserInterface
{
	/// <summary>
	/// This class supports the design mode.
	/// </summary>
	/// <remarks>
	/// This class supports the NetRix infrastructure in designer level and cannot be
	/// instantiated nor used in user code directly.
	/// </remarks>
	public class NetrixUIDesigner : ControlDesigner
	{

		/// <summary>
		/// Optional filter properties for the propertygrid.
		/// </summary>
		/// <remarks>
		/// This method is used to remove property the control doesn't support.
		/// </remarks>
		/// <param name="properties"></param>
		protected override void PostFilterProperties(IDictionary properties)
		{
		}

		/// <summary>
		/// Creates the command which leads to website.
		/// </summary>
		/// <remarks>
		/// This creates the link in the command section and the context menu entry, respectively.
		/// </remarks>
		public override DesignerVerbCollection Verbs
		{
			get
			{
				DesignerVerb[] verbs = new DesignerVerb[1];
				verbs[0] = new DesignerVerb("Visit Guru Components", new EventHandler(this.OnWebEvent));
				return new DesignerVerbCollection(verbs);
			}
		}

        private void OnWebEvent(object sender, EventArgs ea)
		{
            Process.Start("http://www.netrixcomponent.net");
		}
	}

	
}