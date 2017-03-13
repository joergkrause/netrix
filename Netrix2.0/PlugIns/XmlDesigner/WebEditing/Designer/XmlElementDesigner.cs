using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.Design;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.Styles;

namespace GuruComponents.Netrix.XmlDesigner
{

    public abstract class XmlElementDesigner : ContainerControlDesigner
    {
        public const string VIEWLINK_ATTRIB = "__designtime__viewlink";
        protected XmlControl component;
        private ViewLink viewLink;

        protected internal ViewLink AssociatedViewLink
        {
            get { return viewLink; }
            set { viewLink = value; }
        }

        /// <summary>
        /// Sets a value which controls the resize behavior.
        /// </summary>
        /// <remarks>
        /// If set to <c>false</c> the user cannot longer resize the element using the mouse handles.
        /// However, without setting the width and height and block styles the user can still enter more text
        /// than free space within the element and the element will grow, then.
        /// <para>
        /// If resizing is allowed, the resize operation will write Width and Height attributes as well as
        /// width and height styles.
        /// </para>
        /// </remarks>
        public override bool AllowResize
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The block style is a runtime style which controls the appearance of the control within the document.
        /// </summary>
        /// <remarks>
        /// Set here whether you want to handle the element a inline (within text) or block (as paragraph) one.
        /// </remarks>
        protected internal virtual DisplayStyles BlockStyle
        {
            get { return DisplayStyles.Block; }
        }

        /// <summary>
        /// If set to <c>true</c>, the internal elements do not fire any event and only the master element fires.
        /// </summary>
        /// <remarks>
        /// Suppressing internal events disables interactive editing and focus operations.
        /// </remarks>
        protected internal virtual bool FrozenEvents
        {
            get { return false; }        
        }
        /// <summary>
        /// Sets a value that indicates whether the master element of a viewlink is included in the tab sequence of the primary document.
        /// </summary>
        /// <remarks>
        /// Tabbing order goes from every tab stop within the XmlControl to every tab stop in the primary document, 
        /// based on sequence and tabIndex value. By default, the master element participates in the tab sequence of the 
        /// primary document, even if there are no tab stops defined within the XmlControl element.
        /// <para>
        /// When using a XmlControl, the body element inside the control is not tabbable by default; the author must set 
        /// the tabIndex property if the behavior is designed to display a focus rectangle on the linked document. 
        /// Setting the tabIndex property on the body of the document fragment causes the onfocus and onactivate events to 
        /// fire when the XmlControl's document body receives focus.
        /// </para>
        /// </remarks>
        protected internal virtual bool ViewMasterTab
        {
            get { return false; }
        }
        /// <summary>
        /// Sets whether an element behavior can receive focus and participate in the tabbing sequence.
        /// </summary>
        protected internal virtual bool TabStop
        {
            get { return true; }
        }
        /// <summary>
        /// Retrieves the value indicating whether the object can contain rich HTML markup.
        /// </summary>
        protected internal virtual bool CanHaveHtml
        {
            get { return true; }
        }
        /// <summary>
        /// Disables the edit focus once an editable element receives the focus.
        /// </summary>
        /// <remarks>
        /// Ususally, if the element get's the focus and the user can start typing, the element gets a hatched border
        /// around it, called the focus rectangle. Setting this property to <c>false</c> prevents the hatched border,
        /// but it does not prevent the editing capability.
        /// </remarks>
        protected internal virtual bool DisableEditFocus
        {
            get { return true; }
        }
        /// <summary>
        /// Sets the design time style of the control. 
        /// </summary>
        /// <remarks>
        /// This values do not persist, the are available at design time only.
        /// To persist styles just write ordinary Style attributes to the master element (XmlControl).
        /// </remarks>
        /// <param name="runtimeStyle"></param>
        protected internal virtual void  SetRuntimeStyle(ElementStyle runtimeStyle)
        {
        }

        /// <summary>
        /// Activates the caret inside the element with first click.
        /// </summary>
        /// <remarks>
        /// Default is <c>false</c>. Switch this option to <c>true</c> get the desired behavior.
        /// This option internally cancels ControlSelect event and set the element to active mode.
        /// <para>
        /// For more control over the behavior attach the ControlSelect event and call the SetAcive method
        /// in the ExtendedProperty property of XmlControl class.
        /// </para>
        /// </remarks>
        protected internal virtual bool ActivateOnSelect
        {
            get { return false; }
        }

