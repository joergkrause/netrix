using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.Styles;

using GuruComponents.Netrix.XmlDesigner.Edx;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner
{
	
    public class EdxControl : XmlControl
	{

        private IHtmlEditor htmlEditor;
		public EdxNode eobj;	// Associated node

        private bool ignoreCase = true;
        private bool designTimeOnly = false;

        internal protected EdxControl(Interop.IHTMLElement peer, IHtmlEditor htmlEditor) : base(peer, htmlEditor)
        {            
        }

        [Bindable(false)]
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }

        [Browsable(false)]
        public override bool EnableViewState
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotSupportedException();
            }
        }        

        [Browsable(true)]
        public override string ID
        {
            get
            {
                return (string)this.GetAttribute("id");
            }
            set
            {
                this.SetAttribute("id", value);
            }
        }

    }
}
