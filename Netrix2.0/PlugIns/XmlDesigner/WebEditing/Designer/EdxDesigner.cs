using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.XmlDesigner.Edx;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner
{
    /// <summary>
    /// Implementation of managed Edx template based transform features.
    /// </summary>
    public class EdxDesigner : XmlElementDesigner
    {
        EdxDocument edxDoc;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public EdxDocument EdxDoc
        {
            get { return edxDoc; }
        }

        public override void Initialize(IComponent component)
        {
            this.component = (XmlControl)component;
            base.Initialize(component);

            EdxDocument.Initialize(this.component);
            edxDoc = EdxDocument.GetEdxDocument;
            edxDoc.DoInit(this.component.OuterHtml, ((XmlControl)component).TransformXml);
        }

        # region NonEdx
        public override bool AllowResize
        {
            get
            {
                return false;
            }
        }
        # endregion

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this);
            properties.Add("Edx", pdc["EdxDoc"]);
        }

        public override void UpdateDesignTimeHtml()
        {
            Interop.IHTMLElement elem = component.GetBaseElement();
            elem.SetOuterHTML(edxDoc.Update());
        }

        public override string GetDesignTimeHtml()
        {
            string s = edxDoc.Process(this);
            //s = "<h1>TEST</H1>";
            return s; 
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection dvc = new DesignerVerbCollection();
                dvc.Add(new DesignerVerb("MoveUp", new EventHandler(Edx_MoveUp)));
                return base.Verbs;
            }
        }

        private void Edx_MoveUp(object sender, EventArgs e)
        {
            edxDoc.MoveUp();
        }

    }
}