        /// <summary>
        /// Get public access to the ViewLink which peforms the designer behavior.
        /// </summary>
        /// <returns></returns>
        public ViewLink GetViewLinkOfComponent()
        {
            return GetViewLinkFromElement(component.GetBaseElement());
        }

        internal static ViewLink GetViewLinkFromElement(Interop.IHTMLElement element)
        {
            object[] attr = new object[1];
            element.GetAttribute(VIEWLINK_ATTRIB, 0, attr);
            ViewLink vlAttr = attr[0] as ViewLink;
            return vlAttr;
        }

        /// <summary>
        /// Updates the designtime html by writing back the content to the native element.
        /// </summary>
        /// <remarks>
        /// This method works recursively and respects nested elements.
        /// </remarks>
        public override void UpdateDesignTimeHtml()
        {
            Interop.IHTMLElement elem = component.GetBaseElement();
            ViewLink vl = GetViewLinkFromElement(elem);
            string s = String.Empty;
            Interop.IHTMLDOMNode node = ((Interop.IHTMLDOMNode)vl.DesignTimeElementView);
            elem.SetInnerHTML(s);
            Interop.IHTMLDOMNode targetNode = (Interop.IHTMLDOMNode)elem;
            LoopNodes(node, targetNode, ((Interop.IHTMLElement)targetNode).GetDocument() as Interop.IHTMLDocument2);

        }

        private static void LoopNodes(Interop.IHTMLDOMNode node, Interop.IHTMLDOMNode targetNode, Interop.IHTMLDocument2 targetDoc)
        {
            while (node != null)
            {
                if (node.nodeName == "#text")
                {
                    ((Interop.IHTMLElement)targetNode).InsertAdjacentHTML("BeforeEnd", node.nodeValue.ToString());
                }
                else
                {
                    string elementName;
                    if (((Interop.IHTMLElement2)node).GetScopeName() == "HTML")
                    {
                        elementName = ((Interop.IHTMLElement)node).GetTagName();
                    }
                    else
                    {
                        elementName = String.Format("{0}:{1}", ((Interop.IHTMLElement2)node).GetScopeName(), ((Interop.IHTMLElement)node).GetTagName());
                    }
                    Interop.IHTMLElement clone = targetDoc.CreateElement(elementName);
                    ((Interop.IHTMLElement3)clone).mergeAttributes(((Interop.IHTMLElement)node), 0);
                    if (node.hasChildNodes())
                    {
                        Interop.IHTMLDOMChildrenCollection children =
                            node.childNodes as Interop.IHTMLDOMChildrenCollection;
                        if (children != null)
                            for (int i = 0; i < children.length; i++)
                            {
                                Interop.IHTMLDOMNode child = (Interop.IHTMLDOMNode) children.item(i);
                                LoopNodes(child, (Interop.IHTMLDOMNode) clone, targetDoc);
                            }
                    }
                }
                node = node.nextSibling;
            }
        }

        /// <summary>
        /// Override this to set a name for this behavior.
        /// </summary>
        [Browsable(false)]
        public virtual string Name
        {
            get { return ""; }
        }

        /// <summary>
        /// Override this to change the initialization procedure.
        /// </summary>
        /// <remarks>
        /// It's necessary to call the base class, otherwise the internal behavior is not active and the state of the
        /// designer is undefined. Do not call this method directly, it's being called from designerhost.
        /// </remarks>
        /// <param name="component"></param>
        public override void Initialize(IComponent component)
        {
            this.component = (XmlControl)component;
            base.Initialize(component);
        }

        /// <summary>
        /// Override this to determine the appearance of the element by setting public available HTML content.
        /// </summary>
        /// <returns>Visible and editable HTML which is returns as volatile content.</returns>
        public override string GetDesignTimeHtml()
        {
            string html;
            Interop.IHTMLElement elem = component.GetBaseElement();
            html = elem.GetInnerHTML();
            return html;
        }

        [Browsable(false)]
        public new DataBindingCollection DataBindings
        {
            get { return null; }
        }

        # region Binary Behavior Support

