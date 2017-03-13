using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.UI;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.PlugIns;
using GuruComponents.Netrix.VmlDesigner.Elements;
using GuruComponents.Netrix.WebEditing.Documents;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.HtmlFormatting.Elements;
using ImageElement=GuruComponents.Netrix.VmlDesigner.Elements.ImageElement;

namespace GuruComponents.Netrix.VmlDesigner
{
    /// <summary>
    /// VmlDesigner Extender Plug-In.
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(VmlDesigner), "Resources.VmlDesigner.ico")]
    [ProvideProperty("VmlDesigner", "GuruComponents.Netrix.IHtmlEditor")]
    public class VmlDesigner : Component, IExtenderProvider, IPlugIn
    {


        private Hashtable properties;
        private Hashtable behaviors;

        /// <summary>
        /// Default Constructor supports design time behavior.
        /// </summary>
        public VmlDesigner()
        {
            properties = new Hashtable();
            behaviors = new Hashtable();
        }

        /// <summary>
        /// Constructor supports design time behavior.
        /// </summary>
        public VmlDesigner(IContainer parent)
            : this()
        {
            properties = new Hashtable();
            parent.Add(this);
        }

        private VmlDesignerProperties EnsurePropertiesExists(IHtmlEditor key)
        {
            VmlDesignerProperties p = (VmlDesignerProperties)properties[key];
            if (p == null)
            {
                p = new VmlDesignerProperties();
                properties[key] = p;
            }
            return p;
        }

        private VmlDesignerBehavior EnsureBehaviorExists(IHtmlEditor key)
        {
            VmlDesignerBehavior b = (VmlDesignerBehavior)behaviors[key];
            if (b == null)
            {
                b = new VmlDesignerBehavior(key as IHtmlEditor, EnsurePropertiesExists(key), this);
                behaviors[key] = b;
            }
            return b;
        }

        # region +++++ Block: VmlDesigner

        [ExtenderProvidedProperty(), Category("NetRix Component"), Description("VmlDesigner Properties")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public VmlDesignerProperties GetVmlDesigner(IHtmlEditor htmlEditor)
        {
            return this.EnsurePropertiesExists(htmlEditor);
        }
        public void SetVmlDesigner(IHtmlEditor htmlEditor, VmlDesignerProperties Properties)
        {
            // Properties
            EnsurePropertiesExists(htmlEditor).Active = Properties.Active;
            EnsurePropertiesExists(htmlEditor).SnapEnabled = Properties.SnapEnabled;
            EnsurePropertiesExists(htmlEditor).SnapGrid = Properties.SnapGrid;
            EnsurePropertiesExists(htmlEditor).CanRotate = Properties.CanRotate;
            EnsurePropertiesExists(htmlEditor).ElementEvents = Properties.ElementEvents;
            // Designer
            htmlEditor.AddEditDesigner(EnsureBehaviorExists(htmlEditor) as Interop.IHTMLEditDesigner);
            // Behavior
            htmlEditor.RegisterNamespace("v", null);
            htmlEditor.BeforeSnapRect += new BeforeSnapRectEventHandler(htmlEditor_BeforeSnapRect);
            // Commands
            htmlEditor.AddCommand(new CommandWrapper(new EventHandler(VmlDesignerOperation), Commands.Activate));
            htmlEditor.AddCommand(new CommandWrapper(new EventHandler(VmlDesignerOperation), Commands.Deactivate));
            htmlEditor.AddCommand(new CommandWrapper(new EventHandler(VmlDesignerOperation), Commands.EnsureStyle));
            htmlEditor.AddCommand(new CommandWrapper(new EventHandler(VmlDesignerOperation), Commands.InsertMode));
            htmlEditor.AddCommand(new CommandWrapper(new EventHandler(VmlDesignerOperation), Commands.DesignMode));
            // activate behaviors when document is ready, otherwise it will fail
            htmlEditor.RegisterPlugIn(this);
        }

        void htmlEditor_BeforeSnapRect(object sender, BeforeSnapRectEventArgs e)
        {
            // Implement connector feature here
            if (e.Element is IConnector)
            {

            }
        }

        class FlatPoint : IComparable<FlatPoint>
        {
            int val;

            public int Value
            {
                get { return val; }
                set { val = value; }
            }

            #region IComparable<FlatPoint> Members

            public int CompareTo(FlatPoint other)
            {
                if (this.val > other.val) return 1;
                if (this.val < other.val) return -1;
                return 0;
            }

            #endregion
        }

        private List<FlatPoint> XPoints, YPoints;

        private void RefreshConnectorPointList(IHtmlEditor editor)
        {
            if (XPoints == null)
            {
                XPoints = new List<FlatPoint>();
            }
            if (YPoints == null)
            {
                YPoints = new List<FlatPoint>();
            }

            // use findbehavior to get element by alias

            // Put all x in one table, all y in another
            // sort the list
            // set all "bool" to false but the current connector element 

            // get the "true" connector element entry from x
            // check both neirborghs whether they are in range
            // if in range, check y too
            // in case both are in range, move element point to nearest neirborgh
            // refresh list entry


        }

        private VmlDesignerCommands commands;

        [Browsable(false)]
        public VmlDesignerCommands Commands
        {
            get
            {
                if (commands == null)
                {
                    commands = new VmlDesignerCommands();
                }
                return commands;
            }
        }

        private void VmlDesignerOperation(object sender, EventArgs e)
        {
            CommandWrapper cw = (CommandWrapper)sender;
            if (cw.CommandID.Guid.Equals(Commands.CommandGroup))
            {
                switch ((VmlDesignerCommand)cw.ID)
                {
                    case VmlDesignerCommand.Activate:
                        EnsureBehaviorExists(cw.TargetEditor).Active = true;
                        break;
                    case VmlDesignerCommand.Deactivate:
                        EnsureBehaviorExists(cw.TargetEditor).Active = false;
                        break;
                    case VmlDesignerCommand.EnsureStyle:
                        EnsureVmlStyle(cw.TargetEditor);
                        break;
                    case VmlDesignerCommand.InsertMode:
                        EnsureBehaviorExists(cw.TargetEditor).InsertMode = true;
                        break;
                    case VmlDesignerCommand.DesignMode:
                        EnsureBehaviorExists(cw.TargetEditor).InsertMode = false;
                        break;
                }
            }
        }

        internal static void EnsureVmlStyle(IHtmlEditor editor)
        {
            if (editor.GetElementsByTagName("html") == null) return;
            IElement html = (IElement)editor.GetElementsByTagName("html")[0];
            html.SetAttribute("xmlns:v", "urn:schemas-microsoft-com:vml");
            ICollectionBase sc = (ICollectionBase)editor.DocumentStructure.EmbeddedStylesheets;
            Regex rx = new Regex(@"v\\:*\s+{\s+behavior:\s*url(#default#VML);\s+}", RegexOptions.IgnoreCase);
            foreach (IElement element in sc)
            {
                if (element.InnerText == null) continue;
                if (rx.Match(element.InnerText).Success) return;
            }
            IElement style = editor.CreateElement("style");
            sc.Add(style);
            Interop.IHTMLStyleSheet ss = ((Interop.IHTMLStyleElement)style.GetBaseElement()).styleSheet;
            ss.SetCssText(@"v\:* { behavior: url(#default#VML); }");
            editor.DocumentStructure.EmbeddedStylesheets = sc;
        }

        [Browsable(true), ReadOnly(true)]
        public string Version
        {
            get
            {
                return this.GetType().Assembly.GetName().Version.ToString();
            }
        }

        public bool ShouldSerializeVmlDesigner(IHtmlEditor htmlEditor)
        {
            VmlDesignerProperties p = EnsurePropertiesExists(htmlEditor);
            return true;
        }

        # endregion

        #region IExtenderProvider Member

        public bool CanExtend(object extendee)
        {
            if (extendee is IHtmlEditor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        private Hashtable behaviorCookies = new Hashtable();

        public void NotifyReadyStateCompleted(IHtmlEditor htmlEditor)
        {
            VmlTagInfo tagInfo;
            // Predefined Shapes
            tagInfo = new VmlTagInfo("rect", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(RectElement));
            tagInfo = new VmlTagInfo("roundrect", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(RoundRectElement));
            tagInfo = new VmlTagInfo("line", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(LineElement));
            tagInfo = new VmlTagInfo("arc", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(ArcElement));
            tagInfo = new VmlTagInfo("curve", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(CurveElement));
            tagInfo = new VmlTagInfo("image", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(ImageElement));
            tagInfo = new VmlTagInfo("oval", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(OvalElement));
            tagInfo = new VmlTagInfo("polyline", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(PolylineElement));
            tagInfo = new VmlTagInfo("shape", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(ShapeElement));
            tagInfo = new VmlTagInfo("shapetype", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(ShapetypeElement));
            tagInfo = new VmlTagInfo("group", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(GroupElement));
            // Subelements
            tagInfo = new VmlTagInfo("imagedata", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(ImagedataElement));
            tagInfo = new VmlTagInfo("textbox", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(TextboxElement));
            tagInfo = new VmlTagInfo("textpath", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(TextpathElement));
            tagInfo = new VmlTagInfo("background", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(BackgroundElement));
            tagInfo = new VmlTagInfo("fill", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(FillElement));
            tagInfo = new VmlTagInfo("formulas", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(FormulasElement));
            tagInfo = new VmlTagInfo("stroke", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(StrokeElement));
            tagInfo = new VmlTagInfo("shadow", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(ShadowElement));
            tagInfo = new VmlTagInfo("handle", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(HandleElement));
            tagInfo = new VmlTagInfo("h", FormattingFlags.None);
            htmlEditor.RegisterElement(tagInfo, typeof(HElement));

        }

        public override string ToString()
        {
            return "Click plus sign for details";
        }

        #region IPlugIn Members

        public Type Type
        {
            get
            {
                return this.GetType();
            }
        }

        public string Name
        {
            get
            {
                return "VmlDesigner";
            }
        }

        public bool IsExtenderProvider
        {
            get
            {
                return true;
            }
        }

        public Feature Features
        {
            get 
            { 
                return Feature.CreateElements | Feature.EditDesigner | Feature.OwnNamespace;
            }
        }

        public IDictionary GetSupportedNamespaces(IHtmlEditor htmlEditor)
        {
            Hashtable ht = new Hashtable();
            ht.Add("v", "vml");
            return ht;
        }

        public Control CreateElement(string tagName, IHtmlEditor htmlEditor)
        {
            string ns = "v";
            if (tagName.IndexOf(":") != -1)
            {
                tagName = tagName.Substring(tagName.IndexOf(":") + 1);
            }
            Interop.IHTMLDocument2 doc = htmlEditor.GetActiveDocument(false);
            Interop.IHTMLElement el = doc.CreateElement(String.Concat(ns, ":", tagName));
            if (el != null)
            {
                return htmlEditor.GenericElementFactory.CreateElement(el);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// List of element types, which the extender plugin extends.
        /// </summary>
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.PlugIns.IPlugIn.ElementExtenders"/> for background information.
        /// </remarks>
        public List<CommandExtender> GetElementExtenders(IElement component)
        {
            return null;
        }

        #endregion
    }
}