        /// <summary>
        /// Called by NetRix' XmlDesigner to render a behavior in the editor's client area.
        /// </summary>
        /// <remarks>
        /// Override this method to attach binary behavior drawings to custom element.
        /// </remarks>
        /// <param name="gr">Graphics device.</param>
        /// <param name="area">Custom element rectangle.</param>
        internal protected virtual void Draw(Graphics gr, Rectangle area)
        {

        }

        /// <summary>
        /// Called by NetRix when an element is resized.
        /// </summary>
        /// <remarks>
        /// Override this method to handle specific behavior during resize operation, like aligning objects.
        /// This method enables a behavior to respond to window resize events or to element resizing in design mode.
        /// This means, one can check the current size even if the element is resized by window resizing 
        /// </remarks>
        /// <param name="size">The current size</param>
        internal protected virtual void OnResize(Size size)
        {

        }

        /// <summary>
        /// Flag to control how the binary behavior will drawn.
        /// </summary>
        internal protected virtual HtmlPainter HtmlPaintFlag
        {
            get
            {
                return HtmlPainter.Transparent;
            }
        }

        /// <summary>
        /// Flag to control the z order in which the behavior will drawn against the element, surface and window.
        /// </summary>
        internal protected virtual HtmlZOrder HtmlZOrderFlag
        {
            get
            {
                return HtmlZOrder.AboveContent;
            }
        }

        internal Interop.RECT BorderZoneInternal()
        {
            Rectangle rect = BorderZone();
            return new Interop.RECT(rect.Left, rect.Top, rect.Width, rect.Height);
        }

        /// <summary>
        /// Defines a margin around the element which exceeds the drawing area.
        /// </summary>
        /// <remarks>
        /// The rectangle has four parameters which extend the drawing area by the left, top, right, and bottom
        /// respectively.
        /// </remarks>
        /// <returns>Rectangle with margin information.</returns>
        internal protected virtual Rectangle BorderZone()
        {
            return Rectangle.Empty;
        }

        /// <summary>
        /// Called by NetRix renderer to retrieve a value that specifies whether a point is contained in a rendering behavior.
        /// </summary>
        /// <remarks>
        /// NetRix calls this method when calls are made to such methods as ElementFromPoint or ComponentFromPoint that 
        /// need to determine how a point relates to the elements in a document's object tree. The behavior then can 
        /// determine how the element to which it is attached handles events. In particular, the plPartID parameter 
        /// enables a rendering behavior to assign identification numbers for different parts of the behavior. The value 
        /// of this parameter is stored as an event object property. For example, a resizing behavior could return a 
        /// unique value in plPartID for each of its sizing handles. The <see cref="Draw"/> method could then query the 
        /// event object for this value to determine in which direction to resize an element as the user drags the 
        /// sizing handle.
        /// <para>
        /// When the behavior is rendered below the flow layer of an element, the plPartID returned by this method is 
        /// not passed to the event object. Instead, the event object contains information provided by the objects in 
        /// the flow layer, above the behavior. The lZOrder member of the HTML_PAINTER_INFO structure specified by 
        /// GetPainterInfo determines whether the behavior is above or below the flow layer.
        /// </para>
        /// </remarks>
        /// <param name="testPoint">specifies the point clicked relative to the top-left corner of the element to which the behavior is attached.</param>
        /// <param name="plPartID"></param>
        /// <returns>Return <c>true</c> if the point is contained in the element to which the rendering 
        /// behavior is attached, or <c>false</c> otherwise.
        /// </returns>
        internal protected virtual bool HitTestPoint(Point testPoint, out int plPartID)
        {
            plPartID = 0;
            return false;
        }

        /// <summary>
        /// Override this method to set the mouse pointer according to current HitTestPoint results.
        /// </summary>
        /// <remarks>
        /// The <see cref="HitTestPoint"/> function can be overridden to check the current mouse pointer position
        /// against element area spots. If a spot is hit, the method gets called with the spots value as parameter
        /// and the method can return the cursor shape that is supposed to appear.
        /// </remarks>
        /// <param name="elementComponent">Result of hit test.</param>
        /// <returns>Cursor for current spot, base method returns Cursors.Default.</returns>
        internal protected virtual Cursor SetMousePointer(int elementComponent)
        {
            return Cursors.Default;
        }

        # endregion



    }
}
